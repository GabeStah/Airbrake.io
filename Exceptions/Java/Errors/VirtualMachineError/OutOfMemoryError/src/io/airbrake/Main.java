package io.airbrake;

import io.airbrake.utility.Logging;

import java.lang.management.ManagementFactory;
import java.lang.management.MemoryMXBean;
import java.lang.management.MemoryUsage;
import java.text.NumberFormat;

public class Main {
    // Maximum allowed array size in current JVM.
    private static final int MAX_ARRAY_SIZE = Integer.MAX_VALUE - 2;

    public static void main(String[] args) {
        // Minute value.
        long bytes = 24601;
        Logging.lineSeparator(formatNumber(bytes), '=');
        allocateMemory(bytes);

        // Ten million.
        bytes = 10000000;
        Logging.lineSeparator(formatNumber(bytes), '=');
        allocateMemory(bytes);

        // Max integer value.
        bytes = 2147483647;
        Logging.lineSeparator(formatNumber(bytes), '=');
        allocateMemory(bytes);

        // Medium value.
        bytes = 2500000000L;
        Logging.lineSeparator(formatNumber(bytes), '=');
        allocateMemory(bytes);

        // Large value.
        bytes = 3000000000L;
        Logging.lineSeparator(formatNumber(bytes), '=');
        allocateMemory(bytes);

        // Extreme value.
        bytes = 9999999999999999L;
        Logging.lineSeparator(formatNumber(bytes), '=');
        allocateMemory(bytes);
    }

    /**
     * Attempts to allocate memory of the given number of bytes.
     *
     * @param bytes Number of bytes to allocate.
     */
    private static void allocateMemory(long bytes) {
        try {
            Logging.log(String.format("Attempting to allocate %s bytes of memory.", formatNumber(bytes)));
            // Check if bytes exceeds maximum bytes in array.
            if (bytes > MAX_ARRAY_SIZE) {
                // Determine number of memory chunks contained in bytes.
                long chunks = getArrayChunkCount(bytes);
                // Get remainder after chunking.
                int remainder = (int) (bytes - MAX_ARRAY_SIZE * chunks);
                // Two-dimensional array containing an array of MAX_ARRAY_SIZE per chunk.
                byte[][] chunkedByteArray = new byte[(int) chunks][MAX_ARRAY_SIZE];
                // Remainder array.
                byte[] remainingByteArray = new byte[remainder];
            } else {
                // Array of bytes bytes.
                byte[] byteArray = new byte[(int) bytes];
            }
            // Output memory usage info.
            printMemoryUsage();
            Logging.log(String.format("SUCCESSFULLY allocated %s bytes of memory.", formatNumber(bytes)));
        } catch (OutOfMemoryError error) {
            Logging.log(String.format("FAILED to allocate %s bytes of memory.", formatNumber(bytes)));
            // Output expected OutOfMemoryErrors.
            Logging.log(error);
        } catch (Exception | Error exception) {
            Logging.log(String.format("FAILED to allocate %s bytes of memory.", formatNumber(bytes)));
            // Output unexpected Exceptions/Errors.
            Logging.log(exception, false);
        }
    }

    /**
     * Gets the number of maximum-sized arrays that can
     * be created within the passed number of bytes.
     *
     * @param bytes Total number of bytes.
     * @return Number of maximum-sized arrays.
     */
    private static long getArrayChunkCount(long bytes) {
        return (bytes / MAX_ARRAY_SIZE);
    }

    /**
     * Prints current memory usage stats.
     */
    private static void printMemoryUsage() {
        try {
            MemoryMXBean memoryBean = ManagementFactory.getMemoryMXBean();
            MemoryUsage heapMemory = memoryBean.getHeapMemoryUsage();
            MemoryUsage nonHeapMemory = memoryBean.getNonHeapMemoryUsage();
            String format = "%-15s%-15s%-15s%-15s";

            Logging.lineSeparator("HEAP MEMORY");
            Logging.log(String.format(format, "COMMITTED", "INIT", "USED", "MAX"));
            Logging.log(String.format(format,
                    formatNumber(heapMemory.getCommitted()),
                    formatNumber(heapMemory.getInit()),
                    formatNumber(heapMemory.getUsed()),
                    formatNumber(heapMemory.getMax()))
            );

            Logging.lineSeparator("NON-HEAP MEMORY");
            Logging.log(String.format(format, "COMMITTED", "INIT", "USED", "MAX"));
            Logging.log(String.format(format,
                    formatNumber(nonHeapMemory.getCommitted()),
                    formatNumber(nonHeapMemory.getInit()),
                    formatNumber(nonHeapMemory.getUsed()),
                    formatNumber(nonHeapMemory.getMax()))
            );
        } catch (OutOfMemoryError error) {
            // Output expected OutOfMemoryErrors.
            Logging.log(error);
        } catch (Exception | Error exception) {
            // Output unexpected Exceptions/Errors.
            Logging.log(exception, false);
        }
    }

    /**
     * Formats passed number as comma-delimited String.
     *
     * @param number Number to format.
     * @return Comma-delimited String.
     */
    private static String formatNumber(long number) {
        return NumberFormat.getInstance().format(number);
    }
}
