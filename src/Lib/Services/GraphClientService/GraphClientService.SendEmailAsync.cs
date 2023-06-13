﻿using System.Text.Json;
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
            jsonTypeInfo: _jsonSourceGenerationContext.Message
        );

        try
        {
            // Create a draft message.
            string? draftResponse = await _graphClient.SendApiCallAsync(
                endpoint: $"users/{sendAsUser}/messages",
                apiPostBody: messageJson,
                httpMethod: HttpMethod.Post
            );

            Message draftItem = JsonSerializer.Deserialize(
                json: draftResponse!,
                jsonTypeInfo: _jsonSourceGenerationContext.Message
            )!;

            // Send the draft message.
            await _graphClient.SendApiCallAsync(
                endpoint: $"users/{sendAsUser}/messages/{draftItem.Id!}/send",
                apiPostBody: null,
                httpMethod: HttpMethod.Post
            );

            // Delete the draft message.
            await _graphClient.SendApiCallAsync(
                endpoint: $"users/{sendAsUser}/messages/{draftItem.Id!}",
                apiPostBody: null,
                httpMethod: HttpMethod.Delete
            );

        }
        catch (Exception ex)
        {
            throw new Exception($"Error sending email: {ex.Message}", ex);
        }
    }
}