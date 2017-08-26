# PHP Exception Handling - LengthException

Making our way through our in-depth [__PHP Exception Handling__](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) series, on this fine day we'll be going over the LengthException in PHP.  `LengthException` is not a complex beast by any stretch of the imagination.  Instead, it's merely meant to be thrown when a value length should be considered invalid.

Throughout this article we'll explore the `LengthException` in more detail, by starting with a look at where it sits in the [PHP Exception Hierarchy](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy).  We'll also examine some functional sample code that will illustrate how `LengthExceptions` should be used in your own code, so away we go!

## The Technical Rundown

- All PHP errors implement the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface, or are extended from another inherited class therein.
- [`Exception`](http://php.net/manual/en/class.exception.php) implements the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface.
- [`LogicException`](http://php.net/manual/en/class.logicexception.php) extends the [`Exception`](http://php.net/manual/en/class.exception.php) class.
- `LengthException` extends the [`LogicException`](http://php.net/manual/en/class.logicexception.php) class.

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

    // Maximum byte length of author field.
    const AUTHOR_MAX_LENGTH = 255;
    // Maximum byte length of title field.
    const TITLE_MAX_LENGTH = 65535;

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
    Logging::LineSeparator("BASIC TEST");
    createBookTest("Just Some Title", "Just Some Author", 1);

    Logging::LineSeparator("TITLE_MAX_LENGTH TEST");
    createBookTest(str_repeat("a", Book::TITLE_MAX_LENGTH), "Just Some Author", 1);

    Logging::LineSeparator("TITLE_MAX_LENGTH + 1 TEST");
    createBookTest(str_repeat("a", Book::TITLE_MAX_LENGTH + 1), "Just Some Author", 1);

    Logging::LineSeparator("AUTHOR_MAX_LENGTH TEST");
    createBookTest("Just Some Title", str_repeat("a", Book::AUTHOR_MAX_LENGTH), 1);

    Logging::LineSeparator("AUTHOR_MAX_LENGTH + 1 TEST");
    createBookTest("Just Some Title", str_repeat("a", Book::AUTHOR_MAX_LENGTH + 1), 1);

    Logging::LineSeparator("AUTHOR_MAX_LENGTH KANJI TEST");
    createBookTest("Just Some Title", str_repeat("人", Book::AUTHOR_MAX_LENGTH), 1);

    Logging::LineSeparator("AUTHOR_MAX_LENGTH / 3 KANJI TEST");
    createBookTest("Just Some Title", str_repeat("人", Book::AUTHOR_MAX_LENGTH / 3), 1);
}

function createBookTest(string $title, string $author, int $pageCount) {
    try {
        // Create new Book instance.
        $book = new Book($title, $author, $pageCount);
        // Output created Book to log.
        Logging::Log($book);
    } catch (LengthException $exception) {
        // Output expected LengthException.
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

Most modern applications these days use some kind of data layer to retain permanent records, whether this be user credentials (not recommended) to cute puppy pictures (highly recommended).  If your application accepts user or third-party data in some way, it's paramount that the code provides sanity checks and data validations.  These can be as complex as your business logic demands, but the most basic data validation, which should be applied before it is sent to the database, is checking the _length_ of data.  Hence, this is where the use of the `LengthException` can come in quite handy when writing PHP applications.

For our example code today, we'll be extending my favorite `Book` example class, which contains a few simple properties: `author`, `title`, and `pageCount`.  For the sake of this tutorial, we've decided our database columns only need to handle a maximum of `255` bytes for the `author` value and `65,535` bytes for the `title`.  Maybe the CTO did some research and looked into the commonality of names and/or book titles exceeding those lengths, and decided those were smart limits.  Please play along for the sake of the example, since most modern applications would obviously not impose such strict limitations without extremely good reason.

Anyway, to accomplish such restrictions we've added some basic length validation logic to our `Book->setAuthor(string $author)` and `Book->setTitle(string $title)` methods:

```php
/**
 * Class Book
 */
class Book
{
    // ...

    // Maximum byte length of author field.
    const AUTHOR_MAX_LENGTH = 255;
    // Maximum byte length of title field.
    const TITLE_MAX_LENGTH = 65535;

    // ...

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

    // ...

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

    // ...
}
```

We've also defined our aforementioned maximum byte lengths as `const` values, to keep the code a bit cleaner.  As you can see, in the event that the [`strlen(string $string)`](http://php.net/manual/en/function.strlen.php) function returns a value that exceeds the defined maximum length, a new `LengthException` is thrown with a message indicating the problem.

In order to create multiple `Book` instances with varying `title` and `author` lengths, we've created a simple `createBookTest(string $title, string $author, int $pageCount)` function.  By creating a separate function with its own locally scoped `try-catch` block, we can safely catch any thrown exceptions without halting execution of the other `Book` creation calls.  This function simply passes along its parameters to the `Book` constructor and attempts to output the created `book` instance to the log:

```php
function createBookTest(string $title, string $author, int $pageCount) {
    try {
        // Create new Book instance.
        $book = new Book($title, $author, $pageCount);
        // Output created Book to log.
        Logging::Log($book);
    } catch (LengthException $exception) {
        // Output expected LengthException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}
```

Alright, now everything is setup, so let's test it out!  We start with a basic book, just to confirm everything works normally, as expected:

```php
function executeExamples()
{
    Logging::LineSeparator("BASIC TEST");
    createBookTest("Just Some Title", "Just Some Author", 1);
    
    // ...
}
```

Sure enough, everything looks good in the log output:

```
-------------- BASIC TEST --------------
Book (3) (
    private 'author' -> string (16) "Just Some Author"
    private 'pageCount' -> integer 1
    private 'title' -> string (15) "Just Some Title"
)
```

Now, let's try creating some books using our maximum length values for `title` and `author`, then also attempt a book creation that exceeds the maximum length of each of those by one:

```php
Logging::LineSeparator("TITLE_MAX_LENGTH TEST");
createBookTest(str_repeat("a", Book::TITLE_MAX_LENGTH), "Just Some Author", 1);

Logging::LineSeparator("TITLE_MAX_LENGTH + 1 TEST");
createBookTest(str_repeat("a", Book::TITLE_MAX_LENGTH + 1), "Just Some Author", 1);

Logging::LineSeparator("AUTHOR_MAX_LENGTH TEST");
createBookTest("Just Some Title", str_repeat("a", Book::AUTHOR_MAX_LENGTH), 1);

Logging::LineSeparator("AUTHOR_MAX_LENGTH + 1 TEST");
createBookTest("Just Some Title", str_repeat("a", Book::AUTHOR_MAX_LENGTH + 1), 1);
```

As you probably guessed, the attempts to create books with `title` and `author` values equal to the maximum worked fine, while those calls that exceeded the maximum threw `LengthExceptions` with our custom error message, as intended:

```
-------- TITLE_MAX_LENGTH TEST ---------
Book (3) (
    private 'author' -> string (16) "Just Some Author"
    private 'pageCount' -> integer 1
    private 'title' -> string (65535) "aaaaaaaaaaaaaa..."
)

------ TITLE_MAX_LENGTH + 1 TEST -------
[EXPECTED] LengthException: Cannot set Title containing 65536 bytes, which exceeds the maximum of 65535 by 1 bytes. in D:\work\Airbrake.io\Exceptions\PHP\Exception\LogicException\LengthException\code.php on line 97

-------- AUTHOR_MAX_LENGTH TEST --------
Book (3) (
    private 'author' -> string (255) "aaaaaaaaaaaaaa..."
    private 'pageCount' -> integer 1
    private 'title' -> string (15) "Just Some Title"
)

------ AUTHOR_MAX_LENGTH + 1 TEST ------
[EXPECTED] LengthException: Cannot set Author containing 256 bytes, which exceeds the maximum of 255 by 1 bytes. in D:\work\Airbrake.io\Exceptions\PHP\Exception\LogicException\LengthException\code.php on line 53
```

Let's try one last thing and throw in a less-common character into our repeated `author` argument value:

```php
Logging::LineSeparator("AUTHOR_MAX_LENGTH KANJI TEST");
createBookTest("Just Some Title", str_repeat("人", Book::AUTHOR_MAX_LENGTH), 1);
```

Here we're using the `人` kanji character, which commonly means "person."  Even though we are creating an `author` value of `Book::AUTHOR_MAX_LENGTH` length -- just as we did before -- now we're seeing another `LengthException`:

```
----- AUTHOR_MAX_LENGTH KANJI TEST -----
[EXPECTED] LengthException: Cannot set Author containing 765 bytes, which exceeds the maximum of 255 by 510 bytes. in D:\work\Airbrake.io\Exceptions\PHP\Exception\LogicException\LengthException\code.php on line 53
```

So what's going on?  The important lesson here is that the [`strlen(string $string)`](http://php.net/manual/en/function.strlen.php) function, which we're using to test various lengths, doesn't explicitly return the _character count_.  Instead, it returns the _number of bytes_ that make up that string.  For the common alphanumeric characters most Westerners use everyday, such as those belonging to the [Roman alphabet](https://simple.wikipedia.org/wiki/Roman_alphabet), the `character count` and the `byte count` are one in the same.  However, more complex characters require additional bytes to store their data.

As it happens, if you are keen with numbers, you may have determined that the number of bytes used by our kanji example (`765`) is exactly _three times_ the maximum number of `255`.  We can then deduce that each kanji character requires `3 bytes` to store.

Now we can modify our example by dividing the length of our string by `3`, which will ensure the number of **bytes** required by our created `author` string is equal to the `Book::AUTHOR_MAX_LENGTH`:

```php
Logging::LineSeparator("AUTHOR_MAX_LENGTH / 3 KANJI TEST");
createBookTest("Just Some Title", str_repeat("人", Book::AUTHOR_MAX_LENGTH / 3), 1);
```

Sure enough, this works just fine and creates a new `Book` instance in the output:

```
--- AUTHOR_MAX_LENGTH / 3 KANJI TEST ---
Book (3) (
    private 'author' -> UTF-8 string (255) "人人人人人人人人人人人人人人人..."
    private 'pageCount' -> integer 1
    private 'title' -> string (15) "Just Some Title"
)
```

Check out the <a class="js-cta-utm" href="https://airbrake.io/languages/php_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-php">Airbrake-PHP library</a>, designed to quickly and easily integrate into any PHP project, giving you and your team access to real-time error monitoring and reporting throughout your application's entire life cycle.  With automatic, instantaneous error and exception notifications at your fingertips, you'll be constantly aware of your application's health, including any issues that may arise.  Best of all, with Airbrake's robust web dashboard cataloging every error that occurs, you and your team can immediately dive into the exact details of what went wrong, making it easy to quickly recognize and resolve problems.

---

__META DESCRIPTION__

A close look at the LengthException classes in PHP, including code samples illustrating how to properly throw these exceptions in your own code.
---

__SOURCES__

- http://php.net/manual/en/class.throwable.php