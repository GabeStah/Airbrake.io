# .NET Exceptions - System.IO.FileNotFoundException

Continuing along through the __.NET Exception Handling__ series, today we're going to examine the ever-popular `System.IO.FileNotFoundException`.  The `System.IO.FileNotFoundException` is common because, as the name suggests, it primarily rears its head when attempting to access a file that doesn't exist.

In this article, we'll dive into where `System.IO.FileNotFoundException` fits into the .NET exception hierarchy, look at when `System.IO.FileNotFoundExceptions` typically appear, and see how to handle them if you run into one yourself.  Let's get crackin'!

## The Technical Rundown

- All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.
- The [`System.SystemException`](https://msdn.microsoft.com/en-us/library/system.systemexception(v=vs.110).aspx) class is inherited from the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) class.
- [`System.IO.IOException`](https://msdn.microsoft.com/en-us/library/system.io.ioexception(v=vs.110).aspx) is inherited from the [`System.SystemException`](https://msdn.microsoft.com/en-us/library/system.systemexception(v=vs.110).aspx) class.
- `System.IO.FileNotFoundException` is inherited from the [`System.IO.IOException`](https://msdn.microsoft.com/en-us/library/system.io.ioexception(v=vs.110).aspx) class.

## When Should You Use It?

The most common method of accidentally throwing `System.IO.FileNotFoundException` is when manipulating a file that doesn't exist, either due to an incorrect path or otherwise.  For example, let's first take a simple code snippet where we've defined two methods, `WriteLineToFile()` and `ReadLineFromFile()`, both of which perform the function their name describes.  We've also ensured we aren't producing any exceptions by using a `try-catch` block and catching `FileNotFoundExceptions`:

```cs
using System;
using System.IO;

namespace ConsoleApplication
{
    public class FileNotFoundExceptionExample
    {

        public static void Main(string[] args)
        {
            WriteLineToFile(@"names.txt", "Jane Doe");
            ReadLineFromFile(@"names.txt");
        }

        private static void ReadLineFromFile(string fileName)
        {
            FileStream fs = null;
            string line = null;
            
            try   
            {
                // Opening file stream
                fs = new FileStream(fileName, FileMode.Open);
                using (StreamReader reader = new StreamReader(fs))
                {
                    // Read first line
                    line = reader.ReadLine();
                    Console.WriteLine($"Reading first line: {line}");
                }
            }
            catch(FileNotFoundException exception)
            {
                LogException(exception);
            }
        }

        private static void WriteLineToFile(string fileName, string line)
        {
            FileStream fs = null;
            
            try   
            {
                // Opening file stream
                fs = new FileStream(fileName, FileMode.Append);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    // Write line
                    writer.WriteLine(line);
                    Console.WriteLine($"Writing new line: {line}");
                }
            }
            catch(FileNotFoundException exception)
            {
                LogException(exception);
            }
        }

        private static void LogException(Exception exception, bool expected = true)
        {
            Console.WriteLine($"[{(expected ? "EXPECTED" : "UNEXPECTED")}] {exception.ToString()}: {exception.Message}");
        }
    }
}
```

Since we're taking advantage of the `using` code block within both our methods, in which to use our `StreamWriter` and `StreamReader` objects, respectively, we don't need to worry about calling `Flush()` or `Dispose()` methods for our reader/writer; .NET does this for us at the end of our `using` block.  Therefore, the meat is simply in creating a new `FileStream` for the passed in `fileName` (`names.txt` in this case), attaching it to our respective `reader` or `writer`, then writing a new passed in `line` value or reading the first line of the existing file.  In both cases, we output the line to the console.

As expected, our console output confirms that we did write our new line, and then in the call to `ReadLineFromFile()`, we were able to retrieve the line:

```
Writing new line: Jane Doe
Reading first line: Jane Doe
```

Now, what happens if we add a second call to `ReadLineFromFile()` within our `Main()` method, but we pass an invalid file name to it?  Let's keep everything else we have in the previous code snippet, but our `Main()` method block now looks like this:

```cs
public static void Main(string[] args)
{
    WriteLineToFile(@"names.txt", "Jane Doe");
    ReadLineFromFile(@"names.txt");
    ReadLineFromFile(@"invalid.txt");
}
```

As we might've expected, running our code now behaves fine, up until it hits that second call to `ReadLineFromFile()`.  Since the `invalid.txt` file doesn't exist, we catch a `System.IO.FileNotFoundException`:

```
[EXPECTED] System.IO.FileNotFoundException: Could not find file 'g:\dev\work\Airbrake.io\invalid.txt'.
File name: 'g:\dev\work\Airbrake.io\invalid.txt'
   at System.IO.Win32FileStream..ctor(String path, FileMode mode, FileAccess access, FileShare share, Int32 bufferSize, FileOptions options, FileStream parent)
   at System.IO.Win32FileSystem.Open(String fullPath, FileMode mode, FileAccess access, FileShare share, Int32 bufferSize, FileOptions options, FileStream parent)
   at System.IO.FileStream.Init(String path, FileMode mode, FileAccess access, FileShare share, Int32 bufferSize, FileOptions options)
   at System.IO.FileStream..ctor(String path, FileMode mode)
   at ConsoleApplication.FileNotFoundExceptionExample.ReadLineFromFile(String fileName) in g:\dev\work\Airbrake.io\Exceptions\.NET\System.IO.FileNotFoundException\System.IO.FileNotFoundException.cs:line 24: Could not find file 'g:\dev\work\Airbrake.io\invalid.txt'.
```

As it happens, the solution in this case is rather simple.  While arguably not a smart practice in production code, we can change the `System.IO.FileMode` in which our `FileStream` opens our file within the `ReadLineFromFile()` method.  Originally, we were using `FileMode.Open`, which simply attempts to open the specified file path, and if it doesn't exist, we throw a `System.IO.FileNotFoundException`.  Instead, if we want to avoid a `System.IO.FileNotFoundException` in this case, we can change it to `FileMode.OpenOrCreate`, like so:

```cs
fs = new FileStream(fileName, FileMode.OpenOrCreate);
```

While this isn't the best long-term solution (we still produce a console output with no line text to pass to it), at the least we're no longer producing our `System.IO.FileNotFoundException`.

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A basic introduction to the System.IO.FileNotFoundException in .NET, where it sits within the .NET exception hierarchy, and how to handle missing file issues.

---

__SOURCES__

- https://msdn.microsoft.com/en-us/library/system.exception(v=vs.110).aspx
