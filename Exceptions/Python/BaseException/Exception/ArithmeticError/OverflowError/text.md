# Python Exception Handling - OverflowError

Making our way through our detailed [__Python Exception Handling__](https://airbrake.io/blog/python-exception-handling/class-hierarchy) series brings us today to the **OverflowError** within Python.  Just like most other programming languages, the `OverflowError` in Python indicates that an arithmetic operation has exceeded the limits of the current Python runtime.  This is typically due to excessively large `Float` values, as `Integer` values that are too big will opt to raise [`MemoryErrors`](https://docs.python.org/3/library/exceptions.html#MemoryError) instead.

Throughout this article we'll examine the `OverflowError` by looking at where it sits in the overall [Python Exception Class Hierarchy](https://airbrake.io/blog/python-exception-handling/class-hierarchy), then we'll look at some functional sample code illustrating how we might calculate `pi` to a predetermined level of precision using both standard built-in arithmetic and additional libraries.  Let's get started!

## The Technical Rundown

All Python exceptions inherit from the [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException) class, or extend from an inherited class therein.  The full exception hierarchy of this error is:

- [`BaseException`](https://docs.python.org/3/library/exceptions.html#BaseException)
    - [`Exception`](https://docs.python.org/3/library/exceptions.html#Exception)
        - [`ArithmeticError`](https://docs.python.org/3/library/exceptions.html#ArithmeticError)
            - `OverflowError`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```py
import decimal
from enum import Enum

import sys
from gw_utility.logging import Logging
from mpmath import mp


class PiLibType(Enum):
    """Specifies library choices that are used to help calculate pi values."""
    INTEGER = 1
    FLOAT = 2
    DECIMAL = 3
    MPMATH = 4


def main():
    # Precision: 10
    pi_test(10)

    Logging.line_separator(None, 60, '_')

    # Precision: 25
    pi_test(25)

    Logging.line_separator(None, 60, '_')

    # Precision: 256
    pi_test(256)

    Logging.line_separator(None, 60, '_')

    # Precision: 300
    pi_test(300)


def pi_test(precision):
    # Integer
    Logging.line_separator(f'PI WITH PRECISION OF {precision}, USING INTEGERS', 60)
    Logging.log(get_pi(precision))

    # Float
    Logging.line_separator(f'PI WITH PRECISION OF {precision}, USING FLOATS', 60)
    Logging.log(get_pi(precision, PiLibType.FLOAT))

    # Decimal
    Logging.line_separator(f'PI WITH PRECISION OF {precision}, DECIMAL LIB', 60)
    Logging.log(get_pi(precision, PiLibType.DECIMAL))

    # MPMath
    # Set precision one higher to avoid rounding errors.
    Logging.line_separator(f'PI WITH PRECISION OF {precision + 1}, MPMATH LIB', 60)
    Logging.log(get_pi(precision + 1, PiLibType.MPMATH))


def get_pi(precision, lib: PiLibType = PiLibType.INTEGER):
    """Get value of pi with the specified level of precision, using passed numeric or library.

    :param precision: Precision to retrieve.
    :param lib: Type of numeric value or library to use for calculation.
    :return: Pi value with specified precision.
    """
    try:
        if lib == PiLibType.INTEGER:
            return pi_using_integer(precision)
        elif lib == PiLibType.FLOAT:
            return pi_using_float(precision)
        elif lib == PiLibType.DECIMAL:
            return pi_using_decimal_lib(precision)
        elif lib == PiLibType.MPMATH:
            return pi_using_mpmath_lib(precision)
    except OverflowError as error:
        # Output expected OverflowErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output expected Exceptions.
        Logging.log_exception(exception, False)


def pi_using_integer(precision):
    """Get value of pi via BBP formula to specified precision using integers.
    See: https://en.wikipedia.org/wiki/Bailey%E2%80%93Borwein%E2%80%93Plouffe_formula

    :param precision: Precision to retrieve.
    :return: Pi value with specified precision.
    """
    value = 0
    for k in range(precision):
        value += 1 / 16 ** k * (
            4 / (8 * k + 1) -
            2 / (8 * k + 4) -
            1 / (8 * k + 5) -
            1 / (8 * k + 6)
        )
    return value


def pi_using_float(precision):
    """Get value of pi via BBP formula to specified precision using floats.
    See: https://en.wikipedia.org/wiki/Bailey%E2%80%93Borwein%E2%80%93Plouffe_formula

    :param precision: Precision to retrieve.
    :return: Pi value with specified precision.
    """
    value = 0
    for k in range(precision):
        # Dot suffix converts value to a Float.
        value += 1. / 16. ** k * (
            4. / (8. * k + 1.) -
            2. / (8. * k + 4.) -
            1. / (8. * k + 5.) -
            1. / (8. * k + 6.)
        )
    return value


def pi_using_decimal_lib(precision):
    """Get value of pi via BBP formula to specified precision using decimal library.
    See: https://en.wikipedia.org/wiki/Bailey%E2%80%93Borwein%E2%80%93Plouffe_formula

    :param precision: Precision to retrieve.
    :return: Pi value with specified precision.
    """
    # Set precision for decimal library.
    decimal.getcontext().prec = precision
    value = 0
    for k in range(precision):
        value += decimal.Decimal(1) / decimal.Decimal(16) ** k * (
            decimal.Decimal(4) / (decimal.Decimal(8) * k + decimal.Decimal(1)) -
            decimal.Decimal(2) / (decimal.Decimal(8) * k + decimal.Decimal(4)) -
            decimal.Decimal(1) / (decimal.Decimal(8) * k + decimal.Decimal(5)) -
            decimal.Decimal(1) / (decimal.Decimal(8) * k + decimal.Decimal(6))
        )
    return value


def pi_using_mpmath_lib(precision):
    """Get value of pi to specified precision using mpmath library.

    :param precision: Precision to retrieve.
    :return: Pi value with specified precision.
    """
    # Set decimal points (mpmath automatically sets precision when dps is set).
    mp.dps = precision
    # Get pi value to specified precision.
    return mp.pi


if __name__ == "__main__":
    main()

```

This code sample also uses the [`Logging`](https://github.com/GabeStah/Airbrake.io/blob/master/lib/python/build/lib/gw_utility/logging.py) utility class, the source of which can be [found here on GitHub](https://github.com/GabeStah/Airbrake.io/blob/master/lib/python/build/lib/gw_utility/logging.py).

## When Should You Use It?

Last week we [examined the `FloatingPointError`](https://airbrake.io/blog/python-exception-handling/floatingpointerror) and saw how that error requires the `fpectl` to be enabled to raise such errors.  Normally, without the `fpectl` module enabled, `FloatingPointErrors` are dismissed in favor of other `ArithmeticErrors`, such as the `OverflowError` we're looking at today.

To test how `OverflowErrors` might be raised in typical code we're going to be attempting to calculate the value of `pi` out to a specific number of decimal places.  Throughout our calculations we use a `precision` value, as opposed to a number of decimal places.  In arithmetic, `precision` indicates the total number of digits in a value, including both digits before and after the decimal place.  Thus, a value of `12345.67890` has a `precision` of `10`.  Since we all know `pi` begins with `3.1415...`, our `precision` values will indicate one fewer decimal places than you might expect.

Anyway, there are a number of ways to calculate `pi`.  Traditionally, the methods of calculating `pi` to a given digit involved calculating the _preceding_ digits up to the target digit.  For example, the [Leibniz formula](https://en.wikipedia.org/wiki/Leibniz_formula_for_%CF%80) states that an ongoing series of values can be used to calculate `pi` to a specified digit by using `arctangent`.  However, a paper published in 1997 proving the [Bailey-Borwein-Plouffe formula](https://en.wikipedia.org/wiki/Bailey%E2%80%93Borwein%E2%80%93Plouffe_formula) (`BBP`) shows a technique for calculating a specific digit of `pi` using `base 16` mathematics (i.e. hexadecimal), _without_ needing to calculate any previous digits.  Not only is the formula quite beautiful and simple, this ability to calculate any chosen digit is particularly unique.  Here's the basic `BBP` formula:

![Bailey-Borwein-Plouffe formula](https://wikimedia.org/api/rest_v1/media/math/render/svg/af6bc360851499dd2ab2a90bee03fbe2040089d5)
_(Image courtesy of [Wikipedia.org](https://en.wikipedia.org/wiki/Bailey%E2%80%93Borwein%E2%80%93Plouffe_formula))_

To keep things simple we'll be using the `BBP` formula in our code, along with some helper libraries when necessary.  We'll start with a simple `Enum` called `PiLibType`, which will help us later to specify which type of numeric values or mathematic library we're using in our calculation:

```py
class PiLibType(Enum):
    """Specifies library choices that are used to help calculate pi values."""
    INTEGER = 1
    FLOAT = 2
    DECIMAL = 3
    MPMATH = 4
```

Next we've got our `get_pi(precision, lib: PiLibType = PiLibType.INTEGER)` method: 

```py
def get_pi(precision, lib: PiLibType = PiLibType.INTEGER):
    """Get value of pi with the specified level of precision, using passed numeric or library.

    :param precision: Precision to retrieve.
    :param lib: Type of numeric value or library to use for calculation.
    :return: Pi value with specified precision.
    """
    try:
        if lib == PiLibType.INTEGER:
            return pi_using_integer(precision)
        elif lib == PiLibType.FLOAT:
            return pi_using_float(precision)
        elif lib == PiLibType.DECIMAL:
            return pi_using_decimal_lib(precision)
        elif lib == PiLibType.MPMATH:
            return pi_using_mpmath_lib(precision)
    except OverflowError as error:
        # Output expected OverflowErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output expected Exceptions.
        Logging.log_exception(exception, False)
```

This method merely routes calculation to the correct submethod, passing the `precision` argument along with it.  We also handle all errors here.

The `pi_using_integer(precision)` method is our first calculation method, which uses the mentioned `BBP` formula, along with integer numeric values, to calculate `pi` at specified digits.  By looping through each digit up to our specified `precision` parameter value and adding the result to the total `value`, we're able to get the exact `pi` value at each given digit:

```py
def pi_using_integer(precision):
    """Get value of pi via BBP formula to specified precision using integers.
    See: https://en.wikipedia.org/wiki/Bailey%E2%80%93Borwein%E2%80%93Plouffe_formula

    :param precision: Precision to retrieve.
    :return: Pi value with specified precision.
    """
    value = 0
    for k in range(precision):
        value += 1 / 16 ** k * (
            4 / (8 * k + 1) -
            2 / (8 * k + 4) -
            1 / (8 * k + 5) -
            1 / (8 * k + 6)
        )
    return value
```

The `pi_using_float(precision)` method is the same as `pi_using_integer(precision)`, except we explicitly use float numeric values instead of integers:

```py
def pi_using_float(precision):
    """Get value of pi via BBP formula to specified precision using floats.
    See: https://en.wikipedia.org/wiki/Bailey%E2%80%93Borwein%E2%80%93Plouffe_formula

    :param precision: Precision to retrieve.
    :return: Pi value with specified precision.
    """
    value = 0
    for k in range(precision):
        # Dot suffix converts value to a Float.
        value += 1. / 16. ** k * (
            4. / (8. * k + 1.) -
            2. / (8. * k + 4.) -
            1. / (8. * k + 5.) -
            1. / (8. * k + 6.)
        )
    return value
```

We also want to take advantage of a few existing libraries to help us, so we start with the `decimal` library within the `pi_using_decimal_lib(precision)` method:

```py
def pi_using_decimal_lib(precision):
    """Get value of pi via BBP formula to specified precision using decimal library.
    See: https://en.wikipedia.org/wiki/Bailey%E2%80%93Borwein%E2%80%93Plouffe_formula

    :param precision: Precision to retrieve.
    :return: Pi value with specified precision.
    """
    # Set precision for decimal library.
    decimal.getcontext().prec = precision
    value = 0
    for k in range(precision):
        value += decimal.Decimal(1) / decimal.Decimal(16) ** k * (
            decimal.Decimal(4) / (decimal.Decimal(8) * k + decimal.Decimal(1)) -
            decimal.Decimal(2) / (decimal.Decimal(8) * k + decimal.Decimal(4)) -
            decimal.Decimal(1) / (decimal.Decimal(8) * k + decimal.Decimal(5)) -
            decimal.Decimal(1) / (decimal.Decimal(8) * k + decimal.Decimal(6))
        )
    return value
```

The only difference here is that we need to specify the precision of the library before calculations begin.  Then, each of our literal numeric values is represented with a `decimal.Decimal` object.

Finally, to verify our calculated values using the `BBP` formula we're using the `mpmath` library to output a base value of `pi` at the specified precision within the `pi_using_mpmath_lib(precision)` method:

```py
def pi_using_mpmath_lib(precision):
    """Get value of pi to specified precision using mpmath library.

    :param precision: Precision to retrieve.
    :return: Pi value with specified precision.
    """
    # Set decimal points (mpmath automatically sets precision when dps is set).
    mp.dps = precision
    # Get pi value to specified precision.
    return mp.pi
```

Cool.  Everything is setup and ready to test.  Let's make things a little easier to repeat by also adding a `pi_test(precision)` method, which calls `get_pi(precision, lib: PiLibType = PiLibType.INTEGER)` with the specified `precision` parameter, once for each of our four unique numeric value/library types:

```py
def pi_test(precision):
    # Integer
    Logging.line_separator(f'PI WITH PRECISION OF {precision}, USING INTEGERS', 60)
    Logging.log(get_pi(precision))

    # Float
    Logging.line_separator(f'PI WITH PRECISION OF {precision}, USING FLOATS', 60)
    Logging.log(get_pi(precision, PiLibType.FLOAT))

    # Decimal
    Logging.line_separator(f'PI WITH PRECISION OF {precision}, DECIMAL LIB', 60)
    Logging.log(get_pi(precision, PiLibType.DECIMAL))

    # MPMath
    # Set precision one higher to avoid rounding errors.
    Logging.line_separator(f'PI WITH PRECISION OF {precision + 1}, MPMATH LIB', 60)
    Logging.log(get_pi(precision + 1, PiLibType.MPMATH))
```

Nothing abnormal here, except it's worth noting the `mpmath` library will normally round our value at the end of the specified precision, so we explicitly increase the `precision` value by one more to ensure we're seeing an accurate representation when compared to other calculated results.

Alright, now in our `main()` method we actually test this all out with the `pi_test(precision)` method calls at various `precision` values, starting with `10`:

```py
def main():
    # Precision: 10
    pi_test(10)

    Logging.line_separator(None, 60, '_')

    # Precision: 25
    pi_test(25)

    Logging.line_separator(None, 60, '_')

    # Precision: 256
    pi_test(256)

    Logging.line_separator(None, 60, '_')

    # Precision: 300
    pi_test(300)
```

The first call of `pi_test(10)` results in the following output from each of our four calculation methods:

```
--------- PI WITH PRECISION OF 10, USING INTEGERS ----------
3.1415926535897913
---------- PI WITH PRECISION OF 10, USING FLOATS -----------
3.1415926535897913
----------- PI WITH PRECISION OF 10, DECIMAL LIB -----------
3.141592653
----------- PI WITH PRECISION OF 11, MPMATH LIB ------------
3.1415926536
```

The final result from `mpmath` is our baseline confirmation, so we can see that all of our previous three `BBP` formula methods are working as expected and calculating proper values.  However, we've specified a precision of `10`, yet both the `integer` and `float` methods output a value with a precision of much larger (`17`, as it turns out).  This can be explained by looking at the `sys.float_info` struct sequence, which describes the limitations of the current Python executable.  We won't go into detail here, but more information about it can be found in the [official documentation](https://docs.python.org/3/library/sys.html#sys.float_info).  In our case, while both `pi_using_integer(precision)` nor `pi_using_float(precision)` calculate the accurate decimal values of `pi` out to the specified `precision` digit, we aren't explicitly limiting the returned values length (`precision`), so we get the longest floating value Python can represent, as seen in `sys.float_info`.

Alright, let's move on to the results of `pi_test(25)`:

```
--------- PI WITH PRECISION OF 25, USING INTEGERS ----------
3.141592653589793
---------- PI WITH PRECISION OF 25, USING FLOATS -----------
3.141592653589793
----------- PI WITH PRECISION OF 25, DECIMAL LIB -----------
3.141592653589793238462644
----------- PI WITH PRECISION OF 26, MPMATH LIB ------------
3.1415926535897932384626434
```

Once again, without using a library Python cannot represent a float that is too long, but all the calculations are working as expected for a precision of `25`.  As it happens, we can calculate some fairly large values with a `precision` up to `256` digits with the built-in integer and float numeric arithmetic.  Here we see that our `BBP` formula still works as expected, even up to these large digit values:

```
--------- PI WITH PRECISION OF 256, USING INTEGERS ---------
3.141592653589793
---------- PI WITH PRECISION OF 256, USING FLOATS ----------
3.141592653589793
---------- PI WITH PRECISION OF 256, DECIMAL LIB -----------
3.141592653589793238462643383279502884197169399375105820974944592307816406286208998628034825342117067982148086513282306647093844609550582231725359408128481117450284102701938521105559644622948954930381964428810975665933446128475648233786783165271201909145654
----------- PI WITH PRECISION OF 257, MPMATH LIB -----------
3.1415926535897932384626433832795028841971693993751058209749445923078164062862089986280348253421170679821480865132823066470938446095505822317253594081284811174502841027019385211055596446229489549303819644288109756659334461284756482337867831652712019091456486
```

However, once we get up to a `257` digits or higher, we start to run into trouble.  Our call to `pi_test(300)` results in the following output:

```
--------- PI WITH PRECISION OF 300, USING INTEGERS ---------
3.141592653589793
---------- PI WITH PRECISION OF 300, USING FLOATS ----------
[EXPECTED] OverflowError: (34, 'Result too large')
None
---------- PI WITH PRECISION OF 300, DECIMAL LIB -----------
3.14159265358979323846264338327950288419716939937510582097494459230781640628620899862803482534211706798214808651328230664709384460955058223172535940812848111745028410270193852110555964462294895493038196442881097566593344612847564823378678316527120190914564856692346034861045432664821339360726024914131
----------- PI WITH PRECISION OF 301, MPMATH LIB -----------
3.141592653589793238462643383279502884197169399375105820974944592307816406286208998628034825342117067982148086513282306647093844609550582231725359408128481117450284102701938521105559644622948954930381964428810975665933446128475648233786783165271201909145648566923460348610454326648213393607260249141274
```

The attempt to use floats for calculation results in an `OverflowError` being raised once a value of `257` is reached.  This is because we're attempting to calculate the result of our float of `16.0` to the `257th` power (`16. ** 257`), which Python cannot handle as a float value, hence the `OverflowError`.  However, using libraries explicitly designed for larger numbers allows these calculations to continue without any trouble.

Airbrake's robust <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-handling">error monitoring software</a> provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize exception parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-python-exception-handling">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A brief look at the OverflowError in Python, including functional code samples discussing and showing how such errors can be raised.

---

__SOURCES__

- https://docs.python.org/3/library/exceptions.html