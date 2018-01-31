import datetime
import dis

from gw_utility.book import Book
from gw_utility.logging import Logging


def main():
    try:
        # Create Book.
        book = Book("The Hobbit", "J.R.R. Tolkien", 366, datetime.date(1937, 9, 15))

        # Log book object.
        Logging.line_separator("log_object(book)", 60)
        log_object(book)

        # Log invalid object.
        Logging.line_separator("log_invalid_object(book)", 60)
        log_invalid_object(book)

        # Disassemble both log_ functions.
        Logging.line_separator("DISASSEMBLY OF log_object()", 60)
        disassemble_object(log_object)

        Logging.line_separator("DISASSEMBLY OF log_invalid_object()", 60)
        disassemble_object(log_invalid_object)
    except NameError as error:
        # Output expected NameErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


def log_object(value):
    """Logs passed value parameter to console.

    :param value: Value to be logged.
    :return: None
    """
    try:
        Logging.log(value)
    except NameError as error:
        # Output expected NameErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


def log_invalid_object(value):
    """Attempts to log invalid object (valu) to console.

    :param value: Value intended to be logged, but which is instead ignored.
    :return: None
    """
    try:
        Logging.log(valu)
    except NameError as error:
        # Output expected NameErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


def disassemble_object(value):
    """Outputs disassembly of passed object.

    :param value: Object to be disassembled.
    :return: None
    """
    dis.dis(value)


if __name__ == "__main__":
    main()
