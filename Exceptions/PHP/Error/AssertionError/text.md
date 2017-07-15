# PHP Exception Handling - AssertionError

Today we'll continue with our exploratory [__PHP Exception Handling__](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) series by diving deeper into the PHP AssertionError.  As you can probably guess, an `AssertionError` is (sometimes) thrown when an `assert()` call fails.  However, a number of default settings and configuration differences based on PHP versions means that `AssertionErrors` may not always behave as you expect them to.

Let's jump right in by first looking where the `AssertionError` fits in the overall [PHP Exception Hierarchy](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy), then we'll take a look at some functional code samples to help illustrate how different PHP versions and configurations may alter your own experiences.  Let's get started!

## The Technical Rundown

- All PHP errors implement the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface, or are extended from another inherited class therein.
- [`Error`](http://php.net/manual/en/class.error.php) implements the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface.
- `AssertionError` extends the [`Error`](http://php.net/manual/en/class.error.php) class.

## When Should You Use It?

Let's begin by jumping right into some code to test out some assertions.  Normally we'd begin with the entire code sample at once so it's easier to copy for your own experimentation, but in this case we'll need to check (and change) a number of PHP configuration settings throughout our tests, so we'll be using the same functions multiple times for that.

For those unaware, the [`assert()`](http://php.net/manual/en/function.assert.php) function accepts up to two arguments, but the single required argument should be a testable condition, either in the form of a string or a boolean condition, that will be tested by the PHP engine.  If the assertion succeeds, nothing happens.  However, if the condition fails PHP will perform one of a handful of (possible) actions, depending on settings, PHP version, and the like.  For our purposes, our goal is to have a failed `assert()` call throw an `AssertionError` at us.

Additionally, it's worth noting that this example code is running on a _default_ PHP 7 installation.  We're also using a simple `Logging` class to help with output, which can be found below:

```php
<?php

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

Let's start with a basic function called `assertEquality()`, which asserts that both parameters are equal to each other:

```php
function assertEquality($a, $b) {
    try {
        // Assert equality.
        if (assert($a == $b, "assert($a == $b) failed.")) {
            Logging::Log("assert($a == $b) was successful.");
        }
    } catch (AssertionError $error) {
        // Output expected AssertionError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }
}
```

We'll test this out by calling it twice with two sets of arguments:

```php
// Assert that 2 == 2.
assertEquality(2, 2);
// Assert that 1 == 2.
assertEquality(1, 2);
```

We'd expect that the first call would succeed, while the second will fail since `1 != 2`, but our log output shows both were successful:

```
assert(2 == 2) was successful.
assert(1 == 2) was successful.
```

What gives?  As it turns out, PHP 7 comes with a default `php.ini` configuration setting for the `zend.assertions` value of `-1`.  As you can see from the description taken from the `php.ini` file itself, this means that `assert()` function calls are not even compiled:

```ini
[Assertion]
; Switch whether to compile assertions at all (to have no overhead at run-time)
; -1: Do not compile at all
;  0: Jump over assertion at run-time
;  1: Execute assertions
; Changing from or to a negative value is only possible in php.ini! (For turning assertions on and off at run-time, see assert.active, when zend.assertions = 1)
; Default Value: 1
; Development Value: 1
; Production Value: -1
; http://php.net/zend.assertions
zend.assertions = -1
```

We can resolve this by setting `zend.assertions = 1`.  In addition, there's also the `assert.active` setting that defaults to `On`, so we typically don't need to change it, but it's worth double-checking if your own installation is misbehaving.

Now, after running our two assertion tests again we get much different output for the second (failing) assertion call:

```
assert(2 == 2) was successful.

PHP Warning:  assert(): assert(1 == 2) failed. failed in /media/sf_Airbrake.io/Exceptions/PHP/Error/AssertionError/code.php on line 98
PHP Stack trace:
PHP   1. {main}() /media/sf_Airbrake.io/Exceptions/PHP/Error/AssertionError/code.php:0
PHP   2. executeExamples() /media/sf_Airbrake.io/Exceptions/PHP/Error/AssertionError/code.php:134
PHP   3. assertEquality($a = *uninitialized*, $b = *uninitialized*) /media/sf_Airbrake.io/Exceptions/PHP/Error/AssertionError/code.php:81
PHP   4. assert(*uninitialized*, *uninitialized*) /media/sf_Airbrake.io/Exceptions/PHP/Error/AssertionError/code.php:98
```

We're starting to get somewhere now that our `assert(1 == 2)` test failed, but the PHP engine issued a `Warning` instead of an `Error` that we expected.  This means that our `try-catch` block wasn't able to catch the `AssertionError` that we were after.

This failure to throw an `AssertionError` is due to another `php.ini` setting of `assert.exception`.  When this is set to `On` (or `1`), it forces the PHP engine to throw an `AssertionError` when an assertion fails, rather than the default of issuing a warning only.  Again, the official documentation description further elaborates:

> - 1: throw when the assertion fails, either by throwing the object provided as the exception or by throwing a new AssertionError object if exception wasn't provided
> - 0: use or generate a Throwable as described above, but only generate a warning based on that object rather than throwing it (compatible with PHP 5 behaviour)

So we see that, as of PHP 7, the default behavior is to _generate_ a `Throwable` if one is passed (as the second parameter), but not to actually `throw` it (unless `assert.exception` is enabled).

In our case, after setting `assert.exception = On` and rerunning our tests, we get the following output:

```
assert(2 == 2) was successful.

[EXPECTED] AssertionError: assert(1 == 2) failed. in /media/sf_Airbrake.io/Exceptions/PHP/Error/AssertionError/code.php on line 112
```

Now we're starting to get somewhere, as we were able to `throw` and `catch` the produced `AssertionError`.  Yet `assert()` has a few more options that can be used to modify its behavior; namely the `assert_options()` function:

```php
// Enable assertion (Default: true)
assert_options(ASSERT_ACTIVE,   true);
// Enable warning when assertion fails (Default: true)
assert_options(ASSERT_WARNING,  true);
// Enable termination if assertion is failed (Default: false)
assert_options(ASSERT_BAIL,     true);

// Assert that 1 == 2.
assertEquality(1, 2);
// Assert that 2 == 2.
assertEquality(2, 2);
```

Here we've configure a few of the `assert()` behavioral options, most of which are available in the `php.ini` as well, setting them to their default values.  However, we're also setting `ASSERT_BAIL` to true, which will attempt to terminate the executing process if a failed assertion is encountered.  Thus, if we reorder our `assertEquality()` function tests so our failed test comes first (as seen above), our output dramatically changes once again:

```
PHP Fatal error:  Uncaught AssertionError: assert(1 == 2) failed. in /media/sf_Airbrake.io/Exceptions/PHP/Error/AssertionError/code.php:113
Stack trace:
#0 /media/sf_Airbrake.io/Exceptions/PHP/Error/AssertionError/code.php(113): assert(false, 'assert(1 == 2) ...')
#1 /media/sf_Airbrake.io/Exceptions/PHP/Error/AssertionError/code.php(95): assertEquality(1, 2)
#2 /media/sf_Airbrake.io/Exceptions/PHP/Error/AssertionError/code.php(149): executeExamples()
#3 {main}
  thrown in /media/sf_Airbrake.io/Exceptions/PHP/Error/AssertionError/code.php on line 113
PHP Stack trace:
PHP   1. {main}() /media/sf_Airbrake.io/Exceptions/PHP/Error/AssertionError/code.php:0
PHP   2. executeExamples() /media/sf_Airbrake.io/Exceptions/PHP/Error/AssertionError/code.php:149
PHP   3. assertEquality() /media/sf_Airbrake.io/Exceptions/PHP/Error/AssertionError/code.php:95
PHP   4. assert() /media/sf_Airbrake.io/Exceptions/PHP/Error/AssertionError/code.php:113
```

That's quite a mess, but what's important to note is that this produces an _uncaught_ error.  This is because `ASSERT_BAIL = true` forces the process to _immediately terminate_ the moment the failed assertion is encountered, rather than completing execution.  Thus, surrounding `catch` blocks and the like are never processed.

The last setting we'll mess with is `ASSERT_CALLBACK`, which allows us to specify a callback function that will receive a handful of arguments when an assertion fails, which we can use for additional, custom processing.  For our purposes we have the `onAssertionFailure()` function, which just accepts the handful of arguments that are passed and outputs a readable failure message:

```php
/**
 * @param $fileName Name of the executed script file.
 * @param $line Code line of failed assertion.
 * @param $ph Unknown placeholder.
 * @param $message Assertion description.
 */
function onAssertionFailure($fileName, $line, $ph, $message) {
    Logging::Log("Assertion at $fileName:$line failed with message: $message");
}

// Call a custom callback function for assertion failure.
assert_options(ASSERT_CALLBACK, 'onAssertionFailure');

// Assert that 1 == 2.
assertEquality(1, 2);
```

Executing our failed `assertEquality(1, 2)` function one last time with an `ASSERT_CALLBACK` function specified produces our new, modified output:

```
Assertion at /media/sf_Airbrake.io/Exceptions/PHP/Error/AssertionError/code.php:115 failed with message: assert(1 == 2) failed.
[EXPECTED] AssertionError: assert(1 == 2) failed. in /media/sf_Airbrake.io/Exceptions/PHP/Error/AssertionError/code.php on line 115
```

We still generate and `throw` an `AssertionError`, but the callback function is a convenient way to handle such failures, regardless of any `try/catch` or assertion exception settings.

Check out the <a class="js-cta-utm" href="https://airbrake.io/languages/php_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-php">Airbrake-PHP library</a>, designed to quickly and easily integrate into any PHP project, giving you and your team access to real-time error monitoring and reporting throughout your application's entire life cycle.  With automatic, instantaneous error and exception notifications at your fingertips, you'll be constantly aware of your application's health, including any issues that may arise.  Best of all, with Airbrake's robust web dashboard cataloging every error that occurs, you and your team can immediately dive into the exact details of what went wrong, making it easy to quickly recognize and resolve problems.

---

__META DESCRIPTION__

A close look at the AssertionError in PHP, including functional code examples illustrating the various assertion configuration options.