using System.Text.Json;
using Microsoft.Azure.Cosmos;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class CosmosDbClientService
{
    /// <summary>
    /// Get a user search config from Cosmos DB.
    /// </summary>
    /// <param name="id">The unique ID for the user search config.</param>
    /// <returns>The <see cref="UserSearchConfig"/> item.</returns>
    public async Task<UserSearchConfig> GetUserSearchConfigAsync(string id)
    {
        // Get the Cosmos DB container.
        Container container = _cosmosClient.GetContainer(
            databaseId: _databaseName,
            containerId: "configs"
        );

        // Get the user search config item from Cosmos DB.
        ResponseMessage configItemRsp = await container.ReadItemStreamAsync(
            id: id,
            partitionKey: new("user-search-config")
        );

        // Deserialize the user search config item.
        UserSearchConfig configItem = JsonSerializer.Deserialize(
            utf8Json: configItemRsp.Content,
            jsonTypeInfo: _jsonSourceGenerationContext.UserSearchConfig
        )!;

        return configItem;
    }
}