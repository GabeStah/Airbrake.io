# 403 Forbidden Error: What It Is and How to Fix It

The **403 Forbidden Error** is an [`HTTP response status code`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status), which indicates that the identified client does not have proper authorization to access to the requested content.  As with most HTTP response codes, particularly those that indicate an error, the appearance of a `403 Forbidden Error` can be a challenge to properly diagnose and resolve.  With a pool of _over 50_ potential status codes that represent the complex relationship between the client, a web application, a web server, and often multiple third-party web services, determining the cause of a particular status code can be a challenge under the best of circumstances.

In this article we'll examine the `403 Forbidden Error` in more detail by looking at what might cause a message, along with a handful of tips for diagnosing and debugging your own application to find a resolution.  We'll even examine a number of the most popular content management systems (`CMSs`) for potential problem areas that could cause your own website to be generating a `403 Forbidden Error` unexpectedly.  Let's dive in!

## Server- or Client-Side?

All HTTP response status codes that are in the `4xx` category are considered `client error responses`.  These types of messages contrast with errors in the `5xx` category, such as the [`502 Bad Gateway Error`](https://airbrake.io/blog/http-errors/502-bad-gateway-error) we looked at last week, which are considered `server error responses`.  That said, the appearance of a `4xx` error doesn't necessarily mean the issue is something to do with the client, where the `client` is the web browser or device being used to access the application.  Oftentimes, if you're trying to diagnose an issue with your own application, you can immediately ignore most client-side code and components, such as HTML, cascading style sheets (CSS), client-side JavaScript, and so forth.  This doesn't apply _solely_ to web sites, either.  Many smart phone apps that have a modern looking user interface are actually powered by a normal web application behind the scenes; one that is simply hidden from the user.

On the other hand, this doesn't rule out the client as the actual _cause_ of a `403 Forbidden Error`, either.  The client might be trying to access an invalid URL, the browser could be failing to send the proper credentials to the site, and so forth.  We'll explore some of these scenarios (and potential solutions) down below, but be aware that, even though the `403 Forbidden Error` is considered a `client error response`, it doesn't inherently mean we can rule out either the client nor the server as the culprit in this scenario.  In these scenarios, the `server` is still the network object that is producing the `403 Forbidden Error`, and returning it as the HTTP response code to the client, but it could be that the client is causing the issue in some way.

## Start With a Thorough Application Backup

As with anything, it's better to have played it safe at the start than to screw something up and come to regret it later on down the road.  As such, it is _critical_ that you perform a full backup of your application, database, and so forth, before attempting any fixes or changes to the system.  Even better, if you have the capability, create a complete copy of the application onto a secondary `staging` server that isn't "live," or isn't otherwise active and available to the public.  This will give you a clean testing ground with which to test all potential fixes to resolve the issue, without threatening the security or sanctity of your live application.

## Diagnosing a 403 Forbidden Error

As previously mentioned, a `403 Forbidden Error` indicates that the `client` (the web browser, in most cases) is being informed by the `server` that it does not have proper authorization to access the requested content.  A `403 Forbidden Error` can typically occur in one of two scenarios:

- The client sent its authentication `credentials` to the server, the server acknowledged that the client is validly authenticated, and yet, the server rejected the authorized client from accessing the requested content for some reason.
- The requested content is strictly forbidden for all clients, regardless of authorization.  This can occur when attempting to access an invalid or forbidden URL that has been explicitly restricted by the web server software.  For example, [`Apache`](https://en.wikipedia.org/wiki/Apache_HTTP_Server) servers typically return a `403 Forbidden Error` when a client tries to access a URL that corresponds to a file system directory, since providing direct file access is usually a security flaw.

### Troubleshooting on the Client-Side

Since the `403 Forbidden Error` is a `client error response` code, it's best to start by troubleshooting any potential client-side issues that could be causing this error.  Here are a handful of tips to try on the browser or device that is giving you problems.

#### Check the Requested URL

The most common cause of a `403 Forbidden Error` is simply inputting an incorrect URL.  As discussed before, many web servers are tightly secured to disallow access to improper URLs that the server isn't prepared to provide access to.  This could be anything from trying to access a file directory via a URL to attempting to gain access to a private page meant for other users.  Thus, it's a good idea to double-check the exact URL that is returning the `403 Forbidden Error` error to make sure that is the exact resource you intend to request.

#### Clear Relevant Cookies

As you may already be aware, [`HTTP cookies`](https://en.wikipedia.org/wiki/HTTP_cookie) are tiny pieces of data stored on your local device, which are used by websites and applications as a mechanism to "remember" information about this particular browser and/or device.  Most modern web apps take advantage of cookies to store user authentication status, which can be used to easily inform the web application which user is currently active, and what kind of authorization the current client (browser) should be granted.

Thus, when a `403 Forbidden Error` occurs -- which often indicates the client has not been authenticated to perform the particular request -- the first consideration should be a problem with invalid or corrupted cookies, causing improper authentication for the server.  In most cases, you only need to concern yourself with cookies that are relevant to the website or application causing the problem.  Cookies are stored based on the `domain` where the application is located, so you can explicitly remove only cookies that match the website domain (e.g. `airbrake.io`) to keep most other cookies in tact.  However, if you aren't experienced with manually removing certain cookies, it's much easier and safer to clear _all_ cookies at once.  

Clearing cookies can be accomplished in different ways, depending on the browser you're using:

- [Google Chrome](https://support.google.com/accounts/answer/32050)
- [Internet Explorer](https://support.microsoft.com/en-us/help/17442/windows-internet-explorer-delete-manage-cookies)
- [Microsoft Edge](https://support.microsoft.com/en-us/help/10607/microsoft-edge-view-delete-browser-history)
- [Mozilla Firefox](https://support.mozilla.org/en-US/kb/delete-cookies-remove-info-websites-stored)
- [Safari](https://support.apple.com/en-us/HT201265)

#### Clear the Cache

Just like cookies, it's also possible that the local `browser cache` could be causing the `403 Forbidden Error` to appear.  Cache is just a collection of storage dedicated to retaining local copies of web content on your device for later use.  A browser's cache can include just about any type of data, but it is typically used to store compressed snapshots of webpages you frequently visit, including images and other binary data your browser often accesses.  With a local copy of these resources on your device, your browser doesn't need to spend the time or bandwidth to explicitly download this identical data every time you return to the same page.  For example, every time you open up Facebook, a large portion of page you're seeing has already been loaded during a previous visit, and that content was cached and stored on your local device.

Since your browser's cache stores local copies of web content and resources, it's possible that a change to the live version of your application is conflicting with the cached version already on your device, which can sometimes produce a `403 Forbidden Error` as a result.  Try clearing your browser's cache to see if that fixes the issue.  

As with cookies, clearing the cache is browser-dependant, so here are a few links to that relevant documentation for the most popular browsers:

- [Google Chrome](https://support.google.com/accounts/answer/32050)
- [Internet Explorer](https://support.microsoft.com/en-us/help/17438/windows-internet-explorer-view-delete-browsing-history)
- [Microsoft Edge](https://support.microsoft.com/en-us/help/10607/microsoft-edge-view-delete-browser-history)
- [Mozilla Firefox](https://support.mozilla.org/en-US/kb/how-clear-firefox-cache)
- [Safari](https://support.apple.com/en-us/HT201265)

#### Log Out and Log In

If the application you're using has some form of user authentication, the last client-side step to try is to log out and then log back in.  If you've recently cleared the browser cookies, this should usually log you out automatically the next time you try to load the page, so feel free to just try logging back at this point, to see if things are working once again.  In some situations, the application may be running into a problem with your previous `session`, which is just a string that the `server` sends to the `client` to identify that client during future requests.  As with other data, the `session token` (or `session string`) is stored locally on your device in the cookies and is transferred by the client to the server during every request.  If the server doesn't recognize the session token being sent by the client, or something has gone wrong with the server that indicates that particular token is invalid, you may see a `403 Forbidden Error` as a result.

For most web applications, logging out and logging back in will force the local session token to be recreated.

### Debugging Common Platforms

If you're running common software packages on the server that is responding with the `403 Forbidden Error`, you may want to start by looking into the stability and functionality of those platforms first.  The most common content management systems -- like WordPress, Joomla!, and Drupal -- are all typically well-tested out of the box, but once you start making modifications to the underlying extensions or `PHP` code (the language in which nearly all modern content management systems are written in), it's all too easy to cause an unforeseen issue that results in a `403 Forbidden Error`.

Here are a few tips to help you troubleshoot some of these popular software platforms:

#### Rollback Recent Upgrades

If you recently updated the content management system itself just before the `403 Forbidden Error` appeared, you may want to consider rolling back to the previous version you had installed when things were working fine.  Similarly, any extensions or modules that you may have recently upgraded can also cause server-side issues, so reverting to previous versions of those may also help.  For assistance with this task, simply Google "downgrade [PLATFORM_NAME]" and follow along.  In some cases, however, certain CMSs don't really provide a version downgrade capability, which indicates that they consider the base application, along with each new version released, to be extremely stable and bug-free.  This is typically the case for the more popular platforms, so don't be afraid if you can't find an easy way to revert the platform to an older version.

#### Uninstall New Extensions, Modules, or Plugins

Depending on the particular content management system your application is using, the exact name of these components will be different, but they serve the same purpose across every system: improving the capabilities and features of the platform beyond what it's normally capable of out of the box.  But be warned: such extensions can, more or less, take full control of the system and make virtually any changes, whether it be to the `PHP` code, HTML, CSS, JavaScript, or database.  As such, it may be wise to uninstall any new extensions that may have been recently added.  Again, Google the extension name for the official documentation and assistance with this process.  

#### Check for Unexpected Database Changes

It's worth noting that, even _if_ you uninstall an extension through the CMS dashboard, this doesn't _guarantee_ that changes made by the extension have been fully reverted.  This is particularly true for many WordPress extensions, which are given carte blanche within the application, including full access rights to the database.  Unless the extension author explicitly codes such things in, there are scenarios where an extension may modify database records that don't "belong" to the extension itself, but are instead created and managed by other extensions (or even the base CMS itself).  In those scenarios, the extension may not know how to revert alterations to database records, so it will ignore such things during uninstallation.  Diagnosing such problems can be tricky, but I've personally encountered such scenarios multiple times, so your best course of action, assuming you're reasonably convinced an extension is the likely culprit for the `403 Forbidden Error`, is to open the database and manually look through tables and records that were likely modified by the extension.

#### Confirm Proper File Permissions

If the application was working fine before and suddenly this error occurs, permissions are not a very likely culprit.  However, if modifications were recently made (such as upgrades or installations), it's possible that file permissions were changed or are otherwise incorrect, which could cause an issue to propagate its way throughout the application and eventually lead to a `403 Forbidden Error`.  The [vast majority](https://w3techs.com/technologies/overview/operating_system/all) of servers use Unix-based operating systems, so have a [look here](https://en.wikipedia.org/wiki/File_system_permissions#Traditional_Unix_permissions), or elsewhere on the web, for more information on setting up proper permissions for application files and directories to keep things secure, while also allowing your application access where it's needed.

Above all, Google is your friend.  Don't be afraid to search for specific terms related to your issue, such as the name of your application's CMS, along with the `403 Forbidden Error`.  Chances are you'll find someone (or, perhaps, many someones) who have experienced this issue and have potentially been provided a solution.

### Troubleshooting on the Server-Side

If you aren't running a CMS application -- or even if you are, but you're confident the `403 Forbidden Error` isn't related to that -- here are some additional tips to help you troubleshoot what might be causing the issue on the server-side of things.

#### Check Your Web Server Configuration

Most modern web servers provide one or more configuration files that allow you to easily adjust the server behavior, based on a wide range of circumstances.  For example, the server may be configured to reject requests to certain directories or URLs, which could result in a `403 Forbidden Error`.

Configuration options for each different type of web server can vary dramatically, so we'll just list a few popular ones to give you some resources to look through, depending on what type of server your application is running on:

- [Apache](https://httpd.apache.org/docs/2.4/configuring.html)
- [Nginx](http://nginx.org/en/docs/beginners_guide.html)
- [IIS](https://docs.microsoft.com/en-us/iis/configuration/)
- [Node.js](https://nodejs.org/api/)
- [Apache Tomcat](https://tomcat.apache.org/tomcat-9.0-doc/setup.html)

#### Look Through the Logs

Nearly every web application will keep some form of server-side logs.  `Application logs` are typically the history of what the application did, such as which pages were requested, which servers it connected to, which database results it provides, and so forth.  `Server logs` are related to the actual hardware that is running the application, and will often provide details about the health and status of all connected services, or even just the server itself.  Google "logs [PLATFORM_NAME]" if you're using a CMS, or "logs [PROGRAMMING_LANGUAGE]" and "logs [OPERATING_SYSTEM]" if you're running a custom application, to get more information on finding the logs in question.

#### Check the Database for User Authentication

Since the presence of a `403 Forbidden Error` may indicate that the client was properly authenticated, but isn't being provided access to the requested resource by the server for whatever reason, it's worth checking on the server side for any reason this might be happening.  For example, check the database for proper user authentication, if applicable, to make sure the client is actually being authenticated as the intended user.

#### Verify Server Connectivity

While it may sound simple, it's entirely possible that a `403 Forbidden Error` simply indicates that a server somewhere in the chain is down or unreachable for whatever reason.  Most modern applications don't reside on a single server, but may, instead, be spread over multiple systems, or even rely on many third-party services to function.  If any one of these servers are down for maintenance or otherwise inaccessible, this could result in an error that _appears_ to be from your own application.

#### Debug Your Application Code or Scripts

If all else fails, it may be that a problem in some custom code within your application is causing the issue.  Try to diagnose where the issue may be coming from through manually debugging your application, along with parsing through application and server logs.  Ideally, make a copy of the entire application to a local development machine and perform a step-by-step debug process, which will allow you to recreate the exact scenario in which the `403 Forbidden Error` occurred and view the application code at the moment something goes wrong.

---

__META DESCRIPTION__

A detailed explanation of what a 403 Forbidden Error response is, including troubleshooting tips to help you resolve this error in your own application.

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/HTTP/Status
- https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/403
- https://en.wikipedia.org/wiki/List_of_HTTP_status_codes
- https://en.wikipedia.org/wiki/HTTP_403
