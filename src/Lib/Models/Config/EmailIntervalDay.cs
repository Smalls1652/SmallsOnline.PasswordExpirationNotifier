using System.Text.Json.Serialization;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

/// <summary>
/// Holds the day to send the email.
/// </summary>
public class EmailIntervalDay
{
    /// <summary>
    /// The day to send the email.
    /// </summary>
    [JsonPropertyName("value")]
    public int Value { get; set; }
}