# The PHP Exception Class Hierarchy

In the past, understanding the PHP exception class hierarchy was easy, but the hierarchy included a somewhat serious flaw.  In older PHP versions it often proved difficult, if not impossible, to handle fatal errors.  In fact, most fatal errors typically caused the application to halt execution entirely.  Now, thanks to changes introduced in PHP 7, exceptions are thrown (and can therefore be caught) when a fatal error occurs, allowing the application to continue functioning.

As of PHP 7, PHP divides errors into two unique classes: `Exception` and `Error`.  An `Error` is typically used for issues that have historically been considered fatal errors.  When a fatal error occurs, PHP will now throw an `Error` class instance.  An `Exception` instance, on the other hand, is thrown for more traditional, recoverable errors.

To bring these two concepts together PHP 7 introduces the new `Throwable` interface, which both `Exception` and `Error` implement.  Here's a look at the entire PHP exception hierarchy:

- Throwable
    - Error
        - [ArithmeticError](https://airbrake.io/blog/php-exception-handling/arithmeticerror)
            - [DivisionByZeroError](https://airbrake.io/blog/php-exception-handling/divisionbyzeroerror)
        - [AssertionError](https://airbrake.io/blog/php-exception-handling/assertionerror)
        - [ParseError](https://airbrake.io/blog/php-exception-handling/php-parseerror)
        - [TypeError](https://airbrake.io/blog/php-exception-handling/php-typeerror)
    - Exception
        - [ClosedGeneratorException](https://airbrake.io/blog/php-exception-handling/closedgeneratorexception)
        - [DOMException](https://airbrake.io/blog/php-exception-handling/domexception)
        - [ErrorException](https://airbrake.io/blog/php-exception-handling/errorexception)
        - IntlException
        - LogicException
            - BadFunctionCallException
                - BadMethodCallException
            - DomainException
            - InvalidArgumentException
            - LengthException
            - OutOfRangeException
        - PharException
        - ReflectionException
            - RuntimeException
            - mysqli_sql_exception
            - OutOfBoundsException
            - OverflowException
            - PDOException
            - RangeException
            - UnderflowException
            - UnexpectedValueException

Below we'll briefly discuss each top-level exception type, providing a rough overview, which we'll expand upon in much more detail throughout further articles.

As previously discussed, `Errors` encompass issues that would normally be considered fatal.  Such errors are considered `internal` PHP errors.

- `ArithmeticError` - Thrown when attempting invalid mathematical operations, such as performing a negative bitshift, or trying to get a result outside the bounds of `integer`.
- `AssertionError` - Thrown when an assertion made via `assert()` fails.
- `ParseError` - Thrown when an invalid parse attempt is made, such as with the `eval()` function.
- `TypeError` - Thrown when provided argument or return value types do not match the declared type that is expected.

`Exceptions` encompass all user exceptions in PHP -- anything that isn't an internal `Error` is considered an `Exception`.

- `ClosedGeneratorException` - Thrown when trying to request another value from a generator that has no more values to provide, and therefore has been finalized.
- `DOMException` - Thrown when something goes wrong with XML-style document manipulation.
- `ErrorException` - Used to translate from an `Error` exception to an `Exception` exception.
- `IntlException` - Thrown when there's an issue performing internationalization logic.
- `LogicException` - Thrown when faulty programmatic logic is executed.
- `PharException` - Thrown when an issue occurs while manipulating single-file PHP application file archives, typically referred to as `phars`.
- `ReflectionException` - Thrown when attempting to perform an invalid operation during reflection.
- `RuntimeException` - Thrown for exceptions that only occur during runtime, such as overflow or out of bounds issues.

That's just a small taste of the powerful, built-in exception class hierarchy provided with modern PHP.  Stay tuned for more in-depth articles examining each of these exceptions in greater detail, and be sure to check out Airbrake's robust <a class="js-cta-utm" href="https://airbrake.io/languages/ruby_exception_handling?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-php">PHP Exception Tracker</a>, designed to help you streamline all your PHP exception handling practices!

---

__META DESCRIPTION__

A brief overview of the PHP exception class hierarchy, including the dramatic changes introduced in PHP 7 with the new Throwable interface.

---

__SOURCES__

- https://trowski.com/2015/06/24/throwable-exceptions-and-errors-in-php7/
- https://gist.github.com/mlocati/249f07b074a0de339d4d1ca980848e6a
- https://3v4l.org/sDMsv
- http://php.net/manual/en/reserved.exceptions.php