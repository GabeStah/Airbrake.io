package io.airbrake;

import io.airbrake.utility.Logging;

import javax.net.ssl.HttpsURLConnection;
import java.io.IOException;
import java.net.ConnectException;
import java.net.HttpURLConnection;
import java.net.URISyntaxException;
import java.net.URL;
import java.util.GregorianCalendar;

public class Main {

    public static void main(String[] args) throws IOException, URISyntaxException {
        // Publish book with publication date.
        publishBook(new Book(
                "A Game of Thrones",
                "George R.R. Martin",
                848,
                new GregorianCalendar(1996, 8, 6).getTime()));
        // Publish book without publication date.
        publishBook(new Book(
                "Java Exception Handling - IllegalStateException",
                "Andrew Powell-Morse",
                5));

        // Perform connection test using built-in methods.
        connectionTest();
    }

    private static void publishBook(Book book) {
        try {
            Logging.lineSeparator(book.getTitle().toUpperCase(), 60);
            // Attempt to publish book.
            book.publish();
        } catch (IllegalStateException exception) {
            // Output expected IllegalStateException.
            Logging.log(exception);
        } catch (Throwable throwable) {
            // Output unexpected Throwables.
            Logging.log(throwable, false);
        }
    }

    private static void connectionTest() throws IOException, URISyntaxException {
        try {
            // Test connection to a valid host.
            String uri = "https://www.airbrake.io";
            Logging.lineSeparator(String.format("Connecting to %s", uri), 60);
            HttpURLConnection connection = connect(uri);
            // Attempts to set the ifModifiedSince field.
            connection.setIfModifiedSince(0);
        } catch (IllegalStateException exception) {
            // Output expected IllegalStateException.
            Logging.log(exception);
        } catch (Throwable throwable) {
            // Output unexpected Throwables.
            Logging.log(throwable, false);
        }
    }

    /**
     * Attempt connection to passed URI string.
     *
     * @param uri URI string to connect to.
     *
     * @throws IOException
     * @throws URISyntaxException
     */
    private static HttpURLConnection connect(String uri) throws IOException, URISyntaxException {
        try {
            URL url = new URL(uri);
            HttpsURLConnection connection = (HttpsURLConnection) url.openConnection();
            processResponse(connection);
            return connection;
        } catch (IllegalStateException exception) {
            // Output expected IllegalStateException.
            Logging.log(exception);
        } catch (Throwable throwable) {
            // Output unexpected Throwables.
            Logging.log(throwable, false);
        }
        return null;
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
}
