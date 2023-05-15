using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

/// <summary>
/// Interface for the Cosmos DB client service.
/// </summary>
public interface ICosmosDbClientService : IDisposable
{
    /// <summary>
    /// Get a user search config from Cosmos DB.
    /// </summary>
    /// <param name="id">The unique ID for the user search config.</param>
    /// <returns>The <see cref="UserSearchConfig"/> item.</returns>
    Task<UserSearchConfig> GetUserSearchConfigAsync(string id);

    /// <summary>
    /// Get all user search configs from Cosmos DB.
    /// </summary>
    /// <returns>A collection of <see cref="UserSearchConfig"/> items stored in the database.</returns>
    /// <exception cref="NullReferenceException">No configs were found in the database.</exception>
    Task<UserSearchConfig[]?> GetUserSearchConfigsAsync();

    /// <summary>
    /// Get an email template config item from Cosmos DB.
    /// </summary>
    /// <param name="templateId">The unique ID for the template config.</param>
    /// <returns>The <see cref="EmailTemplateConfig" /> item.</returns>
    Task<EmailTemplateConfig> GetEmailTemplateConfigAsync(string templateId);

    /// <summary>
    /// Get all email template configs from Cosmos DB.
    /// </summary>
    /// <returns>A collection of <see cref="EmailTemplateConfig"/> items stored in the database.</returns>
    /// <exception cref="NullReferenceException">No configs were found in the database.</exception>
    Task<EmailTemplateConfig[]?> GetEmailTemplateConfigsAsync();

    /// <summary>
    /// Create or update an email template config in the Cosmos DB database.
    /// </summary>
    /// <param name="emailTemplateConfig">The email template to create/update.</param>
    Task CreateOrUpdateEmailTemplateConfigAsync(EmailTemplateConfig emailTemplateConfig);

    /// <summary>
    /// Creates or updates email attachment items in the Cosmos DB database.
    /// </summary>
    /// <param name="attachmentItems">The email attachment items to create/update.</param>
    Task CreateOrUpdateEmailAttachmentItemAsync(EmailTemplateAttachmentItem[] attachmentItems);

    /// <summary>
    /// Remove an email template config from the Cosmos DB database.
    /// </summary>
    /// <param name="id">The ID of the email template.</param>
    Task RemoveEmailTemplateConfigAsync(string id);

    /// <summary>
    /// Remove an email attachment item from the Cosmos DB database.
    /// </summary>
    /// <param name="id">The ID of the email attachment.</param>
    Task RemoveEmailAttachmentItemAsync(string id);

    /// <summary>
    /// Create or update a user search config in the Cosmos DB database.
    /// </summary>
    /// <param name="userSearchConfig">The user search config to create/update.</param>
    Task CreateOrUpdateUserSearchConfigAsync(UserSearchConfig userSearchConfig);

    /// <summary>
    /// Remove a user search config from the Cosmos DB database.
    /// </summary>
    /// <param name="id">The ID of the user search config.</param>
    Task RemoveUserSearchConfigAsync(string id);

    Task CreateOrUpdateUserRedirectConfigAsync(UserEmailRedirectConfig userEmailRedirectConfig);
    Task<UserEmailRedirectConfig> GetUserRedirectConfigAsync(string id);
    Task<UserEmailRedirectConfig[]?> GetUserRedirectConfigsAsync();
    Task RemoveUserRedirectConfigAsync(string id);
}