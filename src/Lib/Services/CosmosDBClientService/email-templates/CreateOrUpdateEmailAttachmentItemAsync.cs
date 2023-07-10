using System.Text.Json;
using Microsoft.Azure.Cosmos;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class CosmosDbClientService
{
    /// <inheritdoc />
    public async Task CreateOrUpdateEmailAttachmentItemAsync(EmailTemplateAttachmentItem[] attachmentItems)
    {
        Container container = _cosmosClient.GetContainer(
            databaseId: _databaseName,
            containerId: "configs"
        );

        foreach (var attachmentItem in attachmentItems)
        {
            using MemoryStream streamPayload = new();
            await JsonSerializer.SerializeAsync(
                utf8Json: streamPayload,
                value: attachmentItem,
                jsonTypeInfo: _jsonSourceGenerationContext.EmailTemplateAttachmentItem
            );

            try
            {
                // Try to create the item.
                await container.CreateItemStreamAsync(
                    streamPayload: streamPayload,
                    partitionKey: new(attachmentItem.PartitionKey)
                );
            }
            catch
            {
                // If it already exists, replace it.
                await container.ReplaceItemStreamAsync(
                    streamPayload: streamPayload,
                    id: attachmentItem.Id,
                    partitionKey: new(attachmentItem.PartitionKey)
                );
            }
        }
    }
}