# PHP Exception Handling - UnexpectedValueException

Traversing the depths of our detailed [__PHP Exception Handling__](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) series, today we'll dive into the **UnexpectedValueException**.  Not to be confused with the [`InvalidArgumentException`](https://airbrake.io/blog/php-exception-handling/invalidargumentexception) that we [covered in a previous article](https://airbrake.io/blog/php-exception-handling/invalidargumentexception), the `UnexpectedValueException` is typically thrown when a value is not within an expected set of values.  For example, if a function is returning a date value, but the runtime execution of this function produces a malformed date, you may opt to throw an `UnexpectedValueException`, so further execution using this value isn't corrupted by it.

In this article we'll examine the `UnexpectedValueException` by first looking at where it resides in the overall [PHP Exception Hierarchy](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy).  We'll also look through some fully-functional sample code that illustrates how you might use `UnexpectedValueExceptions` in your own code, along with how this exception subtly differs from the [`InvalidArgumentException`](https://airbrake.io/blog/php-exception-handling/invalidargumentexception), so let's get exploring!

## The Technical Rundown

All PHP errors implement the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy)
    - [`Exception`](http://php.net/manual/en/class.exception.php)
        - [`RuntimeException`](http://php.net/manual/en/class.runtimeexception.php)
            - `UnexpectedValueException`

## Full Code Sample

Below is the full code sample we’ll be using in this article.  Feel free to use any or all of the code if you wish to follow along.

```php
<?php
/**
 * Class Publisher
 */
class Publisher {
    private $name;

    /**
     * Publisher constructor.
     *
     * @param string $name Publisher name.
     */
    public function __construct(string $name) {
        $this->setName($name);
    }

    /**
     * Set publisher name.
     *
     * @return string Publisher name.
     */
    public function getName(): string
    {
        return $this->name;
    }

    /**
     * Get the publisher name.
     *
     * @param string $name Publisher name.
     */
    public function setName(string $name)
    {
        // Check that name is Pascal case.
        if (ucwords($name) != $name) {
            throw new InvalidArgumentException("Publisher->name must be Pascal Case; passed name is invalid: {$name}");
        }
        $this->name = $name;
    }
}
```

```php
/**
 * Class Book
 */
class Book
{
    private $author;
    private $pageCount;
    private $publisher;
    private $title;

    // Maximum page count size.
    const MAX_PAGE_COUNT = 9999;

    /**
     * Book constructor.
     *
     * @param Book|string $title Book title.
     * @param Book|string $author Book author.
     * @param Book|int $pageCount Book page count.
     * @param Publisher $publisher Book publisher.
     */
    public function __construct(string $title, string $author, int $pageCount, Publisher $publisher) {
        $this->setAuthor($author);
        $this->setPageCount($pageCount);
        $this->setPublisher($publisher);
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
        // Confirm that page count is an integer.
        if (!is_integer($pageCount)) {
            // If not, throw a new InvalidArgumentException.
            $type = gettype($pageCount);
            throw new InvalidArgumentException("Page Count cannot be set to type ({$type}), must be an integer.");
        }
        // Check if page count within bounds.
        if ($pageCount > self::MAX_PAGE_COUNT) {
            $maxPageCount = self::MAX_PAGE_COUNT;
            throw new UnexpectedValueException("Page Count of ({$pageCount}) is invalid, cannot exceed MAX_PAGE_COUNT of ({$maxPageCount}).");
        }
        $this->pageCount = $pageCount;
    }

    /**
     * Get the publisher.
     *
     * @return Publisher Book publisher.
     */
    public function getPublisher(): Publisher {
        return $this->publisher;
    }

    /**
     * Set the publisher.
     *
     * @param Publisher $publisher Book publisher.
     */
    public function setPublisher(Publisher $publisher) {
        // Confirm that publisher is correct type.
        if (gettype($publisher) == 'object') {
            $class = get_class($publisher);
            if ($class != 'Publisher') {
                // Not a Publisher, so throw a new InvalidArgumentException.
                throw new InvalidArgumentException("Publisher cannot be set to type ({$class}), must be a Publisher object.");
            }
        } else {
            $type = gettype($publisher);
            // Not an object, so cannot be Publisher.
            throw new InvalidArgumentException("Publisher cannot be set to type ({$type}), must be a Publisher object.");
        }
        $this->publisher = $publisher;
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

    /**
     * Get the string representation of Book instance.
     *
     * @return mixed Book string representation.
     */
    public function __toString()
    {
        return "'{$this->getTitle()}' by {$this->getAuthor()} at {$this->getPageCount()} pgs, published by {$this->getPublisher()->getName()}";
    }
}
```


```php
function executeExamples()
{
    Logging::LineSeparator("VALID BOOK");
    createValidBook();
    Logging::LineSeparator("EXCESSIVE PAGE COUNT");
    createBookWithExcessivePageCount();
    Logging::LineSeparator("INVALID BOOK");
    $invalidBook = getInvalidBook();
}

function createValidBook() {
    try {
        // Create new Book instance.
        $book = new Book("Lord of the Flies", "William Golding", 182, new Publisher("Faber & Faber"));
        // Output resulting book.
        Logging::Log($book);
    } catch (UnexpectedValueException $exception) {
        // Output expected UnexpectedValueException.
        Logging::Log($exception);
    } catch (InvalidArgumentException $exception) {
        // Output unexpected InvalidArgumentExceptions.
        Logging::Log($exception, false);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    } catch (Error $error) {
        // Output unexpected Errors.
        Logging::Log($error, false);
    }
}

function createBookWithExcessivePageCount() {
    try {
        // Create new Book instance.
        $book = new Book("The Book Thief", "Markus Zusak", 55200, new Publisher("Nopf Books"));
        // Output resulting book.
        Logging::Log($book);
    } catch (UnexpectedValueException $exception) {
        // Output expected UnexpectedValueException.
        Logging::Log($exception);
    } catch (InvalidArgumentException $exception) {
        // Output unexpected InvalidArgumentExceptions.
        Logging::Log($exception, false);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    } catch (Error $error) {
        // Output unexpected Errors.
        Logging::Log($error, false);
    }
}

function getInvalidBook() {
    try {
        // Create new Book instance with negative page count.
        $book = new Book("A Game of Thrones", "George R. R. Martin", -848, new Publisher("Bantam Spectra"));
        // Output resulting book.
        Logging::Log($book);
        // Confirm book is valid.
        if ($book->getPageCount() < 0) {
            throw new UnexpectedValueException("{$book} is invalid due to page count.");
        }
        return $book;
    } catch (UnexpectedValueException $exception) {
        // Output expected UnexpectedValueException.
        Logging::Log($exception);
    } catch (InvalidArgumentException $exception) {
        // Output unexpected InvalidArgumentExceptions.
        Logging::Log($exception, false);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    } catch (Error $error) {
        // Output unexpected Errors.
        Logging::Log($error, false);
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

As mentioned in the introduction, there is a bit of debate and uncertainty about the difference between the [`InvalidArgumentException`](https://airbrake.io/blog/php-exception-handling/invalidargumentexception) and the `UnexpectedValueException`.  Specifically, in what scenarios should one exception type be used over the other?

To attempt to answer this we'll start with the `InvalidArgumentException` which we've [previously explored](https://airbrake.io/blog/php-exception-handling/invalidargumentexception).  The key to knowing when to use this exception type is in the name -- in particular, the `Argument` keyword.  In most situations, the `InvalidArgumentException` should be used when an argument is passed that is considered improper for one reason or another.  For example, the following `Book::setPublisher(Publisher $publisher)` method confirms that the passed argument is of the proper class/type, and if not, it throws an `InvalidArgumentException` indicating as much:

```php
/**
* Set the publisher.
*
* @param Publisher $publisher Book publisher.
*/
public function setPublisher(Publisher $publisher) {
    // Confirm that publisher is correct type.
    if (gettype($publisher) == 'object') {
        $class = get_class($publisher);
        if ($class != 'Publisher') {
            // Not a Publisher, so throw a new InvalidArgumentException.
            throw new InvalidArgumentException("Publisher cannot be set to type ({$class}), must be a Publisher object.");
        }
    } else {
        $type = gettype($publisher);
        // Not an object, so cannot be Publisher.
        throw new InvalidArgumentException("Publisher cannot be set to type ({$type}), must be a Publisher object.");
    }
    $this->publisher = $publisher;
}
```

Since `InvalidArgumentException` is a child class of `LogicException`, another way to think of this exception type is that its validity should be checked by the logic of the code itself.  This is why it makes sense to perform the above type check, since we can confidently state that any passed argument that _is not_ a `Publisher` class instance is invalid and won't work with our code.

On the other hand, the `UnexpectedValueException` is a child class of `RuntimeException`, indicating that it should be used in response to something that can only be known at runtime.  This might be checking the actual _data_ of an argument, or often, it is used when the `return value` of a function is improper or unexpected in some way.

To illustrate, consider this modified version of our `Book` class:

```php
/**
 * Class Book
 */
class Book
{
    private $author;
    private $pageCount;
    private $publisher;
    private $title;

    // Maximum page count size.
    const MAX_PAGE_COUNT = 9999;

    /**
     * Book constructor.
     *
     * @param Book|string $title Book title.
     * @param Book|string $author Book author.
     * @param Book|int $pageCount Book page count.
     * @param Publisher $publisher Book publisher.
     */
    public function __construct(string $title, string $author, int $pageCount, Publisher $publisher) {
        $this->setAuthor($author);
        $this->setPageCount($pageCount);
        $this->setPublisher($publisher);
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
        // Confirm that page count is an integer.
        if (!is_integer($pageCount)) {
            // If not, throw a new InvalidArgumentException.
            $type = gettype($pageCount);
            throw new InvalidArgumentException("Page Count cannot be set to type ({$type}), must be an integer.");
        }
        // Check if page count within bounds.
        if ($pageCount > self::MAX_PAGE_COUNT) {
            $maxPageCount = self::MAX_PAGE_COUNT;
            throw new UnexpectedValueException("Page Count of ({$pageCount}) is invalid, cannot exceed MAX_PAGE_COUNT of ({$maxPageCount}).");
        }
        $this->pageCount = $pageCount;
    }

    /**
     * Get the publisher.
     *
     * @return Publisher Book publisher.
     */
    public function getPublisher(): Publisher {
        return $this->publisher;
    }

    /**
     * Set the publisher.
     *
     * @param Publisher $publisher Book publisher.
     */
    public function setPublisher(Publisher $publisher) {
        // Confirm that publisher is correct type.
        if (gettype($publisher) == 'object') {
            $class = get_class($publisher);
            if ($class != 'Publisher') {
                // Not a Publisher, so throw a new InvalidArgumentException.
                throw new InvalidArgumentException("Publisher cannot be set to type ({$class}), must be a Publisher object.");
            }
        } else {
            $type = gettype($publisher);
            // Not an object, so cannot be Publisher.
            throw new InvalidArgumentException("Publisher cannot be set to type ({$type}), must be a Publisher object.");
        }
        $this->publisher = $publisher;
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

    /**
     * Get the string representation of Book instance.
     *
     * @return mixed Book string representation.
     */
    public function __toString()
    {
        return "'{$this->getTitle()}' by {$this->getAuthor()} at {$this->getPageCount()} pgs, published by {$this->getPublisher()->getName()}";
    }
}
```

Specifically, we've added some additional logic to the `setPageCount(int $pageCount)` method.  While we explicitly confirm that the passed argument is an integer (throwing an `InvalidArgumentException` if not), we also perform a check on the validity of the data itself by confirming it falls without the bounds set by `MAX_PAGE_COUNT`.  If the page count exceeds this maximum, we throw a new `UnexpectedValueException`, since this is an "unexpected value" that we're seeing at runtime.

We can test this out and see the difference between these two exceptions at runtime with a few example functions.  We'll start with `createValidBook()` for a functional baseline:

```php
function createValidBook() {
    try {
        // Create new Book instance.
        $book = new Book("Lord of the Flies", "William Golding", 182, new Publisher("Faber & Faber"));
        // Output resulting book.
        Logging::Log($book);
    } catch (UnexpectedValueException $exception) {
        // Output expected UnexpectedValueException.
        Logging::Log($exception);
    } catch (InvalidArgumentException $exception) {
        // Output unexpected InvalidArgumentExceptions.
        Logging::Log($exception, false);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    } catch (Error $error) {
        // Output unexpected Errors.
        Logging::Log($error, false);
    }
}
```

This creates a `Book` instance, as expected, and outputs it to the log:

```
-------------- VALID BOOK --------------
Book (4) (
    private 'author' -> string (15) "William Golding"
    private 'pageCount' -> integer 182
    private 'publisher' -> Publisher (1) (
        private 'name' -> string (13) "Faber & Faber"
    )
    private 'title' -> string (17) "Lord of the Flies"
)
```

Now, let's try creating another `Book` with an excessive page count argument:

```php
function createBookWithExcessivePageCount() {
    try {
        // Create new Book instance.
        $book = new Book("The Book Thief", "Markus Zusak", 55200, new Publisher("Nopf Books"));
        // Output resulting book.
        Logging::Log($book);
    } catch (UnexpectedValueException $exception) {
        // Output expected UnexpectedValueException.
        Logging::Log($exception);
    } catch (InvalidArgumentException $exception) {
        // Output unexpected InvalidArgumentExceptions.
        Logging::Log($exception, false);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    } catch (Error $error) {
        // Output unexpected Errors.
        Logging::Log($error, false);
    }
}
```

Sure enough, this throws an `UnexpectedValueException`, indicating that the page count is invalid:

```
--------- EXCESSIVE PAGE COUNT ---------
[EXPECTED] UnexpectedValueException: Page Count of (55200) is invalid, cannot exceed MAX_PAGE_COUNT of (9999). in D:\work\Airbrake.io\Exceptions\PHP\Exception\RuntimeException\UnexpectedValueException\code.php on line 115
```

Finally, some people argue that `UnexpectedValueExceptions` should only be thrown within functions that call _other_ functions within their code block, which could result in unexpected values from other called functions.  Let's see how we might create such a function with `getInvalidBook()`:

```php
function getInvalidBook() {
    try {
        // Create new Book instance with negative page count.
        $book = new Book("A Game of Thrones", "George R. R. Martin", -848, new Publisher("Bantam Spectra"));
        // Output resulting book.
        Logging::Log($book);
        // Confirm book is valid.
        if ($book->getPageCount() < 0) {
            throw new UnexpectedValueException("{$book} is invalid due to page count.");
        }
        return $book;
    } catch (UnexpectedValueException $exception) {
        // Output expected UnexpectedValueException.
        Logging::Log($exception);
    } catch (InvalidArgumentException $exception) {
        // Output unexpected InvalidArgumentExceptions.
        Logging::Log($exception, false);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    } catch (Error $error) {
        // Output unexpected Errors.
        Logging::Log($error, false);
    }
}
```

Running this function produces the following output:

```
------------- INVALID BOOK -------------
Book (4) (
    private 'author' -> string (19) "George R. R. Martin"
    private 'pageCount' -> integer -848
    private 'publisher' -> Publisher (1) (
        private 'name' -> string (14) "Bantam Spectra"
    )
    private 'title' -> string (17) "A Game of Thrones"
)

[EXPECTED] UnexpectedValueException: 'A Game of Thrones' by George R. R. Martin at -848 pgs, published by Bantam Spectra is invalid due to page count. in D:\work\Airbrake.io\Exceptions\PHP\Exception\RuntimeException\UnexpectedValueException\code.php on line 239
```

As you can see here, our `Book` class itself considers this instance valid given all the arguments passed.  However, notice that we've accidentally passed a negative page count argument of `-848`.  Since the `Book` class doesn't currently have any checks for a negative value (which it obviously should, but this is just for illustration), our `getInvalidBook()` function checks the created `$book` instance for validity, and if it finds that the `$book->getPageCount()` method returns a negative number, it throws an `UnexpectedValueException`.

Check out the <a class="js-cta-utm" href="https://airbrake.io/languages/php_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-php">Airbrake-PHP library</a>, designed to quickly and easily integrate into any PHP project, giving you and your team access to real-time error monitoring and reporting throughout your application's entire life cycle.  With automatic, instantaneous error and exception notifications at your fingertips, you'll be constantly aware of your application's health, including any issues that may arise.  Best of all, with Airbrake's robust web dashboard cataloging every error that occurs, you and your team can immediately dive into the exact details of what went wrong, making it easy to quickly recognize and resolve problems.

---

__META DESCRIPTION__

A deep dive into the PHP UnexpectedValueException class, including functional code samples showing how this compares to InvalidArgumentExceptions.

---

__SOURCES__

- http://php.net/manual/en/class.throwable.php