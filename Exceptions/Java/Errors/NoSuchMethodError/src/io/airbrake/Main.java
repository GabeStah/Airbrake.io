package io.airbrake;

import io.airbrake.utility.Logging;

import java.util.GregorianCalendar;

public class Main {

    public static void main(String[] args) {
        Logging.lineSeparator("CREATING AND OUTPUTTING BOOK");
        createBook();
        Logging.lineSeparator("CREATING AND OUTPUTTING POEM");
        createPoem();
    }

    public static void createBook() {
        try {
            Book book = new Book(
                    "A Game of Thrones",
                    "George R.R. Martin",
                    848,
                    new GregorianCalendar(1996, 8, 6).getTime(),
                    "novel"
            );
            Logging.log(book.toFormattedString());
        } catch (NoSuchMethodError error) {
            // Output expected NoSuchMethodErrors.
            Logging.log(error);
        } catch (Exception | Error exception) {
            // Output unexpected Exceptions/Errors.
            Logging.log(exception, false);
        }
    }

    public static void createPoem() {
        try {
            Book poem = new Book(
                    "The Raven",
                    "Edgar Allan Poe",
                    2,
                    new GregorianCalendar(1845, 0, 29).getTime(),
                    "poem"
            );
            Logging.log(poem.toFormattedString());
        } catch (NoSuchMethodError error) {
            // Output expected NoSuchMethodErrors.
            Logging.log(error);
        } catch (Exception | Error exception) {
            // Output unexpected Exceptions/Errors.
            Logging.log(exception, false);
        }
    }
}