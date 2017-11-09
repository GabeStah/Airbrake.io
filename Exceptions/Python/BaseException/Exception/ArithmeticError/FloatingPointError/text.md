# Python Exception Handling - FloatingPointError

Today we get started with our in-depth [__Python Exception Handling__](https://airbrake.io/blog/python-exception-handling/class-hierarchy) series by looking at the **FloatingPointError**.  As with most programming languages, the `FloatingPointError` in Python indicates that something has gone wrong with a floating point calculation.  However, _unlike_ most other languages, Python will not raise a `FloatingPointError` by default.  The ability to do so must be implemented by including the [`fpectl`](https://docs.python.org/3/library/fpectl.html) module when building your local Python environment.

In this article we'll explore the `FloatingPointError` by first looking at where it resides in the overall [Python Exception Class Hierarchy](https://airbrake.io/blog/python-exception-handling/class-hierarchy).  We'll also go over how the `fpectl` module can be enabled, and how doing so can allow the raising of `FloatingPointErrors` in your own code.  Let's get to it!

## The Technical Rundown

All Python exceptions inherit from the [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException) class, or extend from an inherited class therein.  The full exception hierarchy of this error is:

- [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException)
    - [`Exception`](https://docs.python.org/3/library/exceptions.html#Exception)
        - [`ArithmeticError`](https://docs.python.org/3/library/exceptions.html#ArithmeticError)
            - `FloatingPointError`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```py
import fpectl
from gw_utility.logging import Logging


def main():
    Logging.line_separator("FLOATING POINT")
    test_floating_point()

    Logging.line_separator("DIVISION BY ZERO")
    test_division_by_zero()

    Logging.line_separator("FLOATING POINT DIVISION BY ZERO", 60)
    test_floating_point_division_by_zero()


def test_floating_point():
    try:
        Logging.log(round(24.601 / 3.5, 4))
    except FloatingPointError as exception:
        # Output expected FloatingPointErrors.
        Logging.log_exception(exception)
    except Exception as exception:
        # Output expected Exceptions.
        Logging.log_exception(exception, False)


def test_division_by_zero():
    try:
        # Divide by zero.
        Logging.log(24 / 0)
    except FloatingPointError as exception:
        # Output expected FloatingPointErrors.
        Logging.log_exception(exception)
    except Exception as exception:
        # Output expected Exceptions.
        Logging.log_exception(exception, False)


def test_floating_point_division_by_zero():
    try:
        # Divide by floating point zero and round.
        Logging.log(round(24.601 / 0.0, 4))
    except FloatingPointError as exception:
        # Output expected FloatingPointErrors.
        Logging.log_exception(exception)
    except Exception as exception:
        # Output expected Exceptions.
        Logging.log_exception(exception, False)


if __name__ == "__main__":
    main()

```

```py
import math
import sys
import traceback


class Logging:

    separator_character_default = '-'
    separator_length_default = 40

    @classmethod
    def __output(cls, *args, sep: str=' ', end: str='\n', file=None):
        """Prints the passed value(s) to the console.

        :param args: Values to output.
        :param sep: String inserted between values, default a space.
        :param end: String appended after the last value, default a newline.
        :param file: A file-like object (stream); defaults to the current sys.stdout.
        :return: None
        """
        print(*args, sep=sep, end=end, file=file)

    @classmethod
    def line_separator(cls, value: str, length: int=separator_length_default, char: str=separator_character_default):
        """Print a line separator with inserted text centered in the middle.

        :param value: Inserted text to be centered.
        :param length: Total separator length.
        :param char: Separator character.
        """
        output = value

        if len(value) < length:
            #   Update length based on insert length, less a space for margin.
            length -= len(value) + 2
            #   Halve the length and floor left side.
            left = math.floor(length / 2)
            right = left
            #   If odd number, add dropped remainder to right side.
            if length % 2 != 0:
                right += 1

            #   Surround insert with separators.
            output = f'{char * left} {value} {char * right}'

        cls.__output(output)

    @classmethod
    def log(cls, *args, sep: str=' ', end: str='\n', file=None):
        """Prints the passed value(s) to the console.

        :param args: Values to output.
        :param sep: String inserted between values, default a space.
        :param end: String appended after the last value, default a newline.
        :param file: A file-like object (stream); defaults to the current sys.stdout.
        """
        cls.__output(*args, sep=sep, end=end, file=file)

    @classmethod
    def log_exception(cls, exception: BaseException, expected: bool=True):
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

As discussed in the introduction, before a `FloatingPointError` can even appear you'll need to make sure your local Python build includes the [`fpectl`](https://docs.python.org/3/library/fpectl.html) module.  Since this module _is not_ included with most Python builds by default, you'd likely have had to explicitly build your Python with it if desired.  Adding the `fpectl` module to can be accomplished by using the `--with-fpectl` flag when compiling Python.  Going through the compilation process of Python is well beyond the scope of this article, but once `fpectl` is an included module, you can start testing the `FloatingPointError`.

For our example code we're not doing anything spectacular.  In fact, the `FloatingPointError` is effectively raised in situations where _other_ `ArithmeticErrors` would normally appear, except that you're using floating point numbers _and_ the `fpectl` module is enabled.  For example, you might raise a `FloatingPointError` where you'd normally get a `ZeroDivisionError` by attempting to divide by zero using a floating point value.

We've created a few simple testing methods starting with `test_floating_point()`:

```py
def test_floating_point():
    try:
        Logging.log(round(24.601 / 3.5, 4))
    except FloatingPointError as exception:
        # Output expected FloatingPointErrors.
        Logging.log_exception(exception)
    except Exception as exception:
        # Output expected Exceptions.
        Logging.log_exception(exception, False)
```

Executing this code works as expected, performing the floating point calculation and rounding the result to four decimal places before outputting the result to our log:

```
------------ FLOATING POINT ------------
7.0289
```

Now, let's step away from using a floating point value and use regular integers while attempting to divide by zero:

```py
def test_division_by_zero():
    try:
        # Divide by zero.
        Logging.log(24 / 0)
    except FloatingPointError as exception:
        # Output expected FloatingPointErrors.
        Logging.log_exception(exception)
    except Exception as exception:
        # Output expected Exceptions.
        Logging.log_exception(exception, False)
```

This raises an unexpected `ZeroDivisionException` since, even though `fpectl` is enabled, we aren't using a floating point value in our calculation:

```
----------- DIVISION BY ZERO -----------
[UNEXPECTED] ZeroDivisionError: division by zero
  File "D:/work/Airbrake.io/Exceptions/Python/BaseException/Exception/ArithmeticError/FloatingPointError/main.py", line 30, in test_division_by_zero
    Logging.log(24 / 0)
```

Finally, let's try the same division by zero while using floating point values:

```py
def test_floating_point_division_by_zero():
    try:
        # Divide by floating point zero and round.
        Logging.log(round(24.601 / 0.0, 4))
    except FloatingPointError as exception:
        # Output expected FloatingPointErrors.
        Logging.log_exception(exception)
    except Exception as exception:
        # Output expected Exceptions.
        Logging.log_exception(exception, False)
```

As you might suspect, this raises a `FloatingPointError` for us:

```
------------- FLOATING POINT DIVISION BY ZERO --------------
[EXPECTED] FloatingPointError: invalid value encountered in divide
  File "D:/work/Airbrake.io/Exceptions/Python/BaseException/Exception/ArithmeticError/FloatingPointError/main.py", line 42, in test_floating_point_division_by_zero
    Logging.log(round(24.601 / 0.0, 4))
```

There we have the basics of using `FloatingPointErrors`.  However, before you jump into adding the `fpectl` module to your Python to distinguish between `FloatingPointErrors` and normal `ArithmeticErrors`, there are a number of caveats and cautions to be aware of.  The [`IEEE 754`](https://en.wikipedia.org/wiki/IEEE_754) standard for floating point arithmetic defines a number of universal standards for the formatting, rounding, allowed operations, and exception handling practices of floating point numbers.  However, your code must be explicitly told to capture `IEEE 754` exceptions in the form of `SIGFPE` signals generated by the local processor.  Consequently, while Python is configured to do so via the `fpectl` module, many other custom scripts/applications are not.

The other major consideration is that use of the `fpectl` module is generally discouraged, in large part because it is _not thread safe_.  Thread safe applications (that is, most properly developed Python applications) allow data structures to be safely shared between multiple threads without fear of one thread manipulating or altering some data that another thread is using (or where another thread sees different data).  However, using the `fpectl` module means your floating point data is no longer thread safe, which could cause major issues in multithreaded applications.  To be on the safe side, it's generally recommended that you avoid `fpectl` and use another form of application logic to check for arithmetic errors.

Airbrake's robust <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-hierarchy">error monitoring software</a> provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize exception parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-hierarchy">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A brief look at the FloatingPointError in Python, including functional code samples discussing and showing how such errors can be raised.

---

__SOURCES__

- https://docs.python.org/3/library/exceptions.html