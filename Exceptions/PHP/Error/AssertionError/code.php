<?php

require('/media/sf_Airbrake.io/lib/php/Logging.php');

error_reporting(E_ERROR | E_WARNING);

/**
 * Class Book
 */
class Book {

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
    public function getAuthor()
    {
        return $this->author;
    }

    /**
     * Set the author.
     *
     * @param string $value Author value to be set.
     */
    public function setAuthor(string $value)
    {
        $this->author = $value;
    }

    /**
     * Set the current page count of Book.
     *
     * @return mixed Page count of Book.
     */
    public function getPageCount()
    {
        return $this->pageCount;
    }

    /**
     * Get the current page count of Book.
     *
     * @param int $pageCount Page count to set.
     */
    public function setPageCount(int $pageCount)
    {
        $this->pageCount = $pageCount;
    }

    /**
     * Get the title.
     *
     * @return string Book title.
     */
    public function getTitle()
    {
        return $this->title;
    }

    /**
     * Set the title.
     *
     * @param string $value Title value to be set.
     */
    public function setTitle(string $value)
    {
        $this->title = $value;
    }
}

function executeExamples() {
//    // Assert that 1 == 2.
//    assertEquality(1, 2);
//    // Assert that 2 == 2.
//    assertEquality(2, 2);

    //Logging::LineSeparator();

    //assert_options(ASSERT_CALLBACK, 'onAssertionFailure');

//    // Enable assertion (Default: true)
//    assert_options(ASSERT_ACTIVE,   true);
//    // Enable warning when assertion fails (Default: true)
//    assert_options(ASSERT_WARNING,  true);
//    // Enable termination if assertion is failed (Default: false)
//    assert_options(ASSERT_BAIL,     true);
    // Call a custom callback function for assertion failure.
    assert_options(ASSERT_CALLBACK, 'onAssertionFailure');

    // Assert that 1 == 2.
    assertEquality(1, 2);
    // Assert that 2 == 2.
    assertEquality(2, 2);
}

/**
 * @param $fileName Name of the executed script file.
 * @param $line Code line of failed assertion.
 * @param $ph Unknown placeholder.
 * @param $message Assertion description.
 */
function onAssertionFailure($fileName, $line, $ph, $message) {
    Logging::Log("Assertion at $fileName:$line failed with message: $message");
}

function assertEquality($a, $b) {
    try {
        // Assert equality.
        if (assert($a == $b, "assert($a == $b) failed.")) {
            Logging::Log("assert($a == $b) was successful.");
        }
    } catch (AssertionError $error) {
        // Output expected AssertionError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }
}

function assertionExample2() {
    // Enable assertion (Default: true)
    assert_options(ASSERT_ACTIVE,   true);
    // Enable termination if assertion is failed (Default: false)
    assert_options(ASSERT_BAIL,     true);
    // Enable warning when assertion fails (Default: true)
    //assert_options(ASSERT_WARNING,  true);
    assert_options(ASSERT_CALLBACK, 'onAssertionFailure');

    try {
        $a = 1;
        $b = 2;
        // Assert something.
        assert($a == $b, "Assertion ($a == $b) failed.");
    } catch (AssertionError $error) {
        // Output expected AssertionError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }
}


executeExamples();