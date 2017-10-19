# 404 Not Found Error: What It Is and How to Fix It

The **404 Not Found Error** is an [`HTTP response status code`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status), which indicates that the requested resource could not be found.  Like most HTTP response codes, and particularly for those codes that indicate an error, the cause of a `404 Not Found Error` can be difficult to track down and resolve.  There are well over _50_ potential status codes that represent the complex relationship between the client, a web application, a web server, and often multiple third-party web services, so determining the cause of a particular status code can be a challenge under the best of circumstances.

In this article we'll explore the `404 Not Found Error` by examining what might cause this error, while providing a few tips and tricks to help you diagnose and debug your own application that is experiencing such issues.  We'll also look at a handful of the most common content management systems (`CMSs`) that are in use today, giving you some insight into potential problem areas within these systems that might cause an unexpected `404 Not Found Error`, so let's get started!

## Server- or Client-Side?

All HTTP response status codes that are in the `4xx` category are considered `client error responses`.  These types of messages contrast with errors in the `5xx` category, such as the [`502 Bad Gateway Error`](https://airbrake.io/blog/http-errors/502-bad-gateway-error) we looked at last week, which are considered `server error responses`.  That said, the appearance of a `4xx` error doesn't necessarily mean the issue is something to do with the client, where the `client` is the web browser or device being used to access the application.  Oftentimes, if you're trying to diagnose an issue with your own application, you can immediately ignore most client-side code and components, such as HTML, cascading style sheets (CSS), client-side JavaScript, and so forth.  This doesn't apply _solely_ to web sites, either.  Many smart phone apps that have a modern looking user interface are actually powered by a normal web application behind the scenes; one that is simply hidden from the user.

That said, since a `404 Not Found Error` indicates that the resource that's trying to be accessed is unavailable, it's entirely possible the issue steps from the client.  You may be trying to access an invalid URL, the browser could be sending invalid credentials to the application, and so on.  We'll explore some of these scenarios (and potential solutions) down below, but be aware that, even though the `404 Not Found Error` is considered a `client error response`, it doesn't inherently mean we can rule out either the client nor the server as the root of the problem.  In these scenarios, the `server` is still the network object that is producing the `404 Not Found Error`, and returning it as the HTTP response code to the client, but it could be that the client is causing the issue in some way.

## Start With a Thorough Application Backup

As with anything, it's better to have played it safe at the start than to screw something up and come to regret it later on down the road.  As such, it is _critical_ that you perform a full backup of your application, database, and so forth, before attempting any fixes or changes to the system.  Even better, if you have the capability, create a complete copy of the application onto a secondary `staging` server that isn't "live," or isn't otherwise active and available to the public.  This will give you a clean testing ground with which to test all potential fixes to resolve the issue, without threatening the security or sanctity of your live application.

## Diagnosing a 404 Not Found Error

As mentioned, a `404 Not Found Error` indicates that the `client` (web browser) is receiving a message from the `server` (remote computer) that the specific resource (web page/URL) is unavailable.  Such an error can occur in a few scenarios:

- The client sent a proper request to the server, and the server successfully received said request, but the server was unable to find a valid resource at that particular location.  This is typically due to an invalid URL specified by the client; one which the server does not recognize as valid for any number of reasons.  This represents the majority of `404 Not Found Errors`, and results in an actual `404` response code being sent by the server.
- Alternatively, some web applications "fake" `404 Not Found Errors` when an invalid resource is requested.  In such cases, the server returns a standard `200 OK` response code, which normally indicates the resource loaded as expected.  However, the server then has a custom "404 page" that it displays, indicating to the user that the requested resource was, in fact, not found.  However, since the server didn't actually return with a `404` response code, such fake errors are typically referred to as `soft 404 errors`.  This is generally considered bad practice on the part of the server/application, since many automated softwares (such as search engine web crawlers) rely on legitimate `404 Not Found Errors` to determine if resources/links are valid.

In both cases, the provided URL _could_ have been valid in the past, but the server has failed to provide a [`server-side redirect`](https://en.wikipedia.org/wiki/Server-side_redirect), which would typically be used to redirect a request made to an invalid or outdated resource to the new, intended target resource.

### Troubleshooting on the Client-Side

Since the `404 Not Found Error` is a `client error response` code, it's best to start by troubleshooting any potential client-side issues that could be causing this error.  Here are a handful of tips to try on the browser or device that is giving you problems.

#### Check the Requested URL

The most common cause of a `404 Not Found Error` is simply inputting an incorrect URL.  [`Domain names`](https://en.wikipedia.org/wiki/Domain_name) (e.g. `airbrake.io`) are case-insensitive, meaning that this mixed case link to [AirBrAKe.IO](https://AirBrAKe.IO) works just as well as the normal, lowercase version of [airbrake.io](https://airbrake.io).  However, [path, query, or fragment](https://en.wikipedia.org/wiki/Uniform_Resource_Identifier#Syntax) portions that appear after the `domain name`, are quite often case-sensitive, unless the application/server configuration is explicitly designed to pre-process all URLs as lowercase before execution.

For example, while `airbrake.io` can be upper, lower, or mixed case, a link to [airbrake.io/BLOG/](https://airbrake.io/BLOG/) (with `BLOG` in uppercase) is invalid, resulting in our good friend the `404 Not Found Error`.  Of course, the lowercase version to [airbrake.io/blog/](https://airbrake.io/blog/) works just fine, as expected.

This is all to say that it's quite common to have a minor typo in some part of a URL, which often results in an unexpected `404 Not Found Error`.

#### Clear Relevant Cookies

As you may already be aware, [`HTTP cookies`](https://en.wikipedia.org/wiki/HTTP_cookie) are tiny pieces of data stored on your local device, which are used by websites and applications as a mechanism to "remember" information about this particular browser and/or device.  Most modern web apps take advantage of cookies to store user- or browser-specific data, identifying the client and allowing for future visits to be faster and easier.

However, cookies can store just about any information they need to.  In many cases, web applications or services -- such as ad networks -- will use data retrieved from local cookies to redirect or handle incoming requests.  Thus, an invalid or corrupted cookie could "confuse" the server into thinking you aren't who it thinks you are, or that you're trying to access a resource that doesn't exist.

In most cases, you only need to concern yourself with cookies that are relevant to the website or application causing the problem.  Cookies are stored based on the web application's `domain name`, so you can explicitly remove only those cookies that match the website domain (e.g. `airbrake.io`), thereby keeping all other cookies intact.  However, if you are unfamiliar with manually removing certain cookies, it's much easier and safer to clear _all_ cookies at once.  

Clearing cookies can be accomplished in different ways, depending on the browser you're using:

- [Google Chrome](https://support.google.com/accounts/answer/32050)
- [Internet Explorer](https://support.microsoft.com/en-us/help/17442/windows-internet-explorer-delete-manage-cookies)
- [Microsoft Edge](https://support.microsoft.com/en-us/help/10607/microsoft-edge-view-delete-browser-history)
- [Mozilla Firefox](https://support.mozilla.org/en-US/kb/delete-cookies-remove-info-websites-stored)
- [Safari](https://support.apple.com/en-us/HT201265)

#### Log Out and Log In

If the application you're using has some form of user authentication, the last client-side step to try is to log out and then log back in.  If you've recently cleared the browser cookies, this should usually log you out automatically the next time you try to load the page, so feel free to just try logging back at this point, to see if things are working once again.  In some situations, the application may be running into a problem with your previous `session`, which is just a string that the `server` sends to the `client` to identify that client during future requests.  As with other data, the `session token` (or `session string`) is stored locally on your device in the cookies and is transferred by the client to the server during every request.  If the server doesn't recognize the session token being sent by the client, or something has gone wrong with the server that indicates that particular token is invalid, you may get a `404 Not Found Error` as a result.

For most web applications, logging out and logging back in will force the local session token to be recreated.

### Debugging Common Platforms

If you're running common software packages on the server that is responding with the `404 Not Found Error`, you may want to start by looking into the stability and functionality of those platforms first.  The most common content management systems -- like WordPress, Joomla!, and Drupal -- are all typically well-tested out of the box, but once you start making modifications to the underlying extensions or `PHP` code (the language in which nearly all modern content management systems are written in), it's all too easy to cause an unforeseen issue that results in a `404 Not Found Error`.

Here are a few tips to help you troubleshoot some of these popular software platforms:

#### Rollback Recent Upgrades

If you recently updated the content management system itself just before the `404 Not Found Error` appeared, you may want to consider rolling back to the previous version you had installed when things were working fine.  Similarly, any extensions or modules that you may have recently upgraded can also cause server-side issues, so reverting to previous versions of those may also help.  For assistance with this task, simply Google "downgrade [PLATFORM_NAME]" and follow along.  In some cases, however, certain CMSs don't really provide a version downgrade capability, which indicates that they consider the base application, along with each new version released, to be extremely stable and bug-free.  This is typically the case for the more popular platforms, so don't be afraid if you can't find an easy way to revert the platform to an older version.

#### Uninstall New Extensions, Modules, or Plugins

Depending on the particular content management system your application is using, the exact name of these components will be different, but they serve the same purpose across every system: improving the capabilities and features of the platform beyond what it's normally capable of out of the box.  But be warned: such extensions can, more or less, take full control of the system and make virtually any changes, whether it be to the `PHP` code, HTML, CSS, JavaScript, or database.  As such, it may be wise to uninstall any new extensions that may have been recently added.  Again, Google the extension name for the official documentation and assistance with this process.  

#### Check for Unexpected Database Changes

It's worth noting that, even _if_ you uninstall an extension through the CMS dashboard, this doesn't _guarantee_ that changes made by the extension have been fully reverted.  This is particularly true for many WordPress extensions, which are given carte blanche within the application, including full access rights to the database.  Unless the extension author explicitly codes such things in, there are scenarios where an extension may modify database records that don't "belong" to the extension itself, but are instead created and managed by other extensions (or even the base CMS itself).  In those scenarios, the extension may not know how to revert alterations to database records, so it will ignore such things during uninstallation.  Diagnosing such problems can be tricky, but I've personally encountered such scenarios multiple times, so your best course of action, assuming you're reasonably convinced an extension is the likely culprit for the `404 Not Found Error`, is to open the database and manually look through tables and records that were likely modified by the extension.

Above all, Google is your friend.  Don't be afraid to search for specific terms related to your issue, such as the name of your application's CMS, along with the `404 Not Found Error`.  Chances are you'll find someone (or, perhaps, many someones) who have experienced this issue and have potentially been provided a solution.

### Troubleshooting on the Server-Side

If you aren't running a CMS application -- or even if you are, but you're confident the `404 Not Found Error` isn't related to that -- here are some additional tips to help you troubleshoot what might be causing the issue on the server-side of things.

#### Check Your Web Server Configuration

Most modern web servers provide one or more configuration files that allow you to easily adjust the server behavior, based on a wide range of circumstances.  For example, the server may be configured to reject requests to certain directories or URLs, which could result in a `404 Not Found Error`.

Configuration options for each different type of web server can vary dramatically, so we'll just list a few popular ones to give you some resources to look through, depending on what type of server your application is running on:

- [Apache](https://httpd.apache.org/docs/2.4/configuring.html)
- [Nginx](http://nginx.org/en/docs/beginners_guide.html)
- [IIS](https://docs.microsoft.com/en-us/iis/configuration/)
- [Node.js](https://nodejs.org/api/)
- [Apache Tomcat](https://tomcat.apache.org/tomcat-9.0-doc/setup.html)

#### Look Through the Logs

Nearly every web application will keep some form of server-side logs.  `Application logs` are typically the history of what the application did, such as which pages were requested, which servers it connected to, which database results it provides, and so forth.  `Server logs` are related to the actual hardware that is running the application, and will often provide details about the health and status of all connected services, or even just the server itself.  Google "logs [PLATFORM_NAME]" if you're using a CMS, or "logs [PROGRAMMING_LANGUAGE]" and "logs [OPERATING_SYSTEM]" if you're running a custom application, to get more information on finding the logs in question.

#### Validate Application Links

There are a number of useful tools that allow you to evaluate your application, or just specific resources, to check that all links are valid and not producing any `404 Not Found Errors`.  For starters, you may want to consider registering your site with the [Google Search Console](https://www.google.com/webmasters/tools/home) (if you haven't done so already).  This tool gives you insight into what Google's own web crawler bots have found while traversing your site.  Any issues will be displayed here for all your registered applications, and can be an easy (and automatic) way to find any invalid links or other site problems.

If you just need to check a particular resource or URL, then the [W3C Link Checker](https://validator.w3.org/checklink) tool will immediately scan the link you provide as you wait, displaying any issues at the bottom.  

#### Debug Your Application Code or Scripts

If all else fails, it may be that a problem in some custom code within your application is causing the issue.  Try to diagnose where the issue may be coming from through manually debugging your application, along with parsing through application and server logs.  Ideally, make a copy of the entire application to a local development machine and perform a step-by-step debug process, which will allow you to recreate the exact scenario in which the `404 Not Found Error` occurred and view the application code at the moment something goes wrong.

---

__META DESCRIPTION__

An in-depth explanation of what a 404 Not Found Error response code is, including tips to help you resolve this error in your own application.

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/HTTP/Status
- https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/404
- https://en.wikipedia.org/wiki/List_of_HTTP_status_codes
- https://en.wikipedia.org/wiki/HTTP_404
