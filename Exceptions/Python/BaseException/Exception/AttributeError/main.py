import datetime

from gw_utility.book import Book
from gw_utility.logging import Logging


def main():
    test()


def test():
    try:
        Logging.line_separator("CREATE BOOK", 50, '+')
        # Create and output book.
        book = Book("The Hobbit", "J.R.R. Tolkien", 366, datetime.date(1937, 9, 15))
        Logging.log(book)

        # Output valid attributes.
        Logging.log(book.title)
        Logging.log(book.author)

        # Output invalid attribute (publisher).
        Logging.log(book.publisher)
    except AttributeError as error:
        # Output expected AttributeErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


if __name__ == "__main__":
    main()
