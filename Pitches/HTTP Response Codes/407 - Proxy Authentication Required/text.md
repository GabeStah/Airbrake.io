---
categories: [HTTP Errors]
date: 2018-02-01
published: true
title: "407 Proxy Authentication Required: What It Is and How to Fix It"
---

The **407 Proxy Authentication Required** is an [`HTTP response status code`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status) indicating that the server is unable to complete the request because the client lacks proper authentication credentials for a `proxy server` that is intercepting the request between the client and server.  The `407` error code is similar to the [`401 Unauthorized`](https://airbrake.io/blog/http-errors/401-unauthorized-error) error we [looked at a few months ago](https://airbrake.io/blog/http-errors/401-unauthorized-error), which indicates that the client could not be authenticated with the _server_.  However, in the case of a `407 Proxy Authentication Required` error, the server isn't reporting a _direct_ authentication issue, but is instead reporting that the client needs to authenticate with a proxy server, which must send a special [`Proxy-Authenticate`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Proxy-Authenticate) header as part of the response.

As with most HTTP response codes -- especially those that indicate an error -- the cause of a `407 Proxy Authentication Required` error code can be challenging to find and fix.  With a possible pool of [_over 50_ status codes](https://en.wikipedia.org/wiki/List_of_HTTP_status_codes) used to represent the complex relationship between the client, a web application, a web server, and (possibly) multiple third-party web services, determining the cause of a particular status code can be difficult under the best of circumstances.

In this article we'll examine the `407 Proxy Authentication Required` in more detail by looking at what might cause this message to appear, and we'll go over a handful of tips you can use to diagnose and debug the appearance of this error within your own application.  We'll even examine a number of the most popular content management systems (`CMSs`) for potential problem areas that could cause your own website to be generating a `407 Proxy Authentication Required` unexpectedly.  Let's get to it!

## Server- or Client-Side?

All HTTP response status codes that are in the `4xx` category are considered `client error responses`.  Errors in the `4xx` category contrast with those from the `5xx` category, such as the [`503 Service Unavailable Error`](https://airbrake.io/blog/http-errors/503-service-unavailable) we [wrote about a couple months ago](https://airbrake.io/blog/http-errors/503-service-unavailable), which are considered `server error responses`.  That said, the appearance of a `4xx` error doesn't necessarily mean the issue is on the client side (the "client", in this case, is typically the web browser or device being used to access the application).  Oftentimes, if you're trying to diagnose an issue within your own application, you can immediately ignore most client-side code and components, such as HTML, cascading style sheets (CSS), client-side JavaScript, and so forth.  This doesn't apply _solely_ to web sites, either.  Many smart phone apps, which implement a modern looking user interface, are often powered behind the scenes by a normal web application.

On the other hand, the server _could_ be the root cause of a `407 Proxy Authentication Required` error.  In some cases, the server may be misconfigured and handling requests improperly, which can result in `407` code responses and other problematic traffic routing issues.  We'll explore some of these scenarios (and potential solutions) down below, but be aware that, even though the `407 Proxy Authentication Required` is considered a `client error response`, it doesn't inherently mean we can rule out either the client nor the server as the culprit in this scenario.  In these situations, the `server` (or a `proxy server`, in some cases) is still the network object that is producing the `407 Proxy Authentication Required` and returning it as the HTTP response code to the client, but it could be that the client is causing the issue in some way.

## Start With a Thorough Application Backup

As with anything, it's better to have played it safe at the start than to screw something up and come to regret it later on down the road.  As such, it is _critical_ that you perform a full backup of your application, database, and all other components of your website or application _before_ attempting any fixes or changes to the system.  Even better, if you have the capability, create a complete copy of the application and stick the copy on a secondary `staging` server that is either inactive, or publicly inaccessible.  This will give you a clean testing ground on which to test all potential fixes needed to resolve the issue, without threatening the security or sanctity of your live application.

## Diagnosing a 407 Proxy Authentication Required

As discussed in the introduction, a `407 Proxy Authentication Required` indicates that the client has failed to provide proper authentication credentials to a `proxy server` that is a node (i.e. connection) between the client and the primary web server accepting the original request.  As specified by [RFC7235 HTTP/1.1 Authentication](https://tools.ietf.org/html/rfc7235#section-3.2) standards document the proxy server _must_ send a special [`Proxy-Authenticate`](https://tools.ietf.org/html/rfc7235#section-4.3) header, which indicates to the client what type of authentication can be used to complete the original request, and what access that will provide.

The basic syntax of the `Proxy-Authenticate` header is as follows: `Proxy-Authenticate: <type> realm=<realm>`.

The `<type>` value can be any of the handful of valid [`authentication schemes`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Authentication#Authentication_schemes) allowed in HTTP/1.1, with the most common authentication scheme being `Basic`, which accepts a `username` and `password` credential pair to validate authentication.

The `<realm>` value is used as a simple description of the protected area or "scope" of access that this particular authentication process will provide to the client.  

Once the client receives a `407` response code that includes a `Proxy-Authenticate` header indicating the `authentication scheme` the proxy server will accept, the user agent will then typically respond with the corresponding [`Proxy-Authorization`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Proxy-Authorization) request header: `Proxy-Authorization: <type> <credentials>`.

Just as with the `Proxy-Authenticate` response header, `<type>` in the `Proxy-Authorization` request header is used to specify the `authentication scheme`, which should match the scheme required by the proxy server.

The `<credentials>` should be replaced with the valid credentials to authenticate the client.  In the case of a `Basic` authentication scheme the `username` and `password` values are concatenated with a colon separator (i.e. `username:password`), which is then encoded to a [base64](https://en.wikipedia.org/wiki/Base64) text string.  Thus, a full `Proxy-Authorization` request header using the `Basic` scheme with a username and password of `username` and `password` would look like this: `Proxy-Authorization: Basic dXNlcm5hbWU6cGFzc3dvcmQ=`.  Once the user agent includes that header in the follow-up request, the proxy server will authenticate and authorize the client and the request will succeed.

### Troubleshooting on the Client-Side

Since the `407 Proxy Authentication Required` is a `client error response` code, it's best to start by troubleshooting any potential client-side issues that could be causing this error.  Here are a handful of tips to try on the browser or device that is giving you problems.

#### Check the Requested URL

The most common cause of a `407 Proxy Authentication Required` is simply inputting an incorrect URL.  Many servers are tightly secured, so as to disallow unexpected requests to resources that a client/user agent should not have access to.  It may be that the requested URL is slightly incorrect, which is causing the user agent to request an unintended resource, which may be routed through a proxy server that requires authentication.  For example, a request to the URI `https://airbrake.io/login` might route requests through a separate proxy server used to handle user authentication.  If the original request did not contain appropriate credentials, the result could be a `407 Proxy Authentication Required` error response.  It's always a good idea to double-check the exact URL that is returning the `407 Proxy Authentication Required` error to make sure it is intended resource.

### Debugging Common Platforms

If you're running common software packages on the server that is responding with the `407 Proxy Authentication Required`, you may want to start by looking into the stability and functionality of those platforms first.  The most common content management systems -- like WordPress, Joomla!, and Drupal -- are all typically well-tested out of the box, but once you start making modifications to the underlying extensions or `PHP` code (the language in which nearly all modern content management systems are written in), it's all too easy to cause an unforeseen issue that results in a `407 Proxy Authentication Required`.

There are a few tips below aimed at helping you troubleshoot some of these popular software platforms.

#### Rollback Recent Upgrades

If you recently updated the content management system itself just before the `407 Proxy Authentication Required` appeared, you may want to consider rolling back to the previous version you had installed when things were working fine.  Similarly, any extensions or modules that you may have recently upgraded can also cause server-side issues, so reverting to previous versions of those may also help.  For assistance with this task, simply Google "downgrade [PLATFORM_NAME]" and follow along.  In some cases, however, certain CMSs don't really provide a version downgrade capability, which indicates that they consider the base application, along with each new version released, to be extremely stable and bug-free.  This is typically the case for the more popular platforms, so don't be afraid if you can't find an easy way to revert the platform to an older version.

#### Uninstall New Extensions, Modules, or Plugins

Depending on the particular content management system your application is using, the exact name of these components will be different, but they serve the same purpose across every system: improving the capabilities and features of the platform beyond what it's normally capable of out of the box.  But be warned: such extensions can, more or less, take full control of the system and make virtually any changes, whether it be to the `PHP` code, HTML, CSS, JavaScript, or database.  As such, it may be wise to uninstall any new extensions that may have been recently added.  Again, Google the extension name for the official documentation and assistance with this process.  

#### Check for Unexpected Database Changes

It's worth noting that, even _if_ you uninstall an extension through the CMS dashboard, this doesn't _guarantee_ that changes made by the extension have been fully reverted.  This is particularly true for many WordPress extensions, which are given carte blanche within the application, including full access rights to the database.  Unless the extension author explicitly codes such things in, there are scenarios where an extension may modify database records that don't "belong" to the extension itself, but are instead created and managed by other extensions (or even the base CMS itself).  In those scenarios, the extension may not know how to revert alterations to database records, so it will ignore such things during uninstallation.  Diagnosing such problems can be tricky, but I've personally encountered such scenarios multiple times, so your best course of action, assuming you're reasonably convinced an extension is the likely culprit for the `407 Proxy Authentication Required`, is to open the database and manually look through tables and records that were likely modified by the extension.

Above all, don't be afraid to Google your issue.  Try searching for specific terms related to your issue, such as the name of your application's CMS, along with the `407 Proxy Authentication Required`.  Chances are you'll find someone who has experienced the same issue.

### Troubleshooting on the Server-Side

If you aren't running a CMS application -- or even if you are, but you're confident the `407 Proxy Authentication Required` isn't related to that -- here are some additional tips to help you troubleshoot what might be causing the issue on the server-side of things.

### Confirm Your Server Configuration

Your application is likely running on a server that is using one of the two most popular web server softwares, `Apache` or `nginx`.  At the time of publication, both of these web servers make up [84% of the world's web server software](https://w3techs.com/technologies/overview/web_server/all)!  Thus, one of the first steps you can take to determine what might be causing these `407 Proxy Authentication Required` response codes is to check the configuration files for your web server software for unintentional redirect or request handling instructions.

To determine which web server your application is using you'll want to look for a key file.  If your web server is Apache then look for an `.htaccess` file within the root directory of your website file system.  For example, if your application is on a shared host you'll likely have a username associated with the hosting account.  In such a case, the application root directory is typically found at the path of `/home/<username>/public_html/`, so the `.htaccess` file would be at `/home/<username>/public_html/.htaccess`.

If you located the `.htaccess` file then open it in a text editor and look for lines that use `ProxyXXX` directives, which are part of the [`mod_proxy`](https://httpd.apache.org/docs/trunk/mod/mod_proxy.html) module in Apache.  Covering exactly how these directives work is well beyond the scope of this article, however, the basic concept is that proxy directives allow the Apache server to map or associate local server requests and URIs to remote proxy locations.

For example, here we've enabled `ProxyPass` and `ProxyPassReverse` directives to match requests to the local `/login` URI and route them to `https://proxy.airbrake.io/login`.  The `<Location /login>` section defines the authentication scheme and details we're using:

```
ProxyPass        /login https://proxy.airbrake.io/login
ProxyPassReverse /login https://proxy.airbrake.io/login

<Location /login>
    Order deny,allow
    Allow from all
    AuthName "Login Authentication"
    AuthType basic
    AuthUserFile "/usr/local/apache2/conf/httpd.htpasswd"
    AuthGroupFile "/usr/local/apache2/conf/httpd.groups"
</Location>
```

This is just one _possible_ example, but if your own server is using a proxy then your particular configuration will look quite different.  Look for any strange `Proxy` directives in the `.htaccess` file that don't seem to belong, then try temporarily commenting them out (using the `#` character prefix) and restarting your web server to see if this resolves the issue.

On the other hand, if your server is running on `nginx`, you'll need to look for a completely different configuration file.  By default this file is named `nginx.conf` and is located in one of a few common directories: `/usr/local/nginx/conf`, `/etc/nginx`, or `/usr/local/etc/nginx`.  Once located, open `nginx.conf` in a text editor and look for `proxy_` directives.  For example, here is a simple `block directive` (i.e. a named set of directives) that configures a virtual server for `airbrake.io` and ensures that, similar to above, a request to `https://airbrake.io/login` will be authenticated via the `proxy_pass` directive to `https://proxy.airbrake.io/login`:

```
server { 
    listen 80;
    listen 443 ssl;    
    server_name airbrake.io;

    location /login/ {
        auth_basic "Login Authentication";
        auth_basic_user_file /etc/nginx/conf.d/nginx.htpasswd;

        proxy_pass                          https://proxy.airbrake.io/login;
        proxy_set_header  Host              $http_host;
        proxy_set_header  X-Real-IP         $remote_addr;
        proxy_set_header  X-Forwarded-For   $proxy_add_x_forwarded_for;
        proxy_set_header  X-Forwarded-Proto $scheme;
        proxy_read_timeout                  900;
    }
}
```

Have a look through your `nginx.conf` file for any abnormal `proxy_` directives and comment out any abnormalities before restarting the server to see if the issue was resolved.

Configuration options for each different type of web server can vary dramatically, so we'll just list a few popular ones to give you some resources to look through, depending on what type of server your application is running on:

- [Apache](https://httpd.apache.org/docs/2.4/configuring.html)
- [Nginx](http://nginx.org/en/docs/beginners_guide.html)
- [IIS](https://docs.microsoft.com/en-us/iis/configuration/)
- [Node.js](https://nodejs.org/api/)
- [Apache Tomcat](https://tomcat.apache.org/tomcat-9.0-doc/setup.html)

#### Look Through the Logs

Nearly every web application will keep some form of server-side logs.  `Application logs` are typically the history of what the application did, such as which pages were requested, which servers it connected to, which database results it provides, and so forth.  `Server logs` are related to the actual hardware that is running the application, and will often provide details about the health and status of all connected services, or even just the server itself.  Google "logs [PLATFORM_NAME]" if you're using a CMS, or "logs [PROGRAMMING_LANGUAGE]" and "logs [OPERATING_SYSTEM]" if you're running a custom application, to get more information on finding the logs in question.

#### Debug Your Application Code or Scripts

If all else fails, it may be that a problem in some custom code within your application is causing the issue.  Try to diagnose where the issue may be coming from through manually debugging your application, along with parsing through application and server logs.  Ideally, make a copy of the entire application to a local development machine and perform a step-by-step debug process, which will allow you to recreate the exact scenario in which the `407 Proxy Authentication Required` occurred and view the application code at the moment something goes wrong.

No matter the cause -- and even if you managed to fix this particular error this time around -- the appearance of an issue like the `407 Proxy Authentication Required` within your own application is a good indication you may want to implement an error management tool, which will help you automatically detect errors and will alert you the instant they occur.  <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-407-proxy-authentication-required">Airbrake's error monitoring software</a> provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize exception parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-407-proxy-authentication-required">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

An in-depth overview of what a 407 Proxy Authentication Required response code is, with tips to help you resolve this error in your own application.

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/HTTP/Status
- https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/407
- https://en.wikipedia.org/wiki/List_of_HTTP_status_codes
- https://en.wikipedia.org/wiki/HTTP_407
- https://tools.ietf.org/html/rfc7235#section-3.1
