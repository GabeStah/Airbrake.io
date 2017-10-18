# Java Exception Handling - NoSuchMethodError

Making our way through our comprehensive [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) article series, today we'll be looking into the **NoSuchMethodError**.  As you may suspect based solely on the name, the `NoSuchMethodError` is thrown when a class can no longer locate the method definition of the specified method.  While this error is typically caught by the compiler, it's possible to find yourself in scenarios where it can occur during runtime, such as when a class was modified without other dependant classes being updated.

In this article we'll explore the `NoSuchMethodError` in more detail by first looking at where it sits in the larger [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).  We'll then go over some functional sample code that illustrates how `NoSuchMethodErrors` might actually occur during runtime, depending on your own particular development habits and practices.  With that, let's get this party started!

## The Technical Rundown

All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`java.lang.Object`](https://docs.oracle.com/javase/8/docs/api/java/lang/Object.html)
    - [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy)
        - [`java.lang.Error`](https://docs.oracle.com/javase/8/docs/api/java/lang/Error.html)
            - [`java.lang.LinkageError`](https://docs.oracle.com/javase/8/docs/api/java/lang/LinkageError.html)
                - [`java.lang.IncompatibleClassChangeError`](https://docs.oracle.com/javase/8/docs/api/java/lang/IncompatibleClassChangeError.html)
                    - `NoSuchMethodError`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```java
package io.airbrake;

import io.airbrake.utility.Logging;

import java.util.GregorianCalendar;

public class Main {

    public static void main(String[] args) {
        Logging.lineSeparator("CREATING AND OUTPUTTING BOOK");
        createBook();
        Logging.lineSeparator("CREATING AND OUTPUTTING POEM");
        createPoem();
    }

    public static void createBook() {
        try {
            Book book = new Book(
                    "A Game of Thrones",
                    "George R.R. Martin",
                    848,
                    new GregorianCalendar(1996, 8, 6).getTime(),
                    "novel"
            );
            Logging.log(book.toFormattedString());
        } catch (NoSuchMethodError error) {
            // Output expected NoSuchMethodErrors.
            Logging.log(error);
        } catch (Exception | Error exception) {
            // Output unexpected Exceptions/Errors.
            Logging.log(exception, false);
        }
    }

    public static void createPoem() {
        try {
            Book poem = new Book(
                    "The Raven",
                    "Edgar Allan Poe",
                    2,
                    new GregorianCalendar(1845, 0, 29).getTime(),
                    "poem"
            );
            Logging.log(poem.toFormattedString());
        } catch (NoSuchMethodError error) {
            // Output expected NoSuchMethodErrors.
            Logging.log(error);
        } catch (Exception | Error exception) {
            // Output unexpected Exceptions/Errors.
            Logging.log(exception, false);
        }
    }
}
```

```java
// Book.java
package io.airbrake;

import java.util.Date;

/**
 * Simple example class to store book instances.
 */
public class Book
{
    private String author;
    private String title;
    private Integer pageCount;
    private Date publishedAt;
    private static String publicationType = "Book";

    private static final Integer maximumPageCount = 4000;

    /**
     * Ensure publication type is upper case.
     */
    static {
        publicationType = publicationType.toUpperCase();
    }

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
     * Constructs a basic book, with page count.
     *
     * @param title Book title.
     * @param author Book author.
     * @param pageCount Book page count.
     */
    public Book(String title, String author, Integer pageCount, Date publishedAt) {
        setAuthor(author);
        setPageCount(pageCount);
        setTitle(title);
        setPublishedAt(publishedAt);
    }

    /**
     * Constructs a basic book, with page count.
     *
     * @param title Book title.
     * @param author Book author.
     * @param pageCount Book page count.
     */
    public Book(String title, String author, Integer pageCount, Date publishedAt, String publicationType) {
        setAuthor(author);
        setPageCount(pageCount);
        setTitle(title);
        setPublishedAt(publishedAt);
        setPublicationType(publicationType);
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
     * Get publication type of book.
     *
     * @return Publication type.
     */
    public String getPublicationType() { return publicationType; }

    /**
     * Get published date of book.
     *
     * @return Published date.
     */
    public Date getPublishedAt() { return publishedAt; }

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
     * Publish current book.
     * If book already published, throws IllegalStateException.
     */
    public void publish() throws IllegalStateException {
        Date publishedAt = getPublishedAt();
        if (publishedAt == null) {
            setPublishedAt(new Date());
            System.out.println(String.format("Published '%s' by %s.", getTitle(), getAuthor()));
        } else {
            throw new IllegalStateException(
                    String.format("Cannot publish '%s' by %s (already published on %s).",
                            getTitle(),
                            getAuthor(),
                            publishedAt));
        }
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
     * Set publication type of book.
     *
     * @param type Publication type.
     */
    public void setPublicationType(String type) { this.publicationType = type; }

    /**
     * Set published date of book.
     *
     * @param publishedAt Page count.
     */
    public void setPublishedAt(Date publishedAt) {
        this.publishedAt = publishedAt;
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
     * Gets a formatted string representation of Book.
     *
     * @return String Formatted string of Book.
     */
    public String toFormattedString() {
        return String.format("'%s' by %s is %s pgs and published on %s.",
                getTitle(),
                getAuthor(),
                getPageCount(),
                getPublishedAt());
    }

    /**
     * Throw an Exception.
     */
    public void throwException(String message) throws Exception {
        throw new Exception(message);
    }
}
```

```java
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

As mentioned, in most cases a potential `NoSuchMethodError` will be caught by the compiler or integrated development environment (`IDE`) you're working with.  For example, the [`Jet Brains IntelliJ IDEA`](https://www.jetbrains.com/idea/) IDE explicitly highlights and provides an internal error in the editor when a code statement references a class method that no longer exists.  Thus, in most scenarios, it's fairly unlikely to see a `NoSuchMethodError` during actual runtime.  However, that doesn't mean it's not possible, particularly as projects get larger or multiple people become involved.  All it takes to see a runtime `NoSuchMethodError` is to have one class modified and recompiled, while a dependant class _does not_ get recompiled at the same time.  In those situations, the outdated class may still reference a class method that no longer exists, causing a runtime `NoSuchMethodError`.

To see this in action with functional code we start with our basic `Book` class:

```java
// Book.java
package io.airbrake;

import java.util.Date;

/**
 * Simple example class to store book instances.
 */
public class Book
{
    private String author;
    private String title;
    private Integer pageCount;
    private Date publishedAt;
    private static String publicationType = "Book";

    private static final Integer maximumPageCount = 4000;

    /**
     * Ensure publication type is upper case.
     */
    static {
        publicationType = publicationType.toUpperCase();
    }

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
     * Constructs a basic book, with page count.
     *
     * @param title Book title.
     * @param author Book author.
     * @param pageCount Book page count.
     */
    public Book(String title, String author, Integer pageCount, Date publishedAt) {
        setAuthor(author);
        setPageCount(pageCount);
        setTitle(title);
        setPublishedAt(publishedAt);
    }

    /**
     * Constructs a basic book, with page count.
     *
     * @param title Book title.
     * @param author Book author.
     * @param pageCount Book page count.
     */
    public Book(String title, String author, Integer pageCount, Date publishedAt, String publicationType) {
        setAuthor(author);
        setPageCount(pageCount);
        setTitle(title);
        setPublishedAt(publishedAt);
        setPublicationType(publicationType);
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
     * Get publication type of book.
     *
     * @return Publication type.
     */
    public String getPublicationType() { return publicationType; }

    /**
     * Get published date of book.
     *
     * @return Published date.
     */
    public Date getPublishedAt() { return publishedAt; }

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
     * Publish current book.
     * If book already published, throws IllegalStateException.
     */
    public void publish() throws IllegalStateException {
        Date publishedAt = getPublishedAt();
        if (publishedAt == null) {
            setPublishedAt(new Date());
            System.out.println(String.format("Published '%s' by %s.", getTitle(), getAuthor()));
        } else {
            throw new IllegalStateException(
                    String.format("Cannot publish '%s' by %s (already published on %s).",
                            getTitle(),
                            getAuthor(),
                            publishedAt));
        }
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
     * Set publication type of book.
     *
     * @param type Publication type.
     */
    public void setPublicationType(String type) { this.publicationType = type; }

    /**
     * Set published date of book.
     *
     * @param publishedAt Page count.
     */
    public void setPublishedAt(Date publishedAt) {
        this.publishedAt = publishedAt;
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
     * Gets a formatted string representation of Book.
     *
     * @return String Formatted string of Book.
     */
    public String toFormattedString() {
        return String.format("'%s' by %s is %s pgs and published on %s.",
                getTitle(),
                getAuthor(),
                getPageCount(),
                getPublishedAt());
    }

    /**
     * Throw an Exception.
     */
    public void throwException(String message) throws Exception {
        throw new Exception(message);
    }
}
```

Nothing special going on here, we're just using this to represent a possible scenario of the "causal" class that might be updated and recompiled.  Our `Main` class then has the `createBook()` method that does just as it says, creating a `Book` instance:

```java
public static void createBook() {
    try {
        Book book = new Book(
                "A Game of Thrones",
                "George R.R. Martin",
                848,
                new GregorianCalendar(1996, 8, 6).getTime(),
                "novel"
        );
        Logging.log(book.toFormattedString());
    } catch (NoSuchMethodError error) {
        // Output expected NoSuchMethodErrors.
        Logging.log(error);
    } catch (Exception | Error exception) {
        // Output unexpected Exceptions/Errors.
        Logging.log(exception, false);
    }
}
```

Notice that we're explicitly calling the `book.toFormattedString()` method, which outputs the `Book` into a formatted string as seen below:

```java
public String toFormattedString() {
    return String.format("'%s' by %s is %s pgs and published on %s.",
            getTitle(),
            getAuthor(),
            getPageCount(),
            getPublishedAt());
}
```

It's worth noting that we _normally_ would override the built-in `toString()` method of the `Book` class for this purpose, but since Java automatically generates that method if it's not explicitly defined in a class definition, it wouldn't work for our purposes here.

Anyway, executing `createBook()` works fine and produces the output that we expect:

```
----- CREATING AND OUTPUTTING BOOK -----
'A Game of Thrones' by George R.R. Martin is 848 pgs and published on Fri Sep 06 00:00:00 PDT 1996.
```

Now comes the trouble.  Imagine we've made some changed to the `Book` class.  Specifically, we've removed (or renamed) the `toFormattedString()` method.  For our purposes here, we'll just rename it to `toString()`, since that is the "traditional" way to handle this:

```java
public String toString() {
    return String.format("'%s' by %s is %s pgs and published on %s.",
            getTitle(),
            getAuthor(),
            getPageCount(),
            getPublishedAt());
}
```

Now, let's recompile _just_ the modified `Book` class:

```bash
$ javac io/airbrake/Book.java
```

Now, our `Main` class hasn't changed, and it also includes a second method for creating a poem instance of `Book` this time:

```java
public static void createPoem() {
    try {
        Book poem = new Book(
                "The Raven",
                "Edgar Allan Poe",
                2,
                new GregorianCalendar(1845, 0, 29).getTime(),
                "poem"
        );
        Logging.log(poem.toFormattedString());
    } catch (NoSuchMethodError error) {
        // Output expected NoSuchMethodErrors.
        Logging.log(error);
    } catch (Exception | Error exception) {
        // Output unexpected Exceptions/Errors.
        Logging.log(exception, false);
    }
}
```

Executing the `createPoem()` method now throws a `NoSuchMethodError`:

```
----- CREATING AND OUTPUTTING POEM -----
[EXPECTED] java.lang.NoSuchMethodError: io.airbrake.Book.toFormattedString()Ljava/lang/String;
```

As we can see, the problem here is that the call to `poem.toFormattedString()` references a method that no longer exists, since we renamed it to `toString()` and recompiled the `Book` class.

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look at the Java NoSuchMethodError, with functional code samples showing how such errors might be thrown during runtime execution.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html