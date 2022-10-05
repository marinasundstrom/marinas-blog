---
title: Demystifying asynchronicity and non-blocking operations
published: 2022-10-04
tags: [Programming concepts]
---

It is also used in other contexts like: Asynchronous messaging. It may confuse people who come from the code-centric perspective.

## What is "Synchronicity"?

Synchronicity is about something occurring in phase, or simultaneously, at the same time. When something is out of the specified phase it is referred to as being "asynchronous".

Here is a example of a real-life scenario:

Imagine yourself doing cooking according to a recipe. The flow from start to finish is predictable. That is a *synchronous* process. 

But when you are cooking your food you discover that you have forgotten some ingredients to add at the end. So you put your food in the oven, and you go to the super market to get those missing ingredients - while the food is still cooking in the oven. There is no exact time when you need to be back from the supermarket, so it is not in phase with the main task (cooking). That is an *asynchronous* process.

This should not be confused with the sequentiality of operations. Asynchronous operations can be put in sequential or non-sequential. 

In computing, when we talk about (a)synchronicity we often mean one of two things:

1. Between procedures in your program
2. Between dependant services in a system; like micro services.

This article will explain those two things in more detail.

## In program code

By default, computer programs are sequential, meaning that instructions are executed and finished in a predictable order and in phase.

General-purpose operating systems have the concept of threads which allows multiple programs to share execution time on the CPU. While running something in another thread has been easier, maintaining dependencies between those threads have traditionally been hard and often prone to errors.


## Between services


## Blocking
When we maintain a 

## Async in code: Tasks, Promises

Many modern programming languages, like C# and JavaScript, we often hear the about the ```async``` keyword being used together with ```Task``` (C#) or ```Promise``` (JS). Other languages might have the concept of a "co-routine" or "future". All of them represent a task to be finished in the future - a task that may, or may not, return a result. These programming languages gives us syntax for "awaiting"  the tasks to complete. It used to be hard to handle code running on other threads. You had to build your own state-machines. And I/O was always blocking the current thread, especially in GUI applications. It was hard to program multi-threaded applications just because of handling the threads.

"Async" stands for "asynchronous", but might be misleading. A Task does not imply that something is necessarily asynchronous. A "Task" (or Promise) may be used to represent operations that are not bound to another thread, or even asynchronous for that matter. It depends on the implementation.

Asynchronicity means that operations are not performed in a synchronized order. Another task, originating from another thread, might be running and finish earlier than another task. What the ```await someTask``` syntax, in most cases, gives us is a way to handle that asynchronicity in a way that appears sequential to humans.

## Debunking: "Asynchronous" HTTP requests

You have certainly used ```HttpClient``` in .NET, or ```fetch``` in JavaScript, and you know that you can initiate a HTTP request and wait for a response with ```await```.

But using ```async``` and ```await``` in code does not make the actual communication between processes asynchronous. There is a blocking operation, in the form of a connection, between the client and the server.

The async part is just for asynchronicity between threads within your program. It is about the program leaving control to another task while waiting for the server to respond back.












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

The concept of blocking operations us familiar to many. It simply means that an ongoing operation is blocking the execution whole program, preventing it from continuing,

Graphical User-interfaces (GUI) are typical examples of blocking operations. The UI is usually drawn within a single thread. Every event in response to user interaction is also taking place on that thread in a synchronous fashion. That means that when a handler for a button is being clicked it is blocking the process redrawing the Window (if Window-based). The result is that if a performed synchronous operation is long-running, or hangs, the Windows freezes and becomes unresponsive. This is noticeable when waiting for IO operations to complete - for example, reading a big file from disk.

Another example of blocking operations is a client performing an HTTP request to a server. HTTP is a synchronous protocol. You open a connection to the server and send a request, waiting for the server to respond back. It blocks the thread and all the actions at the client's side, as well as the server from handling other requests.

Ideally, you would want for operations to be asynchronous - for other operations to be able to execute while waiting for a specific operation to complete.

## Asynchronous operations

Any operation that is executed outside the normal flow is asynchronous.

There has been many ways of handling asynchronicity. But they have not been as maintainable, or even intuitive to programmers. One such way was to register callback functions when invoking asynchronous methods. Meaning that a procedure that would be executed once the operation finishes, or failed. 

Imagine multiple operations being executed after each other - each being executed from within another callback. 

It would look like so:

```js
function myMission (callback) {
    connect("server.com", function(connection, error) {
        loadData("secrets.txt", function(data, error) {
            connection.send(data, function(response, error) {
                saveData(response.data, function(error) {
                    connection.close();
                    callback(error);
                });
                callback(error);
            });
            callback(error);
        });
        callback(error);
    });
}

myMission(function(error) {
    if(!error) 
    {
        console.log("Mission successful");
    }
})
```

This is what is referred to as "Callback hell". Imagine doing some more logic in those callbacks.

Many modern programming languages, like C# and JavaScript, the keyword ```async``` is being used together with ```Task``` (C#) or ```Promise``` (JS). Other languages might have the similar concept of a "co-routine", or a "future". All the concepts represent a task to be finished in the future - a task that may, or may not, return a result. 

These programming languages gives us syntax for "awaiting" tasks to completed:

```js
async function myMission () {
    const connection = await connect("server.com");
    const data = await loadData("secrets.txt");
    const response = await connection.send(data);
    await saveData(response.data);
    connection.close();
}

try {
    await myMission();
} catch(error) {}
```

This syntax makes code appear as it is sequential and synchronous. But while 

This only affects asynchronicity at a code or system-level. If you use this with ```HttpClient``` or ```fetch``` it will not turn the communication with the Web server asynchronous. The request is still synchronous, but it is not blocking other parts of your code (other tasks) from executing. That is what "asynchronous" means in this context.

Some might think that Tasks necessarily have do to with multi-threading. But in reality, Tasks may run on the same thread as they are being awaited, rather than on another thread. It depends on the implementation and the underlying operation. They often have more to do with scheduling.

## Asynchronous messaging

When handling communication between services, how do we make the parties not block each other?

The answer is asynchronous messaging. Instead of opening upp synchronous channels, like with HTTP or gRPC, we handle messages sent between services. These 
