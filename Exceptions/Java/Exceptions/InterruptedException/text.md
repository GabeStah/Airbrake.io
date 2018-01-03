# Java Exception Handling - InterruptedException

Moving along through our detailed [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series, today we'll be going over the **InterruptedException**.  An `InterruptedException` is thrown when a thread that is sleeping, waiting, or is occupied is interrupted.

In this article we'll explore the `InterruptedException` by first looking at where it resides in the overall [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).  We'll also look at some functional Java code samples that illustrate how working in multithreaded applications can potentially cause `InterruptedExceptions`.  Let's get going!

## The Technical Rundown

All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`java.lang.Object`](https://docs.oracle.com/javase/8/docs/api/java/lang/Object.html)
    - [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy)
        - [`java.lang.Exception`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html)
            - `InterruptedException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```java
package io.airbrake;

public class Main {

    public static void main(String[] args) {
        InterruptableThreadTest tester = new InterruptableThreadTest.InterruptableThreadTestBuilder()
                .setShouldInterrupt(false)
                .setShouldSleepMain(false)
                .createInterruptableThreadTest();
    }
}
```

```java
package io.airbrake;

import io.airbrake.utility.Logging;

public class InterruptableThread extends Thread {

    InterruptableThread(String name) {
        super(name);
    }

    public void run() {
        try {
            Logging.log(String.format("%s '%s' sleeping for %d ms.",
                    this.getClass().getSimpleName(),
                    this.getName(),
                    2000),
                    true);
            sleep(2000);
            Logging.log(String.format("%s '%s' sleeping complete.",
                    this.getClass().getSimpleName(),
                    this.getName()),
                    true);
        } catch (InterruptedException exception) {
            // Output expected InterruptedExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }
}
```

```java
package io.airbrake;

import io.airbrake.utility.Logging;

public class InterruptableThreadTest {

    private boolean shouldSleepMain = false;
    private boolean shouldInterrupt = false;
    private int sleepMainDuration = 2500;

    public InterruptableThreadTest(boolean shouldSleepMain, boolean shouldInterrupt, int sleepMainDuration) {
        this.shouldSleepMain = shouldSleepMain;
        this.shouldInterrupt = shouldInterrupt;
        this.sleepMainDuration = sleepMainDuration;

        try {
            Thread main = Thread.currentThread();
            // Create InterruptableThread named 'secondary.'
            InterruptableThread secondary = new InterruptableThread("secondary");
            // Start thread and sleep, if applicable.
            Logging.log(String.format("%s '%s' started.",
                    secondary.getClass().getSimpleName(),
                    secondary.getName()),
                    true);
            secondary.start();
            if (this.shouldSleepMain) {
                Logging.log(String.format("%s '%s' sleeping for %d ms.",
                        main.getClass().getSimpleName(),
                        main.getName(),
                        this.sleepMainDuration),
                        true);
                Thread.sleep(this.sleepMainDuration);
                Logging.log(String.format("%s '%s' sleeping complete.",
                        main.getClass().getSimpleName(),
                        main.getName()),
                        true);
            }
            // Interrupt, if applicable.
            if (this.shouldInterrupt) {
                Logging.log(String.format("%s '%s' interrupted.",
                        secondary.getClass().getSimpleName(),
                        secondary.getName()),
                        true);
                secondary.interrupt();
            }
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }

    public static class InterruptableThreadTestBuilder {
        private boolean shouldSleepMain;
        private boolean shouldInterrupt;
        private int sleepMainDuration;

        public InterruptableThreadTestBuilder setShouldSleepMain(boolean shouldSleepMain) {
            this.shouldSleepMain = shouldSleepMain;
            return this;
        }

        public InterruptableThreadTestBuilder setShouldInterrupt(boolean shouldInterrupt) {
            this.shouldInterrupt = shouldInterrupt;
            return this;
        }

        public InterruptableThreadTestBuilder setSleepMainDuration(int sleepMainDuration) {
            this.sleepMainDuration = sleepMainDuration;
            return this;
        }

        public InterruptableThreadTest createInterruptableThreadTest() {
            return new InterruptableThreadTest(shouldSleepMain, shouldInterrupt, sleepMainDuration);
        }
    }
}
```

```java
// Logging.java
package io.airbrake.utility;

import java.sql.Timestamp;
import java.util.Arrays;

import org.apache.commons.lang3.ClassUtils;
import org.apache.commons.lang3.builder.*;

/**
 * Houses all logging methods for various debug outputs.
 */
public class Logging {
    private static final char separatorCharacterDefault = '-';
    private static final String separatorInsertDefault = "";
    private static final int separatorLengthDefault = 40;

    /**
     * Get a String of passed char of passed length size.
     * @param character Character to repeat.
     * @param length Length of string.
     * @return Created string.
     */
    private static String getRepeatedCharString(char character, int length) {
        // Create new character array of proper length.
        char[] characters = new char[length];
        // Fill each array element with character.
        Arrays.fill(characters, character);
        // Return generated string.
        return new String(characters);
    }

    /**
     * Outputs any kind of Object.
     * Uses ReflectionToStringBuilder from Apache commons-lang library.
     *
     * @param value Object to be output.
     */
    public static void log(Object value)
    {
        if (value == null) return;
        // If primitive or wrapper object, directly output.
        if (ClassUtils.isPrimitiveOrWrapper(value.getClass()))
        {
            System.out.println(value);
        }
        else
        {
            // For complex objects, use reflection builder output.
            System.out.println(new ReflectionToStringBuilder(value, ToStringStyle.MULTI_LINE_STYLE).toString());
        }
    }

    /**
     * Outputs any kind of String.
     *
     * @param value String to be output.
     */
    public static void log(String value)
    {
        if (value == null) return;
        System.out.println(value);
    }

    /**
     * Outputs any kind of String, with prefixed timestamp.
     *
     * @param value String to be output.
     * @param includeTimestamp Indicates if timestamp should be included.
     */
    public static void log(String value, boolean includeTimestamp)
    {
        if (value == null) return;
        if (includeTimestamp) {
            System.out.println(String.format("[%s] %s", new Timestamp(System.currentTimeMillis()), value));
        } else {
            log(value);
        }
    }

    /**
     * Outputs passed in Throwable exception or error instance.
     * Can be overloaded if expected parameter should be specified.
     *
     * @param throwable Throwable instance to output.
     */
    public static void log(Throwable throwable)
    {
        // Invoke call with default expected value.
        log(throwable, true);
    }

    /**
     * Outputs passed in Throwable exception or error instance.
     * Includes Throwable class type, message, stack trace, and expectation status.
     *
     * @param throwable Throwable instance to output.
     * @param expected Determines if this Throwable was expected or not.
     */
    public static void log(Throwable throwable, boolean expected)
    {
        System.out.println(String.format("[%s] %s", expected ? "EXPECTED" : "UNEXPECTED", throwable.toString()));
        throwable.printStackTrace();
    }

    /**
     * See: lineSeparator(String, int, char)
     */
    public static void lineSeparator() {
        lineSeparator(separatorInsertDefault, separatorLengthDefault, separatorCharacterDefault);
    }

    /**
     * See: lineSeparator(String, int, char)
     */
    public static void lineSeparator(String insert) {
        lineSeparator(insert, separatorLengthDefault, separatorCharacterDefault);
    }

    /**
     * See: lineSeparator(String, int, char)
     */
    public static void lineSeparator(int length) {
        lineSeparator(separatorInsertDefault, length, separatorCharacterDefault);
    }

    /**
     * See: lineSeparator(String, int, char)
     */
    public static void lineSeparator(int length, char separator) {
        lineSeparator(separatorInsertDefault, length, separator);
    }

    /**
     * See: lineSeparator(String, int, char)
     */
    public static void lineSeparator(char separator) {
        lineSeparator(separatorInsertDefault, separatorLengthDefault, separator);
    }

    /**
     * See: lineSeparator(String, int, char)
     */
    public static void lineSeparator(String insert, int length) {
        lineSeparator(insert, length, separatorCharacterDefault);
    }

    /**
     * See: lineSeparator(String, int, char)
     */
    public static void lineSeparator(String insert, char separator) {
        lineSeparator(insert, separatorLengthDefault, separator);
    }

    /**
     * Outputs a dashed line separator with
     * inserted text centered in the middle.
     *
     * @param insert Inserted text to be centered.
     * @param length Length of line to be output.
     * @param separator Separator character.
     */
    public static void lineSeparator(String insert, int length, char separator)
    {
        // Default output to insert.
        String output = insert;

        if (insert.length() == 0) {
            output = getRepeatedCharString(separator, length);
        } else if (insert.length() < length) {
            // Update length based on insert length, less a space for margin.
            length -= (insert.length() + 2);
            // Halve the length and floor left side.
            int left = (int) Math.floor(length / 2);
            int right = left;
            // If odd number, add dropped remainder to right side.
            if (length % 2 != 0) right += 1;

            // Surround insert with separators.
            output = String.format("%s %s %s", getRepeatedCharString(separator, left), insert, getRepeatedCharString(separator, right));
        }

        System.out.println(output);
    }
}
```

## When Should You Use It?

Since the `InterruptedException` is thrown when an active or sleeping thread is interrupted, this is _typically_ only relevant when working with multithreaded applications.  That is to say, since a single-threaded application would immediately halt all execution if the main thread was interrupted, you'll actually be catching and responding to `InterruptedExceptions` only when there's at least one _additional_ thread in which to process the interruption.

With that, let's jump right into our example code.  To make things easier to track and log we've created the `InterruptableThread` class, which extends the base `Thread` class:

```java
package io.airbrake;

import io.airbrake.utility.Logging;

public class InterruptableThread extends Thread {

    InterruptableThread(String name) {
        super(name);
    }

    public void run() {
        try {
            Logging.log(String.format("%s '%s' sleeping for %d ms.",
                    this.getClass().getSimpleName(),
                    this.getName(),
                    2000),
                    true);
            sleep(2000);
            Logging.log(String.format("%s '%s' sleeping complete.",
                    this.getClass().getSimpleName(),
                    this.getName()),
                    true);
        } catch (InterruptedException exception) {
            // Output expected InterruptedExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }
}
```

Nothing particularly fancy going on here.  The constructor expects a single `String name` argument and passes that along to the base `Thread` constructor that also accepts a single `String` parameter for the `name` property.  Otherwise, we implement the `run` method, since `Thread` implements the `Runnable` interface, which provides the `abstract void run()` method that will be executed when the thread starts.  Within `run()` we output some messages to the log and call the `sleep(2000)` method to pause execution of this thread for two seconds.  Otherwise, we catch any exceptions and that's it.

For actually testing `InterruptableThread` instances we've also created the `InterruptableThreadTest` class.  Since working with many default-valued parameters in Java can be annoying, rather than using multiple constructor overloads we've opted for a `builder` pattern, which we deeply dug into in [a previous article](https://airbrake.io/blog/design-patterns/builder-method).  The linked article will explain a great deal more about the `builder` pattern if you're curious, but the basic purpose is to simplify the process of changing many `mutable` (editable) properties, without the need to explicitly specify or modify any particular properties.  We can alter only the properties we care about using chained method calls, while all other properties remain untouched.

The `InterruptableThreadTesterBuilder` class implements the `builder` pattern for the underlying `InterruptableThreadTest` class:

```java
public static class InterruptableThreadTestBuilder {
    private boolean shouldSleepMain;
    private boolean shouldInterrupt;
    private int sleepMainDuration;

    public InterruptableThreadTestBuilder setShouldSleepMain(boolean shouldSleepMain) {
        this.shouldSleepMain = shouldSleepMain;
        return this;
    }

    public InterruptableThreadTestBuilder setShouldInterrupt(boolean shouldInterrupt) {
        this.shouldInterrupt = shouldInterrupt;
        return this;
    }

    public InterruptableThreadTestBuilder setSleepMainDuration(int sleepMainDuration) {
        this.sleepMainDuration = sleepMainDuration;
        return this;
    }

    public InterruptableThreadTest createInterruptableThreadTest() {
        return new InterruptableThreadTest(shouldSleepMain, shouldInterrupt, sleepMainDuration);
    }
}
```

With the `builder` setup we can set the modified properties of `InterruptableThreadTest` in the primary constructor, then actually perform our logic and processing:

```java
public class InterruptableThreadTest {

    private boolean shouldSleepMain = false;
    private boolean shouldInterrupt = false;
    private int sleepMainDuration = 2500;

    public InterruptableThreadTest(boolean shouldSleepMain, boolean shouldInterrupt, int sleepMainDuration) {
        this.shouldSleepMain = shouldSleepMain;
        this.shouldInterrupt = shouldInterrupt;
        this.sleepMainDuration = sleepMainDuration;

        try {
            Thread main = Thread.currentThread();
            // Create InterruptableThread named 'secondary.'
            InterruptableThread secondary = new InterruptableThread("secondary");
            // Start thread and sleep, if applicable.
            Logging.log(String.format("%s '%s' started.",
                    secondary.getClass().getSimpleName(),
                    secondary.getName()),
                    true);
            secondary.start();
            if (this.shouldSleepMain) {
                Logging.log(String.format("%s '%s' sleeping for %d ms.",
                        main.getClass().getSimpleName(),
                        main.getName(),
                        this.sleepMainDuration),
                        true);
                Thread.sleep(this.sleepMainDuration);
                Logging.log(String.format("%s '%s' sleeping complete.",
                        main.getClass().getSimpleName(),
                        main.getName()),
                        true);
            }
            // Interrupt, if applicable.
            if (this.shouldInterrupt) {
                Logging.log(String.format("%s '%s' interrupted.",
                        secondary.getClass().getSimpleName(),
                        secondary.getName()),
                        true);
                secondary.interrupt();
            }
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }
}
```

Here we create a `secondary` `InterruptableThread` and then `start()` it immediately.  We also use the modified properties to determine if the `main` `Thread` should be slept, for how long, and if the `secondary` `InterruptableThread` should be interrupted.

Our `Main.main(...)` method instantiates an `InterruptableThreadTestBuilder` object and calls whatever property setters we need, before finally calling `createInterruptableThreadTest()`, which returns the fully-constructed `InterruptableThreadTest` instance:

```java
package io.airbrake;

public class Main {
    public static void main(String[] args) {
        InterruptableThreadTest tester = new InterruptableThreadTest.InterruptableThreadTestBuilder()
                .setShouldInterrupt(false)
                .setShouldSleepMain(false)
                .createInterruptableThreadTest();
    }
}
```

In this first example we're not interrupting the `secondary` thread and we're not sleeping the `main` thread.  Executing this test produces the following output:

```
[2018-01-02 17:50:21.283] InterruptableThread 'secondary' started.
[2018-01-02 17:50:21.289] InterruptableThread 'secondary' sleeping for 2000 ms.
[2018-01-02 17:50:23.289] InterruptableThread 'secondary' sleeping complete.
```

Everything works as expected.  The `secondary` thread is started, sleeps for two seconds, then completes.  However, let's try interrupting the `secondary` thread and see what happens:

```java
InterruptableThreadTest tester = new InterruptableThreadTest.InterruptableThreadTestBuilder()
        .setShouldInterrupt(true)
        .setShouldSleepMain(false)
        .createInterruptableThreadTest();
```

Running this test throws an `InterruptedException`, because, as we intended, we explicitly interrupted the `secondary` thread during its two second sleep period:

```
[2018-01-02 17:51:00.05] InterruptableThread 'secondary' started.
[2018-01-02 17:51:00.054] InterruptableThread 'secondary' interrupted.
[2018-01-02 17:51:00.054] InterruptableThread 'secondary' sleeping for 2000 ms.
[EXPECTED] java.lang.InterruptedException: sleep interrupted
```

In this case, you'll recall that the `InterruptableThreadTest` constructor checks the `shouldSleepMain` property to determine if the `main` thread should also sleep, which occurs immediately after the `secondary` thread is started:

```java
secondary.start();
if (this.shouldSleepMain) {
    Logging.log(String.format("%s '%s' sleeping for %d ms.",
            main.getClass().getSimpleName(),
            main.getName(),
            this.sleepMainDuration),
            true);
    Thread.sleep(this.sleepMainDuration);
    Logging.log(String.format("%s '%s' sleeping complete.",
            main.getClass().getSimpleName(),
            main.getName()),
            true);
}
```

Thus, let's run another test where we sleep the `main` thread for `2500` milliseconds (which is `500` milliseconds longer than the `secondary` thread is sleeping):

```java
InterruptableThreadTest tester = new InterruptableThreadTest.InterruptableThreadTestBuilder()
        .setShouldInterrupt(true)
        .setShouldSleepMain(true)
        .setSleepMainDuration(2500)
        .createInterruptableThreadTest();
```

Executing this test no longer throws an `InterruptedException` and shows both threads sleeping their expected durations:

```
[2018-01-02 17:53:38.152] InterruptableThread 'secondary' started.
[2018-01-02 17:53:38.157] Thread 'main' sleeping for 2500 ms.
[2018-01-02 17:53:38.157] InterruptableThread 'secondary' sleeping for 2000 ms.
[2018-01-02 17:53:40.158] InterruptableThread 'secondary' sleeping complete.
[2018-01-02 17:53:40.657] Thread 'main' sleeping complete.
[2018-01-02 17:53:40.657] InterruptableThread 'secondary' interrupted.
```

What's also important to note is that we _still_ attempt to interrupt the `secondary` thread by calling the `interrupt()` method.  However, since the `secondary` thread is not in an active or sleep state, doing so doesn't produce an `InterruptedException`.

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A deep look at the Java InterruptedException, with functional code samples illustrating how to work with multithreaded applications.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html