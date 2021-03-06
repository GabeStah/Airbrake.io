<?php

require('/media/sf_Airbrake.io/lib/php/Logging.php');

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
    test("first", 2);
}

/**
 * There are three scenarios where a TypeError may be thrown.
 * The first is where the argument type being passed to a function does not match its corresponding declared parameter
 * type.
 * The second is where a value being returned from a function does not match the declared function return type.
 * The third is where an invalid number of arguments are passed to a built-in PHP function (strict mode only).
 */

function test() {
    $book = new Book("The Two Towers", "J.R.R. Tolkien", 415);
    Logging::Log($book);
    $book->setTitle(123);
    Logging::Log($book);
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