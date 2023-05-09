using System.Text.Json;
using Microsoft.Azure.Cosmos;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class CosmosDbClientService
{
    /// <inheritdoc />
    public async Task<EmailTemplateConfig> GetEmailTemplateConfigAsync(string templateId)
    {
        // Get the Cosmos DB container.
        Container container = _cosmosClient.GetContainer(
            databaseId: _databaseName,
            containerId: "configs"
        );

        // Get the template config item from Cosmos DB.
        ResponseMessage templateItemRsp = await container.ReadItemStreamAsync(
            id: templateId,
            partitionKey: new("email-template-config")
        );

        // Deserialize the template config item.
        EmailTemplateConfig templateItem = JsonSerializer.Deserialize(
            utf8Json: templateItemRsp.Content,
            jsonTypeInfo: _jsonSourceGenerationContext.EmailTemplateConfig
        )!;

        // If the template has included attachments, get them from Cosmos DB.
        if (templateItem.IncludedAttachmentIds is not null)
        {
            // Create an array to hold the included attachments.
            EmailTemplateAttachmentItem[] includedAttachments = new EmailTemplateAttachmentItem[templateItem.IncludedAttachmentIds.Length];

            // Get each included attachment from Cosmos DB.
            for (int i = 0; i < templateItem.IncludedAttachmentIds.Length; i++)
            {
                EmailTemplateAttachmentItem attachmentItem = await container.ReadItemAsync<EmailTemplateAttachmentItem>(
                    id: templateItem.IncludedAttachmentIds[i],
                    partitionKey: new("email-template-attachment-item")
                );

                includedAttachments[i] = attachmentItem;
            }

            // Set the included attachments on the template item.
            templateItem.IncludedAttachments = includedAttachments;
        }

        return templateItem;
    }
}