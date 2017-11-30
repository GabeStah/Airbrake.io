# 302 Found: What It Is and How to Fix It

A **302 Found** message is an [`HTTP response status code`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status) indicating that the requested resource has been temporarily moved to a different `URI`.  Since the location or current redirection directive might be changed in the future, a client that receives a `302 Found` response code should continue to use the original URI for future requests.

Unfortunately, it can be difficult to differentiate between all the possible HTTP response codes and determine the exact cause of errors like the `302 Found` code.  There are _dozens_ of possible HTTP status codes used to represent the complex relationship between the client, a web application, a web server, and the multitude of third-party web services that may be in use, so determining the cause of a particular status code can be challenging.  In this article we'll examine the `302 Found` code by looking at a few troubleshooting tips, along with some potential fixes for common problems that might be causing this issue within your own web applications, so let's get to it!

## The Problem is Server-Side

All HTTP response status codes that are in the `3xx` category are considered `redirection messages`.  Such codes indicate to the user agent (i.e. your web browser) that an additional action is required in order to complete the request and access the desired resource.  Unlike `client error responses` found in the `4xx` codes, like the [`404 Not Found Error`](https://airbrake.io/blog/http-errors/404-not-found-error) we explored recently, which could stem from either a client- or server-side issue, `302 Found` code generally indicates an issue on the actual web server hosting your application.

That said, the appearance of a `302 Found` is usually not something that requires much user intervention.  All modern browsers will automatically detect the `302 Found` response code and process the temporarily redirection action automatically.  The web server hosting the application will typically include a special `Location` header as part of the response it sends to the client.  This `Location` header indicates the new URL where the requested resource can be found.  For example, if a request comes in to access the URL `http://airbrake.io`, but the web server is configured to force redirection to a secure version using `https`, the server response will include the `Location: https://airbrake.io` header.  This tells the browser that it should redirect _this single request_ to `http://airbrake.io` to the secured URL of `https://airbrake.io`.  In most cases, the browser will automatically detect this `302 Found` response code, read the new `Location` URL, and redirect the request to that new location.

However, it's important to keep in mind that the `302 Found` response code is used for completely different purposes than the [`301 Moved Permanently`](https://airbrake.io/blog/http-errors/301-moved-permanently) we looked at last week.  In the case of the former, the redirection should be temporary (i.e. just for the current, single request) -- the browser should "remember" the original URI and process any future requests using that same URI.  However, with a `301 Moved Permanently` code the browser is being told that the original URI is no longer valid, so all future requests should use the new URI provided by the `Location` header.

Since the `302 Found` indicates that something has gone wrong within the `server` of your application, we can largely disregard the `client` side of things.  If you're trying to diagnose an issue with your own application, you can immediately ignore most client-side code and components, such as HTML, cascading style sheets (CSS), client-side JavaScript, and so forth.  This doesn't apply _solely_ to web sites, either.  Many smart phone apps that have a modern looking user interface are actually powered by a normal web application behind the scenes; one that is simply hidden from the user.  If you're using such an application and a `302 Found` occurs, the issue isn't going to be related to the app installed on your phone or local testing device.  Instead, it will be something on the server-side, which is performing most of the logic and processing behind the scenes, outside the purview of the local interface presented to the user.

If your application is generating unexpected `302 Found` response codes there are a number of steps you can take to diagnose the problem.

## Start With a Thorough Application Backup

As with anything, it's better to have played it safe at the start than to screw something up and come to regret it later on down the road.  As such, it is _critical_ that you perform a full backup of your application, database, and so forth, before attempting any fixes or changes to the system.  Even better, if you have the capability, create a complete copy of the application onto a secondary `staging` server that isn't "live," or isn't otherwise active and available to the public.  This will give you a clean testing ground with which to test all potential fixes to resolve the issue, without threatening the security or sanctity of your live application.

## Diagnosing a 302 Found Response Code

A `302 Found` response code indicates that the requested resource should temporarily be accessed at a different URI.  However, it's entirely possible that the server is misconfigured, causing it to improperly respond with `302 Found` codes, instead of the standard and expected `200 OK` code of a normal, functional request.  Thus, a large part of diagnosing the issue will be going through the process of double-checking what resources/URLs are generating `302 Found` response codes and determining if these codes are appropriate or not.

If your application is responding with `302 Found` codes that it _should not_ be issuing, this is an issue that many other visitors may be experiencing as well, dramatically hindering your application's ability to service users.  We'll go over some troubleshooting tips and tricks to help you try to resolve this issue.  If nothing here works, don't forget that Google is your friend.  Try searching for specific terms related to your issue, such as the name of your application's CMS or web server software, along with `302 Found`.  Chances are you'll find others who have experienced this issue and have found a solution.

## Troubleshooting on the Server-Side

Here are some additional tips to help you troubleshoot what might be causing the `302 Found` to appear on the server-side of things:

### Confirm Your Server Configuration

Your application is likely running on a server that is using one of the two most popular web server softwares, `Apache` or `nginx`.  At the time of publication, both of these web servers make up [`over 84%` of the world's web server software](https://w3techs.com/technologies/overview/web_server/all)!  Thus, one of the first steps you can take to determine what might be causing these `302 Found` response codes is to check the configuration files for your web server software for unintentional redirect instructions.

To determine which web server your application is using you'll want to look for a key file.  If your web server is Apache then look for an `.htaccess` file within the root directory of your website file system.  For example, if your application is on a shared host you'll likely have a username associated with the hosting account.  In such a case, the application root directory is typically found at the path of `/home/<username>/public_html/`, so the `.htaccess` file would be at `/home/<username>/public_html/.htaccess`.

If you located the `.htaccess` file then open it in a text editor and look for lines that use `RewriteXXX` directives, which are part of the [`mod_rewrite`](http://httpd.apache.org/docs/current/mod/mod_rewrite.html) module in Apache.  Covering exactly how these rules work is well beyond the scope of this article, however, the basic concept is that a `RewriteCond` directive defines a text-based pattern that will be matched against entered URLs.  If a matching URL is requested by a visitor to the site, the `RewriteRule` directive that follows one or more `RewriteCond` directives is used to perform the actual redirection of the request to the appropriate URL.

For example, here is a simple `RewriteCond` and `RewriteRule` combination that matches all incoming requests to `example.com` and establishes a _temporarily_ redirection to that same URI on the `temporary-example.com` domain instead:

```
RewriteEngine on
RewriteCond %{HTTP_HOST} ^example\.com$
RewriteRule ^(.*)$ http://www.temporary-example.com/$1 [R=302]
```

Notice the extra flag at the end of the `RewriteRule`, which explicitly states that the response code should be `302`, indicating to user agents (browsers) that this is a temporary redirect.  Thus, if you find any strange `RewriteCond` or `RewriteRule` directives in the `.htaccess` file that don't seem to belong, try temporarily commenting them out (using the `#` character prefix) and restarting your web server to see if this resolves the issue.

On the other hand, if your server is running on `nginx`, you'll need to look for a completely different configuration file.  By default this file is named `nginx.conf` and is located in one of a few common directories: `/usr/local/nginx/conf`, `/etc/nginx`, or `/usr/local/etc/nginx`.  Once located, open `nginx.conf` in a text editor and look for `rewrite` directives that are using the `redirect` flag.  For example, here is a simple `block directive` (i.e. a named set of directives) that configures a virtual server by creating a temporary redirection from `example.com` to the `temporary-example.com`:

```
server {
    listen 80;
    listen 443 ssl;
    server_name www.example.com;
    rewrite ^/$ http://www.temporary-example.com redirect;
}
```

`Rewrite` directives in `nginx` are similar to the `RewriteCond` and `RewriteRule` directives found in `Apache`, as they tend to contain more complex text-based patterns for searching.  Either way, look through your `nginx.conf` file for any abnormal `rewrite` directives that include the `redirect` flag (the alternative `permanent` flag will issue `301` response codes instead).  Comment out any abnormalities before restarting the server to see if the issue was resolved.

### Check for Outdated Software

The RFC specification document for [HTTP 1.0](https://tools.ietf.org/html/rfc1945) stated that the purpose of a `302 Found` response code was to indicate the client should perform a temporary redirection.  However, many newer browsers would process a `302` code received via a `POST` request as an erroneous `GET` request instead, which caused problems and confusion for some web server softwares trying to get browsers to perform the proper task when temporary redirection was required.  To resolve this the [HTTP 1.1](https://tools.ietf.org/html/rfc7231#section-6.4.3) RFC specification document added the `303 See Other` and `307 Temporary Redirect` response codes, which are _explicit_ means of handling `POST-to-GET` and temporary direct responses, respectively.

### Scour the Logs

Nearly every web application will keep some form of server-side logs.  `Application logs` are typically the history of what the application did, such as which pages were requested, which servers it connected to, which database results it provides, and so forth.  `Server logs` are related to the actual hardware that is running the application, and will often provide details about the health and status of all connected services, or even just the server itself.  Google "logs [PLATFORM_NAME]" if you're using a CMS, or "logs [PROGRAMMING_LANGUAGE]" and "logs [OPERATING_SYSTEM]" if you're running a custom application, to get more information on finding the logs in question.

### Debug Your Application Code

If all else fails, it may be that a problem in some custom code within your application is causing the issue.  Try to diagnose where the issue may be coming from through manually debugging your application, along with parsing through application and server logs.  Ideally, make a copy of the entire application to a local development machine and perform a step-by-step debug process, which will allow you to recreate the exact scenario in which the `302 Found` occurred and view the application code at the moment something goes wrong.

No matter what the cause, the appearance of a `302 Found` within your own web application is a strong indication that you may need an error management tool to help you automatically detect such errors in the future.  The best of these tools can even alert you and your team _immediately_ when an error occurs.  <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-302-found">Airbrake's error monitoring software</a> provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize exception parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-302-found">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look at what a 302 Found response code is, including troubleshooting tips to help you resolve this error in your own application.

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/HTTP/Status
- https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/302
- https://en.wikipedia.org/wiki/List_of_HTTP_status_codes
- https://en.wikipedia.org/wiki/HTTP_302
- http://www.checkupdown.com/status/E302.html
- https://tools.ietf.org/html/rfc1945
- https://tools.ietf.org/html/rfc7231