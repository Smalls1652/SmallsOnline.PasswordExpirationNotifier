using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Shared.Common;

/// <summary>
/// Component for a delete modal.
/// </summary>
public partial class DeleteModal : ComponentBase
{
    /// <summary>
    /// The ID the modal should have.
    /// </summary>
    [Parameter]
    public string? ModalId { get; set; }

    /// <summary>
    /// The title of the modal.
    /// </summary>
    [Parameter]
    public string? ModalTitle { get; set; }

    /// <summary>
    /// Any child content to be rendered in the modal.
    /// </summary>
    [Parameter]
    public RenderFragment ChildContent { get; set; } = null!;

    /// <summary>
    /// Data to be passed to the callback.
    /// </summary>
    [Parameter]
    public string? CallbackData { get; set; }

    /// <summary>
    /// Data returned from the callback.
    /// </summary>
    [Parameter]
    public EventCallback<string> OnConfirmedCallback { get; set; }

    /// <summary>
    /// Invoke the callback when the confirm button is clicked.
    /// </summary>
    /// <param name="eventArgs">Information about a mouse event.</param>
    private async Task HandleConfirmedAsync(MouseEventArgs eventArgs)
    {
        await OnConfirmedCallback.InvokeAsync(CallbackData);
    }
}