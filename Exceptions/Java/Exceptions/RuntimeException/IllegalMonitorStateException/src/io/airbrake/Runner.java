package io.airbrake;

import io.airbrake.utility.Logging;

public class Runner implements Runnable
{
    private String name;
    private Thread thread;

    Runner(String name) {
        setName(name);
    }

    Runner(String name, Thread thread) {
        setName(name);
    }

    String getName() {
        return this.name;
    }

    private Thread getThread() {
        return thread;
    }

    private void setName(String name) {
        this.name = name;
    }

    void setThread(Thread thread) {
        this.thread = thread;
    }

    public void run()
    {
        try
        {
            // Check for ownership thread.
            if (getThread() == null) {
                Logging.log(String.format("Waiting for thread %s in runner %s.", "Main", getName()));
                // If no thread, wait for main thread.
                Main.main.wait();
            } else {
                synchronized (getThread()) {
                    Logging.log(String.format("Waiting for thread %s in runner %s.", getThread().getName(), getName()));
                    // If thread, invoke wait().
                    getThread().wait();
                }
            }
        } catch (IllegalMonitorStateException exception) {
            // Output expected IllegalMonitorStateExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }
}
