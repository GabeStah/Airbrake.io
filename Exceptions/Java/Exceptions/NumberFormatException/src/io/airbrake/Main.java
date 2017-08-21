package io.airbrake;

import io.airbrake.utility.Logging;

public class Main {

    public static void main(String[] args) {
        Logging.lineSeparator("STRING TO BYTE");
        Logging.log(convertStringToByte("20"));
        Logging.log(convertStringToByte("200"));

        Logging.lineSeparator("STRING TO DOUBLE");
        Logging.log(convertStringToDouble("3.14e7"));
        Logging.log(convertStringToDouble(Double.toString(Double.MAX_VALUE)));
        Logging.log(convertStringToDouble("3.14x"));

        Logging.lineSeparator("STRING TO FLOAT");
        Logging.log(convertStringToFloat("3.14e7"));
        Logging.log(convertStringToFloat("3.14e39"));
        Logging.log(convertStringToFloat("3.14x39"));

        Logging.lineSeparator("STRING TO INTEGER");
        Logging.log(convertStringToInteger("10"));
        Logging.log(convertStringToInteger("10x"));

        Logging.lineSeparator("STRING TO LONG");
        Logging.log(convertStringToLong("20"));
        // 2^63 - 1
        Logging.log(convertStringToLong("9223372036854775807"));
        // 2^63
        Logging.log(convertStringToLong("9223372036854775808"));

        Logging.lineSeparator("STRING TO SHORT");
        Logging.log(convertStringToShort("20"));
        // 2^15 - 1
        Logging.log(convertStringToShort("32767"));
        // 2^15
        Logging.log(convertStringToShort("32768"));
    }

    /**
     * Convert String to Byte.
     *
     * @param string String to be converted.
     * @return Converted Byte.
     */
    private static Byte convertStringToByte(String string) {
        try {
            return Byte.parseByte(string);
        } catch (NumberFormatException exception) {
            // Output expected NumberFormatException.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
        return null;
    }

    /**
     * Convert String to Double.
     *
     * @param string String to be converted.
     * @return Converted Double.
     */
    private static Double convertStringToDouble(String string) {
        try {
            return Double.parseDouble(string);
        } catch (NumberFormatException exception) {
            // Output expected NumberFormatException.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
        return null;
    }

    /**
     * Convert String to Float.
     *
     * @param string String to be converted.
     * @return Converted Float.
     */
    private static Float convertStringToFloat(String string) {
        try {
            return Float.parseFloat(string);
        } catch (NumberFormatException exception) {
            // Output expected NumberFormatException.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
        return null;
    }

    /**
     * Convert String to Integer.
     *
     * @param string String to be converted.
     * @return Converted Integer.
     */
    private static Integer convertStringToInteger(String string) {
        try {
            return Integer.parseInt(string);
        } catch (NumberFormatException exception) {
            // Output expected NumberFormatException.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
        return null;
    }

    /**
     * Convert String to Long.
     *
     * @param string String to be converted.
     * @return Converted Long.
     */
    private static Long convertStringToLong(String string) {
        try {
            return Long.parseLong(string);
        } catch (NumberFormatException exception) {
            // Output expected NumberFormatException.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
        return null;
    }

    /**
     * Convert String to Short.
     *
     * @param string String to be converted.
     * @return Converted Short.
     */
    private static Short convertStringToShort(String string) {
        try {
            return Short.parseShort(string);
        } catch (NumberFormatException exception) {
            // Output expected NumberFormatException.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
        return null;
    }
}
