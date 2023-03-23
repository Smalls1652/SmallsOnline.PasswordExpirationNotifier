using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Shared.EmailTemplates;

public partial class EmailAttachmentItemForm : ComponentBase
{
    [Inject]
    protected ILogger<EmailAttachmentItemForm> _logger { get; set; } = null!;

    [Inject]
    protected IJSRuntime _jsRuntime { get; set; } = null!;

    [Parameter]
    public IEmailTemplateAttachmentItem AttachmentItem { get; set; } = null!;

    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private IJSObjectReference? _jsModule;
    private InputFile? _inputFile;
    private ElementReference _imgElement;
    private byte[]? _imgBytes;
    private string? _blobUrl;
    private bool _isFinishedLoading;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _isFinishedLoading = false;
            _jsModule = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./Shared/EmailTemplates/EmailAttachmentItemForm.razor.js");

            if (AttachmentItem.FileContents is not null)
            {
                _imgBytes = Convert.FromBase64String(AttachmentItem.FileContents);
                _blobUrl = await _jsModule!.InvokeAsync<string>("initPreviewImg", _imgBytes, _imgElement);
            }

            _isFinishedLoading = true;
        }
    }

    private async Task HandleAttachmentChangeAsync(InputFileChangeEventArgs eventArgs)
    {
        _logger.LogInformation("Attachment image changed.");
        StreamContent stream = new(eventArgs.File.OpenReadStream());
        byte[] buffer = await stream.ReadAsByteArrayAsync();

        string base64FileContents = Convert.ToBase64String(buffer);
        AttachmentItem.FileContents = base64FileContents;
        AttachmentItem.FileName = eventArgs.File.Name;

        _blobUrl = await _jsModule!.InvokeAsync<string>("updatePreviewImage", _inputFile!.Element, _imgElement);
    }
}