package io.airbrake;

import io.airbrake.utility.Logging;

import java.util.GregorianCalendar;

public class Main {

    public static void main(String[] args) {
        Logging.lineSeparator("TEST A");
        createBookTestA();
        Logging.lineSeparator("TEST B");
        createBookTestB();
    }

    private static void createBookTestA() {
        try {
            AbstractBook book = new AbstractBook(
                    "A Game of Thrones",
                    "George R.R. Martin",
                    848,
                    new GregorianCalendar(1996, 8, 6).getTime()
            );
            Logging.log(book.getTagline());
        } catch (AbstractMethodError error) {
            // Output expected AbstractMethodErrors.
            Logging.log(error);
        } catch (Exception | Error exception) {
            // Output unexpected Exceptions/Errors.
            Logging.log(exception, false);
        }
    }

    private static void createBookTestB() {
        try {
            AbstractBook book = new AbstractBook(
                    "A Clash of Kings",
                    "George R.R. Martin",
                    761,
                    new GregorianCalendar(1998, 10, 16).getTime()
            );
            Logging.log(book.getTagline());
        } catch (AbstractMethodError error) {
            // Output expected AbstractMethodErrors.
            Logging.log(error);
        } catch (Exception | Error exception) {
            // Output unexpected Exceptions/Errors.
            Logging.log(exception, false);
        }
    }
}

