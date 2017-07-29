# Java Exception Handling - java.lang.ArrayIndexOutOfBoundsException

Today we'll take another journey through the "Land of Errors" of our ongoing [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series, with a deep dive into the java.lang.ArrayIndexOutOfBoundsException.  As the name clearly indicates, the ArrayIndexOutOfBoundsException is thrown when an index is passed to an array which doesn't contain an element at that particular index location.

In this article we'll look a bit closer at the `java.lang.ArrayIndexOutOfBoundsException` by examining where it sits in the [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).  We'll also go over a few simple, functional code samples illustrating how `ArrayIndexOutOfBoundsExceptions` are commonly thrown, so let's get crackin'!

## The Technical Rundown

- All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.
- [`java.lang.Exception`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html) inherits from [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).
- [`java.lang.RuntimeException`](https://docs.oracle.com/javase/8/docs/api/java/lang/RuntimeException.html) inherits from [`java.lang.Exception`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html).
- [`java.lang.IndexOutOfBoundsException`](https://docs.oracle.com/javase/8/docs/api/java/lang/IndexOutOfBoundsException.html) inherits from [`java.lang.RuntimeException`](https://docs.oracle.com/javase/8/docs/api/java/lang/RuntimeException.html).
- Lastly, `java.lang.ArrayIndexOutOfBoundsException` inherits from [`java.lang.IndexOutOfBoundsException`](https://docs.oracle.com/javase/8/docs/api/java/lang/IndexOutOfBoundsException.html).

## When Should You Use It?

Since Java has internal classes and object structures to manage `Arrays` -- and because said objects will produce errors like the `java.lang.ArrayIndexOutOfBoundsException` on their own -- there will rarely be a situation where you'll need to explicitly throw your own ArrayIndexOutOfBoundsException.  For example, if you were creating your own data structure object that contained a non-array collection of elements, you'd likely want to explicitly throw a [`java.lang.IndexOutOfBoundsException`](https://docs.oracle.com/javase/8/docs/api/java/lang/IndexOutOfBoundsException.html), as opposed to a `java.lang.ArrayIndexOutOfBoundsException`, since the JVM will handle that for you most of the time.

That said, to see how `ArrayIndexOutOfBoundsExceptions` are commonly thrown we'll start with the full working code sample, after which we'll explore it in more detail:

```java
package io.airbrake;

import io.airbrake.utility.*;

public class Main {

    public static void main(String[] args) {
        // Create list of Books.
        Book[] library = {
            new Book("The Pillars of the Earth", "Ken Follett", 973),
            new Book("Gone Girl", "Gillian Flynn", 555),
            new Book("His Dark Materials", "Philip Pullman", 399),
            new Book("Life of Pi", "Yann Martel", 460)
        };
        // Iterate over array.
        iterateArray(library);
        Logging.lineSeparator();
        iterateArrayInvalid(library);
    }

    /**
     * Iterate over passed Array, logging each element.
     *
     * @param list
     */
    private static void iterateArray(Book[] list) {
        try {
            // Loop through each element by index.
            for (int index = 0; index < list.length; index++) {
                // Output element.
                Logging.log(list[index]);
            }
        } catch (ArrayIndexOutOfBoundsException exception) {
            // Output expected ArrayIndexOutOfBoundsException.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }

    /**
     * Iterate over passed Array, logging each element.
     *
     * For-loop limit includes list.length, which is invalid.
     *
     * @param list
     */
    private static void iterateArrayInvalid(Book[] list) {
        try {
            // Loop through each element by index.
            // Less-than or equal to (<=) limit results in an exception.
            for (int index = 0; index <= list.length; index++) {
                // Output element.
                Logging.log(list[index]);
            }
        } catch (ArrayIndexOutOfBoundsException exception) {
            // Output expected ArrayIndexOutOfBoundsException.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }
}

// Book.java
package io.airbrake;

/**
 * Simple example class to store book instances.
 */
public class Book
{
    private String author;
    private String title;
    private Integer pageCount;

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
import org.apache.commons.lang3.builder.*;

/**
 * Houses all logging methods for various debug outputs.
 */
public class Logging {

    /**
     * Outputs any kind of Object.
     * Uses ReflectionToStringBuilder from Apache commons-lang library.
     *
     * @param value Object to be output.
     */
    public static void log(Object value)
    {
        if (value instanceof String)
        {
            System.out.println(value);
        }
        else
        {
            System.out.println(new ReflectionToStringBuilder(value, ToStringStyle.MULTI_LINE_STYLE).toString());
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
     * Output a dashed line separator of default (40) length.
     */
    public static void lineSeparator()
    {
        // Invoke default length method.
        lineSeparator(40);
    }

    /**
     * Output a dashed lin separator of desired length.
     *
     * @param length Length of line to be output.
     */
    public static void lineSeparator(int length)
    {
        // Create new character array of proper length.
        char[] characters = new char[length];
        // Fill each array element with character.
        Arrays.fill(characters, '-');
        // Output line of characters.
        System.out.println(new String(characters));
    }
}
```

---

To illustrate a common problem when using arrays we have two similar methods, `iterateArray(Book[] list)` and `iterateArrayInvalid(Book[] list)`:

```java
/**
* Iterate over passed Array, logging each element.
*
* @param list
*/
private static void iterateArray(Book[] list) {
    try {
        // Loop through each element by index.
        for (int index = 0; index < list.length; index++) {
            // Output element.
            Logging.log(list[index]);
        }
    } catch (ArrayIndexOutOfBoundsException exception) {
        // Output expected ArrayIndexOutOfBoundsException.
        Logging.log(exception);
    } catch (Exception exception) {
        // Output unexpected Exceptions.
        Logging.log(exception, false);
    }
}

/**
* Iterate over passed Array, logging each element.
*
* For-loop limit includes list.length, which is invalid.
*
* @param list
*/
private static void iterateArrayInvalid(Book[] list) {
    try {
        // Loop through each element by index.
        // Less-than or equal to (<=) limit results in an exception.
        for (int index = 0; index <= list.length; index++) {
            // Output element.
            Logging.log(list[index]);
        }
    } catch (ArrayIndexOutOfBoundsException exception) {
        // Output expected ArrayIndexOutOfBoundsException.
        Logging.log(exception);
    } catch (Exception exception) {
        // Output unexpected Exceptions.
        Logging.log(exception, false);
    }
}
```

These methods don't do anything fancy and, in fact, basically only serve as wrappers to stick our `try-catch` blocks in, and to differentiate the slight differences in loop logic between the two.  Specifically, as indicated by the code comments, the `iterateArrayInvalid(Book[] list)` method contains a `termination` expression that allows `index` to be less than or _equal to_ the length of `list`.  Since, like most languages, Java uses `zero-based numbering` to index `Arrays` and other collections, an index equal to the length of an array will be **one greater** than the largest index.  To better illustrate, consider this table of `Arrays` showing each array's `length` and its `maximum index`:

Length/Count | Maximum Index
--- | ---
1 | 0
2 | 1
3 | 2
4 | 3
5 | 4
etc. | etc.

With that in mind, we'll get started testing both of the iteration methods.  We'll use our `Book` class just to keep things a little more interesting by creating a few elements for our array, then pass it to both methods so we can review the output of each:

```java
public static void main(String[] args) {
    // Create list of Books.
    Book[] library = {
        new Book("The Pillars of the Earth", "Ken Follett", 973),
        new Book("Gone Girl", "Gillian Flynn", 555),
        new Book("His Dark Materials", "Philip Pullman", 399),
        new Book("Life of Pi", "Yann Martel", 460)
    };
    // Iterate over array.
    iterateArray(library);
    Logging.lineSeparator();
    iterateArrayInvalid(library);
}
```

The output from `iterateArray(Book[] list)` is just as expected, outputting all four elements before execution is completed:

```
io.airbrake.Book@532760d8[
  author=Ken Follett
  title=The Pillars of the Earth
  pageCount=973
]
io.airbrake.Book@5679c6c6[
  author=Gillian Flynn
  title=Gone Girl
  pageCount=555
]
io.airbrake.Book@27ddd392[
  author=Philip Pullman
  title=His Dark Materials
  pageCount=399
]
io.airbrake.Book@19e1023e[
  author=Yann Martel
  title=Life of Pi
  pageCount=460
]
```

On the other hand, invoking `iterateArrayInvalid(Book[] list)` runs into a problem.  While we successfully output all four elements, as mentioned above, the `for` loop iterates one too many times, resulting in a call to `list[4]`, which is an unknown index.  This throws a `java.lang.ArrayIndexOutOfBoundsException`, indicating the `index` value that was out of bounds:

```
io.airbrake.Book@532760d8[
  author=Ken Follett
  title=The Pillars of the Earth
  pageCount=973
]
io.airbrake.Book@5679c6c6[
  author=Gillian Flynn
  title=Gone Girl
  pageCount=555
]
io.airbrake.Book@27ddd392[
  author=Philip Pullman
  title=His Dark Materials
  pageCount=399
]
io.airbrake.Book@19e1023e[
  author=Yann Martel
  title=Life of Pi
  pageCount=460
]

[EXPECTED] java.lang.ArrayIndexOutOfBoundsException: 4
```

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A deeper look at the java.lang.ArrayIndexOutOfBoundsException in Java, including code samples showing both correct and incorrect array for-looping.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html