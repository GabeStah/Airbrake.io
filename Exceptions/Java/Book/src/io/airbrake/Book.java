// Book.java
package io.airbrake;

/**
 * Simple example class to store book instances.
 */
public class Book
{
    public String title;
    public String author;
    public Integer pageCount;

    /**
     * Constructs an empty book.
     */
    public Book() { }

    /**
     * Constructs a basic book.
     *
     * @param title Book title.
     * @param author Book author.
     * @param pageCount Book page count.
     */
    public Book(String title, String author, Integer pageCount) {
        this.title = title;
        this.author = author;
        this.pageCount = pageCount;
    }
}
