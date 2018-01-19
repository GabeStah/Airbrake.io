---
categories: [Java Exception Handling]
date: 2018-01-19
published: true
title: "Java Exception Handling - AbstractMethodError"
---

Moving along through our in-depth [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series, today we'll be going over the **AbstractMethodError**.  This error is thrown when there are incompatibilities between compiled classes/JAR files using `abstract` methods.  

Throughout this article we'll examine the `AbstractMethodError` by looking at where it fits into the overall [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).  We'll also dig into a functional code example that illustrates the basics of using abstract classes and methods in Java, and how their use may lead to `AbstractMethodErrors` in your own code.  Let's get to it!

## The Technical Rundown

All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`java.lang.Object`](https://docs.oracle.com/javase/8/docs/api/java/lang/Object.html)
    - [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy)
        - [`java.lang.Error`](https://docs.oracle.com/javase/8/docs/api/java/lang/Error.html)
            - [`java.lang.LinkageError`](https://docs.oracle.com/javase/8/docs/api/java/lang/LinkageError.html)
                - [`java.lang.IncompatibleClassChangeError`](https://docs.oracle.com/javase/8/docs/api/java/lang/IncompatibleClassChangeError.html)
                    - `AbstractMethodError`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```java
package io.airbrake;

import io.airbrake.utility.Logging;

import java.util.GregorianCalendar;

public class Main {

    public static void main(String[] args) {
        Logging.lineSeparator("TEST A");
        createBookTestA();
        Logging.lineSeparator("TEST B");
        createBookTestB();
    }

    private static void createBookTestA() {
        try {
            AbstractBook book = new AbstractBook(
                    "A Game of Thrones",
                    "George R.R. Martin",
                    848,
                    new GregorianCalendar(1996, 8, 6).getTime()
            );
            Logging.log(book.getTagline());
        } catch (AbstractMethodError error) {
            // Output expected AbstractMethodErrors.
            Logging.log(error);
        } catch (Exception | Error exception) {
            // Output unexpected Exceptions/Errors.
            Logging.log(exception, false);
        }
    }

    private static void createBookTestB() {
        try {
            AbstractBook book = new AbstractBook(
                    "A Clash of Kings",
                    "George R.R. Martin",
                    761,
                    new GregorianCalendar(1998, 10, 16).getTime()
            );
            Logging.log(book.getTagline());
        } catch (AbstractMethodError error) {
            // Output expected AbstractMethodErrors.
            Logging.log(error);
        } catch (Exception | Error exception) {
            // Output unexpected Exceptions/Errors.
            Logging.log(exception, false);
        }
    }
}
```

```java
package io.airbrake;

import java.util.Date;

abstract class Publication {
    /**
     * Get author of book.
     *
     * @return Author name.
     */
    abstract String getAuthor();

    /**
     * Get publication type of book.
     *
     * @return Publication type.
     */
    abstract PublicationType getPublicationType();

    /**
     * Get published date of book.
     *
     * @return Published date.
     */
    abstract Date getPublishedAt();

    /**
     * Get a formatted tagline with author, title, page count, publication date, and publication type.
     *
     * @return Formatted tagline.
     */
    abstract String getTagline();

    /**
     * Get title of book.
     *
     * @return Title.
     */
    abstract String getTitle();

    /**
     * Set author of book.
     *
     * @param author Author name.
     */
    abstract void setAuthor(String author);

    /**
     * Set publication type of book.
     *
     * @param type Publication type.
     */
    abstract void setPublicationType(PublicationType type);

    /**
     * Set published date of book.
     *
     * @param publishedAt Page count.
     */
    abstract void setPublishedAt(Date publishedAt);

    /**
     * Set title of book.
     *
     * @param title Title.
     */
    abstract void setTitle(String title);
}
```

```java
package io.airbrake;

import java.text.DateFormat;
import java.util.Date;

public class AbstractBook extends Publication {
    private String author;
    private String title;
    private Integer pageCount;
    private Date publishedAt;
    private PublicationType publicationType = PublicationType.BOOK;

    private static final Integer maximumPageCount = 4000;

    /**
     * Constructs an empty book.
     */
    public AbstractBook() { }

    /**
     * Constructs a basic book.
     *
     * @param title Book title.
     * @param author Book author.
     */
    public AbstractBook(String title, String author) {
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
    public AbstractBook(String title, String author, Integer pageCount) {
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
    public AbstractBook(String title, String author, Integer pageCount, Date publishedAt) {
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
    public AbstractBook(String title, String author, Integer pageCount, Date publishedAt, PublicationType publicationType) {
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
     * Get string representation of Book.
     *
     * @return String representation.
     */
    public String toString() {
        return getTagline();
    }
}
```

This code sample also uses the [`Logging`](https://github.com/GabeStah/Airbrake.io/blob/master/lib/java/src/io/airbrake/utility/Logging.java) utility class, the source of which can be [found here on GitHub](https://github.com/GabeStah/Airbrake.io/blob/master/lib/java/src/io/airbrake/utility/Logging.java).

## When Should You Use It?

In most situations you'll rarely experience `AbstractMethodErrors` during runtime, because they are typically caught by the compiler _prior to_ execution.  However, there are certain scenarios in which code is referencing outdated abstract classes, which can lead to `AbstractMethodErrors`.  Most commonly, consider a situation where an abstract class and an inherited class are compiled.  Next, modifications are made to the abstract class and _only_ that abstract class is recompiled, while the extended class is left alone.  This can cause `AbstractMethodErrors`, since the runtime doesn't know how to handle the incompatibility between the two classes.

To illustrate we've created a basic `Publication` abstract class, which defines a handful of methods that should be implemented in all extension classes:

```java
package io.airbrake;

import java.util.Date;

abstract class Publication {
    abstract String getAuthor();

    abstract PublicationType getPublicationType();

    abstract Date getPublishedAt();

    abstract String getTagline();

    abstract String getTitle();

    abstract void setAuthor(String author);

    abstract void setPublicationType(PublicationType type);

    abstract void setPublishedAt(Date publishedAt);

    abstract void setTitle(String title);
}
```

The `AbstractBook` class then `extends` `Publication` and provides actual implementation of the abstract methods above:

```java
public class AbstractBook extends Publication {
    // ...
}
```

We can test this out by creating a new `AbstractBook` instance and outputting it to the log:

```java
private static void createBookTestA() {
    try {
        AbstractBook book = new AbstractBook(
                "A Game of Thrones",
                "George R.R. Martin",
                848,
                new GregorianCalendar(1996, 8, 6).getTime()
        );
        Logging.log(book.getTagline());
    } catch (AbstractMethodError error) {
        // Output expected AbstractMethodErrors.
        Logging.log(error);
    } catch (Exception | Error exception) {
        // Output unexpected Exceptions/Errors.
        Logging.log(exception, false);
    }
}
```

Executing the `createBookTestA()` method produces the following output:

```
---------------- TEST A ----------------
'A Game of Thrones' by George R.R. Martin is 848 pages, published Sep 6, 1996 as BOOK type.
```

That's all well and good, but what happens if we modify one of these classes but not the other (effectively causing their versions to be incompatible)?  For example, let's modify the `Publication` abstract class and add another `abstract method`:


```java
abstract class Publication {

    // ...

    abstract String getDetails();

    // ...
}
```

If we compile _just_ this `Publication` class, but not also the inherited `AbstractBook` class, executing our test code will result in an `AbstractMethodError`, since the secondary class hasn't been recompiled and made aware of the changes:

```
----- TEST B -----
[EXPECTED] java.lang.AbstractMethodError: io.airbrake.Publication.getDetails()Ljava/lang/String;
```

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look at the Java AbstractMethodError, with functional code samples showing how such errors might be thrown during runtime execution.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html