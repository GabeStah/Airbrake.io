# 400 Bad Request Error: What It Is and How to Fix It

The **400 Bad Request Error** is an [`HTTP response status code`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status) that indicates that the server was unable to process the request sent by the client due to invalid syntax.  As with the dozens of potential HTTP response codes, receiving a `400 Bad Request Error` while accessing your own application can be both frustrating and challenging to fix.  Such HTTP response codes represent the complex relationship between the client, a web application, a web server, and often multiple third-party web services, so determining the cause of a particular status code can be a difficult, even within a controlled development environment.

Throughout this article we'll examine the `400 Bad Request Error` by digging into whether the root cause is on the local client or remote server.  We'll also go over a few tips and tricks to help you diagnose and debug your own application if it's reporting a `400 Bad Request Error` for some reason.  Lastly, we'll explore a handful of the most common content management systems (`CMSs`) that are in use today and provide you with some insight into potential problem areas within these systems that might cause an unexpected `400 Bad Request Error`, so let's get to it!

## Server- or Client-Side?

All HTTP response status codes that are in the `4xx` category are considered `client error responses`.  These types of messages contrast with errors in the `5xx` category, such as the [`504 Gateway Timeout Error`](https://airbrake.io/blog/http-errors/504-gateway-timeout-error) we looked at last week, which are considered `server error responses`.  With that in mind, the appearance of a `4xx` error doesn't necessarily mean the issue has something to do with the client, where the `client` is the web browser or device being used to access the application.  Oftentimes, if you're trying to diagnose an issue with your own application, you can immediately ignore most client-side code and components, such as HTML, cascading style sheets (CSS), client-side JavaScript, and so forth.  This doesn't apply _solely_ to web sites, either.  Many smart phone apps that have a modern looking user interface are actually powered by a normal web application behind the scenes; one that is simply hidden from the user.

On the other hand, since a `400 Bad Request Error` indicates that the request sent by the client was invalid for one reason or another, it's entirely possible the issue steps from the client.  Your client may be trying to send a file that's too big, the request could be malformed in some way, the request HTTP headers could be invalid, and so forth.  We'll explore some of these scenarios (and potential solutions) down below, but be aware that, even though the `400 Bad Request Error` is considered a `client error response`, it doesn't inherently mean we can rule out either the client nor the server as the root of the problem.  In these scenarios, the `server` is still the network object that is producing the `400 Bad Request Error`, and returning it as the HTTP response code to the client, but it could be that the client is causing the issue in some way.

## Start With a Thorough Application Backup

It's always better to be safer rather than sorry.  This is particularly true when making modifications to your own website or application. As such, it is _critical_ that you perform a full backup of your application, database, and so forth, _before_ attempting any fixes or changes to the system.  Even better, if you have the capability, create a complete copy of the application onto a secondary `staging` server that isn't "live," or isn't otherwise active and available to the public.  This will give you a clean testing ground with which to test all potential fixes to resolve the issue, without threatening the security or sanctity of your live application.

## Diagnosing a 400 Bad Request Error

A [`400 Bad Request Error`](http://httpwg.org/specs/rfc7231.html#status.400) indicates that the `server` (remote computer) is unable (or refuses) to process the request sent by the `client` (web browser), due to an issue that is _perceived_ by the server to be a client problem.  There are a wide variety of scenarios in which a `400 Bad Request Error` could appear in an application, but below are some of the most likely causes:

- The client may be accidentally (or intentionally) sending `deceptive request routing` information.  Some web applications/web servers look for custom HTTP headers to process requests and verify the client isn't attempting anything malicious.  If an expected custom HTTP header is missing or invalid, a `400 Bad Request Error` is a likely result.
- The client may be uploading a file that is too large.  Most web servers or applications have an explicit file size limit that prevents files that are too big from being uploaded and clogging up bandwidth and other resources in the server.  In many cases, the server will produce a `400 Bad Request Error` when a file is too large (and, thus, the request cannot be completed).
- The client is accessing an invalid URL.  If the client is sending a request to an invalid URL -- particularly one that is malformed via improper characters -- this could result in a `400 Bad Request Error`.
- The client is using an invalid or expired local cookie.  Again, this could be malicious or accidental, but it's possible that a local cookie in the web browser is identifying you via a `session cookie`.  If this particular session token matches the session token from _another_ request from a different client, the server/application may see this is a malicious act and produce a `400 Bad Request Error` code. 

### Troubleshooting on the Client-Side

Since the `400 Bad Request Error` is a `client error response` code, it's best to start by troubleshooting any potential client-side issues that could be causing this error.  Here are a handful of tips to try on the browser or device that is giving you problems.

#### Check the Requested URL

As mentioned, the most common cause of a `400 Bad Request Error` is simply inputting an incorrect URL.  [`Domain names`](https://en.wikipedia.org/wiki/Domain_name) (e.g. `airbrake.io`) are case-insensitive, meaning that this mixed case link to [AirBrAKe.IO](https://AirBrAKe.IO) works just as well as the normal, lowercase version of [airbrake.io](https://airbrake.io).  However, [path, query, or fragment](https://en.wikipedia.org/wiki/Uniform_Resource_Identifier#Syntax) portions that appear after the `domain name`, are quite often case-sensitive, unless the application/server configuration is explicitly designed to pre-process all URLs as lowercase before execution.

Most importantly, check the URL for improper special characters that don't belong.  If the server received a malformed URL, it's likely to produce a `400 Bad Request Error` response.

#### Clear Relevant Cookies

As discussed above, one potential cause of a `400 Bad Request Error` is an invalid or duplicate local cookie.  [`HTTP cookies`](https://en.wikipedia.org/wiki/HTTP_cookie) are tiny pieces of data stored on your local device, which are used by websites and applications as a mechanism to "remember" information about this particular browser and/or device.  Most modern web apps take advantage of cookies to store user- or browser-specific data, identifying the client and allowing for future visits to be faster and easier.

However, a cookie that stores session information about your particular user account or device could be conflicting with another session token from another user, giving one (or both of you) a `400 Bad Request Error`.

In most cases, you only need to concern yourself with cookies that are relevant to the website or application causing the problem.  Cookies are stored based on the web application's `domain name`, so you can explicitly remove only those cookies that match the website domain (e.g. `airbrake.io`), thereby keeping all other cookies intact.  However, if you are unfamiliar with manually removing certain cookies, it's much easier and safer to clear _all_ cookies at once.  

Clearing cookies can be accomplished in different ways, depending on the browser you're using:

- [Google Chrome](https://support.google.com/accounts/answer/32050)
- [Internet Explorer](https://support.microsoft.com/en-us/help/17442/windows-internet-explorer-delete-manage-cookies)
- [Microsoft Edge](https://support.microsoft.com/en-us/help/10607/microsoft-edge-view-delete-browser-history)
- [Mozilla Firefox](https://support.mozilla.org/en-US/kb/delete-cookies-remove-info-websites-stored)
- [Safari](https://support.apple.com/en-us/HT201265)

#### Upload a Smaller File

If you're experiencing a `400 Bad Request Error` while uploading a file of some sort, try testing with a different, much smaller file to see if this resolves the `400 Bad Request Error`.  This includes file "uploads" that don't actually come from your local computer -- even files sent from other computers are considered "uploads" from the perspective of the web server running your application.

#### Log Out and Log In

If the application you're using has some form of user authentication, the last client-side step to try is to log out and then log back in.  If you've recently cleared the browser cookies, this should usually log you out automatically the next time you try to load the page, so feel free to just try logging back at this point, to see if things are working once again.  Similar to the local cookie issue, the application may be running into a problem with your previous `session`, which is just a string that the `server` sends to the `client` to identify that client during future requests.  As with other data, the `session token` (or `session string`) is stored locally on your device in the cookies and is transferred by the client to the server during every request.  If the server believes your session token is invalid or compromised you may get a `400 Bad Request Error` as a result.

For most web applications, logging out and logging back in will force the local session token to be recreated.

### Debugging Common Platforms

If you're running common software packages on the server that is responding with the `400 Bad Request Error`, you may want to start by looking into the stability and functionality of those platforms first.  The most common content management systems -- like WordPress, Joomla!, and Drupal -- are all typically well-tested out of the box, but once you start making modifications to the underlying extensions or `PHP` code (the language in which nearly all modern content management systems are written in), it's all too easy to cause an unforeseen issue that results in a `400 Bad Request Error`.

#### Rollback Recent Upgrades

If you recently updated the content management system itself just before the `400 Bad Request Error` appeared, you may want to consider rolling back to the previous version you had installed when things were working fine.  Similarly, any extensions or modules that you may have recently upgraded can also cause server-side issues, so reverting to previous versions of those may also help.  For assistance with this task, simply Google "downgrade [PLATFORM_NAME]" and follow along.  In some cases, however, certain CMSs don't really provide a version downgrade capability, which indicates that they consider the base application, along with each new version released, to be extremely stable and bug-free.  This is typically the case for the more popular platforms, so don't be afraid if you can't find an easy way to revert the platform to an older version.

#### Uninstall New Extensions, Modules, or Plugins

Depending on the particular content management system your application is using, the exact name of these components will be different, but they serve the same purpose across every system: improving the capabilities and features of the platform beyond what it's normally capable of out of the box.  But be warned: such extensions can, more or less, take full control of the system and make virtually any changes, whether it be to the `PHP` code, HTML, CSS, JavaScript, or database.  As such, it may be wise to uninstall any new extensions that may have been recently added.  Again, Google the extension name for the official documentation and assistance with this process.  

#### Check for Unexpected Database Changes

It's worth noting that, even _if_ you uninstall an extension through the CMS dashboard, this doesn't _guarantee_ that changes made by the extension have been fully reverted.  This is particularly true for many WordPress extensions, which are given carte blanche within the application, including full access rights to the database.  Unless the extension author explicitly codes such things in, there are scenarios where an extension may modify database records that don't "belong" to the extension itself, but are instead created and managed by other extensions (or even the base CMS itself).  In those scenarios, the extension may not know how to revert alterations to database records, so it will ignore such things during uninstallation.  Diagnosing such problems can be tricky, but I've personally encountered such scenarios multiple times, so your best course of action, assuming you're reasonably convinced an extension is the likely culprit for the `400 Bad Request Error`, is to open the database and manually look through tables and records that were likely modified by the extension.

Above all, Google is your friend.  Don't be afraid to search for specific terms related to your issue, such as the name of your application's CMS, along with the `400 Bad Request Error`.  Chances are you'll find someone (or, perhaps, many someones) who have experienced this issue and have potentially been provided a solution.

### Troubleshooting on the Server-Side

If you aren't running a CMS application -- or even if you are, but you're confident the `400 Bad Request Error` isn't related to that -- here are some additional tips to help you troubleshoot what might be causing the issue on the server-side of things.

#### Check for Invalid HTTP Headers

This may be tricky for non-developer types, but it's possible that the `400 Bad Request Error` you're seeing from your own application is a result of missing or invalid custom HTTP headers that your server/web application expects.  In such cases, you may be able to analyze the HTTP headers that are sent on the server side and determine if they are invalid or unexpected in some way.

#### Look Through the Logs

Nearly every web application will keep some form of server-side logs.  `Application logs` are typically the history of what the application did, such as which pages were requested, which servers it connected to, which database results it provides, and so forth.  `Server logs` are related to the actual hardware that is running the application, and will often provide details about the health and status of all connected services, or even just the server itself.  Google "logs [PLATFORM_NAME]" if you're using a CMS, or "logs [PROGRAMMING_LANGUAGE]" and "logs [OPERATING_SYSTEM]" if you're running a custom application, to get more information on finding the logs in question.

#### Debug Your Application Code or Scripts

If all else fails, it may be that a problem in some custom code within your application is causing the issue.  Try to diagnose where the issue may be coming from through manually debugging your application, along with parsing through application and server logs.  Ideally, make a copy of the entire application to a local development machine and perform a step-by-step debug process, which will allow you to recreate the exact scenario in which the `400 Bad Request Error` occurred and view the application code at the moment something goes wrong.

No matter the cause -- and even if you managed to fix it this time -- the appearance of an issue like the `400 Bad Request Error` within your own application is a good indication you may want to implement an error management tool, which will help you automatically detect errors and report them to you at the very moment they occur.  <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-400-bad-request">Airbrake's error monitoring software</a> provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize exception parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-400-bad-request">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

An in-depth explanation of what a 400 Bad Request Error response code is, including tips to help you resolve this error in your own application.

__SOURCES__

- http://httpwg.org/specs/rfc7231.html#status.4xx
- https://developer.mozilla.org/en-US/docs/Web/HTTP/Status
- https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/400
- https://en.wikipedia.org/wiki/List_of_HTTP_status_codes
- https://en.wikipedia.org/wiki/HTTP_400
