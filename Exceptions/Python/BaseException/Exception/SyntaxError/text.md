# Python Exception Handling - SyntaxError

Moving along through our in-depth [__Python Exception Handling__](https://airbrake.io/blog/python-exception-handling/class-hierarchy) series, today we'll be going over the **SyntaxError**.  As with (probably?) every other programming language ever created, a `SyntaxError` is an indication that there is a syntactic error in the code, which causes the parser or compiler or executor to be unable to determine what the intention of the code is.  In the case of some programming languages a `SyntaxError` is a compiler error and not considered a `runtime error`, which can be caught by in-code execution.  In the case of Python, a `SyntaxError` can be _either_ a `runtime` or a `compiler` error, depending on the scenario and the code that generated it.

Throughout this article we'll examine the `SyntaxError` by exploring where it sits in the larger [Python Exception Class Hierarchy](https://airbrake.io/blog/python-exception-handling/class-hierarchy).  We'll also look at some fully functional code examples that illustrate the various ways in which `SyntaxError` can occur, and how they can be differentiated between `runtime` and `compiler` errors, so let's get going!

## The Technical Rundown

All Python exceptions inherit from the [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException) class, or extend from an inherited class therein.  The full exception hierarchy of this error is:

- [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException)
    - [`Exception`](https://docs.python.org/3/library/exceptions.html#Exception)
        - `SyntaxError`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```py
# exec_syntax_test.py
from gw_utility.logging import Logging

DOUBLE_DEFINITION = """
def double(x):
    return x * 2
"""

DOUBLE_EXECUTOR = """
Logging.log(double(5))
"""

TRIPLE_DEFINITION = """
def triple(x):
    return x * 3
"""

TRIPLE_EXECUTOR = """
Logging.log(triple(5)
"""


def main():
    try:
        Logging.log("Invoking: exec(DOUBLE_DEFINITION)")
        exec(DOUBLE_DEFINITION)
        Logging.log("Invoking: exec(DOUBLE_EXECUTOR)")
        exec(DOUBLE_EXECUTOR)

        Logging.log("Invoking: exec(TRIPLE_DEFINITION)")
        exec(TRIPLE_DEFINITION)
        Logging.log("Invoking: exec(TRIPLE_EXECUTOR)")
        exec(TRIPLE_EXECUTOR)
    except SyntaxError as error:
        # Output expected SyntaxErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


if __name__ == "__main__":
    main()

```

```py
# eval_syntax_test.py
import datetime
from gw_utility.logging import Logging

TOMORROW_DEFINITION = """
f'Tomorrow is {(today + datetime.timedelta(days=1)).strftime("%A, %B %d, %Y")}'
"""

DAY_AFTER_TOMORROW_DEFINITION = """
f'The day after tomorrow is {(today + datetime.timedelta(days=2))strftime("%A, %B %d, %Y")}'
"""


def main():
    try:
        today = datetime.datetime.now()
        Logging.log(f'Today is {today.strftime("%A, %B %d, %Y")}')

        Logging.log("Invoking: eval(TOMORROW_DEFINITION)")
        Logging.log(eval(TOMORROW_DEFINITION))

        Logging.log("Invoking: eval(DAY_AFTER_TOMORROW_DEFINITION)")
        Logging.log(eval(DAY_AFTER_TOMORROW_DEFINITION))
    except SyntaxError as error:
        # Output expected SyntaxErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


if __name__ == "__main__":
    main()

```

```py
# string_literal_syntax_test.py
from gw_utility.logging import Logging


def main():
    try:
        name = 'Alice
    except SyntaxError as error:
        # Output expected SyntaxErrors.
        Logging.log_exception(error)
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

As previously mentioned, Python is among a number of modern languages that (occasionally) allow you to actually `catch` `SyntaxErrors` under certain circumstances, so that code execution can continue.  That said, it is _usually_ a terribly bad idea, for a variety of reasons, to continue code execution once a `SyntaxError` has been raised.  In many cases, a `SyntaxError` indicates that surrounding code could have been executed in an insecure or unintended way, which could be catastrophic.  Regardless, for the code examples we'll be going through we're making every effort _to_ catch `SyntaxErrors`, just to illustrate the difference between `runtime` and `compiler` versions of the error.

To begin we'll start with the built-in [`exec()`](https://docs.python.org/3/library/functions.html#exec) function, which can be used to dynamically execute Python code, usually by parsing a `string` that contains valid Python code.  The code is evaluated and executed as if it were normal code written directly in a `.py` script file.  This practice is generally considered quite dangerous, given the risk of `injection attacks`, which are when a third-party injects unexpected code into the `string` that is being `exec()`d.  Such injection used to be a popular method of attack against SQL databases in particular, where an attacker inputs malicious SQL statements into form text boxes that are tied to SQL database queries where the incoming query statement isn't properly escaped.

For example, imagine a login form that accepts the value entered into `username` text box and blindly inserts them into an SQL statement:

```sql
SELECT id FROM users WHERE username = '" + username_field_value + "';"
```

In this scenario, anything entered into the `username` test box is injected into the `username_field_value` place in the SQL statement above.  So, what happens if we enter something like `abc'; DROP TABLE users; SELECT * FROM sys.tables WHERE '1' = '1`?  The full SQL statement becomes:

```sql
SELECT id FROM users WHERE username = 'abc'; DROP TABLE users; SELECT * FROM sys.tables WHERE '1' = '1';"
```

We've injected the `DROP TABLE users;` statement into the mix, telling the SQL database to delete the entire `users` table!  The power of such injection attacks should be clear, which is why direct execution of code in Python and other languages via `exec()` or `eval()` can be so dangerous.

All that said, this is for science, so here's our `exec_syntax_test.py` file:

```py
# exec_syntax_test.py
from gw_utility.logging import Logging

DOUBLE_DEFINITION = """
def double(x):
    return x * 2
"""

DOUBLE_EXECUTOR = """
Logging.log(double(5))
"""

TRIPLE_DEFINITION = """
def triple(x):
    return x * 3
"""

TRIPLE_EXECUTOR = """
Logging.log(triple(5)
"""


def main():
    try:
        Logging.log("Invoking: exec(DOUBLE_DEFINITION)")
        exec(DOUBLE_DEFINITION)
        Logging.log("Invoking: exec(DOUBLE_EXECUTOR)")
        exec(DOUBLE_EXECUTOR)

        Logging.log("Invoking: exec(TRIPLE_DEFINITION)")
        exec(TRIPLE_DEFINITION)
        Logging.log("Invoking: exec(TRIPLE_EXECUTOR)")
        exec(TRIPLE_EXECUTOR)
    except SyntaxError as error:
        # Output expected SyntaxErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


if __name__ == "__main__":
    main()

```

To make things a little more readable we've defined all our code strings as global constants at the beginning of the file.  Essentially, the `DOUBLE_DEFINITION` just defines a function called `double(x)` that doubles the passed numeric value and returns the result.  `DOUBLE_EXECUTOR` invokes `double(5)` and logs the result to the console.  Executing our `main()` function produces the following output:

```
Invoking: exec(DOUBLE_DEFINITION)
Invoking: exec(DOUBLE_EXECUTOR)
10

Invoking: exec(TRIPLE_DEFINITION)
Invoking: exec(TRIPLE_EXECUTOR)
[EXPECTED] SyntaxError: unexpected EOF while parsing (<string>, line 2)
  File "D:/work/Airbrake.io/Exceptions/Python/BaseException/Exception/SyntaxError/exec_syntax_test.py", line 33, in main
    exec(TRIPLE_EXECUTOR)
```

As expected, the invocation of `exec()` for the `DOUBLE_X` strings worked fine, but we ran into a `SyntaxError` when invoking the `TRIPLE_EXECUTOR` string.  As you may have noticed, `TRIPLE_EXECUTOR` contains a slight syntax error because it's missing the final closing parenthesis (`)`) to complete the `Logging.log()` function call.  This is the first example of a catchable `SyntaxError`, which is allowed because `exec()` and `eval()` are effectively executed AFTER the containing script code is evaluated and executed.  Thus, all the surrounding code is confirmed to be valid, so statements like `try: ... except SyntaxError as error: ...` work as expected.

Next, let's look at the `eval()` example found in the `eval_syntax_test.py` file.  While I'd normally place all test code in a single file and split it out using different functions or methods, as we'll see in a moment, because some `SyntaxErrors` can't be caught during execution, placing each test in a unique file helps differentiate the scenarios.

```py
# eval_syntax_test.py
import datetime
from gw_utility.logging import Logging

TOMORROW_DEFINITION = """
f'Tomorrow is {(today + datetime.timedelta(days=1)).strftime("%A, %B %d, %Y")}'
"""

DAY_AFTER_TOMORROW_DEFINITION = """
f'The day after tomorrow is {(today + datetime.timedelta(days=2))strftime("%A, %B %d, %Y")}'
"""


def main():
    try:
        today = datetime.datetime.now()
        Logging.log(f'Today is {today.strftime("%A, %B %d, %Y")}')

        Logging.log("Invoking: eval(TOMORROW_DEFINITION)")
        Logging.log(eval(TOMORROW_DEFINITION))

        Logging.log("Invoking: eval(DAY_AFTER_TOMORROW_DEFINITION)")
        Logging.log(eval(DAY_AFTER_TOMORROW_DEFINITION))
    except SyntaxError as error:
        # Output expected SyntaxErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


if __name__ == "__main__":
    main()

```

This test using the `eval()` function is similar to the `exec()` test before, with some slight variance.  The primary difference between `eval()` and `exec()` is that `exec()` is used to directly execute code of any kind, such as a function definition we saw in the last example.  On the other hand, `eval()` is used purely to _evaluate_ the result of a statement, which it "returns" within the calling code.  Put simply, `eval('x + 1')` would return the value of whatever `x` is, plus `1`, whereas `eval('x = 2')` would fail, because `eval()` cannot be used for complex execution the way that `exec()` can.

Executing the `eval_syntax_test.py` file produces the following output:

```
Today is Wednesday, December 20, 2017
Invoking: eval(TOMORROW_DEFINITION)

Tomorrow is Thursday, December 21, 2017
Invoking: eval(DAY_AFTER_TOMORROW_DEFINITION)
[EXPECTED] SyntaxError: invalid syntax (<fstring>, line 1)
  File "D:/work/Airbrake.io/Exceptions/Python/BaseException/Exception/SyntaxError/eval_syntax_test.py", line 23, in main
    Logging.log(eval(DAY_AFTER_TOMORROW_DEFINITION))
```

Once again, we start with a functional example by evaluating `TOMORROW_DEFINITION`, which takes the existing `today` object and adds one more day via the `datetime.timedelta(days=1)` call, then returns a formatted date string.  Meanwhile, the `DAY_AFTER_TOMORROW_DEFINITION` string contains a slight syntax error in the form of a missing period (`.`) separator between `(today + datetime.timedelta(days=2))` and `strftime(...)`.

The final example is the most common type of `SyntaxError`, in which a syntax problem is created outside of `eval()` or `exec()` strings, but is directly in script code.  `string_literal_syntax_test.py` merely attempts to assign the `name` variable to `'Alice`, which _does not_ contain the necessary closing quotation mark (`'`):

```py
# string_literal_syntax_test.py
from gw_utility.logging import Logging


def main():
    try:
        name = 'Alice
    except SyntaxError as error:
        # Output expected SyntaxErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


if __name__ == "__main__":
    main()

```

Executing the `string_literal_syntax_test.py` file produces the following output:

```
$ py string_literal_syntax_test.py
  File "D:\work\Airbrake.io\Exceptions\Python\BaseException\Exception\SyntaxError\string_literal_syntax_test.py", line 7
    name = 'Alice
                ^
SyntaxError: EOL while scanning string literal
```

As you can see, unlike the `eval()` and `exec()` examples, since the problematic code is executed at the same time as the rest of the file, we cannot capture this error because Python doesn't know how to evaluate surrounding code like the `try: ... except SyntaxError as error: ...` block.  Another similar way to raise `SyntaxErrors`, which we won't explore here, is via an `import` statement that tries to import a script that contains a syntax error itself.

Airbrake's robust <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-handling">error monitoring software</a> provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize exception parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-handling">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close examination of the SyntaxError in Python, including a functional code sample showing the difference between runtime and compiler errors.

---

__SOURCES__

- https://docs.python.org/3/library/exceptions.html