---
title: Microservices - What are they?
published: 2021-09-01
tags: [Microservices, Distributed applications] 
---

Microservices is a hot topic in software development nowadays. Everybody wants to do it, but what is the point of it? 

And the most important question: How small should a micro service really be?

## What are Microservices?

Microservices are a response to building traditional monolithic Service-oriented applications (SOA) - addressing the problems in scalability and fault tolerance in distributed software systems.

It used to be that developers built services that had a host of functionality, running in one process on the server. That proved a challenge with both scalability and maintainability as the software grew. And having one service "doing everything" is not fault-proof. If one part of that service stops working, the rest might not run either.

### Common characteristics

Some characteristics for a microservice is that it is:

* Autonomous - it is own independent service.
* Independently deployable - without affecting other services.
* Owns its own data - only the service can directly access and modify the data.
* Scalable - scale horizontally with replicas of your services for redundancy when a service fails or is congested.

## Designing microservices

From a Software design perspective, you are breaking down a system into smaller services. Each service constitutes its own "Bounded context" with its own set of capabilities and behaviors - often defined by the domain it is addressing. For instance, a particular service might contain functionality for Sales, another for the Warehouse. You could divide it further. It depends on the requirements.

This will bring benefits to both the technical and organizational sides when developing software.

### Technical aspects

Some technical considerations when building microservices are:

* Communication between services via HTTP (Web API), RPC or Asynchronous messaging. Loose coupling is in most cases preferred by not addressing other services directly.

* Having separate databases - Although you can engineer it such a way that you partition a shared database.

* Running services in containers - Not a requirement but simplifies deployment.

A common mistake for engineers that are advocating microservices is to just see the technical aspects. After all, it is the latest fad in software development. But adopting the mindset rather than just the tools will bring so many benefits to the teams.

### Implications for an organization

Applying the microservice mindset to an organization - to one or more development teams - implies:

* One team is in charge of a microservice - or often, a set of related microservices.

* DevOps - the development team manages the life-cycle and deployment of the software that they build. 

This often implies CI/CD. Automatic deployment. The Cloud. Using tools such as Docker and Kubernetes. But that is not a requirement.

## Note on Monoliths

Just to say: Monoliths are not inherently bad. The challenges are in maintainability. You can maintain boundaries between parts of your code within a so called "loosely-coupled monolith"- all in one process - using the same techniques and tools that you use when building microservices - Reaping some of the benefits.

## Conclusion

It is not the size that makes a service a “microservice”, but rather how you build it with regards to the engineering concerns you are supposed to solve. 

Microservices are more of a mindset than an actual template for how to build software.
