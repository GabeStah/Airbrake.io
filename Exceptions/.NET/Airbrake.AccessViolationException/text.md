# .NET Exceptions - System.AccessViolationException

Making our way through the [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, today we'll be exploring the `System.AccessViolationException` in more detail.  A `System.AccessViolationException` occurs when unmanaged/unsafe code attempts to use memory that has not been allocated, or to memory that it doesn't have access to.

In this article we'll explore the `System.AccessViolationException` a bit more, looking at where it resides in the .NET exception hierarchy.  We'll also examine a functional C#/C++ code example to illustrate how `System.AccessViolationExceptions` are commonly thrown and what options you have for dealing with them in your own code, so let's get going!

## The Technical Rundown

- All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.
- [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception) is inherited from the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) class.
- `System.AccessViolationException` inherits directly from [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception).

## When Should You Use It?

As mentioned in the introduction, a `System.AccessViolationException` can only occur when your application is using unmanaged code.  For many .NET applications, this will never occur, due to how .NET handles managed versus unmanaged code.

Managed code is code that .NET compiles and executes using the [`common language runtime`](https://msdn.microsoft.com/en-us/library/8bs2ecf4(v=vs.110).aspx) (`CLR`).  Conversely, unmanaged code compiles into machine code, which _is not_ executed within the safety of the `CLR`.

Many languages that rely on .NET, such as C# and Visual Basic, are _entirely_ compiled and executed in the `CLR`.  This means that code written in C# is always managed code and, therefore, can never throw a `System.AccessViolationException`.

However, a language like Visual C++ _does_ allow unmanaged code to be written.  In such cases, it's entirely possible to access unallocated memory, and therefore, throw a `System.AccessViolationException`.

To illustrate these differences and to see how `System.AccessViolationExceptions` might occur in your own projects we'll start with the full working code sample below, then break it down in more detail afterward.  Since we're using both C# and C++ in this example, we'll start with the C# and then move onto the C++:

```cs
using System;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security;
using Utility;

namespace Airbrake.AccessViolationException
{
    class Program
    {       
        static void Main(string[] args)
        {
            ReferenceTest();
            ReferenceTestWithHandler();
        }

        /// <summary>
        /// Create a reference from unmanaged FailingApp.dll C++ code.
        /// </summary>
        /// <returns>Reference.</returns>
        [DllImport(@"D:\work\Airbrake.io\Exceptions\.NET\Debug\FailingApp.dll")]
        private static extern int CreateReference();

        /// <summary>
        /// Test reference creation through unmanaged code.
        /// </summary>
        /// <returns>Reference result.</returns>
        public static int ReferenceTest()
        {
            try
            {
                // Attempt to create a reference through unmanaged code (C++ DLL).
                var result = CreateReference();
                // If no exception occurred, output successful result.
                Logging.Log($"Reference successfully created at: {result}.");
                // Return result.
                return result;
            }
            catch (System.AccessViolationException exception)
            {
                // Output explicit exception.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Output inexplicit exception.
                Logging.Log(exception, false);
            }
            // Return zero to indicate failure.
            return 0;
        }

        /// <summary>
        /// Test reference creation through unmanaged code.
        /// HandleProcessCorruptedStateExceptions attribute allows CLR
        /// to catch normally ignored exceptions due to unmanaged code.
        /// </summary>
        /// <returns>Reference result.</returns>
        [HandleProcessCorruptedStateExceptions]
        public static int ReferenceTestWithHandler()
        {
            try
            {
                // Attempt to create a reference through unmanaged code (C++ DLL).
                var result = CreateReference();
                // If no exception occurred, output successful result.
                Logging.Log($"Reference successfully created at: {result}.");
                // Return result.
                return result;
            }
            catch (System.AccessViolationException exception)
            {
                // Output explicit exception.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Output inexplicit exception.
                Logging.Log(exception, false);
            }
            // Return zero to indicate failure.
            return 0;
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

    }
}
```

And here's the (important) C++ code.  While it obviously takes many more files and far more code to build a working C++ DLL, this shows the main source file (`FailingApp.cpp`) and the `CreateReference()` method we'll be using elsewhere:

```cpp
#include "stdafx.h"
#include "FailingApp.h"
#include <wtypes.h>
#include <unknwn.h>

// Constructor
CFailingApp::CFailingApp()
{
    return;
}

// Creates an invalid reference.
extern "C" __declspec(dllexport) int CreateReference()
{
    IUnknown* pUnk = NULL;
    pUnk->AddRef();
    return 0;
}
```

---

Now then, the entire purpose of the `System.AccessViolationException` is to inform us that something has gone wrong with our unmanaged code, so our example begins by importing a DLL using `DllImport()`, after which we include the method signature for the `CreateReference()` method that we're importing for use in our managed C# app:

```cs
/// <summary>
/// Create a reference from unmanaged FailingApp.dll C++ code.
/// </summary>
/// <returns>Reference.</returns>
[DllImport(@"D:\work\Airbrake.io\Exceptions\.NET\Debug\FailingApp.dll")]
private static extern int CreateReference();
```

Our unmanaged C++ code is very basic and contains just the `CreateReference()` method, inside which we have our breaking code attempting to create an invalid reference:

```cpp
// FailingApp.cpp
#include "stdafx.h"
#include "FailingApp.h"
#include <wtypes.h>
#include <unknwn.h>

// Constructor
CFailingApp::CFailingApp()
{
    return;
}

// Creates an invalid reference.
extern "C" __declspec(dllexport) int CreateReference()
{
    IUnknown* pUnk = NULL;
    pUnk->AddRef();
    return 0;
}
```

With our unmanaged method imported we can try to use it in our managed C# app.  We start with the `ReferenceTest()` method:

```cs
/// <summary>
/// Test reference creation through unmanaged code.
/// </summary>
/// <returns>Reference result.</returns>
public static int ReferenceTest()
{
    try
    {
        // Attempt to create a reference through unmanaged code (C++ DLL).
        var result = CreateReference();
        // If no exception occurred, output successful result.
        Logging.Log($"Reference successfully created at: {result}.");
        // Return result.
        return result;
    }
    catch (System.AccessViolationException exception)
    {
        // Output explicit exception.
        Logging.Log(exception);
    }
    catch (Exception exception)
    {
        // Output inexplicit exception.
        Logging.Log(exception, false);
    }
    // Return zero to indicate failure.
    return 0;
}
```

Here we're making a call to the imported `CreateReference()` method and trying to output a succesful result, while also catching any `System.AccessViolationExceptions` or global `Exceptions` that may occur.  As you might suspect, the call to our imported `CreateReference()` method immediately fails and throws a `System.AccessViolationException`.  **However**, for applications using .NET Framework 4.0 or higher, this exception _is not_ caught by either of our `catch` blocks because the exception occurs _outside_ of the memory reserved by the common language runtime.  In other words, .NET purposefully ignores exceptions that don't occur within the managed code of our C# application -- since the problem occurs within the unmanaged C++ code, we cannot catch this `System.AccessViolationException` by normal means.  Therefore, our output just shows that an uncaught `System.AccessViolationException` has occurred:

```
Exception thrown: 'System.AccessViolationException' in Airbrake.AccessViolationException.exe
```

However, there _may_ be some situations where it's beneficial to actually catch `System.AccessViolationExceptions` and similar unmanaged code exceptions directly within managed code.  This can be accomplished by applying the [`HandleProcessCorruptedStateExceptions`](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.exceptionservices.handleprocesscorruptedstateexceptionsattribute?view=netframework-4.7) attribute to all `methods` within your managed code that should be allowed to catch `System.AccessViolationExceptions` which originate from unmanaged code.  Therefore, our `ReferenceTestWithHandler()` method includes the `HandleProcessCorruptedStateExceptions` attribute, but is otherwise identical to what we saw before in `ReferenceTest()`:

```cs
/// <summary>
/// Test reference creation through unmanaged code.
/// HandleProcessCorruptedStateExceptions attribute allows CLR
/// to catch normally ignored exceptions due to unmanaged code.
/// </summary>
/// <returns>Reference result.</returns>
[HandleProcessCorruptedStateExceptions]
public static int ReferenceTestWithHandler()
{
    try
    {
        // Attempt to create a reference through unmanaged code (C++ DLL).
        var result = CreateReference();
        // If no exception occurred, output successful result.
        Logging.Log($"Reference successfully created at: {result}.");
        // Return result.
        return result;
    }
    catch (System.AccessViolationException exception)
    {
        // Output explicit exception.
        Logging.Log(exception);
    }
    catch (Exception exception)
    {
        // Output inexplicit exception.
        Logging.Log(exception, false);
    }
    // Return zero to indicate failure.
    return 0;
}
```

Now, when we execute `ReferenceTestWithHandle()` and reach the unmanaged `CreateReference()` method we still throw a `System.AccessViolationException`, but our `catch (System.AccessViolationException exception)` block is able to catch it this time and send it along to `Logging.Log()` for proper output:

```
[EXPECTED] System.AccessViolationException: Attempted to read or write protected memory. This is often an indication that other memory is corrupt.
```

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A closer look at the System.AccessViolationException in .NET, including a C#/C++ code sample and a brief review of unmanaged vs managed exceptions.