# main.py
import datetime

from gw_utility.book import Book
from gw_utility.logging import Logging


def main():
    try:
        # Create a dictionary and populate with Books.
        series = {
            1:  Book("The Name of the Wind",    "Patrick Rothfuss",     662,    datetime.date(2007, 3, 27)),
            2:  Book("The Wise Man's Fear",     "Patrick Rothfuss",     994,    datetime.date(2011, 3, 1)),
            3:  Book("Doors of Stone",          "Patrick Rothfuss")
        }

        # Output Books in series dictionary, with and without index.
        Logging.line_separator('Series')
        log_dict(series)
        Logging.line_separator('Series w/ Order Index')
        log_dict(series, True)

        # Output book in series that doesn't exist.
        Logging.line_separator('series[len(series) + 1]')
        Logging.log(f'series[{len(series) + 1}]: {series[len(series) + 1]}')
    except KeyError as error:
        # Output expected KeyErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


def log_dict(collection, include_key=False):
    """Logs the each element in collection to the console.

    :param collection: Collection to be iterated and output.
    :param include_key: Determines if key should be output.
    :return: None
    """
    try:
        # Iterate by getting collection of items.
        for key, item in collection.items():
            if include_key:
                Logging.log(f'collection[{key}]: {item}')
            else:
                Logging.log(item)
    except KeyError as error:
        # Output expected KeyErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


if __name__ == "__main__":
    main()
