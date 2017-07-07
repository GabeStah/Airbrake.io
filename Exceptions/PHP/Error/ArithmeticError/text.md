# PHP Exception Handling - ArithmeticError

Today we begin our new [__PHP Exception Handling__](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) series with a deep dive into the `ArithmeticError`.  As you can probably guess, the `ArithmeticError` is thrown when PHP code attempts to perform an invalid mathematical operation.

Throughout this article we'll dig into the `ArithmeticError` in more detail, looking at where it sits within the [PHP Exception Hierarchy](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy).  We'll also take a look at some functional code aimed to illustrate how `ArithmeticErrors` might occur in your own adventures, so you can plan accordingly.  Let's get to it!

## The Technical Rundown

- All PHP errors implement the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface, or are extended from another inherited class therein.
- [`Error`](http://php.net/manual/en/class.error.php) implements the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface.
- `ArithmeticError` extends the [`Error`](http://php.net/manual/en/class.error.php) class.

## When Should You Use It?

The `ArithmeticError` isn't too difficult to grasp, but it's important to briefly review why it has the `Error` suffix, as opposed to an `Exception` suffix.  Put simply, PHP has two general categories of exceptions -- `internal errors` and `user exceptions`.  Historically, an internal error would've caused a total failure (a fatal error), forcing the PHP execution to halt completely.  However, modern PHP versions now provide a `catchable` class known as `Error` for those more critical internal errors, allowing your application to continue execution after the error is properly handled.

This means, of course, that `ArithmeticError` is considered an internal error, primarily because PHP doesn't know how to handle calculations that cannot be completed as requested by the code.  To illustrate we have a few simple examples.  We'll start with the full, functional code below, then explore each scenario in a bit more detail afterward:

```php
<?php
function ExecuteExamples() {
    DoBitwiseShift();
    Logging::LineSeparator();
    DoInvalidBitwiseShift();

    DoIntegerDivision();
    Logging::LineSeparator();
    DoInvalidIntegerDivision();
}

function DoBitwiseShift() {
    try {
        $a = 1;
        $b = 3;
        // Perform bitwise shift operation.
        $result = $a << $b;
        Logging::Log("Bitwise shift result: $result");
    } catch (ArithmeticError $error) {
        // Output expected ArithmeticError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }
}

function DoInvalidBitwiseShift() {
    try {
        $a = 1;
        $b = -3;
        // Perform bitwise shift operation.
        $result = $a << $b;
        Logging::Log("Bitwise shift result: $result");
    } catch (ArithmeticError $error) {
        // Output expected ArithmeticError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }
}

function DoIntegerDivision() {
    try {
        $a = 2147483647;
        $b = 3;
        // Perform integer division.
        $result = intdiv($a, $b);
        Logging::Log("Integer division result: $result");
    } catch (ArithmeticError $error) {
        // Output expected ArithmeticError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }
}

function DoInvalidIntegerDivision() {
    try {
        $a = PHP_INT_MIN;
        $b = -1;
        // Perform integer division.
        $result = intdiv($a, $b);
        Logging::Log("Integer division result: $result");
    } catch (ArithmeticError $error) {
        // Output expected ArithmeticError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }
}

ExecuteExamples();

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

Most of the time PHP tries to handle improper mathematical issues on its own, without the need for user intervention or throwing an error.  For example, if `integer overflow` might occur, such as using an integer value greater than a 32-bit system can handle (`2,147,483,648` or higher), PHP will often automatically convert that to a `floating point` number to perform the calculations.

However, there are some scenarios where PHP doesn't have an escape route, so to speak, so it must throw an `ArithmeticError` your way.  One such scenario that we're exploring above is performing an improper `bitwise shift` operation.  Here we have a `DoBitwiseShift()` function that takes two numbers (`1` and `3`), and performs a left bitwise shift.  As a reminder, a bitwise shift means to shift the bits of a target object `X` number of times.  Therefore, `$a << $b` indicates we want to shift the bits of `$a` `$b` times to the left.  The result is that we're effectively multiplying `$a` by two, multiplied by `$b`:

```php
function DoBitwiseShift() {
    try {
        $a = 1;
        $b = 3;
        // Perform bitwise shift operation.
        $result = $a << $b;
        Logging::Log("Bitwise shift result: $result");
    } catch (ArithmeticError $error) {
        // Output expected ArithmeticError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }
}
```

The output of our operation above shows that `1 << 3` is equal to `8`, because we're effectively multiplying `1` by `2` three total times:

```
Bitwise shift result: 8
```

However, let's now try changing `$b` to a negative number and see what happens:

```php
function DoInvalidBitwiseShift() {
    try {
        $a = 1;
        $b = -3;
        // Perform bitwise shift operation.
        $result = $a << $b;
        Logging::Log("Bitwise shift result: $result");
    } catch (ArithmeticError $error) {
        // Output expected ArithmeticError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }
}
```

As it happens, we're unable to perform a bit shift a _negative_ number of times because it just doesn't make any logical sense.  Therefore, the above function throws an `ArithmeticError` that we're able to catch:

```php
[EXPECTED] ArithmeticError: Bit shift by negative number in /media/sf_Airbrake.io/Exceptions/PHP/Error/ArithmeticError/code.php on line 36
```

As mentioned before, another potential issue is when trying to force PHP to create objects of a certain class type, yet outside the bounds of what values that type allows.  We see this in action in extreme cases using the [`intdiv`](http://php.net/manual/en/function.intdiv.php) function, which accepts two integer arguments and returns the `integer quotient` of their division operation.

For example, here we're calling `intdiv()` with the largest possible 32-bit integer value as the dividend, and a lowly `3` as the divisor:

```php
function DoIntegerDivision() {
    try {
        $a = 2147483647;
        $b = 3;
        // Perform integer division.
        $result = intdiv($a, $b);
        Logging::Log("Integer division result: $result");
    } catch (ArithmeticError $error) {
        // Output expected ArithmeticError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }
}
```

The output result shows that this works as expected:

```
Integer division result: 715827882
```

Remember that since `intdiv` needs to generate and return an `integer`, we cannot get a `float` or `decimal`-type value as a result.  Therefore, our result above was slightly rounded so it doesn't include the extra third that would've been on there with a floating point number.

However, let's try this again with some extra numbers:

```php
function DoInvalidIntegerDivision() {
    try {
        $a = PHP_INT_MIN;
        $b = -1;
        // Perform integer division.
        $result = intdiv($a, $b);
        Logging::Log("Integer division result: $result");
    } catch (ArithmeticError $error) {
        // Output expected ArithmeticError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }
}
```

PHP doesn't handle this well at all, and sure enough, it throws another `ArithmeticError` our way:

```
[EXPECTED] ArithmeticError: Division of PHP_INT_MIN by -1 is not an integer in /media/sf_Airbrake.io/Exceptions/PHP/Error/ArithmeticError/code.php on line 68
```

This example in particular seems a bit odd at first.  Why is it that diving two whole numbers (integers) by one another should produce a result that isn't also an integer itself?  Particularly when we're dividing by one (well, negative one actually), which should give a number that's effectively the same size as the original dividend.

The answer lies in considering how computers must handle `signed integers`, which are whole numbers that can be positive or negative.  Since everything in most modern computer systems is stored in bits (ones or zeroes), these signed integers always boil down to `base 2` values.  This means that every increase in the potential _size_ of an integer is always adding AT LEAST two possible values (another bit place, which can be either a zero or a one).

Extrapolating from there, let's now imagine we're creating our own programming language like PHP and we have a new integer data type that we need to set a limit on.  We've decided on something small and simple for this example, so let's go with a maximum of _four bits_ that can represent our number in binary.  Since we're creating a `signed integer` here, one of those bits must be reserved and used exclusively for the positive or negative distinction of our numeric value.  This leaves us with _three bits_ to hold the absolute value.

There are a number of ways to represent signed numbers in binary, but we've decided it makes the most sense to only include a _single binary value_ that corresponds to the value of zero.  Therefore, we're going to use a system called `two's complement`.  It is so named because we simply invert all the bits of the number (and add one) when we want to flip the sign.

Let's go through all our possible binary values and their corresponding interpreted numeric value:

Binary Value | Signed Number
--- | ---
1000 | -8
1001 | -7
1010 | -6
1011 | -5
1100 | -4
1101 | -3
1110 | -2
1111 | -1
0000 | 0
0001 | 1
0010 | 2
0011 | 3
0100 | 4
0101 | 5
0110 | 6
0111 | 7

There we have all the possible binary values we can make using four bits.  What's critical to note is that, as mentioned before, our number must always increase in sets of two since each new bit adds a one and zero.  **However, and this is the critical point, these values are not evenly distributed beyond the value of zero when dealing with signed numbers.**

As we can see even from our simple example above, since the value of zero must take one binary value position (`0000`), this leaves us with only `15` remaining binary values to distribute.  Using the `two's complement` system that we did, this means our total possible negative values will include _one extra_ than the total possible positive values.  We can go all the way down to `-8`, but only as high as positive `7` the other way.

Now let's glance back at our broken `intdiv()` example before.  Notice that the `divisor` we're using is `-1`.  This is critical and the key to the issue we're having that is throwing an `ArithmeticError`.  Take our simple 4-bit number above.  Let's take the _minimum_ possible value (`1000` or `-8`) and multiple it by `-1`:

```php
integer 8
```

Since we're multiplying two negatives numbers we get a _positive_ value of `8` as a result.  This is a big problem!  If our new programming language only allows for signed numbers to be represented by four bits, we can't handle the value of positive eight!  The largest number in our system is actually only positive seven.

This is exactly what's going on in our `intdiv()` example, but on a much larger scale.  PHP also uses a `two's complement` number system, with a maximum storage of 64 bits.  PHP also provides global variables to quickly access these two extreme positive and negative values, `PHP_INT_MIN` and `PHP_INT_MAX`.  Outputting those values shows just how big they are:

```
integer -9223372036854775808
integer 9223372036854775807
```

However, just like our 4-bit example above, the same problem applies here in PHP: The smallest negative value is _one fewer_ than the maximum positive value if inversed.  The result is that trying to divide `-9223372036854775808` by `-1` is effectively the same as multiplying `9223372036854775808` by `1`, giving the obvious result of `9223372036854775808`.  Since the largest positive integer PHP can handle is _one less_ than that, an `ArithmeticError` is thrown.

Check out the <a class="js-cta-utm" href="https://airbrake.io/languages/php_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-php">Airbrake-PHP library</a>, designed to quickly and easily integrate into any PHP project, giving you and your team access to real-time error monitoring and reporting throughout your application's entire life cycle.  With automatic, instantaneous error and exception notifications at your fingertips, you'll be constantly aware of your application's health, including any issues that may arise.  Best of all, with Airbrake's robust web dashboard cataloging every error that occurs, you and your team can immediately dive into the exact details of what went wrong, making it easy to quickly recognize and resolve problems.

---

__META DESCRIPTION__

A deep dive into the System.ServiceModel.FaultException in .NET, including a working C# code samples illustrating how to handle service exceptions.