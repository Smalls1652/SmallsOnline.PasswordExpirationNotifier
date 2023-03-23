using Microsoft.AspNetCore.Components;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Shared.UserSearchConfigs;

public partial class EmailIntervalDayItemForm : ComponentBase
{
    [Parameter]
    public int EmailIntervalDay { get; set; }
}