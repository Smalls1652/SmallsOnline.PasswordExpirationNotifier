namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

/// <summary>
/// Interface representing a user item in Microsoft Graph.
/// </summary>
public interface IUser
{
    /// <summary>
    /// The unique identifier for the user in Azure AD.
    /// </summary>
    string Id { get; set; }

    /// <summary>
    /// Whether the account is enabled or not.
    /// </summary>
    bool AccountEnabled { get; set; }

    /// <summary>
    /// The user's user principal name (UPN).
    /// </summary>
    string UserPrincipalName { get; set; }

    /// <summary>
    /// The display name for the user.
    /// </summary>
    string? DisplayName { get; set; }

    /// <summary>
    /// The user's given name (First name).
    /// </summary>
    string? GivenName { get; set; }

    /// <summary>
    /// The user's surname (Last name).
    /// </summary>
    string? Surname { get; set; }

    /// <summary>
    /// The <see cref="DateTimeOffset"/> value of when the user's password was last changed.
    /// </summary>
    DateTimeOffset LastPasswordChangeDateTime { get; set; }

    /// <summary>
    /// The on-premises Active Directory distinguished name for the user.
    /// </summary>
    string? OnPremisesDistinguishedName { get; set; }
}