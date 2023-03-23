namespace SmallsOnline.PasswordExpirationNotifier.Lib.Models;

public interface IManualSendEmailItem
{
    string? UserPrincipalName { get; set; }
    string? UserSearchConfigId { get; set; }
}