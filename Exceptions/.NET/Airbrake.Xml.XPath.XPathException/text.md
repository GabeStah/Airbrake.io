# .NET Exceptions - System.Xml.XPath.XPathException

Continuing our journey through the in-depth [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, today we'll go over the System.Xml.XPath.XPathException.  A `XPathException` is thrown anytime an error occurs while performing basic XPath parsing, using classes such as [`XPathNavigator`](https://docs.microsoft.com/en-us/dotnet/api/system.xml.xpath.xpathnavigator?view=netframework-4.7) and the like.

Throughout this article we'll explore the `XPathException` in more detail, starting with where it sits in the .NET exception hierarchy.  We'll also look over some functional C# code samples that will show how we might typically try to parse an XML file using XPath expressions, and what might, therefore, cause a `XPathException` to occur, so let's get going!

## The Technical Rundown

All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.  The full exception hierarchy of this error is:

- [`System.Object`](https://docs.microsoft.com/en-us/dotnet/api/system.object)
    - [`System.Exception`](https://docs.microsoft.com/en-us/dotnet/api/system.exception)
        - [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception)
            - `XPathException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```xml
<?xml version="1.0" encoding="utf-8" ?>
<!-- books.xml -->
<Airbrake:Books xmlns:xs="http://www.w3.org/2001/XMLSchema-instance" xmlns:Airbrake="https://airbrake.io">
  <Book>
    <Title>The Stand</Title>
    <Author>Stephen King</Author>
    <PageCount>1153</PageCount>
  </Book>
  <Book>
    <Title>The Slow Regard of Silent Things</Title>
    <Author>Patrick Rothfuss</Author>
    <PageCount>159</PageCount>
  </Book>
  <Book>
    <Title>Magician</Title>
    <Author>Raymond E. Feist</Author>
    <PageCount>681</PageCount>
  </Book>
</Airbrake:Books>
```

```cs
using System;
using System.Xml;
using System.Xml.XPath;
using Utility;

namespace Airbrake.Xml.XPath.XPathException
{
    class Program
    {
        private static void Main(string[] args)
        {
            var xPath = @"//Airbrake:Books/Book/Title/text()";
            Logging.LineSeparator(xPath, 60);
            GetXPathNodesFromXml(@"books.xml", xPath);

            xPath = @"//Airbrake:Books/Book/Title/text()";
            Logging.LineSeparator($"{xPath} w/ Namespace", 60);
            GetXPathNodesFromXml(@"books.xml", xPath, "Airbrake", "https://airbrake.io");

            xPath = @"//Airbrake:Books/Book/*";
            Logging.LineSeparator($"{xPath} w/ Namespace", 60);
            GetXPathNodesFromXml(@"books.xml", xPath, "Airbrake", "https://airbrake.io");
        }

        /// <summary>
        /// Outputs the node values using XPath, from passed XML file.
        /// </summary>
        /// <param name="file">XML file path.</param>
        /// <param name="xPath">XPath expression.</param>
        /// <param name="manager">Namespace manager used to select namespaced elements, if applicable.</param>
        internal static void GetXPathNodesFromXml(string file, string xPath, XmlNamespaceManager manager = null)
        {
            try
            {
                // Create a new XPath document from XML file.
                var document = new XPathDocument(file);
                // Create navigator and select nodes using passed xPath expression and manager.
                var navigator = document.CreateNavigator();
                var nodes = navigator.Select(xPath, manager);
                // Iterator through all nodes.
                while (nodes.MoveNext())
                {
                    // Output current node value.
                    Logging.Log(nodes.Current.Value);
                }
            }
            catch (System.Xml.XPath.XPathException exception)
            {
                // Output expected XPathExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Output unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }

        /// <summary>
        /// Outputs the node values using XPath, from passed XML file.
        /// 
        /// Allows specification of XML namespace.
        /// </summary>
        /// <param name="file">XML file path.</param>
        /// <param name="xPath">XPath expression.</param>
        /// <param name="namespace">Namespace to look within.</param>
        /// <param name="namespaceUrl">Namespace URL.</param>
        internal static void GetXPathNodesFromXml(string file, string xPath, string @namespace, string namespaceUrl)
        {
            // Create new namespace manager with empty name table.
            var namespaceManager = new XmlNamespaceManager(new NameTable());
            // Add namespace to manager.
            namespaceManager.AddNamespace(@namespace, namespaceUrl);

            // Forward execution to base method signature.
            GetXPathNodesFromXml(file, xPath, namespaceManager);
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

To see how a `XPathException` might be thrown, we first need to briefly review what [`XML Path Language`](https://en.wikipedia.org/wiki/XPath) (or `XPath`) is and how it works.  In the simplest sense, XPath is an query language syntax used to select nodes in hierarchical XML documents.  Since XML is an elemental, hierarchical format, XPath represents the relationships between nodes (or elements) using forward slashes (`/`), along with a number of special characters.  thus, consider an XML document that looks like this:

```xml
<Base>
    <First>
        <Second>Value</Second>
    </First>
</Base>
```

With XPath, we could select nodes simply by indicating their `element names`, separated by a forward slash.  Thus, the inner-most element could be selected with XPath of: `//Base/First/Second`

There's far more to learn about XPath, so if you're interested, the [`MDN Documentation`](https://developer.mozilla.org/en-US/docs/Web/XPath) is a solid place to start.

For our sample code today we'll be trying to select nodes from a simple `books.xml` file:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<!-- books.xml -->
<Airbrake:Books xmlns:xs="http://www.w3.org/2001/XMLSchema-instance" xmlns:Airbrake="https://airbrake.io">
  <Book>
    <Title>The Stand</Title>
    <Author>Stephen King</Author>
    <PageCount>1153</PageCount>
  </Book>
  <Book>
    <Title>The Slow Regard of Silent Things</Title>
    <Author>Patrick Rothfuss</Author>
    <PageCount>159</PageCount>
  </Book>
  <Book>
    <Title>Magician</Title>
    <Author>Raymond E. Feist</Author>
    <PageCount>681</PageCount>
  </Book>
</Airbrake:Books>
```

Most of our code is found in the `GetXPathNodesFromXml(string file, string xPath, XmlNamespaceManager manager = null)` method:

```cs
/// <summary>
/// Outputs the node values using XPath, from passed XML file.
/// </summary>
/// <param name="file">XML file path.</param>
/// <param name="xPath">XPath expression.</param>
/// <param name="manager">Namespace manager used to select namespaced elements, if applicable.</param>
internal static void GetXPathNodesFromXml(string file, string xPath, XmlNamespaceManager manager = null)
{
    try
    {
        // Create a new XPath document from XML file.
        var document = new XPathDocument(file);
        // Create navigator and select nodes using passed xPath expression and manager.
        var navigator = document.CreateNavigator();
        var nodes = navigator.Select(xPath, manager);
        // Iterator through all nodes.
        while (nodes.MoveNext())
        {
            // Output current node value.
            Logging.Log(nodes.Current.Value);
        }
    }
    catch (System.Xml.XPath.XPathException exception)
    {
        // Output expected XPathExceptions.
        Logging.Log(exception);
    }
    catch (Exception exception)
    {
        // Output unexpected Exceptions.
        Logging.Log(exception, false);
    }
}
```

All this method really does is creates an `XPathDocument` for the passed XML file, then we use a new navigator to select the node(s) using the passed `xPath` string and (optional) `XmlNamespaceManager`.  All selected nodes are output to the log.

To test this out with our `books.xml` document we've defined a relatively basic XPath expression that selects the `Title` element text for each unique `Book` element:

```cs
var xPath = @"//Airbrake:Books/Book/Title/text()";
Logging.LineSeparator(xPath, 60);
GetXPathNodesFromXml(@"books.xml", xPath);
```

Everything looks good, but executing this code immediately outputs a problem, in the form of a thrown `XPathException`:

```
------------ //Airbrake:Books/Book/Title/text() ------------
[EXPECTED] System.Xml.XPath.XPathException: Namespace Manager or XsltContext needed. This query has a prefix, variable, or user-defined function.
```

Experienced XPathers may have noticed the issue before we even tried to run this code, but the problem here is that our XML structure uses an [`XML namespace`](https://msdn.microsoft.com/en-us/library/aa468565.aspx) of `Airbrake`, which precedes the parent `Books` element.  Since the `XmlNamespaceManager manager = null` parameter of our method defaults to null, we didn't pass a manager to the `navigator.Select(...)` method call, which is required in this situation where our XML document uses a namespace.

To resolve this, we've added a signature override that accepts namespace information:

```cs
/// <summary>
/// Outputs the node values using XPath, from passed XML file.
/// 
/// Allows specification of XML namespace.
/// </summary>
/// <param name="file">XML file path.</param>
/// <param name="xPath">XPath expression.</param>
/// <param name="namespace">Namespace to look within.</param>
/// <param name="namespaceUrl">Namespace URL.</param>
internal static void GetXPathNodesFromXml(string file, string xPath, string @namespace, string namespaceUrl)
{
    // Create new namespace manager with empty name table.
    var namespaceManager = new XmlNamespaceManager(new NameTable());
    // Add namespace to manager.
    namespaceManager.AddNamespace(@namespace, namespaceUrl);

    // Forward execution to base method signature.
    GetXPathNodesFromXml(file, xPath, namespaceManager);
}
```

As you can see, all we do here is add a new namespace to the `XmlNamespaceManager` instance, based on the passed `@namespace` name and `namespaceUrl` values, then we forward the rest of the execution onto our previous, baseline `GetXPathNodesFromXml(string file, string xPath, XmlNamespaceManager manager = null)` method signature.  We can test this new method signature with the following code:

```cs
xPath = @"//Airbrake:Books/Book/Title/text()";
Logging.LineSeparator($"{xPath} w/ Namespace", 60);
GetXPathNodesFromXml(@"books.xml", xPath, "Airbrake", "https://airbrake.io");
```

Now that we've added the required namespace, everything works as expected, selecting the `Title` nodes for each `Book` element and outputting the values:

```
----- //Airbrake:Books/Book/Title/text() w/ Namespace ------
The Stand
The Slow Regard of Silent Things
Magician
```

Finally, just for fun, we can slightly modify our XPath expression, using the special asterisk (`*`) character, to select _all_ child elements within each `Book` element:

```cs
xPath = @"//Airbrake:Books/Book/*";
Logging.LineSeparator($"{xPath} w/ Namespace", 60);
GetXPathNodesFromXml(@"books.xml", xPath, "Airbrake", "https://airbrake.io");
```

The output shows that we're able to get the value of each book's child element collection, without needing to know the actual names of said child elements:

```
----------- //Airbrake:Books/Book/* w/ Namespace -----------
The Stand
Stephen King
1153
The Slow Regard of Silent Things
Patrick Rothfuss
159
Magician
Raymond E. Feist
681
```

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A deep dive into the System.Xml.XPath.XPathException in .NET, including C# code illustrating how to parse XML using XPath expressions.