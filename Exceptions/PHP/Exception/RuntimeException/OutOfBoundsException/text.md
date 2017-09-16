# PHP Exception Handling - OutOfBoundsException

Making our way through our detailed [__PHP Exception Handling__](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) series, today we come to the **OutOfBoundsException**.  The `OutOfBoundsException` is not to be confused with the `OutOfRangeException`, which we covered a few weeks ago in our [`PHP Exception Handling - OutOfRangeException`](https://airbrake.io/blog/php-exception-handling/outofrangeexception) article.  While `OutOfRangeException` is meant to be used at `compile time`, the `OutOfBoundsException` inherits directly from the [`RuntimeException`](http://php.net/manual/en/class.runtimeexception.php) and, thus, it is used for key-based errors that occur during `runtime`.

We'll start this article by looking at where the `OutOfBoundsException` fits into the larger [PHP Exception Hierarchy](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy).  Then, we'll explore some functional code samples that illustrate how the `OutOfBoundsException` is typically thrown, or is simply meant to be used, so let's get going!

## The Technical Rundown

All PHP errors implement the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy)
    - [`Exception`](http://php.net/manual/en/class.exception.php)
        - [`RuntimeException`](http://php.net/manual/en/class.runtimeexception.php)
            - `OutOfBoundsException`

## Full Code Sample

Below is the full code sample we’ll be using in this article.  Feel free to use any or all of the code if you wish to follow along.

```php
<?php

include("Book.php");
include("Logging.php");

function executeExamples() {
    try {
        $book = new Book('A Game of Thrones', 'George R.R. Martin', 848, new DateTime('2005-08-06'));

        $tyrion = new Character('Tyrion Lannister', 'Badass little guy with a big heart.');
        $daenerys = new Character('Daenerys Targaryen', 'Mother of Dragons, Breaker of Chains, yadda yadda yadda');
        $jon = new Character('Jon Snow', 'Sorta emo, but people seem to like him.');

        $book->addCharacter($tyrion, 'tyrion');
        $book->addCharacter($daenerys);
        $book->addCharacter($jon);

        Logging::LineSeparator("A GAME OF THRONES");
        Logging::Log($book);

        Logging::LineSeparator("INVALID KEY");
        Logging::Log($book->getCharacter('tyrone'));
    } catch (OutOfBoundsException $exception) {
        // Output expected OutOfBoundsExceptions.
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
// Character.php
/**
 * Class Character
 */
class Character
{
    private $name;
    private $biography;

    /**
     * Character constructor.
     *
     * @param string $name Character name.
     * @param string $biography Character biography.
     */
    public function __construct(string $name, string $biography = null) {
        $this->setName($name);
        $this->setBiography($biography);
    }

    /**
     * Get the character biography.
     *
     * @return mixed Character biography.
     */
    public function getBiography(): ?string {
        return $this->biography;
    }

    /**
     * Set the character biography.
     *
     * @param int $pageCount Biography to set.
     */
    public function setBiography(?string $biography) {
        $this->biography = $biography;
    }

    /**
     * Get the name.
     *
     * @return string Character name.
     */
    public function getName(): string {
        return $this->name;
    }

    /**
     * Set the name.
     *
     * @param string $value Name value to be set.
     */
    public function setName(string $value) {
        $this->name = $value;
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

The [official documentation](http://php.net/manual/en/class.outofboundsexception.php) states that the `OutOfBoundsException` "represents errors that cannot be detected at compile time."  In effect, this means it is ideal for errors that are not caused by a _logical_ issue, but instead are due to invalid data that appears during runtime.  For example, a database that retrieves a value that is used elsewhere in the code could produce an `OutOfBoundsException`, since that particular value may be invalid or "outside of the bounds" of what is allowed.

To illustrate this principle, we're expanding a bit on our trusty `Book` class by adding a `characters` property, which holds a series of `Character` class instances that represent some of the primary characters within the `Book`.  For our example, we'll be creating and retrieving `Characters` directly in the code, but this data could just as easily come from a third-party, such as a database or service API.  Regardless, we can use this example to illustrate how, in some situations, we need to plan for attempts to access data that doesn't exist in a data set, and to respond at runtime with an appropriate `OutOfBoundsException`.

We begin with the new `Character` class:

```php
<?php
// Character.php
/**
 * Class Character
 */
class Character
{
    private $name;
    private $biography;

    /**
     * Character constructor.
     *
     * @param string $name Character name.
     * @param string $biography Character biography.
     */
    public function __construct(string $name, string $biography = null) {
        $this->setName($name);
        $this->setBiography($biography);
    }

    /**
     * Get the character biography.
     *
     * @return mixed Character biography.
     */
    public function getBiography(): ?string {
        return $this->biography;
    }

    /**
     * Set the character biography.
     *
     * @param int $pageCount Biography to set.
     */
    public function setBiography(?string $biography) {
        $this->biography = $biography;
    }

    /**
     * Get the name.
     *
     * @return string Character name.
     */
    public function getName(): string {
        return $this->name;
    }

    /**
     * Set the name.
     *
     * @param string $value Name value to be set.
     */
    public function setName(string $value) {
        $this->name = $value;
    }
}
```

Nothing fancy going on here.  We just need to track the `Character's` `name` and (optional) `biography`.

Now we need a way to add `Characters` to a `Book` instance.  We accomplish this by adding four new methods to the `Book` class:

```php
/**
* Add a Character to the Book.
*
* @param Character $character
* @param mixed $key
*/
public function addCharacter(Character $character, $key = null) {
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
```

The `addCharacter(Character $character, $key = null)` method adds a new `Character`, either to the end of the array or to the specified `$key`.  `getCharacter($key)` retrieves the `Character` element at the specified `$key`.  However, we don't know at compile time whether the passed `$key` will be valid or not, so we need to check if it exists in the `characters` collection property.  If not, we throw a new `OutOfBoundsException` informing the user of the issue.  Finally, `getCharacters()` and `setCharacters(array $characters)` are the standard property getter and setter methods.

Now, let's test this out to make sure everything works as expected.  The `executeExamples()` method contains all our test code, including the `try-catch` block to handle any expected `OutOfBoundsExceptions`:

```php
function executeExamples() {
    try {
        $book = new Book('A Game of Thrones', 'George R.R. Martin', 848, new DateTime('2005-08-06'));

        $tyrion = new Character('Tyrion Lannister', 'Badass little guy with a big heart.');
        $daenerys = new Character('Daenerys Targaryen', 'Mother of Dragons, Breaker of Chains, yadda yadda yadda');
        $jon = new Character('Jon Snow', 'Sorta emo, but people seem to like him.');

        $book->addCharacter($tyrion, 'tyrion');
        $book->addCharacter($daenerys);
        $book->addCharacter($jon);

        Logging::LineSeparator("A GAME OF THRONES");
        Logging::Log($book);

        Logging::LineSeparator("INVALID KEY");
        Logging::Log($book->getCharacter('tyrone'));
    } catch (OutOfBoundsException $exception) {
        // Output expected OutOfBoundsExceptions.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}
```

We start by creating a new `Book` representing _A Game of Thrones_.  Next, we create a trio of `Characters` representing three main characters in the book, then call `$book->addCharacter(Character $character, $key = null)` to add them all.  Just to confirm we can manually specify a `$key` argument, we passed `'tyrion'` as the `$key` parameter for his entry.  After that, we output the `$book` instance to the log to see that everything has been added as expected:

```
---------- A GAME OF THRONES -----------
Book (5) (
    private 'author' -> string (18) "George R.R. Martin"
    private 'characters' -> array (3) [
        'tyrion' => Character (2) (
            private 'biography' -> string (35) "Badass little guy with a big heart."
            private 'name' -> string (16) "Tyrion Lannister"
        )
        0 => Character (2) (
            private 'biography' -> string (55) "Mother of Dragons, Breaker of Chains, yadda yadda yadda"
            private 'name' -> string (18) "Daenerys Targaryen"
        )
        1 => Character (2) (
            private 'biography' -> string (39) "Sorta emo, but people seem to like him."
            private 'name' -> string (8) "Jon Snow"
        )
    ]
    private 'pageCount' -> integer 848
    private 'publicationDate' -> DateTime (3) (
        public 'date' -> string (26) "2005-08-06 00:00:00.000000"
        public 'timezone' -> string (3) "UTC"
        public 'timezone_type' -> integer 3
    )
    private 'title' -> string (17) "A Game of Thrones"
)
```

Everything looks just right!  Now, let's see what happens if we call `$book->GetCharacter('tyrone')`, which represents an invalid `$key` within the `characters` array property:

```php
------------- INVALID KEY --------------
[EXPECTED] OutOfBoundsException: Character element at key tyrone does not exist. in D:\work\Airbrake.io\lib\php\Book.php on line 147
```

Just as intended, an `OutOfBoundsException` is thrown, indicating a `Character` element at the key `'tyrone'` doesn't exist.

Check out the <a class="js-cta-utm" href="https://airbrake.io/languages/php_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-php">Airbrake-PHP library</a>, designed to quickly and easily integrate into any PHP project, giving you and your team access to real-time error monitoring and reporting throughout your application's entire life cycle.  With automatic, instantaneous error and exception notifications at your fingertips, you'll be constantly aware of your application's health, including any issues that may arise.  Best of all, with Airbrake's robust web dashboard cataloging every error that occurs, you and your team can immediately dive into the exact details of what went wrong, making it easy to quickly recognize and resolve problems.

---

__META DESCRIPTION__

A close look the OutOfBoundsException class in PHP, including functional code samples illustrating how best to use this exception in your own code.
---

__SOURCES__

- http://php.net/manual/en/class.throwable.php