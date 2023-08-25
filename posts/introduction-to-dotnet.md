---
title: "Introduction to .NET"
published: 2021-04-25
author: "Marina Sundström"
tags: [.NET, C#]
---

.NET is an open-source platform for building applications for the Web, Mobile, Desktop, and Cloud. Currently, it is at version 5, which was released in November 2020.

.NET provides a runtime environment, the Common Language Runtime (CLR), an extensive base Base Class Library (BCL), programming languages like C#, VB.NET, and F#. On top of the platform you find many application frameworks, such as ASP.NET Core and WPF. It also integrates well with Microsoft Azure.

The platform officially runs on Windows, macOS, and Linux, and targets CPU architectures. You can even run .NET on a Raspberry Pi.

First released in 2002, the platform has kept evolving during the past 20 years. Adding more and more features and APIs.

And the ecosystem is being developed in the open on GitHub. In fact, the .NET ecosystem was the first that Microsoft really started open-sourcing. Since then it has just increased in scope, and Microsoft has become a leading open-source company.

## The "Old" .NET Framework

The versions up to .NET Framework 4.8 run exclusively on Windows and is closed-source, in contrast to .NET 5 which is  open-source, and run on Windows, macOS, and Linux. 

Not all of the frameworks from the older Windows-exclusive versions are available in .NET 5 or later. Although, they did port WPF and Windows Forms ("WinForms") in order to let developers migrate their existing apps to the newer versions of .NET. Taking benefit of the improved performance, and continued supported by component vendors. WPF and WinForms remain exclusive to Windows.

The newer .NET platform (.NET Core 3.0 and later) will be able to reference libraries targeting 4.8. However, there is no guarantee that all the APIs are present, or that the behavior will be the same.

## The CLR

.NET consists of a Common Language Runtime (CLR) that consists of an Virtual Execution Engine (VES), and a Base Class Library (BCL). It supports various operating systems and CPU architectures: Windows, macOS, and Linux. The supported architecture are x86, x65 and ARM64. This means that your .NET apps can run on a Raspberry Pi.

Code is written in a high-level language, such as C#, and compiled into portable bytecode (CIL) that, when executed, is being verified, and compiled by the runtime into the type of machine code that is compatible with the CPU that it is currently running on. The process is called Just-in-Time (JIT) compilation.

.NET also lets you bundle a specific version of the runtime with your app - a so called "Self-contained app". This allows you to run apps using different runtime versions without having to install shared runtimes on your system. It thus removes the risk of a runtime install affecting another. This is ideal for cloud apps that are being hosted on servers.

The other option, Ahead-of-Time (AOT) compilation lets you compile and bundle your app and runtime ahead of time for a specific target Operating System platform. It come with the possibility of publishing you app and runtime as a single-file.

## C# programming language

C# (C Sharp) is the preferred programming language. It is a multiparadigm programming language which is imperative and object-oriented at its core. It is a member of the C language family, and draws a lot of influences from Java in both syntax and semantics. The language also borrows from C++, especially for its low-level interop features.

Over the years, the language has evolved to integrate elements from other programming paradigms, like functional programming, with features such as pattern matching.

## Tools

Development starts with the .NET Command-Line Interface (CLI). The CLI lets you create a new project from a project template, then building your project, and finally publish it. It integrates with the MSBuild engine.

```sh
dotnet new console -o MyApp
cd MyApp

dotnet run
```

There are many Integrated Development Environments (IDE) to choose from: Visual Studio, Visual Studio for Mac, Visual Studio Code, and JetBrains Rider.

## App Models

The .NET stack supports these main scenarios: Web, Mobile Desktop, and Cloud.

You write your code using a language like C#, VB.NET, or F#. These languages are all evolving in the open.

Using those languages you can develop apps for a number of app platforms that come out-of-box with .NET 5 and later:

Web apps with the ASP.NET Core Web Framework (including MVC and Web API). You can build Mobile apps for Windows UWP, iOS, and Android. Cloud apps that integrate with Microsoft Azure services. You can also write Desktop apps for Windows in WinForm or WPF.

The preferred way of defining UIs in UWP and WPF is declaratively using an XML language called XAML - which is to these frameworks what HTML is to the Web. All of the dialects have an evolved templating and styling system.

You can also write modern web apps for client-side app using Blazor, running in the browser, targeting WebAssembly. You build UIs in a templating language called Razor, that mixes HTML and C#.

You can write apps that run in Azure, and you can write apps that run on a Raspberry Pi.

Uno is an open-source project that aims att bringing Microsoft UWP app model to platforms other than Windows, like Android, iOS and macOS. They even support targeting the Web via WebAssembly. Allowing you to write C# and XAML for the Web.

.NET has an ecosystem of libraries and frameworks developed by third-parties, the majority of them are also open-source. They are distributed as NuGet packages through a package manager.

## .NET “Core”

There were some versions of the cross-platform .NET runtime branded “.NET Core “, but they have now been superseded by .NET 5. Using the .NET branding instead.

The initial goal for .NET Core was to build a runtime that was optimized for hosted cloud-scenarios. That later expanded to support more scenarios and app models.

This might be a simplification, but for all intents and purposes, pre-Core versions are considered exclusive to Windows.

## So what's up next?

The upcoming release, .NET 6, will integrate support for developing apps for Android, iOS, and macOS - and bring a new framework for building cross-platform apps: .NET Maui (Multi-platform App UI). This is the successor to the Xamarin.Forms mobile app framework.

A theme that was planned for .NET 5 was the unification of .NET, which was  delayed due of COVID. The .NET 6 release will finally make the tooling uniform across the stacks and existing app models.

The Mobile stack (previously Xamarin), and the WebAssembly stack, that use the Mono runtime, will be fully integrated in .NET 6.

Microsoft is committed to release a new major version of .NET every year. Every other release will be an LTS release. (Long-Term Support) The next LTS release is .NET 6 that will be released in November 2021.

## Conclusion

As you can see, .NET is an evolving ecosystem. And it is open-source, meaning that you as a developer can contribute to the projects that make up the framework.
