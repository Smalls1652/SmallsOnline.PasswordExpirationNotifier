using Microsoft.Azure.Cosmos;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class CosmosDbClientService
{
    /// <summary>
    /// Remove an email template config from the Cosmos DB database.
    /// </summary>
    /// <param name="id">The ID of the email template.</param>
    public async Task RemoveEmailTemplateConfigAsync(string id)
    {
        Container container = _cosmosClient.GetContainer(
            databaseId: _databaseName,
            containerId: "configs"
        );

        // Get the email template config.
        EmailTemplateConfig emailTemplateConfig = await GetEmailTemplateConfigAsync(id);

        if (emailTemplateConfig.IncludedAttachmentIds is not null)
        {
            // Remove all attachments associated with this email template.
            foreach (var attachmentItem in emailTemplateConfig.IncludedAttachmentIds)
            {
                await RemoveEmailAttachmentItemAsync(attachmentItem);
            }
        }

        // Remove the email template config.
        await container.DeleteItemAsync<EmailTemplateConfig>(
            id: id,
            partitionKey: new("email-template-config")
        );
    }
}