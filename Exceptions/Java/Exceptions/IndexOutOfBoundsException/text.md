# Java Exception Handling - IndexOutOfBoundsException

Moving along through the detailed [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series we've been working on, today we'll be going over the **IndexOutOfBoundsException**.  The `IndexOutOfBoundsException` is thrown when attempting to access an invalid index within a collection, such as an `array`, `vector`, `string`, and so forth.  It can also be implemented within custom classes to indicate invalid access was attempted for a collection.

In this article we'll explore the `IndexOutOfBoundsException` by starting with where it resides in the overall [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).  We'll also examine some fully functional Java code samples that will illustrate both the default API invocation of `IndexOutOfBoundsExceptions`, along with a custom class example of how you might throw them in your own code.  Let's get crackin'!

## The Technical Rundown

All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`java.lang.Object`](https://docs.oracle.com/javase/8/docs/api/java/lang/Object.html)
    - [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy)
        - [`java.lang.Exception`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html)
            - [`java.lang.RuntimeException`](https://docs.oracle.com/javase/8/docs/api/java/lang/RuntimeException.html)
                - `IndexOutOfBoundsException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```java
package io.airbrake;

import io.airbrake.utility.Logging;

import java.util.GregorianCalendar;

public class Main {

    public static void main(String[] args) {
        Logging.lineSeparator("CREATE RANDOM NUMBER ARRAY");
        int[] array = createArrayOfSize(10);
        // Output array.
        Logging.log(array);

        Logging.lineSeparator("GET ELEMENT AT INDEX 5");
        Logging.log(getElementByIndex(array, 5));

        Logging.lineSeparator("GET ELEMENT AT INDEX 10");
        Logging.log(getElementByIndex(array, 10));

        Logging.lineSeparator("CREATE BOOK");
        Book book = new Book(
                "A Game of Thrones",
                "George R.R. Martin",
                new GregorianCalendar(1996, 8, 6).getTime(),
                "novel"
        );
        Logging.log(book);

        Logging.lineSeparator("INSERT PAGES");
        // Create Pages array.
        Page[] pages = {
                new Page("“We should start back,” Gared urged as the woods began to grow dark around them. " +
                        "“The wildlings are dead.”"),
                new Page("Until tonight. Something was different tonight. There was an edge to this darkness " +
                        "that made his hackles rise."),
                new Page("“Well, no,” Will admitted")
        };
        book.setPages(pages);
        Logging.log(book);

        Logging.lineSeparator("SET PAGE AT INVALID INDEX");
        setPageAtIndex(book, new Page("Royce nodded."), 3);
    }

    private static void setPageAtIndex(Book book, Page page, int index) {
        try {
            // Set page at index.
            book.setPage(page, index);
            // Output updated book.
            Logging.log(book);
        } catch (IndexOutOfBoundsException error) {
            // Output expected IndexOutOfBoundsExceptions.
            Logging.log(error);
        } catch (Exception | Error exception) {
            // Output unexpected Exceptions/Errors.
            Logging.log(exception, false);
        }
    }

    private static int[] createArrayOfSize(int size) {
        int[] data = new int[size];
        for (int i = 0; i < data.length; i++) {
            data[i] = (int)(Math.random() * 100);
        }
        return data;
    }

    private static Integer getElementByIndex(int[] array, int index) {
        try {
            return array[index];
        } catch (IndexOutOfBoundsException error) {
            // Output expected IndexOutOfBoundsExceptions.
            Logging.log(error);
        } catch (Exception | Error exception) {
            // Output unexpected Exceptions/Errors.
            Logging.log(exception, false);
        }
        return null;
    }
}
```

```java
package io.airbrake;

public class Page
{
    private String content;

    public Page() { }

    public Page(String content) {
        setContent(content);
    }

    public String getContent() {
        return content;
    }

    public void setContent(String content) {
        this.content = content;
    }

    /**
     * Gets a string representation of Page.
     *
     * @return String Formatted string of Page.
     */
    public String toString() {
        return getContent();
    }
}
```

```java
// Book.java
package io.airbrake;

import java.util.Date;

/**
 * Simple example class to store book instances.
 */
public class Book
{
    private String author;
    private String title;
    private Page[] pages;
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
     */
    public Book(String title, String author, Page[] pages) {
        setAuthor(author);
        setTitle(title);
    }

    /**
     * Constructs a basic book, with page count.
     *
     * @param title Book title.
     * @param author Book author.
     */
    public Book(String title, String author, Date publishedAt) {
        setAuthor(author);
        setTitle(title);
        setPublishedAt(publishedAt);
    }

    /**
     * Constructs a basic book, with page count.
     *
     * @param title Book title.
     * @param author Book author.
     */
    public Book(String title, String author, Date publishedAt, String publicationType) {
        setAuthor(author);
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
     * Get current page count.
     *
     * @return Page count.
     */
    public int getPageCount() {
        return pages.length;
    }

    /**
     * Get pages of book.
     *
     * @return Pages.
     */
    public Page[] getPages() {
        return pages;
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
        return String.format("'%s' by %s is %d pages.", this.title, this.author, this.getPageCount());
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
     * Set page at specified index of book.
     *
     * @param page Page to be added.
     * @param index Index of specified page.
     */
    public void setPage(Page page, int index) {
        if (index < 0 || index >= this.pages.length) {
            throw new IndexOutOfBoundsException(String.format("Unable to add page with content \"%s\" at index %s.", page, index));
        }
        this.pages[index] = page;
    }

    /**
     * Set pages of book.
     *
     * @param pages Pages.
     */
    public void setPages(Page[] pages) {
        this.pages = pages;
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
     * Gets a string representation of Book.
     *
     * @return String string of Book.
     */
    public String toString() {
        return String.format("'%s' by %s is %s pgs and published on %s.",
                getTitle(),
                getAuthor(),
                getPageCount(),
                getPublishedAt());
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

Coming across an `IndexOutOfBoundsException` in standard API code simply means an invalid index was accessed for a collection.  To illustrate we have two methods, `createArrayOfSize(int size)` and `getElementByIndex(int[] array, int index)`:

```java
private static int[] createArrayOfSize(int size) {
    int[] data = new int[size];
    for (int i = 0; i < data.length; i++) {
        data[i] = (int)(Math.random() * 100);
    }
    return data;
}

private static Integer getElementByIndex(int[] array, int index) {
    try {
        return array[index];
    } catch (IndexOutOfBoundsException error) {
        // Output expected IndexOutOfBoundsExceptions.
        Logging.log(error);
    } catch (Exception | Error exception) {
        // Output unexpected Exceptions/Errors.
        Logging.log(exception, false);
    }
    return null;
}
```

As you can see, `createArrayOfSize(int size)` just creates and returns a new `int[]` array of the specified size, populating elements with random integers.  We then use `getElementByIndex(int[] array, int index)` to attempt to retrieve specific elements of the passed `int[] array` parameter that correspond with the specified `int index` parameter.

To test this out we'll start by creating a 10-length array, output it to the log, then attempt to retrieve a specific element at index `5`:

```java
public class Main {

    public static void main(String[] args) {
        Logging.lineSeparator("CREATE RANDOM NUMBER ARRAY");
        int[] array = createArrayOfSize(10);
        // Output array.
        Logging.log(array);

        Logging.lineSeparator("GET ELEMENT AT INDEX 5");
        Logging.log(getElementByIndex(array, 5));

        // ...

    }
}
```

Here's the output we get after executing the above code:

```
------ CREATE RANDOM NUMBER ARRAY ------
[I@3ada9e37[
  {0,38,21,70,77,85,26,7,89,59}
]

-------- GET ELEMENT AT INDEX 5 --------
85
```

Everything seems to be working as expected.  Our initial array contains `10` random integers and the element at index `5` is `85`, as confirmed by the call to `getElementByIndex(int[] array, int index)`.

Now, let's try getting an element at an index outside the bounds of our array (`10`, in this case):

```java
Logging.lineSeparator("GET ELEMENT AT INDEX 10");
Logging.log(getElementByIndex(array, 10));
```

The output shows we've thrown a specific `ArrayIndexOutOfBoundsException`, which is an exception class that inherits from `IndexOutOfBoundsException`:

```
------- GET ELEMENT AT INDEX 10 --------
[EXPECTED] java.lang.ArrayIndexOutOfBoundsException: 10
```

That's all well and good, but what happens if we want a more descriptive exception message?  We can implement `IndexOutOfBoundsExceptions` directly into our own custom classes, to be thrown when attempts to access an invalid index would cause problems.  For example, we've created a simple `Page` class that contains a single `content` field to store the content of the `Page`:

```java
package io.airbrake;

public class Page
{
    private String content;

    public Page() { }

    public Page(String content) {
        setContent(content);
    }

    public String getContent() {
        return content;
    }

    public void setContent(String content) {
        this.content = content;
    }

    /**
     * Gets a string representation of Page.
     *
     * @return String Formatted string of Page.
     */
    public String toString() {
        return getContent();
    }
}
```

We then have a modified `Book` class that contains a `Page[] pages` field array, allowing us to specify a series of `Page` elements associated with the `Book`:

```java
// Book.java
package io.airbrake;

import java.util.Date;

/**
 * Simple example class to store book instances.
 */
public class Book
{
    private String author;
    private String title;
    private Page[] pages;
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
     */
    public Book(String title, String author, Page[] pages) {
        setAuthor(author);
        setTitle(title);
    }

    /**
     * Constructs a basic book, with page count.
     *
     * @param title Book title.
     * @param author Book author.
     */
    public Book(String title, String author, Date publishedAt) {
        setAuthor(author);
        setTitle(title);
        setPublishedAt(publishedAt);
    }

    /**
     * Constructs a basic book, with page count.
     *
     * @param title Book title.
     * @param author Book author.
     */
    public Book(String title, String author, Date publishedAt, String publicationType) {
        setAuthor(author);
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
     * Get current page count.
     *
     * @return Page count.
     */
    public int getPageCount() {
        return pages.length;
    }

    /**
     * Get pages of book.
     *
     * @return Pages.
     */
    public Page[] getPages() {
        return pages;
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
        return String.format("'%s' by %s is %d pages.", this.title, this.author, this.getPageCount());
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
     * Set page at specified index of book.
     *
     * @param page Page to be added.
     * @param index Index of specified page.
     */
    public void setPage(Page page, int index) {
        if (index < 0 || index >= this.pages.length) {
            throw new IndexOutOfBoundsException(String.format("Unable to add page with content \"%s\" at index %s.", page, index));
        }
        this.pages[index] = page;
    }

    /**
     * Set pages of book.
     *
     * @param pages Pages.
     */
    public void setPages(Page[] pages) {
        this.pages = pages;
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
     * Gets a string representation of Book.
     *
     * @return String string of Book.
     */
    public String toString() {
        return String.format("'%s' by %s is %s pgs and published on %s.",
                getTitle(),
                getAuthor(),
                getPageCount(),
                getPublishedAt());
    }

    /**
     * Throw an Exception.
     */
    public void throwException(String message) throws Exception {
        throw new Exception(message);
    }
}
```

We have methods to both get and set `Pages` within the `Book` instance, but the most important method is `setPage(Page page, int index)`, which allows us to explicitly change the `Page` element at the specified `int index`.  However, if the passed `index` is invalid or exceeds the current bounds of our `pages` field length, we explicitly throw a new `IndexOutOfBoundsException` indicating the problem:

```java
/**
* Set page at specified index of book.
*
* @param page Page to be added.
* @param index Index of specified page.
*/
public void setPage(Page page, int index) {
    if (index < 0 || index >= this.pages.length) {
        throw new IndexOutOfBoundsException(String.format("Unable to add page with content \"%s\" at index %s.", page, index));
    }
    this.pages[index] = page;
}
```

To test this out we start by instantiating a new `Book` and adding some content to the first few three pages:

```java
Logging.lineSeparator("CREATE BOOK");
Book book = new Book(
        "A Game of Thrones",
        "George R.R. Martin",
        new GregorianCalendar(1996, 8, 6).getTime(),
        "novel"
);
Logging.log(book);

Logging.lineSeparator("INSERT PAGES");
// Create Pages array.
Page[] pages = {
    new Page("“We should start back,” Gared urged as the woods began to grow dark around them. " +
            "“The wildlings are dead.”"),
    new Page("Until tonight. Something was different tonight. There was an edge to this darkness " +
            "that made his hackles rise."),
    new Page("“Well, no,” Will admitted")
};
book.setPages(pages);
Logging.log(book);
```

This produces the initial `Book` output without any `Pages` content, then once we add them via the `setPages(Page[] pages)` method, the output shows our `Pages` have been added successfully:

```
------------- CREATE BOOK --------------
io.airbrake.Book@21a06946[
  author=George R.R. Martin
  title=A Game of Thrones
  pages=<null>
  publishedAt=Fri Sep 06 00:00:00 PDT 1996
]

------------- INSERT PAGES -------------
io.airbrake.Book@21a06946[
  author=George R.R. Martin
  title=A Game of Thrones
  pages={“We should start back,” Gared urged as the woods began to grow dark around them. “The wildlings are dead.”,Until tonight. Something was different tonight. There was an edge to this darkness that made his hackles rise.,“Well, no,” Will admitted}
  publishedAt=Fri Sep 06 00:00:00 PDT 1996
]
```

Cool!  Now let's try this `setPageAtIndex(Book, book, Page page, int index)` helper method, which is just a wrapper for the `Book.setPage(Page page, int index)` method:

```java
private static void setPageAtIndex(Book book, Page page, int index) {
    try {
        // Set page at index.
        book.setPage(page, index);
        // Output updated book.
        Logging.log(book);
    } catch (IndexOutOfBoundsException error) {
        // Output expected IndexOutOfBoundsExceptions.
        Logging.log(error);
    } catch (Exception | Error exception) {
        // Output unexpected Exceptions/Errors.
        Logging.log(exception, false);
    }
}
```

Here we're passing our previous `book` instance, a new fourth `Page`, and an `index` of `3` (to indicate the fourth page):

```java
Logging.lineSeparator("SET PAGE AT INVALID INDEX");
setPageAtIndex(book, new Page("Royce nodded."), 3);
```

Since we explicitly check the validity of the passed `int index` parameter in `Book.setPage(Page page, int index)`, this attempt to set a `Page` for an invalid index throws our custom `IndexOutOfBoundsException`:

```
------ SET PAGE AT INVALID INDEX -------
[EXPECTED] java.lang.IndexOutOfBoundsException: Unable to add page with content "Royce nodded." at index 3.
```

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look at the Java IndexOutOfBoundsException, with code samples showing how to use this exception in both built-in APIs and custom classes.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html