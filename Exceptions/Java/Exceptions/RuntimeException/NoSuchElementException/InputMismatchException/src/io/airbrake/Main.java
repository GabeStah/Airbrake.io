package io.airbrake;

import io.airbrake.utility.Logging;

import java.io.File;
import java.util.InputMismatchException;
import java.util.List;
import java.util.Scanner;
import java.util.regex.MatchResult;
import java.util.stream.Collectors;
import java.util.stream.Stream;

public class Main {

    private static final String FILE_PATH = "2014_world_gdp_with_codes.csv";
    // Pads trio of values by 35 spaces, with explicit alignment of floats on decimal point.
    private static final String OUTPUT_FORMAT = "%-35s%10.2f%25s";
    // Matches CSVs, with first value including optional quotations, spaces, etc.
    private static final String REGEX_PATTERN = "(\"?[-()',\\w\\s]+\"?),(\\d*\\.?\\d*?),(\\w+)\n";

    public static void main(String[] args) {
        Logging.lineSeparator("DELIMITER TEST, BY TYPES", 70);
        delimiterTestByTypes();

        Logging.lineSeparator("DELIMITER TEST, WITH INVALID TYPES", 70);
        delimiterTestByDirectTypes();

        Logging.lineSeparator("RESULT STREAM TEST", 70);
        resultStreamTest();
    }

    /**
     * Retrieves and outputs Scanner results using Java 9 findAll() pattern matching method.
     */
    private static void resultStreamTest() {
        try {
            // Create Scanner to parse passed file.
            Scanner scanner = new Scanner(new File(FILE_PATH));
            // Find all regex matches from REGEX_PATTERN.
            Stream<MatchResult> resultStream = scanner.findAll(REGEX_PATTERN);
            // Use ResultStream to collect results into a list.
            List<MatchResult> list = resultStream.collect(Collectors.toList());
            // Iterate MatchResults to extract and output values.
            for (MatchResult result : list) {
                String country = result.group(1);
                Double gdp = Double.valueOf(result.group(2));
                String code = result.group(3);
                // Output values using OUTPUT_FORMAT.
                Logging.log(String.format(OUTPUT_FORMAT, country, gdp, code));
            }

            // Close scanner after completion.
            scanner.close();
        } catch (InputMismatchException exception) {
            // Output unexpected InputMismatchExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }

    /**
     * Retrieves and outputs Scanner results using Scanner delimiter and getScannerValueByType() results.
     */
    private static void delimiterTestByTypes() {
        try {
            // Create Scanner to parse passed file, using either comma- or newline-delimiter.
            Scanner scanner = new Scanner(new File(FILE_PATH)).useDelimiter("[,\\n]");

            // Iterate through new lines when scanner has a next value.
            while (scanner.hasNextLine() && scanner.hasNext()) {
                // Get next values indirectly through getScannerValueByType() method.
                Object country = getScannerValueByType(String.class, scanner);
                Double gdp = (Double) getScannerValueByType(Double.class, scanner);
                Object code = getScannerValueByType(String.class, scanner);

                // Output values using OUTPUT_FORMAT.
                Logging.log(String.format(OUTPUT_FORMAT, country, gdp, code));
            }

            // Close scanner after completion.
            scanner.close();
        } catch (InputMismatchException exception) {
            // Output unexpected InputMismatchExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }

    /**
     * Retrieves and outputs Scanner results using Scanner delimiter and direct next[TYPE] type method calls.
     */
    private static void delimiterTestByDirectTypes() {
        try {
            // Create Scanner to parse passed file, using either comma- or newline-delimiter.
            Scanner scanner = new Scanner(new File(FILE_PATH)).useDelimiter("[,\\n]");

            // Iterate through new lines when scanner has a next value.
            while (scanner.hasNextLine() && scanner.hasNext()) {
                // Get next values directly, without sanity checks.
                Object country = scanner.next();
                Double gdp = scanner.nextDouble();
                Object code = scanner.next();

                // Output values using OUTPUT_FORMAT.
                Logging.log(String.format(OUTPUT_FORMAT, country, gdp, code));
            }

            // Close scanner after completion.
            scanner.close();
        } catch (InputMismatchException exception) {
            // Output unexpected InputMismatchExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }

    /**
     * Retrieves the appropriate Scanner.next[Type] method result based on passed <T>type</T>.
     *
     * @param clazz Class matching type to retrieve.
     * @param scanner Scanner instance from which to retrieve value.
     * @param <T> Type to retrieve.
     * @return Retrieved value, or null.
     */
    private static <T> Object getScannerValueByType(Class<T> clazz, Scanner scanner) {
        switch (clazz.getSimpleName()) {
            case "Byte":
                if (scanner.hasNextByte())
                    return scanner.nextByte();
                break;
            case "Double":
                if (scanner.hasNextDouble())
                    return scanner.nextDouble();
                break;
            case "Float":
                if (scanner.hasNextFloat())
                    return scanner.nextFloat();
                break;
            case "Integer":
                if (scanner.hasNextInt())
                    return scanner.nextInt();
                break;
            case "Long":
                if (scanner.hasNextLong())
                    return scanner.nextLong();
                break;
            case "Short":
                if (scanner.hasNextShort())
                    return scanner.nextShort();
                break;
            case "String":
                if (scanner.hasNext())
                    return scanner.next();
                break;
            default:
                if (scanner.hasNext())
                    return scanner.next();
                break;
        }
        return null;
    }
}
