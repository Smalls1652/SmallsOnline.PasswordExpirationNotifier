namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models;

/// <summary>
/// Interface for manually sending an email.
/// </summary>
public interface IManualSendEmailItem
{
    /// <summary>
    /// The user principal name of the user to send the email to.
    /// </summary>
    string? UserPrincipalName { get; set; }

    /// <summary>
    /// The unique ID of the <see cref="Config.UserSearchConfig"/> to use for the email.
    /// </summary>
    string? UserSearchConfigId { get; set; }

    /// <summary>
    /// A unique ID to correlate the user search with the email sending.
    /// </summary>
    string CorrelationId { get; set; }
}