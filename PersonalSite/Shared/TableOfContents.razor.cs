using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;

namespace PersonalSite.Shared;

public partial class TableOfContents : IAsyncDisposable
{
    private IJSObjectReference? module;

    [Parameter]
    public string ElementId { get; set; } = "TableOfContents";

    [Inject] public IJSRuntime JS { get; set; }

    [Inject] IWebAssemblyHostEnvironment HostEnv { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (HostEnv.Environment == "Prerendering")
        {
            return;
        }

        module ??= await JS.InvokeAsync<IJSObjectReference>("import",
            "../Shared/TableOfContents.razor.js");

        await module.InvokeVoidAsync("init", ElementId);
    }

    public async ValueTask DisposeAsync()
    {
        if (module is null)
        {
            return;
        }

        try
        {
            await module.InvokeVoidAsync("dispose", ElementId);
        }
        catch (JSDisconnectedException)
        {
            // It's safe to ignore if the JS runtime is no longer available (e.g. during navigation).
        }

        await module.DisposeAsync();
        module = null;
    }
}