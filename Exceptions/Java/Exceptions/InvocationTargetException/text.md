# Java Exception Handling - InvocationTargetException

Moving along through our in-depth [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series, today we'll take a closer look at the java.lang.reflect.InvocationTargetException.  The `java.lang.reflect.InvocationTargetException` is thrown when working with the reflection API while attempting to `invoke` a method that throws an underlying exception itself.

In this article we'll explore the `InvocationTargetException` in more detail by looking at where it resides in the [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).  We'll also dig into a few functional reflection code samples that will illustrate how `java.lang.reflect.InvocationTargetExceptions` are typically thrown, showing how you might handle them in your own code, so let's get crackin'!

## The Technical Rundown

- All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.
- [`java.lang.Exception`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html) inherits from [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).
- [`java.lang.ReflectiveOperationException`](https://docs.oracle.com/javase/8/docs/api/java/lang/ReflectiveOperationException.html) inherits from [`java.lang.Exception`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html).
- Finally, `java.lang.reflect.InvocationTargetException` inherits from [`java.lang.ReflectiveOperationException`](https://docs.oracle.com/javase/8/docs/api/java/lang/ReflectiveOperationException.html).

## When Should You Use It?

Since the `InvocationTargetException` deals with reflection, let's briefly talk a bit about that practice and why explicitly invoking a method via reflection might be useful.  In the most basic sense, `reflection` allows the JVM and your underlying code to inspect classes, methods, interfaces, and the like during runtime, _without_ having directly called or identified those objects during compilation.  This powerful capability means that your code can find classes and invoke methods that it wasn't originally designed to handle out of the box.

To illustrate how invocation in Java works we have a few basic examples.  To keep things simple, we'll start with the full working code sample below, after which we'll break it down in more detail to see what's really going on:

```java
// Main.java
package io.airbrake;

import io.airbrake.utility.Logging;
import java.lang.reflect.*;

public class Main {

    public static void main(String[] args) {
        invokeGetTaglineMethod();
        invokeThrowExceptionMethod();
    }

    private static void invokeGetTaglineMethod() {
        try {
            // Instantiate a new Book object.
            Book book = new Book("The Pillars of the Earth", "Ken Follett", 973);
            // Output book.
            Logging.log(book);
            // Instantiate a Class object for the Book class.
            Class<?> bookClass = Class.forName("io.airbrake.Book");
            // Get an instance of the getTagline method from Book.
            Method getTagline = bookClass.getDeclaredMethod("getTagline");
            // Output result of invoking book.getTagline() method.
            Logging.log(getTagline.invoke(book));
        } catch (InvocationTargetException |
                 ClassNotFoundException |
                 NoSuchMethodException |
                 IllegalAccessException exception) {
            // Catch expected Exceptions.
            Logging.log(exception);
            // Find underlying causal Exception.
            if (exception.getCause() != null) {
                Logging.log(exception.getCause());
            }
        } catch (Exception exception) {
            // Catch unexpected Exceptions.
            Logging.log(exception, false);
        }
    }

    private static void invokeThrowExceptionMethod() {
        try {
            // Instantiate a new Book object.
            Book book = new Book("The Stand", "Stephen King", 823);
            // Output book.
            Logging.log(book);
            // Instantiate a Class object for the Book class.
            Class<?> bookClass = Class.forName("io.airbrake.Book");
            // Get an instance of the throwException method from Book.
            Method throwException = bookClass.getDeclaredMethod("throwException", String.class);
            // Output result of invoking book.throwException() method.
            Logging.log(throwException.invoke(book, "Uh oh, this is an Exception message!"));
        } catch (InvocationTargetException |
                 ClassNotFoundException |
                 NoSuchMethodException |
                 IllegalAccessException exception) {
            // Catch expected Exceptions.
            Logging.log(exception);
            // Find underlying causal Exception.
            if (exception.getCause() != null) {
                Logging.log(exception.getCause());
            }
        } catch (Exception exception) {
            // Catch unexpected Exceptions.
            Logging.log(exception, false);
        }
    }
}

// Logging.java
package io.airbrake.utility;

import java.util.Arrays;
import org.apache.commons.lang3.builder.*;

/**
 * Houses all logging methods for various debug outputs.
 */
public class Logging {

    /**
     * Outputs any kind of Object.
     * Uses ReflectionToStringBuilder from Apache commons-lang library.
     *
     * @param value Object to be output.
     */
    public static void log(Object value)
    {
        if (value instanceof String)
        {
            System.out.println(value);
        }
        else
        {
            System.out.println(new ReflectionToStringBuilder(value, ToStringStyle.MULTI_LINE_STYLE).toString());
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

// Book.java
package io.airbrake;

/**
 * Simple example class to store book instances.
 */
public class Book
{
    private String author;
    private String title;
    private Integer pageCount;

    /**
     * Constructs an empty book.
     */
    public Book() { }

    /**
     * Constructs a basic book.
     *
     * @param title Book title.
     * @param author Book author.
     */
    public Book(String title, String author) {
        setAuthor(author);
        setTitle(title);
    }

    /**
     * Constructs a basic book, with page count.
     *
     * @param title Book title.
     * @param author Book author.
     * @param pageCount Book page count.
     */
    public Book(String title, String author, Integer pageCount) {
        setAuthor(author);
        setPageCount(pageCount);
        setTitle(title);
    }

    /**
     * Get author of book.
     *
     * @return Author name.
     */
    public String getAuthor() {
        return author;
    }

    /**
     * Get page count of book.
     *
     * @return Page count.
     */
    public Integer getPageCount() {
        return pageCount;
    }

    /**
     * Get a formatted tagline with author, title, and page count.
     *
     * @return Formatted tagline.
     */
    public String getTagline() {
        return String.format("'%s' by %s is %d pages.", this.title, this.author, this.pageCount);
    }

    /**
     * Get title of book.
     *
     * @return Title.
     */
    public String getTitle() {
        return title;
    }

    /**
     * Set author of book.
     *
     * @param author Author name.
     */
    public void setAuthor(String author) {
        this.author = author;
    }

    /**
     * Set page count of book.
     *
     * @param pageCount Page count.
     */
    public void setPageCount(Integer pageCount) {
        this.pageCount = pageCount;
    }

    /**
     * Set title of book.
     *
     * @param title Title.
     */
    public void setTitle(String title) {
        this.title = title;
    }

    /**
     * Throw an Exception.
     */
    public void throwException(String message) throws Exception {
        throw new Exception(message);
    }
}
```

---

As briefly mentioned in the introduction, an `InvocationTargetException` is automatically generated by reflection-related objects, and wraps (or attaches) itself to the underlying, actual exception type that caused the problem.  This occurs when calling the [`java.lang.reflect.Method.invoke()`](https://docs.oracle.com/javase/8/docs/api/java/lang/reflect/Method.html#invoke-java.lang.Object-java.lang.Object...-) method, where that invocation target method throws an exception.

To illustrate the issue we've got a basic `Book` class with a few property getter/setter methods, along with a some extraneous methods (`getTagline()` and `throwException()`) that we'll explicitly call in the rest of our code:

```java
// Book.java
package io.airbrake;

/**
 * Simple example class to store book instances.
 */
public class Book
{
    private String author;
    private String title;
    private Integer pageCount;

    /**
     * Constructs an empty book.
     */
    public Book() { }

    /**
     * Constructs a basic book.
     *
     * @param title Book title.
     * @param author Book author.
     */
    public Book(String title, String author) {
        setAuthor(author);
        setTitle(title);
    }

    /**
     * Constructs a basic book, with page count.
     *
     * @param title Book title.
     * @param author Book author.
     * @param pageCount Book page count.
     */
    public Book(String title, String author, Integer pageCount) {
        setAuthor(author);
        setPageCount(pageCount);
        setTitle(title);
    }

    /**
     * Get author of book.
     *
     * @return Author name.
     */
    public String getAuthor() {
        return author;
    }

    /**
     * Get page count of book.
     *
     * @return Page count.
     */
    public Integer getPageCount() {
        return pageCount;
    }

    /**
     * Get a formatted tagline with author, title, and page count.
     *
     * @return Formatted tagline.
     */
    public String getTagline() {
        return String.format("'%s' by %s is %d pages.", this.title, this.author, this.pageCount);
    }

    /**
     * Get title of book.
     *
     * @return Title.
     */
    public String getTitle() {
        return title;
    }

    /**
     * Set author of book.
     *
     * @param author Author name.
     */
    public void setAuthor(String author) {
        this.author = author;
    }

    /**
     * Set page count of book.
     *
     * @param pageCount Page count.
     */
    public void setPageCount(Integer pageCount) {
        this.pageCount = pageCount;
    }

    /**
     * Set title of book.
     *
     * @param title Title.
     */
    public void setTitle(String title) {
        this.title = title;
    }

    /**
     * Throw an Exception.
     */
    public void throwException() throws Exception {
        throw new Exception("Uh oh, this is an Exception message!");
    }
}
```

The `invokeGetTaglineMethod()` method shows a basic, working example of using Java reflection to `invoke()` a class instance method (`Book.getTagline()`, in this case).

```java
private static void invokeGetTaglineMethod() {
    try {
        // Instantiate a new Book object.
        Book book = new Book("The Pillars of the Earth", "Ken Follett", 973);
        // Output book.
        Logging.log(book);
        // Instantiate a Class object for the Book class.
        Class<?> bookClass = Class.forName("io.airbrake.Book");
        // Get an instance of the getTagline method from Book.
        Method getTagline = bookClass.getDeclaredMethod("getTagline");
        // Output result of invoking book.getTagline() method.
        Logging.log(getTagline.invoke(book));
    } catch (InvocationTargetException |
             ClassNotFoundException |
             NoSuchMethodException |
             IllegalAccessException exception) {
        // Catch expected Exceptions.
        Logging.log(exception);
        // Find underlying causal Exception.
        if (exception.getCause() != null) {
            Logging.log(exception.getCause());
        }
    } catch (Exception exception) {
        // Catch unexpected Exceptions.
        Logging.log(exception, false);
    }
}
```

The comments explain most everything that we're doing, but the basic idea is that we starting by creating a `Book` instance object of `book`.  We then need to instantiate a `Class<?>` object, so we get a reflection of `io.airbrake.Book`, from which we can retrieve the declared `getTagline()` method.  Using that `Method` instance of `getTagline()` we're able to `invoke()` the method.  Since this is an instance method, we need to pass an instance of the object as the first argument, so this is where we use the `book` object that was created at the beginning.

Executing this code results in the `book` object output, followed by the output from calling `getTagline.invoke(book)`, as expected:

```
io.airbrake.Book@532760d8[
  author=Ken Follett
  title=The Pillars of the Earth
  pageCount=973
]
'The Pillars of the Earth' by Ken Follett is 973 pages.
```

Now that we've seen how invocation works, let's try invoking a different `Book` instance method, specifically the `Book.throwException()` method.  As its name suggests, `Book.throwException()` merely throws an `Exception`, using the passed `String` argument for the message:

```java
public class Book
{
    /**
     * Throw an Exception.
     */
    public void throwException(String message) throws Exception {
        throw new Exception(message);
    }
}
```

To test this we'll execute the `invokeThrowExceptionMethod()` method, which performs almost exactly the same code as our previous example, except the calls to `getDeclaredMethod()` and `invoke()` differ slightly since we're also passing arguments to the invoked method:

```java
private static void invokeThrowExceptionMethod() {
    try {
        // Instantiate a new Book object.
        Book book = new Book("The Stand", "Stephen King", 823);
        // Output book.
        Logging.log(book);
        // Instantiate a Class object for the Book class.
        Class<?> bookClass = Class.forName("io.airbrake.Book");
        // Get an instance of the throwException method from Book.
        Method throwException = bookClass.getDeclaredMethod("throwException", String.class);
        // Output result of invoking book.throwException() method.
        Logging.log(throwException.invoke(book, "Uh oh, this is an Exception message!"));
    } catch (InvocationTargetException |
             ClassNotFoundException |
             NoSuchMethodException |
             IllegalAccessException exception) {
        // Catch expected Exceptions.
        Logging.log(exception);
        // Find underlying causal Exception.
        if (exception.getCause() != null) {
            Logging.log(exception.getCause());
        }
    } catch (Exception exception) {
        // Catch unexpected Exceptions.
        Logging.log(exception, false);
    }
}
```

Since the `Book.throwException()` method we're invoking explicitly throws an error, our expectation is that executing the above code will result in a `java.lang.reflect.InvocationTargetException` being thrown.  Sure enough, that's exactly what happens:

```
io.airbrake.Book@3ada9e37[
  author=Stephen King
  title=The Stand
  pageCount=823
]

[EXPECTED] java.lang.reflect.InvocationTargetException
java.lang.reflect.InvocationTargetException <3 internal calls>
	at java.lang.reflect.Method.invoke(Method.java:498)
	at io.airbrake.Main.invokeThrowExceptionMethod(Main.java:52)
	at io.airbrake.Main.main(Main.java:10)
Caused by: java.lang.Exception: Uh oh, this is an Exception message!
	at io.airbrake.Book.throwException(Book.java:109)
	... 6 more

[EXPECTED] java.lang.Exception: Uh oh, this is an Exception message!
java.lang.Exception: Uh oh, this is an Exception message!
	at io.airbrake.Book.throwException(Book.java:109)  <3 internal calls>
	at java.lang.reflect.Method.invoke(Method.java:498)
	at io.airbrake.Main.invokeThrowExceptionMethod(Main.java:52)
	at io.airbrake.Main.main(Main.java:10)
```

Normally we'd only catch the parent `InvocationTargetException` in this scenario, which indicates the it was `"Caused by: java.lang.Exception ..."`.  However, a `Throwable` object implements the [`getCause()`](https://docs.oracle.com/javase/7/docs/api/java/lang/Throwable.html#getCause()) method, which returns the causal exception (if applicable).  In the case of a `InvocationTargetException` instance, the cause is always the underlying exception object that occurred inside the invoked method, so checking for a cause when catching such exceptions is a good idea.

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A deep dive into the java.lang.reflect.InvocationTargetException in Java, including samples that show how to use reflection to invoke class methods.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/util/ConcurrentModificationException.html