# .NET Exceptions - System.Threading.ThreadAbortException

Making our way through our detailed [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, today we'll dig into the fun System.Threading.ThreadAbortException.  A `System.Threading.ThreadAbortException` is thrown when the [`Abort()`](https://docs.microsoft.com/en-us/dotnet/api/system.threading.thread.abort?view=netframework-4.7) method is invoked on a `Thread` instance.

In this article we'll go over the `ThreadAbortException` in more detail, examining where it resides in the .NET exception hierarchy, along with some functional sample code illustrating how `System.Threading.ThreadAbortExceptions` are commonly thrown, so let's get to it!

## The Technical Rundown

- All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.
- [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception) is inherited from the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) class.
- `System.Threading.ThreadAbortException` inherits directly from [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception).

## When Should You Use It?

To understand the purpose of the `System.Threading.ThreadAbortException` we first need to discuss threading in .NET and, specifically, how threads can be terminated.  One simple technique of halting a thread is to call the [`Abort()`](https://docs.microsoft.com/en-us/dotnet/api/system.threading.thread.abort?view=netframework-4.7) method on the thread in question.  Doing so will typically throw a `ThreadAbortException` and will then attempt to terminate the thread.

For example, here we create a new thread and pass it a lambda delegate method that just sleeps for one second before completing.  After starting the thread we check the `ThreadState`, then call the `Abort()` method, which throws a `System.Threading.ThreadAbortException` and also causes the `ThreadState` to change to `Aborted`:

```cs
var thread = new Thread(
    () => Thread.Sleep(1000)
);

thread.Start();
// [00:00:00.0009701] Running
Logging.Log(thread.ThreadState, Logging.OutputType.Timestamp);

// Exception thrown: 'System.Threading.ThreadAbortException' in mscorlib.dll
thread.Abort(); 
// [00:00:00.0206406] Aborted
Logging.Log(thread.ThreadState, Logging.OutputType.Timestamp);
```

That said, invoking `Abort()` on a thread _does not_ guarantee it will be terminated.  One such scenario is if the thread contains a `finally` block with extensive code.  Calling `Abort()` triggers such `finally` blocks, so an issue in such code could cause a lengthy delay in actual thread termination (or may never abort the thread at all).

One safety precaution is to call the [`Join`](https://docs.microsoft.com/en-us/dotnet/api/system.threading.thread.join?view=netframework-4.7) method on the thread _after_ `Abort()` is called.  This temporarily blocks the _calling thread_ (the thread in which the method was called) until the thread instance associated with the `Join()` has finished.  Therefore, we can add `thread.Join()` to the end of our example above to ensure that the calling thread (`Main` thread, in most cases) is blocked until it has finished any final processing:

```cs
var thread = new Thread(
    () => Thread.Sleep(1000)
);

thread.Start();
// [00:00:00.0009701] Running
Logging.Log(thread.ThreadState, Logging.OutputType.Timestamp);

// Exception thrown: 'System.Threading.ThreadAbortException' in mscorlib.dll
thread.Abort(); 
// [00:00:00.0206406] Aborted
Logging.Log(thread.ThreadState, Logging.OutputType.Timestamp);

thread.Join();
// [00:00:00.0208147] Aborted
Logging.Log(thread.ThreadState, Logging.OutputType.Timestamp);
```

As you may notice by the timestamps, the invocation of `Join()` takes place almost instantaneously following the call to `Abort()`.  This is because, even though our `ThreadStart` delegation method attempts to delay processing for one second, `Abort()` completes instantly because there's no `finally` block holding things up.

To see how integrating a `finally` block might work we have the `BasicThreadTester` class.  Its constructor method creates a new thread and sets its `ThreadStart` method to `PerformSuspension`.  The `PerformSuspension()` method outputs a starting message, then contains a `finally` block where we've stuck our `Thread.Sleep(1000)` delay:

```cs
internal class BasicThreadTester
{
    internal BasicThreadTester()
    {
        // Create secondary thread and set name.
        var thread = new Thread(PerformSuspension)
            { Name = "Secondary" };
        
        // Start thread.
        thread.Start();
        Logging.Log(thread.ThreadState, Logging.OutputType.Timestamp);

        // Sleep one millisecond so process can begin.
        Thread.Sleep(1);

        // Abort thread.
        thread.Abort();
        Logging.Log(thread.ThreadState, Logging.OutputType.Timestamp);

        // Join new thread with main thread.
        thread.Join();
        Logging.Log($"Joining {Thread.CurrentThread.Name} and {thread.Name} threads.", Logging.OutputType.Timestamp);
    }

    internal void PerformSuspension()
    {
        try
        {
            Logging.Log($"{Thread.CurrentThread.Name} thread started.", Logging.OutputType.Timestamp);
        }
        finally
        {
            // Delay for one second after abort.
            Thread.Sleep(1000);
        }
    }
}
```

Instantiating this class results in the following output, which shows that the call to `Abort()` was properly delayed that extra one second necessary to execute the `finally` block code, before the thread aborted and then joined with the `Main` thread:

```
[00:00:00.0010588] Running

[00:00:00.0013243] Secondary thread started.
The thread 0x15390 has exited with code 0 (0x0).
[00:00:01.0565247] Aborted

[00:00:01.0567066] Joining Main and Secondary threads.
```

---

Now that we have a basic understanding of how aborting a thread works, let's see how a `System.Threading.ThreadAbortException` might come up.  We'll start with the full code sample below, then break it down afterward to see what's going on:

```cs
internal class AdvancedThreadTester
{
    internal AdvancedThreadTester()
    {
        // Instantiate thread manager.
        var bookManager = new BookManager();

        // Add Books to Singleton instance List.
        bookManager.Singleton.Add(new Book("Magician", "Raymond E. Feist", 681));
        bookManager.Singleton.Add(new Book("The Revenant", "Michael Punke", 272));
        bookManager.Singleton.Add(new Book("The Final Empire", "Brandon Sanderson", 541));
        bookManager.Singleton.Add(new Book("The Code Book", "Simon Singh", 412));
        bookManager.Singleton.Add(new Book("Ship of Magic", "Robin Hobb", 880));

        // Create secondary thread and assign DestroyBooks() as delegate.
        var thread = new Thread(
            () => bookManager.DestroyBooks())
            { Name = "Secondary" };

        // Start secondary thread.
        thread.Start();
        Logging.Log($"{thread.Name} thread started.", Logging.OutputType.Timestamp);

        // Count Books in main thread
        bookManager.CountBooks();

        // Abort secondary thread.
        thread.Abort();
        Logging.Log($"{thread.Name} thread aborted.", Logging.OutputType.Timestamp);

        // Join main and secondary thread.
        thread.Join();
        Logging.Log($"Joining {Thread.CurrentThread.Name} and {thread.Name} threads.", Logging.OutputType.Timestamp);
    }
}

internal class BookManager
{
    public Singleton<Book> Singleton = Singleton<Book>.Instance;

    /// <summary>
    /// Destroy all Books in Singleton collection and output each.
    /// </summary>
    /// <param name="delay">Delay between destruction.</param>
    internal void DestroyBooks(int delay = 1000)
    {
        try
        {
            // Check if any values remain.
            while (Singleton.GetValues().IsAny())
            {
                // Delay processing.
                Thread.Sleep(delay);
                // Pop (remove) value and output info.
                Logging.Log($"Book has been destroyed: {Singleton.Pop().Value}, on {Thread.CurrentThread.Name} thread.", Logging.OutputType.Timestamp);
            }
        }
        catch (System.Threading.ThreadAbortException exception)
        {
            // Output expected ThreadAbortException.
            Logging.Log(exception, true, Logging.OutputType.Timestamp);
        }
        catch (Exception exception)
        {
            // Output unexpected Exceptions.
            Logging.Log(exception, false, Logging.OutputType.Timestamp);
        }
    }

    /// <summary>
    /// Count all Books in Singleton collection during loop.
    /// </summary>
    /// <param name="iterations">Number of iteration loops to perform count.</param>
    /// <param name="delay">Delay between counts.</param>
    internal void CountBooks(int iterations = 5, int delay = 900)
    {
        try
        {
            // Loop once per iteration.
            for (var i = 0; i < iterations; i++)
            {
                // Delay processing.
                Thread.Sleep(delay);
                // Count books and output.
                Logging.Log($"Book count: {Singleton.GetValues().Count}, on {Thread.CurrentThread.Name} thread.", Logging.OutputType.Timestamp);
            }
        }
        catch (System.Threading.ThreadAbortException exception)
        {
            // Output expected ThreadAbortException.
            Logging.Log(exception, true, Logging.OutputType.Timestamp);
        }
        catch (Exception exception)
        {
            // Output unexpected Exceptions.
            Logging.Log(exception, false, Logging.OutputType.Timestamp);
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
        public static void LineSeparator(int length = 40)
        {
            Debug.WriteLine(new string('-', length));
        }
    }
}

using System.Collections.Generic;

namespace Utility
{
    /// <summary>
    /// Generic Singleton class used to store a List of type T.
    /// </summary>
    /// <typeparam name="T">Type of objects to store.</typeparam>
    public sealed class Singleton<T>
    {
        /// <summary>
        /// Store the singleton instance of this.
        /// </summary>
        public static Singleton<T> Instance { get; } = new Singleton<T>();

        /// <summary>
        /// List of values.
        /// </summary>
        private List<T> Values { get; } = new List<T>();

        static Singleton() { }

        private Singleton() { }

        /// <summary>
        /// Add value to Values List.
        /// </summary>
        /// <param name="value">Value to add.</param>
        public void Add(T value)
        {
            Values.Add(value);
        }

        /// <summary>
        /// Get the current Values List.
        /// </summary>
        /// <returns>Current Values List.</returns>
        public List<T> GetValues()
        {
            return Values;
        }

        /// <summary>
        /// Remove last value and return tuple of index and value.
        /// </summary>
        /// <returns>Tuple of index and value that was removed.</returns>
        public (int Index, object Value) Pop()
        {
            if (!Values.IsAny()) return (-1, null);
            var index = Values.Count - 1;
            var value = Values[index];
            RemoveAt(index);
            return (index, value);
        }

        /// <summary>
        /// Remove value from Values List.
        /// </summary>
        /// <param name="value">Value to remove.</param>
        public void Remove(T value)
        {
            Values.Remove(value);
        }

        /// <summary>
        /// Remove value, via index, from Values List.
        /// </summary>
        /// <param name="index">Index to remove.</param>
        public void RemoveAt(int index)
        {
            Values.RemoveAt(index);
        }
    }
}
```

---

Our basic goal here is to use a singleton pattern -- which you can learn all about in our [`Singleton Creation Design Pattern`](https://airbrake.io/blog/design-patterns/creational-design-patterns-singleton) tutorial -- to share a collection of `Books` between two threads.  One thread will be actively destroying elements in the collection, while the other thread is outputting the changing `Book` count in the collection.  While this is a simple example, it illustrates a basic structure that is commonly used to handle shared resources across multi-threaded applications.

We won't go into much detail of the `Utility.Singleton<T>` or the `Utility.Logging` classes.  The former just maintains a singleton instance of itself and implements some methods for manipulating an underlying collection of type `T` objects, while the later is used to output to the console.

Instead, we begin with the `BookManager` class, which instantiates a `Singleton<Book>` instance and implements two basic methods, `DestroyBooks(int)` and `CountBooks(int, int)`:

```cs
internal class BookManager
{
    public Singleton<Book> Singleton = Singleton<Book>.Instance;

    /// <summary>
    /// Destroy all Books in Singleton and output each.
    /// </summary>
    /// <param name="delay">Delay between destruction.</param>
    internal void DestroyBooks(int delay = 1000)
    {
        try
        {
            // Check if any values remain.
            while (Singleton.GetValues().IsAny())
            {
                // Delay processing.
                Thread.Sleep(delay);
                // Pop (remove) value and output info.
                Logging.Log($"Book has been destroyed: {Singleton.Pop().Value}, on {Thread.CurrentThread.Name} thread.", Logging.OutputType.Timestamp);
            }
        }
        catch (System.Threading.ThreadAbortException exception)
        {
            // Output expected ThreadAbortException.
            Logging.Log(exception, true, Logging.OutputType.Timestamp);
        }
        catch (Exception exception)
        {
            // Output unexpected Exceptions.
            Logging.Log(exception, false, Logging.OutputType.Timestamp);
        }
    }

    /// <summary>
    /// Count all Books in Singleton collection during loop.
    /// </summary>
    /// <param name="iterations">Number of iteration loops to perform count.</param>
    /// <param name="delay">Delay between counts.</param>
    internal void CountBooks(int iterations = 5, int delay = 900)
    {
        try
        {
            // Loop once per iteration.
            for (var i = 0; i < iterations; i++)
            {
                // Delay processing.
                Thread.Sleep(delay);
                // Count books and output.
                Logging.Log($"Book count: {Singleton.GetValues().Count}, on {Thread.CurrentThread.Name} thread.", Logging.OutputType.Timestamp);
            }
        }
        catch (System.Threading.ThreadAbortException exception)
        {
            // Output expected ThreadAbortException.
            Logging.Log(exception, true, Logging.OutputType.Timestamp);
        }
        catch (Exception exception)
        {
            // Output unexpected Exceptions.
            Logging.Log(exception, false, Logging.OutputType.Timestamp);
        }
    }
}
```

`DestroyBooks(int)` loops through the `Book` collection of `Singleton` while it has values and outputs the result of `Singleton.Pop()`, which removes the last element of the collection.  `CountBooks(int, int)` performs a loop -- once for every `iterations` (default 5) and with a `delay` of milliseconds (default 500) -- and outputs the current number of values in the `Singleton` `Book` collection.

As previously mentioned, we'll be using two different threads to test these methods, aborting one midway through to see what happens.  The `AdvancedThreadTester` class constructor performs all the required logic:

```cs
internal class AdvancedThreadTester
{
    internal AdvancedThreadTester()
    {
        // Instantiate thread manager.
        var bookManager = new BookManager();

        // Add Books to Singleton instance List.
        bookManager.Singleton.Add(new Book("Magician", "Raymond E. Feist", 681));
        bookManager.Singleton.Add(new Book("The Revenant", "Michael Punke", 272));
        bookManager.Singleton.Add(new Book("The Final Empire", "Brandon Sanderson", 541));
        bookManager.Singleton.Add(new Book("The Code Book", "Simon Singh", 412));
        bookManager.Singleton.Add(new Book("Ship of Magic", "Robin Hobb", 880));

        // Create secondary thread and assign DestroyBooks() as delegate.
        var thread = new Thread(
            () => bookManager.DestroyBooks())
            { Name = "Secondary" };

        // Start secondary thread.
        thread.Start();
        Logging.Log($"{thread.Name} thread started.", Logging.OutputType.Timestamp);

        // Count Books in main thread
        bookManager.CountBooks();

        // Abort secondary thread.
        thread.Abort();
        Logging.Log($"{thread.Name} thread aborted.", Logging.OutputType.Timestamp);

        // Join main and secondary thread.
        thread.Join();
        Logging.Log($"Joining {Thread.CurrentThread.Name} and {thread.Name} threads.", Logging.OutputType.Timestamp);
    }
}
```

It starts by instantiating a new `BookManager` instance, then adding a small collection of `Books` to the underlying `Singleton<Book>` instance collection.  Next, a new thread called `Secondary` is created, to which we assign `bookManager.DestroyBooks()` as the delegate method when the thread starts.  Speaking of which, we then `Start()` the secondary thread and output a message.  Then we call `bookManager.CountBooks()`, which is invoked on the `Main` thread (and was so-named elsewhere in the code).

This is where things become more interesting.  Both `DestroyBooks()` and `CountBooks()` feature loops and intentional delays, so both threads are simultaneously processing for a few seconds, outputting their respective results.  Eventually, `CountBooks()` completes execution and the `Main` thread moves onto the `thread.Abort()` call, which forces the `Secondary` thread to terminate itself before it finishes destroying all the books.  As a safety precaution, we also `Join()` both threads together afterward.

The output from this code shows what's going on using relative timestamps, and with indications of which thread is performing what task:

```
[00:00:00.0031626] Secondary thread started.
[00:00:00.9243493] Book count: 5, on Main thread.
[00:00:01.0262553] Book has been destroyed: 'Ship of Magic' by Robin Hobb at 880 pages, on Secondary thread.
[00:00:01.8247234] Book count: 4, on Main thread.
[00:00:02.0267851] Book has been destroyed: 'The Code Book' by Simon Singh at 412 pages, on Secondary thread.
[00:00:02.7251227] Book count: 3, on Main thread.
[00:00:03.0279273] Book has been destroyed: 'The Final Empire' by Brandon Sanderson at 541 pages, on Secondary thread.
[00:00:03.6258959] Book count: 2, on Main thread.
[00:00:04.0289650] Book has been destroyed: 'The Revenant' by Michael Punke at 272 pages, on Secondary thread.
[00:00:04.5271146] Book count: 1, on Main thread.
[00:00:04.5327573] [EXPECTED] System.Threading.ThreadAbortException: Thread was being aborted.
   at System.Threading.Thread.SleepInternal(Int32 millisecondsTimeout)
   at System.Threading.Thread.Sleep(Int32 millisecondsTimeout)
   at Airbrake.Threading.ThreadAbortException.BookManager.DestroyBooks(Int32 delay) in D:\work\Airbrake.io\Exceptions\.NET\Airbrake.Threading.ThreadAbortException\BookManager.cs:line 23: Thread was being aborted.

[00:00:04.5381438] Secondary thread aborted.
[00:00:04.5382808] Joining Main and Secondary threads.
```

As we can see, the simultaneous processing worked as intended, allowing the `Main` thread to count the dwindling number of `Books` while the `Secondary` thread set out to destroy them.  However, once the `Main` thread finished counting and invoked the `Abort()` method, our `Secondary` thread instantly threw a `System.Threading.ThreadAbortException`.

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A close look at System.Threading.ThreadAbortException in .NET, including C# code showing how to share resources in multi-threaded applications.