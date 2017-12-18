# Java Exception Handling - MalformedURLException

Moving along through the in-depth [__Java Exception Handling__](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) series we've been going through, we next arrive at the **MalformedURLException**.  A `MalformedURLException` is thrown when the built-in [`URL`](https://docs.oracle.com/javase/8/docs/api/java/net/URL.html) class encounters an invalid URL; specifically, when the `protocol` that is provided is missing or invalid.

In today's article we'll examine the `MalformedURLException` in more detail by looking at where it resides in the overall [Java Exception Hierarchy](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy).  We'll also take a look at some fully functional code samples that illustrate how a basic HTTP connection might be established using the `URL` class, and how passing improper URL values to it can result in `MalformedURLExceptions`, so let's get started!

## The Technical Rundown

All Java errors implement the [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy) interface, or are extended from another inherited class therein.  The full exception hierarchy of this error is:

- [`java.lang.Object`](https://docs.oracle.com/javase/8/docs/api/java/lang/Object.html)
    - [`java.lang.Throwable`](https://airbrake.io/blog/java-exception-handling/the-java-exception-class-hierarchy)
        - [`java.lang.Exception`](https://docs.oracle.com/javase/8/docs/api/java/lang/Exception.html)
            - [`java.io.IOException`](https://docs.oracle.com/javase/8/docs/api/java/io/IOException.html)
                - `java.net.MalformedURLException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```java
package io.airbrake;

import io.airbrake.utility.Logging;

public class Main {
    public static void main(String[] args) {
        Logging.lineSeparator("https://airbrake.io");
        HttpResponse httpResponse = HttpResponse.create("https://airbrake.io");
        Logging.log(httpResponse.getResponse());

        Logging.lineSeparator("htps://airbrake.io");
        HttpResponse httpResponse2 = HttpResponse.create("htps://airbrake.io");
        Logging.log(httpResponse2.getResponse());

        Logging.lineSeparator("https://airbrakeio");
        HttpResponse httpResponse3 = HttpResponse.create("https://airbrakeio");
        Logging.log(httpResponse3.getResponse());

        Logging.lineSeparator("https://airbrake,io");
        HttpResponse httpResponse4 = HttpResponse.create("https://airbrake,io");
        Logging.log(httpResponse4.getResponse());
    }
}
```

```java
package io.airbrake;

/**
 * HTTP methods to be passed to connection classes/methods.
 */
public enum HttpMethod {
    DELETE,
    GET,
    HEAD,
    OPTIONS,
    PATCH,
    POST,
    PUT,
    TRACE
}
```

```java
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
```

This code sample also uses the [`Logging`](https://github.com/GabeStah/Airbrake.io/blob/master/lib/java/src/io/airbrake/utility/Logging.java) utility class, the source of which can be [found here on GitHub](https://github.com/GabeStah/Airbrake.io/blob/master/lib/java/src/io/airbrake/utility/Logging.java).

## When Should You Use It?

As the [official documentation](https://docs.oracle.com/javase/8/docs/api/java/net/MalformedURLException.html) states, a `MalformedURLException` is thrown either when "no legal protocol could be found in a specification string," or when "the string could not be parsed."  In practice, modern Java versions have particularly robust string parsing capabilities, so the vast majority of `MalformedURLException` occurrences will be due to the former problem: an invalid or unrecognized `protocol`.  The parlance used in Java (and elsewhere) for terms like `protocol` can be a little confusing, so we'll briefly refresh ourselves on what this means in the realm of URLs.

To begin with, the broader term for identifiers of network resources is "Unified Resource Identifier", or `URI` as it's more commonly known.  A `URI` is merely a string of characters that identify a resource over a network.  Uniformed Resource Locators, or `URLs`, are a _type_ of `URI`.  A `URL` is unique in that it typically includes multiple forms of information about accessing the resource, including the `protocol`, the `domain`, and the `resource` to be requested.  For example, in the `URL` `https://airbrake.io/blog/` the specified `protocol` is `https`, the `domain` is `airbrake.io`, and the `resource` is `/blog/`.

It's worth noting that the `protocol` section is often referred to as the `scheme` in the parent `URI` object type, because a `URI` can contain many different types of `schemes` to indicate how the resource should be accessed.  For example, a `scheme` of `https` is common for `URLs`, but a `scheme` value of `file` might be used to access a local file resource.  Moreover, many custom `URI` `schemes` exist, which allow applications to perform special actions and open resources on behalf of other applications.  For example, the popular [`Steam`](https://steampowered.com) online gaming platform will recognize and open `URIs` with a `scheme` of `steam`, as shown in the [developer documentation](https://developer.valvesoftware.com/wiki/Steam_browser_protocol).

All that said, since the `MalformedURLException` in Java is typically thrown via the `URL` class, it stands to reason that the `URL` class refers to the `scheme` portion of the `URL` as the `protocol`.  To illustrate this in action we begin with a simple `HttpMethod` enumeration:

```java
package io.airbrake;

/**
 * HTTP methods to be passed to connection classes/methods.
 */
public enum HttpMethod {
    DELETE,
    GET,
    HEAD,
    OPTIONS,
    PATCH,
    POST,
    PUT,
    TRACE
}
```

This will come in handy in a moment, when we need an easy way to specify the HTTP method we'll be using for our connection.  Speaking of which, we've also created a custom `HttpResponse` class to help simplify the creation and connection process, in order to ultimately retrieve a response from a particular `URL` that we wish to connect to:

```java
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
```

I've opted to use a [`Factory Design Pattern`](https://airbrake.io/blog/design-patterns/factory), which we explored in our [Creational Design Patterns: Factory Method](https://airbrake.io/blog/design-patterns/factory) article, which allows us to execute a sub-method after instantiating a new `HttpResponse` object.  Specifically, we want to execute the `initialize()` method after the `constructor` completes, but we also need a way to avoid race conditions, which is where we might get unexpected results if our code is performing multiple connections simultaneously, and where our code could behave differently depending on the order of execution.  By avoiding race conditions via the factory method pattern, we ensure that execution always occurs in the order we require.

Thus, client code wishing to create a new `HttpResponse` instance can only access the `create()` method overloads, which creates a new `HttpResponse` instance via the `private` constructor, then invokes the `initialize()` method, before finally returning the generated `HttpResponse` object.  The `initialize()` method is where we perform the actual connection attempt and retrieve the response, so we start by creating a new `URL` instance from the `url` property, then open an `HTTPS` connection to that `url`.  We then set the request method using the string-converted `httpMethod` property, which is where the `HttpMethod` `enum` comes into play.  Next, we retrieve the response code and, if it's valid (`200`) we process the response by appending each line to an output and assigning the response to the `response` property.

As a result, client code that invokes the `create()` method can then immediately call the `getResponse()` method, which retrieves the formatted (or `null`) `response` property value of the `HttpResponse` instance:

```java
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
```

To test this out we'll create a handful of `HttpResponse` instances, passing slightly different `URL` string arguments each time:

```java
package io.airbrake;

import io.airbrake.utility.Logging;

public class Main {
    public static void main(String[] args) {
        Logging.lineSeparator("https://airbrake.io");
        HttpResponse httpResponse = HttpResponse.create("https://airbrake.io");
        Logging.log(httpResponse.getResponse());

        Logging.lineSeparator("htps://airbrake.io");
        HttpResponse httpResponse2 = HttpResponse.create("htps://airbrake.io");
        Logging.log(httpResponse2.getResponse());

        Logging.lineSeparator("https://airbrakeio");
        HttpResponse httpResponse3 = HttpResponse.create("https://airbrakeio");
        Logging.log(httpResponse3.getResponse());

        Logging.lineSeparator("https://airbrake,io");
        HttpResponse httpResponse4 = HttpResponse.create("https://airbrake,io");
        Logging.log(httpResponse4.getResponse());
    }
}
```

The first test produces the following output:

```
--------- https://airbrake.io ----------
Establishing connection to https://airbrake.io
Setting request method to GET
Setting User-Agent to Chrome/41.0.2228.0
Response code 200.
Retrieving response.
Response contained 484 lines.
<!DOCTYPE html>
<html>
<head lang='en'>
  <meta name='viewport' content='width=device-width, initial-scale=1.0, maximum-scale=1'>
  <meta charset='utf-8'>

  <title>Error Monitoring and Detection Soft
...
```

Here we can see that everything appears to be working.  We start by establishing a connection to `https://airbrake.io`, set the request method to `GET`, set the `User-Agent` to tha latest Chrome agent string, and get a `response code` of `200`.  The full response contains `484` lines, so we truncate that result down to the maximum of `200` characters, as specified in the `HttpResponse.MAX_RESPONSE_LENGTH` property.

Next, let's try another test to connect to the slightly incorrect `htps://airbrake.io` `URL`:

```
---------- htps://airbrake.io ----------
[EXPECTED] java.net.MalformedURLException: unknown protocol: htps
```

Immediately we get a `MalformedURLException`, indicating that the `protocol` value at the start of our `URL` is invalid.  As previously discussed, `URLs` are a sub-classification of `URIs`, so while a `URI` can have a wide range of standard and custom `scheme` values, a `URL` can only have a select handful of accepted `protocol` values, and `htps` is not one of them.

Our third test attempts to connect to `https://airbrakeio`:

```
---------- https://airbrakeio ----------
Establishing connection to https://airbrakeio
Setting request method to GET
Setting User-Agent to Chrome/41.0.2228.0
[UNEXPECTED] java.net.UnknownHostException: airbrakeio
```

Here we see everything works and no `MalformedURLException` is thrown, but we actually get an `UnknownHostException` instead.  This is because, while `https://airbrakeio` _is_ a perfectly valid form of `URL`, it isn't a recognized `host`.  The same is true when we try to use an invalid character in our `host` name (`,`):

```
--------- https://airbrake,io ----------
Establishing connection to https://airbrake,io
Setting request method to GET
Setting User-Agent to Chrome/41.0.2228.0
[UNEXPECTED] java.net.UnknownHostException: airbrake,io
```

The <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java library</a> provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with Airbrake's state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features <a class="js-cta-utm" href="https://airbrake.io/languages/java_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-java">Airbrake-Java</a> has to offer and see for yourself why so many of the world's best engineering teams are using Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look at the Java MalformedURLException, including functional code samples illustrating how to establish simple HTTPS connections.

---

__SOURCES__

- https://docs.oracle.com/javase/8/docs/api/java/lang/Throwable.html