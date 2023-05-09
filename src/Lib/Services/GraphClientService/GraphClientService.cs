using System.Text.RegularExpressions;
using SmallsOnline.MsGraphClient.Models;

namespace SmallsOnline.PasswordExpirationNotifier.Lib.Services;

/// <summary>
/// Service for interacting with the Microsoft Graph API.
/// </summary>
public partial class GraphClientService : IGraphClientService
{
    private readonly GraphClient _graphClient;
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

    public GraphClientService(string clientId, string tenantId, string clientSecret)
    {
        _graphClient = new(
            baseUri: new("https://graph.microsoft.com/beta/"),
            clientId: clientId,
            tenantId: tenantId,
            credentialType: GraphClientCredentialType.Secret,
            clientSecret: clientSecret,
            apiScopes: new ApiScopesConfig(new[] { "https://graph.microsoft.com/.default" })
        );

        _graphClient.ConnectClient();
    }

    /// <inheritdoc />
    public GraphClient GraphClient => _graphClient;

    [GeneratedRegex("^https:\\/\\/graph.microsoft.com\\/(?'version'v1\\.0|beta)\\/(?'endpoint'.+?)$")]
    private partial Regex _nextLinkRegex();
}