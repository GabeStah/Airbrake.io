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
