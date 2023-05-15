namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

/// <summary>
/// Interface for user email redirect configs.
/// </summary>
public interface IUserEmailRedirectConfig
{
    /// <summary>
    /// The unique identifier for the config.
    /// </summary>
    string Id { get; set; }

    /// <summary>
    /// The partition key for the config in the database.
    /// </summary>
    string PartitionKey { get; set; }

    /// <summary>
    /// The user principal name to redirect emails from.
    /// </summary>
    string? UserPrincipalName { get; set; }

    /// <summary>
    /// The user principal name to redirect emails to.
    /// </summary>
    string? RedirectUserPrincipalName { get; set; }
}