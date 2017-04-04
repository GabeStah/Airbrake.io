# Creational Design Patterns: Abstract Factory

Moving right along through our [`Guide to Software Design Patterns`](https://airbrake.io/blog/software-design/software-design-patterns-guide), today we're going to take a deeper dive into the next `Creational` design pattern on the list: the `abstract factory`.  The `abstract factory` is the final `factory` pattern we'll be exploring in this series and, perhaps unsurprisingly, it is the most robust and detailed of all three `factory` patterns we'll be covering.  Simply put, the `abstract factory` allows you to create `a factory of factories` -- a way to group similar `factories` together, without knowledge of their underlying classes or behavior.

Throughout this article we'll examine what the `abstract factory` design pattern is by using a real world illustration, along with a functional `C#` code example, so let's get started!

## In the Real World

As with other `factory` pattern types, the `abstract factory` relies heavily on the concept of a real world factory in order to rapidly recreate similar objects, without any knowledge (nor understanding) of what precise steps go into creating a particular product.  Sticking with our theme of writing, books, and publication, the real world example of an `abstract factory` pattern expands on these concepts even further.

For example, imagine we have two authors, Edgar Allan Poe and Charles Darwin, both looking to publish their (arguably) most famous works, _The Raven_ and _On the Origin of Species_, respectively.  Since one is a poet and the other a scientist, the _types_ of work they've created are quite different (`poem` and `research paper`).  Moreover, given those disparate types of writings, it's unlikely that they'll both be using the same publisher.  As it happens, _The Raven_ was first published in 1845 by a periodical called _The American Review_, while _On the Origin of Species_ was published in 1859 by _John Murray_, an eclectic English publishing firm.

Since different types of authors produces different kinds of writing -- which are likely to be published by dramatically different publishers -- this is a great use of the `abstract factory` method.  To simplify our comparison, we'll modernize the publisher types a bit and use a `blog` for publishing `poems` and a `scientific journal` for publishing `research papers`.  If we have a `poem` like _The Raven_ ready to be published, we _need_ an appropriate type of publisher for it, such as a `Blog`.  Similarly, a `scientific paper`, like _On the Origin of Species_, might require a `scientific journal` for publication.

We've now created a relationship, which is the heart of the `abstract factory` design pattern.  `Poems` now require a `blog` to publish, while a `scientific paper` requires a `scientific journal` to publish.  This dependency is what defines the `abstract factory`, as we'll see in our code below.

## How It Works In Code

To illustrate the `abstract factory` pattern in code, for your convenience, we'll begin by showing the full code example right away.  Afterward, we'll dissect the code to see how the `abstract factory` was created.

```cs
using Utility;

namespace Airbrake.DesignPatterns.AbstractFactory
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a PoemFactory, generating a Poem book type and a Blog publisher type.
            new PoemFactory(author: "Edgar Allan Poe", 
                            title: "The Raven",
                            publisher: "The American Review");

            // Create a ResearchPaperFactory, generating a ResearchPaper book type
            // and a ScientificJournal publisher type.
            new ResearchPaperFactory(author: "Charles Darwin", 
                                     title: "On the Origin of Species", 
                                     publisher: "John Murray");
        }
    }

    public interface IBook
    {
        string Author { get; set; }
        string Title { get; set; }
    }

    public class Poem : IBook
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public Poem(string author, string title)
        {
            Author = author;
            Title = title;
            Logging.Log($"Made an IBook of type: {this.ToString()}.");
        }
    }

    public class ResearchPaper : IBook
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public ResearchPaper(string author, string title)
        {
            Author = author;
            Title = title;
            Logging.Log($"Made an IBook of type: {this.ToString()}.");
        }
    }

    public interface IPublisher
    {
        string Name { get; set; }
    }

    public class Blog : IPublisher
    {
        public string Name { get; set; }
        public Blog(string name)
        {
            Name = name;
            Logging.Log($"Made an IPublisher of type: {this.ToString()}.");
        }
    }

    public class ScientificJournal : IPublisher
    {
        public string Name { get; set; }
        public ScientificJournal(string name)
        {
            Name = name;
            Logging.Log($"Made an IPublisher of type: {this.ToString()}.");
        }
    }

    public interface IBookFactory
    {
        IBook MakeBook(string author, string title);
        IPublisher MakePublisher(string name);
    }

    public class PoemFactory : IBookFactory
    {
        public PoemFactory() { }

        public PoemFactory(string author, string title, string publisher)
        {
            MakeBook(author: author, title: title);
            MakePublisher(name: publisher);
            Logging.Log($"Made an IBookFactory of type: {this.ToString()}.");
        }

        public IBook MakeBook(string author, string title)
        {
            return new Poem(author: author, title: title);
        }
        
        public IPublisher MakePublisher(string name)
        {
            return new Blog(name: name);
        }
    }

    public class ResearchPaperFactory : IBookFactory
    {
        public ResearchPaperFactory() { }

        public ResearchPaperFactory(string author, string title, string publisher)
        {
            MakeBook(author: author, title: title);
            MakePublisher(name: publisher);
            Logging.Log($"Made an IBookFactory of type: {this.ToString()}.");
        }

        public IBook MakeBook(string author, string title)
        {
            return new ResearchPaper(author: author, title: title);
        }

        public IPublisher MakePublisher(string name)
        {
            return new ScientificJournal(name: name);
        }
    }
}
```

The first part of our `abstract factory` is the creation of two unique types of objects; in this case: `Books` and `Publishers`.  Thus, we begin by defining our unique types of `Books`:

```cs
public interface IBook
{
    string Author { get; set; }
    string Title { get; set; }
}

public class Poem : IBook
{
    public string Author { get; set; }
    public string Title { get; set; }
    public Poem(string author, string title)
    {
        Author = author;
        Title = title;
        Logging.Log($"Made an IBook of type: {this.ToString()}.");
    }
}

public class ResearchPaper : IBook
{
    public string Author { get; set; }
    public string Title { get; set; }
    public ResearchPaper(string author, string title)
    {
        Author = author;
        Title = title;
        Logging.Log($"Made an IBook of type: {this.ToString()}.");
    }
}
```

Nothing fancy going on here.  We've created the `IBook` interface with a few properties, then we create two unique classes that inherit `IBook`, `Poem` and `ResearchPaper`.  Both simply assign values for the `Author` and `Title` properties, then output the name of the class when initialized.  _Note: Since we've used it in a few different articles already, I've excluded the `Utility` namespace and `Logging` class code, as it's just a convenience for generating output._

Next, we'll follow a similar pattern to create our `IPublisher` interface, along with two unique classes that inherit from it:

```cs
public interface IPublisher
{
    string Name { get; set; }
}

public class Blog : IPublisher
{
    public string Name { get; set; }
    public Blog(string name)
    {
        Name = name;
        Logging.Log($"Made an IPublisher of type: {this.ToString()}.");
    }
}

public class ScientificJournal : IPublisher
{
    public string Name { get; set; }
    public ScientificJournal(string name)
    {
        Name = name;
        Logging.Log($"Made an IPublisher of type: {this.ToString()}.");
    }
}
```

This pattern is exactly the same as when creating our `Book` type classes, but now we have two unique types of `Publishers`, `Blog` and `ScientificJournal`.

Now we can get to the juicy bits of actually implementing our `abstract factory`.  The key is to create a `factory` that makes use of both our `IBook` and `IPublisher` interfaces.  That is accomplished with the `IBookFactory` interface that is immediately below:

```cs
public interface IBookFactory
{
    IBook MakeBook(string author, string title);
    IPublisher MakePublisher(string name);
}

public class PoemFactory : IBookFactory
{
    public PoemFactory() { }

    public PoemFactory(string author, string title, string publisher)
    {
        MakeBook(author: author, title: title);
        MakePublisher(name: publisher);
        Logging.Log($"Made an IBookFactory of type: {this.ToString()}.");
    }

    public IBook MakeBook(string author, string title)
    {
        return new Poem(author: author, title: title);
    }
    
    public IPublisher MakePublisher(string name)
    {
        return new Blog(name: name);
    }
}

public class ResearchPaperFactory : IBookFactory
{
    public ResearchPaperFactory() { }

    public ResearchPaperFactory(string author, string title, string publisher)
    {
        MakeBook(author: author, title: title);
        MakePublisher(name: publisher);
        Logging.Log($"Made an IBookFactory of type: {this.ToString()}.");
    }

    public IBook MakeBook(string author, string title)
    {
        return new ResearchPaper(author: author, title: title);
    }

    public IPublisher MakePublisher(string name)
    {
        return new ScientificJournal(name: name);
    }
}
```

Once our `IBookFactory` is defined, we then expand on that to create two unique `factory` classes, the `PoemFactory` and the `ResearchPaperFactory`.  We've given ourselves the option of initializing either an empty version of `PoemFactory`, or passing parameters to immediately call the `MakeBook` and `MakePublisher` methods that were inherited from our `IBookFactory` interface.

What makes this `abstract` is that each unique type of `factory` is able to independently generate and return an appropriate `Book` type class and `Publisher` type class, but the `factory` itself is none the wiser.  For example, notice in the `PoemFactory.MakeBook()` method we're creating a new instance of the `Poem` class, while the `ResearchPaperFactory.MakeBook()` method returns a new `ResearchPaper` instance.

This abstraction allows us to use our `factories` as follows:

```cs
// Create a PoemFactory, generating a Poem book type and a Blog publisher type.
new PoemFactory(author: "Edgar Allan Poe", 
                title: "The Raven",
                publisher: "The American Review");

// Create a ResearchPaperFactory, generating a ResearchPaper book type
// and a ScientificJournal publisher type.
new ResearchPaperFactory(author: "Charles Darwin", 
                            title: "On the Origin of Species", 
                            publisher: "John Murray");
```

We're able to instantiate a `PoemFactory` (and pass in some values) without any underlying knowledge of what `Book` or `Publisher` classes are used under the hood, nor how their respective logic works.  This is the `abstract` part of the `abstract factory` pattern, and the output confirms that we're creating the appropriate types of objects:

```
Made an IBook of type: Airbrake.DesignPatterns.AbstractFactory.Poem.
Made an IPublisher of type: Airbrake.DesignPatterns.AbstractFactory.Blog.
Made an IBookFactory of type: Airbrake.DesignPatterns.AbstractFactory.PoemFactory.
Made an IBook of type: Airbrake.DesignPatterns.AbstractFactory.ResearchPaper.
Made an IPublisher of type: Airbrake.DesignPatterns.AbstractFactory.ScientificJournal.
Made an IBookFactory of type: Airbrake.DesignPatterns.AbstractFactory.ResearchPaperFactory.
```

---

__META DESCRIPTION__

In this third part of Software Design Pattern series we closely examine the Abstract Factory design pattern, including C# example code.