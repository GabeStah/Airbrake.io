package io.airbrake;

public class Main {

    public static void main(String[] args) {
        InterruptableThreadTest tester = new InterruptableThreadTest.InterruptableThreadTestBuilder()
                .setShouldInterrupt(true)
                .setShouldSleepMain(true)
                .setSleepMainDuration(2500)
                .createInterruptableThreadTest();
    }
}