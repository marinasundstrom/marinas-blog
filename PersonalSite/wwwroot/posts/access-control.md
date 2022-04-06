---
title: "Project Highlight: Access Control"
published: 2021-11-16
image: /assets/accesscontrol.jpg
tags: [Projects, .NET, Web, Distributed applications]
---

This is a short presentation about one of my dearest personal projects. The projects combines my passion for programming with my interest in software design - and my desire to learn.

My main goal in this project was to learn more about building distributed applications - with some IoT. I also wanted to learn more about Microsoft Azure.

---

Back in 2017, I was looking for some fun project to do in my spare time - but I needed a cool use case. 

Since I was working as a consultant at [Axis](https://www.axis.com/) I had been exposed to the Internet of Things (IoT) while developing the on-device experiences. In particular, I had been working with their Access Control system. Its purpose is basically to guard physical access points, such as doors. Controlling the lock as well as a card reader. This was my inspiration for trying to build my own security system from scratch - using as Raspberry Pi and code written in .NET and C#.

<a href="/assets/accesscontrol.jpg">
<img class="mb-5" style="
  max-height: 300px; 
  max-width: 300px; 
  float: right;
  margin: 8px 0 0 20px;
  padding: 4px;" 
  src="/assets/accesscontrol.jpg" />
</a>

At the time, there were already multiple open-source home security projects for Raspberry Pi, but I did not intend to compete with them. Remember, I was doing this for fun.

My work started with getting some basic setup up and running. It started with wiring up sensors to Raspberry Pi to see if I could read some values. The first sensor was a magnet sensor. You align one part of the sensor on the door with another on the frame, and when he door opens the circuit is broken and current flows. Soon after, I introduced a RFID Card Reader.

Based on my experience I had an idea of how to design my software. I had researched the domain and its terminology. Access Point is one such core concept. I enjoy learning about the domains that I work with. I think that domain knowledge it is important in order to build good software. You have to understand your software, what is expected from the users perspective - having a shared ubiquitous language.

From there I started to implement the basic logic, and the communication flow between the AccessPoint (Raspberry Pi) and my AppService. In Blazor, I built a UI for controlling the system, and to follow the access events in real time (“Access Log”). From the UI you could lock/unlock and arm/disarm the Access Point. You could also configure the access time. Data such as authorized cards and past events was stored into a database owned by the AppService.

The AccessPoint service, running on the Raspberry Pi, was connected to Azure IoT Hub, and communicated with AppService using Notification Hub and Event Hub.

I also built a mobile app using Xamarin.Forms. The functionality mirrored that of the Web UI. You could sign in and do essentially the same things, even configuring the access point.

Configuring the environment for running the Access Point was not easy at the time. You had to run the services locally. There were no scripts for setting it up.

## Open Sourcing

Back in 2019, I dusted off the [code](https://github.com/robertsundstrom/AccessControl) and made it public on Github. I documented everything. There are details on how to build the setup and how to configure the services to run on Azure.

I published this [video](https://www.youtube.com/watch?v=VlSKTeJASYc) that shows the system in action.

<iframe width="560" height="315" src="https://www.youtube.com/embed/VlSKTeJASYc" title="YouTube video player" frameborder="0" allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture" allowfullscreen></iframe>

## Recent changes

In early 2021, I upgraded AccessControl to .NET 5  and did some major re-architecting. That primarily meant that I restructured the code according to Clean Architecture guidelines. This was to make it easier to follow the flow and easier to make changes. I also introduced Tye, which is a development-time orchestrator that manages Docker containers, much like Docker Compose does. This makes it easier than before to get all parts of the app running, with no manual configuration.

In its current state, Access Control could be considered having a microservice architecture. I sure have learned a lot from this project, and I have learned even more since. 

I like revisiting old projects and make them better. My next step for Access Control would be to decouple it from Azure by introducing an abstraction such as [MassTransit](https://masstransit-project.com). I love that framework so much since it makes asynchronous messaging between microservices so much easier.

I'm also considering archiving this project soon.

As mentioned, the code is open source at GitHub. [Have a look](https://github.com/robertsundstrom/AccessControl)!