# Java Exception Handling - NullPointerException

Today we start the journey through our [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series with a deep dive into the java.lang.NullPointerException.  A `java.lang.NullPointerException` is thrown there's an attempt to use `null` anywhere an object is actually required, such as trying to directly modify a `null` object.

In this article we'll look at where the `java.lang.NullPointerException` sits within the [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy), plus, we'll also explore some functional code samples that illustrate how these exceptions might be raised during your own development adventures, so let's get crackin'!

## The Technical Rundown

- All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.
- [`java.lang.Exception`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html) inherits from [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).
- [`java.lang.RuntimeException`](https://docs.oracle.com/javase/8/docs/api/java/lang/RuntimeException.html) inherits from [`java.lang.Exception`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html).
- Finally, `java.lang.NullPointerException` inherits from the [`java.lang.RuntimeException`](https://docs.oracle.com/javase/8/docs/api/java/lang/RuntimeException.html) class.

## When Should You Use It?

As we saw in the hierarchical breakdown, the `java.lang.NullPointerException` inherits from `java.lang.RuntimeException`, so quite clearly this is an issue that will pop up during execution of an application.  Specifically, virtually anytime an attempt is made to directly access a field, method, or the like of a `null` object a `java.lang.NullPointerException` is thrown.

To illustrate we'll start with the full code example below, after which we'll walk through it step-by-step to see what's going on:

```java
// NullPointerException.java
package io.airbrake;

import io.airbrake.utility.*;

public class NullPointerException {

    private int[] data = new int[100];

    public static void main(String[] args) {
        callInstanceMethodOfNull();
        callInstanceFieldOfNull();
    }

    private static void callInstanceMethodOfNull()
    {
        try
        {
            // Instantiate null object.
            Integer age = null;
            // Attempt to call method on object.
            age.toString();
        }
        catch (java.lang.NullPointerException exception)
        {
            // Catch NullPointerExceptions.
            Logging.log(exception);
        }
        catch (Throwable exception)
        {
            // Catch other Throwables.
            Logging.log(exception, false);
        }
    }

    private static void callInstanceFieldOfNull()
    {
        try
        {
            // Instantiate a new Book.
            Book book = new Book("The Stand", "Stephen King");
            // Set to null.
            book = null;
            // Call null field.
            Logging.log(String.format("Author is: %s", book.author));
        }
        catch (java.lang.NullPointerException exception)
        {
            // Catch NullPointerExceptions.
            Logging.log(exception);
        }
        catch (Throwable exception)
        {
            // Catch other Throwables.
            Logging.log(exception, false);
        }
    }
}
```

```java
// Book.java
package io.airbrake;

/**
 * Simple example class to store book instances.
 */
public class Book
{
    public String title;
    public String author;

    public Book() { }

    public Book(String title, String author) {
        this.title = title;
        this.author = author;
    }
}
```

```java
// Logging.java
package io.airbrake.utility;

import java.util.Arrays;

/**
 * Houses all logging methods for various debug outputs.
 */
public class Logging {

    /**
     * Outputs any kind of Object.
     *
     * @param value Object to be output.
     */
    public static void log(Object value)
    {
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
     * Output a dashed line separator of default (40) length.
     */
    public static void lineSeparator()
    {
        // Invoke default length method.
        lineSeparator(40);
    }

    /**
     * Output a dashed lin separator of desired length.
     *
     * @param length Length of line to be output.
     */
    public static void lineSeparator(int length)
    {
        // Create new character array of proper length.
        char[] characters = new char[length];
        // Fill each array element with character.
        Arrays.fill(characters, '-');
        // Output line of characters.
        System.out.println(new String(characters));
    }

}
```

Probably the most typical case where a `java.lang.NullPointerException` could be thrown on accident is when calling an instance method or field of an object that has (unknowingly) been set to `null`.  Therefore, we begin with the `callInstanceMethodOfNull()` method, which creates a new `age` variable, sets it to `null`, and then attempts to call the `age.toString()` method:

```java
private static void callInstanceMethodOfNull()
{
    try
    {
        // Instantiate null object.
        Integer age = null;
        // Attempt to call method on object.
        age.toString();
    }
    catch (java.lang.NullPointerException exception)
    {
        // Catch NullPointerExceptions.
        Logging.log(exception);
    }
    catch (Throwable exception)
    {
        // Catch other Throwables.
        Logging.log(exception, false);
    }
}
```

As you can probably guess, executing the `callInstanceMethodOfNull()` method results in a `java.lang.NullPointerException` coming our way, as we see in the log output:

```java
[EXPECTED] java.lang.NullPointerException
java.lang.NullPointerException
	at io.airbrake.NullPointerException.callInstanceMethodOfNull(NullPointerException.java:22)
	at io.airbrake.NullPointerException.main(NullPointerException.java:11)
```

The obvious fix in the case above would be to confirm that a mutable object, like `age`, is not `null` prior to calling a method on it:

```java
// ...
// Instantiate null object.
Integer age = null;
// Check for null.
if (age == null) return;
// Attempt to call method on object.
age.toString();
// ...
```

We also have defined a simple `Book` class as part of our example, which we'll use within the `callInstanceFieldOfNull()` method:

```java
/**
 * Simple example class to store book instances.
 */
public class Book
{
    public String title;
    public String author;

    public Book() { }

    public Book(String title, String author) {
        this.title = title;
        this.author = author;
    }
}

private static void callInstanceFieldOfNull()
{
    try
    {
        // Instantiate a new Book.
        Book book = new Book("The Stand", "Stephen King");
        // Set to null.
        book = null;
        // Call null field.
        Logging.log(String.format("Author is: %s", book.author));
    }
    catch (java.lang.NullPointerException exception)
    {
        // Catch NullPointerExceptions.
        Logging.log(exception);
    }
    catch (Throwable exception)
    {
        // Catch other Throwables.
        Logging.log(exception, false);
    }
}
```

As you can see, we're just using `Book` here to illustrate the creation of a custom class instance object.  We then set that new `book` object to `null`, then try to access a the `book.author` field in a log output.  As suspected, this also throws a `java.lang.NullPointerException` at us:

```java
[EXPECTED] java.lang.NullPointerException
java.lang.NullPointerException
	at io.airbrake.NullPointerException.callInstanceFieldOfNull(NullPointerException.java:43)
	at io.airbrake.NullPointerException.main(NullPointerException.java:12)
```

Again, the simple solution is just to check that our `book` object isn't `null` prior to making the field call.  In some cases it might be ideal to directly throw a _different_ exception in the event that an object your code is using is `null` when it shouldn't be:

```java
// ..
// Instantiate a new Book.
Book book = new Book("The Stand", "Stephen King");
// Set to null.
book = null;
// Check if null.
if (book == null) throw new IllegalArgumentException("Book object is null and cannot be processed.");
// Call null field.
Logging.log(String.format("Author is: %s", book.author));
// ..
```

Obviously, in the case above we're just moving from one type of exception to the other, but this is often a good practice if the circumstances are appropriate.  Executing this new method shows our new (unexpected) exception was caught by our `catch (Throwable exception)` block, which is generally a bad practice but helps to illustrate this particular scenario:

```
[UNEXPECTED] java.lang.IllegalArgumentException: Book object is null and cannot be processed.
java.lang.IllegalArgumentException: Book object is null and cannot be processed.
	at io.airbrake.NullPointerException.callInstanceFieldOfNull(NullPointerException.java:45)
	at io.airbrake.NullPointerException.main(NullPointerException.java:12)
```

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look into the java.lang.NullPointerException in Java, including functional code examples illustrating how null objects might be dealt with.

---

__SOURCES__

- https://en.wikibooks.org/wiki/Java_Programming/Preventing_NullPointerException