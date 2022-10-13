---
title: Why is C# and .NET still so underrated?
subtitle: 
published: 2022-10-12
tags: [.NET]
---

It is sad that .NET (pronounced: dotNET) often is forgotten when other modern developer platforms are mentioned. If you look around on the Internet there are still people claiming it is either exclusively for Windows only, or that is just good for very few scenarios. But these claims could not be further from the truth.

The .NET Platform is a modern and evolving software development platform, and C# is a modern programming language. The runtime is cross platform, fully supported by Microsoft, and open-source. There is an entire community of people contributing to the ecosystem. It is not just for building Windows apps. You can build modern Web APIs with ASP.NET Core and component-based UIs in C# with Blazor. Besides targeting Windows, there are also first-class bindings for other app platforms allowing you to build native apps for iOS, macOS, and Android - mobile development. And with .NET Maui you can build one app with one code base and one UI for all those platforms. Even IoT on Raspberry Pi is within your reach. Even native compilation is possible for those who want to optimize the performance of their apps.

Here is a short recap of the history of .NET:

The .NET (Framework) was launched in 2001 as Microsoft’s response to Java. Providing a managed execution environment with automatic memory management using a garbage collector. The main programming language was C#, which improved on the Java language. Back then .NET was proprietary closed-source and only supported on Windows, although it was designed to be platform independent. In fact, there was the successful open-source Mono project that implemented .NET for Linux and Mac OS X. We should not ignore the issues between them and Microsoft. But that is another story. In 2016, Microsoft started building an open-source cross-platform .NET runtime. The ecosystem started to unify. Flash-forward to 2022, .NET is now fully open-source and Mono has been integrated into the .NET runtime to enable building apps for mobile phones and in the browser via WebAssembly. All the development and discussions are held in the open at GitHub.

So who chooses the .NET platform?

My experience is that companies tend to favor .NET because it is backed by Microsoft and thus integrates well with all of Microsoft’s other products that they probably are already using. It is the proof of the strength of the brand. Many companies use Share Point, and the way to program it is using C#. So .NET becomes a natural choice when standardizing their tech stack. That together with partnership programs that Microsoft provide makes the package attractive, especially for business that can afford it.

At companies, where software has a supporting function, there is a lot of existing systems and legacy code still running on .NET Framework. As they are not primarily software companies, keeping that code up-to-date has not been prioritized. But as the digitalization has become the new normal, these companies have come to spend more and more on maintaining and further developing their software. Upgrading is inevitable. Especially, if they require integrating it with modern business-critical systems, for example, accounting software.

Because of all these custom-built systems that have existed for 10+ year, .NET has got a bad reputation of promoting convoluted code bases. Traditionally, like Java, C# has been very heavy on patterns - whatever software pattern was recommended by Microsoft at the time. Developers have also been rotating, especially consultants. Code changing with every project and new group of people working on it, adding complexity. Combine this with the pressure to deliver. Because of this, code eventually acquires so much technical debt and becomes hard if not impossible to work with. That is the nature of software development. Sooner or later they realize that they have to rebuild and modernize their systems. And that is a big investment to make.

.NET has evolved a lot since the platform started to open-source more than 6 years ago. As the other competing programming language gained in popularity, they influenced .NET developers who started to care more about maintaining and evolving code. Developers started sharing their knowledge and experience building software in a simple blog post. Thus the community was established. Microsoft is no longer the ultimate authority on how to best structure your code, though under their stewardship, they still lead and set the standards.

So why, it seems, are there not as many companies building new software from scratch using .NET and C#? 

Simply because it is not the most popular stack. If you are coming from a non-Microsoft Dev Platform background, JavaScript, PHP, and Golang is far more appealing for developing on the backend. From a business standpoint it might be wiser to choose NodeJS since there are more JavaScript developers and resources available. When it comes to picking React, or Angular, before Blazor it is simply bias towards what most other projects use. Some do note every little argument for not picking this or that unproven tech as a risk. Again, you know that there will still be both JavaScript and React developers in the foreseeable future. Your stack is hopefully secured.

In the end, the choice of technology is mostly a preference that is determined by what you (or your organization) feels most comfortable and productive with. For me it happens to be .NET - but I know JavaScript fairly well, and I have good knowledge of React. Taking the leap to try a new technology is not that easy. While programming languages are pretty similar, you often have to learn a new toolset and ecosystem. It takes time to get used to and become productive. But a full-stack developer working on a backend written in .NET while using Angular for frontend might be more inclined to try Blazor since it is .NET, and it integrates well with the backend. Though, they'd be less inclined to switch to NodeJS and JavaScript on the backend.

Despite this worry of .NET not being popular enough, I see a lot of companies and projects that have chosen .NET as their platform. They are not just the super big companies that most other developers looks up to, not Facebook or Google, of course. These corporations are in the service sector and don't show off what they are using since it is not a big deal to them.

What has been lacking in the .NET ecosystem is a modern full stack experience for the Web. This has arguably affected the attractiveness of the platform. Thankfully, now we have ASP.NET Core and Blazor. And I do think that as .NET on WebAssemby is improving in perfomance, we will see more developers start to show interest in Blazor running in the browser as an alternative to JS-based React and Angular. In particular, if they are already invested in .NET on the backend. They can simply use familiar tools and packages directly. 

Another complaint about .NET has for a long time been not supporting building Desktop apps for platforms other than Windows, but that is also being actively being worked on with MAUI that also targets mobile platforms.

.NET has never been as unified as it is today. There is a way to target any platform within one diverse and evolving open ecosystem with 20 years of history.
