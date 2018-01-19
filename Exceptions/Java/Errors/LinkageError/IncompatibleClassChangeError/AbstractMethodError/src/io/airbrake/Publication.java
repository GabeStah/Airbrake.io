package io.airbrake;

import java.util.Date;

abstract class Publication {
    /**
     * Get author of book.
     *
     * @return Author name.
     */
    abstract String getAuthor();

    /**
     * Get published date of book.
     *
     * @return Published date.
     */
    abstract Date getPublishedAt();

    /**
     * Get a formatted tagline with author, title, page count, publication date, and publication type.
     *
     * @return Formatted tagline.
     */
    abstract String getTagline();

    abstract String another();


    /**
     * Get title of book.
     *
     * @return Title.
     */
    abstract String getTitle();

    /**
     * Set author of book.
     *
     * @param author Author name.
     */
    abstract void setAuthor(String author);

    /**
     * Set published date of book.
     *
     * @param publishedAt Page count.
     */
    abstract void setPublishedAt(Date publishedAt);

    /**
     * Set title of book.
     *
     * @param title Title.
     */
    abstract void setTitle(String title);
}
