# Java Exception Handling - UnknownHostException

Next up in our comprehensive [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series we'll be looking over the **UnknownHostException**.  The `UnknownHostException` can be thrown in a variety of scenarios in which a remote connection fails due to an invalid or unknown host (i.e. IP, URL, URI, etc).

Throughout this article we'll explore the `UnknownHostException` in more detail, first looking at where it sits in the overall [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).  We'll then examine some functional code that illustrates how to establish a socket-based client/server connection, and how issues here may result in an unexpected `UnknownHostException`.  Let's get crackin'!

## The Technical Rundown

All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`java.lang.Object`](https://docs.oracle.com/javase/8/docs/api/java/lang/Object.html)
    - [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy)
        - [`java.lang.Exception`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html)
            - [`java.lang.IOException`](https://docs.oracle.com/javase/8/docs/api/java/io/IOException.html)
                - `UnknownHostException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```java
// Main.java
package io.airbrake;

import io.airbrake.unknownhostexception.Client;

public class Main {

    public static void main(String[] args) {
        Client client = new Client();
    }
}
```

```java
// Server.java
package io.airbrake.unknownhostexception;

import io.airbrake.utility.Logging;

import java.io.*;
import java.net.ServerSocket;
import java.net.Socket;
import java.net.UnknownHostException;

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
            Logging.lineSeparator(String.format("CREATING SERVER: localhost:%d", port));
            // Established server socket at port.
            ServerSocket serverSocket = new ServerSocket(port);

            while (true) {
                // Listen for connection.
                Socket socket = serverSocket.accept();
                // Once client has connected, use socket stream to send a prompt message to client.
                PrintWriter printWriter = new PrintWriter(socket.getOutputStream(), true);
                // Prompt for client.
                printWriter.println("Enter a message for the server.");

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
        } catch (UnknownHostException exception) {
            // Output expected UnknownHostExceptions.
            Logging.log(exception);
        } catch (IOException exception) {
            // Output unexpected IOExceptions.
            Logging.log(exception, false);
        }
    }

}
```

```java
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
                // Save input message/
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
```

```java
// Logging.java
package io.airbrake.utility;

import java.util.Arrays;

import org.apache.commons.lang3.ClassUtils;
import org.apache.commons.lang3.builder.*;

/**
 * Houses all logging methods for various debug outputs.
 */
public class Logging {
    private static final char separatorCharacterDefault = '-';
    private static final String separatorInsertDefault = "";
    private static final int separatorLengthDefault = 40;

    /**
     * Get a String of passed char of passed length size.
     * @param character Character to repeat.
     * @param length Length of string.
     * @return Created string.
     */
    private static String getRepeatedCharString(char character, int length) {
        // Create new character array of proper length.
        char[] characters = new char[length];
        // Fill each array element with character.
        Arrays.fill(characters, character);
        // Return generated string.
        return new String(characters);
    }

    /**
     * Outputs any kind of Object.
     * Uses ReflectionToStringBuilder from Apache commons-lang library.
     *
     * @param value Object to be output.
     */
    public static void log(Object value)
    {
        if (value == null) return;
        // If primitive or wrapper object, directly output.
        if (ClassUtils.isPrimitiveOrWrapper(value.getClass()))
        {
            System.out.println(value);
        }
        else
        {
            // For complex objects, use reflection builder output.
            System.out.println(new ReflectionToStringBuilder(value, ToStringStyle.MULTI_LINE_STYLE).toString());
        }
    }

    /**
     * Outputs any kind of String.
     *
     * @param value String to be output.
     */
    public static void log(String value)
    {
        if (value == null) return;
        System.out.println(value);
    }

    /**
     * Outputs passed in Throwable exception or error instance.
     * Can be overloaded if expected parameter should be specified.
     *
     * @param throwable Throwable instance to output.
     */
    public static void log(Throwable throwable)
    {
        // Invoke call with default expected value.
        log(throwable, true);
    }

    /**
     * Outputs passed in Throwable exception or error instance.
     * Includes Throwable class type, message, stack trace, and expectation status.
     *
     * @param throwable Throwable instance to output.
     * @param expected Determines if this Throwable was expected or not.
     */
    public static void log(Throwable throwable, boolean expected)
    {
        System.out.println(String.format("[%s] %s", expected ? "EXPECTED" : "UNEXPECTED", throwable.toString()));
        throwable.printStackTrace();
    }

    /**
     * See: lineSeparator(String, int, char)
     */
    public static void lineSeparator() {
        lineSeparator(separatorInsertDefault, separatorLengthDefault, separatorCharacterDefault);
    }

    /**
     * See: lineSeparator(String, int, char)
     */
    public static void lineSeparator(String insert) {
        lineSeparator(insert, separatorLengthDefault, separatorCharacterDefault);
    }

    /**
     * See: lineSeparator(String, int, char)
     */
    public static void lineSeparator(int length) {
        lineSeparator(separatorInsertDefault, length, separatorCharacterDefault);
    }

    /**
     * See: lineSeparator(String, int, char)
     */
    public static void lineSeparator(int length, char separator) {
        lineSeparator(separatorInsertDefault, length, separator);
    }

    /**
     * See: lineSeparator(String, int, char)
     */
    public static void lineSeparator(char separator) {
        lineSeparator(separatorInsertDefault, separatorLengthDefault, separator);
    }

    /**
     * See: lineSeparator(String, int, char)
     */
    public static void lineSeparator(String insert, int length) {
        lineSeparator(insert, length, separatorCharacterDefault);
    }

    /**
     * See: lineSeparator(String, int, char)
     */
    public static void lineSeparator(String insert, char separator) {
        lineSeparator(insert, separatorLengthDefault, separator);
    }

    /**
     * Outputs a dashed line separator with
     * inserted text centered in the middle.
     *
     * @param insert Inserted text to be centered.
     * @param length Length of line to be output.
     * @param separator Separator character.
     */
    public static void lineSeparator(String insert, int length, char separator)
    {
        // Default output to insert.
        String output = insert;

        if (insert.length() == 0) {
            output = getRepeatedCharString(separator, length);
        } else if (insert.length() < length) {
            // Update length based on insert length, less a space for margin.
            length -= (insert.length() + 2);
            // Halve the length and floor left side.
            int left = (int) Math.floor(length / 2);
            int right = left;
            // If odd number, add dropped remainder to right side.
            if (length % 2 != 0) right += 1;

            // Surround insert with separators.
            output = String.format("%s %s %s", getRepeatedCharString(separator, left), insert, getRepeatedCharString(separator, right));
        }

        System.out.println(output);
    }
}
```

## When Should You Use It?

Since the `UnknownHostException` only occurs when dealing with some kind of IO connection, most of our sample code will illustrate a basic example of how to establish a client/server connection.  Thus, let's start by going over the simple `Server` class:

```java
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
            Logging.lineSeparator(String.format("CREATING SERVER: localhost:%d", port));
            // Established server socket at port.
            ServerSocket serverSocket = new ServerSocket(port);

            while (true) {
                // Listen for connection.
                Socket socket = serverSocket.accept();
                // Once client has connected, use socket stream to send a prompt message to client.
                PrintWriter printWriter = new PrintWriter(socket.getOutputStream(), true);
                // Prompt for client.
                printWriter.println("Enter a message for the server.");

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
        } catch (UnknownHostException exception) {
            // Output expected UnknownHostExceptions.
            Logging.log(exception);
        } catch (IOException exception) {
            // Output unexpected IOExceptions.
            Logging.log(exception, false);
        }
    }
}
```

All the logic occurs within the `CreateServer(int port)` method, so let's briefly walk through what's going on.  We start by creating a new `ServerSocket` instance at the specified port.  Then, we create an infinite loop so we can repeat the process over and over as new clients appear.  Now we await a new client connection.  Once a connection is established, we create an output `writer`, which sends a specified message to the connected client.  Since we want the client to send a message back, we next establish a `reader` and read from the input stream sent by the client.  The sent message is then output to the console, before closing the writer and socket, and repeating the process.

Now, the client side is handled in the `Client` class:

```java
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
                // Save input message/
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
```

Just as before, most of the logic takes place in a single method, `Connect(String host, int port)`.  Again, we're starting with an infinite loop so we can maintain our connection the server once established.  We start by creating a new `Socket` to the specified `host` and `port`, then create a both a `reader` and `writer`.  The `reader` gets the input stream sent from the server and outputs the message.  We also give ourselves a few keywords (`exit` and `quit`) that can be used to actively disconnect from the server.

Since both the client and server scripts effectively block other threads with infinite loops, we'll need to establish two different threads to test this.  We start by executing the `Server` class, creating a new server:

```
--- CREATING SERVER: localhost:24601 ---
```

Now we launch a new `Client` instance, which starts with a default connection to `localhost:24601`.  As the client output shows, this works fine:

```
---- CONNECTING TO localhost:24601 -----
[FROM Server] Enter a message for the server.
```

As intended, the `Server` recognized the connection and sent a message of `"Enter a message for the server."`.  Now, our client side is awaiting user input in the console, so we'll enter the sentence, `"Are you alive?"`:

```
Are you alive?
[TO Server] Are you alive?
[FROM Server] Enter a message for the server.
```

The client output shows that we successfully sent our message to the server.  The `Server` output confirms that the message was received from the `Client`:

```
[FROM Client] Are you alive?
```

Now, let's intentionally shut down this connection to `localhost:24601` by using the `"quit"` keyword:

```
------------ DISCONNECTING -------------
```

This closes the existing client connection and, per our code, establishes a new connection to `localhose:24601`:

```java
public Client() {
    // Default connection.
    Connect();

    // Attempt to connect to invalid host.
    Connect("localhose");
}
```

Notice the slight typo in the host name.  We immediately see the output showing an intention to connect, followed by a brief pause and then an expected `UnknownHostException` is thrown:

```
---- CONNECTING TO localhose:24601 -----
java.net.UnknownHostException: localhose
```

As mentioned in the introduction, the `UnknownHostException` can occur for a variety of reasons, so this is just one example.  In this case, the host `localhose` doesn't exist, so no connection can be established.  Note that this is specifically different from other connection errors where the host simply refuses connections, or hangs and eventually produces a timeout.

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A detailed review of the Java UnknownHostException, with functional code samples illustrating how to create a client/server connection.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html