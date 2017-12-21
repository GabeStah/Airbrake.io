# 307 Temporary Redirect: What It Is and How to Fix It

A **307 Temporary Redirect** message is an [`HTTP response status code`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status) indicating that the requested resource has been _temporarily_ moved to another `URI`, as indicated by the special `Location` header returned within the response.  The `307 Temporary Redirect` code was added to the HTTP standard in HTTP 1.1, as detailed in the [RFC2616](https://tools.ietf.org/html/rfc2616#section-10.3.8) specification document that establishes the standards for that version of HTTP.  As indicated in the RFC, "since the redirection _may_ be altered on occasion, the client _should_ continue to use the Request-URI for future requests."

There are _dozens_ of possible HTTP status codes used to represent the complex relationship between the client, a web application, a web server, and the multitude of third-party web services that may be in use, so determining the cause of a particular HTTP response status code can be difficult.  Since there are so many potential codes, each of which represents a completely different status or event, it can be difficult to differentiate between many of them and determine the exact cause of such errors, including the `307 Temporary Redirect` response code.

Throughout this article we'll explore the `307 Temporary Redirect` code by looking at a handful of troubleshooting tips.  We'll also examine a few useful and easy to implement fixes for common problems that could be causing `307` codes to appear in your own web application.  Let's get down to it!

## The Problem is Server-Side

All HTTP response status codes within the `3xx` category are considered `redirection messages`.  These codes indicate to the user agent (i.e. your web browser) that an additional action is required in order to complete the request and access the desired resource.  The `3xx` response code category is distinctly different from the `5xx` codes category, which encompasses `server error` messages.  For example, the [`502 Bad Gateway`](https://airbrake.io/blog/http-errors/502-bad-gateway-error) error we [looked at](https://airbrake.io/blog/http-errors/502-bad-gateway-error) a few months ago indicates that a server acting as a gateway received and invalid response from a _different_, upstream server.  Thus, while a `5xx` category code indicates an actual problem has occurred on a server, a `3xx` category code, such as `307 Temporary Redirect`, is rarely indicative of an actual _problem_ -- it merely occurs due to the server's behavior or configuration, but is not indicative of an error or bug on the server.

The `307 Temporary Redirect` code may seem familiar to readers that saw our [302 Found: What It Is and How to Fix It](https://airbrake.io/blog/http-errors/302-found) article.  As discussed in that post, the `302` code was actually introduced in HTTP/1.0 standard, as specified in [RFC1945](https://tools.ietf.org/html/rfc1945).  A problem arose shortly thereafter, as many popular user agents (i.e. browsers) actually disregarded the HTTP method that was sent along with the client request.  For example, even if the client request was sent using the `POST` HTTP method, many browsers would automatically send the second request to the temporary `URI` provided in the `Location` header, but would do so using the `GET` HTTP method.  This would often change the conditions under which the request was issued.

To tackle this issue, the HTTP/1.1 standard opted to add the [`303 See Other`](https://airbrake.io/blog/http-errors/303-see-other) response code, which we covered in [this article](https://airbrake.io/blog/http-errors/303-see-other), and the `307 Temporary Redirect` code that we're looking at today.  Both `303` and `307` codes indicate that the requested resource has been _temporarily_ moved, but the key difference between the two is that `303 See Other` indicates that the follow-up request to the new temporary `URI` should be performed using the `GET` HTTP method, while a `307` code indicates that the follow-up request should use the **same** HTTP method of the original request (so `GET` stays `GET`, while `POST` remains `POST`, and so forth).  This is a subtle but critical difference in functionality between the two, so it's important for web developers/admins to account for both scenarios.

That said, the appearance of a `307 Temporary Redirect` is usually not something that requires much user intervention.  All modern browsers will automatically detect the `307 Temporary Redirect` response code and process the redirection action to the new `URI` automatically.  The server sending a `307` code will also include a special `Location` header as part of the response it sends to the client.  This `Location` header indicates the new `URI` where the requested resource can be found.  For example, if an HTTP `POST` method request is sent by the client as an attempt to login at the `https://airbrake.io` URL, the web server may be configured to redirect this `POST` request to a different `URI`, such as `https://airbrake.io/login`.  In this scenario, the server may respond with a `307 Temporary Redirect` code and include the `Location: https://airbrake.io/login` header in the response.  This informs the user agent (browser) that the `POST` request data (login info) was received by the server, but the resource has been temporarily moved to the `Location` header `URI` of `https://airbrake.io/login`.

It's also important to distinguish the purpose and use-cases of the `307 Temporary Redirect` response code from many seemingly similar `3xx` codes, such as the [`301 Moved Permanently`](https://airbrake.io/blog/http-errors/301-moved-permanently) we [looked at last month](https://airbrake.io/blog/http-errors/301-moved-permanently).  Specifically, the `307 Found` code informs the client that the passed `Location` `URI` is only a _temporary_ resource, and that all future requests should continue to access the originally requested `URI`.  On the other hand, the `301 Moved Permanently` message is not temporary, and indicates that passed `Location` `URI` should be used for future (identical) requests.

Additionally, since the `307 Temporary Redirect` indicates that something has gone wrong within the `server` of your application, we can largely disregard the `client` side of things.  If you're trying to diagnose an issue with your own application, you can immediately ignore most client-side code and components, such as HTML, cascading style sheets (CSS), client-side JavaScript, and so forth.  This doesn't apply _solely_ to web sites, either.  Many smart phone apps that have a modern looking user interface are actually powered by a normal web application behind the scenes; one that is simply hidden from the user.  If you're using such an application and a `307 Temporary Redirect` occurs, the issue isn't going to be related to the app installed on your phone or local testing device.  Instead, it will be something on the server-side, which is performing most of the logic and processing behind the scenes, outside the purview of the local interface presented to the user.

If your application is generating unexpected `307 Temporary Redirect` response codes there are a number of steps you can take to diagnose the problem, so we'll explore a few potential work around below.

## Start With a Thorough Application Backup

As with anything, it's better to have played it safe at the start than to screw something up and come to regret it later on down the road.  As such, it is _critical_ that you perform a full backup of your application, database, and so forth, before attempting any fixes or changes to the system.  Even better, if you have the capability, create a complete copy of the application onto a secondary `staging` server that isn't "live," or isn't otherwise active and available to the public.  This will give you a clean testing ground with which to test all potential fixes to resolve the issue, without threatening the security or sanctity of your live application.

## Diagnosing a 307 Temporary Redirect Response Code

A `307 Temporary Redirect` response code indicates that the requested resource can be found at the new `URI` specified in the `Location` response header, but _only_ temporarily.  However, the appearance of this error itself may be erroneous, as it's entirely possible that the server is misconfigured, which could cause it to improperly respond with `307 Temporary Redirect` codes, instead of the standard and expected `200 OK` code seen for most successful requests.  Thus, a large part of diagnosing the issue will be going through the process of double-checking what resources/URLs are generating `307 Temporary Redirect` response codes and determining if these codes are appropriate or not.

If your application is responding with `307 Temporary Redirect` codes that it _should not_ be issuing, this is a problem that many other visitors may be experiencing as well, dramatically hindering your application's ability to service users.  We'll go over some troubleshooting tips and tricks to help you try to resolve this issue.  If nothing here works, don't forget to try Googling for the answer.  Search for specific terms related to your issue, such as the name of your application's CMS or web server software, along with `307 Temporary Redirect`.  Chances are you'll find others who have experienced this issue and have (hopefully) found a solution.

## Troubleshooting on the Server-Side

Here are some additional tips to help you troubleshoot what might be causing the `307 Temporary Redirect` to appear on the server-side of things:

### Confirm Your Server Configuration

Your application is likely running on a server that is using one of the two most popular web server softwares, `Apache` or `nginx`.  At the time of publication, both of these web servers make up [`over 84%` of the world's web server software](https://w3techs.com/technologies/overview/web_server/all)!  Thus, one of the first steps you can take to determine what might be causing these `307 Temporary Redirect` response codes is to check the configuration files for your web server software for unintentional redirect instructions.

To determine which web server your application is using you'll want to look for a key file.  If your web server is Apache then look for an `.htaccess` file within the root directory of your website file system.  For example, if your application is on a shared host you'll likely have a username associated with the hosting account.  In such a case, the application root directory is typically found at the path of `/home/<username>/public_html/`, so the `.htaccess` file would be at `/home/<username>/public_html/.htaccess`.

If you located the `.htaccess` file then open it in a text editor and look for lines that use `RewriteXXX` directives, which are part of the [`mod_rewrite`](http://httpd.apache.org/docs/current/mod/mod_rewrite.html) module in Apache.  Covering exactly how these rules work is well beyond the scope of this article, however, the basic concept is that a `RewriteCond` directive defines a text-based pattern that will be matched against entered URLs.  If a matching URL is requested by a visitor to the site, the `RewriteRule` directive that follows one or more `RewriteCond` directives is used to perform the actual redirection of the request to the appropriate URL.

For example, here is a simple [`RewriteCond`](http://httpd.apache.org/docs/current/mod/mod_rewrite.html#rewritecond) and `RewriteRule` combination that matches all incoming requests to `airbrake.io` using the HTTP `POST` method, and redirecting them to `https://airbrake.io/login` via a `307 Temporary Redirect` response:

```
RewriteEngine on
RewriteCond %{HTTP_HOST} ^airbrake.io$
RewriteCond %{REQUEST_METHOD} POST
RewriteRule ^(.*)$ http://airbrake.io/login$1 [R=307]
```

Notice the extra flag at the end of the `RewriteRule`, which explicitly states that the response code should be `307`, indicating to user agents that the request should be repeated to the specified `URI`, but while retaining the original HTTP method (`POST`, in this case).  Thus, if you find any strange `RewriteCond` or `RewriteRule` directives in the `.htaccess` file that don't seem to belong, try temporarily commenting them out (using the `#` character prefix) and restarting your web server to see if this resolves the issue.

On the other hand, if your server is running on `nginx`, you'll need to look for a completely different configuration file.  By default this file is named `nginx.conf` and is located in one of a few common directories: `/usr/local/nginx/conf`, `/etc/nginx`, or `/usr/local/etc/nginx`.  Once located, open `nginx.conf` in a text editor and look for `return` or `rewrite` directives that are using the `307` response code flag.  For example, here is a simple `block directive` (i.e. a named set of directives) that configures a virtual server by creating a redirection from `airbrake.io` to `airbrake.io/login` for both `POSt` and `GET` HTTP method requests:

```
server {
    listen 80;
    listen 443 ssl;
    server_name airbrake.io;
    if ($request_method = GET) {
        return 303 https://airbrake.io/login$request_uri;
    }
    if ($request_method = POST) {
        return 307 https://airbrake.io/login$request_uri;
    }    
}
```

`Return` directives in `nginx` are similar to the `RewriteCond` and `RewriteRule` directives found in `Apache`, as they tend to contain more complex text-based patterns for searching.  Either way, look through your `nginx.conf` file for any abnormal `return` or `rewrite` directives that include the `307` flag.  Comment out any abnormalities before restarting the server to see if the issue was resolved.

### Scour the Logs

Nearly every web application will keep some form of server-side logs.  `Application logs` are typically the history of what the application did, such as which pages were requested, which servers it connected to, which database results it provides, and so forth.  `Server logs` are related to the actual hardware that is running the application, and will often provide details about the health and status of all connected services, or even just the server itself.  Google "logs [PLATFORM_NAME]" if you're using a CMS, or "logs [PROGRAMMING_LANGUAGE]" and "logs [OPERATING_SYSTEM]" if you're running a custom application, to get more information on finding the logs in question.

### Debug Your Application Code

If all else fails, it may be that a problem in some custom code within your application is causing the issue.  Try to diagnose where the issue may be coming from through manually debugging your application, along with parsing through application and server logs.  Ideally, make a copy of the entire application to a local development machine and perform a step-by-step debug process, which will allow you to recreate the exact scenario in which the `307 Temporary Redirect` occurred and view the application code at the moment something goes wrong.

No matter what the cause, the appearance of a `307 Temporary Redirect` within your own web application is a strong indication that you may need an error management tool to help you automatically detect such errors in the future.  The best of these tools can even alert you and your team _immediately_ when an error occurs.  <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-307-temporary-redirect">Airbrake's error monitoring software</a> provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize exception parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-307-temporary-redirect">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look at the 307 Temporary Redirect response code, including troubleshooting tips to help you resolve this error in your own application.

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/HTTP/Status
- https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/307
- https://en.wikipedia.org/wiki/List_of_HTTP_status_codes
- https://en.wikipedia.org/wiki/HTTP_307
- http://www.checkupdown.com/status/E307.html
- https://tools.ietf.org/html/rfc1945
- https://tools.ietf.org/html/rfc7231
- https://tools.ietf.org/html/rfc2616