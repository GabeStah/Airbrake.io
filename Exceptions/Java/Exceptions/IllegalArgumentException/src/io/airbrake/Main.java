package io.airbrake;

import io.airbrake.utility.*;

import java.util.ArrayList;
import java.util.List;

public class Main {

    public static void main(String[] args) {
        Logging.lineSeparator("VALID EXAMPLE");

        generateLibrary();

        Logging.lineSeparator("INVALID EXAMPLE");

        generateLibraryInvalid();
    }

    /**
     * Create a library Book collection, including an exceedingly length book.
     */
    private static void generateLibraryInvalid() {
        try {
            // Create list of Books.
            List<Book> library = new ArrayList<>();

            // Add a few new Books to list.
            library.add(new Book("The Pillars of the Earth", "Ken Follett", 973));
            library.add(new Book("A Game of Thrones", "George R.R. Martin", 835));
            // Output library size.
            Logging.log(String.format("Library contains %d books.", library.size()));

            // Add another, very lengthy book.
            library.add(new Book("In Search of Lost Time", "Marcel Proust", 4215));

            // Output latest Book addition and updated library size.
            Logging.log(library.get(library.size() - 1));
            Logging.log(String.format("Library contains %d books.", library.size()));
        } catch (IllegalArgumentException exception) {
            // Catch expected IllegalArgumentExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Catch unexpected Exceptions.
            Logging.log(exception, false);
        }
    }

    /**
     * Create a library Book collection.
     */
    private static void generateLibrary() {
        try {
            // Create list of Books.
            List<Book> library = new ArrayList<>();

            // Add a few new Books to list.
            library.add(new Book("His Dark Materials", "Philip Pullman", 399));
            library.add(new Book("Life of Pi", "Yann Martel", 460));
            // Output library size.
            Logging.log(String.format("Library contains %d books.", library.size()));

            // Add another book.
            library.add(new Book("Les Misérables", "Victor Hugo", 1463));

            // Output latest Book addition and updated library size.
            Logging.log(library.get(library.size() - 1));
            Logging.log(String.format("Library contains %d books.", library.size()));
        } catch (IllegalArgumentException exception) {
            // Catch expected IllegalArgumentExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Catch unexpected Exceptions.
            Logging.log(exception, false);
        }
    }
}
