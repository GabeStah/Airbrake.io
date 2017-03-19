# .NET Exceptions - System.InvalidOperationException

Moving right along through the magical world of our __.NET Exception Handling__ series, today we're going to cover the `System.InvalidOperationException`.  The `System.InvalidOperationException` is a fairly common exception, as it is typically thrown when there's a failed attempt to invoke a method, caused by something _other_ than invalid arguments passed to that method.

In this article, we'll examine where `System.InvalidOperationException` sits within the .NET exception hierarchy, look at why `System.InvalidOperationExceptions` typically appear, and see how to deal with them should you encounter one yourself.  Let's get this party started!

## The Technical Rundown

- All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.
- [`System.SystemException`](https://msdn.microsoft.com/en-us/library/system.systemexception(v=vs.110).aspx) is inherited from the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) class.
- `System.InvalidOperationException` is inherited from the [`System.SystemException`](https://msdn.microsoft.com/en-us/library/system.systemexception(v=vs.110).aspx) class.

## When Should You Use It?

Unlike many exceptions in .NET, you may have noticed that `System.InvalidOperationException` resides quite close to the top of the exception hierarchy, as a direct descendent of `System.SystemException`.  This typically indicates a .NET exception that is very common, or is applicable in many scenarios, and `System.InvalidOperationException` (arguably) falls into both camps.

The challenge with describing the `System.InvalidOperationException` is that it can occur in a wide variety of scenarios.  As mentioned in the introduction, a `System.InvalidOperationException` is thrown when a method invocation fails for a reason other than invalid arguments.  The [official documentation](https://msdn.microsoft.com/en-us/library/system.invalidoperationexception(v=vs.110).aspx) gives a few potential examples of what might cause this, including:

- When an `IEnumerator.MoveNext` call is made on a collection that has since been modified after the enumerator was generated (more on this one later).
- Calling `ResourceSet.GetString` if the resource was already closed prior to this call.
- Modifying `XML` via the `XContainer.Add` method, if doing so would generate invalid `XML`.
- Attempting to modify the UI from a secondary thread that is not the main/UI thread.

This is by no means an exhaustive list, so it's impossible to cover all potential scenarios that a `System.InvalidOperationException` could occur.  Instead, to continue our examination, we'll just take a look at one particular means by which a `System.InvalidOperationException` is typically raised: Modifying a collection **after** the enumeration of that collection has been generated, then continuing to utilize the enumeration.

To illustrate this in action, we've got our full code example below:

```cs
using System.Collections.Generic;
using Utility;

namespace Airbrake.InvalidOperationException
{
    class Program
    {
        static void Main(string[] args)
        {
            ValidExample();
            Logging.Log("-----------------");
            InvalidExample();
        }

        private static void InvalidExample()
        {
            try
            {
                List<Book> books = new List<Book>();
                books.Add(new Book("The Stand", "Stephen King"));
                books.Add(new Book("Moby Dick", "Herman Melville"));
                books.Add(new Book("Fahrenheit 451", "Ray Bradbury"));
                books.Add(new Book("A Game of Thrones", "George R.R. Martin"));
                books.Add(new Book("The Name of the Wind", "Patrick Rothfuss"));

                Logging.Log($"Total Book count: {books.Count}.");

                Book newBook = new Book("Robinson Crusoe", "Daniel Defoe");
                foreach (var book in books)
                {
                    Logging.Log($"Current book title: {book.Title}, author: {book.Author}.");
                    if (!books.Contains(newBook))
                    {
                        books.Add(newBook);
                        Logging.Log($"Adding new book: {newBook.Title}, author: {newBook.Author}.");
                    }
                    Logging.Log($"New total Book count: {books.Count}.");
                }
            }
            catch (System.InvalidOperationException exception)
            {
                Logging.Log(exception);
            }
        }

        private static void ValidExample()
        {
            try
            {
                List<Book> books = new List<Book>();
                books.Add(new Book("The Stand", "Stephen King"));
                books.Add(new Book("Moby Dick", "Herman Melville"));
                books.Add(new Book("Fahrenheit 451", "Ray Bradbury"));
                books.Add(new Book("A Game of Thrones", "George R.R. Martin"));
                books.Add(new Book("The Name of the Wind", "Patrick Rothfuss"));

                Logging.Log($"Total Book count: {books.Count}.");

                Book newBook = new Book("Robinson Crusoe", "Daniel Defoe");
                int maxCount = books.Count - 1;
                for (int index = 0; index <= maxCount; index++)
                {
                    Logging.Log($"Current book title: {books[index].Title}, author: {books[index].Author}.");
                    if (!books.Contains(newBook))
                    {
                        books.Add(newBook);
                        Logging.Log($"Adding new book: {newBook.Title}, author: {newBook.Author}.");
                    }
                }
                Logging.Log($"New total Book count: {books.Count}.");
            }
            catch (System.InvalidOperationException exception)
            {
                Logging.Log(exception);
            }
        }
    }

    public interface IBook
    {
        string Author { get; set; }
        string Title { get; set; }
    }

    public class Book : IBook
    {
        public string Title { get; set; }
        public string Author { get; set; }

        public Book() { }

        public Book(string title, string author)
        {
            Title = title;
            Author = author;
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
    }
}
```

We won't get into detail about the `Utility` namespace, but instead we'll look at the `Airbrake.InvalidOperationException` namespace, where our problematic code resides.  In this somewhat realistic example, we've got an `interface` called `IBook`, which specifies a few fields (`Author` and `Title`) for our `Book` class:

```cs
public interface IBook
{
    string Author { get; set; }
    string Title { get; set; }
}

public class Book : IBook
{
    public string Title { get; set; }
    public string Author { get; set; }

    public Book() { }

    public Book(string title, string author)
    {
        Title = title;
        Author = author;
    }
}
```

Now, using our `Book` class, our `InvalidExample` method generates a `List<Book>` collection called `books`, and adds a few of my favorites selections to the list.  From there, we confirm that our collection contains the five `Books` we added, then we generate our `Enumerator` using the `foreach` loop block.  Inside our loop, we output the current `book` instance, then create a `newBook` of `Robinson Crusoe`.  We then call `books.Add` to add our `newBook` to the collection, and continue the loop using our previously established `enumeration`:

```cs
private static void InvalidExample()
{
    try
    {
        List<Book> books = new List<Book>();
        books.Add(new Book("The Stand", "Stephen King"));
        books.Add(new Book("Moby Dick", "Herman Melville"));
        books.Add(new Book("Fahrenheit 451", "Ray Bradbury"));
        books.Add(new Book("A Game of Thrones", "George R.R. Martin"));
        books.Add(new Book("The Name of the Wind", "Patrick Rothfuss"));

        Logging.Log($"Total Book count: {books.Count}.");

        Book newBook = new Book("Robinson Crusoe", "Daniel Defoe");
        foreach (var book in books)
        {
            Logging.Log($"Current book title: {book.Title}, author: {book.Author}.");
            if (!books.Contains(newBook))
            {
                books.Add(newBook);
                Logging.Log($"Adding new book: {newBook.Title}, author: {newBook.Author}.");
            }
            Logging.Log($"New total Book count: {books.Count}.");
        }
    }
    catch (System.InvalidOperationException exception)
    {
        Logging.Log(exception);
    }
}
```

The trouble is, our `enumeration` was established back when we had only `five` books, but before our second call to `foreach`, we've modified the collection and added a `sixth` book.  Sure enough, this throws a `System.InvalidOperationException`:

```
Total Book count: 5.
Current book title: The Stand, author: Stephen King.
Adding new book: Robinson Crusoe, author: Daniel Defoe.
New total Book count: 6.
Exception thrown: 'System.InvalidOperationException' in mscorlib.dll
[EXPECTED] System.InvalidOperationException: Collection was modified; enumeration operation may not execute.
```

If adding new items to the collection during the `enumeration` is a necessity, the best way to do it is to generate a `static enumeration` beforehand; one that doesn't directly access the modified collection each loop.  For example, using `for`, instead of `foreach`, allows us to reference pre-determined `indexes` of the collection items, rather than direct collection items during the loop.  Here is our slightly modified code to illustrate a successful fix:

```cs
private static void ValidExample()
{
    try
    {
        List<Book> books = new List<Book>();
        books.Add(new Book("The Stand", "Stephen King"));
        books.Add(new Book("Moby Dick", "Herman Melville"));
        books.Add(new Book("Fahrenheit 451", "Ray Bradbury"));
        books.Add(new Book("A Game of Thrones", "George R.R. Martin"));
        books.Add(new Book("The Name of the Wind", "Patrick Rothfuss"));

        Logging.Log($"Total Book count: {books.Count}.");

        Book newBook = new Book("Robinson Crusoe", "Daniel Defoe");
        int maxCount = books.Count - 1;
        for (int index = 0; index <= maxCount; index++)
        {
            Logging.Log($"Current book title: {books[index].Title}, author: {books[index].Author}.");
            if (!books.Contains(newBook))
            {
                books.Add(newBook);
                Logging.Log($"Adding new book: {newBook.Title}, author: {newBook.Author}.");
            }
        }
        Logging.Log($"New total Book count: {books.Count}.");
    }
    catch (System.InvalidOperationException exception)
    {
        Logging.Log(exception);
    }
}
```

The key is to detach enumeration from the collection that is changing.  We've initially stated that our enumerator will only loop through a total of the first `five` items (indexes), regardless of what is added to the collection afterward.  This produces an expected result without a `System.InvalidOperationException`:

```
Total Book count: 5.
Current book title: The Stand, author: Stephen King.
Adding new book: Robinson Crusoe, author: Daniel Defoe.
Current book title: Moby Dick, author: Herman Melville.
Current book title: Fahrenheit 451, author: Ray Bradbury.
Current book title: A Game of Thrones, author: George R.R. Martin.
Current book title: The Name of the Wind, author: Patrick Rothfuss.
New total Book count: 6.
```

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A exploration of the System.InvalidOperationException in .NET, and a dive into handling changing collections during loops.