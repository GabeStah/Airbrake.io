# Java Exception Handling - ExceptionInInitializerError

Moving along through our in-depth [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series, today we'll dive into the **ExceptionInInitializerError**, which is thrown when an error occurs within the `static initializer` of a class or object.  Since an `ExceptionInInitializerError` isn't ever the _cause_ of a thrown error, catching such an exception provides an underlying `causal exception` that indicates what the actual source of the issue was.

Throughout this article we'll examine the `ExceptionInInitializerError` by looking at where it resides in the overall [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).  We'll then explore a simple and fully-functional code sample that illustrates how a static initializer works, and what might lead to a thrown `ExceptionInInitializerError` in such an initializer, so let's get to it!

## The Technical Rundown

All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`java.lang.Object`](https://docs.oracle.com/javase/8/docs/api/java/lang/Object.html)
    - [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy)
        - [`java.lang.Error`](https://docs.oracle.com/javase/8/docs/api/java/lang/Error.html)
            - [`java.lang.LinkageError`](https://docs.oracle.com/javase/8/docs/api/java/lang/LinkageError.html)
                - `ExceptionInInitializerError`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```java
package io.airbrake;

import io.airbrake.utility.Logging;

import java.util.GregorianCalendar;

public class Main {

    public static void main(String[] args) {
        try {
            Book book = new Book(
                "A Game of Thrones",
                "George R.R. Martin",
                848,
                new GregorianCalendar(1996, 8, 6).getTime(),
                "novel"
            );
            Logging.log(book);
            Logging.log(String.format("Publication Type: %s", book.getPublicationType()));
        } catch (ExceptionInInitializerError error) {
            // Output expected ExceptionInInitializerErrors.
            Logging.log(error);
            // Output causal exception.
            Logging.lineSeparator(String.format("%s Cause", error.getClass().getSimpleName()), 50);
            Logging.log(error.getCause(), false);
        } catch (Exception | Error exception) {
            // Output unexpected Exceptions/Errors.
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
    private static String publicationType;

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

Since the appearance of an `ExceptionInInitializerError` indicates a problem within a [`static initializer`](https://docs.oracle.com/javase/tutorial/java/javaOO/initial.html), we should briefly examine what a static initializer is and how they're typically used.  Put simply, a static initializer block is a normal code block that is executed when the class is loaded.

A static initializer is created by enclosing code anywhere inside a class definition surrounded by braces (`{` & `}`) and also preceded by the `static` keyword.  For example:

```java
class MyClass {
    static {
        // This is the first static initializer of MyClass.
    }

    static {
        // This is the second static initializer of MyClass.
    }
}
```

As you can see in the code above, there can be as many static initializer blocks as desired within a class definition.  Each will be executed in a top-down manner when the class is first loaded.

Static initializers are not to be confused with `instance initializers` (or `instance members`), which are executed when the class is _instantiated_ (i.e. a new object of that class `type` is created).  Instance initializers are also executed just before the `constructor` method.

An instance initializer is written inside a class definition using two braces (`{` & `}`), but _without_ a preceding `static` keyword:

```java
class MyClass {
    static {
        // This is the first static initializer of MyClass.
    }

    static {
        // This is the second static initializer of MyClass.
    }

    {
        // This is the first instance initializer of MyClass.
    }

    {
        // This is the second instance initializer of MyClass.
    }
}
```

Just as with static versions, multiple instance initializers can be defined and will be executed from top to bottom.

The purpose of a static initializer is typically to execute any initialization code that must occur before a class can be used.  Critically, static initializers are only executed _once ever_, the first time that class is loaded.  Thus, they are great for classes that are frequently used throughout the application, but only need some basic configuration code to be executed once before multiple instances are used elsewhere, such as a database connection class.

To test a static initializer in "real" code we've added a simple snippet and field to our trusty `Book` class:

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
    private static String publicationType;

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
        publicationType = publicationType;
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

A static initializer block has been added near to the top of the class definition, and merely attempts to ensure the `publicationType` static field is an upper case value:

```java
private static String publicationType;

// ...

/**
* Ensure publication type is upper case.
*/
static {
    publicationType = publicationType.toUpperCase();
}
```

With this static initializer in place let's try creating a new Book instance:

```java
public class Main {
    public static void main(String[] args) {
        try {
            Book book = new Book(
                "A Game of Thrones",
                "George R.R. Martin",
                848,
                new GregorianCalendar(1996, 8, 6).getTime(),
                "novel"
            );
            Logging.log(book);
            Logging.log(String.format("Publication Type: %s", book.getPublicationType()));
        } catch (ExceptionInInitializerError error) {
            // Output expected ExceptionInInitializerErrors.
            Logging.log(error);
            // Output causal exception.
            Logging.lineSeparator(String.format("%s Cause", error.getClass().getSimpleName()), 50);
            Logging.log(error.getCause(), false);
        } catch (Exception | Error exception) {
            // Output unexpected Exceptions/Errors.
            Logging.log(exception, false);
        }
    }
}
```

Executing this `main(String[] args)` method produces the following output:

```
[EXPECTED] java.lang.ExceptionInInitializerError
------- ExceptionInInitializerError Cause --------
[UNEXPECTED] java.lang.NullPointerException
```

Sure enough, this threw an `ExceptionInInitializerError` at us.  Just as importantly, since `ExceptionInInitializerErrors` aren't ever going to _cause_ a problem themselves, catching such an exception always contains an actual `causal exception`, which is the error that was thrown within a static initializer that led to the caught `ExceptionInInitializerError`.  In this case, we see the cause was a `NullPointerException`.  With a bit of digging and consideration, we can see that the problem is within the static initializer code inside the `Book` class.  It attempts to get the static `publicationType` value, but we don't explicitly set an initial value for this field, so calling this references a `null` value, hence the `NullPointerException`.  The solution is to either set an initial value for `publicationType`, or to include a `try-catch` block within the static initializer.  In this case, a `try-catch` block handles the error, but doesn't resolve the root cause, so it's probably better to specify a default value instead:

```java
private static String publicationType = "Book";
```

Now, rerunning the same `main(String[] args)` method instantiates our `Book` just fine and outputs the results:

```
io.airbrake.Book@50675690[
  author=George R.R. Martin
  title=A Game of Thrones
  pageCount=848
  publishedAt=Fri Sep 06 00:00:00 PDT 1996
]
Publication Type: novel
```

A few things worth noting.  First, since our code to force the `publicationType` value to be upper case occurs within a static initializer, this occurs _before_ our `Book` instance is initialized, as well as before the `Book(String title, String author, Integer pageCount, Date publishedAt, String publicationType)` `constructor` method is executed.  Thus, even though we successfully change the `publicationType` from the default of `Book` to `novel`, it won't be upper case'd unless we do so inside an `instance` or `member initializer`.

Additionally, since `publicationType` is a `static` field, this means it is unique to the entire `Book` class.  If we were to create a second `Book` instance with a `publicationType` specified in the constructor of `poem`, the `publicationType` of our `Game of Thrones` `Book` instance would also have its `publicationType` changed to `poem`:

```java
Book book = new Book(
    "A Game of Thrones",
    "George R.R. Martin",
    848,
    new GregorianCalendar(1996, 8, 6).getTime(),
    "novel"
);
Logging.log(book);
Logging.log(String.format("Publication Type of Novel: %s", book.getPublicationType()));

Book poem = new Book(
        "The Road Not Taken",
        "Robert Frost",
        1,
        new GregorianCalendar(1916, 1, 1).getTime(),
        "poem"
);
Logging.log(poem);
Logging.log(String.format("Publication Type of Novel: %s", book.getPublicationType()));
Logging.log(String.format("Publication Type of Poem: %s", poem.getPublicationType()));
```

This code produces the following output:

```
io.airbrake.Book@50675690[
  author=George R.R. Martin
  title=A Game of Thrones
  pageCount=848
  publishedAt=Fri Sep 06 00:00:00 PDT 1996
]
Publication Type of Novel: novel

io.airbrake.Book@2cfb4a64[
  author=Robert Frost
  title=The Road Not Taken
  pageCount=1
  publishedAt=Tue Feb 01 00:00:00 PST 1916
]
Publication Type of Novel: poem
Publication Type of Poem: poem
```

As we can see, the `publicationType` of both instances is now the new value of `poem`, so just be ware that `static` members and initializers are global to all instances of a particular class.

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A deep dive into the Java ExceptionInInitializerError, with functional code samples illustrating how to create static and instance initializers.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html