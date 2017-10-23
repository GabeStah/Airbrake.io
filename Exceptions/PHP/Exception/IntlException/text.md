# PHP Exception Handling - IntlException

As we approach the end of our detailed [__PHP Exception Handling__](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) series, today we'll take a closer look at the **IntlException**.   The `IntlException` is thrown by the wide variety of classes, functions, and methods found within the [`Internationalization Functions`](http://php.net/manual/en/book.intl.php) API set.  These functions are are a PHP-based wrapper for the [International Components for Unicode](http://site.icu-project.org/) (`ICU`) library set, allowing code to easily work with strings, numbers, and dates across a variety of locales and formats.

Throughout this article we'll examine the `IntlException` by first looking at where it resides in the overall [PHP Exception Hierarchy](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy).  We'll also explore some fully functional code samples that will illustrate how many of the common `Intl` classes can be used, and how doing so may cause `IntlExceptions` to be thrown, so let's get started!

## The Technical Rundown

All PHP errors implement the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy)
    - [`Exception`](http://php.net/manual/en/class.exception.php)
        - `IntlException`

## Full Code Sample

Below is the full code sample we’ll be using in this article.  Feel free to use any or all of the code if you wish to follow along.

```php
<?php

include("d:\work\Airbrake.io\lib\php\Logging.php");

function executeExamples()
{
    Logging::LineSeparator('FORMAT DATE');
    formatDate(new DateTime());
    Logging::LineSeparator('FORMAT INVALID DATE');
    formatDate(null);

    Logging::LineSeparator('FORMAT NUMBER');
    formatNumber(123.456);
    Logging::LineSeparator('FORMAT INVALID NUMBER');
    formatNumber(123.456, 'en_US', 24601);

    Logging::LineSeparator('FORMAT CURRENCY');
    formatCurrency(123.456);
    Logging::LineSeparator('FORMAT INVALID CURRENCY');
    formatCurrency(123.456, 'ABCDE');

    Logging::LineSeparator('FORMAT MESSAGE');
    formatMessage(array('This is a message!'));
    //formatMessage(null);
    Logging::LineSeparator('FORMAT INVALID MESSAGE');
    //formatMessage(null);
    formatMessage(array('This is a message!'), 'en_US', null);
}

/**
 * Format currency value.
 *
 * @param mixed $value Currency value.
 * @param null|string $currency Currency type.
 * @param null|string $locale Locale.
 * @param int|null $style Number formatter style.
 */
function formatCurrency($value,
                        ?string $currency = 'USD',
                        ?string $locale = 'en_US',
                        ?int $style = NumberFormatter::CURRENCY)
{
    try {
        $formatter = new NumberFormatter($locale, $style);
        // Attempt format.
        Logging::Log($formatter->formatCurrency($value, $currency));
        // Manually throw exception, if necessary.
        throwFormatterException($formatter);
    } catch (IntlException $exception) {
        // Output expected IntlExceptions.
        Logging::Log($exception);
    } catch (Error | Exception $error) {
        // Output unexpected Errors and Exceptions.
        Logging::Log($error, false);
    }
}

/**
 * Format date value.
 *
 * @param mixed $value Date value.
 * @param null|string $locale Locale.
 * @param null|string $timezone Timezone.
 * @param int|null $dateType Date type.
 * @param int|null $timeType Time type.
 * @param int|null $calendarType Calendar type.
 */
function formatDate($value,
                    ?string $locale = 'en_US',
                    ?string $timezone = 'America/Los_Angeles',
                    ?int $dateType = IntlDateFormatter::FULL,
                    ?int $timeType = IntlDateFormatter::FULL,
                    ?int $calendarType = IntlDateFormatter::GREGORIAN)
{
    try {
        $formatter = new IntlDateFormatter($locale, $dateType, $timeType, $timezone, $calendarType);
        // Attempt format.
        Logging::Log($formatter->format($value));
        // Manually throw exception, if necessary.
        throwFormatterException($formatter);
    } catch (IntlException $exception) {
        // Output expected IntlExceptions.
        Logging::Log($exception);
    } catch (Error | Exception $error) {
        // Output unexpected Errors and Exceptions.
        Logging::Log($error, false);
    }
}

/**
 * Format message value.
 *
 * @param array $value Message value.
 * @param null|string $locale Locale.
 * @param null|string $pattern Message formatting pattern.
 */
function formatMessage(array $value, ?string $locale = 'en_US', ?string $pattern = '{0}') {
    try {
        $formatter = new MessageFormatter($locale, $pattern);
        // Attempt format.
        Logging::Log($formatter->format($value));
        // Manually throw exception, if necessary.
        throwFormatterException($formatter);
    } catch (IntlException $exception) {
        // Output expected IntlExceptions.
        Logging::Log($exception);
    } catch (Error | Exception $error) {
        // Output unexpected Errors and Exceptions.
        Logging::Log($error, false);
    }
}

/**
 * Format number value.
 *
 * @param mixed $value Number value.
 * @param null|string $locale Locale.
 * @param int|null $style Number formatter style.
 */
function formatNumber($value, ?string $locale = 'en_US', ?int $style = NumberFormatter::DECIMAL) {
    try {
        $formatter = new NumberFormatter($locale, $style);
        // Attempt format.
        Logging::Log($formatter->format($value));
        // Manually throw exception, if necessary.
        throwFormatterException($formatter);
    } catch (IntlException $exception) {
        // Output expected IntlExceptions.
        Logging::Log($exception);
    } catch (Error | Exception $error) {
        // Output unexpected Errors and Exceptions.
        Logging::Log($error, false);
    }
}

/**
 * Determine if passed object is valid formatter instance.
 *
 * @param object $formatter Formatter to check.
 * @return bool
 */
function isFormatter($formatter) {
    foreach (array('IntlDateFormatter', 'MessageFormatter', 'NumberFormatter') as $class) {
        if ($formatter instanceof $class) return true;
    }
}

/**
 * Throws a new IntlException, if intl.use_exceptions setting
 * disabled, passed formatter is valid type, and error was produced.
 *
 * @param object $formatter Formatter to retrieve error from.
 * @throws IntlException
 */
function throwFormatterException($formatter) {
    // Ensure object is valid formatter.
    if (!isFormatter($formatter)) return;
    // Confirm that use_exceptions setting is disabled.
    if (ini_get('intl.use_exceptions') == 0) {
        $errorCode = $formatter->getErrorCode();
        // Check for failure.
        if (intl_is_failure($errorCode)) {
            Logging::Log("Formatter failed with error code: {$errorCode}.  Throwing exception...");
            throw new IntlException($formatter->getErrorMessage(), $errorCode);
        }
    }
}

executeExamples();
```

```php
<?php
// Logging.php
require('kint.php');

/**
 * Provides basic logging/output functionality.
 */
class Logging {

    /**
     * Logs the passed object, string, or Throwable instance to the console.
     *
     * @param object|string $a Message or value to be logged.
     * @param object|bool $b Secondary value, such as boolean for Throwables indicating if error was expected.
     */
    public static function Log($a, $b = null) {
        if (is_string($a) || is_numeric($a)) {
            Logging::LogString($a);
        } elseif ($a instanceof Throwable) {
            Logging::LogThrowable($a, is_null($b) ? true : $b);
        } else {
            Logging::LogObject($a);
        }
    }

    /**
     * Logs the passed object.
     *
     * @param mixed $object Object to be logged.
     *
     * @see https://github.com/kint-php/kint    Kint tool used for structured outputs.
     */
    private static function LogObject($object) {
        Kint_Renderer_Cli::$force_utf8 = true;
        Kint_Renderer_Text::$decorations = false;
        Kint::dump($object);
    }

    /**
     * Logs the passed string value.
     *
     * @param string $value Value to be logged.
     */
    private static function LogString(string $value) {
        print_r("{$value}\n");
    }

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

## When Should You Use It?

As mentioned, the purpose of the `Intl` class in PHP is to make it easy to work with values that are typically troublesome to convert to the appropriate locale-based format.  As a simple example, the United States uses a period (`.`) as a decimal separator (e.g. `123.456`), while France uses a comma (`,`) decimal separator (e.g. `123,456`).  It would be a monumental task to code localization handling for every project, so the `Intl` class, using the `ICU` library, standardizes localization techniques and makes it relatively easy to format number, date, and string values into the multitude of localized contexts you might need.

However, as with most API functions, the `Intl` class can sometimes run into problems, which can manifest themselves into thrown `IntlExceptions`.  Since the `Intl` class is a built-in PHP `extension`, the first thing you'll need to do is make sure the extension is enabled in your own `php.ini` file.  Typically, this will involve opening your `php.ini` file, searching for `php_intl`, and uncommenting the appropriate line, like so:

```
; ...

extension=php_intl.dll

; ...
```

Now, to test these classes out we're going to manipulate some common types of values, including currencies, dates, numbers, and strings.  To accomplish this we're using the built-in formatter classes including [`IntlDateFormatter`](http://php.net/manual/en/class.intldateformatter.php), [`NumberFormatter`](http://php.net/manual/en/class.numberformatter.php), and [`MessageFormatter`](http://php.net/manual/en/class.messageformatter.php).  Let's begin with a date via our `formatDate(...)` function:

```php
/**
 * Format date value.
 *
 * @param mixed $value Date value.
 * @param null|string $locale Locale.
 * @param null|string $timezone Timezone.
 * @param int|null $dateType Date type.
 * @param int|null $timeType Time type.
 * @param int|null $calendarType Calendar type.
 */
function formatDate($value,
                    ?string $locale = 'en_US',
                    ?string $timezone = 'America/Los_Angeles',
                    ?int $dateType = IntlDateFormatter::FULL,
                    ?int $timeType = IntlDateFormatter::FULL,
                    ?int $calendarType = IntlDateFormatter::GREGORIAN)
{
    try {
        $formatter = new IntlDateFormatter($locale, $dateType, $timeType, $timezone, $calendarType);
        // Attempt format.
        Logging::Log($formatter->format($value));
        // Manually throw exception, if necessary.
        throwFormatterException($formatter);
    } catch (IntlException $exception) {
        // Output expected IntlExceptions.
        Logging::Log($exception);
    } catch (Error | Exception $error) {
        // Output unexpected Errors and Exceptions.
        Logging::Log($error, false);
    }
}
```

As you can see, this function primarily acts as a small wrapper for the `IntlDateFormatter` class constructor method, accepting a number of arguments and passing those parameters to the `IntlDateFormatter` constructor.  We then call the `$formatter->format($value)` method with our passed `$value` parameter as an argument, which actually attempts to perform the format based on all the arguments used during construction.  We log the result to the console.

Now, you'll notice a call to the `throwFormatterException($exception)` function at the end of the `try` block, which is particularly important.  Since the `Intl` classes are from an extension, if these methods or classes have an issue they default to issuing a basic `E_WARNING` message, but otherwise try to use the default settings and locale to process the formatted value, so execution can continue.  To force these classes to actually throw a catchable `IntlException` you'll need to explicitly enable the `intl.use_exceptions` setting in the `php.ini` file.  If this setting is disabled (which is the default value), only warnings will be generated.  _However_, the `Intl` class provides a number of helper functions to check if a given formatter instance caused an error, even if it wasn't an explicitly thrown/caught `IntlException` instance.  To illustrate, let's finally take a look at the aforementioned `throwFormatterException($exception)` method:

```php
/**
 * Throws a new IntlException, if intl.use_exceptions setting
 * disabled, passed formatter is valid type, and error was produced.
 *
 * @param object $formatter Formatter to retrieve error from.
 * @throws IntlException
 */
function throwFormatterException($formatter) {
    // Ensure object is valid formatter.
    if (!isFormatter($formatter)) return;
    // Confirm that use_exceptions setting is disabled.
    if (ini_get('intl.use_exceptions') == 0) {
        $errorCode = $formatter->getErrorCode();
        // Check for failure.
        if (intl_is_failure($errorCode)) {
            Logging::Log("Formatter failed with error code: {$errorCode}.  Throwing exception...");
            throw new IntlException($formatter->getErrorMessage(), $errorCode);
        }
    }
}

/**
 * Determine if passed object is valid formatter instance.
 *
 * @param object $formatter Formatter to check.
 * @return bool
 */
function isFormatter($formatter) {
    foreach (array('IntlDateFormatter', 'MessageFormatter', 'NumberFormatter') as $class) {
        if ($formatter instanceof $class) return true;
    }
}
```

The purpose of this function is to determine if the passed `$formatter` object is actually a proper `Formatter` class type and, if so, determine if the `intl.use_exceptions` `php.ini` setting is disabled (the default setting).  If `intl.use_exceptions` is disabled, we then check if the passed `$formatter` actually produced an error of some sort, which is determined by passing the `$formatter->getErrorCode()` method result to the [`intl_is_failure()`](http://php.net/manual/en/function.intl-is-failure.php) function.  This function returns a boolean indicating if the passed error code indicates an error or not.  If a failure is detected, we output the error code to the log and then _manually_ throw our own `IntlException` with the appropriate message and error code values passed in.

With this knowledge in hand, we can test out our `formatDate(...)`:

```php
function executeExamples()
{
    $number = 1234.567;
    $message = 'This is a message!';

    Logging::LineSeparator('FORMAT DATE');
    formatDate(new DateTime());
    Logging::LineSeparator('FORMAT INVALID DATE');
    formatDate(null);

    // ...
}
```

Here we're first calling `formatDate(...)` with the current date and time, then calling it a second time with a `null` date value specified.  Here we see the output of these calls:

```
------------- FORMAT DATE --------------
Monday, October 23, 2017 at 11:10:34 AM Pacific Daylight Time
--------- FORMAT INVALID DATE ----------
Formatter failed with error code: 1.  Throwing exception...
[EXPECTED] IntlException: datefmt_format: invalid PHP type for date: U_ILLEGAL_ARGUMENT_ERROR in D:\work\Airbrake.io\Exceptions\PHP\Exception\IntlException\code.php on line 169
```

Unsurprisingly, the first call works fine and outputs our date and time.  Meanwhile, the second call shows that execution called the `format(...)` method, but didn't throw an error since `intl.use_exceptions` is disabled by default.  However, we passed the formatter to `throwFormatterException($formatter)` and this determined that the formatter _did_ actually run into trouble, so we output the error code and manually threw an `IntlException` with the actual error message.  In this case, we can see that the `Intl` library calls the `datefmt_format(...)` function behind the scenes, which received an invalid data type for the date since we passed `null` in this second call.  Cool!

Next up, let's look at our `formatNumber(...)` function:

```php
/**
 * Format number value.
 *
 * @param mixed $value Number value.
 * @param null|string $locale Locale.
 * @param int|null $style Number formatter style.
 */
function formatNumber($value, ?string $locale = 'en_US', ?int $style = NumberFormatter::DECIMAL) {
    try {
        $formatter = new NumberFormatter($locale, $style);
        // Attempt format.
        Logging::Log($formatter->format($value));
        // Manually throw exception, if necessary.
        throwFormatterException($formatter);
    } catch (IntlException $exception) {
        // Output expected IntlExceptions.
        Logging::Log($exception);
    } catch (Error | Exception $error) {
        // Output unexpected Errors and Exceptions.
        Logging::Log($error, false);
    }
}
```

The format and execution of this function is the same as we saw in `formatDate(...)`, so we won't explain anything.  Instead, let's test it out:

```php
Logging::LineSeparator('FORMAT NUMBER (en_US)');
formatNumber($number);
Logging::LineSeparator('FORMAT NUMBER (fr_FR)');
formatNumber($number, 'fr_FR');
Logging::LineSeparator('FORMAT NUMBER (de_CH)');
formatNumber($number, 'de_CH');
Logging::LineSeparator('FORMAT INVALID NUMBER');
formatNumber($number, 'en_US', 24601);
```

Here's the output we produce from these four calls:

```
-------- FORMAT NUMBER (en_US) ---------
1,234.567
-------- FORMAT NUMBER (fr_FR) ---------
1 234,567
-------- FORMAT NUMBER (de_CH) ---------
1'234.567
-------- FORMAT INVALID NUMBER ---------
[EXPECTED] IntlException: Constructor failed in D:\work\Airbrake.io\Exceptions\PHP\Exception\IntlException\code.php on line 127
```

As previously mentioned, by using the `ICU` library we're able to automatically convert our value of `1234.567` to the appropriate formats based on the `locale` value we pass to each call.  France uses the [`SI`](https://en.wikipedia.org/wiki/International_System_of_Units) format, while Switzerland uses apostrophes as thousands separators and periods for a decimal separator.  In our last call we passed an invalid `$style` argument value to the underlying `NumberFormatter` constructor, so a legit `IntlException` is thrown and caught.

Now we have the `formatCurrency(...)` method, which does just what `formatNumber(...)` did, except ideally for currencies:

```php
/**
 * Format currency value.
 *
 * @param mixed $value Currency value.
 * @param null|string $currency Currency type.
 * @param null|string $locale Locale.
 * @param int|null $style Number formatter style.
 */
function formatCurrency($value,
                        ?string $currency = 'USD',
                        ?string $locale = 'en_US',
                        ?int $style = NumberFormatter::CURRENCY)
{
    try {
        $formatter = new NumberFormatter($locale, $style);
        // Attempt format.
        Logging::Log($formatter->formatCurrency($value, $currency));
        // Manually throw exception, if necessary.
        throwFormatterException($formatter);
    } catch (IntlException $exception) {
        // Output expected IntlExceptions.
        Logging::Log($exception);
    } catch (Error | Exception $error) {
        // Output unexpected Errors and Exceptions.
        Logging::Log($error, false);
    }
}
```

Let's test it out:

```php
Logging::LineSeparator('FORMAT CURRENCY');
formatCurrency($number);
Logging::LineSeparator('FORMAT INVALID CURRENCY');
formatCurrency($number, 'ABCDE');
```

Here's the output:

```
----------- FORMAT CURRENCY ------------
$1,234.57
------- FORMAT INVALID CURRENCY --------
Formatter failed with error code: 1.  Throwing exception...
[EXPECTED] IntlException: Number formatting failed: U_ILLEGAL_ARGUMENT_ERROR in D:\work\Airbrake.io\Exceptions\PHP\Exception\IntlException\code.php on line 169
```

Again, the first call works as expected, while the second produces an uncaught error, so we manually throw an `IntlException`.

Finally, let's look at formatting messages via `formatMessage(...)`:

```php
/**
 * Format message value.
 *
 * @param array $value Message value.
 * @param null|string $locale Locale.
 * @param null|string $pattern Message formatting pattern.
 */
function formatMessage(array $value, ?string $locale = 'en_US', ?string $pattern = '{0}') {
    try {
        $formatter = new MessageFormatter($locale, $pattern);
        // Attempt format.
        Logging::Log($formatter->format($value));
        // Manually throw exception, if necessary.
        throwFormatterException($formatter);
    } catch (IntlException $exception) {
        // Output expected IntlExceptions.
        Logging::Log($exception);
    } catch (Error | Exception $error) {
        // Output unexpected Errors and Exceptions.
        Logging::Log($error, false);
    }
}
```

And our test code:

```php
Logging::LineSeparator('FORMAT MESSAGE');
formatMessage(array($message));
Logging::LineSeparator('FORMAT INVALID MESSAGE');
formatMessage(array($message), 'en_US', null);
```

Produces this output:

```
------------ FORMAT MESSAGE ------------
This is a message!
-------- FORMAT INVALID MESSAGE --------
[EXPECTED] IntlException: Constructor failed in D:\work\Airbrake.io\Exceptions\PHP\Exception\IntlException\code.php on line 104
```

Since our `$pattern` argument was set to `null`, we again throw an `IntlException` in the second call.

---

As mentioned, the `php.ini` setting of `intl.use_exceptions` is disabled by default, but let's try enabling it and executing these tests again to see how things change:

```
[intl]
;intl.default_locale =
; This directive allows you to produce PHP errors when some error
; happens within intl functions. The value is the level of the error produced.
; Default is 0, which does not produce any errors.
;intl.error_level = E_WARNING
intl.use_exceptions = 1
```

Our expectation is, now that `intl.use_exceptions` is enabled, any problems that a `Formatter` instance experiences during constructor or when calling `format(...)` should result in an explicit `IntlException` being thrown.  Therefore, subsequent calls to our backup `throwFormatterException($exception)` function will be skipped entirely, since these invocations appear _after_ the formatter does its work.

Since all the valid calls we made during our tests will still perform as expected, we'll skip over those and only execute the "INVALID" tests a second time:

```php
function executeInvalidTests() {
    $number = 1234.567;
    $message = 'This is a message!';

    Logging::LineSeparator('FORMAT INVALID DATE');
    formatDate(null);

    Logging::LineSeparator('FORMAT INVALID NUMBER');
    formatNumber($number, 'en_US', 24601);

    Logging::LineSeparator('FORMAT INVALID CURRENCY');
    formatCurrency($number, 'ABCDE');

    Logging::LineSeparator('FORMAT INVALID MESSAGE');
    formatMessage(array($message), 'en_US', null);
}
```

Performing these same invalid calls produces the following output now, confirming that each problematic call explicitly created and threw `IntlException`, as expected:

```
--------- FORMAT INVALID DATE ----------
[EXPECTED] IntlException: datefmt_format: invalid PHP type for date in D:\work\Airbrake.io\Exceptions\PHP\Exception\IntlException\code.php on line 100

-------- FORMAT INVALID NUMBER ---------
[EXPECTED] IntlException: numfmt_create: number formatter creation failed in D:\work\Airbrake.io\Exceptions\PHP\Exception\IntlException\code.php on line 144

------- FORMAT INVALID CURRENCY --------
[EXPECTED] IntlException: Number formatting failed in D:\work\Airbrake.io\Exceptions\PHP\Exception\IntlException\code.php on line 68

-------- FORMAT INVALID MESSAGE --------
[EXPECTED] IntlException: msgfmt_create: message formatter creation failed in D:\work\Airbrake.io\Exceptions\PHP\Exception\IntlException\code.php on line 121
```

Check out the <a class="js-cta-utm" href="https://airbrake.io/languages/php_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-php">Airbrake-PHP library</a>, designed to quickly and easily integrate into any PHP project, giving you and your team access to real-time error monitoring and reporting throughout your application's entire life cycle.  With automatic, instantaneous error and exception notifications at your fingertips, you'll be constantly aware of your application's health, including any issues that may arise.  Best of all, with Airbrake's robust web dashboard cataloging every error that occurs, you and your team can immediately dive into the exact details of what went wrong, making it easy to quickly recognize and resolve problems.

---

__META DESCRIPTION__

A close look at the PHP IntlException class, including code samples illustrating how to work with Intl formatters for dates, strings, and numbers.

---

__SOURCES__

- http://php.net/manual/en/class.throwable.php