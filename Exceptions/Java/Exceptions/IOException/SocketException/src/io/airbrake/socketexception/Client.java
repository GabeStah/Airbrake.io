// Client.java
package io.airbrake.socketexception;

import io.airbrake.utility.Logging;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.Socket;
import java.net.SocketException;

public class Client {
    private final static String DefaultHost = "localhost";
    private final static int DefaultPort = 24601;
    private final static int DefaultTimeout = 2000;
    private final static boolean DefaultShouldSleep = false;

    private String message;
    private Socket socket;

    public static void main(String[] args) {
        Client client = new Client();
    }

    public Client() {
        // Default connection.
        Connect();

        // Attempt to connect using 1 millisecond timeout.
        Connect(DefaultHost, DefaultPort, 1, DefaultShouldSleep);
    }

    private void Connect() {
        Connect(DefaultHost, DefaultPort, DefaultTimeout, DefaultShouldSleep);
    }

    private void Connect(String host) {
        Connect(host, DefaultPort, DefaultTimeout, DefaultShouldSleep);
    }

    private void Connect(int port) {
        Connect(DefaultHost, port, DefaultTimeout, DefaultShouldSleep);
    }

    private void Connect(boolean shouldSleep) {
        Connect(DefaultHost, DefaultPort, DefaultTimeout, shouldSleep);
    }

    /**
     * Connect to passed host and port as client.
     *
     * @param host Host to connect to.
     * @param port Port to connect to.
     * @param timeout Timeout (in milliseconds) to allow for socket connection.
     * @param shouldSleep Indicates if thread should be artificially slept.
     */
    private void Connect(String host, int port, int timeout, boolean shouldSleep) {
        try {
            Logging.lineSeparator(
                String.format(
                        "CONNECTING TO %s:%d WITH %d MS TIMEOUT%s",
                        host,
                        port,
                        timeout,
                        shouldSleep ? " AND 500 MS SLEEP" : ""
                ),
                80
            );

            while (true) {
                socket = new Socket();
                // Connect to socket by host, port, and with specified timeout.
                socket.connect(new InetSocketAddress(InetAddress.getByName(host), port), timeout);

                // Read input stream from server and output said message.
                BufferedReader reader = new BufferedReader(new InputStreamReader(socket.getInputStream()));

                // Check if artificial sleep should occur, to simulate connection delay.
                if (shouldSleep) {
                    // Sleep for half a second.
                    Thread.sleep(500);
                }

                PrintWriter writer = new PrintWriter(socket.getOutputStream(), true);

                Logging.log("[FROM Server] " + reader.readLine());

                // Await user input via System.in (standard input stream).
                BufferedReader userInputBR = new BufferedReader(new InputStreamReader(System.in));
                // Save input message.
                message = userInputBR.readLine();

                // Send message to server via output stream.
                writer.println(message);

                // If message is 'quit' or 'exit', intentionally disconnect.
                if ("quit".equalsIgnoreCase(message) || "exit".equalsIgnoreCase(message)) {
                    Logging.lineSeparator("DISCONNECTING");
                    socket.close();
                    break;
                }

                Logging.log("[TO Server] " + reader.readLine());
            }
        } catch (SocketException exception) {
            // Output expected SocketExceptions.
            Logging.log(exception);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }
}
