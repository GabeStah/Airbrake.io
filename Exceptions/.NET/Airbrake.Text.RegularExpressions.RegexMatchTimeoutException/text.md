# .NET Exceptions - System.Text.RegularExpressions.RegexMatchTimeoutException

Making our way through our detailed [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, we next come to the **RegexMatchTimeoutException**.  This exception is thrown when performing [`RegexMatchTimeoutException.Match()`](https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.match?view=netframework-4.7#System_Text_RegularExpressions_Regex_Match_System_String_System_String_System_Text_RegularExpressions_RegexOptions_System_TimeSpan_) or [`RegexMatchTimeoutException.IsMatch()`](https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.ismatch?view=netframework-4.7#System_Text_RegularExpressions_Regex_IsMatch_System_String_System_String_System_Text_RegularExpressions_RegexOptions_System_TimeSpan_) calls in which the specified `timeout` duration is exceeded while performing the regex operation.  Let's jump right in!

## The Technical Rundown

All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.  The full exception hierarchy of this error is:

- [`System.Object`](https://docs.microsoft.com/en-us/dotnet/api/system.object)
    - [`System.Exception`](https://docs.microsoft.com/en-us/dotnet/api/system.exception)
        - [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception)
            - [`System.TimeoutException`](https://docs.microsoft.com/en-us/dotnet/api/system.timeoutexception)
                - `RegexMatchTimeoutException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```cs
using System;
using System.Text.RegularExpressions;
using Utility;

namespace Airbrake.Text.RegularExpressions.RegexMatchTimeoutException
{
    class Program
    {
        public static void Main()
        {
            // Test a series of input lengths and timeout durations.

            // 100 nanosecond timeout.
            RegexTest(new string('a', 1), "(a)+", new TimeSpan(1));
            RegexTest(new string('a', 1_000), "(a)+", new TimeSpan(1));

            // 1 second timeout.
            RegexTest(new string('a', 1), "(a)+", new TimeSpan(0, 0, 1));
            RegexTest(new string('a', 1_000), "(a)+", new TimeSpan(0, 0, 1));
            RegexTest(new string('a', 1_000_000), "(a)+", new TimeSpan(0, 0, 1));
            RegexTest(new string('a', 1_000_000_000), "(a)+", new TimeSpan(0, 0, 1));
        }

        internal static void RegexTest(string input, 
            string pattern, 
            TimeSpan timeout, 
            RegexOptions options = RegexOptions.IgnoreCase)
        {
            try
            {
                Logging.LineSeparator($"LENGTH: {input.Length:#,#}, PATTERN: {pattern}, TIMEOUT: {timeout}", 75);
                // Get match.
                var match = Regex.Match(input, pattern, options, timeout);
                // Output successful match data.
                Logging.Log($"Match: {match.ToString().Shorten()}");
            }
            catch (System.Text.RegularExpressions.RegexMatchTimeoutException exception)
            {
                // Output expected RegexMatchTimeoutExceptions.
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

There's nothing particularly complicated about the `RegexMatchTimeoutException` -- just about every developer has used regular expressions during his or her time spent coding, as they can be a great tool when working with relatively small strings of text.  However, there's nothing inherently stopping a developer from writing code that attempts to perform regex functions on an _excessively large_ string.  If this occurs, it could very well lead to application bottlenecks, high resource costs, or, in the worst case, crashes and exceptions.

For this reason, some of the core .NET Framework regular expression methods include signatures that accept a [`TimeSpan`](https://docs.microsoft.com/en-us/dotnet/api/system.timespan?view=netframework-4.7) argument.  If provided, this value indicates how long the method should be allowed to run and process before it automatically fails.  In this timeout period is exceeded, a `RegexMatchTimeoutException` is thrown to indicate that something has gone wrong.  This ensures that there aren't any unforeseen lockups or overuse of resources when performing _most_ regular expression logic.

To see this in action, our sample code is quite basic.  Our primary function is `RegexMatchTimeoutExceptionTest(string input, string pattern, TimeSpan timeout, RegexOptions options = RegexOptions.IgnoreCase)`:

```cs
internal static void RegexTest(string input, 
	string pattern, 
	TimeSpan timeout, 
	RegexOptions options = RegexOptions.IgnoreCase)
{
	try
	{
		Logging.LineSeparator($"LENGTH: {input.Length:#,#}, PATTERN: {pattern}, TIMEOUT: {timeout}", 75);
		// Get match.
		var match = Regex.Match(input, pattern, options, timeout);
		// Output successful match data.
		Logging.Log($"Match: {match.ToString().Shorten()}");
	}
	catch (System.Text.RegularExpressions.RegexMatchTimeoutException exception)
	{
		// Output expected RegexMatchTimeoutExceptions.
		Logging.Log(exception);
	}
	catch (Exception exception)
	{
		// Output unexpected Exceptions.
		Logging.Log(exception, false);
	}
}
```

Nothing fancy going on here at all.  We start by outputting some basic information about the regex match that is about to take place, then we execute the `RegexMatchTimeoutException.Match()` method with our passed parameters.  If successful, the matching value is then output to the log.

Now we can test this out in the `Program.Main()` method:

```cs
class Program
{
	public static void Main()
	{
		// Test a series of input lengths and timeout durations.

		// 100 nanosecond timeout.
		RegexTest(new string('a', 1), "(a)+", new TimeSpan(1));
		RegexTest(new string('a', 1_000), "(a)+", new TimeSpan(1));

		// 1 second timeout.
		RegexTest(new string('a', 1), "(a)+", new TimeSpan(0, 0, 1));
		RegexTest(new string('a', 1_000), "(a)+", new TimeSpan(0, 0, 1));
		RegexTest(new string('a', 1_000_000), "(a)+", new TimeSpan(0, 0, 1));
		RegexTest(new string('a', 1_000_000_000), "(a)+", new TimeSpan(0, 0, 1));
	}
}
```

Our `input` string merely contains the letter `a`, repeated the specified number of times.  Our regex pattern of `(a)+` simply tries to find the letter `a`, sequentially repeated as many times as possible.

We begin with a few checks using a `timeout` duration of only `100 nanoseconds`.  The results are as follows:

```
----------- LENGTH: 1, PATTERN: (a)+, TIMEOUT: 00:00:00.0000001 -----------
Match: a

--------- LENGTH: 1,000, PATTERN: (a)+, TIMEOUT: 00:00:00.0000001 ---------
[EXPECTED] System.Text.RegularExpressions.RegexMatchTimeoutException: The RegEx engine has timed out while trying to match a pattern to an input string. This can occur for many reasons, including very large inputs or excessive backtracking caused by nested quantifiers, back-references and other factors.
```

Executing our regex for an `input` string that is merely one character long is no problem, but trying to do so for a thousand-character string proves to be too much to accomplish in under 100 nanoseconds, so a `RegexMatchTimeoutException` is thrown.

That's a pretty short timeout, so let's bump it up to a full second and try again, increasing the length of our `input` string each time:

```
--------------- LENGTH: 1, PATTERN: (a)+, TIMEOUT: 00:00:01 ---------------
Match: a

------------- LENGTH: 1,000, PATTERN: (a)+, TIMEOUT: 00:00:01 -------------
Match: aaaaaaaaaaaaa...aaaaaaaaaaaaaa

----------- LENGTH: 1,000,000, PATTERN: (a)+, TIMEOUT: 00:00:01 -----------
Match: aaaaaaaaaaaaa...aaaaaaaaaaaaaa

--------- LENGTH: 1,000,000,000, PATTERN: (a)+, TIMEOUT: 00:00:01 ---------
[UNEXPECTED] System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
```

Unsurprisingly, modern computers are fast, so my system has no trouble performing this regex function on strings up to one million characters long.  However, at one _billion_ characters in length, rather than a `RegexMatchTimeoutException`, I actually get a `System.OutOfMemoryException`.  Obviously, with enough memory and processing power, we could raise these `length` and `timeout` limitations over and over, until eventually we run into a `RegexMatchTimeoutException` once again, but in my case, turns out memory is the real bottleneck.  Interesting!

There's one last caveat to mention here.  If you run into a situation where you absolutely _must_ ensure that an expensive regular expression method call continues processing indefinitely, without potentially timing out, you can use the [`InfiniteMatchTimeout`](https://docs.microsoft.com/en-us/dotnet/api/system.text.regularexpressions.regex.infinitematchtimeout?view=netframework-4.7) constant.  Just pass that constant to any invocation of a regex match method that expects a `timeout` parameter, and the call will never timeout: `RegexMatchTimeoutExceptionTest(new string('a', 1_000_000), "(a)+", Regex.InfiniteMatchTimeout)`

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A deep dive into the RegexMatchTimeoutException in .NET, including functional C# code samples illustrating how this exception is typically thrown.