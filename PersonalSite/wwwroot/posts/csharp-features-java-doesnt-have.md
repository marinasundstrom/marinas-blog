---
title: C# features that Java doesn't have
published: 2023-04-14
tags: [.NET, C#]
---

In this article I list some of the major features in C# and .NET that have no direct equivalent in Java. Some of the features are just mentioned because they are so essential and cool.  

I explain how stuff works and also demonstrate some in C# code.

My goal has been to show some C# specific syntax without highlighting it, like expression-bodied methods and alternative syntax for initializing objects. You will also notice my alternating use of ``var`` for type inference.

I hope that both C# and Java developers alike are enjoying this read. :) 

*Don't get mad at me if there are any errors! Some of the code may not have been tested. ;)*

## Contents

0. <a href="/articles/csharp-features-java-doesnt-have/#section-0">What is C# and .NET?</a>
1. <a href="/articles/csharp-features-java-doesnt-have/#section-1">Entrypoint (Main method)</a>
2. <a href="/articles/csharp-features-java-doesnt-have/#section-2">Global and Implicit usings</a>
3. <a href="/articles/csharp-features-java-doesnt-have/#section-3">Reference types vs Value types</a>
4. <a href="/articles/csharp-features-java-doesnt-have/#section-4">Properties</a>
5. <a href="/articles/csharp-features-java-doesnt-have/#section-5">Operator overloading</a>
6. <a href="/articles/csharp-features-java-doesnt-have/#section-6">Delegates</a>
7. <a href="/articles/csharp-features-java-doesnt-have/#section-7">Extension methods</a>
8. <a href="/articles/csharp-features-java-doesnt-have/#section-8">LINQ - Language Integrated Query</a>
9. <a href="/articles/csharp-features-java-doesnt-have/#section-9">Async Await</a>
10. <a href="/articles/csharp-features-java-doesnt-have/#section-10">Nullable types</a>
11. <a href="/articles/csharp-features-java-doesnt-have/#section-11">Generics</a>
12. <a href="/articles/csharp-features-java-doesnt-have/#section-12">Expression Trees</a>
13. <a href="/articles/csharp-features-java-doesnt-have/#section-13">IQueryable</a>
14. <a href="/articles/csharp-features-java-doesnt-have/#section-14">Unsafe code & Pointers</a>
15. <a href="/articles/csharp-features-java-doesnt-have/#section-15">Compiler as a Service</a>
16. <a href="/articles/csharp-features-java-doesnt-have/#section-16">Unified ecosystem</a>

<h2 id="section-0">What is C# and .NET</h2>

C# (C Sharp) is a general-purpose imperative object-oriented programming language that debuted in 2001 when the .NET Framework was first released.

Just like C++ is an increment of C, the name C# can be seen as an increment C++. There are four pluses forming a ligature in #. In musical language the # indicates that the written note should be a semitone higher in pitch.

The language initially borrowed a lot of its syntax from Java, which was based on C++. The lead designer Anders Hejlsberg had previously worked on Microsoft's own implementation of Java: J++. But when that endeavour ended he started working on what would become C#. The language was supposed to be like Java for the new .NET Framework. There were influences from Visual Basic. Hejlsberg had previously had a career building compilers for Pascal, Object Pascal and Delphi so he incorporated some influences from that.

.NET itself is the software platform on which C# is based. It provides a managed execution environment, the Common Language Runtime (CLR), similar to Java Virtual Machine. Just like the JVM, the CLR executes  bytecode, and provide automatic memory management via a Garbage Collector. .NET has a Just-in-Time (JIT) compiler that compiles bytecode into machine code on the fly.

.NET comes with a class library containing all the basic necessities, for string manipulation, file I/O, networking and threading etc. There is also a big ecosystem of open-source third-party packages on NuGet - the main distribution channel. And of course, there are the app frameworks for building Web, Mobile, and Desktop apps: ASP.NET Core and MAUI. You can also build games with Unity.

Since its inception C# has shown to not be afraid of incorporating new features and ideas with roots in other programming paradigms. Adding extension methods and LINQ added a declarative element that made C# a functional programming language. And more and more features come from that space: recently record types and pattern matching.

Since its open-sourcing back in 2016, .NET and C# has evolved even faster thanks to a vibrant community and ecosystem.

### Syntax

Core syntax was derived from Java or re-borrowed from C++ to appear and feel familiar to both audiences. From Object Pascal came Properties. Visual Basic influenced with Events used by app frameworks like WinForms.

They added a host of stuff to C# and .NET that they previously added to Microsoft's defunct JVM for J++, such as support for delegates - type-safe method pointers.

.NET APIs follow a CamelCase naming convention. Names in Java mostly start with lower case, except for class names.

C# uses the Allman style indentation, curly braces are placed on separate lines, whereas Java uses ANSI indentation for their curly braces.

### Samples: Style comparison

Here is a style comparison of equivalent code in C# and Java:

C#:

```c#
namespace Starfleet;

public class EnterpriseD : IStarship
{
    public void Engage () 
    {
        Console.WriteLine("Warping space");
    }
}
```

* In .NET, the names of interface types are by convention starting with ``I``.

Java:

```java
package org.starfleet;

public class EnterpriseD implements Starship {
    public void engage () {
        System.out.println("Warping space");
    }
}
```

<h2 id="section-1">Entrypoint (Main method)</h2>

Starting writing a program in C# is easy. Just create a .cs file and write some statements, like this:

```c#
using System.IO;

var fileContent = File.ReadAllText("test.txt");

Console.WriteLine(fileContent);
```

This is possible because C# allows you to define top-level statements in one of your .cs files. This will be the de-facto entry point of the program which is analogous to the traditional Main method in a Program class.

Compared to the traditional way (see below), this removes a lot of boilerplate code (classes, methods, braces) and frees horizontal space. Removing the object-oriented stuff that is not relevant to the entry point. 

The simplified syntax cleans up the code, makes it easier to read and understand, and easier for beginners to learn C#. Easier to prototype.

Top-level statements makes a big difference in ASP.NET Core where all the setup code now is significantly reduced.

### The traditional way

The traditional Main in a ``Program.cs`` would look like this:

```c#
using System;

namespace AwesomeApp;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
    }
}
```

This is similar to how it looks in Java:

```java
package AwesomeApp;

class Program {
    public static void main(String[] args) {
        System.out.println("Hello, World!"); 
    }
}
```

The compiler basically looks for a static method named Main when determining what is to become the entrypoint.

### Did you know?

That C# allows for .cs files to contain multiple type declarations and even namespaces. Java does only allow one class, interface or enum per .java file.

<h2 id="section-2">Global <code>using</code> and implicit "usings"</h2>

C# has a feature called ``global using`` which allows for using declaration (namespace imports) to be applied globally, to avoid having to add certain usings to every .cs file. Eliminating repetition.

By convention you might have your own ```Usings.cs``` that looks like this:

```c#
global using SuperThingy;
global using Foo.Bar;
```

The contents of the namespaces ``SuperThingy``and ``Foo.Bar`` are now in scope in every file in the project.

### Implicit "usings"

Each SDK (ASP.NET Core etc) may choose to add their own implicit global usings to a project. For instance, a Console project implicitly uses ``System`` and a bunch of other commonly used namespaces that were previously being included in every source code file by default when created.

These ``using`` are automatically added by the SDK.

Implicit usings are now enabled by default in .csproj for new projects.

Fun fact: Razor syntax has already had global usings for many years. But that did not apply do C#.

<h2 id="section-3">Reference types vs Value types</h2>

.NET and C# has this distinction between reference types (classes, interfaces, etc) and value types (primitives, enums, and structs). This affects how they are represented and treated differently by the runtime in terms of how memory is managed.

The distinction reference type vs value type basically affects how variables behave with respect to the type. A variable holding a reference type will be a reference to an object. Value types will directly refer to the data, usually allocated on the stack.

### Reference types

Reference types are types that when instantiated result in an object being allocated on the managed heap. Objects are implicitly referred to by object reference, similar to the concept of a pointer but managed by the garbage collector. 

The value of the reference type variable is the reference. When assigning from one variable to another the reference to the object gets copied - not the object itself.

A variable with a reference type is nullable - meaning that it can be set to ``null`` and thus not refer or point to any object instance.

```c#
StringBuilder sb = new ();
sb.AppendLine("Hello, ")

var sb2 = sb; // Copy object reference.

sb = null; // "sb" is set to null.

sb2.AppendLine("World!");

Console.WriteLine(sb2); // Prints "Hello, World!"

```

Once an object is out of references it is marked for deletion and the heap memory is eventually reclaimed by the garbage collector (GC).

### Value types

Value types are types that when instantiated IS the actual data. The variable holds the data as its value. This means that the value is copied when assigning a variable from another. 

When declared as a variable it only exists as long as its enclosing scope or method. Unless the value is declared as a field of an object on the managed heap.

```c#
int x = 42;
int y = x; // Copy value of "x" to "y".

Console.WriteLine(y);
```

Value types derive from the class ``System.ValueType`` which inherits ``System.Object``. That makes them appear like any other object, with methods and properties, but treated as values by the runtime. This makes .NET and C# truly object-oriented.

```c#
var maxValue = int.MaxValue; // Static property on System.Int32
```

C# has no significant distinction between primitive types and a class representation like in Java (no Integer, Boolean etc). So in C# the keyword ``int`` simply is an alias for the ``System.Int32`` struct.

```c#
// Different ways of declaring an int or System.Int32

int x = 42; 
System.Int32 y = 42;
```

### Structs - User-defined value types

User-defined value types can be defined as ```struct``` (keyword). They are similar to classes but with value semantics due to implicitly inheriting from ```System.ValueType```. So structs are passed by value rather than by reference. And of course, they cannot inherit from other types due to its base class. Though structs can implement interfaces.

```c#
public struct Test 
{
    public int X;
}
```

There is so much more to structs that doesn't fit into this article. But I thought they were worth mentioning.

### History fact

Before generics, the type Object was heavily used by collection classes since there were no type parameters. Every collection type had to store and retrieve any item from an array of Object (``object[]``) - then the programmer had to cast the object into the appropriate type. And if an item was of a value type that implied boxing and unboxing them on the managed heap. This had big implications on how you wrote code when dealing with complex value types such as structs. You could not just change a property of a struct in a list since you had to unbox the value and thus copy it to the stack.

However, this is not something that programmers have to deal with today. Writing this article just reminded me about this fact.

<h2 id="section-4">Properties</h2>

Properties are constructs that guard access to a type's fields and that may have logic tied to its accessors. 

C# has properties that syntactically appear as they were fields:

```c#
string str = "Test";
int len = str.Length;
```

A C# property consists of one or two accessors: one for getting the property, and another for setting the property.

```c#
public class Foo 
{
    string _name;

    public Foo(string name) 
    {
        Name = name;
    }

    public string Name 
    {
        get => __name; // Expression-bodied method
        
        private set 
        {
            // TODO: Perhaps perform some validation

            _name = value;
        } 
    }

    public void ChangeName(string name) 
    {
        // Can only set Name from within the class itself due to it being private.

        Name = name;
    }
}
```

```c#
Foo foo = new ("Bar");
foo.ChangeName("Joe");

Console.WriteLine(foo.Name);

foo.Name = "Bob"; // You don't have access to the setter.
```

Java has no special syntax for properties. Properties in Java is a method naming convention using "Get" and "Set" as prefixes. Otherwise, they are normal methods to be called as such.

Beware! There are places in the .NET frameworks where you see methods like ``int GetFoo()`` and ``void SetFoo(int value)``. But .NET properties are preferred.

Under the hood, .NET properties are methods, just that they are represented in a specific way.

### Auto-implemented properties

Auto-implemented properties allows you to define data properties with implicit backing fields without having to implement the accessors yourself.

```c#
public class Foo 
{
    public Foo(string name) 
    {
        Name = name;
    }

    public string Name { get; private set; }
}
```

<h2 id="section-5">Operator overloading</h2>

C# allows you to overload an entire host of operators on your classes and structs. Making your code much more expressive with custom equality semantics. This feature is also useful when implementing your own algebraic types.

Operator overloads come in two variants implicit and explicit (eg. ``(int)x``) cast operators and overloading binary arithmetic and comparison operators.

### Overloading binary operators

Binary operators require you to overload pairs (+ and -, == and != etc). When overloading equality and comparison operators it is recommended that you overload the ``Equals`` method inherited from ``Object``.

```c#
public sealed class Foo
{
    public Foo(int value) 
    {
        Value = value;
    }

    public int Value { get; private set; }

    public static Foo operator + (Foo lhs, Foo rhs) 
    {
        return new Foo(lhs.Value + rhs.Value);
    }

    public static Foo operator - (Foo lhs, Foo rhs) 
    {
        return new Foo(lhs.Value - rhs.Value);
    }
}

Foo foo1 = new (2)
Foo foo2 = new (3)

var result = foo1 + foo2; // result.Value == 5
```

### Casting operators

Casting operators allow you to define static methods that cast objects either implicitly by assigning or explicitly with the cast operator syntax.

#### Implicit cast by assigning to variable of target type

```c#
public class Foo1 
{
    public static implicit operator Foo2(Foo1 foo) => new Foo2(foo.Value);
}

Foo1 foo1 = new ();
Foo2 foo2 = foo1;
```

#### Explicit cast operator

```c#
public class Foo2
{
    public static explicit operator Foo1(Foo2 foo) => new Foo1(foo.Value);
}

Foo2 foo2 = new ();
Foo1 foo1 = (Foo1)foo2;
```

Java has no operator overloading. This results in Java records comparing references with ``==`` and ``!=`` operators.

<h2 id="section-6">Delegates</h2>

Delegates are type-safe method pointers that hold reference to one of more methods. These references can be invoked through the delegate instance. 

Delegates allow you to pass method references as arguments to functions, enabling functional programming patterns.

A delegate has a signature and is type-safe. Meaning that you can not cast between other delegates even if they have a similar signature. Of course, they can take generic type parameters as well.

```c#
delegate int ArithmeticOperation(int lhs, int rhs);
```

```c#
int DoOperation(ArithmeticOperation operation, int lhs, int rhs) 
{
    // Invoking delegate with parameters

    return operation(lhs, rhs);
}
```

```c#
// Assigning lambda to delegate. Also works with methods.
ArithmeticOperation op = (lhs, rhs) => lhs + rhs;

var result = DoOperation(op, 2, 3);

Console.WriteLine(result); //5
```

There are common pre-defined delegate types in .NET that suit most purposes.
The common ones are: ``Action<T>`` and ``Func<T, R>`` with various overloads taking different number of arguments.

```c#
void Test(Func<int, bool> f) 
{
    Console.WriteLine(f(5));
}

Test((x) => x == 2);
```

### Events

Delegates are used for Events in .NET - which are to delegates what properties are to fields. They restrict access to a delegate instance. Events are commonly used by UI frameworks like Windows Forms and WPF.

This sample shows an auto-implemented event. Though you could implement the ``add`` and ``remove`` accessors yourself.

```c#
public class Car 
{
    public event EventHandler Started;

    public void Start() 
    {
        Started?.Invoke(this, EventArgs.Empty); 
        
        // the ? means: call "Invoke" only if Started is not null.
    }
}

var car = new Car();
car.Started += (sender, args) => Console.WriteLine("Car started");

Console.WriteLine("Starting car...");
car.Start();
```

.NET Events make sure that only the declaring type can raise an event to execute event handlers. This restriction doesn't apply to plain delegates which can be invoked everywhere.

The common event delegate types are ``EventHandler`` and ``EventHandler<T>`` (where ``T`` is constrained to ``EventArgs``).

Java has no delegates or specific language constructs for events. Instead it uses interfaces and anonymous classes to implement both lambda expressions and event-listener patterns. Java is creating and passing an object which holds the method implementation of the lambda, rather than a reference to a method.

### Did you know?

Delegates have been part of .NET since version 1.0.

Delegates can represent both static methods and instance methods. And you can retrieve information about both the method and the target object.

```c#
Action action = () => Console.WriteLine("Foo");

MethodInfo methodInfo = action.Method;
```

That means that even if the method is a lambda you can still query that for information like about its parameters och whether the lambda has any attributes.

<h2 id="section-7">Extension methods</h2>

Methods that get attached to existing types (classes, structs, interface, enums etc) as they were instance methods - hence extension methods. 

They are in fact static methods that just appear differently to us and the compiler. So they don't have access to any private or protected members of the types that they extend.

LINQ is built on extension methods for extending ``IEnumerable<T>`` with query operators.

```c#
public class Foo 
{
    public int X { get; set; }
}

public static class SomeExtensions
{
    public static void Add(this Foo source, int value) => source.Value += value;
}
```

```c#
Foo foo = new () { X = 40 };
foo.Add(2); // Appears like it were a member of Foo or a parent class.

Console.WriteLine(foo.X); // 42
```

Java has no extension methods.

<h2 id="section-8">LINQ - Language Integrated Query</h2>

The ability to perform queries on all collection types across the framework. Query any collection with any object type.

LINQ extension methods to add query operators on collection types that implement ``IEnumerable<T>``. Operators may take delegates as predicates for filtering (Where) or for projecting into new form (Select).

```c#
int[] array = new [] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

var evenNumbers = array.Where(x => x % 2 == 0);

foreach(var no in evenNumbers) 
{
    Console.WriteLine(no);
}
```

Most operators are chainable and resulting in a object representing the query that implements ``IEnumerable<T>``. This is how they chain together. 

Conceptually, ``Where`` looks like this when reimplemented:

```c#
public static class Enumerable 
{
    public static IEnumerable<T> Where(this IEnumerable<T> source, Func<T, bool> predicate) 
    {
        foreach (var item in source) 
        {
            if (predicate(item))
            {
                yield return item;
            }
        }
    }
}
```

This method extends ``IEnumerable<T>`` regardless of parameter type, which will be inferred by the compiler based on parameter ``source``. The ``yield return`` in a method returning ``IEnumerable<T>`` means that it is a generator.

An enumerable utilizes deferred execution. The resulting the Enumerator object represents a query that is evaluated only when iterated over by a ``foreach`` or when ``ToArray()`` or ``ToList()`` or equivalent is being called. 

```c#
var persons = new Person[] {
    new ("John", "Smith", 35),
    new ("Jane", "Doe", 23)
};

var q = persons
    .Where(p => p.LastName == "Smith")
    .Where(p => p.Age > 35);

foreach(var p in q) 
{
    Console.WriteLine(p.FirstName);
}

// Return results in array

var result = q.ToArray();

// Project Name with "Select"

foreach(var fullName in q.Select(x => x.FullName)) 
{
    Console.WriteLine($"FullName is: {fullName}");
}

record Person(string FirstName, string LastName, int Age) 
{
    public string FullName => $"{FirstName} {LastName}";
};
```

Java has no extension methods, but instead provide similar functionality to LINQ using "Streams" and by collections extending the ``Stream<T>`` interface.

Streams in C# are something different. They are about a primitive involved in reading buffers of data, like a ``FileStream``.

<h2 id="section-9">Async Await</h2>

.NET has a Task-based model for asynchronous operations. It is similar to promises in JavaScript and Futures in other languages.

An asynchronous method returns a Task which signals to the caller when the operation is completed or have failed, resulting in an exception. The API provides methods for specifying callbacks for continuations that handle the Task.

### ``await`` statement

C# provides provides syntax that simplifies the consumption of Tasks. The ``await`` statement lets you await tasks and in that way deal with asynchronous code as it were synchronous.

Await statements can only occur within methods marked with the ``async`` modifier, unless it is at top-level.

```c#
async Task DownloadPage() 
{
    HttpClient httpClient = new ();

    try 
    {
        Console.WriteLine("Downloading");

        var str = await httpClient.GetStringAsync("http://test.com");

        Console.WriteLine("Downloaded");

        await File.WriteAllTextAsync("page.html", str);

        Console.WriteLine("Saved");

        return true;
    }
    catch (HttpRequestException e) 
    {
        Console.WriteLine(e);

        return false;
    }
}

var result = await DownloadPage();
```

Under the hood the compiler effectively splits the method at each ``await`` statement, and creates another method for the rest which acts as a continuation when the task has completed. There is state machine which handles the transitions between various states and surfaces exceptions.

Java doesn't have a native ``await`` feature similar to the one in C#. But there are initiatives in the Java community that is working on it. JavaScript has had the await syntax with their Promise objects for many years.

### Did you know?

Before the Task-based asynchronous model, there was a couple of other models for dealing with asynchronous code. One of them was based on .NET events that you had to handle. But that lacked the scheduling and thread synchronization part that the Task-based model now provides.

You can turn event-based asynchronous APIs into Tasks using the TaskCompletionSource - which is the factory-part to a Task.

The F# functional language (another .NET language) had tasks before they were standardized in .NET and introduced in C#

<h2 id="section-10">Nullable types</h2>

C# has syntax for declaring types as explicitly "nullable" - that they can be assigned the value ``null``. (``int?``, ``string?``, ``Foo?``). And the compiler is quite smart in telling you whether some variable in a code path is unexpectedly null.

```c#
int? x = null;
Foo? foo = null;
```

Although the syntax is universal, nullable types behave differently for value types and reference types respectively.

### Nullable value types

Value types that are declared as nullable get implicitly "wrapped" by a ``Nullable<T>`` value type. But they will not behave like reference types, of course.

```c#
int x = null; // Not allowed: int x = null;
int? x = null; // System.Nullable<int> x = null;
```

And ``Nullable<T>`` has a very useful method:

```c#
bool? x = true;
var v = x.GetValueOrDefault(); // Returns true

bool? y = null;
var v2 = y.GetValueOrDefault(); // Returns false
```

That is a pretty useful three-state switch. Some UI Dialog APIs return a value of type ``bool?`` with ``null`` indicating that the user did not explicitly press "Yes" or "No". Like for "Cancel".

### Nullable reference types

Reference types have always been nullable by default. The added syntax and nullability checks are pure compile-time analysis so that it preserves compatibility with older versions of the runtime and code. It is also opt-in via csproj.

```c#
Foo foo = null; // Warns
Foo? foo = null; // Allowed
```

You can override the nullability checks by putting ! after an expression that might be null but assigned to a variable with a non-nullable reference type.

```c#
Foo foo = null!; // Allowed - since you told the compiler to ignore the warning with !
```

Java has no nullability syntax. Though they have an ```Option``` type.

### Why "nullable" works as it does

So why is nullability so different for reference types and for value types? Simply: Due to introducing the semantic distinction in the first place.

Reference types have always been implicitly nullable (without the ? syntax). It is something that is built into the runtime. You just had to know whether something was a class and realize that it could be null and had to be handled. Value types have always had a default value and thus they did not require initialization with ``new`` either.

Nullable value types were added in .NET Framework 2.0 and C# 2.0 (together with generics) for a specific reason. The purpose was to be able to map primitive types in C# to nullable columns in databases. In order to not break anything existing, they added this ``Nullable<T>`` wrapper type. C# also introduced the nullability marker ``?`` (``int?``). This of course, caused asymmetry in syntax indicating nullability.

In C# 10, nullable reference types was added. Making it possible to explicitly state when a variable can be ``null`` also for reference types using the same ``?`` nullable marker. In order to not break existing code and behavior, this feature was primarily implemented in the compiler, not in the runtime. Nullable is an opt-in feature that is enabled by default in new projects. But any nullability issue is treated as a warning and not an error, unless you tell the compiler to.

How C# deals with nullable reference types has been greatly influenced by TypeScript, another Microsoft-product. Applying the knowledge from doing control flow analysis on TypeScript/JavaScript code.

<h2 id="section-11">Generics</h2>

.NET has runtime support for generics. That means that the Common Language Runtime (CLR) is aware of an instantiated "closed generic type" and its type parameters. So it can make runtime optimizations depending on whether the parameterized types are value or reference types. 

The program can also query the types at runtime using Reflection to see what generic arguments an object has been instantiated with.

```c#
var list = new List<string>();
var genericArg = list.GetType().GetGenericArguments()[0];

Console.WriteLine(genericArg.Name); // Int32
```

By contrast, Java generics is built on "type erasure". Parameterized types are only known and enforced at compile-time and effectively erased - replaced by ``Object`` - when the code is compiled. So parameter types can not be retrieved at runtime. The JVM doesn't know about the instantiated generic type and its arguments.

In C#, you don't have to wrap a primitive type in a class in order to pass it as a generic type parameter. Primitive types are fully integrated into the language and runtime with corresponding value types (``int`` for ``System.Int32``, ``bool``for ``System.Boolean`` etc).

The .NET way of generics allow for more expressiveness and flexibility. You don't have to pass around a Type object and there are no semi-implicit casts like in Java. It is predictable and powerful.

We can get the runtime type parameter from inside the method or type that takes that parameter, like so:

```c#
static void WhatsTheType<T>() 
{
    var paramType = typeof(T); // System.Type

    Console.WriteLine(paramType.Name);
}

WhatsTheType<int>(); // Prints "Int32"
WhatsTheType<Foo>(); // Prints "Foo"
```

It is impossible to express it this easy in Java, because of type erasure.

In .NET, generic type parameters are widely used by dependency injection frameworks for when to resolve type instances.

### Did you know?

Generics in .NET was designed by Don Syme at Microsoft Research in Cambridge, who later created the functional programming language F#, which is based on OCaml. Generics was incorporated in the F# language.

The current lead designer for C#, Mads Torgersen was involved in Java when they introduced their version of generics.

<h2 id="section-12">Expression Trees</h2>

An expression trees is code that is being represented as an Abstract Tree at runtime.

In their simplest form they are useful for when to represent and analyze code in an abstract sense. Expression trees do not map to C# syntax, but represent generalized programming structures in .NET.

This feature is baked into .NET and supported at compiler-level in C#. There is no equivalent in Java.

You can either construct an expression yourself in code, or write normal C# in a lambda that the compiler turns it into an expression tree that can traversed by code.

Dynamically building an expression tree in code:

```c#
// The expression tree to execute.
BinaryExpression be = Expression.Power(Expression.Constant(2d), Expression.Constant(3d));

// Create a lambda expression.
Expression<Func<double>> le = Expression.Lambda<Func<double>>(be);
```

The expression tree can be modified, compiled into bytecode and executed at runtime. Here is an example of a lambda that the compiler turns into an expression tree to then be dynamically compiled and invoked:

```c#
using System.Linq.Expressions;

Expression<Func<int, bool>> expr = (arg) => arg == 2;

// Compile the lambda expression.
Func<int, bool> compiledExpression = expr.Compile();

// Execute the lambda expression.
bool result = compiledExpression(3);
```

The code is dynamically compiled to bytecode at runtime using the ``System.Reflection.Emit`` API under the hood. Ready for the JIT to compile and execute, as demonstrated.

IQueryable depends of expression trees for representing predicates that get translated into queries.

### Did you know?

Before Expression Trees, or even Roslyn (see below), the only standardized way to analyze code was with the CodeDOM APIs. Similar to Expression Trees, it had an abstract syntax tree, the Code Document Object Model (DOM). A term familiar to those who know about the Web and HTML. The API acted on source code, both C# and Visual Basic.NET, and usually lagged behind the current language version in what features it supported.

<h2 id="section-13">IQueryable</h2>

IQueryable is an interface that enables querying for data against a specific data source. It is used in a way that is similar to IEnumerable but stores its query expressions as expression trees instead of delegates to methods.

IQueryable is what enables using the LINQ query syntax in Entity Framework.

The execution of an IQueryable is dependant on a provider - for instance, the SQL provider in Entity Framework Core (EF Core). EF Core traverses the query (IQueryable), traverses their expression trees, and generates a command to be sent to the database. It then gets a result that it materializes as .NET objects.

This is what consuming an IQueryable from Entity Framework Core would look like:

```c#
using var todoContext = new TodoContext();

var completedTodos = await todoContext.Todos
    .Where(todo => todo.Status == TodoStatus.Completed) //IQueryable<Todo>
    .ToArrayAsync();
```

For every ordinary LINQ operator there is an equivalent LINQ operation. Which one is supported depends on the provider.

There is not equivalent to IQueryable in Java.

<h2 id="section-14">Unsafe code & Pointers</h2>

C# has C/C++-like pointer support within unsafe context. It allows for managing memory and doing low-level interop with native unmanaged code.

"Managed code" is code that is managed by the CLR - C# code that has been turned into bytecode and JIT:ed. "Unmanaged code" is what runs outside the CLR, in the operating system - a.k.a. native code.

```C#
unsafe
{
    int length = 3;
    int* numbers = stackalloc int[length];
    for (var i = 0; i < length; i++)
    {
        numbers[i] = i;
    }
}
```

There is a class called ``NativeMemory`` which allows you to allocate native memory:

```c#
using System.Runtime.InteropServices;

unsafe
{
    int* mem = (int*)NativeMemory.Alloc(sizeof(int));

    // Do something
    *mem = 42; 

    Console.WriteLine($"The value is: {*mem}");

    NativeMemory.Free(mem);
}
```

You also have to declare that you are using unsafe code in the csproj file.

There is a safe managed pointer API (the ```System.IntPtr``` struct) that doesn't require you to enable an unsafe context and to use unsafe pointers in a lot of cases, for instance when doing PInvoke to native code.

Native pointers do not exist in Java.

<h2 id="section-16">Compiler as a Service (Roslyn compiler framework)</h2>

C# is built on the Roslyn compiler framework which has a modern compiler architecture that is providing the the compiler as a service. That means that the developer can integrate into the compiler to build tools that analyze and modify source code. The compiler gives you the APIs to access the Abstract Syntax Tree (AST) or the semantic information that you want.

Common ways of integrating with the C# compiler is by writing extensions such as Code Analyzers, Code Fixes, or Source Generators that then integrate with IDEs like Visual Studio and Rider. Analyzers are portable and can be distributed as NuGet packages. Even run in the .NET CLI.

Code Analyzers and Code Fixes are stuff that usually show up in your IDE as perhaps Warnings, Errors, or Suggestion. A fix is a solution to the analysis.

Source Generators allow you enhance code by generating code-behind based on some logic. It is being used by many serializers and object mapping libraries to generate code and to avoid using Reflection. There are also other cases when they help generate boilerplate code for you, like with the MVVM patterns.

The Roslyn C# compiler is open-source and itself built with C#. So fully bootstrapped.

## Unified ecosystem

.NET easily has the most complete Software Development Kit (SDK) with support for multiple platforms and operating systems.

The SDK has its own CLI tools for creating, building and running apps.

Simply write ``dotnet new console`` to create a console app.
And ``dotnet run`` to run it.

The is a unified project and build system based on the MSBuild tool chain - with the .csproj file. So unlike Java, there is no need to choose between Maven or something else. Of course, you can always use Cake if you want to script your build process in C#.

Package management is almost exclusively through NuGet. Support is built into the SDK tooling and Csproj. There are alternative packet managers that are specialized, like for F#. These do connect to NuGet as well.

There are multiple app frameworks, the popular ones by Microsoft and open-source. ASP.NET Core is the most popular web app framework. Many features in C# and .NET can be tied to what they are doing there.

There is a vibrant .NET Community. People who love .NET and to code and share knowledge and help each other - both beginners and experienced programmers.

Since there are so many frameworks, libraries, and applications of C# and .NET, you never stop learning new stuff. There are endless possibilities.

## What did Microsoft do right with .NET as a platform?

Compared to Java, .NET Framework was open-sourced quite late. That most likely contributed to its success. Microsoft had kept .NET under control while a community had been growing, with the Mono open-source community being one of them. They put much effort into building great APIs and frameworks that developers loved. 

When Microsoft fully open-sourced .NET in 2016, they had already earned experience in open-sourcing parts of the stack. The community had slowly been prepared for the change and Microsoft had in that way built up trust with its users and customers. 

And since they started quite fresh with .NET Core they had the opportunity to experiment and to find the best way forward according to feedback and with contributions from the community.

Some would also point out that a lot of the success of .NET is due to third party technologies like the Unity game engine. So we should not forget that one :) 