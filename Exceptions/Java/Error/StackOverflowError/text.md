# Java Exception Handling - StackOverflowError

Making our way through our in-depth [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series, today we'll dig into the Java StackOverflowError.  As with most programming languages, the `StackOverflowError` in Java occurs when the application performs excessively deep recursion.  However, what exactly qualifies as "excessively deep" depends on many factors.

In this article we'll explore the `StackOverflowError` a bit more by first looking where it resides in the overall [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).  We'll also look at a simple, functional code sample that will illustrate how deep recursion can be created, and what might cause a `StackOverflowError` in your own code.  Let's get going!

## The Technical Rundown

All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`java.lang.Object`](https://docs.oracle.com/javase/8/docs/api/java/lang/Object.html)
    - [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy)
        - [`java.lang.Error`](https://docs.oracle.com/javase/8/docs/api/java/lang/Error.html)
            - [`java.lang.VirtualMachineError`](https://docs.oracle.com/javase/8/docs/api/java/io/IOException.html)
                - `StackOverflowError`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```java
package io.airbrake;

public class Main {
    public static void main(String[] args) {
        Iterator iterator = new Iterator();
        iterator.increment();
    }
}

// Iterator.java
package io.airbrake;

import io.airbrake.utility.Logging;

public class Iterator {
    private static final double MILLION = 1000000.0;
    private long count = 0;
    // Create local reference to logging class, to avoid NoClassDefFoundErrors.
    private Logging logger = new Logging();
    private long startTime = System.nanoTime();

    Iterator() { }

    void increment() {
        try {
            // Increment count and call self.
            this.count++;
            increment();
        } catch (StackOverflowError error) {
            // Get elapsed time.
            long elapsed = System.nanoTime() - startTime;
            // Output iteration count and total elapsed time.
            logger.lineSeparator(String.format("%d iterations in %s milliseconds.", this.count, (double) elapsed / MILLION), 60);
            // Output expected StackOverflowErrors.
            logger.log(error);
        } catch (Throwable throwable) {
            // Output unexpected Throwables.
            logger.log(throwable, false);
        }
    }
}

// Logging.java
package io.airbrake.utility;

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

Before we look at what might cause a `StackOverflowError` in Java code, let's first take a moment to review what a `stack overflow` actually is.  Most applications are allocated a range of `memory addresses` that the application can use during execution.  These addresses are stored and used as simple pointers to bytes of data (i.e. memory).  This collection of addresses is known as the `address space` assigned to the application, and it contains a specific range of `memory addresses` that can be safely used by the application.

Unfortunately, at least for the foreseeable future, available memory is a finite resource.  A Java application is limited to the bounds of its assigned `address space`.  Processes like [`garbage collection`](http://www.oracle.com/webfolder/technetwork/tutorials/obe/java/gc01/index.html) will constantly free up memory that is no longer in use, but, by and large, there is a limited quantity of `memory addresses` available to any given application.

When an application attempts to use memory outside of its assigned `address space` a `stack overflow` error typically occurs.  The runtime that is handling the application cannot safely allow said application to use memory that hasn't been assigned to it, so the only logical course of action is to throw an error of some sort.  In the case of Java, this is where the `StackOverflowError` comes in.

There are many different ways a `stack overflow` can occur within any given application, but one of the most common (and easily understood) is `infinite recursion`.  This essentially means that a function or method is calling itself, over and over, ad nauseam.  Different languages handle infinite recursion differently, but the Java Virtual Machine (`JVM`) handles infinite recursion by eventually throwing a `StackOverflowError`.  To illustrate this behavior our example code is quite simple, primarily performed in the `Iterator` class:

```java
// Iterator.java
package io.airbrake;

import io.airbrake.utility.Logging;

public class Iterator {
    private static final double MILLION = 1000000.0;
    private long count = 0;
    // Create local reference to logging class, to avoid NoClassDefFoundErrors.
    private Logging logger = new Logging();
    private long startTime = System.nanoTime();

    Iterator() { }

    void increment() {
        try {
            // Increment count and call self.
            this.count++;
            increment();
        } catch (StackOverflowError error) {
            // Get elapsed time.
            long elapsed = System.nanoTime() - startTime;
            // Output iteration count and total elapsed time.
            logger.lineSeparator(String.format("%d iterations in %s milliseconds.", this.count, (double) elapsed / MILLION), 60);
            // Output expected StackOverflowErrors.
            logger.log(error);
        } catch (Throwable throwable) {
            // Output unexpected Throwables.
            logger.log(throwable, false);
        }
    }
}
```

As you can see, we have a few private members, along with the `increment()` method, which attempts a simple task: Iterate the `count` field, then call itself again.  We also catch the potential errors (or `Throwables`) that might come up from this process.

Let's create a new instance of `Iterator` and start the recursive process by calling the `increment()` method:

```java
package io.airbrake;

public class Main {
    public static void main(String[] args) {
        Iterator iterator = new Iterator();
        iterator.increment();
    }
}
```

Executing the few lines of code above produces the following output:

```
-------- 7189 iterations in 5.202404 milliseconds. ---------
[EXPECTED] java.lang.StackOverflowError
```

We can see that a `StackOverflowError` was thrown, as expected, and it took about `7,200` iterations before the error occurred, with a total processing time of about `5.2` milliseconds.  This is just one test, so let's run it a few more times and record the results:

```
-------- 7294 iterations in 5.846793 milliseconds. ---------
-------- 7545 iterations in 4.776957 milliseconds. ---------
-------- 8050 iterations in 5.783158 milliseconds. ---------
-------- 7307 iterations in 4.498283 milliseconds. ---------
-------- 7305 iterations in 5.445483 milliseconds. ---------
```

What's immediately interesting is that, while a `StackOverflowError` is thrown every time, the number of **recursive iterations** necessary to cause the error changes every time, but within a reasonably similar range.  The reason for this difference is due to the vast quantity of different factors within the system when execution occurs.  For example, the JVM I'm testing this on is Windows 10 64-bit with 16GB of memory, but if we run this application on other machines ([with different JVM configurations](http://docs.oracle.com/cd/E13150_01/jrockit_jvm/jrockit/jrdocs/refman/optionX.html#wp1024112)), we might see completely different iteration counts and/or elapsed times.

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

An examination of the Java StackOverflowError, including functional sample code showing how infinite recursion may cause StackOverflowErrors.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html