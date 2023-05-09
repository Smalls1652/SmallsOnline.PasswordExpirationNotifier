using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models;

/// <summary>
/// Interface for a user's password expiration details.
/// </summary>
public interface IUserPasswordExpirationDetails
{
    /// <summary>
    /// The unique ID of the user.
    /// </summary>
    string Id { get; set; }

    /// <summary>
    /// The user's data.
    /// </summary>
    User User { get; set; }

    /// <summary>
    /// The ID of the <see cref="Config.UserSearchConfig"/> that was used to find this user.
    /// </summary>
    string UserSearchConfigId { get; set; }

    /// <summary>
    /// The max password age in days.
    /// </summary>
    int MaxPasswordAge { get; set; }

    /// <summary>
    /// The date the user's password was last set.
    /// </summary>
    DateTimeOffset PasswordLastSetDate { get; set; }

    /// <summary>
    /// The total timespan the user's password has been in use.
    /// </summary>
    TimeSpan PasswordLifespan { get; }

    /// <summary>
    /// The date the user's password will expire.
    /// </summary>
    DateTimeOffset PasswordExpirationDate { get; }

    /// <summary>
    /// The timespan until the user's password expires.
    /// </summary>
    TimeSpan PasswordExpiresIn { get; }

    /// <summary>
    /// Whether the user's password has expired or not.
    /// </summary>
    bool PasswordIsExpired { get; }

    /// <summary>
    /// Whether the user's password is expiring soon or not.
    /// </summary>
    bool PasswordIsExpiringSoon { get; }
}