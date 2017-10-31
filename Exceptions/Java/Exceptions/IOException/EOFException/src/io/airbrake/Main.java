package io.airbrake;

import io.airbrake.utility.Logging;

import java.io.*;
import java.util.Arrays;
import java.util.GregorianCalendar;
import java.util.List;

public class Main {

    private static final String FILE = "books.txt";

    private static final List<Book> DATA = Arrays.asList(
            new Book("The Name of the Wind",
                    "Patrick Rothfuss",
                    662,
                    new GregorianCalendar(2007, 2, 27).getTime()),
            new Book("The Wise Man's Fear",
                    "Patrick Rothfuss",
                    994,
                    new GregorianCalendar(2011, 2, 1).getTime()),
            new Book("Doors of Stone",
                    "Patrick Rothfuss",
                    896,
                    new GregorianCalendar(2049, 2, 5).getTime())
    );

    public static void main(String[] args) {
        WriteBooksToFile();

        ReadBooksFromFileImproperly();

        ReadBooksFromFile();
    }

    private static void ReadBooksFromFileImproperly() {
        try {
            DataInputStream inputStream = new DataInputStream(new BufferedInputStream(new FileInputStream(FILE)));

            Logging.lineSeparator(String.format("READING FROM FILE: %s", FILE));
            while (true) {
                String description = inputStream.readUTF();
                Logging.log(description);
            }
        } catch (EOFException exception) {
            // Output expected EOFExceptions.
            Logging.log(exception);
        } catch (IOException exception) {
            // Output unexpected IOExceptions.
            Logging.log(exception, false);
        }
    }

    private static void ReadBooksFromFile() {
        DataInputStream inputStream = null;
        try {
            inputStream = new DataInputStream(new BufferedInputStream(new FileInputStream(FILE)));

            Logging.lineSeparator(String.format("READING FROM FILE: %s", FILE));

            while (true) {
                // Use inner exception block to determine end of file.
                try {
                    String description = inputStream.readUTF();
                    Logging.log(description);
                } catch (EOFException exception) {
                    // Break while loop when file ends.
                    break;
                } catch (IOException exception) {
                    // Output unexpected IOExceptions.
                    Logging.log(exception, false);
                }
            }
        } catch (IOException exception) {
            // Output unexpected IOExceptions.
            Logging.log(exception, false);
        } finally {
            try {
                if (inputStream != null) {
                    inputStream.close();
                }
            } catch (IOException exception) {
                // Output unexpected IOExceptions.
                Logging.log(exception, false);
            }
        }
    }

    private static void WriteBooksToFile() {
        try {
            DataOutputStream outputStream = new DataOutputStream(new BufferedOutputStream(new FileOutputStream(FILE)));

            Logging.lineSeparator(String.format("WRITING TO FILE: %s", FILE));
            for (Book book : DATA) {
                outputStream.writeUTF(book.toString());
                Logging.log(book);
            }

            outputStream.close();
        } catch (EOFException exception) {
            // Output expected EOFExceptions.
            Logging.log(exception);
        } catch (IOException exception) {
            // Output unexpected IOExceptions.
            Logging.log(exception, false);
        }
    }
}
