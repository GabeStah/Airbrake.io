# Java Exception Handling - FileNotFoundException

Next up in our in-depth [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series we'll take a closer look at the FileNotFoundException.  As you probably can guess, the `FileNotFoundException` is thrown when calling a number of IO methods in which you've passed an invalid file path.  We'll look at where `FileNotFoundException` sits in the [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy), and also go over some functional code samples showing how this error might come about, so let's get to it!

## The Technical Rundown

All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`java.lang.Object`](https://docs.oracle.com/javase/8/docs/api/java/lang/Object.html)
    - [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy)
        - [`java.lang.Exception`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html)
            - [`java.io.IOException`](https://docs.oracle.com/javase/8/docs/api/java/io/IOException.html)
                - `FileNotFoundException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```java
package io.airbrake;

import com.fasterxml.jackson.databind.ObjectMapper;
import io.airbrake.utility.Logging;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileWriter;
import java.util.GregorianCalendar;

public class Main {

    public static void main(String[] args) {
        Logging.lineSeparator("BOOKS TO JSON");
        writeBookJson("books.json");

        Logging.lineSeparator("JSON TO BOOKS");
        readBookJson("books.json");

        Logging.lineSeparator("INVALID JSON FILE TO BOOKS");
        readBookJson("invalid.json");
    }

    /**
     * Create file at path with Book elements converted to Json.
     *
     * @param path File path to create.
     */
    private static void writeBookJson(String path) {
        try {
            // Create FileWriter from path.
            FileWriter writer = new FileWriter(path);

            // Generate JSON from Books.
            writer.write("[");

            writer.write(new Book(
                    "A Game of Thrones",
                    "George R.R. Martin",
                    848,
                    new GregorianCalendar(1996, 8, 6).getTime()
            ).toJsonString());

            writer.write(",");

            writer.write(new Book(
                    "The Name of the Wind",
                    "Patrick Rothfuss",
                    662,
                    new GregorianCalendar(2007, 3, 27).getTime()
            ).toJsonString());

            writer.write("]");

            // Flush and close writer.
            writer.flush();
            writer.close();

            Logging.log(String.format("Books added to file: %s", path));
        } catch (FileNotFoundException exception) {
            // Output expected FileNotFoundExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }

    /**
     * Read Books from Json file.
     *
     * @param path Path of Json file to read.
     */
    private static void readBookJson(String path) {
        try {
            // Create new mapper, read value from new file of
            // passed path, and map to array of Book elements.
            Book[] books = new ObjectMapper().readValue(new File(path), Book[].class);
            // Output each Book in array.
            for (Book book : books) {
                Logging.log(book);
            }
        } catch (FileNotFoundException exception) {
            // Output expected FileNotFoundExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }

    }
}

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

The `FileNotFoundException` is thrown when attempting to access a file that doesn't exist, so let's just right into some code to see how it works.  For this example, we're using the `Book` class that we've used in the past to create a couple books, which we then output to a new file in the form of `JSON`.  To easily convert from a `Book` object to a JSON string we are using the `Jackson` library, and in particular the [`com.fasterxml.jackson.databind.ObjectMapper()`](https://github.com/FasterXML/jackson-databind) class:

```java
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

    // ...

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

    // ...

    /**
     * Output
     *
     * @return
     * @throws JsonProcessingException
     */
    public String toJsonString() throws JsonProcessingException {
        return new ObjectMapper().writeValueAsString(this);
    }

    // ...
}
```

The [`writeValueAsString(Object value)`](http://fasterxml.github.io/jackson-databind/javadoc/2.9/com/fasterxml/jackson/databind/ObjectMapper.html#writeValueAsString(java.lang.Object)) method allows us to convert the current `Book` instance into a JSON string value.  `Jackson` handles all the reflection for us, with the special note that we need to implement the [`@JsonIgnoreProperties(ignoreUnknown = true)`](https://fasterxml.github.io/jackson-annotations/javadoc/2.9/com/fasterxml/jackson/annotation/JsonIgnoreProperties.html) interface at the top of the `Book` class definition, which tells the parser to ignore properties it doesn't recognize when reading from a JSON source.  This is necessary because of the `tagline` property obtained from the `Book.getTagline()` method.  `Jackson` picks up on this method and outputs the value in the produced JSON file, but since we don't have a matching `setTagline()` method in the `Book` class, we need to ignore it during import:

```java
/**
* Get a formatted tagline with author, title, and page count.
*
* @return Formatted tagline.
*/
public String getTagline() {
    return String.format("'%s' by %s is %d pages.", this.title, this.author, this.pageCount);
}
```

Next, we have the `readBookJson(String path)` method, which attempts to read the JSON from the provided file `path` parameter, and tries to map it to an `Array` of `Books`, which are then output to the log:

```java
/**
* Read Books from Json file.
*
* @param path Path of Json file to read.
*/
private static void readBookJson(String path) {
    try {
        // Create new mapper, read value from new file of
        // passed path, and map to array of Book elements.
        Book[] books = new ObjectMapper().readValue(new File(path), Book[].class);
        // Output each Book in array.
        for (Book book : books) {
            Logging.log(book);
        }
    } catch (FileNotFoundException exception) {
        // Output expected FileNotFoundExceptions.
        Logging.log(exception);
    } catch (Exception exception) {
        // Output unexpected Exceptions.
        Logging.log(exception, false);
    }
}
```

Lastly, the `writeBookJson(String path)` method performs pretty much the opposite of `readBookJson(String path)`, by attempting to write the data of a few `Books` to the provided `path` file location:

```java
/**
* Create file at path with Book elements converted to Json.
*
* @param path File path to create.
*/
private static void writeBookJson(String path) {
    try {
        // Create FileWriter from path.
        FileWriter writer = new FileWriter(path);

        // Generate JSON from Books.
        writer.write("[");

        writer.write(new Book(
                "A Game of Thrones",
                "George R.R. Martin",
                848,
                new GregorianCalendar(1996, 8, 6).getTime()
        ).toJsonString());

        writer.write(",");

        writer.write(new Book(
                "The Name of the Wind",
                "Patrick Rothfuss",
                662,
                new GregorianCalendar(2007, 3, 27).getTime()
        ).toJsonString());

        writer.write("]");

        // Flush and close writer.
        writer.flush();
        writer.close();

        Logging.log(String.format("Books added to file: %s", path));
    } catch (FileNotFoundException exception) {
        // Output expected FileNotFoundExceptions.
        Logging.log(exception);
    } catch (Exception exception) {
        // Output unexpected Exceptions.
        Logging.log(exception, false);
    }
}
```

_Note_: This is obviously a bit of a hackey way to create valid JSON (explicitly writing out opening/closing brackets, and comma-separators), but the goal here is to provide a somewhat realistic example of reading/writing files, rather than a perfect illustration of working with JSON.

Anyway, with everything now setup we can test stuff out by first trying to write some data to a new file:

```java
Logging.lineSeparator("BOOKS TO JSON");
writeBookJson("books.json");
```

This creates a new `books.json` file and populates it with the `Book` data, as converted to JSON, that we expected:

```json
[{
	"author": "George R.R. Martin",
	"title": "A Game of Thrones",
	"pageCount": 848,
	"publishedAt": 841993200000,
	"tagline": "'A Game of Thrones' by George R.R. Martin is 848 pages."
}, {
	"author": "Patrick Rothfuss",
	"title": "The Name of the Wind",
	"pageCount": 662,
	"publishedAt": 1177657200000,
	"tagline": "'The Name of the Wind' by Patrick Rothfuss is 662 pages."
}]
```

Now let's try reading the data from that file:

```java
Logging.lineSeparator("JSON TO BOOKS");
readBookJson("books.json");
```

If everything was setup correctly, this should see the output of our `Book` objects, as read from the JSON.  Sure enough, that's exactly what we get:

```
------------ JSON TO BOOKS -------------
io.airbrake.Book@d706f19[
  author=George R.R. Martin
  title=A Game of Thrones
  pageCount=848
  publishedAt=Fri Sep 06 00:00:00 PDT 1996
]
io.airbrake.Book@1fe20588[
  author=Patrick Rothfuss
  title=The Name of the Wind
  pageCount=662
  publishedAt=Fri Apr 27 00:00:00 PDT 2007
]
```

Finally, let's see what happens if we try to read the JSON data from an invalid file:

```java
Logging.lineSeparator("INVALID JSON FILE TO BOOKS");
readBookJson("invalid.json");
```

Lo and behold, executing `readBookJson(String path)` with an invalid file path throws a `FileNotFoundException` at us:

```
------ INVALID JSON FILE TO BOOKS ------
[EXPECTED] java.io.FileNotFoundException: invalid.json (The system cannot find the file specified)
```

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look at the Java FileNotFoundException, with sample code illustrating how to convert JSON to Java objects, and vice versa.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html