---
title: Generics in Java vs .NET
subtitle: A .NET developer's perspective
published: 2023-11-11
tags: [C#, .NET, Java, Generics, Programming languages]
---

## Background

I'm a .NET developer, writing C# code. But 9 months ago, I joined a company whose code is almost exclusively Java. As I have gotten more into writing Java code, I have become more aware of the differences between Java and my beloved C#.

Both Java and C# are at their core general-purpose object-oriented programming languages, with similar syntaxes, but there are some fundamental differences that are not apparent until you dig deeper. In particular, when coming to how generics has been implemented. It is not just about the language but their respective platforms and runtime environment.

Java erases generic type arguments when compiling your Java code into bytecode, while .NET has implemented generics in the runtime, so it retains type arguments. This has a huge impact on the code you write - how flexible you can be.

This article will walk you through generics in both languages - highlight the similarities as well as showing you the differences. We will use a comparative approach. There will be a lot of code samples.

And I will provide my thoughts and opinions as a .NET developer.

## Contents

1. <a href="/articles/generics-in-java-vs-dotnet#terminology">Terminology</a>
2. <a href="/articles/generics-in-java-vs-dotnet#what-is-generics">What is generics?</a>
3. <a href="/articles/generics-in-java-vs-dotnet#syntax">Syntax</a>
    1. <a href="/articles/generics-in-java-vs-dotnet#generic-classes">Generic classes</a>
    2. <a href="/articles/generics-in-java-vs-dotnet#generic-methods">Generic methods</a>
    3. <a href="/articles/generics-in-java-vs-dotnet#constraints">Constraints</a>
4. <a href="/articles/generics-in-java-vs-dotnet#java-type-erasure">Java Type erasure</a>
5. <a href="/articles/generics-in-java-vs-dotnet#net-runtime-generics">.NET Runtime generics</a>
6. <a href="/articles/generics-in-java-vs-dotnet#reflection">Reflection</a>
    1. <a href="/articles/generics-in-java-vs-dotnet#retrieve-information-about-a-type">Retrieve information about a type</a>
    2. <a href="/articles/generics-in-java-vs-dotnet#pass-information-about-a-type-parameter-into-a-method">Pass information about a type parameter into a method</a>
    3. <a href="/articles/generics-in-java-vs-dotnet#retrieve-the-type-argument-of-a-generic-type">Retrieve the type argument of a generic type</a>
    4. <a href="/articles/generics-in-java-vs-dotnet#invoke-a-generic-static-method">Invoke a generic static method</a>
    5. <a href="/articles/generics-in-java-vs-dotnet#java-an-issue-with-serializers-and-generic-classes">Java: An issue with serializers and generic classes</a>
7. <a href="/articles/generics-in-java-vs-dotnet#conclusion">Conclusion</a>

## Terminology

Here is a list of some of the terms that will pop up during the course of this article:

* **Type parameter** - The generic type parameter that takes a type as an argument.
* **Type argument** - The type passed into a type parameter.
* **Open generic type** - Type that has not yet been instantiated with a type argument.
* **Closed generic type** - Type that has been instantiated with a type argument.
* **Constraint** - Restricts the possibilities of types that can be passed as argument to a type param.
* **Bounded generic parameter** - A type parameter that has gotten constrained to a set of types. _(Java)_
* **Super class** - Class from which a certain class derive (or inherit) from. _(Java)_
* **Base class** - Synonymous with **Super class** _(.NET)_
* **Sub class** - A class that has been derived from another type. _(Java)_
* **Common Language Runtime (CLR)** - The .NET runtime environment (virtual machine), which C# is targeting.
* **Java Virtual Machine (JVM)** - Javas runtime environment. Where Java bytecode is running.
* **Bytecode** - A specialized instruction set used by virtual machines, such as JVM and the CLR. Compilation target for programming languages.
* **Common Intermediate Language (CIL)** - .NET's bytecode. Also called **MSIL**.
* **Java bytecode** - Java's bytecode
* **Metadata** - Data describing data. In our case, the structure of a program, it's types and their members.

Some terms are more common in one language than the other.

The terms **extending**, **subclassing**, **inheriting from** and **deriving from** all refer to a class taking on characteristics from another class, its _base class_, or _super class_.

## What is generics?

Generic programming, or "generics", is a style of programming in which types and functions take parameters of data types that get specified later. This allow us to _generalize_ algorithms so that they work on different data types, as long as we can make sure they are compatible with the logic itself.

This means that a generic type or a function get instantiated by taken a type argument, telling it what data type it either takes as input, output, or both.

The first language that introduced generics was Ada in 1977. Later C++ came, and introduced _templates_ as its version of generics. Both Java and C# are considered part of the C language family, together with C++.

Here is what a template looks like in C++:

```c++ of same generic type, 
template<typename T>
class List {
  // Class contents.
};

List<Animal> list_of_animals;
List<Car> list_of_cars;

template<typename T>
void Swap(T& a, T& b) {
  T temp = b;
  b = a;
  a = temp;
}

std::string world = "World!";
std::string hello = "Hello, ";
Swap(world, hello);
std::cout << world << hello << ‘\n’;  // Output is "Hello, World!".
```

First Java, and then C#, would iterate on the syntax.

### Generics in Java

In 1995, Java was released. Generics was added in 2004 - in J2SE 5.0. It was implemented in the compiler by means of _type erasures_. All type parameters get removed as part of code compilation. Meaning that the JVM runtime doesn't know about type parameters. Type parameters are essentially replaced by type ``Object``. This was to not introduce big changes that would break existing code. Collections, like ``java.util.ArrayList`` without specifying type parameters continued to work.

In Java, only classes and interfaces may have type parameters.

### Generics in .NET

.NET and C# was released in 2001, and added generics to the runtime and languages in 2005 - as part of .NET Framework 2 and C# 2. The support for generics was built into the type system and the Common Language Runtime (CLR) itself. Meaning that the runtime knows about both generic types, methods, and type arguments in the code that it executes.

The types that can have generic type parameters are classes, structs, interfaces, and delegates.

The introduction of generics in .NET back in 2005 meant that developers would have to make a decision to opt into the new generic collections - going from ``System.Collections.ArrayList`` to ``System.Collections.Generic.List<T>``.

The .NET runtime generics came out of a Microsoft Research project that was headed by computer scientist [Don Syme](https://en.wikipedia.org/wiki/Don_Syme). He later came to create F#, a functional programming language for .NET, based on OCaml, that heavily utilized generics and type inference. C# has since then continually borrowed from F# and the functional programming space.

The current lead architect for the C# programming language at Microsoft, Mads Torgersen, was involved in developing Java, and in particular contributing to generics. So everything comes full circle. All languages are developed by borrowing features, skills, and talents. They are continuously improving to stay relevant.

## Syntax

Let's go through the syntax of Java and C#, respectively, when it concerns generics

### Generic classes

When it comes to defining generic types, both Java and C# fundamentally have a pretty similar syntax. Not surprising because the designers of C# were initially inspired a lot by Java, which preceded C#, so of course they borrowed.

#### Java

This is what a generic class definition looks like in Java.

```java
class MyList<T> {
    public void add(T item) {

    }

    public T first() {

    }
}
```

With multiple parameters:

```java
class MyList<T1, T2> { }
```

#### C#

Here is the basic C# generic class definition:

```csharp
class MyList<T>
{
    public void Add(T item) {

    }

    public T First() {

    }
}
```

With multiple parameters:

```csharp
class MyList<T1, T2> { }
```

### Instantiating generic types

This section is about how you instantiate (create) objects of generic types.

#### Java

In Java, the type param of the expression assigned is optional as it is inferred from the target.

```java
MyList<Foo> list = new MyList<>();
```

But it is, of course, mandatory if you assign to ``var``. (Java 17)

```java
var list = new MyList<Foo>();
```

#### C#

In C#, you have to provide the type param in the expression being assigned:

```csharp
MyList<Foo> list = new MyList<Foo>();

var list = new MyList<Foo>();
```

Unless you use this shorthand target-type initializer:

```csharp
MyList<Foo> list = new ();
```


### Subclassing (Inheritance)

The term _subclassing_ refers to a class deriving from another class. Taking on its characteristics, while still being unique. In this case, from a generic class.

#### Java

```csharp
class SuperList<T> extends MyList<T> { }

class SuperListOfFoo extends MyList<Foo> { }
```

Of course, the same applies to implementing interfaces.

#### C#

In C#, you can subclass from both open and closed generics types, and interfaces:

```csharp
class SuperList<T> : MyList<T> { }

class SuperListOfFoo : MyList<Foo> { }
```

### Generic methods

The main syntactical difference for generic method declarations is where the generic type parameter is placed.

Later, you will also see examples of generic static methods in the context of reflection.

#### Java

Java places the generic parameter list before the return type. The designers probably wanted it to make it clear when it is a generic definition.

```java
class Utils {
    public <T> void add(T item) {

    }
}
```

The syntax is similar for static methods, just that you add the ``static`` keyword.

```java
class MyStaticClass {
    public static <T> void doSomething(T item) {

    }
}
```

##### Issue with method overloading

The overloading of methods with parameters of the same generic type, but with different arguments, is not valid due to type erasure. 

The following is ambiguous to the compiler: 

```java
class Utils {
    public <T> void add(List<int> item) { }

    public <T> void add(List<string> item) { }
}
```

The compiler essentially sees both variants as the following - which causes a conflict:

```java
public <T> void add(List<Object> item){ }
```

The way to solve this in Java is to not do overloading, and to instead give each method a unique names.

#### C#

In C#, the generic parameter list comes after the name of the method. I guess so that the name of the method is in focus.

```csharp
class Utils
{
    public void Add<T>(T item)
    {
        
    }
}
```

##### Method overloading

As .NET retains information about generic type parameters, each method or type is unique by their arguments. 

So you can, unlike in Java, overload a method like this:

```csharp
class Utils
{
    public void Add<T>(List<int> item) { }

    public void Add<T>(List<string> item){ }
}
```

### Constraints

There are some significant difference when it comes to type parameter constraints, both in syntax and what the type systems of each platform support.

In Java, this feature is referred to as "Bounded type parameters".

#### Java

In Java, the constraint is inlined with the type parameter.

Here ``T`` is constrained to ``Foo`` (or derived classes):

```java
class Utils {
    public <T extends Foo> void add(T item) {

    }
}
```

This is what its looks like when constraining a type param to a particular interface (including derived interfaces):

```java
class Utils {
    public <T implement Comparable> void add(T item) {

    }
}
```

You can have multiple constraints:

```java
class Foo<T1 extends Bar, T2 extends Dough> { }
```

Java also has got a wildcard type params constraint (``?``). They refer to an unknown type.

```java
public static void paintAllBuildings(List<? extends Building> buildings) {
    ...
}
```
Assume that we have class ``House`` class derives from class ``Building``. A generic type of ``ArrayList<Building>`` is not assignable from ``ArrayList<House>`` due to the invariance in Java's type system. 

In essence, using a constrained wildcard like in the code above does make it possible to do this:

```Java
ArrayList<House> houses = new ArrayList<>();
houses.add(new House());

ArrayList<Building> buildings = houses;
```

.NET doesn't have wildcards, but there is covariance and contra-variance for interfaces using the ``in`` and ``out`` keywords prefixing the type parameter.

#### C#

In C#, the type parameter constraints are placed after either the method name, or the parameter list of a method.There are also many more constraints available than in Java. (List below)

```csharp
class MyBag<T>
    where T : Foo, IComparable
{

}
```

This is what it looks like for a method:

```csharp
class Utils
{
    public void Add<T>(T item)
        where T : Foo, IComparable
    {
        
    }
}
```

You can even have multiple constraints for different type parameters:

```csharp
class Foo<T1, T2>
    where T1 : Bar
    where T2 : Dough
{

}
```

C# has these constraints:

| Constraint                        | Descirption             |
|-----------------------------------|--------------|
| ``where T : struct``              | The type argument ``T`` must be a non-nullable value type
| ``where T : class``               | The type argument ``T`` must be a reference type
| ``where T : class?``              | The type argument ``T`` must be a nullable reference type. Valid in a nullable context.
| ``where T : notnull``             | The type argument ``T`` must be a non-nullable type.
| ``where T : default``             | This constraint resolves the ambiguity when you need to specify an unconstrained type parameter when you override a method or provide an explicit interface implementation
| ``where T : unmanaged``           | The type argument ``T`` must be a non-nullable unmanaged type
| ``where T : new()``               | The type argument ``T`` must have a public parameterless constructor
| ``where T : <base class name>``   | The type argument ``T`` must derive from the specified base class, or a class derived from it. In a nullable context 
| ``where T : <base class name>?``  | The type argument ``T`` must derive from the specified base class, or a class derived from it. Allows for null values in a nullable context.
| ``where T : <interface name>``    | The type argument ``T`` must derive from the specified interface.
| ``where T : <interface name>?``   | The type argument ``T`` must derive from the specified interface. Allows for null values in a nullable context.
| ``where T : U``                   | The type argument ``T`` must derive from type argument ``U``

The _Nullable context_ is a fancy way of saying that the feature called _"nullable reference types"_ has been enabled (Nullable ``Foo?`` vs non-nullable ``Foo``), and that the compiler will either warn or report an error if you pass a possible null value into a non-nullable variable.

## Java Type erasure

Java works on _type erasure_. In places where types are being passed as type parameters, the compiler just throws away the information of what the type was - substitutes it with ``Object``. Nothing will be emitted as part of compilation (the class files) that will tell you what type was used as an argument. But you will of course know if a class is a generic definition.

The JVM has no runtime concept of an instantiated generic class (close type). The discovery of type arguments is reliant on code trickery in order to persist that information for others to consume. We will dig into that soon.

## .NET Runtime generics

.NET has runtime support for generics. The generic type parameters are stored in the assembly - in the metadata together with the CIL bytecode. Upon executing a program, the CLR (.NET Runtime) loads all metadata, verifies it, and uses it to determine how to Just-in-time (JIT) compile the bytecode into machine code in a way that is optimized for the CPU of machine it is running on. 

The runtime is aware of generics, and make smart choices on how to allocate memory based on the type being passed as a type parameter.

## Reflection

Reflection is the ability to reflect on your program and it's types and their members. In a managed runtime environment like .NET CLR, or the JVM, this is a service provided by the runtime in the form of an API.

Reflection is a powerful feature that allows for meta programming. But it should be used carefully. Not knowing how the APIs work - where the allocation are - might lead to a performance hit. A general advice is that anything retrieved from the APIs should be cached and reused, so to not affect the performance of your application.

### Note on the design of APIs

I do think that the built in reflection API in .NET is very well designed. It is clean, an I prefer it before Java. Much is thanks to how consistent .NET is in treating types at runtime - of course, how it integrates generics.

In .NET ``Type`` represents a specific type in the type system, whether it is a class, value type, or generic type. It can represent both open generic classes (``List<>``) and closed generic classes (``List<int>``).

The API in Java, as we will see, is not that unified due to it's implementation of generics. You have to go through some extra hoops, make method calls and cast types, to retrieve the info about type parameters.

### Retrieve information about a type

This section covers how to obtain information about a type from code, whether statically from its name, or via an object instance.

#### Java

Java primarily uses the ``Class<T>`` object which represents type information about a class. The ``T`` is the actual class. Det You obtain this object either through final ``Foo.class`` field on a class, or by doing ``obj.getClass()`` on an object. 

You can do ``Foo.class`` and ``Bag<>.class`` - but not ``Bag<Coffee>.class`` or ``T.class``. Since generics is erased when code is compiled.

```java
Class<Integer> integerType = Integer.class
Class openListType = ArrayList<>.class;
Class t = Class.forName("java.util.ArrayList");

<T> void foo(Class<T> typeParam) {

}

foo(Integer.class);
```

There is also a low level ``Type`` class, from which the ``Class<T>`` is derived.

Just note, that since type parameters are erased, the generic args ``<Integer>`` of variable types may be optional - depending on your settings. Type parameters are seen as of type ``Object`` by the runtime.

``List<T>`` in Java is an interface, while in C# ``List<T>`` is a class, implement ``IList<T>``. In Java, the most common class implementing ``List<T>`` is ``ArrayList<T>``.

#### C#

In .NET have the ``Type`` class from which you can obtain information of a type at runtime, including generic type parameters, whether the class is open or closed. You have the ``typeof(Foo)`` operator which does work on generic types, both closed ``typeof(Bag<Coffee>)``, and open ``typeof(Bag<>)``. Even on the parameters ``typeof(T)``. From an instance you get its ``Type`` by invoking ``obj.GetType()``.

```csharp
var intType = typeof(int);
var intListType = typeof(List<int>);
var openListType = typeof(List<>);
var t = Type.GetType("System.Collection.List`1");

void Foo<T>() 
{
    var typeParam = typeof(T); // int
}

Foo<int>();
```

The ``int`` keyword is an alias for ``Int32`` which is a value type. In the type system of .NET, everything is an object, even values. 

In Java, ``int`` belongs to the primitive types, and has to be wrapped by the ``Integer`` class in order to be passed as an argument to a generic type parameter.

### Pass information about a type parameter into a method

This has been hinted at in previous samples. 

The way you pass info about type information into a method is different in Java, that used type erasure, compared to in a language like C# that has runtime awareness of generics.

### Java

In Java, in order to pass type information of generic type argument into a method, you have to pass its ``Class<T>`` as a method parameter. This is due to type erasure, since the method can’t resolve that type.

```java
<T> void foo(Class<T> typeParam) {
    var name = typeParam.getSimpleName();
}

Foo(Bar.class)
```

#### C#

In C#, you can simply request the info about a parameter since it has been provided by the metadata and runtime. The CLR even knows about the constraints of respective parameter.

```csharp
void Foo<T>() 
{
    var paramType = typeof(T);
    var name = typeParam.Name;
}

Foo<Bar>();
```

### Retrieve the type argument of a generic type

So how would you retrieve the type argument of a generic type in respective language?

#### Java

This is how you retrieve the actual generic argument in Java:

```java
ArrayList<Integer> list = new ArrayList<>();

Class<ArrayList> listClass = list.getClass();

Type typeArg = ((ParameterizedType) listClass.getGenericInterfaces()[0])
    .getActualTypeArguments()[0];

var typeArgClass = (Class<Object>)typeArg;
```

You could also get the ``Class`` of a type using ``Class.fromClass("java.util.ArrayList")`` or ``Class.fromClass("java.util.ArrayList<Integer>")``;

#### C# 

The approach is similar in C#, but the API a bit cleaner:

```csharp
List<int> list = new ();

Type listType = list.GetType(); // Type for List<Int32>

Type typeArg = listType.GetGenericArguments()[0]; // Type for Int32
```

You could as well get the type statically:

```csharp
Type listType = typeof(List<int>);

Type typeArg = listType.GetGenericArguments()[0]; // Type for Int32
```

### Invoke a generic static method

We will look into how to retrieve generic static methods and invoking them through reflection.

#### Java

Consider this generic static method in Java:

```java
class MyClass {
    public static <T> void myMethod(T value) {}
}
```

Since types are erased, you retrieve any method with a generic parameter as the parameter is of ``Object``:

```java
Method myMethod = MyClass.class.getMethod("myMethod", Object.class);

method.invoke(null, "Hello");
```

Due to the parameter being of type ``Object``, you can pass objects of any type as argument.

#### C#

C# retains type information for methods. You can retrieve information about an open generic methods, and then instantiate a closed version from it. The runtime will also validate arguments when invoking that method.

```csharp
class MyClass
{
    public static void MyMethod<T>(T value) {}
}
```

Here we retrieve the ``MethodInfo`` of the generic static  method, and we make a version using the provided type arguments. Then we invoke the method.

```csharp
MethodInfo myMethod = typeof(MyClass).GetMethod("MyMethod");

var myMethodString = myMethod.MakeGenericMethod(new [] { typeof(string) });

myMethodString.Invoke(null, new [] { "Hello" });
```

If you provide an incompatible type as an argument when invoking the method, the runtime will throw an exception.

### Java: An issue with serializers and generic classes

As I wanted to serialize JSON in Java, I ran into some challenges.

What if the desired class that you want to deserialize to has a generic type parameter. How do you pass that statically to the serialization method so that it knows what typ to deserialize to? 

I was using Jackson. And the API looks like this:

```java
ObjectMapper objectMapper = new ObjectMapper(); 
Foo myFoo = objectMapper.readValue(json, Foo.class);
```

The code below comes from [Baeldung](https://www.baeldung.com/java-deserialize-generic-type-with-jackson).

You pass the ```Class<T>``` to the method. But what if you can't? Like for this generic type.

```java
public class JsonResponse<T> {
    private T result;

    // getters and setters...
}

public class User {
    private Long id;
    private String firstName;
    private String lastName;

    // getters and setters...
}
```

In C#/.NET I would do like this - with System.Text.Json:

```csharp
JsonResponse<User>? weatherForecast = JsonSerializer.Deserialize<JsonResponse<User>>(jsonString);
```

In java, the solution is the _Super token_ pattern_. This implies that we wrap the type parameter in a class that retrieves the type args which can't be retrieve at runtime.

This is the type that Jackson defines:

```java
public abstract class TypeReference<T> { 
    protected final Type _type;

    protected TypeReference() {
        Type superClass = this.getClass().getGenericSuperclass();
        this._type = ((ParameterizedType)superClass).getActualTypeArguments()[0]; 
    } 
}
```

It is then used like this:

```java
TypeReference<JsonResponse<User>> typeRef = new TypeReference<JsonResponse<User>>() {};

JsonResponse<User> jsonResponse = objectMapper.readValue(json, typeRef);
User user = jsonResponse.getResult();
```

If the type is not statically known, you can just construct your own type info:

```java
JavaType javaType = objectMapper.getTypeFactory().constructParametricType(JsonResponse.class, User.class);
JsonResponse<User> jsonResponse = objectMapper.readValue(json, javaType);
```

**On a sidenote:** Constructing a ``Type`` object for closed generic type, from an open one, has an equivalent in C#/.NET:

```csharp
Type responseOfUser = typeof(JsonResponse<>).MakeGenericType([ typeof(User) ]);
```

The ``[ typeof(User) ]`` is a collection expression (Example: ``[1, 2, 3]``) that will take on the collection type of the target parameter, which is ``Object[]``. This syntax was introduced in C# 12, as a way to unify all the ways of initializing arrays and collections.

## Conclusion

This concludes my exploration of generics in Java and C#.

Having explored Java as a .NET develope, I do still prefer the way .NET runtime, the CLR, treats generic types, and types in general, at runtime. I have no problems with the syntax in Java, but it seems much harder to do advanced things without this extra type information at runtime. I simply can express it better in C# and .NET.

Is type erasure and the subsequent lack of runtime support for generics really an issue? 

Not unless you write a framework that relies heavily on reflection - like a serializer - and you have to take generics into account. And of course for me as someone who is used to the runtime generics of .NET. Overall, it might be hard to get into it as a beginner, but you might advance in it, and learn stuff that few others master.

I hope you liked this walkthrough.