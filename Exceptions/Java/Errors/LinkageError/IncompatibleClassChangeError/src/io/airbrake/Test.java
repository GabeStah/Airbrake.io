package io.airbrake;

import java.util.GregorianCalendar;

public class Test {
    public static void main(String[] args) {
        try {
            PaperbackBook paperbackBook = new PaperbackBook(
                    "The Stand",
                    "Stephen King",
                    1153,
                    new GregorianCalendar(1978, 0, 1).getTime()
            );
            System.out.println(paperbackBook.getTagline());
        } catch (IncompatibleClassChangeError error) {
            // Output expected IncompatibleClassChangeErrors.
            System.out.println(String.format("[EXPECTED] %s", error.toString()));
            error.printStackTrace();
        } catch (Exception | Error throwable) {
            // Output unexpected Exceptions/Errors.
            System.out.println(String.format("[UNEXPECTED] %s", throwable.toString()));
            throwable.printStackTrace();
        }
    }
}
