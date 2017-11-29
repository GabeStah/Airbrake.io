# Python Exception Handling - AssertionError

Making our way through our detailed [__Python Exception Handling__](https://airbrake.io/blog/python-exception-handling/class-hierarchy) series, today we're going over the **AssertionError**.  Like many programming languages, Python includes a built-in [`assert`](https://docs.python.org/3/reference/simple_stmts.html#assert) statement that allows you to create simple debug message outputs based on simple logical assertions.  When such an `assert` statement fails (i.e. returns a False-y value), an `AssertionError` is raised.

In this article we'll explore the `AssertionError` in more detail, starting with where it resides in the overall [Python Exception Class Hierarchy](https://airbrake.io/blog/python-exception-handling/class-hierarchy).  We'll also dig into some functional Python code samples that illustrate how `assert` statements can be used, and how the failure of such a statement will raise an `AssertionError` that should be caught and handled, just like any other error.  Let's get to it!

## The Technical Rundown

All Python exceptions inherit from the [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException) class, or extend from an inherited class therein.  The full exception hierarchy of this error is:

- [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException)
    - [`Exception`](https://docs.python.org/3/library/exceptions.html#Exception)
        - `AssertionError`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```py
import datetime

from gw_utility.book import Book
from gw_utility.logging import Logging


def main():
    Logging.line_separator("BOTH INCLUDE PUBLICATION DATES", 50, '+')
    # Create two Books with identical arguments.
    the_stand = Book("The Stand", "Stephen King", 1153, datetime.date(1978, 1, 1))
    the_stand_2 = Book("The Stand", "Stephen King", 1153, datetime.date(1978, 1, 1))

    # Check equivalency of Books.
    check_equality(the_stand, the_stand_2)

    Logging.line_separator("ONE MISSING PUBLICATION DATE", 50, '+')
    # Create two Books, one without publication_date argument specified.
    the_hobbit = Book("The Hobbit", "J.R.R. Tolkien", 366, datetime.date(1937, 9, 15))
    the_hobbit_2 = Book("The Hobbit", "J.R.R. Tolkien", 366)

    # Check equivalency of Books.
    check_equality(the_hobbit, the_hobbit_2)


def check_equality(a, b):
    """Asserts the equivalent of the two passed objects.

    :param a: First object.
    :param b: Second object.
    :return: Indicates if assertion was successful.
    """
    try:
        Logging.line_separator("ASSERTING EQUIVALENCE OF...")
        # Output objects using __str__ method.
        Logging.log(a)
        Logging.log(b)
        # Assert equivalence of objects, indicating inequality if failed.
        assert a == b, "The objects ARE NOT equal."
        # Indicate that assertion succeeded.
        Logging.log("The objects are equal.")
        return True
    except AssertionError as error:
        # Output expected AssertionErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


if __name__ == "__main__":
    main()

```

```py
import datetime


class Book:
    author: str
    page_count: int
    publication_date: datetime.date
    title: str

    def __eq__(self, other):
        """Determines if passed object is equivalent to current object."""
        return self.__dict__ == other.__dict__

    def __init__(self, title: str = None, author: str = None, page_count: int = None,
                 publication_date: datetime.date = None):
        """Initializes Book instance.

        :param title: Title of Book.
        :param author: Author of Book.
        :param page_count: Page Count of Book.
        :param publication_date: Publication Date of Book.
        """
        self.author = author
        self.page_count = page_count
        self.publication_date = publication_date
        self.title = title

    def __len__(self):
        """Returns the length of title."""
        return len(self.title)

    def __str__(self):
        """Returns a formatted string representation of Book."""
        date = '' if self.publication_date is None else f', published on {self.publication_date.__format__("%B %d, %Y")}'
        return f'\'{self.title}\' by {self.author} at {self.page_count} pages{date}.'

```

This code sample also uses the [`Logging`](https://github.com/GabeStah/Airbrake.io/blob/master/lib/python/build/lib/gw_utility/logging.py) utility class, the source of which can be [found here on GitHub](https://github.com/GabeStah/Airbrake.io/blob/master/lib/python/build/lib/gw_utility/logging.py).

## When Should You Use It?

As discussed in the introduction, an `AssertionError` can _only_ occur when an `assert` statement fails.  Therefore, an `AssertionError` should never be a surprise or appear in a section of your application code that is unexpected -- every time you write an `assert` statement, you should also provide appropriate exception handling code to deal with an inevitable `assert` failure.

To illustrate how `assert` statements work we'll be performing some basic equivalence testing to determine if one object is equal to a second object.  To make things a bit more interesting we've created a simple custom `Book` class that stores some basic information about each `Book` instance:

```py
class Book:
    author: str
    page_count: int
    publication_date: datetime.date
    title: str

    def __eq__(self, other):
        """Determines if passed object is equivalent to current object."""
        return self.__dict__ == other.__dict__

    def __init__(self, title: str = None, author: str = None, page_count: int = None,
                 publication_date: datetime.date = None):
        """Initializes Book instance.

        :param title: Title of Book.
        :param author: Author of Book.
        :param page_count: Page Count of Book.
        :param publication_date: Publication Date of Book.
        """
        self.author = author
        self.page_count = page_count
        self.publication_date = publication_date
        self.title = title

    def __len__(self):
        """Returns the length of title."""
        return len(self.title)

    def __str__(self):
        """Returns a formatted string representation of Book."""
        date = '' if self.publication_date is None else f', published on {self.publication_date.__format__("%B %d, %Y")}'
        return f'\'{self.title}\' by {self.author} at {self.page_count} pages{date}.'
```

As usual, we perform our instance property assignment in the `__init__(self, title: str = None, author: str = None, page_count: int = None, publication_date: datetime.date = None)` method.  Aside from that, the `__eq__(self, other)` method is worth noting, since this is the built-in method that will be called when attempting to check equivalence between a `Book` instance and another object.  To handle this we're using the `__dict__` built-in property as a form of comparison (though we could opt for `__str__(self)` comparison or otherwise).

The code we'll be using to test some object instances starts with the `check_equality(a, b)` method:

```py
def check_equality(a, b):
    """Asserts the equivalent of the two passed objects.

    :param a: First object.
    :param b: Second object.
    :return: Indicates if assertion was successful.
    """
    try:
        Logging.line_separator("ASSERTING EQUIVALENCE OF...")
        # Output objects using __str__ method.
        Logging.log(a)
        Logging.log(b)
        # Assert equivalence of objects, indicating inequality if failed.
        assert a == b, "The objects ARE NOT equal."
        # Indicate that assertion succeeded.
        Logging.log("The objects are equal.")
        return True
    except AssertionError as error:
        # Output expected AssertionErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)
```

Most of the code here handles outputting information to the log about the equality test.  The critical line is `assert a == b, "The objects ARE NOT equal."`, which performs an assertion that both passed objects are equivalent to one another.  The second argument of an `assert` statement is the failure message that is used as an argument if a failure occurs.  In practical terms, this failure message argument is added to the `AssertionError` instance `.args` property, giving us an actual error _message_ when catching the exception elsewhere in our code.  Since a failed `assert` statement always raises an `AssertionError`, if execution continues _past_ that statement we can assume the objects are equal and output as much to the log.

With everything setup we can test our assertion method by creating a couple `Book` instances, `the_stand` and `the_stand_2`:

```py
def main():
    Logging.line_separator("BOTH INCLUDE PUBLICATION DATES", 50, '+')
    # Create two Books with identical arguments.
    the_stand = Book("The Stand", "Stephen King", 1153, datetime.date(1978, 1, 1))
    the_stand_2 = Book("The Stand", "Stephen King", 1153, datetime.date(1978, 1, 1))

    # Check equivalency of Books.
    check_equality(the_stand, the_stand_2)

    # ...
```

Passing both `Book` instances to `check_equality(a, b)` produces the following output:

```
+++++++++ BOTH INCLUDE PUBLICATION DATES +++++++++
----- ASSERTING EQUIVALENCE OF... ------
'The Stand' by Stephen King at 1153 pages, published on January 01, 1978.
'The Stand' by Stephen King at 1153 pages, published on January 01, 1978.
The objects are equal.
```

As we can logically assume since all the arguments passed to both `Book` initializers were identical, our `assert` statement succeeded and we see the confirmation output in the log.

However, let's see what happens if we try a second test with two slightly different `Book` objects, where one instance wasn't passed a `publication_date` argument during initialization:

```py
Logging.line_separator("ONE MISSING PUBLICATION DATE", 50, '+')
# Create two Books, one without publication_date argument specified.
the_hobbit = Book("The Hobbit", "J.R.R. Tolkien", 366, datetime.date(1937, 9, 15))
the_hobbit_2 = Book("The Hobbit", "J.R.R. Tolkien", 366)

# Check equivalency of Books.
check_equality(the_hobbit, the_hobbit_2)
```

As you can probably guess, these two `Book` objects are _not_ considered equal, since their underlying `__dict__` properties are different from one another.  Consequently, our `assert` statement fails and raises an `AssertionError` in the output:

```
++++++++++ ONE WITHOUT PUBLICATION DATE ++++++++++
----- ASSERTING EQUIVALENCE OF... ------
'The Hobbit' by J.R.R. Tolkien at 366 pages, published on September 15, 1937.
'The Hobbit' by J.R.R. Tolkien at 366 pages.
[EXPECTED] AssertionError: The objects ARE NOT equal.
```

Airbrake's robust <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-handling">error monitoring software</a> provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize exception parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-handling">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A brief look at the AssertionError in Python, including a functional code sample illustrating how to create and use assert statements.

---

__SOURCES__

- https://docs.python.org/3/library/exceptions.html