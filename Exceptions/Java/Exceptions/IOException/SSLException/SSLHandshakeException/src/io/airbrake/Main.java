package io.airbrake;

import java.io.IOException;

public class Main {

    public static void main(String[] args) throws IOException {
        Thread serverThread = new Thread(new SSLServer());
        serverThread.start();

        Thread clientThread = new Thread(new SSLClient());
        clientThread.start();
    }
}

