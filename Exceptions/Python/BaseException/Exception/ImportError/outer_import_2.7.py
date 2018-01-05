# outer_import_2.7.py
import sys
import gw_utility.Book


def main():
    try:
        print(sys.version)
    except ImportError as error:
        # Output expected ImportErrors.
        print(error.__class__.__name__ + ": " + error.message)
    except Exception as exception:
        # Output unexpected Exceptions.
        print(exception, False)
        print(exception.__class__.__name__ + ": " + exception.message)


if __name__ == "__main__":
    main()
