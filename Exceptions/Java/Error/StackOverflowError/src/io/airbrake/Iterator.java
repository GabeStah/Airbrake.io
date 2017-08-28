// Iterator.java
package io.airbrake;

import io.airbrake.utility.Logging;

public class Iterator {
    private static final double MILLION = 1000000.0;
    private long count = 0;
    // Create local reference to logging class, to avoid NoClassDefFoundErrors.
    private Logging logger = new Logging();
    private long startTime = System.nanoTime();

    Iterator() { }

    void increment() {
        try {
            // Increment count and call self.
            this.count++;
            increment();
        } catch (StackOverflowError error) {
            // Get elapsed time.
            long elapsed = System.nanoTime() - startTime;
            // Output iteration count and total elapsed time.
            logger.lineSeparator(String.format("%d iterations in %s milliseconds.", this.count, (double) elapsed / MILLION), 60);
            // Output expected StackOverflowErrors.
            logger.log(error);
        } catch (Throwable throwable) {
            // Output unexpected Throwables.
            logger.log(throwable, false);
        }
    }
}
