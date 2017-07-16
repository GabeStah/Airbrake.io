// Logging.java
package logging;

import java.util.Arrays;

/**
 * Houses all logging methods for various debug outputs.
 */
public class Logging {

    /**
     * Outputs any kind of Object.
     *
     * @param value Object to be output.
     */
    public static void log(Object value)
    {
        System.out.println(value);
    }

    /**
     * Outputs passed in Throwable exception or error instance.
     * Can be overloaded if expected parameter should be specified.
     *
     * @param throwable Throwable instance to output.
     */
    public static void log(Throwable throwable)
    {
        // Invoke call with default expected value.
        log(throwable, true);
    }

    /**
     * Outputs passed in Throwable exception or error instance.
     * Includes Throwable class type, message, stack trace, and expectation status.
     *
     * @param throwable Throwable instance to output.
     * @param expected Determines if this Throwable was expected or not.
     */
    public static void log(Throwable throwable, boolean expected)
    {
        System.out.println(String.format("[%s] %s", expected ? "EXPECTED" : "UNEXPECTED", throwable.toString()));
        throwable.printStackTrace();
    }

    /**
     * Output a dashed line separator of default (40) length.
     */
    public static void lineSeparator()
    {
        // Invoke default length method.
        lineSeparator(40);
    }

    /**
     * Output a dashed lin separator of desired length.
     *
     * @param length Length of line to be output.
     */
    public static void lineSeparator(int length)
    {
        // Create new character array of proper length.
        char[] characters = new char[length];
        // Fill each array element with character.
        Arrays.fill(characters, '-');
        // Output line of characters.
        System.out.println(new String(characters));
    }

}

