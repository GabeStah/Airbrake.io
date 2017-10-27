// Book.java
package io.airbrake;

import java.util.Date;

/**
 * Simple example class to store book instances.
 */
public class Book
{
    private String author;
    private String title;
    private Page[] pages;
    private Date publishedAt;
    private static String publicationType = "Book";

    private static final Integer maximumPageCount = 4000;

    /**
     * Ensure publication type is upper case.
     */
    static {
        publicationType = publicationType.toUpperCase();
    }

    /**
     * Constructs an empty book.
     */
    public Book() { }

    /**
     * Constructs a basic book.
     *
     * @param title Book title.
     * @param author Book author.
     */
    public Book(String title, String author) {
        setAuthor(author);
        setTitle(title);
    }

    /**
     * Constructs a basic book, with page count.
     *
     * @param title Book title.
     * @param author Book author.
     */
    public Book(String title, String author, Page[] pages) {
        setAuthor(author);
        setTitle(title);
    }

    /**
     * Constructs a basic book, with page count.
     *
     * @param title Book title.
     * @param author Book author.
     */
    public Book(String title, String author, Date publishedAt) {
        setAuthor(author);
        setTitle(title);
        setPublishedAt(publishedAt);
    }

    /**
     * Constructs a basic book, with page count.
     *
     * @param title Book title.
     * @param author Book author.
     */
    public Book(String title, String author, Date publishedAt, String publicationType) {
        setAuthor(author);
        setTitle(title);
        setPublishedAt(publishedAt);
        setPublicationType(publicationType);
    }

    /**
     * Get author of book.
     *
     * @return Author name.
     */
    public String getAuthor() {
        return author;
    }

    /**
     * Get current page count.
     *
     * @return Page count.
     */
    public int getPageCount() {
        return pages.length;
    }

    /**
     * Get pages of book.
     *
     * @return Pages.
     */
    public Page[] getPages() {
        return pages;
    }

    /**
     * Get publication type of book.
     *
     * @return Publication type.
     */
    public String getPublicationType() { return publicationType; }

    /**
     * Get published date of book.
     *
     * @return Published date.
     */
    public Date getPublishedAt() { return publishedAt; }

    /**
     * Get a formatted tagline with author, title, and page count.
     *
     * @return Formatted tagline.
     */
    public String getTagline() {
        return String.format("'%s' by %s is %d pages.", this.title, this.author, this.getPageCount());
    }

    /**
     * Get title of book.
     *
     * @return Title.
     */
    public String getTitle() {
        return title;
    }

    /**
     * Publish current book.
     * If book already published, throws IllegalStateException.
     */
    public void publish() throws IllegalStateException {
        Date publishedAt = getPublishedAt();
        if (publishedAt == null) {
            setPublishedAt(new Date());
            System.out.println(String.format("Published '%s' by %s.", getTitle(), getAuthor()));
        } else {
            throw new IllegalStateException(
                    String.format("Cannot publish '%s' by %s (already published on %s).",
                            getTitle(),
                            getAuthor(),
                            publishedAt));
        }
    }

    /**
     * Set author of book.
     *
     * @param author Author name.
     */
    public void setAuthor(String author) {
        this.author = author;
    }

    /**
     * Set page at specified index of book.
     *
     * @param page Page to be added.
     * @param index Index of specified page.
     */
    public void setPage(Page page, int index) {
        if (index < 0 || index >= this.pages.length) {
            throw new IndexOutOfBoundsException(String.format("Unable to add page with content \"%s\" at index %s.", page, index));
        }
        this.pages[index] = page;
    }

    /**
     * Set pages of book.
     *
     * @param pages Pages.
     */
    public void setPages(Page[] pages) {
        this.pages = pages;
    }

    /**
     * Set publication type of book.
     *
     * @param type Publication type.
     */
    public void setPublicationType(String type) { this.publicationType = type; }

    /**
     * Set published date of book.
     *
     * @param publishedAt Page count.
     */
    public void setPublishedAt(Date publishedAt) {
        this.publishedAt = publishedAt;
    }

    /**
     * Set title of book.
     *
     * @param title Title.
     */
    public void setTitle(String title) {
        this.title = title;
    }

    /**
     * Gets a string representation of Book.
     *
     * @return String string of Book.
     */
    public String toString() {
        return String.format("'%s' by %s is %s pgs and published on %s.",
                getTitle(),
                getAuthor(),
                getPageCount(),
                getPublishedAt());
    }

    /**
     * Throw an Exception.
     */
    public void throwException(String message) throws Exception {
        throw new Exception(message);
    }
}