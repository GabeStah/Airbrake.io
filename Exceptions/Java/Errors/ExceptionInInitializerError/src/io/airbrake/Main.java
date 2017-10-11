package io.airbrake;

import io.airbrake.utility.Logging;

import java.util.GregorianCalendar;

public class Main {
    public static void main(String[] args) {
        try {
            Book book = new Book(
                "A Game of Thrones",
                "George R.R. Martin",
                848,
                new GregorianCalendar(1996, 8, 6).getTime(),
                "novel"
            );
            Logging.log(book);
            Logging.log(String.format("Publication Type of Novel: %s", book.getPublicationType()));

            Book poem = new Book(
                    "The Road Not Taken",
                    "Robert Frost",
                    1,
                    new GregorianCalendar(1916, 1, 1).getTime(),
                    "poem"
            );
            Logging.log(poem);
            Logging.log(String.format("Publication Type of Novel: %s", book.getPublicationType()));
            Logging.log(String.format("Publication Type of Poem: %s", poem.getPublicationType()));
        } catch (ExceptionInInitializerError error) {
            // Output expected ExceptionInInitializerErrors.
            Logging.log(error);
            // Output causal exception.
            Logging.lineSeparator(String.format("%s Cause", error.getClass().getSimpleName()), 50);
            Logging.log(error.getCause(), false);
        } catch (Exception | Error exception) {
            // Output unexpected Exceptions/Errors.
            Logging.log(exception, false);
        }
    }
}

