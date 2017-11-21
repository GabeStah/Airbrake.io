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
