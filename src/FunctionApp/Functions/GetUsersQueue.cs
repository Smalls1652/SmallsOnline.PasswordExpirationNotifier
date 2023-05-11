using System.Diagnostics;
using System.Text.Json;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.Functions.Worker;
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
    private readonly IConfigService _configService;
    private readonly IGraphClientService _graphClientService;
    private readonly ICosmosDbClientService _cosmosDbClientService;
    private readonly IQueueClientService _queueClientService;
    private readonly JsonSourceGenerationContext _jsonSourceGenerationContext = new();
    private readonly TelemetryClient _telemetryClient;

    public GetUsersQueue(IConfigService configService, IGraphClientService graphClientService, ICosmosDbClientService cosmosDbClientService, IQueueClientService queueClientService, TelemetryClient telemetryClient)
    {
        _configService = configService;
        _graphClientService = graphClientService;
        _cosmosDbClientService = cosmosDbClientService;
        _queueClientService = queueClientService;
        _telemetryClient = telemetryClient;
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

        LoggingHelper.LogInformation(_configService.AppInsightsEnabled ? _telemetryClient : logger, "Received request to get users with last name starting with {0} from {1}.", queueItem.CorrelationId, executionContext, queueItem.LastNameStartsWith.ToUpper(), queueItem.DomainName);

        // Get the users from the Graph API.
        LoggingHelper.LogInformation(_configService.AppInsightsEnabled ? _telemetryClient : logger, "Getting users from Graph API...", queueItem.CorrelationId, executionContext);
        User[]? foundUsers = await _graphClientService.GetUsersAsync(
            domainName: queueItem.DomainName,
            ouPath: queueItem.OUPath,
            lastNameStartsWith: queueItem.LastNameStartsWith.ToUpper()
        );

        if (foundUsers is null || foundUsers.Length == 0)
        {
            stopwatch.Stop();
            LoggingHelper.LogInformation(_configService.AppInsightsEnabled ? _telemetryClient : logger, "[{0} - {1}] Found no users in {2}:{3}.{4}.", queueItem.CorrelationId, executionContext, queueItem.LastNameStartsWith, queueItem.DomainName, stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds, stopwatch.Elapsed.Milliseconds);

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
        LoggingHelper.LogWarning(_configService.AppInsightsEnabled ? _telemetryClient : logger, "[{0} - {1}] Found {2} users in {3}:{4}.{5}.", queueItem.CorrelationId, executionContext, queueItem.LastNameStartsWith, queueItem.DomainName, usersWithExpiringPasswords.Length, stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds, stopwatch.Elapsed.Milliseconds);
    }
}