# Java Exception Handling - ClassCastException

Making our way through our detailed [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series, today we'll be going over the **ClassCastException**.  Any attempt to `cast` (i.e. convert) an object to another class for which the original class isn't a inherited from will result in a `ClassCastException`.

In this article we'll examine the `ClassCastException` by looking at where it sits in the grand [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).  We'll also take a gander at some functional code samples that will illustrate how the `ClassCastException` is commonly thrown, and thus, how it can be avoided.  Let's get started!

## The Technical Rundown

All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`java.lang.Object`](https://docs.oracle.com/javase/8/docs/api/java/lang/Object.html)
    - [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy)
        - [`java.lang.Exception`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html)
            - [`java.lang.RuntimeException`](https://docs.oracle.com/javase/8/docs/api/java/lang/RuntimeException.html)
                - `ClassCastException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```java
// Main.java
package io.airbrake;

import io.airbrake.utility.Logging;
import org.jetbrains.annotations.Nullable;

import java.util.GregorianCalendar;

public class Main {

    public static void main(String[] args) {

        // Create a new PaperbackBook.
        Book paperbackBook = new PaperbackBook(
                "The Revenant",
                "Michael Punke",
                272,
                new GregorianCalendar(2015, 1, 6).getTime());
        Logging.log(paperbackBook);

        // Create a new DigitalBook.
        Book digitalBook = new DigitalBook(
                "Magician",
                "Raymond E. Feist",
                681,
                new GregorianCalendar(1982, 10, 1).getTime());
        Logging.log(digitalBook);

        // Attempt to cast PaperbackBook to DigitalBook.
        DigitalBook castDigital = castToDigitalBook(paperbackBook);
        Logging.log(castDigital);

        // Attempt to cast PaperbackBook to Book.
        Book castBook = castToBook(paperbackBook);
        Logging.log(castBook);
    }

    @Nullable
    private static DigitalBook castToDigitalBook(Object source) {
        try {
            Logging.lineSeparator(String.format("CASTING %s TO DigitalBook", source.getClass().getSimpleName()), 60);
            return (DigitalBook) source;
        } catch (ClassCastException exception) {
            // Output expected ClassCastExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
        return null;
    }

    @Nullable
    private static Book castToBook(Object source) {
        try {
            Logging.lineSeparator(String.format("CASTING %s TO Book", source.getClass().getSimpleName()), 60);
            return (Book) source;
        } catch (ClassCastException exception) {
            // Output expected ClassCastExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
        return null;
    }
}

```

```java
// PublicationType.java
package io.airbrake;

public enum PublicationType {
    DIGITAL,
    PAPERBACK,
}
```

```java
// Book.java
package io.airbrake;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.annotation.*;
import io.airbrake.utility.Logging;

import java.util.Date;

/**
 * Simple example class to store book instances.
 */
@JsonIgnoreProperties(ignoreUnknown = true)
public class Book
{
    private String author;
    private String title;
    private Integer pageCount;
    private Date publishedAt;

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
            Logging.log(String.format("Published '%s' by %s.", getTitle(), getAuthor()));
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
     * Output
     *
     * @return
     * @throws JsonProcessingException
     */
    public String toJsonString() throws JsonProcessingException {
        return new ObjectMapper().writeValueAsString(this);
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
// DigitalBook.java
package io.airbrake;

import java.util.Date;

public class DigitalBook extends Book {

    private PublicationType publicationType;

    public DigitalBook(String title, String author, Integer pageCount, Date publishedAt) {
        super(title, author, pageCount, publishedAt);
        publicationType = PublicationType.DIGITAL;
    }
}
```

```java
// PaperbackBook.java
package io.airbrake;

import java.util.Date;

public class PaperbackBook extends Book {

    private PublicationType publicationType;

    public PaperbackBook(String title, String author, Integer pageCount, Date publishedAt) {
        super(title, author, pageCount, publishedAt);
        publicationType = PublicationType.PAPERBACK;
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

To see how a `ClassCastException` is typically thrown we'll be extending a `Book` class with a few inherited classes, namely `PaperbackBook` and `DigitalBook`.  For this simple example, we need a simple class hierarchy, so our paperback and digital versions will each specify their own respective `enum PublicationType` value upon instantiation, differentiating each publication type from the others.

_Note_: To save space, we won't go over the full `Book` class code here, but feel free to scroll up at the full code sample to have a look at it.

Instead, we'll start with the aforementioned `PublicationType` `enum`:

```java
public enum PublicationType {
    DIGITAL,
    PAPERBACK,
}
```

Now, both the `DigitalBook` and `PaperbackBook` extend the `Book` class and, after passing all the standard parameters to the `super` (i.e. `Book`) constructor, they each set their `publicationType` property to the respective value:

```java
public class DigitalBook extends Book {

    private PublicationType publicationType;

    public DigitalBook(String title, String author, Integer pageCount, Date publishedAt) {
        super(title, author, pageCount, publishedAt);
        publicationType = PublicationType.DIGITAL;
    }
}
```

```java
public class PaperbackBook extends Book {

    private PublicationType publicationType;

    public PaperbackBook(String title, String author, Integer pageCount, Date publishedAt) {
        super(title, author, pageCount, publishedAt);
        publicationType = PublicationType.PAPERBACK;
    }
}
```

As you may be aware, by extending `Book` these classes can use most of the functionality and methods that the base `Book` class provides, while also _extending_ it with their own functionality.  Consequently, an instance of the `DigitalBook` class or the `PaperbackBook` class can _also_ be considered an instance of the `Book` class, since they are inherited children.  However, the reverse cannot be said, because a `Book` instance is not necessarily also an instance of `DigitalBook` or `PaperbackBook`.

This can lead to some troubles when trying to perform explicit `casts` from one class type to another.  For example, here we have a simple `castToDigitalBook(Object source)` method, which does just as the name suggests:

```java
@Nullable
private static DigitalBook castToDigitalBook(Object source) {
    try {
        Logging.lineSeparator(String.format("CASTING %s TO DigitalBook", source.getClass().getSimpleName()), 60);
        return (DigitalBook) source;
    } catch (ClassCastException exception) {
        // Output expected ClassCastExceptions.
        Logging.log(exception);
    } catch (Exception exception) {
        // Output unexpected Exceptions.
        Logging.log(exception, false);
    }
    return null;
}
```

To test this out, we'll start by instantiating a new `DigitalBook` and a new `PaperbackBook`:

```java
// Create a new PaperbackBook.
Book paperbackBook = new PaperbackBook(
        "The Revenant",
        "Michael Punke",
        272,
        new GregorianCalendar(2015, 1, 6).getTime());
Logging.log(paperbackBook);

// Create a new DigitalBook.
Book digitalBook = new DigitalBook(
        "Magician",
        "Raymond E. Feist",
        681,
        new GregorianCalendar(1982, 10, 1).getTime());
Logging.log(digitalBook);
```

There's nothing unexpected going on here, so this code outputs both books to the log, confirming they're each their respective subclass types:

```
io.airbrake.PaperbackBook@3ac42916[
  publicationType=PAPERBACK
  author=Michael Punke
  title=The Revenant
  pageCount=272
  publishedAt=Fri Feb 06 00:00:00 PST 2015
]
io.airbrake.DigitalBook@73035e27[
  publicationType=DIGITAL
  author=Raymond E. Feist
  title=Magician
  pageCount=681
  publishedAt=Mon Nov 01 00:00:00 PST 1982
]
```

However, now let's try taking our `paperbackBook` instance and casting it to a `DigitalBook` instance:

```java
// Attempt to cast PaperbackBook to DigitalBook.
DigitalBook castDigital = castToDigitalBook(paperbackBook);
Logging.log(castDigital);
```

This results in an immediate `ClassCastException`, which indicates that we cannot cast a `PaperbackBook` to `DigitalBook`:

```
----------- CASTING PaperbackBook TO DigitalBook -----------
[EXPECTED] java.lang.ClassCastException: io.airbrake.PaperbackBook cannot be cast to io.airbrake.DigitalBook
```

As previously mentioned, this is because the _source_ class (`PaperbackBook`) is not a direct descendant of the _target_ class (`DigitalBook`).  If it were, such a cast operation would work fine, which we can see by defining and calling the `castToBook(Object source)` method:

```java
@Nullable
private static Book castToBook(Object source) {
    try {
        Logging.lineSeparator(String.format("CASTING %s TO Book", source.getClass().getSimpleName()), 60);
        //return (Book) source;
        return Book.class.cast(source);
    } catch (ClassCastException exception) {
        // Output expected ClassCastExceptions.
        Logging.log(exception);
    } catch (Exception exception) {
        // Output unexpected Exceptions.
        Logging.log(exception, false);
    }
    return null;
}
```

```java
// Attempt to cast PaperbackBook to Book.
Book castBook = castToBook(paperbackBook);
Logging.log(castBook);
```

```
-------------- CASTING PaperbackBook TO Book ---------------
io.airbrake.PaperbackBook@3ac42916[
  publicationType=PAPERBACK
  author=Michael Punke
  title=The Revenant
  pageCount=272
  publishedAt=Fri Feb 06 00:00:00 PST 2015
]
```

It's important and interesting to note that, even though we explicitly cast `PaperbackBook` to a `Book`, the resulting object still remains a `PaperbackBook`, because it's the most specific type of object that can be inferred from this cast command.  In other words: There's no reason to cast _upward_ on the chain of inheritance, so it keeps the most extended option it can.

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A detailed look at the Java ClassCastException, with functional code samples illustrating how to perform basic class extension and casting.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html