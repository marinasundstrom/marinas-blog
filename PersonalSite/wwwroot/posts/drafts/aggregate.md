---
title: Separating the aggregate from the root
subtitle: Designing for your context
published: 2022-10-21
tags: [Software design, Domain driven design]
---

An "Aggregate" is a relationship between multiple entities, and aggregates. The "root" entity of the aggregate is called the "Aggregate Root".

The purpose of the aggregate root is to form a boundary that ensures data and behavior consistency across dependant entities. Just like each entity encapsulates its own data and behavior, the aggregate root should do so for all entities that form the aggregate.

Unfortunately, there is still much to wrap your head around. Like, how do you implement this in practice?

In this article we are going to separate the concept of an Aggregate from the concept an Aggregate Root.

## Our model

Let us assume that we have a model of a product catalog, like this one:

![img](/posts/diagram1.png)

In C# code, it would look like this:


```c#
public class Product 
{
    public Guid Id { get; private set; }

    public string Name { get; set; }

    public ProductAvailability Availability { get; set; }

    public decimal Price { get; set; }
    
    public ProductGroup Group { get; set; } = null!;

    public void DoSomething() {}
}

public class ProductGroup
{
    public Guid Id { get; private set; }

    public string Name { get; set; }

    public int ProductCount { get; private set; }

    public HashSet<Product> Products { get; } = new HashSet<Product>();

    public void AddProduct(Product product) 
    {
        Products.Add(product);
        ProductCount++;
    }
}
```

These are two entities that are mutually depending on each other. It might be intuitive to say that there is just one aggregate but, there are in fact two aggregates. But which of them is the root? Well, it depends on context. 

Consider these cases:

1. The user wants to update the Product information. It makes sense to see it from the perspective of the Product. 

2. The user wants to add a Product to a Product Group. Then the Product Group is the subject that has to ensure its own consistency.

In each case, the model should have defined behavior that just allows certain changes within that context. If we assume that the ProductGroup is responsible for adding the Product to itself then we cannot allow other entities to either intentionally, or unintentionally,access that behavior. Remember that we want to ensure consistency.

Given our model above it is possible to do something like this:

```c#
Product product = GetProduct();
product.ProductGroup.AddProduct(product);
```

We can access the behavior belonging to the aggregate root entity via a navigation property. This might introduce inconsistencies if someone comes up with the idea of doing this. So we want to design our model so that this is not possible. It makes for some horrible code as well.

Remember that we in some cases regard the Product as the Product Aggregate Root. ProductGroup. These are two contexts with different behavior attached to each. So instead we can split each 

![img](/posts/diagram2.png)

In the improved code we have split the Product into two types: the Product that is an aggregate root, and the one that is just taking part in an aggregate. They represent the same thing, but might expose different data and behavior. If you load ProductGroup as part of the ProductAggRoot then you should not be able to modify its properties or add a product. That behavior doesn't exist here. 


```c#
public class ProductAggRoot 
{
    public Guid Id { get; private set; }

    public string Name { get; set; }

    public ProductAvailability Availability { get; set; }

    public decimal Price { get; set; }
    
    public ProductGroup Group { get; private set; } = null!;

    public void DoSomething() {}
}

public class ProductGroup
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public int ProductCount { get; private set; }
}

public class ProductGroupAggRoot 
{
    public Guid Id { get; private set; }

    public string Name { get; set; }

    public int ProductCount { get; private set; }

    public HashSet<Product> Products { get; } = new HashSet<Product>();

    public void AddProduct(Product product) 
    {
        Products.Add(product);
        ProductCount++;
    }
}

public class Product 
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public ProductAvailability Availability { get; private set; }

    public decimal Price { get; private set; }
    
    public string GroupId { get; set; } = null!;
}
```

Now we ensure that when we are accessing the ProductGroup from the "ProductAggRoot" we will not access any behavior that we should not. This behavior is not part of our context.

------------


1 to many relationships
You want to include an entity in another context

Separate aggregate from the aggregate root

Entites equal by identity - identifier
That is how we refer to them - by id

Entities come together into aggregates
Aggregates formed by aggregates
The root entity is the aggregate
Aggregate roots are consisticy boundaries

Domain and persistence is separate concerns. Domain model vs data model two different concepts.

As . Net devs we tend to merge EF data model and domain model into one. There are advantages to that. But it might limit our way of thinking. 

Instead, we should vhave different specialed models/entities mapping to the same table.

A product is not the same when part as an aggregate vs being the root. If you have rich domain model then therr diff behavior

Conytext decides

When you fetch a product group including the products you might not want allorop

Domain objects are views

Group(entity agg)
Group (Agg Roor) - Includes many to one relationahipa

When