package io.airbrake;

import java.util.GregorianCalendar;

public class Test {

    public static void main(String[] args) {
        System.out.println("CREATING AND OUTPUTTING BOOK");
        createBook();
        System.out.println("CREATING AND OUTPUTTING POEM");
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
            System.out.println(book.toFormattedString());
        } catch (NoSuchMethodError error) {
            // Output expected NoSuchMethodErrors.
            System.out.println(error);
        } catch (Exception | Error exception) {
            // Output unexpected Exceptions/Errors.
            System.out.println(exception);
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
            System.out.println(poem.toFormattedString());
        } catch (NoSuchMethodError error) {
            // Output expected NoSuchMethodErrors.
            System.out.println(error);
        } catch (Exception | Error exception) {
            // Output unexpected Exceptions/Errors.
            System.out.println(exception);
        }

    }
}