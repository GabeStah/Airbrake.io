---
categories: [HTTP Errors]
date: 2018-02-08
published: true
title: "408 Request Timeout: What It Is and How to Fix It"
---

The **408 Request Timeout** is an [`HTTP response status code`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status) indicating that the server did not receive a complete request from the client within the server's allotted timeout period.  The `408 Request Timeout` error code _appears_ similar to the [`504 Gateway Timeout`](https://airbrake.io/blog/http-errors/504-gateway-timeout-error) error we [explored in a previous article](https://airbrake.io/blog/http-errors/504-gateway-timeout-error), which indicates that that a server acting as a gateway or proxy timed out.  However, the `408 Request Timeout` error isn't a message from a gateway or proxy server somewhere in the node chain, but is a direct message from the active server the client has connected to (like a the web server)

It can be difficult to find the cause of unexpected HTTP response codes and the `408 Request Timeout` error code is no exception.  With a potential pool of [_over 50_ status codes](https://en.wikipedia.org/wiki/List_of_HTTP_status_codes) used to represent the complex relationship between the client, a web application, a web server, and (possibly) multiple third-party web services, determining the cause of a particular status code can be challenging, even under the best of circumstances.

In this article we'll explore the `408 Request Timeout` in greater depth by looking at what might cause this message to appear, including a few tips you can use to diagnose and debug the appearance of this error within your own application.  We'll even look at a number of the most popular content management systems (`CMSs`) for potential problem areas that could cause your own website to be unexpectedly generating `408 Request Timeout` errors.  Let's dive right in!

## Server- or Client-Side?

All HTTP response status codes within the `4xx` category are considered `client error responses`.  Errors in the `4xx` category contrast with those from the `5xx` category, such as the aforementioned [`504 Gateway Timeout`](https://airbrake.io/blog/http-errors/504-gateway-timeout-error) we [examined earlier](https://airbrake.io/blog/http-errors/504-gateway-timeout-error), which are considered `server error responses`.  That said, the appearance of a `4xx` error doesn't necessarily mean the issue is on the client side (the "client", in this case, is typically the web browser or device being used to access the application).  Oftentimes, if you're trying to diagnose an issue within your own application, you can immediately ignore most client-side code and components, such as HTML, cascading style sheets (CSS), client-side JavaScript, and so forth.  This doesn't apply _solely_ to web sites, either.  Smart phone applications often implement modern looking user interfaces that are actually powered by normal web applications behind the scenes.

On the other hand, the server _could_ be the root cause of a `408 Request Timeout` error.  In some cases, the server may be misconfigured and may be handling requests improperly, which can result in `408` code responses and other troublesome traffic routing issues.  We'll explore some of these scenarios (and potential solutions) down below, but be aware that, even though the `408 Request Timeout` is considered a `client error response`, it doesn't necessarily mean we can rule out either the client nor the server as the culprit in this scenario.  In these situations, the `server` is still the network object that is producing the `408 Request Timeout` and returning it as the HTTP response code to the `client`, but it could be that the client is causing the issue in some way.

## Start With a Thorough Application Backup

As usual, it is better to have played it safe at the start than to screw something up and come to regret it later on down the road.  As such, it is _critical_ that you perform a full backup of your application, database, and all other components of your website or application _before_ attempting any fixes or changes to the system.  Even better, if you have the capability, create a complete copy of the application and stick the copy on a secondary `staging` server that is either inactive, or publicly inaccessible.  This will give you a clean testing ground on which to test all potential fixes needed to resolve the issue, without threatening the security or sanctity of your live application.

## Diagnosing a 408 Request Timeout

A `408 Request Timeout` response code indicates that the server did not receive a complete request from the client within a specific period of time tracked by the server (i.e. the `timeout` period).  As specified in the [RFC7235 HTTP/1.1 Semantics and Content](https://tools.ietf.org/html/rfc7231#section-6.5.7) standards document server _should_ include the special [`Connection`]() header with the `close` directive as part of its response (e.g. `Connection: close`), which informs the client that the connection should be closed.  Put simply, a `408` code informs the client that the server has decided to close the connection rather than continue waiting for the transaction to complete.  Upon receiving the `Connection: close` header the client can opt to repeat the original request using a new connection.

Most modern browsers implement [HTTP preconnection mechanisms](https://w3c.github.io/resource-hints/#dfn-preconnect), which provides the user agent (i.e. browser) to speed up overall web surfing experiences by essentially _predicting_ what resources -- and therefore connections -- the client may be using in the immediate future.  The full scope of how browsers use these mechanisms is well beyond the scope of this article, but you can check out the [W3C Resource Hints](https://w3c.github.io/resource-hints) documentation for more details.

### Troubleshooting on the Client-Side

Since the `408 Request Timeout` is a `client error response` code, it's best to start by troubleshooting any potential client-side issues that could be causing this error.  Here are a handful of tips to try on the browser or device that is giving you problems.

#### Check the Requested URL

The most common cause of a `408 Request Timeout` is simply inputting an incorrect URL.  Many servers are tightly secured, so as to disallow unexpected requests to resources that a client/user agent should not have access to.  It may be that the requested URL is slightly incorrect, which is causing the user agent to request an unintended resource, which may be routed through a proxy server that requires authentication.  For example, a request to the URI `https://airbrake.io/login` might route requests through a separate proxy server used to handle user authentication.  If the original request did not contain appropriate credentials, the result could be a `408 Request Timeout` error response.  It's always a good idea to double-check the exact URL that is returning the `408 Request Timeout` error to make sure it is intended resource.

### Debugging Common Platforms

If you're running common software packages on the server that is responding with the `408 Request Timeout`, you may want to start by looking into the stability and functionality of those platforms first.  The most common content management systems -- like WordPress, Joomla!, and Drupal -- are all typically well-tested out of the box, but once you start making modifications to the underlying extensions or `PHP` code (the language in which nearly all modern content management systems are written in), it's all too easy to cause an unforeseen issue that results in a `408 Request Timeout`.

There are a few tips below aimed at helping you troubleshoot some of these popular software platforms.

#### Rollback Recent Upgrades

If you recently updated the content management system itself just before the `408 Request Timeout` appeared, you may want to consider rolling back to the previous version you had installed when things were working fine.  Similarly, any extensions or modules that you may have recently upgraded can also cause server-side issues, so reverting to previous versions of those may also help.  For assistance with this task, simply Google "downgrade [PLATFORM_NAME]" and follow along.  In some cases, however, certain CMSs don't really provide a version downgrade capability, which indicates that they consider the base application, along with each new version released, to be extremely stable and bug-free.  This is typically the case for the more popular platforms, so don't be afraid if you can't find an easy way to revert the platform to an older version.

#### Uninstall New Extensions, Modules, or Plugins

Depending on the particular content management system your application is using, the exact name of these components will be different, but they serve the same purpose across every system: improving the capabilities and features of the platform beyond what it's normally capable of out of the box.  But be warned: such extensions can, more or less, take full control of the system and make virtually any changes, whether it be to the `PHP` code, HTML, CSS, JavaScript, or database.  As such, it may be wise to uninstall any new extensions that may have been recently added.  Again, Google the extension name for the official documentation and assistance with this process.  

#### Check for Unexpected Database Changes

It's worth noting that, even _if_ you uninstall an extension through the CMS dashboard, this doesn't _guarantee_ that changes made by the extension have been fully reverted.  This is particularly true for many WordPress extensions, which are given carte blanche within the application, including full access rights to the database.  Unless the extension author explicitly codes such things in, there are scenarios where an extension may modify database records that don't "belong" to the extension itself, but are instead created and managed by other extensions (or even the base CMS itself).  In those scenarios, the extension may not know how to revert alterations to database records, so it will ignore such things during uninstallation.  Diagnosing such problems can be tricky, but I've personally encountered such scenarios multiple times, so your best course of action, assuming you're reasonably convinced an extension is the likely culprit for the `408 Request Timeout`, is to open the database and manually look through tables and records that were likely modified by the extension.

Above all, don't be afraid to Google your issue.  Try searching for specific terms related to your issue, such as the name of your application's CMS, along with the `408 Request Timeout`.  Chances are you'll find someone who has experienced the same issue.

### Troubleshooting on the Server-Side

If you aren't running a CMS application -- or even if you are, but you're confident the `408 Request Timeout` isn't related to that -- here are some additional tips to help you troubleshoot what might be causing the issue on the server-side of things.

### Confirm Your Server Configuration

Your application is likely running on a server that is using one of the two most popular web server softwares, `Apache` or `nginx`.  At the time of publication, both of these web servers make up [84% of the world's web server software](https://w3techs.com/technologies/overview/web_server/all)!  Thus, one of the first steps you can take to determine what might be causing these `408 Request Timeout` response codes is to check the configuration files for your web server software for unintentional redirect or request handling instructions.

To determine which web server your application is using you'll want to look for a key file.  If your web server is Apache then look for an `.htaccess` file within the root directory of your website file system.  For example, if your application is on a shared host you'll likely have a username associated with the hosting account.  In such a case, the application root directory is typically found at the path of `/home/<username>/public_html/`, so the `.htaccess` file would be at `/home/<username>/public_html/.htaccess`.

If you located the `.htaccess` file then open it in a text editor and look for lines that use `KeepAliveTimeout` or `RequestReadTimeout` directives.  `KeepAliveTimeout` is part of the [`core`](https://httpd.apache.org/docs/2.4/mod/core.html#keepalivetimeout) module, while `RequestReadTimeout` is from the [`reqtimeout`](https://httpd.apache.org/docs/2.4/mod/mod_reqtimeout.html) module in Apache.  Covering exactly how these directives work is well beyond the scope of this article, however, the basic concept is that these timeout directives inform the server to allow for incoming client requests to take only a certain amount of time before they are considered failed and closed via a `408` response.

For example, here we're using the `RequestReadTimeout` directive to set header and body timeouts of `15` and `30` seconds:

```
RequestReadTimeout header=15 body=30
```

Look for any strange timeout directives in the `.htaccess` file that don't seem to belong, then try temporarily commenting them out (using the `#` character prefix) and restarting your web server to see if this resolves the issue.

On the other hand, if your server is running on `nginx`, you'll need to look for a completely different configuration file.  By default this file is named `nginx.conf` and is located in one of a few common directories: `/usr/local/nginx/conf`, `/etc/nginx`, or `/usr/local/etc/nginx`.  Once located, open `nginx.conf` in a text editor and look for `client_body_timeout`, `client_header_timeout`, or `keepalive_timeout` directives, which are all part of the [`http_core`](http://nginx.org/en/docs/http/ngx_http_core_module.html) Nginx module.  For example, here is a simple `block directive` (i.e. a named set of directives) that configures a virtual server for `airbrake.io` and sets the client header and body timeouts to `15` and `30` seconds, respectively:

```
server { 
    listen 80;
    listen 443 ssl;    
    server_name airbrake.io;

    client_header_timeout 15s;
    client_body_timeout 30s;
    keepalive_timeout 60s;
}
```

Have a look through your `nginx.conf` file for any abnormal `_timeout` directives and comment out any abnormalities before restarting the server to see if the issue was resolved.

Configuration options for each different type of web server can vary dramatically, so we'll just list a few popular ones to give you some resources to look through, depending on what type of server your application is running on:

- [Apache](https://httpd.apache.org/docs/2.4/configuring.html)
- [Nginx](http://nginx.org/en/docs/beginners_guide.html)
- [IIS](https://docs.microsoft.com/en-us/iis/configuration/)
- [Node.js](https://nodejs.org/api/)
- [Apache Tomcat](https://tomcat.apache.org/tomcat-9.0-doc/setup.html)

#### Look Through the Logs

Nearly every web application will keep some form of server-side logs.  `Application logs` are typically the history of what the application did, such as which pages were requested, which servers it connected to, which database results it provides, and so forth.  `Server logs` are related to the actual hardware that is running the application, and will often provide details about the health and status of all connected services, or even just the server itself.  Google "logs [PLATFORM_NAME]" if you're using a CMS, or "logs [PROGRAMMING_LANGUAGE]" and "logs [OPERATING_SYSTEM]" if you're running a custom application, to get more information on finding the logs in question.

#### Debug Your Application Code or Scripts

If all else fails, it may be that a problem in some custom code within your application is causing the issue.  Try to diagnose where the issue may be coming from through manually debugging your application, along with parsing through application and server logs.  Ideally, make a copy of the entire application to a local development machine and perform a step-by-step debug process, which will allow you to recreate the exact scenario in which the `408 Request Timeout` occurred and view the application code at the moment something goes wrong.

No matter the cause -- and even if you managed to fix this particular error this time around -- the appearance of an issue like the `408 Request Timeout` within your own application is a good indication you may want to implement an error management tool, which will help you automatically detect errors and will alert you the instant they occur.  <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-408-request-timeout">Airbrake's error monitoring software</a> provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize exception parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-408-request-timeout">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A detailed overview of what a 408 Request Timeout response code is, with tips to help you resolve this error in your own application.

__SOURCES__

- https://httpstatuses.com/
- https://developer.mozilla.org/en-US/docs/Web/HTTP/Status
- https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/408
- https://en.wikipedia.org/wiki/List_of_HTTP_status_codes
- https://en.wikipedia.org/wiki/HTTP_408
- https://tools.ietf.org/html/rfc7235#section-3.1
