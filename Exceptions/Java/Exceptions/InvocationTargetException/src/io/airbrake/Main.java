package io.airbrake;

import io.airbrake.utility.Logging;
import java.lang.reflect.*;

public class Main {

    public static void main(String[] args) {
        invokeGetTaglineMethod();
        invokeThrowExceptionMethod();
    }

    private static void invokeGetTaglineMethod() {
        try {
            // Instantiate a new Book object.
            Book book = new Book("The Pillars of the Earth", "Ken Follett", 973);
            // Output book.
            Logging.log(book);
            // Instantiate a Class object for the Book class.
            Class<?> bookClass = Class.forName("io.airbrake.Book");
            // Get an instance of the getTagline method from Book.
            Method getTagline = bookClass.getDeclaredMethod("getTagline");
            // Output result of invoking book.getTagline() method.
            Logging.log(getTagline.invoke(book));
        } catch (InvocationTargetException |
                ClassNotFoundException |
                NoSuchMethodException |
                IllegalAccessException exception) {
            // Catch expected Exceptions.
            Logging.log(exception);
            // Find underlying causal Exception.
            if (exception.getCause() != null) {
                Logging.log(exception.getCause());
            }
        } catch (Exception exception) {
            // Catch unexpected Exceptions.
            Logging.log(exception, false);
        }
    }

    private static void invokeThrowExceptionMethod() {
        try {
            // Instantiate a new Book object.
            Book book = new Book("The Stand", "Stephen King", 823);
            // Output book.
            Logging.log(book);
            // Instantiate a Class object for the Book class.
            Class<?> bookClass = Class.forName("io.airbrake.Book");
            // Get an instance of the throwException method from Book.
            Method throwException = bookClass.getDeclaredMethod("throwException", String.class);
            // Output result of invoking book.throwException() method.
            Logging.log(throwException.invoke(book, "Uh oh, this is an Exception message!"));
        } catch (InvocationTargetException |
                ClassNotFoundException |
                NoSuchMethodException |
                IllegalAccessException exception) {
            // Catch expected Exceptions.
            Logging.log(exception);
            // Find underlying causal Exception.
            if (exception.getCause() != null) {
                Logging.log(exception.getCause());
            }
        } catch (Exception exception) {
            // Catch unexpected Exceptions.
            Logging.log(exception, false);
        }
    }
}
