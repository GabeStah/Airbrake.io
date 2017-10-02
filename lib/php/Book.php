<?php
// Book.php

include("Character.php");

/**
 * Class Book
 */
class Book
{
    private $author;
    private $characters = [];
    private $pageCount;
    private $publicationDate;
    private $title;

    // Maximum byte length of author field.
    const AUTHOR_MAX_LENGTH = 255;
    // Maximum allow number of characters.
    const MAX_CHARACTER_COUNT = 5;
    // Maximum byte length of title field.
    const TITLE_MAX_LENGTH = 65535;

    /**
     * Book constructor.
     *
     * @param string $title Book title.
     * @param string $author Book author.
     * @param int $pageCount Book page count.
     * @param DateTime $publicationDate Book publication date.
     */
    public function __construct(string $title, string $author, int $pageCount = 0, DateTime $publicationDate = null) {
        $this->setAuthor($author);
        $this->setPageCount($pageCount);
        $this->setPublicationDate($publicationDate);
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
     * Get the publication date.
     *
     * @return DateTime Publication date.
     */
    public function getPublicationDate() : ?DateTime {
        return $this->publicationDate;
    }

    /**
     * Set the publication date.
     *
     * @param DateTime $date Publication date.
     */
    public function setPublicationDate(?DateTime $date) {
        $this->publicationDate = $date;
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
     * Add a Character to the Book.
     *
     * @param Character $character
     * @param mixed $key
     */
    public function addCharacter(Character $character, $key = null) {
        if (count($this->characters) >= self::MAX_CHARACTER_COUNT) {
            $max = self::MAX_CHARACTER_COUNT;
            throw new OverflowException("Character count cannot exceed maximum of $max");
        }
        if (!is_null($key)) {
            $this->characters[$key] = $character;
        } else {
            $this->characters[] = $character;
        }
    }

    /**
     * Get a Character using passed key.
     *
     * @param mixed $key
     * @return mixed
     */
    public function getCharacter($key) {
        if (!array_key_exists($key, $this->characters)) {
            throw new OutOfBoundsException("Character element at key $key does not exist.");
        }
        return $this->characters[$key];
    }

    /**
     * Get Characters collection.
     *
     * @return array
     */
    public function getCharacters() : array {
        return $this->characters;
    }

    /**
     * Set characters collection.
     *
     * @param array $characters
     */
    public function setCharacters(array $characters) {
        $this->characters = $characters;
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