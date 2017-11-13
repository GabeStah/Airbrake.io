package io.airbrake;

import io.airbrake.utility.Logging;

import java.util.AbstractList;
import java.util.GregorianCalendar;

public class Main {

    public static void main(String[] args) {
        DefaultAbstractList<Book> defaultAbstractList = new DefaultAbstractList<>();

        // Add book to default list.
        addBookToList(
            new Book(
                "A Game of Thrones",
                "George R.R. Martin",
                848,
                new GregorianCalendar(1996, 8, 6).getTime()
            ),
            defaultAbstractList
        );

        MutableAbstractList<Book> mutableAbstractList = new MutableAbstractList<>();

        // Add book to extended list.
        addBookToList(
            new Book(
                "A Clash of Kings",
                "George R.R. Martin",
                761,
                new GregorianCalendar(1998, 9, 16).getTime()
            ),
            mutableAbstractList
        );

        ImmutableList<Book> immutableList = new ImmutableList<>();

        // Add book to immutable list.
        addBookToList(
            new Book(
                "A Storm of Swords",
                "George R.R. Martin",
                1177,
                new GregorianCalendar(2000, 7, 8).getTime()
            ),
            immutableList
        );
    }

    private static void addBookToList(Book book, AbstractList<Book> list) {
        try {
            Logging.lineSeparator(
                String.format("ADDING '%s' TO %s",
                    book.getTitle(),
                    list.getClass().getSimpleName()
                ), 60);
            Logging.log(book);

            // Attempt to add book to passed list.
            list.add(0, book);

            // Output modified list data and parent type.
            Logging.lineSeparator(String.format("MODIFIED %s", list.getClass().getSimpleName()));
            Logging.log(list.toString());
        } catch (UnsupportedOperationException exception) {
            // Output expected UnsupportedOperationExceptions.
            Logging.log(exception);
        } catch (Exception | Error exception) {
            // Output unexpected Exceptions/Errors.
            Logging.log(exception, false);
        }
    }

    private static void addBookToList(Book book, ImmutableList<Book> list) {
        try {
            Logging.lineSeparator(
                String.format("ADDING '%s' TO %s",
                    book.getTitle(),
                    list.getClass().getSimpleName()
                ), 60);
            Logging.log(book);

            // Attempt to add book to passed list.
            list.add(book);

            // Output modified list data and parent type.
            Logging.lineSeparator(String.format("MODIFIED %s", list.getClass().getSimpleName()));
            Logging.log(list.toString());
        } catch (UnsupportedOperationException exception) {
            // Output expected UnsupportedOperationExceptions.
            Logging.log(exception);
        } catch (Exception | Error exception) {
            // Output unexpected Exceptions/Errors.
            Logging.log(exception, false);
        }
    }
}
