# PHP Exception Handling - ClosedGeneratorException

Moving along in our detailed [__PHP Exception Handling__](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) series, today we'll take a look at the PHP ClosedGeneratorException.  A closed generator exception occurs when attempting to perform a traversal on a generator that has already been closed or terminated.

Throughout this article we'll explore the `ClosedGeneratorException` in more detail, digging into where it resides in the [PHP Exception Hierarchy](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy), along with some functional code samples that will illustrate how these errors are commonly thrown, so let's get going!

## The Technical Rundown

- All PHP errors implement the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface, or are extended from another inherited class therein.
- [`Exception`](http://php.net/manual/en/class.exception.php) implements the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface.
- `ClosedGeneratorException` extends the [`Exception`](http://php.net/manual/en/class.exception.php) class.

## When Should You Use It?

To understand what might cause a `ClosedGeneratorException` we first need to understand the purpose of `generators` in PHP and how they work.  Put simply, a generator is an easy way to implement an iterator, without the need to implement all the normal methods and functionality that the [`Iterator`](http://php.net/manual/en/class.iterator.php) interface normally requires.

To illustrate the differences between iterators and generators, we've got three different examples.  The full working code sample can be found below, provided for easy copy-and-pasting if you wish to try it out yourself.  After this code block we'll go over the code in more detail and explain what's going on:

```php
/**
 * Class Book
 */
class Book
{
    private $author;
    private $pageCount;
    private $title;

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
        $this->title = $value;
    }
}

function executeExamples()
{
    $books = [
        new Book("Mockingjay", "Suzanne Collins", 390),
        new Book("The Stand", "Stephen King", 823),
        new Book("Adventures of Huckleberry Finn", "Mark Twain", 366),
        new Book("A Game of Thrones", "George R. R. Martin", 835),
        new Book("The Eye of the World", "Robert Jordan", 814),
    ];

    iteratorExample($books);

    Logging::LineSeparator();
    Logging::LineSeparator();

    generatorExample($books);

    Logging::LineSeparator();
    Logging::LineSeparator();

    generatorSelfClosingExample($books);
}

/**
 * Generator for collection.
 *
 * @param $collection
 * @return Generator
 */
function generator($collection) {
    foreach($collection as $element) {
        yield $element;
    }
}

function generatorExample($books) {
    try {
        // Create basic generator.
        $generator = generator($books);

        // Output current element.
        Logging::Log($generator->current());

        // Output next element.
        $generator->next();
        Logging::Log($generator->current());

        // Rewind to original state.
        $generator->rewind();

        // Output next element.
        $generator->next();
        Logging::Log($generator->current());
    } catch (ClosedGeneratorException $exception) {
        // Output expected ClosedGeneratorException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output any unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

/**
 * Collection generator that self-closes after one yield.
 *
 * @param $collection
 * @return Generator|void
 */
function generatorSelfClosing($collection) {
    $count = 0;
    foreach($collection as $element) {
        $count++;
        if ($count >= 2) {
            return;
        }
        yield $element;
    }
}

function generatorSelfClosingExample($books) {
    try {
        // Create basic generator.
        $generator = generatorSelfClosing($books);

        // Output current element.
        Logging::Log($generator->current());

        // Output next element, which should terminate and return null.
        $generator->next();
        Logging::Log($generator->current());

        // Iterator through now-closed generator.
        foreach($generator as $book) {
            Logging::Log($book);
        }
    } catch (ClosedGeneratorException $exception) {
        // Output expected ClosedGeneratorException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output any unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

/**
 * Class CollectionIterator
 *
 * Iterator that handles collections.
 */
class CollectionIterator implements Iterator {
    protected $collection;
    protected $element;
    protected $key = 0;

    public function __construct($collection) {
        $this->collection = $collection;
    }

    /**
     * Revert to original state.
     */
    public function rewind() {
        $this->key = 0;
        $this->element = $this->collection[$this->key];
    }

    /**
     * Determine if current element is valid.
     *
     * @return bool
     */
    public function valid() {
        return false !== $this->element;
    }

    /**
     * Get current element.
     *
     * @return mixed
     */
    public function current() {
        return $this->element;
    }

    /**
     * Get the current iteration key.
     *
     * @return int
     */
    public function key() {
        return $this->key;
    }

    /**
     * Perform iteration by setting current element and iterating key.
     */
    public function next() {
        if (false !== $this->element && $this->key < count($this->collection)) {
            $this->element = $this->collection[$this->key];
            $this->key++;
        }
    }
}

/**
 * Iterates through passed book collection.
 *
 * @param $books Book collection to iterate.
 */
function iteratorExample($books) {
    try {
        // Create new iterator.
        $iterator = new CollectionIterator($books);

        // Output next element.
        $iterator->next();
        Logging::Log($iterator->current());

        // Output next element.
        $iterator->next();
        Logging::Log($iterator->current());

        // Rewind to original state.
        Logging::LineSeparator();
        Logging::Log("Rewinding...");
        Logging::LineSeparator();
        $iterator->rewind();

        // Output next element, should be original.
        $iterator->next();
        Logging::Log($iterator->current());
    } catch (ClosedGeneratorException $exception) {
        // Output expected ClosedGeneratorException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output any unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

executeExamples();

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
     * @param $object Object to be logged.
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

We'll begin with a typical iterator class that inherits from the [`Iterator`](http://php.net/manual/en/class.iterator.php) interface.  _Note: PHP includes a number of built-in iterators, including one for iterating over simple collections like `Arrays`.  However, for our purposes, we've created our own iterator to show how they typically work under the hood._

```php
/**
 * Class CollectionIterator
 *
 * Iterator that handles collections.
 */
class CollectionIterator implements Iterator {
    protected $collection;
    protected $element;
    protected $key = 0;

    public function __construct($collection) {
        $this->collection = $collection;
    }

    /**
     * Revert to original state.
     */
    public function rewind() {
        $this->key = 0;
        $this->element = $this->collection[$this->key];
    }

    /**
     * Determine if current element is valid.
     *
     * @return bool
     */
    public function valid() {
        return false !== $this->element;
    }

    /**
     * Get current element.
     *
     * @return mixed
     */
    public function current() {
        return $this->element;
    }

    /**
     * Get the current iteration key.
     *
     * @return int
     */
    public function key() {
        return $this->key;
    }

    /**
     * Perform iteration by setting current element and iterating key.
     */
    public function next() {
        if (false !== $this->element && $this->key < count($this->collection)) {
            $this->element = $this->collection[$this->key];
            $this->key++;
        }
    }
}
```

Nothing too fancy going on here.  The basic purpose of this class is to accept a collection of elements, iterating through each subsequent element every time `next()` is called.  The current element is retrieved via the `current()` method.

To test out this iterator (along with our generators) we have a `Book` class that we'll use to create a collection of books:

```php
/**
 * Class Book
 */
class Book
{
    private $author;
    private $pageCount;
    private $title;

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
        $this->title = $value;
    }
}
```

Now, the `executeExamples()` function creates the baseline `$books` collection and passed it to each of our example functions:

```php
function executeExamples()
{
    $books = [
        new Book("Mockingjay", "Suzanne Collins", 390),
        new Book("The Stand", "Stephen King", 823),
        new Book("Adventures of Huckleberry Finn", "Mark Twain", 366),
        new Book("A Game of Thrones", "George R. R. Martin", 835),
        new Book("The Eye of the World", "Robert Jordan", 814),
    ];

    iteratorExample($books);

    Logging::LineSeparator();
    Logging::LineSeparator();

    generatorExample($books);

    Logging::LineSeparator();
    Logging::LineSeparator();

    generatorSelfClosingExample($books);
}
```

We begin with the `iteratorExample()` function, which passes the `$books` collection to a new `CollectionIterator` instance, then tests that everything works as expected by iterating a couple times and outputting the results before `rewinding` and checking that the `next()` iteration retrieves the first element again:

```php
/**
 * Iterates through passed book collection.
 *
 * @param $books Book collection to iterate.
 */
function iteratorExample($books) {
    try {
        // Create new iterator.
        $iterator = new CollectionIterator($books);
        
        // Output next element.
        $iterator->next();
        Logging::Log($iterator->current());

        // Output next element.
        $iterator->next();
        Logging::Log($iterator->current());

        // Rewind to original state.
        Logging::LineSeparator();
        Logging::Log("Rewinding...");
        Logging::LineSeparator();
        $iterator->rewind();

        // Output next element, should be original.
        $iterator->next();
        Logging::Log($iterator->current());
    } catch (ClosedGeneratorException $exception) {
        // Output expected ClosedGeneratorException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output any unexpected Exceptions.
        Logging::Log($exception, false);
    }
}
```

Sure enough, the output shows we iterated through the first two elements, rewound, then retrieved the first element once again, as intended:

```
Book (3) (
    private 'author' -> string (15) "Suzanne Collins"
    private 'pageCount' -> integer 390
    private 'title' -> string (10) "Mockingjay"
)
Book (3) (
    private 'author' -> string (12) "Stephen King"
    private 'pageCount' -> integer 823
    private 'title' -> string (9) "The Stand"
)
----------------------------------------
Rewinding...
----------------------------------------
Book (3) (
    private 'author' -> string (15) "Suzanne Collins"
    private 'pageCount' -> integer 390
    private 'title' -> string (10) "Mockingjay"
)
```

### Generators

Iterators are all well and good, but their code and usage can become rather complex if we're not careful.  Hence, the introduction of `generators` in PHP 5.5, which aim to simplify the code necessary to create iterators.  Generators rely on the [`yield`](http://php.net/manual/en/language.generators.syntax.php#control-structures.yield) keyword, which you're probably familiar with from many other languages.  Its purpose is to _pause_ execution at the `yield` statement location, return the currently yielded value, then resume execution from the previous pause point during the next call/iteration.

Creating a generator is as easy as pie: Just create a normal function and add a `yield` statement where you want execution to pause and an iterable or sequential value to be returned.  For example, here's our basic `generator` function that simply iterates over a collection and `yields` each element, in order:

```php
/**
 * Generator for collection.
 *
 * @param $collection
 * @return Generator
 */
function generator($collection) {
    foreach($collection as $element) {
        yield $element;
    }
}
```

The `generatorExample()` function uses the `generator()` function and performs similar logic to our `iteratorExample()` function.  It outputs the first two elements, then performs a rewind to reset the iterator state, then tries to output the first element again.

```php
function generatorExample($books) {
    try {
        // Create basic generator.
        $generator = generator($books);

        // Output current element.
        Logging::Log($generator->current());

        // Output next element.
        $generator->next();
        Logging::Log($generator->current());

        // Rewind to original state.
        $generator->rewind();

        // Output next element.
        $generator->next();
        Logging::Log($generator->current());
    } catch (ClosedGeneratorException $exception) {
        // Output expected ClosedGeneratorException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output any unexpected Exceptions.
        Logging::Log($exception, false);
    }
}
```

Running this function works fine until we hit the `rewind()` method call, which throws an `Exception` because we're not allowed to rewind a generator that has already yielded at least one value:

```
Book (3) (
    private 'author' -> string (15) "Suzanne Collins"
    private 'pageCount' -> integer 390
    private 'title' -> string (10) "Mockingjay"
)
Book (3) (
    private 'author' -> string (12) "Stephen King"
    private 'pageCount' -> integer 823
    private 'title' -> string (9) "The Stand"
)

[UNEXPECTED] Exception: Cannot rewind a generator that was already run in D:\work\Airbrake.io\Exceptions\PHP\Exception\ClosedGeneratorException\code.php on line 131
```

Generators were built with this restriction in mind because their purpose is to be one-time sources of iterable data -- resetting their state and iterating again is contrary to those principles.

For our last example we've created the `generatorSelfClosing()` function, which is similar to our previous generator except it _intentionally_ closes (terminates) itself after the first iteration.  The `return` statement triggers a closing and termination of the active generator:

```php
/**
 * Collection generator that self-closes after one yield.
 *
 * @param $collection
 * @return Generator|void
 */
function generatorSelfClosing($collection) {
    $count = 0;
    foreach($collection as $element) {
        $count++;
        if ($count >= 2) {
            return;
        }
        yield $element;
    }
}
```

This time, in our test function we'll try iterating over our elements _after_ the generator has already been closed/terminated:

```php
function generatorSelfClosingExample($books) {
    try {
        // Create basic generator.
        $generator = generatorSelfClosing($books);

        // Output current element.
        Logging::Log($generator->current());

        // Output next element, which should terminate and return null.
        $generator->next();
        Logging::Log($generator->current());

        // Iterator through now-closed generator.
        foreach($generator as $book) {
            Logging::Log($book);
        }
    } catch (ClosedGeneratorException $exception) {
        // Output expected ClosedGeneratorException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output any unexpected Exceptions.
        Logging::Log($exception, false);
    }
}
```

The expectation is that the first element is retrieved fine, but the second element retrieval will return `null`, since we explicitly closed the generator by `returning` `null` on the second iteration call.  Then, as the output shows, we end up throwing a `ClosedGeneratorException` exception when we attempt to iterate through our now-closed generator:

```
Book (3) (
    private 'author' -> string (15) "Suzanne Collins"
    private 'pageCount' -> integer 390
    private 'title' -> string (10) "Mockingjay"
)

null

[UNEXPECTED] Exception: Cannot traverse an already closed generator in D:\work\Airbrake.io\Exceptions\PHP\Exception\ClosedGeneratorException\code.php on line 174
```

Check out the <a class="js-cta-utm" href="https://airbrake.io/languages/php_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-php">Airbrake-PHP library</a>, designed to quickly and easily integrate into any PHP project, giving you and your team access to real-time error monitoring and reporting throughout your application's entire life cycle.  With automatic, instantaneous error and exception notifications at your fingertips, you'll be constantly aware of your application's health, including any issues that may arise.  Best of all, with Airbrake's robust web dashboard cataloging every error that occurs, you and your team can immediately dive into the exact details of what went wrong, making it easy to quickly recognize and resolve problems.

---

__META DESCRIPTION__

A deep dive into the PHP ClosedGeneratorException, including code samples that illustrate three different methods of iteration.

---

__SOURCES__

- https://wiki.php.net/rfc/generators