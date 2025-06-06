---
title: C# - a functional programming language?
published: 2025-04-21
tags: [C#, .NET, Programming languages, Functional programming]
draft: true
---

Functional programming paradigm. Or is it just a style? Patterns?

Here we are walking through some characteristics of functional programming language and how they apply to C#.

With functional programming patterns getting more popular we should explore what it really means to do functional programming.

Some things to think about when designing your software.

## Abstraction

To look at a problem from another perspective than, normally, just lines of code, statements, being expressed in an imperative fashion.

Tackling the real problem. Zooming out. Looking at it from a higher-level. Domain problem. Not worrying about the implementation details of how make it work. Purpose and declare intention.

We used to worry about SQL, but now we have Object-relational mappers (ORM) abstracting that implementation away so we can focus on what we want to achieve rather than how to do it. While those things are hidden away, it might of course be important to have some knowledge about how it is implemented, and how SQL actually works, since the way you use the ORM affects the SQL generated.

Functional programming borrows some concepts from mathematics with the purpose of processing or transforming data. The programming languages originally being created for certain tasks, like Erlang for telephony switches.

## Focus on data

Some would place this high up, since depending on your view, what ultimately matters is the data. A business owner might be concerned about data, and even the developers might see their code as operations on data. Even if object-oriented programming has been the popular paradigm.

Some problems are better seen doing operations on data, rather than as having objects with behavior (methods).

You need to ask yourself: Are you modeling behavior, or are you modeling data?

When having data to process you probably care less about viewing it as an object. Each instance might be short-lived during. And any methods is external to the data. A transaction script.

Most programmers that say the do object-oriented programming because they use "classes", are really more focused on the data rather than the behavior. So functional programming becomes appealing because of its focus on data, using data records and so.

This shouldn't devalue object-oriented programming, if it is done right for the right purposes. That it helps model the problem is a way that its efficiently solves it. In one area of the solution, like UI framework, the object-oriented approach might be a valid.

## Immutability

Data as value. You can't change the data.

The local variables holding primitive types are values, not reference to a value as is the case with classes. Or more specific 

Consider this record type that will be used for our examples:

```csharp
record Person(string Name, int Age);
```

We instantiate an instance, and we try to change one of its properties:

```csharp
var person = new Person("Max", 12);

person.Name = "Bob"; // Not possible. No setter.
```

That doesn't work since the object is immutable through its type, the record type.

### Equality by value

Primitive types (`int`, `bool` etc) and `string` are all compared with other instances of the same type by their values.

Records are themselves by their semantic defined as being compared by their value, by the value of their properties - much like structs.

```csharp
var person1 = new Person("Max", 12);

var person2 = new Person("Max", 12);

Console.WriteLine($"Are equal? {person1 == person2}"); // True, because their value is the same.
```

Although the record type might be a `class` (by default, unless specified `struct`), instances are always compared by their value.

### Non-destructive mutation

So how do you change something that is not able to change? You create a copy.

Non-destructive mutation is the name of the method with which you create a copy with specific changes. Which you can with record types (and anonymous types). This approach prevents you from changing state.

```csharp
var person = new Person("Max", 12);

var updatedPerson = person with 
{
    Age = 21
};

Console.WriteLine($"Are equal? {person == updatedPerson}"); // false

record Person(string Name, int Age);
```

#### Side note

On a side note, C# and .NET has no way of marking locals themselves as immutable. There is no `const`, `let`, or `val` keyword making the local equivalent to `readonly`. 

## Statelessness

Preference for statelessness. State is not to be shared, not to be passed around by reference, and thus not mutated. So no global variables.

This will come up later when talking about functional pureness.

## Type inference

Types should intuitively flow and be known by the developer. Focus is not about types while type safety still is there.

In functional programming the type of the parameter generally is indicated by the variable and its purpose.

The ``var`` declaration declares a local that takes on the type of the expression immediately assigned to it. Given certain type resolution rules within the compiler.

```csharp

var x = 1 + 3; // x will be type int

var z = Foo(); // Will be what Foo returns: string

string Foo() { /* Omitted */ }
```

Worth noting that some functional languages are weakly typed, what is also referred to as dynamically typed. Meaning that types are evaluated at runtime, using its rules, rather than by the compiler.

Type inference also work on parameters into generic methods, meaning you don't have to explicitly specify the type parameter, unless some cases when you need, because of the inferred type being in an inheritance hierarchy.

```csharp
int ret = NonOp<bool>(true);

var x = NonOp(14); // NonOp<int>

T NonOp<T>(T a) => a;
```

## First-class functions

In functional programming function are first-class, and objects in their own right. You are encouraged to create functions for abstracting and composing logic. This is in stark contrast to object-oriented programming where classes, with methods, inheritance, or design patterns, are used for composition.

Functions can be passed as objects, and subsequently as arguments into other functions. Such a function that either takes one or more functions as arguments, or returns a function as its result, is referred to as a _"higher-order function"_.

In C# we have delegates that allows for the same. Delegates are method references, akin to pointers, but type safe. A delegate holds a reference to a method which then can be passed around and invoked somewhere else in code.

```csharp 
Func<int, int, int> adder = (a, b) => a + b; 

// Types of a and b, and return type, are inferred by the Func<int, int, int> declaration.

var val = Foo(adder, 2)

int Foo(Func<int, int, int> func, int x) 
{
    return func(3, x); // result: 5
}
```

This would also work:

```csharp 
var val = Foo((a, b) => a + b, 2)
```

Although you can create your own delegate types, there are a number of built in ones that are universally used. There are delegates like `Action<TArg>` and `Func<TArg, TReturn>` types in many variations, taking various number of arguments and generic type parameters.

With functions you will feel like data is flowing through functions, rather than being methods called on an object or a certain class.

### Pure functions

Functional programming languages encourage you to write functions: pure functions. They functions are meant to be simple and verifiable. And they are ideal when processing data.

A pure function should be predictable.

One of the hardest things to deal with in object-oriented programming languages is side-effects. Methods always modify some state. And sometimes you don't take into account when a field has been changed, either on purpose of by mistake. Thats why you supposedly should have unit tests. For a function to be considered a "pure function" modifying state must not be allowed.

This is a pure function:

```csharp 
var adder = static (int a, int b) => a + b; // Func<int, int, int> 

var r = add(1, 2);
```

A "pure" function is a function that doesn't cause side-effects. Simply. it takes one of more values as arguments, and return a value. The output always being the same given the input(s). And that is why we talk about values.

Another example:

```csharp
var person = new Person("Max", 12);

// Arguments new Person("Max", 12) and 21 will always yield: new Person("Max", 21);
var newPerson = UpdatePerson(person, 21);

static Person UpdatePerson(Person p, int newAge) 
{
    // Return new value.
    return newPerson = person with 
    {
        Age = newAge
    };
}

record Person(string Name, int Age);
```

#### On impureness

As pointed out, the function must not mutate state outside of itself, like an object passed as a parameter, or variables outside out its closure. That is why many functional languages make it explicit when something, a variable or type members, can mutate using specific syntax, but in C# you need to be thoughtful. 

Passing values or immutable objects (records) to functions would solve some. Also limiting so it can't capture any variable outside its scope. Not acting as a closure.

```csharp
int x = 20;

var val = Op(2);

// Local function "Op" changed the value of "x".
Console.WriteLine(x);

static int Op(int a) 
{
    // Makes the function impure
    x = 30;

    return a + x;
}
```

Making the local function `static` (also applicable to lambdas) make so they become static and can't capture variables outside.

Likewise, embedding a random number generator into the function would make it impure. But that is not necessarily wrong if you can ensure the behavior of the rest of your system.

### Type inference for delegates

Since C# 12, there is enhanced type inference for delegate types. Previously you couldn't assign a lambda expression or method to `var` local declaration. But now you can, provided you specify types:

```csharp 
var adder = (int a, int b) => a + b;

var result = adder(1, 2); // Func<int, int, int>
```

By default, the built in `Action` and `Func` types (it their many variations) will be used. But if none is fitting it will generate an anonymous delegate type.

Type inference is for Minimal API in ASP.NET Core, when passing a lambda directly as a handler in `MapGet`, `MapPost` etc. The methods take an object of type `Delegate` which is the base class for all delegate types.

#### Side note

As a bonus, IDEs, such as Visual Studio Code, might display the delegate type when hovering over method names in debug mode. So that is one way type inference makes tooling smarter.

## Generics

A big part of functional programming is to generalize functions, so they can be used with input of any compatible type.

```csharp
List<string> items = ["c", "A", "b"];

Add<string>(items, "foo");

void Add<T>(List<T> items, T value)
{ 
    items.Add(value);
}
```

We have already seen samples using generics, but lets talk more about specific applications.

### "Generic math"

In .NET there is this feature called "Generic math" which allows you to generalize over number types and mathematical operations. This works well with type inference.

It works by implementing specialized generic interfaces that define instance methods and properties, and as static methods for parsing, and formatting, and for operators.

Here is a method that can take arguments of any number type, thanks to the built-in number value types implementing the interface `INumber<TSelf>` and that implements the `IAdditionOperators<TSelf,TOther,TResult>` that defines the static abstract method for the addition operator (`+`).

```csharp
var a = Add(1, 4); // int: 5

var b = Add(1.2, 4.2); // double: 5.3

T Add<T>(T a, T b) 
    where T : INumber<T> 
{ 
    return a + b;
}
```

The `INumber<T>` type is implementing addition interfaces that define static methods for operator overload. The compiler uses that information when resolving it.

#### Side note

This approach top generalizing maths makes C# a bit more similar to F#. But F# does it in a more discrete way. 

If the F# compiler sees that a function can be generalized then it will infer generics into it based on its usage in code, without the developer having to define type parameters. 

```csharp
let adder a b = a + b; // Equivalent in C#: T adder<T>(T a, T b) => a + b;

let result1 = adder 1 2; // Result: 3

let result2 = adder "2" "3"; // Result: "23"
```

That makes the flow a bit more less disruptive since you don't have to think about types.

## Declarative style

This is perhaps the most important one, since it binds many of the other things we have talked about together.

C# is an imperative programming language. You tell the computer what to do. Declare a variable, if this condition is true, then do this, else call this method. This is pretty standard for computers, giving it instructions:

```c#
int x = 42;

if(x == 42) 
{
    // Do this
    x = x + 5;
}
else 
{
    Foo(x);
}
```

You need to read the code line by line in order to understand what it does, and even the context of the problem. That puts focus on code as an implementation detail, which shouldn't be the focus.

Using the declarative programming style, you declare intentions, what to do to solve a problem, rather than firstly telling how to do something. And this ties into abstraction. 

Declarative programming can be achieved either by language constructs, or APIs, can facilitate this, like with chained methods or "fluent APIs".

 Consider this Fluent API with a Builder pattern:

```c#
var program = new Builder()
    .OpenDoor()
    .SoundAlarm(duration: 10000)
    .CloseDoor()
    .Build();
```

You may hide the implementation details behind this declarative API, in another level of abstraction.

### Language Integrated Query (LINQ)

The LINQ API, built into .NET, is a great example of declarative APIs in how you declare what to do to a collection, using the query operator methods, rather than focusing on how to implement the same yourself using declarative ``for`` loops and ``if`` statements for filtering.

```c#
int[] numbers = [7, 2, 3, 6, 0, 4, 5, 1];

var firstEvenNoOver2 = numbers
    .Where(n => n > 2)
    .Where(n => n % 2 == 0)
    .OrderBy(n => n)
    .First();
```

And you see, we are passing functions as arguments into the chained methods, via delegates.

And the use of type inference makes this smooth.

### Pattern matching

Let's you match data with patterns in an enhanced `switch` expression:

```csharp
var person = new Person("Max", 12);

var result = person switch {
    Person { Name: "Max", Age: >= 18} p =>  42,
    Person { Age: 12 } p =>  5,
    _ => 0
};
```

These are examples of "property patterns", and `_` being the default, or discard pattern.

There are many types of patterns ([list](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/patterns)), and the topic deserves its own article. But you can combine patterns with the `and`, `or` operators.

C# also supports patterns using `is` and `is not` operators. Here matching type `Person` and `Name = "Max"`.

```csharp
var person = new Person("Max", 12);

if(person is Person { Name: "Max" } matchedPerson) 
{
    // "matchedPerson" is not null
}
```

### Collection expressions

```csharp
var items = new List<string>();
items.Add("c");
items.Add("A");
items.Add("b");

var items = new List<string>() { "c", "A", "b" };
```

```csharp
int[] items = [42, 14, 5];

IEnumerable<string> items = ["c", "A", "b"];

List<string> items = ["c", "A", "b"];
```

The actual type will be determined by the target.

Spreading:

```csharp
IEnumerable<string> collectionA = ["c", "A", "b"];
IEnumerable<string> collectionB = [ ..collectionA, "foo"];
```

## What is a "pure" programming language?

Today there are few successful pure programming languages, and they are confined to certain problem domains. They are not suitable for general purpose in how they are designed. You would most likely not write an entire web app in functional style languages, although there are such attempts.

Just like with object-oriented programming languages, the best functional programming languages are those that combine it with other paradigms.

Pureness when it comes to programming paradigms is not as important as understanding the paradigms that are enabled by the programming languages.

## Is C# an object-oriented or a functional programming language?

It is neither. C#, as a general purpose programming language, but with a clear imperative and object-oriented base. It has adapted with time to allow for various paradigms and coding styles. It let's you mix and match these styles.

## Conclusion

Knowing about the functional programming concepts you can adopt these styles in your daily coding routine.

Being a software developer is all about being able to express a solution in a way that is adequate for the task or problem, and intuitive and clear to both you and others reading the code, without having to dig into it.

It makes you a better programmer.