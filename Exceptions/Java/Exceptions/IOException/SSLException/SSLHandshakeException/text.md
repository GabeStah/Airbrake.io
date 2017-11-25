# Java Exception Handling - SSLHandshakeException

Moving along through our detailed [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series we come to the **SSLHandshakeException**.  The `SSLHandshakeException` is thrown when an error occurs while a client and server connection fails to agree on their desired security level.  This exception is one of a handful of classes that inherits from the parent [`SSLException`](https://docs.oracle.com/javase/8/docs/api/javax/net/ssl/SSLException.html) class.

Within this article we'll examine the `SSLHandshakeException` by looking at where it sits in the overall [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).  We'll also dig into some functional sample code that shows how an SSL connection might be established in a Java application, and how failure to set things up properly can lead to `SSLHandshakeExceptions`, so let's get to it!

## The Technical Rundown

All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`java.lang.Object`](https://docs.oracle.com/javase/8/docs/api/java/lang/Object.html)
    - [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy)
        - [`java.lang.Error`](https://docs.oracle.com/javase/8/docs/api/java/lang/Error.html)
            - [`java.io.IOException`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html)
                - [`java.net.ssl.SSLException`](https://docs.oracle.com/javase/8/docs/api/javax/net/ssl/SSLException.html)
                    - `SSLHandshakeException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```java
// SSLServer.java
package io.airbrake;

import io.airbrake.utility.Logging;

import javax.net.ssl.SSLServerSocketFactory;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.ServerSocket;
import java.net.Socket;
import java.net.SocketTimeoutException;

public class SSLServer implements Runnable {

    private final static int DefaultPort = 24601;
    private int Port = DefaultPort;

    SSLServer() { }

    SSLServer(int port) { Port = port; }

    private static void createServer() {
        createServer(DefaultPort);
    }

    /**
     * Create a socket server at passed port.
     *
     * @param port Port onto which server is socketed.
     */
    private static void createServer(int port) {
        try {
            Logging.lineSeparator(String.format("CREATING SSL SERVER: localhost:%d", port));
            SSLServerSocketFactory factory = (SSLServerSocketFactory) SSLServerSocketFactory.getDefault();
            // Established server socket at port.
            ServerSocket serverSocket = factory.createServerSocket(port);

            while (true) {
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
        } catch (SSLHandshakeException exception) {
            // Output expected SSLHandshakeExceptions.
            Logging.log(exception);
        } catch (IOException exception) {
            // Output unexpected IOExceptions.
            Logging.log(exception, false);
        }
    }

    @Override
    public void run() {
        // Create server instance.
        createServer(Port);
    }
}
```

```java
// SSLClient.java
package io.airbrake;

import io.airbrake.utility.Logging;

import javax.net.ssl.SSLSocket;
import javax.net.ssl.SSLSocketFactory;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.SocketTimeoutException;

public class SSLClient implements Runnable {
    private final static String DefaultHost = "localhost";
    private final static int DefaultPort = 24601;
    private final static int DefaultTimeout = 2000;
    private final static boolean DefaultShouldSleep = false;

    private String Host = DefaultHost;
    private int Port = DefaultPort;
    private int Timeout = DefaultTimeout;
    private boolean ShouldSleep = DefaultShouldSleep;

    private String Message;

    SSLClient() {
    }

    public SSLClient(String host) {
        Host = host;
    }

    public SSLClient(int port) {
        Port = port;
    }

    public SSLClient(boolean shouldSleep) {
        ShouldSleep = shouldSleep;
    }

    private void connect() {
        connect(DefaultHost, DefaultPort, DefaultTimeout, DefaultShouldSleep);
    }

    private void connect(String host) {
        connect(host, DefaultPort, DefaultTimeout, DefaultShouldSleep);
    }

    private void connect(int port) {
        connect(DefaultHost, port, DefaultTimeout, DefaultShouldSleep);
    }

    private void connect(boolean shouldSleep) {
        connect(DefaultHost, DefaultPort, DefaultTimeout, shouldSleep);
    }

    /**
     * Attempt SSL connection to passed host and port as client.
     *
     * @param host        Host to connect to.
     * @param port        Port to connect to.
     * @param timeout     Timeout (in milliseconds) to allow for socket connection.
     * @param shouldSleep Indicates if thread should be artificially slept.
     */
    private void connect(String host, int port, int timeout, boolean shouldSleep) {
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
                SSLSocketFactory factory = (SSLSocketFactory) SSLSocketFactory.getDefault();

                SSLSocket socket = (SSLSocket) factory.createSocket(host, port);

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
        } catch (SSLHandshakeException exception) {
            // Output expected SSLHandshakeExceptions.
            Logging.log(exception);
        } catch (InterruptedException | IOException exception) {
            // Output unexpected InterruptedExceptions and IOExceptions.
            Logging.log(exception, false);
        }
    }

    @Override
    public void run() {
        // Attempt connection.
        connect(Host, Port, Timeout, ShouldSleep);
    }
}
```

```java
// Main.java
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
```

This code sample also uses the [`Logging`](https://github.com/GabeStah/Airbrake.io/blob/master/lib/java/src/io/airbrake/utility/Logging.java) utility class, the source of which can be [found here on GitHub](https://github.com/GabeStah/Airbrake.io/blob/master/lib/java/src/io/airbrake/utility/Logging.java).

## When Should You Use It?

To understand what can cause an `SSLHandshakeException` we should briefly discuss how SSL connections differ from non-secure connections in Java.  As it happens, `SSL` (Secure Socket Layer) in general has since been "replaced" with the newer protocol known as `TLS` (Transport Layer Security).  Regardless, many modern code bases, languages, documents, and articles (including this one) continue to refer to this protocol as SSL, since both terms are commonly used interchangeably.

That said, SSL works the same under the hood no matter the language it's being used with.  An SSL connection is established between a client and server using the common practice of [`public-key cryptography`](https://en.wikipedia.org/wiki/Public-key_cryptography).  Essentially, a pair of keys are created that are uniquely linked to one another (through mathematical algorithms).  One key is known only by the server as is known as the `private key`, while the other key is known to everyone, including the client, and is known as the `public key`.  Each key can only perform a transformation of data in one _direction_.  Thus, the `public` key is _only_ capable of `encrypting` data, while the `private` key is _only_ capable of `decrypting` data.  Since only one key is publicly available, and yet both keys are required to successfully transmit, encrypt, and then decrypt data, a secure connection and exchange of information is established, _so long as the client trusts the server_.

This last point is critical and where an `SSL certificate` comes into the picture.  An SSL certificate is a file that combines a `key` with unique information about the organization/site/domain that the certificate represents.  This is why, when you visit a site like [`https://airbrake.io`](https://airbrake.io) you'll typically see a green lock icon in your browser that indicates that the site has a trusted security certificate.  You can even view the details of the certificate.  In Chrome, press `Ctrl + Shift + I`, click `Security`, and then click `View Certificate`.  This dialog shows all the details about the site's certificate, including the `Issued to` field, which shows what specific domain the certificate is valid for.  Most organizations just use their base domain name (e.g. `airbrake.io`), so that the certificate applies to all pages and sub-domains that might be added later.

That said, just _having_ an SSL certificate isn't enough, since that doesn't prove that the site really is trustworthy or is who the browser says it is.  This is where a [`certificate authority`](https://en.wikipedia.org/wiki/Certificate_authority) comes in, which is a third-party that issues SSL certificates.  There are a handful of trusted authorities out there that issue certificates, and these authorities are used to authenticate the signature that is claimed on each SSL certificate they have issued.  For example, the current `airbrake.io` certificate was issued by [`SSL.com`](https://www.ssl.com/).

Alright, so with that out of the way we can explore our Java code sample that attempts to create a simple client/server connection via SSL.  For no particular reason we start with the `SSLServer` class:

```java
// SSLServer.java
package io.airbrake;

import io.airbrake.utility.Logging;

import javax.net.ssl.SSLServerSocketFactory;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.ServerSocket;
import java.net.Socket;
import java.net.SocketTimeoutException;

public class SSLServer implements Runnable {

    private final static int DefaultPort = 24601;
    private int Port = DefaultPort;

    SSLServer() { }

    SSLServer(int port) { Port = port; }

    private static void createServer() {
        createServer(DefaultPort);
    }

    /**
     * Create a socket server at passed port.
     *
     * @param port Port onto which server is socketed.
     */
    private static void createServer(int port) {
        try {
            Logging.lineSeparator(String.format("CREATING SSL SERVER: localhost:%d", port));
            SSLServerSocketFactory factory = (SSLServerSocketFactory) SSLServerSocketFactory.getDefault();
            // Established server socket at port.
            ServerSocket serverSocket = factory.createServerSocket(port);

            while (true) {
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
        } catch (SSLHandshakeException exception) {
            // Output expected SSLHandshakeExceptions.
            Logging.log(exception);
        } catch (IOException exception) {
            // Output unexpected IOExceptions.
            Logging.log(exception, false);
        }
    }

    @Override
    public void run() {
        // Create server instance.
        createServer(Port);
    }
}
```

Most of the logic occurs in the `createServer(int port)` method, where we get the default `SSLServerSocketFactory` instance, then try creating a `ServerSocket` using the passed `port`.  A continuous loop is then used to accept incoming connections, process the incoming information, and output the result to the client.  You may also notice that the `SSLServer` class implements the `Runnable` interface, which includes the `run()` method.  This interface allows us to pass an instance of this class to a new `Thread` instance, so we can create a server in a separate thread.

Next is the `SSLClient` class:

```java
// SSLClient.java
package io.airbrake;

import io.airbrake.utility.Logging;

import javax.net.ssl.SSLSocket;
import javax.net.ssl.SSLSocketFactory;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.SocketTimeoutException;

public class SSLClient implements Runnable {
    private final static String DefaultHost = "localhost";
    private final static int DefaultPort = 24601;
    private final static int DefaultTimeout = 2000;
    private final static boolean DefaultShouldSleep = false;

    private String Host = DefaultHost;
    private int Port = DefaultPort;
    private int Timeout = DefaultTimeout;
    private boolean ShouldSleep = DefaultShouldSleep;

    private String Message;

    SSLClient() {
    }

    public SSLClient(String host) {
        Host = host;
    }

    public SSLClient(int port) {
        Port = port;
    }

    public SSLClient(boolean shouldSleep) {
        ShouldSleep = shouldSleep;
    }

    private void connect() {
        connect(DefaultHost, DefaultPort, DefaultTimeout, DefaultShouldSleep);
    }

    private void connect(String host) {
        connect(host, DefaultPort, DefaultTimeout, DefaultShouldSleep);
    }

    private void connect(int port) {
        connect(DefaultHost, port, DefaultTimeout, DefaultShouldSleep);
    }

    private void connect(boolean shouldSleep) {
        connect(DefaultHost, DefaultPort, DefaultTimeout, shouldSleep);
    }

    /**
     * Attempt SSL connection to passed host and port as client.
     *
     * @param host        Host to connect to.
     * @param port        Port to connect to.
     * @param timeout     Timeout (in milliseconds) to allow for socket connection.
     * @param shouldSleep Indicates if thread should be artificially slept.
     */
    private void connect(String host, int port, int timeout, boolean shouldSleep) {
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
                SSLSocketFactory factory = (SSLSocketFactory) SSLSocketFactory.getDefault();

                SSLSocket socket = (SSLSocket) factory.createSocket(host, port);

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
        } catch (SSLHandshakeException exception) {
            // Output expected SSLHandshakeExceptions.
            Logging.log(exception);
        } catch (InterruptedException | IOException exception) {
            // Output unexpected InterruptedExceptions and IOExceptions.
            Logging.log(exception, false);
        }
    }

    @Override
    public void run() {
        // Attempt connection.
        connect(Host, Port, Timeout, ShouldSleep);
    }
}
```

Since this is a client class we start with the default `SSLSocketFactory`, as opposed to the server version of `SSLServerSocketFactory`.  We then attempt to create and connect to the socket of the specified `host` and `port`, after which we prompt the user for input to pass to the server.  Again, implementation of the `Runnable` interface allows us to execute the `run()` method on a unique thread.

With both the client and server set up we can test things out in our `Main.main(...)` method:

```java
public class Main {

    public static void main(String[] args) throws IOException {
        Thread serverThread = new Thread(new SSLServer());
        serverThread.start();

        Thread clientThread = new Thread(new SSLClient());
        clientThread.start();
    }
}
```

Since both the `SSLServer` and `SSLClient` use infinite loops we need to instantiate each on a separate thread, so we've done so above.  Executing this code produces the following output:

```
-------------- CONNECTING TO localhost:24601 WITH 2000 MS TIMEOUT --------------
--------------------- CREATING SSL SERVER: localhost:24601 ---------------------
[EXPECTED] javax.net.ssl.SSLHandshakeException: Received fatal alert: handshake_failure
[UNEXPECTED] javax.net.ssl.SSLException: Connection has been shutdown: javax.net.ssl.SSLHandshakeException: no cipher suites in common
```

Perhaps unsurprisingly our code threw an `SSLHandshakeException`, indicating that there was a `handshake_failure`.  This is a bit vague, but we can see that the second exception's inner exception says the issue is "no cipher suites in common."  In fact, the issue is that we haven't created or established the `SSL certificate` that the client and server should be referencing!  Normally a certificate is obtained from a trusted certificate authority for a public site, but for testing and development purposes it's common practice to create a `self-signed certificate`, which just means you are the issuer.  These are typically issued to the `localhost` domain, so they'll function locally but not be trusted elsewhere.

Creating a self-signed certificate for Java involves using the [`keytool`](https://docs.oracle.com/javase/8/docs/technotes/tools/unix/keytool.html) command.  We won't go into the full explanation of the following commands, but check out the [official documentation](https://docs.oracle.com/javase/8/docs/technotes/tools/unix/keytool.html) for more details on using `keytool`.  For now, we can start by generating a new key and storing it in a local `keystore` file:

```
$ keytool -genkeypair -alias airbrake -keyalg RSA -validity 7 -keystore keystore
```

Next, we'll export the keystore to a local file:

```
$ keytool -export -alias airbrake -keystore keystore -rfc -file airbrake.cer
```

Now we want to import the certificate into the `truststore`.  The best way to think of the difference between a `keystore` and `truststore` is that the `keystore` is used for `private keys`, while the `truststore` is for public certificates.

```
$ keytool -import -alias airbrake -file airbrake.cer -keystore truststore
```

These files will be generated locally, so we now have `keystore`, `truststore`, and `airbrake.cer`.  We now need to tell our little Java application about these certificates.  There are a number of ways to accomplish this, but for testing purposes we'll just directly inform our application at runtime by setting a few environment variables.

To accomplish this we'll add the following to the `SSLServer` class:

```java
// SSLServer.java
System.setProperty("javax.net.ssl.keyStore", "keystore");
System.setProperty("javax.net.ssl.keyStorePassword", "password");
```

And add this to the `SSLClient` class:

```java
// SSLClient.java
System.setProperty("javax.net.ssl.trustStore", "truststore");
System.setProperty("javax.net.ssl.trustStorePassword", "password");
```

As the property names suggest, we're telling the JVM where it can find the appropriate `keystore` and `truststore`, along with the key passwords for each.  It will then use these property values when an SSL connection is established in the code we've already written.

With our certificate created and stored in the appropriate public and private stores, we can now execute our `Main.main()` method as before, attempting to create a server and client connection.  Doing so now produces the following output:

```
-------------- CONNECTING TO localhost:24601 WITH 2000 MS TIMEOUT --------------
--------------------- CREATING SSL SERVER: localhost:24601 ---------------------
[FROM Server] Enter a message for the server.
Hello there
[TO Server] Hello there
[FROM Client] Hello there
[FROM Server] Enter a message for the server.
exit
------------ DISCONNECTING -------------
[FROM Client] exit
```

Everything works just as expected!  Our server asks for a message, so entering `"Hello there"` sends the message via our SSL connection to the server, which is processed and returned to the client prompting for another message.

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A deep dive into the Java SSLHandshakeException, including fully functional code showing how to create certificates and establish an SSL connection.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html