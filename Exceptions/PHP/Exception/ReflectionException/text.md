# PHP Exception Handling - ReflectionException

Moving along through the encompassing [__PHP Exception Handling__](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) series we've been creating, today we'll be taking a look at the PHP ReflectionException.  A `ReflectionException` occurs when there's an error while performing any sort of reflection; specifically when dealing with the [`Reflector`](http://php.net/manual/en/class.reflector.php), or with other classes that inherit from it.

In this article we'll explore the `ReflectionException` by looking at where it sits in the overall [PHP Exception Hierarchy](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy).  We'll also examine some functional PHP code samples that illustrate how such errors might be thrown, so you can see how they should be handled.  Let's get going!

## The Technical Rundown

All PHP errors implement the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy)
    - [`Exception`](http://php.net/manual/en/class.exception.php)
        - `ReflectionException`

## Full Code Sample

Below is the full code sample we’ll be using in this article.  Feel free to use any or all of the code if you wish to follow along.

```php
<?php
// code.php

function executeExamples()
{
    try {
        Logging::LineSeparator('NORMAL Book INSTANCE');
        Logging::Log(new Book('The Stand', 'Stephen King', 1153, new DateTime('1990-5-1')));

        Logging::LineSeparator('REFLECTING Book CLASS');
        $reflection = new ReflectionClass('Book');
        Logging::Log($reflection);

        Logging::LineSeparator('CONSTANTS');
        Logging::Log($reflection->getConstants());

        Logging::LineSeparator('METHODS');
        Logging::Log($reflection->getMethods());

        Logging::LineSeparator('NEW INSTANCE');
        Logging::Log($reflection->newInstance('The Shining', 'Stephen King', 447, new DateTime('7-1-1980')));

        Logging::LineSeparator('REFLECTING Invalid CLASS');
        Logging::Log(new ReflectionClass('Invalid'));
    } catch (ReflectionException $exception) {
        // Output expected ReflectionException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

executeExamples();
```

```php
<?php
// Book.php
/**
 * Class Book
 */
class Book
{
    private $author;
    private $pageCount;
    private $publicationDate;
    private $title;

    // Maximum byte length of author field.
    const AUTHOR_MAX_LENGTH = 255;
    // Maximum byte length of title field.
    const TITLE_MAX_LENGTH = 65535;

    /**
     * Book constructor.
     *
     * @param string $title Book title.
     * @param string $author Book author.
     * @param int $pageCount Book page count.
     * @param DateTime $publicationDate
     */
    public function __construct(string $title, string $author, int $pageCount = 0, DateTime $publicationDate = null) {
        $this->setAuthor($author);
        $this->setPageCount($pageCount);
        $this->setPublicationDate($publicationDate);
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
        // Check if length exceeds maximum.
        if (strlen($value) > self::AUTHOR_MAX_LENGTH) {
            // Create local variables for string interpolation.
            $length = strlen($value);
            $max = self::AUTHOR_MAX_LENGTH;
            $diff = $length - $max;
            throw new LengthException("Cannot set Author containing $length bytes, which exceeds the maximum of $max by $diff bytes.");
        }
        $this->author = $value;
    }

    /**
     * Get the current page count of Book.
     *
     * @return mixed Page count of Book.
     */
    public function getPageCount(): int {
        return $this->pageCount;
    }

    /**
     * Set the current page count of Book.
     *
     * @param int $pageCount Page count to set.
     */
    public function setPageCount(int $pageCount) {
        $this->pageCount = $pageCount;
    }

    /**
     * Get the publication date.
     *
     * @return DateTime Publication date.
     */
    public function getPublicationDate() {
        return $this->publicationDate;
    }

    /**
     * Set the publication date.
     *
     * @param DateTime $date Publication date.
     */
    public function setPublicationDate(DateTime $date) {
        $this->publicationDate = $date;
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
        // Check if length exceeds maximum.
        if (strlen($value) > self::TITLE_MAX_LENGTH) {
            // Create local variables for string interpolation.
            $length = strlen($value);
            $max = self::TITLE_MAX_LENGTH;
            $diff = $length - $max;
            throw new LengthException("Cannot set Title containing $length bytes, which exceeds the maximum of $max by $diff bytes.");
        }
        $this->title = $value;
    }

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
}
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

In programming, `reflection` is the means by which existing classes and objects can be "reverse-engineered", allowing runtime investigations and use of classes, functions, methods, and so forth, _without any prior knowledge of how those objects work or are coded._  We won't go into much more depth on what reflection is or how it works in this article, but [have a look here](https://en.wikipedia.org/wiki/Reflection_(computer_programming)) if you're interested in learning more.

Since PHP 5, most reflection takes place within the [`ReflectionClass`](http://php.net/manual/en/class.reflectionclass.php), so that's what we'll be using today.  To illustrate how it works, and how we might also run into a `ReflectionException`, we start with our basic `Book` class:

```php
<?php
// Book.php
/**
 * Class Book
 */
class Book
{
    private $author;
    private $pageCount;
    private $publicationDate;
    private $title;

    // Maximum byte length of author field.
    const AUTHOR_MAX_LENGTH = 255;
    // Maximum byte length of title field.
    const TITLE_MAX_LENGTH = 65535;

    /**
     * Book constructor.
     *
     * @param string $title Book title.
     * @param string $author Book author.
     * @param int $pageCount Book page count.
     * @param DateTime $publicationDate
     */
    public function __construct(string $title, string $author, int $pageCount = 0, DateTime $publicationDate = null) {
        $this->setAuthor($author);
        $this->setPageCount($pageCount);
        $this->setPublicationDate($publicationDate);
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
        // Check if length exceeds maximum.
        if (strlen($value) > self::AUTHOR_MAX_LENGTH) {
            // Create local variables for string interpolation.
            $length = strlen($value);
            $max = self::AUTHOR_MAX_LENGTH;
            $diff = $length - $max;
            throw new LengthException("Cannot set Author containing $length bytes, which exceeds the maximum of $max by $diff bytes.");
        }
        $this->author = $value;
    }

    /**
     * Get the current page count of Book.
     *
     * @return mixed Page count of Book.
     */
    public function getPageCount(): int {
        return $this->pageCount;
    }

    /**
     * Set the current page count of Book.
     *
     * @param int $pageCount Page count to set.
     */
    public function setPageCount(int $pageCount) {
        $this->pageCount = $pageCount;
    }

    /**
     * Get the publication date.
     *
     * @return DateTime Publication date.
     */
    public function getPublicationDate() {
        return $this->publicationDate;
    }

    /**
     * Set the publication date.
     *
     * @param DateTime $date Publication date.
     */
    public function setPublicationDate(DateTime $date) {
        $this->publicationDate = $date;
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
        // Check if length exceeds maximum.
        if (strlen($value) > self::TITLE_MAX_LENGTH) {
            // Create local variables for string interpolation.
            $length = strlen($value);
            $max = self::TITLE_MAX_LENGTH;
            $diff = $length - $max;
            throw new LengthException("Cannot set Title containing $length bytes, which exceeds the maximum of $max by $diff bytes.");
        }
        $this->title = $value;
    }

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
}
```

To test things out, we start by making a direct, explicit call to the `Book` constructor, and output the resulting instance to the log:

```php
Logging::LineSeparator('NORMAL Book INSTANCE');
Logging::Log(new Book('The Stand', 'Stephen King', 1153, new DateTime('1990-5-1')));
```

Unsurprisingly, this works just as expected:

```
--------- NORMAL Book INSTANCE ---------
Book (4) (
    private 'author' -> string (12) "Stephen King"
    private 'pageCount' -> integer 1153
    private 'publicationDate' -> DateTime (3) (
        public 'date' -> string (26) "1990-05-01 00:00:00.000000"
        public 'timezone' -> string (3) "UTC"
        public 'timezone_type' -> integer 3
    )
    private 'title' -> string (9) "The Stand"
)
```

Now, let's try creating an instance of `ReflectionClass`, in which we pass the `string` name of the class we want to reflect, `Book`:

```php
Logging::LineSeparator('REFLECTING Book CLASS');
$reflection = new ReflectionClass('Book');
Logging::Log($reflection);
```

If we get a result back and don't produce an error, we're in business.  Sure enough, that's exactly what our output shows:

```
-------- REFLECTING Book CLASS ---------
ReflectionClass (1) (
    public 'name' -> string (4) "Book"
)
```

With the `$reflection` variable holding our `ReflectionClass` instance of the `Book` class, we can now call some of its built-in methods, just to illustrate the basic functionality and power of the `ReflectionClass`.  Here we're getting a list of all the constants and methods within the reflected `Book` class:

```php
Logging::LineSeparator('CONSTANTS');
Logging::Log($reflection->getConstants());

Logging::LineSeparator('METHODS');
Logging::Log($reflection->getMethods());
```

And here's the produced output:

```
-------------- CONSTANTS ---------------
array (2) [
    'AUTHOR_MAX_LENGTH' => integer 255
    'TITLE_MAX_LENGTH' => integer 65535
]

--------------- METHODS ----------------
array (11) [
    0 => ReflectionMethod (2) (
        public 'class' -> string (4) "Book"
        public 'name' -> string (11) "__construct"
    )
    1 => ReflectionMethod (2) (
        public 'class' -> string (4) "Book"
        public 'name' -> string (9) "getAuthor"
    )
    2 => ReflectionMethod (2) (
        public 'class' -> string (4) "Book"
        public 'name' -> string (9) "setAuthor"
    )
    3 => ReflectionMethod (2) (
        public 'class' -> string (4) "Book"
        public 'name' -> string (12) "getPageCount"
    )
    4 => ReflectionMethod (2) (
        public 'class' -> string (4) "Book"
        public 'name' -> string (12) "setPageCount"
    )
    5 => ReflectionMethod (2) (
        public 'class' -> string (4) "Book"
        public 'name' -> string (18) "getPublicationDate"
    )
    6 => ReflectionMethod (2) (
        public 'class' -> string (4) "Book"
        public 'name' -> string (18) "setPublicationDate"
    )
    7 => ReflectionMethod (2) (
        public 'class' -> string (4) "Book"
        public 'name' -> string (8) "getTitle"
    )
    8 => ReflectionMethod (2) (
        public 'class' -> string (4) "Book"
        public 'name' -> string (8) "setTitle"
    )
    9 => ReflectionMethod (2) (
        public 'class' -> string (4) "Book"
        public 'name' -> string (6) "__call"
    )
    10 => ReflectionMethod (2) (
        public 'class' -> string (4) "Book"
        public 'name' -> string (12) "__callstatic"
    )
]
```

Finally, we can even use our `ReflectionClass` instance to create a new instance of the reflected `Book` class by calling the `newInstance()` method:

```php
Logging::LineSeparator('NEW INSTANCE');
Logging::Log($reflection->newInstance('The Shining', 'Stephen King', 447, new DateTime('7-1-1980')));
```

The output should be of a `Book` instance, just like at the top when we explicitly invoked `new Book(...)`:

```
------------- NEW INSTANCE -------------
Book (4) (
    private 'author' -> string (12) "Stephen King"
    private 'pageCount' -> integer 447
    private 'publicationDate' -> DateTime (3) (
        public 'date' -> string (26) "1980-01-07 00:00:00.000000"
        public 'timezone' -> string (3) "UTC"
        public 'timezone_type' -> integer 3
    )
    private 'title' -> string (11) "The Shining"
)
```

That's pretty cool, but let's now see what happens if we try to use `ReflectionClass` improperly.  For example, here we're trying to construct a new instance while passing the name of an invalid class:

```php
    try {
        // ...

        Logging::LineSeparator('REFLECTING Invalid CLASS');
        Logging::Log(new ReflectionClass('Invalid'));
    } catch (ReflectionException $exception) {
        // Output expected ReflectionException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
```

Lo and behold, `ReflectionClass` cannot find a class named `Invalid`, so it throws a `ReflectionException` our way as a result:

```
------- REFLECTING Invalid CLASS -------
[EXPECTED] ReflectionException: Class Invalid does not exist in D:\work\Airbrake.io\Exceptions\PHP\Exception\ReflectionException\code.php on line 26
```

This is just a small taste of what the `ExceptionReflection` class can do, but hopefully it gives you a jumping-off point to use it in your own projects!.

Check out the <a class="js-cta-utm" href="https://airbrake.io/languages/php_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-php">Airbrake-PHP library</a>, designed to quickly and easily integrate into any PHP project, giving you and your team access to real-time error monitoring and reporting throughout your application's entire life cycle.  With automatic, instantaneous error and exception notifications at your fingertips, you'll be constantly aware of your application's health, including any issues that may arise.  Best of all, with Airbrake's robust web dashboard cataloging every error that occurs, you and your team can immediately dive into the exact details of what went wrong, making it easy to quickly recognize and resolve problems.

---

__META DESCRIPTION__

A close look at the ReflectionException class in PHP, including functional code samples showing how to use reflection to manipulate class instances.

---

__SOURCES__

- http://php.net/manual/en/class.throwable.php