using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models;
using SmallsOnline.PasswordExpirationNotifier.FunctionApp.Services;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Services;

namespace SmallsOnline.PasswordExpirationNotifier.FunctionApp;

/// <summary>
/// Function for queuing up user searches.
/// </summary>
public class QueueUpUserSearch
{
    private readonly IConfigService _configService;
    private readonly ICosmosDbClientService _cosmosDbClientService;
    private readonly IQueueClientService _queueClientService;
    private readonly JsonSourceGenerationContext _jsonSourceGenerationContext = new();

    public QueueUpUserSearch(IConfigService configService, ICosmosDbClientService cosmosDbClientService, IQueueClientService queueClientService)
    {
        _configService = configService;
        _cosmosDbClientService = cosmosDbClientService;
        _queueClientService = queueClientService;
    }

    [Function("QueueUpUserSearch")]
    public async Task Run(
        [TimerTrigger(
            schedule: "0 0 23 * * *",
            RunOnStartup = false
        )] TimerInfo timer,
        FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger("QueueUpUserSearch");
        logger.LogInformation("C# HTTP trigger function processed a request.");

        // Get the user search configs from the config service.
        UserSearchConfig[] searchConfigs = Array.FindAll(_configService.UserSearchConfigs, item => item.ConfigEnabled);

        // Create a correlation ID to identify this run.
        string correlationId = Guid.NewGuid().ToString();

        // Loop through each user search config and
        // create a queue message for each letter of the alphabet.
        Range range = new(65, 90);
        foreach (var configItem in searchConfigs)
        {
            for (int i = range.Start.Value; i <= range.End.Value; i++)
            {
                string lastNameStartsWithChar = Convert.ToChar(i).ToString();
                logger.LogInformation("Creating queue message for config '{ConfigName} [{ConfigId}]' with last name starting with '{LastNameStartsWithChar}'. [CorrelationId: {CorrelationId}]", configItem.ConfigName, configItem.Id, lastNameStartsWithChar, correlationId);

                // Create the queue message.
                await _queueClientService.UserSearchQueueClient.SendMessageAsync(
                    messageText: JsonSerializer.Serialize(
                        value: new UserSearchQueueItem()
                        {
                            UserSearchConfigId = configItem.Id,
                            DomainName = configItem.DomainName!,
                            LastNameStartsWith = lastNameStartsWithChar,
                            OUPath = configItem.OUPath,
                            MaxPasswordAge = configItem.MaxPasswordAge,
                            IsEmailIntervalsEnabled = configItem.IsEmailIntervalsEnabled,
                            EmailIntervalDays = configItem.EmailIntervalDays,
                            EmailTemplateId = configItem.EmailTemplateId!,
                            CorrelationId = correlationId
                        },
                        jsonTypeInfo: _jsonSourceGenerationContext.UserSearchQueueItem
                    )
                );
            }
        }
    }
}