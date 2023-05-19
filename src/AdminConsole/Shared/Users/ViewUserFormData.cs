using System.ComponentModel.DataAnnotations;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Shared.Users;

/// <summary>
/// Holds form data for the selected user and search configuration on the <see cref="SmallsOnline.PasswordExpirationNotifier.AdminConsole.Pages.ViewUser" /> page.
/// </summary>
public class ViewUserFormData
{
    /// <summary>
    /// The user principal name of the user.
    /// </summary>
    [Required]
    public string? UserPrincipalName { get; set; }

    /// <summary>
    /// The ID of the <see cref="SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config.UserSearchConfig"/> to use.
    /// </summary>
    [Required]
    [StringLength(int.MaxValue, MinimumLength = 1)]
    public string? SearchConfigId { get; set; }
}