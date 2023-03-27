using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Shared.Common;

public partial class DeleteModal : ComponentBase
{
    [Parameter]
    public string? ModalId { get; set; }

    [Parameter]
    public string? ModalTitle { get; set; }

    [Parameter]
    public RenderFragment ChildContent { get; set; } = null!;

    [Parameter]
    public string? CallbackData { get; set; }

    [Parameter]
    public EventCallback<string> OnConfirmedCallback { get; set; }

    private async Task HandleConfirmedAsync(MouseEventArgs eventArgs)
    {
        await OnConfirmedCallback.InvokeAsync(CallbackData);
    }
}