using System.Text.Json;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class GraphClientService
{
    /// <inheritdoc />
    public async Task SendEmailAsync(MailMessage message, string sendAsUser)
    {
        // Serialize the message to JSON.
        string messageJson = JsonSerializer.Serialize(
            value: message,
            jsonTypeInfo: _jsonSourceGenerationContext.MailMessage
        );

        try
        {
            // Send the email.
            await _graphClient.SendApiCallAsync(
                endpoint: $"users/{sendAsUser}/sendMail",
                apiPostBody: messageJson,
                httpMethod: HttpMethod.Post
            );
        }
        catch (Exception ex)
        {
            throw new Exception($"Error sending email: {ex.Message}", ex);
        }
    }
}