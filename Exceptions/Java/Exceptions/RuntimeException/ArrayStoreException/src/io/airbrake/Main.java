package io.airbrake;

import io.airbrake.utility.Logging;
import java.util.GregorianCalendar;

public class Main {

    public static void main(String[] args) {
        Logging.lineSeparator("String INTO String[]");
        insertIntoArray("Hello world", new String[1]);

        Logging.lineSeparator("Integer INTO String[]");
        insertIntoArray(24601, new String[1]);

        Logging.lineSeparator("Book INTO String[]");
        insertIntoArray(
            new Book(
            "The Stand",
                "Stephen King",
                1153,
                new GregorianCalendar(1978, 8, 1).getTime()
            ),
            new String[1]
        );

        Logging.lineSeparator("String INTO Object[]");
        insertIntoArray("Hello world", new Object[1]);

        Logging.lineSeparator("Integer INTO Object[]");
        insertIntoArray(24601, new Object[1]);

        Logging.lineSeparator("Book INTO Object[]");
        insertIntoArray(
            new Book(
                "It",
                "Stephen King",
                1116,
                new GregorianCalendar(1987, 9, 1).getTime()
            ),
            new Object[1]
        );
    }

    /**
     * Insert passed Object into passed Object[] array at first index.
     *  @param object Object to insert.
     * @param array Recipient array.
     */
    private static void insertIntoArray(Object object, Object[] array) {
        try {
            // Invoke default override.
            insertIntoArray(object, array, 0);
        } catch (ArrayStoreException exception) {
            // Output unexpected ArrayStoreExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }

    /**
     * Insert passed Object into passed Object[] array at specified index.
     *
     * @param object Object to insert.
     * @param array Recipient array.
     * @param index Index at which to insert.
     */
    private static void insertIntoArray(Object object, Object[] array, int index) {
        try {
            // Attempt to insert object at passed index.
            array[index] = object;
            // Output new array.
            Logging.log(array);
        } catch (ArrayStoreException exception) {
            // Output unexpected ArrayStoreExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }
}
