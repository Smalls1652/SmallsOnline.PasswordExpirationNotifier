using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using SmallsOnline.PasswordExpirationNotifier.AdminConsole.Shared.Users;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;
using SmallsOnline.PasswordExpirationNotifier.Lib.Services;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Pages;

public partial class ViewUser : ComponentBase
{
    [Inject]
    protected ICosmosDbClientService _cosmosDbClientService { get; set; } = null!;

    [Inject]
    protected IGraphClientService _graphClientService { get; set; } = null!;

    [Inject]
    protected ILogger<ViewUser> _logger { get; set; } = null!;

    private bool _isLoaded;
    private User? _userData;
    private UserSearchConfig[]? _searchConfigs;
    private UserSearchConfig? _selectedSearchConfig;
    private SelectedViewUserSettings? _selectedViewUserSettings;
    private bool _userDataLoaded;

    private bool _userDataLoadingFailed;
    private string? _userDataLoadingFailedMessage;

    protected override async Task OnInitializedAsync()
    {
        _isLoaded = false;

        _selectedViewUserSettings = new();
        _searchConfigs = await _cosmosDbClientService.GetUserSearchConfigsAsync();

        _isLoaded = true;
    }

    private async Task HandleFormSubmitAsync()
    {
        try
        {
            _userDataLoadingFailed = false;
            _userDataLoaded = false;

            _userData = null;
            _selectedSearchConfig = null;

            if (_selectedViewUserSettings!.UserPrincipalName is null)
            {
                throw new NullReferenceException("User principal name is null.");
            }

            _userData = await _graphClientService.GetUserAsync(_selectedViewUserSettings.UserPrincipalName);

            if (_userData is null)
            {
                throw new Exception($"Could not find user: ${_selectedViewUserSettings.UserPrincipalName}");
            }

            if (_selectedViewUserSettings.SearchConfigId is null)
            {
                throw new NullReferenceException("No search config was selected.");
            }

            _selectedSearchConfig = Array.Find(_searchConfigs!, searchConfig => _selectedViewUserSettings.SearchConfigId == searchConfig.Id);

            _userDataLoaded = true;
        }
        catch (Exception ex)
        {
            _userDataLoadingFailed = true;
            _userDataLoadingFailedMessage = ex.Message;
            _logger.LogError(ex, "Failed to load user data.");
        }
    }
}