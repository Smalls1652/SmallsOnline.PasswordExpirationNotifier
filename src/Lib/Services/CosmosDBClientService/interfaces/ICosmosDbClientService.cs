using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public interface ICosmosDbClientService
{
    void Dispose();
    Task<UserSearchConfig> GetUserSearchConfigAsync(string id);
    Task<UserSearchConfig[]> GetUserSearchConfigsAsync();
    Task<EmailTemplateConfig> GetEmailTemplateConfigAsync(string templateId);
    Task<EmailTemplateConfig[]> GetEmailTemplateConfigsAsync();
}