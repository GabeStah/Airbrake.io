# Java Exception Handling - ConnectException

Moving along through our detailed [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series, today we'll dive into the ConnectException in Java.  As you can probably guess, the `ConnectException` indicates an error has occurred while attempting to connect a socket to a remote address, such as when trying to access a URL that doesn't exist, or which refuses outside connections.

In this article we'll examine the `ConnectException` in more detail, looking at where it sits in the [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).  We'll also go over a bit of functional sample code that aims to illustrate how `ConnectExceptions` are typically thrown using built-in libraries, and also how you might throw your own instances when necessary, so let's get to it!

## The Technical Rundown

All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy)
    - [`java.lang.Exception`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html)
        - [`java.io.IOException`](https://docs.oracle.com/javase/8/docs/api/java/io/IOException.html)
            - [`java.net.SocketException`](https://docs.oracle.com/javase/8/docs/api/java/net/SocketException.html)
                - `ConnectException`

- [`java.lang.Exception`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html) inherits from [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).
- [`java.io.IOException`](https://docs.oracle.com/javase/8/docs/api/java/io/IOException.html) inherits from [`java.lang.Exception`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html).
- [`java.net.SocketException`](https://docs.oracle.com/javase/8/docs/api/java/net/SocketException.html) then inherits from [`java.io.IOException`](https://docs.oracle.com/javase/8/docs/api/java/io/IOException.html).
- Lastly, `ConnectException` inherits from [`java.net.SocketException`](https://docs.oracle.com/javase/8/docs/api/java/net/SocketException.html).

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```java
package io.airbrake;

import io.airbrake.utility.Logging;
import javax.net.ssl.HttpsURLConnection;
import java.io.IOException;
import java.net.*;

public class Main {

    public static void main(String[] args) throws IOException, URISyntaxException {
	    // Test connection to a valid host.
        String uri = "https://www.airbrake.io";
        Logging.lineSeparator(String.format("Connecting to %s", uri), 60);
        connect(uri);

        // Test connection to an invalid host.
        uri = "https://www.brakeair.io";
        Logging.lineSeparator(String.format("Connecting to %s", uri), 60);
        connect(uri);
    }

    /**
     * Attempt connection to passed URI string.
     *
     * @param uri URI string to connect to.
     *
     * @throws IOException
     * @throws URISyntaxException
     */
    private static void connect(String uri) throws IOException, URISyntaxException {
        try {
            URL url = new URL(uri);
            HttpsURLConnection connection = (HttpsURLConnection) url.openConnection();
            processResponse(connection);
        } catch (ConnectException exception) {
            // Output expected ConnectException.
            Logging.log(exception);
        } catch (Throwable throwable) {
            // Output unexpected Throwables.
            Logging.log(throwable, false);
        }
    }

    /**
     * Logs connection information.
     *
     * @param connection Connection to be logged.
     *
     * @throws IOException
     * @throws URISyntaxException
     */
    private static void logConnection(final HttpURLConnection connection) throws IOException, URISyntaxException {
        int code = connection.getResponseCode();
        String message = connection.getResponseMessage();
        String url = connection.getURL().toURI().toString();

        Logging.log(String.format("Response from %s - Code: %d, Message: %s", url, code, message));
    }

    /**
     * Process an HttpURLConnection response information.
     *
     * @param connection Connection to be processed.
     *
     * @throws IOException
     * @throws URISyntaxException
     */
    private static void processResponse(final HttpURLConnection connection) throws IOException, URISyntaxException {
        try {
            logConnection(connection);
        } catch (ConnectException exception) {
            // Output expected ConnectException.
            Logging.log(exception);
        } catch (Throwable throwable) {
            // Output unexpected Throwables.
            Logging.log(throwable, false);
        }
    }

    /**
     * Process an HttpURLConnection response information.
     * Outputs better-formatted ConnectException message.
     *
     * @param connection Connection to be processed.
     *
     * @throws IOException
     * @throws URISyntaxException
     */
    private static void processResponseFormatted(final HttpURLConnection connection) throws IOException, URISyntaxException {
        try {
            logConnection(connection);
        } catch (ConnectException exception) {
            if (exception.getMessage().equals("Connection refused: connect")) {
                throw new ConnectException(String.format("Connection to %s was refused with response code %d", connection.getURL().toString(), connection.getResponseCode()));
            }
        } catch (Throwable throwable) {
            // Output unexpected Throwables.
            Logging.log(throwable, false);
        }
    }
}

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

Since the `ConnectException` is only used when dealing with remote connections, we'll jump right into our code, which attempts to create a basic connection through the [`HttpURLConnection`](https://docs.oracle.com/javase/8/docs/api/java/net/HttpURLConnection.html) class (technically we're using the `HttpsURLConnection` class, since this is a secure connection, but this class inherits from `HttpURLConnection`, so that's what parameter type our method signatures declare).

We start with the `connect(String uri`) method, which creates a new `URI` instance and then tries to establish a connection via the `openConnection()` method:

```java
/**
* Attempt connection to passed URI string.
*
* @param uri URI string to connect to.
*
* @throws IOException
* @throws URISyntaxException
*/
private static void connect(String uri) throws IOException, URISyntaxException {
    try {
        URL url = new URL(uri);
        HttpsURLConnection connection = (HttpsURLConnection) url.openConnection();
        processResponse(connection);
    } catch (ConnectException exception) {
        // Output expected ConnectException.
        Logging.log(exception);
    } catch (Throwable throwable) {
        // Output unexpected Throwables.
        Logging.log(throwable, false);
    }
}
```

As you can see, we perform processing on that `connection` object in the `processResponse(HttpURLConnection connection)` method:

```java
/**
* Process an HttpURLConnection response information.
*
* @param connection Connection to be processed.
*
* @throws IOException
* @throws URISyntaxException
*/
private static void processResponse(final HttpURLConnection connection) throws IOException, URISyntaxException {
    try {
        logConnection(connection);
    } catch (ConnectException exception) {
        // Output expected ConnectException.
        Logging.log(exception);
    } catch (Throwable throwable) {
        // Output unexpected Throwables.
        Logging.log(throwable, false);
    }
}
```

Right now this is just an intermediary method, since it merely passes the `connection` parameter along to the final method in the chain.  However, we include this to show where we might perform our logic based on the `connection` object properties and responses that were provided.  Finally, we call the `logConnection(HttpURLConnection connection)` method, which nicely formats a bit of basic information about the connection and outputs it to the log:

```java
/**
* Logs connection information.
*
* @param connection Connection to be logged.
*
* @throws IOException
* @throws URISyntaxException
*/
private static void logConnection(final HttpURLConnection connection) throws IOException, URISyntaxException {
    int code = connection.getResponseCode();
    String message = connection.getResponseMessage();
    String url = connection.getURL().toURI().toString();

    Logging.log(String.format("Response from %s - Code: %d, Message: %s", url, code, message));
}
```

With everything in place let's try connecting to a few different addresses and see what happens:

```java
public static void main(String[] args) throws IOException, URISyntaxException {
    // Test connection to a valid host.
    String uri = "https://www.airbrake.io";
    Logging.lineSeparator(String.format("Connecting to %s", uri), 60);
    connect(uri);

    // Test connection to an invalid host.
    uri = "https://www.brakeair.io";
    Logging.lineSeparator(String.format("Connecting to %s", uri), 60);
    connect(uri);
}
```

We start with [`https://www.airbrake.io`](https://www.airbrake.io), which should work just fine.  Sure enough, the log output of the first call shows a connection was established and an `OK / 200` response code was returned:

```
---------- Connecting to https://www.airbrake.io -----------
Response from https://airbrake.io/ - Code: 200, Message: OK
```

However, when we try our second call using the URI for `brakeair.io` -- a (currently) non-existent address -- our connection attempt throws a `ConnectException` at us:

```
---------- Connecting to https://www.brakeair.io -----------
[EXPECTED] java.net.ConnectException: Connection refused: connect
```

Quick and easy.  That's the usage of the `ConnectException` in a nutshell.  However, it's briefly worth noting that the actual error message content that we see above isn't all that revealing.  Therefore, in some cases, it might be worthwhile to throw a `ConnectException` yourself with a more detailed error message.  For example, here we've modified the `processResponse(HttpURLConnection connection)` method a bit:

```java
/**
* Process an HttpURLConnection response information.
* Outputs better-formatted ConnectException message.
*
* @param connection Connection to be processed.
*
* @throws IOException
* @throws URISyntaxException
*/
private static void processResponseFormatted(final HttpURLConnection connection) throws IOException, URISyntaxException {
    try {
        logConnection(connection);
    } catch (ConnectException exception) {
        if (exception.getMessage().equals("Connection refused: connect")) {
            throw new ConnectException(String.format("Connection to %s was refused with response code %d", connection.getURL().toString(), connection.getResponseCode()));
        }
    } catch (Throwable throwable) {
        // Output unexpected Throwables.
        Logging.log(throwable, false);
    }
}
```

As you can see, we explicitly check if a `ConnectException` is caught that has an error message of `"Connection refused: connect"`.  If such an exception is caught we throw a new `ConnectException` with a more detailed message.

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look at the the Java ConnectException, with sample code illustrating how to handle such exceptions when connecting via HttpURLConnection.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html