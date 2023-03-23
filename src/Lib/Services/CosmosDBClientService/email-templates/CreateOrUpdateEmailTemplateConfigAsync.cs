using Microsoft.Azure.Cosmos;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class CosmosDbClientService
{
    /// <summary>
    /// Create or update an email template config in the Cosmos DB database.
    /// </summary>
    /// <param name="emailTemplateConfig">The email template to create/update.</param>
    public async Task CreateOrUpdateEmailTemplateConfigAsync(EmailTemplateConfig emailTemplateConfig)
    {
        Container container = _cosmosClient.GetContainer(
            databaseId: _databaseName,
            containerId: "configs"
        );

        try
        {
            // Try to create the item.
            await container.UpsertItemAsync(
                item: emailTemplateConfig,
                partitionKey: new(emailTemplateConfig.PartitionKey)
            );
        }
        catch
        {
            // If it already exists, replace it.
            await container.ReplaceItemAsync(
                item: emailTemplateConfig,
                id: emailTemplateConfig.Id,
                partitionKey: new(emailTemplateConfig.PartitionKey)
            );
        }
    }
}