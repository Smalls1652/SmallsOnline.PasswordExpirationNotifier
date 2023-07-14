using Microsoft.AspNetCore.Components;
using SmallsOnline.PasswordExpirationNotifier.AdminConsole.Shared.Users;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;
using SmallsOnline.PasswordExpirationNotifier.Lib.Services;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Pages;

/// <summary>
/// Page for viewing a user's password expiration details.
/// </summary>
public partial class ViewUser : ComponentBase
{
    /// <summary>
    /// Dependency injected service for interacting with Cosmos DB.
    /// </summary>
    [Inject]
    protected ICosmosDbClientService _cosmosDbClientService { get; set; } = null!;

    /// <summary>
    /// Dependency injected service for interacting with the Microsoft Graph API.
    /// </summary>
    [Inject]
    protected IGraphClientService _graphClientService { get; set; } = null!;

    /// <summary>
    /// Logger for the page.
    /// </summary>
    [Inject]
    protected ILogger<ViewUser> _logger { get; set; } = null!;

    private bool _isLoaded;
    private User? _userData;
    private UserSearchConfig[]? _searchConfigs;
    private UserSearchConfig? _selectedSearchConfig;
    private ViewUserFormData? _formData;
    private bool _userDataLoaded;

    private bool _userDataLoadingFailed;
    private string? _userDataLoadingFailedMessage;

    protected override async Task OnInitializedAsync()
    {
        _isLoaded = false;

        _formData = new();
        _searchConfigs = await _cosmosDbClientService.GetUserSearchConfigsAsync();

        _isLoaded = true;
    }

    /// <summary>
    /// Method for handling the form submit event.
    /// </summary>
    /// <exception cref="NullReferenceException"></exception>
    /// <exception cref="Exception"></exception>
    private async Task HandleFormSubmitAsync()
    {
        try
        {
            _userDataLoadingFailed = false;
            _userDataLoaded = false;

            _userData = null;
            _selectedSearchConfig = null;

            if (_formData!.UserPrincipalName is null)
            {
                throw new NullReferenceException("User principal name is null.");
            }

            _userData = await _graphClientService.GetUserAsync(_formData.UserPrincipalName);

            if (_userData is null)
            {
                throw new Exception($"Could not find user: ${_formData.UserPrincipalName}");
            }

            if (_formData.SearchConfigId is null)
            {
                throw new NullReferenceException("No search config was selected.");
            }

            _selectedSearchConfig = Array.Find(_searchConfigs!, searchConfig => _formData.SearchConfigId == searchConfig.Id);

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