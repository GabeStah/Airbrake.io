# PHP Exception Handling - DOMException

Next up in our in-depth [__PHP Exception Handling__](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) series, today we're digging into the PHP DOMException error.  There are a variety of reasons a `DOMException` can occur, but all of them are related to using the [`Document Object Model`](http://php.net/manual/en/book.dom.php) namespace and its powerful functionality.

In this article we'll examine the `DOMException` in more detail, including where it sits in the [PHP Exception Hierarchy](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy), and how `DOMExceptions` might be commonly thrown using some functional sample code, so let's get to it!

## The Technical Rundown

- All PHP errors implement the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface, or are extended from another inherited class therein.
- [`Exception`](http://php.net/manual/en/class.exception.php) implements the [`Throwable`](https://airbrake.io/blog/php-exception-handling/the-php-exception-class-hierarchy) interface.
- `DOMException` extends the [`Exception`](http://php.net/manual/en/class.exception.php) class.

## When Should You Use It?

Let's jump right into it by looking at the full working code sample we have, which features two kinds of `DOMExceptions` that might typically occur.  Following the sample below, we'll break the code down in more detail to see exactly what's going on:

```php
<?php

function executeExamples()
{
    // Create document.
    $document = new DOMDocument('1.0');

    // Append 'books' element.
    appendElementToDocument(new DOMElement('books'), $document);

    Logging::LineSeparator();

    // Append '$' element.
    appendElementByNameToDocument('$', $document);

    Logging::LineSeparator();

    crossDocumentAppendTest();

    Logging::LineSeparator();

    crossDocumentAppendTestSuccess();
}

/**
 * Create and append DOMElement, by name, to passed DOMDocument.
 *
 * @param string $name
 * @param DOMDocument $document
 * @return DomNode|null
 */
function appendElementByNameToDocument(string $name, DOMDocument $document) : ?DomNode {
    try {
        $node = $document->appendChild(new DOMElement($name));
        Logging::Log("Successfully appended element (by name) [{$node->localName}] to [document].");
        return $node;
    } catch (DOMException $exception) {
        // Output expected DOMException.
        Logging::Log($exception);
        return null;
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
        return null;
    }
}

/**
 * Append passed DOMElement to passed DOMDocument.
 *
 * @param DOMElement $element
 * @param DOMDocument $document
 * @return DOMNode|null
 */
function appendElementToDocument(DOMElement $element, DOMDocument $document) : ?DOMNode {
    try {
        $node = $document->appendChild($element);
        Logging::Log("Successfully appended element [{$node->localName}] to [document].");
        return $node;
    } catch (DOMException $exception) {
        // Output expected DOMException.
        Logging::Log($exception);
        return null;
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
        return null;
    }
}

/**
 * Append element to unattached document.
 */
function crossDocumentAppendTest() {
    try {
        // Create document and element set A.
        $documentA = new DOMDocument('1.0');
        $elementA = new DOMElement('elementA');
        appendElementToDocument($elementA, $documentA);

        // Create document and element set B.
        $documentB = new DOMDocument('1.0');
        $elementB = new DOMElement('elementB');
        appendElementToDocument($elementB, $documentB);

        // Append elementA (appended to documentA) to documentB.
        appendElementToDocument($elementA, $documentB);

        Logging::Log("Successfully appended element [{$elementA->localName}] to [documentB].");
    } catch (DOMException $exception) {
        // Output expected DOMException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

/**
 * Successfully appends element to unattached document.
 */
function crossDocumentAppendTestSuccess() {
    try {
        // Create document and element set A.
        $documentA = new DOMDocument('1.0');
        $elementA = new DOMElement('elementA');
        appendElementToDocument($elementA, $documentA);

        // Create document and element set B.
        $documentB = new DOMDocument('1.0');
        $elementB = new DOMElement('elementB');
        appendElementToDocument($elementB, $documentB);

        // Append elementA (appended to documentA) to documentB.
        $node = appendElementToDocument($elementA, $documentB);

        // Check if appendElementToDocument returned DOMNode or null.
        if (!$node) {
            // If $node not found, import elementA into documentB, then try appending again.
            $importedNode = $documentB->importNode($elementA, true);
            $node = appendElementToDocument($importedNode, $documentB);
            Logging::Log("Successfully appended element [{$node->localName}] to [documentB].");
        }
    } catch (DOMException $exception) {
        // Output expected DOMException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

executeExamples();

// Logging.php
<?php
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
     * @param object $object Object to be logged.
     *
     * @see https://github.com/kint-php/kint    Kint tool used for structured outputs.
     */
    private static function LogObject(object $object) {
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

Our first goal is to create a new `DOMDocument` instance, and append a new `DOMElement` instance to it.  To that end, we have two similar methods, `appendElementToDocument(DOMElement, DOMDocument)` and `appendElementByNameToDocument(string, DOMDocument)`:

```php
/**
 * Create and append DOMElement, by name, to passed DOMDocument.
 *
 * @param string $name
 * @param DOMDocument $document
 * @return DomNode|null
 */
function appendElementByNameToDocument(string $name, DOMDocument $document) : ?DomNode {
    try {
        $node = $document->appendChild(new DOMElement($name));
        Logging::Log("Successfully appended element (by name) [{$node->localName}] to [document].");
        return $node;
    } catch (DOMException $exception) {
        // Output expected DOMException.
        Logging::Log($exception);
        return null;
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
        return null;
    }
}

/**
 * Append passed DOMElement to passed DOMDocument.
 *
 * @param DOMElement $element
 * @param DOMDocument $document
 * @return DOMNode|null
 */
function appendElementToDocument(DOMElement $element, DOMDocument $document) : ?DOMNode {
    try {
        $node = $document->appendChild($element);
        Logging::Log("Successfully appended element [{$node->localName}] to [document].");
        return $node;
    } catch (DOMException $exception) {
        // Output expected DOMException.
        Logging::Log($exception);
        return null;
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
        return null;
    }
}
```

These methods are comparable, except one expects an actual `DOMElement` instance to be passed as an argument, while the other uses a passed string `$name` to create said element.  Either way, the element is then appended to the passed `DOMDocument` using the `appendChild()` method.  If successful, we output a message, and if it fails, we throw (and catch) the exception.

To test these methods out we have just a few lines of code.  We start by creating a new `DOMDocument` instance, then call `appendElementToDocument(DOMElement, DOMDocument)` with a new element named `books`:

```php
// Create document.
$document = new DOMDocument('1.0');

// Append 'books' element.
appendElementToDocument(new DOMElement('books'), $document);
```

This works just as expected and the output confirms the result:

```
Successfully appended element [books] to [document].
```

Now let's try the `appendElementByNameToDocument(string, DOMDocument)` method to create a `DOMElement` with the name of a dollar sign (`$`):

```php
// Append '$' element.
appendElementByNameToDocument('$', $document);
```

As it happens, the `$` symbol is not a valid character within a DOM element name, so a `DOMException` is thrown, indicating as much in the message:

```
[EXPECTED] DOMException: Invalid Character Error in D:\work\Airbrake.io\Exceptions\PHP\Exception\DOMException\code.php on line 36
```

Our second test involves appending `DOMElements` to `DOMDocuments` again, but this time we're trying something new: First, we append an element to a document, then we try to append that same element to a _different_ document.  This is performed in the `crossDocumentAppendTest()` method:

```php
/**
 * Append element to unattached document.
 */
function crossDocumentAppendTest() {
    try {
        // Create document and element set A.
        $documentA = new DOMDocument('1.0');
        $elementA = new DOMElement('elementA');
        appendElementToDocument($elementA, $documentA);

        // Create document and element set B.
        $documentB = new DOMDocument('1.0');
        $elementB = new DOMElement('elementB');
        appendElementToDocument($elementB, $documentB);

        // Append elementA (appended to documentA) to documentB.
        appendElementToDocument($elementA, $documentB);

        Logging::Log("Successfully appended element [{$elementA->localName}] to [documentB].");
    } catch (DOMException $exception) {
        // Output expected DOMException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}
```

Everything works just fine until we get to the third `appendElementToDocument(DOMElement, DOMDocument)` call, which attempts to _crossover_ the append action by appending `$elementA` to `$documentB`, even though `$elementA` is already appended to `$documentA`.  As you can probably guess, this also results in a `DOMException` being thrown, this time indicating that we are manipulating the wrong document (referring to `$documentB`, in this case):

```
Successfully appended element [elementA] to [document].
Successfully appended element [elementB] to [document].
[EXPECTED] DOMException: Wrong Document Error in D:\work\Airbrake.io\Exceptions\PHP\Exception\DOMException\code.php on line 59

Call Stack:
    0.0971     375608   1. {main}() D:\work\Airbrake.io\Exceptions\PHP\Exception\DOMException\code.php:0
    0.1082    1787768   2. executeExamples() D:\work\Airbrake.io\Exceptions\PHP\Exception\DOMException\code.php:135
    1.3642    1792352   3. crossDocumentAppendTest() D:\work\Airbrake.io\Exceptions\PHP\Exception\DOMException\code.php:20
    3.7300    1810528   4. appendElementToDocument(???, ???) D:\work\Airbrake.io\Exceptions\PHP\Exception\DOMException\code.php:89
    9.8372    1814896   5. DOMDocument->appendChild(???) D:\work\Airbrake.io\Exceptions\PHP\Exception\DOMException\code.php:59
```

We solve this issue within the `crossDocumentAppendTestSuccess()` method:

```php
/**
 * Successfully appends element to unattached document.
 */
function crossDocumentAppendTestSuccess() {
    try {
        // Create document and element set A.
        $documentA = new DOMDocument('1.0');
        $elementA = new DOMElement('elementA');
        appendElementToDocument($elementA, $documentA);

        // Create document and element set B.
        $documentB = new DOMDocument('1.0');
        $elementB = new DOMElement('elementB');
        appendElementToDocument($elementB, $documentB);

        // Append elementA (appended to documentA) to documentB.
        $node = appendElementToDocument($elementA, $documentB);

        // Check if appendElementToDocument returned DOMNode or null.
        if (!$node) {
            // If $node not found, import elementA into documentB, then try appending again.
            $importedNode = $documentB->importNode($elementA, true);
            $node = appendElementToDocument($importedNode, $documentB);
            Logging::Log("Successfully appended element [{$node->localName}] to [documentB].");
        }
    } catch (DOMException $exception) {
        // Output expected DOMException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}
```

As you may have noticed, the `appendElementToDocument(DOMElement, DOMDocument)` method returns a `DOMNode` _or_ `null` object, which we've specified using the `: ?DOMNode` syntax at the end of the method declaration.  This is a new feature introduced in PHP 7.1+, which makes it easy for us to use the result of that method call for further logic.  Thus, in `crossDocumentAppendTestSuccess()` we assign the `appendElementToDocument(DOMElement, DOMDocument)` result to a `$node` variable and check if it exists or was `null`).  In the event that no `DOMNode` instance was returned, that indicates our original append attempt failed (just as in the previous method), so we can perform one little trick necessary to get this crossover append to work.

The solution is to call the `importNode(DOMNode)` method on the `DOMDocument` instance that is to receive the new appendage.  This imports the node into the document tree, behind the scenes, and returns the resulting `DOMNode` instance, which we've assigned to `$importedNode`.  From there, it's just a matter of calling the `appendElementToDocument(DOMElement, DOMDocument)` method one last time, but **now** we pass the `$importedNode` instance to `$documentB`, instead of the original `$elementA` version.

Our log shows this works just as expected, and the ultimate result is the same, but without throwing an unnecessary `DOMException`:

```
Successfully appended element [elementA] to [documentB].
```

Check out the <a class="js-cta-utm" href="https://airbrake.io/languages/php_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-php">Airbrake-PHP library</a>, designed to quickly and easily integrate into any PHP project, giving you and your team access to real-time error monitoring and reporting throughout your application's entire life cycle.  With automatic, instantaneous error and exception notifications at your fingertips, you'll be constantly aware of your application's health, including any issues that may arise.  Best of all, with Airbrake's robust web dashboard cataloging every error that occurs, you and your team can immediately dive into the exact details of what went wrong, making it easy to quickly recognize and resolve problems.

---

__META DESCRIPTION__

A close look into the PHP DOMException class, including code samples illustrating how to handle common DOMException scenarios yourself.
---

__SOURCES__

- http://php.net/manual/en/class.throwable.php