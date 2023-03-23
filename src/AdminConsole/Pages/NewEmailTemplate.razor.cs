using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Services;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Pages;

public partial class NewEmailTemplate : ComponentBase
{
    [Inject]
    protected ICosmosDbClientService _cosmosDbClientService { get; set; } = null!;

    [Inject]
    protected ILogger<EmailTemplates> _logger { get; set; } = null!;

    [Inject]
    protected AuthenticationStateProvider _authenticationStateProvider { get; set; } = null!;

    private EmailTemplateConfig? _emailTemplateConfig;

    protected override async Task OnInitializedAsync()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        if (authState.User.Identity is not null && authState.User.Identity.IsAuthenticated)
        {
            _emailTemplateConfig = new()
            {
                Id = Guid.NewGuid().ToString(),
                PartitionKey = "email-template-config"
            };
        }
        else
        {
            _logger.LogInformation("User is not authenticated.");
        }
    }
}