---
categories: [Java Exception Handling]
date: 2018-01-26
published: true
title: "Java Exception Handling - IncompatibleClassChangeError"
---

Next up, in our deep dive into [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy), today we'll be digging into the **IncompatibleClassChangeError**.  This error is a base exception class for a variety of errors thrown when the Java Virtual Machine (JVM) recognizes an incompatibility between two compiled class definitions that are executing in tandem.  In child class, which we just [looked at last week](https://airbrake.io/blog/java-exception-handling/abstractmethoderror), is the `AbstractMethodError`, which extends the `IncompatibleClassChangeError` and is thrown when `abstract` method incompatibilities are detected between two classes.

In today's article we'll explore the `IncompatibleClassChangeError` by seeing where it resides in the overall [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).  We'll also look at some functional sample code that illustrates a common scenario for class extension, and how a handful of incompatible changes can result in `IncompatibleClassChangeErrors` in your own code.  Let's get into it!

## The Technical Rundown

All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`java.lang.Object`](https://docs.oracle.com/javase/8/docs/api/java/lang/Object.html)
    - [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy)
        - [`java.lang.Error`](https://docs.oracle.com/javase/8/docs/api/java/lang/Error.html)
            - [`java.lang.LinkageError`](https://docs.oracle.com/javase/8/docs/api/java/lang/LinkageError.html)
                - `IncompatibleClassChangeError`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```java
package io.airbrake;

import java.util.GregorianCalendar;

public class Test {
    public static void main(String[] args) {
        try {
            PaperbackBook paperbackBook = new PaperbackBook(
                    "The Stand",
                    "Stephen King",
                    1153,
                    new GregorianCalendar(1978, 0, 1).getTime()
            );
            System.out.println(paperbackBook.getTagline());
        } catch (IncompatibleClassChangeError error) {
            // Output expected IncompatibleClassChangeErrors.
            System.out.println(String.format("[EXPECTED] %s", error.toString()));
            error.printStackTrace();
        } catch (Exception | Error throwable) {
            // Output unexpected Exceptions/Errors.
            System.out.println(String.format("[UNEXPECTED] %s", throwable.toString()));
            throwable.printStackTrace();
        }
    }
}
```

```java
package io.airbrake;

import java.util.Date;

public class BaseBook {
    private String author;
    private Integer pageCount;
    private Date publishedAt;
    private String title;

    /**
     * Constructs a basic book.
     *
     * @param title Book title.
     * @param author Book author.
     */
    public BaseBook(String title, String author) {
        this.author = author;
        this.title = title;
    }

    /**
     * Constructs a basic book, with page count.
     *
     * @param title Book title.
     * @param author Book author.
     * @param pageCount Book page count.
     */
    public BaseBook(String title, String author, Integer pageCount) {
        this.author = author;
        this.title = title;
        this.pageCount = pageCount;
    }

    /**
     * Constructs a basic book, with page count, publication date, and publication type.
     *
     * @param title Book title.
     * @param author Book author.
     * @param pageCount Book page count.
     * @param publishedAt Book publication date.
     */
    public BaseBook(String title, String author, Integer pageCount, Date publishedAt) {
        this.author = author;
        this.title = title;
        this.pageCount = pageCount;
        this.publishedAt = publishedAt;
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
    public Date getPublishedAt() {
        return publishedAt;
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
}
```

```java
package io.airbrake;

import java.text.DateFormat;
import java.util.Date;

public class PaperbackBook extends BaseBook {
    public PaperbackBook(String title, String author) {
        super(title, author);
    }

    public PaperbackBook(String title, String author, Integer pageCount) {
        super(title, author, pageCount);
    }

    public PaperbackBook(String title, String author, Integer pageCount, Date publishedAt) {
        super(title, author, pageCount, publishedAt);
    }

    /**
     * Get a formatted tagline with author, title, and page count.
     *
     * @return Formatted tagline.
     */
    public String getTagline() {
        return String.format("'%s' by %s is %d pages, in PAPERBACK format, and published %s.",
                getTitle(),
                getAuthor(),
                getPageCount(),
                DateFormat.getDateInstance().format(getPublishedAt()));
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

The appearance of an `IncompatibleClassChangeError` -- or any child error therein -- is a result of what is known as `binary incompatibility` in Java.  There is a [great article here](http://wiki.eclipse.org/Evolving_Java-based_APIs_2) detailing all the ways an API package will (and will not) break binary compatibility, but we can summarize the common scenarios below.  Incompatibilities occur when an existing binary (i.e. compiled Java class) references a newly-modified binary, and that modified binary contains any one of a handful of potentially incompatible changes:

- A `non-final` field is changed to `static`.
- A `non-constant` field is changed to `non-static`.
- A `class` is changed to an `interface`.
- Or, an `interface` is changed to a `class`.

These four modifications are the primary way an `IncompatibleClassChangeError` can be thrown.  To illustrate, our simple example uses two similar classes to handle book object creation, starting with the `BaseBook` class:

```java
package io.airbrake;

import java.util.Date;

public class BaseBook {
    private String author;
    private Integer pageCount;
    private Date publishedAt;
    private String title;

    /**
     * Constructs a basic book.
     *
     * @param title Book title.
     * @param author Book author.
     */
    public BaseBook(String title, String author) {
        this.author = author;
        this.title = title;
    }

    /**
     * Constructs a basic book, with page count.
     *
     * @param title Book title.
     * @param author Book author.
     * @param pageCount Book page count.
     */
    public BaseBook(String title, String author, Integer pageCount) {
        this.author = author;
        this.title = title;
        this.pageCount = pageCount;
    }

    /**
     * Constructs a basic book, with page count, publication date, and publication type.
     *
     * @param title Book title.
     * @param author Book author.
     * @param pageCount Book page count.
     * @param publishedAt Book publication date.
     */
    public BaseBook(String title, String author, Integer pageCount, Date publishedAt) {
        this.author = author;
        this.title = title;
        this.pageCount = pageCount;
        this.publishedAt = publishedAt;
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
    public Date getPublishedAt() {
        return publishedAt;
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
}
```

`BaseBook` contains four private properties and associated getter/setter methods for `title`, `author`, `pageCount`, and `publishedAt`.  For our use case we want to create a handful of extension classes based on `BaseBook`, which will be used to create specific _types_ of book publications.  One such extension class is `PaperbackBook`:

```java
package io.airbrake;

import java.text.DateFormat;
import java.util.Date;

public class PaperbackBook extends BaseBook {
    public PaperbackBook(String title, String author) {
        super(title, author);
    }

    public PaperbackBook(String title, String author, Integer pageCount) {
        super(title, author, pageCount);
    }

    public PaperbackBook(String title, String author, Integer pageCount, Date publishedAt) {
        super(title, author, pageCount, publishedAt);
    }

    /**
     * Get a formatted tagline with author, title, and page count.
     *
     * @return Formatted tagline.
     */
    public String getTagline() {
        return String.format("'%s' by %s is %d pages, in PAPERBACK format, and published %s.",
                getTitle(),
                getAuthor(),
                getPageCount(),
                DateFormat.getDateInstance().format(getPublishedAt()));
    }
}
```

For this example we're not adding much to the `PaperbackBook` class, save for the new `getTagline()` method.  To make sure a `PaperbackBook` object inherits everything from `BaseBook` that we need we'll test it in the `Test.main(String[] args)` method:

```java
package io.airbrake;

import java.util.GregorianCalendar;

public class Test {
    public static void main(String[] args) {
        try {
            PaperbackBook paperbackBook = new PaperbackBook(
                    "The Stand",
                    "Stephen King",
                    1153,
                    new GregorianCalendar(1978, 0, 1).getTime()
            );
            System.out.println(paperbackBook.getTagline());
        } catch (IncompatibleClassChangeError error) {
            // Output expected IncompatibleClassChangeErrors.
            System.out.println(String.format("[EXPECTED] %s", error.toString()));
            error.printStackTrace();
        } catch (Exception | Error throwable) {
            // Output unexpected Exceptions/Errors.
            System.out.println(String.format("[UNEXPECTED] %s", throwable.toString()));
            throwable.printStackTrace();
        }
    }
}
```

Executing the `main(String[] args)` method works as expected, outputting the generated `tagline` of our book:

```
'The Stand' by Stephen King is 1153 pages, in PAPERBACK format, and published Jan 1, 1978.
```

Cool!  We now have `binary compatibility` between the compiled `BaseBook` and `PaperbackBook` classes.  However, what happens if we start to modify the `BaseBook` class definition and recompile it, without also recompiling the `PaperbackBook` class?  As we saw above, the first of the four possible ways to cause `binary incompatibility` is to change a `non-final` field to `static`.  Therefore, let's change the `BaseBook.getTitle()` method (and associated `private String title` field) to `static`:

```java
public class BaseBook {
    // ...

    private static String title;

    // ...

    /**
     * Get title of book.
     *
     * @return Title.
     */
    public static String getTitle() {
        return title;
    }

    // ...

}
```

We want to save our changes in the `BaseBook` binary so we need to recompile it:

```bash
$ javac io/airbrake/BaseBook.java
```

With the `title` field now set to `static` let's execute the `Test.main(String[] args)` method a second time and see what happens:

```bash
$ java io.airbrake.Test

[EXPECTED] java.lang.IncompatibleClassChangeError: Expecting non-static method io.airbrake.PaperbackBook.getTitle()Ljava/lang/String;
        at io.airbrake.PaperbackBook.getTagline(PaperbackBook.java:26)
        at io.airbrake.Test.main(Test.java:14)
```

As you probably expected, we're now catching an `IncompatibleClassChangeError`, which indicates that `PaperbackBook.getTitle()` (which is, of course, extended from `BaseBook.getTitle()`) should've been `non-static`.  In such scenarios, the obvious solution is to _either_ use an IDE that will catch such incompatibilities before runtime, or to ensure you always recompile all associated classes simultaneously, even when the source code of a single class is modified.

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A deep dive into the Java IncompatibleClassChangeError, with code samples showing how binary incompatibilities may be unintentionally created.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html