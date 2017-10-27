package io.airbrake;

import io.airbrake.utility.Logging;

import java.util.GregorianCalendar;

public class Main {

    public static void main(String[] args) {
        Logging.lineSeparator("CREATE RANDOM NUMBER ARRAY");
        int[] array = createArrayOfSize(10);
        // Output array.
        Logging.log(array);

        Logging.lineSeparator("GET ELEMENT AT INDEX 5");
        Logging.log(getElementByIndex(array, 5));

        Logging.lineSeparator("GET ELEMENT AT INDEX 10");
        Logging.log(getElementByIndex(array, 10));

        Logging.lineSeparator("CREATE BOOK");
        Book book = new Book(
                "A Game of Thrones",
                "George R.R. Martin",
                new GregorianCalendar(1996, 8, 6).getTime(),
                "novel"
        );
        Logging.log(book);

        Logging.lineSeparator("INSERT PAGES");
        // Create Pages array.
        Page[] pages = {
                new Page("“We should start back,” Gared urged as the woods began to grow dark around them. " +
                        "“The wildlings are dead.”"),
                new Page("Until tonight. Something was different tonight. There was an edge to this darkness " +
                        "that made his hackles rise."),
                new Page("“Well, no,” Will admitted")
        };
        book.setPages(pages);
        Logging.log(book);

        Logging.lineSeparator("SET PAGE AT INVALID INDEX");
        setPageAtIndex(book, new Page("Royce nodded."), 3);
    }

    private static void setPageAtIndex(Book book, Page page, int index) {
        try {
            // Set page at index.
            book.setPage(page, index);
            // Output updated book.
            Logging.log(book);
        } catch (IndexOutOfBoundsException error) {
            // Output expected IndexOutOfBoundsExceptions.
            Logging.log(error);
        } catch (Exception | Error exception) {
            // Output unexpected Exceptions/Errors.
            Logging.log(exception, false);
        }
    }

    private static int[] createArrayOfSize(int size) {
        int[] data = new int[size];
        for (int i = 0; i < data.length; i++) {
            data[i] = (int)(Math.random() * 100);
        }
        return data;
    }

    private static Integer getElementByIndex(int[] array, int index) {
        try {
            return array[index];
        } catch (IndexOutOfBoundsException error) {
            // Output expected IndexOutOfBoundsExceptions.
            Logging.log(error);
        } catch (Exception | Error exception) {
            // Output unexpected Exceptions/Errors.
            Logging.log(exception, false);
        }
        return null;
    }
}
