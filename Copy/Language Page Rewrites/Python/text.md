# Python Exception Handling

`Airbrake-Python` is the perfect tool for providing robust exception tracking and instantaneous, automatic error reporting throughout all your Python applications.  `Airbrake-Python` is easy to install, features an extensive API, and integrates with the powerful `Airbrake` web dashboard, allowing you and your team to dramatically revolutionize how you manage exceptions.

Browse through some of the great features `Airbrake-Python` has to offer and see why high-performance Python projects all around the globe rely on `Airbrake` to simplify and enhance their exception handling practices!

## Features

`Airbrake-Python` is loaded with features specifically built by and designed for Python developers.  At its core, `Airbrake-Python` ensures that every single exception that crops up in your application is instantly and automatically sent to `Airbrake`, alerting you and your team via email, on the `Airbrake` dashboard, and through the many optional third-party service integrations `Airbrake` supports.  This guarantees that you remain acutely aware of the health of your application and are able to immediately respond to any issues which may arise.  Plus, your application data is always secure, since all communication between `Airbrake-Python` and the `Airbrake` servers is SSL encrypted.

When you need some additional error information, or to customize the behavior of your application before exceptions are sent, we have you covered.  `Airbrake-Python` features a robust API which allows you to add additional custom parameters to your exception records.  You can even filter out exception data using your own custom criteria, so there's never any concern of transferring private data.  `Airbrake-Python` also makes it easy to automatically handle exceptions by tightly integrating with Python's built-in [`logging`](https://docs.python.org/3/library/logging.html) module, allowing `Airbrake-Python` to easily integrate into whatever existing logging functionality you are already using.

Take a look below at just a handful of the amazing features `Airbrake-Python` has to offer, or check out the full [documentation](https://github.com/airbrake/airbrake-python) for all the details and find out how `Airbrake-Python` can transform the way your team handles exceptions!

### Adjustable Logging Integration

Most Python developers will be familiar with the built-in [`logging`](https://docs.python.org/3/library/logging.html) module, which provides a flexible logging API that is used across an extensive number of libraries and applications.  Logging can be used for all manner of outputs, which is why `Airbrake-Python` takes advantage of this capability and makes it easy for you to quickly integrate exception handling into your application through its own logger.  This makes it a breeze to setup a global exception handler across your entire application, integrated with all other logging functionality you have, so errors are immediately reported to `Airbrake` and pushed out to you and your team right away.

On the other hand, if you don't want to rely on logger integration to handle exception reporting `Airbrake-Python` has you covered there too.  `Airbrake-Python` allows you to directly create your own notifiers, so you can report exceptions anywhere in your code base, which gives you maximum control over how exceptions are handled.

Get the full details and find out more about how to use `Airbrake-Python` with and without a logger in our [Quick Setup](#quick-setup) instructions below, or check out our extensive [documentation](https://github.com/airbrake/airbrake-python#using-this-library-without-a-logger).

### Unlimited Custom Parameters

By default, the `Airbrake-Python` library includes an abundance of information attached to each exception report it creates, including a timestamp, backtrace, message, error level, process and thread details, environment, remote origin, OS, and much more!  Yet, there may be situations where you need additional, application-specific details that are unique to your application.  `Airbrake-Python` supports such scenarios with its easy-to-use API, allowing you to completely configure and customize the metadata associated with exception reports, _before_ they're sent to `Airbrake`.  With just a few lines of code you can completely control what extra contextual info is attached to an exception, and since it's all setup using simple `dictionary` objects in Python, the only limit is your imagination and your particular business requirements.

Find our more about adding custom parameters in the [documentation](https://github.com/airbrake/airbrake-python#giving-your-exceptions-more-context).

### Flexible Exception Filtering

Adjust the metadata associated with exceptions before they're sent to `Airbrake` with the easy `filtering` API provided by `Airbrake-Python`.  Quickly create a `whitelist` or a `blacklist`, specifying certain keywords within the exception metadata that should always be included (or excluded) depending on your needs.  You can even go beyond keywords and filter out entire exception records based on their underlying data, which is great for excluding errors that originate from certain environments such as `development` or `test`.

## Quick Setup

To begin using `Airbrake-Python`, start by [Creating an Airbrake account](https://airbrake.io/account/new), signing in, and making a new project.

### Install with Logger Integration

1. Install `Airbrake-Python` via [`pip`](https://pip.pypa.io/en/stable/):

```bash
$ pip install -U airbrake
```

2. Configure `Airbrake-Python` to use your newly created project by exporting a few environment variables.  Just copy and paste the appropriate `Project ID` and `Project API Key` values, which can be found on the right-hand side of the `Project Settings` page:

```bash
export AIRBRAKE_PROJECT_ID=[PROJECT_ID]
export AIRBRAKE_API_KEY=[PROJECT_API_KEY]
```

3. In your code `import airbrake`, create a new instance of the `Airbrake-Python` `logger`, then call the `exception()` method wherever you want exception handling to occur:

```python
# Import the Airbrake-Python module.
import airbrake

# Create a new logger instance.
logger = airbrake.getLogger()

try:
    # Raising an exception.
    1 / 0
except Exception as e:
    # Sends a 'division by zero' exception to Airbrake.
    logger.exception(e)
```

4. If you don't want to configure `Airbrake-Python` through environment variables, you can also pass `Project ID` and `Project API Key` as arguments to the new logger instance:

```python
# Create a new logger instance, with project settings:
logger = airbrake.getLogger(api_key="PROJECT_API_KEY", project_id=PROJECT_ID)
```

5. You're all set!  `Airbrake-Python` will send all exceptions that pass through your `logger.exception()` calls to `Airbrake`!

### Install without Logger Integration

1. Install `Airbrake-Python` via [`pip`](https://pip.pypa.io/en/stable/):

```bash
$ pip install -U airbrake
```

2. Configure `Airbrake-Python` to use your newly created project by exporting a few environment variables.  Just copy and paste the appropriate `Project ID` and `Project API Key` values, which can be found on the right-hand side of the `Project Settings` page:

```bash
export AIRBRAKE_PROJECT_ID=[PROJECT_ID]
export AIRBRAKE_API_KEY=[PROJECT_API_KEY]
```

3. In your code `import Airbrake`, create a new instance of the `Airbrake`, then call the `notify()` method to send a specific exception to `Airbrake`.  Alternatively, you can also call the `capture()` method to send all unspecified exceptions:

```python
# Import the Airbrake class.
from airbrake.notifier import Airbrake

# Create a new Airbrake instance.
manager = Airbrake()

try:
    # Raising an exception.
    1 / 0
except ZeroDivisionError as e:
    # Sends a 'division by zero' exception to Airbrake.
    manager.notify(e)
except:
    # Sends all other exceptions to Airbrake.
    manager.capture()
```

4. If you don't want to configure `Airbrake-Python` through environment variables, you can also pass `Project ID` and `Project API Key` as arguments to the new `Airbrake` instance:

```python
# Create a new Airbrake instance, with project settings:
manager = Airbrake(api_key="PROJECT_API_KEY", project_id=PROJECT_ID)
```

5. All done!  You now have full control over every single exception that is sent to `Airbrake`!