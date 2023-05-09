using System;
using System.Diagnostics;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models;
using SmallsOnline.PasswordExpirationNotifier.FunctionApp.Services;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;
using SmallsOnline.PasswordExpirationNotifier.Lib.Services;

namespace SmallsOnline.PasswordExpirationNotifier.FunctionApp;

/// <summary>
/// Function to get users' password expiration details, triggered by a message in the user-search-queue.
/// </summary>
public class GetUsersQueue
{
    private readonly IGraphClientService _graphClientService;
    private readonly ICosmosDbClientService _cosmosDbClientService;
    private readonly IQueueClientService _queueClientService;
    private readonly JsonSourceGenerationContext _jsonSourceGenerationContext = new();

    public GetUsersQueue(IGraphClientService graphClientService, ICosmosDbClientService cosmosDbClientService, IQueueClientService queueClientService)
    {
        _graphClientService = graphClientService;
        _cosmosDbClientService = cosmosDbClientService;
        _queueClientService = queueClientService;
    }

    [Function("GetUsersQueue")]
    public async Task Run(
        [QueueTrigger("user-search-queue")] string queueItemContents,
        FunctionContext executionContext
    )
    {
        // TODO: Remove this after testing.
        Stopwatch stopwatch = Stopwatch.StartNew();
        var logger = executionContext.GetLogger(nameof(GetUsersQueue));

        // Deserialize the queue item.
        UserSearchQueueItem queueItem = JsonSerializer.Deserialize(
            json: queueItemContents,
            jsonTypeInfo: _jsonSourceGenerationContext.UserSearchQueueItem
        )!;

        // Get the user search config.
        UserSearchConfig searchConfig = await _cosmosDbClientService.GetUserSearchConfigAsync(queueItem.UserSearchConfigId);

        logger.LogInformation("Received request to get users with last name starting with {lastNameStartsWith} from {domainName}.", queueItem.LastNameStartsWith.ToUpper(), queueItem.DomainName);

        // Get the users from the Graph API.
        logger.LogInformation("Getting users from Graph API...");
        User[]? foundUsers = await _graphClientService.GetUsersAsync(
            domainName: queueItem.DomainName,
            ouPath: queueItem.OUPath,
            lastNameStartsWith: queueItem.LastNameStartsWith.ToUpper()
        );

        if (foundUsers is null || foundUsers.Length == 0)
        {
            stopwatch.Stop();
            logger.LogInformation("[{LastNameStartsWith} - {DomainName}] Found no users in {Minutes}:{Seconds}.{Milliseconds}.", queueItem.LastNameStartsWith, queueItem.DomainName, stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds, stopwatch.Elapsed.Milliseconds);

            return;
        }

        // Enrich the users info with password expiration details.
        UserPasswordExpirationDetails[] foundUsersEnriched = new UserPasswordExpirationDetails[foundUsers.Length];
        for (int i = 0; i < foundUsers.Length; i++)
        {
            foundUsersEnriched[i] = new(foundUsers[i], searchConfig, queueItem.CorrelationId);
        }

        // Get the users with expiring passwords.
        UserPasswordExpirationDetails[] usersWithExpiringPasswords = searchConfig.IgnorePasswordAge switch
        {
            true => foundUsersEnriched,
            _ => Array.FindAll(foundUsersEnriched, user => user.PasswordIsExpiringSoon && user.PasswordIsExpired == false)
        };

        // Send the users with expiring passwords to the email queue.
        foreach (var userItem in usersWithExpiringPasswords)
        {
            await _queueClientService.EmailQueueClient.SendMessageAsync(JsonSerializer.Serialize(
                value: userItem,
                jsonTypeInfo: _jsonSourceGenerationContext.UserPasswordExpirationDetails)
            );
        }

        stopwatch.Stop();
        logger.LogInformation("[{LastNameStartsWith} - {DomainName}] Found {foundUsersCount} users in {Minutes}:{Seconds}.{Milliseconds}.", queueItem.LastNameStartsWith, queueItem.DomainName, usersWithExpiringPasswords.Length, stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds, stopwatch.Elapsed.Milliseconds);
    }
}