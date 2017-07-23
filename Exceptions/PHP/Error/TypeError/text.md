# PHP Exception Handling - TypeError

Next up in our in-depth [__PHP Exception Handling__](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) series we're taking a closer look at the PHP TypeError.  `TypeErrors` are thrown in a few different scenarios where the PHP engine expects a particular object `type`, but a different type is provided instead.

In this article we'll explore the `TypeError` in more detail, looking at where it sits within the [PHP Exception Hierarchy](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy), and then examining some functional code samples that illustrate the three different scenarios in which `TypeErrors` can be thrown.  Let's get going!

## The Technical Rundown

- All PHP errors implement the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface, or are extended from another inherited class therein.
- [`Error`](http://php.net/manual/en/class.error.php) implements the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface.
- `TypeError` extends the [`Error`](http://php.net/manual/en/class.error.php) class.

## When Should You Use It?

As previously mentioned, `TypeError` can be thrown in three different situations:

- When a method declares an explicit argument type, but a different (incompatible) argument type is passed instead.
- When a method declares an explicit return type, but a different typed is returned instead.
- When an invalid number of arguments are passed to a built-in PHP function, while the `declare()` function of the executed file sets the `strict_type` declaration to `true`.

We'll explore all three of these scenarios in our sample code, so we'll start with the full working example below, then explore it in more detail to follow:

```php
<?php
declare(strict_types=1);

require('/media/sf_Airbrake.io/lib/php/Logging.php');

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
    incorrectArgumentTypeTest();
    incorrectReturnTypeExample();
    incorrectArgumentCountExample();
}

/**
 * Pass an invalid object type (string) to a method that expects a different type (Publisher).
 */
function incorrectArgumentTypeTest()
{
    try {
        // Create a new Book instance.
        $book = new Book("Mockingjay", "Suzanne Collins", 390, new Publisher("Harper"));
        // Output default book.
        Logging::Log($book);
        // Attempt to set correct publisher via string.
        $book->setPublisher("Scholastic");
        // Output modified book.
        Logging::Log($book);
    } catch (TypeError $error) {
        // Output expected TypeError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }
}

/**
 * Return an invalid object type (string) from a method that declares a different return type (Publisher).
 */
function incorrectReturnTypeExample()
{
    try {
        // Create a new Book instance.
        $book = new Book("The Two Towers",
            "J.R.R. Tolkien",
            415,
            new Publisher("Allen & Unwin"));
        // Output default book.
        Logging::Log($book);
        // Output publisher.
        Logging::Log($book->getPublisher());
    } catch (TypeError $error) {
        // Output expected TypeError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }
}

/**
 * Pass an incorrect number of arguments to a built-in PHP function.
 */
function incorrectArgumentCountExample()
{
    try {
        // Create basic array.
        $array = array(123, 'Alice', 'Brill');
        // Join array values with comma, passing an extra argument value.
        $combined = implode(",", $array, "extra");
        // Output combined string.
        Logging::Log($combined);
    } catch (TypeError $error) {
        // Output expected TypeError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }
}

executeExamples();

<?php

require('kint.php');

/**
 * Provides basic logging/output functionality.
 */
class Logging {

    /**
     * Logs the passed object, string, or Throwable instance to the console.
     *
     * @param $a Primary message or value to be logged.
     * @param null $b Secondary value, such as boolean for Throwables indicating if error was expected.
     */
    public static function Log($a, $b = null) {
        if (is_string($a)) {
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
     * @param $object Object to be logged.
     *
     * @see https://github.com/kint-php/kint    Kint tool used for structured outputs.
     */
    private static function LogObject($object) {
        Kint::dump($object);
    }

    /**
     * Logs the passed string value.
     *
     * @param $value Value to be logged.
     */
    private static function LogString($value) {
        print_r("{$value}\n");
    }

    /**
     * Logs the passed Throwable object.  
     * Includes message, className if error was expected, and stack trace.
     *
     * Uses internal Reflection to retrieve protected/private properties.
     *
     * @param $throwable Throwable object to be output.
     * @param bool $expected Indicates if error was expected or not.
     */
    private static function LogThrowable($throwable, bool $expected = true) {
        $expected = $expected ? "EXPECTED" : "UNEXPECTED";
        $message = substr($throwable->xdebug_message, 1);
        // Output whether error was expected or not, the class name, the message, and stack trace.
        print_r("[{$expected}] {$message}\n");
        // Add line separator to keep it tidy.
        self::LineSeparator();
    }

    /**
     * Outputs a separator line to log.
     *
     * @param int $length Length of the line separator.
     * @param string $character Character to use as separator.
     */
    public static function LineSeparator(int $length = 40, string $character = '-') {
        $break = str_repeat($character, $length);
        print_r("{$break}\n");
    }
}
```

---

We begin with some simple classes, `Book` and `Publisher`, which both include a number of relevant properties like `Publisher->name` and `Book->title`.  Most importantly, the getter and setter methods for these properties explicitly declare argument and return types, which is important for our use later on (and is generally a smart practice, anyway):

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

Our first function is testing what happens when we pass an invalid argument type to the `Book->setPublisher(Publisher $publisher)` method.  Instead of passing an actual `Publisher` class instance object as the argument we're passing a plain `string` to attempt to fix the publisher name to the proper one:

```php
/**
 * Pass an invalid object type (string) to a method that expects a different type (Publisher).
 */
function incorrectArgumentTypeTest()
{
    try {
        // Create a new Book instance.
        $book = new Book("Mockingjay", "Suzanne Collins", 390, new Publisher("Harper"));
        // Output default book.
        Logging::Log($book);
        // Attempt to set correct publisher via string.
        $book->setPublisher("Scholastic");
        // Output modified book.
        Logging::Log($book);
    } catch (TypeError $error) {
        // Output expected TypeError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }
}
```

Since `Book->setPublisher(Publisher $publisher)` doesn't expect a `string` argument, a `TypeError` is thrown in the output:

```
┌──────────────────────────────────────────────────────────────────────────────┐
│ $object                                                                      │
└──────────────────────────────────────────────────────────────────────────────┘
Book (4) (
    private 'author' -> string (15) "Suzanne Collins"
    private 'pageCount' -> integer 390
    private 'publisher' -> Publisher (1) (
        private 'name' -> string (6) "Harper"
    )
    private 'title' -> string (10) "Mockingjay"
)
════════════════════════════════════════════════════════════════════════════════
Called from .../Logging.php:34 [Logging::LogObject()]

[EXPECTED] TypeError: Argument 1 passed to Book::setPublisher() must be an instance of Publisher, string given, called in /media/sf_Airbrake.io/Exceptions/PHP/Error/TypeError/code.php on line 158 in /media/sf_Airbrake.io/Exceptions/PHP/Error/TypeError/code.php on line 117
```

The obvious solution is to create a new `Publisher` instance and then pass it to the `Book->setPublisher(Publisher $publisher)` method:

```php
$book->setPublisher(new Publisher("Scholastic"));
```

The next scenario we might experience a `TypeError` is when a method's return type is explicitly declared, but the method attempts to return a different type.  In this second `incorrectReturnTypeExample()` function we've created a different book instance, and then make an explicit call to the `$book->getPublisher()` method.  The return value of that call is passed to `Logging::Log(string $value)`, which attempts to output a string value it receives:

```php
/**
 * Return an invalid object type (string) from a method that declares a different return type (Publisher).
 */
function incorrectReturnTypeExample()
{
    try {
        // Create a new Book instance.
        $book = new Book("The Two Towers",
            "J.R.R. Tolkien",
            415,
            new Publisher("Allen & Unwin"));
        // Output default book.
        Logging::Log($book);
        // Output publisher.
        Logging::Log($book->getPublisher());
    } catch (TypeError $error) {
        // Output expected TypeError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }
}
```

Unfortunately, the `Book->getPublisher()` method has a minor issue -- rather than returning the local `Publisher` object (`$this->publisher`), it instead returns a `string` value by returning the publisher's name:


```php
class Book {
    // ...

    /**
     * Get the publisher.
     *
     * @return mixed Book publisher.
     */
    public function getPublisher(): Publisher {
        return $this->publisher->getName();
    }

    // ...
}
```

As a result of this incompatibility, executing the `incorrectReturnTypeExample()` function results in another `TypeError`, indicating the issue:

```
┌──────────────────────────────────────────────────────────────────────────────┐
│ $object                                                                      │
└──────────────────────────────────────────────────────────────────────────────┘
Book (4) (
    private 'author' -> string (14) "J.R.R. Tolkien"
    private 'pageCount' -> integer 415
    private 'publisher' -> Publisher (1) (
        private 'name' -> string (13) "Allen & Unwin"
    )
    private 'title' -> string (14) "The Two Towers"
)
════════════════════════════════════════════════════════════════════════════════
Called from .../Logging.php:34 [Logging::LogObject()]

[EXPECTED] TypeError: Return value of Book::getPublisher() must be an instance of Publisher, string returned in /media/sf_Airbrake.io/Exceptions/PHP/Error/TypeError/code.php on line 109
``` 

The solution here is to fix the `Book->getPublisher()` return type to its intended type, which is just `$this->publisher`:

```php
class Book {
    // ...

    /**
     * Get the publisher.
     *
     * @return mixed Book publisher.
     */
    public function getPublisher(): Publisher {
        return $this->publisher;
    }

    // ...
}
```

The last scenario we could potentially experience a `TypeError` can only occur if our script has a `declare()` function statement that sets the [`strict_types`](http://php.net/manual/en/functions.arguments.php#functions.arguments.type-declaration.strict) declaration value to `1` (`true`).  This is necessary because, by default, PHP attempts to convert certain value types into other, compatible types wherever applicable.  For example, passing an `integer` value of `123` to a `testMethod(string $value)` method, which expects a `string` value as an argument, causes the integer to be automatically converted to a string (`"123"`).  However, in some situations it may be useful to disallow this behavior in PHP, so attempting to automatically convert differing value types may result in an error.

As with any `declare()` statements, this must occur at the beginning of the file:

```php
<?php
declare(strict_types=1);
```

The `incorrectArgumentCountExample()` method declares an array with a few values, then passes it (along with two _other_ arguments) to the built-in [`implode`](http://php.net/manual/en/function.implode.php) method, which only expects a maximum of _two_ total arguments:

```php
/**
 * Pass an incorrect number of arguments to a built-in PHP function.
 */
function incorrectArgumentCountExample()
{
    try {
        // Create basic array.
        $array = array(123, 'Alice', 'Brill');
        // Output array.
        Logging::Log($array);
        // Join array values with comma, passing an extra argument value.
        $combined = implode(",", $array, "extra");
        // Output combined string.
        Logging::Log($combined);
    } catch (TypeError $error) {
        // Output expected TypeError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }
}
```

Since `strict_types` is enabled, passing an extra number of arguments to `implode()` results in a thrown `TypeError`:

```
┌──────────────────────────────────────────────────────────────────────────────┐
│ $object                                                                      │
└──────────────────────────────────────────────────────────────────────────────┘
array (3) [
    0 => integer 123
    1 => string (5) "Alice"
    2 => string (5) "Brill"
]
════════════════════════════════════════════════════════════════════════════════
Called from .../Logging.php:34 [Logging::LogObject()]

[EXPECTED] TypeError: implode() expects at most 2 parameters, 3 given in /media/sf_Airbrake.io/Exceptions/PHP/Error/TypeError/code.php on line 203
```

Check out the <a class="js-cta-utm" href="https://airbrake.io/languages/php_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-php">Airbrake-PHP library</a>, designed to quickly and easily integrate into any PHP project, giving you and your team access to real-time error monitoring and reporting throughout your application's entire life cycle.  With automatic, instantaneous error and exception notifications at your fingertips, you'll be constantly aware of your application's health, including any issues that may arise.  Best of all, with Airbrake's robust web dashboard cataloging every error that occurs, you and your team can immediately dive into the exact details of what went wrong, making it easy to quickly recognize and resolve problems.

---

__META DESCRIPTION__

A close look at the PHP TypeError class, including code samples to illustrating the three different scenarios in which TypeErrors can occur.