TODO: Change `a-m` to `Airbrake-Magento`

# Magento Exception Handling

Magento is one of the leading eCommerce platforms on the market for PHP-centric applications and with the power of the `a-m` plugin, you can quickly and easily enable the power of real-time error monitoring and automatic exception reporting across your entire Magento application in just a couple minutes.  

The `Airbrake-Java` library provides real-time error monitoring and automatic exception reporting for all your Java-based projects.  Tight integration with `Airbrake.io's` state of the art web dashboard ensures that `Airbrake-Java` gives you round-the-clock status updates on your application's health and error rates.  `Airbrake-Java` easily integrates with all the latest Java frameworks and platforms like `Spring`, `Maven`, `log4j`, `Struts`, `Kotlin`, `Grails`, `Groovy`, and many more.  Plus, `Airbrake-Java` allows you to easily customize exception parameters and gives you full, configurable filter capabilities so you only gather the errors that matter most.

Check out all the amazing features `Airbrake-Java` has to offer and see for yourself why so many of the world's best engineering teams are using `Airbrake` to revolutionize their exception handling practices!

## Features

`Airbrake-Java` is loaded with features designed by (and built for) Java developers.  No matter the business requirements you're aiming to meet, the frameworks you're using, the software stack you're integrating with, or the overall size of your project, `Airbrake-Java` makes it easy to stay on top of any application issues.  `Airbrake-Java` generates automatic, instantaneous exception reports the moment an issue occurs, whether that's within an enclosed development environment or even out in the wilds of live production.  Best of all, exceptions are sent to you and your team without the need for user-intervention or writing out cumbersome error reports.  With instant email alerts, robust exception filters, unlimited customized parameters, and full Java support, you and your team will be constantly informed and able to respond immediately when an unexpected issue occurs.

Have a glance a few of the great features below or head over to the full [documentation](https://github.com/airbrake/airbrake-java) to find out how `Airbrake-Java` can help you with your next project!

### Extensive Framework and Library Support

`Airbrake-Java` works great out of the box with all plain Java applications.  However, many modern projects take advantage of the latest and greatest Java frameworks and libraries, so `Airbrake-Java` is built to easily integrate with any third-party framework your may application rely on.  `Airbrake-Java` works well with everything from Apache `Maven` and `log4j` to `Spring` and `Struts`.  In just a few minutes you'll have `Airbrake-Java` integrated and directly reporting exceptions from within your existing Java app.  Plus, when integrated with a powerful logging tool like `log4j`, `Airbrake-Java` will automatically track and send exceptions to `Airbrake.io` without requiring you to write any additional code beyond the brief initial configuration.

Feel free to have a look at the [documentation](https://github.com/airbrake/airbrake-java#setting-up-with-maven) for a full overview on configuring `Airbrake-Java` with third-party frameworks and libraries.

### Unlimited Custom Parameters

All exceptions sent to the `Airbrake.io` dashboard include all the basic information you'd expect, such as the error message, backtrace, environment, exception origin, and more.  However, there are often many situations where you want to associate extra information with exceptions, making them easier to manage and filter with the robust search tools found in the `Airbrake.io` dashboard, or through one of the many third-party integrations `Airbrake.io` offers.

To meet this demand the `Airbrake-Java` library makes it easy to associate near-limitless parameters with each and every exception your application may encounter.  Whether you want to attach user information during a failed login exception, product data when a checkout fails, or anything else you can imagine, the `Airbrake-Java` tool makes it easy!

### Robust Exception Filtering

`Airbrake-Java` makes it a breeze to filter the exceptions your application reports to the `Airbrake.io` dashboard, so you can ensure only the most important issues are highlighted.  Moreover, you can also quickly filter out custom data fields, ensuring no private or sensitive information is sent to `Airbrake.io`, nor to any integrated third-party services.  `Airbrake-Java` also makes it painless to filter environment or system properties associated with each error, ensuring only the most critical information remains, before being sent along to `Airbrake.io` and wherever else you need it!

## Installation

### Install via Maven and Log4j

Getting `Airbrake-Java` integrated with a `Maven` project only takes a few minutes.

1. [Create an Airbrake account](https://airbrake.io/account/new) and sign in.
2. Create a new project then copy the `Project API Key` to your clipboard, which can be found on the right-hand side of the `Project Settings` page.
3. Add the `Airbrake-Java` dependency to your Maven Project Object Model file (`pom.xml`), like so:

```xml
<project xmlns="http://maven.apache.org/POM/4.0.0" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
  xsi:schemaLocation="http://maven.apache.org/POM/4.0.0 http://maven.apache.org/xsd/maven-4.0.0.xsd">
  <modelVersion>4.0.0</modelVersion>

  <groupId>myGroup</groupId>
  <artifactId>myArtifact</artifactId>
  <version>1.0-SNAPSHOT</version>
  <packaging>jar</packaging>

  <name>myArtifact</name>
  <url>http://maven.apache.org</url>

  <dependencies>
    <dependency>
      <groupId>io.airbrake</groupId>
      <artifactId>airbrake-java</artifactId>
      <version>2.2.8</version>
    </dependency>
  </dependencies>
</project>
```

4. Configure `log4j` to include the `Airbrake-Java` appender, along with the `Project API Key` you copied earlier.  Here we're using a `log4j.properties` file, but any form of configuration is acceptable:

```
log4j.rootLogger=INFO, stdout, airbrake

log4j.appender.stdout=org.apache.log4j.ConsoleAppender
log4j.appender.stdout.layout=org.apache.log4j.PatternLayout
log4j.appender.stdout.layout.ConversionPattern=[%d,%p] [%c{1}.%M:%L] %m%n

log4j.appender.airbrake=airbrake.AirbrakeAppender
log4j.appender.airbrake.api_key=PROJECT_API_KEY
// Change environment as desired.
log4j.appender.airbrake.env=development
log4j.appender.airbrake.enabled=true
log4j.appender.airbrake.url=http://api.airbrake.io/notifier_api/v2/notices
```

5. That's it!  Now, any exceptions passed to the `error()` method of a `log4j` `Logger` class instance will be automatically sent to `Airbrake.io` via the `Airbrake-Java` library.

### Install Manually

Installing `Airbrake-Java` within plain Java projects is also a piece of cake.

1. [Create an Airbrake account](https://airbrake.io/account/new) and sign in.
2. Create a new project then copy the `Project API Key` to your clipboard, which can be found on the right-hand side of the `Project Settings` page.
3. Download the [Airbrake-Java JAR](https://github.com/airbrake/airbrake-java/blob/master/maven2/io/airbrake/airbrake-java/2.2.8/airbrake-java-2.2.8.jar?raw=true) file and add it to your `classpath`.
4. In your code, create a new `AirbrakeNotifier` and `AirbrakeNotice` instance using your `Project API Key`:

```java
import airbrake.*;

public class Main {
    public static void main(String[] args) {
        try {
            throw new Exception("Uh oh, an error!");
        }
        catch (Exception exception) {
            // Create new AirbrakeNotifier.
            AirbrakeNotifier notifier = new AirbrakeNotifier();
            // Create new AirbrakeNotice via Builder, passing PROJECT_API_KEY, exception, and optional environment string.
            AirbrakeNotice notice = new AirbrakeNoticeBuilder("966795e9ddb0543867ccf847df898318", exception, "development") {
                {
                    // Configure custom parameters.
                    addSessionKey("userId", 12345);
                }
            }.newNotice();
            // Pass generated notice to notifier, pushing exception to Airbrake.io.
            notifier.notify(notice);
        }
    }
}
```

5. Viola!  Your exceptions are now being pushed to `Airbrake.io`!