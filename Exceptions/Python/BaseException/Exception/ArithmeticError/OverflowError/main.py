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
