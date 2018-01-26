---
categories: [HTTP Errors]
date: 2018-01-35
published: true
title: "406 Not Acceptable: What It Is and How to Fix It"
---

The **406 Not Acceptable** is an [`HTTP response status code`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status) indicating that the client has requested a response using `Accept-` headers that the server is unable to fulfill.  This is typically a result of the user agent (i.e. browser) specifying an acceptable character set (via [`Accept-Charset`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Accept-Charset)), language (via [`Accept-Language`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Accept-Language)), and so forth that should be responded with, and the server being unable to provide such a response.

Like most HTTP response codes -- especially for those that indicate an error -- the cause of a `406 Not Acceptable` error code can be difficult to track down and fix.  With a potential pool of [_over 50_ status codes](https://en.wikipedia.org/wiki/List_of_HTTP_status_codes) that represent the complex relationship between the client, a web application, a web server, and often multiple third-party web services, determining the cause of a particular status code can be a challenge under the best of circumstances.

In this article we'll examine the `406 Not Acceptable` in more detail by looking at what might cause a message, along with a handful of tips for diagnosing and debugging the appearance of this error within your own application.  We'll even examine a number of the most popular content management systems (`CMSs`) for potential problem areas that could cause your own website to be generating a `406 Not Acceptable` unexpectedly.  Let's dive in!

## Server- or Client-Side?

All HTTP response status codes that are in the `4xx` category are considered `client error responses`.  This category contrasts with `5xx` classification errors, such as the [`504 Gateway Timeout Error`](https://airbrake.io/blog/http-errors/504-gateway-timeout-error) we [explored a few months ago](https://airbrake.io/blog/http-errors/504-gateway-timeout-error), which are considered `server error responses`.  That said, the appearance of a `4xx` error doesn't necessarily mean the issue is on the client side, where the "client" is the web browser or device being used to access the application.  Oftentimes, if you're trying to diagnose an issue within your own application, you can immediately ignore most client-side code and components, such as HTML, cascading style sheets (CSS), client-side JavaScript, and so forth.  This doesn't apply _solely_ to web sites, either.  Many smart phone apps, which implement a modern looking user interface, are actually powered by a normal web application behind the scenes that is simply hidden from the user.

On the other hand, the server _could_ be the root cause of a `406 Not Acceptable` error.  In some cases, the server may be misconfigured and handling requests improperly, which can result in `406` code responses and other problematic traffic routing issues.  We'll explore some of these scenarios (and potential solutions) down below, but be aware that, even though the `406 Not Acceptable` is considered a `client error response`, it doesn't inherently mean we can rule out either the client nor the server as the culprit in this scenario.  In these scenarios, the `server` is still the network object that is producing the `406 Not Acceptable` and returning it as the HTTP response code to the client, but it could be that the client is causing the issue in some way.

## Start With a Thorough Application Backup

As with anything, it's better to have played it safe at the start than to screw something up and come to regret it later on down the road.  As such, it is _critical_ that you perform a full backup of your application, database, and all other components of your website or application _before_ attempting any fixes or changes to the system.  Even better, if you have the capability, create a complete copy of the application and stick the copy on a secondary `staging` server that isn't active or is inaccessible to the public.  This will give you a clean testing ground with which to test all potential fixes to resolve the issue, without threatening the security or sanctity of your live application.

## Diagnosing a 406 Not Acceptable

As discussed in the introduction, a `406 Not Acceptable` indicates that the user agent (the web browser, in most cases) has requested a valid resource, _however_ the request included a special `Accept-` header that indicates to the server a valid response can only contain certain _types_ of information.  Here are a few examples of such scenarios:

- The user agent may be localized to a particular locale or language that the server cannot provide.  For example, a user agent may use the [`Accept-Language`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Accept-Language) request header to specify a valid language of French (`Accept-Language: fr`), but if the server cannot serve a response in French, a `406` code may be the only proper response.
- The user agent may be requesting a specific type of content to be returned by the server.  These content types, commonly know as [`MIME types`](https://www.iana.org/assignments/media-types/media-types.xhtml), define things like plain text (`text/plain`), PNG images (`image/png`), mp4 videos (`video/mp4`), and so forth.  Thus, the client may include the [`Accept`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Accept) header in the request and define an explicit MIME type that should be provided by the server (e.g. `Accept: application/xml`).  If the server is unable to respond with the matching content type that was requested a `406 Not Acceptable` response may be necessary.

There are handful of other `Accept-` headers that can be provided in HTTP requests, but the vast majority of scenarios are similar to above: The user agent wants an _explicit_ type of response, and the server either provides it, or it may return a `406` code indicating it cannot fulfill the request.

### Troubleshooting on the Client-Side

Since the `406 Not Acceptable` is a `client error response` code, it's best to start by troubleshooting any potential client-side issues that could be causing this error.  Here are a handful of tips to try on the browser or device that is giving you problems.

#### Check the Requested URL

The most common cause of a `406 Not Acceptable` is simply inputting an incorrect URL.  Many servers are tightly secured, so as to disallow unexpected requests to resources that a client/user agent should not have access to.  It may be that the requested URL is slightly incorrect, which is causing the user agent to request a specific type of response.  For example, a request to the URI `https://airbrake.io?json` might indicate to the server that a `JSON` response is required.  Since `406` codes are not as common as `404` codes, the appearance of a `406` _could_ means that the requested URL is valid, but the browser may be misinterpreting the intended request type.  Either way, it's a good idea to double-check the exact URL that is returning the `406 Not Acceptable` error to make sure it is intended resource.

### Debugging Common Platforms

If you're running common software packages on the server that is responding with the `406 Not Acceptable`, you may want to start by looking into the stability and functionality of those platforms first.  The most common content management systems -- like WordPress, Joomla!, and Drupal -- are all typically well-tested out of the box, but once you start making modifications to the underlying extensions or `PHP` code (the language in which nearly all modern content management systems are written in), it's all too easy to cause an unforeseen issue that results in a `406 Not Acceptable`.

There are a few tips below aimed at helping you troubleshoot some of these popular software platforms.

#### Rollback Recent Upgrades

If you recently updated the content management system itself just before the `406 Not Acceptable` appeared, you may want to consider rolling back to the previous version you had installed when things were working fine.  Similarly, any extensions or modules that you may have recently upgraded can also cause server-side issues, so reverting to previous versions of those may also help.  For assistance with this task, simply Google "downgrade [PLATFORM_NAME]" and follow along.  In some cases, however, certain CMSs don't really provide a version downgrade capability, which indicates that they consider the base application, along with each new version released, to be extremely stable and bug-free.  This is typically the case for the more popular platforms, so don't be afraid if you can't find an easy way to revert the platform to an older version.

#### Uninstall New Extensions, Modules, or Plugins

Depending on the particular content management system your application is using, the exact name of these components will be different, but they serve the same purpose across every system: improving the capabilities and features of the platform beyond what it's normally capable of out of the box.  But be warned: such extensions can, more or less, take full control of the system and make virtually any changes, whether it be to the `PHP` code, HTML, CSS, JavaScript, or database.  As such, it may be wise to uninstall any new extensions that may have been recently added.  Again, Google the extension name for the official documentation and assistance with this process.  

#### Check for Unexpected Database Changes

It's worth noting that, even _if_ you uninstall an extension through the CMS dashboard, this doesn't _guarantee_ that changes made by the extension have been fully reverted.  This is particularly true for many WordPress extensions, which are given carte blanche within the application, including full access rights to the database.  Unless the extension author explicitly codes such things in, there are scenarios where an extension may modify database records that don't "belong" to the extension itself, but are instead created and managed by other extensions (or even the base CMS itself).  In those scenarios, the extension may not know how to revert alterations to database records, so it will ignore such things during uninstallation.  Diagnosing such problems can be tricky, but I've personally encountered such scenarios multiple times, so your best course of action, assuming you're reasonably convinced an extension is the likely culprit for the `406 Not Acceptable`, is to open the database and manually look through tables and records that were likely modified by the extension.

Above all, don't be afraid to Google your issue.  Try searching for specific terms related to your issue, such as the name of your application's CMS, along with the `406 Not Acceptable`.  Chances are you'll find someone who has experienced the same issue.

### Troubleshooting on the Server-Side

If you aren't running a CMS application -- or even if you are, but you're confident the `406 Not Acceptable` isn't related to that -- here are some additional tips to help you troubleshoot what might be causing the issue on the server-side of things.

### Confirm Your Server Configuration

Your application is likely running on a server that is using one of the two most popular web server softwares, `Apache` or `nginx`.  At the time of publication, both of these web servers make up [84% of the world's web server software](https://w3techs.com/technologies/overview/web_server/all)!  Thus, one of the first steps you can take to determine what might be causing these `406 Not Acceptable` response codes is to check the configuration files for your web server software for unintentional redirect or request handling instructions.

To determine which web server your application is using you'll want to look for a key file.  If your web server is Apache then look for an `.htaccess` file within the root directory of your website file system.  For example, if your application is on a shared host you'll likely have a username associated with the hosting account.  In such a case, the application root directory is typically found at the path of `/home/<username>/public_html/`, so the `.htaccess` file would be at `/home/<username>/public_html/.htaccess`.

If you located the `.htaccess` file then open it in a text editor and look for lines that use `RewriteXXX` directives, which are part of the [`mod_rewrite`](http://httpd.apache.org/docs/current/mod/mod_rewrite.html) module in Apache.  Covering exactly how these rules work is well beyond the scope of this article, however, the basic concept is that a `RewriteCond` directive defines a text-based pattern that will be matched against entered URLs.  If a matching URL is requested by a visitor to the site, the `RewriteRule` directive that follows one or more `RewriteCond` directives is used to perform the actual redirection of the request to the appropriate URL.

For example, here is a simple `RewriteRule` that matches all incoming requests to `https://airbrake.io/users/json` that _do not_ contain an `Accept: application/json` request header.  The result is a redirection and `406 Not Acceptable` response error code:

```
RewriteEngine on
RewriteCond %{REQUEST_URI} ^/users/json/?.*$
RewriteCond %{HTTP_ACCEPT} !application/json
RewriteRule ^(.*)$ http://airbrake.io/users/json$1 [R=406,L]
```

Notice the `R=406` flag at the end of the `RewriteRule`, which explicitly states that the response code should be `406`, indicating to user agents that the resource exists, but the explicit `Accept-` headers could not be fulfilled.  Thus, if you find any strange `RewriteCond` or `RewriteRule` directives in the `.htaccess` file that don't seem to belong, try temporarily commenting them out (using the `#` character prefix) and restarting your web server to see if this resolves the issue.

On the other hand, if your server is running on `nginx`, you'll need to look for a completely different configuration file.  By default this file is named `nginx.conf` and is located in one of a few common directories: `/usr/local/nginx/conf`, `/etc/nginx`, or `/usr/local/etc/nginx`.  Once located, open `nginx.conf` in a text editor and look for directives that are using the `406` response code flag.  For example, here is a simple `block directive` (i.e. a named set of directives) that configures a virtual server for `airbrake.io` and ensures that, similar to above, a request to `https://airbrake.io/users/json` that _doesn't_ include an `Accept: application/json` request header will fail and is met with a `406` response code:

```
server { 
    listen 80;
    listen 443 ssl;    
    server_name airbrake.io;    
    location /users/json {
        if ($http_accept != application/json) {
            return 406 https://airbrake.io/users/json$request_uri;
        }
    }
}
```

Have a look through your `nginx.conf` file for any abnormal directives or lines that include the `406` flag.  Comment out any abnormalities before restarting the server to see if the issue was resolved.

Configuration options for each different type of web server can vary dramatically, so we'll just list a few popular ones to give you some resources to look through, depending on what type of server your application is running on:

- [Apache](https://httpd.apache.org/docs/2.4/configuring.html)
- [Nginx](http://nginx.org/en/docs/beginners_guide.html)
- [IIS](https://docs.microsoft.com/en-us/iis/configuration/)
- [Node.js](https://nodejs.org/api/)
- [Apache Tomcat](https://tomcat.apache.org/tomcat-9.0-doc/setup.html)

#### Look Through the Logs

Nearly every web application will keep some form of server-side logs.  `Application logs` are typically the history of what the application did, such as which pages were requested, which servers it connected to, which database results it provides, and so forth.  `Server logs` are related to the actual hardware that is running the application, and will often provide details about the health and status of all connected services, or even just the server itself.  Google "logs [PLATFORM_NAME]" if you're using a CMS, or "logs [PROGRAMMING_LANGUAGE]" and "logs [OPERATING_SYSTEM]" if you're running a custom application, to get more information on finding the logs in question.

#### Debug Your Application Code or Scripts

If all else fails, it may be that a problem in some custom code within your application is causing the issue.  Try to diagnose where the issue may be coming from through manually debugging your application, along with parsing through application and server logs.  Ideally, make a copy of the entire application to a local development machine and perform a step-by-step debug process, which will allow you to recreate the exact scenario in which the `406 Not Acceptable` occurred and view the application code at the moment something goes wrong.

No matter the cause -- and even if you managed to fix it this time -- the appearance of an issue like the `406 Not Acceptable` within your own application is a good indication you may want to implement an error management tool, which will help you automatically detect errors and will alert you the very moment they occur.  <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-406-not-acceptable">Airbrake's error monitoring software</a> provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize exception parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-406-not-acceptable">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

An in-depth overview of what a 406 Not Acceptable response is, including troubleshooting tips to help you resolve this error in your own application.

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/HTTP/Status
- https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/406
- https://en.wikipedia.org/wiki/List_of_HTTP_status_codes
- https://en.wikipedia.org/wiki/HTTP_406
- https://tools.ietf.org/html/rfc7235#section-3.1
