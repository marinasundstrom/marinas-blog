---
title: "Introduction to Blazor"
published: 2019-05-30
tags: [Blazor, .NET, Web, WebAssembly]
---

This is an introductory article on Blazor.

Simply put: Blazor is a framework for client-side .NET web development.

<img src="https://github.com/robertsundstrom/blog/blob/master/assets/blazor_brand.png?raw=true" alt="Blazor Brand" width="150"/>

## History
At NDC Oslo back in 2017, Steve Sanderson, of Microsoft, held a presentation titled ["Web Apps can’t really do *that*, can they?"](https://www.youtube.com/watch?v=MiLAE6HMr10&feature=youtu.be&t=31m45s) In this presentation he, among other things, demonstrated WebAssembly (Wasm), how it works, and the benefits it brings to web development. Later, he unveiled an experiment of his: a *web component framework* for .NET, called: Blazor. Sanderson then showed how to write a basic app in C# that run directly in the browser, thanks to WebAssembly.

That probably blew the minds of the people watching it.

 *Sidenote:* At the time, Sanderson was the one who oversaw the integration of SPA frameworks (React and Angular) in ASP.NET Core. Also, worth mentioning is that Sanderson previously had created another popular web framework for the web: [Knockout](https://knockoutjs.com/).

The Blazor project was first hosted on Steve Sandersons own GitHub profile, back when it was based on a portable .NET runtime called DNA. That runtime was later  [replaced by the more mature Mono runtime](http://blog.stevensanderson.com/2017/11/05/blazor-on-mono/) that had had ambition to support WebAssembly.

Then the project moved to its own repository under the ASP.NET organization when it became an official Microsoft experiment. That was followed by shifting focus to the server-side model.

Now it has been integrated into the ASP.NET Core product, and server-side Blazor has shipped with version 3.0. Client-side Blazor is still in preview as the WebAssembly support in Mono is maturing.

## What is Blazor?
Blazor lets you create web apps based on components written in C# and Open Web standards, using the familiar Razor template syntax of ASP.NET MVC and Razor Pages.

A Blazor app is as pure .NET app with access to the entire .NET ecosystem, including thousands of packages on NuGet. Although Blazor apps are exclusively written in C#, it is possible to interop with JavaScript code when needed.

There are two hosting models: *Client-side* in the browser (thanks to WebAssembly), or on *Server-side*, pushing changes out to the client using SignalR. (WebSocket protocol)

Microsoft now recommends people who have been using WebForms to use Blazor instead. Seeing Blazor as its modern successor.

## Origin of the name
The exact origin of the name Blazor is uncertain. Sanderson has been joking about the ”B” standing for "Blockchain", but it more likely comes from "Browser" and the rest from Razor - the view engine and the @ symbol used in the templates. Regardless of origin, the name has stuck since its first public use and become the name of the product.

Server-side Blazor was briefly known as ”Razor Components”. The name had regretably shown to cause confusion with Razor Pages so it later changed back to the more popular name Blazor. 

## Sample: Counter

Blazor components are defined in *.razor* files. The name of the file becomes the name of the component.

A basic example that comes right from the project template is the *Counter*  component:

```csharp
@page "/counter"

<h1>Counter</h1>

<p>Current count: @currentCount</p>

<button class="btn btn-primary" onclick="@IncrementCount">Click me</button>

@functions {
    private int currentCount = 0;

    [Parameter]
    private int IncrementAmount { get; set; } = 1;

    private void IncrementCount()
    {
        currentCount += IncrementAmount;
    }
}
```

Blazor apps are built using the .NET CLI.

This is what it looks like when running the app:

<a href="https://github.com/robertsundstrom/blog/blob/master/assets/blazor_counter.png?raw=true">
    <img src="https://github.com/robertsundstrom/blog/blob/master/assets/blazor_counter.png?raw=true" alt="Counter Page" width="100%"/>
</a>

The component can be instantiated from another component like so:

```csharp
<Counter />
```

Let us walk through the code:

As you can see, components are created using the Razor syntax, which combines HTML with C# for templating. The @ indicates that C# code is being written. 

We define a button and set an event handler for the *onclick* event. The event handler is in C#. It increments the value of the *currentCount* field by the .

Blazor knows when to re-render the component, causing the current count to update when the field changes.

There is also a parameter *IncrementAmount* with a default value of 1. It can be set like so:

```csharp
<Counter IncrementAmount="3" />
```

We are not going to cover routing in depth in this introductory article, but the *@page* directive registers the components as a page, meaning that it has a route (/counter). Provided that you have set up the infrastructure, the Router, this will just work. This comes out of the box so no extra packages is required.

C# class code lives in the *@functions* block. Code-behind support is expected to come soon.

A component is just a C# class. If you want more control, you can write a plain component class and all the render logic yourself, write to the Render Tree, without the use of Razor. But in most cases Razor syntax is the preferred choice.

### More samples

A more extensive example is the [FlightFinder](tree/master/samples/aspnetcore/blazor) app. Then there is the [workshop](https://github.com/dotnet-presentations/blazor-workshop) that is intended for those who want to learn how to build a full-fledged app in Blazor.

## How does Blazor work?

Blazor uses the Razor templating engine - the same as ASP.NET Core uses. Instead of directly rendering static HTML, it first renders to Virtual DOM, with which changes get diffed before actually getting rendered to the HTML DOM. By doing so, only the changed parts (entire elements or just attributes) get re-endered. This is similar to how other component frameworks, such as React and Vue, work.

Under the hood, all rendering is implemented as calls to JavaScript code that manipulates the DOM.

In client-side Blazor all logic is executed in the browser. In the server-side models, on the other hand, Blazor sends and receives changes from the server. The browser part is a *thin* client.

Here is Sanderson's [technical introduction to Blazor](http://blog.stevensanderson.com/2018/02/06/blazor-intro/).


## Client-side vs Server-side
Blazor supports two hosting models that each have its benefits and trade-offs.

Switching between the modes is very easy.

### Client-side
Blazor is running on top of the Mono runtime that has been compiled to WebAssembly, and it runs in the same browser sandbox as JavaScript. Due to this you are limited to what the browser allows you to do.

The resources are limited and you cannot in anyway escape the sandbox. You can, of course, do interop with JavaScript from C#. 

Since the code is being executed on the client it can run without an Internet connection. That makes this model ideal for Progressive Web Apps (PWA).

There is a project template for a client-side Blazor app that is being hosted by ASP.NET Core. It demonstrates how to share code between sever-side and client-side.

There is no debugging support for client-side Blazor yet. To debug on client-side you can set the project up to run server-side when debugging.

If you are targeting older browsers that do not support WebAssembly, such as Internet Explorer, you can polyfill with ASM.js.

If you are interested in knowing more about WebAssembly, I recommend reading my [article](http://robertsundstrom.com/?p=/2019/05/25/explaining-webassembly).

### Server-side
Blazor is running on the server, as part of an ASP.NET Core app. User input and rendered components are sent back and forth between client and server using SignalR (WebSocket protocol). 

All the logic is being executed on the server which means that you have the full .NET Core runtime at your disposal. You can connect to databases and perform complex calculations with the resources you have on that machine.

The obvious downside is that a connection to the server is required for the app to work. It cannot run offline, because it is server-side after all.

The choice is about the trade-offs.

## Blazor for Electron
Another way of running Blazor on the client is using the Electron shell - a hosted instance of the Chromium browser engine. This model lets you create a Blazor desktop app running on .NET Core, instead of WebAssembly. 

This is still experimental and still unsupported.

The experiment can be found [here](https://github.com/aspnet/AspLabs/tree/master/src/ComponentsElectron/sample/SampleApp).

## Benefits of Blazor
With Blazor you can write event-driven web components in C# and Razor syntax, using the tools you already are familiar with.

The benefits

* Cross-platform
* Re-use the skills and tools
* The .NET ecosystem: NuGet packages

## Open Source
Blazor is open source. It is part of the ASP.NET Core repository on GitHub. There you can follow and contribute in the discussion and development of the framework.

## Resources
* [Blazor](https://www.blazor.net) website
* [Blazor: a technical introduction](http://blog.stevensanderson.com/2018/02/06/blazor-intro/) - Steve Sanderson
* VIDEO: ["Web Apps can’t really do *that*, can they?"](https://www.youtube.com/watch?v=MiLAE6HMr10&feature=youtu.be&t=31m45s) (NDC Oslo 2017)
* VIDEO: [Full stack web development with ASP.NET Core 3.0 and Blazor - BRK3017](https://www.youtube.com/watch?v=y7LAbdoNBJA) (Build 2019)

I hope you found this introduction useful.
