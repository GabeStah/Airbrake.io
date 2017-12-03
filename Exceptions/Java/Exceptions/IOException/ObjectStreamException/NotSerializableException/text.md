# Java Exception Handling - NotSerializableException

Continuing along through our in-depth [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series, today we'll be looking into the **NotSerializableException**.  The `NotSerializableException` is thrown when attempting to serialize or deserialize an object that does not implement the [`java.io.Serializable`](https://docs.oracle.com/javase/8/docs/api/java/io/Serializable.html) interface.

Throughout this article we'll get into the nitty gritty of the `NotSerializableException`, starting with where it resides in the overall [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).  We'll also dig into a functional code example illustrating how to perform basic serialization on Java objects, and how improperly doing so might lead to `NotSerializableExceptions`, so let's get to it!

## The Technical Rundown

All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`java.lang.Object`](https://docs.oracle.com/javase/8/docs/api/java/lang/Object.html)
    - [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy)
        - [`java.lang.Error`](https://docs.oracle.com/javase/8/docs/api/java/lang/Error.html)
            - [`java.io.IOException`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html)
                - [`java.io.ObjectStreamException`](https://docs.oracle.com/javase/8/docs/api/java/io/ObjectStreamException.html)
                    - `NotSerializableException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```java
package io.airbrake;

import io.airbrake.utility.Logging;

import java.io.*;
import java.util.GregorianCalendar;

public class Main {

    public static void main(String[] args) {
        // Create book and test serialization.
        Book nameOfTheWind = new Book("The Name of the Wind", "Patrick Rothfuss", 662, new GregorianCalendar(2007, 2, 27).getTime());
        TestBookSerialization(nameOfTheWind);

        // Create and test another book.
        Book wiseMansFear = new Book("The Wise Man's Fear", "Patrick Rothfuss", 994, new GregorianCalendar(2011, 2, 1).getTime());
        TestBookSerialization(wiseMansFear);
    }

    /**
     * Tests serialization of passed Book object.
     *
     * @param book Book to be tested.
     */
    private static void TestBookSerialization(Book book) {
        // Determine serialization file name.
        String fileName = String.format("%s.ser", book.toFilename());
        // Output file info.
        Logging.lineSeparator(String.format("SAVING TO FILE: %s", fileName), 75);
        // Serialize Book to file.
        Serialize(book, fileName);
        // Deserialize Book from file.
        Deserialize(fileName);
    }

    /**
     * Serializes the passed object to the passed filePath.
     *
     * @param object Object that is being serialized.
     * @param filePath File path where object should be stored.
     */
    private static void Serialize(Object object, String filePath) {
        try {
            FileOutputStream fileOutputStream = new FileOutputStream(filePath);
            ObjectOutputStream objectOutputStream = new ObjectOutputStream(fileOutputStream);
            objectOutputStream.writeObject(object);
            fileOutputStream.close();
            Logging.log(String.format("SERIALIZED [%s]: %s", object.getClass().getName(), object));
        } catch (NotSerializableException exception) {
            // Output expected NotSerializeableExceptions.
            Logging.log(exception);
        } catch (IOException exception) {
            // Output unexpected IOException.
            Logging.log(exception, false);
        }
    }

    /**
     * Deserializes object found in passed filePath.
     *
     * @param filePath Path to file where serialized object is found.
     * @param <T> Type of object that is expected.
     */
    private static <T> void Deserialize(String filePath) {
        try {
            FileInputStream fileInputStream = new FileInputStream(filePath);
            ObjectInputStream objectInputStream = new ObjectInputStream(fileInputStream);
            T object = (T) objectInputStream.readObject();
            objectInputStream.close();
            fileInputStream.close();
            Logging.log(String.format("DESERIALIZED [%s]: %s", object.getClass().getName(), object));
        } catch (NotSerializableException exception) {
            // Output expected NotSerializeableExceptions.
            Logging.log(exception);
        } catch (IOException | ClassNotFoundException exception) {
            // Output unexpected IOExceptions and ClassNotFoundExceptions.
            Logging.log(exception, false);
        }
    }
}
```

```java
// Book.java
package io.airbrake;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.annotation.*;
import io.airbrake.utility.Logging;

import java.io.Serializable;
import java.io.UnsupportedEncodingException;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.text.DateFormat;
import java.util.Date;

/**
 * Simple example class to store book instances.
 */
@JsonIgnoreProperties(ignoreUnknown = true)
public class Book implements Serializable
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
     * Get a formatted tagline with author, title, page count, and publication date.
     *
     * @return Formatted tagline.
     */
    public String getTagline() {
        return String.format("'%s' by %s is %d pages, published %s.",
                getTitle(),
                getAuthor(),
                getPageCount(),
                DateFormat.getDateInstance().format(getPublishedAt()));
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

Let's briefly take a moment to review what serialization is, in a general programming sense.  Effectively, serialization is the act of transforming a structured piece of data (such as an object) into a format that can be easily stored and transmitted.  Typically, a serialized format might be a memory stream, a series of characters (i.e. a string), a local file, and so forth.  Additionally, deserialization is the act of reversing a previous serialization process -- converting a string, memory stream, file, or what not to the original structured piece of data (object) that the element began as.

There are many advantages to serialization, but this ability to convert complex data structures into a rudimentary form (such as a string) makes it particularly easy to transmit specific objects from one source to another, then re-create the original with no data loss.

As with many other programming languages, Java includes built-in serialization support through the [`Serializable`](https://docs.oracle.com/javase/8/docs/api/java/io/Serializable.html) interface.  Implementing this interface within a class definition allows that class to be serialized and deserialized through standard methods.  To illustrate, we begin with our custom `Book` class:

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
public class Book implements Serializable
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
     * Get a formatted tagline with author, title, page count, and publication date.
     *
     * @return Formatted tagline.
     */
    public String getTagline() {
        return String.format("'%s' by %s is %d pages, published %s.",
                getTitle(),
                getAuthor(),
                getPageCount(),
                DateFormat.getDateInstance().format(getPublishedAt()));
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

Most importantly, we've implemented the `Serializable` interface for the `Book` class, which will allow our `Book` objects to be serialized:

```java
public class Book implements Serializable
{
    // ...
}
```

We've also added the `toFilename()` method, which attempts to encode a compatible file name out of the `author` and `title` fields of the `Book` instance.  We'll use this somewhat unique filename to create a local file for serialization purposes:

```java
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
```

Speaking of which, let's look at our `Main.Serialize(Object object, String filePath)` method:

```java
/**
* Serializes the passed object to the passed filePath.
*
* @param object Object that is being serialized.
* @param filePath File path where object should be stored.
*/
private static void Serialize(Object object, String filePath) {
    try {
        FileOutputStream fileOutputStream = new FileOutputStream(filePath);
        ObjectOutputStream objectOutputStream = new ObjectOutputStream(fileOutputStream);
        objectOutputStream.writeObject(object);
        fileOutputStream.close();
        Logging.log(String.format("SERIALIZED [%s]: %s", object.getClass().getName(), object));
    } catch (NotSerializableException exception) {
        // Output expected NotSerializeableExceptions.
        Logging.log(exception);
    } catch (IOException exception) {
        // Output unexpected IOException.
        Logging.log(exception, false);
    }
}
```

This method attempts to serialize the passed `Object object` parameter by passing it to the `ObjectOutputStream.writeObject(...)` method.  Our `ObjectOutputStream` instance is attempted to write to the `FileOutputStream` created from the `String filePath` parameter.  If successful, we output a log message, otherwise we catch and log any exceptions.

Conversely, the `Deserialize(String filePath)` generic type method reverses the process by calling `ObjectInputStream.readObject()` to deserialize the object found in the passed `String filePath` location:

```java
/**
* Deserializes object found in passed filePath.
*
* @param filePath Path to file where serialized object is found.
* @param <T> Type of object that is expected.
*/
private static <T> void Deserialize(String filePath) {
    try {
        FileInputStream fileInputStream = new FileInputStream(filePath);
        ObjectInputStream objectInputStream = new ObjectInputStream(fileInputStream);
        T object = (T) objectInputStream.readObject();
        objectInputStream.close();
        fileInputStream.close();
        Logging.log(String.format("DESERIALIZED [%s]: %s", object.getClass().getName(), object));
    } catch (NotSerializableException exception) {
        // Output expected NotSerializeableExceptions.
        Logging.log(exception);
    } catch (IOException | ClassNotFoundException exception) {
        // Output unexpected IOExceptions and ClassNotFoundExceptions.
        Logging.log(exception, false);
    }
}
```

To test this out we have the `TestBookSerialization(Book book)` wrapper method:

```java
/**
* Tests serialization of passed Book object.
*
* @param book Book to be tested.
*/
private static void TestBookSerialization(Book book) {
    // Determine serialization file name.
    String fileName = String.format("%s.ser", book.toFilename());
    // Output file info.
    Logging.lineSeparator(String.format("SAVING TO FILE: %s", fileName), 75);
    // Serialize Book to file.
    Serialize(book, fileName);
    // Deserialize Book from file.
    Deserialize(fileName);
}
```

As you can see, all we're doing here is creating a local file name (using the `Book.toFilename()` method we looked at before), then attempting to first serialize and then deserialize the passed `Book book` object.

To see if everything works as expected our `Main.main(...)` method creates two different `Book` instances and performs our serialization tests:

```java
public static void main(String[] args) {
    // Create book and test serialization.
    Book nameOfTheWind = new Book("The Name of the Wind", "Patrick Rothfuss", 662, new GregorianCalendar(2007, 2, 27).getTime());
    TestBookSerialization(nameOfTheWind);

    // Create and test another book.
    Book wiseMansFear = new Book("The Wise Man's Fear", "Patrick Rothfuss", 994, new GregorianCalendar(2011, 2, 1).getTime());
    TestBookSerialization(wiseMansFear);
}
```

Executing the `Main.main()` method produces the following output:

```
-------- SAVING TO FILE: the+name+of+the+wind-patrick+rothfuss.ser --------
SERIALIZED [io.airbrake.Book]: 'The Name of the Wind' by Patrick Rothfuss is 662 pages, published Mar 27, 2007.
DESERIALIZED [io.airbrake.Book]: 'The Name of the Wind' by Patrick Rothfuss is 662 pages, published Mar 27, 2007.
------- SAVING TO FILE: the+wise+man%27s+fear-patrick+rothfuss.ser --------
SERIALIZED [io.airbrake.Book]: 'The Wise Man's Fear' by Patrick Rothfuss is 994 pages, published Mar 1, 2011.
DESERIALIZED [io.airbrake.Book]: 'The Wise Man's Fear' by Patrick Rothfuss is 994 pages, published Mar 1, 2011.
```

Everything seems to be working just as expected.  Our encoded filenames are created, and our `Book` instances are successfully being serialized and saved to the file, then deserialized.  To confirm, if we open up the `the+name+of+the+wind-patrick+rothfuss.ser` local file the contents look like this:

```
�� sr io.airbrake.Bookm���smh4 L authort Ljava/lang/String;L 	pageCountt Ljava/lang/Integer;L publishedAtt Ljava/util/Date;L titleq ~ xpt Patrick Rothfusssr java.lang.Integer⠤���8 I valuexr java.lang.Number������  xp  �sr java.util.Datehj�KYt  xpw  �1��xt The Name of the Wind
```

Looks like a mess to us since it's not stored in a human-readable format, but this further confirms that our serialization is working properly.

However, let's pretend we did everything as before, expect for one minor mistake: We forgot to add `implements Serializable` to our `Book` class definition:

```java
public class Book
{
    // ...
}
```

With all other testing code exactly as it was before, executing our `Main.main()` method again now produces the following output:

```
-------- SAVING TO FILE: the+name+of+the+wind-patrick+rothfuss.ser --------
[EXPECTED] java.io.NotSerializableException: io.airbrake.Book
[UNEXPECTED] java.io.WriteAbortedException: writing aborted; java.io.NotSerializableException: io.airbrake.Book
------- SAVING TO FILE: the+wise+man%27s+fear-patrick+rothfuss.ser --------
[EXPECTED] java.io.NotSerializableException: io.airbrake.Book
[UNEXPECTED] java.io.WriteAbortedException: writing aborted; java.io.NotSerializableException: io.airbrake.Book
```

Suddenly we experience our first `NotSerializableExceptions`, which indicate that the `io.airbrake.Book` class is not serializable.  In this case, we know it's because we didn't implement the `Serializable` interface.  However, it's worth noting that serialization can _only_ work if the entire series of related objects are serializable.  This means that, if one of the fields of our `Book` class was `private Publisher publisher`, where `Publisher` was a unique class of its own, we could only serialize a `Book` instance if _both_ `Book` and `Publisher` classes implemented the `Serializable` interface.  Alternatively, we could also serialize a `Book` instance even if `Publisher` did not implement `Serializable`, but only _if_ the `private Publisher publisher` field was annotated as a [`transient`](https://docs.oracle.com/javase/8/docs/api/java/beans/Transient.html) field.  The `transient` annotation indicates that the field should _not_ be serialized.

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look at the Java NotSerializableException, including fully functional code showing how to serialize and deserialize Java objects.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html