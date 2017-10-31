# Java Exception Handling - EOFException

Making our way through our detailed [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series, today we'll tackle the **EOFException**.  Most developers will probably recognize that the acronym `EOF` in this exception name usually stands for "end of file", which is exactly the case here.  When an `EOFException` is thrown in Java, this indicates that the end of the file or stream has been reached unexpectedly.

In this article we'll examine the `EOFException` in more detail, starting with where it sits in the larger [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).  We'll also go over some functional sample code that shows basic file manipulation, and how failing to improperly handle reaching the end of file or memory streams will result in uncaught `EOFExceptions`.  Let's get to it!

## The Technical Rundown

All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`java.lang.Object`](https://docs.oracle.com/javase/8/docs/api/java/lang/Object.html)
    - [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy)
        - [`java.lang.Exception`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html)
            - [`java.io.IOException`](https://docs.oracle.com/javase/8/docs/api/java/io/IOException.html)
                - `EOFException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```java
package io.airbrake;

import io.airbrake.utility.Logging;

import java.io.*;
import java.util.Arrays;
import java.util.GregorianCalendar;
import java.util.List;

public class Main {

    private static final String FILE = "books.txt";

    private static final List<Book> DATA = Arrays.asList(
            new Book("The Name of the Wind",
                    "Patrick Rothfuss",
                    662,
                    new GregorianCalendar(2007, 2, 27).getTime()),
            new Book("The Wise Man's Fear",
                    "Patrick Rothfuss",
                    994,
                    new GregorianCalendar(2011, 2, 1).getTime()),
            new Book("Doors of Stone",
                    "Patrick Rothfuss",
                    896,
                    new GregorianCalendar(2049, 2, 5).getTime())
    );

    public static void main(String[] args) {
        WriteBooksToFile();

        ReadBooksFromFileImproperly();

        ReadBooksFromFile();
    }

    private static void ReadBooksFromFileImproperly() {
        try {
            DataInputStream inputStream = new DataInputStream(new BufferedInputStream(new FileInputStream(FILE)));

            Logging.lineSeparator(String.format("READING FROM FILE: %s", FILE));
            while (true) {
                String description = inputStream.readUTF();
                Logging.log(description);
            }
        } catch (EOFException exception) {
            // Output expected EOFExceptions.
            Logging.log(exception);
        } catch (IOException exception) {
            // Output unexpected IOExceptions.
            Logging.log(exception, false);
        }
    }

    private static void ReadBooksFromFile() {
        DataInputStream inputStream = null;
        try {
            inputStream = new DataInputStream(new BufferedInputStream(new FileInputStream(FILE)));

            Logging.lineSeparator(String.format("READING FROM FILE: %s", FILE));

            while (true) {
                // Use inner exception block to determine end of file.
                try {
                    String description = inputStream.readUTF();
                    Logging.log(description);
                } catch (EOFException exception) {
                    // Break while loop when file ends.
                    break;
                } catch (IOException exception) {
                    // Output unexpected IOExceptions.
                    Logging.log(exception, false);
                }
            }
        } catch (IOException exception) {
            // Output unexpected IOExceptions.
            Logging.log(exception, false);
        } finally {
            try {
                if (inputStream != null) {
                    inputStream.close();
                }
            } catch (IOException exception) {
                // Output unexpected IOExceptions.
                Logging.log(exception, false);
            }
        }
    }

    private static void WriteBooksToFile() {
        try {
            DataOutputStream outputStream = new DataOutputStream(new BufferedOutputStream(new FileOutputStream(FILE)));

            Logging.lineSeparator(String.format("WRITING TO FILE: %s", FILE));
            for (Book book : DATA) {
                outputStream.writeUTF(book.toString());
                Logging.log(book);
            }

            outputStream.close();
        } catch (EOFException exception) {
            // Output expected EOFExceptions.
            Logging.log(exception);
        } catch (IOException exception) {
            // Output unexpected IOExceptions.
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

Since the appearance of an `EOFException` simply indicates that the end of the file or memory stream was reached, the best way to show how to _properly_ use and handle this exception is in code, so let's jump right into our sample.

We start with a basic `Book` class that contains a few fields, which we'll be using to create some real-world objects to output to a local file:

```java
// Book.java
package io.airbrake;

import com.fasterxml.jackson.core.JsonProcessingException;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.annotation.*;
import io.airbrake.utility.Logging;

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

In our `Main` program class we start by defining a basic `List<Book>` private property called `DATA`, along with the path to our `FILE`:

```java
public class Main {

    private static final String FILE = "books.txt";

    private static final List<Book> DATA = Arrays.asList(
            new Book("The Name of the Wind",
                    "Patrick Rothfuss",
                    662,
                    new GregorianCalendar(2007, 2, 27).getTime()),
            new Book("The Wise Man's Fear",
                    "Patrick Rothfuss",
                    994,
                    new GregorianCalendar(2011, 2, 1).getTime()),
            new Book("Doors of Stone",
                    "Patrick Rothfuss",
                    896,
                    new GregorianCalendar(2049, 2, 5).getTime())
    );

    // ...

}
```

Next we have the `WriteBooksToFile()` method, which does just as the name suggests:

```java
private static void WriteBooksToFile() {
    try {
        DataOutputStream outputStream = new DataOutputStream(new BufferedOutputStream(new FileOutputStream(FILE)));

        Logging.lineSeparator(String.format("WRITING TO FILE: %s", FILE));
        for (Book book : DATA) {
            outputStream.writeUTF(book.toString());
            Logging.log(book);
        }

        outputStream.close();
    } catch (EOFException exception) {
        // Output expected EOFExceptions.
        Logging.log(exception);
    } catch (IOException exception) {
        // Output unexpected IOExceptions.
        Logging.log(exception, false);
    }
}
```

By using a `DataOutputStream` instance we're able to loop through the collection of `Books` found in our `DATA` property and create a new string via the `writeUTF(...)` method.  Once complete, we close the stream, and all is taken care of.  Executing this method produces the following output to the log:

```
------ WRITING TO FILE: books.txt ------
io.airbrake.Book@28864e92[
  author=Patrick Rothfuss
  title=The Name of the Wind
  pageCount=662
  publishedAt=Tue Mar 27 00:00:00 PDT 2007
]
io.airbrake.Book@4ec6a292[
  author=Patrick Rothfuss
  title=The Wise Man's Fear
  pageCount=994
  publishedAt=Tue Mar 01 00:00:00 PST 2011
]
io.airbrake.Book@1b40d5f0[
  author=Patrick Rothfuss
  title=Doors of Stone
  pageCount=896
  publishedAt=Fri Mar 05 00:00:00 PST 2049
]
```

To confirm the formatted `Book` strings are being locally saved we can open up the local `books.txt` file.  Here's the current contents of that file (Note that this file is actually in binary, even though it mostly appears as plain text):

```
 P'The Name of the Wind' by Patrick Rothfuss is 662 pages, published Mar 27, 2007. N'The Wise Man's Fear' by Patrick Rothfuss is 994 pages, published Mar 1, 2011. I'Doors of Stone' by Patrick Rothfuss is 896 pages, published Mar 5, 2049.
```

Cool.  Now, to retrieve the data that was saved to `books.txt` we start with the `ReadBooksFromFileImproperly()` method:

```java
 private static void ReadBooksFromFileImproperly() {
    try {
        DataInputStream inputStream = new DataInputStream(new BufferedInputStream(new FileInputStream(FILE)));

        Logging.lineSeparator(String.format("READING FROM FILE: %s", FILE));
        while (true) {
            String description = inputStream.readUTF();
            Logging.log(description);
        }
    } catch (EOFException exception) {
        // Output expected EOFExceptions.
        Logging.log(exception);
    } catch (IOException exception) {
        // Output unexpected IOExceptions.
        Logging.log(exception, false);
    }
}
```

Executing this method produces the following output:

```
----- READING FROM FILE: books.txt -----
'The Name of the Wind' by Patrick Rothfuss is 662 pages, published Mar 27, 2007.
'The Wise Man's Fear' by Patrick Rothfuss is 994 pages, published Mar 1, 2011.
'Doors of Stone' by Patrick Rothfuss is 896 pages, published Mar 5, 2049.
[EXPECTED] java.io.EOFException
```

Everything seems to be working propertly at first but, as you can see, once we reach the end of the file an `EOFException` is thrown, which we've caught and output to the log.  However, this method isn't configured very well, since any _other_ unexpected exception (such as an `IOException`) might take precedent over the _expected_ `EOFException`, which we'll get every time.

Therefore, the recommended way to handle reaching the end of a file in this sort of scenario is to enclose the stream-reading statements in their **very own** `try-catch` block, to explicitly handle `EOFExceptions`.  Once the expected `EOFException` is caught, this can be used as `control flow` statement to redirect execution flow to the next proper statement in the code.  While this sort of practice is _usually_ frowned on in most languages, in this particular case it is _the only_ way to handle `EOFExceptions`.  To illustrate one such example let's look at the modified `ReadBooksFromFile()` method:

```java
private static void ReadBooksFromFile() {
    DataInputStream inputStream = null;
    try {
        inputStream = new DataInputStream(new BufferedInputStream(new FileInputStream(FILE)));

        Logging.lineSeparator(String.format("READING FROM FILE: %s", FILE));

        while (true) {
            // Use inner exception block to determine end of file.
            try {
                String description = inputStream.readUTF();
                Logging.log(description);
            } catch (EOFException exception) {
                // Break while loop when file ends.
                break;
            } catch (IOException exception) {
                // Output unexpected IOExceptions.
                Logging.log(exception, false);
            }
        }
    } catch (IOException exception) {
        // Output unexpected IOExceptions.
        Logging.log(exception, false);
    } finally {
        try {
            if (inputStream != null) {
                inputStream.close();
            }
        } catch (IOException exception) {
            // Output unexpected IOExceptions.
            Logging.log(exception, false);
        }
    }
}
```

As you can see, this method contains quite a bit more code than the improper version, but it explicitly surrounds the statements that are reading from the stream with `try-catch` block to handle `EOFExceptions`.  When an `EOFException` is caught, the `break;` statement breaks the infinite `while (true)` loop and continues executing the rest of the method, like normal.  Additionally, we can then perform all our normal exception handling with the outer `try-catch` block covering the entirety of the method code.

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A deep dive into the Java EOFException, with code samples illustrating how to write to a file, and how to propertly (and improperly) read from it.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html