# PHP Exception Handling - ErrorException

Moving along through our detailed [__PHP Exception Handling__](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) series, today we'll dive into the PHP ErrorException class.  In the most basic sense, the PHP ErrorException is meant to be explicitly thrown when you want to catch and handle errors that would otherwise be ignored, such as `Notices` or `Warnings`.

In this article we'll explore the `ErrorException` in a bit more detail by first looking at where it resides in the [PHP Exception Hierarchy](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy).  We'll also take a look at a number of different scenarios that combine various `error_handling` severity levels with custom error handler functions, to see how `ErrorExceptions` are typically caught and thrown, so let's get going!

## The Technical Rundown

- All PHP errors implement the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface, or are extended from another inherited class therein.
- [`Exception`](http://php.net/manual/en/class.exception.php) implements the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface.
- `ErrorException` extends the [`Exception`](http://php.net/manual/en/class.exception.php) class.

## When Should You Use It?

As mentioned in the introduction, the `ErrorException` class can be used when an exception should be created (and handled) as if it were a normal `Exception`-inherited class object.  One possible scenario is when PHP issues a `Warning`.  During execution, PHP will effectively ignore `Warnings` and continue execution as normal, depending on how the code is configured to handle errors.  However, this may not always be desirable, as oftentimes a `Warning` is an indication of a failure that should redirect current script execution, if not halt the script entirely (as a fatal error would normally do).

For example, consider trying to access a file via the [`file_get_contents(string $filename)`](http://php.net/manual/en/function.file-get-contents.php) function.  As indicated by the documentation, "an `E_WARNING` level error is generated if `filename` cannot be found, `maxlength` is less than zero, or if seeking to the specified `offset` in the stream fails."  The function itself also returns a `false` boolean value if reading the data/file failed for some reason.  Here we'll try to access an invalid file:

```php
file_get_contents("invalid.txt");
```

Which produces the following default output to the console (without halting execution):

```
Warning: file_get_contents(invalid.txt): failed to open stream: No such file or directory in D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php on line 93

Call Stack:
    0.0968     367640   1. {main}() D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:0
    0.1025    1784288   3. file_get_contents(???) D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:93
```

In this situation, we want to actually produce an `Exception`, as opposed to just a `Warning`, when the passed file path cannot be found.  Therefore, we need to make use of `ErrorException`.  Below is the full code sample.  Following this code we'll break down the functions and logic to see exactly what's going on and how `ErrorExceptions` can be used in various error handling scenarios:

```php
<?php

include("D:\work\Airbrake.io\lib\php\Logging.php");

/**
 * Access invalid file,
 * with error_reporting and set_error_handler calls.
 *
 * @param int $errorLevel Error level constant.
 * @param string|null $handlerFunc Error handler function to be used.
 */
function accessInvalidFile($errorLevel, $handlerFunc = null) {
    error_reporting($errorLevel);

    set_error_handler($handlerFunc);

    getFileContents("invalid.txt");
}

/**
 * Custom error handler.
 *
 * @param $severity
 * @param $message
 * @param $file
 * @param $line
 * @throws ErrorException
 */
function errorHandler($severity, $message, $file, $line) {
    throw new ErrorException($message, 0, $severity, $file, $line);
}

/**
 * Custom error handler, which checks if error
 * severity is handled by error_reporting.
 *
 * @param $severity
 * @param $message
 * @param $file
 * @param $line
 * @throws ErrorException
 */
function errorHandlerWithReportingCheck($severity, $message, $file, $line) {
    // Check if error number is handled by error_reporting setting.
    if (!(error_reporting() & $severity)) {
        return;
    }
    throw new ErrorException($message, 0, $severity, $file, $line);
}

/**
 * Executes examples.
 */
function executeExamples()
{
    Logging::LineSeparator("E_ERROR", 40, '=');
    accessInvalidFile(E_ERROR);

    Logging::LineSeparator("E_ERROR w/ ErrorHandler");
    accessInvalidFile(E_ERROR, 'errorHandler');

    Logging::LineSeparator("E_ERROR w/ ReportingCheck");
    accessInvalidFile(E_ERROR, 'errorHandlerWithReportingCheck');

    Logging::LineSeparator("E_WARNING", 40, '=');
    accessInvalidFile(E_WARNING);

    Logging::LineSeparator("E_WARNING w/ ErrorHandler");
    accessInvalidFile(E_WARNING, 'errorHandler');

    Logging::LineSeparator("E_WARNING w/ ReportingCheck");
    accessInvalidFile(E_WARNING, 'errorHandlerWithReportingCheck');

    Logging::LineSeparator("E_NOTICE", 40, '=');
    accessInvalidFile( E_NOTICE);

    Logging::LineSeparator("E_NOTICE w/ ErrorHandler");
    accessInvalidFile( E_NOTICE, 'errorHandler');

    Logging::LineSeparator("E_NOTICE w/ ReportingCheck");
    accessInvalidFile( E_NOTICE, 'errorHandlerWithReportingCheck');
}

/**
 * Get contents of file via file_get_contents().
 *
 * @param string $path File path.
 * @return bool|string Retrieved file contents.
 */
function getFileContents(string $path) {
    try {
        // Attempt to get file contents.
        return file_get_contents($path);
    } catch (ErrorException $exception) {
        // Catch expected ErrorExceptions.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Catch unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

executeExamples();

// Logging.php
require('kint.php');

/**
 * Provides basic logging/output functionality.
 */
class Logging {

    /**
     * Logs the passed Throwable object.  
     * Includes message, className if error was expected, and stack trace.
     *
     * Uses internal Reflection to retrieve protected/private properties.
     *
     * @param Throwable $throwable Throwable object to be output.
     * @param bool $expected Indicates if error was expected or not.
     */
    private static function LogThrowable(Throwable $throwable, bool $expected = true) {
        $expected = $expected ? "EXPECTED" : "UNEXPECTED";
        $message = substr($throwable->xdebug_message, 1);
        // Output whether error was expected or not, the class name, the message, and stack trace.
        print_r("[{$expected}] {$message}\n");
        // Add line separator to keep it tidy.
        self::LineSeparator();
    }

    /**
     * Outputs a dashed line separator with
     * inserted text centered in the middle.
     *
     * @param array ...$args Insert, length, and separator character.
     */
    public static function LineSeparator(...$args) {
        $insert = empty($args[0]) ? "" : $args[0];
        $length = empty($args[1]) ? 40 : $args[1];
        $separator = empty($args[2]) ? '-' : $args[2];

        $output = $insert;

        if (strlen($insert) == 0) {
            $output = str_repeat($separator, $length);
        } elseif (strlen($insert) < $length) {
            // Update length based on insert length, less a space for margin.
            $length -= (strlen($insert) + 2);
            // Halve the length and floor left side.
            $left = floor($length / 2);
            $right = $left;
            // If odd number, add dropped remainder to right side.
            if ($length % 2 != 0) $right += 1;

            // Create separator strings.
            $left = str_repeat($separator, $left);
            $right = str_repeat($separator, $right);

            // Surround insert with separators.
            $output = "{$left} {$insert} {$right}";
        }

        print_r("{$output}\n");
    }
}
```

---

For these examples we'll stick with the `file_get_contents()` method.  We want to react to the `Warnings` it produces by throwing and catching an `ErrorException`, when appropriate.  So, we begin with the `getFileContents(string $path)` function, which is a simple wrapper for `file_get_contents()` and some basic error handling:

```php
/**
 * Get contents of file via file_get_contents().
 *
 * @param string $path File path.
 * @return bool|string Retrieved file contents.
 */
function getFileContents(string $path) {
    try {
        // Attempt to get file contents.
        return file_get_contents($path);
    } catch (ErrorException $exception) {
        // Catch expected ErrorExceptions.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Catch unexpected Exceptions.
        Logging::Log($exception, false);
    }
}
```

We're going to make use of the built-in [`set_error_handler()`](http://php.net/manual/en/function.set-error-handler.php) function, which allows us to set a user-defined error handling function.  We've specified two different error handler functions to serve this purpose -- one that checks if the current [`error_reporting`](http://php.net/manual/en/function.error-reporting.php) level allows the passed error to be handled, and the other, which ignores the `error_reporting` configuration entirely.  In both cases, we ensure that our error handler throws a new `ErrorException` and passes the original values to it, so we can use this in the `try-catch` blocks elsewhere in the code:

```php
/**
 * Custom error handler.
 *
 * @param $severity
 * @param $message
 * @param $file
 * @param $line
 * @throws ErrorException
 */
function errorHandler($severity, $message, $file, $line) {
    throw new ErrorException($message, 0, $severity, $file, $line);
}

/**
 * Custom error handler, which checks if error
 * severity is handled by error_reporting.
 *
 * @param $severity
 * @param $message
 * @param $file
 * @param $line
 * @throws ErrorException
 */
function errorHandlerWithReportingCheck($severity, $message, $file, $line) {
    // Check if error number is handled by error_reporting setting.
    if (!(error_reporting() & $severity)) {
        return;
    }
    throw new ErrorException($message, 0, $severity, $file, $line);
}
```

We then combine these calls within the `accessInvalidFile($errorLevel, $handlerFunc = null)` method, which configures the `error_handling` level and sets the error handler function (if applicable), before calling `getFileContents(string $path)` with an invalid file name passed in:

```php
/**
 * Access invalid file,
 * with error_reporting and set_error_handler calls.
 *
 * @param int $errorLevel Error level constant.
 * @param string|null $handlerFunc Error handler function to be used.
 */
function accessInvalidFile($errorLevel, $handlerFunc = null) {
    error_reporting($errorLevel);

    set_error_handler($handlerFunc);

    getFileContents("invalid.txt");
}
```

With everything setup we can call `accessInvalidFile()` with assorted error reporting levels and handler functions passed to it, and see how changes here alter the behavior of our invalid file retrieval attempt:

```php
/**
 * Executes examples.
 */
function executeExamples()
{
    Logging::LineSeparator("E_ERROR", 40, '=');
    accessInvalidFile(E_ERROR);

    Logging::LineSeparator("E_ERROR w/ ErrorHandler");
    accessInvalidFile(E_ERROR, 'errorHandler');

    Logging::LineSeparator("E_ERROR w/ ReportingCheck");
    accessInvalidFile(E_ERROR, 'errorHandlerWithReportingCheck');

    Logging::LineSeparator("E_WARNING", 40, '=');
    accessInvalidFile(E_WARNING);

    Logging::LineSeparator("E_WARNING w/ ErrorHandler");
    accessInvalidFile(E_WARNING, 'errorHandler');

    Logging::LineSeparator("E_WARNING w/ ReportingCheck");
    accessInvalidFile(E_WARNING, 'errorHandlerWithReportingCheck');

    Logging::LineSeparator("E_NOTICE", 40, '=');
    accessInvalidFile(E_NOTICE);

    Logging::LineSeparator("E_NOTICE w/ ErrorHandler");
    accessInvalidFile(E_NOTICE, 'errorHandler');

    Logging::LineSeparator("E_NOTICE w/ ReportingCheck");
    accessInvalidFile(E_NOTICE, 'errorHandlerWithReportingCheck');
}
```

As you can see, we're calling `accessInvalidFile()` for each of the three most common `error_reporting` levels: `E_ERROR`, `E_WARNING`, and `E_NOTICE`.  Within each error reporting level we also make a call with no error handler, a call with a basic error handler, and then a call with the advanced handler that checks against `error_reporting` severity levels.

The goal here is to see how these different configurations affect the output that PHP produces when a `Warning` occurs.  We begin with the output set using an `E_ERROR` reporting level:

```
=============== E_ERROR ================
------- E_ERROR w/ ErrorHandler --------
[EXPECTED] ErrorException: file_get_contents(invalid.txt): failed to open stream: No such file or directory in D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php on line 93

Call Stack:
    0.0946     367456   1. {main}() D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:0
    0.1019    1784104   2. executeExamples() D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:103
    0.1021    1784280   3. accessInvalidFile(???, ???) D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:60
    0.1021    1784280   4. getFileContents(???) D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:17
    0.1021    1784280   5. file_get_contents(???) D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:93
    0.1021    1785480   6. errorHandler(???, ???, ???, ???, ???) D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:93

----------------------------------------
------ E_ERROR w/ ReportingCheck -------
```

The first thing to notice is that, without an error handler function specified, no output is produced.  This makes sense, since we're not including the `E_WARNING` reporting level flag, so the `Warning` that is created is ignored.  

On the other hand, the basic error handler _does_ grab the `Warning` and convert it into a thrown `ErrorException`, which we're then able to catch.  However, since the advanced error handler checks if `error_reporting` is configured to handle the `E_WARNING` severity level of the passed error, no `ErrorException` is thrown during the final call.

Next let's look at the `E_WARNING` outputs:

```
============== E_WARNING ===============
Warning: file_get_contents(invalid.txt): failed to open stream: No such file or directory in D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php on line 93

Call Stack:
    0.0946     367456   1. {main}() D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:0
    0.1019    1784104   2. executeExamples() D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:103
    0.1023    1784976   3. accessInvalidFile(???, ???) D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:66
    0.1023    1784976   4. getFileContents(???) D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:17
    0.1023    1784976   5. file_get_contents(???) D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:93

------ E_WARNING w/ ErrorHandler -------
[EXPECTED] ErrorException: file_get_contents(invalid.txt): failed to open stream: No such file or directory in D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php on line 93

Call Stack:
    0.0946     367456   1. {main}() D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:0
    0.1019    1784104   2. executeExamples() D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:103
    0.1024    1784976   3. accessInvalidFile(???, ???) D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:69
    0.1024    1784976   4. getFileContents(???) D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:17
    0.1024    1784976   5. file_get_contents(???) D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:93
    0.1025    1785800   6. errorHandler(???, ???, ???, ???, ???) D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:93

----------------------------------------
----- E_WARNING w/ ReportingCheck ------
[EXPECTED] ErrorException: file_get_contents(invalid.txt): failed to open stream: No such file or directory in D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php on line 93

Call Stack:
    0.0946     367456   1. {main}() D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:0
    0.1019    1784104   2. executeExamples() D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:103
    0.1025    1784976   3. accessInvalidFile(???, ???) D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:72
    0.1025    1784976   4. getFileContents(???) D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:17
    0.1025    1784976   5. file_get_contents(???) D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:93
    0.1026    1785800   6. errorHandlerWithReportingCheck(???, ???, ???, ???, ???) D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:93

----------------------------------------
```

It shouldn't come as much surprise that all three of the `E_WARNING` reporting level calls produce an output.  Since the underlying error is itself a `Warning`, the call without an error handler just passes along that `Warning` to the console.  Meanwhile, the basic error handler works as we saw before, while the advanced handler confirms that the `E_WARNING` flag passed to `error_reporting` is equivalent to the error's severity level, so it also throws an exception.

Finally, we have the `E_NOTICE` call outputs:

```
=============== E_NOTICE ===============
------- E_NOTICE w/ ErrorHandler -------
[EXPECTED] ErrorException: file_get_contents(invalid.txt): failed to open stream: No such file or directory in D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php on line 93

Call Stack:
    0.0946     367456   1. {main}() D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:0
    0.1019    1784104   2. executeExamples() D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:103
    0.1027    1784976   3. accessInvalidFile(???, ???) D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:78
    0.1027    1784976   4. getFileContents(???) D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:17
    0.1027    1784976   5. file_get_contents(???) D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:93
    0.1027    1785800   6. errorHandler(???, ???, ???, ???, ???) D:\work\Airbrake.io\Exceptions\PHP\Exception\ErrorException\code.php:93

----------------------------------------
------ E_NOTICE w/ ReportingCheck ------
```

These are identical to the outputs from `E_ERROR` calls, since the logic is the same: The `Warning` doesn't qualify as an `E_NOTICE`, so there's no match for the advanced error handler, nor the non-error handler call.

Check out the <a class="js-cta-utm" href="https://airbrake.io/languages/php_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-php">Airbrake-PHP library</a>, designed to quickly and easily integrate into any PHP project, giving you and your team access to real-time error monitoring and reporting throughout your application's entire life cycle.  With automatic, instantaneous error and exception notifications at your fingertips, you'll be constantly aware of your application's health, including any issues that may arise.  Best of all, with Airbrake's robust web dashboard cataloging every error that occurs, you and your team can immediately dive into the exact details of what went wrong, making it easy to quickly recognize and resolve problems.

---

__META DESCRIPTION__

A close look at the PHP ErrorException class, including code samples illustrating how various error handling configurations alter reporting behavior.

---

__SOURCES__

- http://php.net/manual/en/class.throwable.php