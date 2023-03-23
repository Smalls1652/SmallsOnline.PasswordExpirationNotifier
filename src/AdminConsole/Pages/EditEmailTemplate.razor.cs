﻿using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using SmallsOnline.PasswordExpirationNotifier.Lib.Models.Config;
using SmallsOnline.PasswordExpirationNotifier.Lib.Services;

namespace SmallsOnline.PasswordExpirationNotifier.AdminConsole.Pages;

/// <summary>
/// Page for editing email templates.
/// </summary>
public partial class EditEmailTemplate : ComponentBase
{
    /// <summary>
    /// Dependency injected service for interacting with Cosmos DB.
    /// </summary>
    [Inject]
    protected ICosmosDbClientService _cosmosDbClientService { get; set; } = null!;

    /// <summary>
    /// Logger for the page.
    /// </summary>
    [Inject]
    protected ILogger<EmailTemplates> _logger { get; set; } = null!;

    /// <summary>
    /// The authentication state provider.
    /// </summary>
    [Inject]
    protected AuthenticationStateProvider _authenticationStateProvider { get; set; } = null!;

    /// <summary>
    /// The ID of the email template to edit.
    /// </summary>
    [Parameter]
    public string Id { get; set; } = null!;

    private EmailTemplateConfig? _emailTemplateConfig;

    protected override async Task OnInitializedAsync()
    {
        // Get the authentication state.
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        if (authState.User.Identity is not null && authState.User.Identity.IsAuthenticated)
        {
            // If the user is authenticated, get the email templates.
            _logger.LogInformation("Getting email template configs...");
            _emailTemplateConfig = await _cosmosDbClientService.GetEmailTemplateConfigAsync(Id);
        }
        else
        {
            // If the user is not authenticated, log it.
            _logger.LogInformation("User is not authenticated.");
        }
    }
}