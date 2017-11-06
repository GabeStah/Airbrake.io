package io.airbrake;

import java.util.ArrayList;
import java.util.Objects;

public class ThreadingManager {
    private ArrayList<Runner> runners = new ArrayList<>();
    private ArrayList<Thread> threads = new ArrayList<>();

    ThreadingManager() { }

    void addRunner(Runner runner) {
        this.runners.add(runner);
    }

    void addThread(Thread thread) {
        threads.add(thread);
    }

    public Runner getRunner(int index) {
        return runners.get(index);
    }

    Runner getRunner(String name) {
        for (Runner runner : runners) {
            if (Objects.equals(runner.getName(), name)) {
                return runner;
            }
        }
        return null;
    }

    public ArrayList<Runner> getRunners() {
        return runners;
    }

    public Thread getThread(int index) {
        return threads.get(index);
    }

    Thread getThread(String name) {
        for (Thread thread : threads) {
            if (Objects.equals(thread.getName(), name)) {
                return thread;
            }
        }
        return null;
    }

    public ArrayList<Thread> getThreads() {
        return threads;
    }

    public void setRunners(ArrayList<Runner> runners) {
        this.runners = runners;
    }

    public void setThreads(ArrayList<Thread> threads) {
        this.threads = threads;
    }
}
