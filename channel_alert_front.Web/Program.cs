using Microsoft.FluentUI.AspNetCore.Components;

using ApexCharts;

using channel_alert_front.Web;
using channel_alert_front.Web.Components;
using channel_alert_front.Web.States;
using Microsoft.AspNetCore.DataProtection;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add service defaults & Aspire components.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddCircuitOptions((options) =>
    {
        options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(5);
        options.JSInteropDefaultCallTimeout = TimeSpan.FromMinutes(30);
        options.DetailedErrors = true;
    })
    .AddHubOptions((options) =>
    {
        options.MaximumReceiveMessageSize = 10 * 1024 * 1024;
    });

builder.Services.AddSignalR();

builder.Services.AddOutputCache();

builder.Services
    .AddScoped<CustomCircuitHandler>()
    .AddSingleton<ThemeState>()
    .AddSingleton<HomeState>();

builder.Services.AddHttpClient();
builder.Services.AddFluentUIComponents();
builder.Services.AddApexCharts();
builder.Services.AddRadzenComponents();

builder.Services.AddHttpClient<WeatherApiClient>(client =>
    {
        // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
        // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
        client.BaseAddress = new("https+http://apiservice");
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

//app.UseOutputCache();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapDefaultEndpoints();

app.Run();
