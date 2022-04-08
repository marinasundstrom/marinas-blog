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

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddMarkdownServices();
builder.Services.AddScoped<Highlight>();

builder.Services.AddScoped<DisqusService>();

builder.Services.AddSingleton<DisqusConfig>(sp => new DisqusConfig() {
                Site = "sundstrom-dev"
            });

builder.Services.AddGoogleAnalytics("G-8WNKYRD04R");

CultureInfo.CurrentCulture = new CultureInfo("en-US");
CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture;

await builder.Build().RunAsync();
