<?php

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