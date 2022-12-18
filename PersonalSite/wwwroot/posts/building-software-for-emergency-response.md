---
title: Building software for emergency response
subtitle: How my software career started
published: 2022-10-06
tags: [Work experience]
---

I started my career back in 2014, as a part-time employee doing a project at the radio comm department at a small IT company in a small town in southern Skåne, Sweden. The company had developed a platform primarily for helping first responders (police, ambulance, firefighters). It consisted of the company's custom-made automation hardware which integrated with other technologies. Now they wanted a Windows application to allow for customers to interface with it and take advantage of all the functionality.

I still remember my first day. I appeared at the office, unknowing of what would happen, and met up with the technician who was soon-to-be my supervisor and colleague - the product owner. He was a bit confused because he had not expected me until the day after. As he was on his way out to do work for a customer, I got to follow him there. We went to a big fire station, serving a big area, were I got to see the platform in action. During that day he and I got to know each other. I will say that he came to have influence on me a lot. To me he was a well-meaning down-to-earth guy who wanted to understand the perspectives of other. Easy to talk to. A bit stubborn sometimes.

<a href="/assets/blis.png" target="_blank" alt="BLIS Messenger">
<img class="right" src="/assets/blis.png" /></a>

So work started on this application called "Messenger". At that time the scope was unclear to me. I was a newbie and the only developer working on this under the direction of they guy that I talked about, who was the de-facto product owner. The requirement was that it should be a Windows application, use .NET Framework, and run on Windows XP. I started building an app using [Windows Presentation Foundation](https://en.wikipedia.org/wiki/Windows_Presentation_Foundation) (WPF). The preferred pattern for building an app was [Model-View-ViewModel](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel) (MVVM), of which I, at the time, had limited knowledge of. I learned a lot from structuring the code and also focusing on the User Interface (UI) and broader User Experience (UX). Sure, the product owner had things to say, but it mostly a collaboration in which I had a lot of freedom. I built a feature for texting comm radios. This was done by entering what was analogous to a phone number. Another feature let the user execute functions that controlled hardware relays. What I essentially did was turning user interactions into commands for the hardware to consume.

The platform was (and still is) being used by first responders, and some industries, all around Sweden. I can't disclose a number, but they are in the hundreds. The main purpose is alarming and automating facilities - opening doors, sounding alarms at specific events etc. As I alluded to, it integrated with various equipment for radio communication, including sending text messages, to notify people when an alarm. The app that I built, "Messenger", could be added to this service.

During the development process the guy that I worked with continuously and thoroughly tested the app against the hardware - down to the every predictable problem. Due to the nature of the product it was important that the were no issues with the app itself. So I got instant feedback on bugs. We sometimes discovered bugs in the firmware that we reported to firmware engineer and got immediately fixed. The feedback loop was very fast. Everybody was dedicated to this project and helped each other. I really enjoyed being around these people.

What made this project so great was its focus on the product and that I was involved in the process of shaping it.

When "Messenger" finally had been released, I went on to build the program for programming the hardware. Believe it or not previously, the programming had been done through an Excel spreadsheet. We continued to improve "Messenger". We were adding some new major features, but not at the same rate.

But times changed. I joined the development department as a System Engineer, and focused on supporting other departments as well - Broadband and IT support. Then I eventually left the company. But my legacy still remains out there. Though the app, together with company, has been sold and is now owned by a different company.

What really makes me proud is that the software that I originally built is still out there saving the lives of people all around Sweden.

I still fondly remember my time at this company. The friendships that I formed with people both with and after this first project. It was for me forming years that affected me deeply as a person.