---
title: Building distributed apps in .NET with MassTransit
published: 2023-11-10
tags: [C#, .NET, Distributed apps, Microservices, MassTransit, RabbitMQ]
---

## Introduction

So many apps that are being built today are part of distributed systems made up of two or more services that communicate with each other. They do so by sending messages, or publishing events. They are so called "Event-driven architectures". 

Learning everything that enables this, the technologies and the patterns, might be cumbersome, in particular if you don't know where to start.

The goal of this article is to give you a starting point from which you can continue exploring building distributed applications in C#/.NET.

In this article will be talking about MassTransit, what it is about, and viewing some examples of how to use it. 

We will be using RabbitMQ as our transport. It is easy to get started with using Docker. The contents of the Docker Compose files will be provided.

## Contents

Here is what is covered in this article:

1. <a href="/articles/building-distributed-apps-with-masstransit#what-is-a-distributed-app">What is a distributed app?</a>
2. <a href="/articles/building-distributed-apps-with-masstransit#what-is-a-distributed-app">What is asynchronous messaging?</a>
3. <a href="/articles/building-distributed-apps-with-masstransit#what-is-masstransit">What is MassTransit?</a>
4. <a href="/articles/building-distributed-apps-with-masstransit#getting-started">Getting started</a>
    1. <a href="/articles/building-distributed-apps-with-masstransit#the-app">The app</a>
5. <a href="/articles/building-distributed-apps-with-masstransit#basic-concepts">Basic concepts</a>
6. <a href="/articles/building-distributed-apps-with-masstransit#publishing-and-consuming-a-message">Publishing and consuming a message</a>
    1. <a href="/articles/building-distributed-apps-with-masstransit#message">Message</a>
    2. <a href="/articles/building-distributed-apps-with-masstransit#consumer">Consumer</a> 
    3. <a href="/articles/building-distributed-apps-with-masstransit#publisher">Publisher</a>
    4. <a href="/articles/building-distributed-apps-with-masstransit#running-the-app">Running the app</a>
7. <a href="/articles/building-distributed-apps-with-masstransit#publishing-message-from-consumer">Publishing message from consumer</a>
8. <a href="/articles/building-distributed-apps-with-masstransit#remote-procedure-call-rpc">Remote procedure call (RPC)</a>
9. <a href="/articles/building-distributed-apps-with-masstransit#configuring-endpoints-manually">Configuring endpoints manually</a>
10. <a href="/articles/building-distributed-apps-with-masstransit#error-handling-and-retry">Error handling and retry</a>
11. <a href="/articles/building-distributed-apps-with-masstransit#consuming-faults">Consuming faults</a>
12. <a href="/articles/building-distributed-apps-with-masstransit#state-machines-and-sagas">State machines and Sagas</a>
13. <a href="/articles/building-distributed-apps-with-masstransit#transactional-outbox-pattern">Transactional Outbox pattern</a>
14. <a href="/articles/building-distributed-apps-with-masstransit#scaling-vertically">Scaling vertically</a>
15. <a href="/articles/building-distributed-apps-with-masstransit#testing-in-masstransit">Testing in MassTransit</a>
16. <a href="/articles/building-distributed-apps-with-masstransit#distributed-tracing">Distributed tracing</a>
17. <a href="/articles/building-distributed-apps-with-masstransit#using-another-transport">Using another transport</a>
18. <a href="/articles/building-distributed-apps-with-masstransit#conclusion">Conclusion</a>

## What is a distributed app?

A distributed app is an application that consists of multiple services that work together. Hence the app is distributed across those multiple services.

Does it sound familiar? Yep, Microservices.

We need ways for these services to communicate - should we use HTTP or gRPC? But those create coupling between services? There is another option: Asynchronous messaging with brokers and queues. But there are so many types of brokers and APIs to learn to master. It takes time to learn the specifics of each.

That is where MassTransit comes in.

## What is asynchronous messaging?

First we should explain what asynchronous communication is about.

Asynchronous messaging is a communication method where participants on both sides are allowed to pause and resume conversations on their own terms. The party that is sending a message does not have to wait for a connection and a response.

This is something entirely different from asynchronous code execution in ``await`` in C#.

Asynchronous messaging enables us the implement event-driven patterns, including event sourcing, within a system of multiple services that depend on each other.

There are multiple technologies that each allow for asynchronous messaging. Each with its own concepts. MassTransit supports many of the popular ones, both those that can run on-premise and those that are exclusively hosted in the cloud.

We will use RabbitMQ, which is a queue-based system. It can be explained as we are sending messages to exchanges in a post office, that then distribute the messages (copies) across dedicated postboxes, or queues, that receivers consume.

MassTransit will abstract all these details related to a "transport" away, but you should know about them.

## What is MassTransit?

[MassTransit](https://masstransit.io/) is a modern open-source framework that facilitates asynchronous messaging between services. Some would call it a service bus. It certainly can be used that way, but is not limited to that. The main selling point is that you can build [loosely coupled](https://en.wikipedia.org/wiki/Loose_coupling) systems with little effort.

The framework provides a solid unified abstraction of producers and consumers, on top of transports such as RabbitMQ, Azure ServiceBus, Amazon SQS etc. In addition to that, it also come with functionality for error handling, retries, and scaling. There even is an experimental mode for using SQL Server for sending messages, instead of a message bus.

MassTransit is easy to get started with, since it requires just a few lines of code. And it is based on dependency injection.

I recommend you to look into their excellent [documentation](https://masstransit.io/documentation/concepts), as well as awesome videos produced by its creator and maintainer, [Chris Patterson](https://www.youtube.com/c/phatboyg) (@PhatBoyG).

Some of the samples have been put together specifically for this article, while some have been taken from MassTransit's own docs.

## Getting started

Run this, to add the latest version of MassTransit to your application: (or an empty ASP.NET Core Web app: ``dotnet new web``)

```sh
dotnet add package MassTransit.RabbitMQ
```

This will add MassTransit with RabbitMQ support to your project.

### The app

```csharp
using MassTransit;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    x.AddConsumers(typeof(Program).Assembly);

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host("localhost", "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ConfigureEndpoints(context);
    });
});

var app = builder.Build();

app.MapGet("/hello", () => "Hello, World!");

app.Run();
```

Now we need a RabbitMQ instance to run against.

Here is a Docker Compose file that runs a container in Docker:

```yaml
version: '3.4'
name: masstransit-test

services:
  rabbitmq:
    image: rabbitmq:management
    container_name: rabbitmq
    hostname: rabbitmq
    ports:
      - 5672:5672
      - 15672:15672
    environment:
      - RABBITMQ_ERLANG_COOKIE=SomeRandomStringHere
    volumes:
      - "./data/rabbitmq:/var/lib/rabbitmq"
```

Provided that you have Docker Desktop installed, run this to start RabbitMQ:

```
docker compose up -d
```

But if you run the app, it doesn't do anything yet. We need to add some producers and consumers.

## Basic concepts

There are three central concepts to know about:

* **Message** - The message (or data) being sent. We also talk about a _contract_.
* **Producer** - Produces messages, i.e. publishes, or sends them.
* **Consumer** - Consumes messages by performing some logic. It's a message handler.

Then there is the concept of a **Transport**, which is the means by which messages are being sent. In our case it is RabbitMQ, but there are others as mentioned, including Azure Service Bus.

A message do often take on the meaning of an _event_ that is signifying that something has happened - as reflected in the name of the message, for example, ``OrderPlaced``.

An event can either be "thin" - meaning it just contains details regarding what the event was concerned, or it might also contain state. The latter is referred to as _Event-carried state_ transfer. There are many opinions on what road to go when designing events.

## Publishing and consuming a message

The most basic thing you can do is to publish a message which can be consumed by one or more consumers. So let's add some things to our app that enable us to do just that.

### Message

We first need to define a contract (message type) for our message.

The message contract is a record with properties that have explicit ``init`` setters. This enables for the properties to be deserialized. (Records don't have public setters by default)

```csharp
public record SendMessage
{
    public required string Text { get; init; }
}
```

It is worth noting that messages, by convention, are identified by the full name of the type (namespace + type). That is unless you configure the binding manually. Coincidentally, you can redefine a message in another assembly as long as all instances have the same name, and are in the same namespace.

Message contracts are usually shared via common class libraries. But you may declare them if you prefer. Just make sure that they follow the rules mentioned above,

### Consumer

The consumer that consumes the message looks like this:

```csharp
namespace WebApp;

public sealed class SendMessageConsumer : IConsumer<SendMessage>
{
    public async Task Consume(ConsumeContext<SendMessage> context)
    {
        Console.WriteLine($"The message is: {context.Message.Text}");
    }
}
```

So how does MassTransit know about this consumer, and the message?

If we look at the code for our Web app, ``x.AddConsumers(typeof(Program).Assembly);`` will discover and register the consumers in the executing assembly - looking for classes that implement ``IConsumer<T>``. And ``cfg.ConfigureEndpoints(context);`` will configure the RabbitMQ endpoints for us.

### Publisher

What is now needed is somewhere to publish the message from, so we do it from an API endpoint. We inject and use the ``IPublishEndpoint`` to publish messages:

```csharp
app.MapPost("/test", static async (string text, IPublishEndpoint publishEndpoint) => await publishEndpoint.Publish(new SendMessage { Text = text }))
    .WithName("WebApp_Test")
    .WithTags("WebApp");
```

The ``IPublishEndpoint`` is a scoped service.

### Running the app

Here is a ``.http`` file that can be used to test the endpoint in Visual Studio, and VS Code:

```
@WebApp_HostAddress = http://localhost:<YOUR-PORT>

POST {{WebApp_HostAddress}}/test
Accept: application/json

"Hello, World!"

###
```

You can use this unless you want to setup Open API and test it in Swagger UI.

When you send that POST request to the endpoint, the message will be published to a queue in RabbitMQ. The consumer in the same app will receive the message, and write ``"The message is: Hello, World!"`` to the console.

### RabbitMQ Management plugin

This is article is not about RabbitMQ, but you should know about this.

RabbitMQ has a Management plugin that provides a useful Web UI. In it you can view your exchanges and queues in real-time.

In your browser, navigate to: [http://localhost:15672/](http://localhost:15672/)

Provided that you are running the app, the UI will display at least one connection, an exchange, and a queues. Available to you are statistics and the current state of the queues. You can also publish messages manually from here.

On a side note: MassTransit will clean-up resources in RabbitMQ when an app stops.

## Where are the other services?

But... wait! Isn't this supposed to show how to build a distributed app? We need another service.

That's easy to fix! Just copy the project! 😊 

## Publishing message from consumer

When publishing a message from a consumer, you don't need to inject any extra service. The ``ConsumeContext`` itself provides the functionality for communicating with the bus. 

This is what a modified version of our consumer would look like:

```csharp
using MassTransit;

namespace WebApp;

public sealed class SendMessageConsumer : IConsumer<SendMessage>
{
    public async Task Consume(ConsumeContext<SendMessage> context)
    {
        Console.WriteLine($"The message is: {context.Message.Text}");

        await context.Publish(new MessageSent {
            Date = DateTimeOffset.UtcNow
        });
    }
}
```

In the other project you will define this consumer for the ``MessageSent`` message:

```csharp
using MassTransit;

namespace WebApp;

public sealed class MessageSentConsumer : IConsumer<MessageSent>
{
    public async Task Consume(ConsumeContext<MessageSent> context)
    {
        Console.WriteLine($"The time was: {context.Message.Date}");
    }
}
```

Btw, here is the contract:

```csharp
public record MessageSent
{
    public required DateTimeOffset Date { get; init; }
}
```

## Remote procedure call (RPC)

You can do remote procedure calls (RPC), meaning that you send a request, and await a response back. Not unlike HTTP requests and responses, but still loosely coupled because of the inherent asynchrony of RabbitMQ as a transport.

You can obtain a client by injecting the ``IRequestClient<T>`` interface, where ``T`` is the message type. Then use the async ``GetResponse<TResponse>`` method to send a request and await the response.

```csharp
using MassTransit;

namespace WebApp;

public sealed class GetTodosConsumer(ITodoService todoService) : IConsumer<GetTodos>
{
    public async Task Consume(ConsumeContext<GetTodos> context)
    {
        var todos = await todoService.GetTodosAsync(context.CancellationToken);

        consumeContext.RespondAsync<GetTodosResponse>(new GetTodosResponse {
            Todos = todos
        });
    }
}
```

This sample is not complete. The ``ITodoService`` class has been omitted as it is not relevant for this example.

The contracts:

```csharp
public record Todo
{
    public required string Text { get; init; }

    public required bool Done { get; init; }
}

public record GetTodos();

public record GetTodosResponse
{
    public required IEnumerable<Todo> Todos { get; init; }
}
```

Here is the how you use the request client:

```csharp
app.MapGet("/todos", static async (IRequestClient<GetTodos> requestClient) => {
    var response = await requestClient.GetResponse<GetTodosResponse>(new GetTodos());
    return response.Todos;
}
.WithName("WebApp_GetTodos")
.WithTags("WebApp");
```

### Handling multiple response types

A RPC request might have multiple responds with different response types. This is how you would handle these responses from the client:

```csharp
var response = await client.GetResponse<OrderStatusResult, OrderNotFound>(new { OrderId = id});

if (response.Is(out Response<OrderStatusResult> responseA))
{
    // do something with the order
}
else if (response.Is(out Response<OrderNotFound> responseB))
{
    // the order was not found
}
```

This is not a complete sample though.

## Configuring endpoints manually

In the samples so far, endpoints have been configured automatically. But you can do it manually for the chosen transport.

Here we have declare a ``ReceiveEndpoint`` which maps to the queue "submit-order". (Spoilers) It has retries configured. The endpoint is handled by the consumer ``SubmitOrderConsumer``.

```csharp
services.AddMassTransit(x =>
{
    x.AddConsumer<SubmitOrderConsumer>();

    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.ReceiveEndpoint("submit-order", e =>
        {
            e.UseMessageRetry(r => r.Immediate(5));

            e.ConfigureConsumer<SubmitOrderConsumer>(context);
        });
    });
```


## Error handling and Retry

It sometimes happens that a consumer fails for some reason, an exception is being thrown. For instance, because of the database failing. In that case you would want to retry running the consumer. 

```csharp
public class SubmitOrderConsumer :
    IConsumer<SubmitOrder>
{
    ISessionFactory _sessionFactory;

    public async Task Consume(ConsumeContext<SubmitOrder> context)
    {
        using(var session = _sessionFactory.OpenSession())
        using(var transaction = session.BeginTransaction())
        {
            var customer = session.Get<Customer>(context.Message.CustomerId);

            // continue with order processing

            transaction.Commit();
        }
    }
}
```

Retries can be configured like so:

```csharp
services.AddMassTransit(x =>
{
    x.AddConsumer<SubmitOrderConsumer>();

    x.UsingRabbitMq((context,cfg) =>
    {
        cfg.UseMessageRetry(r => r.Immediate(5));

        cfg.ConfigureEndpoints(context);
    });
});
```

This will apply to all consumers. But you can apply them to specific ones as well.

You can even use exception filters to make MassTransit just handle certain exception types.

```csharp
e.UseMessageRetry(r => 
{
    r.Handle<ArgumentNullException>();
    r.Ignore(typeof(InvalidOperationException), typeof(InvalidCastException));
    r.Ignore<ArgumentException>(t => t.ParamName == "orderTotal");
});
```

## Consuming faults

When a consumer has truly failed, you perhaps want to handle that fault. You do that by creating a consumer that consumes a message of ```Fault<T>``` where ``T`` is the message type.

```csharp
public class DashboardFaultConsumer :
    IConsumer<Fault<SubmitOrder>>
{
    public async Task Consume(ConsumeContext<Fault<SubmitOrder>> context)
    {
        // update the dashboard
    }
}
```

For RabbitMQ, MassTransit publishes faulted messages to consumer specific error queues. In the process, a message gets wrapped by the fault information as represented by ``Fault<T>``. Imagine something similar happening when using another transport.

## State machines and Sagas

MassTransit has its own state machine API, called _Automatonymous_, which allows you to define the state machines, with states and transitions in between, driven by the messages. The library can even be used standalone outside of MassTransit.

Automatonymous makes it easier to implement the _Saga design pattern_. A _saga_ is a way to ensure data consistency across multiple services within a transaction. 

There are two approaches to implementing sagas: _choreography_ and _orchestration_. Both patterns are used to coordinate messages. Choreography is about each service knowing how to handle those states from the messages it receives. On the other hand, the orchestration pattern has a centralized point which orchestrates all messages sent between services. This could imply that a separate orchestrator service is running the state machine. Which approach to choose depends on the case.

This is an incomplete example ([source](https://masstransit.io/documentation/patterns/saga/state-machine)) of what a state machine for an orchestrator looks like in MassTransit. It demonstrates the concepts of events and states within that state machine.

```csharp
public interface SubmitOrder
{
    Guid OrderId { get; }

    DateTime OrderDate { get; }
}

public interface OrderAccepted
{
    Guid OrderId { get; }    
}

public class OrderState :
    SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }

    public DateTime? OrderDate { get; set; }
}

public interface OrderSubmitted
{
    Guid OrderId { get; }    
}

public class OrderSubmittedEvent :
    OrderSubmitted
{
    public OrderSubmittedEvent(Guid orderId)
    {
        OrderId = orderId;
    }

    public Guid OrderId { get; }    
}

public class OrderStateMachine :
    MassTransitStateMachine<OrderState>
{
    public OrderStateMachine()
    {
        Event(() => SubmitOrder, x => x.CorrelateById(context => context.Message.OrderId));

        Event(() => OrderAccepted, x => x.CorrelateById(context => context.Message.OrderId));

        Initially(
            When(SubmitOrder)
                .Then(x => x.Saga.OrderDate = x.Message.OrderDate)
                .Publish(context => (OrderSubmitted)new OrderSubmittedEvent(context.Saga.CorrelationId))
                .TransitionTo(Submitted),
            When(OrderAccepted)
                .TransitionTo(Accepted));

        During(Submitted,
            When(OrderAccepted)
                .TransitionTo(Accepted));

        During(Accepted,
            When(SubmitOrder)
                .Then(x => x.Saga.OrderDate = x.Message.OrderDate));
    }

    public State Submitted { get; private set; }
    
    public State Accepted { get; private set; }

    public Event<SubmitOrder> SubmitOrder { get; private set; }

    public Event<OrderAccepted> OrderAccepted { get; private set; }
}
```

Each service could have its own state machine, if your decide to do choreography. In the end, it is about where the state machine logic exists. Whether, or not, you desire to centralize the logic in an orchestrator.

## Transactional Outbox pattern

The transactional outbox pattern solves the issue of consistency when both saving data and emitting events. You might want data to be saved to the database before publishing the events.

It stores the messages in an outbox (in a database) in the same transaction as the data. If the save succeeds, the messages will be published by a periodic process that is checking the outbox in a specific interval.

If you have domain events then this patterns is pretty much a requirement.

## Scaling vertically

Using MassTransit you can easily scale services vertically. 

Using a queue-based transport, having multiple instances (or replicas) of the same app, consuming the same queues, will cause the instances to compete for messages. This is how you get both scaling, and load balancing for free.

This will work for other transports, even if they are not necessarily using message queues.

## Testing in MassTransit

MassTransit provides a ``TestHarness`` that is an in-memory implementation of a transport specifically built for testing. Using that you can verify that messages have been published and consumed.

Consider this unit test case (NUnit):

```csharp
[Test] 
public async Task An_example_unit_test() 
{
    await using var provider = new ServiceCollection()
        .AddYourBusinessServices() // register all of your normal business services
        .AddMassTransitTestHarness(x =>
        {
            x.AddConsumer<SubmitOrderConsumer>();
        })
        .BuildServiceProvider(true);

    var harness = provider.GetRequiredService<ITestHarness>();

    await harness.Start();

    var client = harness.GetRequestClient<SubmitOrder>();

    var response = await client.GetResponse<OrderSubmitted>(new
    {
        OrderId = InVar.Id,
        OrderNumber = "123"
    });

    Assert.IsTrue(await harness.Sent.Any<OrderSubmitted>());

    Assert.IsTrue(await harness.Consumed.Any<SubmitOrder>());

    var consumerHarness = harness.GetConsumerHarness<SubmitOrderConsumer>();

    Assert.That(await consumerHarness.Consumed.Any<SubmitOrder>());

    // test side effects of the SubmitOrderConsumer here
}
```

## Distributed tracing

Building distributed apps involves challenges when doing tracing and debugging communication between services - what services are being hit during the course of a request. 

Distributed tracing alleviates that headache by collecting and putting this information together, so that it can be easily visualized in a tool like Jaeger, or Zipkin. You can easily see latencies, and get information about specific errors.

MassTransit has built in support for tracing with the OpenTelemetry standard.

To add tracing to your project, add these packages:

```xml
<PackageReference Include="OpenTelemetry.Exporter.Console" Version="1.3.2" />
<PackageReference Include="OpenTelemetry.Extensions.Hosting" Version="1.0.0-rc9.6" />
<PackageReference Include="OpenTelemetry.Instrumentation.AspNetCore" Version="1.0.0-rc9.6" />
<PackageReference Include="OpenTelemetry.Instrumentation.Http" Version="1.0.0-rc9.6" />
<PackageReference Include="OpenTelemetry.Exporter.Zipkin" Version="1.4.0-alpha.2" />
<PackageReference Include="OpenTelemetry.Instrumentation.MassTransit" Version="1.0.0-beta.3" /
```

Add tracing to the service container:

```csharp
app.Services.AddOpenTelemetry()
    .WithTracing(tracing =>
    {
        tracing.SetResourceBuilder(resource)
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .AddMassTransitInstrumentation()
                .AddSource("MassTransit")
                .AddZipkinExporter(zipkin =>
                {
                    var zipkinUrl = "http://localhost:9411";
                    zipkin.Endpoint = new Uri($"{zipkinUrl}/api/v2/spans");
                });
        //.AddConsoleExporter();
    });
```

Here is the Docker Compose:

```yaml
version: '3.4'
name: masstransit-test

services:
  rabbitmq:
    image: rabbitmq:management
    container_name: rabbitmq
    hostname: rabbitmq
    ports:
      - 5672:5672
      - 15672:15672
    environment:
      - RABBITMQ_ERLANG_COOKIE=SomeRandomStringHere
    volumes:
      - "./data/rabbitmq:/var/lib/rabbitmq"

  zipkin:
    image: openzipkin/zipkin
    container_name: zipkin
    ports:
      - 9411:9411
```

Run it:

```
docker compose up -d
```

You can now open your browser, and navigate to: [http://localhost:9411/zipkin/](http://localhost:9411/zipkin/)

There you can query the requests and follow the flow of operations, including where messages are being passed.

Beyond this, you can add additional packages that provide metrics for your applications, which can be viewed in tools like Grafana. But that is not in scope for this article.

## Using another transport

So what if you want to use another transport? You might want to run your app in the cloud using Azure Service Bus.

It is easy to just switch out RabbitMQ for something else, as it is just a matter of installing the provider and declaring what provider to use. Then MassTransit will configure the rest for you.

For Azure Service Bus you add this to your project file

```xml
<PackageReference Include="MassTransit.Azure.ServiceBus.Core" Version="8.1.1" />
```

Then configure like so:

```csharp
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    x.AddConsumers(typeof(Program).Assembly);

    x.UsingAzureServiceBus((context, cfg) =>
    {
        cfg.Host($"sb://{builder.Configuration["Azure:ServiceBus:Namespace"]}.servicebus.windows.net");

        cfg.ConfigureEndpoints(context);
    });
});
```

This article is not about how you configure Azure Service Bus.

### Using different transports depending on environment

This piece of code shows you how to configure MassTransit to use RabbitMQ locally, but Azure Service Bus when in production:

```csharp
builder.Services.AddMassTransit(x =>
{
    x.SetKebabCaseEndpointNameFormatter();

    x.AddConsumers(typeof(Program).Assembly);

    if (builder.Environment.IsProduction())
    {
        x.UsingAzureServiceBus((context, cfg) =>
        {
            cfg.Host($"sb://{builder.Configuration["Azure:ServiceBus:Namespace"]}.servicebus.windows.net");

            cfg.ConfigureEndpoints(context);
        });
    }
    else
    {
        x.UsingRabbitMq((context, cfg) =>
        {
            var rabbitmqHost = builder.Configuration["RABBITMQ_HOST"] ?? "localhost";

            cfg.Host(rabbitmqHost, "/", h =>
            {
                h.Username("guest");
                h.Password("guest");
            });

            cfg.ConfigureEndpoints(context);
        });
    }
});
```

## Conclusion

test