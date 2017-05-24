# Node.js Exception Handling

`Airbrake-Node` is the powerful, professional tool designed to dramatically improve how you and your team handle exceptions throughout all your Node applications.  `Airbrake-Node` gives you robust error tracking and automatic, instantaneous error reporting, regardless of the severity of the issue, the size of your install base, or who might be using the application.  `Airbrake-Node` can be installed and working in just a few minutes, autonomously handling all exceptions and reporting them to you via email, the `Airbrake` web dashboard, and also through any third-party service integrations you may have configured.  Best of all, `Airbrake-Node` is constantly working behind-the-scenes, reacting immediately when something unexpected goes wrong and providing you with a constant pulse on your application's health, without the need for awkward and time-consuming user-generated error reports.

Take a quick look below at some of the features `Airbrake-Node` has to offer and find out why `Airbrake` is the go-to exception handling service for a multitude of professionals and applications all around the world!

## Features

`Airbrake-Node` brings the power of automatic, tightly-integrated exception handling to all your new or existing Node projects.  `Airbrake-Node` ensures that every error your application encounters is instantly and automatically sent to `Airbrake`, which triggers alerts for you and your team through email, records the exception in the state-of-the-art `Airbrake` dashboard, and also pushes error reports out to the many (optional) third-party service integrations `Airbrake` supports.  Moreover, `Airbrake-Node` is stable and production ready --  its been in active development for 6 years with dozens of releases and a constant stream of updates designed to ensure your application is never incompatible nor stops reporting errors during a critical period.

Not only does `Airbrake-Node` make it easy to automatically report all exceptions via `Airbrake`, it also comes with built-in integration support for popular frameworks like `Express` and `hapi`, so you'll have robust exception handling setup in no time, no matter what type of application you're creating.  Furthermore, when reporting all exceptions is more than you need, `Airbrake-Node` has you covered with `filters`, which give you the ability to ignore certain errors based on virtually any criteria you can dream up.  Plus, while `Airbrake-Node` includes an abundance of useful default information in each exception report, you're never limited to these default values.  `Airbrake-Node` makes it a breeze to attach your own custom data to exception reports _before_ they're sent to `Airbrake`.  This advanced data automatically appears in reports on the `Airbrake` dashboard, making it easy for you and your team to search through and filter errors, so you can find exactly what you're looking for and get working on a solution right away.

Check out just a handful of the amazing features `Airbrake-Node` provides, or click over to the full [documentation](https://github.com/airbrake/node-airbrake) for all the details and begin revolutionizing your exception handling practices today!

### Unlimited Custom Parameters

By default, all exceptions reported by `Airbrake-Node` include an abundance of useful information such as the error message, backtrace, process info, request origin, and much more.  All these fields will appears on the `Airbrake` web dashboard and can be filtered and searched using any custom parameters you desire.  However, in many situations you may also want to add _additional_ data to your exception records before they're sent to `Airbrake`.  `Airbrake-Node` makes this is a piece of cake so you can modify your exception reports to include every piece of relevant information you care about (or even exclude data you may want to ignore).

Check out the [documentation](https://github.com/airbrake/node-airbrake#adding-context-to-errors) for all the details on how to add custom context data to your `Airbrake-Node` error reports.

### Robust Error Filtering

While it may be considered prudent to catch and handle all exceptions that are thrown by your Node application, there are often situations where certain errors just don't matter; whether these are third-party errors or otherwise, your team may wish to ignore them entirely.  In these situations `Airbrake-Node` has you covered with its robust error filtering capabilities.  Just as `Airbrake-Node` allows you to add and modify custom contextual data before the error is sent to `Airbrake`, the `addFilter()` API method allows you to ignore (or include) exceptions based on said contextual data.  Since filter functions are executed for every error that passes through the `Airbrake-Node` library, you're only limited by your imagination and requirements of your business logic.

Have a look at the [documentation](https://github.com/airbrake/node-airbrake#filtering-errors) for more information on how `Airbrake-Node` makes it easy to filter whatever you wish.

### Easy Framework Integration

`Airbrake-Node` is robust enough to be compatible with every type of Node application right out of the box.  Yet, for developers working with some of the most popular Node frameworks, `Airbrake-Node` comes with a built-in exception handler that easily integrates with these frameworks and continues to automatically report all errors.  `Airbrake-Node` currently includes a powerful exception handler for both the [`Express`](https://expressjs.com/) and [`hapi`](https://hapijs.com/) frameworks, making integration with either a breezy endeavor.

You can find out more details on integrating `Airbrake-Node` with web frameworks in the [documentation](https://github.com/airbrake/node-airbrake#express-integration).

### Powerful Deployment Tracking

`Airbrake-Node` makes it simple to automatically track the deployments of all your Node-based projects.  Informing `Airbrake` of your deployments provides a number of benefits like automatically marking all existing errors in the specified environment as `Resolved`.  This gives you and your team a clean slate, making it easy to determine which errors have been fixed by this new release, or which regressions might have recently popped up as a result.  Plus, just as you can filter exceptions on the `Airbrake` dashboard using contextual error data, you can also filter errors by their `deployment` identifier.  This also allows you to follow links in your exception backtrace to the relevant `revision > file > line` on code repository integrations like `GitHub` and `GitLab`.

Feel free to check out the [documentation](https://github.com/airbrake/node-airbrake#tracking-deployments) to see just how `Airbrake-Node` can be easily integrated into your  project deployments.

## Quick Setup

1. To begin using `Airbrake-Node` start by [Creating an Airbrake account](https://airbrake.io/account/new), signing in, and making a new project.
2. Install `Airbrake-Node` by adding the dependency to your `package.json` file:

```json
{
  "dependencies": {
    "airbrake": "^2.0.1"
  }
}
```

3. Alternatively, `Airbrake-Node` can be manually installed via [`npm`](https://www.npmjs.com/):

```bash
$ npm install airbrake
```

4. `require` the `Airbrake-Node` package, then create a new client instance with the `createClient()` method.  Be sure to copy and paste the `Project API Key` and `Project ID`, which can be found on the right-hand side of the `Project Settings` page, as arguments of the `createClient()` call:

```js
var airbrake = require('airbrake').createClient(
  'PROJECT_ID',
  'PROJECT_API_KEY'
);
```

5. Now call the `handleExceptions()` method of the `Airbrake-Node` client instance:

```js
airbrake.handleExceptions();
```

6. That's it!  Now when an exception is thrown `Airbrake-Node` will package it up and automatically report it to you via the `Airbrake` dashboard!