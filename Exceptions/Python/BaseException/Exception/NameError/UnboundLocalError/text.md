---
categories: [Python Exception Handling]
date: 2018-02-07
published: true
title: "Python Exception Handling - UnboundLocalError"
---

Making our way through our in-depth [__Python Exception Handling__](https://airbrake.io/blog/python-exception-handling/class-hierarchy) series, today we'll be getting into the **UnboundLocalError**.  An `UnboundLocalError` is raised when a local variable is referenced before it has been assigned.  This error is a subclass of the Python [`NameError`](https://airbrake.io/blog/python-exception-handling/python-nameerror) we explored in another [recent article](https://airbrake.io/blog/python-exception-handling/python-nameerror).

Throughout the remainder of this post we'll examine the `UnboundLocalError` in more detail, starting with where it sits in the larger [Python Exception Class Hierarchy](https://airbrake.io/blog/python-exception-handling/class-hierarchy).  We'll also look at some functional sample code showing the slight difference between `NameErrors` and `UnboundLocalErrors`, and how you can avoid `UnboundLocalErrors` with special statements found in Python like [`global`](https://docs.python.org/3/reference/simple_stmts.html#the-global-statement).  Let the games begin!

## The Technical Rundown

All Python exceptions inherit from the [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException) class, or extend from an inherited class therein.  The full exception hierarchy of this error is:

- [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException)
    - [`Exception`](https://docs.python.org/3/library/exceptions.html#Exception)
        - [`NameError`](https://docs.python.org/3/library/exceptions.html#NameError)
            - `UnboundLocalError`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```py
import datetime
import dis

from gw_utility.book import Book
from gw_utility.logging import Logging


def main():
    try:
        # Increment local count.
        increment_local_count()

        # Set local book title.
        set_local_book_title("The Silmarillion")

        # Set global book title.
        set_global_book_title("The Silmarillion")

        # Disassemble functions.
        Logging.line_separator("DISASSEMBLY OF increment_count.", 60)
        disassemble_object(increment_local_count)

        Logging.line_separator("DISASSEMBLY OF set_local_book_title.", 60)
        disassemble_object(set_local_book_title)

        Logging.line_separator("DISASSEMBLY OF set_global_book_title.", 60)
        disassemble_object(set_global_book_title)
    except NameError as error:
        # Output expected NameErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


def increment_local_count():
    """Increment count by one and output new value.

    :return: None
    """
    try:
        Logging.line_separator("Incrementing LOCAL count.", 60)
        count += 1
        Logging.log("Count incremented to: {}".format(count))
    except UnboundLocalError as error:
        # Output expected UnboundLocalErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


def set_local_book_title(title):
    """Set title property of local book to passed value and output.

    :param title: Title to be set.
    :return: None
    """
    try:
        Logging.line_separator("Setting LOCAL book title to '{}'.".format(title), 60)
        book.title = title
        Logging.log(book)
    except UnboundLocalError as error:
        # Output expected UnboundLocalErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


global_book = Book("The Hobbit", "J.R.R. Tolkien", 365, datetime.date(1977, 9, 15))


def set_global_book_title(title):
    """Set title property of global_book to passed value and output.

    :param title: Title to be set.
    :return: None
    """
    try:
        Logging.line_separator("Setting GLOBAL book title to '{}'.".format(title), 60)
        global global_book
        global_book.title = title
        Logging.log(global_book)
    except UnboundLocalError as error:
        # Output expected UnboundLocalErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


def disassemble_object(value):
    """Outputs disassembly of passed object.

    :param value: Object to be disassembled.
    :return: None
    """
    dis.dis(value)


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

As the name suggests, `UnboundLocalErrors` are only raised when improperly referencing an unassigned `local` variable.  In most cases this will occur when trying to modify a local variable _before_ it is actually assigned within the local scope.  To illustrate we'll get right into our sample code and the `increment_local_count()` function:

```py
def increment_local_count():
    """Increment count by one and output new value.

    :return: None
    """
    try:
        Logging.line_separator("Incrementing LOCAL count.", 60)
        count += 1
        Logging.log("Count incremented to: {}".format(count))
    except UnboundLocalError as error:
        # Output expected UnboundLocalErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)
```

As you can see the `increment_local_count()` function does just what the name implies: trying to increment the local `count` variable by one and then outputting the result.  However, there's a distinct _lack_ of assignment for the `count` variable in the local scope of our function block, so executing this code raises an `UnboundLocalError`:

```
---------------- Incrementing LOCAL count. -----------------
[EXPECTED] UnboundLocalError: local variable 'count' referenced before assignment
```

That sort of makes sense.  Since no `count` variable could be located by the parser no resolution can occur for the increment statement.  However, this may look _very_ similar to the [NameError](https://airbrake.io/blog/python-exception-handling/python-nameerror) we looked at previously, which is raised when "global or _local_ names are not found."  So, what makes `UnboundLocalError` different from `NameError`?  We can illustrate this difference in our second test function, `set_locaL_book_title(title)`:

```py
def set_local_book_title(title):
    """Set title property of local book to passed value and output.

    :param title: Title to be set.
    :return: None
    """
    try:
        Logging.line_separator("Setting LOCAL book title to '{}'.".format(title), 60)
        book.title = title
        Logging.log(book)
    except UnboundLocalError as error:
        # Output expected UnboundLocalErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)
```

Just as with the increment test we're trying to use a local variable (`book`) that has not been assigned within the local function scope.  However, executing this function raises a `NameError`, rather than an `UnboundLocalError`:

```
----- Setting LOCAL book title to 'The Silmarillion'. ------
[UNEXPECTED] NameError: name 'book' is not defined
```

As we can see by the produced error message the difference here is that, since we're referencing the `title` property of `book`, the compiler assumes that `book` is actually a `global` name, so the CPython interpreter evaluates the instruction before it even _reaches_ the secondary instruction that references the `title` property we're ultimately attempting to reference.  We can use the `dis` module to disassemble our functions and see the full `bytecode` that the CPython interpreter actually processes during execution.  We won't go into full detail of these instructions and how the interpreter parses them, but check out our [`NameError` article](https://airbrake.io/blog/python-exception-handling/python-nameerror) from last week for more details.

Here is the `bytecode` instruction set for the `book.title = title` source code line in `set_local_book_title(title)`:

```
 61          20 LOAD_GLOBAL              3 (book)
             22 DUP_TOP
             24 LOAD_ATTR                4 (title)
             26 LOAD_FAST                0 (title)
             28 INPLACE_ADD
             30 ROT_TWO
             32 STORE_ATTR               4 (title)
```

As we can see, the first instruction is `LOAD_GLOBAL` using the `book` argument, indicating that the interpreter thinks `book` is a global.  This is why a `NameError` is produced, even though `book` is actually an undefined _local_ in this case.

One way to resolve this is by using the special [`global`](https://docs.python.org/3/reference/simple_stmts.html#the-global-statement) statement to reference a `global` variable that is assigned _outside_ the local function scope.  Here we see `global_book` is assigned to a value outside of the `set_global_book_title(title)` function scope:

```py
global_book = Book("The Hobbit", "J.R.R. Tolkien", 365, datetime.date(1977, 9, 15))

# Set global book title.
set_global_book_title("The Silmarillion")


def set_global_book_title(title):
    """Set title property of global_book to passed value and output.

    :param title: Title to be set.
    :return: None
    """
    try:
        Logging.line_separator("Setting GLOBAL book title to '{}'.".format(title), 60)
        global global_book
        global_book.title = title
        Logging.log(global_book)
    except UnboundLocalError as error:
        # Output expected UnboundLocalErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)
```

However, if we run this test function we're able to successfully update the `global_book.title` property:

```
----- Setting GLOBAL book title to 'The Silmarillion'. -----
'The Silmarillion' by J.R.R. Tolkien at 365 pages, published on September 15, 1977.
```

Just as within `set_local_book_title(title)`, the `bytecode` of `set_global_book_title(title)` shows the `global_book.title = title` source code statement contains the `LOAD_GLOBAL` instruction for the `global_book` object, but our use of the `global` statement informs the interpreter to actually seek out the globally-scoped name for reference:

```
 83          20 LOAD_FAST                0 (title)
             22 LOAD_GLOBAL              3 (global_book)
             24 STORE_ATTR               4 (title)
```

Airbrake's robust <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-handling">error monitoring software</a> provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize exception parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-handling">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A deep dive into the UnboundLocalError in Python, with code samples illustrating the difference between NameErrors and UnboundLocalErrors.

---

__SOURCES__

- https://docs.python.org/3/library/exceptions.html