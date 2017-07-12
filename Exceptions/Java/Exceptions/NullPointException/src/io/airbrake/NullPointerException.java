// NullPointerException.java
package io.airbrake;

import io.airbrake.utility.*;

public class NullPointerException {

    private int[] data = new int[100];

    public static void main(String[] args) {
        callInstanceMethodOfNull();
        callInstanceFieldOfNull();
    }

    private static void callInstanceMethodOfNull()
    {
        try
        {
            // Instantiate null object.
            Integer age = null;
            // Attempt to call method on object.
            age.toString();
        }
        catch (java.lang.NullPointerException exception)
        {
            // Catch NullPointerExceptions.
            Logging.log(exception);
        }
        catch (Throwable exception)
        {
            // Catch other Throwables.
            Logging.log(exception, false);
        }
    }

    private static void callInstanceFieldOfNull()
    {
        try
        {
            // Instantiate a new Book.
            Book book = new Book("The Stand", "Stephen King");
            // Set to null.
            book = null;
            // Check if null.
            if (book == null) throw new IllegalArgumentException("Book object is null and cannot be processed.");
            // Call null field.
            Logging.log(String.format("Author is: %s", book.author));
        }
        catch (java.lang.NullPointerException exception)
        {
            // Catch NullPointerExceptions.
            Logging.log(exception);
        }
        catch (Throwable exception)
        {
            // Catch other Throwables.
            Logging.log(exception, false);
        }
    }
}

