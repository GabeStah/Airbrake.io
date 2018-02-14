---
categories: 
  - Java Exception Handling
date: 2018-02-14
description: "A close look at the Java ArrayStoreException, with code samples illustrating how Java handles object inheritance and array manipulation."
published: true
sources:
  - https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html
title: "Java Exception Handling - ArrayStoreException"
---

Making our way through our detailed [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series, today we'll be examining the **ArrayStoreException**.  The `ArrayStoreException` is thrown when an attempt is made to add an object of the incorrect type to an array.

Throughout this article we'll examine the `ArrayStoreException` in more detail, starting with where it resides in the overall [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).  We'll also look at some simple, functional sample code showing how Java array manipulation can potentially lead to `ArrayStoreExceptions` under certain circumstances.  Let's get right to it!

## The Technical Rundown

All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`java.lang.Object`](https://docs.oracle.com/javase/9/docs/api/java/lang/Object.html)
    - [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy)
        - [`java.lang.Exception`](https://docs.oracle.com/javase/9/docs/api/java/lang/Exception.html)
            - [`java.lang.RuntimeException`](https://docs.oracle.com/javase/9/docs/api/java/lang/RuntimeException.html)
                - `ArrayStoreException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```java
package io.airbrake;

import io.airbrake.utility.Logging;
import java.util.GregorianCalendar;

public class Main {

    public static void main(String[] args) {
        Logging.lineSeparator("String INTO String[]");
        insertIntoArray("Hello world", new String[1]);

        Logging.lineSeparator("Integer INTO String[]");
        insertIntoArray(24601, new String[1]);

        Logging.lineSeparator("Book INTO String[]");
        insertIntoArray(
            new Book(
            "The Stand",
                "Stephen King",
                1153,
                new GregorianCalendar(1978, 8, 1).getTime()
            ),
            new String[1]
        );

        Logging.lineSeparator("String INTO Object[]");
        insertIntoArray("Hello world", new Object[1]);

        Logging.lineSeparator("Integer INTO Object[]");
        insertIntoArray(24601, new Object[1]);

        Logging.lineSeparator("Book INTO Object[]");
        insertIntoArray(
            new Book(
                "It",
                "Stephen King",
                1116,
                new GregorianCalendar(1987, 9, 1).getTime()
            ),
            new Object[1]
        );
    }

    /**
     * Insert passed Object into passed Object[] array at first index.
     *  @param object Object to insert.
     * @param array Recipient array.
     */
    private static void insertIntoArray(Object object, Object[] array) {
        try {
            // Invoke default override.
            insertIntoArray(object, array, 0);
        } catch (ArrayStoreException exception) {
            // Output unexpected ArrayStoreExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }

    /**
     * Insert passed Object into passed Object[] array at specified index.
     *
     * @param object Object to insert.
     * @param array Recipient array.
     * @param index Index at which to insert.
     */
    private static void insertIntoArray(Object object, Object[] array, int index) {
        try {
            // Attempt to insert object at passed index.
            array[index] = object;
            // Output new array.
            Logging.log(array);
        } catch (ArrayStoreException exception) {
            // Output unexpected ArrayStoreExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
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

Since `ArrayStoreExceptions` only occur when working with arrays in Java we'll jump right into our sample code and starting messing around with some array manipulation.  Our basic helper function is `insertIntoArray(Object object, Object[] array, int index)` (and the companion signature method `insertIntoArray(Object object, Object[] array)`):

```java
/**
* Insert passed Object into passed Object[] array at first index.
*  @param object Object to insert.
* @param array Recipient array.
*/
private static void insertIntoArray(Object object, Object[] array) {
    try {
        // Invoke default override.
        insertIntoArray(object, array, 0);
    } catch (ArrayStoreException exception) {
        // Output unexpected ArrayStoreExceptions.
        Logging.log(exception);
    } catch (Exception exception) {
        // Output unexpected Exceptions.
        Logging.log(exception, false);
    }
}

/**
* Insert passed Object into passed Object[] array at specified index.
*
* @param object Object to insert.
* @param array Recipient array.
* @param index Index at which to insert.
*/
private static void insertIntoArray(Object object, Object[] array, int index) {
    try {
        // Attempt to insert object at passed index.
        array[index] = object;
        // Output new array.
        Logging.log(array);
    } catch (ArrayStoreException exception) {
        // Output unexpected ArrayStoreExceptions.
        Logging.log(exception);
    } catch (Exception exception) {
        // Output unexpected Exceptions.
        Logging.log(exception, false);
    }
}
```

As you can see we're performing very little logic within this method.  In fact, it's merely just a wrapper for attempting to insert the passed `object` into the passed `Object[] array` at the specified `index`.

With our methods setup we can perform some basic tests, starting with trying to insert the `String` `"Hello world"` into a `String[1]` array:

```java
Logging.lineSeparator("String INTO String[]");
insertIntoArray("Hello world", new String[1]);
```

As you probably suspect, this works just fine and inserts our string and outputs the full array to the console:

```
--------- String INTO String[] ---------
[Ljava.lang.String;@153f5a29[
  {Hello world}
]
```

Alright, that's all well and good, but let's see what happens if we try a couple more tests, first by trying to insert an `Integer` and then a `Book` instance into a `String[1]` array:

```java
Logging.lineSeparator("Integer INTO String[]");
insertIntoArray(24601, new String[1]);

Logging.lineSeparator("Book INTO String[]");
insertIntoArray(
    new Book(
    "The Stand",
        "Stephen King",
        1153,
        new GregorianCalendar(1978, 8, 1).getTime()
    ),
    new String[1]
);
```

Both of these tests throw an `ArrayStoreException` at us, indicating that the passed object type (`Integer` and `Book`, in this case) are incompatible with the type of the target array (`String`):

```
-------- Integer INTO String[] ---------
[EXPECTED] java.lang.ArrayStoreException: java.lang.Integer
	at io.airbrake.Main.insertIntoArray(Main.java:72)
	at io.airbrake.Main.insertIntoArray(Main.java:52)
	at io.airbrake.Main.main(Main.java:13)

---------- Book INTO String[] ----------
[EXPECTED] java.lang.ArrayStoreException: io.airbrake.Book
	at io.airbrake.Main.insertIntoArray(Main.java:72)
	at io.airbrake.Main.insertIntoArray(Main.java:52)
	at io.airbrake.Main.main(Main.java:16)
```

The fundamental problem indicated by an `ArrayStoreException` is that Java is one of many [`strongly typed`](https://en.wikipedia.org/wiki/Strong_and_weak_typing) programming languages.  There are slight variations between languages on how strong typing is implemented, but the basic concept is that source code will indicate an _expected_ type of an object, and any future attempts to assign a value of a _different_ or incompatible type to said object will result in an error.  This can most readily be seen in Java by writing two lines of code:

```java
String name = "Alice";
name = 123;
```

The Java compiler won't even allow this code to be compiled and it will produce an `incompatible types` error, indicating that a `java.lang.String` cannot be converted to an `int`.  Well, the `ArrayStoreException` is merely an extension of this strongly typed concept, and is a means of indicating that the underlying `type` of an array doesn't match the type of the object attempting to be inserted.

Alright, so we saw that we were unable to add `Integer` or `Book` objects into `String[]` arrays above, but that's because neither `Integer` nor `Book` directly _inherit from_ the `String` class.  However, _everything_ in Java inherits from the [`Object`](https://docs.oracle.com/javase/8/docs/api/java/lang/Object.html) superclass, which means that we _should_ be able to create an `Object[]` array and successfully insert any and all inherited objects (e.g. `Integer` or `Book`) that we wish.  We'll test this out with three more tests, this time trying to pass the same three types of objects into our `new Object[1]` array:

```java
Logging.lineSeparator("String INTO Object[]");
insertIntoArray("Hello world", new Object[1]);

Logging.lineSeparator("Integer INTO Object[]");
insertIntoArray(24601, new Object[1]);

Logging.lineSeparator("Book INTO Object[]");
insertIntoArray(
    new Book(
        "It",
        "Stephen King",
        1116,
        new GregorianCalendar(1987, 9, 1).getTime()
    ),
    new Object[1]
);
```

Sure enough, since `Object` is the superclass of all other classes in Java, all three of our different object types were successfully added to the array and output to the console:

```
--------- String INTO Object[] ---------
[Ljava.lang.Object;@3e6fa38a[
  {Hello world}
]
-------- Integer INTO Object[] ---------
[Ljava.lang.Object;@66a3ffec[
  {24601}
]
---------- Book INTO Object[] ----------
[Ljava.lang.Object;@77caeb3e[
  {'It' by Stephen King is 1116 pages, published Oct 1, 1987 as DIGITAL type.}
]
```

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!