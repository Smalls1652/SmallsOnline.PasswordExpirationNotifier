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
        AppInsightsEnabled = AppSettingsHelper.GetSettingValue("APPLICATIONINSIGHTS_CONNECTION_STRING") is not null && AppSettingsHelper.GetSettingValue("APPINSIGHTS_INSTRUMENTATIONKEY") is not null;

        // Get the user search configs and email template configs from Cosmos DB.
        var getUserSearchConfigsTask = Task.Run(async () => await cosmosDbClientService.GetUserSearchConfigsAsync());
        var getEmailTemplateConfigsTask = Task.Run(async () => await cosmosDbClientService.GetEmailTemplateConfigsAsync());
        var getUserRedirectConfigsTask = Task.Run(async () => await cosmosDbClientService.GetUserRedirectConfigsAsync());

        // Wait for the tasks to complete.
        // TODO: Look into optimizing this, so we're not using Task.WaitAll().
        Task.WaitAll(getUserSearchConfigsTask, getEmailTemplateConfigsTask, getUserRedirectConfigsTask);

        // Set the properties with the results.
        UserSearchConfigs = getUserSearchConfigsTask.Result;
        EmailTemplateConfigs = getEmailTemplateConfigsTask.Result;
        UserRedirectConfigs = getUserRedirectConfigsTask.Result;
    }

    /// <inheritdoc />
    public UserSearchConfig[] UserSearchConfigs { get; }

    /// <inheritdoc />
    public EmailTemplateConfig[] EmailTemplateConfigs { get; }

    public UserEmailRedirect[] UserRedirectConfigs { get; }

    public bool AppInsightsEnabled { get; }
}