package io.airbrake

import io.airbrake.utility.Logging
import java.util.*

fun main(args: Array<String>) {
    val book = Book(
            "The Music of the Primes: Searching to Solve the Greatest Mystery in Mathematics",
            "Marcus du Sautoy",
            335,
            GregorianCalendar(2014, 10, 14).time
    )

    Logging.log(book)
}