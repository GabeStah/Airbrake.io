# .NET Exceptions - System.Web.HttpParseException

Making our way through our detailed [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, today we'll be taking a closer look at the **System.Web.HttpParseException**.  An `HttpParseException` should be thrown when something goes wrong while attempting to parse an HTML page.

In this article we'll examine the `HttpParseException` by looking at where it resides in the overall .NET exception hierarchy.  We'll also go over a simple code sample that illustrates how one might use `HttpParseExceptions` in their own code, so let's get to it!

## The Technical Rundown

All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.  The full exception hierarchy of this error is:

- [`System.Object`](https://docs.microsoft.com/en-us/dotnet/api/system.object)
    - [`System.Exception`](https://docs.microsoft.com/en-us/dotnet/api/system.exception)
        - [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception)
            - [`System.Runtime.InteropServices.ExternalException`](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.externalexception?view=netframework-4.7)
                - [`System.Web.HttpException`](https://docs.microsoft.com/en-us/dotnet/api/system.web.httpexception?view=netframework-4.7)
                    - `System.Web.HttpParseException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```cs
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Utility;

namespace Airbrake.Web.HttpParseException
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Instantiate a new select builder.
            var selectBuilder = new MySelectBuilder
            {
                ID = "names"
            };

            // Attempt to get builder control with MyCustomOption suffix.
            Logging.LineSeparator("TESTING MyCustomOption SUFFIX");
            GetChildControlOfControlBuilder(selectBuilder, $"Select-{MyCustomOption.Suffix}");

            // Attempt to get builder control with Invalid suffix.
            Logging.LineSeparator("TESTING Invalid SUFFIX");
            GetChildControlOfControlBuilder(selectBuilder, $"Select-Invalid");
        }

        private static void GetChildControlOfControlBuilder(ControlBuilder builder, string tagName)
        {
            try
            {
                // Get the child control type of passed 
                // builder using passed tag name.
                var type = builder.GetChildControlType(
                    tagName, 
                    new Dictionary<string, string>()
                );
                Logging.Log($"Child Control Type of {builder.GetType().Name} (ID: {builder.ID}) is {type}");
            }
            catch (System.Web.HttpParseException exception)
            {
                // Output expected HttpParseExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Output unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }
    }

    /// <summary>
    /// A basic custom Select Option.
    /// </summary>
    public class MyCustomOption
    {
        public const string Suffix = "MyCustomOption";
        public string Id { get; set; }
        public string Value { get; set; }
    }

    /// <summary>
    /// Custom HtmlSelectBuilder, which only allows MyCustomOption child controls.
    /// </summary>
    public class MySelectBuilder : HtmlSelectBuilder
    {
        [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
        public override Type GetChildControlType(string tagName, IDictionary attributes)
        {
            // Verify that child control tag ends with MyCustomOption suffix.
            if (tagName != null && tagName.EndsWith(MyCustomOption.Suffix))
            {
                // Return MyCustomOption type.
                return typeof(MyCustomOption);
            }
            // If a different tagName is passed, throw HttpParseException.
            throw new System.Web.HttpParseException($"Unable to get child control type.  '{GetType()}' control requires child element type of '{MyCustomOption.Suffix}.'");
        }

    }

    /// <summary>
    /// Custom HtmlSelect that uses MySelectBuilder.
    /// </summary>
    [ControlBuilder(typeof(MySelectBuilder))]
    public class CustomHtmlSelect : HtmlSelect
    {
        // Override the AddParsedSubObject method.
        protected override void AddParsedSubObject(object obj)
        {
            // Create new custom option.
            var option = obj as MyCustomOption;
            // Ensure option is not null.
            if (option == null) return;
            // Create select option text.
            var text = $"{option.Id} : {option.Value}";
            // Add option to Items list.
            var listItem = new ListItem(text, option.Value);
            Items.Add(listItem);
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

The overall inner workings of ASP.NET pages are fairly complex, but to properly understand what the presence of an `HttpParseException` means we'll just go over the simple basics of page creation.  Specifically, we need a basic understanding of how .NET converts the XML-like content of ASP.NET pages (`Web Forms`) into executable .NET code.

Firstly, ASP.NET is a [`dynamically compiled`](https://msdn.microsoft.com/en-us/library/ms366723.aspx) language.  Thus, when the first client request comes into the application all ASP.NET pages (`.aspx`), services (`.asmx`), handlers (`.ashx`), and global app files (`.asax`) are parsed by a [`PageParser`](https://docs.microsoft.com/en-us/dotnet/api/system.web.ui.pageparser?view=netframework-4.7).  This is effectively an advanced text parser that recognizes the XML markup of ASP.NET pages.  When a valid tag is recognized and parsed, that element is `compiled` into a normal .NET class instance, just like instances created in "normal" code.  The parser and compiler work together to create the proper relationships between all controls and elements in the page, as well as across the entire application.

In short, this parsing and compilation process is where an `HttpParseException` can be thrown, when appropriate, to indicate that something about the parsing of the ASP.NET page has gone awry.  A normal ASP.NET application is rather complex and difficult to fully include in text.  Thus, our example code for this article shows the basic structure of creating a custom control and throwing an `HttpParseException` when necessary, without including all the fluff that would normally be in a full ASP.NET application.

We're showing how we can force a custom `HtmlSelect` control to only contain our own custom dropdown option controls as child elements.  Thus, we start with the `MyCustomOption` class, which is the option child control we want to add:

```cs
/// <summary>
/// A basic custom Select Option.
/// </summary>
public class MyCustomOption
{
    public const string Suffix = "MyCustomOption";
    public string Id { get; set; }
    public string Value { get; set; }
}
```

Now, the `MySelectBuilder` class inherits from the standard `HtmlSelectBuilder`, and will be used by our custom `HtmlSelect` control to check if child controls are of the appropriate type:

```cs
/// <summary>
/// Custom HtmlSelectBuilder, which only allows MyCustomOption child controls.
/// </summary>
public class MySelectBuilder : HtmlSelectBuilder
{
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    public override Type GetChildControlType(string tagName, IDictionary attributes)
    {
        // Verify that child control tag ends with MyCustomOption suffix.
        if (tagName != null && tagName.EndsWith(MyCustomOption.Suffix))
        {
            // Return MyCustomOption type.
            return typeof(MyCustomOption);
        }
        // If a different tagName is passed, throw HttpParseException.
        throw new System.Web.HttpParseException($"Unable to get child control type.  '{GetType()}' control requires child element type of '{MyCustomOption.Suffix}.'");
    }
}
```

The `GetChildControlType(string tagName, IDictionary attributes)` method is overridden from the base `HtmlSelectBuilder` class and performs a basic logical check that child controls are of the `MyCustomOption` type.  If not, a new `HttpParseException` is thrown, indicating that we only allow such child controls.

Finally, the actual `CustomHtmlSelect` class, which inherits from the standard `HtmlSelect` class and overrides just one method, `AddParsedSubObject(object obj)`:

```cs
/// <summary>
/// Custom HtmlSelect that uses MySelectBuilder.
/// </summary>
[ControlBuilder(typeof(MySelectBuilder))]
public class CustomHtmlSelect : HtmlSelect
{
    // Override the AddParsedSubObject method.
    protected override void AddParsedSubObject(object obj)
    {
        // Create new custom option.
        var option = obj as MyCustomOption;
        // Ensure option is not null.
        if (option == null) return;
        // Create select option text.
        var text = $"{option.Id} : {option.Value}";
        // Add option to Items list.
        var listItem = new ListItem(text, option.Value);
        Items.Add(listItem);
    }
}
```

As the method name suggests, `AddParsedSubObject(object obj)` adds the parsed `obj` parameter control to the `CustomHtmlSelect` control.  We're explicitly casting from the passed `obj` object to `MyCustomOption`, which results in a `null` of the cast fails (hence, the `nullibility` check afterward).  If successful, we know the parsed child object is of the proper `MyCustomOption` type, so we create some text and add the item to the `Items` list property.

With everything setup, we can now test things out.  As mentioned, we're foregoing the inclusion into a full ASP.NET application by just directly creating a builder and calling the `GetChildControlType(string tagName, IDictionary attributes)` method:

```cs
internal class Program
{
    private static void Main(string[] args)
    {
        // Instantiate a new select builder.
        var selectBuilder = new MySelectBuilder
        {
            ID = "names"
        };

        // Attempt to get builder control with MyCustomOption suffix.
        Logging.LineSeparator("TESTING MyCustomOption SUFFIX");
        GetChildControlOfControlBuilder(selectBuilder, $"Select-{MyCustomOption.Suffix}");

        // Attempt to get builder control with Invalid suffix.
        Logging.LineSeparator("TESTING Invalid SUFFIX");
        GetChildControlOfControlBuilder(selectBuilder, $"Select-Invalid");
    }

    private static void GetChildControlOfControlBuilder(ControlBuilder builder, string tagName)
    {
        try
        {
            // Get the child control type of passed 
            // builder using passed tag name.
            var type = builder.GetChildControlType(
                tagName, 
                new Dictionary<string, string>()
            );
            Logging.Log($"Child Control Type of {builder.GetType().Name} (ID: {builder.ID}) is {type}");
        }
        catch (System.Web.HttpParseException exception)
        {
            // Output expected HttpParseExceptions.
            Logging.Log(exception);
        }
        catch (Exception exception)
        {
            // Output unexpected Exceptions.
            Logging.Log(exception, false);
        }
    }
}
```

The `GetChildControlOfControlBuilder(ControlBuilder builder, string tagName)` method attempts to get the child control of the passed builder.  If successful, it outputs some basic information about the builder and the type that was found as a child control.  Otherwise, it catches expected or unexpected `Exceptions`.

We test this out by calling `GetChildControlOfControlBuilder(ControlBuilder builder, string tagName)` twice using a new `MySelectBuilder` instance.  First, we check for child controls with a `MyCustomOption.Suffix` suffix, which works as expected and produces the following output:

```
---- TESTING MyCustomOption SUFFIX -----
Child Control Type of MySelectBuilder (ID: names) is Airbrake.Web.HttpParseException.MyCustomOption
```

However, our second attempt to get child controls with the `Invalid` suffix throws the expected `HttpParseException`, informing us that such child controls are invalid:

```
-------- TESTING Invalid SUFFIX --------
[EXPECTED] System.Web.HttpParseException (0x80004005): Unable to get child control type.  'Airbrake.Web.HttpParseException.MySelectBuilder' control requires child element type of 'MyCustomOption.'
```

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A close look at the System.Web.HttpParseException in .NET, including a basic explanation of ASP.NET parsing with a functional C# code sample.