# eval_syntax_test.py
import datetime
from gw_utility.logging import Logging

TOMORROW_DEFINITION = """
f'Tomorrow is {(today + datetime.timedelta(days=1)).strftime("%A, %B %d, %Y")}'
"""

DAY_AFTER_TOMORROW_DEFINITION = """
f'The day after tomorrow is {(today + datetime.timedelta(days=2))strftime("%A, %B %d, %Y")}'
"""


def main():
    try:
        today = datetime.datetime.now()
        Logging.log(f'Today is {today.strftime("%A, %B %d, %Y")}')

        Logging.log("Invoking: eval(TOMORROW_DEFINITION)")
        Logging.log(eval(TOMORROW_DEFINITION))

        Logging.log("Invoking: eval(DAY_AFTER_TOMORROW_DEFINITION)")
        Logging.log(eval(DAY_AFTER_TOMORROW_DEFINITION))
    except SyntaxError as error:
        # Output expected SyntaxErrors.
        Logging.log_exception(error)
    except Exception as exception:
        # Output unexpected Exceptions.
        Logging.log_exception(exception, False)


if __name__ == "__main__":
    main()
