---
title: The anatomy of an ASP.NET Core app
subtitle: Understanding the basic concepts
published: 2023-09-07
tags: [.NET, ASP.NET Core, Web development, Blazor, Razor]
---

## Preface

The goal of this article is to give someone who is new to ASP.NET Core an overview over the basic concepts of ASP.NET Core - by not going too much into technical details. Though, I will try to cover the essentials - Showing you the simplicity of the ASP.NET Core app model.

If you come from NodeJS, and you have been using ExpressJS, then you might find many of the concepts familiar.

Hopefully, you will find the examples useful in understanding what ASP.NET Core is about, and what you might be able to do with it.

I have included some links at the end, if you want to go deeper into the topics.

_Parts of the article, specifically that about _Razor components_, is about the upcoming .NET 8. (To be released in November 2023)_


## Contents

1. <a href="/articles/the-anatomy-of-an-aspnetcore-app#section-1">What is ASP.NET Core?</a>
2. <a href="/articles/the-anatomy-of-an-aspnetcore-app#section-2">Hello World!</a>
3. <a href="/articles/the-anatomy-of-an-aspnetcore-app#section-3">Core concepts</a>
   1. <a href="/articles/the-anatomy-of-an-aspnetcore-app#section-3-1">Application</a>
   2. <a href="/articles/the-anatomy-of-an-aspnetcore-app#section-3-2">Request and Response</a>
   3. <a href="/articles/the-anatomy-of-an-aspnetcore-app#section-3-3">Route handlers</a>
   4. <a href="/articles/the-anatomy-of-an-aspnetcore-app#section-3-4">Middleware</a>
4. <a href="/articles/the-anatomy-of-an-aspnetcore-app#section-4">Razor components</a>
5. <a href="/articles/the-anatomy-of-an-aspnetcore-app#section-5">Native AOT compilation</a>
6. <a href="/articles/the-anatomy-of-an-aspnetcore-app#section-6">Conclusion</a>
7. <a href="/articles/the-anatomy-of-an-aspnetcore-app#section-7">Links</a>

<h2 id="section-1">What is ASP.NET Core?</h2>

ASP.NET Core is a framework for building apps for the Web that run on the server, using .NET, and programming languages like C#. Running on the server means that it processes HTTP requests and produces HTTP responses.

The framework gives you the tools and abstractions to facilities the generation server-side rendered HTML pages using Razor syntax, as well as the creation of Web APIs, serving data in formats such as JSON.

Both .NET and ASP.NET Core is cross-platform, and runs on Windows, Mac and Linux. It is also open-source, being developed in the open by Microsoft with help from the community.

ASP.NET Core is also Web standards-compliant.

<h2 id="section-2">Hello World!</h2>

This is the simplest ASP.NET Core app that you can write (``Program.cs``):

```csharp
var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/hello", () => "Hello, World!");

app.Run();
```

This is the project file (``.csproj``):

```xml
<Project Sdk="Microsoft.NET.Sdk.Web">

  <PropertyGroup>
    <TargetFramework>net8.0</TargetFramework>
    <Nullable>enable</Nullable>
    <ImplicitUsings>enable</ImplicitUsings>
  </PropertyGroup>

</Project>
```

To run the app:

```sh
dotnet run
```

Then, view the page in your browser: ``http://localhost:5000/``

I don't think that I need to explain this code. It is a simple endpoint for ``GET /hello`` that returns a string .

All you need to do is to target the ``Microsoft.NET.Sdk.Web``. No additional dependencies are required for this since this is built into .NET SDK.

The example above uses top-level statements, which is a feature that eliminates the ceremony of having to declare a ``Main`` method.


<h2 id="section-3">Core concepts</h2>

This section is meant to give you a basic understanding of what an ASP.NET Core app looks like - the anatomy of an ASP.NET Core application, if you want.

<h3 id="section-3-1">Application</h3>

The ``WebApplication`` object is representing the application with what is needed to serve HTTP, and the ``WebApplicationBuilder`` is responsible for constructing that object. It sets up the basic dependencies for the app to work - the Web server.

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the builder

var app = builder.Build();

// Map endpoints, and/or add middleware.

app.Run();
```

The factory method ``WebApplication.CreateBuilder`` creates a builder that has default services added to it - such as the HTTP Server, and facilities for logging.

Once the ``WebApplication`` object has been created in your code, you can start mapping routes to handlers, and add middleware to the pipeline - to add some behavior.

And finally you can start the app, by calling ``Run``.

#### Dependency injection

Dependency injection is built into ASP.NET Core. And it is with the builder that you register any dependencies that you want to go into the service container of the app. 

If you want to enable, for instance Open API, then you have to add services that allow for that. Usually you are provided with handy extension methods that do all that hard work for you. These often take delegates that can be used to configure those services.

This is what dependency injection looks like when registering your own service:

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<Dog>();

var app = builder.Build();

app.MapGet("/give-treat", (string treat, Dog dog) => $"{dog.Name} got a {treat}!");

app.Run();

class Dog 
{
    public string Name => "Fido";
}
```

Hit the endpoint: ``/give-treat?treat=snack``. The server responds with: ``Fido got a snack``.

You see that services might be injected as parameters into any handler, next to bound parameters. The framework will try to figure out whether, or not, a class type is a bound parameter, or a service - based on what is in the service container.

The ``AddScoped<T>()`` adds a service to the service container with the scoped lifetime. In the ASP.NET Core context, that means that the same instance of type ``T`` is being resolved during the scope of the current HTTP request. When there is another request, a new instance is being created.

The frameworks makes some object available through dependency injection, including ``HttpContext``, and both ``HttpRequest`` and ``HttpResponse`` separately. The ones mentioned are scoped.

<h3 id="section-3-2">Request and Response</h3>

Since ASP.NET Core is used to build Web apps on the server, it deals with HTTP. That means processing HTTP Requests and producing HTTP Responses.

#### How requests are processed

Requests are being processed in the following way by ASP.NET Core:

1. _Request_ comes in
2. _Middleware_ processes request
3. _Route handler_ handles request.
4. _Response_ is returned

We will further explain the concepts later in this article.

#### HttpContext

Information about the current request and the response is encapsulated in the ``HttpContext`` object - which has properties representing both the ``Request`` and ``Response``.

You can read the request, and inspect its headers and content - to the determine what to do in either route handlers, or in middleware.

You can then modify the response - setting the status code, writing to the body, and modifying the response headers

The ``HttpContext`` is accessible from middleware, and injectible into route handlers. Both ``HttpRequest`` and ``HttpResponse`` can be separately injected as well.

```csharp
var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", async (HttpContext httpContext) => 
    await httpContext.Response.WriteAsync("Hello world"));

app.Run();
```

In general, you don't need to worry about the ``HttpContext`` since route handlers have parameter binding.

<h3 id="section-3-3">Route handlers</h3>

The perhaps most important part of your Web application is to handle requests that are being sent to specific routes in order to perform some operation, and then return a response with some result. 

You can map a a specific path (or route) and HTTP verb, such as ``GET`` and ``POST``, to a route handler using the ``Map`` (``MapGet``, ``MapPost`` etc) extension methods on the ``WebApplication`` object.

In software development, we often interchangeably refer to a route handler that handles a specific route as an **Endpoint**.

Sometimes you also might stumble on the term "Minimal API" endpoints in ASP.NET Core. That is simply a name for this feature.

```csharp
var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// GET /
app.MapGet("/", () => "Hello, World!");

// GET /test?id=20
app.MapGet("/test", (int id) => $"The Id is: {id}");

// GET /foo?value=42 - Returns: { "Value": 42 }
app.MapGet("/foo", (int value) => new FooResult(value));

// GET /items/foo
app.MapGet("/items/{id}", (string id) =>  $"Item Id: {id}");

// POST /greet (JSON Body: { "name": "Bob" } )
app.MapPost("/greet", (GreetingRequest request) => $"Hello, {request.Name}!");

app.Run();

public record FooResult(int Value);

public record GreetingRequest(string Name);
```

#### Parameter binding

As demonstrated above, you can bind route parameters, query strings, and request bodies to parameters. You can also inject services as parameters, as seen earlier. 

And you can return both primitive values and complex objects, as well as results that modify the response and its status code. 

And worth noting that the default serialization format is JSON.

Before the route handlers, middleware might be executed. We will have a look at Middlware later in this article.

#### Results

A route handler might return a specific result object that modifies the response, its status code, and content. Examples of this is emitting a response with a status code like ``400 BadRequest``, or ``201 Created``, or for sending a file.

The ``TypedResults`` factory class contains static methods for creating these result objects:

```csharp
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// GET /?number=20
app.MapGet("/", Results<Ok<string>, BadRequest> (int number) => 
{
    if(number > 10) 
    {
        return TypedResults.BadRequest();
    }

    return TypedResults.Ok($"The number is: {number}");
});

app.Run();
```

Note that we can specify the return type for the lambda expression: ``Results<Ok<int>, BadRequest>``. 

The type ``Results<TResult1,TResult2>`` acts as a discriminated union that makes it explicit what results the endpoint can and may return. 

Using result types will make your endpoints type-safe, and aid when generating Open API specifications (Swagger) - if that has been enabled.

##### Bonus: Improve readability

To simplify and improve readability of your code, you can import the static members of ``TypedResult``, that is, the factory methods, into the current scope - like so:

```csharp
using static Microsoft.AspNetCore.Http.TypedResults;

// Code omitted

app.MapGet("/helloworld", Results<Ok<string>, BadRequest> () => 
{
    // Equivalent to: TypedResults.Ok()

    return Ok("Hello, World"!);
});
```

#### Filters

A filter is a piece of code that runs after the route, or endpoint, has been resolved, but before the route handler has been invoked.

Filters allow you to intercept, access parameters, and do some manipulation. The reasons for intercepting an endpoint might be logging, or validation of data. 

```csharp
var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

string ColorName(string color) => $"Color specified: {color}!";

app.MapGet("/colorSelector/{color}", ColorName)
    .AddEndpointFilter(async (invocationContext, next) =>
    {
        var color = invocationContext.GetArgument<string>(0);

        if (color == "Red")
        {
            return Results.Problem("Red not allowed!");
        }
        return await next(invocationContext);
    });

app.Run();
```

Filters are similar to Middleware (next section) but differ in that they act on the endpoint, while middleware act on the request, before the endpoint is resolved.

You can chain filters to an endpoint. They will be executed in the order if First In, Last Out (FIFO), before invoking the route handler.

You can make endpoint filers re-usable by putting them into their own classes that implement ``IEndpointFilter``.


<h3 id="section-3-4">Middleware</h3>

The purpose of middleware is to process a request and to modify the response. They are placed in a pipeline, and executed before the actual route handler.

Using middleware we extend our web apps with additional functionality, such as Response Compression, and Open API/Swagger support.

For example, the Response Compression middleware compresses a response. It can be added to the container, and then the Web App, using extension methods.

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddResponseCompression();

var app = builder.Build();

app.UseResponseCompression();

app.MapGet("/", () => "Hello World!");

app.Run();
```

As seen above, middleware can be dependant on specific services. Extension methods like ``AddResponseCompression``, are convenient for making your code cleaner by hiding away service registration as well as middleware stuff.

As mentioned, middleware are in a pipeline, so they are executed in the order that they have been added to that pipeline. And when a middleware has executed it returns to the previous middleware, this allows it to execute logic afterwards. _(See example below)_

You can define custom middleware delegates like so:

```csharp
var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.Use(async (context, next) =>
{
    // Do work that can write to the Response.

    Console.WriteLine("Start");

    await next.Invoke();

    // Do logging or other work that doesn't write to the Response.

     Console.WriteLine("End");
});

app.Use(async (context, next) =>
{
    Console.WriteLine("Foo");

    await next.Invoke();

    Console.WriteLine("Bar");
});

app.MapGet("/hello", () => "Hello, World!");

app.Run();
```

This is what it will look like when invoking ``/hello``:

```sh
Start
Foo
"/hello" route handler
Bar
End
```

You can also define middleware as a class, and add it to the pipeline using the ``UseMiddleware<T>()`` method.

<h2 id="section-4">Razor components</h2>

_This section is about things coming in .NET 8. (To be released in November 2023)_

Razor components are components written in the Razor syntax, which is a templating language that combines HTML and C# code in order to dynamically generate web pages. It basically is for .NET what JSX is to React - but not really.

Consider this ``HelloWorld.razor`` component:

```razor
<h1>@title</h1>

<ul>
@foreach(var item in items) 
{
    <li>@item.Name</li>
}
</ul>

@code 
{
    string title = "Hello, World!";
    List<Item> items = new List<Item>();

    protected override void OnInitialized()
    {
        items.Add(new Item("A"));
        items.Add(new Item("B"));
    }

    public record Item(string Name);
}
```

The Razor templating language is named for its ``@`` character (at-symbol) which signifies the start of C# code. The tooling is quite smart about that when compiling the template into C#. 

And as you might have noticed, Razor components live in files with the ``.razor`` extension.

You can return a Razor component from an endpoint using the ``RazorComponentResult`` result type:

```csharp
using TestApp1;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents();

var app = builder.Build();

app.Map("/helloworld", () => new RazorComponentResult<HelloWorld>());

app.Run();
```

Upon navigating to ``/helloworld`` this will be rendered:

```html
<h1>Hello, World!</h1>

<ul>
    <li>A</li>
    <li>B</li>
</ul>
```

### Interactive components

Although Razor components are server-side rendered by default, you can make them interactive. You can build an entire interactive SPA (Single-page application) with routable pages if you so want.

Interactive apps are more commonly known as Blazor apps. Blazor is the product name. Though the name dates back to the early prototype.

With Blazor, you basically get the choice of running the component on the server, or in the browser using WebAssembly. These are called _Render modes_.

This is how you set up simple app with one routable interactive component: ``Counter``.

The ``App.razor`` is the root component of the page:

```razor
@using Microsoft.AspNetCore.Components.Routing
@using Microsoft.AspNetCore.Components.Web

<!DOCTYPE html>
<html lang="en">

<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <meta http-equiv="refresh" content="3600">
    <base href="/" />
    <!-- <link rel="stylesheet" href="TestApp1.styles.css" /> -->
    <HeadOutlet />
</head>

<body>
    <CascadingAuthenticationState>
        <Router AppAssembly="@typeof(App).Assembly">
            <Found Context="routeData">
                <RouteView RouteData="@routeData"></RouteView>
                <FocusOnNavigate RouteData="@routeData" Selector="h1" />
            </Found>
        </Router>
    </CascadingAuthenticationState>

    <script src="_framework/blazor.web.js" suppress-error="BL9992"></script>
</body>
</html>
```

Here is the ``Counter.razor`` page component:

```razor
@using Microsoft.AspNetCore.Components.Web
@page "/"
@attribute [RenderModeServer]

<PageTitle>Counter</PageTitle>

<p role="status">Current count: @currentCount</p>

<button class="btn btn-primary" @onclick="IncrementCount">Click me</button>

@code {
    private int currentCount = 0;

    private void IncrementCount()
    {
        currentCount++;
    }
}
```

The ``@page`` directive tells the router that this component is a page with a specific route: ``/``. 

The initialization in ``Program.cs`` is a bit different because the app will itself make sure to find the page components and register endpoints for them. And also, we enable interactivity with the Server render mode.

```csharp
using TestApp1;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddServerComponents();
    
var app = builder.Build();

app.UseStaticFiles();

app.MapRazorComponents<App>()
    .AddServerRenderMode();

app.Run();
```

When navigating to ``/`` in your browser, this will happen:

The component will show up in your browser. Set up a WebSocket connection to the server where the component will run - the reason for the script. 

When you click the button, the ``IncrementCount``method will be invoked on the server, and increase the value of the ``currentCount`` field by 1. The UI will then be updated to reflect that change of state.

What makes a difference is the ``@attribute [RenderModeServer]``, and the little piece of JS script that activates interactivity in the browser.

<h2 id="section-5">Native AOT compilation</h2>

ASP.NET Core 8 does support Ahead-of-Time (AOT) compilation scenarios when those are needed. Meaning that you can compile an app to native machine code - but it is not for everyone.

The framework is giving you ways to do stuff that is not reliant on Reflection. And by only including what is only necessary to run the app, you get a small app. The drawback is that you as a developer have to add stuff back manually - which might be fine if you have the concerns that AOT is supposed to solve.

When you are serializing JSON, you instead will use source generators to generate the C# code that parses and map JSON to objects at compile-time - because Reflection is not supported in native AOT code.

This is why AOT really is not for every app.

Because .NET 8 and ASP.NET Core already are well-optimized, you most likely never have go down this route when building a normal Web application. You might not gain anything significant from going AOT, compared to the effort put in.

<h2 id="section-6">Conclusion</h2>

This concludes this article about the basic anatomy of an ASP.NET Core.

If you want to learn more, then I recommend you to check out the links below.


<h2 id="section-7">Links</h2>

These are links to the official documentation at Microsoft Learn:

* [ASP.NET Core documentation](https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-8.0)
* [Dependency injection in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-8.0)
* [Use HttpContext in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/use-http-context?view=aspnetcore-8.0)
* [Minimal APIs quick reference](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-8.0)
* [ASP.NET Core Middleware](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-8.0)
* [ASP.NET Core Razor components](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/?view=aspnetcore-8.0)
* [ASP.NET Core support for native AOT](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/native-aot?view=aspnetcore-8.0)