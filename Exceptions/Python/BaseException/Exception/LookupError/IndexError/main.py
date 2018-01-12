# main.py
import datetime

from gw_utility.book import Book
from gw_utility.logging import Logging


def main():
    try:
        # Create list and populate with Books.
        books = list()
        books.append(Book("Shadow of a Dark Queen", "Raymond E. Feist", 497, datetime.date(1994, 1, 1)))
        books.append(Book("Rise of a Merchant Prince", "Raymond E. Feist", 479, datetime.date(1995, 5, 1)))
        books.append(Book("Rage of a Demon King", "Raymond E. Feist", 436, datetime.date(1997, 4, 1)))

        # Output Books in list, with and without index.
        Logging.line_separator('Books')
        log_list(books)
        Logging.line_separator('Books w/ index')
        log_list(books, True)

        # Output list element outside bounds.
        Logging.line_separator('books[len(books)]')
        Logging.log(f'books[{len(books)}]: {books[len(books)]}')
    except IndexError as error:
        # Output expected IndexErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


def log_list(collection, include_index=False):
    """Logs the each element in collection to the console.

    :param collection: Collection to be iterated and output.
    :param include_index: Determines if index is also output.
    :return: None
    """
    try:
        # Iterate by converting to enumeration.
        for index, item in enumerate(collection):
            if include_index:
                Logging.log(f'collection[{index}]: {item}')
            else:
                Logging.log(item)
    except IndexError as error:
        # Output expected IndexErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


if __name__ == "__main__":
    main()
