using System.Text.Json.Serialization;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models;

public class CosmosDbResponse<T>
{
    [JsonPropertyName("Documents")]
    public T[]? Documents { get; set; }

    [JsonPropertyName("_count")]
    public int Count { get; set; }
}