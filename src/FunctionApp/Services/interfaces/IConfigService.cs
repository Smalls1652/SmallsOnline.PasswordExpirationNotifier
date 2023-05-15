using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.FunctionApp.Services;

/// <summary>
/// Interface for the config service.
/// </summary>
public interface IConfigService
{
    /// <summary>
    /// User search configs for the application.
    /// </summary>
    UserSearchConfig[] UserSearchConfigs { get; }

    /// <summary>
    /// Email template configs for the application.
    /// </summary>
    EmailTemplateConfig[] EmailTemplateConfigs { get; }

    UserEmailRedirect[] UserRedirectConfigs { get; }

    bool AppInsightsEnabled { get; }
}