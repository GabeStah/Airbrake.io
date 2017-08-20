# .NET Exceptions - System.OverflowException

Moving along through our detailed [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, today we'll be tackling the ever-popular System.OverflowException.  With a name that most developers will probably recognize, the `System.OverflowException` in .NET indicates that an invalid arithmetic, casting, or conversion error occurred within a `checked` context.

Throughout this article we'll explore the `System.OverflowException` in greater detail, starting with a look at the (rather simple) .NET exception hierarchy chain into which it falls.  We'll also go over a few functional C# code samples that will illustrate the two different scenarios in which `System.OverflowExceptions` are commonly thrown, so you're better able to handle them in your own code.  Let's get going!

## The Technical Rundown

- All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.
- [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception) is inherited from the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) class.
- `System.OverflowException` inherits from [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception).

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```cs
// <Airbrake.OverflowException>/Program.cs
using System;
using System.Reflection;
using Utility;

namespace Airbrake.OverflowException
{
    class Program
    {
        static void Main(string[] args)
        {
            // Multiple one billion by three in checked context.
            Logging.LineSeparator("CHECKED MULTIPLICATION");
            MultiplyNumbers(1_000_000_000, 3);

            // Multiple one billion by three in unchecked context.
            Logging.LineSeparator("UNCHECKED MULTIPLICATION");
            MultiplyNumbers(1_000_000_000, 3, false);

            const int value = 200;
            // Convert 200 to SByte in checked context.
            Logging.LineSeparator("CHECKED TYPE CONVERSION");
            ConvertValueToType<sbyte>(value);

            // Convert 200 to SByte in unchecked context.
            Logging.LineSeparator("UNCHECKED TYPE CONVERSION");
            ConvertValueToType<sbyte>(value, false);

            // Convert 200 directly, without Convert.ChangeType().
            Logging.LineSeparator("DIRECT UNCHECKED TYPE CONVERSION");
            unchecked
            {
                var result = (sbyte) value;
                // Output result.
                Logging.Log($"[UNCHECKED] {value:n0} converted to type {result.GetType().Name}: {result:n0}.");
            }
        }

        /// <summary>
        /// Attempts to multiple two passed integer values together.
        /// </summary>
        /// <param name="a">First value.</param>
        /// <param name="b">Second value.</param>
        /// <param name="checked">Determines if 'checked' context is used for multiplication attempt.</param>
        internal static void MultiplyNumbers(int a, int b, bool @checked = true)
        {
            try
            {
                int result;
                if (@checked)
                {
                    // Enable overflow checking.
                    checked
                    {
                        // Multiple numbers.
                        result = a * b;
                    }
                }
                // Disable overflow checking.
                unchecked
                {
                    // Multiple numbers.
                    result = a * b;
                }
                // Output result.
                Logging.Log($"[{(@checked ? "CHECKED" : "UNCHECKED")}] {a:n0} * {b:n0} = {result:n0}");
            }
            catch (System.OverflowException exception)
            {
                // Catch expected OverflowExceptions.
                Logging.Log(exception);
                Logging.Log($"[{(@checked ? "CHECKED" : "UNCHECKED")}] {a:n0} * {b:n0} exceeds int.MaxValue: {int.MaxValue:n0}");
            }
            catch (Exception exception)
            {
                // Catch unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }

        /// <summary>
        /// Attempts to convert passed value to specified generic type T.
        /// </summary>
        /// <typeparam name="T">Generic type to convert to.</typeparam>
        /// <param name="value">Value to be converted.</param>
        /// <param name="checked">Determines if 'checked' context is used for multiplication attempt.</param>
        internal static void ConvertValueToType<T>(int value, bool @checked = true)
        {
            try
            {
                object result;
                if (@checked)
                {
                    // Enable overflow checking.
                    checked
                    {
                        // Convert to type T.
                        result = (T) Convert.ChangeType(value, typeof(T));
                        
                    }
                }
                // Disable overflow checking.
                unchecked
                {
                    // Convert to type T.
                    result = (T) Convert.ChangeType(value, typeof(T));
                }
                // Output result.
                Logging.Log($"[{(@checked ? "CHECKED" : "UNCHECKED")}] {value:n0} converted to type {result.GetType().Name}: {result:n0}.");
            }
            catch (System.OverflowException exception)
            {
                // Catch expected OverflowExceptions.
                Logging.Log(exception);
                // Since this is a generic type, need to use reflection to get the MaxValue field, if applicable.
                var maxValueField = typeof(T).GetField("MaxValue", BindingFlags.Public
                                                                         | BindingFlags.Static);
                if (maxValueField == null)
                {
                    throw new NotSupportedException(typeof(T).Name);
                }
                var maxValue = (T) maxValueField.GetValue(null);

                Logging.Log($"[{(@checked ? "CHECKED" : "UNCHECKED")}] {value:n0} cannot be converted to type {typeof(T).Name} because it exceeds {typeof(T).Name}.MaxValue: {maxValue:n0}");
            }
            catch (Exception exception)
            {
                // Catch unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }
    }
}

// <Utility>/Logging.cs
using System;
using System.Diagnostics;

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
            Debug.WriteLine(outputType == OutputType.Timestamp
                ? $"[{StopwatchProxy.Instance.Stopwatch.Elapsed}] {ObjectDumper.Dump(value)}"
                : ObjectDumper.Dump(value));
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

To understand what the `System.OverflowException` is, we should first briefly examine what `overflow` errors are in a more general sense.  There are a few different types of overflow in computing.

As you may recall, we explored the notion of `stack overflow` in our [`.NET Exception Handling – System.StackOverflowException`](https://airbrake.io/blog/dotnet-exception-handling/system-stackoverflowexception) article.  Feel free to check that out for all the details, but the long and short of it is that a `stack overflow` occurs when an application attempts to utilize more memory than was allocated to it in the memory `address space`.  This results in a `stack overflow` exception in most applications, because the system recognizes the application is attempting to use memory that doesn't "belong" to it.

As in the case of the `System.OverflowException`, a general `overflow` typically refers to an [`integer overflow`](https://en.wikipedia.org/wiki/Integer_overflow).  This is similar to a `stack overflow`, except that, instead of attempting to use _memory_ outside of the allowable bounds, the application is attempting to create _numeric values_ outside the allowable bounds.  We explored this concept a bit in our [`Ruby Exception Handling: NoMemoryError`](https://airbrake.io/blog/ruby-exception-handling/nomemoryerror) article.  The basic issue is that any given data type, such as an `integer`, has a maximum value based on the number of `bits` (or `bytes`) it is allocated.  When your application attempts to create a value that would exceed that number of bits to store, unless it can be automatically converted to a larger data type, an `integer overflow` is thrown.

To see this in action we've created two methods for testing.  The first of these methods is `MultiplyNumbers(int a, int b, bool @checked = true)`:

```cs
/// <summary>
/// Attempts to multiple two passed integer values together.
/// </summary>
/// <param name="a">First value.</param>
/// <param name="b">Second value.</param>
/// <param name="checked">Determines if 'checked' context is used for multiplication attempt.</param>
internal static void MultiplyNumbers(int a, int b, bool @checked = true)
{
    try
    {
        int result;
        if (@checked)
        {
            // Enable overflow checking.
            checked
            {
                // Multiple numbers.
                result = a * b;
            }
        }
        // Disable overflow checking.
        unchecked
        {
            // Multiple numbers.
            result = a * b;
        }
        // Output result.
        Logging.Log($"[{(@checked ? "CHECKED" : "UNCHECKED")}] {a:n0} * {b:n0} = {result:n0}");
    }
    catch (System.OverflowException exception)
    {
        // Catch expected OverflowExceptions.
        Logging.Log(exception);
        Logging.Log($"[{(@checked ? "CHECKED" : "UNCHECKED")}] {a:n0} * {b:n0} exceeds int.MaxValue: {int.MaxValue:n0}");
    }
    catch (Exception exception)
    {
        // Catch unexpected Exceptions.
        Logging.Log(exception, false);
    }
}
```

Nothing too fancy going on here, except we're using the `@checked` boolean parameter to determine if we should use a [`checked` or `unchecked` context](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/checked-and-unchecked) during the calculation.  In C#, a `checked` context forces the common language runtime (`CLR`) to raise `System.OverflowExceptions` when overflows occur, while `unchecked` context simply ignores them and truncates the resulting value.

To test out our `MultiplyNumbers(int a, int b, bool @checked = true)` method we are making two calls to it:

```cs
// Multiple one billion by three in checked context.
Logging.LineSeparator("CHECKED MULTIPLICATION");
MultiplyNumbers(1_000_000_000, 3);

// Multiple one billion by three in unchecked context.
Logging.LineSeparator("UNCHECKED MULTIPLICATION");
MultiplyNumbers(1_000_000_000, 3, false);
```

If you aren't familiar, the underscores (`_`) in our large number literal are a new feature added in C# 7.0 called `digit separators`, which allow us to _visually_ change how numbers are displayed without impacting their underlying value.  Feel free to read more about this and other C# 7.0 features in our [`What’s New in C# 7.0? – Digit Separators, Reference Returns, and Binary Literals`](https://airbrake.io/blog/csharp/digit-separators-reference-returns-and-binary-literals) article.

Anyway, as you can see we're merely attempting to multiple one billion by three, which should result in a value of three billion.  Executing these calls gives us the following output:

```
-------- CHECKED MULTIPLICATION --------
[EXPECTED] System.OverflowException: Arithmetic operation resulted in an overflow.
[CHECKED] 1,000,000,000 * 3 exceeds int.MaxValue: 2,147,483,647
------- UNCHECKED MULTIPLICATION -------
[UNCHECKED] 1,000,000,000 * 3 = -1,294,967,296
```

As we can see, the first `checked` context call throws a `System.OverflowException` because the total value of three billion exceeds the maximum value that can be stored in an `integer` of `2,147,483,647`.  On the other hand, the second `unchecked` context call doesn't throw an error, and instead produces an interesting result of `-1,294,967,296`.  Why on earth are we getting a _negative_ result when multiplying two positive numbers together?  The answer lies in how the computer handles overflow truncation.

To examine this, what what happens if we take the `-1,294,967,296` result we got, then subtract `int.MinValue` and add `int.MaxValue` to it:

```
-1,294,967,296 - -2,147,483,648 + 2,147,483,647 = 2,999,999,999
```

That's incredibly close to our expected result of three billion.  In fact, the loss of one extra digit is due to the fact that `signed number` values must represent the number zero with a particular combination of bits.  This "extra" value that is taken up during overflow rollover/truncation is lost in our resulting number above, giving us one less than the expected value when added back together.

Cool.  Now let's take a look at the `ConvertValueToType<T>(int value, bool @checked = true)` method:

```cs
/// <summary>
/// Attempts to convert passed value to specified generic type T.
/// </summary>
/// <typeparam name="T">Generic type to convert to.</typeparam>
/// <param name="value">Value to be converted.</param>
/// <param name="checked">Determines if 'checked' context is used for multiplication attempt.</param>
internal static void ConvertValueToType<T>(int value, bool @checked = true)
{
    try
    {
        object result;
        if (@checked)
        {
            // Enable overflow checking.
            checked
            {
                // Convert to type T.
                result = (T) Convert.ChangeType(value, typeof(T));
                
            }
        }
        // Disable overflow checking.
        unchecked
        {
            // Convert to type T.
            result = (T) Convert.ChangeType(value, typeof(T));
        }
        // Output result.
        Logging.Log($"[{(@checked ? "CHECKED" : "UNCHECKED")}] {value:n0} converted to type {result.GetType().Name}: {result:n0}.");
    }
    catch (System.OverflowException exception)
    {
        // Catch expected OverflowExceptions.
        Logging.Log(exception);
        // Since this is a generic type, need to use reflection to get the MaxValue field, if applicable.
        var maxValueField = typeof(T).GetField("MaxValue", BindingFlags.Public
                                                                    | BindingFlags.Static);
        if (maxValueField == null)
        {
            throw new NotSupportedException(typeof(T).Name);
        }
        var maxValue = (T) maxValueField.GetValue(null);

        Logging.Log($"[{(@checked ? "CHECKED" : "UNCHECKED")}] {value:n0} cannot be converted to type {typeof(T).Name} because it exceeds {typeof(T).Name}.MaxValue: {maxValue:n0}");
    }
    catch (Exception exception)
    {
        // Catch unexpected Exceptions.
        Logging.Log(exception, false);
    }
}
```

This one is a bit more complex because we're allowing the method to be called with a generic type (`T`).  This method attempts to convert the passed `value` into whatever type `T` was provided.  We can use this to make some type conversions in our calling code:

```cs
const int value = 200;
// Convert 200 to SByte in checked context.
Logging.LineSeparator("CHECKED TYPE CONVERSION");
ConvertValueToType<sbyte>(value);

// Convert 200 to SByte in unchecked context.
Logging.LineSeparator("UNCHECKED TYPE CONVERSION");
ConvertValueToType<sbyte>(value, false);
```

Just as before, we're making two calls to our method, the first being `checked` and the second `unchecked` context.  Our goal here is to convert the `int` value of `200` into a [`sbyte`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/sbyte), which is stored as a `signed 8-bit integer` in the background.  Calling this code produces the following output:

```
------- CHECKED TYPE CONVERSION --------
[EXPECTED] System.OverflowException: Value was either too large or too small for a signed byte.
[CHECKED] 200 cannot be converted to type SByte because it exceeds SByte.MaxValue: 127
------ UNCHECKED TYPE CONVERSION -------
[EXPECTED] System.OverflowException: Value was either too large or too small for a signed byte.
   at System.Convert.ToSByte(Int32 value)
   at System.Int32.System.IConvertible.ToSByte(IFormatProvider provider)
   at System.Convert.ChangeType(Object value, Type conversionType, IFormatProvider provider)
   at System.Convert.ChangeType(Object value, Type conversionType)
   at Airbrake.OverflowException.Program.ConvertValueToType[T](Int32 value, Boolean checked) in D:\work\Airbrake.io\Exceptions\.NET\Airbrake.OverflowException\Program.cs:line 106: Value was either too large or too small for a signed byte.
[UNCHECKED] 200 cannot be converted to type SByte because it exceeds SByte.MaxValue: 127
```

The first `checked` context call, as expected, throws a `System.OverflowException` because we're attempting to convert a value of `200` into a `signed 8-bit integer`, which can only handle a maximum positive value of `127`.  However, our `unchecked` context call is _also_ throwing a `System.OverflowException` for the same reason, even though it shouldn't produce overflow errors.  The reason can be gleaned from the stack trace that produced the error, which I've left in the output above.  Since our method makes use of the [`Convert.ChangeType()`](https://docs.microsoft.com/en-us/dotnet/api/system.convert.changetype?view=netframework-4.7) method to perform the actual conversion, that built-in .NET method is overriding our `unchecked` context setting and enabling a `checked` context within its own execution.

To remedy this we can try explicitly converting our `int value` to an `sbyte`:

```cs
// Convert 200 directly, without Convert.ChangeType().
Logging.LineSeparator("DIRECT UNCHECKED TYPE CONVERSION");
unchecked
{
    var result = (sbyte) value;
    // Output result.
    Logging.Log($"[UNCHECKED] {value:n0} converted to type {result.GetType().Name}: {result:n0}.");
}
```

Sure enough, running this code results in a properly converted value, as expected:

```
--- DIRECT UNCHECKED TYPE CONVERSION ---
[UNCHECKED] 200 converted to type SByte: -56.
```

As before, the result is negative because we've "rolled over" the maximum positive value of `127`, just as we did above with our integer multiplication attempts.

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A close look at the System.OverflowException in .NET, including C# code showing how overflow manipulations and rollovers are handled in most systems.