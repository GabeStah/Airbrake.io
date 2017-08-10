<?php

include("D:\work\Airbrake.io\lib\php\Logging.php");

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
    callInvalidInstanceMethod();
    callInvalidStaticMethod();
    callInvalidFunction();
}

function callInvalidInstanceMethod() {
    try {
        // Create new Book instance.
        $book = new Book("A Game of Thrones", "George R. R. Martin", 835);
        // Call invalid method.
        $book->checkout();
    } catch (BadMethodCallException $exception) {
        // Output expected BadMethodCallException.
        Logging::Log($exception);
    } catch (BadFunctionCallException $exception) {
        // Output unexpected BadFunctionCallException.
        Logging::Log($exception, false);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

function callInvalidStaticMethod() {
    try {
        // Call invalid static method.
        Book::checkout();
    } catch (BadMethodCallException $exception) {
        // Output expected BadMethodCallException.
        Logging::Log($exception);
    } catch (BadFunctionCallException $exception) {
        // Output unexpected BadFunctionCallException.
        Logging::Log($exception, false);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

function callInvalidFunction() {
    try {
        // Call an invalid function.
        invalidFunction();
    } catch (BadMethodCallException $exception) {
        // Output unexpected BadMethodCallException.
        Logging::Log($exception, false);
    } catch (BadFunctionCallException $exception) {
        // Output expected BadFunctionCallException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    } catch (Error $error) {
        // Output unexpected Errors.
        Logging::Log($error,false);
    }
}

executeExamples();