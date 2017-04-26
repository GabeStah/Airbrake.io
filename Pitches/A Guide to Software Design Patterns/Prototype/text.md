# Creational Design Patterns: Prototype

Our next stop during our travels through the [`Guide to Software Design Patterns`](https://airbrake.io/blog/software-design/software-design-patterns-guide) series has us exploring another `Creational` technique known as the `prototype` pattern.  In the simplest terms, the `prototype` pattern allows for a new instance of an object to be created by duplicating or `cloning` an existing object, as opposed to creating a new object instance outright.

In this article we'll explore the `prototype` pattern through both some real world instances and a functional `C#` code example, so let's get started!

## In the Real World

A common example of `prototyping`, or `cloning`, which most computer users are familiar with, is when we copy a file on our hard drive.  By selecting a text document on your desktop, then hitting `Ctrl/Cmd+C` then `Ctrl/Cmd+V`, you're asking your computer to create a clone of that text document.  In essence, your system is analyzing the existing document and duplicating of all the relevant bits and bytes that make up that entity.  A rather complex process behind the scenes, but a very straightforward example of everyday cloning that most of us experience.

To bring a bit more life into it, another real world example of `cloning` occurred back in 1996, when [Dolly the Sheep](https://en.wikipedia.org/wiki/Dolly_(sheep)) was born, making her the first successful cloning of a mammal through a process known as [nuclear transfer](https://en.wikipedia.org/wiki/Nuclear_transfer).  This success has led to numerous other mammalian clones including deer, horses, bulls, and even four identical clones of the original Dolly as of July 2016.  Wild stuff!

## How It Works In Code

To illustrate how `prototyping` works within actual `C#` code we'll stick to our tried-and-true topic of **books**.  As usual, we'll start with the full working code example that you can copy and modify yourself, after which we'll walk through it step-by-step to see what's going on and how `prototyping` is typically implemented:

```cs
using System;
using Utility;

namespace Prototype
{
    class Program
    {
        static void Main(string[] args)
        {
            var book = new Book("A Game of Thrones", "George R.R. Martin", 694);

            var shallowClone = book.Clone();
            Logging.Log("---- Base Book ----");
            Logging.Log(book);
            Logging.Log("---- Shallow Clone ----");
            Logging.Log(shallowClone);


            Logging.Log("#### MODIFIED BASE BOOK ####");
            book.Title = "A Clash of Kings";
            book.Pages.PageCount = 768;

            Logging.Log("---- Base Book ----");
            Logging.Log(book);
            Logging.Log("---- Shallow Clone ----");
            Logging.Log(shallowClone);


            book = new Book("A Game of Thrones", "George R.R. Martin", 694);

            var deepClone = book.DeepClone();
            Logging.Log("---- Base Book ----");
            Logging.Log(book);
            Logging.Log("---- Deep Clone ----");
            Logging.Log(deepClone);

            Logging.Log("#### MODIFIED BASE BOOK ####");
            book.Title = "A Clash of Kings";
            book.Pages.PageCount = 768;

            Logging.Log("---- Base Book ----");
            Logging.Log(book);
            Logging.Log("---- Deep Clone ----");
            Logging.Log(deepClone);
        }
    }

    public class Pages
    {
        public int PageCount { get; set; }

        public Pages(int pageCount)
        {
            this.PageCount = pageCount;
        }
    }

    public interface IBook
    {
        string Author { get; set; }
        string Title { get; set; }
        Pages Pages { get; set; }
    }

    public class Book : IBook, ICloneable
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public Pages Pages { get; set; }

        public Book(string title, string author, int pageCount)
        {
            Title = title;
            Author = author;
            Pages = new Pages(pageCount);
        }

        // Create a deep clone of Book instance.
        public Book DeepClone()
        {
            // Create shallow clone with explicit conversion to Book type.
            Book clone = (Book)this.MemberwiseClone();
            // Copy Title string.
            clone.Title = String.Copy(Title);
            // Copy Author string.
            clone.Author = String.Copy(Author);
            // Create new instance of Pages class and pass instance's page count.
            clone.Pages = new Pages(Pages.PageCount);
            // Return deep clone object.
            return clone;
        }

        // Create a shallow clone of instance.
        public object Clone()
        {
            return this.MemberwiseClone();
        }
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
        public static void Log(object value)
        {
#if DEBUG
            Debug.WriteLine(ObjectDumper.Dump(value));
#else
            Console.WriteLine(ObjectDumper.Dump(value));
#endif
        }

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
}
```

As usual we begin with a baseline object, which is our `Book` class in this case.  `Book` uses the `IBook` interface, which just specifies a few simple properties our `Book` class must declare: `Title`, `Author`, and `Pages`.  `Pages` is of particular note because we have a separate `Pages` class with its own `PageCount` property that tracks the actual number of pages.  The reason for this abstraction of `Pages` as a separate class will become apparent shortly:

```cs
public class Pages
{
    public int PageCount { get; set; }

    public Pages(int pageCount)
    {
        this.PageCount = pageCount;
    }
}

public interface IBook
{
    string Author { get; set; }
    string Title { get; set; }
    Pages Pages { get; set; }
}

public class Book : IBook, ICloneable
{
    public string Title { get; set; }
    public string Author { get; set; }
    public Pages Pages { get; set; }

    public Book(string title, string author, int pageCount)
    {
        Title = title;
        Author = author;
        Pages = new Pages(pageCount);
    }

    // Create a deep clone of Book instance.
    public Book DeepClone()
    {
        // Create shallow clone with explicit conversion to Book type.
        Book clone = (Book)this.MemberwiseClone();
        // Copy Title string.
        clone.Title = String.Copy(Title);
        // Copy Author string.
        clone.Author = String.Copy(Author);
        // Create new instance of Pages class and pass instance's page count.
        clone.Pages = new Pages(Pages.PageCount);
        // Return deep clone object.
        return clone;
    }

    // Create a shallow clone of instance.
    public object Clone()
    {
        return this.MemberwiseClone();
    }
}
```

It's also worth noting that, in addition to the `IBook` interface, our `Book` class also inherits the `ICloneable` interface.  [`ICloneable`](https://docs.microsoft.com/en-us/dotnet/api/system.icloneable?view=netframework-4.7) is an interface provided by the .NET API that contains only one member, the `Clone` method.  By using this interface, we're forcing our `Book` class to declare a `Clone` method, which we've specified:

```cs
// Create a shallow clone of instance.
public object Clone()
{
    return this.MemberwiseClone();
}
```

This is a basic way of `cloning` an object in `C#`, through the use of the [`MemberwiseClone`](https://docs.microsoft.com/en-us/dotnet/api/system.object.memberwiseclone?view=netframework-4.7) method.  Using `MemberwiseClone` method to clone an object creates what is called a `shallow` clone.  A `shallow` clone of an object copies all the nonstatic fields of the source object.  This works fine for simple fields like `Strings` or `Integers`, but what happens when a field is just a reference type to another object (as in the case of the `Book#Pages` field, which is a reference to the `Pages` class object)?  The `shallow` clone copies the _reference_ to that object, but the actual _referenced object_ **is not** copied.  This means that our `shallow` clone, created via the `MemberwiseClone` method within our `Clone` instance method, _will not_ retain the proper `Pages` object instance that has been created.

This is why we also have added a `DeepClone` method to the `Book` class:

```cs
// Create a deep clone of Book instance.
public Book DeepClone()
{
    // Create shallow clone with explicit conversion to Book type.
    Book clone = (Book)this.MemberwiseClone();
    // Copy Title string.
    clone.Title = String.Copy(Title);
    // Copy Author string.
    clone.Author = String.Copy(Author);
    // Create new instance of Pages class and pass instance's page count.
    clone.Pages = new Pages(Pages.PageCount);
    // Return deep clone object.
    return clone;
}
```

While we begin with a `MemberwiseClone` call just as before, we then explicitly assign the property values of our `clone` object to be the same as the corresponding `Book` object properties.  This is particularly important for the `clone.Pages` property assignment, where we explicitly generate a new instance of `Pages` with the appropriate `PageCount` property value passed into it.

To see how this all comes together, we can call our `Book` class and create some clones.  We start by creating a new instance of `Book` called `book`, then immediately create a `shallow` clone, after which we output the values of both our `Base Book` and `Shallow Clone`:

```cs
var book = new Book("A Game of Thrones", "George R.R. Martin", 694);

var shallowClone = book.Clone();
Logging.Log("---- Base Book ----");
Logging.Log(book);
Logging.Log("---- Shallow Clone ----");
Logging.Log(shallowClone);
```

The output shows that all properties are identical across both books, as expected:

```
---- Base Book ----
{Prototype.Book}
  Title: "A Game of Thrones"
  Author: "George R.R. Martin"
  Pages: { }
    {Prototype.Pages}
      PageCount: 694

---- Shallow Clone ----
{Prototype.Book}
  Title: "A Game of Thrones"
  Author: "George R.R. Martin"
  Pages: { }
    {Prototype.Pages}
      PageCount: 694
```

Now, let's see what happens when we modify the `Title` and `Pages.PageCount` properties of our `Base Book` instance:

```cs
Logging.Log("#### MODIFIED BASE BOOK ####");
book.Title = "A Clash of Kings";
book.Pages.PageCount = 768;

Logging.Log("---- Base Book ----");
Logging.Log(book);
Logging.Log("---- Shallow Clone ----");
Logging.Log(shallowClone);
```

The produced output:

```
#### MODIFIED BASE BOOK ####
---- Base Book ----
{Prototype.Book}
  Title: "A Clash of Kings"
  Author: "George R.R. Martin"
  Pages: { }
    {Prototype.Pages}
      PageCount: 768

---- Shallow Clone ----
{Prototype.Book}
  Title: "A Game of Thrones"
  Author: "George R.R. Martin"
  Pages: { }
    {Prototype.Pages}
      PageCount: 768
```

What's particularly important to note here is which properties changed and which didn't.  Because we created a `shallow` clone, after we modified the `Title` property of `book` our `shallowClone` instance retained the original `Title` property it was given.  However, changing the `Pages.PageCount` property of the base `book` instance _also_ causes that same change to propagate to our `shallowClone`.  As discussed above, this is because a `shallow` clone of an object retains all _references_ to outside objects that it might have.  In this case, the `Pages` property of our instances are simply a _reference_ to the `Pages` class instance, which both the base `book` and `shallowBook` continue to share.

This behavior may often be desired when using the `prototype` pattern, but in cases where separation is required we can use the `DeepClone` method of our `Book` class, like so:

```cs
book = new Book("A Game of Thrones", "George R.R. Martin", 694);

var deepClone = book.DeepClone();
Logging.Log("---- Base Book ----");
Logging.Log(book);
Logging.Log("---- Deep Clone ----");
Logging.Log(deepClone);
```

Again, we start with the same base `book` instance, and from that we create a `deepClone` instance.  The output shows, as expected, that both contain the same properties at this point:

```
---- Base Book ----
{Prototype.Book}
  Title: "A Game of Thrones"
  Author: "George R.R. Martin"
  Pages: { }
    {Prototype.Pages}
      PageCount: 694

---- Deep Clone ----
{Prototype.Book}
  Title: "A Game of Thrones"
  Author: "George R.R. Martin"
  Pages: { }
    {Prototype.Pages}
```

However, now let's see what happens when we make the same modifications to the base `book` instance by changing `Title` and `Pages.PageCount`:

```cs
Logging.Log("#### MODIFIED BASE BOOK ####");
book.Title = "A Clash of Kings";
book.Pages.PageCount = 768;

Logging.Log("---- Base Book ----");
Logging.Log(book);
Logging.Log("---- Deep Clone ----");
Logging.Log(deepClone)
```

Unlike with the `shallow` clone technique, our `deepClone` instance _does not_ copy the reference to the same `Pages` class instance of our base `book` instance.  Therefore, `deepClone` has its own unique instance, retaining the original `Pages.PageCount` value of `694`:

```
#### MODIFIED BASE BOOK ####
---- Base Book ----
{Prototype.Book}
  Title: "A Clash of Kings"
  Author: "George R.R. Martin"
  Pages: { }
    {Prototype.Pages}
      PageCount: 768

---- Deep Clone ----
{Prototype.Book}
  Title: "A Game of Thrones"
  Author: "George R.R. Martin"
  Pages: { }
    {Prototype.Pages}
      PageCount: 694
```

---

__META DESCRIPTION__

Part 5 of our Software Design Pattern series in which we cover the Prototype design pattern, including C# example code.