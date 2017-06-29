# .NET Exceptions - System.ComponentModel.Win32Exception

Today, in our continued journey through the [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, we'll be exploring the wide world of the `System.ComponentModel.Win32Exception`.  As indicated by the `Win32` part of the name, the `System.ComponentModel.Win32Exception` occurs only when dealing with legacy-style applications or code -- where your application must invoke direct operating system calls, such as trying to execute other applications.

Throughout this article we'll dive into the `System.ComponentModel.Win32Exception` in more detail, including where it resides in the .NET exception hierarchy.  We'll also dig into a few functional C# code examples to better illustrate how `System.ComponentModel.Win32Exceptions` might be commonly thrown in your own coding adventures, so let's get to it!

## The Technical Rundown

- All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.
- [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception) is inherited from the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) class.
- [`System.Runtime.InteropServices.ExternalException`](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.externalexception?view=netframework-4.7) inherits directly from [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception).
- Finally, `System.ComponentModel.Win32Exception` inherits from [`System.Runtime.InteropServices.ExternalException`](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.externalexception?view=netframework-4.7).

## When Should You Use It?

The `System.ComponentModel.Win32Exception` is the most basic exception type that will occur within your .NET applications when something goes wrong while using internal `win32`-style operating system calls.  These can vary from invalid path and file not found errors to network address issues and resource management problems.  Since `System.ComponentModel.Win32Exceptions` are wrappers for older forms of exceptions, each of the possible errors you can encounter will have its own `NativeErrorCode` property value, which is a 32-bit `integer` referring to the relevant [`Win32 Error Code`](https://msdn.microsoft.com/en-us/library/cc231199.aspx) value associated with the exception that was thrown.  We won't go over them all here, but that win32 error code URL will be a good reference when trying to debug your own `System.ComponentModel.Win32Exceptions`.

To see a `System.ComponentModel.Win32Exception` in action the best place to start is with our example code.  The whole snippet is displayed below, after which we'll go over it in a bit more detail:

```cs
using System.Diagnostics;
using Utility;

namespace Airbrake.ComponentModel.Win32Exception
{
    class Program
    {
        static void Main(string[] args)
        {
            StartProcessFromPath("c:/windows/notepad.exe");
            Logging.LineSeparator();
            StartProcessFromPath("c:/windows/invalid.exe");
        }

        static void StartProcessFromPath(string path)
        {
            try
            {
                // Create a new process with StartInfo.FileName set to provided path.
                var process = new Process { StartInfo = { FileName = path } };
                // Attempt to start the process using provided executable path.
                var success = process.Start();
                if (success)
                {
                    Logging.Log($"Successfully launched '{process.ProcessName.ToString()}' process!");
                    // Sleep for two seconds to allow time for window to be shown.
                    System.Threading.Thread.Sleep(2000);
                    // Kill process.
                    process.Kill();
                    Logging.Log($"Killed '{process.ProcessName.ToString()}' process.");
                }
                else
                {
                    // This code never executes since we're catching
                    // an exception from the process.Start() invocation line.
                }
            }
            catch (System.ComponentModel.Win32Exception exception)
            {
                // Indicate failure to start.
                Logging.Log($"Unable to start process with executable path '{path}'.");
                // Output caught exception.
                Logging.Log(exception);
                Logging.Log($"Native Win32 Error Code: {exception.NativeErrorCode}");
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

Most of our example occurs within the `StartProcessFromPath()` method, which accepts a string path value that should point to an executable file.  We then instantiate a new `Process` and attempt to start it with the `.Start()` method.  The `.Start()` method returns a boolean indicating if the attempt was successful, so we simply use that boolean to perform some extra logic, such as outputting some information to the log to indicate a success or failure, along with a small `Sleep()` period to give us time to see the newly launched window, if applicable.  We finish up by killing the process with the `.Kill()` method.

```cs
static void StartProcessFromPath(string path)
{
    try
    {
        // Create a new process with StartInfo.FileName set to provided path.
        var process = new Process { StartInfo = { FileName = path } };
        // Attempt to start the process using provided executable path.
        var success = process.Start();
        if (success)
        {
            Logging.Log($"Successfully launched '{process.ProcessName.ToString()}' process!");
            // Sleep for two seconds to allow time for window to be shown.
            System.Threading.Thread.Sleep(2000);
            // Kill process.
            process.Kill();
            Logging.Log($"Killed '{process.ProcessName.ToString()}' process.");
        }
        else
        {
            // This code never executes since we're catching
            // an exception from the process.Start() invocation line.
        }
    }
    catch (System.ComponentModel.Win32Exception exception)
    {
        // Indicate failure to start.
        Logging.Log($"Unable to start process with executable path '{path}'.");
        // Output caught exception.
        Logging.Log(exception);
        Logging.Log($"Native Win32 Error Code: {exception.NativeErrorCode}");
    }
}
```

To illustrate both a success and failure our `Main()` method tries to launch `notepad.exe` as well as `invalid.exe`:

```cs
static void Main(string[] args)
{
    StartProcessFromPath("c:/windows/notepad.exe");
    Logging.LineSeparator();
    StartProcessFromPath("c:/windows/invalid.exe");
}
```

As you might suspect, the first `StartProcessFromPath()` call works just fine and we see the `Notepad` window pop up for two seconds before disappearing due to the `Kill()` method call.  We also get confirmation of the success from the console log output:

```
Successfully launched 'notepad' process!
Killed 'notepad' process.
```

On the other hand, our second `StartProcessFromPath()` call to the `invalid.exe` fails and throws a `System.ComponentModel.Win32Exception` our way:

```
Unable to start process with executable path: 'c:/windows/invalid.exe'.
[EXPECTED] System.ComponentModel.Win32Exception (0x80004005): The system cannot find the file specified
Native Win32 Error Code: 2
```

We also made sure to output the relevant `Win32 Error Code` by outputting the `NativeErrorCode` property when a `System.ComponentModel.Win32Exception` is caught, so we're able to see that the `win32` error code is: `2`.  If we refer back to the [`Win32 Error Code`](https://msdn.microsoft.com/en-us/library/cc231199.aspx) table we find that the value of `2` (i.e. `0x00000002`) corresponds to the `ERROR_FILE_NOT_FOUND` enumeration, with the description of `The system cannot find the file specified.`  Sure enough, this description matches the `System.ComponentModel.Win32Exception` output message we got exactly!

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A close look at the System.ComponentModel.Win32Exception in .NET, including a C# code sample illustrating how to find matching win32 error codes.