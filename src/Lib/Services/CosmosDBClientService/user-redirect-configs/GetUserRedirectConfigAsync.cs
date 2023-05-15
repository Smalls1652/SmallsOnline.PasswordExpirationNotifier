using System.Text.Json;
using Microsoft.Azure.Cosmos;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class CosmosDbClientService
{
    /// <inheritdoc />
    public async Task<UserEmailRedirect> GetUserRedirectConfigAsync(string id)
    {
        // Get the Cosmos DB container.
        Container container = _cosmosClient.GetContainer(
            databaseId: _databaseName,
            containerId: "configs"
        );

        // Get the user search config item from Cosmos DB.
        ResponseMessage configItemRsp = await container.ReadItemStreamAsync(
            id: id,
            partitionKey: new("user-redirect-config")
        );

        // Deserialize the user search config item.
        UserEmailRedirect configItem = JsonSerializer.Deserialize(
            utf8Json: configItemRsp.Content,
            jsonTypeInfo: _jsonSourceGenerationContext.UserEmailRedirect
        )!;

        return configItem;
    }
}