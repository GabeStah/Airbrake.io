# Swift Exception Handling

`Airbrake-Swift` makes capturing and managing exceptions within your Swift applications a breeze.  `Airbrake-Swift` can be installed and configured in mere minutes, giving you and your application both robust exception tracking and automatic, instantaneous error reporting capabilities.  Whether you're developing a basic mobile game or a full-fledged enterprise application, `Airbrake-Swift` has you covered with quick setup and powerful exception alert functionality.  `Airbrake-Swift` is designed to be directly integrated alongside your own application code, so it is always running behind the scenes, monitoring for any errors and alerting you and your team the moment something goes wrong.  Moreover, `Airbrake-Swift` is tightly integrated into the `Airbrake` service, allowing exception reports to be sent to you via email, the `Airbrake` web dashboard, and also through the many third-party service integrations that `Airbrake` supports.  Best of all, `Airbrake-Swift` works in tandem with your active Swift application and automatically notifies you of any unhandled exceptions, so there's never any need for user-intervention and time-consuming error reports.

Take a look at a few of the features `Airbrake-Swift` has to offer and find out why `Airbrake` is a leader in mobile exception handling services!

## Features

`Airbrake-Swift` gives your application the powerful capability of automatically and instantly reporting all exceptions that occur.  Every unhandled exception (plus a few important Unix signals) will be caught by `Airbrake-Swift` and then sent along to `Airbrake`, triggering email alerts, `Airbrake` dashboard reports, and even (optional) pushes to third-party service integrations you may be opted into.

`Airbrake-Swift` automatically reports a great deal of default exception information on its own, but you can also customize the contextual parameters included with each exception, ensuring that every important detail of your specific application state or business logic is included.  Moreover, all data reported by `Airbrake-Swift` can be filtered and searched using the powerful `Airbrake` web dashboard, giving your team direct access to exactly the set of exceptions that contain the specific metadata you care about most.

Have a glance below at just a few of the great features `Airbrake-Swift` has to offer, or head over to the official [documentation](https://github.com/airbrake/airbrake-ios) for all the details and see how you and your team can immediately revolutionize your mobile exception handling practices!

### Unlimited Custom Parameters

When `Airbrake-Swift` reports an exception it automatically includes a plethora of useful data such as backtrace, device type, app version, request origin, time stamps, and much more.  However, in cases where you and your team need to gather additional contextual information about the exceptions that are being reported, `Airbrake-Swift` has your back.  `Airbrake-Swift` makes it easy to catch exceptions in your code and then pass additional parameters to the `ABNotifier.logException` method, which will be packaged up and sent along to the `Airbrake` web dashboard.  You're free to add near limitless parameters to each exception report, and best of all, all that contextual data is filterable and searchable through the advanced `Airbrake` dashboard, making it easy for you and your team to hone in on exactly the error you're trying to find and get to work on a solution.

Check out the [documentation](https://github.com/airbrake/airbrake-ios#custom-exception-logging) and even the [source code](https://github.com/airbrake/airbrake-ios/blob/master/Airbrake/notifier/ABNotifier.h#L174) to find out how to get started adding custom parameters to your exception reports through `Airbrake-Swift`!

### Automatic Exception Notification

`Airbrake-Swift` is built to handle exceptions instantly and automatically, so you're never in the dark about what's going on with your live application.  Once installed and configured within just a few short minutes, `Airbrake-Swift` will notify you of all unhandled exceptions that occur within your application.  It even goes a step beyond by recognizing and issuing notifications for a handful of Unix signals:

- `SIGABRT`
- `SIGBUS`
- `SIGFPE`
- `SIGILL`
- `SIGSEGV`
- `SIGTRAP`

Of course, you are by no means forced into using automatic exception handling.  For situations where you need more control over what exceptions are reported and where in your code they trigger from, `Airbrake-Swift` makes it easy with custom exception logging capabilities right out of the box.

Head over to the full [documentation](https://github.com/airbrake/airbrake-ios#signals) for more details on how `Airbrake-Swift` can begin automating exception reporting in your Swift application.

### Minimum Versioning Support

The `Airbrake` dashboard and the `Airbrake-Swift` library will work in tandem and allow you to ignore exceptions based on the `version` of your application on which the exception was reported.  Since mobile applications tend to go through many iterations and version releases this capability makes it easy to keep track of which exceptions are new or should be taken note of and which are still being reported, even after you've already fixed them in a newer release.

Within your `Airbrake` project dashboard simply access the `Settings` page then set your current (or minimum) application version, using [semantic versioning rules](http://semver.org/).  Now, when an exception is reported by `Airbrake-Swift`, the `Airbrake` dashboard will first check if the exception's app version is older than the minimum version you specified, and if so, the exception will be ignored on your behalf.

Have a look at the [versioning documentation](https://airbrake.io/docs/airbrake-android-ios/app-versions/) for more details on how to setup `Airbrake-Swift` and `Airbrake` to handle exception versioning for your mobile applications.

## Quick Setup

_Note: `Airbrake-Swift` uses the [`Airbrake-iOS`](https://github.com/airbrake/airbrake-ios) library for Swift application integration._

1. To begin using `Airbrake-Swift` start by [Creating an Airbrake account](https://airbrake.io/account/new), signing in, and making a new project.

### Install via Source Code as Framework

1. Drag the `Airbrake` folder to your project, making sure that `Copy items into destination group's folder (if needed)` is checked and `Create groups for any added folders` is selected.
2. Add `SystemConfiguration.framework` to your project.
3. Add the `CrashReporter.framework` directory from the `Airbrake` source directory to your project.
4. Add `Airbrake-iOS` to the `podfile`:

```
use_frameworks!
pod 'Airbrake-iOS'
```

5. Next `import Airbrake-iOS` in your app delegate.
6. Set up the `ABNotifer` in your app delegate at the beginning of your `func application(application: UIApplication!, didFinishLaunchingWithOptions launchOptions: NSDictionary!) -> Bool {` call.  Be sure to copy and paste the `Project ID` and `Project API Key` values into the appropriate fields, which can be found on the right-hand side of the `Project Settings` page of the `Airbrake` dashboard:

```objective-c
ABNotifier.startNotifierWithAPIKey(
  PROJECT_API_KEY,
  projectID: PROJECT_ID,
  environmentName: ABNotifierAutomaticEnvironment
);
```

7. You're all set and `Airbrake-Swift` is now configured and ready to report exceptions!

### Install via Cocoapods as Static Library

1. If you are using [Cocoapods](https://cocoapods.org/) then simply issue the `pod 'Airbrake-iOS'` command to install `Airbrake-iOS`:

```bash
$ pod 'Airbrake-iOS'
```

2. Add a new file to your project using the `Header File` template and name the file `[ProjectName]_Bridging_Header.h` then save it to the project's root directory.
3. Open `[ProjectName]_Bridging_Header.h` and `#import` the `ABNotifier` header file:

```objective-c
#ifndef [ProjectName]_Bridging_Header
#define [ProjectName]_Bridging_Header
#import "ABNotifier.h"
#endif
```

4. In your `Project Build Settings`, find `Swift Compiler – Code Generation`, and next to `Objective-C Bridging Header` add the `[ProjectName]_Bridging_Header.h` file.

5. Finally, set up the `ABNotifer` in your app delegate at the beginning of your `func application(application: UIApplication!, didFinishLaunchingWithOptions launchOptions: NSDictionary!) -> Bool {` call.  Be sure to copy and paste the `Project ID` and `Project API Key` values into the appropriate fields, which can be found on the right-hand side of the `Project Settings` page of the `Airbrake` dashboard:

```objective-c
ABNotifier.startNotifierWithAPIKey(
  PROJECT_API_KEY,
  projectID: PROJECT_ID,
  environmentName: ABNotifierAutomaticEnvironment,
  useSSL: true
);
```

6. You're all done and ready to start reporting exceptions!