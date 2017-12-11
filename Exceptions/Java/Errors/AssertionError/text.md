# Java Exception Handling - AssertionError

Making our way through our detailed [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series we arrive at the **AssertionError** that we'll be going over today.  As with many other languages, the `AssertionError` in Java is thrown when an [`assert`](https://docs.oracle.com/javase/7/docs/technotes/guides/language/assert.html) statement fails (i.e. the result is false).

Within today's article we'll explore the detailed of the `AssertionError` by first looking at where it sits in the larger [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).  We'll also examine some fully functional Java code that will illustrate one example of a best practice for using assertions in Java applications, and how such code should lead to an unexpected `AssertionError` if something goes wrong.  Let's take a look!

## The Technical Rundown

All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`java.lang.Object`](https://docs.oracle.com/javase/8/docs/api/java/lang/Object.html)
    - [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy)
        - [`java.lang.Error`](https://docs.oracle.com/javase/8/docs/api/java/lang/Error.html)
            - `java.lang.AssertionError`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```java
package io.airbrake;

import io.airbrake.utility.Logging;

import java.util.GregorianCalendar;

public class Main {

    public static void main(String[] args) {
        testStringPublicationType();

        testEnumPublicationType();
    }

    private static void testEnumPublicationType() {
        try {
            // Create book with PublicationType: PublicationType.PAPERBACK
            Book book = new Book(
                    "The Name of the Wind",
                    "Patrick Rothfuss",
                    662,
                    new GregorianCalendar(2007, 2, 27).getTime(),
                    PublicationType.PAPERBACK);
            // Output Book.
            Logging.log(book);
            // Change to invalid publication type.
            Logging.lineSeparator("CHANGE PUBLICATION TYPE TO 'PublicationType.INVALID'", 60);
            //book.setPublicationType(PublicationType.INVALID);
            // Output modified Book.
            Logging.log(book);
        } catch (AssertionError error) {
            // Output expected AssertionErrors.
            Logging.log(error);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }

    private static void testStringPublicationType() {
        try {
            // Create book with PublicationType: 'PAPERBACK'
            BookWithStringPublicationType bookWithStringPublicationType = new BookWithStringPublicationType(
                    "The Wise Man's Fear",
                    "Patrick Rothfuss",
                    994,
                    new GregorianCalendar(2011, 2, 1).getTime(),
                    "PAPERBACK");
            // Output Book.
            Logging.log(bookWithStringPublicationType);
            // Change to invalid publication type.
            Logging.lineSeparator("CHANGE PUBLICATION TYPE TO 'INVALID' String", 60);
            bookWithStringPublicationType.setPublicationType("INVALID");
            // Output modified Book.
            Logging.log(bookWithStringPublicationType);
        } catch (AssertionError error) {
            // Output expected AssertionErrors.
            Logging.log(error);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
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
     * Constructs a basic book, with page count.
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

This code sample also uses the [`Logging`](https://github.com/GabeStah/Airbrake.io/blob/master/lib/java/src/io/airbrake/utility/Logging.java) utility class, the source of which can be [found here on GitHub](https://github.com/GabeStah/Airbrake.io/blob/master/lib/java/src/io/airbrake/utility/Logging.java).

## When Should You Use It?

To understand how `AssertionErrors` should be thrown within Java code we should first briefly review how assertions and the `assert` keyword are intended for use.  The purpose of assertion is to _test assumptions_ about the application's logic.  These assumptions might include pre-conditions, post-conditions, invariants, and the like.  As with most other programming assertion features, the Java `assert` keyword expects a boolean expression that the code _assumes_ will be/should be true.  This is because, the moment an `assert` expression is false the Java Virtual Machine (`JVM`) will throw an `AssertionError`, which should typically halt execution of the application.

Before we go any further it's worth noting that, by default, assertions are disabled for most JVMs.  Thus, executing code with a failing assertion will completely ignore the failure and will not throw an `AssertionError`.  This is typically not desired, since the purpose of using `assert` expressions at all is to properly test the assumptions throughout your code.  To enable assertion checking you'll need to add a command-line switch: `-enableassertions` or `-ea`.  For example, if you're executing code via the `java` command line tool, you'd need to issue something like the following command: `$ java -ea <class>`.  For most IDEs such as IntelliJ IDEA or Eclipse, you should look in the `run configuration` for `JVM/VM` command-line options, in which you can add the `-ea` flag.

There is a bit of discussion and debate in the Java development community about the exact and proper use of assertions, but the general consensus is that the appearance of an `AssertionError` should indicate a fundamentally broken application/code snippet.  Consequently, `assert` statements should be used as a form of sanity checks as final "no-turning-back" statements that should never be reachable by proper code.  In other words, it is common to use an `assert` statement that always produces a `false` value, yet in a location where execution of the `assert` statement should not be possible.

To illustrate this particular usage of assertions we've created a modified version of our `Book` class called `BookWithStringPublicationType`:

```java
// Book.java
package io.airbrake;

import com.fasterxml.jackson.annotation.JsonIgnoreProperties;
import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import io.airbrake.utility.Logging;

import java.io.UnsupportedEncodingException;
import java.text.DateFormat;
import java.util.Date;

/**
 * Simple example class to store book instances.
 */
@JsonIgnoreProperties(ignoreUnknown = true)
public class BookWithStringPublicationType
{
    private String author;
    private String title;
    private Integer pageCount;
    private Date publishedAt;
    private String publicationType = "DIGITAL";

    private static final Integer maximumPageCount = 4000;

    /**
     * Constructs an empty book.
     */
    public BookWithStringPublicationType() { }

    /**
     * Constructs a basic book.
     *
     * @param title Book title.
     * @param author Book author.
     */
    public BookWithStringPublicationType(String title, String author) {
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
    public BookWithStringPublicationType(String title, String author, Integer pageCount) {
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
    public BookWithStringPublicationType(String title, String author, Integer pageCount, Date publishedAt) {
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
    public BookWithStringPublicationType(String title, String author, Integer pageCount, Date publishedAt, String publicationType) {
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
    void setPublicationType(String type) {
        switch(type) {
            case "DIGITAL":
                this.publicationType = type;
                break;
            case "PAPERBACK":
                this.publicationType = type;
                break;
            // Default assertion should never execute; otherwise, code is improper.
            default: assert false : String.format("PublicationType of [%s] is unacceptable.", type);
        }
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

As the name indicates we're using a simple `String` value to store the `PublicationType` field of each `Book`.  We then perform a simple sanity check prior to setting the value within the `setPublicationType(String type)` method:

```java
/**
* Set publication type of book.
*
* @param type Publication type.
*/
void setPublicationType(String type) {
    switch(type) {
        case "DIGITAL":
            this.publicationType = type;
            break;
        case "PAPERBACK":
            this.publicationType = type;
            break;
        // Default assertion should never execute; otherwise, code is improper.
        default:
            assert false : String.format("PublicationType of [%s] is unacceptable.", type);
    }
}
```

As you can see we perform a simple `switch` case test on the passed `String type` parameter.  If it is one of the two valid values we set the value and explicitly `break` from the `switch` statement.  However, any other value will reach the `default` case, which contains an always-false `assert false` expression, with an additional argument passed to it that will be used for the error message of the subsequent `AssertionError` instance.  In this case, the goal of the code is to be completely certain that a `publicationType` field can never be set to something that is invalid.  Attempting to do so will produce a failure via an `AssertionError`, which will require alteration by a developer to fix the bug.

To illustrate how this works in practice we have the `testEnumPublicationType` method:

```java
private static void testStringPublicationType() {
    try {
        // Create book with PublicationType: 'PAPERBACK'
        BookWithStringPublicationType bookWithStringPublicationType = new BookWithStringPublicationType(
                "The Wise Man's Fear",
                "Patrick Rothfuss",
                994,
                new GregorianCalendar(2011, 2, 1).getTime(),
                "PAPERBACK");
        // Output Book.
        Logging.log(bookWithStringPublicationType);
        // Change to invalid publication type.
        Logging.lineSeparator("CHANGE PUBLICATION TYPE TO 'INVALID' String", 60);
        bookWithStringPublicationType.setPublicationType("INVALID");
        // Output modified Book.
        Logging.log(bookWithStringPublicationType);
    } catch (AssertionError error) {
        // Output expected AssertionErrors.
        Logging.log(error);
    } catch (Exception exception) {
        // Output unexpected Exceptions.
        Logging.log(exception, false);
    }
}
```

We start by creating a valid `BookWithStringPublicationType` instance with a `publicationType` property of `"PAPERBACK"`.  We then try to change to `publicationType` to `"INVALID"` before outputting the resulting object.  To give us something to show when a failure occurs we're explicitly catching any `AssertionErrors` that are thrown here, but in normal code we _would not_ want to `catch` such errors, allowing them to instead crash the application.

Executing the `testStringPublicationType()` method produces the following output:

```
io.airbrake.BookWithStringPublicationType@50675690[
  author=Patrick Rothfuss
  title=The Wise Man's Fear
  pageCount=994
  publishedAt=Tue Mar 01 00:00:00 PST 2011
  publicationType=PAPERBACK
]
------- CHANGE PUBLICATION TYPE TO 'INVALID' String --------
[EXPECTED] java.lang.AssertionError: PublicationType of [INVALID] is unacceptable.
```

Everything works just as expected.  Our original `BookWithStringPublicationType` instance as a `"PAPERBACK"` is instantiating just fine, but attempting to change it to `"INVALID"` throws an `AssertionError`, since execution within the `BookWithStringPublicationType.setPublicationType(String type)` method reached the final, `default` switch case and executing the `assert false` statement.

While this example illustrates one common way to use assertions in Java, we can also modify how the `BookWithStringPublicationType` class handles the `publicationType` to ensure it doesn't rely on an assertion statement, while _also_ ensuring that code cannot try to set invalid values for the field.  We do this for the plain `Book` class by using the `PublicationType` `enum`:

```java
// PublicationType.java
package io.airbrake;

public enum PublicationType {
    DIGITAL,
    PAPERBACK,
}
```

Since only the `publicationType` field and its related getter/setter methods were modified to use the `PublicationType` enum we'll only look at these changes in code:

```java
public class Book
{
    // ...

    private PublicationType publicationType = PublicationType.DIGITAL;

    /**
     * Get publication type of book.
     *
     * @return Publication type.
     */
    public PublicationType getPublicationType() { return publicationType; }

    /**
     * Set publication type of book.
     *
     * @param type Publication type.
     */
    public void setPublicationType(PublicationType type) { this.publicationType = type; }

    // ...
}
```

As with enumerations in other languages, using one in Java allows us to maintain a collection of valid values for a particular data type.  To illustrate this the `testEnumPublicationType()` method seen below creates an intial `Book` instance with the `PublicationType.PAPERBACK` value for `publicationType`, then attempts to invoke the `book.setPublicationType(PublicationType.INVALID)` method call to change it:

```java
private static void testEnumPublicationType() {
    try {
        // Create book with PublicationType: PublicationType.PAPERBACK
        Book book = new Book(
                "The Name of the Wind",
                "Patrick Rothfuss",
                662,
                new GregorianCalendar(2007, 2, 27).getTime(),
                PublicationType.PAPERBACK);
        // Output Book.
        Logging.log(book);
        // Change to invalid publication type.
        Logging.lineSeparator("CHANGE PUBLICATION TYPE TO 'PublicationType.INVALID'", 60);
        book.setPublicationType(PublicationType.INVALID);
        // Output modified Book.
        Logging.log(book);
    } catch (AssertionError error) {
        // Output expected AssertionErrors.
        Logging.log(error);
    } catch (Exception exception) {
        // Output unexpected Exceptions.
        Logging.log(exception, false);
    }
}
```

As it happens, since the JVM can identify the enumeration that is used here during compilation time, we can't even execute this code -- the compiler detects that `PublicationType.INVALID` is, well, not a valid symbol within the `PublicationType` enum, so it halts compilation and delivers an error message.  This implementation serves the same purpose of the `assert` technique seen above, except it captures an issue at compilation/development time, rather than during runtime.

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A deep dive into the Java AssertionError, including fully functional code showing how to use assertions within Java, and how to avoid it at times.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html