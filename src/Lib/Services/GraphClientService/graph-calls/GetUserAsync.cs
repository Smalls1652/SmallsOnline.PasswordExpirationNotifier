using System.Text.Json;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class GraphClientService
{
    /// <inheritdoc />
    public async Task<User> GetUserAsync(string userId)
    {
        string apiEndpoint = $"users/{userId}?$select={string.Join(",", _graphUserProps)}";

        string? apiResultString = await SendApiCallAsync(
            endpoint: apiEndpoint,
            httpMethod: HttpMethod.Get
        );

        if (apiResultString is null)
        {
            throw new Exception("API result string is null.");
        }

        User user;
        try
        {
            user = JsonSerializer.Deserialize(
                json: apiResultString,
                jsonTypeInfo: GraphJsonContext.Default.User
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
                jsonTypeInfo: GraphJsonContext.Default.GraphErrorResponse
            );

            throw new Exception(errorResponse!.Error!.Message);
        }

        return user;
    }
}