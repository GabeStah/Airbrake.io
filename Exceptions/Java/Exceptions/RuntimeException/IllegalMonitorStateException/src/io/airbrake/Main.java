package io.airbrake;

import io.airbrake.utility.Logging;

public class Main {

    static Main main = null;

    public static void main(String[] args)
    {
        main = new Main();
    }

    private Main()
    {
        Logging.lineSeparator("DUAL THREADING");
//        DualThreadingTest();

        Logging.lineSeparator("DUAL THREADING w/ OWNERSHIP");
        DualThreadingWithOwnershipTest();
    }

    private void DualThreadingTest() {
        try {
            // Create manager.
            ThreadingManager manager = new ThreadingManager();

            // Create runners and add to manager.
            manager.addRunner(new Runner("primary"));
            manager.addRunner(new Runner("secondary"));

            // Create threads, set runners, and add to manager.
            manager.addThread(new Thread(manager.getRunner("primary"), "primary"));
            manager.addThread(new Thread(manager.getRunner("secondary"), "secondary"));

            // Start threads.
            manager.getThread("primary").start();
            manager.getThread("secondary").start();
        } catch (IllegalMonitorStateException exception) {
            // Output expected IllegalMonitorStateExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }

    private void DualThreadingWithOwnershipTest() {
        try
        {
            // Create manager.
            ThreadingManager manager = new ThreadingManager();

            // Create runners and add to manager.
            manager.addRunner(new Runner("primary"));
            manager.addRunner(new Runner("secondary"));

            // Create threads, set runners, and add to manager.
            manager.addThread(new Thread(manager.getRunner("primary"), "primary"));
            manager.addThread(new Thread(manager.getRunner("secondary"), "secondary"));

            // Set runner thread ownership.
            manager.getRunner("primary").setThread(manager.getThread("primary"));
            manager.getRunner("secondary").setThread(manager.getThread("secondary"));

            // Start threads.
            manager.getThread("primary").start();
            manager.getThread("secondary").start();
        } catch (IllegalMonitorStateException exception) {
            // Output expected IllegalMonitorStateExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }
}
