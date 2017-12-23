// Server.java
package io.airbrake.socketexception;

import io.airbrake.utility.Logging;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.ServerSocket;
import java.net.Socket;
import java.net.SocketException;

public class Server {

    private final static int DefaultPort = 24601;

    public static void main(String[] args) {
        CreateServer();
    }

    private static void CreateServer() {
        CreateServer(DefaultPort);
    }

    /**
     * Create a socket server at passed port.
     *
     * @param port Port onto which server is socketed.
     */
    private static void CreateServer(int port) {
        try {
            Logging.lineSeparator(String.format("CREATING SERVER: localhost:%d", port), 80);
            // Established server socket at port.
            ServerSocket serverSocket = new ServerSocket(port);

            while (true) {
                // Listen for connection.
                Socket socket = serverSocket.accept();
                // Once client has connected, use socket stream to send a prompt message to client.
                PrintWriter printWriter = new PrintWriter(socket.getOutputStream(), true);

                // Prompt for client.
                String prompt = "Enter a message.";
                Logging.log(String.format("[TO Client] %s", prompt));
                printWriter.println(prompt);

                // Get input stream produced by client (to read sent message).
                BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(socket.getInputStream()));
                String output = bufferedReader.readLine();

                // Output sent message from client.
                printWriter.println(output);

                // Close writer and socket.
                printWriter.close();
                socket.close();

                // Output message from client.
                Logging.log(String.format("[FROM Client] %s", output));

                // Loop back, awaiting a new client connection.
            }
        } catch (SocketException exception) {
            // Output expected SocketExceptions.
            Logging.log(exception);
        } catch (IOException exception) {
            // Output unexpected IOExceptions.
            Logging.log(exception, false);
        }
    }
}
