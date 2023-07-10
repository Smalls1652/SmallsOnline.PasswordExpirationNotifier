using Microsoft.Identity.Client;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class GraphClientService
{
    public async Task<AuthenticationResult> GetAuthTokenAsync()
    {
        AuthenticationResult? authToken = await _confidentialClientApplication
            .AcquireTokenForClient(_apiScopes)
            .ExecuteAsync();

        return authToken;
    }
}