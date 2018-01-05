# inner_import_2.7.py
import sys


def main():
    try:
        print(sys.version)
        import gw_utility.Book
    except ImportError as error:
        # Output expected ImportErrors.
        print(error.__class__.__name__ + ": " + error.message)
    except Exception as exception:
        # Output unexpected Exceptions.
        print(exception, False)
        print(exception.__class__.__name__ + ": " + exception.message)


if __name__ == "__main__":
    main()
