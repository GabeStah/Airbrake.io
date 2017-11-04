# .NET Exceptions - System.Data.SqlTypes.SqlTypeException

Fast approaching the conclusion of our current [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, today we'll be looking into the **System.Data.SqlTypes.SqlTypeException**.  The appearance of an `SqlTypeException` is the result of something going wrong while using the [`System.Data.SqlTypes`](https://docs.microsoft.com/en-us/dotnet/api/system.data.sqltypes?view=netframework-4.7.1) namespace classes.

In this article we'll examine the `SqlTypeException` by seeing where it resides in the overall .NET exception hierarchy.  We'll then look at a fully functional C# code sample that will illustrate one specific technique for connecting to an ADO.NET data source, performing queries, and how we could encounter `SqlTypeExceptions` under certain circumstances, particularly when dealing with abnormal or difficult to manage data types.  Let's get to it!

## The Technical Rundown

All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.  The full exception hierarchy of this error is:

- [`System.Object`](https://docs.microsoft.com/en-us/dotnet/api/system.object)
    - [`System.Exception`](https://docs.microsoft.com/en-us/dotnet/api/system.exception)
        - [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception)
            - `SqlTypeException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```cs
using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Utility;

namespace Airbrake.Data.SqlTypes.SqlTypeException
{
    internal class Program
    {
        private const string ConnectionString = @"Data Source=I7\SQLEXPRESS;Initial Catalog=pubs;Integrated Security=True";

        private enum SqlCommandExecutionType
        {
            NonQuery,
            Reader,
            Scalar,
            XmlReader
        }

        private static void Main()
        {
            Logging.LineSeparator("INSERT VALID, PROPER DATE");
            ExecuteQuery(GetQueryStringFromBook(
                new Book("Magician", "Raymond E. Feist", 681, new DateTime(1982, 10, 1))
            ));

            Logging.LineSeparator("GET DATA");
            ExecuteQuery("SELECT * FROM dbo.Book;", SqlCommandExecutionType.Reader);
            
            Logging.LineSeparator("INSERT INVALID DATE");
            ExecuteQuery(GetQueryStringFromBook(
                new Book("Silverthorn", "Raymond E. Feist", 432, new DateTime(1750, 1, 1))
            ));

            Logging.LineSeparator("INSERT INVALID, CONVERTED DATE");
            ExecuteQuery(GetQueryStringFromBook(
                new Book("A Darkness At Sethanon", "Raymond E. Feist", 527, new DateTime(1750, 1, 1)), true)
            );
        }

        /// <summary>
        /// Executes the passed query string, using the passed SqlCommandExecutionType.
        /// </summary>
        /// <param name="query">Query string to execute.</param>
        /// <param name="type">SqlCommandExecutionType to use, if applicable.</param>
        private static void ExecuteQuery(string query, SqlCommandExecutionType type = SqlCommandExecutionType.NonQuery)
        {
            // Instantiate connection in using block to properly close afterward.
            using (var connection = new SqlConnection(ConnectionString))
            {
                // Instantiate a command.
                var command = new SqlCommand(query, connection);
 
                try
                {
                    connection.Open();
                    // If no command text, return.
                    if (command.CommandText == "") return;

                    // Check passed execution type.
                    switch (type)
                    {
                        case SqlCommandExecutionType.NonQuery:
                            command.ExecuteNonQuery();
                            break;
                        case SqlCommandExecutionType.Reader:
                            var reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                var data = new object[reader.FieldCount - 1];
                                reader.GetValues(data);
                                Logging.Log(data);
                            }
                            reader.Close();
                            break;
                        case SqlCommandExecutionType.Scalar:
                            command.ExecuteScalar();
                            break;
                        case SqlCommandExecutionType.XmlReader:
                            var xmlReader = command.ExecuteXmlReader();
                            while (xmlReader.Read())
                            {
                                Logging.Log(xmlReader);
                            }
                            xmlReader.Close();
                            break;
                        default:
                            command.ExecuteNonQuery();
                            break;
                    }
                }
                catch (System.Data.SqlTypes.SqlTypeException exception)
                {
                    // Output expected SqlTypeExceptions.
                    Logging.Log(exception);
                }
                catch (SqlException exception)
                {
                    // Output unexpected SqlExceptions.
                    Logging.Log(exception, false);
                }
                catch (Exception exception)
                {
                    // Output unexpected Exceptions.
                    Logging.Log(exception, false);
                }
            }
        }

        /// <summary>
        /// Create a query string from passed Book.
        /// </summary>
        /// <param name="book">Book from which to create query string.</param>
        /// <param name="shouldChangeDateType">Determines if date values should be converted to compatible type.</param>
        /// <returns>Query string.</returns>
        private static string GetQueryStringFromBook(IBook book, bool shouldChangeDateType = false)
        {
            try
            {
                if (shouldChangeDateType)
                {
                    return "INSERT INTO dbo.Book (Title, Author, PageCount, PublicationDate) " +
                           $"VALUES ('{book.Title}', '{book.Author}', '{book.PageCount}', '{new SqlDateTime(book.PublicationDate.Value)}');";
                }
                return "INSERT INTO dbo.Book (Title, Author, PageCount, PublicationDate) " +
                       $"VALUES ('{book.Title}', '{book.Author}', '{book.PageCount}', '{book.PublicationDate}');";
            }
            catch (System.Data.SqlTypes.SqlTypeException exception)
            {
                // Output expected SqlTypeExceptions.
                Logging.Log(exception);
            }
            catch (SqlException exception)
            {
                // Output unexpected SqlExceptions.
                Logging.Log(exception, false);
            }
            catch (Exception exception)
            {
                // Output unexpected Exceptions.
                Logging.Log(exception, false);
            }
            return null;
        }
    }
}
```

This code sample also uses the [`Book.cs`](https://github.com/GabeStah/Airbrake.io/blob/master/lib/csharp/Utility/Utility/Book.cs) class, the full code of which can be [seen here via GitHub](https://github.com/GabeStah/Airbrake.io/blob/master/lib/csharp/Utility/Utility/Book.cs).

This code sample also uses the [`Logging.cs`](https://github.com/GabeStah/Airbrake.io/blob/master/lib/csharp/Utility/Utility/Logging.cs) helper class, the full code of which can be [seen here via GitHub](https://github.com/GabeStah/Airbrake.io/blob/master/lib/csharp/Utility/Utility/Logging.cs).

## When Should You Use It?

Since an `SqlTypeException` only appears when dealing with [`System.Data.SqlTypes`](https://docs.microsoft.com/en-us/dotnet/api/system.data.sqltypes?view=netframework-4.7.1) namespaced classes, let's start at a more basic level with a simple SQL database connection and query.  Our `Program` class has a property and an enumeration to start things off, which we'll use in a moment to simplify our connection and query methods:

```cs
internal class Program
{
    private const string ConnectionString = @"Data Source=I7\SQLEXPRESS;Initial Catalog=pubs;Integrated Security=True";

    private enum SqlCommandExecutionType
    {
        NonQuery,
        Reader,
        Scalar,
        XmlReader
    }

    // ...
}
```

Our primary query logic takes place in the `ExecuteQuery(string query, SqlCommandExecutionType type = SqlCommandExecutionType.NonQuery)` method:

```cs
/// <summary>
/// Executes the passed query string, using the passed SqlCommandExecutionType.
/// </summary>
/// <param name="query">Query string to execute.</param>
/// <param name="type">SqlCommandExecutionType to use, if applicable.</param>
private static void ExecuteQuery(string query, SqlCommandExecutionType type = SqlCommandExecutionType.NonQuery)
{
    // Instantiate connection in using block to properly close afterward.
    using (var connection = new SqlConnection(ConnectionString))
    {
        // Instantiate a command.
        var command = new SqlCommand(query, connection);

        try
        {
            connection.Open();
            // If no command text, return.
            if (command.CommandText == "") return;

            // Check passed execution type.
            switch (type)
            {
                case SqlCommandExecutionType.NonQuery:
                    command.ExecuteNonQuery();
                    break;
                case SqlCommandExecutionType.Reader:
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        var data = new object[reader.FieldCount - 1];
                        reader.GetValues(data);
                        Logging.Log(data);
                    }
                    reader.Close();
                    break;
                case SqlCommandExecutionType.Scalar:
                    command.ExecuteScalar();
                    break;
                case SqlCommandExecutionType.XmlReader:
                    var xmlReader = command.ExecuteXmlReader();
                    while (xmlReader.Read())
                    {
                        Logging.Log(xmlReader);
                    }
                    xmlReader.Close();
                    break;
                default:
                    command.ExecuteNonQuery();
                    break;
            }
        }
        catch (System.Data.SqlTypes.SqlTypeException exception)
        {
            // Output expected SqlTypeExceptions.
            Logging.Log(exception);
        }
        catch (SqlException exception)
        {
            // Output unexpected SqlExceptions.
            Logging.Log(exception, false);
        }
        catch (Exception exception)
        {
            // Output unexpected Exceptions.
            Logging.Log(exception, false);
        }
    }
}
```

This method starts by establishing a connection to the local `ConnectionString` property, which, in this case, is connecting to a local `Sql Express` instance (but would work with any valid connection string).  By establishing the connection within a `using` block we ensure that the connection closes itself once this code block has completed execution.

We use the connection and passed `query` string to create a new `SqlCommand` instance, then attempt to `Open()` the connection within our `try-catch` block.  We ensure that the `CommandText` property isn't empty, since this can occur if we get exceptions elsewhere in the code that might prevent the `SqlCommand` from being properly formed.

Finally, we perform a `switch` check for the passed `SqlCommandExecutionType type` parameter to determine how we need to process this particular `query` string.  For stuff like `INSERT` or `DELETE` commands we'd typically use `ExecuteNonQuery()`, while reading via `SELECT` is often going to use `ExecuteReader()`.  This basic structure can obviously be expanded a great deal to properly handle different types of incoming data structures, but it'll serve our basic purposes here.  In the event we're executing a query with an output, we use the `Logging.Log(...)` method to output the information to the log.

We're making use of our `Book` class here to have a simple data structure we can try to insert into our database.  I've already created the `Book` database table via this SQL query:

```sql
CREATE TABLE [dbo].[Book] (
    [Id]              INT         IDENTITY (1, 1) NOT NULL,
    [Title]           NCHAR (100) NOT NULL,
    [Author]          NCHAR (100) NOT NULL,
    [PageCount]       INT         NULL,
    [PublicationDate] DATETIME    NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);
```

With this basic data structure and the properties of our `Book` class we can create the `GetQueryFromBook(IBook book, bool shouldChangeDataType = false)` method:

```cs
/// <summary>
/// Create a query string from passed Book.
/// </summary>
/// <param name="book">Book from which to create query string.</param>
/// <param name="shouldChangeDateType">Determines if date values should be converted to compatible type.</param>
/// <returns>Query string.</returns>
private static string GetQueryStringFromBook(IBook book, bool shouldChangeDateType = false)
{
    try
    {
        if (shouldChangeDateType)
        {
            return "INSERT INTO dbo.Book (Title, Author, PageCount, PublicationDate) " +
                    $"VALUES ('{book.Title}', '{book.Author}', '{book.PageCount}', '{new SqlDateTime(book.PublicationDate.Value)}');";
        }
        return "INSERT INTO dbo.Book (Title, Author, PageCount, PublicationDate) " +
                $"VALUES ('{book.Title}', '{book.Author}', '{book.PageCount}', '{book.PublicationDate}');";
    }
    catch (System.Data.SqlTypes.SqlTypeException exception)
    {
        // Output expected SqlTypeExceptions.
        Logging.Log(exception);
    }
    catch (SqlException exception)
    {
        // Output unexpected SqlExceptions.
        Logging.Log(exception, false);
    }
    catch (Exception exception)
    {
        // Output unexpected Exceptions.
        Logging.Log(exception, false);
    }
    return null;
}
```

This method attempts to create a simple `INSERT` query string from the passed `IBook book` parameter object.  There's not much logic here, save for the `bool shouldChangeDataType` parameter value, which determines if we should attempt to change the data type of our `DateTime` parameter before inserting it into our query string.  We'll see what this does in a moment.

To test everything out we'll start simple by creating a new `Book` instance, retrieving an `INSERT` query string from this new `Book's` properties, and then executing the query via the `ExecuteQuery(...)` method.  Just to confirm things work properly, we'll try a simple `SELECT` query to follow our insertion, to see if our `Book` was actually added to the database:

```cs
private static void Main()
{
    Logging.LineSeparator("INSERT VALID, PROPER DATE");
    ExecuteQuery(GetQueryStringFromBook(
        new Book("Magician", "Raymond E. Feist", 681, new DateTime(1982, 10, 1))
    ));

    Logging.LineSeparator("GET DATA");
    ExecuteQuery("SELECT * FROM dbo.Book;", SqlCommandExecutionType.Reader);

    // ..
    
}
```

Executing the code above works as expected and produces the following output:

```
------ INSERT VALID, PROPER DATE -------
--------------- GET DATA ---------------
31
"Magician                                                                                            "
"Raymond E. Feist                                                                                    "
681
```

You'll have to pardon the odd string formatting in the output.  Since `ExecuteQuery(...)` doesn't optimize the output, and merely pushes all column values into an `object[]` array, we get some strange formatting.  But, the look doesn't matter.  What matters is we've confirmed our `INSERT` worked and our `Book` was properly added to the database!

However, let's try another `Book` insertion with a slightly invalid `DateTime` of `January 1st, 1750`:

```cs
Logging.LineSeparator("INSERT INVALID DATE");
ExecuteQuery(GetQueryStringFromBook(
    new Book("Silverthorn", "Raymond E. Feist", 432, new DateTime(1750, 1, 1))
));
```

Executing these lines produces an unexpected `SqlException` (not to be confused with an `SqlTypeException`):

```
--------- INSERT INVALID DATE ----------
[UNEXPECTED] System.Data.SqlClient.SqlException (0x80131904): The conversion of a varchar data type to a datetime data type resulted in an out-of-range value.
```

As the error message indicates, the problem here is that our code has attempted to convert a `varchar` to a `datetime` type that is **out of range** of expected values.  In other words, our `PublicationDate` property of `January 1st, 1750` is being converted to a valid format (i.e. `1/1/1750 12:00:00 AM`).  However, the SQL column type of [`DATETIME`](https://docs.microsoft.com/en-us/sql/t-sql/data-types/datetime-transact-sql), which is what the `dbo.Book.PublicationDate` column is set to, can only accept date values of `January 1, 1753, through December 31, 9999`.  Since the year `1750` is before this period, we get the `SqlException` seen above.

There are a few solutions to this issue.  The first (and arguably best) option is to simply **avoid using `DATETIME` column types in SQL tables.**  `DATETIME` is a bit outdated, and should be replaced with the [`DATETIME2`](https://docs.microsoft.com/en-us/sql/t-sql/data-types/datetime2-transact-sql) SQL column type, which was designed to be both backward compatible with all previous `DATETIME` values, while also giving a bigger date range of `January 1,1 CE through December 31, 9999 CE`.  Thus, if our `dbo.Book.PublicationDate` column was a `DATETIME2` type, we'd be fine.

However, since changing database columns is dangerous and not always feasible for existing data sets, another alternative solution introduced in .NET is the aforementioned [`System.Data.SqlTypes`](https://docs.microsoft.com/en-us/dotnet/api/system.data.sqltypes?view=netframework-4.7.1) classes.  These SQL-specific data types are explicitly designed to mirror the functionality of SQL column types in your database.  Thus, if your .NET class objects use [`SqlDateTime`](https://docs.microsoft.com/en-us/dotnet/api/system.data.sqltypes.sqldatetime?view=netframework-4.7.1) instead of the normal `DateTime` type, this will ensure that you cannot create a date within an object that isn't compatible with your SQL database.

To illustrate this, here we're creating another `Book` and generating an `INSERT` query:

```cs
Logging.LineSeparator("INSERT INVALID, CONVERTED DATE");
ExecuteQuery(GetQueryStringFromBook(
    new Book("A Darkness At Sethanon", "Raymond E. Feist", 527, new DateTime(1750, 1, 1)), true)
);
```

The second `true` argument passed to `GetQueryStringFromBook(...)` will return this generated query string:

```cs
return "INSERT INTO dbo.Book (Title, Author, PageCount, PublicationDate) " +
        $"VALUES ('{book.Title}', '{book.Author}', '{book.PageCount}', '{new SqlDateTime(book.PublicationDate.Value)}');";
```

As you can see, this explicitly creates a new `SqlDateTime` instance with the value of `book.PublicationDate.Value`.

Executing this code produces the following output:

```
---- INSERT INVALID, CONVERTED DATE ----
[EXPECTED] System.Data.SqlTypes.SqlTypeException: SqlDateTime overflow. Must be between 1/1/1753 12:00:00 AM and 12/31/9999 11:59:59 PM.
```

Now we're actually getting an `SqlTypeException`, rather than the `SqlException` we saw above.  This error message is much more explicit, informing us that `SqlDateTime` cannot accept a date value outside of the specified range.  If we were to refactor our `Book` class we could change the `PublicationDate` type to `SqlDateTime`, which would prevent this issue from coming up again since we'd be unable to use invalid date values.

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A close look at the System.Data.SqlTypes.SqlTypeException in .NET, including a basic code sample to perform SQL queries using abnormal dates.