<?php

include("D:\work\Airbrake.io\lib\php\Logging.php");

/**
 * Class Publication
 */
class Publication
{
    const PublicationTypes = [
        'audio',
        'blog',
        'digital',
        'novel',
    ];

    private $author;
    private $publicationType;
    private $title;

    /**
     * Publication constructor.
     *
     * @param Publication|string $title Publication title.
     * @param Publication|string $author Publication author.
     * @param string $publicationType Publication type.
     */
    public function __construct(string $title, string $author, string $publicationType) {
        $this->setAuthor($author);
        $this->setPublicationType($publicationType);
        $this->setTitle($title);
    }

    /**
     * Get the author.
     *
     * @return string Publication author.
     */
    public function getAuthor(): ?string {
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
     * Set the publication type.
     *
     * @return string Publication type.
     */
    public function getPublicationType(): ?string {
        return $this->publicationType;
    }

    /**
     * Get the publication type.
     *
     * @param string $publicationType Publication type value to be set.
     */
    public function setPublicationType(string $publicationType) {
        // Check if passed type is in valid types list.
        if (in_array($publicationType, Publication::PublicationTypes)) {
            // Set publication type.
            $this->publicationType = $publicationType;
        } else {
            // If passed type not found in valid list, throw Domain Exception.
            throw new DomainException("Cannot set publication type to unknown type: $publicationType");
        }
    }

    /**
     * Get the title.
     *
     * @return string Publication title.
     */
    public function getTitle(): ?string {
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
    Logging::LineSeparator("VALID PUBLICATION TYPE");
    setValidPublicationType();

    Logging::LineSeparator("INVALID PUBLICATION TYPE");
    setInvalidPublicationType();
}

function setValidPublicationType() {
    try {
        // Create new Publication instance with a valid publication type.
        $publication = new Publication("A Game of Thrones", "George R. R. Martin", 'digital');
        Logging::Log($publication);
    } catch (DomainException $exception) {
        // Output expected DomainException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

function setInvalidPublicationType() {
    try {
        // Create new Publication instance with an invalid publication type.
        $publication = new Publication("A Storm of Swords", "George R. R. Martin", 'poem');
        Logging::Log($publication);
    } catch (DomainException $exception) {
        // Output expected DomainException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
}

executeExamples();