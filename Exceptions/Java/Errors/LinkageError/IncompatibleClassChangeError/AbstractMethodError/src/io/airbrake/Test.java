package io.airbrake;

import java.util.GregorianCalendar;

public class Test {

    public static void main(String[] args) {
        System.out.print("TEST A");
        createBookTestA();
    }

    private static void createBookTestA() {
        try {
            AbstractBook book = new AbstractBook(
                    "A Game of Thrones",
                    "George R.R. Martin",
                    848,
                    new GregorianCalendar(1996, 8, 6).getTime()
            );
            System.out.print(book.getTagline());
        } catch (AbstractMethodError error) {
            // Output expected AbstractMethodErrors.
            System.out.print(error);
        } catch (Exception | Error exception) {
            // Output unexpected Exceptions/Errors.
            System.out.print(exception);
        }
    }
}

