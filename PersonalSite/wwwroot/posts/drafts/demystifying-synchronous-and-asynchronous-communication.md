---
title: Demystifying asynchronicity and non-blocking operations
published: 2022-10-04
tags: [Programming concepts]
---

As a developer, you might have come across terms like "asynchronous" and "non-blocking operations". You might already use them today, but you don't really know what the concepts really mean.

In this article we will explore these concepts in some more depth.

## What is Synchronicity?

Synchronicity is about something occurring in phase, or simultaneously, at the same time. When something is following the same phase it said to be "synchronous". When it is out of phase it is "asynchronous".

Here is a example of a real-life scenario:

Imagine yourself baking. There are a fixed number of steps to be performed: You add eggs, then you add flour etc. You can only perform one step at a time. That is a *synchronous* process. 

But when you are baking your cake you discover that you have forgotten some ingredients for the icing. You call your friend asking them whether they can go to the supermarket to get the missing ingredients. So you put the cake in the oven and wait while your friend is off to the supermarket. There is no exact time when your friend comes back from supermarket. It might be before or after the cake has been finished in the oven. This operation is out of phase. Baking is now an *asynchronous* process with your friend going to the supermarket being an *asynchronous task*.

In the case above you try to synchronize

Synchronicity should not be confused with whether operations are sequential or not.

## Blocking operations

Blocking means that a task is blocking a certain resource from being used by another task. The latter having to wait for the former to release its hold on the resource in order to use it. This is a common problem within classical programming, especially when sharing resources between threads.

Graphical User-interfaces (GUI) are typical examples of blocking operations. The UI is usually drawn within a single thread - also known as the "UI thread". Every event in response to user interaction is also taking place on that thread in a synchronous fashion. That means that when a handler for a button is being clicked it is blocking the process that is redrawing the window. The program then freezes. This is noticeable when waiting for synchronous IO operations to complete - for example, reading a big file from disk.

Another example of blocking operations is a client performing an HTTP request to a server. HTTP is a synchronous protocol. You open a connection to the server and send a request, waiting for the server to respond back. That blocks the calling thread and all the actions at the client's side, as well as the server from handling other requests.

Ideally, you would want the flow to halt, let other operations execute, and to resume when the specific asynchronous operation has completed.

## Asynchronous operations

An operation that is executed out of phase with its calling flow is referred to as an *asynchronous operation*.

It is not the same as the concept of multi-threading.

In computing, there are other kinds of asynchronous processes, besides threading, such as I/O operations (reading an writing to peripherals), and communication across the network.

Communicating with an service across a network can either be synchronous or asynchronous depending on whether it blocks that service from being use by other consumers.

## Async programming models

Software platforms have had multiple programming models for dealing with asynchronous operations within an application. Historically,they have been with either events or callbacks - all which come with their own advantages and sets of challenges. 

### Callbacks

It used to be that asynchronous operations was made by passing "callbacks". That means that when the  operations had completed, a specified function was used to call back into the calling context. Imagine having multiple operations in a sequence and having to nest callbacks. With or without lambda functions, this was a nightmare. This is what is referred to as "Callback hell".

This shows how code would normally be written in JavaScript for NodeJS, about 6 years ago:

```js
myMission(function(error) {
    if(!error) 
    {
        console.log("Mission successful");
    }
})

console.log("Hello");

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
```

The callback passed into ```myMission``` could be called at any time - theoretically before ```console.log```, but that is not likely not a long-running operation. Due to this jumping between callback functions, there is no obvious sense of sequentiality in this program.

Although the pattern might look a bit different in other programming languages they are essentially doing the same thing.

### Task-based asynchronous model

Many programming languages have settled on as Task-based model for dealing with asynchronous operation. A *task* represents a unit of operation that keeps track of the status of its own execution. From a task you can also retrieve a result if any. 

It is a higher-level concept that should not be confused with threads, which is a operating-system concept. You should also bear in mind that a task is basically just representing an operation, whether it is asynchronous or not in its implementation.

The beauty of tasks is that they can have other operations chained after the. These are referred to as *continuations*. They themselves return unique tasks representing this continuation of operations.

This is how you would create a task in JavaScript from wrapping an existing callback-based asynchronous operation. But instead of Tasks, they call them "Promises" since they are promises to call back when the operation has finished.

```js
const ms = 5000; 

const delayPromise = new Promise((success, error) => {
    setTimeout(ms, () => {
        success();
    });
});

const continuationPromise = delayPromise.then(() => {
    console.log("Completed");
});
```

*FYI: APIs usually provide tasks, so you don't normally have to create them yourself. This is just for showing you how tasks work.*

Using tasks we can keep track of our asynchronous operations. But do you notice that we still have callbacks to maintain? What if we could express the flow as we do with normal sequential code?

### The "Await" syntax

As the computer power has increased, compilers and tooling have evolved. Languages now have syntax that integrates tasks, and makes consuming them much easier. With the ```await``` syntax, that has been adopted by many major programming languages, the program would look something like this:

```js
async function DoSomething() 
{
    const ms = 5000; 

    await delay(ms);

    console.log("OK");

    return 42;
}

const result = await DoSomething();

console.log(`Completed with the result: ${result}`);

function delay(ms) {
    return new Promise((success, error) => {
        setTimeout(ms, () => {
            success();
        });
    });
}
```

You find that this syntax expresses the intent more clearly. 

So how is this achieved by the compiler? The compiler actually rewrites each function that has been marked as ```async```, in our case ```DoSomething```, into a sequence of continuations based on where ```await``` is being used. Essentially, the compiler has generated a state machine behind the scene. Then it cleverly hides that from the programmer.

Of course, since an "async" function is returning a task, you could chain continuations after it.

```js
await DoSomething().then((value) => {
    console.log(`The result: ${value}`);
});
```

## Asynchronous messaging

When handling communication between services, how do we make the parties not block each other?

The answer is asynchronous messaging. Instead of opening upp synchronous channels, like with HTTP or gRPC, we handle messages sent between services. These 
