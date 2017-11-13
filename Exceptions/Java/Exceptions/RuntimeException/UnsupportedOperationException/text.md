# Java Exception Handling - UnsupportedOperationException

Making our way through our detailed [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series, today we'll dive into the **UnsupportedOperationException**.  The `UnsupportedOperationException` is used by a number of built-in Java methods to indicate that the method in question is not currently implemented.  This is functionally similar to exceptions found in other languages, such as the .NET [`NotImplementedException`](https://airbrake.io/blog/dotnet-exception-handling/system-notimplementedexception) we explored in a [previous article](https://airbrake.io/blog/dotnet-exception-handling/system-notimplementedexception).

In today's article we'll examine the `UnsupportedOperationException` in more detail, beginning with a dive into where it sits in the overall [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).  We'll also go over a handful of functional code samples illustrating how `UnsupportedOperationExceptions` can be thrown in the standard Java library, along with how you might consider using them in your own code, so let's get started!

## The Technical Rundown

All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`java.lang.Object`](https://docs.oracle.com/javase/8/docs/api/java/lang/Object.html)
    - [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy)
        - [`java.lang.Exception`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html)
            - [`java.lang.RuntimeException`](https://docs.oracle.com/javase/8/docs/api/java/lang/RuntimeException.html)
                - `UnsupportedOperationException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```java
package io.airbrake;

import io.airbrake.utility.Logging;

import java.util.AbstractList;
import java.util.GregorianCalendar;

public class Main {

    public static void main(String[] args) {
        DefaultAbstractList<Book> defaultAbstractList = new DefaultAbstractList<>();

        // Add book to default list.
        addBookToList(
            new Book(
                "A Game of Thrones",
                "George R.R. Martin",
                848,
                new GregorianCalendar(1996, 8, 6).getTime()
            ),
            defaultAbstractList
        );

        MutableAbstractList<Book> mutableAbstractList = new MutableAbstractList<>();

        // Add book to extended list.
        addBookToList(
            new Book(
                "A Clash of Kings",
                "George R.R. Martin",
                761,
                new GregorianCalendar(1998, 9, 16).getTime()
            ),
                mutableAbstractList
        );

        ImmutableList<Book> immutableList = new ImmutableList<>();

        // Add book to immutable list.
        addBookToList(
            new Book(
                "A Storm of Swords",
                "George R.R. Martin",
                1177,
                new GregorianCalendar(2000, 7, 8).getTime()
            ),
            immutableList
        );
    }

    private static void addBookToList(Book book, AbstractList<Book> list) {
        try {
            Logging.lineSeparator(
                String.format("ADDING '%s' TO %s",
                    book.getTitle(),
                    list.getClass().getSimpleName()
                ), 60);
            Logging.log(book);

            // Attempt to add book to passed list.
            list.add(0, book);

            // Output modified list data and parent type.
            Logging.lineSeparator(String.format("MODIFIED %s", list.getClass().getSimpleName()));
            Logging.log(list.toString());
        } catch (UnsupportedOperationException exception) {
            // Output expected UnsupportedOperationExceptions.
            Logging.log(exception);
        } catch (Exception | Error exception) {
            // Output unexpected Exceptions/Errors.
            Logging.log(exception, false);
        }
    }

    private static void addBookToList(Book book, ImmutableList<Book> list) {
        try {
            Logging.lineSeparator(
                String.format("ADDING '%s' TO %s",
                    book.getTitle(),
                    list.getClass().getSimpleName()
                ), 60);
            Logging.log(book);

            // Attempt to add book to passed list.
            list.add(book);

            // Output modified list data and parent type.
            Logging.lineSeparator(String.format("MODIFIED %s", list.getClass().getSimpleName()));
            Logging.log(list.toString());
        } catch (UnsupportedOperationException exception) {
            // Output expected UnsupportedOperationExceptions.
            Logging.log(exception);
        } catch (Exception | Error exception) {
            // Output unexpected Exceptions/Errors.
            Logging.log(exception, false);
        }
    }
}
```

```java
package io.airbrake;

import java.util.AbstractList;

public class DefaultAbstractList<E> extends AbstractList<E> {

    @Override
    public E get(int index) {
        return null;
    }

    @Override
    public int size() {
        return 0;
    }
}
```

```java
package io.airbrake;

import java.util.AbstractList;
import java.util.ArrayList;

public class MutableAbstractList<E> extends AbstractList<E> {

    private ArrayList<E> list = new ArrayList<>();

    @Override
    public void add(int index, E element) {
        list.add(index, element);
    }

    @Override
    public E get(int index) {
        return list.get(index);
    }

    @Override
    public int size() {
        return list.size();
    }
}
```

```java
package io.airbrake;

import java.util.ArrayList;

public class ImmutableList<E> {

    private ArrayList<E> list = new ArrayList<>();

    /**
     * Adds a new element to the list.
     * Always throws UnsupportedOperationException.
     *
     * @param element Element to be added.
     * @throws UnsupportedOperationException
     */
    void add(E element) throws UnsupportedOperationException {
        throw new UnsupportedOperationException(
                String.format("%s is immutable, so the element [%s] cannot be added.",
                        this.getClass().getName(),
                        element.toString())
        );
    }

    /**
     * Adds a new element to the list at specified index.
     * Always throws UnsupportedOperationException.
     *
     * @param index Index at which to add element.
     * @param element Element to be added.
     * @throws UnsupportedOperationException
     */
    void add(int index, E element) throws UnsupportedOperationException {
        throw new UnsupportedOperationException(
                String.format("%s is immutable, so the element [%s] cannot be added at index [%d].",
                        this.getClass().getName(),
                        element.toString(),
                        index)
                );
    }

    /**
     * Gets the element at specified index.
     *
     * @param index Index of element.
     * @return Retrieved element.
     */
    public E get(int index) {
        return list.get(index);
    }

    /**
     * Gets the size of list.
     *
     * @return Number of elements in list.
     */
    public int size() {
        return list.size();
    }
}
```

This code sample also uses the [`Book.java`](https://github.com/GabeStah/Airbrake.io/blob/master/lib/java/src/io/airbrake/Book.java) class, the source of which can be [found on GitHub](https://github.com/GabeStah/Airbrake.io/blob/master/lib/java/src/io/airbrake/Book.java).

It also uses the [`Logging.java`](https://github.com/GabeStah/Airbrake.io/blob/master/lib/java/src/io/airbrake/utility/Logging.java) class, the source of which can be [found on GitHub](https://github.com/GabeStah/Airbrake.io/blob/master/lib/java/src/io/airbrake/utility/Logging.java).

## When Should You Use It?

The overall purpose of the `UnsupportedOperationException` is to indicate that the called method is not implemented or supported at present.  This might be due to ongoing development, which has to yet to finish the method.  Another common use for the `UnsupportedOperationException` is when the called method is _required_ due to implementation of an `interface`, yet invoking said method would be considered improper or invalid.  This is similar behavior to the .NET [`InvalidOperationException`](https://airbrake.io/blog/dotnet-exception-handling/system-invalidoperationexception) that we [looked at a few months ago](https://airbrake.io/blog/dotnet-exception-handling/system-invalidoperationexception).

No matter what the intention is, throwing an `UnsupportedOperationException` is the primary means of indicating that a method is not supported and should produce an error.  To illustrate this behavior we'll start with some of the built-in Java library usage of the `UnsupportedOperationException`.  A common collection of classes that make heavy use of this exception are, well, the [`Collection`](https://docs.oracle.com/javase/8/docs/api/java/util/Collection.html)-interfaced classes.  There are many types of collections within the standard library, and many of them share the same interface implementations, yet they don't all wish to allow the same functionality.

As an example, imagine that you need a collection that is _unmodifiable_ (i.e. `immutable`).  To accomplish this you might look to the [`AbstractList<E>`](https://docs.oracle.com/javase/8/docs/api/java/util/AbstractList.html) class.  Since this is an `abstract` class we must imeplement it ourselves in our own class, so we'll do so in the `DefaultAbstractList<E>` class:

```java
public class DefaultAbstractList<E> extends AbstractList<E> {

    @Override
    public E get(int index) {
        return null;
    }

    @Override
    public int size() {
        return 0;
    }
    
}
```

As it happens, if we simply need to create an `immutable` list from `AbstractList<E>`, we merely need to extend the class and override the `get(int index)` and `size()` methods, as seen above.  Now we can test our `DefaultAbstractList<E>` out.  We'll be creating a `Book` instance and attempting to add that element to our `DefaultAbstractList<Book>` instance.  Our `addBookToList(Book book, AbstractList<Book> list)` helper method will make this process easier to repeat:

```java
private static void addBookToList(Book book, AbstractList<Book> list) {
    try {
        Logging.lineSeparator(
            String.format("ADDING '%s' TO %s",
                book.getTitle(),
                list.getClass().getSimpleName()
            ), 60);
        Logging.log(book);

        // Attempt to add book to passed list.
        list.add(0, book);

        // Output modified list data and parent type.
        Logging.lineSeparator(String.format("MODIFIED %s", list.getClass().getSimpleName()));
        Logging.log(list.toString());
    } catch (UnsupportedOperationException exception) {
        // Output expected UnsupportedOperationExceptions.
        Logging.log(exception);
    } catch (Exception | Error exception) {
        // Output unexpected Exceptions/Errors.
        Logging.log(exception, false);
    }
}
```

Since `AbstractList<E>` is a parent class of our `DefaultAbstractList<E>` class, we're able to specify it as the expected parameter type of our method, allowing us to pass other class instances that extend `AbstractList<E>` later on.  Otherwise, the functionality of `addBookToList(...)` is fairly simple: We start by outputting the `book` info and `list` class we're attempting to add it to.  Then we call the `add(int index, E element)` method of the `list` parameter.  Finally, we output the modified `list` information, including the newly added element.

To test this out our `Main.main(...)` method creates a new `DefaultAbstractList<Book>` instance, then attempts to add a new `Book` instance to it via `addBookToList(...)`:

```java
public static void main(String[] args) {
    DefaultAbstractList<Book> defaultAbstractList = new DefaultAbstractList<>();

    // Add book to default list.
    addBookToList(
        new Book(
            "A Game of Thrones",
            "George R.R. Martin",
            848,
            new GregorianCalendar(1996, 8, 6).getTime()
        ),
        defaultAbstractList
    );
    
    // ...
}
```

If you're familiar with using `AbstractLists` already, you may see what's coming.  Executing the test code above produces the following output:

```
---- ADDING 'A Game of Thrones' TO DefaultAbstractList -----
io.airbrake.Book@1c53fd30[
  author=George R.R. Martin
  title=A Game of Thrones
  pageCount=848
  publishedAt=Fri Sep 06 00:00:00 PDT 1996
]
[EXPECTED] java.lang.UnsupportedOperationException
```

Here we catch our first `UnsupportedOperationException` which, unfortunately, fails to provide much useful information since there's no associated error message.  A bit of digging reveals that the issue is that, while the `AbstractList<E>` includes the [`add(int index, E element)`](https://docs.oracle.com/javase/8/docs/api/java/util/AbstractList.html#add-int-E-) method, it intentionally throws an `UnsupportedOperationException` when invoked.  This is because calling an `add(...)` method of any collection indicates that the collection should be changeable (`mutable`), which we did not plan for in our `DefaultAbstractList<E>` class.

To resolve this we'll create another class that is designed to handle mutable lists, `MutableAbstractList<E>`:

```java
package io.airbrake;

import java.util.AbstractList;
import java.util.ArrayList;

public class MutableAbstractList<E> extends AbstractList<E> {

    private ArrayList<E> list = new ArrayList<>();

    @Override
    public void add(int index, E element) {
        list.add(index, element);
    }

    @Override
    public E get(int index) {
        return list.get(index);
    }

    @Override
    public int size() {
        return list.size();
    }
}
```

Our `MutableAbstractList<E>` class actually includes some basic collection functionality.  We've added a private `ArrayList<E> list` property, along with implementation of the `add(int index, E element)`, `get(int index)`, and `size()` methods.  Once again, let's test this out by creating another new `Book` instance and attempting to add it to a new `MutableAbstractList<E>`:

```java
MutableAbstractList<Book> mutableAbstractList = new MutableAbstractList<>();

// Add book to extended list.
addBookToList(
    new Book(
        "A Clash of Kings",
        "George R.R. Martin",
        761,
        new GregorianCalendar(1998, 9, 16).getTime()
    ),
    mutableAbstractList
);
```

This time we have no trouble invoking the `add(int index, E element)` method to add our new `Book` instance, which results in the expected output showing the book being added to the collection:

```
----- ADDING 'A Clash of Kings' TO MutableAbstractList -----
io.airbrake.Book@63d4e2ba[
  author=George R.R. Martin
  title=A Clash of Kings
  pageCount=761
  publishedAt=Fri Oct 16 00:00:00 PDT 1998
]
----- MODIFIED MutableAbstractList -----
['A Clash of Kings' by George R.R. Martin is 761 pages, published Oct 16, 1998.]
```

That's cool and we've now seen how the standard Java library sometimes throws `UnsupportedOperationExceptions` to indicate invalid method calls.  However, let's look briefly at how we might throw an `UnsupportedOperationException` in our own code, under the right circumstances.  To accomplish this we'll look at our last custom class, aptly named `ImmutableList<E>`:

```java
package io.airbrake;

import java.util.ArrayList;

public class ImmutableList<E> {

    private ArrayList<E> list = new ArrayList<>();

    /**
     * Adds a new element to the list.
     * Always throws UnsupportedOperationException.
     *
     * @param element Element to be added.
     * @throws UnsupportedOperationException
     */
    void add(E element) throws UnsupportedOperationException {
        throw new UnsupportedOperationException(
                String.format("%s is immutable, so the element [%s] cannot be added.",
                        this.getClass().getName(),
                        element.toString())
        );
    }

    /**
     * Adds a new element to the list at specified index.
     * Always throws UnsupportedOperationException.
     *
     * @param index Index at which to add element.
     * @param element Element to be added.
     * @throws UnsupportedOperationException
     */
    void add(int index, E element) throws UnsupportedOperationException {
        throw new UnsupportedOperationException(
                String.format("%s is immutable, so the element [%s] cannot be added at index [%d].",
                        this.getClass().getName(),
                        element.toString(),
                        index)
                );
    }

    /**
     * Gets the element at specified index.
     *
     * @param index Index of element.
     * @return Retrieved element.
     */
    public E get(int index) {
        return list.get(index);
    }

    /**
     * Gets the size of list.
     *
     * @return Number of elements in list.
     */
    public int size() {
        return list.size();
    }
}
```

Functionally this class is similar to the `MutableAbstractList<E>` except, as the name suggests, it shouldn't allow collection elements to be modified.  Thus, both implementations of the `add(...)` method in this class intentionally throw an `UnsupportedOperationException`.  Unlike the standard Java throws we saw earlier, we've opted to include some more useful information in the error message.

Although subtle, we have to use a slightly different `addBookToList(...)` method signature than before, since `ImmutableList<E>` doesn't extend the `AbstractList<E>` class that our previous examples did.  Attempting to use the `addBookToList(Book book, AbstractList<E> list)` method signature with an `ImmutableList<E>` argument would result in a compilation error, so we're using this second method signature instead:

```java
private static void addBookToList(Book book, ImmutableList<Book> list) {
    try {
        Logging.lineSeparator(
            String.format("ADDING '%s' TO %s",
                book.getTitle(),
                list.getClass().getSimpleName()
            ), 60);
        Logging.log(book);

        // Attempt to add book to passed list.
        list.add(book);

        // Output modified list data and parent type.
        Logging.lineSeparator(String.format("MODIFIED %s", list.getClass().getSimpleName()));
        Logging.log(list.toString());
    } catch (UnsupportedOperationException exception) {
        // Output expected UnsupportedOperationExceptions.
        Logging.log(exception);
    } catch (Exception | Error exception) {
        // Output unexpected Exceptions/Errors.
        Logging.log(exception, false);
    }
}
```

Otherwise, our call to `addBookToList(...)` is much the same as before:

```java
ImmutableList<Book> immutableList = new ImmutableList<>();

// Add book to immutable list.
addBookToList(
    new Book(
        "A Storm of Swords",
        "George R.R. Martin",
        1177,
        new GregorianCalendar(2000, 7, 8).getTime()
    ),
    immutableList
);
```

As expected and intended, executing the above results in a thrown `UnsupportedOperationException`, which includes our detailed error message:

```
------- ADDING 'A Storm of Swords' TO ImmutableList --------
io.airbrake.Book@4563e9ab[
  author=George R.R. Martin
  title=A Storm of Swords
  pageCount=1177
  publishedAt=Tue Aug 08 00:00:00 PDT 2000
]
[EXPECTED] java.lang.UnsupportedOperationException: io.airbrake.ImmutableList is immutable, so the element ['A Storm of Swords' by George R.R. Martin is 1177 pages, published Aug 8, 2000.] cannot be added.
```

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look at the Java UnsupportedOperationException, with code samples showing how they are used in standard Java libraries, or in your own code.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html