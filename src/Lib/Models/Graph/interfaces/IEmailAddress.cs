namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

/// <summary>
/// Interface representing an email address item in Microsoft Graph.
/// </summary>
public interface IEmailAddress
{
    /// <summary>
    /// The email address.
    /// </summary>
    string Address { get; set; }

    /// <summary>
    /// The display name of the email address.
    /// </summary>
    string? Name { get; set; }
}