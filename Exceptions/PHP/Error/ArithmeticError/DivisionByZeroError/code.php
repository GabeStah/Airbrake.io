<?php

require('/media/sf_Airbrake.io/lib/php/Logging.php');

error_reporting(E_ERROR | E_WARNING);

function executeExamples() {
    // Divide by 2.
    divide(10, 2);
    Logging::LineSeparator();

    // Divide by zero.
    divide(10, 0);
    Logging::LineSeparator();

    // Modulo by 3.
    modulo(10, 3);
    Logging::LineSeparator();

    // Modulo by zero.
    modulo(10, 0);

    // Divide by 2.
    performIntDiv(15, 2);
    Logging::LineSeparator();

    // Divide by zero.
    performIntDiv(15, 0); 
}

function divide($dividend, $divisor) {
    try {
        // Perform the operation.
        $result = $dividend / $divisor;
        Logging::Log("Division result of ($dividend / $divisor): $result");
    } catch (DivisionByZeroError $error) {
        // Output expected ArithmeticError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }
}

function modulo($dividend, $divisor) {
    try {
        // Perform the operation.
        $result = $dividend % $divisor;
        Logging::Log("Modulo result of ($dividend % $divisor): $result");
    } catch (DivisionByZeroError $error) {
        // Output expected ArithmeticError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }
}

function performIntDiv($dividend, $divisor)
{
    try {
        // Perform the operation.
        $result = intdiv($dividend, $divisor);
        Logging::Log("Modulo result of ($dividend % $divisor): $result");
    } catch (DivisionByZeroError $error) {
        // Output expected ArithmeticError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }    
}

executeExamples();