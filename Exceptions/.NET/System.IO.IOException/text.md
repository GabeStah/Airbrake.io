Taking a short left turn here, a quick right turn there, we continue down the path of our __.NET Exception Handling__ series.  Today we're taking a closer look at the `System.IO.IOException`.  The `System.IO.IOException` is one of the most common exceptions that might pop up in .NET, but is itself actually a base class.

In this article, we'll dig deeper into where `System.IO.IOException` resides within the .NET exception hierarchy, examine how these errors might appear, and also look at how to deal with them.  Let's get to it!

## The Technical Rundown

- All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.
- The [`System.SystemException`](https://msdn.microsoft.com/en-us/library/system.systemexception(v=vs.110).aspx) class is inherited from the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) class.
- `System.IO.IOException` is a inherited from the [`System.SystemException`](https://msdn.microsoft.com/en-us/library/system.systemexception(v=vs.110).aspx) class, and is a parent class to a number of `IO` exceptions as well.

## When Should You Use It?

Digging into the `System.IO.IOException` is fairly straightforward, but the key thing to recognize is that the `System.IO.IOException` class is rarely thrown by default .NET code.  More often, one of the child classes that inherits from `System.IO.IOException` will be thrown instead, such as the [`System.IO.DirectoryNotFoundException`](https://msdn.microsoft.com/en-us/library/system.io.directorynotfoundexception(v=vs.110).aspx) or the [`System.IO.EndOfStreamException`](https://msdn.microsoft.com/en-us/library/system.io.endofstreamexception(v=vs.110).aspx).

Therefore, to `catch` a `System.IO.IOException` in normal circumstances, we're most likely going to be creating a _different_ exception type.  For example, here we're creating a `System.IO.DirectoryNotFoundException` by trying to call the `Directory.SetCurrentDirectory()` method with a `directory` argument that is invalid (doesn't exist on the system):

```cs
using System;
using System.IO;

namespace ConsoleApplication
{
    public class Program
    {

        public static void Main(string[] args)
        {
            try
            {
                // Specify our directory
                string dir = @"g:\dev\missing";

                // Set the current directory
                Directory.SetCurrentDirectory(dir);
            }
            catch (IOException exception)
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

The key lines are commented, but effectively we're requesting a directory that doesn't exist.  When that failure occurs and an exception is thrown, we use `try-catch` block, specifying that the explicit exception type we wish to `catch` is an `IOException`.  From there, we use our basic `LogException()` method to help us with formatting the exception output message, along with the name of the exception class, to confirm whether it's the explicit class we expected or not.

Sure enough, we attempted to `catch` any inherited class of `IOException`, and we caught the `System.IO.DirectoryNotFoundException`, as shown in our output:

```
[EXPECTED] System.IO.DirectoryNotFoundException: Could not find a part of the path 'g:\dev\missing'.
   at System.IO.Win32FileSystem.SetCurrentDirectory(String fullPath)
   at System.IO.Directory.SetCurrentDirectory(String path)
   at ConsoleApplication.Program.Main(String[] args) in g:\dev\work\Airbrake.io\Exceptions\.NET\System.IO.IOException\Program.cs:line 17: Could not find a part of the path 'g:\dev\missing'.
```

Remember, throughout .NET, class inheritance is used liberally, so that we can perform basic tasks like this one.  Rather than explicitly `catching` only the `System.IO.DirectoryNotFoundException` that we actually threw here, we were able to perform a broader `catch` by specifying the `IOException` base class instead.  Thus, our `catch` block will grab **all** children of `System.IO.IOException`.

That's not to suggest that `System.IO.IOExceptions` can never be used directly.  It may be useful to throw your own exceptions at times, and just like any type of exception in .NET, you can explicitly raise your own if you choose.  For example, here we are explicitly throwing a new `System.IO.IOException`, which we can then `catch` and examine:

```cs
using System;
using System.IO;

namespace ConsoleApplication
{
    public class Program
    {

        public static void Main(string[] args)
        {
            try
            {
                // Throw an IOException
                throw new IOException("Uh oh, something broke!");
            }
            catch (IOException exception)
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

As expected, now our caught exception class is, in fact, `System.IO.IOException`:

```
[EXPECTED] System.IO.IOException: Uh oh, something broke!
   at ConsoleApplication.Program.Main(String[] args) in g:\dev\work\Airbrake.io\Exceptions\.NET\System.IO.IOException\Program.cs:line 13: Uh oh, something broke!
```

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A basic introduction to the System.IO.IOException in .NET, where it sits within the .NET exception hierarchy, and how to handle IOExceptions.

---

__SOURCES__

- https://msdn.microsoft.com/en-us/library/system.exception(v=vs.110).aspx
