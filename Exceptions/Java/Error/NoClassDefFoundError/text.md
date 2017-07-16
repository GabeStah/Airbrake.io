# Java Exception Handling - NoClassDefFoundError

Today we continue our adventure through the [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series with a close look at the java.lang.NoClassDefFoundError.  In most cases, a `java.lang.NoClassDefFoundError` is thrown when a class reference was available during compile time, but has since disappeared (for whatever reason) during execution.

In this article we'll explore the `java.lang.NoClassDefFoundError` in more detail, looking at where it resides in the [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy), along with some simple example code and module distribution practices that might lead to `java.lang.NoClassDefFoundErrors` being thrown in day-to-day development.  Let's get on with the show!

## The Technical Rundown

- All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.
- [`java.lang.Error`](https://docs.oracle.com/javase/8/docs/api/java/lang/Error.html) inherits from [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).
- [`java.lange.LinkageError`](https://docs.oracle.com/javase/8/docs/api/java/lang/LinkageError.html) inherits from [`java.lang.Error`](https://docs.oracle.com/javase/8/docs/api/java/lang/Error.html).
- Lastly, `java.lang.NoClassDefFoundError` inherits from the [`java.lange.LinkageError`](https://docs.oracle.com/javase/8/docs/api/java/lang/LinkageError.html) class.

## When Should You Use It?

Understanding what might cause a `java.lang.NoClassDefFoundError` requires an understanding that running a Java application typically hinges on two major steps:

1. The application must be compiled, which forces Java to grab all relevant and referenced packages/modules from the specified `classpath`, before it can generate executable code.
2. The application or target script is then executed, utilizing all previously-compiled packages to properly gain access to the required classes.

That's a extremely simplified explanation, but the critical aspect is that Java cannot execute an application without _first_ compiling all the relevant code.

To illustrate, here we've setup a basic project called `ClassLoader` that contains two independent classes that we'll be using: `io.airbrake.Main` and `io.airbrake.Book`, which we're going to include in our compilation and make use of in the code.

Because Java is so dependant on how directory and file structures are built (at least, without using a helper framework like `Maven` or `Gradle`), for this example it'll be important to show how our project is configured.  Here's our basic file structure, where `coded` entries are files and plain entries are directories:

- ClassLoader
    - Book
        - io
            - airbrake
                - `Book.java`
    - ClassLoader
        - io
            - airbrake
                - utility
                    - `Logging.java`
                - `Main.java`

Normally we'd place classes with identical parent namespacing (e.g `io.airbrake.Book` and `io.airbrake.Main`) into the same directory, but it's important for this example that we separate them.

Once we've compiled these classes we've created an `out/production` directory in our main `ClassLoader` project directory that contains our converted `.class` files (this won't always be true, as it depends on how you configure/build your own Java applications, but the overall concept is the same for most scenarios).  Thus, our compiled output directory structure looks like this:

- ClassLoader
    - out
        - production
            - Book
                - io
                    - airbrake
                        - `Book.class`
            - ClassLoader
                - io
                    - airbrake
                        - utility
                            - `Logging.class`
                        - `Main.class`

Many developers will probably use an IDE like `IntelliJ IDEA` or `Eclipse` when working with Java, which will (attempt) to automatically handle the [`classpath`](https://docs.oracle.com/javase/8/docs/technotes/tools/windows/classpath.html) on your behalf, based on how you configure your project within the editor.  For example, IntelliJ IDEA allows a developer to easily mark any directory as a `"Source Root"`, which informs the editor that it should add that directory to the `classpath` list for the current project during compilation and execution.

For this reason, in _most_ cases your development environment will do everything in its power to prevent you from accidentally getting in a situation where a `java.lang.NoClassDefFoundError` could be thrown.  However, in some rare scenarios, particularly if you're manually altering the `classpath` value, sometimes a `java.lang.NoClassDefFoundError` will pop up.

To illustrate this possibility, we'll finally take a look at the basic code we're using in this example:

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
        this.title = title;
        this.author = author;
    }
}

// Main.java
package io.airbrake;

import io.airbrake.utility.Logging;

public class Main {

    public static void main(String[] args) {
        try {
            // Create a new io.airbrake.Book instance.
            Book book = new Book("The Stand", "Stephen King");
            Logging.log(String.format("Created Book: '%s' written by %s", book.title, book.author));
        } catch (NoClassDefFoundError error) {
            Logging.log(error);
        } catch (Error error) {
            Logging.log(error);
        }
    }
}


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

In essence, all we're doing in `Main.main()` is creating a new `Book` instance then attempting to output a basic info string to the console.  Remember that, in this particular example, the compiled `Main.class` and `Book.class` files are located in slightly different output directories, so we'll need to include both locations in our `classpath` value so things will run smoothly.  Here we're issuing the `java` console command and passing the `-classpath` parameter with both our `ClassLoader` and `Book` subdirectory locations as the two parent directories to be loaded.  We're then calling the `io.airbrake.Main` class, which will automatically run the `Main.main()` method for us:

```bash
$ java -classpath ClassLoader/out/production/ClassLoader:ClassLoader/out/production/Book io.airbrake.Main
Created Book: 'The Stand' written by Stephen King
```

As we can see, the result is what we expected: a new `Book` was created and we output some basic information about it.

Now, watch what happens if we _remove_ the reference to where our compiled `Book` class is located (`ClassLoader/out/production/Book`) in the `-classpath` parameter value:

```bash
$ java -classpath ClassLoader/out/production/ClassLoader io.airbrake.Main
[EXPECTED] java.lang.NoClassDefFoundError: io/airbrake/Book
java.lang.NoClassDefFoundError: io/airbrake/Book
	at io.airbrake.Main.main(Main.java:10)
Caused by: java.lang.ClassNotFoundException: io.airbrake.Book
	at java.net.URLClassLoader.findClass(URLClassLoader.java:381)
	at java.lang.ClassLoader.loadClass(ClassLoader.java:424)
	at sun.misc.Launcher$AppClassLoader.loadClass(Launcher.java:335)
	at java.lang.ClassLoader.loadClass(ClassLoader.java:357)
	... 1 more
```

Lo and behold, we've caused a `java.lang.NoClassDefFoundError` to be thrown, which we were also able to successfully `catch` and output using the `Logging.log()` method.  Essentially, what's happening here is that our _source code_ is configured correctly, and therefore we were able to compile everything just fine, since the JVM was able to locate the necessary `io.airbrake.Book.java` class file that was referenced.  However, because we neglected to reference the compiled version of that `Book.java` class (in this case, in the form of `Book.class`, but often in `.jar` form or otherwise), the JVM couldn't locate `Book` during runtime, so a `java.lang.NoClassDefFoundError` is thrown.

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look into the java.lang.NoClassDefFoundError in Java, including code that shows how project structure impacts compilation and execution.

---

__SOURCES__

- http://javarevisited.blogspot.com/2011/06/noclassdeffounderror-exception-in.html
- https://dzone.com/articles/how-resolve-0