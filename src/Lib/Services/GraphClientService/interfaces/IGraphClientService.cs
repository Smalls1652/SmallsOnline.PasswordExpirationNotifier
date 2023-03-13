using SmallsOnline.MsGraphClient.Models;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public interface IGraphClientService
{
    GraphClient GraphClient { get; }
    Task<User> GetUserAsync(string userId);
    Task<User[]> GetUsersAsync(string domainName, string? ouPath, string? lastNameStartsWith);
    Task SendEmailAsync(MailMessage message, string sendAsUser);
}