using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Services;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Pages;

public partial class UserSearchConfigs : ComponentBase
{
    [Inject]
    protected ICosmosDbClientService _cosmosDbClientService { get; set; } = null!;

    [Inject]
    protected ILogger<UserSearchConfigs> _logger { get; set; } = null!;

    [Inject]
    protected AuthenticationStateProvider _authenticationStateProvider { get; set; } = null!;

    private UserSearchConfig[]? _userSearchConfigs;

    private bool _loading = false;

    protected override async Task OnInitializedAsync()
    {
        _loading = true;
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        if (authState.User.Identity is not null && authState.User.Identity.IsAuthenticated)
        {
            _logger.LogInformation("Getting user search configs...");
            _userSearchConfigs = await _cosmosDbClientService.GetUserSearchConfigsAsync();
        }
        else
        {
            _logger.LogInformation("User is not authenticated.");
        }
        _loading = false;
    }

    private async Task HandleRemoveUserSearchConfigAsync(string id)
    {
        _logger.LogWarning("Removing user search config with id: {id}", id);
        await _cosmosDbClientService.RemoveUserSearchConfigAsync(id);
        _userSearchConfigs = Array.FindAll(_userSearchConfigs!, item => item.Id != id);
    }
}