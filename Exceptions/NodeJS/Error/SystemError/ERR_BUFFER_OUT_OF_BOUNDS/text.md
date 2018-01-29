---
categories: [NodeJS Error Handling]
date: 2018-01-29
published: true
title: "Node.js Error Handling - ERR_BUFFER_OUT_OF_BOUNDS"
---

Node.js contains a plethora of possible errors it can throw during execution, so today we'll continue our in-depth [**Node.js Error Handling**](https://airbrake.io/blog/nodejs-error-handling/nodejs-error-class-hierarchy) series by looking at one of the many `System Errors` Node produces: the **ERR_BUFFER_OUT_OF_BOUNDS**.  Node throws a `System Error` when an exception occurs within the program's runtime environment, and such errors are typically an indication that there was an operational problem within the application.  An `ERR_BUFFER_OUT_OF_BOUNDS` error indicates that an operation was made outside the bounds of a [`Buffer`](https://nodejs.org/api/buffer.html) object.

Throughout this article we'll explore the `ERR_BUFFER_OUT_OF_BOUNDS` system error by looking at where it sits in the overall [Node.js Error Class Hierarchy](https://airbrake.io/blog/nodejs-error-handling/nodejs-error-class-hierarchy).  We'll also explore the basic process of using [`Buffers`](https://nodejs.org/api/buffer.html) in Node.js, which will illustrate how passing invalid bounds arguments (offsets or lengths, in particular) to some `Buffer` class methods can throw `ERR_BUFFER_OUT_OF_BOUNDS` errors.  Let's get into it!

## The Technical Rundown

Most Node.js errors inherit from the [`Error`](https://nodejs.org/api/errors.html#errors_class_error) base class, or extend from an inherited class therein.  The full error hierarchy of this error is:

- [`Error`](https://nodejs.org/api/errors.html#errors_class_error)
    - [`SystemError`](https://nodejs.org/dist/latest-v8.x/docs/api/errors.html#errors_system_errors)
      - `ERR_BUFFER_OUT_OF_BOUNDS`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```js
/**
 * index.js
 */
const logging = require('logging');

function executeTests () {
  // Create int8 array populated with first 5 numeric key values.
  const data = Uint8Array.from(new Uint8Array(5).keys());

  logging.lineSeparator('bufferFromValue(data.buffer)', 60);
  bufferFromValue(data.buffer);

  logging.lineSeparator('bufferFromValue(data.buffer, 3)', 60);
  bufferFromValue(data.buffer, 3);

  logging.lineSeparator('bufferFromValue(data.buffer, 6)', 60);
  bufferFromValue(data.buffer, 6);

  logging.lineSeparator('bufferFromValue(data.buffer, 2, 7)', 60);
  bufferFromValue(data.buffer, 2, 7);
}

/**
 * Retrieves a buffer from passed value, using optional encoding/offset and length.
 *
 * @param value String, buffer, array-like Object to get buffer from.
 * @param encodingOrOffset Encoding (for Strings) or offset (for array-like Objects) at which to offset returned buffer.
 * @param length Length of buffer to retrieve.
 * @returns {Buffer2} Generated buffer.
 */
function bufferFromValue (value, encodingOrOffset, length) {
  try {
    // Invokes Buffer.from method with passed parameters.
    let buffer = Buffer.from(value, encodingOrOffset, length);
    // Log generated buffer.
    logging.log(buffer);
    // Return generated buffer.
    return buffer;
  } catch (e) {
    if (e instanceof RangeError && e.code === 'ERR_BUFFER_OUT_OF_BOUNDS') {
      // Output expected ERR_BUFFER_OUT_OF_BOUNDS RangeErrors.
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
// logging module - index.js
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

The [`Buffer`](https://nodejs.org/api/buffer.html) class was introduced into early versions of Node.js to give developers a means of reading and manipulating streams of binary data.  Since then, however, [JavaScript ES6 (ECMAScript 2015)](https://airbrake.io/blog/javascript/es6-javascript-whats-new-1) introduced the [`TypedArray`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/TypedArray) object to handle binary data buffers.  Therefore, modern Node.js versions have adapted the [`Buffer`](https://nodejs.org/api/buffer.html) class to focus on implementing a more optimized [`Uint8Array`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Uint8Array) API.

It's also worth briefly noting that the `Buffer` class constructor [has been deprecated](https://github.com/nodejs/node/blob/master/lib/buffer.js#L149-L159), so the process of actually creating buffers should use the `Buffer.from()`, `buffer.allocUnsafe()`, and `buffer.alloc()` methods.  For our example code we'll be using the simplest of these, `Buffer.from(value, encodingOrOffset, length)`, which retrieves a binary buffer representation of the first `value` argument passed to it.  We can also specify additional arguments to alter the offset or length of the returned buffer.

To test this stuff out we have a simple `bufferFromValue (value, encodingOrOffset, length)` helper method:

```js
/**
 * Retrieves a buffer from passed value, using optional encoding/offset and length.
 *
 * @param value String, buffer, array-like Object to get buffer from.
 * @param encodingOrOffset Encoding (for Strings) or offset (for array-like Objects) at which to offset returned buffer.
 * @param length Length of buffer to retrieve.
 * @returns {Buffer2} Generated buffer.
 */
function bufferFromValue (value, encodingOrOffset, length) {
  try {
    // Invokes Buffer.from method with passed parameters.
    let buffer = Buffer.from(value, encodingOrOffset, length);
    // Log generated buffer.
    logging.log(buffer);
    // Return generated buffer.
    return buffer;
  } catch (e) {
    if (e instanceof RangeError && e.code === 'ERR_BUFFER_OUT_OF_BOUNDS') {
      // Output expected ERR_BUFFER_OUT_OF_BOUNDS RangeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}
```

As you can see, this method is merely a wrapper for the aforementioned `Buffer.from(...)` method, but it allows us to perform some simple, self-contained tests by passing different arguments.  As such, we begin our tests by creating a simple `data` `Uint8Array` that is populated with the first five numeric values as keys (i.e. `0`, `1`, `2`, `3`, and `4`):

```js
// Create int8 array populated with first 5 keys.
const data = Uint8Array.from(new Uint8Array(5).keys());

logging.lineSeparator('bufferFromValue(data.buffer)', 60);
bufferFromValue(data.buffer);
```

We then pass the `data.buffer` value to `bufferFromValue(...)` to ensure everything works.  Sure enough, the output we get is our `Buffer` object with the five expected values:

```
-------------- bufferFromValue(data.buffer) --------------
<Buffer 00 01 02 03 04>
```

Next, let's try passing an `offset` value of `3`:

```js
logging.lineSeparator('bufferFromValue(data.buffer, 3)', 60);
bufferFromValue(data.buffer, 3);
```

This also works just fine and outputs the same `Buffer` object as before, except we've offset the result by three values:

```
------------ bufferFromValue(data.buffer, 3) -------------
<Buffer 03 04>
```

Cool!  However, what happens if we pass an `offset` value that exceeds the bounds of the original array-like object?

```js
logging.lineSeparator('bufferFromValue(data.buffer, 6)', 60);
bufferFromValue(data.buffer, 6);
```

As you can certainly guess, this throws a `RangeError` with a `code` property of `ERR_BUFFER_OUT_OF_BOUNDS`:

```
------------ bufferFromValue(data.buffer, 6) -------------
[EXPLICIT] RangeError [ERR_BUFFER_OUT_OF_BOUNDS]: "offset" is outside of buffer bounds
    at fromArrayBuffer (buffer.js:375:11)
    at Function.from (buffer.js:192:12)
    at bufferFromValue (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_BUFFER_OUT_OF_BOUNDS\index.js:34:25)
    at executeTests (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_BUFFER_OUT_OF_BOUNDS\index.js:17:3)
    at Object.<anonymous> (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_BUFFER_OUT_OF_BOUNDS\index.js:50:1)
    at Module._compile (module.js:657:14)
    at Object.Module._extensions..js (module.js:671:10)
    at Module.load (module.js:573:32)
    at tryModuleLoad (module.js:513:12)
    at Function.Module._load (module.js:505:3)
```

Since our buffer bounds only go up to `5`, an `offset` of `6` or higher is invalid.  Similarly, let's try passing an in-bounds `offset` with an invalid `length` argument:

```
logging.lineSeparator('bufferFromValue(data.buffer, 2, 7)', 60);
bufferFromValue(data.buffer, 2, 7);
```

Once again, this throws an `ERR_BUFFER_OUT_OF_BOUNDS` `RangeError`, this time with the indication that `length` is outside the buffer bounds:

```
----------- bufferFromValue(data.buffer, 2, 7) -----------
[EXPLICIT] RangeError [ERR_BUFFER_OUT_OF_BOUNDS]: "length" is outside of buffer bounds
    at fromArrayBuffer (buffer.js:387:15)
    at Function.from (buffer.js:192:12)
    at bufferFromValue (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_BUFFER_OUT_OF_BOUNDS\index.js:34:25)
    at executeTests (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_BUFFER_OUT_OF_BOUNDS\index.js:20:3)
    at Object.<anonymous> (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_BUFFER_OUT_OF_BOUNDS\index.js:50:1)
    at Module._compile (module.js:657:14)
    at Object.Module._extensions..js (module.js:671:10)
    at Module.load (module.js:573:32)
    at tryModuleLoad (module.js:513:12)
    at Function.Module._load (module.js:505:3)
```

If we dig into the Node.js `Buffer` class source code we can quickly find exactly where these errors originate from.  In this case, they are both thrown within the [`fromArrayBuffer(obj, byteOffset, length)` internal function](https://github.com/nodejs/node/blob/master/lib/buffer.js#L362-L392):

```js
// Source: https://github.com/nodejs/node/blob/master/lib/buffer.js#L362-L392
function fromArrayBuffer(obj, byteOffset, length) {
  // convert byteOffset to integer
  if (byteOffset === undefined) {
    byteOffset = 0;
  } else {
    byteOffset = +byteOffset;
    // check for NaN
    if (byteOffset !== byteOffset)
      byteOffset = 0;
  }

  const maxLength = obj.byteLength - byteOffset;

  if (maxLength < 0)
    throw new errors.RangeError('ERR_BUFFER_OUT_OF_BOUNDS', 'offset');

  if (length === undefined) {
    length = maxLength;
  } else {
    // convert length to non-negative integer
    length = +length;
    // Check for NaN
    if (length !== length) {
      length = 0;
    } else if (length > 0) {
      if (length > maxLength)
        throw new errors.RangeError('ERR_BUFFER_OUT_OF_BOUNDS', 'length');
    } else {
      length = 0;
    }
  }
```

Airbrake's robust <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-nodejs-error-handling">error monitoring software</a> provides real-time error monitoring and automatic error reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize error parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-nodejs-error-handling">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A deep dive into the ERR_BUFFER_OUT_OF_BOUNDS RangeError in Node.js, with sample code showing how to create Buffers and avoid these RangeErrors.

---

__SOURCES__

- https://nodejs.org/api/errors.html
- https://github.com/nodejs/