---
categories: [Python Exception Handling]
date: 2018-01-31
published: true
title: "Python Exception Handling - NameError"
---

Our journey continues through our detailed [__Python Exception Handling__](https://airbrake.io/blog/python-exception-handling/class-hierarchy) series with a deep look at the **NameError** found in Python.  Just as with many other programming languages, Python source code (typically found in `.py` files) is initially `compiled` into [`bytecode`](https://docs.python.org/3/glossary.html#term-bytecode), which is a low level representation of source code that can be executed by a virtual machine via the CPython interpreter.  Part of this process involves loading `local` or `global` objects into the callstack.  However, when Python attempts to load an object that doesn't exist elsewhere in the callstack it will forcefully raise a `NameError` indicating as much.

In today's article we'll explore the `NameError` by looking at where it resides in the overall [Python Exception Class Hierarchy](https://airbrake.io/blog/python-exception-handling/class-hierarchy).  We'll also look at some functional sample code that illustrates the basic compilation process Python source code goes through to turn into `bytecode`, and how improper references can result in `NameErrors` during this process.  Let's get right into it!

## The Technical Rundown

All Python exceptions inherit from the [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException) class, or extend from an inherited class therein.  The full exception hierarchy of this error is:

- [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException)
    - [`Exception`](https://docs.python.org/3/library/exceptions.html#Exception)
        - `NameError`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```py
import datetime
import dis

from gw_utility.book import Book
from gw_utility.logging import Logging


def main():
    try:
        # Create Book.
        book = Book("The Hobbit", "J.R.R. Tolkien", 366, datetime.date(1937, 9, 15))

        # Log book object.
        Logging.line_separator("log_object(book)", 60)
        log_object(book)

        # Log invalid object.
        Logging.line_separator("log_invalid_object(book)", 60)
        log_invalid_object(book)

        # Disassemble both log_ functions.
        Logging.line_separator("DISASSEMBLY OF log_object()", 60)
        disassemble_object(log_object)

        Logging.line_separator("DISASSEMBLY OF log_invalid_object()", 60)
        disassemble_object(log_invalid_object)
    except NameError as error:
        # Output expected NameErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


def log_object(value):
    """Logs passed value parameter to console.

    :param value: Value to be logged.
    :return: None
    """
    try:
        Logging.log(value)
    except NameError as error:
        # Output expected NameErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


def log_invalid_object(value):
    """Attempts to log invalid object (valu) to console.

    :param value: Value intended to be logged, but which is instead ignored.
    :return: None
    """
    try:
        Logging.log(valu)
    except NameError as error:
        # Output expected NameErrors.
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

As mentioned in the introduction, a `NameError` will occur when the CPython interpreter does not recognize a `local` or `global` object name that has been provided in the Python source code.  Let's jump right into some example code in normal Python, after which we'll see how we can disassemble this code into the `bytecode` that CPython actually reads and interprets.

We begin with two _extremely_ simple functions, `log_object(value)` and `log_invalid_object(value)`:

```py
def log_object(value):
    """Logs passed value parameter to console.

    :param value: Value to be logged.
    :return: None
    """
    try:
        Logging.log(value)
    except NameError as error:
        # Output expected NameErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


def log_invalid_object(value):
    """Attempts to log invalid object (valu) to console.

    :param value: Value intended to be logged, but which is instead ignored.
    :return: None
    """
    try:
        Logging.log(valu)
    except NameError as error:
        # Output expected NameErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)
```

The majority of the code for each of these is merely there for error handling, as the core functionality takes place on a single line: `Logging.log(value)` and `Logging.log(valu)`, respectively.  In essence, we're merely using these two functions to log the content of the passed `value` parameter to the console.  However, in the case of `log_invalid_object()` we have a slight typo of `valu` instead of `value`.

Let's test these out by creating a simple `Book` object instance and passing it to each of our two `log_` functions:

```py
# Create Book.
book = Book("The Hobbit", "J.R.R. Tolkien", 366, datetime.date(1937, 9, 15))

# Log book object.
Logging.line_separator("log_object(book)", 60)
log_object(book)

# Log invalid object.
Logging.line_separator("log_invalid_object(book)", 60)
log_invalid_object(book)
```

As you can probably guess, executing this code produces an expected `Book` object output, followed by raising a `NameError` because our typo of `valu` is not a recognized name:

```
--------------------- log_object(book) ---------------------
'The Hobbit' by J.R.R. Tolkien at 366 pages, published on September 15, 1937.

----------------- log_invalid_object(book) -----------------
[EXPECTED] NameError: name 'valu' is not defined
```

That's all well and good, but Python is a powerful language that allows us to look "under the hood" a bit and see the actual `bytecode` that each of these `log_` functions generates for the CPython interpreter.  We'll be using the built-in [`dis`](https://docs.python.org/3.5/library/dis.html) disassembler module, which was created for this very purpose.  By passing a function reference to the `dis.dis()` method we are provided a full output of the disassembled `bytecode` that CPython interprets during execution.  Our local `disassemble_object(value)` function is a small wrapper for this purpose:

```py
def disassemble_object(value):
    """Outputs disassembly of passed object.

    :param value: Object to be disassembled.
    :return: None
    """
    dis.dis(value)
```

Thus, we can see what the `bytecode` of the `log_object(value)` function looks like by running the following:

```py
# Disassemble both log_ functions.
Logging.line_separator("DISASSEMBLY OF log_object()", 60)
disassemble_object(log_object)
```

This produces the following output:

```
--------------- DISASSEMBLY OF log_object() ----------------
 41           0 SETUP_EXCEPT            14 (to 16)

 42           2 LOAD_GLOBAL              0 (Logging)
              4 LOAD_ATTR                1 (log)
              6 LOAD_FAST                0 (value)
              8 CALL_FUNCTION            1
             10 POP_TOP
             12 POP_BLOCK
             14 JUMP_FORWARD            88 (to 104)

 43     >>   16 DUP_TOP
             18 LOAD_GLOBAL              2 (NameError)
             20 COMPARE_OP              10 (exception match)
             22 POP_JUMP_IF_FALSE       58
             24 POP_TOP
             26 STORE_FAST               1 (error)
             28 POP_TOP
             30 SETUP_FINALLY           16 (to 48)

 45          32 LOAD_GLOBAL              0 (Logging)
             34 LOAD_ATTR                3 (log_exception)
             36 LOAD_FAST                1 (error)
             38 CALL_FUNCTION            1
             40 POP_TOP
             42 POP_BLOCK
             44 POP_EXCEPT
             46 LOAD_CONST               1 (None)
        >>   48 LOAD_CONST               1 (None)
             50 STORE_FAST               1 (error)
             52 DELETE_FAST              1 (error)
             54 END_FINALLY
             56 JUMP_FORWARD            46 (to 104)

 46     >>   58 DUP_TOP
             60 LOAD_GLOBAL              4 (Exception)
             62 COMPARE_OP              10 (exception match)
             64 POP_JUMP_IF_FALSE      102
             66 POP_TOP
             68 STORE_FAST               2 (exception)
             70 POP_TOP
             72 SETUP_FINALLY           18 (to 92)

 48          74 LOAD_GLOBAL              0 (Logging)
             76 LOAD_ATTR                3 (log_exception)
             78 LOAD_FAST                2 (exception)
             80 LOAD_CONST               2 (False)
             82 CALL_FUNCTION            2
             84 POP_TOP
             86 POP_BLOCK
             88 POP_EXCEPT
             90 LOAD_CONST               1 (None)
        >>   92 LOAD_CONST               1 (None)
             94 STORE_FAST               2 (exception)
             96 DELETE_FAST              2 (exception)
             98 END_FINALLY
            100 JUMP_FORWARD             2 (to 104)
        >>  102 END_FINALLY
        >>  104 LOAD_CONST               1 (None)
            106 RETURN_VALUE
```

This may appear a bit overwhelming at first, but this data is actually quite easy to interpret with a bit of knowledge about what we're looking at in each column.  The first column (e.g. `41`, `42`, `43` ... `48`) is the actual line number in the source doe for the corresponding set of instructions.  Thus, we can see that all of the following instructions...

```
 42           2 LOAD_GLOBAL              0 (Logging)
              4 LOAD_ATTR                1 (log)
              6 LOAD_FAST                0 (value)
              8 CALL_FUNCTION            1
             10 POP_TOP
             12 POP_BLOCK
             14 JUMP_FORWARD            88 (to 104)
```

...were generated from a single line of source code (#42):

```py
Logging.log(value)
```

The column with multiples of two (`0`, `2`, `4`, etc) is the `memory address` in the underlying `bytecode` for the given instruction.  Modern Python stores instructions using two bytes of data, hence the multiples of two.  The next column contains the `opname` (i.e. instruction) that should be executed, all of which can be found in the [official documentation](https://docs.python.org/3.5/library/dis.html#python-bytecode-instructions).

The column after that contains any `arguments`, if applicable, that each particular instruction will use.  The final column provides a human-friendly version of the instruction, so we can better visualize how the `bytecode` instruction correlates to source code.

Thus, let's look back at the single line `42` source code of `Logging.log(value)` and the generated `bytecode` instruction set to see what's going on:

```
 42           2 LOAD_GLOBAL              0 (Logging)
              4 LOAD_ATTR                1 (log)
              6 LOAD_FAST                0 (value)
              8 CALL_FUNCTION            1
             10 POP_TOP
             12 POP_BLOCK
             14 JUMP_FORWARD            88 (to 104)
```

It starts with `LOAD_GLOBAL` to load the global name `Logging` onto the stack.  It then loads the `log` attribute onto the top of the stack (`TOS`).  `LOAD_FAST` pushes a reference to a `local` variable called `value` onto the stack.  Next, `CALL_FUNCTION` calls the function at argument stack `1`, which is the `log` method added two instructions prior.  `POP_TOP` removes the most recent item added onto the stack, which is the `local` `value` object.  Every frame of execution contains a stack of code `blocks`, which are the logical groupings we see and create when writing source code that is locally grouped.  For example, a nested `loop` or, in this case, a `try-except` block, is contained within a separate code block in the stack.  Since the _next_ instruction that we're jumping to with `JUMP_FORWARD 88` is exiting the end of the `try` block found in our source code, `POP_BLOCK` is used to remove the top (current) block from the code block stack.

Cool, so let's see how this compiled `bytecode` for `log_object` differs from the _slightly_ modified `log_invalid_object` function:

```py
Logging.line_separator("DISASSEMBLY OF log_invalid_object()", 60)
disassemble_object(log_invalid_object)
```

We'll ignore the majority of the `bytecode` produced here since it is identical to that produced by `log_object`, but here we have the instruction set from the same corresponding `Logging.log(valu)` source code line we examined before:

```
 58           2 LOAD_GLOBAL              0 (Logging)
              4 LOAD_ATTR                1 (log)
              6 LOAD_GLOBAL              2 (valu)
              8 CALL_FUNCTION            1
             10 POP_TOP
             12 POP_BLOCK
             14 JUMP_FORWARD            88 (to 104)
```

Everything looks _exactly_ the same as before with two exceptions: The `line number` of `58` is obviously different, since we're compiling a different line of source code.  The second difference is the third instruction, which changed from `LOAD_FAST 0 (value)` to `LOAD_GLOBAL 2 (valu)`.  Why?  Because the compiler cannot reconcile a `local` object named `valu`, since the actual `local` parameter passed into the function is `value`, without the typo.  Therefore, the compiler assumes `valu` is a `global` name, and tries to load it via `LOAD_GLOBAL`.  As we know from executing the `log_invalid_object` function earlier, the CPython interpreter is unable to locate a `global` named `valu` during execution, so a `NameError` is raised to indicate as much.  Neat!

Airbrake's robust <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-handling">error monitoring software</a> provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize exception parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-handling">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look at the NameError in Python, with code samples illustrating how to view and evaluated compiled bytecode read by the CPython interpreter.

---

__SOURCES__

- https://docs.python.org/3/library/exceptions.html