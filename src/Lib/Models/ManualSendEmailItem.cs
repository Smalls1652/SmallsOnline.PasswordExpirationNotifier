using System.Text.Json.Serialization;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models;

/// <summary>
/// Holds information for manually sending an email.
/// </summary>
public class ManualSendEmailItem : IManualSendEmailItem
{
    /// <inheritdoc />
    [JsonPropertyName("userPrincipalName")]
    public string? UserPrincipalName { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("userSearchConfigId")]
    public string? UserSearchConfigId { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("correlationId")]
    public string CorrelationId { get; set; } = null!;
}