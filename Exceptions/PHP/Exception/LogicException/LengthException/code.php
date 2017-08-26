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