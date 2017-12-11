package io.airbrake;

import io.airbrake.utility.Logging;

import java.util.GregorianCalendar;

public class Main {

    public static void main(String[] args) {
        testStringPublicationType();

        //testEnumPublicationType();
    }

    private static void testEnumPublicationType() {
        try {
            // Create book with PublicationType: PublicationType.PAPERBACK
            Book book = new Book(
                    "The Name of the Wind",
                    "Patrick Rothfuss",
                    662,
                    new GregorianCalendar(2007, 2, 27).getTime(),
                    PublicationType.PAPERBACK);
            // Output Book.
            Logging.log(book);
            // Change to invalid publication type.
            Logging.lineSeparator("CHANGE PUBLICATION TYPE TO 'PublicationType.INVALID'", 60);
            book.setPublicationType(PublicationType.INVALID);
            // Output modified Book.
            Logging.log(book);
        } catch (AssertionError error) {
            // Output expected AssertionErrors.
            Logging.log(error);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }

    private static void testStringPublicationType() {
        try {
            // Create book with PublicationType: 'PAPERBACK'
            BookWithStringPublicationType bookWithStringPublicationType = new BookWithStringPublicationType(
                    "The Wise Man's Fear",
                    "Patrick Rothfuss",
                    994,
                    new GregorianCalendar(2011, 2, 1).getTime(),
                    "PAPERBACK");
            // Output Book.
            Logging.log(bookWithStringPublicationType);
            // Change to invalid publication type.
            Logging.lineSeparator("CHANGE PUBLICATION TYPE TO 'INVALID' String", 60);
            bookWithStringPublicationType.setPublicationType("INVALID");
            // Output modified Book.
            Logging.log(bookWithStringPublicationType);
        } catch (AssertionError error) {
            // Output expected AssertionErrors.
            Logging.log(error);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }
}
