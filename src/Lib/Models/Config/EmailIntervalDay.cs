using System.Text.Json.Serialization;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

public class EmailIntervalDay
{
    [JsonPropertyName("value")]
    public int Value { get; set; }
}