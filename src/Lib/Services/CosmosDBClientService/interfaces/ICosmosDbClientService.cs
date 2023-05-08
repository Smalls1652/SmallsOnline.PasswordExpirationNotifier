using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public interface ICosmosDbClientService
{
    void Dispose();
    Task<UserSearchConfig> GetUserSearchConfigAsync(string id);
    Task<UserSearchConfig[]?> GetUserSearchConfigsAsync();
    Task<EmailTemplateConfig> GetEmailTemplateConfigAsync(string templateId);
    Task<EmailTemplateConfig[]?> GetEmailTemplateConfigsAsync();
    Task CreateOrUpdateEmailTemplateConfigAsync(EmailTemplateConfig emailTemplateConfig);
    Task CreateOrUpdateEmailAttachmentItemAsync(EmailTemplateAttachmentItem[] attachmentItems);
    Task RemoveEmailTemplateConfigAsync(string id);
    Task RemoveEmailAttachmentItemAsync(string id);
    Task CreateOrUpdateUserSearchConfigAsync(UserSearchConfig userSearchConfig);
    Task RemoveUserSearchConfigAsync(string id);
}