# .NET Exceptions - System.BadImageFormatException

Today, as we continue along through our [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, we're going to take a look at the `System.BadImageFormatException`.  `System.BadImageFormatException` has nothing to do with `gifs` or `jpgs`, but instead, occurs when a .NET application attempts to load a dynamic link library (`.dll`) or executable (`.exe`) that doesn't match the proper format that the current common language runtime (`CLR`) expects.

Throughout this article we'll see exactly where `System.BadImageFormatException` sits within the .NET exception hierarchy and look at a few potential causes of `System.BadImageFormatExceptions`, so let's get to it!

## The Technical Rundown

- All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.
- [`System.SystemException`](https://msdn.microsoft.com/en-us/library/system.systemexception(v=vs.110).aspx) is inherited from the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) class.
- `System.BadImageFormatException` is inherited from the [`System.SystemException`](https://msdn.microsoft.com/en-us/library/system.systemexception(v=vs.110).aspx) class.

## When Should You Use It?

As previously mentioned, a `System.BadImageFormatException` occurs in very specific circumstances: When .NET attempts to make use of a `.dll` or `.exe` that is, in someway, incompatible with the current common language runtime.  What qualifies as an "incompatible common language runtime" can vary somewhat, but typically this means either the .NET version (`1.1`, `2.0`, etc) __OR__ the CPU type (`32-bit` vs `64-bit`) of the various compiled assemblies do not match.

Ultimately, `System.BadImageFormatExceptions` are an indication of incompatible versioning.  For many modern software applications, major versions of often include breaking compatibility issues, preventing backward compatibility with some aspects of previous versions.  .NET assemblies (`.dlls` or `.exes`) are much the same, and attempting to make use of two different types of assemblies that contain incompatibilities will often generate a `System.BadImageFormatException`.

To illustrate, we'll go through a few different examples.  I've included the full code example below for reference, after which we'll explore the specifics in more detail:

```cs
using System;
using System.Reflection;
using Utility;

namespace Airbrake.BadImageFormatException
{
    class Program
    {
        static void Main(string[] args)
        {
            LoadingNonDotNetLibraryExample();
            Logging.Log("-----------------");
            DifferingCPUExample();
            Logging.Log("-----------------");
            OldDotNetExample();
        }

        private static void LoadingNonDotNetLibraryExample()
        {
            try
            {
                // Generate path to notepad.exe.
                string filePath = Environment.ExpandEnvironmentVariables("%windir%") + @"\System32\notepad.exe";
                Assembly assem = Assembly.LoadFile(filePath);
            }
            catch (System.BadImageFormatException exception)
            {
                Logging.Log(exception);
            }
        }

        private static void DifferingCPUExample()
        {
            try
            {
                // Load Utility.dll, a 64-bit assembly.
                Assembly assem = Assembly.LoadFrom(@".\Utility.dll");
                Logging.Log(assem.ToString());
            }
            catch (System.BadImageFormatException exception)
            {
                Logging.Log(exception);
            }
        }

        private static void OldDotNetExample()
        {
            try
            {
                // Load Author-1.1.dll (compiled in .NET 1.1).
                Assembly assem = Assembly.LoadFrom(@".\Author-1.1.dll");
                Logging.Log(assem.ToString());
            }
            catch (System.BadImageFormatException exception)
            {
                Logging.Log(exception);
            }
        }
    }
}

using System;
using System.Diagnostics;

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
        public static void Log(object value)
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
    }
}
```

The first (and arguably most common) way to raise a `System.BadImageFormatException` is when attempting to make use of an `unmanaged assembly`, as if it were an assembly created with the `.NET Framework`.  `Unmanaged assemblies` are assemblies that have been generated from code that _is not_ handled and compiled by the common language runtime of .NET.  This includes many older applications and assemblies, particularly those created for `32-bit` systems.

As an example, here we're trying to load an `unmanaged assembly` -- specifically the well-known `notepad.exe` assembly that resides in our `Windows/System32` directory:

```cs
private static void LoadingNonDotNetLibraryExample()
{
    try
    {
        // Generate path to notepad.exe.
        string filePath = Environment.ExpandEnvironmentVariables("%windir%") + @"\System32\notepad.exe";
        Assembly assem = Assembly.LoadFile(filePath);
    }
    catch (System.BadImageFormatException exception)
    {
        Logging.Log(exception);
    }
}
```

.NET is not to pleased with this, since `notepad.exe` is not `managed` (wasn't compiled using .NET), and thus throws a `System.BadImageFormatException`:

```
[EXPECTED] System.BadImageFormatException: The module was expected to contain an assembly manifest. (Exception from HRESULT: 0x80131018)
   at System.Reflection.RuntimeAssembly.nLoadFile(String path, Evidence evidence)
   at System.Reflection.Assembly.LoadFile(String path)
   at Airbrake.BadImageFormatException.Program.LoadingNonDotNetLibraryExample() in D:\work\Airbrake.io\Exceptions\.NET\Airbrake.BadImageFormatException\Program.cs:line 26: The module was expected to contain an assembly manifest. (Exception from HRESULT: 0x80131018)
```

Another way we might throw a `System.BadImageFormatException` is when trying to load an assembly that was compiled using a _different CPU_ type than what we're executing .NET on currently.

For example, throughout many of our code snippets we've been making use of our simple `Utility` namespace that contains the `Logging` class, making it a bit easier to output log information during debugging and testing.  By default, our `Utility.dll` is compiled as a `64-bit` assembly.  However, if we switch our current `CPU configuration` to execute as an `x86` (`32-bit`) CPU, we run into some trouble:

```cs
private static void DifferingCPUExample()
{
    try
    {
        // Generate path to Utility.dll, a 64-bit assembly.
        Assembly assem = Assembly.LoadFrom(@".\Utility.dll");
        Logging.Log(assem.ToString());
    }
    catch (System.BadImageFormatException exception)
    {
        Logging.Log(exception);
    }
}
```

When attempting to load our `Utility.dll` assembly, which is `64-bit`, while compiling our current code as `32-bit`, .NET throws a `System.BadImageFormatException`, informing us these formats are mismatched:

```
[EXPECTED] System.BadImageFormatException: An attempt was made to load a program with an incorrect format. (Exception from HRESULT: 0x8007000B)
   at System.Reflection.RuntimeAssembly.nLoadFile(String path, Evidence evidence)
   at System.Reflection.Assembly.LoadFile(String path)
   at Airbrake.BadImageFormatException.Program.DifferingCPUExample() in D:\work\Airbrake.io\Exceptions\.NET\Airbrake.BadImageFormatException\Program.cs:line 40: An attempt was made to load a program with an incorrect format. (Exception from HRESULT: 0x8007000B)
```

Lastly, we also run into trouble if we attempt to load an assembly that was compiled using a _much_ older version of .NET (such as `.NET 1.1`):

```cs
private static void OldDotNetExample()
{
    try
    {
        // Load Author-1.1.dll (compiled in .NET 1.1).
        Assembly assem = Assembly.LoadFrom(@".\Author-1.1.dll");
        Logging.Log(assem.ToString());
    }
    catch (System.BadImageFormatException exception)
    {
        Logging.Log(exception);
    }
}
```

In cases like the one above, the `System.BadImageFormatException` will often be thrown at compile-time, rather than at runtime, since the .NET compiler recognizes the incompatibility before attempting to execute any of the code in the first place.

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A close look at the System.BadImageFormatException in .NET and assorted scenarios in which it might occur, including C# code examples.