<?php

include("D:\work\Airbrake.io\lib\php\Logging.php");

/**
 * Class Book
 */
class Book
{
    private $author;
    private $pageCount;
    private $publicationMonth;
    private $publicationYear;
    private $title;

    // Maximum byte length of author field.
    const AUTHOR_MAX_LENGTH = 255;
    // Minimum publication month.
    const PUBLICATION_MONTH_MIN = 1;
    // Maximum publication month.
    const PUBLICATION_MONTH_MAX = 12;
    // Maximum byte length of title field.
    const TITLE_MAX_LENGTH = 65535;

    /**
     * Book constructor.
     *
     * @param Book|string $title Book title.
     * @param Book|string $author Book author.
     * @param Book|int $pageCount Book page count.
     */
    public function __construct(string $title, string $author, int $pageCount, int $publicationMonth, int $publicationYear) {
        $this->setAuthor($author);
        $this->setPageCount($pageCount);
        $this->setPublicationMonth($publicationMonth);
        $this->setPublicationYear($publicationYear);
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
     * Get the current page count of Book.
     *
     * @return mixed Page count of Book.
     */
    public function getPageCount(): int {
        return $this->pageCount;
    }

    /**
     * Set the current page count of Book.
     *
     * @param int $pageCount Page count to set.
     */
    public function setPageCount(int $pageCount) {
        $this->pageCount = $pageCount;
    }

    /**
     * Get the month of publication.
     *
     * @return int Numeric publication month.
     */
    public function getPublicationMonth(): int {
        return $this->publicationMonth;
    }

    /**
     * Set the month of publication.
     *
     * @param int $month Numeric publication month.
     */
    public function setPublicationMonth(int $month) {
        if ($month < self::PUBLICATION_MONTH_MIN || $month > self::PUBLICATION_MONTH_MAX) {
            throw new OutOfRangeException("Invalid publication month: $month.  Must be between " . self::PUBLICATION_MONTH_MIN . " and " . self::PUBLICATION_MONTH_MAX, E_COMPILE_ERROR);
        }
        $this->publicationMonth = $month;
    }

    /**
     * Get the year of publication.
     *
     * @return int Numeric publication year.
     */
    public function getPublicationYear(): int {
        return $this->publicationYear;
    }

    /**
     * Set the year of publication.
     *
     * @param int $year Numeric publication year.
     */
    public function setPublicationYear(int $year) {
        $this->publicationYear = $year;
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
    try {
        Logging::LineSeparator("FARSEER TRILOGY");
        Logging::Log(new Book("Assassin's Apprentice", "Robin Hobb", 448, 5, 1995));
        Logging::Log(new Book("Royal Assassin", "Robin Hobb", 675, 4, 1996));
        Logging::Log(new Book("Assassin's Quest", "Robin Hobb", 757, 3, 1997));

        Logging::LineSeparator("LIFESHIP TRADERS");
        Logging::Log(new Book("Ship of Magic", "Robin Hobb", 880, 1, 1998));
        Logging::Log(new Book("The Mad Ship", "Robin Hobb", 906, 11, 1999));
        Logging::Log(new Book("Ship of Destiny", "Robin Hobb", 789, 18, 2000));
    } catch (OutOfRangeException $exception) {
        // Output expected OutOfRangeExceptions.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

executeExamples();