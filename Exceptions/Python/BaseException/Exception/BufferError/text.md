# Python Exception Handling - BufferError

Making our way through our detailed [__Python Exception Handling__](https://airbrake.io/blog/python-exception-handling/class-hierarchy) series, next up on the docket is the **BufferError**.  The `BufferError` occurs when a problem arises while working with any sort of memory buffer within Python.  Specifically, classes like `memoryview` and `BytesIO` tend to raise these error types when something goes wrong.

In today's article we'll explore the `BufferError` by examining where it resides in the overall [Python Exception Class Hierarchy](https://airbrake.io/blog/python-exception-handling/class-hierarchy), and then we'll take a look at some sample code that illustrate how one might work with buffers and memory in Python, and how that might lead to raising `BufferErrors` under certain circumstances.  Let's get to it!

## The Technical Rundown

All Python exceptions inherit from the [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException) class, or extend from an inherited class therein.  The full exception hierarchy of this error is:

- [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException)
    - [`Exception`](https://docs.python.org/3/library/exceptions.html#Exception)
        - `BufferError`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```py
import io

from gw_utility.logging import Logging


def main():
    buffer_test()


def log_view(view: memoryview):
    Logging.line_separator("MEMORY VIEW OUTPUT")
    Logging.log(f'tobytes(): {view.tobytes()}')
    Logging.log(f'tolist(): {view.tolist()}')
    Logging.log(f'hex(): {view.hex()}')


def buffer_test():
    try:
        # Create byte array with string 'Hello'.
        array = io.BytesIO(b'Hello')
        # Create a read-write copy of the bytearray.
        view = array.getbuffer()
        # Output copied memory view.
        log_view(view)
        # Add string ' world!' to existing bytearray.
        array.write(b' world!')
    except BufferError as error:
        # Output expected BufferErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
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

In most cases, Python raises `BufferErrors` when attempting to make restricted modifications to an existing buffer.  For example, if you create a `bytes` literal using the `b` prefix (e.g. `b'hello'`), that collection of bytes is made up of a memory array (or `buffer`).  A handful of built-in objects are considered buffers, such as `bytes`, `bytearray`, and a few extension types like `array.array`.  This type of data structure is used to house the binary data of an object in memory.

Moreover, just as with any form of binary data, even the smallest change will fundamentally alter what the data represents.  That is to say, a binary value of `10` could be changed to `11`, but this would alter the numeric value and change it from `2` to `3`.  This is normally not a problem to change existing data during an application's execution, but you need to be somewhat careful when altering buffers in Python.  Since Python buffers are stored in memory, and all references to that buffer point to the same memory address where the binary data is located, you may run into situations where multiple objects are referencing the _same_ buffer, and an attempt to modify the buffer (that is, change the underlying binary data) can result in a `BufferError`.

To illustrate this let's take a look at our simple sample code.  We'll start with the `log_view` helper function, which expects a `memoryview` object to be passed to it, and it outputs some information about this memoryview object, so we can verify the data:

```py
def log_view(view: memoryview):
    Logging.line_separator("MEMORY VIEW OUTPUT")
    Logging.log(f'tobytes(): {view.tobytes()}')
    Logging.log(f'tolist(): {view.tolist()}')
    Logging.log(f'hex(): {view.hex()}')
```

The core of our test takes place in the `buffer_test()` function, which creates a new `io.BytesIO` `bytearray` object with the bytes of string `'Hello'` as the value.  It then retrieves a read-write copy of the data in the form of a `memoryview` instance called `view`, which is then passed to the `log_view(view: memoryview)` function, to confirm that the original `bytes` copy was created.  Finally, we attempt to modify the original `bytearray` by calling the `write()` method and adding the byte string `' world!'` to it:

```py
def buffer_test():
    try:
        # Create byte array with string 'Hello'.
        array = io.BytesIO(b'Hello')
        # Create a read-write copy of the bytearray.
        view = array.getbuffer()
        # Output copied memory view.
        output_buffer(view)
        # Add string ' world!' to existing bytearray.
        array.write(b' world!')
    except BufferError as error:
        # Output expected BufferErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)
```

Executing the `buffer_test()` function produces the following output:

```
------------ BUFFER OUTPUT -------------
tobytes(): b'Hello'
tolist(): [72, 101, 108, 108, 111]
hex(): 48656c6c6f
[EXPECTED] BufferError: Existing exports of data: object cannot be re-sized
```

We can confirm that the `memoryview` read-write copy of the original `b'Hello'` string was created and matches, but we can see that trying to write additional bytes to the existing `bytearray` raises a `BufferError`, indicating that the object cannot be re-sized.  This occurs _because_ we created a `memoryview` copy.  As previously mentioned, Python doesn't actually create a new in-memory copy of the binary data, and instead, merely points both the `array` and `view` pointers to the same in-memory `buffer`.  Therefore, while `array` and `view` are the same at first, attempting to modify the underlying binary of data of `array` by adding `b' world!'` onto it would _also_ have to alter the size of the buffer used by `view`, which is not allowed.  Hence, a `BufferError` must be raised.

Airbrake's robust <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-handling">error monitoring software</a> provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize exception parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-handling">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look at the BufferError in Python, including a functional code sample showing how to create and use memory array buffers in Python.

---

__SOURCES__

- https://docs.python.org/3/library/exceptions.html