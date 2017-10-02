<?php
// Publication.php
/**
 * Class Publication
 */
class Publication extends Book
{
    const PublicationTypes = [
        'audio',
        'blog',
        'digital',
        'novel',
    ];

    public $publicationType;

    /**
     * Publication constructor.
     *
     * @param Publication|string $title Publication title.
     * @param Publication|string $author Publication author.
     * @param string $publicationType Publication type.
     * @param int $pageCount Publication page count.
     * @param DateTime $publicationDate Publication publication date.
     */
    public function __construct(string $title, string $author, string $publicationType = null, int $pageCount = 0, DateTime $publicationDate = null) {
        parent::__construct($title, $author, $pageCount, $publicationDate);
        $this->setPublicationType($publicationType);
    }

    /**
     * Set the publication type.
     *
     * @return string Publication type.
     */
    public function getPublicationType(): ?string {
        // Check if current type is in valid types list.
        if (in_array($this->publicationType, Publication::PublicationTypes)) {
            // Return valid type.
            return $this->publicationType;
        } else {
            // If current type is invalid, throw RangeException.
            throw new RangeException("Publication set to unknown type: $this->publicationType");
        }
    }

    /**
     * Get the publication type.
     *
     * @param string $publicationType Publication type value to be set.
     */
    public function setPublicationType(string $publicationType = null) {
        if ($publicationType == null) return;
        // Check if passed type is in valid types list.
        if (in_array($publicationType, Publication::PublicationTypes)) {
            // Set publication type.
            $this->publicationType = $publicationType;
        } else {
            // If passed type not found in valid list, throw Domain Exception.
            throw new DomainException("Cannot set publication type to unknown type: $publicationType");
        }
    }
}