# .NET Exceptions - System.ServiceModel.FaultException

Moving along through our [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series today we're going to take a gander at the `System.ServiceModel.FaultException`.  The `System.ServiceModel.FaultException` is the first error we've dealt with in this series that is tied directly to the Windows Communication Foundation (`WCF`) platform of services.  These services act as a remote interface, such as an API, that can be accessed by clients and other applications to perform some actions, without the client knowing about the underlying logic of the service.

In this article we'll dig into just what the `System.ServiceModel.FaultException` is including where it sits in the .NET exception hierarchy.  We'll also explore some functional C# code examples which will illustrate how `System.ServiceModel.FaultExceptions` are thrown and how to properly handle them, so let's get started!

## The Technical Rundown

- All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.
- [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception) is inherited from the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) class.
- [`System.ServiceModel.CommunicationException`](https://docs.microsoft.com/en-us/dotnet/api/system.servicemodel.communicationexception?view=netframework-4.7) inherits directly from [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception).
- Finally, `System.ServiceModel.FaultException` inherits from [`System.ServiceModel.CommunicationException`](https://docs.microsoft.com/en-us/dotnet/api/system.servicemodel.communicationexception?view=netframework-4.7).

## When Should You Use It?

Working with WCF services can be a little confusing compared to "normal", localized code, so we'll start out with the full working code example and then walk through it afterward:

```cs
using System.Runtime.Serialization;
using System.ServiceModel;

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
        [OperationContract]
        [FaultContract(typeof(InvalidBookFault))]
        bool ReserveBook(string title, string author);
    }
}

using System;
using System.ServiceModel;
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
                    throw new System.ArgumentException($"CheckoutBook() parameter 'title' must be a valid string.");
                }
                // Check if author value is null or has no characters.
                // Null must be checked prior to length to avoid checking an invalid object.
                if (author is null || author.Length == 0)
                {
                    throw new System.ArgumentException($"CheckoutBook() parameter 'author' must be a valid string.");
                }

                // Create book record and reserve it (in a real service
                // a database record would likely be modified.
                Book book = new Book(title, author);
                book.Reserved = true;
                Logging.Log("RESERVATION SUCCESSFUL");
                Logging.LineSeparator();
                Logging.Log(book);
                return true;
            }
            catch (ArgumentException e)
            {
                // Log the exception.
                Logging.Log(e);

                // Generate new fault and set details.
                var fault = new InvalidBookFault();
                fault.Description = e.Message;
                fault.Message = e.Message;
                fault.Result = false;

                // Throw newly created FaultException of appropriate type.
                throw new FaultException<InvalidBookFault>(fault, new FaultReason(e.Message));
            }
            catch (Exception e)
            {
                Logging.Log(e, false);
            }
            return false;
        }
    }
}

using System;
using System.ServiceModel;
using Utility;

namespace LibraryClient
{
    class Program
    {
        static void Main(string[] args)
        {           
            LibraryServiceReference.LibraryServiceClient client = new LibraryServiceReference.LibraryServiceClient(); ;

            try
            {
                // Open client connection.
                client.Open();

                // Reserve a valid book.
                client.ReserveBook("The Stand", "Stephen King");
                // Reserve an invalid book.
                client.ReserveBook("The Stand", null);

                // Close client connection.
                client.Close();
            }
            // Catch our SOAP fault type. 
            catch (FaultException<LibraryServiceReference.InvalidBookFault> e)
            {
                Logging.Log(e);
                client.Abort();
            }
            catch (FaultException e)
            {
                Logging.Log(e, false);
                client.Abort();
            }
            catch (Exception e)
            {
                Logging.Log(e, false);
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

Ignoring our `Utility` functions for logging, our service application example consists of three basic components: An `ILibraryService` interface, a `LibraryService` class, and then a `LibraryClient` that uses the service to make something happen.  Therefore we start with a basic (albeit very simple) service interface:

```cs
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
        [OperationContract]
        [FaultContract(typeof(InvalidBookFault))]
        bool ReserveBook(string title, string author);
    }
}
```

Here we're using a lot of `[attributes]` to specify the expected behavior of our service interface.  We really just have one invokable service method called `ReserveBook`, which finds a book by `title` and `author` before trying to reserve it.  What's particularly important here is the `InvalidBookFault` class, which we use to communicate the potential error from the `LibraryService` to our client.  This allows the client to be informed of what specifically went wrong.

Next is our `LibraryService` class which implements the `ILibraryService` interface.  To make things a bit more interesting we've specified that the `ReserveBook()` method should be provided valid `title` and `author` argument strings, otherwise it'll throw an `ArgumentException` error.  However, this is still all "server-side" so we need to `catch` the produced `ArgumentException` and generate a contractual `fault` that can be relayed to the invoking client.  This is where our previously-defined `InvalidBookFault` comes into play.  Within the `ArgumentException` catch we create a new `InvalidBookFault` instance, populate it with values from our exception (such as the basic error message), then `throw` a new `FaultException<InvalidBookFault>`:

```cs
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
                    throw new System.ArgumentException($"CheckoutBook() parameter 'title' must be a valid string.");
                }
                // Check if author value is null or has no characters.
                // Null must be checked prior to length to avoid checking an invalid object.
                if (author is null || author.Length == 0)
                {
                    throw new System.ArgumentException($"CheckoutBook() parameter 'author' must be a valid string.");
                }

                // Create book record and reserve it (in a real service
                // a database record would likely be modified.
                Book book = new Book(title, author);
                book.Reserved = true;
                Logging.Log("RESERVATION SUCCESSFUL");
                Logging.LineSeparator();
                Logging.Log(book);
                return true;
            }
            catch (ArgumentException e)
            {
                // Log the exception.
                Logging.Log(e);

                // Generate new fault and set details.
                var fault = new InvalidBookFault();
                fault.Description = e.Message;
                fault.Message = e.Message;
                fault.Result = false;

                // Throw newly created FaultException of appropriate type.
                throw new FaultException<InvalidBookFault>(fault, new FaultReason(e.Message));
            }
            catch (Exception e)
            {
                Logging.Log(e, false);
            }
            return false;
        }
    }
}
```

Finally our simple `LibraryClient` application actually invokes our `LibraryService` (via a service reference in Visual Studio) to acquire a client connection.  From this we can open the connection and then call the `ReserveBook()` service method:

```cs
namespace LibraryClient
{
    class Program
    {
        static void Main(string[] args)
        {           
            LibraryServiceReference.LibraryServiceClient client = new LibraryServiceReference.LibraryServiceClient(); ;

            try
            {
                // Open client connection.
                client.Open();

                // Reserve a valid book.
                client.ReserveBook("The Stand", "Stephen King");
                // Reserve an invalid book.
                client.ReserveBook("The Stand", null);

                // Close client connection.
                client.Close();
            }
            // Catch our SOAP fault type. 
            catch (FaultException<LibraryServiceReference.InvalidBookFault> e)
            {
                Logging.Log(e);
                client.Abort();
            }
            catch (FaultException e)
            {
                Logging.Log(e, false);
                client.Abort();
            }
            catch (Exception e)
            {
                Logging.Log(e, false);
                client.Abort();
            }
        }
    }
}
```

We begin with a valid set of `title` and `author` arguments for our first `ReserveBook()` call, so the result is as expected and our service "reserves" our book for us as show in the output:

```
RESERVATION SUCCESSFUL
--------------------
{LibraryService.Book(HashCode:37916227)}
  Title: "The Stand"
  Author: "Stephen King"
  Reserved: True
```

However, the second `ReserveBook()` call includes a `null` value for the second argument.  As you'll recall this isn't acceptable and triggers our chain of exception and fault events: The `LibraryService` `throws` a new `ArgumentException`, which is caught and then creates and throws a new `InvalidBookFault`.  Our `LibraryClient` then catches the `FaultException<LibraryServiceReference.ArgumentExceptionFault>` and can safely report that issue to the end-user/client, as we see in the output:

```
[EXPECTED] System.ServiceModel.FaultException`1[LibraryClient.LibraryServiceReference.InvalidBookFault]: CheckoutBook() parameter 'author' must be a valid string. (Fault Detail is equal to LibraryClient.LibraryServiceReference.InvalidBookFault).: CheckoutBook() parameter 'author' must be a valid string.
```

We've successfully created a `System.ServiceModel.FaultException` of type `LibraryClient.LibraryServiceReference.InvalidBookFault` and it's populated by relevant error information that came from our service.  This powerful technique allows clients and services to securely communicate exception information without breaking any contracts.

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A deep dive into the System.ServiceModel.FaultException in .NET, including a working C# code samples illustrating how to handle service exceptions.