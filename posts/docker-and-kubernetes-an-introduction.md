---
title: Docker and Kubernetes
subtitle: An Introduction
published: 2021-09-01
image: /assets/container-ship.jpeg
tags: [Docker, Kubernetes, DevOps, Distributed applications]
---

*Updated on September 20th, 2021.*

There are two pieces of software that everybody is talking about right now: Docker and Kubernetes.

## Docker

Docker is a virtualization software that lets you build composable software parts, called "images", that then run in isolated compartments: "containers".

Imagine wanting a Web Server. Instead of manually installing and configuring Nginx ("Engine-X") in a Virtual Machine, you just tell Docker to download and run a pre-built image, and it is up and running in seconds. 

If you restart the container, it will start up clean again. And when you are done, you just delete the container, and there is no trace of ever having it on your system.

### How it works

Docker is virtualization at Operating System (OS) level. It runs on top of an existing OS kernel - usually the Linux kernel. The kernel is the lowest level of an operating system which handles the computer's resources. So instead of being virtualized as a full virtual machine, by a software called a *hypervisor*, each Docker container shares the kernel resources of the host operating system. Each container has its own separate filesystem, and houses their own OS distribution. This allows Docker containers to be lightweight, as opposed to traditional virtual machines.

Linux is not itself an operating system, but a kernel that is being used by multiple Linux distributions - such as Ubuntu and CentOS, which are the actual operating systems, each containing their own set of programs and desktop environments.

Docker containers run in one or more separate virtual networks that you cannot access by default. In order for your host to access a server running in a container you have to explicitly tell Docker to map the internal port in the container with an external one on the host system.

### Composability

Docker images are layered so you can build your own images with your software on top of existing base images. Use Nginx as a base, and then add the files for your site on top. You've got a new Docker image!

Now you can distribute the image, and others can easily run it without having to install any dependencies except Docker.

The Nginx image is based on an image of the lightweight Alpine Linux distribution. So that is how Docker put software in composable layers.

Using the Docker Compose (docker-compose) tool, you can orchestrate multiple containers. For instance, your web app running in one container, and a database server in another. The beauty is in that you describe your configuration in one file, and the Docker Compose tool essentially translates it into Docker commands. You simply tell Docker Compose to start everything.

### How it relates to Microservices

You have probably heard of Docker in the context of "microservices". A "microservice" is an independently deployable software service - as opposed to a big service monolith that is published all at once. A microservice might run in a container in Docker, and interact with other containers. The communication is done through HTTP, RPC - or via Asynchronous Messaging through a message queue like RabbitMQ, which also can run in a separate container.

An application that is distributed across multiple separate applications or services (or replicas), such as a microservice, is called a "Distributed application" or a "Distributed system".

## Kubernetes

If you want to run containers in a Production environment, then you should consider using Kubernetes.

Just like Docker Compose, Kubernetes is a software that orchestrates containers and how they communicate with each other, but it gives you production features. It enables running clusters on separate machines, replication and load balancing both within and across clusters, and it can even restart services in case a container is down. It also provides capabilities for monitoring and controlling how you are exposing services to the outside world.

## Conclusion

This was a short introduction to Docker and Kubernetes. I hope that this was useful in understanding these two products.