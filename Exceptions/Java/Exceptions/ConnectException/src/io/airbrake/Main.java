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
