package io.airbrake;

import io.airbrake.utility.Logging;

public class Main {

    public static void main(String[] args) {
        try {
            // Create a new io.airbrake.Book instance.
            Book book = new Book("The Stand", "Stephen King");
            Logging.log(String.format("Created Book: '%s' written by %s", book.title, book.author));
        } catch (NoClassDefFoundError error) {
            Logging.log(error);
        } catch (Error error) {
            Logging.log(error);
        }
    }
}
