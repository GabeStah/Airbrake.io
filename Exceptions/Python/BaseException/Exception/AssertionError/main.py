import datetime

from gw_utility.book import Book
from gw_utility.logging import Logging


def main():
    Logging.line_separator("BOTH INCLUDE PUBLICATION DATES", 50, '+')
    # Create two Books with identical arguments.
    the_stand = Book("The Stand", "Stephen King", 1153, datetime.date(1978, 1, 1))
    the_stand_2 = Book("The Stand", "Stephen King", 1153, datetime.date(1978, 1, 1))

    # Check equivalency of Books.
    check_equality(the_stand, the_stand_2)

    Logging.line_separator("ONE MISSING PUBLICATION DATE", 50, '+')
    # Create two Books, one without publication_date argument specified.
    the_hobbit = Book("The Hobbit", "J.R.R. Tolkien", 366, datetime.date(1937, 9, 15))
    the_hobbit_2 = Book("The Hobbit", "J.R.R. Tolkien", 366)

    # Check equivalency of Books.
    check_equality(the_hobbit, the_hobbit_2)


def check_equality(a, b):
    """Asserts the equivalent of the two passed objects.

    :param a: First object.
    :param b: Second object.
    :return: Indicates if assertion was successful.
    """
    try:
        Logging.line_separator("ASSERTING EQUIVALENCE OF...")
        # Output objects using __str__ method.
        Logging.log(a)
        Logging.log(b)
        # Assert equivalence of objects, indicating inequality if failed.
        assert a == b, "The objects ARE NOT equal."
        # Indicate that assertion succeeded.
        Logging.log("The objects are equal.")
        return True
    except AssertionError as error:
        # Output expected AssertionErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


if __name__ == "__main__":
    main()
