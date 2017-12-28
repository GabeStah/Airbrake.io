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
