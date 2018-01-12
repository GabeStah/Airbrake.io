---
categories: [Python Exception Handling]
date: 2018-01-12
published: true
title: Python Exception Handling - IndexError
---

Moving right along through our in-depth [__Python Exception Handling__](https://airbrake.io/blog/python-exception-handling/class-hierarchy) series, today we'll be going over the **IndexError**, in all its glory.  The `IndexError` is one of the more basic and common exceptions found in Python, as it is raised whenever attempting to access an index that is outside the bounds of a `list`.

In today's article we'll examine the `IndexError` in more detail, starting with where it resides in the larger [Python Exception Class Hierarchy](https://airbrake.io/blog/python-exception-handling/class-hierarchy).  We'll also look into some functional Python sample code that will illustrate how basic lists are used and improper indexing can lead to `IndexErrors`.  Let's get into it!

## The Technical Rundown

All Python exceptions inherit from the [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException) class, or extend from an inherited class therein.  The full exception hierarchy of this error is:

- [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException)
    - [`Exception`](https://docs.python.org/3/library/exceptions.html#Exception)
        - [`LookupError`](https://docs.python.org/3/library/exceptions.html#LookupError)
            - `IndexError`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```py
# main.py
import datetime

from gw_utility.book import Book
from gw_utility.logging import Logging


def main():
    try:
        # Create list and populate with Books.
        books = list()
        books.append(Book("Shadow of a Dark Queen", "Raymond E. Feist", 497, datetime.date(1994, 1, 1)))
        books.append(Book("Rise of a Merchant Prince", "Raymond E. Feist", 479, datetime.date(1995, 5, 1)))
        books.append(Book("Rage of a Demon King", "Raymond E. Feist", 436, datetime.date(1997, 4, 1)))

        # Output Books in list, with and without index.
        Logging.line_separator('Books')
        log_list(books)
        Logging.line_separator('Books w/ index')
        log_list(books, True)
        # Output list element outside bounds.
        Logging.line_separator('books[len(books)]')
        Logging.log(f'books[{len(books)}]: {books[len(books)]}')
    except IndexError as error:
        # Output expected IndexErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


def log_list(collection, include_index=False):
    """Logs the each element in collection to the console.

    :param collection: Collection to be iterated and output.
    :param include_index: Determines if index is also output.
    :return: None
    """
    try:
        # Iterate by converting to enumeration.
        for index, item in enumerate(collection):
            if include_index:
                Logging.log(f'collection[{index}]: {item}')
            else:
                Logging.log(item)
    except IndexError as error:
        # Output expected IndexErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


if __name__ == "__main__":
    main()

```

```py
# book.py
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
# logging.py
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

Since the `IndexError` is raised when dealing with `lists`, our code sample is setup to handle the basic creation and iteration of a `list` of `Books`.  The `log_list(collection, include_index=False)` method iterates through the passed `collection` list by converting to an enumeration, then outputting each item (with or without the `index`):

```py
def log_list(collection, include_index=False):
    """Logs the each element in collection to the console.

    :param collection: Collection to be iterated and output.
    :param include_index: Determines if index is also output.
    :return: None
    """
    try:
        # Iterate by converting to enumeration.
        for index, item in enumerate(collection):
            if include_index:
                Logging.log(f'collection[{index}]: {item}')
            else:
                Logging.log(item)
    except IndexError as error:
        # Output expected IndexErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)
```

To test this out we start by creating a new `list` and appending a handful of books (from a great fantasy series, by the way) onto it:

```py
# Create list and populate with Books.
books = list()
books.append(Book("Shadow of a Dark Queen", "Raymond E. Feist", 497, datetime.date(1994, 1, 1)))
books.append(Book("Rise of a Merchant Prince", "Raymond E. Feist", 479, datetime.date(1995, 5, 1)))
books.append(Book("Rage of a Demon King", "Raymond E. Feist", 436, datetime.date(1997, 4, 1)))
```

Now, we need to pass our `books` list to the `log_list(...)` method, so we'll confirm that both the basic and index-included outputs work:

```py
# Output Books in list, with and without index.
Logging.line_separator('Books')
log_list(books)
Logging.line_separator('Books w/ index')
log_list(books, True)
```

As expected, this code works as it should and outputs the following `Book` details to the console:

```
---------------- Books -----------------
'Shadow of a Dark Queen' by Raymond E. Feist at 497 pages, published on January 01, 1994.
'Rise of a Merchant Prince' by Raymond E. Feist at 479 pages, published on May 01, 1995.
'Rage of a Demon King' by Raymond E. Feist at 436 pages, published on April 01, 1997.
------------ Books w/ index ------------
collection[0]: 'Shadow of a Dark Queen' by Raymond E. Feist at 497 pages, published on January 01, 1994.
collection[1]: 'Rise of a Merchant Prince' by Raymond E. Feist at 479 pages, published on May 01, 1995.
collection[2]: 'Rage of a Demon King' by Raymond E. Feist at 436 pages, published on April 01, 1997.
```

Just like arrays and other common collections in programming, Python `lists` are zero-indexed, so our three elements are only indexed up to a maximum of `2`.  However, let's see what happens if we try to access index `3` of the `books` list:

```py
# Output list element outside bounds.
Logging.line_separator('books[len(books)]')
Logging.log(f'books[{len(books)}]: {books[len(books)]}')
```

As you probably suspect, this raises an `IndexError`, indicating that the list index is out of range:

```
---------- books[len(books)] -----------
[EXPECTED] IndexError: list index out of range
  File "D:\work\Airbrake.io\Exceptions\Python\BaseException\Exception\LookupError\IndexError\main.py", line 23, in main
    Logging.log(f'books[{len(books)}]: {books[len(books)]}')
```

Airbrake's robust <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-handling">error monitoring software</a> provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize exception parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-handling">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A closer look at the IndexError in Python, with code samples illustrating the basic use of lists, and how invalid indices can raise IndexErrors.

---

__SOURCES__

- https://docs.python.org/3/library/exceptions.html