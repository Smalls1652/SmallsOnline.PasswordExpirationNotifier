﻿using System.Text.Json;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class GraphClientService
{
    /// <summary>
    /// Get user information from the Microsoft Graph API.
    /// </summary>
    /// <param name="userId">The unique ID of the user.</param>
    /// <returns>The user's data.</returns>
    /// <exception cref="NullReferenceException">No user was found.</exception>
    public async Task<User> GetUserAsync(string userId)
    {
        string apiEndpoint = $"users/{userId}?$select={string.Join(",", _graphUserProps)}";

        string? apiResultString = await _graphClient.SendApiCallAsync(
            endpoint: apiEndpoint,
            apiPostBody: null,
            httpMethod: HttpMethod.Get
        );

        if (apiResultString is null)
        {
            throw new NullReferenceException($"No user was returned from the API for user ID '{userId}'.");
        }

        User user = JsonSerializer.Deserialize(
            json: apiResultString,
            jsonTypeInfo: _jsonSourceGenerationContext.User
        )!;

        return user;
    }
}