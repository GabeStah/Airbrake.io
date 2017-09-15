package io.airbrake;

import io.airbrake.utility.Logging;
import java.util.*;

class Main {

    static void main(String[] args) {
        Book book = new Book(
                "The Music of the Primes: Searching to Solve the Greatest Mystery in Mathematics",
                "Marcus du Sautoy",
                335,
                new GregorianCalendar(2014, 10, 14).getTime()
        );

        Logging.log(book);
    }
}
