using System.Text.Json.Serialization;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Json;

public class CosmosResponse
{
    [JsonPropertyName("Documents")]
    public string Documents { get; set; } = null!;
}