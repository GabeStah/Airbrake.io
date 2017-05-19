# .NET Exception Handling

The `Sharpbrake` library provides robust exception tracking capabilities for all of your C# and .NET applications.  `Sharpbrake` brings real-time error monitoring and automatic exception reporting across your entire project, so you and your team are immediately alerted at the smallest hiccup and can appropriately respond, before it turns into something worse.  With a robust API and tight integration with the powerful `Airbrake` web dashboard, `Sharpbrake` will revolutionize how your team manages exceptions.

Check out all the great features `Sharpbrake` brings to the table and see why so many of the world's top development teams use `Airbrake` to dramatically improve their exception handling practices!

## Features

`Sharpbrake` comes jam-packed with features purposefully built for modern .NET developers.  Whether you're working with the slightly-aged .NET 3.5 or the cutting-edge .NET 4.7 and above, `Sharpbrake` supports it, and even includes support for the latest .NET Core platforms.  `Sharpbrake` also includes built-in SSL encryption and proxy server support, so your exception data is always secure and sent to `Airbrake` exactly how you want it.  And when you need to tweak exception data before it gets pushed out, `Sharpbrake` has you covered with robust configuration options, custom exception filters, and adjustable environmental settings to precisely hone your exception reports before an alert comes your way.

Plus, you're not limited to only client-side applications.  `Sharpbrake` also includes the `ASP.NET HTTP` and `ASP.NET Core Middleware` modules, making it easy to quickly integrate `Sharpbrake` into all your web-based .NET applications as well.

`Sharpbrake` generates automatic, instantaneous exception reports the very moment an issue occurs, whether that's in a local development environment or even out in the wilds of live production with real users.  Best of all, exceptions are sent directly to you and your team, without the need for user-intervention and writing out cumbersome error reports.  With the multitude of features `Sharpbrake` provides, you will be continuously aware of any unexpected issues that may arise and able to immediately respond.

Take a peak at just a few of the great features below or head over to the full [documentation](https://github.com/airbrake/sharpbrake) and find out how `Sharpbrake` can help you and your team with your next .NET project!

### Asynchronous Exception Reporting

As expected, `Sharpbrake` allows you to create and send exception reports to `Airbrake` synchronously, in which your application waits for the exception report to be sent and a response to return before moving on to additional tasks.  However, `Sharpbrake` also comes with a powerful asynchronous notification system as well, allowing your application to queue up exception reports to be sent, then move onto other tasks while they are processed in the background.  This makes `Sharpbrake` the perfect exception handling tool for high-performance applications where multi-threading or other demanding tasks are a necessity.

Check out the [documentation](https://github.com/airbrake/sharpbrake#airbrakenotifier) for all the details on integrating synchronous versus asynchronous exception reporting with `Sharpbrake`.

### Limitless Custom Parameters

Exceptions sent to the `Airbrake` dashboard automatically include all the basic information you'd expect, such as the error message, backtrace, environment, remote origin, and more.  Yet `Sharpbrake` takes these capabilities much further by providing a simple API to add near limitless custom parameters and data onto your exception records before they are sent to `Airbrake` or to one of the many third-party integrations `Airbrake` offers.  This ensures that your exception reports always contain as much (or as little) relevant information as you need, so you and your team can quickly hone in on the root cause of the problem and get to work on a solution.

### Flexible Exception Filtering

Beyond adding additional parameter data to your exception reports, `Sharpbrake` also makes it easy to filter exceptions based on nearly any criteria you can think of.  For example, `Sharpbrake` makes it easy to ignore exceptions that originated from an `environment` you don't currently wish to monitor.  You can also `blacklist` or `whitelist` a limitless set of words or phrases, ensuring that exceptions which contain parameter, sessions, or environmental data with those matching keys are either always ignored always included.  Best of all, `Sharpbrake` allows you to create your own custom filter functions, making it easy to modify exception content before it is sent to `Airbrake`, or even perform your own custom filtering to ensure only a particular set of exceptions are reported in the first place.

Take a look at the full [documentation](https://github.com/airbrake/sharpbrake#ignoreenvironments) for more information on integrating the many robust filtering capabilities `Sharpbrake` has to offer.

### ASP.NET Inegration

In addition to client-side applications, `Sharpbrake` also includes modules that make it easy to integrate `Airbrake's` revolutionary exception handling and reporting services into ASP.NET web applications.  `Sharbrake` comes with the full-blown `ASP.NET HTTP` module that can be easily installed via `NuGet` and configured in just a few minutes.  Alternatively, if you need a middleware option, `Sharpbrake` also includes a powerful `ASP.NET Core Middleware` module that can be quickly integrated into the stack of other middleware components your application may be using.

No matter the type of .NET application you're developing or maintaining, `Sharpbrake` has you covered, so feel free to check out all the details in the [documentation](https://github.com/airbrake/sharpbrake#aspnet-integration).

## Quick Setup

### 