<?php

include("D:\work\Airbrake.io\lib\php\Logging.php");

function executeExamples()
{
    Logging::LineSeparator("FIND MAXIMUM SCALE ACCURACY");
    Logging::Log(getScaleAccuracy(true));

    Logging::LineSeparator("NO SCALE");
    addNumbers(123, 24478);

    Logging::LineSeparator("SCALE: 1");
    addNumbers(123.4, 24477.6);

    Logging::LineSeparator("SCALE: 2");
    addNumbers(123.45, 24477.55);

    Logging::LineSeparator("SCALE: 3");
    addNumbers(123.456, 24477.544);

    Logging::LineSeparator("SCALE: 4");
    addNumbers(123.4567, 24477.5433);
}

/**
 * Adds two numbers together.
 *
 * @param int|float|string $a First number to add.
 * @param int|float|string $b Second number to add.
 * @return mixed Result of summing $a and $b.
 */
function addNumbers($a, $b) {
    try {
        $maximumScale = getScaleAccuracy();
        if (getScale($a) > $maximumScale) {
            throw new UnderflowException("Scale of $a exceeds maximum accurate scale of $maximumScale.");
        } elseif (getScale($b) > $maximumScale) {
            throw new UnderflowException("Scale of $b exceeds maximum accurate scale of $maximumScale.");
        }
        $sum = $a + $b;
        Logging::Log("$a + $b == $sum");
        return $sum;
    } catch (UnderflowException $exception) {
        // Output expected UnderflowException.
        Logging::Log($exception);
    } catch (Exception $exception) {
        // Output unexpected Exceptions.
        Logging::Log($exception, false);
    }
    return null;
}

/**
 * Gets the scale of the passed float/decimal.
 *
 * @param int|float|string $number The number to retrieve scale of.
 * @return int Scale of passed $number.
 */
function getScale($number) {
    return strlen(strstr($number, '.')) - 1;
}

/**
 * Gets the maximum scale (number of places after decimal)
 * in which current PHP engine is accurate with floating points.
 *
 * @param bool $debugOutput Determine if debug output should be included.
 * @return int Maximum scale of accuracy.
 */
function getScaleAccuracy($debugOutput = false) {
    $scale = 1;
    while (true) {
        // Check if scale is accurate.
        if (!isScaleAccurate($scale, $debugOutput)) {
            return $scale - 1;
        }
        $scale++;
    }
}

/**
 * Determine if passed $scale value is accurate.
 *
 * @param int $scale Scale value to check.
 * @param bool $debugOutput Determine if debug output should be included.
 * @return bool Indicates if passed $scale is accurate.
 */
function isScaleAccurate($scale, $debugOutput = false) {
    // Create float (0.999...n) to n scale places.
    $string = '0.' . str_repeat('9', $scale);
    // Convert to float.
    $float = (float) $string;
    // Get result.
    $result = (1 - $float);
    // Determine if result is in the form of 0.00...1,
    // which indicates an accurate decimal value.
    if (substr($result, 0, 1) == "0") {
        if ($debugOutput) Logging::Log("Float scale to ($scale) places is accurate.");
        if ($debugOutput) Logging::Log("1 - $float == $result");
        return true;
    } else {
        // If converted to floating point, the form
        // is 9.99...8, which indicates an inaccuracy.
        if ($debugOutput) Logging::Log("Float scale of ($scale) places is inaccurate.");
        if ($debugOutput) Logging::Log("1 - $float == $result");
        return false;
    }
}

executeExamples();