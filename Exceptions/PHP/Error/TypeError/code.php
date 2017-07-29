<?php
declare(strict_types=1);

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

executeExamples();