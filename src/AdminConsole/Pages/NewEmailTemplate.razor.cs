using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Services;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Pages;

/// <summary>
/// Page for creating new email templates.
/// </summary>
public partial class NewEmailTemplate : ComponentBase
{
    /// <summary>
    /// Logger for the page.
    /// </summary>
    [Inject]
    protected ILogger<NewEmailTemplate> _logger { get; set; } = null!;

    /// <summary>
    /// The authentication state provider.
    /// </summary>
    [Inject]
    protected AuthenticationStateProvider _authenticationStateProvider { get; set; } = null!;

    private EmailTemplateConfig? _emailTemplateConfig;

    protected override async Task OnInitializedAsync()
    {
        // Get the authentication state.
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        if (authState.User.Identity is not null && authState.User.Identity.IsAuthenticated)
        {
            // If the user is authenticated, create a new email template.
            _emailTemplateConfig = new()
            {
                Id = Guid.NewGuid().ToString(),
                PartitionKey = "email-template-config"
            };
        }
        else
        {
            // If the user is not authenticated, log it.
            _logger.LogInformation("User is not authenticated.");
        }
    }
}