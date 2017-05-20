# .NET Exception Handling

The `Sharpbrake` library provides robust exception tracking capabilities for all of your C# and .NET applications.  `Sharpbrake` provides real-time error monitoring and automatic exception reporting across your entire project, so you and your team are immediately alerted to even the smallest hiccup, and can appropriately respond before it turns into something major.  With a robust API and tight integration with the powerful `Airbrake` web dashboard, `Sharpbrake` will revolutionize how your team manages exceptions.

Have a look around at all the great features `Sharpbrake` brings to the table and see why so many of the world's top development teams use `Airbrake` to dramatically improve their exception handling practices!

## Features

`Sharpbrake` comes jam-packed with features purposefully built for modern .NET developers.  Whether you're working with the slightly-aged .NET 3.5 or the latest cutting-edge .NET version, `Sharpbrake` supports it, including all the latest .NET Core platforms.  `Sharpbrake` also includes built-in SSL encryption and proxy server support, so your exception data is always secure and sent to `Airbrake` exactly how you want it.  And when you need to tweak exception data before it gets pushed out, `Sharpbrake` has you covered with robust configuration options, custom exception filters, and adjustable environmental settings to precisely hone your exception reports before an alert comes your way.

Plus, you're not limited to only client-side applications.  `Sharpbrake` also includes the `ASP.NET HTTP Module` and `ASP.NET Core Middleware`, making it easy to quickly integrate `Sharpbrake` into all your web-based .NET applications as well.

`Sharpbrake` generates automatic, instantaneous exception reports the very moment an issue occurs, whether that's within a local development environment or even out in the wilds of live production.  Best of all, exceptions are sent directly to you and your team, without the need for user intervention or writing out painstaking error reports.  With the multitude of features `Sharpbrake` provides, you will be continuously aware of any unexpected issues that may arise, and will be able to immediately respond.

Take a peak at just a few of the great features below or head over to the full [documentation](https://github.com/airbrake/sharpbrake) and find out how `Sharpbrake` can help you and your team with your next .NET project!

### Asynchronous Exception Reporting

As expected, `Sharpbrake` allows you to create and send exception reports to `Airbrake` synchronously, in which your application waits for the exception report to be sent and a response to return before moving on to additional tasks.  However, `Sharpbrake` also comes with a powerful asynchronous notification system as well, allowing your application to queue up exception reports that are waiting to be sent, while your app moves on to other tasks while the reports are processed in the background.  This makes `Sharpbrake` the perfect exception handling tool for high-performance applications where multi-threading or other demanding tasks are a requirement.

Check out the [documentation](https://github.com/airbrake/sharpbrake#airbrakenotifier) for all the details on integrating synchronous versus asynchronous exception reporting with `Sharpbrake`.

### Limitless Custom Parameters

Exceptions sent to the `Airbrake` dashboard automatically include all the basic information you'd expect, such as the error message, backtrace, environment, remote origin, and more.  Yet `Sharpbrake` takes these capabilities much further by providing a simple API for adding near limitless custom parameters and data onto your exception records, before they are sent to `Airbrake` or to one of the many third-party integrations `Airbrake` offers.  This ensures that your exception reports always contain as much (or as little) relevant information as you need, so you and your team can quickly hone in on the root cause of the problem and get to work on a solution.

### Flexible Exception Filtering

Beyond adding additional parameter data to your exception reports, `Sharpbrake` also makes it easy to filter exceptions based on nearly any criteria you can think of.  For example, `Sharpbrake` makes it simple to ignore exceptions that originated from an `environment` you don't currently wish to monitor.  You can also `blacklist` or `whitelist` a limitless set of words or phrases, ensuring that exceptions which contain parameter, sessions, or environmental data with those matching keys are either always ignored or always included.  Best of all, `Sharpbrake` allows you to create your own custom filter functions, making it a cinch to modify exception content before it is sent to `Airbrake`, or even to perform your own custom filtering to ensure only a particular set of exceptions are reported in the first place.

Take a look at the full [documentation](https://github.com/airbrake/sharpbrake#ignoreenvironments) for more information on integrating the many robust filtering capabilities `Sharpbrake` has to offer.

### ASP.NET Integration

In addition to client-side applications, `Sharpbrake` also includes modules that make it a breeze to integrate `Airbrake's` revolutionary exception handling and reporting services into ASP.NET web applications.  `Sharpbrake` comes with the full-blown `ASP.NET HTTP Module` that can be easily installed via `NuGet` and configured in just a few minutes.  Alternatively, if you need a middleware option, `Sharpbrake` also includes a powerful `ASP.NET Core Middleware` that can be quickly integrated into the stack of other middleware components your application may be using.

No matter the type of .NET application you're developing or maintaining, `Sharpbrake` has you covered, so feel free to check out all the details in our full [documentation](https://github.com/airbrake/sharpbrake#aspnet-integration).

## Quick Setup

If you haven't done so already, all installations start by [Creating an Airbrake account](https://airbrake.io/account/new), signing in, and creating a new project to configure.

### Install the Sharpbrake Client

1. Open a [Package Manager Console](https://docs.microsoft.com/en-us/nuget/tools/package-manager-console) and install the [`Sharpbrake.Client`](https://www.nuget.org/packages/Sharpbrake.Client) package:

```bash
PM> Install-Package Sharpbrake.Client
```

2. In your code, include the `Sharpbrake.Client` library and create new `AirbrakeConfig` and `AirbrakeNotifier` instances:

```cs
using System;
using Sharpbrake.Client;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var airbrake = new AirbrakeNotifier(new AirbrakeConfig
            {
                ProjectId = "PROJECT_ID",
                ProjectKey = "PROJECT_API_KEY"
            });

            try
            {
                throw new Exception("Oops!"));
            }
            catch (Exception ex)
            {
                var response = airbrake.NotifyAsync(ex).Result;
                Console.WriteLine("Status: {0}, Id: {1}, Url: {2}", response.Status, response.Id, response.Url);
            }
        }
    }
}
```

3. On the `Airbrake` project dashboard, locate your `Project API Key` and `Project ID`, which can be found on the right-hand side of the `Project Settings` page, and copy/paste those values into the `ProjectKey` and `ProjectId` fields of the `AirbrakeConfig` instance.
4. That's it!  You can now use the `AirbrakeNotifier` instance to send synchronous or asynchronous exception reports to the `Airbrake` dashboard!

### Install the ASP.NET HTTP Module

1. Open a [Package Manager Console](https://docs.microsoft.com/en-us/nuget/tools/package-manager-console) and install the [`Sharpbrake.Http.Module`](https://www.nuget.org/packages/Sharpbrake.Http.Module) package:

```bash
PM> Install-Package Sharpbrake.Http.Module
```

2. Configure the `appSettings` element in `Web.config` by adding the `Airbrake.ProjectId` and `Airbrake.ProjectKey` fields and pasting in the appropriate `Project ID` and `Project API Key` values, which can be found on the right-hand side of the `Project Settings` page:

```xml
<appSettings>
    <add key="Airbrake.ProjectId" value="PROJECT_ID"/>
    <add key="Airbrake.ProjectKey" value="PROJECT_API_KEY"/>
</appSettings>
```

3. Add the module to the `system.webServer > modules` element in `Web.config`:

```xml
<system.webServer>
    <modules>
        <add name="Airbrake" type="Sharpbrake.Http.Module.AirbrakeHttpModule, Sharpbrake.Http.Module"/>
    </modules>
</system.webServer>
```

4. You're all set and all exceptions will now be instantly and automatically reported to `Airbrake`!

### Install the ASP.NET Core Middleware

1. Open a [Package Manager Console](https://docs.microsoft.com/en-us/nuget/tools/package-manager-console) and install the [`Sharpbrake.Http.Middleware`](https://www.nuget.org/packages/Sharpbrake.Http.Middleware) package:

```bash
PM> Install-Package Sharpbrake.Http.Middleware
```

2. Add the `Airbrake` element to the `appsettings.json` file by pasting the appropriate `Project ID` and `Project API Key` values, which can be found on the right-hand side of the `Project Settings` page:

```xml
"Airbrake": {
  "ProjectId": "PROJECT_ID",
  "ProjectKey": "PROJECT_API_KEY",
}
```

3. Include the `Sharpbrake.Http.Middleware` in your `Startup.cs` file:

```cs
using Sharpbrake.Http.Middleware;
```

4. Lastly, make sure to add the middleware to your pipeline by updating the `Configure` method:

```cs
app.UseAirbrake(Configuration.GetSection("Airbrake"));
```

5. That's all there is to it!  All exceptions will now be pushed to `Airbrake's` dashboard!