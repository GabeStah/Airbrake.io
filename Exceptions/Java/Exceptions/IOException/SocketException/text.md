# Java Exception Handling - SocketException

Making our way through our in-depth [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series, today we'll be going over the **SocketException**.  As the name suggests, a `SocketException` occurs when a problem occurs while trying to create or access a [`Socket`](https://docs.oracle.com/javase/8/docs/api/java/net/Socket.html).

Throughout this article we'll examine the `SocketException` in more detail, starting with where it sits in the overall [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).  We'll then look at some functional sample code that will illustrate how a typical socket connection can be established, and how how connectivity issues between client/server can produce unexpected `SocketExceptions`, so let's begin!

## The Technical Rundown

All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`java.lang.Object`](https://docs.oracle.com/javase/8/docs/api/java/lang/Object.html)
    - [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy)
        - [`java.lang.Error`](https://docs.oracle.com/javase/8/docs/api/java/lang/Error.html)
            - [`java.io.IOException`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html)
                - `SocketException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```java
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
```

```java
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
```

This code sample also uses the [`Logging`](https://github.com/GabeStah/Airbrake.io/blob/master/lib/java/src/io/airbrake/utility/Logging.java) utility class, the source of which can be [found here on GitHub](https://github.com/GabeStah/Airbrake.io/blob/master/lib/java/src/io/airbrake/utility/Logging.java).

## When Should You Use It?

The `SocketException` is similar to the [`SocketTimeoutException`](https://airbrake.io/blog/java-exception-handling/sockettimeoutexception) we [explored last month](https://airbrake.io/blog/java-exception-handling/sockettimeoutexception), but, believe it or not, `SocketTimeoutException` is not inherited from the broader `SocketException` class.  Instead, `SocketTimeoutException` -- which indicates a timeout occurred during a read or acceptance message within a socket connection -- inherits from `java.io.InterruptedIOException`, which itself inherits from `java.io.IOException` (the same parent class used by `SocketException`).  In practical terms, this means that `SocketException` is usually indicative of a more generic connection issue, while `SocketTimeoutException` is explicitly thrown only when timeout problems occur.

To illustrate how a `SocketException` can commonly occur we'll start with a simple client/server configuration, similar to that we used in a previous post.  We run the test by running both the client and server on separate threads, then the server will prompt the client to enter a message.  Once the client enters a message, that value is transferred to and received by the server, before the server then prompts for a new message.  This repeats until the client gracefully ends the connection.

We'll start by looking at the `Server.java` class:

```java
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
```

Executing the `Server.main()` method invokes `CreateServer(int port)` and creates a `ServerSocket(int port)` and the `DefaultPort` of `24601`.  The server socket accepts incoming connections on that port, creates a `PrintWriter` using an output stream to send a prompt message to the client.  Once sent, the server creates a new `InputStreamReader` from the input stream coming from the client, then reads and outputs the message the client sent.  A continuous `while` loop is used to repeat this whole process over and over, awaiting a connection, prompting for input, and outputting the received input from the client.

Meanwhile, here's the `Client.java` class:

```java
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
```

The `Client.main()` method creates a new `Client` instance, which invokes `Connect(...)` two times.  The first invocation uses default values (including `port` `24601`), while the second connection does the same but with a very short `1 ms` timeout period.  Otherwise, functionality of `Client.Connect(...)` is rather similar to `Server.CreateServer(int port)`.  It begins by establishing a new `Socket` instance and connecting to the proper host (`localhost`), port, and using the passed timeout value.  An `InputStreamReader` is used to get the input stream from the server, which is output to the client.  The client is then prompted to send a message, which is passed to the server.  If the client enters `quit` or `exit` keywords, the socket connection is closed.  Meanwhile, another `while` loop repeats thie entire process over and over.

If we test this out we start by running `Server.java`, then `Client.java` afterward.  The server outputs:

```
----------------------- CREATING SERVER: localhost:24601 -----------------------
[TO Client] Enter a message.
```

And the client outputs after entering "Hello" at the prompt:

```
-------------- CONNECTING TO localhost:24601 WITH 2000 MS TIMEOUT --------------
[FROM Server] Enter a message.
Hello
[TO Server] Hello
[FROM Server] Enter a message.
```

Everything is working as expected.  But, what happens if we interrupt the client connection in some way, such as terminating the process?  Immediately the server experiences a problem and catches the thrown `SocketException`:

```
[EXPECTED] java.net.SocketException: Connection reset
```

This shouldn't be much of a surprise that closing the client connection produces an error on the server.  However, what happens if we reverse the process and terminate the server process while connected?  As it happens, the client prompt remains open and awaits input from the user, due to these code statements:

```java
// Await user input via System.in (standard input stream).
BufferedReader userInputBR = new BufferedReader(new InputStreamReader(System.in));
// Save input message.
message = userInputBR.readLine();
```

Execution halts while awaiting for user input.  However, as soon as a message is entered the client recognizes that the server has disconnected, so the client now throws a `SocketException` when execution resumes:

```
[EXPECTED] java.net.SocketException: Connection reset
```

The `Client` constructor invokes a second `Client.Connect(...)` call after the first, so now that the server has been terminated, the second attempt by the client to establish a new `Socket` connection results in a `SocketTimeoutException`, since no connection could be established to the server:

```
--------------- CONNECTING TO localhost:24601 WITH 1 MS TIMEOUT ----------------
[UNEXPECTED] java.net.SocketTimeoutException: connect timed out
```

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look at the Java SocketException, with functional code samples illustrating how to create a simple client/server socket connection.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html