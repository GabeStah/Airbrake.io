# .NET Exceptions - System.Data.SqlClient.SqlException

As we continue down the beautiful path that winds through our __.NET Exception Handling__ series, today we'll be examining the `System.Data.SqlClient.SqlException`.  The `System.Data.SqlClient.SqlException` is typically thrown when an accessed `SQL Server` returns a warning or error of its own.

In this article, we'll explore where `System.Data.SqlClient.SqlException` resides within the .NET exception hierarchy, examine when `System.Data.SqlClient.SqlExceptions` most commonly appear, and see how to handle them should you encounter one yourself.  So, let's get to it!

## The Technical Rundown

- All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.
- [`System.SystemException`](https://msdn.microsoft.com/en-us/library/system.systemexception(v=vs.110).aspx) is inherited from the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) class.
- [`System.Runtime.InteropServices.ExternalException`](https://msdn.microsoft.com/en-us/library/system.runtime.interopservices.externalexception(v=vs.110).aspx) is inherited from the [`System.SystemException`](https://msdn.microsoft.com/en-us/library/system.systemexception(v=vs.110).aspx) class.
- [`System.Data.Common.DbException`](https://msdn.microsoft.com/en-us/library/system.data.common.dbexception(v=vs.110).aspx) is inherited from the [`System.Runtime.InteropServices.ExternalException`](https://msdn.microsoft.com/en-us/library/system.runtime.interopservices.externalexception(v=vs.110).aspx) class.
- Finally, `System.Data.SqlClient.SqlException` is inherited from the [`System.Data.Common.DbException`](https://msdn.microsoft.com/en-us/library/system.data.common.dbexception(v=vs.110).aspx) class.

## When Should You Use It?

Since the occurrence of a `System.Data.SqlClient.SqlException` is directly related to a problem with the `SQL server`, it's important to take a moment to understand how to connect a `C#` application to an `SQL server`, and therefore what scenarios `System.Data.SqlClient.SqlExceptions` might occur.

As with most anything in .NET, there are many ways to tackle the problem of database connection and usage, so we'll just provide an example for this article, and you can apply it to your own experiences or setups.  For the following code, we're using a [`Microsoft Azure`](https://azure.microsoft.com/en-us/) cloud-based `SQL Server` and `SQL Database`, so there's no need to setup an SQL server on our local machine.  Setting up an `SQL Server/Database` on `Azure` is beyond the scope of this article, but once it's configured, we can connect to it using a standard [`ADO Connection String`](https://docs.microsoft.com/en-us/sql/ado/reference/ado-api/connectionstring-property-ado), which is just a group of `key/value` pairs that informs an application where our database server is located and how to connect to it.  An `ADO` string typically looks something like: `Server=[server],1433;Initial Catalog=[catalog];...`

To make use of our database, and to keep our example code a bit cleaner, we've opted to store our `ADO string` and our `SQL credentials` within the `App.config` file for our C# project.  Below you can see the full `App.config` we're using, with a bit of obfuscation where necessary to retain privacy.  It includes the `applicationSettings` element, where we've stored our app settings related to our `SQL` server:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <configSections>
        <sectionGroup name="applicationSettings" type="System.Configuration.ApplicationSettingsGroup, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" >
            <section name="Airbrake.Data.SqlClient.SqlException.Properties.Settings" type="System.Configuration.ClientSettingsSection, System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false" />
        </sectionGroup>
    </configSections>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5.2" />
    </startup>
    <applicationSettings>
        <Airbrake.Data.SqlClient.SqlException.Properties.Settings>
            <setting name="SqlUserID" serializeAs="String">
                <value>[username]</value>
            </setting>
            <setting name="SqlPassword" serializeAs="String">
                <value>[password]</value>
            </setting>
            <setting name="SqlConnectionString" serializeAs="String">
                <value>Server=[server],1433;Initial Catalog=[catalog];Persist Security Info=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;</value>
            </setting>
        </Airbrake.Data.SqlClient.SqlException.Properties.Settings>
    </applicationSettings>
</configuration>
```

With that in place, we'll take a look at the example application code, which aims to perform two `SQL` queries (one successfully, and one unsuccessfully).  The full code is below, after which we can break it down a bit more to see what's going on:

```cs
using System.Data;
using System.Data.SqlClient;
using Utility;

namespace Airbrake.Data.SqlClient.SqlException
{
    class Program
    {
        static void Main(string[] args)
        {
            // Valid query.
            PerformQuery(@"SELECT TOP 20 pc.Name as CategoryName, p.name as ProductName
                FROM [SalesLT].[ProductCategory] pc
                JOIN [SalesLT].[Product] p
                ON pc.productcategoryid = p.productcategoryid");

            // Invalid query.
            PerformQuery(@"EXECUTE InvalidStoredProcedure");
        }

        private static void PerformQuery(string query)
        {
            try
            {
                // Generate connection string from application settings data.
                SqlConnectionStringBuilder stringBuilder = new SqlConnectionStringBuilder(Properties.Settings.Default.SqlConnectionString)
                {
                    UserID = Properties.Settings.Default.SqlUserID,
                    Password = Properties.Settings.Default.SqlPassword
                };

                // Use connection string to generate new SqlConnection.
                using (SqlConnection connection = new SqlConnection(stringBuilder.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Generate a blank command using connection, then assign CommandText.
                    SqlCommand command = new SqlCommand("", connection)
                    {
                        CommandType = CommandType.Text,
                        CommandText = query
                    };

                    // Execute command as SqlDataReader.
                    SqlDataReader reader = command.ExecuteReader();

                    // Loop through reader to evaluate returned data.
                    while (reader.Read())
                    {
                        // Output data.
                        Logging.Log($"{reader.GetString(0)}\t{reader.GetString(1)}");
                    }

                    // Close reader.
                    reader.Close();

                    // Close connection.
                    connection.Close();
                }
            }
            catch (System.Data.SqlClient.SqlException exception)
            {
                // Output exception information to log.
                Logging.Log(exception);
            }
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

The `Utility` namespace is where our `Logging` class resides, which is just used as a convenience for outputting information during debugging, so we won't go into anymore detail on that section.  The meat and potatoes of our code here is in the `Program.PerformQuery` method:

```cs
private static void PerformQuery(string query)
{
    try
    {
        // Generate connection string from application settings data.
        SqlConnectionStringBuilder stringBuilder = new SqlConnectionStringBuilder(Properties.Settings.Default.SqlConnectionString)
        {
            UserID = Properties.Settings.Default.SqlUserID,
            Password = Properties.Settings.Default.SqlPassword
        };

        // Use connection string to generate new SqlConnection.
        using (SqlConnection connection = new SqlConnection(stringBuilder.ConnectionString))
        {
            // Open the connection.
            connection.Open();

            // Generate a blank command using connection, then assign CommandText.
            SqlCommand command = new SqlCommand("", connection)
            {
                CommandType = CommandType.Text,
                CommandText = query
            };

            // Execute command as SqlDataReader.
            SqlDataReader reader = command.ExecuteReader();

            // Loop through reader to evaluate returned data.
            while (reader.Read())
            {
                // Output data.
                Logging.Log($"{reader.GetString(0)}\t{reader.GetString(1)}");
            }

            // Close reader.
            reader.Close();

            // Close connection.
            connection.Close();
        }
    }
    catch (System.Data.SqlClient.SqlException exception)
    {
        // Output exception information to log.
        Logging.Log(exception);
    }
}
```

The comments provide a bit of guidance, but effectively this is just one way we can create a new connection to our `SQL server` (using the `SqlConnectionStringBuilder` class), open it, pass our `query` parameter to the `CommandText`, then issue an `ExecuteReader` method call to make that request to the server.  Once a result is returned, we loop through it, outputting the information to our log, before closing everything out.  We also are checking to see if any `System.Data.SqlClient.SqlExceptions` occur, and catching those if necessary.

Lastly, we have two actual query strings we're trying:

```cs
// Valid query.
PerformQuery(@"SELECT TOP 20 pc.Name as CategoryName, p.name as ProductName
    FROM [SalesLT].[ProductCategory] pc
    JOIN [SalesLT].[Product] p
    ON pc.productcategoryid = p.productcategoryid");

// Invalid query.
PerformQuery(@"EXECUTE InvalidStoredProcedure");
```

The first is a basic query using the well-known [`AdventureWorks`](https://msdn.microsoft.com/en-us/library/ms124501(v=sql.100).aspx) sample database, grabbing the first 20 `Products` and their associated `ProductCategory`.  This query works fine and the output is as expected:

```
Road Frames	HL Road Frame - Black, 58
Road Frames	HL Road Frame - Red, 58
Helmets	Sport-100 Helmet, Red
Helmets	Sport-100 Helmet, Black
Socks	Mountain Bike Socks, M
Socks	Mountain Bike Socks, L
Helmets	Sport-100 Helmet, Blue
Caps	AWC Logo Cap
Jerseys	Long-Sleeve Logo Jersey, S
Jerseys	Long-Sleeve Logo Jersey, M
Jerseys	Long-Sleeve Logo Jersey, L
Jerseys	Long-Sleeve Logo Jersey, XL
Road Frames	HL Road Frame - Red, 62
Road Frames	HL Road Frame - Red, 44
Road Frames	HL Road Frame - Red, 48
Road Frames	HL Road Frame - Red, 52
Road Frames	HL Road Frame - Red, 56
Road Frames	LL Road Frame - Black, 58
Road Frames	LL Road Frame - Black, 60
Road Frames	LL Road Frame - Black, 62
```

However, our second query explicitly makes a call to a `stored procedure` that doesn't exist in our database, so we're expecting a `System.Data.SqlClient.SqlException` to be raised.  Sure enough, the output shows us exactly the problem:

```
Exception thrown: 'System.Data.SqlClient.SqlException' in System.Data.dll
[EXPECTED] System.Data.SqlClient.SqlException (0x80131904): Could not find stored procedure 'InvalidStoredProcedure'.
```

While this is just a brief glimpse and example, hopefully it illustrates just how a `System.Data.SqlClient.SqlException` might show up, and give some insight for your own future projects working with `SQL` servers.

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A exploration of the System.Data.SqlClient.SqlException in .NET, including an explanation and code example for connecting to and querying SQL databases.
