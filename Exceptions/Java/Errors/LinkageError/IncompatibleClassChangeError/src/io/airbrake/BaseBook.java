package io.airbrake;

import java.util.Date;

public class BaseBook {
    private String author;
    private Integer pageCount;
    private Date publishedAt;
    private static String title;

    /**
     * Constructs a basic book.
     *
     * @param title Book title.
     * @param author Book author.
     */
    public BaseBook(String title, String author) {
        this.author = author;
        this.title = title;
    }

    /**
     * Constructs a basic book, with page count.
     *
     * @param title Book title.
     * @param author Book author.
     * @param pageCount Book page count.
     */
    public BaseBook(String title, String author, Integer pageCount) {
        this.author = author;
        this.title = title;
        this.pageCount = pageCount;
    }

    /**
     * Constructs a basic book, with page count, publication date, and publication type.
     *
     * @param title Book title.
     * @param author Book author.
     * @param pageCount Book page count.
     * @param publishedAt Book publication date.
     */
    public BaseBook(String title, String author, Integer pageCount, Date publishedAt) {
        this.author = author;
        this.title = title;
        this.pageCount = pageCount;
        this.publishedAt = publishedAt;
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
     * Get page count of book.
     *
     * @return Page count.
     */
    public Integer getPageCount() {
        return pageCount;
    }

    /**
     * Get published date of book.
     *
     * @return Published date.
     */
    public Date getPublishedAt() {
        return publishedAt;
    }

    /**
     * Get title of book.
     *
     * @return Title.
     */
    public static String getTitle() {
        return title;
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
     * Set page count of book.
     *
     * @param pageCount Page count.
     */
    public void setPageCount(Integer pageCount) {
        this.pageCount = pageCount;
    }

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
}
