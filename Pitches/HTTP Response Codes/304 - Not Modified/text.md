# 304 Not Modified: What It Is and How to Fix It

A **304 Not Modified** message is an [`HTTP response status code`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Status) indicating that the requested resource has not been modified since the previous transmission, so there is no need to retransmit the requested resource to the client.  In effect, a `304 Not Modified` response code acts as an implicit redirection to a cached version of the requested resource.

Often it can be challenging to distinguish between all the possible HTTP response codes and determine the exact cause of errors like the `304 Not Modified` code.  There are _dozens_ of possible HTTP status codes used to represent the complex relationship between the client, a web application, a web server, and the multitude of third-party web services that may be in use, so determining the cause of a particular status code can be challenging.  In this article we'll examine the `304 Not Modified` code by looking at a few troubleshooting tips, along with some potential fixes for common problems that might be causing this issue within your own web applications, so let's get to it!

## The Problem is Server-Side

All HTTP response status codes that are in the `3xx` category are considered `redirection messages`.  Such codes indicate to the user agent (i.e. your web browser) that an additional action is required in order to complete the request and access the desired resource.  Unlike `client error responses` found in the `4xx` codes, like the [`403 Forbidden Error`](https://airbrake.io/blog/http-errors/403-forbidden-error) we looked at recently, which could occur due to either a client- or server-side issue, a `304 Not Modified` code generally indicates an issue on the actual web server hosting your application.

Having said that, the appearance of a `304 Not Modified` is typically not an issue that requires much user intervention on your part.  This is because a `304` code is a response when the original user agent request occurred using a [`safe`](https://developer.mozilla.org/en-US/docs/Glossary/safe) method.  Any HTTP method that doesn't alter the state of the server is considered `safe`.  Thus, any request method that only requires the server to response with a read-only operation would be `safe`.  The most common of these `safe` HTTP methods is [`GET`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Methods/GET), but others include [`HEAD`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Methods/HEAD) and [`OPTIONS`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Methods/OPTIONS).

If the user agent request included either of the special headers [`If-None-Match`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/If-None-Match) or [`If-Modified-Since`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/If-Modified-Since) then the server will check the cached version of the resource against the requested version.  If the version matches then that cached version can be returned, rather than regenerating a new copy.  The [`If-None-Match`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/If-None-Match) header indicates that the [`ETag`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/ETag) response header should be verified, which contains a specific resource version.  On the other hand, the [`If-Modified-Since`](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/If-Modified-Since) with a specific last modified date with which to compare the last modified date of the resource.

Since the `304 Not Modified` indicates that something has gone wrong within the `server` of your application, we can largely disregard the `client` side of things.  If you're trying to diagnose an issue with your own application, you can immediately ignore most client-side code and components, such as HTML, cascading style sheets (CSS), client-side JavaScript, and so forth.  This doesn't apply _solely_ to web sites, either.  Many smart phone apps that have a modern looking user interface are actually powered by a normal web application behind the scenes; one that is simply hidden from the user.  If you're using such an application and a `304 Not Modified` occurs, the issue isn't going to be related to the app installed on your phone or local testing device.  Instead, it will be something on the server-side, which is performing most of the logic and processing behind the scenes, outside the purview of the local interface presented to the user.

If your application is generating unexpected `304 Not Modified` response codes there are a number of steps you can take to diagnose the problem.

## Start With a Thorough Application Backup

As with anything, it's better to have played it safe at the start than to screw something up and come to regret it later on down the road.  As such, it is _critical_ that you perform a full backup of your application, database, and so forth, before attempting any fixes or changes to the system.  Even better, if you have the capability, create a complete copy of the application onto a secondary `staging` server that isn't "live," or isn't otherwise active and available to the public.  This will give you a clean testing ground with which to test all potential fixes to resolve the issue, without threatening the security or sanctity of your live application.

## Diagnosing a 304 Not Modified Response Code

A `304 Not Modified` response code indicates that the requested resource has not been modified since the previous transmission.  This typically means there is no need to retransmit the requested resource to the client, and a cached version can be used, instead.  However, it's possible that the server is improperly configured, which is causing it to incorrectly respond with a `304 Not Modified` code, instead of the standard and expected `200 OK` code of a normal, functional request.  Thus, a large part of diagnosing the issue will be going through the process of double-checking what resources/URLs are generating `304 Not Modified` response codes and determining if these codes are appropriate or not.

If your application is responding with `304 Not Modified` codes that it _should not_ be issuing, this is an issue that many other visitors may be experiencing as well, dramatically hindering your application's ability to service users.  We'll go over some troubleshooting tips and tricks to help you try to resolve this issue.  If nothing here works, don't forget that Google is your friend.  Try searching for specific terms related to your issue, such as the name of your application's CMS or web server software, along with `304 Not Modified`.  Chances are you'll find others who have experienced this issue and have found a solution.

## Troubleshooting on the Server-Side

Here are some additional tips to help you troubleshoot what might be causing the `304 Not Modified` to appear on the server-side of things:

### Confirm Your Server Configuration

Your application is likely running on a server that is using one of the two most popular web server softwares, `Apache` or `nginx`.  At the time of publication, both of these web servers make up [`over 84%` of the world's web server software](https://w3techs.com/technologies/overview/web_server/all)!  Thus, one of the first steps you can take to determine what might be causing these `304 Not Modified` response codes is to check the configuration files for your web server software for unintentional redirect instructions.

To determine which web server your application is using you'll want to look for a key file.  If your web server is Apache then look for an `.htaccess` file within the root directory of your website file system.  For example, if your application is on a shared host you'll likely have a username associated with the hosting account.  In such a case, the application root directory is typically found at the path of `/home/<username>/public_html/`, so the `.htaccess` file would be at `/home/<username>/public_html/.htaccess`.  That said, if you have access to the system Apache configuration file then you should open the `/etc/apache2/httpd.conf` or `/etc/apache2/apache2.conf` file instead.  

The most likely culprit for producing unexpected `304` codes in Apache is the [`mod_cache`](https://httpd.apache.org/docs/2.4/mod/mod_cache.html) module.  Thus, within the configuration file you have open look for a section that checks for the `mod_cache.c` file.  Here's an example from the [official documentation](https://httpd.apache.org/docs/2.4/mod/mod_cache.html):

```conf
LoadModule cache_module modules/mod_cache.so
<IfModule mod_cache.c>
    LoadModule cache_disk_module modules/mod_cache_disk.so
    <IfModule mod_cache_disk.c>
        CacheRoot "c:/cacheroot"
        CacheEnable disk  "/"
        CacheDirLevels 5
        CacheDirLength 3
    </IfModule>

    # When acting as a proxy, don't cache the list of security updates
    CacheDisable "http://security.update.server/update-list/"
</IfModule>
```

Since you don't want to cause irreversible damage, don't delete anything, but instead just temporarily comment out the caching section by adding `#` characters at the start of every line to be commented out.  Save the modified configuration file then restart the Apache web server to see if this fixed the problem.

On the other hand, if your server is running on `nginx`, you'll need to look for a completely different configuration file.  By default this file is named `nginx.conf` and is located in one of a few common directories: `/usr/local/nginx/conf`, `/etc/nginx`, or `/usr/local/etc/nginx`.  By default, `nginx` actually comes with built-in caching, so it is not uncommon for static resources to be cached and for a `304 Not Modified` response code to be sent when refreshing a page or what not.  Thus, troubleshooting for unexpected caching can be a bit more challenging then with Apache.

The main thing you should look for within the `nginx.conf` file is the [`expires`](http://nginx.org/en/docs/http/ngx_http_headers_module.html) directive, which can be used within a `block directive` (i.e. a name set of directives) to define when requested files from within that server should expire (that is, when the cached versions should be refreshed from the server).  For example, here the `expires` configuration maps a handful of content types to differing expiration timestamps:

```
map $sent_http_content_type $expires {
    default                    off;
    text/html                  24h;
    text/css                   24h;
    application/javascript     max;
    ~image/                    max;
}

server {
    listen 80;
    listen 443 ssl;
    server_name www.example.com;
    expires $expires;
}
```

If you find an `expires` directive in your own configuration, try temporarily commenting it out by preceding it with `#` characters.  Save the changed file then restart the server and test if the issue was resolved.

### Look Through the Logs

Nearly every web application will keep some form of server-side logs.  `Application logs` are typically the history of what the application did, such as which pages were requested, which servers it connected to, which database results it provides, and so forth.  `Server logs` are related to the actual hardware that is running the application, and will often provide details about the health and status of all connected services, or even just the server itself.  Google "logs [PLATFORM_NAME]" if you're using a CMS, or "logs [PROGRAMMING_LANGUAGE]" and "logs [OPERATING_SYSTEM]" if you're running a custom application, to get more information on finding the logs in question.

### Debug Your Application Code

If all else fails, it may be that a problem in some custom code within your application is causing the issue.  Try to diagnose where the issue may be coming from through manually debugging your application, along with parsing through application and server logs.  Ideally, make a copy of the entire application to a local development machine and perform a step-by-step debug process, which will allow you to recreate the exact scenario in which the `304 Not Modified` occurred and view the application code at the moment something goes wrong.

No matter what the cause, the appearance of a `304 Not Modified` within your own web application is a strong indication that you may need an error management tool to help you automatically detect such errors in the future.  The best of these tools can even alert you and your team _immediately_ when an error occurs.  <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-304-not-modified">Airbrake's error monitoring software</a> provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize exception parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-304-not-modified">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look at what a 304 Not Modified response code is, including troubleshooting tips to help you resolve this error in your own application.

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/HTTP/Status
- https://developer.mozilla.org/en-US/docs/Web/HTTP/Status/304
- https://en.wikipedia.org/wiki/List_of_HTTP_status_codes
- https://en.wikipedia.org/wiki/HTTP_304
- http://www.checkupdown.com/status/E304.html
- https://tools.ietf.org/html/rfc1945
- https://tools.ietf.org/html/rfc7231