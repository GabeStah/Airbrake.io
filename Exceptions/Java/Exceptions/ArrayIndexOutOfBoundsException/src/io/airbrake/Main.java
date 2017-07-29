package io.airbrake;

import io.airbrake.utility.*;

public class Main {

    public static void main(String[] args) {
        // Create list of Books.
        Book[] library = {
            new Book("The Pillars of the Earth", "Ken Follett", 973),
            new Book("Gone Girl", "Gillian Flynn", 555),
            new Book("His Dark Materials", "Philip Pullman", 399),
            new Book("Life of Pi", "Yann Martel", 460)
        };
        // Iterate over array.
        iterateArray(library);
        Logging.lineSeparator();
        iterateArrayInvalid(library);
    }

    /**
     * Iterate over passed Array, logging each element.
     *
     * @param list
     */
    private static void iterateArray(Book[] list) {
        try {
            // Loop through each element by index.
            for (int index = 0; index < list.length; index++) {
                // Output element.
                Logging.log(list[index]);
            }
        } catch (ArrayIndexOutOfBoundsException exception) {
            // Output expected ArrayIndexOutOfBoundsException.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }

    /**
     * Iterate over passed Array, logging each element.
     *
     * For-loop limit includes list.size(), which is invalid.
     *
     * @param list
     */
    private static void iterateArrayInvalid(Book[] list) {
        try {
            // Loop through each element by index.
            // Less-than or equal to (<=) limit results in an exception.
            for (int index = 0; index <= list.length; index++) {
                // Output element.
                Logging.log(list[index]);
            }
        } catch (ArrayIndexOutOfBoundsException exception) {
            // Output expected ArrayIndexOutOfBoundsException.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }
}
