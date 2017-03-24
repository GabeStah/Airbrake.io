# Creational Design Patterns: Factory Method

Today, as we continue our journey through our [`Guide to Software Design Patterns`](https://airbrake.io/blog/software-design/software-design-patterns-guide), we'll keep on the path of `Creational` design by taking a closer look at the `factory method` pattern.  The `factory method` is similar to the [`simple factory`](https://airbrake.io/blog/design-patterns/simple-factory) pattern that we explored previously, except the `factory method` provides a simple way to further abstract the underlying class (and associated logic) from the client that is making use of our factory.

In this article we'll cover what the `factory method` is, using both real world examples and functional `C#` code, so let's get going!

## In the Real World

Just as with the `simple factory` article we published previously, the `factory method` pattern hinges on that core concept of a `factory`.  Like real world factories, our code should be able to make use of an _intermediary_ `factory` class of some sort, which eases the rapid production of objects for the client (i.e. the developer making use of our classes).  If setup correctly, the client can easily use of code without knowing (or caring) about how everything works behind the scenes; the `factory` should take care of the work for us.

To continue with my love of books, for our real world example of implementing the `factory method` we'll explore the relationships between `authors` and `publishers`.  There are many different types of `authors`, including those that specialize in `fiction` and those that prefer `nonfiction`.  And, just like `authors`, different `publishers` may opt to focus on publishing the works of `authors` that specialize in particular fields or writing styles.

For example, a `newspaper` is a type of `publisher` that primarily focuses on publishing `nonfiction authors`.  Similarly, a `blog` that wants funny cat gifs and articles with titles like _Top 10 Zaniest Things You Never Knew Were So Zany!_ is likely to publish stuff from `fiction authors`.  The publication process may not only differ from one `publisher` to the other, but also the types of `authors` working for them will be dramatically different.

However, some things will remain the same.  For example, a `publisher` will always perform a few basic tasks, such as `hiring an appropriate author` and `publishing`.  Therefore, the baseline concept of a `publisher` acts as the `factory method` for this process, abstracting and separating the **types** of `publishers` from the **types** of `authors`.

## How It Works In Code

Our code example continues with the `publisher` and `author` analogy.  First, here's the code example in full, after which we'll break down the components to illustrate what is going on:

```cs
using Utility;

namespace Airbrake.DesignPatterns.FactoryMethod
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a Blog and publish.
            var blog = new Blog();
            blog.Publish();

            // Create a Newspaper and publish.
            var newspaper = new Newspaper();
            newspaper.Publish();
        }
    }

    interface IAuthor
    {
        void Write();
    }

    class FictionAuthor : IAuthor
    {
        public void Write()
        {
            Logging.Log($"I'm an {this.ToString()} and I write fiction!");
        }
    }

    class NonfictionAuthor : IAuthor
    {
        public void Write()
        {
            Logging.Log($"I'm an {this.ToString()} and I write nonfiction!");
        }
    }

    abstract class Publisher
    {
        abstract public IAuthor HireAuthor();

        public void Publish()
        {
            IAuthor author = HireAuthor();
            author.Write();
        }
    }

    class Blog : Publisher
    {
        public override IAuthor HireAuthor()
        {
            return new FictionAuthor();
        }
    }

    class Newspaper : Publisher
    {
        public override IAuthor HireAuthor()
        {
            return new NonfictionAuthor();
        }
    }
}

using System;
using System.Diagnostics;

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
        public static void Log(object value)
        {
#if DEBUG
            Debug.WriteLine(value);
#else
            Console.WriteLine(value);
#endif
        }
    }
}
```

The basic goal here of our `factory method` is to separate the logic of `hiring` an appropriate type of `author` from the particular type of `publisher`.  Therefore, we start with a basic `IAuthor` interface, which contains the baseline of what our `Author` sub-classes must include: the method to `Write()`.  From there, we create two unique `types` of `Authors`, the `FictionAuthor` and the `NonfictionAuthor`, both of which must include the `Write()` method:

```cs
interface IAuthor
{
    void Write();
}

class FictionAuthor : IAuthor
{
    public void Write()
    {
        Logging.Log($"I'm an {this.ToString()} and I write fiction!");
    }
}

class NonfictionAuthor : IAuthor
{
    public void Write()
    {
        Logging.Log($"I'm an {this.ToString()} and I write nonfiction!");
    }
}
```

Now we create our `Publisher` class, which contains the core component of our `factory method` pattern, and is where the crossover between `Author` and `Publisher` takes place:

```cs
abstract class Publisher
{
    abstract public IAuthor HireAuthor();

    public void Publish()
    {
        IAuthor author = HireAuthor();
        author.Write();
    }
}
```

In this case, we don't want the client to be able to create an instance of a basic `Publisher` class, so we make it `abstract`.  We then include a `HireAuthor()` method, which returns our `IAuthor` interface.  Finally, the fundamental purpose of a `publisher` is to publish work, so we add the `Publish()` method, inside which we instantiate a generic `IAuthor` using the `HireAuthor()` method, before forcing our `author` to `Write()`.

The last part of our structure is to create some non-abstract `publications`, in this case `Blog` and `Newspaper`.  Since both inherit from `Publisher`, we must include (and `override`) the `HireAuthor()` method.  These `HireAuthor()` methods each return an appropriate **type** of `Author`: the `FictionAuthor` and `NonfictionAuthor`, respectively:

```cs
class Blog : Publisher
{
    public override IAuthor HireAuthor()
    {
        return new FictionAuthor();
    }
}

class Newspaper : Publisher
{
    public override IAuthor HireAuthor()
    {
        return new NonfictionAuthor();
    }
}
```

What's cool about this `factory method` pattern is that the implementation (and logic) of `Blog.HireAuthor()` and `Newspaper.HireAuthor()` can be different.  However, since `Publisher.Publish()` simply accepts whatever `IAuthor` is provided, and issues a `Write()` method command, the client doesn't need to know what's going on behind the scenes here.  For example, here's a bit of our example code that makes use of both types of `Publishers`:

```cs
static void Main(string[] args)
{
    // Create a Blog and publish.
    var blog = new Blog();
    blog.Publish();

    // Create a Newspaper and publish.
    var newspaper = new Newspaper();
    newspaper.Publish();
}
```

The client is able to freely create a `Blog` and a `Newspaper`, issuing the `Publish()` command for both, without knowing how the `factory method` logic works in the background.  The result, as expected, is that our output shows that appropriate types of `IAuthors` were generated for each, in accordance with the type of author each type of publication expects:

```
I'm an Airbrake.DesignPatterns.FactoryMethod.FictionAuthor and I write fiction!
I'm an Airbrake.DesignPatterns.FactoryMethod.NonfictionAuthor and I write nonfiction!
```

While this is just a simple example of the `factory method` in action, it hopefully illustrates the purpose and power of this pattern.  Our `Publisher` class could easily be further expanded and improved, and so long as it still makes use of the `IAuthor's` ability to `Write()` somewhere, any inherited classes from `Publisher` can continue to be referenced as in our above example, without ever knowing how the authorship or writing process works behind the scenes.

---

__META DESCRIPTION__

Part 2 of our Software Design Pattern series, in which we carefully examine the Factory Method design pattern, including C# example code.