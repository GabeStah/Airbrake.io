# Python Exception Handling - AttributeError

Moving along through our in-depth [__Python Exception Handling__](https://airbrake.io/blog/python-exception-handling/class-hierarchy) series, today we'll dig into the **AttributeError**.  The `AttributeError` in Python is raised when an invalid [attribute reference](https://docs.python.org/3/reference/expressions.html#attribute-references) is made, or when an attribute assignment fails.  While most objects support attributes, those that do not will merely raise a `TypeError` when an attribute access attempt is made.

Throughout this article we'll examine the `AttributeError` by looking at where it sits in the larger [Python Exception Class Hierarchy](https://airbrake.io/blog/python-exception-handling/class-hierarchy).  We'll also discover a bit about how attributes and `attribute references` work in Python, then look at some functional sample code illustrating how to handle built-in and custom attribute access, and how doing so can raise `AttributeErrors` in your own code.  Let's get started!

## The Technical Rundown

All Python exceptions inherit from the [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException) class, or extend from an inherited class therein.  The full exception hierarchy of this error is:

- [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException)
    - [`Exception`](https://docs.python.org/3/library/exceptions.html#Exception)
        - `AttributeError`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```py
import datetime

from gw_utility.book import Book
from gw_utility.logging import Logging


def main():
    test()


def test():
    try:
        Logging.line_separator("CREATE BOOK", 50, '+')
        # Create and output book.
        book = Book("The Hobbit", "J.R.R. Tolkien", 366, datetime.date(1937, 9, 15))
        Logging.log(book)

        # Output valid attributes.
        Logging.log(book.title)
        Logging.log(book.author)

        # Output invalid attribute (publisher).
        Logging.log(book.publisher)
    except AttributeError as error:
        # Output expected AttributeErrors.
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

    def __init__(self,
                 title: str = None,
                 author: str = None,
                 page_count: int = None,
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

    def __getattr__(self, name: str):
        """Returns the attribute matching passed name."""
        # Get internal dict value matching name.
        value = self.__dict__.get(name)
        if not value:
            # Raise AttributeError if attribute value not found.
            raise AttributeError(f'{self.__class__.__name__}.{name} is invalid.')
        # Return attribute value.
        return value

    def __len__(self):
        """Returns the length of title."""
        return len(self.title)

    def __str__(self):
        """Returns a formatted string representation of Book."""
        date = '' if self.publication_date is None else f', published on {self.publication_date.__format__("%B %d, %Y")}'
        return f'\'{self.title}\' by {self.author} at {self.page_count} pages{date}.'
```

```py
import math
import sys
import traceback


class Logging:
    separator_character_default = '-'
    separator_length_default = 40

    @classmethod
    def __output(cls, *args, sep: str = ' ', end: str = '\n', file=None):
        """Prints the passed value(s) to the console.

        :param args: Values to output.
        :param sep: String inserted between values, default a space.
        :param end: String appended after the last value, default a newline.
        :param file: A file-like object (stream); defaults to the current sys.stdout.
        :return: None
        """
        print(*args, sep=sep, end=end, file=file)

    @classmethod
    def line_separator(cls, value: str = None, length: int = separator_length_default,
                       char: str = separator_character_default):
        """Print a line separator with inserted text centered in the middle.

        :param value: Inserted text to be centered.
        :param length: Total separator length.
        :param char: Separator character.
        """
        output = value

        # If no value passed, output separator of length.
        if value == None or len(value) == 0:
            output = f'{char * length}'
        elif len(value) < length:
            #   Update length based on insert length, less a space for margin.
            length -= len(value) + 2
            #   Halve the length and floor left side.
            left = math.floor(length / 2)
            right = left
            #   If odd number, add dropped remainder to right side.
            if length % 2 != 0:
                right += 1

            # Surround insert with separators.
            output = f'{char * left} {value} {char * right}'

        cls.__output(output)

    @classmethod
    def log(cls, *args, sep: str = ' ', end: str = '\n', file=None):
        """Prints the passed value(s) to the console.

        :param args: Values to output.
        :param sep: String inserted between values, default a space.
        :param end: String appended after the last value, default a newline.
        :param file: A file-like object (stream); defaults to the current sys.stdout.
        """
        cls.__output(*args, sep=sep, end=end, file=file)

    @classmethod
    def log_exception(cls, exception: BaseException, expected: bool = True):
        """Prints the passed BaseException to the console, including traceback.

        :param exception: The BaseException to output.
        :param expected: Determines if BaseException was expected.
        """
        output = "[{}] {}: {}".format('EXPECTED' if expected else 'UNEXPECTED', type(exception).__name__, exception)
        cls.__output(output)
        exc_type, exc_value, exc_traceback = sys.exc_info()
        traceback.print_tb(exc_traceback)

```

## When Should You Use It?

As previously mentioned, the `AttributeError` is raised when attempting to access an invalid attribute of an object.  The typically way to access an attribute is through an [`attribute reference`](https://docs.python.org/3/reference/expressions.html#attribute-references) syntax form, which is to separate the `primary` (the object instance) and the attribute `identifier` name with a period (`.`).  For example, `person.name` would attempt to retrieve the `name` attribute of the `person` object.

When evoking an attribute reference, under the hood Python expects to find the attribute that was directly accessed.  If it fails to locate a matching attribute it will then call the [`__getattr__()`](https://docs.python.org/3/reference/datamodel.html#object.__getattr__) method of the object, which performs a less efficient lookup of the instance attribute.  If this _also_ fails to find a matching attribute then an `AttributeError` is raised to indicate that an invalid attribute was accessed.

To illustrate with a code example we start with a modified `Book` class:

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

    def __init__(self,
                 title: str = None,
                 author: str = None,
                 page_count: int = None,
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

    def __getattr__(self, name: str):
        """Returns the attribute matching passed name."""
        # Get internal dict value matching name.
        value = self.__dict__.get(name)
        if not value:
            # Raise AttributeError if attribute value not found.
            raise AttributeError(f'{self.__class__.__name__}.{name} is invalid.')
        # Return attribute value.
        return value

    def __len__(self):
        """Returns the length of title."""
        return len(self.title)

    def __str__(self):
        """Returns a formatted string representation of Book."""
        date = '' if self.publication_date is None else f', published on {self.publication_date.__format__("%B %d, %Y")}'
        return f'\'{self.title}\' by {self.author} at {self.page_count} pages{date}.'
```

Notice that we've explicitly overriden the `__getattr__(self, name: str)` method of the base object class.  This allows us to perform any custom logic that might be necessary when an attribute is not immediately located.  To test this our `test()` method creates a new `Book` instance, outputs some explicit attributes to the log to prove everything works, and then attempts to directly access an invalid attribute of `book.publisher`:

```py
import datetime

from gw_utility.book import Book
from gw_utility.logging import Logging


def main():
    test()


def test():
    try:
        Logging.line_separator("CREATE BOOK", 50, '+')
        # Create and output book.
        book = Book("The Hobbit", "J.R.R. Tolkien", 366, datetime.date(1937, 9, 15))
        Logging.log(book)

        # Output valid attributes.
        Logging.log(book.title)
        Logging.log(book.author)

        # Output invalid attribute (publisher).
        Logging.log(book.publisher)
    except AttributeError as error:
        # Output expected AttributeErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


if __name__ == "__main__":
    main()

```

Executing this code produces the following output:

```
++++++++++++++++++ CREATE BOOK +++++++++++++++++++
'The Hobbit' by J.R.R. Tolkien at 366 pages, published on September 15, 1937.
The Hobbit
J.R.R. Tolkien
[EXPECTED] AttributeError: Book.publisher is invalid.
```

As you can see, everything worked fine until our attempt to acccess the `book.publisher` attribute, at which point the overridden `Book.__getattr__()` method was invoked, in which we raised an `AttributeError` with the custom error message seen above.

However, there's no reason to override the `__getattr()__` method if custom logic isn't required.  To illustrate this we'll temporarily comment out our `Book.__getattr__()` method so the built-in `__getattr__()` method is invoked instead.  Rerunning our code now produces the following output with a slightly different `AttributeError` message:

```
++++++++++++++++++ CREATE BOOK +++++++++++++++++++
'The Hobbit' by J.R.R. Tolkien at 366 pages, published on September 15, 1937.
The Hobbit
J.R.R. Tolkien
[EXPECTED] AttributeError: 'Book' object has no attribute 'publisher'
```

Airbrake's robust <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-handling">error monitoring software</a> provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize exception parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-handling">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look at the AttributeError in Python, including a functional code sample showing how to manually override attribute access in your classes.

---

__SOURCES__

- https://docs.python.org/3/library/exceptions.html