---
title: "Explaining WebAssembly"
published: 2019-05-25
tags: [WebAssembly, Web]
---

There has been a lot of talk about WebAssembly lately. But what is it really?

In this article we will have a look at the technology and the benefits it brings for developers.

<img src="https://github.com/marinasundstrom/blog/blob/master/assets/wasm_logo.png?raw=true" alt="WebAssembly Logo" width="150"/>

## Background

Programming the Web has for a long time been limited to using [JavaScript](https://en.wikipedia.org/wiki/JavaScript). It is true that in the early days we had proprietary browser plugins, like [Java](https://en.wikipedia.org/wiki/Java_(programming_language)), [Flash](https://en.wikipedia.org/wiki/Adobe_Flash) or [Silverlight](https://en.wikipedia.org/wiki/Microsoft_Silverlight). There has been no standard for targeting the web using any other programming language than JavaScript.

Due to its long-time status as the exclusive language of the Web, JavaScript has been widely used as a compilation target for other languages - [CoffeScript](https://en.wikipedia.org/wiki/CoffeeScript)) might come to mind. That is why it has been nicknamed the *"Assembly language of the Web"*.

Another early initiative was the [Emscripten](https://en.wikipedia.org/wiki/Emscripten) toolchain project that enabled for C/C++ code to be compiled into JavaScript. Further attempts in trying to improve  performance gave birth to [ASM.js](https://en.wikipedia.org/wiki/Asm.js), a highly optimized version of JavaScript that layed the foundation for WebAssembly.

## What is WebAssembly?

[WebAssembly](https://en.wikipedia.org/wiki/WebAssembly) (or Wasm, for short) is set of open web standards that define a portable binary format for a stack-based execution environment (a [Virtual Machine](https://en.wikipedia.org/wiki/Virtual_Machine), or VM). It is analogous to the [Java Bytecode](https://en.wikipedia.org/wiki/Java_Bytecode), or [CIL/MSIL](https://en.wikipedia.org/wiki/Common_Intermediate_Language) of .NET.

The motivation for creating the Wasm standard is to enable languages other than JavaScript to target the Web. It therefore serves as a low-level compilation target. Most developers will probably not write in Wasm, but rather compile their existing code to this new binary format.

Wasm has been designed with JavaScript interop in mind. Currently (at the time of writing), there is no direct way of accessing the browser's Web API:s. So if you want to manipulate the [HTML DOM](https://en.wikipedia.org/wiki/Document_Object_Model), or use [WebGL](https://en.wikipedia.org/wiki/WebGL), then you have to write some "glue code" in JavaScript.

Still being worked on, for another iteration of the standard, is support for *threading* and *garabage collection* (GC). That is expected to be finalized in the near future.

There is a system interface, called [WASI](https://wasi.dev/) that serves as a interface for the host platform.

All modern web browsers support WebAssembly. When targeting older browsers, like for example Internet Explorer, it can be easily polyfilled using ASM.js.

## Targeting WebAssembly

The simplest and most clearest reason for targeting WebAssembly involves some existing C/C++ code that you want to leverage in your web app.

To compile that code you would use [Emscripten](https://emscripten.org/). The project provides a LLVM backend for WebAssembly that also will generate the necessary glue code.

There are many other languages and platforms targeting WebAssembly. Rust is one of them that directly [targets WebAssembly](https://rustwasm.github.io/).

Then there is also [Mono](https://www.mono-project.com/), the portable .NET runtime, a virtual machine, that has been compiled to [WebAssembly](https://www.mono-project.com/news/2017/08/09/hello-webassembly/). 

Microsoft is developing a client-side web component framework, called [Blazor](http://www.blazor.net/), written in C#. It is running on Mono, on WebAssembly, in the browser.

## Use Cases

WebAssembly lets developers leverage existing code on the Web, without having to rewrite any logic in JavaScript and suffer in performance.

Using Wasm as a compilation target, many companies have now been able to port their apps to the web and earned performance gains, compared to when using JavaScript and ASM.js as a compilation target.

Some notable examples are [AutoCAD](https://www.infoq.com/presentations/autocad-webassembly) and SketchUp apps. Even eBay is [using WebAssembly](https://www.ebayinc.com/stories/blogs/tech/webassembly-at-ebay-a-real-world-use-case/).

You will find an extensive list of use cases [here](https://webassembly.org/docs/use-cases/).


## Will WebAssembly replace JavaScript?

WebAssembly is not a replacement for JavaScript. It enables for other languages to target the Web, and expands the capabilities of the Web as a platform.

JavaScript could theoretically target WebAssembly. In fact, there are JavaScript engines running on WebAssembly.

## Not only for the Web

Despite the name, WebAssembly is not tied to the Web, or JavaScript for that matter.

There are a couple of projects in which they are developing standalone runtimes that run WebAssembly outside the browser, similar to what NodeJS does with JavaScript. Some with specialized goals, like running directly in kernel mode to be as performant as possible.

[Wasmer](https://wasmer.io/), by Mozilla, is one of these standalone runtimes. It comes with a package manager, the [Wapm](https://wapm.io/). Using it, you can download apps like [Nginx](https://en.wikipedia.org/wiki/Nginx) and [JavaScriptCore](https://en.wikipedia.org/wiki/WebKit#JavaScriptCore) (MacOS's JS engine), and immediately run them on any platform using Wasmer.

The WebAssembly ecosystem is being developed in the open. This shows how much effort and investment is going into it as a platform.

## What about NodeJS?

Users of [NodeJS](https://nodejs.org/) should not worry. Full stack JavaScript will not die very soon. It will continue to flourish as there is already a lot of investment in the ecosystem.

But we might see a shift in the tooling ecosystem, since JavaScript/NodeJS no longer will be the only platform for building full stack web apps.

WebAssembly will greatly benefit those projects that have a dependency on native addons. Avoiding all of the hassle having to target each platform and architecture (Linux, MacOS, Windows etc), the C/C++ code will simply be compiled into WebAssembly instead.

This is already being leveraged by psome opular NodeJS-based tools, including [node-sass](https://github.com/sass/node-sass), a tool for preprocessing CSS, that is dependant on the [libsass](https://sass-lang.com/libsass) library which is written in C/C++.

## Conclusion

WebAssembly is a new open and standardized way for other languages to target not just Web but many platforms.

It will be interesting to see what the future will bring to this new platform.
