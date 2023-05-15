using System.Text.Json;
using Microsoft.ApplicationInsights;
using Microsoft.Azure.Functions.Worker;
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
    private readonly TelemetryClient _telemetryClient;

    public SendEmail(ICosmosDbClientService cosmosDbClientService, IGraphClientService graphClientService, IConfigService configService, TelemetryClient telemetryClient)
    {
        _cosmosDbClientService = cosmosDbClientService;
        _graphClientService = graphClientService;
        _configService = configService;
        _telemetryClient = telemetryClient;
    }

    [Function("SendEmail")]
    public async Task Run([QueueTrigger("email-queue")] string queueItemContents,
        FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger("SendEmail");

        // Deserialize the queue item.
        UserPasswordExpirationDetails queueItem = JsonSerializer.Deserialize(
            json: queueItemContents,
            jsonTypeInfo: _jsonSourceGenerationContext.UserPasswordExpirationDetails
        )!;

        LoggingHelper.LogInformation(_configService.AppInsightsEnabled ? _telemetryClient : logger, "Processing queue item for '{0}'.", queueItem.CorrelationId, executionContext, queueItem.User.UserPrincipalName);

        // Get the user search config and email template config for the queue item.
        UserSearchConfig userSearchConfigItem = Array.Find(_configService.UserSearchConfigs, config => config.Id == queueItem.UserSearchConfigId)!;
        EmailTemplateConfig emailTemplateConfigItem = Array.Find(_configService.EmailTemplateConfigs, config => config.Id == userSearchConfigItem.EmailTemplateId)!;

        LoggingHelper.LogInformation(_configService.AppInsightsEnabled ? _telemetryClient : logger, "Resolved search config to '{0} [{1}]'.", queueItem.CorrelationId, executionContext, userSearchConfigItem.ConfigName, userSearchConfigItem.Id);

        LoggingHelper.LogInformation(_configService.AppInsightsEnabled ? _telemetryClient : logger, "Sending email with template '{0} [{1}]' to '{2}'.", queueItem.CorrelationId, executionContext, emailTemplateConfigItem.TemplateName, emailTemplateConfigItem.Id, queueItem.User.UserPrincipalName);

        if (userSearchConfigItem.IsEmailIntervalsEnabled == true && userSearchConfigItem.EmailIntervalDays!.Find(interval => interval.Value == (int)Math.Round(queueItem.PasswordExpiresIn.TotalDays, 0)) is null)
        {
            LoggingHelper.LogInformation(_configService.AppInsightsEnabled ? _telemetryClient : logger, "{0} is not expected to receive an email yet. Skipping...", queueItem.CorrelationId, executionContext, queueItem.User.UserPrincipalName);
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

        UserEmailRedirectConfig? redirectConfig = Array.Find(_configService.UserRedirectConfigs, config => config.UserPrincipalName == queueItem.User.UserPrincipalName);
        string toRecipient = redirectConfig is null ? queueItem.User.UserPrincipalName : redirectConfig.RedirectUserPrincipalName!;

        // Create the email message.
        MailMessage emailMessage = new(
            message: new(
                subject: $"Alert: Password Expiration Notice ({Math.Round(queueItem.PasswordExpiresIn.TotalDays, 0)} days)",
                body: new(emailBody, "HTML"),
                toRecipient: new Recipient[]
                {
                    new(toRecipient, null)
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
            LoggingHelper.LogInformation(_configService.AppInsightsEnabled ? _telemetryClient : logger, "Email sending is disabled for this search config. Skipping...", queueItem.CorrelationId, executionContext);
        }
    }
}