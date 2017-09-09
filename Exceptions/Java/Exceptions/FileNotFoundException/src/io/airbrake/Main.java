package io.airbrake;

import com.fasterxml.jackson.databind.ObjectMapper;
import io.airbrake.utility.Logging;

import java.io.File;
import java.io.FileNotFoundException;
import java.io.FileWriter;
import java.util.GregorianCalendar;

public class Main {

    public static void main(String[] args) {
        Logging.lineSeparator("BOOKS TO JSON");
        writeBookJson("books.json");

        Logging.lineSeparator("JSON TO BOOKS");
        readBookJson("books.json");

        Logging.lineSeparator("INVALID JSON FILE TO BOOKS");
        readBookJson("invalid.json");
    }

    /**
     * Create file at path with Book elements converted to Json.
     *
     * @param path File path to create.
     */
    private static void writeBookJson(String path) {
        try {
            // Create FileWriter from path.
            FileWriter writer = new FileWriter(path);

            // Generate JSON from Books.
            writer.write("[");

            writer.write(new Book(
                    "A Game of Thrones",
                    "George R.R. Martin",
                    848,
                    new GregorianCalendar(1996, 8, 6).getTime()
            ).toJsonString());

            writer.write(",");

            writer.write(new Book(
                    "The Name of the Wind",
                    "Patrick Rothfuss",
                    662,
                    new GregorianCalendar(2007, 3, 27).getTime()
            ).toJsonString());

            writer.write("]");

            // Flush and close writer.
            writer.flush();
            writer.close();

            Logging.log(String.format("Books added to file: %s", path));
        } catch (FileNotFoundException exception) {
            // Output expected FileNotFoundExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }

    /**
     * Read Books from Json file.
     *
     * @param path Path of Json file to read.
     */
    private static void readBookJson(String path) {
        try {
            // Create new mapper, read value from new file of
            // passed path, and map to array of Book elements.
            Book[] books = new ObjectMapper().readValue(new File(path), Book[].class);
            // Output each Book in array.
            for (Book book : books) {
                Logging.log(book);
            }
        } catch (FileNotFoundException exception) {
            // Output expected FileNotFoundExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }
}

