# Structural Design Patterns: Bridge

Today we continue our journey through the common set of `Structural` design patterns within the larger [`Guide to Software Design Patterns`](https://airbrake.io/blog/software-design/software-design-patterns-guide) series.  Next on the docket is the `bridge` design pattern, which is intended to decouple the abstractions of an multiple objects from their actual implementation, so changing one will not effect the other.

The goal of the `bridge` pattern is to effectively allow for two (or more) levels of abstraction, whereby one category of object (the `implementor`) can be altered and have no impact on the second category of object (the `bridge`), which can, in turn, also be altered and abstracted without impacting the `implementor`.  Many applications and programming frameworks use the `bridge` pattern to help handle UI/UX components.  For example, an object used to define the `layout` of a component might be abstracted and used in combination with an object used to `render` the visual output of that same component.  By using the `bridge` pattern and abstracting the `layout` and the `render` objects away from one another both can be altered without them impacting one another.

Throughout this article we'll closely examine the `bridge` pattern by taking a look at a real world example where the `bridge` pattern is used, as well as some functional `C#` code examples of the `bridge` pattern in action, so let's get started!

## In the Real World

Expanding a bit on the visual example used above, in the real world we often see the `bridge` pattern used by book publishers when determining the different styles of book binding used to produce physical books.  There are many different types of binding ranging from perfect binding (which most of us are used to) to spiral binding and even saddle-stitch binding, which typically just folds the whole set of paper sheets directly in half at the seam.

Additionally, there are countless novels, both new and old, and they often can be categorized into a number of unique genres such as fantasy, science fiction, and mystery.  When it comes time to publish the publisher must decide which book to publish and what binding type(s) to produce.  In many cases the genre of the book may be a big factor in what type of bindings are used, since it'd probably be uncommon to see massive fantasy novels printed using saddle-stitch binding due to the sheer volume of pages and inability to easily fold them together.

In these publishing scenarios the combination of `genre` and `binding` are two levels of abstraction that are combined using a `bridge` pattern.  No matter what changes are made to the way a particular `genre` is handled -- nor how a specific `binding` is created -- changes to one will never impact the other.

## How It Works In Code

Taking that same book publishing concept into our example code seems to make a lot of sense, so below we have a working example that creates a number of abstractions based on the `IBinding` interface and the `IBook` interface.  We can then combine both interface abstractions to create a single book instance based on a `genre` and whatever `binding` we want.  As usual, it'll be easiest if we start with the full code example, after which we'll step through the sections one at a time and explain what's going on.

```cs
using Utility;

namespace Bridge
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a new binding instance.
            var spiralBinding = new SpiralBinding();
            // Create a fantasy book instance using spiral binding.
            var fantasyBook = new FantasyBook("The Name of the Wind", "Patrick Rothfuss", spiralBinding);
            // Create a different book type using the same spiral binding instance.
            var sciFiBook = new ScienceFictionBook("Dune", "Frank Herbert", spiralBinding);
        }
    }

    /// <summary>
    /// Basic interface for book binding types with one Name method.
    /// </summary>
    interface IBinding
    {
        string Name { get; }
    }

    /// <summary>
    /// Base class for all binding class types.
    /// </summary>
    class Binding : IBinding
    {
        /// <summary>
        /// Retrieves the name of the current binding class.
        /// </summary>
        public string Name
        {
            get
            {
                return this.GetType().Name;
            }
        }
    }

    class PerfectBinding : Binding { }

    class SpiralBinding : Binding { }

    class SaddleStitchBinding : Binding { }

    /// <summary>
    /// Basic interface for books with Author, Binding, and Title.
    /// </summary>
    interface IBook
    {
        string Author { get; set; }
        Binding Binding { get; set; }
        string Title { get; set; }
    }

    /// <summary>
    /// Base class for all book genre types.  Assigns passed arguments as basic property values and 
    /// outputs creation message with book class type, title, author, and binding name.
    /// </summary>
    class Book : IBook
    {
        public string Author { get; set; }
        public Binding Binding { get; set; }
        public string Title { get; set; }

        public Book(string title, string author, Binding binding)
        {
            Author = author;
            Binding = binding;
            Title = title;
            Logging.Log($"Created {this.GetType().Name} of \"{title}\" by {author} using {binding.Name}.");
        }
    }

    class MysteryBook : Book
    {
        public MysteryBook(string title, string author, Binding binding) : base(title, author, binding) { }
    }

    class FantasyBook : Book
    {
        public FantasyBook(string title, string author, Binding binding) : base(title, author, binding) { }
    }

    class ScienceFictionBook : Book
    {
        public ScienceFictionBook(string title, string author, Binding binding) : base(title, author, binding) { }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Utility
{
    /// <summary>
    /// Houses all logging methods for various debug outputs.
    /// </summary>
    public static class Logging
    {
        /// <summary>
        /// Outputs to <see cref="System.Diagnostics.Debug.WriteLine"/> if DEBUG mode is enabled,
        /// otherwise uses standard <see cref="Console.WriteLine"/>.
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        public static void Log(string value)
        {
#if DEBUG
            Debug.WriteLine(value);
#else
            Console.WriteLine(value);
#endif
        }
    }

    // ...
}
```

Let's begin with how we handing the bindings:

```cs
/// <summary>
/// Basic interface for book binding types with one Name method.
/// </summary>
interface IBinding
{
    string Name { get; }
}

/// <summary>
/// Base class for all binding class types.
/// </summary>
class Binding : IBinding
{
    /// <summary>
    /// Retrieves the name of the current binding class.
    /// </summary>
    public string Name
    {
        get
        {
            return this.GetType().Name;
        }
    }
}

class PerfectBinding : Binding { }

class SpiralBinding : Binding { }

class SaddleStitchBinding : Binding { }
```

We start with the `IBinding` interface that contains just a single `Name` property, which will later help us illustrate which specific binding is being used.  To make things easier for our unique binding abstractions later on we inherit the `IBinding` interface in a `Binding` class that actually implements our `Name.get()` property method.  From there we can create our binding classes (`PerfectBinding`, `SpiralBinding`, and `SaddleStitchBinding`), which could all have their own unique logic if needed, though we don't need to include anything more in this simple example.

With our `Binding` abstraction classes created we then move onto the second object we want to abstract, which is the `IBook` interface:

```cs
/// <summary>
/// Basic interface for books with Author, Binding, and Title.
/// </summary>
interface IBook
{
    string Author { get; set; }
    Binding Binding { get; set; }
    string Title { get; set; }
}

/// <summary>
/// Base class for all book genre types.  Assigns passed arguments as basic property values and 
/// outputs creation message with book class type, title, author, and binding name.
/// </summary>
class Book : IBook
{
    public string Author { get; set; }
    public Binding Binding { get; set; }
    public string Title { get; set; }

    public Book(string title, string author, Binding binding)
    {
        Author = author;
        Binding = binding;
        Title = title;
        Logging.Log($"Created {this.GetType().Name} of \"{title}\" by {author} using {binding.Name}.");
    }
}

class MysteryBook : Book
{
    public MysteryBook(string title, string author, Binding binding) : base(title, author, binding) { }
}

class FantasyBook : Book
{
    public FantasyBook(string title, string author, Binding binding) : base(title, author, binding) { }
}

class ScienceFictionBook : Book
{
    public ScienceFictionBook(string title, string author, Binding binding) : base(title, author, binding) { }
}
```

These require a bit more code since we're actually passing in arguments and using the parameters to set property values, but overall the idea is similar to when we created our bindings.  The `IBook` interface specifies the `Author`, `Title`, and (most importantly) `Binding` properties.  Again we inherit `IBook` into the base `Book` class to make abstraction and inheritance easier for our genre-specific classes later on, which is where most of the instantiation and logic takes place.  Of particular importance is the `Logging.Log()` method call, which acts as a simple way to output information about the book we're creating, the genre, and the specific binding associated with it.  Finally we create some genre-specific book classes of `MysteryBook`, `FantasyBook`, and `ScienceFictionBook`, each of which inherit from our base `Book` class.

With our binding and book objects ready to go we can now make use of this `bridge` pattern, which we do in the `Program.main()` method:

```cs
class Program
{
    static void Main(string[] args)
    {
        // Create a new binding instance.
        var spiralBinding = new SpiralBinding();
        // Create a fantasy book instance using spiral binding.
        var fantasyBook = new FantasyBook("The Name of the Wind", "Patrick Rothfuss", spiralBinding);
        // Create a different book type using the same spiral binding instance.
        var sciFiBook = new ScienceFictionBook("Dune", "Frank Herbert", spiralBinding);
    }
}
```

We start by instantiating a new `SpiralBinding` object, then we create two unique books, one of which is a `FantasyBook` and the other a `ScienceFictionBook`.  What's important to note here is that since all of our genre-specific book classes inherit from the `Book` class, the third argument we pass must be a `Binding`-inherited class, which all our binding type classes are.  However, there's no logic or limitations within the `Book` (or subclass) implementations that specify anything about how the associated binding will behave.  As a result, we can freely mix and match various types of bindings with various types of book genres, making logical changes to both without fear of impacting the other.  This is power of the `bridge` design pattern at work.

To confirm that all is working as expected let's take a look at the `Logging.Log()` output we produced when instantiating our books:

```
Created FantasyBook of "The Name of the Wind" by Patrick Rothfuss using SpiralBinding.
Created ScienceFictionBook of "Dune" by Frank Herbert using SpiralBinding.
```

Sure enough both unique book classes were able to use the same binding type since they're effectively "binding agnostic" now that we've properly implemented them using the `bridge` pattern.

---

__META DESCRIPTION__

Part 8 of our Software Design Pattern series in which examine the Bridge design pattern using fully-functional C# code examples.