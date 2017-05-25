# iOS Exception Handling

`Airbrake-iOS` provides robust exception tracking capabilities to all of your iOS applications, no matter their scope or size.  With an installation and configuration process that takes less than 3 minutes your iOS application will supply both robust exception tracking and automatic, instantaneous error reporting.  Developed by iOS developers for iOS developers, `Airbrake-iOS` is designed to be directly integrated into your own application code, constantly running behind the scenes and monitoring for any exceptions.  Once an error is detected, `Airbrake-iOS` will immediately alert you and your team that something has gone wrong, no matter when or where the error occurred.  Best of all, `Airbrake-iOS` is deeply integrated into the `Airbrake` service which ensures exception reports are immediately available, however you wish to receive them, whether that be through email alerts, the `Airbrake` web dashboard, or even through the many third-party service integrations that `Airbrake` supports.  Plus, `Airbrake-iOS` automatically syncs up with your active iOS application to report unhandled exceptions, no matter where it's installed, so there's never any need for user-intervention or awkward error reports.

Have a look at some of the great features `Airbrake-iOS` provides and find out why `Airbrake` is used every day by world-class businesses and mobile developers to help them revolutionize their exception handling practices!

## Features

`Airbrake-iOS` allows your iOS application to automatically and instantly report all exceptions to you and your team.  Every unhandled exception (along with a few important Unix signals to boot) is caught by `Airbrake-iOS` before being immediately sent along to `Airbrake`.  These exception reports sent to `Airbrake` then trigger numerous types of notifications ranging from email alerts to `Airbrake` dashboard reports and even to (optional) pushes to any third-party service integrations your team is using.

While `Airbrake-iOS` builds default exception reports that are designed to be beneficial to the widest variety of development teams, `Airbrake-iOS` also makes it a breeze to customize the contextual metadata that is associated with each exception before it is packaged up and reported to you via `Airbrake`.  This makes it easy to include only the exception data you care about most -- stuff that is critical to your specific application and business requirements.  Best of all, both default and custom exception metadata sent by `Airbrake-iOS` can be filtered and searched using the powerful `Airbrake` web dashboard, which gives you and your team detailed control over and access to the exact group of exceptions you wish to investigate.

Take a quick peek below at just a handful of the great features `Airbrake-iOS` has to offer, or head over to the full [documentation](https://github.com/airbrake/airbrake-ios) for all the details and see how your team can quickly and easily improve how you handle exceptions in your mobile apps!

### Instantaneous Exception Notification

`Airbrake-iOS` is designed to recognize and report exceptions immediately and automatically, to ensure that you and your team are never blindsided when it comes to the health of your application.  After a few brief minutes to take care of installation and integration into your application, `Airbrake-iOS` will begin notifying you of all unhandled exceptions that are thrown.  Whether you're notified via email, the `Airbrake` dashboard, or through third-party service integrations, `Airbrake-iOS` has you covered.

In addition to reporting all unhandled exceptions, `Airbrake-iOS` also recognizes the occurrence of and provides notifications for a number of critical Unix signals within your app including:

- `SIGABRT`
- `SIGBUS`
- `SIGFPE`
- `SIGILL`
- `SIGSEGV`
- `SIGTRAP`

Even though `Airbrake-iOS` was designed to make exception handling as simple and as automated as possible, you're never forced to use automatic exception handling with `Airbrake-iOS`.  If your team needs direct control over the subset of exceptions that are reported, or simply to control where in your code exception reports are generated from, `Airbrake-iOS` makes it easy by providing custom exception logging capabilities out of the box.

Feel free to browse through the full [documentation](https://github.com/airbrake/airbrake-ios#signals) for all the details on how `Airbrake-iOS` makes it simple to automate exception reporting in all your iOS applications.

### Minimum Versioning Support

Most mobile applications go through many iterations and version releases throughout their entire development life cycle, so it's only natural that you and your team need a way to keep track of exceptions based on what version of the application they occurred in.  `Airbrake-iOS`, in conjunction with the robust `Airbrake` dashboard, gives you the means to solve this problem through minimum versioning.

For example, imagine your team releases version `1.0.0` of your stellar new app and an exception is detected and immediately reported to you via `Airbrake`.  Even though you then quickly fix the issue that caused that exception and release a new patched version of `1.0.1`, there may be nothing preventing the end-user who caused the error in the first place from keeping the first `1.0.0` version installed and triggering even more of the same exception reports.  While `Airbrake-iOS` makes it easy to suppress errors so they don't appear to the end-user, you don't want to continue being bombarded with exception reports for an issue that you've already fixed in the latest release.

This is where the power of minimum versioning comes in.  Within your `Airbrake` project dashboard simply access the `Settings` page, then set your current (or minimum) application version using [semantic versioning rules](http://semver.org/).  Now, when an exception is reported by `Airbrake-iOS`, the `Airbrake` dashboard will first check if the exception's app version is older than the minimum version you specified, and if so, the exception will be ignored.

Take a look at the versioning [documentation](https://airbrake.io/docs/airbrake-android-ios/app-versions/) for all the details on configuring `Airbrake-iOS` with `Airbrake` to handle exception versioning for your mobile apps.

### Limitless Custom Parameters

By default, all exception reports generated by `Airbrake-iOS` automatically include an abundance of useful metadata such as backtrace, app version, request origin, device type, time stamps, and much more.  Yet, in many cases you and your team need to collect additional contextual information about the exceptions that are being reported, and `Airbrake-iOS` can handle that.  With `Airbrake-iOS` you can directly catch exceptions in your code and then pass additional metadata to the `ABNotifier.logException` method.  These extra fields will be packaged up and sent along with the rest of the exception metadata to the `Airbrake` web dashboard.  Since you have direct control over this parameter object you're free to add near limitless parameters to exception reports.  Best of all, all exception metadata is available directly on the `Airbrake` dashboard, making the data easily filterable and searchable, so your team can quickly identify the most critical exceptions at any given moment and get working on solutions right away.

Check out the [documentation](https://github.com/airbrake/airbrake-ios#custom-exception-logging), or even the actual `Airbrake-iOS` [source code](https://github.com/airbrake/airbrake-ios/blob/master/Airbrake/notifier/ABNotifier.h#L174), and see how you can get started adding custom parameters to your exception reports with `Airbrake-iOS`!

## Quick Setup

1. To begin using `Airbrake-iOS` start by [Creating an Airbrake account](https://airbrake.io/account/new), signing in, and making a new project.

### Install from Source Code

1. Drag the `Airbrake` folder to your project, making sure that `Copy items into destination group's folder (if needed)` is checked and `Create groups for any added folders` is selected.
2. Add `SystemConfiguration.framework` to your project.
3. Add the `CrashReporter.framework` directory from the `Airbrake` source directory to your project.

### Install with Cocoapods

1. If you are using [Cocoapods](https://cocoapods.org/) then simply issue the `pod 'Airbrake-iOS'` command to install `Airbrake-iOS`:

```bash
$ pod 'Airbrake-iOS'
```

### Configure in Objective-C Applications

1. Import the `ABNotifier` header file in your app delegate:

```objective-c
#import "ABNotifier.h"
```

2. Now call the `startNotifierWithAPIKey` method at the beginning of your `application:didFinishLaunchingWithOptions` call.  Be sure to copy and paste the `Project ID` and `Project API Key` values into the appropriate fields, which can be found on the right-hand side of the `Project Settings` page of the `Airbrake` dashboard:

```objective-c
[ABNotifier startNotifierWithAPIKey:@"PROJECT_API_KEY"
                          projectID:@"PROJECT_ID"
                    environmentName:ABNotifierAutomaticEnvironment
                           delegate:self];
```

3. Optionally, you can also change the `environmentName` field value to any of the built-in values provided by `Airbrake-iOS`, which allows you to distinguish between environments in your exception reports (plus you can also [create custom environment variables](https://github.com/airbrake/airbrake-ios#environment-variables)):

- ABNotifierAutomaticEnvironment
- ABNotifierDevelopmentEnvironment
- ABNotifierAdHocEnvironment
- ABNotifierAppStoreEnvironment
- ABNotifierReleaseEnvironment

4. That's all there is to it!  `Airbrake-iOS` is now setup and will begin automatically reporting exceptions to you via `Airbrake`!