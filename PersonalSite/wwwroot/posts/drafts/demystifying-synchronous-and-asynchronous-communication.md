---
title: Demystifying asynchronicity and non-blocking operations
published: 2022-10-04
tags: [Programming concepts]
---

As a developer, you might have come around terms like "asynchronous" and "non-blocking operations". You might already use them today, but you don't really know what the concepts really mean.

In this article we will explore these concepts in some more depth.

## What is "Synchronicity"?

Synchronicity is about something occurring in phase, or simultaneously, at the same time. When something is out of the specified phase it is referred to as being "asynchronous".

Here is a example of a real-life scenario:

Imagine yourself doing cooking according to a recipe. The flow from start to finish is predictable. That is a *synchronous* process. 

But when you are cooking your food you discover that you have forgotten some things to for serving. So you put your food in the oven, and you go to the super market to get those missing things - while the food is still cooking in the oven. There is no exact time when you need to be back from the supermarket, so it is not in phase with the main task (cooking). That is an *asynchronous* process.

This should not be confused with the sequentiality - that something happens in a particular order.

## Blocking operations

The concept of blocking operations is familiar to many. It simply means that an ongoing operation is blocking the execution of the entire program, preventing it from continuing. This is a common problem within classical synchronous programming. The result is that if a performed synchronous operation is long-running, or hangs, the Window becomes unresponsive.

Graphical User-interfaces (GUI) are typical examples of blocking operations. The UI is usually drawn within a single thread - the main thread - also known as the "UI thread". Every event in response to user interaction is also taking place on that thread in a synchronous fashion. That means that when a handler for a button is being clicked it is blocking the process redrawing the Window. The program then freezes. This is noticeable when waiting for IO operations to complete - for example, reading a big file from disk.

Another example of blocking operations is a client performing an HTTP request to a server. HTTP is a synchronous protocol. You open a connection to the server and send a request, waiting for the server to respond back. It blocks the thread and all the actions at the client's side, as well as the server from handling other requests.

Ideally, you would want for operations to be asynchronous - for other operations to be able to execute while waiting for a specific operation to complete.

## Asynchronous operations

Any operation that is executed out of phase with other operations is asynchronous. It is not the same as the concept of multi-threading. Asynchronous tasks can be executed on the same thread. Asynchrony allows ar

In order to utilize their power we have to find some way to deal with it - to "synchronize" those operations within our program. It up to the environment or the programming languages to provide this.

This only affects asynchronicity at a code or system-level. If you use this with ```HttpClient``` or ```fetch``` it will not turn the communication with the Web server asynchronous. The request is still synchronous, but it is not blocking other parts of your code (other tasks) from executing. That is what "asynchronous" means in this context.

Some might think that Tasks necessarily have do to with multi-threading. But in reality, Tasks may run on the same thread as they are being awaited, rather than on another thread. It depends on the implementation and the underlying operation. They often have more to do with scheduling.

## Asynchronous messaging

When handling communication between services, how do we make the parties not block each other?

The answer is asynchronous messaging. Instead of opening upp synchronous channels, like with HTTP or gRPC, we handle messages sent between services. These 
