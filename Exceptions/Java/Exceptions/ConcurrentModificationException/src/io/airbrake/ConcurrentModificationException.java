package io.airbrake;

import java.util.*;
import io.airbrake.utility.*;

public class ConcurrentModificationException {

    public static void main(String args[]){
        modifiedListExample();
        modifiedIteratorExample();
    }

    /**
     * Perform looped iteration of List while it is being modified.
     */
    private static void modifiedListExample()
    {
        try {
            // Create list of Books.
            List<Book> library = new ArrayList<>();
            // Add a few new Books to list.
            library.add(new Book("The Pillars of the Earth", "Ken Follett", 973));
            library.add(new Book("A Game of Thrones", "George R.R. Martin", 835));
            library.add(new Book("Gone Girl", "Gillian Flynn", 555));
            library.add(new Book("His Dark Materials", "Philip Pullman", 399));
            library.add(new Book("Life of Pi", "Yann Martel", 460));

            // Loop through each book object in list.
            for (Book book : library) {
                // Output next book.
                Logging.log(book);
                // If current book title is "Gone Girl", remove that book from list.
                if (book.getTitle().equals("Gone Girl")) {
                    library.remove(book);
                }
            }
        } catch (java.util.ConcurrentModificationException exception) {
            // Catch ConcurrentModificationExceptions.
            Logging.log(exception);
        } catch (Throwable throwable) {
            // Catch any other Throwables.
            Logging.log(throwable);
        }
    }

    /**
     * Perform loop through Iterator while it is being modified.
     */
    private static void modifiedIteratorExample()
    {
        try {
            // Create list of Books.
            List<Book> library = new ArrayList<>();
            // Add a few new Books to list.
            library.add(new Book("The Pillars of the Earth", "Ken Follett", 973));
            library.add(new Book("A Game of Thrones", "George R.R. Martin", 835));
            library.add(new Book("Gone Girl", "Gillian Flynn", 555));
            library.add(new Book("His Dark Materials", "Philip Pullman", 399));
            library.add(new Book("Life of Pi", "Yann Martel", 460));


            // Create iterator to loop through each book in list.
            for(Iterator<Book> bookIterator = library.iterator(); bookIterator.hasNext();){
                // Get next book.
                Book book = bookIterator.next();
                // If current book title is "Gone Girl", remove that book from list.
                if (book.getTitle().equals("Gone Girl")) {
                    bookIterator.remove();
                } else {
                    // Output current book.
                    Logging.log(book);
                }
            }

            //library.removeIf(book -> book.getTitle().equals("Gone Girl"));
        } catch (java.util.ConcurrentModificationException exception) {
            // Catch ConcurrentModificationExceptions.
            Logging.log(exception);
        } catch (Throwable throwable) {
            // Catch any other Throwables.
            Logging.log(throwable);
        }
    }
}