---
categories: [Python Exception Handling]
date: 2018-01-17
published: true
title: Python Exception Handling - KeyError
---

Today, as we make our way through our detailed [__Python Exception Handling__](https://airbrake.io/blog/python-exception-handling/class-hierarchy) series, we'll be looking into the **KeyError**, which is the close sibling of the [`IndexError`](https://airbrake.io/blog/python-exception-handling/python-indexerror) we looked at last week.  Whereas the [`IndexError`](https://airbrake.io/blog/python-exception-handling/python-indexerror) is raised when trying to access an invalid `index` within a `list`, the `KeyError` is raised when accessing an invalid `key` within a `dict`.

Throughout this article we'll explore the `KeyError` in great depth by first looking at where it sits in the overall [Python Exception Class Hierarchy](https://airbrake.io/blog/python-exception-handling/class-hierarchy).  We'll also show some fully functional Python code samples that illustrate the basic usage of dictionaries in Python, and how improper key access can lead to `KeyErrors`.  Let's go!

## The Technical Rundown

All Python exceptions inherit from the [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException) class, or extend from an inherited class therein.  The full exception hierarchy of this error is:

- [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException)
    - [`Exception`](https://docs.python.org/3/library/exceptions.html#Exception)
        - [`LookupError`](https://docs.python.org/3/library/exceptions.html#LookupError)
            - `KeyError`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```py
# main.py
import datetime

from gw_utility.book import Book
from gw_utility.logging import Logging


def main():
    try:
        # Create a dictionary and populate with Books.
        series = {
            1:  Book("The Name of the Wind",    "Patrick Rothfuss",     662,    datetime.date(2007, 3, 27)),
            2:  Book("The Wise Man's Fear",     "Patrick Rothfuss",     994,    datetime.date(2011, 3, 1)),
            3:  Book("Doors of Stone",          "Patrick Rothfuss")
        }

        # Output Books in series dictionary, with and without index.
        Logging.line_separator('Series')
        log_dict(series)
        Logging.line_separator('Series w/ Order Index')
        log_dict(series, True)

        # Output book in series that doesn't exist.
        Logging.line_separator('books[len(books) + 1]')
        Logging.log(f'books[{len(series) + 1}]: {series[len(series) + 1]}')
    except KeyError as error:
        # Output expected KeyErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


def log_dict(collection, include_key=False):
    """Logs the each element in collection to the console.

    :param collection: Collection to be iterated and output.
    :param include_key: Determines if key should be output.
    :return: None
    """
    try:
        # Iterate by getting collection of items.
        for key, item in collection.items():
            if include_key:
                Logging.log(f'collection[{key}]: {item}')
            else:
                Logging.log(item)
    except KeyError as error:
        # Output expected KeyErrors.
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
        pages = '' if self.page_count is None else f' at {self.page_count} pages'
        return f'\'{self.title}\' by {self.author}{pages}{date}.'

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

Since the `IndexError` deals with `lists` and the `KeyError` deals with `dicts`, we should briefly explore the difference between these two common data structures in Python.  Python's [`lists`](https://docs.python.org/3/tutorial/datastructures.html) are similar to `arrays` in most other programming languages.  It is an ordered collection of objects that are each assigned in incremental numeric `index` to identify each element.  `Lists` are commonly used as `stacks`, which allows for the "first-in, last-out" behavior that is so crucial in many applications.

[`Dicts`](https://docs.python.org/3/tutorial/datastructures.html#dictionaries), on the other hand, are known as `associative arrays` in most other languages.  A `dict` is also a collection of objects, but it is _unordered_, and instead of using numeric `indices`, a `dict` uses immutable data types as `keys`.  When you see reference to `key: value` pairs in Python, this is an indication the collection holding those pairs is a `dict`.

To illustrate how to use `dicts` we'll create a `series` `dict` object and add a trio of `Books` to it.  Our `key` values are merely the relative order each book is found in the series, but we could have used anything for these `keys`:

```py
def main():
    try:
        # Create a dictionary and populate with Books.
        series = {
            1:  Book("The Name of the Wind",    "Patrick Rothfuss",     662,    datetime.date(2007, 3, 27)),
            2:  Book("The Wise Man's Fear",     "Patrick Rothfuss",     994,    datetime.date(2011, 3, 1)),
            3:  Book("Doors of Stone",          "Patrick Rothfuss")
        }

        # Output Books in series dictionary, with and without index.
        Logging.line_separator('Series')
        log_dict(series)
        Logging.line_separator('Series w/ Order Index')
        log_dict(series, True)

        # Output book in series that doesn't exist.
        Logging.line_separator('series[len(series) + 1]')
        Logging.log(f'series[{len(series) + 1}]: {series[len(series) + 1]}')
    except KeyError as error:
        # Output expected KeyErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)
```

Once our `series` dictionary has some elements we'll output them to the console to confirm what's in there using the `log_dict(collection, include_key=False)` method:

```py
def log_dict(collection, include_key=False):
    """Logs the each element in collection to the console.

    :param collection: Collection to be iterated and output.
    :param include_key: Determines if key should be output.
    :return: None
    """
    try:
        # Iterate by getting collection of items.
        for key, item in collection.items():
            if include_key:
                Logging.log(f'collection[{key}]: {item}')
            else:
                Logging.log(item)
    except KeyError as error:
        # Output expected KeyErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)
```

Executing our `main(...)` method produces the following output:

```
---------------- Series ----------------
'The Name of the Wind' by Patrick Rothfuss at 662 pages, published on March 27, 2007.
'The Wise Man's Fear' by Patrick Rothfuss at 994 pages, published on March 01, 2011.
'Doors of Stone' by Patrick Rothfuss.
-------- Series w/ Order Index ---------
collection[1]: 'The Name of the Wind' by Patrick Rothfuss at 662 pages, published on March 27, 2007.
collection[2]: 'The Wise Man's Fear' by Patrick Rothfuss at 994 pages, published on March 01, 2011.
collection[3]: 'Doors of Stone' by Patrick Rothfuss.
```

Everything looks as expected -- our `Books` were added and are being output via the modified `Book.__str__(self)` method:

```py
class Book:

    # ...

    def __str__(self):
        """Returns a formatted string representation of Book."""
        date = '' if self.publication_date is None else f', published on {self.publication_date.__format__("%B %d, %Y")}'
        pages = '' if self.page_count is None else f' at {self.page_count} pages'
        return f'\'{self.title}\' by {self.author}{pages}{date}.'
```

However, let's see what happens if we try to access an invalid `key` in our `dict`:

```py
# Output book in series that doesn't exist.
Logging.line_separator('series[len(series) + 1]')
Logging.log(f'series[{len(series) + 1}]: {series[len(series) + 1]}')
```

Here we're trying to access the `len(series) + 1` `key` of the dictionary, which is a shorthand way of accessing the `key` of value `4`.  Since the series only contains three `Books` in total, executing this code raises a `KeyError`:

```
-------- series[len(books) + 1] --------
[EXPECTED] KeyError: 4
  File "D:/work/Airbrake.io/Exceptions/Python/BaseException/Exception/LookupError/KeyError/main.py", line 25, in main
    Logging.log(f'series[{len(series) + 1}]: {series[len(series) + 1]}')
```

Airbrake's robust <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-handling">error monitoring software</a> provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize exception parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-handling">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A deep look at the KeyError in Python, with code samples illustrating the basic use of dictionary, and how invalid keys can raise KeyErrors.

---

__SOURCES__

- https://docs.python.org/3/library/exceptions.html