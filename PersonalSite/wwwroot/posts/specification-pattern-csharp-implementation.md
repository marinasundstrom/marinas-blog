---
title: "Specification"
---
*The topic described in this article is a part of my [Specification Pattern in C# Pluralsight](http://test) course.*

Specification pattern is not a new topic, there are many of its implementations on the Internet already. In this post, I’d like to discuss the use cases for the pattern and compare several common implementations to each other.

## 1. Specification pattern: what’s that?

Specification pattern is a pattern that allows us to encapsulate some piece of domain knowledge into a single unit - specification - and reuse it in different parts of the code base.

Use cases for this pattern are best expressed with an example. Let’s say we have the following class in our domain model:

```c#
public class Movie : Entity
{
    public string Name { get; }
    public DateTime ReleaseDate { get; }
    public MpaaRating MpaaRating { get; }
    public string Genre { get; }
    public double Rating { get; }
}
 
public enum MpaaRating
{
    G,
    PG13,
    R
}
```

Now, let’s assume that users want to find some relatively new movies to watch. To implement this, we can add a method to a repository class, like this:

```c#
public class MovieRepository
{
    public IReadOnlyList<Movie> GetByReleaseDate(DateTime minReleaseDate)
    {
        /* ... */
    }
}
```