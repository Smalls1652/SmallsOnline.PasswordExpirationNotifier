using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

public partial class GraphClientService
{
    private async Task<string?> SendApiCallAsync(string endpoint, HttpMethod httpMethod) => await SendApiCallAsync(endpoint, httpMethod, null);
    private async Task<string?> SendApiCallAsync(string endpoint, HttpMethod httpMethod, string? body)
    {
        await ConnectAsync();

        string? content = null;
        bool isFinished = false;

        while (!isFinished)
        {
            using HttpRequestMessage request = new(
                method: httpMethod,
                requestUri: endpoint
            );

            request.Headers.Authorization = new AuthenticationHeaderValue(
                scheme: "Bearer",
                parameter: _authToken!.AccessToken
            );

            if (body is not null)
            {
                request.Content = new StringContent(
                    content: body,
                    encoding: Encoding.UTF8,
                    mediaType: "application/json"
                );
            }

            using HttpResponseMessage response = await _graphClient.SendAsync(request);

            switch (response.StatusCode)
            {
                case HttpStatusCode.TooManyRequests:
                    RetryConditionHeaderValue retryAfter = response.Headers.RetryAfter!;

                    TimeSpan retryBuffer = retryAfter.Delta!.Value.Add(TimeSpan.FromSeconds(15));

                    await Task.Delay(retryBuffer);
                    break;
                
                default:
                    content = await response.Content.ReadAsStringAsync();
                    isFinished = true;
                    break;
            }
        }

        return content;
    }
}