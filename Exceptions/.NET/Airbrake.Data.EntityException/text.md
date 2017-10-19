# .NET Exceptions - System.Data.EntityException

Moving along through the detailed [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, today we will be exploring the **EntityException**.  The `EntityException` is the base exception class used by the [`EntityClient`](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/ef/entityclient-provider-for-the-entity-framework) provider, which is part of the overall [`Entity Framework`](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/ef/overview), which enables model-to-object mapping and relationships throughout ADO-based applications.

In this article we'll examine `EntityExceptions` by looking at where it resides in the overall .NET exception hierarchy.  We'll also go over some functional C# sample code to illustrate the basics of working with the `Entity Framework` in .NET, which can be used to create, read, update, and delete SQL databases with ease.  Let's get started!

## The Technical Rundown

All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.  The full exception hierarchy of this error is:

- [`System.Object`](https://docs.microsoft.com/en-us/dotnet/api/system.object)
    - [`System.Exception`](https://docs.microsoft.com/en-us/dotnet/api/system.exception)
        - [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception)
            - [`System.Data.DataException`](https://docs.microsoft.com/en-us/dotnet/api/system.data.dataexception?view=netframework-4.7.1)
                - `EntityException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```cs
using System;
using System.Linq;
using Utility;

namespace Airbrake.Data.EntityCommandCompilationException
{
    internal class Program
    {
        private static void Main()
        {
            try
            {
                // Instantiate database context.
                var context = new BookEntityContext();

                // Delete database, if already exists.
                Logging.LineSeparator("DELETE DATABASE");
                context.Database.Delete();

                // Create database.
                Logging.LineSeparator("CREATE DATABASE");
                context.Database.Create();

                // Add some BookEntities to context.
                AddBookEntityToContext(
                    new BookEntity(
                        "Magician: Apprentice",
                        "Raymond E. Feist",
                        485,
                        new DateTime(1982, 10, 1)),
                    context);

                AddBookEntityToContext(
                    new BookEntity(
                        "Magician: Master",
                        "Raymond E. Feist",
                        499,
                        new DateTime(1982, 11, 1)),
                    context);

                AddBookEntityToContext(
                    new BookEntity(
                        "Silverthorn",
                        "Raymond E. Feist",
                        432,
                        new DateTime(1985, 5, 7)),
                    context);

                // Output BookEntities found in context.
                OutputBookEntitiesOfContext(context);
            }
            catch (EntityException exception)
            {
                // Output expected EntityExceptions.
                Logging.Log(exception);
                if (exception.InnerException != null)
                {
                    // Output unexpected InnerExceptions.
                    Logging.Log(exception.InnerException, false);
                }
            }
            catch (Exception exception)
            {
                // Output unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }

        /// <summary>
        /// Add a BookEntity to pass context and save changes.
        /// </summary>
        /// <param name="book">BookEntity to be added.</param>
        /// <param name="context">Context to which BookEntity should be added.</param>
        private static void AddBookEntityToContext(BookEntity book, BookEntityContext context)
        {
            context.Books.Add(book);
            context.SaveChanges();
        }

        /// <summary>
        /// Logs the list of BookEntities in passed context.
        /// </summary>
        /// <param name="context">Context containing BookEntities.</param>
        private static void OutputBookEntitiesOfContext(BookEntityContext context)
        {
            // Select all books, ordered by descending publication date.
            var query = 
                from book
                in context.Books
                orderby book.PublicationDate descending
                select book;
            Logging.LineSeparator("CURRENT BOOK LIST");
            // Output query result (books).
            Logging.Log(query);
        }
    }
}
```

```cs
using System.Data.Entity;

namespace Airbrake.Data.EntityCommandCompilationException
{
    public class BookEntityContext : DbContext
    {
        public DbSet<BookEntity> Books { get; set; }
    }
}
```

```cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using Utility;

namespace Airbrake.Data.EntityCommandCompilationException
{
    public class BookEntity : Book
    {
        /// <summary>
        /// Composited key for entity.
        /// 
        /// Concatenates alphanumeric characters from Author and Title properties.
        /// </summary>
        [Key, Column("CompositeId", Order = 1)]
        public string CompositeId
        {
            get => $"{Regex.Replace(Author.ToLower(), @"[^A-Za-z0-9]+", "")}-{Regex.Replace(Title.ToLower(), @"[^A-Za-z0-9]+", "")}";
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
            }
        }

        /// <summary>
        /// Primary key for entity.
        /// 
        /// Column.Order determines which key is used primarily and secondarily.
        /// </summary>
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        #region Constructors

        public BookEntity() { }

        public BookEntity(string title, string author)
            : base(title, author) { }

        public BookEntity(string title, string author, int pageCount)
            : base(title, author, pageCount) { }

        public BookEntity(string title, string author, int pageCount, DateTime publicationDate)
            : base(title, author, pageCount, publicationDate) { }

        #endregion
    }
}
```

```cs
// <Utility>/Book.cs
using System;

namespace Utility
{
    public interface IBook
    {
        string Author { get; set; }
        int PageCount { get; set; }
        DateTime? PublicationDate { get; set; }
        string Title { get; set; }
    }

    /// <summary>
    /// Simple Book class.
    /// </summary>
    public class Book : IBook
    {
        public string Author { get; set; }
        public int PageCount { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string Title { get; set; }

        public Book() { }

        public Book(string title, string author)
        {
            Author = author;
            Title = title;
        }

        public Book(string title, string author, int pageCount)
        {
            Author = author;
            PageCount = pageCount;
            Title = title;
        }

        public Book(string title, string author, int pageCount, DateTime publicationDate)
        {
            Author = author;
            PageCount = pageCount;
            PublicationDate = publicationDate;
            Title = title;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var publicationDate = PublicationDate is null ? null : $", published on {PublicationDate.Value.ToLongDateString()}";
            return $"'{Title}' by {Author} at {PageCount} pages{publicationDate}";
        }
    }
}
```

```cs
// <Utility/>Logging.cs
using System;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Utility
{
    /// <summary>
    /// Houses all logging methods for various debug outputs.
    /// </summary>
    public static class Logging
    {
        private const char SeparatorCharacterDefault = '-';
        private const int SeparatorLengthDefault = 40;

        /// <summary>
        /// Determines type of output to be generated.
        /// </summary>
        public enum OutputType
        {
            /// <summary>
            /// Default output.
            /// </summary>
            Default,
            /// <summary>
            /// Output includes timestamp prefix.
            /// </summary>
            Timestamp
        }

        /// <summary>
        /// Outputs to <see cref="Debug.WriteLine(String)"/>.
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        /// <param name="outputType">Output type.</param>
        public static void Log(string value, OutputType outputType = OutputType.Default)
        {
            Output(value, outputType);
        }

        /// <summary>
        /// Outputs to <see cref="Debug.WriteLine(String)"/>.
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        /// <param name="arg0"></param>
        public static void Log(string value, object arg0)
        {
            Debug.WriteLine(value, arg0);
        }

        /// <summary>
        /// Outputs to <see cref="Debug.WriteLine(String)"/>.
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        public static void Log(string value, object arg0, object arg1)
        {
            Debug.WriteLine(value, arg0, arg1);
        }

        /// <summary>
        /// Outputs to <see cref="Debug.WriteLine(String)"/>.
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        public static void Log(string value, object arg0, object arg1, object arg2)
        {
            Debug.WriteLine(value, arg0, arg1, arg2);
        }

        /// <summary>
        /// When <see cref="Exception"/> parameter is passed, modifies the output to indicate
        /// if <see cref="Exception"/> was expected, based on passed in `expected` parameter.
        /// <para>Outputs the full <see cref="Exception"/> type and message.</para>
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to output.</param>
        /// <param name="expected">Boolean indicating if <see cref="Exception"/> was expected.</param>
        /// <param name="outputType">Output type.</param>
        public static void Log(Exception exception, bool expected = true, OutputType outputType = OutputType.Default)
        {
            var value = $"[{(expected ? "EXPECTED" : "UNEXPECTED")}] {exception}: {exception.Message}";

            Output(value, outputType);
        }

        private static void Output(string value, OutputType outputType = OutputType.Default)
        {
            Debug.WriteLine(outputType == OutputType.Timestamp
                ? $"[{StopwatchProxy.Instance.Stopwatch.Elapsed}] {value}"
                : value);
        }

        /// <summary>
        /// Outputs to <see cref="Debug.WriteLine(Object)"/>.
        /// 
        /// ObjectDumper: http://stackoverflow.com/questions/852181/c-printing-all-properties-of-an-object&amp;lt;/cref
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        /// <param name="outputType">Output type.</param>
        public static void Log(object value, OutputType outputType = OutputType.Default)
        {
            if (value is IXmlSerializable)
            {
                Debug.WriteLine(value);
            }
            else
            {
                Debug.WriteLine(outputType == OutputType.Timestamp
                    ? $"[{StopwatchProxy.Instance.Stopwatch.Elapsed}] {ObjectDumper.Dump(value)}"
                    : ObjectDumper.Dump(value));
            }
        }

        /// <summary>
        /// Outputs a dashed line separator to <see cref="Debug.WriteLine(String)"/>.
        /// </summary>
        /// <param name="length">Total separator length.</param>
        /// <param name="char">Separator character.</param>
        public static void LineSeparator(int length = SeparatorLengthDefault, char @char = SeparatorCharacterDefault)
        {
            Debug.WriteLine(new string(@char, length));
        }

        /// <summary>
        /// Outputs a dashed line separator to <see cref="Debug.WriteLine(String)"/>,
        /// with inserted text centered in the middle.
        /// </summary>
        /// <param name="insert">Inserted text to be centered.</param>
        /// <param name="length">Total separator length.</param>
        /// <param name="char">Separator character.</param>
        public static void LineSeparator(string insert, int length = SeparatorLengthDefault, char @char = SeparatorCharacterDefault)
        {
            // Default output to insert.
            var output = insert;

            if (insert.Length < length)
            {
                // Update length based on insert length, less a space for margin.
                length -= insert.Length + 2;
                // Halve the length and floor left side.
                var left = (int) Math.Floor((decimal) (length / 2));
                var right = left;
                // If odd number, add dropped remainder to right side.
                if (length % 2 != 0) right += 1;

                // Surround insert with separators.
                output = $"{new string(@char, left)} {insert} {new string(@char, right)}";
            }
            
            // Output.
            Debug.WriteLine(output);
        }
    }
}
```

## When Should You Use It?

Exploring the entirety of the [`Entity Framework`](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/ef/overview) is well beyond the scope of this article, but we need at least a basic understanding of it's purpose and functionality to see how `EntityExceptions` may be thrown and captured.  Traditionally, associating a physical object model (i.e. a row in a database table) with a logical object model (i.e. a programmatic instance of an object written in source code) would require manually creating and managing complex SQL statements.

On the other hand, the `Entity Framework` provides an easy means of mapping logical and physical database models to one another.  Thus, with just a few extra lines of code, the `Entity Framework` can associate an existing logical model with the underlying data layer and its physical model representation.  The framework will handle all "standard" SQL statements, database creation, and CRUD (create, read, update, delete) functionality.

To see this in action we've got a simple example.  We start with a `Book` class, which is implements from the `IBook` interface and primarily consists of a few basic properties:

```cs
// <Utility>/Book.cs

using System;

namespace Utility
{
    public interface IBook
    {
        string Author { get; set; }
        int PageCount { get; set; }
        DateTime? PublicationDate { get; set; }
        string Title { get; set; }
    }

    /// <summary>
    /// Simple Book class.
    /// </summary>
    public class Book : IBook
    {
        public string Author { get; set; }
        public int PageCount { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string Title { get; set; }

        public Book() { }

        public Book(string title, string author)
        {
            Author = author;
            Title = title;
        }

        public Book(string title, string author, int pageCount)
        {
            Author = author;
            PageCount = pageCount;
            Title = title;
        }

        public Book(string title, string author, int pageCount, DateTime publicationDate)
        {
            Author = author;
            PageCount = pageCount;
            PublicationDate = publicationDate;
            Title = title;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var publicationDate = PublicationDate is null ? null : $", published on {PublicationDate.Value.ToLongDateString()}";
            return $"'{Title}' by {Author} at {PageCount} pages{publicationDate}";
        }
    }
}
```

Critically, since the `Entity Framework` maps logical models like `Book` with a physical version within a **relational** database, our logical `Book` model needs some sort of identifying key.  By default, `Entity Framework` will look for a property named `[ClassName]Id` (e.g. `BookId`).  However, we don't want to directly add a key to the `Book` class, so we'll inherit it with the new `BookEntity` class, which we'll use to add extra stuff just for this `Entity Framework` example:

```cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using Utility;

namespace Airbrake.Data.EntityCommandCompilationException
{
    public class BookEntity : Book
    {
        /// <summary>
        /// Composited key for entity.
        /// 
        /// Concatenates alphanumeric characters from Author and Title properties.
        /// </summary>
        [Key, Column("CompositeId", Order = 1)]
        public string CompositeId
        {
            get => $"{Regex.Replace(Author.ToLower(), @"[^A-Za-z0-9]+", "")}-{Regex.Replace(Title.ToLower(), @"[^A-Za-z0-9]+", "")}";
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
            }
        }

        /// <summary>
        /// Primary key for entity.
        /// 
        /// Column.Order determines which key is used primarily and secondarily.
        /// </summary>
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        #region Constructors

        public BookEntity() { }

        public BookEntity(string title, string author)
            : base(title, author) { }

        public BookEntity(string title, string author, int pageCount)
            : base(title, author, pageCount) { }

        public BookEntity(string title, string author, int pageCount, DateTime publicationDate)
            : base(title, author, pageCount, publicationDate) { }

        #endregion
    }
}
```

The `constructors` are self-explanatory and they merely implement the base `Book` constructors.  However, we've added two new properties of `CompositeId` and `Id`.  `Id` is our primary key, which we specify with the `KeyAttribute` and `DatabaseGeneratedAttribute`.  We want it to be an auto-incremented identity, so we also want to pass the `DatabaseGeneratedOption.Identity` argument to the `DatabaseGeneratedAttribute`.  Just to illustrate the functionality of creating composite keys, the `CompositeId` dynamically retrieves its value by concatenating the `Author` and `Title` properties.

Now, to connect our logical `BookEntity` model to a physical model we need to create a new `System.Data.Entity.DbContext` instance that includes a `DbSet<BookEntity>` property:

```cs
using System.Data.Entity;

namespace Airbrake.Data.EntityCommandCompilationException
{
    public class BookEntityContext : DbContext
    {
        public DbSet<BookEntity> Books { get; set; }
    }
}
```

Now, instantiating `BookEntityContext` allows us to perform all manner of functionality within our physical database model.  To test things out we start with the `AddBookEntityToContext(BookEntity book, BookEntityContext context)` method:

```cs
/// <summary>
/// Add a BookEntity to pass context and save changes.
/// </summary>
/// <param name="book">BookEntity to be added.</param>
/// <param name="context">Context to which BookEntity should be added.</param>
private static void AddBookEntityToContext(BookEntity book, BookEntityContext context)
{
    context.Books.Add(book);
    context.SaveChanges();
}
```

As you can see, this method merely adds the passed `BookEntity` parameter to the passed `BookEntityContext`, then saves the new changes to the physical database model.  Additionally, after making modifications, we'll want to look at the database contents, so the `OutputBookEntitiesOfContext(BookEntityContext context)` method does this for us with a basic `LINQ` statement and log output:

```cs
/// <summary>
/// Logs the list of BookEntities in passed context.
/// </summary>
/// <param name="context">Context containing BookEntities.</param>
private static void OutputBookEntitiesOfContext(BookEntityContext context)
{
    // Select all books, ordered by descending publication date.
    var query = 
        from book
        in context.Books
        orderby book.PublicationDate descending
        select book;
    Logging.LineSeparator("CURRENT BOOK LIST");
    // Output query result (books).
    Logging.Log(query);
}
```

Alright!  Everything is setup, so let's test this out in our `Program.Main()` method:

```cs
private static void Main()
{
    try
    {
        // Instantiate database context.
        var context = new BookEntityContext();

        // Delete database, if already exists.
        Logging.LineSeparator("DELETE DATABASE");
        context.Database.Delete();

        // Create database.
        Logging.LineSeparator("CREATE DATABASE");
        context.Database.Create();

        // Add some BookEntities to context.
        AddBookEntityToContext(
            new BookEntity(
                "Magician: Apprentice",
                "Raymond E. Feist",
                485,
                new DateTime(1982, 10, 1)),
            context);

        AddBookEntityToContext(
            new BookEntity(
                "Magician: Master",
                "Raymond E. Feist",
                499,
                new DateTime(1982, 11, 1)),
            context);

        AddBookEntityToContext(
            new BookEntity(
                "Silverthorn",
                "Raymond E. Feist",
                432,
                new DateTime(1985, 5, 7)),
            context);

        // Output BookEntities found in context.
        OutputBookEntitiesOfContext(context);
    }
    catch (EntityException exception)
    {
        // Output expected EntityExceptions.
        Logging.Log(exception);
        if (exception.InnerException != null)
        {
            // Output unexpected InnerExceptions.
            Logging.Log(exception.InnerException, false);
        }
    }
    catch (Exception exception)
    {
        // Output unexpected Exceptions.
        Logging.Log(exception, false);
    }
}
```

We start by creating a new instance of `BookEntityContext`.  We then need to delete the underlying physical database (if it exists), since running this code multiple times would otherwise cause issues.  We also then recreate the database after deleting it, so we start with a clean slate before adding data.  Next, we call `AddBookEntityToContext(BookEntity book, BookEntityContext context)` a few times by passing in some books from the excellent _Riftwar Saga_ series, before finally outputting the current data.

Executing this code results in the following output:

```
----------- DELETE DATABASE ------------
----------- CREATE DATABASE ------------
---------- CURRENT BOOK LIST -----------
{Airbrake.Data.EntityCommandCompilationException.BookEntity(HashCode:3913996)}
  CompositeId: "raymondefeist-silverthorn"
  Id: 3
  Author: "Raymond E. Feist"
  PageCount: 432
  PublicationDate: 5/7/1985
  Title: "Silverthorn"
{Airbrake.Data.EntityCommandCompilationException.BookEntity(HashCode:8807292)}
  CompositeId: "raymondefeist-magicianmaster"
  Id: 2
  Author: "Raymond E. Feist"
  PageCount: 499
  PublicationDate: 11/1/1982
  Title: "Magician: Master"
{Airbrake.Data.EntityCommandCompilationException.BookEntity(HashCode:27416314)}
  CompositeId: "raymondefeist-magicianapprentice"
  Id: 1
  Author: "Raymond E. Feist"
  PageCount: 485
  PublicationDate: 10/1/1982
  Title: "Magician: Apprentice"
```

Cool!  Everything worked just as expected.  As we can see, our `BookEntities` were properly created, and our extra `Id` and `CompositeId` properties were populated as expected.

Now, you may be asking, "Where's the database connection string?"  In an effort to keep things as simple as possible out of the box, the `Entity Framework` defaults to trying to use `localdb` or `SQL Express`, if either is locally installed (which is usually the case when using modern versions of Visual Studio).  In this case, we can connect to `sqlexpress.Airbrake.Data.EntityException.BookEntityContext.dbo` and are greeted with a `BookEntities` table that has all the appropriate columns and is populated with the three recently added books!

Since there are so many potential problems that could lead to `EntityExceptions` that we'll just look at a simple example.  Here we've added the `UpdateBookPageCount(int id, int pageCount, BookEntityContext context)` method:

```cs
/// <summary>
/// Update book page count via iteration.
/// </summary>
/// <param name="id">Book Id.</param>
/// <param name="pageCount">Book Page Count.</param>
/// <param name="context">Context containing Book to update.</param>
private static void UpdateBookPageCount(int id, int pageCount, BookEntityContext context)
{
    // Loop through all Books.
    foreach (var book in context.Books)
    {
        // If id matches, continue.
        if (book.Id != id) continue;
        // Update Page Count.
        book.PageCount = pageCount;
        // Save changes to context.
        context.SaveChanges();
    }
}
```

This method attempts to update the page count of a `BookEntity` via its `Id` property.  However, this implementation is poor, since we're finding said matching book by iterating through all `context.Books` elements, rather than performing a LINQ query or similar.  Still, it should get the job done, so let's test it out:

```cs
UpdateBookPageCount(1, 24_601, context);
```

Unfortunately, executing this code throws an `EntityException` at us, which includes an inner exception:

```
[EXPECTED] System.Data.Entity.Core.EntityException: An error occurred while starting a transaction on the provider connection. See the inner exception for details. ---> System.Data.SqlClient.SqlException: New transaction is not allowed because there are other threads running in the session.
[UNEXPECTED] System.Data.SqlClient.SqlException (0x80131904): New transaction is not allowed because there are other threads running in the session.
```

As indicated by the error, the issue is that we're attempting to perform a new transaction, via `context.SaveChanges()`, while an active transaction thread is already occurring due to the `foreach(var book in context.Books)` iteration loop.  In essence, we aren't allowed to make changes to an element of an iterated collection while said iteration is still taking place.

To resolve this we'll clean up the way we find a book by `Id` by using a simple query, as previously mentioned:

```cs
private static void UpdateBookPageCount(int id, int pageCount, BookEntityContext context)
{
    // Find book by id.
    var result = context.Books.SingleOrDefault(book => book.Id == id);
    if (result == null) return;
    // Update page count.
    result.PageCount = pageCount;
    // Save changes to context.
    context.SaveChanges();
}
```

Executing this new version works as expected, adjusting the `PageCount` property to our database record for the `BookEntity` with `Id` of `1`:

```
{Airbrake.Data.EntityCommandCompilationException.BookEntity(HashCode:45420240)}
  CompositeId: "raymondefeist-magicianapprentice"
  Id: 1
  Author: "Raymond E. Feist"
  PageCount: 24601
  PublicationDate: 10/1/1982
  Title: "Magician: Apprentice"
```

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A close look at the EntityException in .NET, including C# code illustrating how to create, read, update, and delete Entity Framework models.