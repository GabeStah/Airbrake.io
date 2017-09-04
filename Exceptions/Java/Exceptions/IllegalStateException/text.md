# Java Exception Handling - IllegalStateException

Today we make our way to the IllegalStateException in Java, as we continue our journey through [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series.  The "proper" use of the `IllegalStateException` class is somewhat subjective, since the [official documentation](https://docs.oracle.com/javase/8/docs/api/java/lang/IllegalStateException.html) simply states that such an exception "signals that a method has been invoked at an illegal or inappropriate time. In other words, the Java environment or Java application is not in an appropriate state for the requested operation."

Throughout the rest of this article we'll explore the `IllegalStateException` in greater detail, starting with where it resides in the overall [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).  We'll also look at a couple functional code samples that illustrate how `IllegalStateExceptions` are used in built-in Java APIs, as well as how you might throw `IllegalStateExceptions` in your own code, so let's get started!

## The Technical Rundown

All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`java.lang.Object`](https://docs.oracle.com/javase/8/docs/api/java/lang/Object.html)
    - [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy)
        - [`java.lang.Exception`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html)
            - [`java.lang.RuntimeException`](https://docs.oracle.com/javase/8/docs/api/java/lang/RuntimeException.html)
                - `IllegalStateException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```java
package io.airbrake;

import io.airbrake.utility.Logging;

import javax.net.ssl.HttpsURLConnection;
import java.io.IOException;
import java.net.ConnectException;
import java.net.HttpURLConnection;
import java.net.URISyntaxException;
import java.net.URL;
import java.util.GregorianCalendar;

public class Main {

    public static void main(String[] args) throws IOException, URISyntaxException {
        // Publish book with publication date.
        publishBook(new Book(
                "A Game of Thrones",
                "George R.R. Martin",
                848,
                new GregorianCalendar(1996, 8, 6).getTime()));
        // Publish book without publication date.
        publishBook(new Book(
                "Java Exception Handling - IllegalStateException",
                "Andrew Powell-Morse",
                5));

        // Perform connection test using built-in methods.
        connectionTest();
    }

    private static void publishBook(Book book) {
        try {
            Logging.lineSeparator(book.getTitle().toUpperCase(), 60);
            // Attempt to publish book.
            book.publish();
        } catch (IllegalStateException exception) {
            // Output expected IllegalStateException.
            Logging.log(exception);
        } catch (Throwable throwable) {
            // Output unexpected Throwables.
            Logging.log(throwable, false);
        }
    }

    private static void connectionTest() throws IOException, URISyntaxException {
        try {
            // Test connection to a valid host.
            String uri = "https://www.airbrake.io";
            Logging.lineSeparator(String.format("Connecting to %s", uri), 60);
            HttpURLConnection connection = connect(uri);
            // Attempts to set the ifModifiedSince field.
            connection.setIfModifiedSince(0);
        } catch (IllegalStateException exception) {
            // Output expected IllegalStateException.
            Logging.log(exception);
        } catch (Throwable throwable) {
            // Output unexpected Throwables.
            Logging.log(throwable, false);
        }
    }

    /**
     * Attempt connection to passed URI string.
     *
     * @param uri URI string to connect to.
     *
     * @throws IOException
     * @throws URISyntaxException
     */
    private static HttpURLConnection connect(String uri) throws IOException, URISyntaxException {
        try {
            URL url = new URL(uri);
            HttpsURLConnection connection = (HttpsURLConnection) url.openConnection();
            processResponse(connection);
            return connection;
        } catch (IllegalStateException exception) {
            // Output expected IllegalStateException.
            Logging.log(exception);
        } catch (Throwable throwable) {
            // Output unexpected Throwables.
            Logging.log(throwable, false);
        }
        return null;
    }

    /**
     * Logs connection information.
     *
     * @param connection Connection to be logged.
     *
     * @throws IOException
     * @throws URISyntaxException
     */
    private static void logConnection(final HttpURLConnection connection) throws IOException, URISyntaxException {
        int code = connection.getResponseCode();
        String message = connection.getResponseMessage();
        String url = connection.getURL().toURI().toString();

        Logging.log(String.format("Response from %s - Code: %d, Message: %s", url, code, message));
    }

    /**
     * Process an HttpURLConnection response information.
     *
     * @param connection Connection to be processed.
     *
     * @throws IOException
     * @throws URISyntaxException
     */
    private static void processResponse(final HttpURLConnection connection) throws IOException, URISyntaxException {
        try {
            logConnection(connection);
        } catch (ConnectException exception) {
            // Output expected ConnectException.
            Logging.log(exception);
        } catch (Throwable throwable) {
            // Output unexpected Throwables.
            Logging.log(throwable, false);
        }
    }
}

// Book.java
package io.airbrake;

import io.airbrake.utility.Logging;

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
     * Throw an Exception.
     */
    public void throwException(String message) throws Exception {
        throw new Exception(message);
    }
}

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

As mentioned, the properly using the `IllegalStateException` class is really a matter of personal taste and opinion.  For me, I feel it's best used when attempting to manipulate an object instance in such a way that doesn't make sense.  For example, an application that implements the [`state design pattern`](https://en.wikipedia.org/wiki/State_pattern) would contain objects that track some internal `state` of being, such as a field value.  When this object is in a particular `state`, it may be illogical to allow calling/execution of certain methods.  In such cases, an `IllegalStateException` is, in my opinion, the ideal exception to throw.

To illustrate in code we have two unique examples.  The first example we'll go over uses our own `Book` class and explicitly throwing an `IllegalStateException`:

```java
// Book.java
package io.airbrake;

import io.airbrake.utility.Logging;

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
     * Throw an Exception.
     */
    public void throwException(String message) throws Exception {
        throw new Exception(message);
    }
}
```

The first critical method for this code example is the `Book(String title, String author, Integer pageCount, Date publishedAt)` constructor, which allows calling code to pass in a publication date:

```java
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
```

The other important method is `publish()`, which checks if a publication date already exists, in which case it throws a new `IllegalStateException` indicating that the book cannot be published a second time:

```java
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
```

This is simple logic, but it illustrates how you might go about using the `IllegalStateException` in your own code.  Here, we've made the decision to disallow calling `publish()` for a `Book` that has already been published.  Arguably, we could opt to ignore this issue and only perform `publish()` logic when `getPublishedAt()` returns null.  In this case, however, our business logic requires throwing an exception instead.

The code to test this out consists of creating two unique `Book` instances, one with a publication date and one without, and then attempting to `publish()` them through the `publishBook(Book book)` method:

```java
public static void main(String[] args) throws IOException, URISyntaxException {
    // Publish book with publication date.
    publishBook(new Book(
            "A Game of Thrones",
            "George R.R. Martin",
            848,
            new GregorianCalendar(1996, 8, 6).getTime()));
    // Publish book without publication date.
    publishBook(new Book(
            "Java Exception Handling - IllegalStateException",
            "Andrew Powell-Morse",
            5));

    // ...
}

private static void publishBook(Book book) {
    try {
        Logging.lineSeparator(book.getTitle().toUpperCase(), 60);
        // Attempt to publish book.
        book.publish();
    } catch (IllegalStateException exception) {
        // Output expected IllegalStateException.
        Logging.log(exception);
    } catch (Throwable throwable) {
        // Output unexpected Throwables.
        Logging.log(throwable, false);
    }
}
```

Executing this code produces the following output:

```
-------------------- A GAME OF THRONES ---------------------
[EXPECTED] java.lang.IllegalStateException: Cannot publish 'A Game of Thrones' by George R.R. Martin (already published on Fri Sep 06 00:00:00 PDT 1996).
----- JAVA EXCEPTION HANDLING - ILLEGALSTATEEXCEPTION ------
Published 'Java Exception Handling - IllegalStateException' by Andrew Powell-Morse.
```

As desired, attempting to publish the previously-published _A Game of Thrones_ `Book` results in an `IllegalStateException`, while the instance representing this very article doesn't have a publication date, so publishing it works just fine.

In addition to using `IllegalStateException` in your own custom code, these exceptions are also used throughout the codebase of other modules and libraries, including the JDK API.  For example, we can reuse a bit of the code from our previous [`Java Exception Handling - ConnectException`](https://airbrake.io/blog/java-exception-handling/connectexception) article, which attempts to connect to a provided URL and outputs the results:

```java
private static void connectionTest() throws IOException, URISyntaxException {
    try {
        // Test connection to a valid host.
        String uri = "https://www.airbrake.io";
        Logging.lineSeparator(String.format("Connecting to %s", uri), 60);
        HttpURLConnection connection = connect(uri);
        // Attempts to set the ifModifiedSince field.
        connection.setIfModifiedSince(0);
    } catch (IllegalStateException exception) {
        // Output expected IllegalStateException.
        Logging.log(exception);
    } catch (Throwable throwable) {
        // Output unexpected Throwables.
        Logging.log(throwable, false);
    }
}

/**
* Attempt connection to passed URI string.
*
* @param uri URI string to connect to.
*
* @throws IOException
* @throws URISyntaxException
*/
private static HttpURLConnection connect(String uri) throws IOException, URISyntaxException {
    try {
        URL url = new URL(uri);
        HttpsURLConnection connection = (HttpsURLConnection) url.openConnection();
        processResponse(connection);
        return connection;
    } catch (IllegalStateException exception) {
        // Output expected IllegalStateException.
        Logging.log(exception);
    } catch (Throwable throwable) {
        // Output unexpected Throwables.
        Logging.log(throwable, false);
    }
    return null;
}

/**
* Logs connection information.
*
* @param connection Connection to be logged.
*
* @throws IOException
* @throws URISyntaxException
*/
private static void logConnection(final HttpURLConnection connection) throws IOException, URISyntaxException {
    int code = connection.getResponseCode();
    String message = connection.getResponseMessage();
    String url = connection.getURL().toURI().toString();

    Logging.log(String.format("Response from %s - Code: %d, Message: %s", url, code, message));
}

/**
* Process an HttpURLConnection response information.
*
* @param connection Connection to be processed.
*
* @throws IOException
* @throws URISyntaxException
*/
private static void processResponse(final HttpURLConnection connection) throws IOException, URISyntaxException {
    try {
        logConnection(connection);
    } catch (ConnectException exception) {
        // Output expected ConnectException.
        Logging.log(exception);
    } catch (Throwable throwable) {
        // Output unexpected Throwables.
        Logging.log(throwable, false);
    }
}
```

The critical addition is within the `connectionTest()` method, where we attempt to invoke the `setIfModifiedSince(long ifmodifiedsince)` method _after_ we've already established a connection.  The source code of the `java.net.URLConnection` class shows that this throws an `IllegalStateException`, since we've already established a connection (and, therefore, setting this field makes no sense):

```java
public abstract class URLConnection {
    // ...

    /**
     * Sets the value of the {@code ifModifiedSince} field of
     * this {@code URLConnection} to the specified value.
     *
     * @param   ifmodifiedsince   the new value.
     * @throws IllegalStateException if already connected
     * @see     #getIfModifiedSince()
     */
    public void setIfModifiedSince(long ifmodifiedsince) {
        if (connected)
            throw new IllegalStateException("Already connected");
        ifModifiedSince = ifmodifiedsince;
    }

    // ...
}
```

Sure enough, executing the `connectionTest()` method successfully connects, but then throws an `IllegalStateException` when invoking `setIfModifiedSince(long ifmodifiedsince)`:

```
---------- Connecting to https://www.airbrake.io -----------
Response from https://airbrake.io/ - Code: 200, Message: OK
[EXPECTED] java.lang.IllegalStateException: Already connected
```

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A deep dive into the Java IllegalStateException, with sample code illustrating its usage in both custom code and built-in JDK APIs.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html