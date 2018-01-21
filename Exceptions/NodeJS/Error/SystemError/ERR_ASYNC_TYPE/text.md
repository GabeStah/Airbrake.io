---
categories: [NodeJS Error Handling]
date: 2018-01-22
published: true
title: Node.js Error Handling - ERR_ASYNC_TYPE
---

The number of possible Node.js errors is extensive, so today we continue our detailed [**Node.js Error Handling**](https://airbrake.io/blog/nodejs-error-handling/nodejs-error-class-hierarchy) series by looking at one of the many `System Errors`-categorized errors called **ERR_ASYNC_TYPE**.  Node throws a `System Error` when an exception occurs within the program's runtime environment, and such errors are typically an indication that there was an operational problem within the application.  An `ERR_ASYNC_TYPE` error indicates that an attempt was made to pass an invalid data type to the [`AsyncResource`](https://github.com/nodejs/node/blob/master/doc/api/async_hooks.md#class-asyncresource) class constructor.

Throughout this article we'll explore the `ERR_ASYNC_TYPE` system error by looking at where it sits in the overall [Node.js Error Class Hierarchy](https://airbrake.io/blog/nodejs-error-handling/nodejs-error-class-hierarchy).  We'll also examine the basics of using the [`AsyncResource`](https://github.com/nodejs/node/blob/master/doc/api/async_hooks.md#class-asyncresource) class, and how passing invalid arguments might result in `ERR_ASYNC_TYPEs`, so let's get to it!

## The Technical Rundown

Most Node.js errors inherit from the [`Error`](https://nodejs.org/api/errors.html#errors_class_error) base class, or extend from an inherited class therein.  The full error hierarchy of this error is:

- [`Error`](https://nodejs.org/api/errors.html#errors_class_error)
    - [`SystemError`](https://nodejs.org/dist/latest-v8.x/docs/api/errors.html#errors_system_errors)
      - `ERR_ASYNC_TYPE`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```js
/**
 * asyncResourceTests.js
 */
const async_hooks = require('async_hooks');
const { AsyncResource, executionAsyncId } = async_hooks;
const logging = require('logging');

function executeTests () {
  logging.lineSeparator("getAsyncResource('MyNetResource', ...)", 60);
  getAsyncResource('MyNetResource', { triggerAsyncId: executionAsyncId() } );

  logging.lineSeparator("getAsyncResource(24601)", 60);
  getAsyncResource(24601);

  logging.lineSeparator("getAsyncResource('')", 60);
  getAsyncResource('');
}

/**
 * Creates an AsyncResource instance using passed options, performing basic net.server connection test.
 *
 * @param options Type and other arguments.
 */
function getAsyncResource (options) {
  try {
    // Create AsyncResource.
    let resource = new AsyncResource(options);

    // Create server and listen on port 8080.
    let server = require('net').createServer(() => {
    }).listen(8080, () => {
      // Invoke resource.emitBefore().
      logging.logSync(`resource.emitBefore(): ${resource.emitBefore()}`);
      resource.emitBefore()

      // Output server ready message after 1 second.
      setTimeout(() => {
        logging.lineSeparator('SERVER ACCEPTING CONNECTIONS', 60);
        logging.logSync(`resource.asyncId(): ${resource.asyncId()}`);
        logging.logSync(`resource.triggerAsyncId(): ${resource.triggerAsyncId()}`);
      }, 1000);

      // Invoke resource.emitAfter().
      logging.logSync(`resource.emitAfter(): ${resource.emitAfter()}`);
      resource.emitAfter()

      // Close connection after 3 seconds.
      setTimeout(() => {
        server.close();
      }, 3000);
    });

    // Invoke resource.emitDestroy() when server closed.
    server.on('close', function () {
      logging.logSync(`resource.emitDestroy(): ${resource.emitDestroy()}`);
      resource.emitDestroy()
    })

    return resource;
  } catch (e) {
    if (e instanceof TypeError && e.code === 'ERR_ASYNC_TYPE') {
      // Output expected ERR_ASYNC_TYPE TypeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

executeTests();
```

```js
/**
 * logging module: app.js
 */
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

As some readers may recall we explored the [`Node.js ERR_ASYNC_CALLBACK`](https://airbrake.io/blog/nodejs-error-handling/err_async_callback) error in an [article last week](https://airbrake.io/blog/nodejs-error-handling/err_async_callback), which was similar to the `ERR_ASYNC_TYPE` error, except the `ERR_ASYNC_CALLBACK` error indicated an invalid data type used as `async_hook` callback arguments.  On the other hand, the `ERR_ASYNC_TYPE` error is a result of using the `AsyncResource` class constructor and passing an invalid `type` parameter.  To illustrate this difference we'll start with a simple code sample that creates a new `AsyncResource` instance, uses it in conjunction with a basic `net` module `server` instance to connect to `localhost:8080` and send a message:

```js
/**
 * Creates an AsyncResource instance using passed options, performing basic net.server connection test.
 *
 * @param options Type and other arguments.
 */
function getAsyncResource (options) {
  try {
    // Create AsyncResource.
    let resource = new AsyncResource(options);

    // Create server and listen on port 8080.
    let server = require('net').createServer(() => {
    }).listen(8080, () => {
      // Invoke resource.emitBefore().
      logging.logSync(`resource.emitBefore(): ${resource.emitBefore()}`);
      resource.emitBefore()

      // Output server ready message after 1 second.
      setTimeout(() => {
        logging.lineSeparator('SERVER ACCEPTING CONNECTIONS', 60);
        logging.logSync(`resource.asyncId(): ${resource.asyncId()}`);
        logging.logSync(`resource.triggerAsyncId(): ${resource.triggerAsyncId()}`);
      }, 1000);

      // Invoke resource.emitAfter().
      logging.logSync(`resource.emitAfter(): ${resource.emitAfter()}`);
      resource.emitAfter()

      // Close connection after 3 seconds.
      setTimeout(() => {
        server.close();
      }, 3000);
    });

    // Invoke resource.emitDestroy() when server closed.
    server.on('close', function () {
      logging.logSync(`resource.emitDestroy(): ${resource.emitDestroy()}`);
      resource.emitDestroy()
    })

    return resource;
  } catch (e) {
    if (e instanceof TypeError && e.code === 'ERR_ASYNC_TYPE') {
      // Output expected ERR_ASYNC_TYPE TypeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}
```

The typical usage of `AsyncResource` is to extend it with your own class (e.g. `class MyAsyncObject extends AsyncResource { ... }`), but for our simple example we're just directly invoking the callback emitters, such as `emitBefore()`.  Each of these emitters invokes all associated callbacks, which means the `emitBefore()` method invokes all `before` callbacks.

We'll test out our `getAsyncResource(options)` function with a handful of tests, each of which passes a different set of values into the `AsyncResource(...)` constructor:

```js
function executeTests () {
  logging.lineSeparator("getAsyncResource('MyNetResource', ...)", 60);
  getAsyncResource('MyNetResource', { triggerAsyncId: executionAsyncId() } );

  logging.lineSeparator("getAsyncResource(24601)", 60);
  getAsyncResource(24601);

  logging.lineSeparator("getAsyncResource('')", 60);
  getAsyncResource('');
}
```

Executing the first of these tests works as expected and produces the following output:

```
--------- getAsyncResource('MyNetResource', ...) ---------
resource.emitBefore(): [object Object]
resource.emitAfter(): [object Object]
-------------- SERVER ACCEPTING CONNECTIONS --------------
resource.asyncId(): 8
resource.triggerAsyncId(): 1
resource.emitDestroy(): [object Object]
```

Our `net` server is created and connects, then performs its basic process of sending a `SERVER ACCEPTING CONNECTIONS` message after a 1-second delay.  We also retrieve the `asyncId` and `triggerAsyncId` values from the `AsyncResource` instance, before eventually closing the connection after 2 more seconds and invoking `resource.emitDestroy()`.

The `'MyNetResource'` string is the `type` argument passed into `AsyncResource`, so what happens if we pass a numeric value like `24601` as seen in our second test?  We're presented with an error, though not the `ERR_ASYNC_TYPE`, but an `ERR_INVALID_ARG_TYPE` instead, indicating that the `type` argument must be a string:

```
---------------- getAsyncResource(24601) -----------------
[INEXPLICIT] TypeError [ERR_INVALID_ARG_TYPE]: The "type" argument must be of type string
    at new AsyncResource (async_hooks.js:266:13)
    at getAsyncResource (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_ASYNC_TYPE\asyncResourceTests.js:27:20)
    at executeTests (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_ASYNC_TYPE\asyncResourceTests.js:13:4)
    at Object.<anonymous> (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_ASYNC_TYPE\asyncResourceTests.js:71:1)
    at Module._compile (module.js:632:14)
    at Object.Module._extensions..js (module.js:646:10)
    at Module.load (module.js:554:32)
    at tryModuleLoad (module.js:497:12)
    at Function.Module._load (module.js:489:3)
    at Function.Module.runMain (module.js:676:10)
```

Alright, so our last test passes an empty string as the `type` argument, which produces the following output:

```
------------------ getAsyncResource('') ------------------
[EXPLICIT] TypeError [ERR_ASYNC_TYPE]: Invalid name for async "type": 
    at emitInitScript (async_hooks.js:370:11)
    at new AsyncResource (async_hooks.js:279:5)
    at getAsyncResource (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_ASYNC_TYPE\asyncResourceTests.js:27:20)
    at executeTests (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_ASYNC_TYPE\asyncResourceTests.js:16:3)
    at Object.<anonymous> (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_ASYNC_TYPE\asyncResourceTests.js:71:1)
    at Module._compile (module.js:632:14)
    at Object.Module._extensions..js (module.js:646:10)
    at Module.load (module.js:554:32)
    at tryModuleLoad (module.js:497:12)
    at Function.Module._load (module.js:489:3)
```

Here we see an `ERR_ASYNC_TYPE` error, which illustrates an interesting and subtle difference between what type of parameters the `AsyncResource` constructor expects -- since anything that isn't a string throws an `ERR_INVALID_ARG_TYPE`, the _only_ scenario in which an `ERR_ASYNC_TYPE` error is (currently) thrown is when passing an empty string.  The reason for this result can be seen by digging into the Node.js source code a bit within the `async_hooks.js` files:

```js
/**
 * node/lib/async_hooks.js
 */
const internal_async_hooks = require('internal/async_hooks');

// ...
const {
  // ...
  emitInit,
  // ...
} = internal_async_hooks;

class AsyncResource {
  constructor(type, opts = {}) {
    if (typeof type !== 'string')
      throw new errors.TypeError('ERR_INVALID_ARG_TYPE', 'type', 'string');

    if (typeof opts === 'number') {
      opts = { triggerAsyncId: opts, requireManualDestroy: false };
    } else if (opts.triggerAsyncId === undefined) {
      opts.triggerAsyncId = getDefaultTriggerAsyncId();
    }

    // Unlike emitInitScript, AsyncResource doesn't supports null as the
    // triggerAsyncId.
    const triggerAsyncId = opts.triggerAsyncId;
    if (!Number.isSafeInteger(triggerAsyncId) || triggerAsyncId < -1) {
      throw new errors.RangeError('ERR_INVALID_ASYNC_ID',
                                  'triggerAsyncId',
                                  triggerAsyncId);
    }

    this[async_id_symbol] = newUid();
    this[trigger_async_id_symbol] = triggerAsyncId;
    // this prop name (destroyed) has to be synchronized with C++
    this[destroyedSymbol] = { destroyed: false };

    emitInit(
      this[async_id_symbol], type, this[trigger_async_id_symbol], this
    );

    if (!opts.requireManualDestroy) {
      registerDestroyHook(this, this[async_id_symbol], this[destroyedSymbol]);
    }
  }

  emitBefore() {
    emitBefore(this[async_id_symbol], this[trigger_async_id_symbol]);
    return this;
  }

  emitAfter() {
    emitAfter(this[async_id_symbol]);
    return this;
  }

  emitDestroy() {
    this[destroyedSymbol].destroyed = true;
    emitDestroy(this[async_id_symbol]);
    return this;
  }

  asyncId() {
    return this[async_id_symbol];
  }

  triggerAsyncId() {
    return this[trigger_async_id_symbol];
  }
}
```

Here we can see that the `AsyncResource` constructor calls the `emitInit(...)` method, which is retrieved from `internal/async_hooks.js` at the top of the file:

```js
const internal_async_hooks = require('internal/async_hooks');

// ...
const {
  // ...
  emitInit,
  // ...
} = internal_async_hooks;
```

The `internal/async_hooks.js` module exports the local `emitInitScript` function as `emitInit`:

```js
module.exports = {
  // ...
  emitInit: emitInitScript,
  // ...
};
```

```js
/**
 * node/lib/internal/async_hooks.js
 */
function emitInitScript(asyncId, type, triggerAsyncId, resource) {
  validateAsyncId(asyncId, 'asyncId');
  if (triggerAsyncId !== null)
    validateAsyncId(triggerAsyncId, 'triggerAsyncId');
  if (async_hook_fields[kCheck] > 0 &&
      (typeof type !== 'string' || type.length <= 0)) {
    throw new errors.TypeError('ERR_ASYNC_TYPE', type);
  }

  // Short circuit all checks for the common case. Which is that no hooks have
  // been set. Do this to remove performance impact for embedders (and core).
  if (async_hook_fields[kInit] === 0)
    return;

  // This can run after the early return check b/c running this function
  // manually means that the embedder must have used getDefaultTriggerAsyncId().
  if (triggerAsyncId === null) {
    triggerAsyncId = getDefaultTriggerAsyncId();
  }

  emitInitNative(asyncId, type, triggerAsyncId, resource);
}
```

Finally, looking at the `emitInitScript` function shows where the `ERR_ASYNC_TYPE` error is coming from.  If `type != 'string'` or the length is less than or equal to zero, a new `ERR_ASYNC_TYPE` err is thrown.

Airbrake's robust <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-nodejs-error-handling">error monitoring software</a> provides real-time error monitoring and automatic error reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize error parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-nodejs-error-handling">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look at the ERR_ASYNC_TYPE TypeError in Node.js, with sample code showing the basics of using the AsyncResource class in Node.

---

__SOURCES__

- https://nodejs.org/api/errors.html
- https://github.com/nodejs/