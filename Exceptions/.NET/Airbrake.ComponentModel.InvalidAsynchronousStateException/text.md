# .NET Exceptions - System.ComponentModel.InvalidAsynchronousStateException

As we approach the end of our in-depth [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, today we'll be taking a closer look at the **System.ComponentModel.InvalidAsynchronousStateException**.  An `InvalidAsynchronousStateException` is a complex exception that occurs when an operation is attempted for which its _target_ thread no longer exists, or has no message loop.

Throughout this article we'll explore the `InvalidAsynchronousStateException` by looking first at where it sits in the larger .NET exception hierarchy.  Then, we'll take a look at some functional sample code that shows how you might run into an `InvalidAsynchronousStateException` in your own multi-threaded application if you aren't careful as a developer.  Let's get started!

## The Technical Rundown

All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.  The full exception hierarchy of this error is:

- [`System.Object`](https://docs.microsoft.com/en-us/dotnet/api/system.object)
    - [`System.Exception`](https://docs.microsoft.com/en-us/dotnet/api/system.exception)
        - [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception)
            - [`System.ArgumentException`](https://docs.microsoft.com/en-us/dotnet/api/system.argumentexception?view=netframework-4.7.1)
                - `InvalidAsynchronousStateException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```cs
using System;
using System.Windows.Forms;

namespace Airbrake.ComponentModel.InvalidAsynchronousStateException
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Instantiate DualThreadTester.
            new DualThreadTester();

            // Create form.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
```

```cs
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Utility;

namespace Airbrake.ComponentModel.InvalidAsynchronousStateException
{
    internal class DualThreadTester
    {
        private Form _backgroundForm;

        public DualThreadTester()
        {
            // Create foreground and background threads.
            var foreground = new Thread(IterationTest)
            {
                Name = "Foreground",
                IsBackground = false
            };
            var background = new Thread(IterationTest)
            {
                Name = "Background",
                IsBackground = true
            };

            // Start both threads nearly simultaneously.
            foreground.Start();
            background.Start();
        }

        /// <summary>
        /// Delegate to use with Background thread.
        /// </summary>
        /// <param name="message">Message to be output to log.</param>
        public delegate void BackgroundThreadDelegate(string message);

        /// <summary>
        /// Output passed message to log.
        /// </summary>
        /// <param name="message">Message to output.</param>
        public void BackgroundThreadDelegateMethod(string message)
        {
            Logging.Log(message);
        }

        /// <summary>
        /// Performs basic iteration test for currently active thread.
        /// </summary>
        public void IterationTest()
        {
            try
            {
                // Check if current thread is background.
                if (Thread.CurrentThread.IsBackground)
                {
                    // Create and show BackgroundForm.
                    _backgroundForm = new BackgroundForm();
                    _backgroundForm.Show();
                }
                // Loop a few times.
                for (var count = 0; count < 10; count++)
                {
                    // Check if thread is foreground.
                    if (!Thread.CurrentThread.IsBackground)
                    {
                        // Confirm BackgroundForm exists.
                        if (_backgroundForm != null)
                        {
                            // Confirm that invocation is required.
                            if (_backgroundForm.InvokeRequired)
                            {
                                // Invoke BackgroundThreadDelegateMethod with current count argument.
                                _backgroundForm.Invoke(new BackgroundThreadDelegate(BackgroundThreadDelegateMethod),
                                    count);
                            }
                            else
                            {
                                // If no invocation required, directly set color.
                                _backgroundForm.BackColor = Color.Red;
                            }
                        }
                    }
                    Logging.Log($"{Thread.CurrentThread.Name} thread count: {count}");
                    Thread.Sleep(250);
                }
                Logging.Log($"{Thread.CurrentThread.Name} finished.");
            }
            catch (System.ComponentModel.InvalidAsynchronousStateException exception)
            {
                // Output expected InvalidAsynchronousStateExceptions.
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

As mentioned, the appearance of an `InvalidAsynchronousStateException` indicates a problem with threading within your application.  Usually, it indicates that an operation was attempted within a thread that no longer exists.  Since multi-threading is such a broad and complex topic, it's well beyond the scope of this tiny article, so we'll just cover the basics in our own code example.  Just keep in mind that this technique is by no means representative of the _only_ way to accomplish multi-threading.

For our example we're creating a `Windows Form Application` (i.e. an application with user interface elements, as opposed to the normal console-based applications we usually create).  There's no need to include all the code, but here we can see that our main `Program.Main()` method includes a call to our custom `DualThreadTester()` class:

```cs
using System;
using System.Windows.Forms;

namespace Airbrake.ComponentModel.InvalidAsynchronousStateException
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Instantiate DualThreadTester.
            new DualThreadTester();

            // Create form.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
```

The `DuelThreadTester` class is intended to create a new foreground and background thread, and perform some basic iteration a few times a second with output.  In addition, we want to create a new UI element (a `background Form`, in this case), and be able to perform an `InvalidAsynchronousStateExceptionocation` within this UI control to invoke a specific method.  This will make more sense in code, so let's start with the structure, including our single `_backgroundForm` property, along with the `BackgroundThreadDelegate(string message)` delegate, and the `BackgroundThreadDelegateMethod(string message)` method:

```cs
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Utility;

namespace Airbrake.ComponentModel.InvalidAsynchronousStateException
{
    internal class DualThreadTester
    {
        private Form _backgroundForm;

        /// <summary>
        /// Delegate to use with Background thread.
        /// </summary>
        /// <param name="message">Message to be output to log.</param>
        public delegate void BackgroundThreadDelegate(string message);

        /// <summary>
        /// Output passed message to log.
        /// </summary>
        /// <param name="message">Message to output.</param>
        public void BackgroundThreadDelegateMethod(string message)
        {
            Logging.Log(message);
        }
    }
}
```

We'll be `InvalidAsynchronousStateExceptionoking` this delegate and method from our `_backgroundForm` control, so it's important we establish these first.

Next comes the primary looping method of `IterationTest()`:

```cs
/// <summary>
/// Performs basic iteration test for currently active thread.
/// </summary>
public void IterationTest()
{
    try
    {
        // Check if current thread is background.
        if (Thread.CurrentThread.IsBackground)
        {
            // Create and show BackgroundForm.
            _backgroundForm = new BackgroundForm();
            _backgroundForm.Show();
        }
        // Loop a few times.
        for (var count = 0; count < 10; count++)
        {
            // Check if thread is foreground.
            if (!Thread.CurrentThread.IsBackground)
            {
                // Confirm BackgroundForm exists.
                if (_backgroundForm != null)
                {
                    // Confirm that invocation is required.
                    if (_backgroundForm.InvokeRequired)
                    {
                        // Invoke BackgroundThreadDelegateMethod with current count argument.
                        _backgroundForm.Invoke(new BackgroundThreadDelegate(BackgroundThreadDelegateMethod),
                            count);
                    }
                    else
                    {
                        // If no invocation required, directly set color.
                        _backgroundForm.BackColor = Color.Red;
                    }
                }
            }
            Logging.Log($"{Thread.CurrentThread.Name} thread count: {count}");
            Thread.Sleep(250);
        }
        Logging.Log($"{Thread.CurrentThread.Name} finished.");
    }
    catch (System.ComponentModel.InvalidAsynchronousStateException exception)
    {
        // Output expected InvalidAsynchronousStateExceptions.
        Logging.Log(exception);
    }
    catch (Exception exception)
    {
        // Output unexpected Exceptions.
        Logging.Log(exception, false);
    }
}
```

Since this method is used by both our `foreground` and `background` threads, we start by checking if the current thread is background, in which case we instantiate a new `BackgroundForm` control and `Show()` it.

From there, we perform a simple iterative loop 10 times.  During each loop, since our `foreground` thread will execute slightly before our `background` thread (in most cases), we check if `_backgroundForm` exists within the `foreground` thread iteration.  If it exists, we use the [thread safe method](https://docs.microsoft.com/en-us/dotnet/framework/winforms/controls/how-to-make-thread-safe-calls-to-windows-forms-controls) of calling an `InvalidAsynchronousStateExceptionoke()` method on our `_backgroundForm` control.  The `InvalidAsynchronousStateExceptionokeRequired` property simply checks if the current thread differs from the thread that created the `_backgroundForm` control.  If so (which is always the case here), we explicitly call `_backgroundForm.Invoke(...)` and use our `BackgroundThreadDelegate` delegate to create a simple output message.  If `InvalidAsynchronousStateExceptionokeRequired` is false, we can just directly change the control (in this case, by changing the background color to red).

Regardless of which thread is iterating, we output the thread name and the current iteration count to the log, then pause for a quarter of a second before repeating.  Once all iterations complete, we output that the thread has finished iteration.

Overall, it's not too complicated, so let's create our actual threads in the `DualThreadTester` constructor:

```cs
public DualThreadTester()
{
    // Create foreground and background threads.
    var foreground = new Thread(IterationTest)
    {
        Name = "Foreground",
        IsBackground = false
    };
    var background = new Thread(IterationTest)
    {
        Name = "Background",
        IsBackground = true
    };

    // Start both threads nearly simultaneously.
    foreground.Start();
    background.Start();
}
```

As the comments explain, we're simply instantiating two new threads, naming them and setting the appropriate `IsBackground` property, then starting them up.  This will cause both threads to begin execution of the `IterationTest` method.  Let's execute this code and see what happens.

```
Foreground thread count: 0
Background thread count: 0
Background thread count: 1
Background thread count: 2
Background thread count: 3
Background thread count: 4
Background thread count: 5
Background thread count: 6
Background thread count: 7
Background thread count: 8
Background thread count: 9
Background finished.
[EXPECTED] System.ComponentModel.InvalidAsynchronousStateException: An error occurred invoking the method.  The destination thread no longer exists.
```

There we go.  As we can see from the output, our `foreground` thread only began a single iteration, then reached the `_backgroundForm.Invoke(new BackgroundThreadDelegate(BackgroundThreadDelegateMethod),count);` statement, which blocks the `foreground` thread from executing until this invocation can be executed.  Thus, `foreground` waited the 2.5 seconds until the `background` thread finished its iterations, then the `_backgroundForm.Invoke(...)` method call was executed.  However, as indicated by the `InvalidAsynchronousStateException` we see, this operation could no longer complete because the `background` thread closed itself out once it finished its iterations.

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A close look at the System.ComponentModel.InvalidAsynchronousStateException in .NET, including a simple multi-threaded code example.