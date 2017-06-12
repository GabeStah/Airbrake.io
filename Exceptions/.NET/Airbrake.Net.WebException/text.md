# .NET Exceptions - System.Net.WebException

Moving along through our [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series today we'll take a closer look at the `System.Net.WebException`.  While .NET provides _numerous_ exceptions that are related to connectivity and web issues, the `System.Net.WebException` is one of the most generic (and thus most common) errors you'll see in your own adventures.

Throughout this article we'll dive into the `System.Net.WebException` in more detail and see where it sits within the .NET exception hierarchy.  We'll also look at a fully-functional C# code example to help illustrate how `System.Net.WebExceptions` might occur in day-to-day coding, so let's get started!

## The Technical Rundown

- All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.
- [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception) is inherited from the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) class.
- [`System.InvalidOperationException`](https://docs.microsoft.com/en-us/dotnet/api/system.invalidoperationexception?view=netframework-4.7) inherits directly from [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception).
- Finally, `System.Net.WebException` inherits directly from [`System.InvalidOperationException`](https://docs.microsoft.com/en-us/dotnet/api/system.invalidoperationexception?view=netframework-4.7).

## When Should You Use It?

Since .NET tends to feature far more complex (and specific) exception classes than simpler languages like Ruby or JavaScript, we usual with our `System.Net.WebException` exploration we'll just start off by looking at the full example source code first and then we'll walk through the major sections to see what's going on:

```cs
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using Utility;

namespace Airbrake.Net.WebException
{
    class Program
    {
        static void Main(string[] args)
        {
            // Connect to Airbrake.io.
            ParseTest("https://airbrake.io");
            Logging.LineSeparator();
            // Try connecting to localhost:24601.
            ParseTest("http://localhost:24601");
        }

        static void ParseTest(string uri)
        {
            try
            {
                // Generate document using AngleSharp.
                var document = WebParser.GetHtmlDocument(uri);
                // Get all h1 and h2 headers from document, concat into single 
                // collection, and then get TextContent and insert into array.
                var headers = document.QuerySelectorAll("h1")
                                      .Concat(document.QuerySelectorAll("h2"))
                                      .Select(element => element.TextContent)
                                      .ToArray();
                // Output title and headers to test if connection was successful.
                Logging.Log($"Title: {document.Title}");
                Logging.Log("Headers:");
                Logging.Log(headers);
            }
            catch (System.Net.WebException exception)
            {
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                Logging.Log(exception, false);
            }
        }
    }

    public static class WebParser
    {
        public static IHtmlDocument GetHtmlDocument(string uri)
        {
            HttpContent content;
            HttpClient client = new HttpClient();
            HttpResponseMessage response;
            HtmlParser parser = new HtmlParser();

            try
            {
                // Get async result from uri.
                response = client.GetAsync(uri).Result;
                // Get response content.
                content = response.Content;
                // Read result string (HTML).
                var result = content.ReadAsStringAsync().Result;
                // Parse HTML and return produced IHtmlDocument.
                return parser.Parse(result);
            }
            catch (Exception exception) when (exception is System.Net.WebException ||
                                              exception is HttpRequestException ||
                                              exception is SocketException)
            {
                Logging.Log(exception);
            }
            catch (Exception exception) when (exception is AggregateException)
            {
                // Gather InnerExceptions into a collection then filter out only WebException matches.
                foreach (var e in exception.FromHierarchy(e => e.InnerException)
                                           .Where(e => e.GetType() == typeof(System.Net.WebException)))
                {
                    // Log any expected and matched exceptions.
                    Logging.Log(e);
                }
            }

            return null;
        }
    }
}
```

We're also using a few `Utility` namespace classes which we won't explain here but feel free to check out the inline documentation for more info.  Note that we're also making use of the powerful [AngleSharp](https://github.com/AngleSharp/AngleSharp) HTML parser library, so be sure to add that reference to your project if you're trying this yourself.

```cs
using System;
using System.Collections.Generic;

namespace Utility
{
    /// <summary>
    /// All collection and enumerable methods.
    /// </summary>
    public static class Collections
    {
        /// <summary>
        /// Convert Enumerable in hierarchy format to Enumerable collection.
        /// </summary>
        /// <typeparam name="TSource">Originating source collection type.</typeparam>
        /// <param name="source">Originating source collection type.</param>
        /// <param name="nextItem">Function to retrieve next item in collection.</param>
        /// <param name="canContinue">Boolean function indicating if next item exists.</param>
        /// <returns>The collection from a hierarchical format.</returns>
        public static IEnumerable<TSource> FromHierarchy<TSource>(
            this TSource source,
            Func<TSource, TSource> nextItem,
            Func<TSource, bool> canContinue)
        {
            for (var current = source; canContinue(current); current = nextItem(current))
            {
                yield return current;
            }
        }

        /// <summary>
        /// Recursively enumerates over hierarchy to get collection.
        /// </summary>
        /// <typeparam name="TSource">Originating source collection type.</typeparam>
        /// <param name="source">Originating source collection type.</param>
        /// <param name="nextItem">Function to retrieve next item in collection.</param>
        /// <returns>Single yielded enumerable object.</returns>
        public static IEnumerable<TSource> FromHierarchy<TSource>(
            this TSource source,
            Func<TSource, TSource> nextItem)
            where TSource : class
        {
            return FromHierarchy(source, nextItem, s => s != null);
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
        /// if DEBUG mode is enabled, oherwise uses standard <see cref="Console.WriteLine"/>.
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

Since `System.Net.WebExceptions` are rather generic the basic purpose of our example script here is to send a [`WebRequest`](https://docs.microsoft.com/en-us/dotnet/api/system.net.webrequest?view=netframework-4.7) to a specific URI of our choice.  If we're able to successfully connect we then want to grab the HTML of the page in question using a [`WebResponse`](https://docs.microsoft.com/en-us/dotnet/api/system.net.webresponse?view=netframework-4.7) object.  To simplify things so we don't have to write everything from scratch we're also using the `AngleSharp` parsing library to handle most of this work behind the scenes.

Therefore, all we really need to start is our `WebParser` class and the `GetHtmlDocument()` method that uses `AngleSharp` to try to connect to the provided `uri` string, get a response result, read the result as a string (which is the HTML of the page in this case), then parse that result to convert it to an `IHtmlDocument` which we'll use later to extract some data from the page.  The `catch` blocks in the `GetHtmlDocument()` method may seem a bit convoluted, but it ensures that we can capture any potential `System.Net.WebExceptions` that might be thrown.

```cs
public static class WebParser
{
    public static IHtmlDocument GetHtmlDocument(string uri)
    {
        HttpContent content;
        HttpClient client = new HttpClient();
        HttpResponseMessage response;
        HtmlParser parser = new HtmlParser();

        try
        {
            // Get async result from uri.
            response = client.GetAsync(uri).Result;
            // Get response content.
            content = response.Content;
            // Read result string (HTML).
            var result = content.ReadAsStringAsync().Result;
            // Parse HTML and return produced IHtmlDocument.
            return parser.Parse(result);
        }
        catch (Exception exception) when (exception is System.Net.WebException ||
                                          exception is HttpRequestException ||
                                          exception is SocketException)
        {
            Logging.Log(exception);
        }
        catch (Exception exception) when (exception is AggregateException)
        {
            // Gather InnerExceptions into a collection then filter out only WebException matches.
            foreach (var e in exception.FromHierarchy(e => e.InnerException)
                                       .Where(e => e.GetType() == typeof(System.Net.WebException)))
            {
                // Log any expected and matched exceptions.
                Logging.Log(e);
            }
        }

        return null;
    }
}
```

Now we have a simple `ParseTest()` method that we use to perform our business logic.  In this case we're creating an `IHtmlDocument` object from the `uri`, querying the resulting HTML to get all major header elements (`h1` and `h2`), extracting the text content of those headers, and then splitting the headers up into an array.  This just serves as a simple example of creating a remote web connection and parsing the HTML for some info we want.  The last step is to output the title of the page and the headers we collected to our log:

```cs
static void ParseTest(string uri)
{
    try
    {
        // Generate document using AngleSharp.
        var document = WebParser.GetHtmlDocument(uri);
        // Get all h1 and h2 headers from document, concat into single 
        // collection, and then get TextContent and insert into array.
        var headers = document.QuerySelectorAll("h1")
                              .Concat(document.QuerySelectorAll("h2"))
                              .Select(element => element.TextContent)
                              .ToArray();
        // Output title and headers to test if connection was successful.
        Logging.Log($"Title: {document.Title}");
        Logging.Log("Headers:");
        Logging.Log(headers);
    }
    catch (System.Net.WebException exception)
    {
        Logging.Log(exception);
    }
    catch (Exception exception)
    {
        Logging.Log(exception, false);
    }
}
```

To see this in action we have two different `URIs` we want to try in our `ParseTest()` method:

```cs
static void Main(string[] args)
{
    // Connect to Airbrake.io.
    ParseTest("https://airbrake.io");
    Logging.LineSeparator();
    // Try connecting to localhost:24601.
    ParseTest("http://localhost:24601");
}
```

For our first test we're trying to connect to the `Airbrake.io` homepage and, as you might suspect, everything goes as planned.  We were able to connect and gather the title of the page and the main header values, which is indicated by the log output of our script:

```
Title: Error Monitoring and Detection Software | Airbrake
Headers:
"No more searching log files"
"Capture and track your application's exceptions in 3 minutes"
"All the tools you need to find and fix errors - fast!"
"The world’s best engineering teams use Airbrake"
```

Sure enough if we manually open the [`Airbrake.io` homepage](https://airbrake.io/) in a browser window we can confirm that the title and main headers all match what our application collected -- cool!

However, our second `ParseTest()` call attempts to connect to a localhost port that my development machine doesn't have open, so we invariably produce a number of errors.  Most of those exceptions are actually contained within a [`System.AggregateException`](https://docs.microsoft.com/en-us/dotnet/api/system.aggregateexception?view=netframework-4.7) instance, which is why we needed the complicated method of exception extraction to see what actual exception object types we're getting by digging into the `InnerException` property.

Therefore, as it happens -- in addition to a few other exception types -- we also end up `catching` a `System.Net.WebException`, as seen in the log output:

```
[EXPECTED] System.Net.WebException: Unable to connect to the remote server ---> System.Net.Sockets.SocketException: No connection could be made because the target machine actively refused it 127.0.0.1:24601
```

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A deep dive into the System.Net.WebException of .NET, including a basic C# code example illustrating how to parse the HTML of a URI.