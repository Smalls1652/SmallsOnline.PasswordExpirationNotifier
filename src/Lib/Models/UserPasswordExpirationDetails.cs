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
        PasswordLastSetDate = user.LastPasswordChangeDateTime is not null ? user.LastPasswordChangeDateTime.Value : DateTimeOffset.MinValue;
        MaxPasswordAge = searchConfig.MaxPasswordAge;
    }

    /// <summary>
    /// The unique ID of the user.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <summary>
    /// The user's data.
    /// </summary>
    [JsonPropertyName("user")]
    public User User { get; set; } = null!;

    /// <summary>
    /// The ID of the user search config that was used to find this user.
    /// </summary>
    [JsonPropertyName("userSearchConfigId")]
    public string UserSearchConfigId { get; set; } = null!;

    /// <summary>
    /// The max password age in days.
    /// </summary>
    [JsonPropertyName("maxPasswordAge")]
    public int MaxPasswordAge { get; set; }

    /// <summary>
    /// The date the user's password was last set.
    /// </summary>
    [JsonPropertyName("passwordLastSetDate")]
    public DateTimeOffset PasswordLastSetDate { get; set; }

    /// <summary>
    /// The total timespan the user's password has been in use.
    /// </summary>
    [JsonPropertyName("passwordLifespan")]
    public TimeSpan PasswordLifespan => DateTimeOffset.Now - PasswordLastSetDate;

    /// <summary>
    /// The date the user's password will expire.
    /// </summary>
    [JsonPropertyName("passwordExpirationDate")]
    public DateTimeOffset PasswordExpirationDate => PasswordLastSetDate.AddDays(MaxPasswordAge);

    /// <summary>
    /// The timespan until the user's password expires.
    /// </summary>
    [JsonPropertyName("passwordExpiresIn")]
    public TimeSpan PasswordExpiresIn => PasswordExpirationDate - DateTimeOffset.Now;

    /// <summary>
    /// Whether the user's password has expired or not.
    /// </summary>
    [JsonPropertyName("passwordIsExpired")]
    public bool PasswordIsExpired => DateTimeOffset.Now >= PasswordExpirationDate;

    /// <summary>
    /// Whether the user's password is expiring soon or not.
    /// </summary>
    [JsonPropertyName("passwordIsExpiringSoon")]
    public bool PasswordIsExpiringSoon => PasswordExpiresIn.Days <= TimeSpan.FromDays(10).Days;
}