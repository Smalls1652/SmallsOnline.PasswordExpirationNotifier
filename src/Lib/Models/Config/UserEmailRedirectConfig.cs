using System.Text.Json.Serialization;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

/// <summary>
/// Holds config data for redirecting emails for one user to another.
/// </summary>
public class UserEmailRedirectConfig : IUserEmailRedirectConfig
{
    /// <inheritdoc />
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("partitionKey")]
    public string PartitionKey { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("userPrincipalName")]
    public string? UserPrincipalName { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("redirectUserPrincipalName")]
    public string? RedirectUserPrincipalName { get; set; }
}