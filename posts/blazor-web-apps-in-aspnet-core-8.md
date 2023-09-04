---
title: Blazor Web Apps in ASP.NET Core 8
published: 2023-09-02
tags: [Blazor, .NET, ASP.NET Core, Web development]
---

Some big exciting stuff is coming to Blazor on the server in ASP.NET Core 8.

## Background

In the beginning of time, Web development used to be about the web server receiving a request from a client browser, generating some HTML, and returning it with the response. The result was a static non-interactive page that the browser rendered on screen. 

Then JavaScript came along, and added interactivity on the client-side - the ability to make changes to the content of the page based on user input. And with JavaScript you got the ability to make requests to the server, to then dynamically update parts of that static page. The content of the page was not longer tied to just one request.

Following the advancements and increased computing power on client devices, we started making fully dynamic and interactive Single-page applications, so called SPAs - using JavaScript frameworks like Backbone, and later React and Angular. Blazor is a pretty recent addition.

But recently, developers have realized that SPAs are not suitable or even necessary for everything.

Sometimes you just want to generate and serve content very fast and efficiently - an example of this are big e-commerce sites with a lot of load. But at the same time you want to enable interactivity were it is needed.

A recent trend among modern JavaScript Web app frameworks (such as Astro) is to combine server-side rendering and client-side interactivity. Enabling you to choose whether a certain part of the page - a component - should be rendered on the server, or be activated as an interactive component on the client.

This is a major theme for Blazor in ASP.NET Core 8 - to enable these mixed server-side rendering and interactivity scenarios.

## Unified Blazor architecture - on the server

The big theme for Blazor in .NET 8 is the unification of Blazor on the server. A great deal of plumbing has been done to add _Server-side rendering (SSR)_, and to enable interactivity per component with so called _Render modes_. In essence, this is an improved Blazor Server hosting model, where SSR now is the default.

There are other significant enhancements, such as _enhanced navigation_, that will make your server-side rendered apps feel as smooth as single-page applications.

This doesn't affect other hosting models, such as standalone Blazor WebAssembly and MAUI Blazor Hybrid. Though they are of course getting all the enhancements to Blazor itself.

The features covered in this article are in the new "Blazor Web App" template.

## Server-side rendering (SSR)

ASP.NET Core 8 and Blazor Server-side rendering (SSR) fills the remaining gap between ASP.NET Core and Blazor - by making Blazor Server able to serve traditional non-interactive pages, like MVC Razor Pages does, but with Razor components. The twist is that you can make any component interactive if you want to. 

To clarify, Blazor is the name of the product in all of its variations - _Razor Components_ is the technology behind it. _Razor_ is the C#/HTML templating syntax, of which there are the MVC/Razor Pages and Blazor flavors.

Previously, Blazor Server had also been reliant on Razor Pages for bootstrapping, but that is no longer necessary since ASP.NET Core now can render Razor components as plain HTML directly at a certain endpoint.

So the way you should think of this new "Blazor Web App" an improvement on the _Blazor Server_ hosting model. The standalone WebAssembly type is still the same, but these improvements do open up for some interesting hybrid scenarios on the server as we will see with render modes.

### Routing

The Blazor router fully integrates and take advantage of ASP.NET Core endpoints in SSR mode.

Then you let ASP.NET Core register the components in the Dependency Injection container.

```csharp
builder.Services.AddRazorComponents()
    .AddWebAssemblyComponents()
    .AddServerComponents();
```

And then map them to endpoints - with ``App`` being the root component:

```csharp
app.MapRazorComponents<App>()
    .AddWebAssemblyRenderMode()
    .AddServerRenderMode();
```

Given that you have set the router up, you then add the ``@page`` as you normally do.

```csharp
@page "/my-page"
```

Unless the render modes of either the Router component or the page component has been set to Server or WebAssembly, this will be served as a static page. More on render modes below.

#### Using Minimal API

You can manually map a component to an endpoint, by creating your own Minimal API endpoint that returns a ``RazorComponentResult``.

```csharp
app.Map("/mything", () => new RazorComponentResult<MyPageComponent>());
```

## Render modes

A Blazor Web App is fundamentally rendered on the server. But as mentioned, you can turn on interactivity per-component in the SSR context. You can also tell a component where it should run: on the server, or on the client using WebAssembly. It all is a seamless experience. And pre-rendering just works out of box.

```csharp
@* For being rendered on the server *@
<Counter @rendermode="RenderMode.Server" />

@* For running in WebAssembly *@
<Counter @rendermode="RenderMode.WebAssembly" />
```

You can also specify the default render mode for a component using these attributes.

```csharp
@attribute [RenderModeServer]

@attribute [RenderModeWebAssembly]
```

The render mode is then inherited by sub-components. You can apply these attributes to the router, and that would make the entire site interactive. But by default, a project created from a template has no interactivity at root-level.

The WebAssembly render mode does require you to set up a separate project that will contain all the bits (components and services) that will be shipped to the browsers. So although integrated, it are still technically two apps.

### Auto render mode

The Auto render mode will prefer WebAssembly, but fall back on Server for interactivity when the WebAssembly files have not yet been downloaded. Once the WebAssembly files have downloaded, the next time you load those components, the component will be running in WebAssembly.

```csharp
<Counter @rendermode="RenderMode.Auto" />
```

## Enhanced navigation

Whenever you normally navigate on a new server-side rendered page, the entire DOM is replaced, and the window is redrawn. This sometimes causes flickers that affect the user experience even if the browser tries to mitigate it.

Blazor has got a trick:

Whenever the user navigates to a page inside the application, the navigation gets intercepted, and instead a HTTP request gets sent. Once a response has returned, Blazor figures out the difference between the HTML returned in the response and the one of the current page in the browser, and then applies only the changes to the browser. 

This makes your server-side rendered app feel like it is a SPA while it is not. There will be no flickering. You will see that the shell of the application and its menus get preserved.

## Stream rendering

When you first visit a page you may have to wait for the page to execute some logic, and to then return the fully rendered page - while all you are seeing is a blank screen. It can be quite frustrating even when it is just for a second or two.

Blazor has got yet another trick: 

Blazor can render and send you the shell before the page component has been fully initialized. Once that has completed, it just sends the rest of the rendered content, and applies it to the page in the browser.

Just add this attribute to your Page components.

```csharp
@attribute [StreamRendering(prerender: true)]
```

Stream rendering utilizes HTTP Streaming to send partial content in chunks over the same HTTP Request.

## Form binding in Blazor SSR

In Blazor SSR, you can bind a model to a form using the ``[SupplyParameterFromForm]`` attribute - and with some minor adjustments to the form because this page is bound to a HTTP request. You can even have multiple named forms on a page.

```csharp
@using System.ComponentModel.DataAnnotations

<EditForm method="post" FormName="contact" OnValidSubmit="AddContact">
    <DataAnnotationsValidator />

    <div>
        <label for="name">Name</label>
        <InputText id="name" @bind-Value="NewContact.Name" />
    </div>
    <div>
        <label for="email">Email</label>
        <InputText id="email" @bind-Value="NewContact.Email" />
    </div>
    <div>
        <InputCheckbox id="send-me-deals" @bind-Value="NewContact.SendMeDeals" />
        <label for="send-me-deals">Send me deals</label>
    </div>
    <button>Submit</button>
</EditForm>

@code {
    [SupplyParameterFromForm]
    public Contact NewContact { get; set; } = new();

    public class Contact
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        public bool SendMeDeals { get; set; }
    }

    private async Task AddContact()
    {
        // Add contact...
        NewContact = new();
    }
}
```

Based on sample: [ASP.NET Core updates in .NET 8 Preview 7](https://devblogs.microsoft.com/dotnet/asp-net-core-updates-in-dotnet-8-preview-7/)

On a side note: Antiforgery has also been implemented for Minimal APIs. This applies to forms such as this one in Blazor SSR.

## Pre-rendering of components

It is worth noting that pre-rendering is supported by default in Blazor Web apps. Entire pages including embedded interactive components will be rendered statically on the first request. 

You have to take pre-rendering into account while writing code for initializing components - especially for interactive client components that will run in WebAssembly, since they will be pre-rendered in the server context.

A way to persist component state across pre-rendering and Blazor WebAssembly apps is still in the works.

## What about the Single-project model?

Microsoft did announce quite early that they were seeking to try out a single-project model that would allow developers to put both server code and client code targeting WebAssembly in one project. This was in fact part of the proof-of-concept.

But after presenting the alternative ways of implementing this to the community, who voiced their opinions, it was decided to not pursue the single-project model for ASP.NET Core 8. It was simply because a majority of community members did not think that the developer experience would be that great.

So instead Blazor will follow the well-established project dependency pattern: Blazor Web app will have a separate project for the Client WebAssembly stuff, but the server project can reference and utilize those components. This is way more familiar to developers.

## When should you use Blazor Web app? 

You might consider Blazor Web app when...

* You have an app that largely runs on the server

* Your app serve pages with content - being a CMS, or e-commerce site.

* You might have limited need for interactivity - wanting to opt-in where it is needed.

* You want to use the Razor component model - over for instance, MVC Razor Pages.

## Conclusion

With these enhancements that are coming to ASP.NET Core 8, Blazor on the server will be fully integrated into ASP.NET Core - enabling mixed server-side rendered and interactive experiences for the future to come.

Check out the new "Blazor Web App" template in .NET 8 Previews or the upcoming RC 1.

Ad follow the progress in the [ASP.NET Core repo](https://github.com/dotnet/aspnetcore) on GitHub.

## And let me know

**Are you excited for what is coming to Blazor in ASP.NET Core 8? Or do you still prefer MVC Razor Pages over Blazor?**