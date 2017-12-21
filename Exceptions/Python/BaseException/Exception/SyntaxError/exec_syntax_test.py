# exec_syntax_test.py
from gw_utility.logging import Logging

DOUBLE_DEFINITION = """
def double(x):
    return x * 2
"""

DOUBLE_EXECUTOR = """
Logging.log(double(5))
"""

TRIPLE_DEFINITION = """
def triple(x):
    return x * 3
"""

TRIPLE_EXECUTOR = """
Logging.log(triple(5)
"""


def main():
    try:
        Logging.log("Invoking: exec(DOUBLE_DEFINITION)")
        exec(DOUBLE_DEFINITION)
        Logging.log("Invoking: exec(DOUBLE_EXECUTOR)")
        exec(DOUBLE_EXECUTOR)

        Logging.log("Invoking: exec(TRIPLE_DEFINITION)")
        exec(TRIPLE_DEFINITION)
        Logging.log("Invoking: exec(TRIPLE_EXECUTOR)")
        exec(TRIPLE_EXECUTOR)
    except SyntaxError as error:
        # Output expected SyntaxErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


if __name__ == "__main__":
    main()
