using System.Text.Json;
using Azure.Core.Serialization;
using Microsoft.Azure.Cosmos;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Json;

public class CosmosSystemTextJsonSerializer : CosmosSerializer
{
    private readonly JsonObjectSerializer _jsonObjectSerializer;

    public CosmosSystemTextJsonSerializer(JsonSerializerOptions options)
    {
        _jsonObjectSerializer = new(options);
    }

    public override T FromStream<T>(Stream stream)
    {
        if (stream.CanSeek && stream.Length == 0)
        {
            return default!;
        }

        if (typeof(Stream).IsAssignableFrom(typeof(T)))
        {
            return (T)(object)stream;
        }

        T? deserializedItem = (T?)_jsonObjectSerializer.Deserialize(
            stream: stream,
            returnType: typeof(T?),
            cancellationToken: default
        );

        if (deserializedItem is null)
        {
            throw new JsonException($"Unable to deserialize {typeof(T).Name} from Cosmos DB.");
        }

        stream.Dispose();

        return deserializedItem;
    }

    public override Stream ToStream<T>(T input)
    {
        MemoryStream memoryStream = new();

        _jsonObjectSerializer.Serialize(
            stream: memoryStream,
            value: input,
            inputType: typeof(T),
            cancellationToken: default
        );
        memoryStream.Position = 0;

        return memoryStream;
    }
}