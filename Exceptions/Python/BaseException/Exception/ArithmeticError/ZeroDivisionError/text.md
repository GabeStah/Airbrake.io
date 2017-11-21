# Python Exception Handling - ZeroDivisionError

Moving along through our in-depth [__Python Exception Handling__](https://airbrake.io/blog/python-exception-handling/class-hierarchy) series, today we'll be looking at the **ZeroDivisionError**.  As you may suspect, the `ZeroDivisionError` in Python indicates that the second argument used in a division (or modulo) operation was zero.

Throughout this article we'll examine the `ZeroDivisionError` by looking at where it fits within the overall [Python Exception Class Hierarchy](https://airbrake.io/blog/python-exception-handling/class-hierarchy), then we'll look at some functional sample code illustrating how such errors may be raised in your own code.  We'll also see how different numeric types (and common mathematical libraries) produce slightly different results when handling division by zero issues.

## The Technical Rundown

All Python exceptions inherit from the [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException) class, or extend from an inherited class therein.  The full exception hierarchy of this error is:

- [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException)
    - [`Exception`](https://docs.python.org/3/library/exceptions.html#Exception)
        - [`ArithmeticError`](https://docs.python.org/3/library/exceptions.html#ArithmeticError)
            - `ZeroDivisionError`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```py
import decimal
from enum import Enum

from gw_utility.logging import Logging
from mpmath import mpf


class NumberType(Enum):
    """Specifies number type or library used for calculating values."""
    INTEGER = 1
    FLOAT = 2
    DECIMAL = 3
    MPMATH = 4


def main():
    Logging.line_separator("FRACTION TEST", 40, '+')

    divide_test(5, 25)

    Logging.line_separator("WHOLE NUMBER TEST", 40, '+')

    divide_test(25, 5)

    Logging.line_separator("DIVIDE BY ZERO TEST", 40, '+')

    divide_test(5, 0)


def divide_test(denominator, numerator):
    """Perform division tests using all different numeric types and mathematic libraries.

    :param denominator: Denominator.
    :param numerator: Numerator.
    """
    Logging.line_separator('as int')
    Logging.log(divide(denominator, numerator))

    Logging.line_separator('as float')
    Logging.log(divide(denominator, numerator, NumberType.FLOAT))

    Logging.line_separator('as decimal.Decimal')
    Logging.log(divide(denominator, numerator, NumberType.DECIMAL))

    Logging.line_separator('as mpmath.mpf')
    Logging.log(divide(denominator, numerator, NumberType.MPMATH))


def divide(numerator, denominator, lib: NumberType = NumberType.INTEGER):
    """Get result of division of numerator and denominator, using passed numeric type or library.

    :param numerator: Numerator.
    :param denominator: Denominator.
    :param lib: Type of numeric value or library to use for calculation.
    :return: Division result.
    """
    try:
        if lib == NumberType.INTEGER:
            # Divide using standard integer.
            return numerator / denominator
        elif lib == NumberType.FLOAT:
            # Convert to floats before division.
            return float(numerator) / float(denominator)
        elif lib == NumberType.DECIMAL:
            # Divide the decimal.Decimal value.
            return decimal.Decimal(numerator) / decimal.Decimal(denominator)
        elif lib == NumberType.MPMATH:
            # Divide using the mpmath.mpf (real float) value.
            return mpf(numerator) / mpf(denominator)
        else:
            # Divide using standard integer (default).
            return numerator / denominator
    except ZeroDivisionError as error:
        # Output expected ZeroDivisionErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


if __name__ == "__main__":
    main()

```

This code sample also uses the [`Logging`](https://github.com/GabeStah/Airbrake.io/blob/master/lib/python/build/lib/gw_utility/logging.py) utility class, the source of which can be [found here on GitHub](https://github.com/GabeStah/Airbrake.io/blob/master/lib/python/build/lib/gw_utility/logging.py).

## When Should You Use It?

The appearance of a `ZeroDivisionError` is never really surprising -- it just indicates that, somewhere in your code, a calculation took place and the denominator where zero.  Thus, we'll dive right into the sample code to look at how these errors slightly different depending on exactly what types of numeric values we're using.

We start with the `NumeerType(Enum)`, which we'll use throughout the code to differentiate between the various numeric types and mathematical libraries we'll be using, including `int`, `float`, `decimal.Decimal`, and `mpmath.mpf`:

```py
class NumberType(Enum):
    """Specifies number type or library used for calculating values."""
    INTEGER = 1
    FLOAT = 2
    DECIMAL = 3
    MPMATH = 4
```

Our `divide(numerator, denominator, lib: NumberType = NumberType.INTEGER)` method is where the majority of our logic and calculations take place:

```py
def divide(numerator, denominator, lib: NumberType = NumberType.INTEGER):
    """Get result of division of numerator and denominator, using passed numeric type or library.

    :param numerator: Numerator.
    :param denominator: Denominator.
    :param lib: Type of numeric value or library to use for calculation.
    :return: Division result.
    """
    try:
        if lib == NumberType.INTEGER:
            # Divide using standard integer.
            return numerator / denominator
        elif lib == NumberType.FLOAT:
            # Convert to floats before division.
            return float(numerator) / float(denominator)
        elif lib == NumberType.DECIMAL:
            # Divide the decimal.Decimal value.
            return decimal.Decimal(numerator) / decimal.Decimal(denominator)
        elif lib == NumberType.MPMATH:
            # Divide using the mpmath.mpf (real float) value.
            return mpf(numerator) / mpf(denominator)
        else:
            # Divide using standard integer (default).
            return numerator / denominator
    except ZeroDivisionError as error:
        # Output expected ZeroDivisionErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)
```

We start by checking for the `lib` parameter value, which determines what type the `numerator` and `denominator` should be converted to _before_ the calculation is performed.  We also catch possible errors and exceptions within this method.

The `divide_test(denominator, numerator)` method performs a series of calls to the `divide(...)` method above, ensuring we test each of the four different numeric types at least once for each set of passed `denominator` and `numerator` pairs:

```py
def divide_test(denominator, numerator):
    """Perform division tests using all different numeric types and mathematic libraries.

    :param denominator: Denominator.
    :param numerator: Numerator.
    """
    Logging.line_separator('as int')
    Logging.log(divide(denominator, numerator))

    Logging.line_separator('as float')
    Logging.log(divide(denominator, numerator, NumberType.FLOAT))

    Logging.line_separator('as decimal.Decimal')
    Logging.log(divide(denominator, numerator, NumberType.DECIMAL))

    Logging.line_separator('as mpmath.mpf')
    Logging.log(divide(denominator, numerator, NumberType.MPMATH))
```

Alright.  Everything is setup so now we'll perform a few basic tests within our `main()` method:

```py
def main():
    Logging.line_separator("FRACTION TEST", 40, '+')

    divide_test(5, 25)

    Logging.line_separator("WHOLE NUMBER TEST", 40, '+')

    divide_test(25, 5)

    Logging.line_separator("DIVIDE BY ZERO TEST", 40, '+')

    divide_test(5, 0)
```

Nothing fancy going on here.  We want to perform tests that should result in a fractional (decimal) number, a whole (integer) number, and an attempt to divide by zero.  Executing the code above produces the following output:

```
++++++++++++ FRACTION TEST +++++++++++++
---------------- as int ----------------
0.2
--------------- as float ---------------
0.2
---------- as decimal.Decimal ----------
0.2
------------ as mpmath.mpf -------------
0.2

++++++++++ WHOLE NUMBER TEST +++++++++++
---------------- as int ----------------
5.0
--------------- as float ---------------
5.0
---------- as decimal.Decimal ----------
5
------------ as mpmath.mpf -------------
5.0

+++++++++ DIVIDE BY ZERO TEST ++++++++++
---------------- as int ----------------
[EXPECTED] ZeroDivisionError: division by zero
None
--------------- as float ---------------
[EXPECTED] ZeroDivisionError: float division by zero
None
---------- as decimal.Decimal ----------
[EXPECTED] DivisionByZero: [<class 'decimal.DivisionByZero'>]
None
------------ as mpmath.mpf -------------
[EXPECTED] ZeroDivisionError: 
None
```

The fraction test isn't too surprising; every type test produced the exact same result of `0.2`.  The whole number test shows a slight difference in how the `decimal` library handles the result, truncating the insignificant digit (likely because it attempts to convert to an `int` after calculation, whereas other methods retain a `float` value).

What's most interesting is the divide by zero results.  Performing the calculation using plain `ints` results in a `ZeroDivisionError` with a `division by zero` message, while the same calculation with `float` values changes the message to `float division by zero`.  The `decimal` library has its own exception type of `DivisionByZero`, which inherits from the built-in `ZeroDivisionError` (we know this because our `except ZeroDivisionError` statement caught it).  Finally, `mpmath.mpf` values are effectively `floats`, except we see that the resulting `ZeroDivisionError` doesn't include an error message of any kind (i.e. the `error` value's `args` tuple is empty).

Airbrake's robust <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-handling">error monitoring software</a> provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize exception parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-handling">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A brief look at the ZeroDivisionError in Python, including a functional code sample showing how different numeric types produces different results.

---

__SOURCES__

- https://docs.python.org/3/library/exceptions.html