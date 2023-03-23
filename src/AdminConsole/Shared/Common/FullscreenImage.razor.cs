using Microsoft.AspNetCore.Components;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Shared.Common;

/// <summary>
/// Component for displaying an email attachment image in a fullscreen overlay.
/// </summary>
public partial class FullscreenImage : ComponentBase
{
    /// <summary>
    /// The email template attachment item to display.
    /// </summary>
    [Parameter]
    public IEmailTemplateAttachmentItem? AttachmentItem { get; set; }

    /// <summary>
    /// Whether the image is visible.
    /// </summary>
    /// <remarks>
    /// Not implemented yet.
    /// </remarks>
    [Parameter]
    public bool IsVisible { get; set; }
}