# Kotlin: The Better Form of Java

If you haven't been keeping up with new developments in the world of, well, Java development, then you may have overlooked the ground-breaking release of the Kotlin language version 1.0 in early 2016.  Kotlin rethinks many of the syntaxes, shortcomings, and limitations of Java and pushes the boundaries for Java and Android application development.

What makes Kotlin so good?  For long-time (or even newcomer) Java developers, why should the leap be taken to learn and get into the Kotlin ecosystem?  That's exactly what we'll be exploring in this article; looking at some of the cool new features (and improvements over Java) that Kotlin brings to the table.  Let's have a look!

## Syntactic Simplicity

One of the best things about Kotlin is how _concise_ the syntax is.  Generally speaking, Java has always been a rather verbose.  It can take a lot of code to write simple, common structures in vanilla Java.  Kotlin attempts to remedy this by providing a much cleaner and streamlined syntax.

For example, here we have a `Plain Old Java Object` (`POJO`), written in Java, which represents a `Person`:

```java
public class Person {
    private Integer age;
    private String name;

    public Person(String name, Integer age) {
        this.age = age;
        this.name = name;
    }

    public Integer getAge() {
        return age;
    }

    public void setAge(Integer age) {
        this.age = age;
    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }
}
```

Our `Person` class only has two properties (`age` and `name`), along with a single constructor, yet it requires around `25 lines` and over `425 characters` to create in normal Java.

Now let's look at the same `Person` `POJO`, written in Kotlin:

```kotlin
data class Person(val name: String, val age: Int)
```

That's it.  The same functionality can be had in Kotlin with a single line of code, using merely `49` characters.  That's approaching an order of magnitude difference between the two!

This power stems primarily from the [`data class`](https://kotlinlang.org/docs/reference/data-classes.html) keyword, which essentially informs Kotlin that the class that follows will be a `POJO`, and thus, it should automatically infer some basic functionality.  This includes generation of `properties`, `getters` and `setters`, `equals()` and `toString()` methods, and more!

## Comparing Complex Classes

The word "complex" is used loosely here, but let's take a look at a slightly more involved class, and see how plain Java compares to Kotlin when we can't rely on a basic `data class` / `POJO` implementation.

Here we have the `Book` class, written in plain Java:

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
```

The `Book` class has four properties, each with their own getters and setters.  It also has a couple extra methods, such as `toJsonString()`.  This class is beyond a simple `POJO`, so implementing it in Kotlin will be a bit trickier than our previous example.  Let's see what it looks like in Kotlin:

```kotlin
// Book.kt
package io.airbrake

import com.fasterxml.jackson.databind.ObjectMapper
import com.fasterxml.jackson.annotation.*
import io.airbrake.utility.Logging

import java.util.Date

@JsonIgnoreProperties(ignoreUnknown = true)
class Book {

    private var author: String? = null

    private val maximumPageCount: Int = 4000

    private var pageCount: Int? = null
        @Throws(IllegalArgumentException::class)
        set(pageCount) {
            if (pageCount!! > maximumPageCount) {
                throw IllegalArgumentException(String.format("Page count value [%d] exceeds maximum limit [%d].", pageCount, maximumPageCount))
            }
            field = pageCount
        }

    private var publishedAt: Date? = null

    /**
     * Get a formatted tagline with author, title, and page count.
     *
     * @return Formatted tagline.
     */
    val tagline: String
        get() = String.format("'%s' by %s is %d pages.", title, author, pageCount)

    private var title: String? = null

    /**
     * Constructs an empty book.
     */
    constructor()

    /**
     * Constructs a basic book.
     *
     * @param title Book title.
     * @param author Book author.
     */
    constructor(title: String, author: String) {
        this.author = author
        this.title = title
    }

    /**
     * Constructs a basic book, with page count.
     *
     * @param title Book title.
     * @param author Book author.
     * @param pageCount Book page count.
     */
    constructor(title: String, author: String, pageCount: Int?) {
        this.author = author
        this.pageCount = pageCount
        this.title = title
    }

    /**
     * Constructs a basic book, with page count.
     *
     * @param title Book title.
     * @param author Book author.
     * @param pageCount Book page count.
     */
    constructor(title: String, author: String, pageCount: Int?, publishedAt: Date) {
        this.author = author
        this.pageCount = pageCount
        this.title = title
        this.publishedAt = publishedAt
    }

    /**
     * Publish current book.
     *
     * If book already published, throws IllegalStateException.
     */
    fun publish() {
        if (publishedAt == null) {
            publishedAt = Date()
            Logging.log(String.format("Published '%s' by %s.", title, author))
        } else {
            throw IllegalStateException(
                    String.format("Cannot publish '%s' by %s (already published on %s).",
                            title,
                            author,
                            publishedAt))
        }
    }

    /**
     * Output to JSON.
     *
     * @return Current Book formatted as String.
     */
    fun toJsonString(): String {
        return ObjectMapper().writeValueAsString(this)
    }

    /**
     * Throw an Exception.
     */
    fun throwException(message: String) {
        throw Exception(message)
    }
}
```

This time the difference in line and character count isn't so severe, but it's still quite significant.  The `Book.java` file clicks in at `183` lines and `4374` characters, while `Book.kt` is `114` lines and `2925` characters.  Beyond the actual amount of code, let's dig into the Kotlin version in a bit more detail and see what syntactically differentiates the two languages.

The `package`, `import`, and `class` definition statements look similar, but the first immediate difference is in how `properties` are handled:

```kotlin
    private var author: String? = null

    private val maximumPageCount: Int = 4000

    private var pageCount: Int? = null
        @Throws(IllegalArgumentException::class)
        set(pageCount) {
            if (pageCount!! > maximumPageCount) {
                throw IllegalArgumentException(String.format("Page count value [%d] exceeds maximum limit [%d].", pageCount, maximumPageCount))
            }
            field = pageCount
        }

    private var publishedAt: Date? = null
```

As we can see, Kotlin allows for a lot of the standard functionality to be inferred from the code itself.  Most properties, such as `author` and `publishedAt`, don't need to explicitly specify their `getter` or `setter` methods.  Kotlin handles that behind the scenes.  However, it's flexible enough to also allow inline `getter` and `setter` declaration, as seen in the `pageCount` property example, which requires a bit of extra logic for the `setter` method.

Additionally, you may notice the use of `var` and `val` keywords throughout Kotlin.  The `var` keyword indicates that the value is `mutable` (can be modified), while the `val` keyword is for `immutable` (unchangeable) values.  Thus, we can see that properties like `author` and `pageCount` can be changed at runtime, whereas `maximumPageCount` is effectively a `static final` value.  Kotlin doesn't have `static` methods.  Instead, it provides [`companion object`](https://kotlinlang.org/docs/reference/object-declarations.html#companion-objects), which _can_ be used as static methods, but can also be expanded by doing things like implementing interfaces.

The next difference we see in the `Book.kt` file is the `constructor` syntax:

```kotlin
    /**
     * Constructs an empty book.
     */
    constructor()

    /**
     * Constructs a basic book.
     *
     * @param title Book title.
     * @param author Book author.
     */
    constructor(title: String, author: String) {
        this.author = author
        this.title = title
    }

    // ...
```

Kotlin makes it a bit easier to create constructors by simply using the `constructor` keyword, rather than explicitly using the name of the class every time.  While we don't use it here, Kotlin also provides two _types_ of constructors: a `primary constructor` and `secondary constructors`.  All the constructors seen in the `Book.kt` file above are `secondary constructors`, because they are explicit `constructor()` methods listed within the class block.  A `primary constructor`, on the other hand, appears directly within the class header, following the declared name of the class.

For example, here's a `Person` class with a `primary constructor` that expects two arguments:

```kotlin
class Person(name: String, age: Int) {
    // ...
}
```

We next get to some of the non-property methods of the `Book` class, which shows another different in Kotlin: Functions (and class methods) are declared using the [`fun`](https://kotlinlang.org/docs/reference/functions.html) keyword:

```kotlin
    /**
     * Output to JSON.
     *
     * @return Current Book formatted as String.
     */
    fun toJsonString(): String {
        return ObjectMapper().writeValueAsString(this)
    }
```

We can also see the `: String` syntax used to indicate the return value type, as opposed to the preceding technique used in plain Java.  In addition, as you may have already noticed, Kotlin provides [`semicolon inference`](https://kotlinlang.org/docs/reference/grammar.html#semicolons), which basically allows Kotlin to assume or _infer_ that a semicolon is meant to exist at the end of most lines (where a `newline` character appears).

The last thing we'll look at for now is best illustrated in some test code used to actually create a new `Book` instance.  Here's our plain `Java` code used to create a new `Book`:

```java
Book book = new Book(
        "The Music of the Primes: Searching to Solve the Greatest Mystery in Mathematics",
        "Marcus du Sautoy",
        335,
        new GregorianCalendar(2014, 10, 14).getTime()
);
```

And here's the same in Kotlin:

```kotlin
val book = Book(
        "The Music of the Primes: Searching to Solve the Greatest Mystery in Mathematics",
        "Marcus du Sautoy",
        335,
        GregorianCalendar(2014, 10, 14).time
)
```

The first thing to notice is the use of the `val` keyword in place of explicit `Book` type declaration.  This is because, like .NET and other languages, Kotlin is typically able to [`implicitly infer`](https://kotlinlang.org/docs/reference/basic-types.html) data types from the context.  In addition, Kotlin doesn't require the `new` keyword when instantiating a class object -- it simply knows a new instance is being created.

Finally, take a look at the slight (but important) difference at the end of the `GregorianCalendar(...)` instance declaration, when converting it to a time value.  Plain Java requires calling the `getTime()` method, while Kotlin, once again, _infers_ that a reference to a `property` of an instance object, such as `time`, should point to the `getter` method by the same name.  It knows that the `time` property refers to the `getTime()` getter method.  Cool!

---

We're out of time for today, but we've just scratched the surface of all the cool new features that Kotlin brings to Java development, so come back soon for future articles exploring all the rest of this juicy goodness!

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look at why Kotlin is the better form of Java, including functional code sample comparisons between the two langauges.

---

__SOURCES__

- https://kotlinlang.org/docs/reference/