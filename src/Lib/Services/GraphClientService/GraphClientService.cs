using System.Text.RegularExpressions;
using Microsoft.Identity.Client;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

/// <summary>
/// Service for interacting with the Microsoft Graph API.
/// </summary>
public partial class GraphClientService : IGraphClientService
{
    private readonly IEnumerable<string> _apiScopes;
    private readonly HttpClient _graphClient;
    private readonly IConfidentialClientApplication _confidentialClientApplication;
    private readonly JsonSourceGenerationContext _jsonSourceGenerationContext = new();
    private readonly string[] _graphUserProps = new[]
    {
        "id",
        "accountEnabled",
        "userPrincipalName",
        "displayName",
        "givenName",
        "surname",
        "lastPasswordChangeDateTime",
        "onPremisesDistinguishedName"
    };

    public GraphClientService(GraphClientConfig config)
    {
        _apiScopes = config.ApiScopes;

        _confidentialClientApplication = ConfidentialClientApplicationBuilder
            .Create(config.ClientId)
            .WithTenantId(config.TenantId)
            .WithClientSecret(config.Credential.ClientSecret!)
            .Build();

        _graphClient = new()
        {
            BaseAddress = new Uri("https://graph.microsoft.com/beta/")
        };
        _graphClient.DefaultRequestHeaders.Add("ConsistencyLevel", "eventual");
    }

    /// <inheritdoc />
    public HttpClient GraphClient => _graphClient;

    private bool _isConnected => _authToken is not null;
    private AuthenticationResult? _authToken;

    [GeneratedRegex("^https:\\/\\/graph.microsoft.com\\/(?'version'v1\\.0|beta)\\/(?'endpoint'.+?)$")]
    private partial Regex _nextLinkRegex();
}