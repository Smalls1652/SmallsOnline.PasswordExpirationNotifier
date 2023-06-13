using SmallsOnline.MsGraphClient.Models;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

/// <summary>
/// Interface for the Graph Client Service.
/// </summary>
public interface IGraphClientService
{
    /// <summary>
    /// The underlying <see cref="SmallsOnline.MsGraphClient.Models.GraphClient"/> instance.
    /// </summary>
    GraphClient GraphClient { get; }

    /// <summary>
    /// Get user information from the Microsoft Graph API.
    /// </summary>
    /// <param name="userId">The unique ID of the user.</param>
    /// <returns>The user's data.</returns>
    /// <exception cref="NullReferenceException">No user was found.</exception>
    Task<User> GetUserAsync(string userId);

    /// <summary>
    /// Get all users from the Microsoft Graph API, based on the provided parameters.
    /// </summary>
    /// <param name="domainName">The domain name of the users.</param>
    /// <param name="ouPath">The OU path for the users in an on-premises Active Directory domain.</param>
    /// <param name="lastNameStartsWith">The first character of the users' last name.</param>
    /// <returns>A collection of <see cref="User">users</see>.</returns>
    /// <exception cref="NullReferenceException">No users were found,</exception>
    Task<User[]?> GetUsersAsync(string domainName, string? ouPath, string? lastNameStartsWith);

    /// <summary>
    /// Send an email using the Microsoft Graph API.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <param name="sendAsUser">The user principal name of the user to send the email as.</param>
    /// <exception cref="Exception"></exception>
    Task SendEmailAsync(Message message, string sendAsUser);
}