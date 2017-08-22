# PHP Exception Handling - InvalidArgumentException

Making our way through our in-depth [__PHP Exception Handling__](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) series, today we'll go over the InvalidArgumentException in PHP.  The `InvalidArgumentException` should be thrown when an inappropriate argument is passed to a method or function.  This can be either due to the actual object data type, or because the data itself is invalid in some way.

Throughout this article we'll explore the `InvalidArgumentException` in more detail, starting with where it sits in the [PHP Exception Hierarchy](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy).  We'll also go over a few functional sample code examples that aim to show how `InvalidArgumentExceptions` might be used, and where they differ from `TypeErrors` which we saw explored in our [`PHP Exception Handling - TypeError`](https://airbrake.io/blog/php-exception-handling/php-typeerror) article last month.  Let's get to it!

## The Technical Rundown

- All PHP errors implement the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface, or are extended from another inherited class therein.
- [`Exception`](http://php.net/manual/en/class.exception.php) implements the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface.
- [`LogicException`](http://php.net/manual/en/class.logicexception.php) extends the [`Exception`](http://php.net/manual/en/class.exception.php) class.
- Lastly, `InvalidArgumentException` extends the [`LogicException`](http://php.net/manual/en/class.logicexception.php) class.

## Full Code Sample

Below is the full code sample we’ll be using in this article.  Feel free to use any or all of the code if you wish to follow along.

```php
<?php
//declare(strict_types=1);

include("D:\work\Airbrake.io\lib\php\Logging.php");

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

/**
 * Class Book
 */
class Book
{
    private $author;
    private $pageCount;
    private $publisher;
    private $title;

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
        $this->pageCount = $pageCount;
    }

    /**
     * Get the publisher.
     *
     * @return mixed Book publisher.
     */
    public function getPublisher(): Publisher {
        return $this->publisher->getName();
    }

    /**
     * Set the publisher.
     *
     * @param Publisher $publisher Book publisher.
     */
    public function setPublisher(Publisher $publisher) {
        // Confirm that page count is an integer.
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
}

function executeExamples()
{
    Logging::LineSeparator("FLOAT TO INTEGER");
    passFloatToInteger();
    Logging::LineSeparator("STRING TO INTEGER");
    passStringToInteger();
    Logging::LineSeparator("STRING TO PUBLISHER");
    passStringToPublisher();
    Logging::LineSeparator("INVALID PUBLISHER TO PUBLISHER");
    passInvalidPublisherToPublisher();
}

function passFloatToInteger() {
    try {
        // Create new Book instance.
        $book = new Book("1984", "George Orwell", 238, new Publisher("Harvill Secker"));
        // Try to set correct page count as float.
        $book->setPageCount(328.0);
        // Output resulting book.
        Logging::Log($book);
    } catch (InvalidArgumentException $exception) {
        // Output expected InvalidArgumentExceptions.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    } catch (Error $error) {
        // Output unexpected Errors.
        Logging::Log($error, false);
    }
}

function passStringToInteger() {
    try {
        // Create new Book instance.
        $book = new Book("A Game of Thrones", "George R. R. Martin", 385, new Publisher("Bantam Spectra"));
        // Try to set correct page count as string.
        $book->setPageCount("835");
        // Output resulting book.
        Logging::Log($book);
    } catch (InvalidArgumentException $exception) {
        // Output expected InvalidArgumentExceptions.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    } catch (Error $error) {
        // Output unexpected Errors.
        Logging::Log($error, false);
    }
}

function passStringToPublisher() {
    try {
        // Create new Book instance.
        $book = new Book("The Book Thief", "Markus Zusak", 552, new Publisher("Nopf Books"));
        // Try to set correct publisher as string.
        $book->setPublisher("Knopf Books");
        // Output resulting book.
        Logging::Log($book);
    } catch (InvalidArgumentException $exception) {
        // Output expected InvalidArgumentExceptions.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    } catch (Error $error) {
        // Output unexpected Errors.
        Logging::Log($error, false);
    }
}

function passInvalidPublisherToPublisher() {
    try {
        // Create new Book instance.
        $book = new Book("Lord of the Flies", "William Golding", 182, new Publisher(""));
        // Try to set publisher with lowercase string as name.
        $book->setPublisher(new Publisher("penguin great books"));
        // Output resulting book.
        Logging::Log($book);
    } catch (InvalidArgumentException $exception) {
        // Output expected InvalidArgumentExceptions.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    } catch (Error $error) {
        // Output unexpected Errors.
        Logging::Log($error, false);
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

The ability to include `type declarations` was introduced with the release of PHP 5.  This allows code to explicitly define when parameters are expected to be of a certain data type.  In PHP 5, the effect of passing an invalid object type is a recoverable fatal error.  Nowadays, with PHP 7, the result of an invalid argument is a [`TypeError`](https://airbrake.io/blog/php-exception-handling/php-typeerror).  Generally speaking, it's considered good practice to explicitly define your type declarations wherever possible, since this tends to produce tighter, less error-prone code.  However, the existence of built-in declaration type-checking raises an interesting question in the context of an article about `InvalidArgumentExceptions`: **What is the purpose of explicitly throwing an `InvalidArgumentException`, when the language itself includes built-in means for detecting improper argument types?**

There are two scenarios where using `InvalidArgumentExceptions` might be appropriate.  The first is when explicitly changing the [`strict_types`](http://php.net/manual/en/functions.arguments.php#functions.arguments.type-declaration.strict) flag, which disables the default coercion from one data type argument to that of the declared type, when possible.  That is, if passing a `string` to an `int` declared type argument, PHP 7 will attempt to convert the value to an `int` automatically.  With `strict_types` enabled, this coercion is disabled.

The other scenario in which explicitly throwing an `InvalidArgumentException` can be useful is when the data _type_ is correct, but the data _value_ is considered invalid.  Be careful here though, since PHP 7 also includes other `Exception` types that may be more appropriate depending on the invalidity of the data, such as `LengthException` and `OutOfRangeException`.

To examine how this all works we have setup a testing ground in our code in which we'll be creating a few new `Book` instances, which each have an associated `Publisher` instance assigned to them:

```php
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

/**
 * Class Book
 */
class Book
{
    private $author;
    private $pageCount;
    private $publisher;
    private $title;

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
        $this->pageCount = $pageCount;
    }

    /**
     * Get the publisher.
     *
     * @return mixed Book publisher.
     */
    public function getPublisher(): Publisher {
        return $this->publisher->getName();
    }

    /**
     * Set the publisher.
     *
     * @param Publisher $publisher Book publisher.
     */
    public function setPublisher(Publisher $publisher) {
        // Confirm that page count is an integer.
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
}
```

Of particular importance is the `setPageCount(int $pageCount)` method:

```php
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
    $this->pageCount = $pageCount;
}
```

As you can see, we have an explicit `type declaration` of `int` for the `$pageCount` parameter, but we also go a step further and directly confirm if the passed value is an `integer` type.  If not, we throw a new `InvalidArgumentException`.

To test this out, here we have a couple functions that create a new `Book` instance and attempt to change the `pageCount` property by passing non-integer values of `float` and `string`, respectively:

```php
function passFloatToInteger() {
    try {
        // Create new Book instance.
        $book = new Book("1984", "George Orwell", 238, new Publisher("Harvill Secker"));
        // Try to set correct page count as float.
        $book->setPageCount(328.0);
        // Output resulting book.
        Logging::Log($book);
    } catch (InvalidArgumentException $exception) {
        // Output expected InvalidArgumentExceptions.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    } catch (Error $error) {
        // Output unexpected Errors.
        Logging::Log($error, false);
    }
}

function passStringToInteger() {
    try {
        // Create new Book instance.
        $book = new Book("A Game of Thrones", "George R. R. Martin", 385, new Publisher("Bantam Spectra"));
        // Try to set correct page count as string.
        $book->setPageCount("835");
        // Output resulting book.
        Logging::Log($book);
    } catch (InvalidArgumentException $exception) {
        // Output expected InvalidArgumentExceptions.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    } catch (Error $error) {
        // Output unexpected Errors.
        Logging::Log($error, false);
    }
}
```

Calling both these test functions doesn't produce any problems and the output shows our initial `Book` instances both have their page count updated to the correct value:

```
----------- FLOAT TO INTEGER -----------
Book (4) (
    private 'author' -> string (13) "George Orwell"
    private 'pageCount' -> integer 328
    private 'publisher' -> Publisher (1) (
        private 'name' -> string (14) "Harvill Secker"
    )
    private 'title' -> string (4) "1984"
)
---------- STRING TO INTEGER -----------
Book (4) (
    private 'author' -> string (19) "George R. R. Martin"
    private 'pageCount' -> integer 835
    private 'publisher' -> Publisher (1) (
        private 'name' -> string (14) "Bantam Spectra"
    )
    private 'title' -> string (17) "A Game of Thrones"
)
```

What we're seeing here is the previously discussed automatic coercion from the compatible types of `float` and `string` into the declared parameter type of `int`.  PHP knows how to convert the `string` value `"835"` and the `float` value of `328.0` to `int` automatically, so it does so without any trouble.

However, now let's enable `strict_types` at the top of the file and run these functions again:

```php
<?php
declare(strict_types=1);
// ...
```

```
----------- FLOAT TO INTEGER -----------
[UNEXPECTED] TypeError: Argument 1 passed to Book::setPageCount() must be of the type integer, float given, called in D:\work\Airbrake.io\Exceptions\PHP\Exception\LogicException\InvalidArgumentException\code.php on line 200 in D:\work\Airbrake.io\Exceptions\PHP\Exception\LogicException\InvalidArgumentException\code.php on line 99

---------- STRING TO INTEGER -----------
[UNEXPECTED] TypeError: Argument 1 passed to Book::setPageCount() must be of the type integer, string given, called in D:\work\Airbrake.io\Exceptions\PHP\Exception\LogicException\InvalidArgumentException\code.php on line 220 in D:\work\Airbrake.io\Exceptions\PHP\Exception\LogicException\InvalidArgumentException\code.php on line 99
```

As we can see, suddenly we're producing unexpected `TypeErrors` because PHP is detecting that the explicitly declared type of `int` is not being passed.

That's all well and good, but let's look at what happens when we use a class object instead of a primitive type in our type declaration.  Here we have the `Book->setPublisher(Publisher $publisher)` method:

```php
/**
* Set the publisher.
*
* @param Publisher $publisher Book publisher.
*/
public function setPublisher(Publisher $publisher) {
    // Confirm that page count is an integer.
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

To test this out we'll try passing a `string` as the only parameter:

```php
function passStringToPublisher() {
    try {
        // Create new Book instance.
        $book = new Book("The Book Thief", "Markus Zusak", 552, new Publisher("Nopf Books"));
        // Try to set correct publisher as string.
        $book->setPublisher("Knopf Books");
        // Output resulting book.
        Logging::Log($book);
    } catch (InvalidArgumentException $exception) {
        // Output expected InvalidArgumentExceptions.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    } catch (Error $error) {
        // Output unexpected Errors.
        Logging::Log($error, false);
    }
}
```

Calling this function produces an unsurprising `TypeError` once again:

```
--------- STRING TO PUBLISHER ----------
[UNEXPECTED] TypeError: Argument 1 passed to Book::setPublisher() must be an instance of Publisher, string given, called in D:\work\Airbrake.io\Exceptions\PHP\Exception\LogicException\InvalidArgumentException\code.php on line 240 in D:\work\Airbrake.io\Exceptions\PHP\Exception\LogicException\InvalidArgumentException\code.php on line 123
```

In fact, since we're dealing with `object` instances here, PHP doesn't know how to implicitly coerce a `string` value into a complex `Publisher` class instance.  Therefore, no matter whether `strict_types` is enabled or not, the above function always throws a `TypeError`.

Does this mean that `InvalidArgumentExceptions` are never appropriate or can never be used for methods with explicit type declarations of class object types?  No, it just means that an `InvalidArgumentException` should be used based on the data _value_ itself, rather than the data _type_.  For example, let's look at our last test function:

```php
function passInvalidPublisherToPublisher() {
    try {
        // Create new Book instance.
        $book = new Book("Lord of the Flies", "William Golding", 182, new Publisher(""));
        // Try to set publisher with lowercase string as name.
        $book->setPublisher(new Publisher("penguin great books"));
        // Output resulting book.
        Logging::Log($book);
    } catch (InvalidArgumentException $exception) {
        // Output expected InvalidArgumentExceptions.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    } catch (Error $error) {
        // Output unexpected Errors.
        Logging::Log($error, false);
    }
}
```

Here we're trying to pass a new `Publisher` instance with the correct name of the publisher, except it's all lowercase.  We only want to accept `Pascal Case` values as valid publisher names, so the `Publisher->setName(string $name)` method performs a simple bit of logic to check if the `$name` parameter is in `Pascal Case` or not:

```php
class Publisher {
    // ...

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

_Note: We **could** obviously just change the passed value to the result of the `ucwords()` function, prior to assignment to the `name` property, but we're purposely neglecting to do so in this example._  Either way, execution of the `passInvalidPublisherToPublisher()` function throws the `InvalidArgumentException` we were expecting:

```
---- INVALID PUBLISHER TO PUBLISHER ----
[EXPECTED] InvalidArgumentException: Publisher->name must be Pascal Case; passed name is invalid: penguin great books in D:\work\Airbrake.io\Exceptions\PHP\Exception\LogicException\InvalidArgumentException\code.php on line 40
```

Check out the <a class="js-cta-utm" href="https://airbrake.io/languages/php_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-php">Airbrake-PHP library</a>, designed to quickly and easily integrate into any PHP project, giving you and your team access to real-time error monitoring and reporting throughout your application's entire life cycle.  With automatic, instantaneous error and exception notifications at your fingertips, you'll be constantly aware of your application's health, including any issues that may arise.  Best of all, with Airbrake's robust web dashboard cataloging every error that occurs, you and your team can immediately dive into the exact details of what went wrong, making it easy to quickly recognize and resolve problems.

---

__META DESCRIPTION__

A close look at the PHP InvalidArgumentException class, including functional code samples showing how to properly use both these and TypeErrors.
---

__SOURCES__

- http://php.net/manual/en/class.throwable.php