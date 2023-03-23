using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Services;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Pages;

public partial class EditUserSearchConfig : ComponentBase
{
    [Inject]
    protected ICosmosDbClientService _cosmosDbClientService { get; set; } = null!;

    [Inject]
    protected ILogger<EditUserSearchConfig> _logger { get; set; } = null!;

    [Inject]
    protected AuthenticationStateProvider _authenticationStateProvider { get; set; } = null!;

    [Parameter]
    public string Id { get; set; } = null!;

    private UserSearchConfig? _userSearchConfig;

    protected override async Task OnInitializedAsync()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        if (authState.User.Identity is not null && authState.User.Identity.IsAuthenticated)
        {
            _logger.LogInformation("Getting user search config...");
            _userSearchConfig = await _cosmosDbClientService.GetUserSearchConfigAsync(Id);
        }
        else
        {
            _logger.LogInformation("User is not authenticated.");
        }
    }
}