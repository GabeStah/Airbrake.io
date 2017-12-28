# Python Exception Handling - EOFError

Moving along through our in-depth [__Python Exception Handling__](https://airbrake.io/blog/python-exception-handling/class-hierarchy) series, today we'll be going over the **EOFError**.  The `EOFError` is raised by Python in a handful of specific scenarios: When the `input()` function is interrupted in both Python 2.7 and Python 3.6+, or when `input()` reaches the end of a file unexpectedly in Python 2.7.

Throughout this article we'll examine the `EOFError` by seeing where it resides in the overall [Python Exception Class Hierarchy](https://airbrake.io/blog/python-exception-handling/class-hierarchy).  We'll also look at some fully functional code examples that illustrate how the different major versions of Python handle user input, and how improper use of this functionality can sometimes produce `EOFErrors`, so let's get to it!

## The Technical Rundown

All Python exceptions inherit from the [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException) class, or extend from an inherited class therein.  The full exception hierarchy of this error is:

- [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException)
    - [`Exception`](https://docs.python.org/3/library/exceptions.html#Exception)
        - `EOFError`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```py
# input_test_3.6.py
import sys
from gw_utility.logging import Logging


def main():
    try:
        Logging.log(sys.version)
        title = input("Enter a book title: ")
        author = input("Enter the book's author: ")
        Logging.log(f'The book you entered is \'{title}\' by {author}.')
    except EOFError as error:
        # Output expected EOFErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


if __name__ == "__main__":
    main()

```

```py
# input_test_2.7.py
import sys


def main():
    try:
        print(sys.version)
        title = input("Enter a book title: ")
        author = input("Enter the book's author: ")
        print('The book you entered is \'' + title + '\' by ' + author + '.')
    except EOFError as error:
        # Output expected EOFErrors.
        print(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        print(exception, False)


if __name__ == "__main__":
    main()

```

```py
# raw_input_test_2.7.py
import sys


def main():
    try:
        print(sys.version)
        title = raw_input("Enter a book title: ")
        author = raw_input("Enter the book's author: ")
        print('The book you entered is \'' + title + '\' by ' + author + '.')
    except EOFError as error:
        # Output expected EOFErrors.
        print(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        print(exception, False)


if __name__ == "__main__":
    main()

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

Before we can take a look at some code samples we need to briefly review the built-in [`input()`](https://docs.python.org/3/library/functions.html#input) function in Python.  Simply put, in Python 3 `input()` presents a console prompt to the user and awaits user input, which is then converted to a `string` and returned as the result of the `input()` function invocation.  However, Python 2 had a slightly different behavior for `input()`, as it still prompts the user, but the input the user provides is parsed as _Python code_ and is evaluated as such.  To process user input using the Python 3 behavior, Python 2 also included the `raw_input()` function, which behaves the same as the Python 3 `input()` function.

To better illustrate these differences let's take a look at a few simple code snippets, starting with the `input_test_3.6.py` file:

```py
# input_test_3.6.py
import sys
from gw_utility.logging import Logging


def main():
    try:
        Logging.log(sys.version)
        title = input("Enter a book title: ")
        author = input("Enter the book's author: ")
        Logging.log(f'The book you entered is \'{title}\' by {author}.')
    except EOFError as error:
        # Output expected EOFErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


if __name__ == "__main__":
    main()

```

As you can see, the only thing we're doing here is requesting user `input()` two times for a book author and title, then outputting the result to the log.  Executing this code in Python 3.6 produces the following output:

```
3.6.3 (v3.6.3:2c5fed8, Oct  3 2017, 18:11:49) [MSC v.1900 64 bit (AMD64)]
Enter a book title: The Stand
Enter the book's author: Stephen King
The book you entered is 'The Stand' by Stephen King.
```

That works as expected.  After entering `The Stand` at the prompt for a title and `Stephen King` at the author prompt, our values are converted to strings and concatenated in the final output.  However, let's try executing the same test in Python 2.7 with the `input_test_2.7.py` file:

```py
# input_test_2.7.py
import sys


def main():
    try:
        print(sys.version)
        title = input("Enter a book title: ")
        author = input("Enter the book's author: ")
        print('The book you entered is \'' + title + '\' by ' + author + '.')
    except EOFError as error:
        # Output expected EOFErrors.
        print(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        print(exception, False)


if __name__ == "__main__":
    main()

```

Running this and entering `The Stand` for a title immediately raises a [SyntaxError](https://airbrake.io/blog/python-exception-handling/python-syntaxerror), with an underlying `EOFError`:

```
2.7.14 (v2.7.14:84471935ed, Sep 16 2017, 20:25:58) [MSC v.1500 64 bit (AMD64)]
Enter a book title: The Stand
(SyntaxError('unexpected EOF while parsing', ('<string>', 1, 9, 'The Stand')), False)
```

As discussed earlier, the problem here is how Python 2 interprets input from the, well, `input()` function.  Rather than converting the input value to a string, it evaluates the input as actual Python code.  Consequently, `The Stand` isn't valid code, so the end of file is detected and an error is thrown.

The resolution is to use the `raw_input()` function for Python 2 builds, as seen in `raw_input_test_2.7.py`:

```py
# raw_input_test_2.7.py
import sys


def main():
    try:
        print(sys.version)
        title = raw_input("Enter a book title: ")
        author = raw_input("Enter the book's author: ")
        print('The book you entered is \'' + title + '\' by ' + author + '.')
    except EOFError as error:
        # Output expected EOFErrors.
        print(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        print(exception, False)


if __name__ == "__main__":
    main()

```

Executing this in Python 2.7 produces the following output:

```
2.7.14 (v2.7.14:84471935ed, Sep 16 2017, 20:25:58) [MSC v.1500 64 bit (AMD64)]
Enter a book title: The Stand
Enter the book's author: Stephen King
The book you entered is 'The Stand' by Stephen King.
```

Everything works just fine and behaves exactly like the `input()` test running on Python 3.6.  However, we also need to be careful that the input isn't terminated prematurely, otherwise an `EOFError` will also be raised.  To illustrate, let's execute `raw_input_test_2.7.py` in Python 2.7 again, but this time we'll manually terminate the process (`Ctrl+D`) once the title prompt is shown:

```
2.7.14 (v2.7.14:84471935ed, Sep 16 2017, 20:25:58) [MSC v.1500 64 bit (AMD64)]
Enter a book title: ^D
EOF when reading a line
```

Unexpectedly terminating the input raises an `EOFError`, since the behavior from the Python interpreter's perspective is identical to if it evaluated input and reached the end of the file.  Similarly, let's perform the same manual termination with `input_test_3.6.py` running on Python 3.6:

```
3.6.3 (v3.6.3:2c5fed8, Oct  3 2017, 18:11:49) [MSC v.1900 64 bit (AMD64)]
Enter a book title: ^D
[EXPECTED] EOFError: EOF when reading a line
  File "D:/work/Airbrake.io/Exceptions/Python/BaseException/Exception/EOFError/input_test_3.6.py", line 9, in main
    title = input("Enter a book title: ")
```

It's worth noting that the `gw_utility` helper module isn't written for Python 2 versions, so we don't see the fancier error output in the previous Python 2 example, but otherwise the behavior and result is identical in both Python 2 and Python 3.

Airbrake's robust <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-handling">error monitoring software</a> provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize exception parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-handling">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look at the EOFError in Python, including a functional code sample showing how to handle user input in both Python 2 and Python 3.

---

__SOURCES__

- https://docs.python.org/3/library/exceptions.html