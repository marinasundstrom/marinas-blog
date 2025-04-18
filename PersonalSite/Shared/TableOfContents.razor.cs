using System.Runtime.InteropServices.JavaScript;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;

namespace PersonalSite.Shared;

public partial class TableOfContents
{
    private IJSObjectReference? module;

    [Inject] public IJSRuntime JS { get; set; }

    [Inject] IWebAssemblyHostEnvironment HostEnv { get; set; }

    protected override async Task OnInitializedAsync()
    {
        if (HostEnv.Environment == "Prerendering")
            return;

        module = await JS.InvokeAsync<IJSObjectReference>("import",
            "../Shared/TableOfContents.razor.js");

        await module.InvokeVoidAsync("init");
    }
}