# .NET Exceptions - System.Web.Services.Protocols.SoapException

Making our way through the detailed [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, today we'll be tackling the System.Web.Services.Protocols.SoapException.  The `SoapException` is only the second exception we've covered in this series that deals with the Windows Communication Foundation (`WCF`) framework, which is used to build service-oriented applications.  The `System.Web.Services.Protocols.SoapException` is a rather fundamental error type, as it's the basis of exceptions thrown by WCF apps when a client makes service calls via the SOAP protocol.

In this article we'll explore the `SoapException` in more detail, looking at where it fits within the .NET exception hierarchy, along with some sample code to illustrate how `SoapExceptions` are best thrown (and caught) in your own apps.  Let's get to it!

## The Technical Rundown

- All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.
- [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception) is inherited from the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) class.
- `System.Web.Services.Protocols.SoapException` inherits directly from [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception).

## When Should You Use It?

The sample code we'll be using has just one major goal: Produce an exception on the server side (within the `service`), which should translate into a `SoapException` on the `client` side.  Since we'll be dealing with the WCF framework and a service application to illustrate how `System.Web.Services.Protocols.SoapExceptions` should be used, we'll be using some of the example service application code from our [`System.ServiceModel.FaultException`](https://airbrake.io/blog/dotnet-exception-handling/system-servicemodel-faultexception) article.  We'll begin with the full sample code example below, after which we'll dig into it in a bit more detail.

```cs
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace LibraryService
{
    [DataContract]
    public class InvalidBookFault
    {
        [DataMember]
        public bool Result { get; set; }
        [DataMember]
        public string Message { get; set; }
        [DataMember]
        public string Description { get; set; }
    }

    [ServiceContract]
    public interface ILibraryService
    {
        [FaultContract(typeof(InvalidBookFault))]
        [OperationContract]
        [WebGet]
        bool ReserveBook(string title, string author);
    }
}

using System.ServiceModel;
using System.Web.Services.Protocols;
using Utility;

namespace LibraryService
{
    public interface IBook
    {
        string Author { get; set; }
        string Title { get; set; }
        bool Reserved { get; set; }
    }

    public class Book : IBook
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public bool Reserved { get; set; }

        public Book() { }

        public Book(string title, string author)
        {
            Title = title;
            Author = author;
        }
    }

    public class LibraryService : ILibraryService
    {
        public bool ReserveBook(string title, string author)
        {
            try
            {
                // Check if title value is null or has no characters.
                // Null must be checked prior to length to avoid checking an invalid object.
                if (title is null || title.Length == 0)
                {
                    throw new SoapException($"ReserveBook() parameter 'title' must be a valid string.",
                        new System.Xml.XmlQualifiedName("InvalidParameter", this.ToString()));
                }
                // Check if author value is null or has no characters.
                // Null must be checked prior to length to avoid checking an invalid object.
                if (author is null || author.Length == 0)
                {
                    throw new SoapException($"ReserveBook() parameter 'title' must be a valid string.",
                        new System.Xml.XmlQualifiedName("InvalidParameter", this.ToString()));
                }

                // Create book record and reserve it (a real service
                // would likely use a database connection instead).
                var book = new Book(title, author)
                {
                    Reserved = true
                };
                // Output reservation and book data to server.
                Logging.Log("RESERVATION SUCCESSFUL");
                Logging.LineSeparator();
                Logging.Log(book);
                return true;
            }
            catch (SoapException exception)
            {
                // Log the exception to server.
                Logging.Log(exception);

                // Generate new fault and set details.
                var fault = new InvalidBookFault
                {
                    Description = exception.Message,
                    Message = exception.Message,
                    Result = false
                };

                // Throw newly created FaultException of appropriate type.
                throw new FaultException<InvalidBookFault>(fault, new FaultReason(exception.Message));
            }
        }
    }
}

using System;
using System.ServiceModel;
using Utility;

namespace Airbrake.Web.Services.Protocols.SoapException
{
    class Program
    {
        static void Main(string[] args)
        {

            // Instantiate new LibraryServiceClient.
            var client = new LibraryServiceReference.LibraryServiceClient();

            try
            {
                // Open client connection.
                client.Open();

                // Reserve a valid book.
                client.ReserveBook("The Hobbit", "J.R.R. Tolkien");
                // Reserve an invalid book.
                client.ReserveBook("The Hobbit", null);

                // Close client connection.
                client.Close();
            }
            catch (System.Web.Services.Protocols.SoapException exception)
            {
                // Never triggers since client only receives FaultExceptions.
                Logging.Log(exception);
                client.Abort();
            }
            // Catch our SOAP fault type. 
            catch (FaultException<LibraryServiceReference.InvalidBookFault> exception)
            {
                // Log expected FaultException<LibraryServiceReference.InvalidBookFault>.
                Logging.Log(exception);
                client.Abort();
            }
            catch (FaultException exception)
            {
                // Log unexpected FaultExceptions.
                Logging.Log(exception, false);
                client.Abort();
            }
            catch (Exception exception)
            {
                // Log unexpected Exceptions.
                Logging.Log(exception, false);
                client.Abort();
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
        /// 
        /// ObjectDumper class from <see cref="http://stackoverflow.com/questions/852181/c-printing-all-properties-of-an-object"/>.
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
        public static void LineSeparator(int length = 40)
        {
#if DEBUG
            Debug.WriteLine(new string('-', length));
#else
            Console.WriteLine(new string('-', length));
#endif
        }
    }
}
```

---

Before we break down the example code, it's worth noting that the type of application this code is used in matters (to some degree).  In particular, since we're using the WCF framework for the `LibraryService.ILibraryService` interface and the `LibraryService.LibraryService` class, that code should be used in a `WCF Service Library` or similar project.  This will generate some proper defaults values within files like the critical `App.config`.

With that out of the way, let's dig into the code.  We begin with a basic `Book` class that implements the `IBook` interface, which we'll use as the basis of our service:

```cs
public interface IBook
{
    string Author { get; set; }
    string Title { get; set; }
    bool Reserved { get; set; }
}

public class Book : IBook
{
    public string Title { get; set; }
    public string Author { get; set; }
    public bool Reserved { get; set; }

    public Book() { }

    public Book(string title, string author)
    {
        Title = title;
        Author = author;
    }
}
```

Next comes the basic structure of our service.  We've defined the `InvalidBookFault` class, which applies the `DataContract` attribute (along with the `DataMember` attribute for its properties).  These attributes simply tell the runtime that this object can be serialized (i.e. transformed to and from text)making it easier to transfer across the web:

```cs
[DataContract]
public class InvalidBookFault
{
    [DataMember]
    public bool Result { get; set; }
    [DataMember]
    public string Message { get; set; }
    [DataMember]
    public string Description { get; set; }
}
```

We then make use of that `InvalidBookFault` class in the `ILibraryService` interface, which declares just one `ReserveBook` method.  The interface applies the `ServiceContract` attribute, which does what it sounds like and creates a service contract with the underlying WCF application.  The `ReserveBook` method's `OperationContract` is used in conjunction with `ServiceContract`, informing the service that this method can be invoked by the WCF app.  Lastly, the `FaultContract` attribute specifies that we want any errors during processing within this method to return an `InvalidBookFault` (instead of the default `FaultException`):

```cs
[ServiceContract]
public interface ILibraryService
{
    [FaultContract(typeof(InvalidBookFault))]
    [OperationContract]
    [WebGet]
    bool ReserveBook(string title, string author);
}
```

The final part of our `service` code is the actual `ReserveBook(string title, string author)` method implementation.  This method doesn't do much and is mostly just an example, but its purpose is to reserve a book based on the passed `title` and `author`.  If either field is empty an exception is thrown, otherwise the book is reserved and output is displayed:

```cs
public class LibraryService : ILibraryService
{
    public bool ReserveBook(string title, string author)
    {
        // Check if title value is null or has no characters.
        // Null must be checked prior to length to avoid checking an invalid object.
        if (title is null || title.Length == 0)
        {
            throw new SoapException($"ReserveBook() parameter 'title' must be a valid string.",
                new System.Xml.XmlQualifiedName("InvalidParameter", this.ToString()));
        }
        // Check if author value is null or has no characters.
        // Null must be checked prior to length to avoid checking an invalid object.
        if (author is null || author.Length == 0)
        {
            throw new SoapException($"ReserveBook() parameter 'title' must be a valid string.",
                new System.Xml.XmlQualifiedName("InvalidParameter", this.ToString()));
        }

        // Create book record and reserve it (a real service
        // would likely use a database connection instead).
        var book = new Book(title, author)
        {
            Reserved = true
        };
        // Output reservation and book data to server.
        Logging.Log("RESERVATION SUCCESSFUL");
        Logging.LineSeparator();
        Logging.Log(book);
        return true;
    }
}
```

Now that our `service` layer is all setup we can move onto the `client` code, which isn't too fancy.  It begins by creating a new client connection to the `LibraryServiceReference`, then tries to reserve two books (first a valid, then an invalid one that's missing an author argument).  It also has numerous `catch` clauses to handle any unexpected exceptions:

```cs
static void Main(string[] args)
{
    // Instantiate new LibraryServiceClient.
    var client = new LibraryServiceReference.LibraryServiceClient();

    try
    {
        // Open client connection.
        client.Open();

        // Reserve a valid book.
        client.ReserveBook("The Hobbit", "J.R.R. Tolkien");
        // Reserve an invalid book.
        client.ReserveBook("The Hobbit", null);

        // Close client connection.
        client.Close();
    }
    catch (System.Web.Services.Protocols.SoapException exception)
    {
        // Never triggers since client only receives FaultExceptions.
        Logging.Log(exception);
        client.Abort();
    }
    // Catch our SOAP fault type. 
    catch (FaultException<LibraryServiceReference.InvalidBookFault> exception)
    {
        // Log expected FaultException<LibraryServiceReference.InvalidBookFault>.
        Logging.Log(exception);
        client.Abort();
    }
    catch (FaultException exception)
    {
        // Log unexpected FaultExceptions.
        Logging.Log(exception, false);
        client.Abort();
    }
    catch (Exception exception)
    {
        // Log unexpected Exceptions.
        Logging.Log(exception, false);
        client.Abort();
    }
}
```

If we execute the above `client` code and attempt to reserve some books we get the following output including a success then a failure:

```
RESERVATION SUCCESSFUL
--------------------
{LibraryService.Book(HashCode:37916227)}
  Title: "The Hobbit"
  Author: "J.R.R. Tolkien"
  Reserved: True

[UNEXPECTED] System.ServiceModel.FaultException: The server was unable to process the request due to an internal error.  For more information about the error, either turn on IncludeExceptionDetailInFaults (either from ServiceBehaviorAttribute or from the <serviceDebug> configuration behavior) on the server in order to send the exception information back to the client, or turn on tracing as per the Microsoft .NET Framework SDK documentation and inspect the server trace logs.
```

There's a couple interesting things going on here.  Consider that the `LibraryService.ReserveBook(string title, string author)` method explicitly throws a `System.Web.Services.Protocols.SoapException` when a reservation fails.  Moreover, the `ILibraryService` interface implemented a `FaultContract` attribute, so we return a `LibraryServiceReference.InvalidBookFault` if something goes wrong.  However, in spite of both of these, we can see from the above output that our client code ended up catching a plain `FaultException`.

As indicated by the error message, this is due to the configuration of the service application.  Specifically, we currently have the `<serviceDebug includeExceptionDetailInFaults="False" />` `behavior` set, which suppresses some exception details that would otherwise be included.  Let's try setting that value to `True` and rerunning our client code:

```
RESERVATION SUCCESSFUL
--------------------
{LibraryService.Book(HashCode:37916227)}
  Title: "The Hobbit"
  Author: "J.R.R. Tolkien"
  Reserved: True

[UNEXPECTED] System.ServiceModel.FaultException`1[System.ServiceModel.ExceptionDetail]: ReserveBook() parameter 'title' must be a valid string. (Fault Detail is equal to An ExceptionDetail, likely created by IncludeExceptionDetailInFaults=true, whose value is:
System.Web.Services.Protocols.SoapException: ReserveBook() parameter 'title' must be a valid string.
```

Even though we're still catching a basic `FaultException` on the client side, we're able to see that the underlying reason (the base exception on the server side) was a `System.Web.Services.Protocols.SoapException`, which was our intent.  However, we're still not making use of the custom `InvalidBookFault` that we created.

As it happens, the proper way to handle custom exceptions on the server side of a WCF service application is to explicitly create a new instance of the fault object when it's necessary, then throw a `FaultException<T>` of that type.  Therefore, we've surrounded our logical code within the `LibraryService.ReserveBook(string title, string author)` method with a `try-catch` block that catches a `SoapException`.  When a `SoapException` is caught we create a new `InvalidBookFault` instance, assign some values to its properties, then `throw` a new `FaultException<InvalidBookFault>` and pass in the `InvalidBookFault` instance that was created:

```cs
public class LibraryService : ILibraryService
{
    public bool ReserveBook(string title, string author)
    {
        try
        {
            // Check if title value is null or has no characters.
            // Null must be checked prior to length to avoid checking an invalid object.
            if (title is null || title.Length == 0)
            {
                throw new SoapException($"ReserveBook() parameter 'title' must be a valid string.",
                    new System.Xml.XmlQualifiedName("InvalidParameter", this.ToString()));
            }
            
            //...
        }
        catch (SoapException exception)
        {
            // Log the exception to server.
            Logging.Log(exception);

            // Generate new fault and set details.
            var fault = new InvalidBookFault
            {
                Description = exception.Message,
                Message = exception.Message,
                Result = false
            };

            // Throw newly created FaultException of appropriate type.
            throw new FaultException<InvalidBookFault>(fault, new FaultReason(exception.Message));
        }
    }
}
```

As a result, calling the service from our client code now produces the following output:

```
RESERVATION SUCCESSFUL
--------------------
{LibraryService.Book(HashCode:28068188)}
  Title: "The Hobbit"
  Author: "J.R.R. Tolkien"
  Reserved: True

[EXPECTED] System.Web.Services.Protocols.SoapException: ReserveBook() parameter 'title' must be a valid string.

[EXPECTED] System.ServiceModel.FaultException`1[Airbrake.Web.Services.Protocols.SoapException.LibraryServiceReference.InvalidBookFault]: ReserveBook() parameter 'title' must be a valid string. (Fault Detail is equal to Airbrake.Web.Services.Protocols.SoapException.LibraryServiceReference.InvalidBookFault).: ReserveBook() parameter 'title' must be a valid string.
```

We're now able to catch all _expected_ exceptions throughout the process, both server side and client side.

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A close look at the System.Web.Services.Protocols.SoapException class in .NET, including a C# code sample showing how to properly pass custom faults.