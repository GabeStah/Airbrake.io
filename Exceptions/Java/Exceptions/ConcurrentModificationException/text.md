# Java Exception Handling - ConcurrentModificationException

Today we'll bite off another morsel from our delicious and mildly satiating [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series with a close look at the java.util.ConcurrentModificationException.  The `java.util.ConcurrentModificationException` is typically thrown when code attempts to modify a data collection while that collection is actively in use, such as being iterated.  

We'll take the time in this article to further explore the `java.util.ConcurrentModificationException`, looking at where it sits in the [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy), along with some simple, functional code samples that will illustrate common scenarios that `java.util.ConcurrentModificationExceptions` might occur, and how to prevent them.  Let's get to it!

## The Technical Rundown

- All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.
- [`java.lang.Exception`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html) inherits from [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).
- [`java.lang.RuntimeException`](https://docs.oracle.com/javase/8/docs/api/java/lang/RuntimeException.html) inherits from [`java.lang.Exception`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html).
- Finally, `java.util.ConcurrentModificationException` inherits from [`java.lang.RuntimeException`](https://docs.oracle.com/javase/8/docs/api/java/lang/RuntimeException.html)/

## When Should You Use It?

As discussed in the introduction, the most typical scenario that might lead to a `java.util.ConcurrentModificationException` is when performing modifications on a collection object that is currently in use.  To illustrate, in our example code we'll be creating a basic `ArrayList<>` object, adding a few items to it, then iterating through the elements.  Depending on _how_ we build our application and, therefore, how we choose to perform any modifications during the iteration, we'll see that the application will behave very differently.

To begin, let's start with the full working code sample below.  At the end of the sample block we'll go through each section in more detail to see what's going on:

```java
package io.airbrake;

import java.util.*;
import io.airbrake.utility.*;

public class ConcurrentModificationException {

    public static void main(String args[]){
        modifiedListExample();
        modifiedIteratorExample();
    }

    /**
     * Perform looped iteration of List while it is being modified.
     */
    private static void modifiedListExample()
    {
        try {
            // Create list of Books.
            List<Book> library = new ArrayList<>();
            // Add a few new Books to list.
            library.add(new Book("The Pillars of the Earth", "Ken Follett", 973));
            library.add(new Book("A Game of Thrones", "George R.R. Martin", 835));
            library.add(new Book("Gone Girl", "Gillian Flynn", 555));
            library.add(new Book("His Dark Materials", "Philip Pullman", 399));
            library.add(new Book("Life of Pi", "Yann Martel", 460));

            // Loop through each book object in list.
            for (Book book : library) {
                // Output next book.
                Logging.log(book);
                // If current book title is "Gone Girl", remove that book from list.
                if (book.getTitle().equals("Gone Girl")) {
                    library.remove(book);
                }
            }
        } catch (java.util.ConcurrentModificationException exception) {
            // Catch ConcurrentModificationExceptions.
            Logging.log(exception);
        } catch (Throwable throwable) {
            // Catch any other Throwables.
            Logging.log(throwable);
        }
    }

    /**
     * Perform loop through Iterator while it is being modified.
     */
    private static void modifiedIteratorExample()
    {
        try {
            // Create list of Books.
            List<Book> library = new ArrayList<>();
            // Add a few new Books to list.
            library.add(new Book("The Pillars of the Earth", "Ken Follett", 973));
            library.add(new Book("A Game of Thrones", "George R.R. Martin", 835));
            library.add(new Book("Gone Girl", "Gillian Flynn", 555));
            library.add(new Book("His Dark Materials", "Philip Pullman", 399));
            library.add(new Book("Life of Pi", "Yann Martel", 460));


            // Create iterator to loop through each book in list.
            for(Iterator<Book> bookIterator = library.iterator(); bookIterator.hasNext();){
                // Get next book.
                Book book = bookIterator.next();
                // If current book title is "Gone Girl", remove that book from list.
                if (book.getTitle().equals("Gone Girl")) {
                    bookIterator.remove();
                } else {
                    // Output current book.
                    Logging.log(book);
                }
            }

            //library.removeIf(book -> book.getTitle().equals("Gone Girl"));
        } catch (java.util.ConcurrentModificationException exception) {
            // Catch ConcurrentModificationExceptions.
            Logging.log(exception);
        } catch (Throwable throwable) {
            // Catch any other Throwables.
            Logging.log(throwable);
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
     * Set author of book.
     *
     * @param author Author name.
     */
    public void setAuthor(String author) {
        this.author = author;
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
     * Set page count of book.
     *
     * @param pageCount Page count.
     */
    public void setPageCount(Integer pageCount) {
        this.pageCount = pageCount;
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
     * Set title of book.
     *
     * @param title Title.
     */
    public void setTitle(String title) {
        this.title = title;
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
        System.out.println(new ReflectionToStringBuilder(value, ToStringStyle.MULTI_LINE_STYLE).toString());
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

To make our example a bit more realistic we've added a simple `Book` class with a few fields to store basic book information:

```java
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
     * Set author of book.
     *
     * @param author Author name.
     */
    public void setAuthor(String author) {
        this.author = author;
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
     * Set page count of book.
     *
     * @param pageCount Page count.
     */
    public void setPageCount(Integer pageCount) {
        this.pageCount = pageCount;
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
     * Set title of book.
     *
     * @param title Title.
     */
    public void setTitle(String title) {
        this.title = title;
    }
}
```

Our goal is to build a `List` collection of `Books` that we can iterate through, while simultaneously checking for a particular element in the list and proactively _removing_ any matching book element from the list while the iteration continues.  To accomplish this we start with the `modifiedListExample()` method:

```java
private static void modifiedListExample()
{
    try {
        // Create list of Books.
        List<Book> library = new ArrayList<>();
        // Add a few new Books to list.
        library.add(new Book("The Pillars of the Earth", "Ken Follett", 973));
        library.add(new Book("A Game of Thrones", "George R.R. Martin", 835));
        library.add(new Book("Gone Girl", "Gillian Flynn", 555));
        library.add(new Book("His Dark Materials", "Philip Pullman", 399));
        library.add(new Book("Life of Pi", "Yann Martel", 460));

        // Loop through each book object in list.
        for (Book book : library) {
            // Output next book.
            Logging.log(book);
            // If current book title is "Gone Girl", remove that book from list.
            if (book.getTitle().equals("Gone Girl")) {
                library.remove(book);
            }
        }
    } catch (java.util.ConcurrentModificationException exception) {
        // Catch ConcurrentModificationExceptions.
        Logging.log(exception);
    } catch (Throwable throwable) {
        // Catch any other Throwables.
        Logging.log(throwable);
    }
}
```

As you can see, this method creates a new `ArrayList<Book>` object called `library`, adds a handful of `Books` to the list, then performs an advanced `for` loop on the `library` list.  For each `book` element in the list we output the `book` info to the log, then also check if the current `book` element has a `title` of `"Gone Girl"`, in which case we remove that element from the `library` list.  Let's execute this method and see what we get in the log output:

```
io.airbrake.Book@4ac68d3e[
  author=Ken Follett
  title=The Pillars of the Earth
  pageCount=973
]
io.airbrake.Book@4f47d241[
  author=George R.R. Martin
  title=A Game of Thrones
  pageCount=835
]
io.airbrake.Book@4c3e4790[
  author=Gillian Flynn
  title=Gone Girl
  pageCount=555
]
[EXPECTED] java.util.ConcurrentModificationException
java.util.ConcurrentModificationException
	at java.util.ArrayList$Itr.checkForComodification(ArrayList.java:901)
	at java.util.ArrayList$Itr.next(ArrayList.java:851)
	at io.airbrake.ConcurrentModificationException.modifiedListExample(ConcurrentModificationException.java:29)
	at io.airbrake.ConcurrentModificationException.main(ConcurrentModificationException.java:9)
```

Everything was working fine at first, but after removing `"Gone Girl"` from the `library` list we ran into a problem and threw a `java.util.ConcurrentModificationException`.  The reason for this is that the JVM couldn't determine what element should've come next in our `for` loop, since, for example, the original _fourth_ element (`"His Dark Materials"`) is no longer where it should've been.

To fix this issue we can slightly modify how we perform our iteration of `library` elements and, instead of using an advanced `for` loop syntax as above, we can create a new `Iterator<Book>` object, which will serve as our means of iterating through elements and, most importantly, allow us to remove any matched `book` elements without affecting the iteration process.

Thus let's take a look at the `modifiedIteratorExample()` method:

```java
private static void modifiedIteratorExample()
{
    try {
        // Create list of Books.
        List<Book> library = new ArrayList<>();
        // Add a few new Books to list.
        library.add(new Book("The Pillars of the Earth", "Ken Follett", 973));
        library.add(new Book("A Game of Thrones", "George R.R. Martin", 835));
        library.add(new Book("Gone Girl", "Gillian Flynn", 555));
        library.add(new Book("His Dark Materials", "Philip Pullman", 399));
        library.add(new Book("Life of Pi", "Yann Martel", 460));

        // Create iterator to loop through each book in list.
        for(Iterator<Book> bookIterator = library.iterator(); bookIterator.hasNext();){
            // Get next book.
            Book book = bookIterator.next();
            // If current book title is "Gone Girl", remove that book from list.
            if (book.getTitle().equals("Gone Girl")) {
                bookIterator.remove();
            } else {
                // Output current book.
                Logging.log(book);
            }
        }
    } catch (java.util.ConcurrentModificationException exception) {
        // Catch ConcurrentModificationExceptions.
        Logging.log(exception);
    } catch (Throwable throwable) {
        // Catch any other Throwables.
        Logging.log(throwable);
    }
}
```

The big difference is the `for` loop using a new `Iterator<Book>` object, grabbed from the `library.iterator()` method.  We then call the `bookIterator.hasNext()` method to check if another element exists, then retrieve it inside the loop with `bookIterator.next()`.  Lastly, and most importantly, we are not calling `library.remove()` directly on the parent list object.  Instead we use the `bookIterator.remove()` method, which allows us to safely remove the current element from the iterator, so the upcoming `bookIterator.hasNext()` and `bookIterator.next()` don't fail and throw another `java.util.ConcurrentModificationException`.

The result is that our log output shows all books _except_ the one we removed and skipped over:

```
io.airbrake.Book@198e2867[
  author=Ken Follett
  title=The Pillars of the Earth
  pageCount=973
]
io.airbrake.Book@12f40c25[
  author=George R.R. Martin
  title=A Game of Thrones
  pageCount=835
]
io.airbrake.Book@3ada9e37[
  author=Philip Pullman
  title=His Dark Materials
  pageCount=399
]
io.airbrake.Book@5cbc508c[
  author=Yann Martel
  title=Life of Pi
  pageCount=460
]
```

It's also worth noting that the above sample code is a bit verbose, just to better illustrate what's going on with our iteration.  We could almost entirely remove the `for` code block and use the following snippet instead, which uses a lambda expression to check each `book` element and perform a `remove` function _if_ the `title` is what we're filtering:

```java
library.removeIf(book -> book.getTitle().equals("Gone Girl"));
```

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look into the java.util.ConcurrentModificationException in Java, including code samples illustrating the a few different iteration methods.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/util/ConcurrentModificationException.html