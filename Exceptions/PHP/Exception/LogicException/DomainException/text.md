# PHP Exception Handling - DomainException

Next up in our in-depth [__PHP Exception Handling__](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) series we'll be looking into the domain of the, well, DomainException.  The specific scenarios in with a `DomainException` should be used are varied and somewhat debated, but the current best practice is to throw a `DomainException` when a value doesn't adhere to the valid "data domain" of the given context.

In this article we'll further explore what that means and how the `DomainException` is typically used by looking at some simple, functional code samples.  We'll also see where the `DomainException` sits within the [PHP Exception Hierarchy](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy).  Without further ado, let the games begin!

## The Technical Rundown

- All PHP errors implement the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface, or are extended from another inherited class therein.
- [`Exception`](http://php.net/manual/en/class.exception.php) implements the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface.
- [`LogicException`](http://php.net/manual/en/class.logicexception.php) extends the [`Exception`](http://php.net/manual/en/class.exception.php) class.
- `DomainException` extends the [`LogicException`](http://php.net/manual/en/class.logicexception.php) class.

## Full Code Sample

Below is the full code sample we’ll be using in this article.  Feel free to use any or all of the code if you wish to follow along.

```php
<?php

/**
 * Class Publication
 */
class Publication
{
    const PublicationTypes = [
        'audio',
        'blog',
        'digital',
        'novel',
    ];

    private $author;
    private $publicationType;
    private $title;

    /**
     * Publication constructor.
     *
     * @param Publication|string $title Publication title.
     * @param Publication|string $author Publication author.
     * @param string $publicationType Publication type.
     */
    public function __construct(string $title, string $author, string $publicationType) {
        $this->setAuthor($author);
        $this->setPublicationType($publicationType);
        $this->setTitle($title);
    }

    /**
     * Get the author.
     *
     * @return string Publication author.
     */
    public function getAuthor(): ?string {
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
     * Set the publication type.
     *
     * @return string Publication type.
     */
    public function getPublicationType(): ?string {
        return $this->publicationType;
    }

    /**
     * Get the publication type.
     *
     * @param string $publicationType Publication type value to be set.
     */
    public function setPublicationType(string $publicationType) {
        // Check if passed type is in valid types list.
        if (in_array($publicationType, Publication::PublicationTypes)) {
            // Set publication type.
            $this->publicationType = $publicationType;
        } else {
            // If passed type not found in valid list, throw Domain Exception.
            throw new DomainException("Cannot set publication type to unknown type: $publicationType");
        }
    }

    /**
     * Get the title.
     *
     * @return string Publication title.
     */
    public function getTitle(): ?string {
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
    Logging::LineSeparator("VALID PUBLICATION TYPE");
    setValidPublicationType();

    Logging::LineSeparator("INVALID PUBLICATION TYPE");
    setInvalidPublicationType();
}

function setValidPublicationType() {
    try {
        // Create new Publication instance with a valid publication type.
        $publication = new Publication("A Game of Thrones", "George R. R. Martin", 'digital');
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
        $publication = new Publication("A Storm of Swords", "George R. R. Martin", 'poem');
        Logging::Log($publication);
    } catch (DomainException $exception) {
        // Output expected DomainException.
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

Although it's a bit dated at this point, an interesting discussion took place a few years back in the [official PHP bug tracker](https://bugs.php.net/bug.php?id=47097) _specifically_ regarding the vagueness of `DomainException's` purpose.  Even many _contributing developers_ of the PHP codebase at the time weren't entirely sure what the `DomainException` was intended to accomplish.  As mentioned in the introduction, the best explanation at the time (and that persists in the documentation today) is that a `DomainException` should be thrown when a value is passed to a method that is outside the bounds or "domain" of that data type.

Critically, this scenario is subtlety, yet distinctly, different from when an `InvalidArgumentException` would be used.  An `InvalidArgumentException` should be thrown when an argument is not of the proper `type` -- for example, trying to pass a `string` as an argument that expects an `int` type instead.

Conversely, the `DomainException` is better suited to handling an argument that is of the proper `type`, but is otherwise outside of the valid, expected values.  In other words, the value is not within the target "domain."

To see how a `DomainException` might be used in real world code we have the `Publication` class, as seen below:

```php
/**
 * Class Publication
 */
class Publication
{
    const PublicationTypes = [
        'audio',
        'blog',
        'digital',
        'novel',
    ];

    private $author;
    private $publicationType;
    private $title;

    /**
     * Publication constructor.
     *
     * @param Publication|string $title Publication title.
     * @param Publication|string $author Publication author.
     * @param string $publicationType Publication type.
     */
    public function __construct(string $title, string $author, string $publicationType) {
        $this->setAuthor($author);
        $this->setPublicationType($publicationType);
        $this->setTitle($title);
    }

    /**
     * Get the author.
     *
     * @return string Publication author.
     */
    public function getAuthor(): ?string {
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
     * Set the publication type.
     *
     * @return string Publication type.
     */
    public function getPublicationType(): ?string {
        return $this->publicationType;
    }

    /**
     * Get the publication type.
     *
     * @param string $publicationType Publication type value to be set.
     */
    public function setPublicationType(string $publicationType) {
        // Check if passed type is in valid types list.
        if (in_array($publicationType, Publication::PublicationTypes)) {
            // Set publication type.
            $this->publicationType = $publicationType;
        } else {
            // If passed type not found in valid list, throw Domain Exception.
            throw new DomainException("Cannot set publication type to unknown type: $publicationType");
        }
    }

    /**
     * Get the title.
     *
     * @return string Publication title.
     */
    public function getTitle(): ?string {
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

This class contains a few properties, but the critical components are the `const PublicationTypes` array and the `setPublicationType(string $publicationType)` method.  The latter of these performs a basic logic check to determine if the passed `$publicationType` value is found within the valid list of `PublicationTypes`.  If so, it sets the instance property value.  However, if the passed type cannot be found in the list, a new `DomainException` is thrown indicating the problem.  Again, this isn't an appropriate scenario for an `InvalidArgumentException` because the `type` that was passed is valid (`string`).  Instead, we're throwing a `DomainException` because the value itself doesn't meet our requirements (in this case, it's missing from the valid types list).

To test this functionality out and make sure everything is working we have two functions, `setValidPublicationType()` and `setInvalidPublicationType()`:

```php
function executeExamples()
{
    Logging::LineSeparator("VALID PUBLICATION TYPE");
    setValidPublicationType();

    Logging::LineSeparator("INVALID PUBLICATION TYPE");
    setInvalidPublicationType();
}

function setValidPublicationType() {
    try {
        // Create new Publication instance with a valid publication type.
        $publication = new Publication("A Game of Thrones", "George R. R. Martin", 'digital');
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
        $publication = new Publication("A Storm of Swords", "George R. R. Martin", 'poem');
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

In the first function we're using a valid `PublicationType` of `digital`.  On the other hand, the second `Publication` call passed an invalid `PublicationType` of `poem`.  The log output shows just what we expected: The first function behaves fine and the second throws a `DomainException` our way:

```
-------- VALID PUBLICATION TYPE --------
Publication (3) (
    private 'author' -> string (19) "George R. R. Martin"
    private 'publicationType' -> string (7) "digital"
    private 'title' -> string (17) "A Game of Thrones"
)

------- INVALID PUBLICATION TYPE -------
[EXPECTED] DomainException: Cannot set publication type to unknown type: poem in D:\work\Airbrake.io\Exceptions\PHP\Exception\LogicException\DomainException\code.php on line 73
```

Check out the <a class="js-cta-utm" href="https://airbrake.io/languages/php_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-php">Airbrake-PHP library</a>, designed to quickly and easily integrate into any PHP project, giving you and your team access to real-time error monitoring and reporting throughout your application's entire life cycle.  With automatic, instantaneous error and exception notifications at your fingertips, you'll be constantly aware of your application's health, including any issues that may arise.  Best of all, with Airbrake's robust web dashboard cataloging every error that occurs, you and your team can immediately dive into the exact details of what went wrong, making it easy to quickly recognize and resolve problems.

---

__META DESCRIPTION__

A closer look at the PHP DomainException class, including code samples showing the difference between DomainException and InvalidArgumentException.
---

__SOURCES__

- http://php.net/manual/en/class.throwable.php