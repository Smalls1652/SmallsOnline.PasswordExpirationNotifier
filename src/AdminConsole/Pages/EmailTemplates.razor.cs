using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Services;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Pages;

public partial class EmailTemplates : ComponentBase
{
    [Inject]
    protected ICosmosDbClientService _cosmosDbClientService { get; set; } = null!;

    [Inject]
    protected ILogger<EmailTemplates> _logger { get; set; } = null!;

    [Inject]
    protected AuthenticationStateProvider _authenticationStateProvider { get; set; } = null!;

    private EmailTemplateConfig[]? _emailTemplateConfigs;

    private bool _loading = false;

    protected override async Task OnInitializedAsync()
    {
        _loading = true;
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        if (authState.User.Identity is not null && authState.User.Identity.IsAuthenticated)
        {
            _logger.LogInformation("Getting email template configs...");
            _emailTemplateConfigs = await _cosmosDbClientService.GetEmailTemplateConfigsAsync();
        }
        else
        {
            _logger.LogInformation("User is not authenticated.");
        }
        _loading = false;
    }

    private async Task HandleRemoveEmailTemplateAsync(string id)
    {
        await _cosmosDbClientService.RemoveEmailTemplateConfigAsync(id);
        _emailTemplateConfigs = Array.FindAll(_emailTemplateConfigs!, item => item.Id != id);
    }
}