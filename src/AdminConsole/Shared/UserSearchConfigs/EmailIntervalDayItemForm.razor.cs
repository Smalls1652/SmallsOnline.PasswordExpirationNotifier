using Microsoft.AspNetCore.Components;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Shared.UserSearchConfigs;

/// <summary>
/// Component for an email interval day item form.
/// </summary>
public partial class EmailIntervalDayItemForm : ComponentBase
{
    /// <summary>
    /// The day, relative to when the password expires, to send the email on.
    /// </summary>
    [Parameter]
    public int EmailIntervalDay { get; set; }
}