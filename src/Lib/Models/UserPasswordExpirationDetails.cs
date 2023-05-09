using System.Text.Json.Serialization;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models;

/// <summary>
/// Holds information about a user's password expiration.
/// </summary>
public class UserPasswordExpirationDetails : IUserPasswordExpirationDetails
{
    [JsonConstructor]
    public UserPasswordExpirationDetails()
    {}

    public UserPasswordExpirationDetails(User user, UserSearchConfig searchConfig)
    {
        Id = user.Id;
        User = user;
        UserSearchConfigId = searchConfig.Id;
        PasswordLastSetDate = user.LastPasswordChangeDateTime is not null ? user.LastPasswordChangeDateTime.Value.UtcDateTime : DateTimeOffset.MinValue;
        MaxPasswordAge = searchConfig.MaxPasswordAge;
    }

    public UserPasswordExpirationDetails(User user, UserSearchConfig searchConfig, string correlationId)
    {
        Id = user.Id;
        User = user;
        UserSearchConfigId = searchConfig.Id;
        PasswordLastSetDate = user.LastPasswordChangeDateTime is not null ? user.LastPasswordChangeDateTime.Value.UtcDateTime : DateTimeOffset.MinValue;
        MaxPasswordAge = searchConfig.MaxPasswordAge;
        CorrelationId = correlationId;
    }

    /// <inheritdoc />
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("user")]
    public User User { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("userSearchConfigId")]
    public string UserSearchConfigId { get; set; } = null!;

    /// <inheritdoc />
    [JsonPropertyName("maxPasswordAge")]
    public int MaxPasswordAge { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("passwordLastSetDate")]
    public DateTimeOffset PasswordLastSetDate { get; set; }

    /// <inheritdoc />
    [JsonPropertyName("passwordLifespan")]
    public TimeSpan PasswordLifespan => DateTimeOffset.Now - PasswordLastSetDate;

    /// <inheritdoc />
    [JsonPropertyName("passwordExpirationDate")]
    public DateTimeOffset PasswordExpirationDate => PasswordLastSetDate.AddDays(MaxPasswordAge);

    /// <inheritdoc />
    [JsonPropertyName("passwordExpiresIn")]
    public TimeSpan PasswordExpiresIn => PasswordExpirationDate - DateTimeOffset.Now;

    /// <inheritdoc />
    [JsonPropertyName("passwordIsExpired")]
    public bool PasswordIsExpired => DateTimeOffset.Now >= PasswordExpirationDate;

    /// <inheritdoc />
    [JsonPropertyName("passwordIsExpiringSoon")]
    public bool PasswordIsExpiringSoon => PasswordExpiresIn.Days <= TimeSpan.FromDays(10).Days;

    /// <inheritdoc />
    [JsonPropertyName("correlationId")]
    public string CorrelationId { get; set; } = null!;
}