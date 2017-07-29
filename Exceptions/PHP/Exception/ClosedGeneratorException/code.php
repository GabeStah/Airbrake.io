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
}

function executeExamples()
{
    $books = [
        new Book("Mockingjay", "Suzanne Collins", 390),
        new Book("The Stand", "Stephen King", 823),
        new Book("Adventures of Huckleberry Finn", "Mark Twain", 366),
        new Book("A Game of Thrones", "George R. R. Martin", 835),
        new Book("The Eye of the World", "Robert Jordan", 814),
    ];

    iteratorExample($books);

    Logging::LineSeparator();
    Logging::LineSeparator();

    generatorExample($books);

    Logging::LineSeparator();
    Logging::LineSeparator();

    generatorSelfClosingExample($books);
}

/**
 * Generator for collection.
 *
 * @param $collection
 * @return Generator
 */
function generator($collection) {
    foreach($collection as $element) {
        yield $element;
    }
}

function generatorExample($books) {
    try {
        // Create basic generator.
        $generator = generator($books);

        // Output current element.
        Logging::Log($generator->current());

        // Output next element.
        $generator->next();
        Logging::Log($generator->current());

        // Rewind to original state.
        $generator->rewind();

        // Output next element.
        $generator->next();
        Logging::Log($generator->current());
    } catch (ClosedGeneratorException $exception) {
        // Output expected ClosedGeneratorException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output any unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

/**
 * Collection generator that self-closes after one yield.
 *
 * @param $collection
 * @return Generator|void
 */
function generatorSelfClosing($collection) {
    $count = 0;
    foreach($collection as $element) {
        $count++;
        if ($count >= 2) {
            return;
        }
        yield $element;
    }
}

function generatorSelfClosingExample($books) {
    try {
        // Create basic generator.
        $generator = generatorSelfClosing($books);

        // Output current element.
        Logging::Log($generator->current());

        // Output next element, which should terminate and return null.
        $generator->next();
        Logging::Log($generator->current());

        // Iterator through now-closed generator.
        foreach($generator as $book) {
            Logging::Log($book);
        }
    } catch (ClosedGeneratorException $exception) {
        // Output expected ClosedGeneratorException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output any unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

/**
 * Class CollectionIterator
 *
 * Iterator that handles collections.
 */
class CollectionIterator implements Iterator {
    protected $collection;
    protected $element;
    protected $key = 0;

    public function __construct($collection) {
        $this->collection = $collection;
    }

    /**
     * Revert to original state.
     */
    public function rewind() {
        $this->key = 0;
        $this->element = $this->collection[$this->key];
    }

    /**
     * Determine if current element is valid.
     *
     * @return bool
     */
    public function valid() {
        return false !== $this->element;
    }

    /**
     * Get current element.
     *
     * @return mixed
     */
    public function current() {
        return $this->element;
    }

    /**
     * Get the current iteration key.
     *
     * @return int
     */
    public function key() {
        return $this->key;
    }

    /**
     * Perform iteration by setting current element and iterating key.
     */
    public function next() {
        if (false !== $this->element && $this->key < count($this->collection)) {
            $this->element = $this->collection[$this->key];
            $this->key++;
        }
    }
}

/**
 * Iterates through passed book collection.
 *
 * @param $books Book collection to iterate.
 */
function iteratorExample($books) {
    try {
        // Create new iterator.
        $iterator = new CollectionIterator($books);

        // Output next element.
        $iterator->next();
        Logging::Log($iterator->current());

        // Output next element.
        $iterator->next();
        Logging::Log($iterator->current());

        // Rewind to original state.
        Logging::LineSeparator();
        Logging::Log("Rewinding...");
        Logging::LineSeparator();
        $iterator->rewind();

        // Output next element, should be original.
        $iterator->next();
        Logging::Log($iterator->current());
    } catch (ClosedGeneratorException $exception) {
        // Output expected ClosedGeneratorException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output any unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

executeExamples();