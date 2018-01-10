package io.airbrake;

import io.airbrake.utility.Logging;

import java.util.ArrayList;
import java.util.GregorianCalendar;
import java.util.Iterator;
import java.util.NoSuchElementException;

public class Main {

    public static void main(String[] args) {
        try {
            Logging.lineSeparator("CREATING BOOKS");
            ArrayList<Book> books = new ArrayList<>();
            books.add(
                new Book(
                    "The Stand",
                    "Stephen King",
                    1153,
                    new GregorianCalendar(1978, 8, 1).getTime()
                )
            );

            books.add(
                new Book(
                    "It",
                    "Stephen King",
                    1116,
                    new GregorianCalendar(1987, 9, 1).getTime()
                )
            );

            books.add(
                new Book(
                    "The Gunslinger",
                    "Stephen King",
                    231,
                    new GregorianCalendar(1982, 5, 10).getTime()
                )
            );

            Logging.lineSeparator("FOREACH LOOP TEST");
            forEachLoopTest(books);

            Logging.lineSeparator("ITERATOR TEST");
            Iterator iterator = iteratorTest(books);

            Logging.log(iterator.next());
        } catch (NoSuchElementException exception) {
            // Output expected NoSuchElementExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }

    /**
     * Loops through passed ArrayList using built-in forEach() method.  Outputs elements to log.
     *
     * @param list List to be looped through.
     */
    private static void forEachLoopTest(ArrayList<Book> list) {
        try {
            // Output list via forEach method and Logging::log method reference.
            list.forEach(Logging::log);
        } catch (NoSuchElementException exception) {
            // Output expected NoSuchElementExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }

    /**
     * Loops through passed ArrayList using Iterator of list and converting each element to Book before output.
     *
     * @param list List to be iterated through.
     * @return Iterator obtained from ArrayList.
     */
    private static Iterator iteratorTest(ArrayList<Book> list) {
        try {
            // Create iterator from list.
            Iterator iterator = list.iterator();
            // While next element exists, iteratorTest.
            while (iterator.hasNext())
            {
                // Get next element and output.
                Book book = (Book) iterator.next();
                Logging.log(book);
            }
            // Return iterator.
            return iterator;
        } catch (NoSuchElementException exception) {
            // Output expected NoSuchElementExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
        return null;
    }
}
