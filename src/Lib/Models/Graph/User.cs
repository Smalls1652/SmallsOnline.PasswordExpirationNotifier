using System.Text.Json.Serialization;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

/// <summary>
/// Represents a user.
/// </summary>
public class User : IUser
{
    /// <inheritdoc />
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("accountEnabled")]
    public bool AccountEnabled { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("userPrincipalName")]
    public string UserPrincipalName { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("displayName")]
    public string? DisplayName { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("givenName")]
    public string? GivenName { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("surname")]
    public string? Surname { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("lastPasswordChangeDateTime")]
    public DateTimeOffset? LastPasswordChangeDateTime { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("onPremisesDistinguishedName")]
    public string? OnPremisesDistinguishedName { get; set; }
}