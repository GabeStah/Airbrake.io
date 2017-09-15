package io.airbrake

import io.airbrake.utility.Logging

fun main(args: Array<String>) {
    val alice = io.airbrake.java.Person("Alice Smith", 30)
    Logging.log(alice)

    val bob = io.airbrake.kotlin.Person("Bob Smith", 32)
    Logging.log(bob)
}