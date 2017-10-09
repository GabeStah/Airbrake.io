# PHP Exception Handling - UnderflowException

Making our way through our in-depth [__PHP Exception Handling__](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) series, today we'll be going over the **UnderflowException**.   If you're familiar with computer-based arithmetic rules, you'll likely be able to deduce that the `UnderflowException` is intended when performing arithmetic using decimal numbers that result in an inaccurate value, because the PHP engine cannot represent the proper `scale` of the actual, absolute value.

In this article we'll explore the `UnderflowException` in more detail, starting with where it sits in the larger [PHP Exception Hierarchy](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy).  We'll also examine a fully functional PHP code sample illustrating the basic process of determining how accurate your own PHP installation is when it comes to decimal `scale`, and how to use that information to throw `UnderflowExceptions` where appropriate.  Let's get started!

## The Technical Rundown

All PHP errors implement the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy)
    - [`Exception`](http://php.net/manual/en/class.exception.php)
        - [`RuntimeException`](http://php.net/manual/en/class.runtimeexception.php)
            - `UnderflowException`

## Full Code Sample

Below is the full code sample we’ll be using in this article.  Feel free to use any or all of the code if you wish to follow along.

```php
<?php

include("D:\work\Airbrake.io\lib\php\Logging.php");

function executeExamples()
{
    Logging::LineSeparator("FIND MAXIMUM SCALE ACCURACY");
    Logging::Log(getScaleAccuracy(true));

    Logging::LineSeparator("NO SCALE");
    addNumbers(123, 24478);

    Logging::LineSeparator("SCALE: 1");
    addNumbers(123.4, 24477.6);

    Logging::LineSeparator("SCALE: 2");
    addNumbers(123.45, 24477.55);

    Logging::LineSeparator("SCALE: 3");
    addNumbers(123.456, 24477.544);

    Logging::LineSeparator("SCALE: 4");
    addNumbers(123.4567, 24477.5433);
}

/**
 * Adds two numbers together.
 *
 * @param int|float|string $a First number to add.
 * @param int|float|string $b Second number to add.
 * @return mixed Result of summing $a and $b.
 */
function addNumbers($a, $b) {
    try {
        $maximumScale = getScaleAccuracy();
        if (getScale($a) > $maximumScale) {
            throw new UnderflowException("Scale of $a exceeds maximum accurate scale of $maximumScale.");
        } elseif (getScale($b) > $maximumScale) {
            throw new UnderflowException("Scale of $b exceeds maximum accurate scale of $maximumScale.");
        }
        $sum = $a + $b;
        Logging::Log("$a + $b == $sum");
        return $sum;
    } catch (UnderflowException $exception) {
        // Output expected UnderflowException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
    return null;
}

/**
 * Gets the scale of the passed float/decimal.
 *
 * @param int|float|string $number The number to retrieve scale of.
 * @return int Scale of passed $number.
 */
function getScale($number) {
    return strlen(strstr($number, '.')) - 1;
}

/**
 * Gets the maximum scale (number of places after decimal)
 * in which current PHP engine is accurate with floating points.
 *
 * @param bool $debugOutput Determine if debug output should be included.
 * @return int Maximum scale of accuracy.
 */
function getScaleAccuracy($debugOutput = false) {
    $scale = 1;
    while (true) {
        // Check if scale is accurate.
        if (!isScaleAccurate($scale, $debugOutput)) {
            return $scale - 1;
        }
        $scale++;
    }
}

/**
 * Determine if passed $scale value is accurate.
 *
 * @param int $scale Scale value to check.
 * @param bool $debugOutput Determine if debug output should be included.
 * @return bool Indicates if passed $scale is accurate.
 */
function isScaleAccurate($scale, $debugOutput = false) {
    // Create float (0.999...n) to n scale places.
    $string = '0.' . str_repeat('9', $scale);
    // Convert to float.
    $float = (float) $string;
    // Get result.
    $result = (1 - $float);
    // Determine if result is in the form of 0.00...1,
    // which indicates an accurate decimal value.
    if (substr($result, 0, 1) == "0") {
        if ($debugOutput) Logging::Log("Float scale to ($scale) places is accurate.");
        if ($debugOutput) Logging::Log("1 - $float == $result");
        return true;
    } else {
        // If converted to floating point, the form
        // is 9.99...8E-n, which indicates an inaccuracy.
        if ($debugOutput) Logging::Log("Float scale of ($scale) places is inaccurate.");
        if ($debugOutput) Logging::Log("1 - $float == $result");
        return false;
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

As mentioned in the introduction, [`arithmetic underflow`](https://en.wikipedia.org/wiki/Arithmetic_underflow) indicates that the result of a calculation is a smaller absolute value than the system can accurately store.  This can be for a variety of reasons, but typically `underflow` occurs when performing operations with large `decimal` numbers.  The [`precision`](https://docs.microsoft.com/en-us/sql/t-sql/data-types/precision-scale-and-length-transact-sql) of such a number indicates the total number of digits (i.e. `length`), while the `scale` indicates the number of digits following the decimal point.  For example, `24.601` has a `precision` of `5` and a `scale` of `3`.

Most programming languages, operating systems, CPUs, and so forth have a maximum amount of memory that can be devoted to storing a single value.  Thus, there must be a limit to the total accuracy (`precision` and/or `scale`) that can be stored for a decimal.  When the calculating engine cannot hold a `decimal` value that exceeds those limits, that number is typically converted to a `floating point` value.  This conversion is where problems can arise, which may lead to `UnderflowExceptions` in the case of PHP.  As we'll see in the example code in just a moment, PHP can only represent decimal values with a relatively small `scale` limit, and once that `scale` value is exceeded (i.e. the absolute value is too infinitesimal), PHP represents the value with a _slightly_ inaccurate `floating point` representation.  

These imprecise numeric values are usually insignificant, but in the case of strict application domains, such as banking or security, it can be _critical_ that there is _no_ chance for data loss or inaccuracy when handling particularly big or small numbers.

To illustrate these situations we start with the `isScaleAccurate($scale, $debugOutput = false)` function:

```php
/**
 * Determine if passed $scale value is accurate.
 *
 * @param int $scale Scale value to check.
 * @param bool $debugOutput Determine if debug output should be included.
 * @return bool Indicates if passed $scale is accurate.
 */
function isScaleAccurate($scale, $debugOutput = false) {
    // Create float (0.999...n) to n scale places.
    $string = '0.' . str_repeat('9', $scale);
    // Convert to float.
    $float = (float) $string;
    // Get result.
    $result = (1 - $float);
    // Determine if result is in the form of 0.00...1,
    // which indicates an accurate decimal value.
    if (substr($result, 0, 1) == "0") {
        if ($debugOutput) Logging::Log("Float scale to ($scale) places is accurate.");
        if ($debugOutput) Logging::Log("1 - $float == $result");
        return true;
    } else {
        // If converted to floating point, the form
        // is 9.99...8E-n, which indicates an inaccuracy.
        if ($debugOutput) Logging::Log("Float scale of ($scale) places is inaccurate.");
        if ($debugOutput) Logging::Log("1 - $float == $result");
        return false;
    }
}
```

This function creates a string in the form of `0.999...n` with a total `scale` equal to the passed `$scale` parameter, then converts that value to a `float`.  By subtracting that value from `1`, an _accurate_ `$result` would be a decimal in the form of `0.000...1`.  However, PHP cannot handle decimals with very large `scales`, so if the `$result` is converted to a `float` it will be in the form of `9.999...8E-n`.  No matter the actual floating point value, such conversion always indicates a slight loss in accuracy compared to the aforementioned decimal form.  Therefore, we can check the form of the `$result` to see if there is accuracy loss or not, given the passed `$scale` parameter.

We make use of the `isScaleAccurate($scale, $debugOutput = false)` function inside `getScaleAccuracy($debugOutput = false)`:

```php
/**
 * Gets the maximum scale (number of places after decimal)
 * in which current PHP engine is accurate with floating points.
 *
 * @param bool $debugOutput Determine if debug output should be included.
 * @return int Maximum scale of accuracy.
 */
function getScaleAccuracy($debugOutput = false) {
    $scale = 1;
    while (true) {
        // Check if scale is accurate.
        if (!isScaleAccurate($scale, $debugOutput)) {
            return $scale - 1;
        }
        $scale++;
    }
}
```

This function retrieves the highest `scale` value that remains accurate within the current PHP engine.  With this maximum `scale` value in hand, we can use it to properly throw an `UnderflowException` throughout our custom code, wherever appropriate.  For example, here we have the `addNumbers($a, $b)` function, that adds two numbers:

```php
/**
 * Adds two numbers together.
 *
 * @param int|float|string $a First number to add.
 * @param int|float|string $b Second number to add.
 * @return mixed Result of summing $a and $b.
 */
function addNumbers($a, $b) {
    try {
        $maximumScale = getScaleAccuracy();
        if (getScale($a) > $maximumScale) {
            throw new UnderflowException("Scale of $a exceeds maximum accurate scale of $maximumScale.");
        } elseif (getScale($b) > $maximumScale) {
            throw new UnderflowException("Scale of $b exceeds maximum accurate scale of $maximumScale.");
        }
        $sum = $a + $b;
        Logging::Log("$a + $b == $sum");
        return $sum;
    } catch (UnderflowException $exception) {
        // Output expected UnderflowException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
    return null;
}

/**
 * Gets the scale of the passed float/decimal.
 *
 * @param int|float|string $number The number to retrieve scale of.
 * @return int Scale of passed $number.
 */
function getScale($number) {
    return strlen(strstr($number, '.')) - 1;
}
```

It's critical that our `addNumbers($a, $b)` function only produces the precisely accurate results.  Thus, we use the `getScale($number)` helper function, along with `getScaleAccuracy()`, to determine if the two passed `$a` and `$b` parameters contain a `scale` value that exceeds the maximum `scale` accuracy our current PHP engine can handle.  If so, we throw a new `UnderflowException` indicating as much, otherwise we perform the calculation and output the result to the log.

To test this out we start by calling `getScaleAccuracy($debugOutput = false)` to display the `scale` accuracy output of the current PHP engine:

```php
function executeExamples()
{
    Logging::LineSeparator("FIND MAXIMUM SCALE ACCURACY");
    Logging::Log(getScaleAccuracy(true));

    // ...
}
```

```
----- FIND MAXIMUM SCALE ACCURACY ------
Float scale to (1) places is accurate.
1 - 0.9 == 0.1
Float scale to (2) places is accurate.
1 - 0.99 == 0.01
Float scale to (3) places is accurate.
1 - 0.999 == 0.001
Float scale of (4) places is inaccurate.
1 - 0.9999 == 9.9999999999989E-5
3
```

As we can see from the output, my current PHP maxes out at a `scale` value of `3`, after which it produces inaccurate `floating point` values.  Thus, we'll test the `addNumbers($a, $b)` function by passing in a series of increasingly-precise decimals:


```php
Logging::LineSeparator("NO SCALE");
addNumbers(123, 24478);

Logging::LineSeparator("SCALE: 1");
addNumbers(123.4, 24477.6);

Logging::LineSeparator("SCALE: 2");
addNumbers(123.45, 24477.55);

Logging::LineSeparator("SCALE: 3");
addNumbers(123.456, 24477.544);

Logging::LineSeparator("SCALE: 4");
addNumbers(123.4567, 24477.5433);
```

Executing the above tests produces the following output:

```
--------------- NO SCALE ---------------
123 + 24478 == 24601

--------------- SCALE: 1 ---------------
123.4 + 24477.6 == 24601

--------------- SCALE: 2 ---------------
123.45 + 24477.55 == 24601

--------------- SCALE: 3 ---------------
123.456 + 24477.544 == 24601

--------------- SCALE: 4 ---------------
[EXPECTED] UnderflowException: Scale of 123.4567 exceeds maximum accurate scale of 3. in D:\work\Airbrake.io\Exceptions\PHP\Exception\RuntimeException\UnderflowException\code.php on line 92
```

As expected, we're able to perform totally accurate calculations up until we exceed a `scale` maximum of `3`.  Trying to use values with `scale` of `4+` produces an `UnderflowException`, indicating the issue to the user.

Check out the <a class="js-cta-utm" href="https://airbrake.io/languages/php_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-php">Airbrake-PHP library</a>, designed to quickly and easily integrate into any PHP project, giving you and your team access to real-time error monitoring and reporting throughout your application's entire life cycle.  With automatic, instantaneous error and exception notifications at your fingertips, you'll be constantly aware of your application's health, including any issues that may arise.  Best of all, with Airbrake's robust web dashboard cataloging every error that occurs, you and your team can immediately dive into the exact details of what went wrong, making it easy to quickly recognize and resolve problems.

---

__META DESCRIPTION__

A close look at the PHP UnderflowException class, including code samples showing how to maintain complete accuracy with decimals and floating points.
---

__SOURCES__

- http://php.net/manual/en/class.throwable.php