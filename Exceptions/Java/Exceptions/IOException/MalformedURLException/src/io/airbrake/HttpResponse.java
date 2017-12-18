package io.airbrake;

import io.airbrake.utility.Logging;

import javax.net.ssl.HttpsURLConnection;
import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.MalformedURLException;
import java.net.URL;

/**
 * Used to create simple HttpsURLConnections and retrieve the remote response.
 * Factory-based .create() method must be used to instantiate, in order to prevent race conditions.
 */
public class HttpResponse {
    /**
     * Maximum allowed length of response property string.
     */
    private final static int MAX_RESPONSE_LENGTH = 200;

    /**
     * Browser agent to use.
     */
    private String agent = "Chrome/41.0.2228.0";

    /**
     * HTTP Method to use.
     */
    private HttpMethod httpMethod = HttpMethod.GET;

    /**
     * Houses the response value, if applicable.
     */
    private String response = null;

    /**
     * URL to connect to.
     */
    private String url = "https://airbrake.io";

    /**
     * Constructor method.  Is private so instantiation must pass through corresponding .create() factory method.
     */
    private HttpResponse() { }

    /**
     * Constructor method.  Is private so instantiation must pass through corresponding .create() factory method.
     */
    private HttpResponse(HttpMethod httpMethod) {
        this.httpMethod = httpMethod;
    }

    /**
     * Constructor method.  Is private so instantiation must pass through corresponding .create() factory method.
     */
    private HttpResponse(String url) {
        this.url = url;
    }

    /**
     * Constructor method.  Is private so instantiation must pass through corresponding .create() factory method.
     */
    private HttpResponse(String url, HttpMethod httpMethod) {
        this.url = url;
        this.httpMethod = httpMethod;
    }

    /**
     * Initializes actual connection based on assigned properties, and retrieves response code and response value.
     */
    private void initialize() {
        try {
            // Create URL from url property.
            URL url = new URL(this.url);

            // Open a connection to url.
            Logging.log(String.format("Establishing connection to %s", this.url));
            HttpsURLConnection httpsURLConnection = (HttpsURLConnection) url.openConnection();

            // Set the request method to property value.
            Logging.log(String.format("Setting request method to %s", this.httpMethod));
            httpsURLConnection.setRequestMethod(this.httpMethod.toString());

            // Set User-Agent to property value.
            Logging.log(String.format("Setting User-Agent to %s", this.agent));
            httpsURLConnection.setRequestProperty("User-Agent", this.agent);

            // Retrieve response code.
            int responseCode = httpsURLConnection.getResponseCode();
            Logging.log(String.format("Response code %d.", responseCode));

            // If OK/200 response code, proceed.
            if (responseCode == 200) {

                // Pass input stream of connection to reader.
                Logging.log("Retrieving response.");
                BufferedReader responseReader = new BufferedReader(
                    new InputStreamReader(httpsURLConnection.getInputStream())
                );

                int lineCount = 0;
                String responseLine;
                StringBuilder stringBuilder = new StringBuilder();

                // Append each line of response to builder and increment line count.
                while ((responseLine = responseReader.readLine()) != null) {
                    lineCount++;
                    stringBuilder.append(responseLine).append("\n");
                }

                // Close reader.
                responseReader.close();

                // Output line count.
                Logging.log(String.format("Response contained %d lines.", lineCount));

                // Assign response property.
                this.response = stringBuilder.toString();
            }
        } catch (MalformedURLException error) {
            // Output expected MalformedURLExceptions.
            Logging.log(error);
        } catch (Exception exception) {
            // Output unexpected Exceptions.
            Logging.log(exception, false);
        }
    }

    /**
     * Factory method that returns instance of HttpResponse, after initializing, based on passed parameters.
     *
     * @return HttpResponse HttpResponse instance.
     */
    static HttpResponse create() {
        HttpResponse httpResponse = new HttpResponse();
        httpResponse.initialize();
        return httpResponse;
    }

    static HttpResponse create(HttpMethod httpMethod) {
        HttpResponse httpResponse = new HttpResponse(httpMethod);
        httpResponse.initialize();
        return httpResponse;
    }

    static HttpResponse create(String url) {
        HttpResponse httpResponse = new HttpResponse(url);
        httpResponse.initialize();
        return httpResponse;
    }

    static HttpResponse create(String url, HttpMethod httpMethod) {
        HttpResponse httpResponse = new HttpResponse(url, httpMethod);
        httpResponse.initialize();
        return httpResponse;
    }

    /**
     * Get response property value.
     * Value is truncated if it exceeds MAX_RESPONSE_LENGTH.
     *
     * @return String/null Response property value.
     */
    String getResponse() {
        // Ensure response isn't null.
        if (this.response == null) return null;
        // If response length exceeds maximum, return maximum truncation plus ellipses.
        if (this.response.length() > MAX_RESPONSE_LENGTH) {
            return String.format("%s\n...", this.response.substring(0, MAX_RESPONSE_LENGTH));
        } else {
            return this.response;
        }
    }
}
