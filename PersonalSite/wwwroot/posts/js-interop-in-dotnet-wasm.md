---
title: "JavaScript interop in .NET on WebAssembly"
published: 2019-10-20
tags: [.NET, WebAssembly, WASM, JavaScript, Blazor]
---

> This article was written back in 2019-06-15.

This article is about JavaScript interop in .NET on WebAssembly in the context of Blazor.

Since WASM development, at least, is about the web we will assume that we can do some interop with the browser, like DOM manipulation. But how?

## JavaScript interop in Blazor

If you want to interop with JavaScript today, you are most likely writing a Blazor app.

In fact, Blazor on client-side (browser) is itself implemented with JavaScript interop, for rendering to the DOM, since WebAssembly does not not provide direct access to the DOM API:s.

To do JS Interop you inject and instance of the *JSRuntime* object.

In a Blazor (Razor) component file:

```csharp
@using Microsoft.JSInterop
@inject IJSRuntime JSRuntime
```

In a component class:

```csharp
using Microsoft.JSInterop;

[Inject]
IJSRuntime JSRuntime { get; set; }
```

<h3 id="js"> .NET to JavaScript</h3>

The basic case of interop is to call JavaScript code from C#. 

Let's have a look at some [examples](https://docs.microsoft.com/en-us/aspnet/core/blazor/javascript-interop?view=aspnetcore-3.0) from the Blazor documentation.


In *wwwroot/exampleJsInterop.js*:

```javascript
window.exampleJsFunctions = {
  showPrompt: function (text) {
    return prompt(text, 'Type your name here');
  }
};
```

And in your Blazor component:

```csharp
// showPrompt is implemented in wwwroot/exampleJsInterop.js
var name = await JSRuntime.InvokeAsync<string>(
    "exampleJsFunctions.showPrompt",
    "What's your name?");
```

We simply define and object on the *window* global object containing the function that we want to invoke. Then, from C#, we invoke that function by name and passing the necessary argument, letting the JSRuntime API perform all the interop. 

It kind of resembles how you would call a member using Reflection, or rather any scripting language engine (IronPython, Peachpie etc.) implemented in .NET.

In the case of functions that return *void*, *undefined* or any unspecified type of value, you pass *object* as a type parameters when invoking it from .NET code:

```csharp
await JSRuntime.InvokeAsync<object>(
    "exampleJsFunctions.nullReturningAction");
```

### JavaScript to .NET

Sometimes you want to call into .NET from JavaScript. This is also fully supported.

On the JavaScript side you have the *DotNet*. It will give you the tools to interop with .NET.

Like invoking a method:

```javascript
await DotNet.invokeMethodAsync('class', 'method', 'arg1', 'arg2' ... )
```

The method returns a *Promise* containing the result, hence *await*.

To invoke .NET code, you need to declare a static method that is decorated with the *JSInvokable* attribute. It is required in order for the runtime to know that it should export the method and make it invokable from JavaScript.

Consider this code that is declared in a Blazor component:

```csharp
<button type="button" class="btn btn-primary"
        @onclick="exampleJsFunctions.returnArrayAsyncJs()">
    Trigger .NET static method ReturnArrayAsync
</button>

@code {
    [JSInvokable]
    public static Task<int[]> ReturnArrayAsync()
    {
        return Task.FromResult(new int[] { 1, 2, 3 });
    }
}
```

Bear in mind, the code that is being invoked from JavaScript could be declared in a static class.

The code that invokes the method looks like this:

```javascript
window.exampleJsFunctions = {
  returnArrayAsyncJs: async function () {
    const data = await DotNet.invokeMethodAsync('BlazorSample', 'ReturnArrayAsync');
    data.push(4);
    console.log(data);
  },
};
```

As you can see, it even marshalls the .NET Array as an JavaScript array.

### Passing .NET references to JavaScript

You can also pass .NET object references to JavaScript, and invoke its members from there. 

Using this API, you wrap an object reference:

```csharp
DotNetObjectRef.Create(obj);
```

As demonstrated here:

```csharp
public class ExampleJsInterop
{
    private readonly IJSRuntime _jsRuntime;

    public ExampleJsInterop(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public Task CallHelloHelperSayHello(string name)
    {
        // sayHello is implemented in wwwroot/exampleJsInterop.js
        return _jsRuntime.InvokeAsync<object>(
            "exampleJsFunctions.sayHello",
            DotNetObjectRefnew HelloHelper(name)));
    }
}
```

In JavaScript, the object is being wrapped and invoked in a way similar to  when invoking JavaScript from .NET using the *JSRuntime* class:

```javascript
window.exampleJsFunctions = {
  sayHello: function (dotnetHelper) {
    return dotnetHelper.invokeMethodAsync('SayHello')
      .then(r => console.log(r));
  }
};
```

## Capture DOM Element references

In Blazor you can capture references to elements that you then can pass to JavaScript.

```csharp
<input @ref="username" ... />

@code {
    ElementRef username;
}
```

Consider this piece of JavaScript code that simply calls the *focus* function on the element that has been passed as an argument:

```javascript
window.exampleJsFunctions = {
  focusElement : function (element) {
    element.focus();
  }
}
```

The runtime does not need to wrap the object as it is a JavaScript object.

Like in the previous examples, you pass the element reference like this from .NET:

```csharp
@inject IJSRuntime JSRuntime

<input @ref="username" />
<button @onclick="SetFocus">Set focus on username</button>

@code {
    private ElementRef username;

    public async void SetFocus()
    {
        await JSRuntime.InvokeAsync<object>(
                "exampleJsFunctions.focusElement", username);
    }
}
```

This can be made cleaner by wrapping the interop call in an extension method:

```csharp
public static Task Focus(this ElementRef elementRef, IJSRuntime jsRuntime)
{
    return jsRuntime.InvokeAsync<object>(
        "exampleJsFunctions.focusElement", elementRef);
}
```

That can be invoked like this:

```csharp
@inject IJSRuntime JSRuntime
@using JsInteropClasses

<input @ref="username" />
<button @onclick="SetFocus">Set focus on username</button>

@code {
    private ElementRef username;

    public async Task SetFocus()
    {
        await username.Focus(JSRuntime);
    }
}
```

## DOM interop and the Future

Right now, there is no clear way of interoping with the Browser API:s, such as the DOM. You have to write your own glue code, as there is not yet an officially supported binding. 

However, work is being done to enable to easily bind .NET objects to JavaScript objects. That will lead the way for better experience in interop with the DOM.

Most of the innovation around WASM on .NET is happening in the [repository](https://github.com/mono/mono/blob/75741eb68d902f244db0769ec783211e35aa5cda/sdks/wasm/) of the Mono project.

You can actually try it out today.

Assuming that you have a Blazor project, or and Uno.Bootstrapper project.

Install the *WebAssembly.Bindings* package (Unofficial package(?))
```sh
dotnet add package WebAssembly.Bindings
```

Here is a sample, showing what you can do:

```csharp
var window = (JSObject)WebAssembly.Runtime.GetGlobalObject("window");
var document = (JSObject)WebAssembly.Runtime.GetGlobalObject("document");

var listener = new Action<object>((ev) =>
{
    var documentElement = (JSObject)document.GetObjectProperty("documentElement");
    var scrollTop = (int)documentElement.GetObjectProperty("scrollTop");
    Console.WriteLine(scrollTop);
});

window.Invoke("addEventListener", "scroll", listener);
```

It gives us infrastructure based around the *WebAssembly.Runtime*, that is part of the Mono SDK for WASM.

This [sample](https://github.com/mono/mono/blob/75741eb68d902f244db0769ec783211e35aa5cda/sdks/wasm/sample.cs), gives us this base class:

```csharp
// Serves as a wrapper around a JSObject.
class DOMObject : IDisposable
{
    public JSObject ManagedJSObject { get; private set; }

    public DOMObject(object jsobject)
    {
        ManagedJSObject = jsobject as JSObject;
        if (ManagedJSObject == null)
            throw new NullReferenceException($"{nameof(jsobject)} must be of type JSObject and non null!");
    }

    public DOMObject(string globalName) : this((JSObject)Runtime.GetGlobalObject(globalName))
    { }

    public object GetProperty(string property)
    {
        return ManagedJSObject.GetObjectProperty(property);
    }

    public object Invoke(string method, params object[] args)
    {
        return ManagedJSObject.Invoke(method, args);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        ManagedJSObject?.Dispose();
        ManagedJSObject = null;
    }
}
```

It can be utilized like this to wrap a JavaScript browser object:

```csharp
var window = new JSObject("document");
var title = window.GetProperty("title");

Console.WriteLine(title);
```

The purpose of this class, clearly, is to serve as a base class for a future binding to the JavaScript DOM API:s.

You can easily extend that class like this: (also from the [sample](https://github.com/mono/mono/blob/75741eb68d902f244db0769ec783211e35aa5cda/sdks/wasm/sample.cs)):

```csharp
class PositionEventArgs : EventArgs
{
    public Position Position { get; set; }
}

class GeoLocation : DOMObject
{
    public event EventHandler<Position> OnSuccess;
    public event EventHandler<PositionError> OnError;

    public GeoLocation(object jsobject) : base(jsobject)
    {
    }

    public void GetCurrentPosition()
    {
        var success = new Action<object>((pos) =>
        {
            OnSuccess?.Invoke(this, new Position(pos));
        });

        var error = new Action<object>((err) =>
        {
            OnError?.Invoke(this, new PositionError(err));
        });

        ManagedJSObject.Invoke("getCurrentPosition", success, error);
    }
}

class Position : DOMObject
{

    public Position(object jsobject) : base(jsobject)
    {
    }

    public Coordinates Coordinates => new Coordinates(ManagedJSObject.GetObjectProperty("coords"));
}

class PositionError : DOMObject
{

    public PositionError(object jsobject) : base(jsobject)
    {
    }

    public int Code => (int)ManagedJSObject.GetObjectProperty("code");
    public string message => (string)ManagedJSObject.GetObjectPropert("message");
}

class Coordinates : DOMObject
{

    public Coordinates(object jsobject) : base(jsobject)
    {
    }

    public double Latitude => (double)ManagedJSObject.GetObjectProperty("latitude");
    public double Longitude => (double)ManagedJSObject.GetObjectProperty("longitude");
}
```

Just to demonstrate how the class could be consumed:

```csharp
JSObject document = new JSObject("document");
JSObject output = document.Invoke("getElementById", "output");

GeoLocation geoLocation;
try
{
    geoLocation = new GeoLocation(navigator.GetProperty("geolocation"));
}
catch
{
    output.SetObjectProperty("innerHTML", "<p>Geolocation is not supported by your browser</p>");
    return;
}

output.SetObjectProperty("innerHTML", "<p>Locating…</p>");

geoLocation.OnSuccess += (object sender, Position position) =>
{
    using (position)
    {
        using (var coords = position.Coordinates)
        {
            var latitude = coords.Latitude;
            var longitude = coords.Longitude;

            output.SetObjectProperty("innerHTML", $"<p>Latitude is {latitude} ° <br>Longitude is {longitude} °</p>");
        }
    }
};

geoLocation.OnError += (object sender, PositionError e) =>
{
    output.SetObjectProperty("innerHTML", $"Unable to retrieve your location: Code: {e.Code} - {e.message}");
};

geoLocation.GetCurrentPosition();

geoLocation = null;
```

Then, imagine generating a class library for the whole Web browser API from existing TypeScript type definitions. The progress can be tracked in [this issue](https://github.com/mono/mono/issues/10775) in the Mono repo.

## Conclusion

As you can see, there are a number of ways to interop between .NET and JavaScript when targeting WebAssembly. It will be even better when there is a binding to the Browser API:s.

The future looks promising.

## References
* [Blazor JavaScript interop](https://docs.microsoft.com/en-us/aspnet/core/blazor/javascript-interop) - ASP.NET Core Documentation
* [Mono WASM SDK](https://github.com/mono/mono/tree/75741eb68d902f244db0769ec783211e35aa5cda/sdks/wasm) - Mono on GitHub
