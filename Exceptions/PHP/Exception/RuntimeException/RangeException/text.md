# PHP Exception Handling - RangeException

Moving along through our detailed [__PHP Exception Handling__](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) series, today we'll be going over the **RangeException**.   The `RangeException` is similar to the [`DomainException`, which we covered previously](https://airbrake.io/blog/php-exception-handling/domainexception).  In essence, the `DomainException` is used when _input_ values (such as method arguments) are invalid and don't fit the context or _domain_ of the current code.  On the other hand, `RangeExceptions` are ideal when _output_ values are invalid, or otherwise don't make sense.

In this article we'll examine the `RangeException` by first looking at where it resides in the overall [PHP Exception Hierarchy](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy).  We'll then take a look at some functional code samples that illustrate the difference between `DomainExceptions` and `RangeExceptions`, and show how `RangeExceptions` should be used in your own code, so let's get to it!

## The Technical Rundown

All PHP errors implement the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy)
    - [`Exception`](http://php.net/manual/en/class.exception.php)
        - [`RuntimeException`](http://php.net/manual/en/class.runtimeexception.php)
            - `RangeException`

## Full Code Sample

Below is the full code sample we’ll be using in this article.  Feel free to use any or all of the code if you wish to follow along.

```php
<?php

include("D:\work\Airbrake.io\lib\php\Logging.php");
include("D:\work\Airbrake.io\lib\php\Book.php");
include('Publication.php');

function executeExamples()
{
    Logging::LineSeparator("VALID PUBLICATION TYPE");
    setValidPublicationType();

    Logging::LineSeparator("INVALID PUBLICATION TYPE");
    setInvalidPublicationType();

    Logging::LineSeparator("ASSIGN PUBLICATION TYPE");
    assignPublicationType();
}

function setValidPublicationType() {
    try {
        // Create new Publication instance with a valid publication type.
        $publication = new Publication("A Game of Thrones", "George R. R. Martin", 'digital', 848, new DateTime('1996-08-06'));
        Logging::Log($publication);
    } catch (DomainException $exception) {
        // Output expected DomainException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

function setInvalidPublicationType() {
    try {
        // Create new Publication instance with an invalid publication type.
        $publication = new Publication("A Clash of Kings", "George R. R. Martin", 'poem', 761, new DateTime('1998-11-16'));
        Logging::Log($publication);
    } catch (DomainException $exception) {
        // Output expected DomainException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

function assignPublicationType() {
    try {
        // Create new Publication instance with an invalid publication type.
        $publication = new Publication("A Storm of Swords", "George R. R. Martin", 'novel', 1177, new DateTime('2000-08-08'));
        // Output valid publication.
        Logging::Log($publication);
        // Directly modify publicationType to invalid type.
        $publication->publicationType = 'epic';
        // Output current publication type.
        Logging::Log($publication->getPublicationType());
    } catch (RangeException $exception) {
        // Output expected RangeException.
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
     * @param DateTime $publicationDate Book publication date.
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
// Publication.php
/**
 * Class Publication
 */
class Publication extends Book
{
    const PublicationTypes = [
        'audio',
        'blog',
        'digital',
        'novel',
    ];

    public $publicationType;

    /**
     * Publication constructor.
     *
     * @param Publication|string $title Publication title.
     * @param Publication|string $author Publication author.
     * @param string $publicationType Publication type.
     * @param int $pageCount Publication page count.
     * @param DateTime $publicationDate Publication publication date.
     */
    public function __construct(string $title, string $author, string $publicationType = null, int $pageCount = 0, DateTime $publicationDate = null) {
        parent::__construct($title, $author, $pageCount, $publicationDate);
        $this->setPublicationType($publicationType);
    }

    /**
     * Set the publication type.
     *
     * @return string Publication type.
     */
    public function getPublicationType(): ?string {
        // Check if current type is in valid types list.
        if (in_array($this->publicationType, Publication::PublicationTypes)) {
            // Return valid type.
            return $this->publicationType;
        } else {
            // If current type is invalid, throw RangeException.
            throw new RangeException("Publication set to unknown type: $this->publicationType");
        }
    }

    /**
     * Get the publication type.
     *
     * @param string $publicationType Publication type value to be set.
     */
    public function setPublicationType(string $publicationType = null) {
        if ($publicationType == null) return;
        // Check if passed type is in valid types list.
        if (in_array($publicationType, Publication::PublicationTypes)) {
            // Set publication type.
            $this->publicationType = $publicationType;
        } else {
            // If passed type not found in valid list, throw Domain Exception.
            throw new DomainException("Cannot set publication type to unknown type: $publicationType");
        }
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

As we saw in our previous [PHP Exception Handling - DomainException](https://airbrake.io/blog/php-exception-handling/domainexception) article, we integrated the `DomainException` into the _input_ of a value called `PublicationType` for our `Publication` class.  Specifically, when the `setPublicationType(string $publicationType = null)` method is called, if the `$publicationType` parameter isn't a valid value, a new `DomainException` is thrown to indicate to the client that something needs to be changed.

To explore the `RangeException`, we've extended this example a bit by continuing with the `Publication` class, which extends the `Book` class:

```php
<?php
// Publication.php
/**
 * Class Publication
 */
class Publication extends Book
{
    const PublicationTypes = [
        'audio',
        'blog',
        'digital',
        'novel',
    ];

    public $publicationType;

    /**
     * Publication constructor.
     *
     * @param Publication|string $title Publication title.
     * @param Publication|string $author Publication author.
     * @param string $publicationType Publication type.
     * @param int $pageCount Publication page count.
     * @param DateTime $publicationDate Publication publication date.
     */
    public function __construct(string $title, string $author, string $publicationType = null, int $pageCount = 0, DateTime $publicationDate = null) {
        parent::__construct($title, $author, $pageCount, $publicationDate);
        $this->setPublicationType($publicationType);
    }

    /**
     * Set the publication type.
     *
     * @return string Publication type.
     */
    public function getPublicationType(): ?string {
        // Check if current type is in valid types list.
        if (in_array($this->publicationType, Publication::PublicationTypes)) {
            // Return valid type.
            return $this->publicationType;
        } else {
            // If current type is invalid, throw RangeException.
            throw new RangeException("Publication set to unknown type: $this->publicationType");
        }
    }

    /**
     * Get the publication type.
     *
     * @param string $publicationType Publication type value to be set.
     */
    public function setPublicationType(string $publicationType = null) {
        if ($publicationType == null) return;
        // Check if passed type is in valid types list.
        if (in_array($publicationType, Publication::PublicationTypes)) {
            // Set publication type.
            $this->publicationType = $publicationType;
        } else {
            // If passed type not found in valid list, throw Domain Exception.
            throw new DomainException("Cannot set publication type to unknown type: $publicationType");
        }
    }
}
```

In addition to the data checking within `setPublicationType(string $publicationType = null)`, we now have data validation going on in the `getPublicationType()` method.  Here, we ensure that the current value of the `publicationType` property is found within the `Publication::PublicationTypes` array and, if not, a new `RangeException` is thrown.  As discussed in the introduction, this distinction between an _input_ and an _output_ value is what differentiates the use of `DomainException` from `RangeException`.  In this case, we're ensuring that the `publicationType` property value is valid _before_ outputting it as a result of the `getPublicationType()` method.

To test this and show how `RangeExceptions` differ from `DomainExceptions` in practice, we have three test methods, starting with `setValidPublicationDate()`:

```php
function setValidPublicationType() {
    try {
        // Create new Publication instance with a valid publication type.
        $publication = new Publication("A Game of Thrones", "George R. R. Martin", 'digital', 848, new DateTime('1996-08-06'));
        Logging::Log($publication);
    } catch (DomainException $exception) {
        // Output expected DomainException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}
```

Invoking this method works as expected, since the `digital` `publicationType` value is valid.  The new `Publication` is created and output to the log:

```
-------- VALID PUBLICATION TYPE --------
Publication (6) (
    public 'publicationType' -> string (7) "digital"
    private 'author' -> string (19) "George R. R. Martin"
    private 'characters' -> array (0) []
    private 'pageCount' -> integer 848
    private 'publicationDate' -> DateTime (3) (
        public 'date' -> string (26) "1996-08-06 00:00:00.000000"
        public 'timezone' -> string (3) "UTC"
        public 'timezone_type' -> integer 3
    )
    private 'title' -> string (17) "A Game of Thrones"
)
```

Next we have the `setInvalidPublicationType()` method, which passes an invalid `publicationType` of `poem` to the `setPublicationType(string $publicationType = null)` method within the `Publication` class constructor:

```php
function setInvalidPublicationType() {
    try {
        // Create new Publication instance with an invalid publication type.
        $publication = new Publication("A Clash of Kings", "George R. R. Martin", 'poem', 761, new DateTime('1998-11-16'));
        Logging::Log($publication);
    } catch (DomainException $exception) {
        // Output expected DomainException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}
```

As we saw earlier, since this invalid `publicationType` is an _input_ value, we should expect a `DomainException` to be thrown, which is confirmed by the output we see:

```
------- INVALID PUBLICATION TYPE -------
[EXPECTED] DomainException: Cannot set publication type to unknown type: poem in D:\work\Airbrake.io\Exceptions\PHP\Exception\RuntimeException\RangeException\Publication.php on line 60
```

Finally, let's invoke the `assignPublicationType()` method, which initially sets a valid `publicationType` of `novel`, but then _directly_ modifies the `publicationType` property to a value of `epic`, before trying to retrieve this now-invalid value via `getPublicationType()`:

```php
function assignPublicationType() {
    try {
        // Create new Publication instance with an invalid publication type.
        $publication = new Publication("A Storm of Swords", "George R. R. Martin", 'novel', 1177, new DateTime('2000-08-08'));
        // Output valid publication.
        Logging::Log($publication);
        // Directly modify publicationType to invalid type.
        $publication->publicationType = 'epic';
        // Output current publication type.
        Logging::Log($publication->getPublicationType());
    } catch (RangeException $exception) {
        // Output expected RangeException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}
```

It's worth noting that this specific example is somewhat contrived, because normally we'd have other defenses setup to combat direct modification of the `publicationType` property.  For example, it would be set to `private` visibility (instead of its current `public` setting), which would normally force client code to use the `setPublicationType(string $publicationType = null)` method to make changes.

At any rate, directly changing the `publicationType` property and then trying to retrieve it throws a `RangeException`, as expected, indicating that that the _output_ value we're trying to retrieve is invalid:

```
------- ASSIGN PUBLICATION TYPE --------
Publication (6) (
    public 'publicationType' -> string (5) "novel"
    private 'author' -> string (19) "George R. R. Martin"
    private 'characters' -> array (0) []
    private 'pageCount' -> integer 1177
    private 'publicationDate' -> DateTime (3) (
        public 'date' -> string (26) "2000-08-08 00:00:00.000000"
        public 'timezone' -> string (3) "UTC"
        public 'timezone_type' -> integer 3
    )
    private 'title' -> string (17) "A Storm of Swords"
)

[EXPECTED] RangeException: Publication set to unknown type: epic in D:\work\Airbrake.io\Exceptions\PHP\Exception\RuntimeException\RangeException\Publication.php on line 43
```

Check out the <a class="js-cta-utm" href="https://airbrake.io/languages/php_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-php">Airbrake-PHP library</a>, designed to quickly and easily integrate into any PHP project, giving you and your team access to real-time error monitoring and reporting throughout your application's entire life cycle.  With automatic, instantaneous error and exception notifications at your fingertips, you'll be constantly aware of your application's health, including any issues that may arise.  Best of all, with Airbrake's robust web dashboard cataloging every error that occurs, you and your team can immediately dive into the exact details of what went wrong, making it easy to quickly recognize and resolve problems.

---

__META DESCRIPTION__

A deep dive into the PHP RangeException class, including code samples showing the difference between DomainException and RangeException.
---

__SOURCES__

- http://php.net/manual/en/class.throwable.php