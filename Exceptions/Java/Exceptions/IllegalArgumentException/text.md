# Java Exception Handling - IllegalArgumentException

Moving right along through our in-depth [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series, today we'll be digging into java.lang.IllegalArgumentException.  The `IllegalArgumentException` is intended to be used anytime a method is called with any argument(s) that is improper, for whatever reason.

We'll spend the few minutes of this article exploring the `IllegalArgumentException` in greater detail by examining where it resides in the [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).  We'll also show some simple example code that will illustrate how you might use `IllegalArgumentExceptions` in your own Java projects, so let's get going!

## The Technical Rundown

- All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.
- [`java.lang.Exception`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html) inherits from [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).
- [`java.lang.RuntimeException`](https://docs.oracle.com/javase/8/docs/api/java/lang/RuntimeException.html) inherits from [`java.lang.Exception`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html).
- Finally, `java.lang.IllegalArgumentException` inherits from [`java.lang.RuntimeException`](https://docs.oracle.com/javase/8/docs/api/java/lang/RuntimeException.html)/

## When Should You Use It?

As previously mentioned, the `IllegalArgumentException` should be thrown when a value is provided to an argument that just doesn't quite work within the business logic of the application, _but_ isn't strictly illegal from the perspective of the JVM.  For example, here we have a method that accepts an `String` parameter called `title`:

```java
/**
* Set title of book.
*
* @param title Title.
*/
public void setTitle(String title) {
    this.title = title;
}

// Try to pass an int value.
setTitle(123);
```

If we attempt to call this method while passing a non-String value (such as an `int`), as seen above, the compiler will catch the issue and prevent us from even executing the application.  In this case, the compiler issues an error indicating that `int` cannot be converted to a `String`:

```
Error:(41, 18) java: incompatible types: int cannot be converted to java.lang.String
```

Therefore, the only time an `IllegalArgumentException` is raised is if the method in question was _explicitly designed_ to `throw` such an error under certain circumstances -- situations that the compiler cannot detect are problems, but the author of the code deems improper.

To illustrate how to properly use an `IllegalArgumentException`, we have a bit of code.  The full sample is provided below for easy copy/pasting, after which we'll dig into the important bits and explore what's going on:

```java
package io.airbrake;

import io.airbrake.utility.*;

import java.util.ArrayList;
import java.util.List;

public class Main {

    public static void main(String[] args) {
        Logging.lineSeparator("VALID EXAMPLE");

        generateLibrary();

        Logging.lineSeparator("INVALID EXAMPLE");

        generateLibraryInvalid();
    }

    /**
     * Create a library Book collection, including an exceedingly length book.
     */
    private static void generateLibraryInvalid() {
        try {
            // Create list of Books.
            List<Book> library = new ArrayList<>();

            // Add a few new Books to list.
            library.add(new Book("The Pillars of the Earth", "Ken Follett", 973));
            library.add(new Book("A Game of Thrones", "George R.R. Martin", 835));
            // Output library size.
            Logging.log(String.format("Library contains %d books.", library.size()));

            // Add another, very lengthy book.
            library.add(new Book("In Search of Lost Time", "Marcel Proust", 4215));

            // Output latest Book addition and updated library size.
            Logging.log(library.get(library.size() - 1));
            Logging.log(String.format("Library contains %d books.", library.size()));
        } catch (IllegalArgumentException exception) {
            // Catch expected IllegalArgumentExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Catch unexpected Exceptions.
            Logging.log(exception, false);
        }
    }

    /**
     * Create a library Book collection.
     */
    private static void generateLibrary() {
        try {
            // Create list of Books.
            List<Book> library = new ArrayList<>();

            // Add a few new Books to list.
            library.add(new Book("His Dark Materials", "Philip Pullman", 399));
            library.add(new Book("Life of Pi", "Yann Martel", 460));
            // Output library size.
            Logging.log(String.format("Library contains %d books.", library.size()));

            // Add another book.
            library.add(new Book("Les Misérables", "Victor Hugo", 1463));

            // Output latest Book addition and updated library size.
            Logging.log(library.get(library.size() - 1));
            Logging.log(String.format("Library contains %d books.", library.size()));
        } catch (IllegalArgumentException exception) {
            // Catch expected IllegalArgumentExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Catch unexpected Exceptions.
            Logging.log(exception, false);
        }
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

    private static final Integer maximumPageCount = 4000;

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
    public void setPageCount(Integer pageCount) throws IllegalArgumentException {
        if (pageCount > maximumPageCount) {
            throw new IllegalArgumentException(String.format("Page count value [%d] exceeds maximum limit [%d].", pageCount, maximumPageCount));
        }
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

// Logging.java
package io.airbrake.utility;

import java.util.Arrays;
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

---

Outside of our `Book` and `Logging` helper classes, all our logic takes place in two methods, `Main.generateLibrary()` and `Main.generateLibraryInvalid()`.  We start with the working method of the pair:

```java
/**
    * Create a library Book collection.
    */
private static void generateLibrary() {
    try {
        // Create list of Books.
        List<Book> library = new ArrayList<>();

        // Add a few new Books to list.
        library.add(new Book("His Dark Materials", "Philip Pullman", 399));
        library.add(new Book("Life of Pi", "Yann Martel", 460));
        // Output library size.
        Logging.log(String.format("Library contains %d books.", library.size()));

        // Add another book.
        library.add(new Book("Les Misérables", "Victor Hugo", 1463));

        // Output latest Book addition and updated library size.
        Logging.log(library.get(library.size() - 1));
        Logging.log(String.format("Library contains %d books.", library.size()));
    } catch (IllegalArgumentException exception) {
        // Catch expected IllegalArgumentExceptions.
        Logging.log(exception);
    } catch (Exception exception) {
        // Catch unexpected Exceptions.
        Logging.log(exception, false);
    }
}
```

Nothing too crazy going on here.  We create a little `library` list of `Books` and output the count, along with the final book we've added at the end (`"Les Misérables"`).  Pay special attention to the final page count argument that is passed for the final `Book` added to the list in these examples (`1463` in the case of `"Les Misérables"`). The output should be just as expected with no thrown exceptions:

```
------------ VALID EXAMPLE -------------
Library contains 2 books.
io.airbrake.Book@78308db1[
  author=Victor Hugo
  title=Les Misérables
  pageCount=1463
]
Library contains 3 books.
```

The `generateLibraryInvalid()` method is similar in that we add two books, output the count, then output a third book.  However, in this case the third addition is `"In Search of Lost Time"` by `Marcel Proust`, a notoriously lengthy book by any measure at around `4215` pages all told:

```java
/**
* Create a library Book collection, including an exceedingly length book.
*/
private static void generateLibraryInvalid() {
    try {
        // Create list of Books.
        List<Book> library = new ArrayList<>();

        // Add a few new Books to list.
        library.add(new Book("The Pillars of the Earth", "Ken Follett", 973));
        library.add(new Book("A Game of Thrones", "George R.R. Martin", 835));
        // Output library size.
        Logging.log(String.format("Library contains %d books.", library.size()));

        // Add another, very lengthy book.
        library.add(new Book("In Search of Lost Time", "Marcel Proust", 4215));

        // Output latest Book addition and updated library size.
        Logging.log(library.get(library.size() - 1));
        Logging.log(String.format("Library contains %d books.", library.size()));
    } catch (IllegalArgumentException exception) {
        // Catch expected IllegalArgumentExceptions.
        Logging.log(exception);
    } catch (Exception exception) {
        // Catch unexpected Exceptions.
        Logging.log(exception, false);
    }
}
```

While that's very little difference between the logic of these two methods, running this second one results in a thrown `IllegalArgumentException`, indicating that the page count of our last book exceeds the maximum allowed limit of `4000`:

```
----------- INVALID EXAMPLE ------------
Library contains 2 books.
[EXPECTED] java.lang.IllegalArgumentException: Page count value [4215] exceeds maximum limit [4000].
java.lang.IllegalArgumentException: Page count value [4215] exceeds maximum limit [4000].
	at io.airbrake.Book.setPageCount(Book.java:96)
	at io.airbrake.Book.<init>(Book.java:40)
	at io.airbrake.Main.generateLibraryInvalid(Main.java:35)
	at io.airbrake.Main.main(Main.java:17)
```

Why is this exception thrown?  The stack trace gives some indication, but the real reason is we made a small modification to the `Book.setPageCount(Integer)` method definition:

```java
private static final Integer maximumPageCount = 4000;

// ...

/**
* Set page count of book.
*
* @param pageCount Page count.
*/
public void setPageCount(Integer pageCount) throws IllegalArgumentException {
    if (pageCount > maximumPageCount) {
        throw new IllegalArgumentException(String.format("Page count value [%d] exceeds maximum limit [%d].", pageCount, maximumPageCount));
    }
    this.pageCount = pageCount;
}
```

Maybe our publisher has a hard limit on the maximum page length we can produce, but for whatever reason, we've decided that the most pages a `Book` can contain is `4000`.  To confirm this is the case, we ensure that `setPageCount(Integer)` throws an `IllegalArgumentException` anytime the passed `pageCount` argument exceeds that limit.

While this is a simple example, it should illustrate the kind of scenarios in which an `IllegalArgumentException` should be used and explicitly raised in your own code.

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look into the java.lang.IllegalArgumentException in Java, with sample code showing how to properly throw and handle IllegalArgumentExceptions.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html