using System.Text.Json;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class GraphClientService
{
    /// <inheritdoc />
    public async Task SendEmailAsync(Message message, string sendAsUser)
    {
        // Serialize the message to JSON.
        string messageJson = JsonSerializer.Serialize(
            value: message,
            jsonTypeInfo: JsonSourceGenerationContext.Default.Message
        );

        try
        {
            // Create a draft message.
            string? draftResponse = await SendApiCallAsync(
                endpoint: $"users/{sendAsUser}/messages",
                httpMethod: HttpMethod.Post,
                body: messageJson
            );

            Message draftItem = JsonSerializer.Deserialize(
                json: draftResponse!,
                jsonTypeInfo: JsonSourceGenerationContext.Default.Message
            )!;

            // Send the draft message.
            await SendApiCallAsync(
                endpoint: $"users/{sendAsUser}/messages/{draftItem.Id!}/send",
                httpMethod: HttpMethod.Post
            );

            // Delete the draft message.
            await SendApiCallAsync(
                endpoint: $"users/{sendAsUser}/messages/{draftItem.Id!}",
                httpMethod: HttpMethod.Delete
            );

        }
        catch (Exception ex)
        {
            throw new Exception($"Error sending email: {ex.Message}", ex);
        }
    }
}