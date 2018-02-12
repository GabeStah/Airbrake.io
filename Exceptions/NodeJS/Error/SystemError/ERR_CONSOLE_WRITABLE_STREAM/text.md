---
categories: [NodeJS Error Handling]
date: 2018-02-12
published: true
title: "Node.js Error Handling - ERR_CONSOLE_WRITABLE_STREAM"
---

There are many possible errors in Node.js, so today we'll continue our detailed [**Node.js Error Handling**](https://airbrake.io/blog/nodejs-error-handling/nodejs-error-class-hierarchy) series by looking at one of the assorted `System Errors` Node produces, **ERR_BUFFER_TOO_LARGE**.  Node throws a `System Error` when an exception occurs within the program's runtime environment, and such errors are typically an indication that there was an operational problem within the application.  An `ERR_BUFFER_TOO_LARGE` error indicates that an attempt was made to instantiate or allocate a [`Buffer`](https://nodejs.org/api/buffer.html) object of a size exceeding the current maximum (typically `2,147,483,647`, or the maximum size of a 32-bit signed binary integer).

In today's article examine the `ERR_BUFFER_TOO_LARGE` system error by looking at where it resides in the overall [Node.js Error Class Hierarchy](https://airbrake.io/blog/nodejs-error-handling/nodejs-error-class-hierarchy).  We'll also look at some functional code samples illustrating how direct `Buffer` object manipulation doesn't allow method calls that _may_ directly throw `ERR_BUFFER_TOO_LARGE` errors.  Instead, certain modules (like [`zlib`](https://nodejs.org/api/zlib.html)) _can_ potentially generate such errors in extreme circumstances.  Let's dig in!

## The Technical Rundown

Most Node.js errors inherit from the [`Error`](https://nodejs.org/api/errors.html#errors_class_error) base class, or extend from an inherited class therein.  The full error hierarchy of this error is:

- [`Error`](https://nodejs.org/api/errors.html#errors_class_error)
    - [`SystemError`](https://nodejs.org/dist/latest-v8.x/docs/api/errors.html#errors_system_errors)
      - `ERR_BUFFER_TOO_LARGE`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```js
/**
 * index.js
 */
const { kMaxLength } = require('buffer');
const logging = require('logging');

function executeTests () {
  logging.lineSeparator(`instantiateBuffer(1)`, 60);
  instantiateBuffer(1)

  logging.lineSeparator(`instantiateBuffer(kMaxLength)`, 60);
  instantiateBuffer(kMaxLength)

  logging.lineSeparator(`instantiateBuffer(kMaxLength + 1)`, 60);
  instantiateBuffer(kMaxLength + 1);

  logging.lineSeparator(`allocateBuffer(1)`, 60);
  allocateBuffer(1);

  logging.lineSeparator(`allocateBuffer(kMaxLength)`, 60);
  allocateBuffer(kMaxLength);

  logging.lineSeparator(`allocateBuffer(kMaxLength + 1)`, 60);
  allocateBuffer(kMaxLength + 1);
}

/**
 * Allocates a new Buffer of size `size`.
 *
 * @param size Size of Buffer to allocate.
 * @returns {Buffer} Allocated Buffer.
 */
function allocateBuffer (size) {
  try {
    let buffer = Buffer.alloc(size);
    logging.log(`Successfully allocated new Buffer(${size}).`);
    return buffer;
  } catch (e) {
    if (e instanceof RangeError && e.code === 'ERR_BUFFER_TOO_LARGE') {
      // Output expected ERR_BUFFER_TOO_LARGE RangeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

/**
 * Instantiates a new Buffer of size `size`.
 *
 * @param size Size of Buffer to instantiate.
 * @returns {Buffer} Instantiated Buffer.
 */
function instantiateBuffer (size) {
  try {
    let buffer = new Buffer(size);
    logging.log(`Successfully instantiated new Buffer(${size}).`);
    return buffer;
  } catch (e) {
    if (e instanceof RangeError && e.code === 'ERR_BUFFER_TOO_LARGE') {
      // Output expected ERR_BUFFER_TOO_LARGE RangeErrors.
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

As we discussed in our [`ERR_BUFFER_OUT_OF_BOUNDS`](https://airbrake.io/blog/nodejs-error-handling/err_buffer_out_of_bounds) article last week, the [`Buffer`](https://nodejs.org/api/buffer.html) class was added to early versions of Node.js provide simple means of reading and manipulating streams of binary data.  More recently, [JavaScript ES6 (ECMAScript 2015)](https://airbrake.io/blog/javascript/es6-javascript-whats-new-1) introduced the [`TypedArray`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/TypedArray) object to handle binary data buffers.  Therefore, modern Node.js versions have adapted the [`Buffer`](https://nodejs.org/api/buffer.html) class to focus on implementing a more optimized [`Uint8Array`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Uint8Array) API.

In modern Node.js versions the `Buffer` class constructor [has been deprecated](https://github.com/nodejs/node/blob/master/lib/buffer.js#L149-L159), so the process of actually creating buffers would ideally use the `Buffer.from()`, `Buffer.alloc()`, and other similar methods.  For our example code we'll use both techniques, starting with a direct instantiation using the deprecated `new Buffer(...)`  constructor, and afterward we'll also use the `Buffer.alloc()` method.

Our first test method is `instantiateBuffer(size)`, which uses the deprecated `new Buffer(size)` constructor to instantiate a new buffer object:

```js
/**
 * Instantiates a new Buffer of size `size`.
 *
 * @param size Size of Buffer to instantiate.
 * @returns {Buffer} Instantiated Buffer.
 */
function instantiateBuffer (size) {
  try {
    let buffer = new Buffer(size);
    logging.log(`Successfully instantiated new Buffer(${size}).`);
    return buffer;
  } catch (e) {
    if (e instanceof RangeError && e.code === 'ERR_BUFFER_TOO_LARGE') {
      // Output expected ERR_BUFFER_TOO_LARGE RangeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}
```

We'll test out creating three different buffer sizes, `1`, `kMaxLength`, and `kMaxLength + 1`:

```js
const { kMaxLength } = require('buffer');

function executeTests () {
  logging.lineSeparator(`instantiateBuffer(1)`, 60);
  instantiateBuffer(1)

  logging.lineSeparator(`instantiateBuffer(kMaxLength)`, 60);
  instantiateBuffer(kMaxLength)

  logging.lineSeparator(`instantiateBuffer(kMaxLength + 1)`, 60);
  instantiateBuffer(kMaxLength + 1);

  // ...
}
```

`kMaxLength` is exported from `buffer`, which is actually imported from `process.binding('buffer')` and represents the maximum size of a 32-bit signed integer (`2,147,483,647`), which is typically used in Node to limit the sizes of many objects, including buffers.  Executing these three different size tests produces the following output:

```
------------------ instantiateBuffer(1) ------------------
Successfully instantiated new Buffer(1).

------------- instantiateBuffer(kMaxLength) --------------
Successfully instantiated new Buffer(2147483647).

----------- instantiateBuffer(kMaxLength + 1) ------------
[INEXPLICIT] RangeError [ERR_INVALID_OPT_VALUE]: The value "2147483648" is invalid for option "size"
    at Function.alloc (buffer.js:257:3)
    at new Buffer (buffer.js:168:19)
    at instantiateBuffer (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_BUFFER_TOO_LARGE\index.js:57:18)
    at executeTests (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_BUFFER_TOO_LARGE\index.js:15:3)
    at Object.<anonymous> (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_BUFFER_TOO_LARGE\index.js:70:1)
    at Module._compile (module.js:657:14)
    at Object.Module._extensions..js (module.js:671:10)
    at Module.load (module.js:573:32)
    at tryModuleLoad (module.js:513:12)
    at Function.Module._load (module.js:505:3)
```

As you can see, even though the direct `new Buffer(...)` constructor is deprecated it still functions and allows us to create buffer objects of size `1` and `kMaxLength`.  However, a size of `kMaxLength + 1` produces an error, but, surprisingly, it's an `ERR_INVALID_OPT_VALUE` rather than an `ERR_BUFFER_TOO_LARGE`.

Let's try allocation rather than direct instantiation:

```js
/**
 * Allocates a new Buffer of size `size`.
 *
 * @param size Size of Buffer to allocate.
 * @returns {Buffer} Allocated Buffer.
 */
function allocateBuffer (size) {
  try {
    let buffer = Buffer.alloc(size);
    logging.log(`Successfully allocated new Buffer(${size}).`);
    return buffer;
  } catch (e) {
    if (e instanceof RangeError && e.code === 'ERR_BUFFER_TOO_LARGE') {
      // Output expected ERR_BUFFER_TOO_LARGE RangeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}
```

Our `allocateBuffer(size)` method performs really the same process as `instantiateBuffer(size)`, but through the `Buffer.alloc(size)` method instead of `new Buffer(size)`.  Once again, we'll test it using the same trio of size allocations:

```js
logging.lineSeparator(`allocateBuffer(1)`, 60);
allocateBuffer(1);

logging.lineSeparator(`allocateBuffer(kMaxLength)`, 60);
allocateBuffer(kMaxLength);

logging.lineSeparator(`allocateBuffer(kMaxLength + 1)`, 60);
allocateBuffer(kMaxLength + 1);
```

Executing these tests produces nearly identical output as our direct instantiation test:

```
------------------- allocateBuffer(1) --------------------
Successfully allocated new Buffer(1).

--------------- allocateBuffer(kMaxLength) ---------------
Successfully allocated new Buffer(2147483647).

------------- allocateBuffer(kMaxLength + 1) -------------
[INEXPLICIT] RangeError [ERR_INVALID_OPT_VALUE]: The value "2147483648" is invalid for option "size"
    at Function.alloc (buffer.js:257:3)
    at allocateBuffer (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_BUFFER_TOO_LARGE\index.js:35:25)
    at executeTests (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_BUFFER_TOO_LARGE\index.js:24:3)
    at Object.<anonymous> (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_BUFFER_TOO_LARGE\index.js:70:1)
    at Module._compile (module.js:657:14)
    at Object.Module._extensions..js (module.js:671:10)
    at Module.load (module.js:573:32)
    at tryModuleLoad (module.js:513:12)
    at Function.Module._load (module.js:505:3)
    at Function.Module.runMain (module.js:701:10)
```

Hmm, yet another `ERR_INVALID_OPT_VALUE`.  We can actually see why these behave so similarly by digging into the Node [`buffer.js`](https://github.com/nodejs/node/blob/master/lib/buffer.js) module code.  First, we can see inside the direct `new Buffer(...)` constructor that, if the first argument is a `number`, it simply passes execution to the `Buffer.alloc(...)` method on our behalf:

```js
function Buffer(arg, encodingOrOffset, length) {
  doFlaggedDeprecation();
  // Common case.
  if (typeof arg === 'number') {
    if (typeof encodingOrOffset === 'string') {
      throw new errors.TypeError(
        'ERR_INVALID_ARG_TYPE', 'string', 'string', arg
      );
    }
    return Buffer.alloc(arg);
  }
  return Buffer.from(arg, encodingOrOffset, length);
}
```

The `Buffer.alloc(...)` method (along with nearly every other Buffer method in the module) starts by invoking the `assertSize(size)` method:

```js
Buffer.alloc = function alloc(size, fill, encoding) {
  assertSize(size);

  // ...
}
```

Inside `assertSize(size)` we see the final statement that our tests were hitting to throw the `ERR_INVALID_OPT_VALUE`:

```js
function assertSize(size) {
  let err = null;

  if (typeof size !== 'number') {
    err = new errors.TypeError('ERR_INVALID_ARG_TYPE', 'size', 'number', size);
  } else if (size < 0) {
    err = new errors.RangeError('ERR_INVALID_OPT_VALUE', 'size', size);
  } else if (size > kMaxLength) {
    err = new errors.RangeError('ERR_INVALID_OPT_VALUE', 'size', size);
  }

  if (err) {
    Error.captureStackTrace(err, assertSize);
    throw err;
  }
}
```

As you can see, the `Buffer` module actually performs checks against the `kMaxLength` value for all Buffer size allocations _before_ it actually creates any buffers or does any processing.  Therefore, as previously mentioned, it is impossible to throw an `ERR_BUFFER_TOO_LARGE` error directly from using the `Buffer` module.  However, it should be noted that some other Node modules _may_ be capable of producing `ERR_BUFFER_TOO_LARGE` errors under certain circumstances.  At present, the only library that can throw such errors in [`zlib`](https://github.com/nodejs/node/blob/master/lib/zlib.js#L496).

Airbrake's robust <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-nodejs-error-handling">error monitoring software</a> provides real-time error monitoring and automatic error reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize error parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-nodejs-error-handling">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A look at the ERR_BUFFER_TOO_LARGE RangeError in Node.js, with sample code showing the similarities between instantiating and allocating Buffers.

---

__SOURCES__

- https://nodejs.org/api/errors.html
- https://github.com/nodejs/