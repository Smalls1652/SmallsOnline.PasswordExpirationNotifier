using Microsoft.AspNetCore.Components;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Services;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Shared.EmailTemplates;

/// <summary>
/// Component for editing an email template.
/// </summary>
public partial class EmailTemplateForm : ComponentBase
{
    /// <summary>
    /// Dependency injected Cosmos DB client service.
    /// </summary>
    [Inject]
    protected ICosmosDbClientService _cosmosDbClientService { get; set; } = null!;

    /// <summary>
    /// Logger for the component.
    /// </summary>
    [Inject]
    protected ILogger<EmailTemplateForm> _logger { get; set; } = null!;

    /// <summary>
    /// Navigation manager for the component to handle navigation.
    /// </summary>
    [Inject]
    protected NavigationManager _navigationManager { get; set; } = null!;

    /// <summary>
    /// The email template to edit.
    /// </summary>
    [Parameter]
    public EmailTemplateConfig EmailTemplateConfig { get; set; } = null!;

    private List<EmailTemplateAttachmentItem>? _attachmentItems;

    private List<EmailTemplateAttachmentItem> _removedAttachmentItems = new();

    protected override void OnInitialized()
    {
        if (EmailTemplateConfig.IncludedAttachments is not null)
        {
            // If the email template has attachments, set the attachment items to
            // the _attachmentItems list.
            _attachmentItems = new(EmailTemplateConfig.IncludedAttachments);
        }
        else
        {
            // Otherwise, set the _attachmentItems list to a new list.
            _attachmentItems = new();
        }
    }

    /// <summary>
    /// Method for handling the form submission.
    /// </summary>
    private async Task HandleFormSubmitAsync()
    {
        _logger.LogInformation("Updating template...");

        // Set the last modified date to now.
        EmailTemplateConfig.TemplateLastModified = DateTimeOffset.Now;

        // Update the email template in the database.
        await _cosmosDbClientService.CreateOrUpdateEmailTemplateConfigAsync(EmailTemplateConfig);

        if (_attachmentItems is not null)
        {
            // If the _attachmentItems list is not null, update the attachments.
            _logger.LogInformation("Updating attachments...");
            await _cosmosDbClientService.CreateOrUpdateEmailAttachmentItemAsync(_attachmentItems!.ToArray());
        }

        if (_removedAttachmentItems.Count != 0)
        {
            // If the _removedAttachmentItems list has items, remove the attachments.
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

        // Navigate to the email templates page.
        _navigationManager.NavigateTo("/email-templates");
    }

    /// <summary>
    /// Method for handling when an attachment is added.
    /// </summary>
    private void HandleAddAttachment()
    {
        _logger.LogInformation("Adding attachment...");
        if (_attachmentItems is null)
        {
            // If the _attachmentItems list is null, set it to a new list.
            _attachmentItems = new();
        }

        // Create a new attachment item and add it to the _attachmentItems list.
        _attachmentItems.Add(new EmailTemplateAttachmentItem()
        {
            Id = Guid.NewGuid().ToString(),
            PartitionKey = "email-template-attachment-item",
            FileName = "new-attachment.jpg",
            IsUploaded = false
        });

        // Add the attachment item's id to the email template's included attachment ids.
        EmailTemplateConfig.IncludedAttachmentIds = _attachmentItems.Select(item => item.Id).ToArray();
    }

    /// <summary>
    /// Method for handling when an attachment item is deleted.
    /// </summary>
    /// <param name="attachmentItem">The attachment to remove.</param>
    private void HandleDeleteAttachment(EmailTemplateAttachmentItem attachmentItem)
    {
        _logger.LogInformation("Deleting attachment...");
        // Remove the attachment item from the _attachmentItems list and the email template's included attachment ids.
        _attachmentItems!.Remove(attachmentItem);
        EmailTemplateConfig.IncludedAttachmentIds = Array.FindAll(EmailTemplateConfig.IncludedAttachmentIds!, item => item != attachmentItem.Id);

        // Add the attachment item to the _removedAttachmentItems list.
        _removedAttachmentItems.Add(attachmentItem);
    }
}