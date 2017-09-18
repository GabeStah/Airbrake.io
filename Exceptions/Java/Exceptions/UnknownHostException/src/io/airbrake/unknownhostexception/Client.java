// Client.java
package io.airbrake.unknownhostexception;

import io.airbrake.utility.Logging;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.Socket;
import java.net.UnknownHostException;

public class Client {
    private final static String DefaultHost = "localhost";
    private final static int DefaultPort = 24601;

    private String Message;

    public Client() {
        // Default connection.
        Connect();

        // Attempt to connect to invalid host.
        Connect("localhose");
    }

    private void Connect() {
        Connect(DefaultHost, DefaultPort);
    }

    private void Connect(String host) {
        Connect(host, DefaultPort);
    }

    private void Connect(int port) {
        Connect(DefaultHost, port);
    }

    /**
     * Connect to passed host and port as client.
     *
     * @param host Host to connect to.
     * @param port Port to connect to.
     */
    private void Connect(String host, int port) {
        try {
            Logging.lineSeparator(String.format("CONNECTING TO %s:%d", host, port));

            while (true) {
                // Create and connect to socket at specified host and port.
                Socket socket = new Socket(host, port);

                // Read input stream from server and output said message.
                BufferedReader reader = new BufferedReader(new InputStreamReader(socket.getInputStream()));
                PrintWriter writer = new PrintWriter(socket.getOutputStream(), true);

                Logging.log("[FROM Server] " + reader.readLine());

                // Await user input via System.in (standard input stream).
                BufferedReader userInputBR = new BufferedReader(new InputStreamReader(System.in));
                // Save input message.
                Message = userInputBR.readLine();

                // Send message to server via output stream.
                writer.println(Message);

                // If message is 'quit' or 'exit', intentionally disconnect.
                if ("quit".equalsIgnoreCase(Message) || "exit".equalsIgnoreCase(Message)) {
                    Logging.lineSeparator("DISCONNECTING");
                    socket.close();
                    break;
                }

                Logging.log("[TO Server] " + reader.readLine());
            }
        } catch (UnknownHostException exception) {
            // Output expected UnknownHostExceptions.
            Logging.log(exception);
        } catch (IOException exception) {
            // Output unexpected IOExceptions.
            Logging.log(exception, false);
        }
    }
}
