<?php

require('/media/sf_Airbrake.io/lib/php/Logging.php');

function ExecuteExamples() {
    DoBitwiseShift();
    Logging::LineSeparator();
    DoInvalidBitwiseShift();

    DoIntegerDivision();
    Logging::LineSeparator();
    DoInvalidIntegerDivision();
}

function DoBitwiseShift() {
    try {
        $a = 1;
        $b = 3;
        // Perform bitwise shift operation.
        $result = $a << $b;
        Logging::Log("Bitwise shift result: $result");
    } catch (ArithmeticError $error) {
        // Output expected ArithmeticError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }
}

function DoInvalidBitwiseShift() {
    try {
        $a = 1;
        $b = -3;
        // Perform bitwise shift operation.
        $result = $a << $b;
        Logging::Log("Bitwise shift result: $result");
    } catch (ArithmeticError $error) {
        // Output expected ArithmeticError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }
}

function DoIntegerDivision() {
    try {
        $a = 2147483647;
        $b = 3;
        // Perform integer division.
        $result = intdiv($a, $b);
        Logging::Log("Integer division result: $result");
    } catch (ArithmeticError $error) {
        // Output expected ArithmeticError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }
}

function DoInvalidIntegerDivision() {
    try {
        $a = PHP_INT_MIN;
        $b = -1;
        // Perform integer division.
        $result = intdiv($a, $b);
        Logging::Log("Integer division result: $result");
    } catch (ArithmeticError $error) {
        // Output expected ArithmeticError.
        Logging::Log($error);
    } catch (Error $error) {
        // Output any unexpected errors.
        Logging::Log($error, false);
    }
}

ExecuteExamples();