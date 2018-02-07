import datetime
import dis

from gw_utility.book import Book
from gw_utility.logging import Logging


def main():
    try:
        # Increment local count.
        increment_local_count()

        # Set local book title.
        set_local_book_title("The Silmarillion")

        # Set global book title.
        set_global_book_title("The Silmarillion")

        # Disassemble functions.
        Logging.line_separator("DISASSEMBLY OF increment_count.", 60)
        disassemble_object(increment_local_count)

        Logging.line_separator("DISASSEMBLY OF set_local_book_title.", 60)
        disassemble_object(set_local_book_title)

        Logging.line_separator("DISASSEMBLY OF set_global_book_title.", 60)
        disassemble_object(set_global_book_title)
    except NameError as error:
        # Output expected NameErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


def increment_local_count():
    """Increment count by one and output new value.

    :return: None
    """
    try:
        Logging.line_separator("Incrementing LOCAL count.", 60)
        count += 1
        Logging.log("Count incremented to: {}".format(count))
    except UnboundLocalError as error:
        # Output expected UnboundLocalErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


def set_local_book_title(title):
    """Set title property of local book to passed value and output.

    :param title: Title to be set.
    :return: None
    """
    try:
        Logging.line_separator("Setting LOCAL book title to '{}'.".format(title), 60)
        book.title = title
        Logging.log(book)
    except UnboundLocalError as error:
        # Output expected UnboundLocalErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


global_book = Book("The Hobbit", "J.R.R. Tolkien", 365, datetime.date(1977, 9, 15))


def set_global_book_title(title):
    """Set title property of global_book to passed value and output.

    :param title: Title to be set.
    :return: None
    """
    try:
        Logging.line_separator("Setting GLOBAL book title to '{}'.".format(title), 60)
        global global_book
        global_book.title = title
        Logging.log(global_book)
    except UnboundLocalError as error:
        # Output expected UnboundLocalErrors.
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
