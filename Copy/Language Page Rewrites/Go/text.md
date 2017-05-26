# Go Exception Handling

`Gobrake` is a powerful, custom-built package that provides robust error tracking and automatic, instantaneous exception reporting capabilities to all your Go language applications.  `Gobrake` is both flexible and sturdy, ensuring that it is both easy to integrate into every project and also strong enough to handle heavy exception workloads for even the largest apps.  After a brief setup that takes less than 3 minutes, `Gobrake` will be tightly woven into your application code, guaranteeing that all errors are tracked and reported to you and your team, no matter when or where those errors occurred.  With tight integration into the `Airbrake` service, `Gobrake` can notify your team in a variety of ways including through email, the `Airbrake` web dashboard, and even via the many third-party service integrations `Airbrake` supports.  Best of all, since `Gobrake` reports errors instantly and automatically, there's never any need for inelegant error reports or awkward user-intervention.

Take a look at some of the great features `Gobrake` has to offer and see why `Airbrake` is a leader at providing better, faster, and stronger error handling tools for Go apps worldwide!

## Features

At its core, `Gobrake` aims to give you and your team access to automatic and instantaneous error reports across all your Go applications.  Every exception your application encounters is captured by `Gobrake` and is securely packaged up before being sent along to the `Airbrake` service.  Upon receiving an error report, `Airbrake` immediately notifies you and your team of the issue via email, the `Airbrake` dashboard, and (optionally) through any of the third-party service integrations you may have configured.  Plus, `Gobrake` provides a simple [integration library](https://github.com/airbrake/glog) for the popular `glog` logging package, ensuring that all logged errors are immediately reported to you via `Airbrake`.

Moreover, `Gobrake` error reports are packed with useful contextual data about each exception that occurs, yet with the simple `Gobrake` API you can go well beyond the defaults and add your own custom metadata to each record, _before_ it is sent to `Airbrake`.  Additionally, all error metadata is automatically displayed on the `Airbrake` dashboard, where it is easily searchable and filterable to ensure you find the exact exception you care about most at any given moment.  

Take a peek below at just a few of the amazing features `Gobrake` has to offer, or head over to the [documentation](https://github.com/airbrake/gobrake) for all the details and find out how you and your team can begin revolutionizing your Go exception handling practices today!

### Unlimited Custom Parameters

By default, `Gobrake` error reports include an abundance of useful metadata about each particular exception including the error message, backtrace, remote origin, severity, time stamps, and much more.  However, in many cases you and your team may have specific business logic that requires that additional, application-specific metadata be associated with each error record.  In such cases, `Gobrake` has your back with the ability to easily add extra contextual data to error record before they are packaged up and securely sent to the `Airbrake` dashboard for reporting.  In fact, since you're able to add extra context _directly within your Go code_, there's virtually no limit on the number of fields or types of data you can cram in there!

Check out the [documentation](https://github.com/airbrake/gobrake#setting-severity) for all the details on modifying contextual parameters inside your `Gobrake` error reports.

### Simple Glog Integration

Having direct control over when and where in your code you create error notifications with `Gobrake` is useful, but many Go applications also rely on a more robust logging package such as `glog`.  Thankfully, `Gobrake` makes it easy to quickly integrate with `glog's` logging functionality so that any errors that `glog` handles will also be instantly and automatically reported to `Airbrake` via the `Gobrake` library.

Take a look at the full `Airbrake-glog` [documentation](https://github.com/airbrake/glog) for more information on how you can easily add `Gobrake` to your standard `glog` logging functionality.

### Powerful Error Filtering

The `Gobrake` API provides the easy-to-use yet robust `AddFilter` method, which allows you to perform any kind of logical checks you can dream of against every error notification record your application produces -- _before_ it is sent along to `Airbrake` and you're alerted of the issue.  With just a few lines of code you can ensure that only the particular types of errors you care about most are actually reported to you via `Gobrake`.

Check out the [documentation](https://github.com/airbrake/gobrake#ignoring-notices) for all the details on ignoring error notices with `Gobrake`.

## Quick Setup

1. To begin using `Gobrake` start by [Creating an Airbrake account](https://airbrake.io/account/new), signing in, and making a new project.
2. Install the `Gobrake` package using the `go get` command from your terminal:

```bash
$ go get github.com/airbrake/gobrake
```

3. Add the `Gobrake` source path and the `errors` package to `import`:

```go
import (
    "errors"

    "github.com/airbrake/gobrake"
)
```

4. Add some variables to hold the appropriate `Project ID` and `Project API Key` values, both of which are found on the right-hand side of the `Project Settings` page of the `Airbrake` dashboard:

```go
var projectID int64 = 12345
var projectAPIKey = "PROJECT_API_KEY"
```

5. Create a `NewNotifier` instance from `Gobrake` and pass in the appropriate `Project ID` and `Project API Key` variables.

```go
// Create notifier instance with Project ID and Project API Keys
var notifier = gobrake.NewNotifier(projectID, projectAPIKey)
```

### Direct Error Reporting

To directly report errors using `Gobrake` simply call the `Notify` method of the notifier instance you created and pass in an error object argument: 

```go
func main() {
    // Always close notifier
    defer notifier.Close()
    // Always notify on panic
    defer notifier.NotifyOnPanic()

    // Create a new error and send via notifier
    notifier.Notify(errors.New("oh oh, something broke"), nil)
}
```

That's all there is to it!  `Gobrake` will now instantly and automatically report errors from your Go application to `Airbrake`.

### Glog Error Reporting

If you're using [`glog`](https://github.com/golang/glog) for your application logging `Gobrake` provides an integration package that makes it easy to work with `glog`.

1. Install the `Airbrake-glog` package using the `go get` command from your terminal:

```bash
$ go get github.com/airbrake/glog
```

2. Add `Airbrake-glog` to your `import`:

```go
import (
    "errors"

    "github.com/airbrake/glog"
    "github.com/airbrake/gobrake"
)
```

3. Create a new notifier with the `Project ID` and `Project API Key` as before and then assign that instance to `glog.Gobrake`:

```go
var projectID int64 = 12345
var projectAPIKey = "PROJECT_API_KEY"

// Create notifier instance with Project ID and Project API Keys
var notifier = gobrake.NewNotifier(projectID, projectAPIKey)

func main() {
    // Always close notifier
    defer notifier.Close()
    // Always notify on panic
    defer notifier.NotifyOnPanic()

    // Set glog instance
    glog.Gobrake = notifier

    // Create a new error and log it with glog
    glog.Errorf("Error logged: %s", errors.New("uh oh, something broke"))
}
```

4. You're all set!  By default this will send all `error` log entries to `Airbrake`, though you can easily change the `severity` threshold if needed (check out the full [documentation](https://github.com/airbrake/glog) for more details.)

### Adding Filters

`Gobrake` also makes it simple to add filters to your `Notifiers`.  In this example we're adding some contextual data to each notice:

```go
func init() {
    notifier := gobrake.NewNotifier(projectID, projectAPIKey)
    // Create a filter that executes over every error notice
    notifier.AddFilter(func(notice *gobrake.Notice) *gobrake.Notice {
        // Set the contextual 'environment' field to 'development'
        notice.Context["environment"] = "development"
        // When a notice is returned, it is reported to Airbrake
        return notice
    })
}
```

Here we're using a filter to _ignore_ errors where the `environment` value is equal to `development`:

```go
func init() {
    notifier := gobrake.NewNotifier(projectID, projectAPIKey)
    // Create a filter that executes over every error notice
    notifier.AddFilter(func(notice *gobrake.Notice) *gobrake.Notice {
        // Check if 'environment' field is set to 'development'
        if notice.Context["environment"] == "development" {
            // Ignore this notice
            return nil
        }
        // All other notices are reported as normal
        return notice
    })
}
```