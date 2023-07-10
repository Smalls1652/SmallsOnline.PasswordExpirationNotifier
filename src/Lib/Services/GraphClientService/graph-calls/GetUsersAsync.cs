using System.Text.Json;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class GraphClientService
{
    /// <inheritdoc />
    public async Task<User[]?> GetUsersAsync(string domainName, string? ouPath, string? lastNameStartsWith)
    {
        User[]? users = null;

        // Build the query filter based on if lastNameStartsWith is null or not.
        string queryFilter = (lastNameStartsWith is null) switch
        {
            true => $"(endsWith(userPrincipalName, '@{domainName}')) and (accountEnabled eq true)",
            _ => $"(endsWith(userPrincipalName, '@{domainName}')) and (accountEnabled eq true) and (startsWith(surname, '{lastNameStartsWith}'))"
        };

        // Start the loop to get all users.
        string? apiEndpoint = null;
        bool hasNextLink = true;
        int currentIndex = 0;
        while (hasNextLink)
        {
            // If apiEndpoint is null, then this is the first iteration of the loop.
            // Set the apiEndpoint to get the first page of users.
            apiEndpoint ??= $"users?$select={string.Join(",", _graphUserProps)}&$filter={queryFilter}&$count=true";

            // Call the API to get the users.
            string? apiResultString = await SendApiCallAsync(
                endpoint: apiEndpoint!,
                httpMethod: HttpMethod.Get
            );

            // If the API returned null, then throw an exception.
            if (apiResultString is null)
            {
                throw new NullReferenceException("No users were returned from the API.");
            }

            // Deserialize the JSON into a UserCollection object.
            UserCollection userCollection = JsonSerializer.Deserialize(
                json: apiResultString,
                jsonTypeInfo: _jsonSourceGenerationContext.UserCollection
            )!;

            // If users is null, then this is the first iteration of the loop.
            // Set the size of the users array to the number of users returned from the API.
            if (users is null)
            {
                users = new User[userCollection.OdataCount];
            }

            // Add the users from the current page to the users array.
            for (int i = 0; i < userCollection.Value!.Length; i++)
            {
                users[currentIndex] = userCollection.Value[i];
                currentIndex++;
            }

            if (userCollection.ODataNextLink is null)
            {
                // If the ODataNextLink property is null, then there are no more pages of users.
                hasNextLink = false;
            }
            else
            {
                // Otherwise, set the apiEndpoint to the next page of users.
                apiEndpoint = _nextLinkRegex().Match(userCollection.ODataNextLink!).Groups["endpoint"].Value;
            }
        }

        if (users is not null && ouPath is not null)
        {
            // If ouPath is not null, then filter the users by the OU path.
            users = users.Where(item => item.OnPremisesDistinguishedName is not null && item.OnPremisesDistinguishedName.EndsWith(ouPath)).ToArray();
        }

        return users;
    }
}