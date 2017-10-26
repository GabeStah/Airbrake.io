People who develop and maintain web applications usually dread a 500 Internal Server Error. This isn't because these errors are usually hard to fix, however. It's because the error is usually not specific and can occur for a number of different reasons. The best way to debug the error depends upon the server and what's actually running at the time. Consider some debugging tips to help diagnose and fix common causes of this problem.

## Diagnosing a 500 Internal Server Error

It's important to note that the server side of an application generates this error even though you may see the error code inside your browser. That means that your HTML, client-side JavaScript, or anything else that runs in a browser is not the source of a 500 Internal Server Error. Of course, the name does imply that it's a server error, but systems have grown so complex today, that this reminder could be helpful.

## Debugging Common Platforms

Is the server running a common software set like WordPress, Joomla, or Drupal? Obviously, production versions of well-tested software like this should not cause a 500 Internal Server Error if everything is set correctly. It still happens because of incompatible versions, bad installations, or server permissions that were not set correctly.

These are some common problems that might cause an error like this with a popular and well-used software platform:

- If the software was just upgraded to a new version, it's likely that the upgrade failed and needs to be refreshed. There may be instructions to help with this on the vendor's website. If the software was just installed, something may have failed in the installation process.
- If a new plugin or theme was just activated, it's probably a good idea to roll that change back and try again. Even well-tested plugins might conflict with other plugins in the installation.
- If the software has been upgraded, older plugins or themes might not be compatible with the upgrade. The only course of action is to start deactivating things until the error goes away. That doesn't directly solve the problem; however, it will find the culprit. It might be possible to get the plugin developer to release an update.
- The host may not have permissions set correctly. Alternatively, there might be a problem with the directory's .htaccess file. Scripts, files, or other resources can't be accessed, so the host just displays a general error.

When common software scripts and packages generate a 500 Internal Server Error, the best place to look for general answers is on the support sites for these platforms. If this error has happened to one user, it has probably happened to multiple users, has been reported upon, and has generated discussion. Of course, it usually happens because of certain circumstances, so it's important to look for help with an idea of what might have changed or gone wrong since the last time the software ran correctly.

## Debugging Server-Side Scripts

This problem might also occur because of a custom script that is just getting developed and tested. In order to find and debug the issue, consider some common solutions to the dreaded 500 Internal Server Error:

- **Server permission**: Very commonly, incorrect permissions on a file or folder that contains one of the scripts causes this this error. The script isn't throwing the error because the server can't even run the script. Check to see what the permissions should be and ensure that's how they are set on the server.
- **Script timeout**: A PHP or other server-side script may have timed out. This could be because of a programming error that put the script in an endless loop or a resource that wasn't available.
- **Server timeout**: If the server was busy, needed to be rebooted, or the connection was lost, the script might still report a 500 Internal Server Error. It's possible that the error might not happen the next time. This is still something to check out. If a script times out during testing, it's likely to do it again during production when it's accessed by more users.
- **Errors in .htaccess files**: Occasionally, an error in the coding of an .htaccess file may cause this error. Certainly, it's a good idea to check this file if none of the situations listed above apply.
- **Errors in a script**: If a custom script generated the error, it might be possible to get that script to offer some more specific information. For example, a PHP script could have display_errors turned on in order to either send specific errors to a log file or display them on the screen. Other server-side languages have similar functionality. The runtime environment might have defaulted to hide errors from users, but this isn't helpful for debugging.

## Get Help From a Server Administrator

In some cases, developers don't have control over everything in their server's environment. If the script runs on a third-party host, that host might help in a few different ways:

- Look for documentation that is specific to the server about common reasons for this error. Different operating systems and even different configurations may generate this error for a variety of reasons.
- Ask the server provider to help by accessing error logs that may contain more clues. For example, the technician or administrator should be able to find out if the server happened to be busy or down when the browser reported the error. This may be the only way to solve some intermittent errors.
Typically, a 500 Internal Server Error is Easy to Fix

Really, in most cases, very simple problems cause this error. These problems are very easy to fix. The issue is that the error is so general that finding the bug can sometimes seem like looking for a needle in a haystack. It's typically easiest to resolve this error if the software developer or tester can remember what conditions changed to start causing the error.

Keep in mind, that these changes may have been made by someone other than the developer -- like a host administrator. If nothing has truly changed, it's likely that the host itself caused the error because the environment was incompatible with software or there were performance problems.

At the end of the day, the appearance of a `500 Internal Server Error` is a strong indication that you may need an error management tool that will automatically detect problems and report them to you and your fellow team members the instant they happen.  <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-500-internal-server-error">Airbrake's error monitoring software</a> provides real-time error monitoring and automatic exception reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize exception parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-500-internal-server-error">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

Written by: Andrew Powell-Morse, Airbrake.io
CTA written by: Gabe Wyatt, GabeWyatt.com