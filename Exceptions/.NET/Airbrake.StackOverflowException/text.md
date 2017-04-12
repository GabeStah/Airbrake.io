TODO: Replace `sysex` with `System.StackOverflowException`

# .NET Exceptions - System.StackOverflowException

Taking the next glorious step down the shining path of our [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, today we'll be looking over the amazing `System.OutOfMemoryException`.  As the name implies, the `System.OutOfMemoryException` typically occurs when the common language runtime (`CLR`) is unable to allocate enough memory that would be necessary to perform the current operation.

We'll spend this article seeing exactly where the `System.OutOfMemoryException` resides within the .NET exception hierarchy, while also examining a trio of possible causes that could present a `System.OutOfMemoryException` in your own code.  Let the adventure begin!

## The Technical Rundown

- All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.
- [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception) is inherited from the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) class.
- `System.OutOfMemoryException` is inherited from the [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception) class.

## When Should You Use It?


To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A detailed examination of the System.OutOfMemoryException in .NET, along with a handful of potential causes, which include C# code examples.