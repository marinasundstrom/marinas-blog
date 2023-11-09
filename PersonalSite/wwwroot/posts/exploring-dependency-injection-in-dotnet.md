---
title: Exploring dependency injection in .NET
published: 2023-11-09
tags: [C#, .NET]
---

Microsoft provides a nifty set of dependency injection framework abstractions, as well as a default service container (IoC) implementation. Because of this architecture, it does however allow you to plug in your favorite dependency framework, like Autofac.

We will cover the ``ServiceCollection``, ``ServiceProvider``, service lifetimes, and scopes.

For the code to run in a Console application, you need to add a package reference:

```xml
<PackageReference Include="Microsoft.Extensions.DependencyInjection" Version="7.0.0" />
```

Though, ASP.NET Core already references that packages.

## What is Dependency injection?

Dependency injection simply means that we inject a service (or dependency) into another class, like via constructors, or parameters.

This is constructor injection, in which a dependency is injected as a parameter in the constructor:

```csharp
var todoService = new TodoService();
var todoController = new TodoController(todoService);
```

There are several types of dependency injection:

* **Constructor injection** - as demonstrated above
* **Field or Property injection** - into a field, or a property
* **Method parameter injection** - as a parameter into a method

The framework that is covered only supports constructor injection. But there are places like in Blazor, where services can be injected into properties. That is not ultimately handled by this framework, although it uses it.

### Unit testing

Dependency injection is especially useful when doing unit testing. Since you can pass mock instances into the class.

## What is Inversion of control?

The default way of creating ("newing up") an object in a class would be like this:

```csharp
class Foo 
{
    Bar bar = new Bar();
}
```

This has its downsides. The object is created inside the class, and you have no easy way of controlling what type you are instantiating. What if you need to switch that type and its implementation out for another type when doing testing, or perhaps when running on another platform?

What if we instead could hand over that control of creating the object to an external service, that is inverting the control.

The "How" gets clear when you combine it with dependency injection.

```csharp
class Foo(Bar bar)
{
    Bar bar = bar;
}
```

We may get a service instance from a service provider - a so called IoC container.

As demonstrated below, you request an instance of a service from the service container, and it creates the object, resolves the service dependencies that are to be passed into it as parameters.

```csharp
using Microsoft.Extensions.DependencyInjection;

ServiceCollection services = new ();

services.AddTransient<Foo>();
services.AddSingleton<Bar>();

var serviceProvider = services.BuildProvider();

var foo = serviceProvider.GetService<Foo>();

var bar = foo.Bar;

var foo2 = serviceProvider.GetService<Foo>();

var bar2 = foo2.Bar;

Console.WriteLine($"Same Foo: {foo == foo2}");
Console.WriteLine($"Same Boo: {bar == bar2}");

// Same Foo: false
// Same Bar: true
```

As you can see, ``Bar`` has a singleton lifetime, so there will only be one instance of it within the container.

While ``Foo`` is transient, and a new instance is created each time when being requested.

We will go through service lifetimes.

## ServiceCollection

Let's dig into the service collection, what it is, and how to use it.

### Adding services

In its simplest form you call a method:

```csharp
using Microsoft.Extensions.DependencyInjection;

ServiceCollection services = new ();

// Service type
services.AddTransient<Foo>();
```

There are 4 essential ways of registering a service, and it will decide the way it is being created.

```csharp
using Microsoft.Extensions.DependencyInjection;

ServiceCollection services = new ();

// Service type
services.AddTransient(typeof(Foo));

services.AddTransient<Foo>();

// Service type and implementation type
services.AddTransient(typeof(IFoo), typeof(Foo));

services.AddTransient<IFoo, Foo>();

// Factory
services.AddTransient(serviceProvider => new Foo());

// Instance
services.AddTransient(new Foo());

// The above are aliases for inserting a ServiceDescriptor
services.Insert(0, new ServiceDescriptor(typeof(Foo), typeof(Foo), ServiceLifetime.Scoped));
```

The factory overload makes it possible to control some of the service creation. It receives the actual ``IServiceProvider`` as an argument so you can resolve services.

#### Registering open generic types

You can register open generic types, and resolve a instance with type parameter.

This is ideal for repositories, and factory types. The standard ```ILogger<T>``` interface has been registered this way.

```csharp
using Microsoft.Extensions.DependencyInjection;

ServiceCollection services = new ();

services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

var serviceProvider = services.BuildProvider();

var fooRepository = serviceProvider.GetService<IRepository<Foo>>();


class Foo {}

interface IRepository<T> {}

class Repository<T> : IRepository<T> {}
```

#### Assert that a service is added just once

The order you add dependencies will matter if you register the same service type twice.

To prevent problems with double-registration you can use the ``TryAdd`` overloads that tries to add a service, and returns a boolean value telling whether the service was added, or not.

```csharp
bool added = services.TryAddTransient<Foo>(); // True
bool added2 = services.TryAddTransient<Foo>(); // False
```

#### Manipulating the service collection

It happens sometimes that you need to remove, replace, or reorder dependencies, during the registration. For that you can query for the service descriptor, and then remove the descriptor.

```csharp
services.AddScoped<Foo>();

var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(Foo));
services.Remove(serviceDescriptor);
```

### Service lifetimes

There are 3 service lifetimes that each determine the creation and lifetime of a requested service:

* **Singleton** - there will be one shared instance of a service
* **Transient** - get a new service each time requested
* **Scoped** - the instance is bound to a scope (explained later)

These are the specific methods that can be used to register services with the lifetimes:

```csharp
using Microsoft.Extensions.DependencyInjection;

ServiceCollection services = new ();

services.AddSingleton<Foo>();

services.AddTransient<Foo>();

services.AddScoped<Foo>();
```

The overloads mentioned above are available for all variants.

In order to be able to resolve instances, the lifetimes must be compatible. You can't resolve scoped services from singletons, for obvious reasons.

## ServiceProvider

The actual instances are created and managed by the ``ServiceProvider``. You have already seen that in action.

But, here are the methods that can be used when resolving services:

```csharp
using Microsoft.Extensions.DependencyInjection;

ServiceCollection services = new ();

services.AddTransient<Foo>();

var serviceProvider = services.BuildProvider();

// GetService (Non-generic)
var foo = (Foo)serviceProvider.GetService(typeof(Foo));

// GetService (Generic)
var foo2 = serviceProvider.GetService<Foo>();

// This will throw an exception if service type has not been registered
var foo3 = serviceProvider.GetRequiredService<Foo>();
```

You can't add or register new services to an existing service provider.

### Disposing services

The framework will handle the disposal of any services, by calling ``Dispose`` on services that implement ``IDisposable``. 

There is one exception though:

When you provide an already instantiated object (Instance) that implements ``IDisposable``, the container will not call the ``Dispose`` method. You just have to deal with it yourself, or register it as a factory.

### Service scopes

You can limit a service's lifetime to scopes that you open in your app.

In ASP.NET Core, each request has its own scope, to which certain services are bound. Meaning they get created when the request starts, and disposed when the request has finished. ``HttpContext`` is an example of such a service.

```csharp
using Microsoft.Extensions.DependencyInjection;

ServiceCollection services = new ();

services.AddScoped<Foo>();

var serviceProvider = services.BuildProvider();

// Create a scope

using(var scope = serviceProvider.CreateScope()) 
{
    var foo = scope.ServiceProvider.GetService<Foo>();
}

// New scope

using(var scope = serviceProvider.CreateScope()) 
{
    // New instance
    var foo2 = scope.ServiceProvider.GetService<Foo>();
}
```

As mentioned, you can't resolve scoped services from singletons.

#### Keyed services

From .NET 8 and on, you can register services with keys. Meaning that you can register multiple instances of the same service type with different keys. 

The keys themselves can be of any type of object, or value, not just strings.

```csharp
using Microsoft.Extensions.DependencyInjection;

ServiceCollection services = new ();

services.AddTransient<Consumer>();

services.AddKeyedSingleton<IFoo, Foo1>("foo1");

services.AddKeyedSingleton<IFoo, Foo2>("foo2");

var serviceProvider = services.BuildProvider();

var foo1 = serviceProvider.GetKeyedService<IFoo>("foo1");

var foo2 = serviceProvider.GetKeyedService<IFoo>("foo2");

Console.WriteLine($"Equal: {foo1 == foo2}"); // False
```

You can use the ``FromKeyedServices`` attribute to specify what instance to inject as parameter in constructors.

```csharp
// Uses the key "foo1" to select the IFoo specifically
public class Consumer([FromKeyedServices("foo1")] IFoo foo)
{
    
}
```

## Dependency injection in ASP.NET Core

ASP.NET Core has dependency injection and IoC built in. The ``WebApplicationBuilder`` has its own ``ServiceCollection``, and the built app will have its own ``ServiceProvider``.

The framework will try to resolve endpoint parameters with the service provider.

```csharp
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<Foo>();

var app = builder.Build();

// Retrieve the ServiceProvider.
// var services = app.Services;

app.MapGet("/foo", (Foo foo) => foo.Name);

app.Run();
```

There is also a generic app builder, without ASP.NET Core. It has a similar interface, but for console apps, and services.

## Using Autofac as service container

With the introduction of keyed services, there is not as much of need to choose another service container - except for preferences. But the option to use, for instance, Autofac is there:

```c#
var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

// Register services directly with Autofac here. Don't
// call builder.Populate(), that happens in AutofacServiceProviderFactory.
builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new MyApplicationModule()));

var app = builder.Build();

public class MyApplicationModule : Module
{
  public bool ObeySpeedLimit { get; set; }

  protected override void Load(ContainerBuilder builder)
  {
    builder.Register(c => new Car(c.Resolve<IDriver>())).As<IVehicle>();

    if (ObeySpeedLimit)
      builder.RegisterType<SaneDriver>().As<IDriver>();
    else
      builder.RegisterType<CrazyDriver>().As<IDriver>();
  }
}
```

This will allow you to use the ``IServiceProvider``abstraction with Autofac.

## Conclusion

This was an introduction of dependency injection in .NET. 

Do you have any thoughts, or questions? Please, leave them below. :) 