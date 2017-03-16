# Creational Design Patterns: Simple Factory

Our first leg of the journey through our [`Guide to Software Design Patterns`](https://airbrake.io/blog/software-design/software-design-patterns-guide) takes us into the world of `Creational` design patterns, specifically the `simple factory` pattern.  At its core, the `simple factory` design pattern is a form of abstraction, which hides the actual logic of implementation of an object so the initialization code can focus on usage, rather than the inner workings.

Throughout this article we'll explain, in real world terms, what a `simple factory` design pattern attempts to do, and then move onto a functional code example in the `C#` language (though, as with all patterns, this idea can be translated to most any language or platform).  Let's get to it!

## In the Real World

The keyword of the `simple factory` pattern is, of course, `factory`.  It's no mistake that this design pattern is named as such, because the core concept is based on the real world of assembly lines and factory work in general.  A giant machine that presses metal into cogs is just one component of the factory, and when a customer orders a batch of cogs, he or she is not concerned with how those cogs are made.  Instead, the request is merely, "Make me some cogs."  The factory then does the work, and unbeknownst to the customer, out comes a batch of ordered cogs.

The printing press is an obvious real world example of the `simple factory` design pattern in action.  Think about the plates, ink, and huge rollers that all work together to create each print of your local newspaper.  Prior to the invention of the printing press, if you wanted to create your own newspaper, you'd need to painstakingly craft each character, line, and page, across dozens of double-sided pages.  Every single copy would require the same insane level of work (in fact, you'd probably never finish since the next morning might roll around before one copy is completed).

Yet, for modern newspapers, once the plates are made and everything is configured, creating a second copy of that daily edition is no more difficult than creating the first.  Hundreds or thousands of copies can be printed with no additional effort, without having to deal with anything beyond the initial creation.  This is a common, easy example of the `simple factory` design pattern in action.

## How It Works In Code

Sticking with the printing press analogy a bit, our code sample for the `simple factory` pattern focuses on `books`.  We'll start with the full code, then we'll explain the components that make up the `simple factory` pattern, and how they work together:

```cs
using System;
using Utility;

namespace SimpleFactory
{
    class Program
    {
        static void Main(string[] args)
        {
            var book = BookFactory.MakeBook("The Stand", "Stephen King", 823);
            Logging.Log("Title: " + book.Title);
            Logging.Log("Author: " + book.Author);
            Logging.Log("Page Count: " + book.PageCount);
            Logging.Log("Cover Type: " + book.CoverType);
            Logging.Log("Object Class: " + book.ToString());
        }
    }
    
    public enum CoverType
    {
        Digital,
        Hard,
        Paperback
    }

    public interface IBook
    {
        string Author { get; set; }
        string Title { get; set; }
        int PageCount { get; set; }
        CoverType CoverType { get; }
    }

    public class PaperbackBook : IBook
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int PageCount { get; set; }
        public CoverType CoverType { get; private set; }

        public PaperbackBook(string title, string author, int pageCount)
        {
            Title = title;
            Author = author;
            PageCount = pageCount;
            CoverType = CoverType.Paperback;
        }
    }

    class BookFactory
    {
        public static IBook MakeBook(string title, string author, int pageCount)
        {
            return new PaperbackBook(title, author, pageCount);
        }
    }
}
```

The goal here of our `simple factory` pattern is to be able to easily create new copies and types of books, without the need to know how those particular underlying classes are implemented.  Thus, the beginning of our `simple factory` pattern starts with an `interface` (and an `enumeration` for simplicity):

```cs
public enum CoverType
{
    Digital,
    Hard,
    Paperback
}

public interface IBook
{
    string Author { get; set; }
    string Title { get; set; }
    int PageCount { get; set; }
    CoverType CoverType { get; }
}
```

In most object oriented programming languages, an `interface` is simply a means of describing the behavior or capabilities of an object, without specifying how it executes that behavior.  For our `IBook` `interface` here, we've created a number of basic `members` and indicated the default `get` and `set` behavior for `Author`, `Title`, and `PageCount`.  This informs C# that this interface allows the values of those three `members` to be both changed (`set`) and retrieved (`get`).  To make our later code a bit more interesting, we've also added a `CoverType` member, using the `CoverType` `enum` above, which _is only_ `gettable`.  This means that the `CoverType` value cannot be `set` by classes which inherit this interface (by default).

With the basic structure of what our `Book` looks like, we next want to create a unique `Book` class that uses our `IBook` interface, but expands on it in some way.  For our purpose, we've created a **type** of `Book` called the `PaperbackBook`:

```cs
public class PaperbackBook : IBook
{
    public string Title { get; set; }
    public string Author { get; set; }
    public int PageCount { get; set; }
    public CoverType CoverType { get; private set; }

    public PaperbackBook(string title, string author, int pageCount)
    {
        Title = title;
        Author = author;
        PageCount = pageCount;
        CoverType = CoverType.Paperback;
    }
}
```

We're able to then use all the members of our `IBook` interface, but we've specified that, since this is a `paperback` book, we want our `PaperbackBook` class to be able to `privately` `set` the `CoverType` member.  While not relevant to this example, this allows our other classes that inherit `IBook`, such as `HardcoverBook` or `DigitalBook`, to set their own unique value for the `CoverType` member, just as we've done here in the constructor for `PaperbackBook`, setting it to `CoverType.Paperback`.

Now comes the meat of our `simple factory`, in which we create a `BookFactory` class, which returns an instance of our `IBook` interface through the `MakeBook()` method:

```cs
class BookFactory
{
    public static IBook MakeBook(string title, string author, int pageCount)
    {
        return new PaperbackBook(title, author, pageCount);
    }
}
```

The magic is when we want to create a book, as shown in this example code:

```cs
var book = BookFactory.MakeBook("The Stand", "Stephen King", 823);
Logging.Log("Title: " + book.Title);
Logging.Log("Author: " + book.Author);
Logging.Log("Page Count: " + book.PageCount);
Logging.Log("Cover Type: " + book.CoverType);
Logging.Log("Object Class: " + book.ToString());
```

If we were using a normal class, we'd directly call and create a new instance of `PaperbackBook`: `new PaperbackBook("The Stand", "Stephen King", 823)`.  However, by using our `BookFactory` class in the `simple factory` pattern, we've abstracted the process of creating our `PaperbackBook` class.  All that logic and implementation can occur behind the scenes, as all we care about is that when we call `BookFactory.MakeBook()`, an appropriate book is made.

Best of all, notice in the output, the `book.ToString()` method call shows that the inheritance functioned as expected, so we're getting a `PaperbackBook` object like we wanted, even though there's no direct knowledge of (or reference to) it in our initializing code:

```
Title: The Stand
Author: Stephen King
Page Count: 823
Cover Type: Paperback
Object Class: SimpleFactory.PaperbackBook
```

This is a basic example of the `simple factory` pattern in action.  We could further add new book types, and perform some basic logic within `MakeBook` to ensure that the proper book type class is generated and returned, but no matter what, our initializing code through our `BookFactory` doesn't know or care about how that works.

---

__META DESCRIPTION__

The first part of our Software Design Pattern series, in which we closely examine the Simple Factory design pattern, including C# example code.