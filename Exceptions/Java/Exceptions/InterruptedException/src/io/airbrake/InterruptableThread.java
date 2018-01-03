package io.airbrake;

import io.airbrake.utility.Logging;

public class InterruptableThread extends Thread {

    InterruptableThread(String name) {
        super(name);
    }

    public void run() {
        try {
            Logging.log(String.format("%s '%s' sleeping for %d ms.",
                    this.getClass().getSimpleName(),
                    this.getName(),
                    2000),
                    true);
            sleep(2000);
            Logging.log(String.format("%s '%s' sleeping complete.",
                    this.getClass().getSimpleName(),
                    this.getName()),
                    true);
        } catch (InterruptedException exception) {
            // Output expected InterruptedExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }
}
