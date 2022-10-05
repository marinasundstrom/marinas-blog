---
title: "How does it compute?"
subtitle:  "The Human Computer"
published: 2021-11-10
tags: [Computing, Computer science, Programming]
---

*The idea of this article came about this summer, after having a conversation about computers with a car mechanic. This how I would have explained how a computer operates to him.*

---

In this exercise, you will play the part of a human computer. 

The goal is for you to learn how a computer interprets and executes instructions, using real-world analogies, and an imaginary machine language.

## The Memory

Imagine a piece of tape that is divided into equal squares - or slots. This is our computer memory. The content of these slots will be what is important to us later.

When we are starting, we are at the first slot at the start of the tape. We keep track of this location.

Each slot is numbered in the order that they occur. This number is our “address”. We are currently at slot 1 with the address 0. 

Programmers like to start at zero because we have not really advanced in this list yet. We usually refer this position as an "index".

Being able to address slots will be important when executing our program. 

The tape is our "working memory" - what in a computer would be the *[Random-access Memory](https://en.wikipedia.org/wiki/Random-access_memory)*, or *RAM*. It will be allocated for as long as the program runs, and after that, the data will be throw away.

## Executing instructions

At address 0 (slot 1) we find some data, in our case a number: 62. This number can be interpreted in two ways: Either as just a number value or as an instruction.

An instruction tells us that we ought to do something.

Since this is a computer, we would expect that the first slot that we scan is an instruction. The kind of instruction is determined by the number.

An instruction can take arguments that it operate on depending on the instruction. 

Our instruction 62 tells us to load the value of the next slot on the tape into a separate “box” called 1.

```
| 62 | 5 |  - Load value 5 into Box A
```

In processor design, we call such a special box a [“register”](https://en.wikipedia.org/wiki/Processor_register). They are meant for efficiently storing data when performing operarations. It is fast since it is a part of the CPU.

We now have two important concepts: The ***Memory*** that is divided into addressable *Slots* containing instructions and data, and a limited number of ***Boxes*** in which we can store values used when computating.

When we have executed the instruction we have advanced past the slots containing both the instruction and eventual arguments. Then we also keep this address of this new location in our head, because this will be our next instruction to execute. The new address is 2.

In low level computer-lingo, this location where we store the address of the next instruction is called a [“Program Counter” or "Instruction Pointer"](https://en.wikipedia.org/wiki/Program_counter). In processors this is its own special "Box", or Register.

The next instruction is 24. It is an add-operation taking 2 arguments: The number of the box, and then a value to add to the value in the specified box. It will store the result in the box.

```
| 24 | 1  | 10 |  // Add 10 to the value in Box A.
```

In pseudo code the operation is expressed as:

```
[Box A] := [Box A] + 10  // Box A contains the value 5
````

After the operation, the value in Box A will be: 15

The full program: ```| 62 | 5 | 24 | 1 | 10 |```

And our current address is 5. We have done our first computation. :) 

Storing data in these boxes are not optimal, since they are limited in number and will be used when performing operations.

We can instead allocate a certain “address space” on the tape for storing data while the program is being executed. This will require memory management on the part of the computer. Compilers do this for you when writing code in a high-level language like C. Now you are both the compiler and the computer 🙂 

For the sake of demonstrating, an instruction for storing the data of a box in memory would be:

```
| 75 | 9 | 1 |  // Store the value of Box A in memory slot with absolute address 42
```

Updated full program: ```| 62 | 5 | 24 | 1 | 10 | 75 | 9 | 1 |```

And this is what the memory would look like: ```| 62 | 5 | 24 | 1 | 10 | 75 | 9 | 1 | 0 | 15  | 0 | 0 |  ...```

The value 15 has been stored at address 9. The 0's represent the rest of the unused memory that continues to infinity... not really.

It is possible to overwrite values in slots where there is an instruction. That would possibly lead to a crash if the value of these slots executed as instructions

## More on memory, addressing, and branching

Once we have stored our data in the working memory (the tape) we can do interesting things with it, like storing and loading data in any form. For example, a sequence of slots with each containing a value to be interpreted as a character (letter or digit). This is called a string (of characters). What is stored in a slot is really just a representation of something. At this level, how we interpret the data is entirely up to the programmer. Is this it a number, or is it a character?

We use addresses to refer to that data. Sometimes addresses are absolute or relative to the current address depending on instruction or context. Jump 3 slots ahead or back is an example of a relative branch instruction that changes the program counter relative to the current value.

A common case for branching is to test a condition, and then, depending on whether the result was true or false, jump to another slot, and instruction. That is how you achieve [control flow](https://en.wikipedia.org/wiki/Control_flow), like “if” and “while” statements, but in machine code.

We can group parts of the program into reusable *[subroutines](https://en.wikipedia.org/wiki/Subroutine)* that are invoked through the address of its first instruction. Usually, when writing code in high-level langauges, we assign identifiers, or labels, to refer to where addresses that we want to branch to are.

## Data and Instructions

Having the ability to conditionally branch makes the computer *[Turing complete](https://en.wikipedia.org/wiki/Turing_completeness)*, meaning that you can encode and perform any mathematical computation using this infrastructure - thanks to having a memory containing data and instructions, and the possibility to branch and execute.

As you probably have noticed: There is no real distinction between data and code at a machine-level. Code is just data. It is simply about how the processor interprets the data.  

## Peripherals

For a computer, addresses are also important when communicating with any of its parts, such as the graphics unit in order to draw something on the screen. You write values to its memory to make it do stuff.

There are *buses*, which are specialized data channels through which you can communicate with all kinds of peripherals - graphics devices, storage units, and web cams etc. This is out of scope for this article.

## Conclusion

In summary, this is essentially what your computer does: It jumps around on a tape of slots; reading, interpreting, and writing values on that tape.

This has been a short introduction to how computers, more specifically a processor, work.

Hopefully, you know understand the concepts on which modern computing is based. 