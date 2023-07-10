using System.Text.Json;
using Microsoft.Azure.Cosmos;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class CosmosDbClientService
{
    /// <inheritdoc />
    public async Task CreateOrUpdateEmailTemplateConfigAsync(EmailTemplateConfig emailTemplateConfig)
    {
        Container container = _cosmosClient.GetContainer(
            databaseId: _databaseName,
            containerId: "configs"
        );

        using MemoryStream streamPayload = new();
        await JsonSerializer.SerializeAsync(
            utf8Json: streamPayload,
            value: emailTemplateConfig,
            jsonTypeInfo: _jsonSourceGenerationContext.EmailTemplateConfig
        );

        try
        {
            // Try to create the item.
            await container.UpsertItemStreamAsync(
                streamPayload: streamPayload,
                partitionKey: new(emailTemplateConfig.PartitionKey)
            );
        }
        catch
        {
            // If it already exists, replace it.
            await container.ReplaceItemStreamAsync(
                streamPayload: streamPayload,
                id: emailTemplateConfig.Id,
                partitionKey: new(emailTemplateConfig.PartitionKey)
            );
        }
    }
}