using System.ComponentModel.DataAnnotations;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Shared.Users;

public class SelectedViewUserSettings
{
    [Required]
    public string? UserPrincipalName { get; set; }

    [Required]
    [StringLength(int.MaxValue, MinimumLength = 1)]
    public string? SearchConfigId { get; set; }
}