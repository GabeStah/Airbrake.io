# .NET Exceptions - System.IO.PathTooLongException

Today, on our continued journey through the in-depth [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, we'll be taking a closer look at the System.IO.PathTooLongException in .NET.  As the name implies, the `System.IO.PathTooLongException` is _usually_ thrown when a path is passed to a variety of `System.IO` namespaced methods that is too long for the current .NET version and/or operating system configuration.

In this article we'll examine the `System.IO.PathTooLongException` in more detail, starting with how the exception fits into the larger .NET Exception hierarchy.  We'll then take a brief look at what path sizes are allowed, along with some functional C# code samples that illustrate how `System.IO.PathTooLongExceptions` are commonly thrown and can be avoided, so let's get to it!

## The Technical Rundown

- All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.
- [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception) is inherited from the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) class.
- [`System.IO.IOException`](https://docs.microsoft.com/en-us/dotnet/api/system.io.ioexception?view=netframework-4.7) inherits from [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception).
- `System.IO.PathTooLongException` inherits from [`System.IO.IOException`](https://docs.microsoft.com/en-us/dotnet/api/system.io.ioexception?view=netframework-4.7).

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```cs
// <Airbrake.IO.PathTooLongException>/Program.cs
using System;
using System.IO;
using System.Reflection;
using Utility;

namespace Airbrake.IO.PathTooLongException
{
    class Program
    {
        public static string CurrentPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private static void Main(string[] args)
        {
            // Creating file with path length of 1.
            CreateFileByPathLength(1);
            // Creating file with path length of 100.
            CreateFileByPathLength(90, 'b');
            // Creating file with path length of 255.
            CreateFileByPathLength(255, 'c');
            // Creating file with path length of 259.
            CreateFileByPathLength(259, 'd');
            // Creating file with path length of 260.
            CreateFileByPathLength(260, 'e');
            // Create file with name length of 32767 (Int16 max value)
            CreateFileByPathLength(short.MaxValue, 'f');
            // Create file with name length of 32768.
            CreateFileByPathLength(short.MaxValue + 1, 'g');
        }

        /// <summary>
        /// Create file by passed name.
        /// </summary>
        /// <param name="name">Name of file.</param>
        private static void CreateFileByName(string name)
        {
            try
            {
                // Output shortened file name and actual length.
                Logging.Log($"Creating file: {name.Shorten(20)} [Name length: {name.Length}, Full path length: {Path.GetFullPath(name).Length}]");
                // Try to create file.
                var file = File.Create(name);
                // Output success message.
                Logging.Log($"{file.Name.Shorten(20)} successfully created.");
            }
            catch (System.IO.PathTooLongException exception)
            {
                // Catch expected PathTooLongExceptions.
                Logging.Log(exception);
            }
            catch (IOException exception)
            {
                // Catch expected IOExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Catch unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }

        /// <summary>
        /// Create file by specified total path length,
        /// by repeating passed character parameter for file name.
        /// </summary>
        /// <param name="length">Total path length of created file.</param>
        /// <param name="character">Character to repeat for file name.</param>
        private static void CreateFileByPathLength(int length, char character = 'a')
        {
            try
            {
                const string extension = ".txt";

                // Check if current path plus extension length is larger than total length.
                if (CurrentPath.Length + extension.Length - 1 >= length)
                {
                    Logging.Log($"Length of {length} is less than base path length, aborting.");
                    return;
                }

                // Create full path.
                // Manually creating path, rather than calling Path.GetFullPath(),
                // to avoid potential exceptions before log output can be generated.
                var path =
                    $"{CurrentPath}\\{new string(character, length - CurrentPath.Length - extension.Length - 1)}{extension}";

                // Output shortened file name and actual length.
                Logging.Log($"Attempting to create file: {path.Shorten()} [Actual length: {path.Length}]");
                // Try to create file.
                var file = File.Create(path);
                // Output success message.
                Logging.Log($"{path.Shorten()} successfully created.");
            }
            catch (System.IO.PathTooLongException exception)
            {
                // Catch expected PathTooLongExceptions.
                Logging.Log(exception);
            }
            catch (IOException exception)
            {
                // Catch expected IOExceptions.
                Logging.Log(exception);
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

One of the biggest challenges when dealing with IO during development can be handling local file creation and access.  From permissions to synchronicity to path structure and length, every platform has slightly different rules you must abide by.  Therefore, exceptions like the `System.IO.PathTooLongException` that we're looking at today must be present to handle outside cases.

Since we'll be looking at the maximum allowed path length, let's briefly go over the piece that make up a full path.  We can use the [`System.IO.Path`](https://docs.microsoft.com/en-us/dotnet/api/system.io.path?view=netframework-4.7) class to help us accomplish this through a variety of built-in methods:

```cs
// Backslash must be escaped, so '\\' is actually equal to '\'.
var path = "C:\\Airbrake\\Projects\\MyProject\\data.xml";
Logging.Log($"GetPathRoot: {Path.GetPathRoot(path)}");
Logging.Log($"GetDirectoryName: {Path.GetDirectoryName(path)}");
Logging.Log($"GetFileName: {Path.GetFileName(path)}");
Logging.Log($"GetExtension: {Path.GetExtension(path)}");
Logging.Log($"GetFullPath: {Path.GetFullPath(path)}");
```

We start with a path, then extract and output all the various components that make up the path, until we finally retrieve the full path at the end.  The result is an output that shows exactly what makes up a path to a file:

```
GetPathRoot: C:\
GetDirectoryName: C:\Airbrake\Projects\MyProject
GetFileName: data.xml
GetExtension: .xml
GetFullPath: C:\Airbrake\Projects\MyProject\data.xml
```

It's important to understand that .NET is looking at the **full path** when determining whether a path length is too long and, therefore, if a `System.IO.PathTooLongException` should be thrown.

Most of our code sample logic takes place in the `CreateFileByPathLength(int length, char character = 'a')` method:

```cs
public static string CurrentPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

/// <summary>
/// Create file by specified total path length,
/// by repeating passed character parameter for file name.
/// </summary>
/// <param name="length">Total path length of created file.</param>
/// <param name="character">Character to repeat for file name.</param>
private static void CreateFileByPathLength(int length, char character = 'a')
{
    try
    {
        const string extension = ".txt";

        // Check if current path plus extension length is larger than total length.
        if (CurrentPath.Length + extension.Length - 1 >= length)
        {
            Logging.Log($"Length of {length} is less than base path length, aborting.");
            return;
        }

        // Create full path.
        // Manually creating path, rather than calling Path.GetFullPath(),
        // to avoid potential exceptions before log output can be generated.
        var path =
            $"{CurrentPath}\\{new string(character, length - CurrentPath.Length - extension.Length - 1)}{extension}";

        // Output shortened file name and actual length.
        Logging.Log($"Attempting to create file: {path.Shorten()} [Actual length: {path.Length}]");
        // Try to create file.
        var file = File.Create(path);
        // Output success message.
        Logging.Log($"{path.Shorten()} successfully created.");
    }
    catch (System.IO.PathTooLongException exception)
    {
        // Catch expected PathTooLongExceptions.
        Logging.Log(exception);
    }
    catch (IOException exception)
    {
        // Catch expected IOExceptions.
        Logging.Log(exception);
    }
    catch (Exception exception)
    {
        // Catch unexpected Exceptions.
        Logging.Log(exception, false);
    }
}
```

Since all we care about is the length of the path, we're creating file names with a single character repeated over and over, enough times to result in the `full path length` being equal to the passed `length` parameter.  The `CurrentPath` property provides a convenient way of retrieving the full directory (without a file name) of where our executable resides, so we this to test if the `length` parameter is the same size (or smaller) than the full path length we'd need to actually create a new file.

To create a new file we manually create a new `path` by combining the `CurrentPath` with a file name that repeats the passed `character` parameter as many times as necessary to result in a path length equal to `length`.

Finally, we output the (shortened) version of the path and the _actual_ path length to the console, before attempting to create a new file at that location.  If successful, we output a message, and if creation fails, an exception is thrown and we catch and log that instead.

That's really all we need for setup, so now we can test this out by attempting to create files with full path lengths of various sizes.  At this point it's worth noting that the [official documentation](https://docs.microsoft.com/en-us/dotnet/api/system.io.pathtoolongexception?view=netframework-4.7) tells us that applications that target .NET Framework 4.6.2 or later will support paths up to `32,767` characters in length (the `System.Int16.MaxValue` value), _unless_ the operating system returns a `COR_E_PATHTOOLONG` `HRESULT` value.

The platform this code is being written and tested on is `Windows 10` and the `App.config` file shows that we're targeting a .NET Framework that exceeds 4.6.2:

```xml
<!-- App.config -->
<?xml version="1.0" encoding="utf-8" ?>
<configuration>
    <startup> 
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.7" />
    </startup>
</configuration>
```

Keeping those things in mind, let's see what happens when we test out different path length sizes:

```cs
private static void Main(string[] args)
{
    // Creating file with path length of 1.
    CreateFileByPathLength(1);
    // Creating file with path length of 90.
    CreateFileByPathLength(90, 'b');
    // Creating file with path length of 255.
    CreateFileByPathLength(255, 'c');
    // Creating file with path length of 259.
    CreateFileByPathLength(259, 'd');
    // Creating file with path length of 260.
    CreateFileByPathLength(260, 'e');
    // Create file with name length of 32767 (Int16 max value)
    CreateFileByPathLength(short.MaxValue, 'f');
    // Create file with name length of 32768.
    CreateFileByPathLength(short.MaxValue + 1, 'g');
}
```

As you can see, we're trying to cover the full spectrum by creating full paths a variety of path lengths.  Our logging setup should cover every scenario, so let's take a look at the output after running this code:

```
Length of 1 is less than base path length, aborting.

Attempting to create file: D:\work\Airbr...ug\bbbbbbb.txt [Actual length: 90]
D:\work\Airbr...ug\bbbbbbb.txt successfully created.

Attempting to create file: D:\work\Airbr...cccccccccc.txt [Actual length: 255]
D:\work\Airbr...cccccccccc.txt successfully created.

Attempting to create file: D:\work\Airbr...dddddddddd.txt [Actual length: 259]
D:\work\Airbr...dddddddddd.txt successfully created.

Attempting to create file: D:\work\Airbr...eeeeeeeeee.txt [Actual length: 260]
[EXPECTED] System.IO.DirectoryNotFoundException: Could not find a part of the path 'D:\work\Airbrake.io\Exceptions\.NET\Airbrake.IO.PathTooLongException\bin\Debug\eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee.txt'.

Attempting to create file: D:\work\Airbr...ffffffffff.txt [Actual length: 32767]
[EXPECTED] System.IO.PathTooLongException: The specified path, file name, or both are too long. The fully qualified file name must be less than 260 characters, and the directory name must be less than 248 characters.

Attempting to create file: D:\work\Airbr...gggggggggg.txt [Actual length: 32768]
[EXPECTED] System.IO.PathTooLongException: The specified path, file name, or both are too long. The fully qualified file name must be less than 260 characters, and the directory name must be less than 248 characters.
```

Each call is separated by a blank line, along with the explicit output of the full path length for each attempt, so we can see what lengths work and which fail.  As expected, a length of `1` far shorter than the base path length, so that was aborted.  `90`, `255`, and `259` all work just fine, successfully creating new files of various lengths so the full path is equal to those sizes.

The first interesting result we see is at exactly `260` length.  In spite of the documentation claim that full paths must _exceed_ `260` length to be a problem, when our path is _exactly_ `260` characters we get a [`DirectoryNotFoundException`](https://docs.microsoft.com/en-us/dotnet/api/system.io.directorynotfoundexception?view=netframework-4.7).  This seems to be some strange quirk, since obviously the directories in this call are no different than they were in the previously successful calls.

Moreover, even though we saw that the .NET Framework 4.7 version we're running on meets the 4.6.2 or higher requirement, our operating system is disallowed longer paths by default, so `32767` doesn't work, throwing a `System.IO.PathTooLongException` our way instead.  We also see that, in contradiction to the official documentation, the actual `System.IO.PathTooLongException` error message indicates that our path length must be _less than_ `260` characters, which could explain the exception when using exactly `260` characters.

One potential fix for this issue, depending on your Windows version, is to enable the use of long paths within the registry.  Usual caveat: Don't make changes to your registry unless you know what you're doing.  We take no responsibility for any damages you may incur.

That said, to enable long paths for some Windows versions you can set the `LongPathsEnabled` registry entry to `1` (true):

```
Key Name: Computer\HKEY_LOCAL_MACHINE\SYSTEM\CurrentControlSet\Control\FileSystem\LongPathsEnabled
Value Data: 1
```

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A close look at the System.IO.PathTooLongException in .NET, including C# code showing how different path lengths produce various results.