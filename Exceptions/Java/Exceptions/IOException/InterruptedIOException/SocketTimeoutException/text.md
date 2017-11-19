# Java Exception Handling - SocketTimeoutException

Making our way through our in-depth [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series, today we'll be going over the **SocketTimeoutException**.  As you may suspect based on the name, the `SocketTimeoutException` is thrown when a timeout occurs during a read or acceptance message within a socket connection.

Throughout this article we'll explore the `SocketTimeoutException` in more detail, starting with where it resides in the overall [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).  We'll then look at some functional sample code that will illustrate how a common socket connection might be established, and how failure to plan ahead for potential connection timeouts can lead to unexpected `SocketTimeoutExceptions`.  Let's get this party started!

## The Technical Rundown

All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`java.lang.Object`](https://docs.oracle.com/javase/8/docs/api/java/lang/Object.html)
    - [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy)
        - [`java.lang.Error`](https://docs.oracle.com/javase/8/docs/api/java/lang/Error.html)
            - [`java.io.IOException`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html)
                - [`java.io.InterruptedIOException`](https://docs.oracle.com/javase/8/docs/api/java/io/InterruptedIOException.html)
                    - `SocketTimeoutException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```java
// Server.java
package io.airbrake.sockettimeoutexception;

import io.airbrake.utility.Logging;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.ServerSocket;
import java.net.Socket;
import java.net.SocketTimeoutException;

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
        } catch (SocketTimeoutException exception) {
            // Output expected SocketTimeoutExceptions.
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
package io.airbrake.sockettimeoutexception;

import io.airbrake.utility.Logging;

import java.io.*;
import java.net.*;

public class Client {
    private final static String DefaultHost = "localhost";
    private final static int DefaultPort = 24601;
    private final static int DefaultTimeout = 2000;
    private final static boolean DefaultShouldSleep = false;

    private String Message;

    public Client() {
        // Default connection.
        Connect();

        // Attempt to connect using 1 millisecond timeout.
        Connect(DefaultHost, DefaultPort, 1, DefaultShouldSleep);

        // Attempt to connect using 1 millisecond timeout, with artificial sleep to simulate connection delay.
        Connect(DefaultHost, DefaultPort, 1, true);
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
                Socket socket = new Socket();
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
        } catch (SocketTimeoutException exception) {
            // Output expected SocketTimeoutExceptions.
            Logging.log(exception);
        } catch (InterruptedException | IOException exception) {
            // Output unexpected InterruptedExceptions and IOExceptions.
            Logging.log(exception, false);
        }
    }
}
```

```java
// Main.java
package io.airbrake;

import io.airbrake.sockettimeoutexception.Client;

public class Main {

    public static void main(String[] args) {
        Client client = new Client();
    }
}
```

This code sample also uses the [`Logging`](https://github.com/GabeStah/Airbrake.io/blob/master/lib/java/src/io/airbrake/utility/Logging.java) utility class, the source of which can be [found here on GitHub](https://github.com/GabeStah/Airbrake.io/blob/master/lib/java/src/io/airbrake/utility/Logging.java).

## When Should You Use It?

A `SocketTimeoutException` can really occur under only a handful of circumstances (at least, while using the standard Java libraries).  All of these scenarios occur when dealing directly with the [`Socket`](https://docs.oracle.com/javase/8/docs/api/java/net/Socket.html) class, which allows for client sockets to be established to remote endpoints.  In other words, `Sockets` are used to establish connections between two systems, including the common `client` and `server` configuration that'll be using today in our example code.

To begin we'll need the `Server` class:

```java
// Server.java
package io.airbrake.sockettimeoutexception;

import io.airbrake.utility.Logging;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.ServerSocket;
import java.net.Socket;
import java.net.SocketTimeoutException;

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
        } catch (SocketTimeoutException exception) {
            // Output expected SocketTimeoutExceptions.
            Logging.log(exception);
        } catch (IOException exception) {
            // Output unexpected IOExceptions.
            Logging.log(exception, false);
        }
    }
}
```

The `Server` class file also includes a `main(...)` method so we can easily execute the server creation process in a separate thread from our client code, which we'll look at in a moment.  Overall, the server is established by creating a new [`ServerSocket`](https://docs.oracle.com/javase/8/docs/api/java/net/ServerSocket.html) instance, associated with the specified `port`.  We then enter a constant loop listening for incoming socket connections and printing out a stream with the data message sent by the client, if applicable.  This process repeats until the connection is lost or an error occurs.

Meanwhile, our `Client` class is slightly more complex, but still fairly simple:

```java
// Client.java
package io.airbrake.sockettimeoutexception;

import io.airbrake.utility.Logging;

import java.io.*;
import java.net.*;

public class Client {
    private final static String DefaultHost = "localhost";
    private final static int DefaultPort = 24601;
    private final static int DefaultTimeout = 2000;
    private final static boolean DefaultShouldSleep = false;

    private String Message;

    public Client() {
        // Default connection.
        Connect();

        // Attempt to connect using 1 millisecond timeout.
        Connect(DefaultHost, DefaultPort, 1, DefaultShouldSleep);

        // Attempt to connect using 1 millisecond timeout, with artificial sleep to simulate connection delay.
        Connect(DefaultHost, DefaultPort, 1, true);
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
                Socket socket = new Socket();
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
        } catch (SocketTimeoutException exception) {
            // Output expected SocketTimeoutExceptions.
            Logging.log(exception);
        } catch (InterruptedException | IOException exception) {
            // Output unexpected InterruptedExceptions and IOExceptions.
            Logging.log(exception, false);
        }
    }
}
```

The `Connect(String host, int port, int timeout, boolean shouldSleep)` method overload is where most of the logic occurs, but we've also added a number of other `Connect(...)` method overloads for simplicity and usability, relying on a handful of default property values for things like the `host`, `port`, and `timeout` period.  Just as with our server code, the `Client` connection process creates an infinite loop in which it establishes a new `Socket`, then attempts a direct connection to the specified `host` and `port`.  Critically, we're also passing the second parameter to the `Socket.connect(...)` method, which indicates the `timeout` period (in milliseconds) that the connection is allowed before timing out.

Once a connection is established the client creates a reader and writer, which we use to send and receive data messages between the server.  The client can write messages in the console, which are sent to the server and the server will respond to the client, in kind.  Sending `quit` or `exit` will close the connection and exit the loop.

To test things out we start with a baseline connection test.  First we must initialize our `Server`:

```java
public static void main(String[] args) {
    CreateServer();
}
```

As expected, this outputs a confirmation indicating the server is up and running:

```
--- CREATING SERVER: localhost:24601 ---
```

Now, we'll begin with a basic client connection using our default parameters:

```java
public Client() {
    // Default connection.
    Connect();

    // ...
}
```

The client output shows that a connection was established and the server sends a message prompting us for some input:

```
-------------- CONNECTING TO localhost:24601 WITH 2000 MS TIMEOUT --------------
[FROM Server] Enter a message for the server.
```

Let's just say "hello" to the server, which outputs the following for the client:

```
Hello
[TO Server] Hello
```

Meanwhile, the server receives the message and outputs this;

```
[FROM Client] Hello world
```

Cool.  Everything seems to be working as expected.  If we type `exit` or `quit` we'll close the client connection and can move onto the next test:

```
quit
------------ DISCONNECTING -------------
```

Our next `Connect(...)` method call includes a 1 millisecond timeout:

```java
// Attempt to connect using 1 millisecond timeout.
Connect(DefaultHost, DefaultPort, 1, DefaultShouldSleep);
```

Normally, 1 millisecond is an _exceptionally_ inadequate timeout period.  However, since both the client and server are running locally in our development setup, running this code will _usually_ not result in any problems.  As we see here, we were able to connect without experiencing a timeout, and successfully sent another message to the server:

```
--------------- CONNECTING TO localhost:24601 WITH 1 MS TIMEOUT ----------------
[FROM Server] Enter a message for the server.
Hi, again.
[TO Server] Hi, again.
```

To simulate an actual delayed network connection we'll lastly try adding an explicit pause during our connection process by calling `Thread.sleep(...)` within the `Connect(...)` method:

```java
// Attempt to connect using 1 millisecond timeout, with artificial sleep to simulate connection delay.
Connect(DefaultHost, DefaultPort, 1, true);
```

Executing this code will typically throw a `SocketTimeoutException` our way, indicating that the connection could not be established within the extremely short one millisecond timeout period:

```
------- CONNECTING TO localhost:24601 WITH 1 MS TIMEOUT AND 500 MS SLEEP -------
[EXPECTED] java.net.SocketTimeoutException
```

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A deep dive into the Java SocketTimeoutException, with functional code samples illustrating how to create a simple client/server socket connection.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html