# PHP Exception Handling - ParseError

Next up in our in-depth [__PHP Exception Handling__](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) series we're taking a closer look at the PHP ParseError.  As you might suspect, a `ParseError` is thrown when PHP has trouble parsing a line of code.  This could either be due to a typo or syntax error, or even while using the terrifying [`eval()`](http://php.net/manual/en/function.eval.php) function.

We'll begin by looking at where the `ParseError` resides in the [PHP Exception Hierarchy](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy), then we'll explore some basic code samples that illustrate how `ParseErrors` might occur, so let's get going!

## The Technical Rundown

- All PHP errors implement the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface, or are extended from another inherited class therein.
- [`Error`](http://php.net/manual/en/class.error.php) implements the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface.
- `ParseError` extends the [`Error`](http://php.net/manual/en/class.error.php) class.

## When Should You Use It?

At the most basic level, a `ParseError` can be thrown anytime code that is not syntactically correct is executed.  This can occur in a variety of ways, but the most obvious is a typo.  For example, perhaps you have two variables, `$a` and `$b`, and you want to assign the value of one to the other, so you'd write a statement like:

```php
$a = $b;
```

Super basic and common, but what happens if we accidentally forget the `$` symbol for one of those variables?

```php
a = $b;
```

Boom!  Already we're getting a `ParseError` because PHP doesn't recognize `a` without a signifier (like `$`), or some indication that it's another type of object (like a function):

```
PHP Parse error:  syntax error, unexpected '=' in /media/sf_Airbrake.io/Exceptions/PHP/Error/ParseError/code.php on line 8
```

Minor typos are all well and good, but the biggest potential for `ParseError` troubles is when using the [`eval()`](http://php.net/manual/en/function.eval.php) function.  While many developers would suggest `eval()` be ignored and left by the wayside, the basic purpose of `eval()` is to evaluate and execute the passed `string` argument as PHP code.  _Any string_ passed into `eval()` will be executed as if it was normal PHP code.  This has the power to be beneficial in _some_ outlier cases, but, for the most part, using `eval()` is considered extremely dangerous.  At the very least, it should never be used in combination with user-provided data, since that would open an avenue of attack similar to an SQL injection attack, except with direct access to your application's codebase.  Yikes!

Heh, of course, now that we've said all that, we'll continue this `ParseError` examination with a bunch of `eval()` examples!  (Do as I say, not as I do, yadda yadda yadda...)  While all the following code is taken from (and found in) the provided code snippet, it's always wise to understand any code that's acquired elsewhere before executing it on your own system, _particularly_ if it's using dangerous capabilities like the `eval()` function.  Therefore, while this code is completely safe, if you don't understand something about it then it's best to avoid running it.

Here's the full, working example code we'll be using for the rest of this article.  We'll break it down a bit more afterward:

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

    /**
     * Book constructor.
     *
     * @param Book|string $title Book title.
     * @param Book|string $author Book author.
     * @param Book|int $pageCount Book page count.
     */
    public function __construct(string $title, string $author, int $pageCount) {
        $this->setTitle($title);
        $this->setAuthor($author);
        $this->setPageCount($pageCount);
    }

    /**
     * Get the author.
     *
     * @return string Book author.
     */
    public function getAuthor() {
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
    public function getPageCount() {
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
    public function getTitle() {
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
    // Execute normal, inline code.
    normalCodeTest();

    Logging::LineSeparator();

    // Perform eval() with valid string.
    evaluationTest();

    Logging::LineSeparator();

    // Perform eval() with invalid string.
    invalidEvaluationTest();
}

/**
 * Perform normal code test, defining a book, outputting it, changing book properties, and confirming updates.
 */
function normalCodeTest()
{
    // Create a new Book instance.
    $book = new Book("Mockingjay", "Suzanne Collins", 390);
    // Output book.
    Logging::Log($book);

    // Divide output
    Logging::LineSeparator();


    $book->setTitle("The Stand");
    $book->setAuthor("Stephen King");
    $book->setPageCount(823);
    Logging::Log($book);
}

/**
 * Execute code similar to normalCodeTest, but through string to eval().
 */
function evaluationTest()
{
    $code = <<<'CODE'
        $book = new Book("The Hobbit", "J.R.R. Tolkien", 304);
        Logging::Log($book);
    
        Logging::LineSeparator();
    
        $book->setTitle("The Fellowship of the Ring");
        $book->setAuthor("J.R.R. Tolkien");
        $book->setPageCount(479);
        Logging::Log($book);
CODE;

    // Pass to evaluator.
    evaluateCode($code);
}

/**
 * Pass invalid string to eval().
 */
function invalidEvaluationTest()
{
    $code = <<<'CODE'
        $book = new Book(The Two Towers", "J.R.R. Tolkien", 415);
        Logging::Log($book);
    
        Logging::LineSeparator();
    
        $book->setTitle("The Return of the King");
        $book->setAuthor("J.R.R. Tolkien");
        $book->setPageCount(349);
        Logging::Log($book);
CODE;

    // Pass to evaluator.
    evaluateCode($code);
}

/**
 * Evaluate passed code using eval() function.  Catch any Errors that may occur.
 *
 * @param Code|string $code Code to be evaluated.
 */
function evaluateCode(string $code)
{
    try {
        // Evaluate code.
        eval($code);
    } catch (ParseError $error) {
        // Output expected ParseError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }
}

executeExamples();

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

To do something a little more interesting then just assigning the value of variable `$b` to variable `$a`, we've included a simple `Book` class to create book objects:

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
        $this->setTitle($title);
        $this->setAuthor($author);
        $this->setPageCount($pageCount);
    }

    /**
     * Get the author.
     *
     * @return string Book author.
     */
    public function getAuthor() {
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
    public function getPageCount() {
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
    public function getTitle() {
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

Now, before we get into using the frightening `eval()` function we'll start with a normal code example.  In this function we're creating a new `Book` object and passing some initial property values as arguments, then outputting the content of the `$book` object variable to the log.  Finally, we modify the properties of our object and output again to confirm the changes have taken place:

```php
/**
 * Perform normal code test, defining a book, outputting it, changing book properties, and confirming updates.
 */
function normalCodeTest()
{
    // Create a new Book instance.
    $book = new Book("Mockingjay", "Suzanne Collins", 390);
    // Output book.
    Logging::Log($book);

    // Divide output
    Logging::LineSeparator();

    // Modify properties and output again.
    $book->setTitle("The Stand");
    $book->setAuthor("Stephen King");
    $book->setPageCount(823);
    Logging::Log($book);
}
```

As expected, we see both outputs for our original and modified property set work just fine:

```
┌──────────────────────────────────────────────────────────────────────────────┐
│ $object                                                                      │
└──────────────────────────────────────────────────────────────────────────────┘
Book (3) (
    private 'author' -> string (15) "Suzanne Collins"
    private 'pageCount' -> integer 390
    private 'title' -> string (10) "Mockingjay"
)
════════════════════════════════════════════════════════════════════════════════

┌──────────────────────────────────────────────────────────────────────────────┐
│ $object                                                                      │
└──────────────────────────────────────────────────────────────────────────────┘
Book (3) (
    private 'author' -> string (12) "Stephen King"
    private 'pageCount' -> integer 823
    private 'title' -> string (9) "The Stand"
)
════════════════════════════════════════════════════════════════════════════════
```

### Heredoc and Nowdoc

Before we get into the code that makes use of `eval()` we should briefly discuss the special syntax we'll be using to define the larger-than-normal strings that we'll be passing to `eval()`.  This syntax is called `heredoc` or `nowdoc`, depending on a slight variation.  Both `heredoc` and `nowdoc` define strings with three less-than symbols (`<<<`).  This operator is then followed by an `identifier`, then a new line, including however many lines of strings you wish to add, followed, at last, by another new line with a matching `identifier` symbol and a closing semi-colon (`;`).

For example, the following is a `heredoc` declaration of a single line of text, using the `identifier` of `IDENT`:

```php
$value = <<<IDENT
    This is my line of text.
IDENT;
```

The `identifier` can be any set of alphanumeric characters that you desire, though many examples online will use `EOF` or `EOD`.  It is also _required_ that the closing `identifier` tag appear _without indentation_ on the final line.  Adding indentation beforehand causes the parser to think it's a continuation of the actual string content.

`nowdoc` syntax is _slightly_ different than `heredoc` (seen above), in that `nowdoc` requires the initial `identifier` to be surrounded by single-quotations: `<<<'IDENT'`.

The difference between `heredoc` and `nowdoc` is that `heredoc` will _parse the string value for potential evaluation points_, whereas `nowdoc` will _ignore_ all potential evaluations.  To illustrate, consider this example, where we've declared the `$name` variable and are using it inline with both a `heredoc` and a `nowdoc` string declaration:

```php
$name = "John Doe";

// Create heredoc version.
$output = <<<LINE
    My name is $name.
LINE;
// Output heredoc version.
print_r($output);

// Create nowdoc version.
$output = <<<'LINE'
    My name is $name.
LINE;
// Output nowdoc version.
print_r($output);
```

And here's the output of both `print_r()` calls:

```
My name is John Doe.
My name is $name.
```

As we can see, since `heredoc` evaluates any inner variables and the like before the string is generated, the parser converts `$name` to `John Doe` prior to output.  On the other hand, `nowdoc` treats the entire string as a literal, thereby avoiding evaluation of `$name`.

Alright, now that we're covered our string declaration method we can get back to the code example.  In the `evaluationTest()` function we're executing nearly identical code as we did above, except we're using different books.  Most importantly, we're writing everything out **as a `string`** value, which we assign to the local `$code` variable and pass to the `evaluateCode()` function.  `evaluateCode()` passes its `$code` string parameter to the `eval()` function, while attempting to catch any potential errors that might be thrown.

We're also using `nowdoc` instead of `heredoc` to declare our string, because this forces the PHP parser to _ignore_ any potential statements it might otherwise evaluate, so everything is treated as a literal string, which is a requirement if we want `eval()` to read our code as expected:

```php
/**
 * Execute code similar to normalCodeTest, but through string to eval().
 */
function evaluationTest()
{
    $code = <<<'CODE'
        $book = new Book("The Hobbit", "J.R.R. Tolkien", 304);
        Logging::Log($book);
    
        Logging::LineSeparator();
    
        $book->setTitle("The Fellowship of the Ring");
        $book->setAuthor("J.R.R. Tolkien");
        $book->setPageCount(479);
        Logging::Log($book);
CODE;

    // Pass to evaluator.
    evaluateCode($code);
}

/**
 * Evaluate passed code using eval() function.  Catch any Errors that may occur.
 *
 * @param Code|string $code Code to be evaluated.
 */
function evaluateCode(string $code)
{
    try {
        // Evaluate code.
        eval($code);
    } catch (ParseError $error) {
        // Output expected ParseError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }
}
```

Executing the above passes our string to `eval()`, which then evaluates it as if it were normally written code.  The result is a nearly-identical output as before, but with the appropriate book info changes:

```
┌──────────────────────────────────────────────────────────────────────────────┐
│ $object                                                                      │
└──────────────────────────────────────────────────────────────────────────────┘
Book (3) (
    private 'author' -> string (14) "J.R.R. Tolkien"
    private 'pageCount' -> integer 304
    private 'title' -> string (10) "The Hobbit"
)
════════════════════════════════════════════════════════════════════════════════

┌──────────────────────────────────────────────────────────────────────────────┐
│ $object                                                                      │
└──────────────────────────────────────────────────────────────────────────────┘
Book (3) (
    private 'author' -> string (14) "J.R.R. Tolkien"
    private 'pageCount' -> integer 479
    private 'title' -> string (26) "The Fellowship of the Ring"
)
════════════════════════════════════════════════════════════════════════════════
```

That's all well and good, and in a perfect world, we could use `eval()` all day without running into any trouble.  However, it's all too easy to pass a _slightly_ incorrect string to `eval()` and cause the whole thing to break down (or worse).  To illustrate, here we have the `invalidEvaluationTest()` function:

```php
/**
 * Pass invalid string to eval().
 */
function invalidEvaluationTest()
{
    $code = <<<'CODE'
        $book = new Book(The Two Towers", "J.R.R. Tolkien", 415);
        Logging::Log($book);
    
        Logging::LineSeparator();
    
        $book->setTitle("The Return of the King");
        $book->setAuthor("J.R.R. Tolkien");
        $book->setPageCount(349);
        Logging::Log($book);
CODE;

    // Pass to evaluator.
    evaluateCode($code);
}
```

It may be difficult to spot, but there's a small (yet, significant) typo in our declared code string that we want to pass to `evaluateCode()`: The initial quotation mark surrounding `"The Two Towers"` title is missing.  As a result, executing this function throws a `ParseError` at us:

```
EXPECTED] ParseError: syntax error, unexpected 'Two' (T_STRING), expecting ',' or ')' in /media/sf_Airbrake.io/Exceptions/PHP/Error/ParseError/code.php(169) : eval()'d code on line 1

Call Stack:
    0.2864     373496   1. {main}() /media/sf_Airbrake.io/Exceptions/PHP/Error/ParseError/code.php:0
    0.3047    1672400   2. executeExamples() /media/sf_Airbrake.io/Exceptions/PHP/Error/ParseError/code.php:179
    0.3311    1529048   3. invalidEvaluationTest() /media/sf_Airbrake.io/Exceptions/PHP/Error/ParseError/code.php:95
    0.3311    1529048   4. evaluateCode(???) /media/sf_Airbrake.io/Exceptions/PHP/Error/ParseError/code.php:157
```

Check out the <a class="js-cta-utm" href="https://airbrake.io/languages/php_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-php">Airbrake-PHP library</a>, designed to quickly and easily integrate into any PHP project, giving you and your team access to real-time error monitoring and reporting throughout your application's entire life cycle.  With automatic, instantaneous error and exception notifications at your fingertips, you'll be constantly aware of your application's health, including any issues that may arise.  Best of all, with Airbrake's robust web dashboard cataloging every error that occurs, you and your team can immediately dive into the exact details of what went wrong, making it easy to quickly recognize and resolve problems.

---

__META DESCRIPTION__

A deep dive into the PHP ParseError class, including code samples to illustrate and explain the heredoc syntax, nowdoc syntax, and eval function.