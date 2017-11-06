# Java Exception Handling - IllegalMonitorStateException

Moving along through our in-depth [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series, today we'll get into the **IllegalMonitorStateException**.  The `IllegalMonitorStateException` is thrown when a thread has been instructed to wait for an object's monitor that the specified thread does not have ownership of.

Throughout this article we'll explore the `IllegalMonitorStateException` in greater detail, starting with where it resides in the overall[Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).  We'll also look at a functional code sample that illustrates how multithreading and concurrent waiting might be implemented, and how `IllegalMonitorStateExceptions` can appear if such an implementation has some small flaws in it.  Let's get crackin'!

## The Technical Rundown

All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`java.lang.Object`](https://docs.oracle.com/javase/8/docs/api/java/lang/Object.html)
    - [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy)
        - [`java.lang.Exception`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html)
            - [`java.lang.RuntimeException`](https://docs.oracle.com/javase/8/docs/api/java/lang/RuntimeException.html)
                - `IllegalMonitorStateException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```java
// Main.java
package io.airbrake;

import io.airbrake.utility.Logging;

public class Main {

    static Main main = null;

    public static void main(String[] args)
    {
        main = new Main();
    }

    private Main()
    {
        Logging.lineSeparator("DUAL THREADING");
        DualThreadingTest();

        Logging.lineSeparator("DUAL THREADING w/ OWNERSHIP");
        DualThreadingWithOwnershipTest();
    }

    private void DualThreadingTest() {
        try {
            // Create manager.
            ThreadingManager manager = new ThreadingManager();

            // Create runners and add to manager.
            manager.addRunner(new Runner("primary"));
            manager.addRunner(new Runner("secondary"));

            // Create threads, set runners, and add to manager.
            manager.addThread(new Thread(manager.getRunner("primary"), "primary"));
            manager.addThread(new Thread(manager.getRunner("secondary"), "secondary"));

            // Start threads.
            manager.getThread("primary").start();
            manager.getThread("secondary").start();
        } catch (IllegalMonitorStateException exception) {
            // Output expected IllegalMonitorStateExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }

    private void DualThreadingWithOwnershipTest() {
        try
        {
            // Create manager.
            ThreadingManager manager = new ThreadingManager();

            // Create runners and add to manager.
            manager.addRunner(new Runner("primary"));
            manager.addRunner(new Runner("secondary"));

            // Create threads, set runners, and add to manager.
            manager.addThread(new Thread(manager.getRunner("primary"), "primary"));
            manager.addThread(new Thread(manager.getRunner("secondary"), "secondary"));

            // Set runner thread ownership.
            manager.getRunner("primary").setThread(manager.getThread("primary"));
            manager.getRunner("secondary").setThread(manager.getThread("secondary"));

            // Start threads.
            manager.getThread("primary").start();
            manager.getThread("secondary").start();
        } catch (IllegalMonitorStateException exception) {
            // Output expected IllegalMonitorStateExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }
}
```

```java
// ThreadingManager.java
package io.airbrake;

import java.util.ArrayList;
import java.util.Objects;

public class ThreadingManager {
    private ArrayList<Runner> runners = new ArrayList<>();
    private ArrayList<Thread> threads = new ArrayList<>();

    ThreadingManager() { }

    void addRunner(Runner runner) {
        this.runners.add(runner);
    }

    void addThread(Thread thread) {
        threads.add(thread);
    }

    public Runner getRunner(int index) {
        return runners.get(index);
    }

    Runner getRunner(String name) {
        for (Runner runner : runners) {
            if (Objects.equals(runner.getName(), name)) {
                return runner;
            }
        }
        return null;
    }

    public ArrayList<Runner> getRunners() {
        return runners;
    }

    public Thread getThread(int index) {
        return threads.get(index);
    }

    Thread getThread(String name) {
        for (Thread thread : threads) {
            if (Objects.equals(thread.getName(), name)) {
                return thread;
            }
        }
        return null;
    }

    public ArrayList<Thread> getThreads() {
        return threads;
    }

    public void setRunners(ArrayList<Runner> runners) {
        this.runners = runners;
    }

    public void setThreads(ArrayList<Thread> threads) {
        this.threads = threads;
    }
}
```

```java
// Runner.java
package io.airbrake;

import io.airbrake.utility.Logging;

public class Runner implements Runnable
{
    private String name;
    private Thread thread;

    Runner(String name) {
        setName(name);
    }

    Runner(String name, Thread thread) {
        setName(name);
    }

    String getName() {
        return this.name;
    }

    private Thread getThread() {
        return thread;
    }

    private void setName(String name) {
        this.name = name;
    }

    void setThread(Thread thread) {
        this.thread = thread;
    }

    public void run()
    {
        try
        {
            // Check for ownership thread.
            if (getThread() == null) {
                Logging.log(String.format("Waiting for thread %s in runner %s.", "Main", getName()));
                // If no thread, wait for main thread.
                Main.main.wait();
            } else {
                synchronized (getThread()) {
                    Logging.log(String.format("Waiting for thread %s in runner %s.", getThread().getName(), getName()));
                    // If thread, invoke wait().
                    getThread().wait();
                }
            }
        } catch (IllegalMonitorStateException exception) {
            // Output expected IllegalMonitorStateExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }
}
```

This code sample also uses the [`Logging.java`](https://github.com/GabeStah/Airbrake.io/blob/master/lib/java/src/io/airbrake/utility/Logging.java) class, the source of which can be [found on GitHub](https://github.com/GabeStah/Airbrake.io/blob/master/lib/java/src/io/airbrake/utility/Logging.java).

## When Should You Use It?

As described by the official documentation, an `IllegalMonitorStateException` can occur when a thread attempts to wait on an object's monitor, or to notify other threads waiting for said object's monitor, when that thread does not _own_ the monitor in question.  Put another way, if the [`Object.wait()`](https://docs.oracle.com/javase/8/docs/api/java/lang/Object.html#wait--) method is called on an object which _was not_ created by the _current_ thread, an `IllegalMonitorStateException` will be thrown.  A thread can become the owner of an object's monitor by:

- Executing a synchronized instance method of the object.
- Executing a code block of a synchronized statement that synchronizes on the object.
- Executing a synchronized static method of the object, _if_ the object is a `Class` type.

This is easier to understand with some code examples, but before we get into that it's worth briefly noting that the use of `Object.wait()` and `notify()` methods is generally not the go-to technique for performing multithreaded or concurrent programming in modern Java.  If you're interested in how most Java developments handle concurrency, take a look at the [`java.util.concurrent`](https://docs.oracle.com/javase/8/docs/api/index.html?java/util/concurrent/package-summary.html) package.

The goal with our code sample is simple: To create two threads and assign each their own [`Runnable`](https://docs.oracle.com/javase/8/docs/api/java/lang/Runnable.html) object, which is an interface that implements the `run()` method.  A `Runnable` instance can be passed to a new `Thread` instance, and the `Runnable.run()` method will be automatically executed by the thread once it's started.

Thus, we begin with our `Runner` class, which implements the `Runnable` interface:

```java
package io.airbrake;

import io.airbrake.utility.Logging;

public class Runner implements Runnable
{
    private String name;
    private Thread thread;

    Runner(String name) {
        setName(name);
    }

    Runner(String name, Thread thread) {
        setName(name);
    }

    String getName() {
        return this.name;
    }

    private Thread getThread() {
        return thread;
    }

    private void setName(String name) {
        this.name = name;
    }

    void setThread(Thread thread) {
        this.thread = thread;
    }

    public void run()
    {
        try
        {
            // Check for ownership thread.
            if (getThread() == null) {
                Logging.log(String.format("Waiting for thread %s in runner %s.", "Main", getName()));
                // If no thread, wait for main thread.
                Main.main.wait();
            } else {
                synchronized (getThread()) {
                    Logging.log(String.format("Waiting for thread %s in runner %s.", getThread().getName(), getName()));
                    // If thread, invoke wait().
                    getThread().wait();
                }
            }
        } catch (IllegalMonitorStateException exception) {
            // Output expected IllegalMonitorStateExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }
}
```

Aside from a few getters and setters, the core logic is found in the `run()` method.  It starts by checking if the `Thread thread` property of the `Runner` exists via the `getThread()` method.  If not, it explicitly calls the `Main.main.wait()` method, otherwise it calls the `wait()` method of the associated `Thread` object for this `Runner` instance.

The `ThreadingManager` class is just a helper class to make it easier to generate and keep track of `Runners` and `Threads` by storing them in private `ArrayList<Runner> runners` and `ArrayList<Thread> threads` properties.  We'll also use many of the helper methods, such as `getRunner(String name)`, to retrieve specific instances of `Runners` and `Threads` for testing purposes later on:

```java
package io.airbrake;

import java.util.ArrayList;
import java.util.Objects;

public class ThreadingManager {
    private ArrayList<Runner> runners = new ArrayList<>();
    private ArrayList<Thread> threads = new ArrayList<>();

    ThreadingManager() { }

    void addRunner(Runner runner) {
        this.runners.add(runner);
    }

    void addThread(Thread thread) {
        threads.add(thread);
    }

    public Runner getRunner(int index) {
        return runners.get(index);
    }

    Runner getRunner(String name) {
        for (Runner runner : runners) {
            if (Objects.equals(runner.getName(), name)) {
                return runner;
            }
        }
        return null;
    }

    public ArrayList<Runner> getRunners() {
        return runners;
    }

    public Thread getThread(int index) {
        return threads.get(index);
    }

    Thread getThread(String name) {
        for (Thread thread : threads) {
            if (Objects.equals(thread.getName(), name)) {
                return thread;
            }
        }
        return null;
    }

    public ArrayList<Thread> getThreads() {
        return threads;
    }

    public void setRunners(ArrayList<Runner> runners) {
        this.runners = runners;
    }

    public void setThreads(ArrayList<Thread> threads) {
        this.threads = threads;
    }
}
```

Speaking of testing, let's look at our `Main` class and the first test method, `DualThreadingTest()`:

```java
public class Main {
    // ...

    private void DualThreadingTest() {
        try {
            // Create manager.
            ThreadingManager manager = new ThreadingManager();

            // Create runners and add to manager.
            manager.addRunner(new Runner("primary"));
            manager.addRunner(new Runner("secondary"));

            // Create threads, set runners, and add to manager.
            manager.addThread(new Thread(manager.getRunner("primary"), "primary"));
            manager.addThread(new Thread(manager.getRunner("secondary"), "secondary"));

            // Start threads.
            manager.getThread("primary").start();
            manager.getThread("secondary").start();
        } catch (IllegalMonitorStateException exception) {
            // Output expected IllegalMonitorStateExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }

    // ...
}
```

As you can see, our test begins with a new `ThreadingManager` instance, from which we create two new `Runners` named `primary` and `secondary`, respectively.  We then create two new `Threads` with the same names, and retrieve the appropriate `Runners` from our `manager` instance to pass to those `new Thread(...)` constructor calls.  As mentioned before, passing a `Runnable` instance (like a `Runner` object) to a `Thread` constructor ensures that the `Runner's` `run()` method is executed when the thread starts execution.  Consequently, our last step is to `start()` both threads.

Executing the `DualThreadingTest()` code produces the following output, which includes throwing some `IllegalMonitorStateExceptions`:

```
------------ DUAL THREADING ------------
Waiting for thread Main in runner primary.
Waiting for thread Main in runner secondary.
[EXPECTED] java.lang.IllegalMonitorStateException
[EXPECTED] java.lang.IllegalMonitorStateException
```

Since we didn't explicitly tell our `Runner` instances which specific `Thread` instance each was assigned, they both attempted to call the `wait()` method of the `Main.main` object, as seen in this snippet from the `Runner.run()` method:

```java
// ...

// Check for ownership thread.
if (getThread() == null) {
    Logging.log(String.format("Waiting for thread %s in runner %s.", "Main", getName()));
    // If no thread, wait for main thread.
    Main.main.wait();
} else {
    synchronized (getThread()) {
        Logging.log(String.format("Waiting for thread %s in runner %s.", getThread().getName(), getName()));
        // If thread, invoke wait().
        getThread().wait();
    }
}

// ...
```

At the most basic level, every object in Java has an intrinsic lock associated with it.  Anytime a thread needs exclusive access to that object it must acquire _ownership_ of the object's intrinsic lock.  Once exclusive access requirements are finished, it can release the lock.  In some scenarios, like the one we created above, a thread may attempt an action on an object for which it doesn't have an exclusive lock, which can cause an `IllegalMonitorStateException` in some situations.

One solution is to use the built-in [`synchronized`](https://docs.oracle.com/javase/tutorial/essential/concurrency/locksync.html) statement, which specifies an object that will be providing the intrinsic lock to the code block within the statement.  You can see an example of this in the `else { ... }` block of our `Runner.run()` method above.  By using the `synchronized (getThread())` statement, we give the code within the `synchronized` block access to the lock of the associated thread of this `Runner` instance (as acquired by `getThread()`).  It's worth noting that it's usually best programming practice to use a local variable and retrieve the `Thread` instance via `getThread()` one time, then reuse it throughout the code.  However, in this case, using a local variable within a `synchronized` statement can be tricky to control, since synchronization may occur out of order (slower or faster) than code outside of it that actually produced the local variable.  Thus, it's safer to make the explicit call every time in this case.

To test this behavior we have the `DualThreadingWithOwnershipTest()` method, which is similar to `DualThreadingTest()`, except we explicitly assign `Thread` instances to the `Runner` instances before `start()` is called for both `Threads`:

```java
private void DualThreadingWithOwnershipTest() {
    try
    {
        // Create manager.
        ThreadingManager manager = new ThreadingManager();

        // Create runners and add to manager.
        manager.addRunner(new Runner("primary"));
        manager.addRunner(new Runner("secondary"));

        // Create threads, set runners, and add to manager.
        manager.addThread(new Thread(manager.getRunner("primary"), "primary"));
        manager.addThread(new Thread(manager.getRunner("secondary"), "secondary"));

        // Set runner thread ownership.
        manager.getRunner("primary").setThread(manager.getThread("primary"));
        manager.getRunner("secondary").setThread(manager.getThread("secondary"));

        // Start threads.
        manager.getThread("primary").start();
        manager.getThread("secondary").start();
    } catch (IllegalMonitorStateException exception) {
        // Output expected IllegalMonitorStateExceptions.
        Logging.log(exception);
    } catch (Exception exception) {
        // Output unexpected Exceptions.
        Logging.log(exception, false);
    }
}
```

As a result, this invokes the `synchronized(...)` statement for each appropriate thread within the `Runner.run()` method, ensuring our concurrency works as expected and doesn't throw any `inels` in the output:

```
----- DUAL THREADING w/ OWNERSHIP ------
Waiting for thread primary in runner primary.
Waiting for thread secondary in runner secondary.
```

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look at the Java IllegalMonitorStateException, with a code sample illustrating how to handle such exceptions within multithreaded applications.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html