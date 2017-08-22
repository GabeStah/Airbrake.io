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