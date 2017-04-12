# JavaScript Exception Handling

Airbrake's `Airbrake-JS` error tracking tool can quickly and easily integrate into any new or existing JavaScript application, providing you and your team with real-time error reporting and instant insight into exactly what went wrong.  `Airbrake-JS` works with all the latest JavaScript frameworks and supports full source map control, to ensure your error reports are as detailed as possible.  `Airbrake-JS` also allows you to easily customize error parameters and grants simple filtering tools so you only capture and report the errors you care about most.

## Features

`Airbrake-JS` includes a range of JavaScript-specific features designed to improve your workflow and ease error reporting throughout your project's development life cycle.  Have a glance at a handful of those features below, or take a look at the [official documentation](https://github.com/airbrake/airbrake-js) for more information.

### Robust Module and Framework Integration

`Airbrake-JS` easily integrates with all the most popular JavaScript frameworks and modules, including:

- [Angular](https://github.com/airbrake/airbrake-js/blob/master/examples/angular)
- [Angular 2](https://github.com/airbrake/airbrake-js/blob/master/examples/angular-2)
- [Bower](https://github.com/airbrake/airbrake-js/blob/master/examples/bower-wiredep)
- [Browserify](https://github.com/airbrake/airbrake-js/blob/master/examples/browserify)
- [Express.js](https://github.com/airbrake/airbrake-js/blob/master/examples/express)
- [hapi.js](https://github.com/airbrake/airbrake-js/blob/master/examples/hapi)
- [Legacy JavaScript](https://github.com/airbrake/airbrake-js/blob/master/examples/legacy)
- [Rails](https://github.com/airbrake/airbrake-js/blob/master/examples/rails)
- [React](https://github.com/airbrake/airbrake-js/blob/master/examples/react)
- [RequireJS](https://github.com/airbrake/airbrake-js/blob/master/examples/requirejs)

### Full Source Map Control

`Airbrake-JS` supports [direct control](https://github.com/airbrake/airbrake-js#source-map) of your `source maps`, allowing you to easily map between a transformed file and the original source file for all necessary scripts.  This ensures that your `Airbrake` error reports include well-defined, readable call stacks and full context -- just as it appears in the original source code -- rather than the jumbled mess that would normally be reported from a highly compact, minified version of the script. 

### Limitless Custom Parameters

You are not constrained to only the basic exception information that JavaScript provides.  With `Airbrake-JS`, you can easily pass unlimited custom parameters, as objects, to the `notify()` method of the `Airbrake-JS Client`:

```js
try {
  // ...
} catch (err) {
  // Without custom parameters.
  airbrake.notify(err);
  // With custom parameters.
  airbrake.notify({
    error:       err,
    context:     { component: 'library' },
    environment: { server: 'production' },
    params:      { name: 'Bob Smith' },
    session:     { user: 'bobsmith@example.com' },
  });
  throw err;
}
```

### Precise Error Filtering

As if often the case, there may be certain types of errors you don't wish to monitor via `Airbrake-JS`, such as those from third-party libraries or browser extensions.  To filter certain errors with `Airbrake-JS`, simply use the `addFilter()` method, which can be passed a function argument that processes and returns the provided `notice` object to be sent to `Airbrake`.  If the function returns `null`, the `notice` is ignored, otherwise the returned `notice` is submitted:

```js
airbrake.addFilter(function(notice) {
  if (notice.environment.server == 'development') {
    // Ignore errors from the development server.
    return null;
  }
  return notice;
});

try {
  // ...
} catch (err) {
  // This error would not be sent to Airbrake, 
  // since it matches the filter criteria above.
  airbrake.notify({
    error:       err,
    environment: { server: 'development' },
  });
}
```

## Installation

#### Install via `NPM`

```
npm install airbrake-js
```

#### Install via `Bower`

```
bower install airbrake-js-client
```

#### Include via `CDN`

1. Visit [`airbrake-js`](https://cdnjs.com/libraries/airbrake-js) on [`cdnjs`](https://cdnjs.com/libraries/airbrake-js).
2. Select the appropriate release type (`full`, `minified`, etc).
3. Copy and paste the `URI` into a `<script>` tag within your `<head>` HTML tag: `<script src="https://cdnjs.cloudflare.com/ajax/libs/airbrake-js/0.8.4/client.min.js"></script>`.

## Usage

Once installed, using `Airbrake-JS` is quick and easy.

#### Initializing the Client

Initialize the notifier by creating a new client object using the appropriate `Project ID` and `Project API Key`, which can be found on the right-hand side of the `Project Settings` page.

To initialization when directly including `Airbrake-JS` (i.e. `client.js` or `client.min.js`):

```js
var airbrake = new airbrakeJs.client({
  projectId: YOUR_PROJECT_ID,
  projectKey: 'YOUR_PROJECT_API_KEY'
});
```

To initialization when using `Node.js` (or modules therein) and its `require()` function:

```js
var airbrakeJs = require('airbrake-js');
var airbrake = new airbrakeJs({
  projectId: YOUR_PROJECT_ID,
  projectKey: 'YOUR_PROJECT_API_KEY'
});
```

#### Direct Error Notification

To report an error to `Airbrake` directly, call the `airbrake.notify()` method:

```js
var airbrake = new airbrakeJs.Client({
  projectId: YOUR_PROJECT_ID, 
  projectKey: 'YOUR_PROJECT_API_KEY'
})

try {
  throw new Error('Direct example error.');
} catch (err) {
  // Notify Airbrake of error.
  promise = airbrake.notify(err);
  promise.then(function(notice) {
    console.log('Id:', notice.id);
  }, function(err) {
    console.log('Airbrake failed:', err);
  });
}
```

#### Indirect Error Notification

In cases where you don't wish to directly call the `notify()` method each time an error occurs, you can wrap potentially problematic code in the `wrap()` method:

```js
var myFunction = function() {
  throw new Error('Wrap example error.');
}

// Wrap myFunction, to monitor all errors occurring within that function scope.
startApp = airbrake.wrap(myFunction);

// Execute the newly defined wrap function.
startApp();
```

If you don't need to postpone execution of the `wrap()` call from when inner function execution occurs, you can use the `call()` method shortcut instead:

```js
var myFunction = function() {
  throw new Error('Call example error.');
}

// Call myFunction, which will execute the underlying function immediately
// and notify Airbrake of all errors occurring within that function scope.
airbrake.call(myFunction);
```

### Common Integrations

Integrating `Airbrake-JS` with any of the common JavaScript modules is easy.  For example, here we're integrating `Airbrake-JS` with [`Express`](https://expressjs.com/):

```js
var express = require('express');
var app = express();
var AirbrakeClient = require('airbrake-js');
var makeErrorHandler = require('airbrake-js/dist/instrumentation/express');

var airbrake = new AirbrakeClient({
  projectId: 141379, 
  projectKey: '52e0659442daa64cd52d8fd9110f66c1'
});

app.get('/', function hello (req, res) {
  throw new Error('Express example error.');
  res.send('Hello World!');
})

// Use the express instrumentation to catch and notify errors.
app.use(makeErrorHandler(airbrake));

app.listen(3000, function () {
  console.log('Example app listening on port 3000!');
})
```

All other example integrations can be [found here](https://github.com/airbrake/airbrake-js/tree/master/examples).