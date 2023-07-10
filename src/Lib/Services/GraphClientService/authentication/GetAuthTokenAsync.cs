using Microsoft.Identity.Client;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class GraphClientService
{
    private async Task<AuthenticationResult> GetAuthTokenAsync()
    {
        AuthenticationResult? authToken = await _confidentialClientApplication
            .AcquireTokenForClient(_apiScopes)
            .ExecuteAsync();

        return authToken;
    }
}