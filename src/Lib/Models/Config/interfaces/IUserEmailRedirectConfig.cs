namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

public interface IUserEmailRedirectConfig
{
    string Id { get; set; }
    string PartitionKey { get; set; }
    string? UserPrincipalName { get; set; }
    string? RedirectUserPrincipalName { get; set; }
}