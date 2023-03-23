using Microsoft.Azure.Cosmos;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class CosmosDbClientService
{
    /// <summary>
    /// Creates or updates email attachment items in the Cosmos DB database.
    /// </summary>
    /// <param name="attachmentItems">The email attachment items to create/update.</param>
    public async Task CreateOrUpdateEmailAttachmentItemAsync(EmailTemplateAttachmentItem[] attachmentItems)
    {
        Container container = _cosmosClient.GetContainer(
            databaseId: _databaseName,
            containerId: "configs"
        );

        foreach (var attachmentItem in attachmentItems)
        {
            try
            {
                // Try to create the item.
                await container.CreateItemAsync(
                    item: attachmentItem,
                    partitionKey: new(attachmentItem.PartitionKey)
                );
            }
            catch
            {
                // If it already exists, replace it.
                await container.ReplaceItemAsync(
                    item: attachmentItem,
                    id: attachmentItem.Id,
                    partitionKey: new(attachmentItem.PartitionKey)
                );
            }
        }
    }
}