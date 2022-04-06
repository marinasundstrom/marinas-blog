---
title: Scaling a Monolith - without Microservices
published: 2021-09-06
tags: [Software engineering, Distributed applications]
---

One myth is that you need to "do" microservices in order to scale your service applications horizontally. This has prompted a lot of people to rewrite their existing monoliths into microservices.

Don't get me wrong. There are benefits in microservices, but you need to understand the problems that the tools address before you start to use them - especially in an existing project. The cost of changing existing code might outrun the potential benefits in terms of the technical challenges and development time.

A monolith implies that there is a single codebase.

As a first step, when you want to scale an existing monolith, you should start by thinking about decoupling your functionality. It is totally possible to scale a monolith horizontally without dividing it into microservices.

## The Scenario

Let us assume that you have an existing monolithic Service-oriented application (SOA) which handles a lot of stuff. It spans multiple domains in the field of a Mobile service provider: From device provisioning, subscriptions, and handing of payments. Allt the data is stored in a single database.

### Goals

The goals that you have are:

* You want to be able to change, version, and deploy individual parts of the software without interrupting the whole system.
* You want to scale services horizontally, add load balancing, and to ensure fault tolerance in a production environment. * If one services is busy or goes down, you want a replica to step in.

This puts some requirements in the way that we build software. But it does not automatically imply microservices.

### Bounded Contexts

The first thing you learn about microservices is that each "micro" service should have its own Bounded context, a fancy word for a determined boundary within a domain. For example, payments could be considered its own Bounded context. It is generally up to the Domain experts to identify these boundaries. Often in smaller projects, in which there are no designer or architect roles, developers are left to design and to organize code. Arguably, this is how we ended up with monoliths in the first place. Programmers get instructions and just write code - without a strategy.

In the case of our example above, often you see that the Provisioning part of the service, might directly call into Subscription, which then calls Payments. This is introducing coupling and complexity.

## The Remedy

### Decoupling

With the right mindset you can (re)write an application as a set of loosely-coupled functionality within one single codebase. That means that Subscriptions does not really have to know about, for instance, Payments. It knows that there is an interface for the Payment service but it does not have access the implementation.

By building your project around sending Messages - like Commands and Events, you can make your single codebase more maintainable.

Picture the the application like this: Subscription sends a CreateInvoice command/request. Provisioning listens for a SubscriptionChanged event. Who handles the commands or events is irrelevant to the producer - hence it is decoupled.

### Project boundaries

If you have not already, you can split the parts of the application (your services) into separate projects, while still referencing them from the main project. That will create a boundary between, for instance, Provision and Subscriptions, which can not directly access each ones implementations. 

How would you access functionality across these project boundaries?

You put all your service contracts, interfaces, Data Transfer Objects (DTOs), Commands, etc in another project. That project is in its turn referenced by the consuming service. The contracts can then be used when wiring up Dependency Injection or using a Message Bus.

### Database

Another trait of microservices is that a service own its own data - meaning that only that service can directly insert into or update the database. In other words, the bounded context applies even here. In a monolith database it is totally possible to maintain boundaries using something like schemas. You can also enforce the boundaries at code level via a well defined Data Access layer.

On top of this, you can scale the database itself using replication.

### Asynchronous Messaging

Regardless whether you have a microservice or a monolith, you can off-load internal communication between context by using out-off-process messaging, such as a message queue, like RabbitMQ. This way you can orchestrate messages across multiple consumers. This goes hand-in-hand with what was mentioned about "messaging" before.

Example: Subscription will issue a CreateInvoice command/message to the queue, and Billing will consume (or handle) the message once available.

Remember that a message is going to be handled by the first consumer that is available - be it a service, or an instance of a service.

### Scaling horizontally

To scale the functionality horizontally, you can run replicas of the service applications that are being fronted by a load balancers that redirects requests to any of the replicas that are available.

To repeat, a microservice is based around a single bounded context. A monolith code base usually contains multiple contexts. If you logically separate them, you can isolate the functionality, to be able to turn it on and off. Then you can dedicate a certain instance to handling the bounded context for Subscription etc. You just disable the functionality of the other contexts in that particular instance,

If you use asynchronous out-of-process messaging you can communicate with other instances. So you can invoke functionality that is enabled in other instances through messages. even if that is turned off in the current instance. If there are replicas, the message will be handled by the first instance available.

This is how you achieve scaling in a monolithic application.

## Conclusion

Keep in mind! Evolving an existing system towards microservices is a step by step process. And once you go from a monolith to having a set of microservices, you actually have a system. You have decoupled your application, increased maintainabilty of the components, but also increased complexity.

Sometimes it is, in fact, easier to maintain a monolithic codebase. What makes a difference is how you design your application.