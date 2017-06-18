# .NET Exceptions - System.IndexOutOfRangeException

Making our way through the [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, today we'll take a closer look at the `System.IndexOutOfRangeException`.  As indicated by its name, the `System.IndexOutOfRangeException` is raised when attempts are made to access an invalid index of a collection, such as a list or array.

In this article we'll dig deeper into the `System.IndexOutOfRangeException`, seeing where it resides within the .NET exception hierarchy, along with a few functional C# cod examples to illustrate how `System.IndexOutOfRangeExceptions` are typically thrown, so let's get to it!

## The Technical Rundown

- All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.
- [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception) is inherited from the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) class.
- `System.IndexOutOfRangeException` inherits directly from [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception).

## When Should You Use It?

As usual, to begin examining the `System.IndexOutOfRangeException` in more detail we'll start with the full code example and then explore what's going on in more detail below.

```cs
using System;
using System.Collections.Generic;
using Utility;

namespace Airbrake.IndexOutOfRangeException
{
    /// <summary>
    /// Book interface.
    /// </summary>
    public interface IBook
    {
        string Author { get; set; }
        string Title { get; set; }
    }

    /// <summary>
    /// Basic Book class.
    /// </summary>
    public class Book : IBook
    {
        public string Author { get; set; }
        public string Title { get; set; }

        public Book(string title, string author)
        {
            Author = author;
            Title = title;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ValidListIterationExample();
            Logging.LineSeparator();
            ArgumentOutOfRangeExample();
            Logging.LineSeparator();
            ChangeExistingElement();
            Logging.LineSeparator();
            OutOfRangeReferenceExample();
        }

        static void ValidListIterationExample()
        {
            try
            {
                // Create Book list.
                List<Book> library = new List<Book>();
                // Add a few books.
                library.Add(new Book("The Stand", "Stephen King"));
                library.Add(new Book("The Name of the Wind", "Patrick Rothfuss"));
                library.Add(new Book("Robinson Crusoe", "Daniel Defoe"));
                library.Add(new Book("The Hobbit", "J.R.R. Tolkien"));
                // Iterate over Book list using index.
                for (int index = 0; index < library.Count; index++)
                {
                    // Output Book instance.
                    Logging.Log(library[index]);
                }
            }
            catch (Exception exception) when (exception is System.ArgumentOutOfRangeException ||
                                  exception is System.IndexOutOfRangeException)
            {
                // Output caught exception.
                Logging.Log(exception);
            }
        }

        static void ArgumentOutOfRangeExample()
        {
            try
            {
                // Create Book list.
                List<Book> library = new List<Book>();
                // Add a few books.
                library.Add(new Book("The Stand", "Stephen King"));
                library.Add(new Book("The Name of the Wind", "Patrick Rothfuss"));
                library.Add(new Book("Robinson Crusoe", "Daniel Defoe"));
                library.Add(new Book("The Hobbit", "J.R.R. Tolkien"));
                // Iterate over Book list using index.
                // Count can equal library.Count, which will exceed index count by 1.
                for (int index = 0; index <= library.Count; index++)
                {
                    // Output Book instance.
                    Logging.Log(library[index]);
                }
            }
            catch (Exception exception) when (exception is System.ArgumentOutOfRangeException ||
                                              exception is System.IndexOutOfRangeException)
            {
                // Output caught exception.
                Logging.Log(exception);
            }
        }

        static void ChangeExistingElement()
        {
            try
            {
                // Create Book list.
                List<Book> library = new List<Book>();
                // Add a few books.
                library.Add(new Book("The Stand", "Stephen King"));
                library.Add(new Book("The Name of the Wind", "Patrick Rothfuss"));
                library.Add(new Book("Robinson Crusoe", "Daniel Defoe"));
                library.Add(new Book("The Hobbit", "J.R.R. Tolkien"));
                // Verify current library count.
                Logging.Log($"Current library count: {library.Count}");
                // Assign new book to last item in list.
                library[library.Count - 1] = new Book("Seveneves", "Neal Stephenson");
                // Output updated library.
                Logging.Log(library);
            }
            catch (Exception exception) when (exception is System.ArgumentOutOfRangeException ||
                                              exception is System.IndexOutOfRangeException)
            {
                // Output caught exception.
                Logging.Log(exception);
            }
        }

        static void OutOfRangeReferenceExample()
        {
            try
            {
                // Create Book list.
                List<Book> library = new List<Book>();
                // Add a few books.
                library.Add(new Book("The Stand", "Stephen King"));
                library.Add(new Book("The Name of the Wind", "Patrick Rothfuss"));
                library.Add(new Book("Robinson Crusoe", "Daniel Defoe"));
                library.Add(new Book("The Hobbit", "J.R.R. Tolkien"));
                // Verify current library count.
                Logging.Log($"Current library count: {library.Count}");
                // Assign new book to invalid index (index maxes out at Count - 1).
                library[library.Count] = new Book("Seveneves", "Neal Stephenson");
                // Output updated library.
                Logging.Log(library);
            }
            catch (Exception exception) when (exception is System.ArgumentOutOfRangeException ||
                                              exception is System.IndexOutOfRangeException)
            {
                // Output caught exception.
                Logging.Log(exception);
            }
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
        public static void Log(string value)
        {
#if DEBUG
            Debug.WriteLine(value);
#else
            Console.WriteLine(value);
#endif
        }

        /// <summary>
        /// When <see cref="Exception"/> parameter is passed, modifies the output to indicate
        /// if <see cref="Exception"/> was expected, based on passed in `expected` parameter.
        /// <para>Outputs the full <see cref="Exception"/> type and message.</para>
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to output.</param>
        /// <param name="expected">Boolean indicating if <see cref="Exception"/> was expected.</param>
        public static void Log(Exception exception, bool expected = true)
        {
            string value = $"[{(expected ? "EXPECTED" : "UNEXPECTED")}] {exception.ToString()}: {exception.Message}";
#if DEBUG
            Debug.WriteLine(value);
#else
            Console.WriteLine(value);
#endif
        }

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
        /// Outputs a dashed line separator to <see cref="System.Diagnostics.Debug.WriteLine"/> 
        /// if DEBUG mode is enabled, otherwise uses standard <see cref="Console.WriteLine"/>.
        /// </summary>
        public static void LineSeparator()
        {
#if DEBUG
            Debug.WriteLine(new string('-', 20));
#else
            Console.WriteLine(new string('-', 20));
#endif
        }
    }
}
```

To begin we have a simple `Book` class that inherits from our `IBook` interface, allowing us to create some basic objects for manipulation:

```cs
/// <summary>
/// Book interface.
/// </summary>
public interface IBook
{
    string Author { get; set; }
    string Title { get; set; }
}

/// <summary>
/// Basic Book class.
/// </summary>
public class Book : IBook
{
    public string Author { get; set; }
    public string Title { get; set; }

    public Book(string title, string author)
    {
        Author = author;
        Title = title;
    }
}
```

We then have a simple method in which we generate a new `Book` `List`, add a few books to it, and then perform a basic iteration over our book list using the `index`:

```cs
static void ValidListIterationExample()
{
    try
    {
        // Create Book list.
        List<Book> library = new List<Book>();
        // Add a few books.
        library.Add(new Book("The Stand", "Stephen King"));
        library.Add(new Book("The Name of the Wind", "Patrick Rothfuss"));
        library.Add(new Book("Robinson Crusoe", "Daniel Defoe"));
        library.Add(new Book("The Hobbit", "J.R.R. Tolkien"));
        // Iterate over Book list using index.
        for (int index = 0; index < library.Count; index++)
        {
            // Output Book instance.
            Logging.Log(library[index]);
        }
    }
    catch (Exception exception) when (exception is System.ArgumentOutOfRangeException ||
                            exception is System.IndexOutOfRangeException)
    {
        // Output caught exception.
        Logging.Log(exception);
    }
}
```

Unsurprisingly this outputs all our book records for us:

```
{Airbrake.IndexOutOfRangeException.Book(HashCode:21083178)}
  Author: "Stephen King"
  Title: "The Stand"

{Airbrake.IndexOutOfRangeException.Book(HashCode:15368010)}
  Author: "Patrick Rothfuss"
  Title: "The Name of the Wind"

{Airbrake.IndexOutOfRangeException.Book(HashCode:4094363)}
  Author: "Daniel Defoe"
  Title: "Robinson Crusoe"

{Airbrake.IndexOutOfRangeException.Book(HashCode:36849274)}
  Author: "J.R.R. Tolkien"
  Title: "The Hobbit"
```

However, in our next method we try much the same thing but our `index` value is allowed to go up to `library.Count`.  This will cause a problem because, like most programming languages, collections are `zero-based` so the largest index our `library` list contains is actually _one less_ than the `.Count` property:

```cs
static void ArgumentOutOfRangeExample()
{
    try
    {
        // Create Book list.
        List<Book> library = new List<Book>();
        // Add a few books.
        library.Add(new Book("The Stand", "Stephen King"));
        library.Add(new Book("The Name of the Wind", "Patrick Rothfuss"));
        library.Add(new Book("Robinson Crusoe", "Daniel Defoe"));
        library.Add(new Book("The Hobbit", "J.R.R. Tolkien"));
        // Iterate over Book list using index.
        // Count can equal library.Count, which will exceed index count by 1.
        for (int index = 0; index <= library.Count; index++)
        {
            // Output Book instance.
            Logging.Log(library[index]);
        }
    }
    catch (Exception exception) when (exception is System.ArgumentOutOfRangeException ||
                                        exception is System.IndexOutOfRangeException)
    {
        // Output caught exception.
        Logging.Log(exception);
    }
}
```

Sure enough this throws an error our way:

```
[EXPECTED] System.ArgumentOutOfRangeException: Index was out of range. Must be non-negative and less than the size of the collection.
Parameter name: index
```

It's worth noting that while the actual class type that was thrown there was a `System.ArgumentOutOfRangeException` (instead of a `System.IndexOutOfRangeException`), the error message clearly indicates that this is a problem with the provided index being out of range.

Another common technique is to change the values of an existing collection.  There are many ways to accomplish this, but one method is to specify the collection element using the index of the element to be changed, which is what we're doing in this next example method:

```cs
static void ChangeExistingElement()
{
    try
    {
        // Create Book list.
        List<Book> library = new List<Book>();
        // Add a few books.
        library.Add(new Book("The Stand", "Stephen King"));
        library.Add(new Book("The Name of the Wind", "Patrick Rothfuss"));
        library.Add(new Book("Robinson Crusoe", "Daniel Defoe"));
        library.Add(new Book("The Hobbit", "J.R.R. Tolkien"));
        // Verify current library count.
        Logging.Log($"Current library count: {library.Count}");
        // Assign new book to last item in list.
        library[library.Count - 1] = new Book("Seveneves", "Neal Stephenson");
        // Output updated library.
        Logging.Log(library);
    }
    catch (Exception exception) when (exception is System.ArgumentOutOfRangeException ||
                                        exception is System.IndexOutOfRangeException)
    {
        // Output caught exception.
        Logging.Log(exception);
    }
}
```

Here we access and change the last element in our collection by changing `library[library.Count - 1]` (the last index) and assigning it to a new `Book` instance.  The output shows that our `library` has been updated with a different final book:

```
{Airbrake.IndexOutOfRangeException.Book(HashCode:42119052)}
  Author: "Stephen King"
  Title: "The Stand"
{Airbrake.IndexOutOfRangeException.Book(HashCode:43527150)}
  Author: "Patrick Rothfuss"
  Title: "The Name of the Wind"
{Airbrake.IndexOutOfRangeException.Book(HashCode:56200037)}
  Author: "Daniel Defoe"
  Title: "Robinson Crusoe"
{Airbrake.IndexOutOfRangeException.Book(HashCode:36038289)}
  Author: "Neal Stephenson"
  Title: "Seveneves"
```

Now let's try that again but this time we'll be accessing an index of our `library` collection that doesn't exist (`library.Count`, which is one value greater than the maximum index):

```cs
static void OutOfRangeReferenceExample()
{
    try
    {
        // Create Book list.
        List<Book> library = new List<Book>();
        // Add a few books.
        library.Add(new Book("The Stand", "Stephen King"));
        library.Add(new Book("The Name of the Wind", "Patrick Rothfuss"));
        library.Add(new Book("Robinson Crusoe", "Daniel Defoe"));
        library.Add(new Book("The Hobbit", "J.R.R. Tolkien"));
        // Verify current library count.
        Logging.Log($"Current library count: {library.Count}");
        // Assign new book to invalid index (index maxes out at Count - 1).
        library[library.Count] = new Book("Seveneves", "Neal Stephenson");
        // Output updated library.
        Logging.Log(library);
    }
    catch (Exception exception) when (exception is System.ArgumentOutOfRangeException ||
                                        exception is System.IndexOutOfRangeException)
    {
        // Output caught exception.
        Logging.Log(exception);
    }
}
```

Sure enough this throws another exception our way, indicating that the index provided isn't within the allowed bounds:

```
Current library count: 4
[EXPECTED] System.ArgumentOutOfRangeException: Index was out of range. Must be non-negative and less than the size of the collection.
Parameter name: index
```

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A close look at the System.IndexOutOfRangeException in .NET, including a some simple C# code examples illustrating how out of range exceptions occur.