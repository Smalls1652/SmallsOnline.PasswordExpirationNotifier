namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class GraphClientService
{
    private async Task ConnectAsync()
    {
        bool needsToConnect = !_isConnected;

        if (_authToken is not null)
        {
            if (DateTimeOffset.Now >= _authToken.ExpiresOn)
            {
                needsToConnect = true;
            }
        }

        if (needsToConnect)
        {
            _authToken = await GetAuthTokenAsync();
        }
    }
}