# PHP Exception Handling - OutOfRangeException

Travelling along through the deep undergrowth, in the [__PHP Exception Handling__](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) forest we've created, today we come to the lush meadow that is the OutOfRangeException.  In spite of what you might suspect based on the name, the `OutOfRangeException` is not thrown when attempting to access an index outside the bounds of an `array` or other collection object.  Instead, an `OutOfRangeException` is meant to be used for `compile time` issues, such as trying to access an object that doesn't make logical sense in the context.

In this article we'll dig into the `OutOfRangeException` a bit more, including where it resides in the overall [PHP Exception Hierarchy](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy).  We'll also look at some sample code that illustrates just one example of illogical "compile time" code that should cause a `OutOfRangeException` in the first place, so let's get going!

## The Technical Rundown

All PHP errors implement the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy)
    - [`Exception`](http://php.net/manual/en/class.exception.php)
        - [`LogicException`](http://php.net/manual/en/class.logicexception.php)
            - `OutOfRangeException`

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
    private $publicationMonth;
    private $publicationYear;
    private $title;

    // Maximum byte length of author field.
    const AUTHOR_MAX_LENGTH = 255;
    // Minimum publication month.
    const PUBLICATION_MONTH_MIN = 1;
    // Maximum publication month.
    const PUBLICATION_MONTH_MAX = 12;
    // Maximum byte length of title field.
    const TITLE_MAX_LENGTH = 65535;

    /**
     * Book constructor.
     *
     * @param Book|string $title Book title.
     * @param Book|string $author Book author.
     * @param Book|int $pageCount Book page count.
     */
    public function __construct(string $title, string $author, int $pageCount, int $publicationMonth, int $publicationYear) {
        $this->setAuthor($author);
        $this->setPageCount($pageCount);
        $this->setPublicationMonth($publicationMonth);
        $this->setPublicationYear($publicationYear);
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
     * Get the month of publication.
     *
     * @return int Numeric publication month.
     */
    public function getPublicationMonth(): int {
        return $this->publicationMonth;
    }

    /**
     * Set the month of publication.
     *
     * @param int $month Numeric publication month.
     */
    public function setPublicationMonth(int $month) {
        if ($month < self::PUBLICATION_MONTH_MIN || $month > self::PUBLICATION_MONTH_MAX) {
            throw new OutOfRangeException("Invalid publication month: $month.  Must be between " . self::PUBLICATION_MONTH_MIN . " and " . self::PUBLICATION_MONTH_MAX);
        }
        $this->publicationMonth = $month;
    }

    /**
     * Get the year of publication.
     *
     * @return int Numeric publication year.
     */
    public function getPublicationYear(): int {
        return $this->publicationYear;
    }

    /**
     * Set the year of publication.
     *
     * @param int $year Numeric publication year.
     */
    public function setPublicationYear(int $year) {
        $this->publicationYear = $year;
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

function executeExamples()
{
    try {
        Logging::LineSeparator("FARSEER TRILOGY");
        Logging::Log(new Book("Assassin's Apprentice", "Robin Hobb", 448, 5, 1995));
        Logging::Log(new Book("Royal Assassin", "Robin Hobb", 675, 4, 1996));
        Logging::Log(new Book("Assassin's Quest", "Robin Hobb", 757, 3, 1997));

        Logging::LineSeparator("LIFESHIP TRADERS");
        Logging::Log(new Book("Ship of Magic", "Robin Hobb", 880, 1, 1998));
        Logging::Log(new Book("The Mad Ship", "Robin Hobb", 906, 11, 1999));
        Logging::Log(new Book("Ship of Destiny", "Robin Hobb", 789, 18, 2000));
    } catch (OutOfRangeException $exception) {
        // Output expected OutOfRangeExceptions.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

executeExamples();

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

As briefly discussed in the introduction, the [official documentation](http://php.net/manual/en/class.outofrangeexception.php) states that the `OutOfRangeException` "represents errors that should be detected at compile time."  This presents challenges for many PHP developers, because PHP is an [`interpreted language`](https://en.wikipedia.org/wiki/Interpreted_language) (as opposed to a traditional `compiled language`).  That's not to say that PHP code doesn't _get_ "compiled" at some point prior to execution -- the source code is translated from the written form into [`opcodes`](http://php.net/manual/en/internals2.opcodes.list.php) that can be interpreted and executed by the PHP program.  However, for the sake of debugging and detecting errors "at compile time," as the official documentation claims, that's not something that can be easily accomplished.

Therefore, the interpretation of the `OutOfRangeException` we'll be using in our code sample is that such errors should be thrown when indicating a logical fallacy in the code.  For example, here we have our trusty little `Book` class we've seen in previous articles:

```php
/**
 * Class Book
 */
class Book
{
    private $author;
    private $pageCount;
    private $publicationMonth;
    private $publicationYear;
    private $title;

    // Maximum byte length of author field.
    const AUTHOR_MAX_LENGTH = 255;
    // Minimum publication month.
    const PUBLICATION_MONTH_MIN = 1;
    // Maximum publication month.
    const PUBLICATION_MONTH_MAX = 12;
    // Maximum byte length of title field.
    const TITLE_MAX_LENGTH = 65535;

    /**
     * Book constructor.
     *
     * @param Book|string $title Book title.
     * @param Book|string $author Book author.
     * @param Book|int $pageCount Book page count.
     */
    public function __construct(string $title, string $author, int $pageCount, int $publicationMonth, int $publicationYear) {
        $this->setAuthor($author);
        $this->setPageCount($pageCount);
        $this->setPublicationMonth($publicationMonth);
        $this->setPublicationYear($publicationYear);
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
     * Get the month of publication.
     *
     * @return int Numeric publication month.
     */
    public function getPublicationMonth(): int {
        return $this->publicationMonth;
    }

    /**
     * Set the month of publication.
     *
     * @param int $month Numeric publication month.
     */
    public function setPublicationMonth(int $month) {
        if ($month < self::PUBLICATION_MONTH_MIN || $month > self::PUBLICATION_MONTH_MAX) {
            throw new OutOfRangeException("Invalid publication month: $month.  Must be between " . self::PUBLICATION_MONTH_MIN . " and " . self::PUBLICATION_MONTH_MAX, E_COMPILE_ERROR);
        }
        $this->publicationMonth = $month;
    }

    /**
     * Get the year of publication.
     *
     * @return int Numeric publication year.
     */
    public function getPublicationYear(): int {
        return $this->publicationYear;
    }

    /**
     * Set the year of publication.
     *
     * @param int $year Numeric publication year.
     */
    public function setPublicationYear(int $year) {
        $this->publicationYear = $year;
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

For the purpose of this exception tutorial we've added two new fields, `publicationMonth` and `publicationYear`, both of which are integer values.  Normally we'd probably use a single `publicationDate` or `publishedAt` field that was a `DateTime` object or similar but, again, for the sake of this example, we're using numeric values for our date.

Anyway, since we know that there are only twelve months in a year, it would be a illogical to allow a `publicationMonth` value that is outside the bounds of `1` through `12`.  Thus, we've added a bit of extra code to the `setPublicationMonth(int $month)` function to check that the passed parameter is within those bounds.  If not, an `OutOfRangeException` is thrown:

```php
/**
* Set the month of publication.
*
* @param int $month Numeric publication month.
*/
public function setPublicationMonth(int $month) {
    if ($month < self::PUBLICATION_MONTH_MIN || $month > self::PUBLICATION_MONTH_MAX) {
        throw new OutOfRangeException("Invalid publication month: $month.  Must be between " . self::PUBLICATION_MONTH_MIN . " and " . self::PUBLICATION_MONTH_MAX, E_COMPILE_ERROR);
    }
    $this->publicationMonth = $month;
}
```

To test this out we've created a few `Books` from a couple of cool trilogy series by author Robin Hobb, each with their associated publication month and years:

```php
function executeExamples()
{
    try {
        Logging::LineSeparator("FARSEER TRILOGY");
        Logging::Log(new Book("Assassin's Apprentice", "Robin Hobb", 448, 5, 1995));
        Logging::Log(new Book("Royal Assassin", "Robin Hobb", 675, 4, 1996));
        Logging::Log(new Book("Assassin's Quest", "Robin Hobb", 757, 3, 1997));

        Logging::LineSeparator("LIFESHIP TRADERS");
        Logging::Log(new Book("Ship of Magic", "Robin Hobb", 880, 1, 1998));
        Logging::Log(new Book("The Mad Ship", "Robin Hobb", 906, 11, 1999));
        Logging::Log(new Book("Ship of Destiny", "Robin Hobb", 789, 18, 2000));
    } catch (OutOfRangeException $exception) {
        // Output expected OutOfRangeExceptions.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}
```

As expected, executing this code produces the output we're after:

```
----------- FARSEER TRILOGY ------------
Book (5) (
    private 'author' -> string (10) "Robin Hobb"
    private 'pageCount' -> integer 448
    private 'publicationMonth' -> integer 5
    private 'publicationYear' -> integer 1995
    private 'title' -> string (21) "Assassin's Apprentice"
)

Book (5) (
    private 'author' -> string (10) "Robin Hobb"
    private 'pageCount' -> integer 675
    private 'publicationMonth' -> integer 4
    private 'publicationYear' -> integer 1996
    private 'title' -> string (14) "Royal Assassin"
)

Book (5) (
    private 'author' -> string (10) "Robin Hobb"
    private 'pageCount' -> integer 757
    private 'publicationMonth' -> integer 3
    private 'publicationYear' -> integer 1997
    private 'title' -> string (16) "Assassin's Quest"
)

----------- LIFESHIP TRADERS -----------
Book (5) (
    private 'author' -> string (10) "Robin Hobb"
    private 'pageCount' -> integer 880
    private 'publicationMonth' -> integer 1
    private 'publicationYear' -> integer 1998
    private 'title' -> string (13) "Ship of Magic"
)

Book (5) (
    private 'author' -> string (10) "Robin Hobb"
    private 'pageCount' -> integer 906
    private 'publicationMonth' -> integer 11
    private 'publicationYear' -> integer 1999
    private 'title' -> string (12) "The Mad Ship"
)
[EXPECTED] OutOfRangeException: Invalid publication month: 18.  Must be between 1 and 12 in D:\work\Airbrake.io\Exceptions\PHP\Exception\LogicException\OutOfRangeException\code.php on line 100
```

Uh oh!  Everything was working fine until we got the last `Book` creation of _Ship of Destiny_.  As the `OutOfRangeException` message indicates, the value we passed of `18` is outside the allowed bounds.  This was probably a typo, since the actual publication month was August (or `8`), instead.

Check out the <a class="js-cta-utm" href="https://airbrake.io/languages/php_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-php">Airbrake-PHP library</a>, designed to quickly and easily integrate into any PHP project, giving you and your team access to real-time error monitoring and reporting throughout your application's entire life cycle.  With automatic, instantaneous error and exception notifications at your fingertips, you'll be constantly aware of your application's health, including any issues that may arise.  Best of all, with Airbrake's robust web dashboard cataloging every error that occurs, you and your team can immediately dive into the exact details of what went wrong, making it easy to quickly recognize and resolve problems.

---

__META DESCRIPTION__

A deep dive into the OutOfRangeException class in PHP, including functional code samples illustrating how this exception might best be used.
---

__SOURCES__

- http://php.net/manual/en/class.throwable.php