# .NET Exceptions - System.OutOfMemoryException

Taking the next glorious step down the shining path of our [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, today we'll be looking over the amazing `System.OutOfMemoryException`.  As the name implies, the `System.OutOfMemoryException` typically occurs when the common language runtime (`CLR`) is unable to allocate enough memory that would be necessary to perform the current operation.

We'll spend this article seeing exactly where the `System.OutOfMemoryException` resides within the .NET exception hierarchy, while also examining a trio of possible causes that could present a `System.OutOfMemoryException` in your own code.  Let the adventure begin!

## The Technical Rundown

- All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.
- [`System.SystemException`](https://msdn.microsoft.com/en-us/library/system.systemexception(v=vs.110).aspx) is inherited from the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) class.
- `System.OutOfMemoryException` is inherited from the [`System.SystemException`](https://msdn.microsoft.com/en-us/library/system.systemexception(v=vs.110).aspx) class.

## When Should You Use It?

In spite of the name, the most likely cause of a `System.OutOfMemoryException` is not _technically_ due to a lack of memory.  Instead, a `System.OutOfMemoryException` can occur when attempting to increase the length of an instance of the [`StringBuilder`](https://msdn.microsoft.com/en-us/library/system.text.stringbuilder(v=vs.110).aspx) class, beyond what is specified by its current [`MaxCapacity`](https://msdn.microsoft.com/en-us/library/system.text.stringbuilder.maxcapacity(v=vs.110).aspx) property.

To illustrate, here we have some simple code that generates a new `StringBuilder` instance called `builder`:

```cs
public static void StringBuilderExample()
{
    try
    {
        string firstName = "Bob";
        string lastName = "Smith";
        // Initialize with allocated length (MaxCapacity) equal to initial value length.
        StringBuilder builder = new StringBuilder(firstName.Length, firstName.Length);
        Logging.Log($"builder.MaxCapacity: {builder.MaxCapacity}");
        // Append initial value.
        builder.Append(firstName);
        // Attempt to insert additional value to builder already at MaxCapacity character count.
        builder.Insert(value: lastName,
                       index: firstName.Length - 1,
                       count: 1);
    }
    catch (System.OutOfMemoryException e)
    {
        Logging.Log(e, true);
    }
}
```

As indicated by the comments, we're using a particular override of `StringBuilder`, in this case the [`StringBuilder(Int32, Int32)`](https://msdn.microsoft.com/en-us/library/hbb08bby(v=vs.110).aspx) override, which defines the `capacity` and `MaxCapacity` property during initialization.  In this case, both are set to `3`, the length of our `firstName` string.

We then `.Append` that initial value to our `builder`, after which we attempt to `.Insert` our second value at the end of the existing string index.  However, because we've already set the `MaxCapacity` value to `3`, and we've appended `3` characters, we've used up all allocated memory for our `StringBuilder` instance.  Thus, our `.Insert` attempt throws a `System.OutOfMemoryException`:

```
builder.MaxCapacity: 3
[EXPECTED] System.OutOfMemoryException: Insufficient memory to continue the execution of the program.
```

In this case, the issue is that we've told the `CLR` how much memory to allocate using the `MaxCapacity` property, which was assigned by using the `StringBUilder(Int32, Int32)` override.  The simplest solution is to use a different override, one that doesn't assign the `MaxCapacity` property.  This will cause the default value to be set, which is [`Int32.MaxValue`](https://msdn.microsoft.com/en-us/library/system.int32.maxvalue(v=vs.110).aspx) (i.e. roughly 2.15 billion).

Another potential cause of a `System.OutOfMemoryException` is, of course, actually running out of memory during execution.  This could be due to repeatedly concatenating large strings, executing as a 32-bit process (which can only allocate a maximum of 2GB of memory), or attempting to retain massive data sets in memory during execution.  We'll use the latter issue in our example snippet below:

```cs
private static void LargeDataSetExample()
{
    Random random = new Random();
    List<Double> list = new List<Double>();
    int maximum = 200000000;
    int split = 10000000;
    try
    {
        for (int count = 1; count <= maximum; count++)
        {
            list.Add(random.NextDouble());
            if (count % split == 0)
            {
                Logging.Log($"Total item count: {count}.");
            }
        }
    }
    catch (System.OutOfMemoryException e)
    {
        Logging.Log(e, true);
    }
}
```

This code serves no real functional purpose, but instead just illustrates one possible way of manipulating a huge data set within memory, without using any form of [`chunking`](https://en.wikipedia.org/wiki/Chunking_(computing)) to reduce the allocated memory footprint of the application.  In this case, we're just looping some `200 million` times and adding a random number to our `list` of `Doubles` every time.  Every `10 million` loops we also output our current total.

The result is that, eventually, the system cannot handle the amount of memory being used, so a `System.OutOfMemoryException` is thrown:

```
Total item count: 10000000.
Total item count: 20000000.
Total item count: 30000000.
Total item count: 40000000.
Total item count: 50000000.
Total item count: 60000000.
Total item count: 70000000.
Total item count: 80000000.
Total item count: 90000000.
Total item count: 100000000.
Total item count: 110000000.
Total item count: 120000000.
Total item count: 130000000.
[EXPECTED] System.OutOfMemoryException: Exception of type 'System.OutOfMemoryException' was thrown.
```

The final snippet we'll look at today is taken from the [`official documentation`](https://msdn.microsoft.com/en-us/library/system.outofmemoryexception(v=vs.110).aspx).  However, this code isn't producing a `System.OutOfMemoryException` due to a memory issue, as with our other examples.  Instead, this snippet illustrates how `System.OutOfMemoryExceptions` should be properly handled:

```cs
public static void ThrowExample()
{
    try
    {
        // Outer block to handle any unexpected exceptions.
        try
        {
            string s = "This";
            s = s.Insert(2, "is ");

            // Throw an OutOfMemoryException exception.
            throw new System.OutOfMemoryException();
        }
        catch (ArgumentException)
        {
            Logging.Log("ArgumentException in String.Insert");
        }

        // Execute program logic.
    }
    catch (System.OutOfMemoryException e)
    {
        Logging.Log("Terminating application unexpectedly...");
        Environment.FailFast(String.Format("Out of Memory: {0}",
                                            e.Message));
    }
}
```

Since a `System.OutOfMemoryException` indicates a _catastrophic_ error within the system, it's recommended that anywhere a potential `System.OutOfMemoryException` could occur be passed to the [`Environment.FailFast`](https://msdn.microsoft.com/en-us/library/ms131100(v=vs.110).aspx) method, which terminates the process and writes a message to the `Windows Log`.  Sure enough, executing the snippet above generates a log entry in the `Windows Log`, which we can see using the `Event Viewer` application:

```
Application: Airbrake.OutOfMemoryException.exe
Framework Version: v4.0.30319
Description: The application requested process termination through System.Environment.FailFast(string message).
Message: Out of Memory: Insufficient memory to continue the execution of the program.
Stack:
   at System.Environment.FailFast(System.String)
   at Airbrake.OutOfMemoryException.Program.ThrowExample()
   at Airbrake.OutOfMemoryException.Program.Main(System.String[])
```

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A detailed examination of the System.OutOfMemoryException in .NET, along with a handful of potential causes that include C# code examples.