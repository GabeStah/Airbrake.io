---
categories: [NodeJS Error Handling]
date: 2018-01-15
published: true
title: Node.js Error Handling - ERR_ASYNC_CALLBACK
---

Next up in our deep [**Node.js Error Handling**](https://airbrake.io/blog/nodejs-error-handling/nodejs-error-class-hierarchy) series we'll be tackling the **ERR_ASYNC_CALLBACK** error type, which falls into the broader `System Errors` category of Node.  Node throws a `System Error` when an exception occurs within the program's runtime environment, and such errors are typically an indication that there was an operational problem within the application.  An `ERR_ASYNC_CALLBACK` error indicates that an attempt was made to register a non-function data type as an [`async_hooks`](https://nodejs.org/api/async_hooks.html) constructor callback.

Throughout this article we'll explore the `ERR_ASYNC_CALLBACK` system error by looking at where it sits in the greater [Node.js Error Class Hierarchy](https://airbrake.io/blog/nodejs-error-handling/nodejs-error-class-hierarchy).  We'll also examine the basics of the [`async_hooks`](https://nodejs.org/api/async_hooks.html) module, and how improper use could result in `ERR_ASYNC_CALLBACKs`, so let's dig in!

## The Technical Rundown

Most Node.js errors inherit from the [`Error`](https://nodejs.org/api/errors.html#errors_class_error) base class, or extend from an inherited class therein.  The full error hierarchy of this error is:

- [`Error`](https://nodejs.org/api/errors.html#errors_class_error)
    - [`SystemError`](https://nodejs.org/dist/latest-v8.x/docs/api/errors.html#errors_system_errors)
      - `ERR_ASYNC_CALLBACK`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```js
/**
 * createAsyncHook.js
 */

const async_hooks = require('async_hooks');
const logging = require('logging');

function executeTests () {
  let callbacks = {
    init: init,
    before: before,
    after: after,
    destroy: destroy,
    promiseResolve: promiseResolve
  };
  logging.lineSeparator('async_hooks.createHook(...)', 60);
  getAsyncHook(callbacks);
}

/**
 * Gets an AsyncHook instance.
 *
 * @param callbacks
 * @returns {*}
 */
function getAsyncHook (callbacks) {
  try {
    // Get instance from constructor, log to console, and return object.
    let asyncHook = async_hooks.createHook(callbacks).enable();

    // Create server and listen on port 8080.
    require('net').createServer(() => {}).listen(8080, () => {
      // Output server ready message after 1 second.
      setTimeout(() => {
        logging.lineSeparator('SERVER ACCEPTING CONNECTIONS', 60);
      }, 1000);
    });

    return asyncHook;
  } catch (e) {
    // Catch TypeError with code property of ERR_ASYNC_CALLBACK.
    if (e instanceof TypeError && e.code === 'ERR_ASYNC_CALLBACK') {
      // Output expected ERR_ASYNC_CALLBACK TypeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

/**
 * Called during object construction.
 *
 * @param asyncId
 * @param type
 * @param triggerAsyncId
 * @param resource
 */
function init(asyncId, type, triggerAsyncId, resource) {
  const executionId = async_hooks.executionAsyncId();
  logging.logSync(`(INIT) Id: ${asyncId}, TriggerId: ${triggerAsyncId}, ExecutionId: ${executionId}, Type: ${type}`);
}

/**
 * Called before resource's callback.
 *
 * @param asyncId
 */
function before(asyncId) {
  logging.logSync(`(BEFORE) Id: ${asyncId}`);
}

/**
 * Called after resource's callback.
 *
 * @param asyncId
 */
function after(asyncId) {
  logging.logSync(`(AFTER) Id: ${asyncId}`);
}

/**
 * Called when AsyncWrap instance is destroyed.
 *
 * @param asyncId
 */
function destroy(asyncId) {
  logging.logSync(`(DESTROY) Id: ${asyncId}`);
}

/**
 * Called when a Promise resource's resolve function is passed to the Promise constructor.
 *
 * @param asyncId
 */
function promiseResolve(asyncId) {
  logging.logSync(`(PROMISE) Id: ${asyncId}`);
}

executeTests();
```

```js
/**
 * testOnAsyncHook.js
 */

/**
 * Performs basic tracing for async_hooks.
 * See: https://github.com/lrlna/on-async-hook
 *
 * @type {onAsyncHook}
 */
const onAsyncHook = require('on-async-hook');

// Create asyncHook instance and log data to console.
let stopAsyncHook = onAsyncHook(function (data) {
  console.log(data)
});

// Create server and respond with 'Hello world' to incoming connections on port 8080.
require('http').createServer(function (request, response) {
  response.end('Hello world')
}).listen(8080);

// Stop asyncHook after 2 seconds.
setTimeout(() => {
  stopAsyncHook();
}, 2000);
```

```js
/**
 * createInvalidAsyncHook.js
 */

const async_hooks = require('async_hooks');
const logging = require('logging');

function executeTests () {
  let callbacks = {
    // Setting init object to non-function type.
    init: false,
    before: before,
    after: after,
    destroy: destroy,
    promiseResolve: promiseResolve
  };
  logging.lineSeparator('async_hooks.createHook(...)', 60);
  getAsyncHook(callbacks);
}

/**
 * Gets an AsyncHook instance.
 *
 * @param callbacks
 * @returns {*}
 */
function getAsyncHook (callbacks) {
  try {
    // Get instance from constructor, log to console, and return object.
    let asyncHook = async_hooks.createHook(callbacks).enable();

    // Create server and listen on port 8080.
    require('net').createServer(() => {}).listen(8080, () => {
      // Output server ready message after 1 second.
      setTimeout(() => {
        logging.lineSeparator('SERVER ACCEPTING CONNECTIONS', 60);
      }, 1000);
    });

    return asyncHook;
  } catch (e) {
    // Catch TypeError with code property of ERR_ASYNC_CALLBACK.
    if (e instanceof TypeError && e.code === 'ERR_ASYNC_CALLBACK') {
      // Output expected ERR_ASYNC_CALLBACK TypeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

/**
 * Called during object construction.
 *
 * @param asyncId
 * @param type
 * @param triggerAsyncId
 * @param resource
 */
function init(asyncId, type, triggerAsyncId, resource) {
  const executionId = async_hooks.executionAsyncId();
  logging.logSync(`(INIT) Id: ${asyncId}, TriggerId: ${triggerAsyncId}, ExecutionId: ${executionId}, Type: ${type}`);
}

/**
 * Called before resource's callback.
 *
 * @param asyncId
 */
function before(asyncId) {
  logging.logSync(`(BEFORE) Id: ${asyncId}`);
}

/**
 * Called after resource's callback.
 *
 * @param asyncId
 */
function after(asyncId) {
  logging.logSync(`(AFTER) Id: ${asyncId}`);
}

/**
 * Called when AsyncWrap instance is destroyed.
 *
 * @param asyncId
 */
function destroy(asyncId) {
  logging.logSync(`(DESTROY) Id: ${asyncId}`);
}

/**
 * Called when a Promise resource's resolve function is passed to the Promise constructor.
 *
 * @param asyncId
 */
function promiseResolve(asyncId) {
  logging.logSync(`(PROMISE) Id: ${asyncId}`);
}

executeTests();
```

```js
// logging module - app.js
const fs = require("fs");
const SeparatorCharacterDefault = '-';
const SeparatorLengthDefault = 40;

module.exports = {
  /**
   * Outputs a line separator via console.log, with optional first argument text centered in the middle.
   */
  lineSeparator: function () {
    // Check if at least one argument of string type is passed.
    if (arguments.length >= 1 && typeof(arguments[0]) === 'string') {
      lineSeparatorWithInsert(arguments[0], arguments[1], arguments[2]);
    } else {
      // Otherwise, assume default separator without insertion.
      lineSeparator(arguments[0], arguments[1]);
    }
  },

  /**
   * Log the passed object or value.
   *
   * @param value Value to be logged to the console.
   */
  log: function (value) {
    if (value instanceof Error) {
      logError(value, arguments[1]);
    } else {
      logValue(value);
    }
  },

  /**
   * Synchronously log the passed object or value.
   *
   * @param value Value to be logged to the console.
   */
  logSync: function (value) {
    if (value instanceof Error) {
      logError(value, arguments[1], true);
    } else {
      logValue(value, true);
    }
  }
};

/**
 * Outputs a line separator via console.log.
 *
 * @param length Total separator length.
 * @param char Separator character.
 */
function lineSeparator (length = SeparatorLengthDefault, char = SeparatorCharacterDefault) {
  // Default output to insertion.
  logValue(Array(length).join(char));
}

/**
 * Outputs a line separator via console.log with inserted text centered in the middle.
 *
 * @param insert Inserted text to be centered.
 * @param length Total separator length.
 * @param char Separator character.
 */
function lineSeparatorWithInsert (insert, length = SeparatorLengthDefault, char = SeparatorCharacterDefault) {
  // Default output to insertion.
  let output = insert;

  if (insert.length < length) {
    // Update length based on insert length, less a space for margin.
    length -= insert.length + 2;
    // Halve the length and floor left side.
    let left = Math.floor(length / 2);
    let right = left;
    // If odd number, add dropped remainder to right side.
    if ((length % 2) !== 0) {
      right += 1;
    }

    // Surround insert with separators.
    output = `${Array(left).join(char)} ${insert} ${Array(right).join(char)}`;
  }

  logValue(output);
}

/**
 * Logs an Error with explicit/inexplicit tag, error name, and message.
 *
 * @param error Error to be logged.
 * @param explicit Determines if passed Error was explicit (intended) or not.
 * @param synchronous Determines if output must be synchronous.
 */
function logError(error, explicit = true, synchronous = false) {
  let message = `[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`;
  // To avoid duplication get stack without initial error message line.
  let stack = error.stack.slice(error.stack.indexOf("\n") + 1);
  // Use fs.writeSync for synchronous output, otherwise console.log.
  if (synchronous) {
    fs.writeSync(1, message + '\n');
    fs.writeSync(1, stack);
  } else {
    console.log(message);
    console.log(stack);
  }
}

/**
 * Logs a value (string, object, number, etc).
 *
 * @param value Value to be logged.
 * @param synchronous Determines if output must be synchronous.
 */
function logValue(value, synchronous = false) {
  // Use fs.writeSync for synchronous output, otherwise console.log.
  if (synchronous) {
    fs.writeSync(1, value + '\n');
  } else {
    console.log(value);
  }
}
```

## When Should You Use It?

We won't go too in-depth on it since it's a big topic, but we should briefly review how Node handles asynchronous hooks via the [`async_hooks`](https://nodejs.org/api/async_hooks.html) module.  The `async_hooks` module provides a way to register callbacks for tracking the lifetime of asynchronous resources within your application.  In this context a `resource` is any object with an associated callback, such as a `net` connection, and each resource can (and will often be) called multiple times.  A resource can also be closed _before_ the callback is called.

The basic [`AsyncHook` class constructor](https://github.com/nodejs/node/blob/3b8da4cbe8a7f36fcd8892c6676a55246ba8c3be/lib/async_hooks.js#L44) accepts a handful of parameters, each of which is expected to be the callback function for different stages in the asynchronous execution process.  As seen in the source code, each of these arguments, if defined, is expected to be a function so a callback can be performed.

We'll test `async_hooks` by defining each of the five basic callback functions:

```js
/**
 * Called during object construction.
 *
 * @param asyncId
 * @param type
 * @param triggerAsyncId
 * @param resource
 */
function init(asyncId, type, triggerAsyncId, resource) {
  const executionId = async_hooks.executionAsyncId();
  logging.logSync(`(INIT) Id: ${asyncId}, TriggerId: ${triggerAsyncId}, ExecutionId: ${executionId}, Type: ${type}`);
}

/**
 * Called before resource's callback.
 *
 * @param asyncId
 */
function before(asyncId) {
  logging.logSync(`(BEFORE) Id: ${asyncId}`);
}

/**
 * Called after resource's callback.
 *
 * @param asyncId
 */
function after(asyncId) {
  logging.logSync(`(AFTER) Id: ${asyncId}`);
}

/**
 * Called when AsyncWrap instance is destroyed.
 *
 * @param asyncId
 */
function destroy(asyncId) {
  logging.logSync(`(DESTROY) Id: ${asyncId}`);
}

/**
 * Called when a Promise resource's resolve function is passed to the Promise constructor.
 *
 * @param asyncId
 */
function promiseResolve(asyncId) {
  logging.logSync(`(PROMISE) Id: ${asyncId}`);
}
```

Now we'll add each of these functions to the appropriate `callbacks` object attribute, which is passed to the `async_hooks` constructor:

```js
let callbacks = {
  init: init,
  before: before,
  after: after,
  destroy: destroy,
  promiseResolve: promiseResolve
};

/**
 * Gets an AsyncHook instance.
 *
 * @param callbacks
 * @returns {*}
 */
function getAsyncHook (callbacks) {
  try {
    // Get instance from constructor, log to console, and return object.
    let asyncHook = async_hooks.createHook(callbacks).enable();

    // Create server and listen on port 8080.
    require('net').createServer(() => {}).listen(8080, () => {
      // Output server ready message after 1 second.
      setTimeout(() => {
        logging.lineSeparator('SERVER ACCEPTING CONNECTIONS', 60);
      }, 1000);
    });

    return asyncHook;
  } catch (e) {
    // Catch TypeError with code property of ERR_ASYNC_CALLBACK.
    if (e instanceof TypeError && e.code === 'ERR_ASYNC_CALLBACK') {
      // Output expected ERR_ASYNC_CALLBACK TypeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}
```

After creating a hook with the passed `callbacks` we `enable()` it, then create a basic server and listen on port `8080` for incoming connections.  We also wait one second to output a basic message to the log.  Meanwhile, all the `async_hooks` callback functions are outputting info when they are invoked.

Executing this test code produces the following output:

```
-------------- async_hooks.createHook(...) ---------------
(INIT) Id: 8, TriggerId: 1, ExecutionId: 1, Type: TCPWRAP
(INIT) Id: 9, TriggerId: 8, ExecutionId: 1, Type: TickObject
(BEFORE) Id: 7
(AFTER) Id: 7
(BEFORE) Id: 9
(INIT) Id: 10, TriggerId: 9, ExecutionId: 9, Type: Timeout
(INIT) Id: 11, TriggerId: 9, ExecutionId: 9, Type: TIMERWRAP
(AFTER) Id: 9
(DESTROY) Id: 7
(DESTROY) Id: 9
(BEFORE) Id: 6
(AFTER) Id: 6
(DESTROY) Id: 6
(BEFORE) Id: 11
(BEFORE) Id: 10
(INIT) Id: 12, TriggerId: 4, ExecutionId: 10, Type: WRITEWRAP
-------------- SERVER ACCEPTING CONNECTIONS --------------
(INIT) Id: 13, TriggerId: 10, ExecutionId: 10, Type: TickObject
(AFTER) Id: 10
(AFTER) Id: 11
(BEFORE) Id: 13
(AFTER) Id: 13
(DESTROY) Id: 10
(DESTROY) Id: 13
(DESTROY) Id: 11
(BEFORE) Id: 12
(AFTER) Id: 12
(DESTROY) Id: 12
```

This output shows the basic execution pattern of these asynchronous callbacks, including the callback function, the asynchronous `id`, and (in the case of `init`), the `type`.  Since our server isn't explicitly halted in our code it will sit and wait for connections, so we can explicitly connect via `curl` in a terminal:

```
$ curl localhost:8080
```

And this will immediately trigger additional asynchronous callbacks with new Ids:

```
(INIT) Id: 14, TriggerId: 8, ExecutionId: 0, Type: TCPWRAP
(BEFORE) Id: 8
(AFTER) Id: 8
(BEFORE) Id: 14
(INIT) Id: 15, TriggerId: 14, ExecutionId: 14, Type: TickObject
(AFTER) Id: 14
(BEFORE) Id: 15
(AFTER) Id: 15
(DESTROY) Id: 15
```

We're missing a good deal of useful information, such as timestamps, so we can execute the `testOnAsyncHook.js` file, which uses the handy [on-async-hook](https://github.com/lrlna/on-async-hook) package to output such trace details:

```js
/**
 * testOnAsyncHook.js
 */

/**
 * Performs basic tracing for async_hooks.
 * See: https://github.com/lrlna/on-async-hook
 *
 * @type {onAsyncHook}
 */
const onAsyncHook = require('on-async-hook');

// Create asyncHook instance and log data to console.
let stopAsyncHook = onAsyncHook(function (data) {
  console.log(data)
});

// Create server and respond with 'Hello world' to incoming connections on port 8080.
require('http').createServer(function (request, response) {
  response.end('Hello world')
}).listen(8080);

// Stop asyncHook after 2 seconds.
setTimeout(() => {
  stopAsyncHook();
}, 2000);
```

All we're doing here is creating an `onAsyncHook(...)` instance and having it log trace data to the console.  We then create a basic server that responds to incoming requests with `Hello world`, before we automatically stop the async hook after two seconds.  Executing this test produces the following output:

```
...
{ startTime: 267101289811886,
  id: 19392,
  spans: 
   [ { id: 19392,
       type: 'WRITEWRAP',
       parent: 4,
       startTime: 267101289811886,
       endTime: 267101289829684,
       duration: 17798 } ],
  endTime: 267101289829684,
  duration: 17798 }
{ startTime: 267101289959391,
  id: 19394,
  spans: 
   [ { id: 19394,
       type: 'WRITEWRAP',
       parent: 4,
       startTime: 267101289959391,
       endTime: 267101289982309,
       duration: 22918 } ],
  endTime: 267101289982309,
  duration: 22918 }
{ startTime: 267101290196373,
  id: 19396,
  spans: 
   [ { id: 19396,
       type: 'WRITEWRAP',
       parent: 4,
       startTime: 267101290196373,
       endTime: 267101290227337,
       duration: 30964 } ],
  endTime: 267101290227337,
  duration: 30964 }
...
```

Tracing modules like this can help you perform very explicit tracking and management of the various resources being used and invoked in your Node application.  However, this is just to improve the output we see, but let's perform one more test where we don't pass exactly the right argument types to the `async_hooks.createHook(...)` constructor:

```js
let callbacks = {
  // Setting init object to non-function type.
  init: false,
  before: before,
  after: after,
  destroy: destroy,
  promiseResolve: promiseResolve
};

/**
 * Gets an AsyncHook instance.
 *
 * @param callbacks
 * @returns {*}
 */
function getAsyncHook (callbacks) {
  try {
    // Get instance from constructor, log to console, and return object.
    let asyncHook = async_hooks.createHook(callbacks).enable();

    // Create server and listen on port 8080.
    require('net').createServer(() => {}).listen(8080, () => {
      // Output server ready message after 1 second.
      setTimeout(() => {
        logging.lineSeparator('SERVER ACCEPTING CONNECTIONS', 60);
      }, 1000);
    });

    return asyncHook;
  } catch (e) {
    // Catch TypeError with code property of ERR_ASYNC_CALLBACK.
    if (e instanceof TypeError && e.code === 'ERR_ASYNC_CALLBACK') {
      // Output expected ERR_ASYNC_CALLBACK TypeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}
```

Everything is the same as before, except you may notice that the `callbacks.init` property is set to `false`, rather than assigning it to the existing `init` function.  When we execute this code we suddenly get a `TypeError` with an `ERR_ASYNC_CALLBACK` code, indicating that the `init` argument passed to the `AsyncHook` constructor should be a function:

```
-------------- async_hooks.createHook(...) ---------------
[EXPLICIT] TypeError [ERR_ASYNC_CALLBACK]: init must be a function
    at new AsyncHook (async_hooks.js:118:13)
    at Object.createHook (async_hooks.js:238:10)
    at getAsyncHook (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_ASYNC_CALLBACK\createInvalidAsyncHook.js:30:33)
    at executeTests (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_ASYNC_CALLBACK\createInvalidAsyncHook.js:18:3)
    at Object.<anonymous> (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_ASYNC_CALLBACK\createInvalidAsyncHook.js:102:1)
    at Module._compile (module.js:632:14)
    at Object.Module._extensions..js (module.js:646:10)
    at Module.load (module.js:554:32)
    at tryModuleLoad (module.js:497:12)
    at Function.Module._load (module.js:489:3)
```

Airbrake's robust <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-nodejs-error-handling">error monitoring software</a> provides real-time error monitoring and automatic error reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize error parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-nodejs-error-handling">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look at the ERR_ASYNC_CALLBACK TypeError in Node.js, with sample code showing the basics of using asynchronous callbacks in Node.

---

__SOURCES__

- https://nodejs.org/api/errors.html
- https://github.com/nodejs/