<?php
// Character.php
/**
 * Class Character
 */
class Character
{
    private $name;
    private $biography;

    /**
     * Character constructor.
     *
     * @param string $name Character name.
     * @param string $biography Character biography.
     */
    public function __construct(string $name, string $biography = null) {
        $this->setName($name);
        $this->setBiography($biography);
    }

    /**
     * Get the character biography.
     *
     * @return mixed Character biography.
     */
    public function getBiography(): ?string {
        return $this->biography;
    }

    /**
     * Set the character biography.
     *
     * @param int $pageCount Biography to set.
     */
    public function setBiography(?string $biography) {
        $this->biography = $biography;
    }

    /**
     * Get the name.
     *
     * @return string Character name.
     */
    public function getName(): string {
        return $this->name;
    }

    /**
     * Set the name.
     *
     * @param string $value Name value to be set.
     */
    public function setName(string $value) {
        $this->name = $value;
    }
}