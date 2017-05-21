# Django Exception Handling

`Airbrake-Django` is the idea companion tool for streamlining your exception handling methods in all your Django-based projects.  `Airbrake-Django` provides state-of-the-art exception tracking and instantaneous, automatic error reporting, no matter the environment your application is running on or who may be using it.  `Airbrake-Django` can be installed and configured in just a few minutes and features a truly plug-and-play experience -- once installed, it just works and handles all exceptions for you.  Best of all, `Airbrake-Django` integrates directly with the powerful `Airbrake` web dashboard, allowing you and your team to dramatically revolutionize how you manage exceptions.

Have a glance at the features `Airbrake-Django` has to offer and find out why `Airbrake` is used every day by stellar development shops and world-class businesses to help streamline and enhance their exception handling practices!

## Features

`Airbrake-Django` is designed to integrate quickly and easily into new or existing Django projects of any size.  At the basic level, `Airbrake-Django` ensures that every exception that crops up in your application is instantly and automatically sent to `Airbrake`, which triggers alerts for you and your team via email, displays exceptions on the `Airbrake` dashboard, and pushes errors out to the many optional third-party service integrations `Airbrake` supports.  With `Airbrake-Django`, there's no more concern about the health of your application and no more need to worry that something could be going wrong that you just aren't aware of.  `Airbrake-Django` integrates directly with your Django application, so all exceptions are instantly and automatically reported to you through `Airbrake`, allowing you and your team to investigate the root problem and immediately begin working on a solution.  Plus, all communication between `Airbrake-Django` and the `Airbrake` servers is SSL encrypted, ensuring your data is always secure.

Have a look below at a few of the great features `Airbrake-Django` provides, or check out the full [documentation](https://github.com/airbrake/Airbrake-django) for all the details to see how `Airbrake-Django` can dramatically improve how your team does Django exception handling!

### Rapid Installation and Django Integration

Even for less technically-minded individuals, installing and configuring `Airbrake-Django` is a breeze, making it the perfect exception handling solution for all types of Django applications.  In less than **3 minutes** you can have `Airbrake-Django` setup, tracking, and reporting every exception your Django application encounters, throughout its entire life cycle.  Best of all, `Airbrake-Django` integrates seamlessly with all other Django middleware modules, so there's no need to alter your application structure or code in anyway before you can begin using the power of `Airbrake-Django`.

Check out our [Quick Setup](#quick-setup) instructions below to find out just how rapidly you can be up and running with `Airbrake-Django`!

### Automatic Exception Reporting

`Airbrake-Django` dramatically streamlines exception handling practices by completely automating the error reporting process.  Once installed, `Airbrake-Django` takes over and will autonomously recognize when a Python exception has been thrown, immediately packaging it up and pushing it out to you via email alerts, third-party integrations, and the `Airbrake` dashboard.  Plus, `Airbrake-Django` includes all sorts of relevant exception metadata including: error message, url, file and backtrace, user session data, Django action/component, environment, remote origin, and much more!

With so much relevant data associated with each exception report sent to `Airbrake`, you and your team are never in the dark.  With the powerful search and filter capabilities of the `Airbrake` web dashboard, you and your team can explicitly hone in on exactly the set of errors you care about at that moment, making it easy to discover the underlying cause.

### Manual Exception Reporting & Custom Parameters

While `Airbrake-Django` allows for all application exceptions to be automatically detected and reported, `Airbrake-Django` also allows for direct, manual control over exception handling as well.  With just a few extra lines of code you can easily create a client to connect to `Airbrake`, then send only the specific exceptions you want.  Moreover, with direct control over the code that reports exceptions, you will also have access to the associated information attached to the exception report, such as the `session` data.  This makes it easy to modify and add near-limitless custom fields and values to the exception record before it is packaged up and reported to you via `Airbrake`.

## Quick Setup

1. To begin using `Airbrake-Django`, start by [Creating an Airbrake account](https://airbrake.io/account/new), signing in, and making a new project.
2. Install `Airbrake-Django` via [`pip`](https://pip.pypa.io/en/stable/):

```bash
$ pip install git+https://github.com/airbrake/airbrake-django.git
```

3. Add `airbrake` to the `INSTALLED_APPS` tuple in the `settings.py` file of your Django project:

```python
INSTALLED_APPS = (
    'django.contrib.admin',
    # ...
    'airbrake'
)
``` 

4. Configure `Airbrake-Django` by creating an `AIRBRAKE` dictionary in your `settings.py` file.  Be sure to paste the appropriate `Project API Key` value, which can be found on the right-hand side of the `Project Settings` page:

```python
# Airbrake settings
AIRBRAKE = {
    'API_KEY': 'PROJECT_API_KEY',
    'TIMEOUT': 5,
    'ENVIRONMENT': 'production',
}
```

5. Almost done!  Now decide whether you want to use [automatic](#setup-automatic-exception-reporting) or [manual](#setup-manual-exception-reporting) exception reporting and follow the final step below.

### Setup Automatic Exception Reporting

To have `Airbrake-Django` automatically report exceptions to `Airbrake` simply add the `AirbrakeNotifierMiddleware` to the `MIDDLEWARE` tuple in your `settings.py` file:

```python
MIDDLEWARE = (
    # ...,
    'airbrake.middleware.AirbrakeNotifierMiddleware',
)
```

### Setup Manual Exception Reporting

To setup `Airbrake-Django` for manual exception reporting just create a new `Airbrake` `Client` instance and call the `notify()` method, passing the caught exception and request object to be reported:

```python
from airbrake.utils.client import Client
from django.http import HttpResponse

def do_something(request):
    try:
        # Throw a ZeroDivisionError.
        1/0
    except Exception as exception:
        # Create a new Airbrake Client instance.
        airbrake = Client()
        # Report the caught exception to Airbrake.
        airbrake.notify(exception, request)

    return HttpResponse("Request was a success.")
```