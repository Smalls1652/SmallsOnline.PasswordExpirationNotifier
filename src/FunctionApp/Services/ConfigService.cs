using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Services;

namespace SmallsOnline.PasswordExpirationNotifier.FunctionApp.Services;

/// <summary>
/// Service for storing configuration data for the application.
/// </summary>
public class ConfigService : IConfigService
{
    public ConfigService(ICosmosDbClientService cosmosDbClientService)
    {
        // Get the user search configs and email template configs from Cosmos DB.
        var getUserSearchConfigsTask = Task.Run(async () => await cosmosDbClientService.GetUserSearchConfigsAsync());
        var getEmailTemplateConfigsTask = Task.Run(async () => await cosmosDbClientService.GetEmailTemplateConfigsAsync());

        // Wait for the tasks to complete.
        // TODO: Look into optimizing this, so we're not using Task.WaitAll().
        Task.WaitAll(getUserSearchConfigsTask, getEmailTemplateConfigsTask);

        // Set the properties with the results.
        UserSearchConfigs = getUserSearchConfigsTask.Result;
        EmailTemplateConfigs = getEmailTemplateConfigsTask.Result;
    }

    /// <summary>
    /// User search configs for the application.
    /// </summary>
    public UserSearchConfig[] UserSearchConfigs { get; }

    /// <summary>
    /// Email template configs for the application.
    /// </summary>
    public EmailTemplateConfig[] EmailTemplateConfigs { get; }
}