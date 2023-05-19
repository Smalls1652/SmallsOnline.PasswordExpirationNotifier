using System.Text.Json;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class GraphClientService
{
    /// <inheritdoc />
    public async Task<User> GetUserAsync(string userId)
    {
        string apiEndpoint = $"users/{userId}?$select={string.Join(",", _graphUserProps)}";

        string? apiResultString = await _graphClient.SendApiCallAsync(
            endpoint: apiEndpoint,
            apiPostBody: null,
            httpMethod: HttpMethod.Get
        );

        User user;
        try
        {
            user = JsonSerializer.Deserialize(
                json: apiResultString,
                jsonTypeInfo: _jsonSourceGenerationContext.User
            )!;

            if (string.IsNullOrEmpty(user.Id))
            {
                throw new Exception("User ID is null or empty.");
            }
        }
        catch
        {
            GraphErrorResponse? errorResponse = JsonSerializer.Deserialize(
                json: apiResultString,
                jsonTypeInfo: _jsonSourceGenerationContext.GraphErrorResponse
            );

            throw new Exception(errorResponse!.Error!.Message);
        }

        return user;
    }
}