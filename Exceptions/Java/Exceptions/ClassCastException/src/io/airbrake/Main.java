// Main.java
package io.airbrake;

import io.airbrake.utility.Logging;
import org.jetbrains.annotations.Nullable;

import java.util.GregorianCalendar;

public class Main {

    public static void main(String[] args) {
        // Create a new PaperbackBook.
        Book paperbackBook = new PaperbackBook(
                "The Revenant",
                "Michael Punke",
                272,
                new GregorianCalendar(2015, 1, 6).getTime());
        Logging.log(paperbackBook);

        // Create a new DigitalBook.
        Book digitalBook = new DigitalBook(
                "Magician",
                "Raymond E. Feist",
                681,
                new GregorianCalendar(1982, 10, 1).getTime());
        Logging.log(digitalBook);

        // Attempt to cast PaperbackBook to DigitalBook.
        DigitalBook castDigital = castToDigitalBook(paperbackBook);
        Logging.log(castDigital);

        // Attempt to cast PaperbackBook to Book.
        Book castBook = castToBook(paperbackBook);
        Logging.log(castBook);
    }

    @Nullable
    private static DigitalBook castToDigitalBook(Object source) {
        try {
            Logging.lineSeparator(String.format("CASTING %s TO DigitalBook", source.getClass().getSimpleName()), 60);
            return (DigitalBook) source;
        } catch (ClassCastException exception) {
            // Output expected ClassCastExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
        return null;
    }

    @Nullable
    private static Book castToBook(Object source) {
        try {
            Logging.lineSeparator(String.format("CASTING %s TO Book", source.getClass().getSimpleName()), 60);
            //return (Book) source;
            return Book.class.cast(source);
        } catch (ClassCastException exception) {
            // Output expected ClassCastExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
        return null;
    }
}
