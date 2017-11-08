# import fpectl
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
