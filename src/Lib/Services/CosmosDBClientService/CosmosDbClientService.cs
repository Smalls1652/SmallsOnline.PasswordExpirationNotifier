using System.Text.Json;
using Microsoft.Azure.Cosmos;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Json;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

/// <summary>
/// Service for interacting with Cosmos DB.
/// </summary>
public partial class CosmosDbClientService : ICosmosDbClientService, IDisposable
{
    private readonly string _databaseName;
    private readonly CosmosClient _cosmosClient;
    private readonly JsonSourceGenerationContext _jsonSourceGenerationContext = new();
    private bool _isDisposed = false;

    public CosmosDbClientService(string connectionString, string databaseName)
    {
        _databaseName = databaseName;
        _cosmosClient = new(
            connectionString: connectionString,
            clientOptions: new()
            {
                Serializer = new CosmosSystemTextJsonSerializer(new JsonSerializerOptions())
            }
        );
    }



    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_isDisposed)
        {
            return;
        }

        if (disposing)
        {
            _cosmosClient.Dispose();
        }

        _isDisposed = true;
    }
}