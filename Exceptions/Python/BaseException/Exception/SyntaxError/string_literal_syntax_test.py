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
