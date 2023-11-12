---
title: What is new in C# 12
published: 2023-11-13
tags: [C#, .NET, Programming languages]
---

With the advent of .NET 8 (on November 14, 2023), and .NET Conf soon to take place, let's us see what the next version of C# (12) will bring to developers.

C# 12 is a fairly minor update to the programming language. And I have picked out the features that I think are worth mentioning. 

One of the features have been previewed many years ago, and been long-awaited.

A full list of all new language features can be found [here](https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-12).

## Contents

1. <a href="/articles/what-is-new-in-csharp-12#primary-constructors">Primary constructors</a>
2. <a href="/articles/what-is-new-in-csharp-12#collection-expressions">Collection expressions</a>
3. <a href="/articles/what-is-new-in-csharp-12#alias-any-type">Alias any type</a>
4. <a href="/articles/what-is-new-in-csharp-12#default-parameters-for-lambdas">Default parameters for lambdas</a>


## Primary constructors

This was a feature that was previewed many years ago - before Roslyn (Compiler-as-a-service). I adopted it in my code, then the design team decided not to proceed with it at that time. So I had to revert back.

So let's start from the basics.

You are used to write you instance class constructors like so:

```csharp
public class Foo 
{
    public Foo(int value) 
    {
        Value = value;
    }

    public int Value { get; }
}
```

Most constructors are usually fairly simple. You assign fields or properties from the constructor parameters. Why should you have to you have to add a couple of lines and some vertical space for this?

Well, in C# 12, you can do this:

```csharp
public class Foo(int value) 
{
    public Value => value;
}
```

As the example shows, that means that we can capture constructor parameters inside the members in our class - like in methods and properties. But if you want to be on the safe-side you should assign the values to readonly fields.

```csharp
public class Foo(int value) 
{
    readonly int value = value;

    public Value => value;
}
```

When it comes to having multiple constructors besides the primary constructor. All the other constructors will have to call back to the primary constructor. That is so to initialize the parameters of the primary constructor, which are exposed to the class members by default.

```csharp
public class Person(string name, int age) 
{
    readonly string favoritePet;

    public Person(string name, int age, string favoritePet) : base(name, age) 
    {
        this.favoritePet = favoritePet;
    }

    public Name => age;

    public Age => age;
}
```

But why did this feature take so long to implement? 

Simply because of priority, and that the rules as well as the way to implement it was unclear. The designers probably wanted to solve pattern matching and other stuff leading up to record types before this.

### Difference from Record types

Although the syntax is similar, a significant difference between record types and classes with primary constructors is that the constructor parameters of record types are properties, while for classes they are not. For the latter, you have to declare and assign the properties yourself, as shown below:

```csharp
record FooRecord(string Name);

class FooClass(string name) 
{

}

var rcrd = new FooRecord("Foo bar");
var name = rcrd.Name

var cls = new FooClass("Foo bar");
// No "name" property
```

The criticism towards this feature would be that it is confusing developers because the syntax is so similar but the behavior so different. We just have to learn and get used to the difference.

## Collection expressions

In C#, there are so many ways to initialize collections:

```csharp
int[] arr1 = new int [] { 1, 2, 3 };

int[] arr2 = new [] { 1, 2, 3 };

var arr3 = new int [] { 1, 2, 3 };

var list1 = new List<int> { 1, 2, 3 };

List<int> list2 = new () { 1, 2, 3 };
```

In C# 12, you can instead use the brand new collection expression:

```csharp
List<int> arr = [ 1, 2, 3 ];
```

This new syntax will hopefully make code clearer, as well as provide some symmetry with list patterns that were introduced in C# 11.

```csharp
List<int> numbers = [ 1, 2, 3 ];

if (numbers is [var first, _, _])
{
    Console.WriteLine($"The first element of a three-item list is {first}.");
}
```

So how do collection expressions work?

In simple terms, the target collection take the type of the target. If it is an ``int[]`` then the expression is expected to be of either number literals or variables compatible with ``int``. The rule is the least common type.

Collection expressions work for ``List<T>``, ``Span<T>``, and ``ReadOnlySpan<T>``. Even inline arrays are supported.

```csharp
List<string> superHeroes = [ "Tony Stark", "Steve Rogers" ];

Span<double> importantNumbers = [ 42, 3.14 ];

ReadOnlySpan<Foo> objs = [ new Foo("Foo"), new Foo("Bar") ];
```

The feature supports any type with a collection initializer, or when the target type is providing a collection builder - via ``CollectionBuilderAttribute``.

There is also a code analyzer that suggest to you that you use collection expressions instead of the "old" initializers wherever applicable.

### Spreading

Of course, you can spread one or more collections in a collection expressions:

```csharp
string[] vowels = ["a", "e", "i", "o", "u"];
string[] consonants = ["b", "c", "d", "f", "g", "h", "j", "k", "l", "m",
                       "n", "p", "q", "r", "s", "t", "v", "w", "x", "z"];
string[] alphabet = [.. vowels, .. consonants, "y"];
```

### Alias any type

Previously you hav been able to create aliases for simple type names and generic types.

```csharp
using Weight = double;
using IntList = System.Collections.Generic.List<int>;
```

Now you can alias any type, including tuples, arrays, pointer types, and other unsafe types.

```csharp
using Point = (double X, double Y);
using Matrix = double[,];

Point point = new Point(3, 1);

Matrix matrix = new double[,];
matrix[1, 4] = 2.5;
```

I personally don't think that I will use this feature that much, but it is nice to have if I will ever need it.


### Default parameters for lambdas

Something that you haven't been able to do, but now can, is defining default parameters (aka optional parameters) for lambda expressions and delegates.

```csharp
var adder => (int a, int b = 1) => a + b;

Console.WriteLine(adder(2, 3)); //5
Console.WriteLine(adder(1)); // 2
```

This is partly possible because, in C# 10, they added support for type inference with lambda expressions and ``var``. Meaning that a delegate type will be created with the matching method signature of the lambda. That one will have the default argument.

This is a another feature that is nice to have, but I don't use default parameters that often.