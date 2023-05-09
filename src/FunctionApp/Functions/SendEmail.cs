﻿using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using SmallsOnline.PasswordExpirationNotifier.FunctionApp.Services;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;
using SmallsOnline.PasswordExpirationNotifier.Lib.Services;

namespace SmallsOnline.PasswordExpirationNotifier.FunctionApp;

/// <summary>
/// Function for sending an email to a user when a new item is added to the email queue.
/// </summary>
public class SendEmail
{
    private readonly ICosmosDbClientService _cosmosDbClientService;
    private readonly IGraphClientService _graphClientService;
    private readonly IConfigService _configService;
    private readonly JsonSourceGenerationContext _jsonSourceGenerationContext = new();

    public SendEmail(ICosmosDbClientService cosmosDbClientService, IGraphClientService graphClientService, IConfigService configService)
    {
        _cosmosDbClientService = cosmosDbClientService;
        _graphClientService = graphClientService;
        _configService = configService;
    }

    [Function("SendEmail")]
    public async Task Run([QueueTrigger("email-queue")] string queueItemContents,
        FunctionContext context)
    {
        var logger = context.GetLogger("SendEmail");

        // Deserialize the queue item.
        UserPasswordExpirationDetails queueItem = JsonSerializer.Deserialize(
            json: queueItemContents,
            jsonTypeInfo: _jsonSourceGenerationContext.UserPasswordExpirationDetails
        )!;

        logger.LogInformation("Processing queue item for '{UserPrincipalName}'. [CorrelationId: {CorrelationId}]", queueItem.User.UserPrincipalName, queueItem.CorrelationId);

        // Get the user search config and email template config for the queue item.
        UserSearchConfig userSearchConfigItem = Array.Find(_configService.UserSearchConfigs, config => config.Id == queueItem.UserSearchConfigId)!;
        EmailTemplateConfig emailTemplateConfigItem = Array.Find(_configService.EmailTemplateConfigs, config => config.Id == userSearchConfigItem.EmailTemplateId)!;

        logger.LogInformation("Resolved search config to '{ConfigName} [{ConfigId}]'. [CorrelationId: {CorrelationId}]", userSearchConfigItem.ConfigName, userSearchConfigItem.Id, queueItem.CorrelationId);
        logger.LogInformation("Sending email with template '{EmailTemplateName} [{EmailTemplateId}]' to '{UserPrincipalName}'. [CorrelationId: {CorrelationId}]", emailTemplateConfigItem.TemplateName, emailTemplateConfigItem.Id, queueItem.User.UserPrincipalName, queueItem.CorrelationId);

        if (userSearchConfigItem.IsEmailIntervalsEnabled == true && userSearchConfigItem.EmailIntervalDays!.Find(interval => interval.Value == (int)Math.Round(queueItem.PasswordExpiresIn.TotalDays, 0)) is null)
        {
            logger.LogInformation("{UserPrincipalName} is not expected to receive an email yet. Skipping... [CorrelationId: {CorrelationId}]", queueItem.User.UserPrincipalName, queueItem.CorrelationId);
            return;
        }

        // Convert the expiration date to a specific timezone.
        // TODO: Make this configurable.

        DateTimeOffset expirationTimeToTimezone = userSearchConfigItem.DefaultTimeZone is not null
            ? TimeZoneInfo.ConvertTime(queueItem.PasswordExpirationDate, TimeZoneInfo.FindSystemTimeZoneById(userSearchConfigItem.DefaultTimeZone))
            : queueItem.PasswordExpirationDate;

        // Create the body of the email with the template and the queue item data.
        string emailBody = emailTemplateConfigItem.TemplateHtml!
            .Replace("{{USERNAME}}", queueItem.User.DisplayName)
            .Replace("{{EXPIREINDAYS}}", Math.Round(queueItem.PasswordExpiresIn.TotalDays, 0).ToString("00"))
            .Replace("{{EXPIREDATE}}", expirationTimeToTimezone.ToString("MMMM dd, yyyy hh:mm tt zzz"));

        // Get the attachments for the email.
        FileAttachment[]? fileAttachments = null;
        if (emailTemplateConfigItem.IncludedAttachments is not null)
        {
            fileAttachments = new FileAttachment[emailTemplateConfigItem.IncludedAttachments.Length];
            for (int i = 0; i < emailTemplateConfigItem.IncludedAttachments.Length; i++)
            {
                fileAttachments[i] = new(
                    fileName: emailTemplateConfigItem.IncludedAttachments[i].FileName,
                    fileContentBytesBase64: emailTemplateConfigItem.IncludedAttachments[i].FileContents!,
                    isInline: emailTemplateConfigItem.IncludedAttachments[i].IsInline
                );
            }
        }

        // Create the email message.
        MailMessage emailMessage = new(
            message: new(
                subject: $"Alert: Password Expiration Notice ({Math.Round(queueItem.PasswordExpiresIn.TotalDays, 0)} days)",
                body: new(emailBody, "HTML"),
                toRecipient: new Recipient[]
                {
                    new(queueItem.User.UserPrincipalName, null)
                },
                attachments: fileAttachments
            )
            {
                CcRecipient = new Recipient[] { }
            },
            saveToSentItems: false
        );

        if (!userSearchConfigItem.DoNotSendEmails)
        {
            await _graphClientService.SendEmailAsync(emailMessage, emailTemplateConfigItem.TemplateSendAsUser!);
        }
        else
        {
            logger.LogWarning("Email sending is disabled for this search config. Skipping... [CorrelationId: {CorrelationId}]", queueItem.CorrelationId);
        }
    }
}