# .NET Exceptions - System.FormatException

Moving along through our in-depth [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, today we come across the System.FormatException.  As the name implies, the `System.FormatException` is thrown in a wide variety of scenarios, yet they all revolve around providing an improperly formatted argument to a vast array of methods and API calls.

We'll spend some time in this article exploring the `System.FormatException` in more detail, such as where it resides in the .NET exception hierarchy.  We'll also take a look at a few functional C# code samples that will illustrate how the `System.FormatException` might be thrown in some everyday code, so you can better plan for handling them yourself.  Let's get going!

## The Technical Rundown

- All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.
- [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception) is inherited from the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) class.
- `System.FormatException` inherits directly from [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception).

## When Should You Use It?

Like many of the `Exception` classes within .NET that inherit directly from `System.SystemException`, the `System.FormatException` is quite broad and encompasses a great deal of potential format-related issues.  In fact, there are far too many edge cases to go through them all here, let alone provide code examples, so we'll just briefly list a few common scenarios that could lead to a `System.FormatException`:

- Conversion attempts using `Convert` class methods in an attempt to change a string into another data type by passing an invalid string value.
- `DateTime` class parse attempts that don't conform to the expected _culture-specific_ formatting patterns.
- `GUIDs` that aren't 32-hexadecimal digits.
- Attempting to pass a format string to an object that implements the `IFormattable` interface, but where the format string isn't one of the [standard format strings](https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings?view=netframework-4.7).
- Calling [`String.Format()`](https://docs.microsoft.com/en-us/dotnet/api/system.string.format?view=netframework-4.7), whereby the provided format string contains a greater number of indices than the number of insertion object arguments that were given (e.g. `String.Format("{0:t} on {1:D}", DateTime.Now)`).

There are many other possible scenarios to consider, so you are encouraged to check out the [official documentation](https://docs.microsoft.com/en-us/dotnet/api/system.formatexception?view=netframework-4.7) for all the details on the `System.FormatException` and see in which scenarios it might pop up.

For our code examples today we'll be looking at two scenarios outlined above: Using methods of the `Convert` class with improper string values passed, and using an invalid format string for an `IFormattable` implementor object.  As usual, we'll start with the full code sample below, then walk through it in more detail afterward:

```cs
using System;
using System.Linq;
using Utility;

namespace Airbrake.FormatException
{
    class Program
    {
        static void Main(string[] args)
        {
            // Convert name string to character.
            var name = "John";
            ConvertStringToChar(name);
            // Convert first character of name to character.
            name = name.First().ToString();
            ConvertStringToChar(name);

            Logging.LineSeparator();

            // Convert string to boolean.
            var booleanString = "true";
            ConvertStringToBoolean(booleanString);
            // Convert invalid string to boolean.
            booleanString = "truthy";
            ConvertStringToBoolean(booleanString);

            Logging.LineSeparator();

            // Format decimal using currency format string.
            var value = 49.95m;
            var format = "{0:C2}";
            Logging.Log($"Formatted decimal: {value} via format: {format} into result: {FormatDecimal(value, format)}");
            // Format decimal using non-standard format string.
            format = "{0:Z2}";
            Logging.Log($"Formatted decimal: {value} via format: {format} into result: {FormatDecimal(value, format)}");
        }

        /// <summary>
        /// Converts a string to a character.
        /// </summary>
        /// <param name="value">String value to be converted.</param>
        /// <returns>Converted character result.</returns>
        static char? ConvertStringToChar(string value)
        {
            try
            {
                // Convert string to character.
                var result = Convert.ToChar(value);
                // Log conversion result.
                Logging.Log($"Converted string: {value} to character: {result}.");
                // Return result.
                return result;
            }
            catch (System.FormatException exception)
            {
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                Logging.Log(exception, false);
            }
            return null;
        }

        /// <summary>
        /// Converts a string to a boolean.
        /// </summary>
        /// <param name="value">String value to be converted.</param>
        /// <returns>Converted boolean result.</returns>
        static bool? ConvertStringToBoolean(string value)
        {
            try
            {
                // Convert string to boolean.
                var result = Convert.ToBoolean(value);
                // Log conversion result.
                Logging.Log($"Converted string: {value} to boolean: {result}.");
                // Return result.
                return result;
            }
            catch (System.FormatException exception)
            {
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                Logging.Log(exception, false);
            }
            return null;
        }

        /// <summary>
        /// Format a decimal value using provided format string.
        /// </summary>
        /// <param name="value">Decimal value to format.</param>
        /// <param name="format">Format string to use.</param>
        /// <returns>Resulting formatted string.</returns>
        static string FormatDecimal(decimal value, string format)
        {
            try
            {
                // Directly format passed value.
                return String.Format($"{format}.", value);
            }
            catch (System.FormatException exception)
            {
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                Logging.Log(exception, false);
            }
            return null;
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

We begin with the `ConvertStringToChar(string value)` method, which does just as the name implies.  As with all our example methods, we're logging any caught `System.FormatExceptions` that may be thrown:

```cs
/// <summary>
/// Converts a string to a character.
/// </summary>
/// <param name="value">String value to be converted.</param>
/// <returns>Converted character result.</returns>
static char? ConvertStringToChar(string value)
{
    try
    {
        // Convert string to character.
        var result = Convert.ToChar(value);
        // Log conversion result.
        Logging.Log($"Converted string: {value} to character: {result}.");
        // Return result.
        return result;
    }
    catch (System.FormatException exception)
    {
        Logging.Log(exception);
    }
    catch (Exception exception)
    {
        Logging.Log(exception, false);
    }
    return null;
}
```

We start by declaring a `name` variable and then passing it to `ConvertStringToChar(string value)`:

```cs
// Convert name string to character.
var name = "John";
ConvertStringToChar(name);
```

As you may be aware, the `Convert.ToChar(string value)` method requires that the passed string be only a single character long, so we end up throwing a `System.FormatException`:

```
[EXPECTED] System.FormatException: String must be exactly one character long.
```

Instead, we'll try setting `name` equal to only the first character, then pass it a second time (this is a bit convoluted, since `name.First()` already converts itself to a `char` object):

```cs
// Convert first character of name to character.
name = name.First().ToString();
ConvertStringToChar(name);
```

Sure enough, this conversion works fine:

```
Converted string: J to character: J.
```

We also have a similar `ConvertStringToBoolean(string value)` method that, again, attempts just what the method name states:

```cs
/// <summary>
/// Converts a string to a boolean.
/// </summary>
/// <param name="value">String value to be converted.</param>
/// <returns>Converted boolean result.</returns>
static bool? ConvertStringToBoolean(string value)
{
    try
    {
        // Convert string to boolean.
        var result = Convert.ToBoolean(value);
        // Log conversion result.
        Logging.Log($"Converted string: {value} to boolean: {result}.");
        // Return result.
        return result;
    }
    catch (System.FormatException exception)
    {
        Logging.Log(exception);
    }
    catch (Exception exception)
    {
        Logging.Log(exception, false);
    }
    return null;
}
```

To test this we'll first start with a string value of `"true"`, followed by a value of `"truthy"`:

```cs
// Convert string to boolean.
var booleanString = "true";
ConvertStringToBoolean(booleanString);
// Convert invalid string to boolean.
booleanString = "truthy";
ConvertStringToBoolean(booleanString);
```

As it happens, `Convert.ToBoolean(string value)` only accepts values of `"False"`, `"false"`, `"True"`, or `"true"`, so while our first call succeeds, our second call throws another `System.FormatException`:

```
Converted string: true to boolean: True.
[EXPECTED] System.FormatException: String was not recognized as a valid Boolean.
```

Our last example attempts to provide a format string to an object that implements `IFormattable`.  In this case, the `FormatDecimal(decimal value, string format)` method tries to format the passed `value` using the passed `format` string, then returns the result:

```cs
/// <summary>
/// Format a decimal value using provided format string.
/// </summary>
/// <param name="value">Decimal value to format.</param>
/// <param name="format">Format string to use.</param>
/// <returns>Resulting formatted string.</returns>
static string FormatDecimal(decimal value, string format)
{
    try
    {
        // Directly format passed value.
        return String.Format($"{format}.", value);
    }
    catch (System.FormatException exception)
    {
        Logging.Log(exception);
    }
    catch (Exception exception)
    {
        Logging.Log(exception, false);
    }
    return null;
}
```

To test this method we first declare the `value` variable, along with `format` using a valid [`standard numeric format string`](https://docs.microsoft.com/en-us/dotnet/standard/base-types/standard-numeric-format-strings?view=netframework-4.7) (`c`, in this case, which signifies a currency).  For our second call we try using a `numeric format string` of `Z`, which is invalid:

```cs
// Format decimal using currency format string.
var value = 49.95m;
var format = "{0:C2}";
Logging.Log($"Formatted decimal: {value} via format: {format} into result: {FormatDecimal(value, format)}");
// Format decimal using non-standard format string.
format = "{0:Z2}";
Logging.Log($"Formatted decimal: {value} via format: {format} into result: {FormatDecimal(value, format)}");
```

As expected, our first call performs just as expected, while the second call throws yet another `System.FormatException`, informing us of the issue:

```
Formatted decimal: 49.95 via format: {0:C2} to result: $49.95.
[EXPECTED] System.FormatException: Format specifier was invalid.
```

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A deep dive into the System.FormatException class in .NET, including a C# code illustrating a few common conversion and formatting examples.