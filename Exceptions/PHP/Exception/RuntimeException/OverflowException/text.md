# PHP Exception Handling - OverflowException

Moving along through our in-depth [__PHP Exception Handling__](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) series, today we'll take a closer look at the **OverflowException**.  Unlike most other languages, in PHP the `OverflowException` is not _explicitly_ related to attempts to use memory space/addresses that are outside the bounds of the memory assigned to the application.  Instead, the `OverflowException` in PHP is focused around a simple concept: Adding an element to a collection that is already full.

Throughout this article we'll explore the `OverflowException` by looking at where it resides in the larger [PHP Exception Hierarchy](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy).  We'll also look at some fully functional sample code that will illustrate how `OverflowExceptions` might be used in your own PHP projects, so let's get to it!

## The Technical Rundown

All PHP errors implement the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy)
    - [`Exception`](http://php.net/manual/en/class.exception.php)
        - [`RuntimeException`](http://php.net/manual/en/class.runtimeexception.php)
            - `OverflowException`

## Full Code Sample

Below is the full code sample we’ll be using in this article.  Feel free to use any or all of the code if you wish to follow along.

```php
// code.php
<?php

function executeExamples() {
    try {
        $book = new Book('The Name of the Wind', 'Patrick Rothfuss', 662, new DateTime('2007-03-27'));

        $kvothe = new Character('Kvothe');
        $bast = new Character('Bast');
        $chronicler = new Character('Chronicler');
        $denna = new Character('Denna');
        $auri = new Character('Auri');
        $wilem = new Character('Wilem');

        $book->addCharacter($kvothe);
        $book->addCharacter($bast);
        $book->addCharacter($chronicler);
        $book->addCharacter($denna);
        $book->addCharacter($auri);

        Logging::LineSeparator("THE NAME OF THE WIND");
        Logging::Log($book);

        Logging::LineSeparator("ADDITIONAL CHARACTER");
        Logging::Log($book->addCharacter($wilem));
    } catch (OverflowException $exception) {
        // Output expected OverflowExceptions.
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

include("Character.php");

/**
 * Class Book
 */
class Book
{
    private $author;
    private $characters = [];
    private $pageCount;
    private $publicationDate;
    private $title;

    // Maximum byte length of author field.
    const AUTHOR_MAX_LENGTH = 255;
    // Maximum allow number of characters.
    const MAX_CHARACTER_COUNT = 5;
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
    public function getPublicationDate() : ?DateTime {
        return $this->publicationDate;
    }

    /**
     * Set the publication date.
     *
     * @param DateTime $date Publication date.
     */
    public function setPublicationDate(?DateTime $date) {
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
     * Add a Character to the Book.
     *
     * @param Character $character
     * @param mixed $key
     */
    public function addCharacter(Character $character, $key = null) {
        if (count($this->characters) >= self::MAX_CHARACTER_COUNT) {
            $max = self::MAX_CHARACTER_COUNT;
            throw new OverflowException("Character count cannot exceed maximum of $max");
        }
        if (!is_null($key)) {
            $this->characters[$key] = $character;
        } else {
            $this->characters[] = $character;
        }
    }

    /**
     * Get a Character using passed key.
     *
     * @param mixed $key
     * @return mixed
     */
    public function getCharacter($key) {
        if (!array_key_exists($key, $this->characters)) {
            throw new OutOfBoundsException("Character element at key $key does not exist.");
        }
        return $this->characters[$key];
    }

    /**
     * Get Characters collection.
     *
     * @return array
     */
    public function getCharacters() : array {
        return $this->characters;
    }

    /**
     * Set characters collection.
     *
     * @param array $characters
     */
    public function setCharacters(array $characters) {
        $this->characters = $characters;
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

As previously discussed, the purpose of the `OverflowException` -- at least, as it pertains to PHP -- is to alert the user or application that an element was added to a collection or container that doesn't have space for that extra item.  This _could_ result in a memory overflow, which is the traditional meaning of `"overflow"` in most languages, but PHP has much broader limitations on what the `OverflowException` can indicate.

To illustrate, we start with the trusty `Book` class that we've been using in previous code samples.  

```php
<?php
// Book.php

include("Character.php");

/**
 * Class Book
 */
class Book
{
    private $author;
    private $characters = [];
    private $pageCount;
    private $publicationDate;
    private $title;

    // Maximum byte length of author field.
    const AUTHOR_MAX_LENGTH = 255;
    // Maximum allow number of characters.
    const MAX_CHARACTER_COUNT = 5;
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
    public function getPublicationDate() : ?DateTime {
        return $this->publicationDate;
    }

    /**
     * Set the publication date.
     *
     * @param DateTime $date Publication date.
     */
    public function setPublicationDate(?DateTime $date) {
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
     * Add a Character to the Book.
     *
     * @param Character $character
     * @param mixed $key
     */
    public function addCharacter(Character $character, $key = null) {
        if (count($this->characters) >= self::MAX_CHARACTER_COUNT) {
            $max = self::MAX_CHARACTER_COUNT;
            throw new OverflowException("Character count cannot exceed maximum of $max");
        }
        if (!is_null($key)) {
            $this->characters[$key] = $character;
        } else {
            $this->characters[] = $character;
        }
    }

    /**
     * Get a Character using passed key.
     *
     * @param mixed $key
     * @return mixed
     */
    public function getCharacter($key) {
        if (!array_key_exists($key, $this->characters)) {
            throw new OutOfBoundsException("Character element at key $key does not exist.");
        }
        return $this->characters[$key];
    }

    /**
     * Get Characters collection.
     *
     * @return array
     */
    public function getCharacters() : array {
        return $this->characters;
    }

    /**
     * Set characters collection.
     *
     * @param array $characters
     */
    public function setCharacters(array $characters) {
        $this->characters = $characters;
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

Take particular notice of the `const MAX_CHARACTER_COUNT = 5;` statement, which indicates a limit on the allowable number of characters that can be added to a `Book` instance.  Obviously, no such limitation can (or should) exist in reality, but for our example, this will serve nicely.

Now, the `addCharacter(Character $character, $key = null)` method does just what the name suggests by adding the passed `Character` parameter to the `characters` field.  However, we want to limit the total allowed characters to five or fewer, so our code checks if the current maximum is reached or exceeded, in which case it throws a new `OverflowException`, which is the official purpose of this PHP exception:

```php
/**
* Add a Character to the Book.
*
* @param Character $character
* @param mixed $key
*/
public function addCharacter(Character $character, $key = null) {
    if (count($this->characters) >= self::MAX_CHARACTER_COUNT) {
        $max = self::MAX_CHARACTER_COUNT;
        throw new OverflowException("Character count cannot exceed maximum of $max");
    }
    if (!is_null($key)) {
        $this->characters[$key] = $character;
    } else {
        $this->characters[] = $character;
    }
}
```

You can probably guess where this is going.  To test this out, we've created a new `Book` instance for _The Name of the Wind_ (one of my personal all time favorites), then created a series of common `Characters` found throughout the book:

```php
function executeExamples() {
    try {
        $book = new Book('The Name of the Wind', 'Patrick Rothfuss', 662, new DateTime('2007-03-27'));

        $kvothe = new Character('Kvothe');
        $bast = new Character('Bast');
        $chronicler = new Character('Chronicler');
        $denna = new Character('Denna');
        $auri = new Character('Auri');
        $wilem = new Character('Wilem');

        $book->addCharacter($kvothe);
        $book->addCharacter($bast);
        $book->addCharacter($chronicler);
        $book->addCharacter($denna);
        $book->addCharacter($auri);

        Logging::LineSeparator("THE NAME OF THE WIND");
        Logging::Log($book);

        // ...
```

Having done that, we output the `$book` instance value to the log, to confirm all five initial characters were added:

```
--------- THE NAME OF THE WIND ---------
Book (5) (
    private 'author' -> string (16) "Patrick Rothfuss"
    private 'characters' -> array (5) [
        0 => Character (2) (
            private 'biography' -> null
            private 'name' -> string (6) "Kvothe"
        )
        1 => Character (2) (
            private 'biography' -> null
            private 'name' -> string (4) "Bast"
        )
        2 => Character (2) (
            private 'biography' -> null
            private 'name' -> string (10) "Chronicler"
        )
        3 => Character (2) (
            private 'biography' -> null
            private 'name' -> string (5) "Denna"
        )
        4 => Character (2) (
            private 'biography' -> null
            private 'name' -> string (4) "Auri"
        )
    ]
    private 'pageCount' -> integer 662
    private 'publicationDate' -> DateTime (3) (
        public 'date' -> string (26) "2007-03-27 00:00:00.000000"
        public 'timezone' -> string (3) "UTC"
        public 'timezone_type' -> integer 3
    )
    private 'title' -> string (20) "The Name of the Wind"
)
```

Just as intended, all `Characters` were added to the `$book` object.  However, let's see what happens if we try to add `Wilem`, the sixth `Character` to the collection:

```php
Logging::LineSeparator("ADDITIONAL CHARACTER");
Logging::Log($book->addCharacter($wilem));
```

Unsurprisingly, this produces the `OverflowException` we intended, indicating that the number of characters cannot exceed our maximum setting of `5`:

```
--------- ADDITIONAL CHARACTER ---------
[EXPECTED] OverflowException: Character count cannot exceed maximum of 5 in d:\work\Airbrake.io\lib\php\Book.php on line 136
```

Check out the <a class="js-cta-utm" href="https://airbrake.io/languages/php_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-php">Airbrake-PHP library</a>, designed to quickly and easily integrate into any PHP project, giving you and your team access to real-time error monitoring and reporting throughout your application's entire life cycle.  With automatic, instantaneous error and exception notifications at your fingertips, you'll be constantly aware of your application's health, including any issues that may arise.  Best of all, with Airbrake's robust web dashboard cataloging every error that occurs, you and your team can immediately dive into the exact details of what went wrong, making it easy to quickly recognize and resolve problems.

---

__META DESCRIPTION__

A brief overview of the OverflowException class in PHP, including functional code samples illustrating how to use this exception in your own code.

---

__SOURCES__

- http://php.net/manual/en/class.throwable.php