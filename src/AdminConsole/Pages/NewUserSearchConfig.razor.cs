using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Services;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Pages;

public partial class NewUserSearchConfig : ComponentBase
{
    [Inject]
    protected ILogger<EmailTemplates> _logger { get; set; } = null!;

    [Inject]
    protected AuthenticationStateProvider _authenticationStateProvider { get; set; } = null!;

    private UserSearchConfig? _userSearchConfig;

    protected override async Task OnInitializedAsync()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        if (authState.User.Identity is not null && authState.User.Identity.IsAuthenticated)
        {
            _userSearchConfig = new()
            {
                Id = Guid.NewGuid().ToString(),
                PartitionKey = "user-search-config"
            };
        }
        else
        {
            _logger.LogInformation("User is not authenticated.");
        }
    }
}