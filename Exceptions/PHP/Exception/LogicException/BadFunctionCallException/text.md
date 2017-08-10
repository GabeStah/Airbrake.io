# PHP Exception Handling - BadMethodCallException

Moving along through our detailed [__PHP Exception Handling__](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) series, today we'll take a closer look at the BadMethodCallException in PHP, along with its lesser-used parent BadFunctionCallException.  The primary scenario in which a `BadMethodCallException` is thrown is when calling either an instance or static method that doesn't exist, or expects a different argument signature than the one provided.

In this article we'll dig into the `BadMethodCallException` a bit more, looking at where it resides in the [PHP Exception Hierarchy](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy).  We'll also take a look at how `BadMethodCallExceptions` should be used and handled in your own coding endeavors, so let's get movin'!

## The Technical Rundown

- All PHP errors implement the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface, or are extended from another inherited class therein.
- [`Exception`](http://php.net/manual/en/class.exception.php) implements the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface.
- [`LogicException`](http://php.net/manual/en/class.logicexception.php) extends the [`Exception`](http://php.net/manual/en/class.exception.php) class.
- [`BadFunctionCallException`](http://php.net/manual/en/class.badfunctioncallexception.php) extends the [`LogicException`](http://php.net/manual/en/class.logicexception.php) class.
- Finally, `BadMethodCallException` extends the [`BadFunctionCallException`](http://php.net/manual/en/class.badfunctioncallexception.php) class.

## Full Code Sample

Below is the full code sample we’ll be using in this article.  Feel free to use any or all of the code if you wish to follow along.

```php
<?php

/**
 * Class Book
 */
class Book
{
    private $author;
    private $pageCount;
    private $title;

    /**
     * Magic method triggers when inaccessible instance method is invoked.
     *
     * Throws BadMethodCallException.
     *
     * @param string $name Name of invoked method.
     * @param array $args Additional arguments.
     */
    public function __call(string $name, array $args) {
        throw new BadMethodCallException("Instance method Book->$name() doesn't exist");
    }

    /**
     * Magic method triggers when inaccessible static method is invoked.
     *
     * Throws BadMethodCallException.
     *
     * @param string $name Name of invoked method.
     * @param array $args Additional arguments.
     */
    public static function __callstatic(string $name, array $args) {
        throw new BadMethodCallException("Static method Book::$name() doesn't exist");
    }

    /**
     * Book constructor.
     *
     * @param Book|string $title Book title.
     * @param Book|string $author Book author.
     * @param Book|int $pageCount Book page count.
     */
    public function __construct(string $title, string $author, int $pageCount) {
        $this->setAuthor($author);
        $this->setPageCount($pageCount);
        $this->setTitle($title);
    }

    /**
     * Get the author.
     *
     * @return string Book author.
     */
    public function getAuthor(): string {
        return $this->author;
    }

    /**
     * Set the author.
     *
     * @param string $value Author value to be set.
     */
    public function setAuthor(string $value) {
        $this->author = $value;
    }

    /**
     * Set the current page count of Book.
     *
     * @return mixed Page count of Book.
     */
    public function getPageCount(): int {
        return $this->pageCount;
    }

    /**
     * Get the current page count of Book.
     *
     * @param int $pageCount Page count to set.
     */
    public function setPageCount(int $pageCount) {
        $this->pageCount = $pageCount;
    }

    /**
     * Get the title.
     *
     * @return string Book title.
     */
    public function getTitle(): string {
        return $this->title;
    }

    /**
     * Set the title.
     *
     * @param string $value Title value to be set.
     */
    public function setTitle(string $value) {
        $this->title = $value;
    }
}

function executeExamples()
{
    callInvalidInstanceMethod();
    callInvalidStaticMethod();
    callInvalidFunction();
}

function callInvalidInstanceMethod() {
    try {
        // Create new Book instance.
        $book = new Book("A Game of Thrones", "George R. R. Martin", 835);
        // Call invalid method.
        $book->checkout();
    } catch (BadMethodCallException $exception) {
        // Output expected BadMethodCallException.
        Logging::Log($exception);
    } catch (BadFunctionCallException $exception) {
        // Output unexpected BadFunctionCallException.
        Logging::Log($exception, false);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

function callInvalidStaticMethod() {
    try {
        // Call invalid static method.
        Book::checkout();
    } catch (BadMethodCallException $exception) {
        // Output expected BadMethodCallException.
        Logging::Log($exception);
    } catch (BadFunctionCallException $exception) {
        // Output unexpected BadFunctionCallException.
        Logging::Log($exception, false);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

function callInvalidFunction() {
    try {
        // Call an invalid function.
        invalidFunction();
    } catch (BadMethodCallException $exception) {
        // Output unexpected BadMethodCallException.
        Logging::Log($exception, false);
    } catch (BadFunctionCallException $exception) {
        // Output expected BadFunctionCallException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    } catch (Error $error) {
        // Output unexpected Errors.
        Logging::Log($error,false);
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
     * @param object $object Object to be logged.
     *
     * @see https://github.com/kint-php/kint    Kint tool used for structured outputs.
     */
    private static function LogObject(object $object) {
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

We have three examples that aim to illustrate how `BadMethodCallExceptions` and `BadFunctionCallExceptions` might come up.  We start with the `Book` class, which contains a few property getters and setters, along with two PHP "magic methods": `__call()` and `__callstatic()`.

```php
/**
 * Class Book
 */
class Book
{
    private $author;
    private $pageCount;
    private $title;

    /**
     * Magic method triggers when inaccessible instance method is invoked.
     *
     * Throws BadMethodCallException.
     *
     * @param string $name Name of invoked method.
     * @param array $args Additional arguments.
     */
    public function __call(string $name, array $args) {
        throw new BadMethodCallException("Instance method Book->$name() doesn't exist");
    }

    /**
     * Magic method triggers when inaccessible static method is invoked.
     *
     * Throws BadMethodCallException.
     *
     * @param string $name Name of invoked method.
     * @param array $args Additional arguments.
     */
    public static function __callstatic(string $name, array $args) {
        throw new BadMethodCallException("Static method Book::$name() doesn't exist");
    }

    /**
     * Book constructor.
     *
     * @param Book|string $title Book title.
     * @param Book|string $author Book author.
     * @param Book|int $pageCount Book page count.
     */
    public function __construct(string $title, string $author, int $pageCount) {
        $this->setAuthor($author);
        $this->setPageCount($pageCount);
        $this->setTitle($title);
    }

    /**
     * Get the author.
     *
     * @return string Book author.
     */
    public function getAuthor(): string {
        return $this->author;
    }

    /**
     * Set the author.
     *
     * @param string $value Author value to be set.
     */
    public function setAuthor(string $value) {
        $this->author = $value;
    }

    /**
     * Set the current page count of Book.
     *
     * @return mixed Page count of Book.
     */
    public function getPageCount(): int {
        return $this->pageCount;
    }

    /**
     * Get the current page count of Book.
     *
     * @param int $pageCount Page count to set.
     */
    public function setPageCount(int $pageCount) {
        $this->pageCount = $pageCount;
    }

    /**
     * Get the title.
     *
     * @return string Book title.
     */
    public function getTitle(): string {
        return $this->title;
    }

    /**
     * Set the title.
     *
     * @param string $value Title value to be set.
     */
    public function setTitle(string $value) {
        $this->title = $value;
    }
}
```

The `__call()` magic method triggers when an invalid instance method is called on a `Book` class instance.  Similarly, the `__callstatic()` magic method is triggered when attempting to call an invalid static method.  In both cases, the common technique is to `throw` a new `BadMethodCallException` indicating the problem.  This ensures that a typo or incorrect method call is noticed immediately and can be remedied.

To see this in action, we start with the `callInvalidInstanceMethod()` function:

```php
function callInvalidInstanceMethod() {
    try {
        // Create new Book instance.
        $book = new Book("A Game of Thrones", "George R. R. Martin", 835);
        // Call invalid method.
        $book->checkout();
    } catch (BadMethodCallException $exception) {
        // Output expected BadMethodCallException.
        Logging::Log($exception);
    } catch (BadFunctionCallException $exception) {
        // Output unexpected BadFunctionCallException.
        Logging::Log($exception, false);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}
```

All we're doing here is creating a new `Book` instance and then trying to call the `checkout()` method, which doesn't exist.  This throws a `BadMethodCallException`, as expected, which we catch and output:

```
[EXPECTED] BadMethodCallException: Instance method Book->checkout() doesn't exist in D:\work\Airbrake.io\Exceptions\PHP\Exception\LogicException\BadFunctionCallException\code.php on line 90
```

Not too surprisingly, we'll be using the `callInvalidStaticMethod()` method to test the `Book::__callstatic()` magic method:

```php
function callInvalidStaticMethod() {
    try {
        // Call invalid static method.
        Book::checkout();
    } catch (BadMethodCallException $exception) {
        // Output expected BadMethodCallException.
        Logging::Log($exception);
    } catch (BadFunctionCallException $exception) {
        // Output unexpected BadFunctionCallException.
        Logging::Log($exception, false);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}
```

We need very little logical code to properly test this one -- all we need to do is to try calling the missing static `Book::checkout()` method.  The output shows that our slightly different `BadMethodCallException` was thrown, as intended:

```
[EXPECTED] BadMethodCallException: Static method Book::checkout() doesn't exist in D:\work\Airbrake.io\Exceptions\PHP\Exception\LogicException\BadFunctionCallException\code.php on line 102
```

Now we run into a bit of a strange situation.  As we saw in the hierarchy section above, `BadMethodCallException` inherits from the `BadFunctionCallException` class.  However, causing a `BadFunctionCallException` is quite uncommon and is generally not considered appropriate in userland code.  There are a few reasons for this, but the biggest reason is that PHP itself throws a _fatal error_ when an attempt is made to call an invalid function.  To illustrate, take a look at the `callInvalidFunction()` function:

```php
function callInvalidFunction() {
    try {
        // Call an invalid function.
        invalidFunction();
    } catch (BadMethodCallException $exception) {
        // Output unexpected BadMethodCallException.
        Logging::Log($exception, false);
    } catch (BadFunctionCallException $exception) {
        // Output expected BadFunctionCallException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    } catch (Error $error) {
        // Output unexpected Errors.
        Logging::Log($error,false);
    }
}
```

Here we attempt to call the `invalidFunction()` function, which doesn't exist in this context.  One might _expect_ a `BadFunctionCallException` to be thrown, but PHP doesn't handle undefined functions that way.  Instead, notice we're also catching any `Errors` at the end of our `catch` block series.  Sure enough, thanks to the ability to catch such errors in PHP 7+, the output shows that PHP threw an unexpected error because of our invalid function call:

```
[UNEXPECTED] Error: Call to undefined function invalidFunction() in D:\work\Airbrake.io\Exceptions\PHP\Exception\LogicException\BadFunctionCallException\code.php on line 150
```

While you _could_ explicitly throw `BadFunctionCallExceptions` in your own code when a function isn't properly invoked, with most modern object-oriented design patterns, the `BadMethodCallException` will likely be more appropriate to your needs.

Check out the <a class="js-cta-utm" href="https://airbrake.io/languages/php_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-php">Airbrake-PHP library</a>, designed to quickly and easily integrate into any PHP project, giving you and your team access to real-time error monitoring and reporting throughout your application's entire life cycle.  With automatic, instantaneous error and exception notifications at your fingertips, you'll be constantly aware of your application's health, including any issues that may arise.  Best of all, with Airbrake's robust web dashboard cataloging every error that occurs, you and your team can immediately dive into the exact details of what went wrong, making it easy to quickly recognize and resolve problems.

---

__META DESCRIPTION__

A deep dive into the PHP BadMethodCallException and BadFunctionCallException classes, including code samples showing the difference between the two.
---

__SOURCES__

- http://php.net/manual/en/class.throwable.php