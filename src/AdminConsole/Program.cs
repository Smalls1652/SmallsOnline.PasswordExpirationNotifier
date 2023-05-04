using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using SmallsOnline.PasswordExpirationNotifier.Lib.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services
    .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdConfig"));

builder.Services
    .AddControllersWithViews()
    .AddMicrosoftIdentityUI();

builder.Services.AddAuthorization();

builder.Services.AddRazorPages();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

builder.Services
    .AddServerSideBlazor()
    .AddMicrosoftIdentityConsentHandler();

builder.Services
    .AddSingleton<ICosmosDbClientService, CosmosDbClientService>(
        provider => new CosmosDbClientService(
            connectionString: builder.Configuration.GetSection("cosmosDbConnectionString").Value!,
            databaseName: builder.Configuration.GetSection("cosmosDbDatabaseName").Value!
        )
    );

builder.Services
    .AddSingleton<IGraphClientService, GraphClientService>(
        provider => new(
            clientId: builder.Configuration.GetSection("backendClientID").Value!,
            clientSecret: builder.Configuration.GetSection("backendClientSecret").Value!,
            tenantId: builder.Configuration.GetSection("backendTenantId").Value!
        )
    );

builder.Services
    .AddSingleton<IQueueClientService, QueueClientService>(
        provider => new(
            connectionString: builder.Configuration.GetSection("storageConnectionString").Value!
        )
    );

var app = builder.Build();

app.Use((context, next) =>
{
    context.Request.Scheme = "https";
    return next(context);
});

app.UseForwardedHeaders();

if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();