using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.FunctionApp.Services;

public interface IConfigService
{
    UserSearchConfig[] UserSearchConfigs { get; }
    EmailTemplateConfig[] EmailTemplateConfigs { get; }
}