# .NET Exceptions - System.Data.ObjectNotFoundException

Making our way through our detailed [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, today we'll tackle the lovely System.Data.ObjectNotFoundException.  The `System.Data.ObjectNotFoundException` is typically used and thrown when dealing with ADO.NET (or other data layer components) and an expected object cannot be found.  

Throughout this article we'll explore the `System.Data.ObjectNotFoundException` in a bit more detail, starting with a brief look at where it sits in the overall .NET exception hierarchy.  We'll also look at some functional C# code examples illustrating how `System.Data.ObjectNotFoundExceptions` might be used in your own applications, so let's get to it!

## The Technical Rundown

- All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.
- [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception) is inherited from the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) class.
- [`System.Data.DataException`](https://docs.microsoft.com/en-us/dotnet/api/system.data.dataexception?view=netframework-4.7) inherits from [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception).
- Lastly, `System.Data.ObjectNotFoundException` inherits from [`System.Data.DataException`](https://docs.microsoft.com/en-us/dotnet/api/system.data.dataexception?view=netframework-4.7).

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```xml
<?xml version="1.0" encoding="utf-8"?>
<!-- App.config -->
<configuration>
  <configSections>
    <sectionGroup name="applicationSettings" type="System.Configuration.ApplicationSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089">
      <section name="Airbrake.Data.ObjectNotFoundException.Properties.Settings" type="System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />
    </sectionGroup>
    <!-- For more information on Entity Framework configuration, visit http://go.microsoft.com/fwlink/?LinkID=237468 -->
    <section name="entityFramework" type="System.Data.Entity.Internal.ConfigFile.EntityFrameworkSection, EntityFramework, Version=6.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />
  </configSections>
  <startup>
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.7" />
  </startup>
  <applicationSettings>
    <Airbrake.Data.ObjectNotFoundException.Properties.Settings>
      <setting name="SqlConnectionString" serializeAs="String">
        <value>Server=tcp:[databaseName].database.windows.net,1433;Initial Catalog=adventure;Persist Security Info=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;</value>
      </setting>
      <setting name="SqlUserID" serializeAs="String">
        <value>[user]</value>
      </setting>
      <setting name="SqlPassword" serializeAs="String">
        <value>[password]</value>
      </setting>
    </Airbrake.Data.ObjectNotFoundException.Properties.Settings>
  </applicationSettings>
  <entityFramework>
    <defaultConnectionFactory type="System.Data.Entity.Infrastructure.SqlConnectionFactory, EntityFramework" />
    <providers>
      <provider invariantName="System.Data.SqlClient" type="System.Data.Entity.SqlServer.SqlProviderServices, EntityFramework.SqlServer" />
    </providers>
  </entityFramework>
</configuration>
```

```cs
using System;
using System.Data;
using System.Data.SqlClient;
using Utility;

namespace Airbrake.Data.ObjectNotFoundException
{
    class Program
    {
        static void Main(string[] args)
        {
            // Find Customers with last name 'Miller'.
            Logging.LineSeparator("LastName: Miller");
            ExecuteQuery(
                @"SELECT TOP 20 
                    c.CustomerID, 
                    c.FirstName,
                    c.LastName,
                    c.CompanyName
                FROM
                    [SalesLT].[Customer] c
                WHERE
                    c.LastName = 'Miller'");

            // Find Customers with last name 'Bates'.
            Logging.LineSeparator("LastName: Bates");
            ExecuteQuery(
                @"SELECT TOP 20 
                    c.CustomerID, 
                    c.FirstName,
                    c.LastName,
                    c.CompanyName
                FROM 
                    [SalesLT].[Customer] c
                WHERE
                    c.LastName = 'Bates'");
        }

        /// <summary>
        /// Perform the passed SQL query.
        /// </summary>
        /// <param name="query">Query string to be executed.</param>
        private static void ExecuteQuery(string query)
        {
            try
            {
                // Generate connection string from application settings data.
                var stringBuilder = new SqlConnectionStringBuilder(Properties.Settings.Default.SqlConnectionString)
                {
                    UserID = Properties.Settings.Default.SqlUserID,
                    Password = Properties.Settings.Default.SqlPassword
                };

                // Use connection string to generate new SqlConnection.
                using (var connection = new SqlConnection(stringBuilder.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Generate a blank command using connection, then assign CommandText.
                    var command = new SqlCommand("", connection)
                    {
                        CommandType = CommandType.Text,
                        CommandText = query
                    };

                    // Execute command as SqlDataReader.
                    var reader = command.ExecuteReader();
                    // If no rows in data set, throw ObjectNotFoundException.
                    if (!reader.HasRows)
                        throw new System.Data.ObjectNotFoundException($"No results returned from query: {query}");

                    // Loop through reader to evaluate returned data.
                    while (reader.Read())
                    {
                        // Output row data to log.
                        var data = new object[reader.FieldCount];
                        reader.GetValues(data);
                        Logging.Log(data);
                    }

                    // Close reader.
                    reader.Close();

                    // Close connection.
                    connection.Close();
                }
            }
            catch (System.Data.ObjectNotFoundException exception)
            {
                // Output expected ObjectNotFoundExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Output unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }
    }

}

// <Utility>/Logging.cs
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
            Debug.WriteLine(outputType == OutputType.Timestamp
                ? $"[{StopwatchProxy.Instance.Stopwatch.Elapsed}] {value}"
                : value);
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

As you may be aware, the .NET Framework includes literally _hundreds_ of built-in `Exception` types, each intended for a particular scenario or purpose.  To that end, many built-in exceptions are part of a specific namespace, so even if the actual _name_ of an exception class doesn't give much indication about its intended purpose, the namespace to which it belongs can help clue us in.

I bring this up because our little friend, the `System.Data.ObjectNotFoundException`, is an example of just such an exception.  The phrase `ObjectNotFound` is extremely generic, and the [official documentation](https://docs.microsoft.com/en-us/dotnet/api/system.data.objectnotfoundexception?view=netframework-4.7) description is just as vague: "The exception that is thrown when an object is not present."  With these limited clues as to its intended purpose, we're left to our own devices as developers to decide when it's most appropriate to use the `System.Data.ObjectNotFoundException`.

Enter the actual namespace to which the `System.Data.ObjectNotFoundException` belongs.  While `ObjectNotFound` could obviously apply to any sort of code, `System.Data` indicates it should be applied to database-specific activities, so that's where we'll be using it in our example code.  Specifically, our goal is to throw a `System.Data.ObjectNotFoundException` when one of our attempted SQL queries fails to provide any resulting data.

Before we get into the code, it's worth noting that we're using a remote [`Microsoft Azure`](https://azure.microsoft.com/en-us/) SQL Server, which has a copy of the [`AdventureWorks`](https://msftdbprodsamples.codeplex.com/) sample database installed for testing purposes.  We'll be connecting to this server with a basic ADO.NET connection string, along with our credentials that we've setup in our project's `Properties > Settings` panel in Visual Studio.  These settings are then automatically copied in the `App.config` file, which we can access programmatically when necessary:

```xml
<?xml version="1.0" encoding="utf-8"?>
<!-- App.config -->
<configuration>
  <!-- ... -->
  <applicationSettings>
    <Airbrake.Data.ObjectNotFoundException.Properties.Settings>
      <setting name="SqlConnectionString" serializeAs="String">
        <value>Server=tcp:[databaseName].database.windows.net,1433;Initial Catalog=adventure;Persist Security Info=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;</value>
      </setting>
      <setting name="SqlUserID" serializeAs="String">
        <value>[user]</value>
      </setting>
      <setting name="SqlPassword" serializeAs="String">
        <value>[password]</value>
      </setting>
    </Airbrake.Data.ObjectNotFoundException.Properties.Settings>
  </applicationSettings>
  <!-- ... -->
</configuration>
```

We've also created a `ExecuteQuery(string query)` method, which handles all the database connection and query execution logic for us:

```cs
/// <summary>
/// Perform the passed SQL query.
/// </summary>
/// <param name="query">Query string to be executed.</param>
private static void ExecuteQuery(string query)
{
    try
    {
        // Generate connection string from application settings data.
        var stringBuilder = new SqlConnectionStringBuilder(Properties.Settings.Default.SqlConnectionString)
        {
            UserID = Properties.Settings.Default.SqlUserID,
            Password = Properties.Settings.Default.SqlPassword
        };

        // Use connection string to generate new SqlConnection.
        using (var connection = new SqlConnection(stringBuilder.ConnectionString))
        {
            // Open the connection.
            connection.Open();

            // Generate a blank command using connection, then assign CommandText.
            var command = new SqlCommand("", connection)
            {
                CommandType = CommandType.Text,
                CommandText = query
            };

            // Execute command as SqlDataReader.
            var reader = command.ExecuteReader();
            // If no rows in data set, throw ObjectNotFoundException.
            if (!reader.HasRows)
                throw new System.Data.ObjectNotFoundException($"No results returned from query: {query}");

            // Loop through reader to evaluate returned data.
            while (reader.Read())
            {
                // Output row data to log.
                var data = new object[reader.FieldCount];
                reader.GetValues(data);
                Logging.Log(data);
            }

            // Close reader.
            reader.Close();

            // Close connection.
            connection.Close();
        }
    }
    catch (System.Data.ObjectNotFoundException exception)
    {
        // Output expected ObjectNotFoundExceptions.
        Logging.Log(exception);
    }
    catch (Exception exception)
    {
        // Output unexpected Exceptions.
        Logging.Log(exception, false);
    }
}
```

We use a `SqlConnectionStringBuilder` instance to create a connection string using our aforementioned project settings.  Once connected, we then perform a rudimentary `SqlCommand` execution by passing the `query` parameter as the `CommandText` property.  After executing the command we critically check if any rows are present in the data result.  If not, we throw a new `System.Data.ObjectNotFoundException` and include the problematic query in the error message.  Otherwise, we collect the row data and output it to the log.

Again, this is by no means a complete method to actually handle all (let alone most) SQL queries, but it serves the purpose of illustrating how we might use `System.Data.ObjectNotFoundExceptions` in data-related code.  To test this method out we are passing two queries, both of which query customers in the database based on their respective last names:

```cs
static void Main(string[] args)
{
    // Find Customers with last name 'Miller'.
    Logging.LineSeparator("LastName: Miller");
    ExecuteQuery(
        @"SELECT TOP 20 
            c.CustomerID, 
            c.FirstName,
            c.LastName,
            c.CompanyName
        FROM
            [SalesLT].[Customer] c
        WHERE
            c.LastName = 'Miller'");

    // Find Customers with last name 'Bates'.
    Logging.LineSeparator("LastName: Bates");
    ExecuteQuery(
        @"SELECT TOP 20 
            c.CustomerID, 
            c.FirstName,
            c.LastName,
            c.CompanyName
        FROM 
            [SalesLT].[Customer] c
        WHERE
            c.LastName = 'Bates'");
}
```

The first query, which retrieves all records with the `LastName` of `"Miller"`, works just fine and outputs a handful of customer records:

```
----------- LastName: Miller -----------
310
"Frank"
"Miller"
"Orange Bicycle Company"

313
"Dylan"
"Miller"
"Metropolitan Manufacturing"

316
"Ben"
"Miller"
"Low Price Cycles"

322
"Matthew"
"Miller"
"Tachometers and Accessories"

325
"Virginia"
"Miller"
"All Cycle Shop"

30018
"Virginia"
"Miller"
"All Cycle Shop"

30019
"Matthew"
"Miller"
"Tachometers and Accessories"

30021
"Ben"
"Miller"
"Low Price Cycles"

30022
"Dylan"
"Miller"
"Metropolitan Manufacturing"

30023
"Frank"
"Miller"
"Orange Bicycle Company"
```

Now, let's try the same query, but using the `LastName` of `"Bates"`, which doesn't exist for any customer records in the database:

```
----------- LastName: Bates ------------
[EXPECTED] System.Data.ObjectNotFoundException: No results returned from query: SELECT TOP 20 
                    c.CustomerID, 
                    c.FirstName,
                    c.LastName,
                    c.CompanyName
                FROM 
                    [SalesLT].[Customer] c
                WHERE
                    c.LastName = 'Bates' 
```

All in all, quite boring if you ask me, but our code did what we asked.  Most of the time we'll get some data back from a query and output said data.  However, when a query doesn't return any results, we throw a `System.Data.ObjectNotFoundException`.  Obviously it's not typically ideal to use exceptions as means of controlling application flow, but in certain situations I can imagine a use for throwing a `System.Data.ObjectNotFoundException` when a database query fails to find expected records for one reason or another.

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A close look at the System.Data.ObjectNotFoundException in .NET, including C# code showing when throwing such exceptions may be relevant.