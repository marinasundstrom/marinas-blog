using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PersonalSite;
using PersonalSite.Markdown;
using PersonalSite.Disqus;
using PersonalSite.Theming;
using Blazor.Analytics;
using System.Globalization;
using Blazored.LocalStorage;
using Microsoft.JSInterop;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

ConfigureServices(builder.Services, builder.HostEnvironment.BaseAddress);

var app = builder.Build();

var themeManager = app.Services.GetRequiredService<IThemeManager>();
themeManager.ColorSchemeChanged += async (s, e) => {
    using var scope = app.Services.CreateScope();
    var jsRuntime = scope.ServiceProvider.GetRequiredService<IJSRuntime>();
    await jsRuntime.InvokeVoidAsync("setColorScheme", e.ColorScheme == ColorScheme.Dark ? "dark" : "light");
};
themeManager.Initialize();

await app.RunAsync();

static void ConfigureServices(IServiceCollection services, string baseAddress)
{
    services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });

    services.AddBlazoredLocalStorage();

    services.AddThemeServices();

    services.AddMarkdownServices();
    services.AddScoped<Highlight>();

    services.AddScoped<DisqusService>();

    services.AddSingleton<DisqusConfig>(sp => new DisqusConfig() {
                    Site = "sundstrom-dev"
                });

    services.AddGoogleAnalytics("G-8WNKYRD04R");

    CultureInfo.CurrentCulture = new CultureInfo("en-US");
    CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture;
}