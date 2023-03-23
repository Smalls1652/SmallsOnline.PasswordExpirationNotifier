using Microsoft.AspNetCore.Components;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Services;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Shared.EmailTemplates;

public partial class EmailTemplateForm : ComponentBase
{
    [Inject]
    protected ICosmosDbClientService _cosmosDbClientService { get; set; } = null!;

    [Inject]
    protected ILogger<EmailTemplateForm> _logger { get; set; } = null!;

    [Inject]
    protected NavigationManager _navigationManager { get; set; } = null!;

    [Parameter]
    public EmailTemplateConfig EmailTemplateConfig { get; set; } = null!;

    private List<EmailTemplateAttachmentItem>? _attachmentItems;

    private List<EmailTemplateAttachmentItem> _removedAttachmentItems = new();

    protected override void OnInitialized()
    {
        if (EmailTemplateConfig.IncludedAttachments is not null)
        {
            _attachmentItems = new(EmailTemplateConfig.IncludedAttachments);
        }
        else
        {
            _attachmentItems = new();
        }
    }

    private async Task HandleFormSubmitAsync()
    {
        _logger.LogInformation("Updating template...");

        EmailTemplateConfig.TemplateLastModified = DateTimeOffset.Now;

        await _cosmosDbClientService.CreateOrUpdateEmailTemplateConfigAsync(EmailTemplateConfig);

        if (_attachmentItems is not null)
        {
            _logger.LogInformation("Updating attachments...");
            await _cosmosDbClientService.CreateOrUpdateEmailAttachmentItemAsync(_attachmentItems!.ToArray());
        }

        if (_removedAttachmentItems.Count != 0)
        {
            _logger.LogInformation("Removing attachments...");
            foreach (var attachmentItem in _removedAttachmentItems)
            {
                if (attachmentItem.IsUploaded)
                {
                    await _cosmosDbClientService.RemoveEmailAttachmentItemAsync(attachmentItem.Id);
                }
                else
                {
                    _logger.LogInformation("Attachment item, {ItemName} was not uploaded, so it was not removed.", attachmentItem.FileName);
                }
            }
        }

        _navigationManager.NavigateTo("/email-templates");
    }

    private void HandleAddAttachment()
    {
        _logger.LogInformation("Adding attachment...");
        if (_attachmentItems is null)
        {
            _attachmentItems = new();
        }

        _attachmentItems.Add(new EmailTemplateAttachmentItem()
        {
            Id = Guid.NewGuid().ToString(),
            PartitionKey = "email-template-attachment-item",
            FileName = "new-attachment.jpg",
            IsUploaded = false
        });

        EmailTemplateConfig.IncludedAttachmentIds = _attachmentItems.Select(item => item.Id).ToArray();
    }

    private void HandleDeleteAttachment(EmailTemplateAttachmentItem attachmentItem)
    {
        _logger.LogInformation("Deleting attachment...");
        _attachmentItems!.Remove(attachmentItem);
        EmailTemplateConfig.IncludedAttachmentIds = Array.FindAll(EmailTemplateConfig.IncludedAttachmentIds!, item => item != attachmentItem.Id);
        _removedAttachmentItems.Add(attachmentItem);
    }
}