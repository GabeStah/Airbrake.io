# input_test_2.7.py
import sys


def main():
    try:
        print(sys.version)
        title = input("Enter a book title: ")
        author = input("Enter the book's author: ")
        print('The book you entered is \'' + title + '\' by ' + author + '.')
    except EOFError as error:
        # Output expected EOFErrors.
        print(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        print(exception, False)


if __name__ == "__main__":
    main()
