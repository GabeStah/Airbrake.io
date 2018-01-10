---
categories: [Java exception handling]
date: 2018-01-10
published: true
title: Java Exception Handling - NoSuchElementException
---

Our journey continues through our in-depth [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series as, today, we dig into the depths of the **NoSuchElementException**.  As the name suggests, a `NoSuchElementException` is thrown when trying to access an invalid element using a few built-in methods from the [`Enumeration`](https://docs.oracle.com/javase/8/docs/api/java/util/Enumeration.html#nextElement--) and [`Iterator`](https://docs.oracle.com/javase/8/docs/api/java/util/Iterator.html#next--) classes.

Throughout this article we'll examine the `NoSuchElementException` in greater detail by looking at where it sits in the overall [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).  We'll also get into some function Java code samples that will illustrate the basic usage of an `Iterator`, how it compares to more modern Java versions collection iteration practices, and how improper use of `Iterators` can lead to `NoSuchElementExceptions` in your own code, so let's get going!

## The Technical Rundown

All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`java.lang.Object`](https://docs.oracle.com/javase/8/docs/api/java/lang/Object.html)
    - [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy)
        - [`java.lang.Exception`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html)
            - [`java.lang.RuntimeException`](https://docs.oracle.com/javase/8/docs/api/java/lang/RuntimeException.html)
                - `java.util.NoSuchElementException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```java
package io.airbrake;

import io.airbrake.utility.Logging;

import java.util.ArrayList;
import java.util.GregorianCalendar;
import java.util.Iterator;
import java.util.NoSuchElementException;

public class Main {

    public static void main(String[] args) {
        try {
            Logging.lineSeparator("CREATING BOOKS");
            ArrayList<Book> books = new ArrayList<>();
            books.add(
                new Book(
                    "The Stand",
                    "Stephen King",
                    1153,
                    new GregorianCalendar(1978, 8, 1).getTime()
                )
            );

            books.add(
                new Book(
                    "It",
                    "Stephen King",
                    1116,
                    new GregorianCalendar(1987, 9, 1).getTime()
                )
            );

            books.add(
                new Book(
                    "The Gunslinger",
                    "Stephen King",
                    231,
                    new GregorianCalendar(1982, 5, 10).getTime()
                )
            );

            Logging.lineSeparator("FOREACH LOOP TEST");
            forEachLoopTest(books);

            Logging.lineSeparator("ITERATOR TEST");
            Iterator iterator = iteratorTest(books);

            Logging.log(iterator.next());
        } catch (NoSuchElementException exception) {
            // Output expected NoSuchElementExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }

    /**
     * Loops through passed ArrayList using built-in forEach() method.  Outputs elements to log.
     *
     * @param list List to be looped through.
     */
    private static void forEachLoopTest(ArrayList<Book> list) {
        try {
            // Output list via forEach method and Logging::log method reference.
            list.forEach(Logging::log);
        } catch (NoSuchElementException exception) {
            // Output expected NoSuchElementExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }

    /**
     * Loops through passed ArrayList using Iterator of list and converting each element to Book before output.
     *
     * @param list List to be iterated through.
     * @return Iterator obtained from ArrayList.
     */
    private static Iterator iteratorTest(ArrayList<Book> list) {
        try {
            // Create iterator from list.
            Iterator iterator = list.iterator();
            // While next element exists, iteratorTest.
            while (iterator.hasNext())
            {
                // Get next element and output.
                Book book = (Book) iterator.next();
                Logging.log(book);
            }
            // Return iterator.
            return iterator;
        } catch (NoSuchElementException exception) {
            // Output expected NoSuchElementExceptions.
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
// Book.java
package io.airbrake;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import io.airbrake.utility.Logging;

import java.io.Serializable;
import java.io.UnsupportedEncodingException;
import java.text.DateFormat;
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
    private PublicationType publicationType = PublicationType.DIGITAL;

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
     * Constructs a basic book, with page count, publication date, and publication type.
     *
     * @param title Book title.
     * @param author Book author.
     * @param pageCount Book page count.
     */
    public Book(String title, String author, Integer pageCount, Date publishedAt, PublicationType publicationType) {
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
    public PublicationType getPublicationType() { return publicationType; }

    /**
     * Get published date of book.
     *
     * @return Published date.
     */
    public Date getPublishedAt() { return publishedAt; }

    /**
     * Get a formatted tagline with author, title, page count, publication date, and publication type.
     *
     * @return Formatted tagline.
     */
    public String getTagline() {
        return String.format("'%s' by %s is %d pages, published %s as %s type.",
                getTitle(),
                getAuthor(),
                getPageCount(),
                DateFormat.getDateInstance().format(getPublishedAt()),
                getPublicationType());
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
     * Set publication type of book.
     *
     * @param type Publication type.
     */
    public void setPublicationType(PublicationType type) { this.publicationType = type; }

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
     * Get the filename formatted version of this Book.
     *
     * @return Filename.
     */
    String toFilename() {
        try {
            return java.net.URLEncoder.encode(String.format("%s-%s", getTitle(), getAuthor()).toLowerCase(), "UTF-8");
        } catch (UnsupportedEncodingException exception) {
            Logging.log(exception);
        }
        return null;
    }

    /**
     * Output to JSON string.
     *
     * @return
     * @throws JsonProcessingException
     */
    public String toJsonString() throws JsonProcessingException {
        return new ObjectMapper().writeValueAsString(this);
    }

    /**
     * Get string representation of Book.
     *
     * @return String representation.
     */
    public String toString() {
        return getTagline();
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

Java includes a few different ways to iterate through elements in a collection.  The first of these classes, [`Enumeration`](https://docs.oracle.com/javase/8/docs/api/java/util/Enumeration.html), was introduced in JDK1.0 and is generally considered deprecated in favor of newer iteration classes, like [`Iterator`](https://docs.oracle.com/javase/8/docs/api/java/util/Iterator.html) and [`ListIterator`](https://docs.oracle.com/javase/8/docs/api/java/util/ListIterator.html).  As with most programming languages, the `Iterator` class includes a `hasNext()` method that returns a boolean indicating if the iteration has anymore elements.  If `hasNext()` returns `true`, then the `next()` method will return the next element in the iteration.  Unlike `Enumeration`, `Iterator` also has a `remove()` method, which removes the last element that was obtained via `next()`.  While `Iterator` is generalized for use with all collections in the Java Collections Framework, `ListIterator` is more specialized and only works with `List`-based collections, like `ArrayList`, `LinkedList`, and so forth.  However, `ListIterator` adds even more functionality by allowing iteration to traverse in both directions via `hasPrevious()` and `previous()` methods.

For our example today we'll just be sticking with the traditional `Iterator` class, along with a comparison to a more modern built-in syntax for iterating over common collections (an `ArrayList`, in this case).  We start with the `forEachLoopTest(ArrayList<Book> list)`:

```java
/**
    * Loops through passed ArrayList using built-in forEach() method.  Outputs elements to log.
    *
    * @param list List to be looped through.
    */
private static void forEachLoopTest(ArrayList<Book> list) {
    try {
        // Output list via forEach method and Logging::log method reference.
        list.forEach(Logging::log);
    } catch (NoSuchElementException exception) {
        // Output expected NoSuchElementExceptions.
        Logging.log(exception);
    } catch (Exception exception) {
        // Output unexpected Exceptions.
        Logging.log(exception, false);
    }
}
```

This method uses the [`ArrayList.forEach(Consumer<? super E> action)`](https://docs.oracle.com/javase/8/docs/api/java/util/ArrayList.html#forEach-java.util.function.Consumer-) method, which provides a simple method for performing an action on every element in an `Iterable` collection, such as our `ArrayList<Book>`.  Moreover, we're using the modern [method reference](https://docs.oracle.com/javase/tutorial/java/javaOO/methodreferences.html) syntax for our lambda expression, which simply passes the `Book` element that is obtained in the `forEach(...)` loop to our `Logging.log` method, which outputs it to the log.

To test this out we create a new `ArrayList<Book>` collection and add a few new `Books` to it:

```java
public static void main(String[] args) {
    try {
        Logging.lineSeparator("CREATING BOOKS");
        ArrayList<Book> books = new ArrayList<>();
        books.add(
            new Book(
                "The Stand",
                "Stephen King",
                1153,
                new GregorianCalendar(1978, 8, 1).getTime()
            )
        );

        books.add(
            new Book(
                "It",
                "Stephen King",
                1116,
                new GregorianCalendar(1987, 9, 1).getTime()
            )
        );

        books.add(
            new Book(
                "The Gunslinger",
                "Stephen King",
                231,
                new GregorianCalendar(1982, 5, 10).getTime()
            )
        );

        Logging.lineSeparator("FOREACH LOOP TEST");
        forEachLoopTest(books);

        Logging.lineSeparator("ITERATOR TEST");
        Iterator iterator = iteratorTest(books);

        Logging.log(iterator.next());
    } catch (NoSuchElementException exception) {
        // Output expected NoSuchElementExceptions.
        Logging.log(exception);
    } catch (Exception exception) {
        // Output unexpected Exceptions.
        Logging.log(exception, false);
    }
}
```

Executing this code produces the following output:

```
------------ CREATING BOOKS ------------
---------- FOREACH LOOP TEST -----------
Disconnected from the target VM, address: '127.0.0.1:62285', transport: 'socket'
io.airbrake.Book@5ebec15[
  author=Stephen King
  title=The Stand
  pageCount=1153
  publishedAt=Fri Sep 01 00:00:00 PDT 1978
  publicationType=DIGITAL
]
io.airbrake.Book@26ba2a48[
  author=Stephen King
  title=It
  pageCount=1116
  publishedAt=Thu Oct 01 00:00:00 PDT 1987
  publicationType=DIGITAL
]
io.airbrake.Book@5f2050f6[
  author=Stephen King
  title=The Gunslinger
  pageCount=231
  publishedAt=Thu Jun 10 00:00:00 PDT 1982
  publicationType=DIGITAL
]
```

As expected, our `Books` are created and added to our `ArrayList<Book> books` collection, then passed to and iterated through using the `forEach(Logging::log)` lambda method expression.  This is the simplest way to iterate over all elements, but if we need more explicit control over iteration, we'd likely want to use an actual `Iterator` instance, which we're testing in the `iteratorTest(ArrayList<Book> list)` method:

```java
/**
* Loops through passed ArrayList using Iterator of list and converting each element to Book before output.
*
* @param list List to be iterated through.
* @return Iterator obtained from ArrayList.
*/
private static Iterator iteratorTest(ArrayList<Book> list) {
    try {
        // Create iterator from list.
        Iterator iterator = list.iterator();
        // While next element exists, iteratorTest.
        while (iterator.hasNext())
        {
            // Get next element and output.
            Book book = (Book) iterator.next();
            Logging.log(book);
        }
        // Return iterator.
        return iterator;
    } catch (NoSuchElementException exception) {
        // Output expected NoSuchElementExceptions.
        Logging.log(exception);
    } catch (Exception exception) {
        // Output unexpected Exceptions.
        Logging.log(exception, false);
    }
    return null;
}
```

Here we've obtained the `Iterator` instance of our `ArrayList<Book> list` collection via the `iterator()` method, then perform a loop while `iterator.hasNext()` is `true`, in which we grab the next element via `iterator.next()`, convert it from an `Object` to a `Book` type, and output it to the log.  We also return the obtained `Iterator` instance, which we'll use in a moment.

Just as before, this works as expected, so executing this method produces the following output:

```
------------ ITERATOR TEST -------------
io.airbrake.Book@5ebec15[
  author=Stephen King
  title=The Stand
  pageCount=1153
  publishedAt=Fri Sep 01 00:00:00 PDT 1978
  publicationType=DIGITAL
]
io.airbrake.Book@26ba2a48[
  author=Stephen King
  title=It
  pageCount=1116
  publishedAt=Thu Oct 01 00:00:00 PDT 1987
  publicationType=DIGITAL
]
io.airbrake.Book@5f2050f6[
  author=Stephen King
  title=The Gunslinger
  pageCount=231
  publishedAt=Thu Jun 10 00:00:00 PDT 1982
  publicationType=DIGITAL
]
```

However, let's see what happens if we use the now-exhausted `Iterator` instance returned by the `iteratorTest(ArrayList<Book> list)` method and try to obtain the `next()` element from it:

```java
Logging.lineSeparator("ITERATOR TEST");
Iterator iterator = iteratorTest(books);

Logging.log(iterator.next());
```

Unsurprisingly, this throws a `NoSuchElementException` our way, because we've already retrieved all three elements of the `iterator` within the `iteratorTest(ArrayList<Book> list)` method itself:

```
[EXPECTED] java.util.NoSuchElementException
```

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look at the Java NoSuchElementException, with functional code samples illustrating how to perform different types if iteration in Java.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html