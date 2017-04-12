# .NET Exceptions - System.StackOverflowException

Today, as we continue through our [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, we'll be taking a closer look at the `System.StackOverflowException`.  As indicated by the name, the `System.StackOverflowException` is thrown when a `stack overflow` occurs within .NET execution.

Throughout this article we'll explore the `System.StackOverflowException` in more detail, including where it sits within the .NET exception hierarchy, along with a few code examples to illustrate some potential causes of `System.StackOverflowExceptions`.  Let's get to it!

## The Technical Rundown

- All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.
- [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception) is inherited from the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) class.
- `System.StackOverflowException` is inherited from the [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception) class.

## When Should You Use It?

Before we get into the specifics of how .NET deals with the `System.StackOverflowException`, we should first briefly review what a `stack overflow` indicates when it pops up during program execution.  In most cases, computer programs are allocated a certain range of `memory addresses` upon execution.  These addresses are simply pointers to `bytes` of data (memory) that the application can use.  Thus, the `address space` of an executing application contains a specific quantity and range of `memory addresses` which can be safely used by the application to store and manipulate memory.

Since available memory is finite, the `address space` assigned to our .NET application is limited to certain bounds.  The actual _size_ of this `address space` depends on many factors, but the result is that our application can only allocate and use a certain amount of memory before it runs out.  In most cases, the common language runtime (`CLR`) will constantly free up unused memory from the `stack`, to be used by future processing.

However, in some cases, the application's code may attempt to perform tasks that require more space (more memory) in the `address space` than was allocated to the program in the first place.  If this happens, the application will generate a `stack overflow` error which, in the case of .NET, throws a `System.StackOverflowException`.

To illustrate how a `System.StackOverflowException` might appear in actual code, let's take a look at a simple example:

```cs
private const int OUTPUT_FREQUENCY = 1000;
static int counter = 0;

static void Main(string[] args)
{
    StackOverflowExample();
    Logging.Log("-----------------");
    // Reset counter.
    counter = 0;
    PreventativeExample();

}

private static void StackOverflowExample()
{
    try
    {
        // Iterate counter.
        counter++;

        // Output counter value every so often.
        if (counter % OUTPUT_FREQUENCY == 0)
        {
            Logging.Log($"Current counter: {counter}.");
        }

        // Recursively call self method.
        StackOverflowExample();
    }
    catch (System.StackOverflowException exception)
    {
        Logging.Log(exception);
    }
}
```

Here we have a simple `StackOverflowExample()` method in which we're iterating our `counter` value.  We then **recursively** call the self method (`StackOverflowExample()`), forcing execution to repeat our method call ad nauseam.  Just to confirm we're performing an iteration every time, we also output our `counter` value every `1000` iterations.

Eventually, this infinite recursion will cause a problem; specifically, it will throw a `System.StackOverflowException`:

```
Current counter: 1000.
Current counter: 2000.
Current counter: 3000.
Current counter: 4000.
Current counter: 5000.
Current counter: 6000.
Current counter: 7000.
Current counter: 8000.
Current counter: 9000.
Current counter: 10000.
Current counter: 11000.
Process is terminated due to StackOverflowException.
```

As it happens, newer versions of .NET do not allow for `System.StackOverflowExceptions` to be caught in the typical `try-catch` block.  Instead, the process terminates itself by default.  Execution will `break` if a `System.StackOverflowException` occurs during debugging within Visual Studio, for example.  But, in normal production applications, a `System.StackOverflowException` simply causes a fatal crash, which is obviously bad news.

Therefore, the suggested way to handle potential `System.StackOverflowExceptions` is to write code that prevents them in the first place.  In the example above with an infinite recursion, we can add a simple stop-gap measure that checks how many times our recursion has occurred, and back out of the process after a set number of times:

```cs
private const int COUNTER_MAX = 10000;
private const int OUTPUT_FREQUENCY = 1000;
static int counter = 0;

static void Main(string[] args)
{
    PreventativeExample();
}

private static void PreventativeExample()
{
    try
    {
        // Iterate counter.
        counter++;

        // Output counter value every so often.
        if (counter % OUTPUT_FREQUENCY == 0)
        {
            Logging.Log($"Current counter: {counter}.");
        }

        // Check if counter has reached maximum value; if not, allow recursion.
        if (counter <= COUNTER_MAX)
        {
            // Recursively call self method.
            PreventativeExample();
        }
        else
        {
            Logging.Log("Recursion halted.");
        }
    }
    catch (System.StackOverflowException exception)
    {
        Logging.Log(exception);
    }
}
```

For the `PrevantativeExample()` method we've added a new `COUNTER_MAX` constant with a value of `10,000`.  Within our method code, we only allow recursion to occur if `counter <= COUNTER_MAX`.  Once our `counter` exceeds `COUNTER_MAX`, we halt recursion:

```
Current counter: 1000.
Current counter: 2000.
Current counter: 3000.
Current counter: 4000.
Current counter: 5000.
Current counter: 6000.
Current counter: 7000.
Current counter: 8000.
Current counter: 9000.
Current counter: 10000.
Recursion halted.
```

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A detailed look at the System.StackOverflowException in .NET, along with an infinite recursion example which includes C# code.