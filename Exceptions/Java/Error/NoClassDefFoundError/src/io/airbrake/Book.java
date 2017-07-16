// Book.java
package io.airbrake;

/**
 * Simple example class to store book instances.
 */
public class Book
{
    public String title;
    public String author;

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
        this.title = title;
        this.author = author;
    }
}
