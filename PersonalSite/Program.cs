using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PersonalSite;
using PersonalSite.Markdown;
using PersonalSite.Disqus;
using Blazor.Analytics;
using System.Globalization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

ConfigureServices(builder.Services, builder.HostEnvironment.BaseAddress);

await builder.Build().RunAsync();

static void ConfigureServices(IServiceCollection services, string baseAddress)
{
    services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(baseAddress) });

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