using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Shared.EmailTemplates;

/// <summary>
/// Component for editing an email attachment item.
/// </summary>
public partial class EmailAttachmentItemForm : ComponentBase
{
    /// <summary>
    /// Logger for the component.
    /// </summary>
    [Inject]
    protected ILogger<EmailAttachmentItemForm> _logger { get; set; } = null!;

    /// <summary>
    /// JavaScript interop runtime.
    /// </summary>
    [Inject]
    protected IJSRuntime _jsRuntime { get; set; } = null!;

    /// <summary>
    /// The email template attachment item to edit.
    /// </summary>
    [Parameter]
    public IEmailTemplateAttachmentItem AttachmentItem { get; set; } = null!;

    /// <summary>
    /// The child content of the component.
    /// </summary>
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

            // Load the JavaScript module for the component.
            _jsModule = await _jsRuntime.InvokeAsync<IJSObjectReference>("import", "./Shared/EmailTemplates/EmailAttachmentItemForm.razor.js");

            // If the attachment item has a file, load the image.
            if (AttachmentItem.FileContents is not null)
            {
                _imgBytes = Convert.FromBase64String(AttachmentItem.FileContents);
                _blobUrl = await _jsModule!.InvokeAsync<string>("initPreviewImg", _imgBytes, _imgElement);
            }

            _isFinishedLoading = true;
        }
    }

    /// <summary>
    /// Method for when the attachment image is changed in the input file element.
    /// </summary>
    /// <param name="eventArgs"></param>
    private async Task HandleAttachmentChangeAsync(InputFileChangeEventArgs eventArgs)
    {
        _logger.LogInformation("Attachment image changed.");

        // Get the file contents and write it to a byte array.
        StreamContent stream = new(eventArgs.File.OpenReadStream());
        byte[] buffer = await stream.ReadAsByteArrayAsync();

        // Convert the byte array to a base64 string and set the file contents and file name.
        string base64FileContents = Convert.ToBase64String(buffer);
        AttachmentItem.FileContents = base64FileContents;
        AttachmentItem.FileName = eventArgs.File.Name;

        // Update the preview image.
        _blobUrl = await _jsModule!.InvokeAsync<string>("updatePreviewImage", _inputFile!.Element, _imgElement);
    }
}