# Python Exception Handling - ImportError and ModuleNotFoundError

Making our way through our detailed [__Python Exception Handling__](https://airbrake.io/blog/python-exception-handling/class-hierarchy) series we arrive at the **ImportError**, along with its single child subclass of **ModuleNotFoundError**.  The `ImportError` is raised when an [`import`](https://docs.python.org/3.6/reference/simple_stmts.html#import) statement has trouble successfully importing the specified module.  Typically, such a problem is due to an invalid or incorrect path, which will raise a `ModuleNotFoundError` in Python 3.6 and newer versions.

Within this article we'll explore the `ImportError` and `ModuleNotFoundError` in a bit more detail, beginning with where they sit in the overall [Python Exception Class Hierarchy](https://airbrake.io/blog/python-exception-handling/class-hierarchy).  We'll also take a look at some simple code samples that illustrate the differences in `import` statement failures across newer (3.6) and older (2.7) versions of Python, so let's get started!

## The Technical Rundown

All Python exceptions inherit from the [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException) class, or extend from an inherited class therein.  The full exception hierarchy of this error is:

- [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException)
    - [`Exception`](https://docs.python.org/3/library/exceptions.html#Exception)
        - `ImportError`
            - `ModuleNotFoundError`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```py
# outer_import_2.7.py
import sys
import gw_utility.Book


def main():
    try:
        print(sys.version)
    except ImportError as error:
        # Output expected ImportErrors.
        print(error.__class__.__name__ + ": " + error.message)
    except Exception as exception:
        # Output unexpected Exceptions.
        print(exception, False)
        print(exception.__class__.__name__ + ": " + exception.message)


if __name__ == "__main__":
    main()

```

```py
# inner_import_2.7.py
import sys


def main():
    try:
        print(sys.version)
        import gw_utility.Book
    except ImportError as error:
        # Output expected ImportErrors.
        print(error.__class__.__name__ + ": " + error.message)
    except Exception as exception:
        # Output unexpected Exceptions.
        print(exception, False)
        print(exception.__class__.__name__ + ": " + exception.message)


if __name__ == "__main__":
    main()

```

```py
# outer_import_3.6.py
import sys
import gw_utility.Book
from gw_utility.logging import Logging


def main():
    try:
        Logging.log(sys.version)
    except ImportError as error:
        # Output expected ImportErrors.
        Logging.log_exception(error)
        # Include the name and path attributes in output.
        Logging.log(f'error.name: {error.name}')
        Logging.log(f'error.path: {error.path}')
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


if __name__ == "__main__":
    main()

```

```py
# inner_import_3.6.py
import sys
from gw_utility.logging import Logging


def main():
    try:
        Logging.log(sys.version)
        import gw_utility.Book
    except ImportError as error:
        # Output expected ImportErrors.
        Logging.log_exception(error)
        # Include the name and path attributes in output.
        Logging.log(f'error.name: {error.name}')
        Logging.log(f'error.path: {error.path}')
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


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

The seemingly simple `import` statement found in Python is actually rather complex when looking under the hood.  At the most basic level an `import` statement is used to perform two tasks.  First, it attempts to find the module specified by name, then loads and initializes it, if necessary.  It also automatically defines a name in the local namespace within the scope of the associated `import` statement.  This local name can then be used to reference the the accessed module throughout the following scoped code.

While the `import` statement is the most common technique used to gain access to code from other modules, Python also provides other [methods and functions](https://docs.python.org/3.6/reference/import.html#importsystem) that makeup the built-in import system.  Developers can opt to use specific functions to have more fine-grained control over the import process.

For our code samples we'll stick to the common `import` statement that most of us are accustomed to.  As mentioned in the introduction, behavior for failed `imports` differs depending on the Python version.  To illustrate we start with the `outer_import_2.7.py` file:

```py
# outer_import_2.7.py
import sys
import gw_utility.Book


def main():
    try:
        print(sys.version)
    except ImportError as error:
        # Output expected ImportErrors.
        print(error.__class__.__name__ + ": " + error.message)
    except Exception as exception:
        # Output unexpected Exceptions.
        print(exception, False)
        print(exception.__class__.__name__ + ": " + exception.message)


if __name__ == "__main__":
    main()

```

The `outer` prefix for the file name indicates that we're testing an "outer" or globally scoped `import` statement of `gw_utility.Book`.  Executing this code produces the following output:

```
Traceback (most recent call last):
  File "C:\Users\Gabe\AppData\Local\JetBrains\Toolbox\apps\PyCharm-P\ch-0\172.3968.37\helpers\pydev\pydevd.py", line 1599, in <module>
    globals = debugger.run(setup['file'], None, None, is_module)
  File "C:\Users\Gabe\AppData\Local\JetBrains\Toolbox\apps\PyCharm-P\ch-0\172.3968.37\helpers\pydev\pydevd.py", line 1026, in run
    pydev_imports.execfile(file, globals, locals)  # execute the script
  File "D:/work/Airbrake.io/Exceptions/Python/BaseException/Exception/ImportError/outer_import_2.7.py", line 3, in <module>
    import gw_utility.Book
ImportError: No module named Book
```

The overall issue here is that the `gw_utility.Book` module doesn't exist.  In fact, the proper module is _lowercase_: `gw_utility.book`.  Since the `import` statement is at the top of the file, it exists outside our `try-except` block, so the `ImportError` we get in the log is not caught -- execution was terminated entirely when the error was raised.

Alternatively, let's see what happens if we move the `import` statement inside a `try-except` block, as seen in `inner_import_2.7.py`:

```py
# inner_import_2.7.py
import sys


def main():
    try:
        print(sys.version)
        import gw_utility.Book
    except ImportError as error:
        # Output expected ImportErrors.
        print(error.__class__.__name__ + ": " + error.message)
    except Exception as exception:
        # Output unexpected Exceptions.
        print(exception, False)
        print(exception.__class__.__name__ + ": " + exception.message)


if __name__ == "__main__":
    main()

```

Running this code -- also using Python 2.7 -- produces the same `ImportError`, but we're able to catch it and perform further processing of the caught `ImportError`, if necessary:

```
2.7.14 (v2.7.14:84471935ed, Sep 16 2017, 20:25:58) [MSC v.1500 64 bit (AMD64)]
ImportError: No module named Book
```

The `ModuleNotFoundError` was added in Python 3.6 as a subclass of `ImportError` and an explicit indication of the same kind of errors we're seeing above in the 2.7 code.  For example, let's look at the outer `import` example in Python 3.6 with `outer_import_3.6.py`:

```py
# outer_import_3.6.py
import sys
import gw_utility.Book
from gw_utility.logging import Logging


def main():
    try:
        Logging.log(sys.version)
    except ImportError as error:
        # Output expected ImportErrors.
        Logging.log_exception(error)
        # Include the name and path attributes in output.
        Logging.log(f'error.name: {error.name}')
        Logging.log(f'error.path: {error.path}')
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


if __name__ == "__main__":
    main()

```

Once again, here we're performing the `import` outside the `try-except` block, so running this code halts execution and produces the following output:

```
Traceback (most recent call last):
  File "C:\Users\Gabe\AppData\Local\JetBrains\Toolbox\apps\PyCharm-P\ch-0\172.3968.37\helpers\pydev\pydevd.py", line 1599, in <module>
    globals = debugger.run(setup['file'], None, None, is_module)
  File "C:\Users\Gabe\AppData\Local\JetBrains\Toolbox\apps\PyCharm-P\ch-0\172.3968.37\helpers\pydev\pydevd.py", line 1026, in run
    pydev_imports.execfile(file, globals, locals)  # execute the script
  File "C:\Users\Gabe\AppData\Local\JetBrains\Toolbox\apps\PyCharm-P\ch-0\172.3968.37\helpers\pydev\_pydev_imps\_pydev_execfile.py", line 18, in execfile
    exec(compile(contents+"\n", file, 'exec'), glob, loc)
  File "D:/work/Airbrake.io/Exceptions/Python/BaseException/Exception/ImportError/outer_import_3.6.py", line 3, in <module>
    import gw_utility.Book
ModuleNotFoundError: No module named 'gw_utility.Book'
```

The cause of this error is the exact same as the 2.7 version, but with 3.6+ the more specific `ModuleNotFoundError` is now raised.  Additionally, we can actually catch such errors if the `import` is executed within a `try-except` context:

```py
# inner_import_3.6.py
import sys
from gw_utility.logging import Logging


def main():
    try:
        Logging.log(sys.version)
        import gw_utility.Book
    except ImportError as error:
        # Output expected ImportErrors.
        Logging.log_exception(error)
        # Include the name and path attributes in output.
        Logging.log(f'error.name: {error.name}')
        Logging.log(f'error.path: {error.path}')
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


if __name__ == "__main__":
    main()

```

This code allows us to output the Python version and process the error:

```
3.6.3 (v3.6.3:2c5fed8, Oct  3 2017, 18:11:49) [MSC v.1900 64 bit (AMD64)]
[EXPECTED] ModuleNotFoundError: No module named 'gw_utility.Book'
error.name: gw_utility.Book
error.path: None
```

We're also outputting the `name` and `path` attributes of the `ImportError` object, which were added in Python 3.3 to indicate the name of the module that was attempted to be imported, along with the path to the file that triggered the exception, if applicable.  In this case our code is rather simple so, unfortunately, neither attribute is particularly useful.

Airbrake's robust <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-handling">error monitoring software</a> provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize exception parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-handling">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A deep dive into the ImportError and ModuleNotFoundError in Python, with code samples showing how to deal with failed imports in Python 2.7 and 3.6.

---

__SOURCES__

- https://docs.python.org/3/library/exceptions.html