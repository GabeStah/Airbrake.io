package io.airbrake;

import io.airbrake.utility.Logging;

public class InterruptableThreadTest {

    private boolean shouldSleepMain = false;
    private boolean shouldInterrupt = false;
    private int sleepMainDuration = 2500;

    public InterruptableThreadTest(boolean shouldSleepMain, boolean shouldInterrupt, int sleepMainDuration) {
        this.shouldSleepMain = shouldSleepMain;
        this.shouldInterrupt = shouldInterrupt;
        this.sleepMainDuration = sleepMainDuration;

        try {
            Thread main = Thread.currentThread();
            // Create InterruptableThread named 'secondary.'
            InterruptableThread secondary = new InterruptableThread("secondary");
            // Start thread and sleep, if applicable.
            Logging.log(String.format("%s '%s' started.",
                    secondary.getClass().getSimpleName(),
                    secondary.getName()),
                    true);
            secondary.start();
            if (this.shouldSleepMain) {
                Logging.log(String.format("%s '%s' sleeping for %d ms.",
                        main.getClass().getSimpleName(),
                        main.getName(),
                        this.sleepMainDuration),
                        true);
                Thread.sleep(this.sleepMainDuration);
                Logging.log(String.format("%s '%s' sleeping complete.",
                        main.getClass().getSimpleName(),
                        main.getName()),
                        true);
            }
            // Interrupt, if applicable.
            if (this.shouldInterrupt) {
                Logging.log(String.format("%s '%s' interrupted.",
                        secondary.getClass().getSimpleName(),
                        secondary.getName()),
                        true);
                secondary.interrupt();
            }
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }

    public static class InterruptableThreadTestBuilder {
        private boolean shouldSleepMain;
        private boolean shouldInterrupt;
        private int sleepMainDuration;

        public InterruptableThreadTestBuilder setShouldSleepMain(boolean shouldSleepMain) {
            this.shouldSleepMain = shouldSleepMain;
            return this;
        }

        public InterruptableThreadTestBuilder setShouldInterrupt(boolean shouldInterrupt) {
            this.shouldInterrupt = shouldInterrupt;
            return this;
        }

        public InterruptableThreadTestBuilder setSleepMainDuration(int sleepMainDuration) {
            this.sleepMainDuration = sleepMainDuration;
            return this;
        }

        public InterruptableThreadTest createInterruptableThreadTest() {
            return new InterruptableThreadTest(shouldSleepMain, shouldInterrupt, sleepMainDuration);
        }
    }
}
