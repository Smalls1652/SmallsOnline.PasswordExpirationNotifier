using Microsoft.AspNetCore.Components;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Graph;
using SmallsOnline.PasswordExpirationNotifier.Lib.Services;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Shared.UserSearchConfigs;

/// <summary>
/// Component for editing a user search config.
/// </summary>
public partial class UserSearchConfigForm : ComponentBase
{
    /// <summary>
    /// Dependency injection for the Cosmos DB client service.
    /// </summary>
    [Inject]
    protected ICosmosDbClientService _cosmosDbClientService { get; set; } = null!;

    [Inject]
    protected IGraphClientService _graphClientService { get; set; } = null!;

    /// <summary>
    /// Logger for the component.
    /// </summary>
    [Inject]
    protected ILogger<UserSearchConfigForm> _logger { get; set; } = null!;

    /// <summary>
    /// Navigation manager for the component to handle navigation.
    /// </summary>
    [Inject]
    protected NavigationManager _navigationManager { get; set; } = null!;

    /// <summary>
    /// The user search config item to edit.
    /// </summary>
    [Parameter]
    public UserSearchConfig UserSearchConfig { get; set; } = null!;

    private List<int>? _emailIntervalDays;

    private EmailTemplateConfig[]? _emailTemplateConfigs;

    private User[]? _users;
    private bool _loadingUsers = false;

    protected override async Task OnInitializedAsync()
    {
        if (UserSearchConfig.EmailIntervalDays is null)
        {
            // If the user search config has no email interval days, set the email interval days to
            // a new empty list.
            _emailIntervalDays = new();
        }

        // Get the email template configs from the Cosmos DB.
        _emailTemplateConfigs = await _cosmosDbClientService.GetEmailTemplateConfigsAsync();
    }

    /// <summary>
    /// Method for handling the form submission.
    /// </summary>
    private async Task HandleFormSubmitAsync()
    {
        _logger.LogInformation("Updating template...");

        // Set the last modified date to now.
        UserSearchConfig.ConfigLastModified = DateTimeOffset.Now;
        //UserSearchConfig.EmailIntervalDays = _emailIntervalDays;

        // Update the user search config in the Cosmos DB.
        await _cosmosDbClientService.CreateOrUpdateUserSearchConfigAsync(UserSearchConfig);

        // Navigate back to the user search configs page.
        _navigationManager.NavigateTo("/user-search-configs");
    }

    /// <summary>
    /// Method for handling when a new email interval day is added.
    /// </summary>
    private void HandleAddEmailIntervalDay()
    {
        if (UserSearchConfig.EmailIntervalDays is null)
        {
            // If the user search config has no email interval days, initialize the list.
            UserSearchConfig.EmailIntervalDays = new();
        }

        // Add a new email interval day to the list.
        UserSearchConfig.EmailIntervalDays.Add(new());
    }

    /// <summary>
    /// Method for handling when an email interval day is removed.
    /// </summary>
    /// <param name="emailIntervalDay">The interval day to remove.</param>
    private void HandleRemoveEmailIntervalDay(EmailIntervalDay emailIntervalDay)
    {
        UserSearchConfig.EmailIntervalDays!.Remove(emailIntervalDay);
    }

    private async Task HandleGetUsersAsync()
    {
        _loadingUsers = true;

        _users = await _graphClientService.GetUsersAsync(
            domainName: UserSearchConfig.DomainName!,
            ouPath: UserSearchConfig.OUPath,
            lastNameStartsWith: null
        );
        
        _loadingUsers = false;
    }
}