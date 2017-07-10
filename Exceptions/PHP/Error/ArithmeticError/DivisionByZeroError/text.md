# PHP Exception Handling - DivisionByZeroError

In today's article we'll continue our travels through the [__PHP Exception Handling__](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) series with a closer look at the `DivisionByZeroError`.  As you might suspect, the `DivisionByZeroError` is (sometimes) thrown when attempting to divide a value by zero.  The challenge is the caveat of "sometimes", because this particular error might _not_ be thrown as often as you think.

We'll start by looking at where the `DivisionByZeroError` sits in the [PHP Exception Hierarchy](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy), before moving on to some functional code samples that illustrate when `DivisionByZeroErrors` will (and won't) be thrown, so let's get going!

## The Technical Rundown

- All PHP errors implement the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface, or are extended from another inherited class therein.
- [`Error`](http://php.net/manual/en/class.error.php) implements the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface.
- [`ArithmeticError`](https://airbrake.io/blog/php-exception-handling/arithmeticerror) extends the [`Error`](http://php.net/manual/en/class.error.php) class.
- Finally, `DivisionByZeroError` extends the [`ArithmeticError`](https://airbrake.io/blog/php-exception-handling/arithmeticerror).

## When Should You Use It?

Most of us are familiar with the trouble that comes when trying to divide by zero.  From playing with our calculator back in school days to modern programming and development, the fact remains that attempting to divide by zero is always considered _undefined_.  Since it does us no good to dwell any further on _why_ that is the case, let's jump right into our code sample and see _how_ `DivisionByZeroErrors` are commonly thrown in the realm of PHP, and what we can (and cannot) do to handle them:

```php
<?php

// Set error reporting level.
error_reporting(E_ERROR);

function executeExamples() {
    // Modulo by 3.
    modulo(10, 3);
    Logging::LineSeparator();

    // Modulo by zero.
    modulo(10, 0);
    Logging::LineSeparator();

    // Divide by 2.
    divide(10, 2);
    Logging::LineSeparator();

    // Divide by zero.
    divide(10, 0);

    // Divide by 2.
    performIntDiv(15, 2);
    Logging::LineSeparator();

    // Divide by zero.
    performIntDiv(15, 0);     
}

function divide($dividend, $divisor) {
    try {
        // Perform the operation.
        $result = $dividend / $divisor;
        Logging::Log("Division result of ($dividend / $divisor): $result");
    } catch (DivisionByZeroError $error) {
        // Output expected ArithmeticError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }
}

function modulo($dividend, $divisor) {
    try {
        // Perform the operation.
        $result = $dividend % $divisor;
        Logging::Log("Modulo result of ($dividend % $divisor): $result");
    } catch (DivisionByZeroError $error) {
        // Output expected ArithmeticError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }
}

function performIntDiv($dividend, $divisor)
{
    try {
        // Perform the operation.
        $result = intdiv($dividend, $divisor);
        Logging::Log("Modulo result of ($dividend % $divisor): $result");
    } catch (DivisionByZeroError $error) {
        // Output expected ArithmeticError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }    
}

executeExamples();

require('kint.php');

/**
 * Provides basic logging/output functionality.
 */
class Logging {

    /**
     * Logs the passed object, string, or Throwable instance to the console.
     *
     * @param $a Primary message or value to be logged.
     * @param null $b Secondary value, such as boolean for Throwables indicating if error was expected.
     */
    public static function Log($a, $b = null) {
        if (is_string($a)) {
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
     * @param $object Object to be logged.
     *
     * @see https://github.com/kint-php/kint    Kint tool used for structured outputs.
     */
    private static function LogObject($object) {
        Kint::dump($object);
    }

    /**
     * Logs the passed string value.
     *
     * @param $value Value to be logged.
     */
    private static function LogString($value) {
        print_r("{$value}\n");
    }

    /**
     * Logs the passed Throwable object.  
     * Includes message, className if error was expected, and stack trace.
     *
     * Uses internal Reflection to retrieve protected/private properties.
     *
     * @param $throwable Throwable object to be output.
     * @param bool $expected Indicates if error was expected or not.
     */
    private static function LogThrowable($throwable, bool $expected = true) {
        $expected = $expected ? "EXPECTED" : "UNEXPECTED";
        $message = substr($throwable->xdebug_message, 1);
        // Output whether error was expected or not, the class name, the message, and stack trace.
        print_r("[{$expected}] {$message}\n");
        // Add line separator to keep it tidy.
        self::LineSeparator();
    }

    /**
     * Outputs a separator line to log.
     *
     * @param int $length Length of the line separator.
     * @param string $character Character to use as separator.
     */
    public static function LineSeparator(int $length = 40, string $character = '-') {
        $break = str_repeat($character, $length);
        print_r("{$break}\n");
    }
}
```

Let's begin with our `modulo()` function, which accepts two parameters and performs a division of the two before spitting out any remainder:

```php
function modulo($dividend, $divisor) {
    try {
        // Perform the operation.
        $result = $dividend % $divisor;
        Logging::Log("Modulo result of ($dividend % $divisor): $result");
    } catch (DivisionByZeroError $error) {
        // Output expected ArithmeticError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }
}
```

To test these out we've made two calls to `modulo()`: The first using `10` and `3`, while the second uses `10` and `0`:

```php
// Modulo by 3.
modulo(10, 3);
Logging::LineSeparator();

// Modulo by zero.
modulo(10, 0);
Logging::LineSeparator();
```

The log output shows us that the first invocation works just fine, while the second call throws a `DivisionByZeroError`, which we were able to successfully catch as intended:

```
Modulo result of (10 % 3): 1
----------------------------------------
[EXPECTED] DivisionByZeroError: Modulo by zero in /media/sf_Airbrake.io/Exceptions/PHP/Error/ArithmeticError/DivisionByZeroError/code.php on line 41

Call Stack:
    0.0002     368520   1. {main}() /media/sf_Airbrake.io/Exceptions/PHP/Error/ArithmeticError/DivisionByZeroError/code.php:0
    0.0058    1668352   2. executeExamples() /media/sf_Airbrake.io/Exceptions/PHP/Error/ArithmeticError/DivisionByZeroError/code.php:52
    0.0058    1668384   3. modulo() /media/sf_Airbrake.io/Exceptions/PHP/Error/ArithmeticError/DivisionByZeroError/code.php:21
```

Now let's try our `divide()` function, which behaves much the same way, but attempts a straight division instead of a modulo operation:

```php
function divide($dividend, $divisor) {
    try {
        // Perform the operation.
        $result = $dividend / $divisor;
        Logging::Log("Division result of ($dividend / $divisor): $result");
    } catch (DivisionByZeroError $error) {
        // Output expected ArithmeticError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }
}
```

This time we'll pass in `10` and `2` the first time, along with `10` and `0` again the second call:

```php
// Divide by 2.
divide(10, 2);
Logging::LineSeparator();

// Divide by zero.
divide(10, 0);
```

Again, the first call behaves as expected and outputs our result, but the second call is a little bewildering.  Even though we _clearly_ performed a division operation using a divisor of zero, no `DivisionByZeroError` is thrown.  Instead, the `$result` of the calculation merely returns as `INF`:

```
Division result of (10 / 2): 5
----------------------------------------
Division result of (10 / 0): INF
```

Strange, what gives?  Keen observers may have noticed we explicitly set the error reporting level at the top of our script to only include the `E_ERROR` flag, which is described in the documentation as including only "Fatal run-time errors."  As it turns out, PHP doesn't consider straight up division by zero, as we saw in our second `divide()` call above, to be a fatal run-time error.  Instead, it considers it a warning, which can be seen if we include the `E_WARNING` flag in our `error_reporting()` function call at the top:

```php
error_reporting(E_ERROR | E_WARNING);
```

Now, when we rerun our two `divide()` calls we _still_ get the same `INF` `$result` of the second call, but we also see a PHP `Warning` indicating that there was an attempt to divide by zero:

```
Division result of (10 / 2): 5
----------------------------------------
PHP Warning:  Division by zero in /media/sf_Airbrake.io/Exceptions/PHP/Error/ArithmeticError/DivisionByZeroError/code.php on line 34
PHP Stack trace:
PHP   1. {main}() /media/sf_Airbrake.io/Exceptions/PHP/Error/ArithmeticError/DivisionByZeroError/code.php:0
PHP   2. executeExamples() /media/sf_Airbrake.io/Exceptions/PHP/Error/ArithmeticError/DivisionByZeroError/code.php:74
PHP   3. divide() /media/sf_Airbrake.io/Exceptions/PHP/Error/ArithmeticError/DivisionByZeroError/code.php:13

Division result of (10 / 0): INF
```

The last thing we'll try is calling the built-in `intdiv()` function by using our `performIntDiv()` wrapper function:

```php
function performIntDiv($dividend, $divisor)
{
    try {
        // Perform the operation.
        $result = intdiv($dividend, $divisor);
        Logging::Log("Modulo result of ($dividend % $divisor): $result");
    } catch (DivisionByZeroError $error) {
        // Output expected ArithmeticError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }    
}

//...

// Divide by 2.
performIntDiv(15, 2);
Logging::LineSeparator();

// Divide by zero.
performIntDiv(15, 0);

//...
```

As it happens, `intdiv()` behaves much the same as a modulo, meaning it will also throw an actual `DivisionByZeroError` when a value of zero is provided as the second argument, as shown by the output from above:

```
Modulo result of (15 % 2): 7
----------------------------------------
[EXPECTED] DivisionByZeroError: Division by zero in /media/sf_Airbrake.io/Exceptions/PHP/Error/ArithmeticError/DivisionByZeroError/code.php on line 63

Call Stack:
    0.0002     369792   1. {main}() /media/sf_Airbrake.io/Exceptions/PHP/Error/ArithmeticError/DivisionByZeroError/code.php:0
    0.0164    1669624   2. executeExamples() /media/sf_Airbrake.io/Exceptions/PHP/Error/ArithmeticError/DivisionByZeroError/code.php:74
    0.0164    1669656   3. performIntDiv() /media/sf_Airbrake.io/Exceptions/PHP/Error/ArithmeticError/DivisionByZeroError/code.php:28
    0.0164    1669656   4. intdiv() /media/sf_Airbrake.io/Exceptions/PHP/Error/ArithmeticError/DivisionByZeroError/code.php:63
```

Check out the <a class="js-cta-utm" href="https://airbrake.io/languages/php_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-php">Airbrake-PHP library</a>, designed to quickly and easily integrate into any PHP project, giving you and your team access to real-time error monitoring and reporting throughout your application's entire life cycle.  With automatic, instantaneous error and exception notifications at your fingertips, you'll be constantly aware of your application's health, including any issues that may arise.  Best of all, with Airbrake's robust web dashboard cataloging every error that occurs, you and your team can immediately dive into the exact details of what went wrong, making it easy to quickly recognize and resolve problems.

---

__META DESCRIPTION__

A deep dive into the DivisionByZeroError in PHP, including functional code examples illustrating the quirks of division by zero behaviors in PHP.