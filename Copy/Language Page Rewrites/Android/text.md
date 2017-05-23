# Android Exception Handling

`Airbrake-Android` makes it easy to integrate robust exception tracking and automatic, instantaneous error reporting into all your Android mobile applications.  Whether you're developing a simple prototype or a massive end-user product, `Airbrake-Android` has you covered with easy setup and powerful exception alert capabilities.  Since `Airbrake-Android` is integrated directly into your application and running behind the scenes at all times, you and your team will be immediately alerted the very moment something goes wrong.  With tight integration into the `Airbrake` service, `Airbrake-Android` can trigger exception reports via email, the `Airbrake` web dashboard, and also through the many third-party service integrations you may have configured.  Best of all, `Airbrake-Android` works seamlessly with your running application and is completely automatic, so there's no need for user-intervention and time-consuming error reports.

Have a look at some of the features `Airbrake-Android` has to offer and find out why `Airbrake` has become the go-to service for mobile exception handling!

## Features

`Airbrake-Android` provides automatic and instantaneous exception handling and reporting from all your Android applications.  Every error that is thrown in your application will be immediately captured by `Airbrake-Android` and sent along to `Airbrake`, where you'll be alerted to the problem via email, the `Airbrake` dashboard, and (optionally) through the many third-party service integrations that `Airbrake` supports.

While `Airbrake-Android` will report exceptions with an abundance of useful default information about each error, you can also opt to modify and add additional contextual data to each exception record _before_ it is reported.  All metadata associated with exception reports will automatically appear on the `Airbrake` dashboard, which provides powerful and robust filter and search tools, so you can sift through all the detritus to find the exact issue details you're looking for and get started on a solution right away.

Have a glance below at just a few of the great features `Airbrake-Android` has to offer, or head over to the [documentation](https://github.com/airbrake/airbrake-android) for all the details and see how your team can start revolutionizing their Android exception handling practices immediately!

### Automatic Exception Reporting

`Airbrake-Android` is designed to be locally built and then directly integrated, as a library dependency, into your own Android application.  This means your application is able to consistently and instantly detect errors and push them through the `Airbrake-Android` library, automatically reporting them to you via the `Airbrake` dashboard and elsewhere.  Best of all, since `Airbrake-Android` is running locally and behind-the-scenes, all exceptions that you care about are reported by the library itself, without requiring the end-user to contact support or send you a convoluted error report.  In fact, in some cases you may even wish to hide the display of error messages from the end-user entirely, allowing you and your team to continue receiving error reports while your end-users remain none the wiser.

### Simple Installation

Most Android developers will be familiar with adding additional library dependencies to their application and `Airbrake-Android` is just as easy to add as any other library.  After quickly generating a new build using the [`Apache Ant`](http://ant.apache.org/) Java library, `Airbrake-Android` can be added to the list of other library dependencies in Android Studio, IntelliJ IDEA, or whatever Android development environment you're using.  Once added, a handful of extra code lines are all that's needed to get `Airbrake-Android` setup within your project and sending error reports your way.

Check out our [Quick Setup](#quick-setup) instructions below to see exactly how you can get `Airbrake-Android` integrated into your own application in just a few minutes.

### Limitless Custom Parameters

Since Android applications are based in Java, just like the [`Airbrake-Java`](https://airbrake.io/languages/java_bug_tracker) exception handling tool, `Airbrake-Android` gives you direct access to the exception record _before_ it is packaged up and reported to you through the `Airbrake` dashboard.  This provides you with full control over the metadata that is associated with each exception report.  Including additional contextual information with each error report is a breeze, whether it may be extra device info or custom business data based on your own application requirements -- you're only limited by your imagination!

Check out the `Airbrake-Android` [documentation](https://github.com/airbrake/airbrake-android) and the `Airbrake-Java` [documentation](https://github.com/airbrake/airbrake-java) for more details on modifying contextual parameters within your error reports.

## Quick Setup

1. To begin using `Airbrake-Android`, start by [Creating an Airbrake account](https://airbrake.io/account/new), signing in, and making a new project.
2. Clone the [Airbrake-Android GitHub repository](https://github.com/airbrake/airbrake-android) to a local directory.
3. Open the `build.xml` file and verify that the `env.ANDROID_HOME` and `lib.dir` path property values point to your Android SDK and platform directories:

```xml
<project default="package">
    <!-- ... -->
    <!-- Path to your Android SDK directory -->
    <property name="env.ANDROID_HOME" value="/usr/local/android_sdk"/>
    <!-- Path within Android SDK to your installed Android platform -->
    <property name="lib.dir" value="${env.ANDROID_HOME}/platforms/android-7/" />
    <!-- ... -->
</project>
```

4. Now from a terminal window build a new `airbrake-android-<version>.jar` package file using [`Apache Ant`](http://ant.apache.org/):

```bash
$ ant package
```

5. Copy and paste the `build/airbrake-android-<version>.jar` file that you just created into the `libs/` directory of your Android application (or wherever you store library dependencies).
6. Within your code `import` the `Android-Airbrake` notifier library at the top of your application's main `Activity` class file:

```java
// Airbrake-Android library
import com.loopj.android.airbrake.AirbrakeNotifier;
```

7. Within the `onCreate()` method of your main `Activity` class call the `AirbrakeNotifier.register()` method.  Be sure to pass the `Project API Key` as the second argument, which can be found on the right-hand side of the `Project Settings` page of the `Airbrake` dashboard:

```java
public class MainActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        // ...

        // Register Airbrake-Android notifier
        AirbrakeNotifier.register(this, "PROJECT_API_KEY");
        
        // ...
    }
}
```

8. Now make a call to the `AirbrakeNotifier.notify()` method anywhere you wish to catch and report exceptions:

```java
try {
    // Throw a new Exception
    throw new Exception("Something went wrong");
} catch (Exception e) {
    // Catch exceptions and report them via AirbrakeNotifier
    AirbrakeNotifier.notify(e);
}
```

9. That's it!  Now when an exception is thrown by your app `Airbrake-Android` will instantly and automatically report it to you via the `Airbrake` dashboard!