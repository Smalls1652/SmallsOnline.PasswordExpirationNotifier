using Microsoft.AspNetCore.Components;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Shared.Common;

public partial class FullscreenImage : ComponentBase
{
    [Parameter]
    public IEmailTemplateAttachmentItem? AttachmentItem { get; set; }

    [Parameter]
    public bool IsVisible { get; set; }
}