---
categories: 
  - NodeJS Error Handling
date: 2018-02-12
description: "A close look at the ERR_ENCODING_INVALID_ENCODED_DATA TypeError in Node.js, with sample code showing how to perform basic encoding/decoding."
published: true
sources:
  - https://nodejs.org/api/errors.html
  - https://github.com/nodejs/
title: "Node.js Error Handling - ERR_ENCODING_INVALID_ENCODED_DATA"  
---

Moving along through our detailed [**Node.js Error Handling**](https://airbrake.io/blog/nodejs-error-handling/nodejs-error-class-hierarchy) series, today we'll be going over the **ERR_ENCODING_INVALID_ENCODED_DATA** error, which is one of the many `System Errors` Node can generate.  Node throws `System Errors` when an exception occurs within the application's runtime environment, and such errors typically indicate that there was an operational problem within the program.  An `ERR_ENCODING_INVALID_ENCODED_DATA` error indicates that an invocation of the [`TextDecoder`](https://github.com/nodejs/node/blob/master/lib/internal/encoding.js#L355) failed because of incompatibility between the specified encoding and the decoding target.

In the rest of this article we'll examine the `ERR_ENCODING_INVALID_ENCODED_DATA` error by looking at where it sits in the larger [Node.js Error Class Hierarchy](https://airbrake.io/blog/nodejs-error-handling/nodejs-error-class-hierarchy).  We'll also explore some fully functional code samples that illustrate how the `TextDecoder` class can be used to handle `Buffer` decoding, and how incompatibilities can occasionally lead to `ERR_ENCODING_INVALID_ENCODED_DATA` errors.  Let's do this!

## The Technical Rundown

Most Node.js errors inherit from the [`Error`](https://nodejs.org/api/errors.html#errors_class_error) base class, or extend from an inherited class therein.  The full error hierarchy of this error is:

- [`Error`](https://nodejs.org/api/errors.html#errors_class_error)
    - [`SystemError`](https://nodejs.org/dist/latest-v8.x/docs/api/errors.html#errors_system_errors)
      - `ERR_ENCODING_INVALID_ENCODED_DATA`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```js
/**
 * index.js
 */
const logging = require('logging');
const { TextDecoder } = require('util');

function executeTests () {
  logging.lineSeparator(`DECODE: Airbrake.io`, 100, '=');
  decodeBuffer(Buffer.from('Airbrake.io'));

  logging.lineSeparator(`DECODE: Âirbrake.io`, 100, '=');
  decodeBuffer(Buffer.from('Âirbrake.io'));

  logging.lineSeparator(`DECODE: [0xc3, 0x82, 0x69, 0x72, 0x62, 0x72, 0x61, 0x6b, 0x65, 0x2e, 0x69, 0x6f]`, 100, '=');
  decodeBuffer(Buffer.from([0xc3, 0x82, 0x69, 0x72, 0x62, 0x72, 0x61, 0x6b, 0x65, 0x2e, 0x69, 0x6f]));

  logging.lineSeparator(`NON-FATALLY DECODE: Âirbrake.io`, 100, '=');
  decodeBuffer(Buffer.from('Âirbrake.io'), 'utf-8', false);
}

/**
 * Decodes passed Buffer via TextDecoder, using passed encoding and fatal option.
 * Iterates and outputs each byte segment of full buffer.
 *
 * @param buffer Buffer to be decoded.
 * @param encoding Encoding to use.
 * @param fatal Determines if decoder will throw errors if an issue occurs.
 */
function decodeBuffer (buffer, encoding='utf-8', fatal=true) {
  try {
    // Create text decoder with proper encoding and fatal option.
    const decoder = new TextDecoder(encoding, { fatal: fatal });

    // Iterate through buffer, slicing from start to each index, then outputting and decoding.
    for (let i = buffer.length; i >= 1; i--) {
      let slice = buffer.slice(0, i);

      logging.lineSeparator(`slice(0, ${i})`, 23, '_');
      logging.log(slice);
      logging.log(`Decoded slice: ${decoder.decode(slice)}`);
    }
  } catch (e) {
    if (e instanceof TypeError && e.code === 'ERR_ENCODING_INVALID_ENCODED_DATA') {
      // Output expected ERR_ENCODING_INVALID_ENCODED_DATA TypeErrors.
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

Since the `ERR_ENCODING_INVALID_ENCODED_DATA` error indicates a problem with the `TextDecoder` module we'll start our code sample by creating a simple wrapper function designed to decode a `Buffer` object using `TextDecoder`:

```js
/**
 * Decodes passed Buffer via TextDecoder, using passed encoding and fatal option.
 * Iterates and outputs each byte segment of full buffer.
 *
 * @param buffer Buffer to be decoded.
 * @param encoding Encoding to use.
 * @param fatal Determines if decoder will throw errors if an issue occurs.
 */
function decodeBuffer (buffer, encoding='utf-8', fatal=true) {
  try {
    // Create text decoder with proper encoding and fatal option.
    const decoder = new TextDecoder(encoding, { fatal: fatal });

    // Iterate through buffer, slicing from start to each index, then outputting and decoding.
    for (let i = buffer.length; i >= 1; i--) {
      let slice = buffer.slice(0, i);

      logging.lineSeparator(`slice(0, ${i})`, 23, '_');
      logging.log(slice);
      logging.log(`Decoded slice: ${decoder.decode(slice)}`);
    }
  } catch (e) {
    if (e instanceof TypeError && e.code === 'ERR_ENCODING_INVALID_ENCODED_DATA') {
      // Output expected ERR_ENCODING_INVALID_ENCODED_DATA TypeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}
```

This function instantiates a `TextDecoder` object using the passed `encoding` and `fatal` parameters, then iterates through and outputs each chunk of the passed `buffer` before passing it to our `decoder` instance.  This iteration will be important in a moment, but for now it just allows us to see the full byte composition each individual slice of our buffer, and how each of those corresponds to the string characters in the buffer.

We'll start our tests by creating a `Buffer` from a string of `'Airbrake.io'`:

```js
logging.lineSeparator(`DECODE: Airbrake.io`, 100, '=');
decodeBuffer(Buffer.from('Airbrake.io'));
```

This test produces the following output:

```
====================================== DECODE: Airbrake.io =======================================
___ slice(0, 11) ____
<Buffer 41 69 72 62 72 61 6b 65 2e 69 6f>
Decoded slice: Airbrake.io
___ slice(0, 10) ____
<Buffer 41 69 72 62 72 61 6b 65 2e 69>
Decoded slice: Airbrake.i
____ slice(0, 9) ____
<Buffer 41 69 72 62 72 61 6b 65 2e>
Decoded slice: Airbrake.
____ slice(0, 8) ____
<Buffer 41 69 72 62 72 61 6b 65>
Decoded slice: Airbrake
____ slice(0, 7) ____
<Buffer 41 69 72 62 72 61 6b>
Decoded slice: Airbrak
____ slice(0, 6) ____
<Buffer 41 69 72 62 72 61>
Decoded slice: Airbra
____ slice(0, 5) ____
<Buffer 41 69 72 62 72>
Decoded slice: Airbr
____ slice(0, 4) ____
<Buffer 41 69 72 62>
Decoded slice: Airb
____ slice(0, 3) ____
<Buffer 41 69 72>
Decoded slice: Air
____ slice(0, 2) ____
<Buffer 41 69>
Decoded slice: Ai
____ slice(0, 1) ____
<Buffer 41>
Decoded slice: A
```

Cool!  We can see each iteration outputs the `slice(...)` values it is using, along with the `Buffer` byte array of that slice, and finally the decoded string of that slice.  We're using `utf-8` encoding, so these basic characters aren't likely to give us much trouble.  However, let's make a slight alteration by changing the `A` in our string to a special `Â` character:

```js
logging.lineSeparator(`DECODE: Âirbrake.io`, 100, '=');
decodeBuffer(Buffer.from('Âirbrake.io'));
```

Everything else is the same as before, except that first character, so let's look at the test output:

```
====================================== DECODE: Âirbrake.io =======================================
___ slice(0, 12) ____
<Buffer c3 82 69 72 62 72 61 6b 65 2e 69 6f>
Decoded slice: Âirbrake.io
___ slice(0, 11) ____
<Buffer c3 82 69 72 62 72 61 6b 65 2e 69>
Decoded slice: Âirbrake.i
___ slice(0, 10) ____
<Buffer c3 82 69 72 62 72 61 6b 65 2e>
Decoded slice: Âirbrake.
____ slice(0, 9) ____
<Buffer c3 82 69 72 62 72 61 6b 65>
Decoded slice: Âirbrake
____ slice(0, 8) ____
<Buffer c3 82 69 72 62 72 61 6b>
Decoded slice: Âirbrak
____ slice(0, 7) ____
<Buffer c3 82 69 72 62 72 61>
Decoded slice: Âirbra
____ slice(0, 6) ____
<Buffer c3 82 69 72 62 72>
Decoded slice: Âirbr
____ slice(0, 5) ____
<Buffer c3 82 69 72 62>
Decoded slice: Âirb
____ slice(0, 4) ____
<Buffer c3 82 69 72>
Decoded slice: Âir
____ slice(0, 3) ____
<Buffer c3 82 69>
Decoded slice: Âi
____ slice(0, 2) ____
<Buffer c3 82>
Decoded slice: Â
____ slice(0, 1) ____
<Buffer c3>
[EXPLICIT] TypeError [ERR_ENCODING_INVALID_ENCODED_DATA]: The encoded data was not valid for encoding utf-8
    at TextDecoder.decode (internal/encoding.js:390:21)
    at decodeBuffer (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_ENCODING_INVALID_ENCODED_DATA\index.js:40:45)
    at executeTests (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_ENCODING_INVALID_ENCODED_DATA\index.js:12:3)
    at Object.<anonymous> (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_ENCODING_INVALID_ENCODED_DATA\index.js:53:1)
    at Module._compile (module.js:657:14)
    at Object.Module._extensions..js (module.js:671:10)
    at Module.load (module.js:573:32)
    at tryModuleLoad (module.js:513:12)
    at Function.Module._load (module.js:505:3)
    at Function.Module.runMain (module.js:701:10)
```

This time we caught an `ERR_ENCODING_INVALID_ENCODED_DATA` error that indicates the encoded data was invalid with the `utf-8` encoding we're using.  What's critical to note is _where_ in our iteration this error was thrown.  Everything was working just fine until we reached the `slice(0, 1)` byte, which we can see from the `Buffer` output is equal to the octal `0xc3`.  However, the _previous_ iteration where our `Buffer` slice contained two octals of `0xc3` and `0x82` worked just fine.  This is because the combination of those two bytes (i.e. the `hexadecimal` representation) is how the [unicode](http://www.fileformat.info/info/unicode/char/00c2/index.htm) of our special `Â` character is defined.  But, the singular `0xc3` byte/hex _is not_ representative of any unicode character in `utf-8`, which is why the final `Buffer` slice produces the `ERR_ENCODING_INVALID_ENCODED_DATA` error.

To further illustrate we can actually create a `Buffer` from an array of octals.  Here we're performing the exact same test as before, except we're representing the `Âirbrake.io` string in its octal/hexadecimal format:

```js
logging.lineSeparator(`DECODE: [0xc3, 0x82, 0x69, 0x72, 0x62, 0x72, 0x61, 0x6b, 0x65, 0x2e, 0x69, 0x6f]`, 100, '=');
decodeBuffer(Buffer.from([0xc3, 0x82, 0x69, 0x72, 0x62, 0x72, 0x61, 0x6b, 0x65, 0x2e, 0x69, 0x6f]));
```

Once again, the output is the same as our previous test, with an `ERR_ENCODING_INVALID_ENCODED_DATA` error while trying to decode the final (invalid) slice:

```
======== DECODE: [0xc3, 0x82, 0x69, 0x72, 0x62, 0x72, 0x61, 0x6b, 0x65, 0x2e, 0x69, 0x6f] ========
___ slice(0, 12) ____
<Buffer c3 82 69 72 62 72 61 6b 65 2e 69 6f>
Decoded slice: Âirbrake.io
___ slice(0, 11) ____
<Buffer c3 82 69 72 62 72 61 6b 65 2e 69>
Decoded slice: Âirbrake.i
___ slice(0, 10) ____
<Buffer c3 82 69 72 62 72 61 6b 65 2e>
Decoded slice: Âirbrake.
____ slice(0, 9) ____
<Buffer c3 82 69 72 62 72 61 6b 65>
Decoded slice: Âirbrake
____ slice(0, 8) ____
<Buffer c3 82 69 72 62 72 61 6b>
Decoded slice: Âirbrak
____ slice(0, 7) ____
<Buffer c3 82 69 72 62 72 61>
Decoded slice: Âirbra
____ slice(0, 6) ____
<Buffer c3 82 69 72 62 72>
Decoded slice: Âirbr
____ slice(0, 5) ____
<Buffer c3 82 69 72 62>
Decoded slice: Âirb
____ slice(0, 4) ____
<Buffer c3 82 69 72>
Decoded slice: Âir
____ slice(0, 3) ____
<Buffer c3 82 69>
Decoded slice: Âi
____ slice(0, 2) ____
<Buffer c3 82>
Decoded slice: Â
____ slice(0, 1) ____
<Buffer c3>
[EXPLICIT] TypeError [ERR_ENCODING_INVALID_ENCODED_DATA]: The encoded data was not valid for encoding utf-8
    at TextDecoder.decode (internal/encoding.js:390:21)
    at decodeBuffer (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_ENCODING_INVALID_ENCODED_DATA\index.js:40:45)
    at executeTests (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_ENCODING_INVALID_ENCODED_DATA\index.js:15:3)
    at Object.<anonymous> (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_ENCODING_INVALID_ENCODED_DATA\index.js:53:1)
    at Module._compile (module.js:657:14)
    at Object.Module._extensions..js (module.js:671:10)
    at Module.load (module.js:573:32)
    at tryModuleLoad (module.js:513:12)
    at Function.Module._load (module.js:505:3)
    at Function.Module.runMain (module.js:701:10)
```

Keen observers may have noticed our internal call to the `TextDecoder` constructor passed a special `{ fatal: fatal }` argument.  Our default value for this argument is `true`, but let's try one more test with our special character string, but where we set this `fatal` argument to `false`:

```js
logging.lineSeparator(`NON-FATALLY DECODE: Âirbrake.io`, 100, '=');
decodeBuffer(Buffer.from('Âirbrake.io'), 'utf-8', false);
```

We'll skip most of the iteration output this time since nothing has changed, but one major difference is immediately noticeable: There's no error thrown during the final slice decoding.

```
================================ NON-FATALLY DECODE: Âirbrake.io =================================
___ slice(0, 12) ____
<Buffer c3 82 69 72 62 72 61 6b 65 2e 69 6f>
Decoded slice: Âirbrake.io

...

____ slice(0, 2) ____
<Buffer c3 82>
Decoded slice: Â
____ slice(0, 1) ____
<Buffer c3>
Decoded slice: �
```

My particular console produces the `�` character when it's trying to output something that cannot be rendered, but we can see that no error was thrown _because_ the `fatal` option of the `TextDecoder` was set to `false`.  This is handy for when you want to decode something, but you don't care if the input is actually totally valid for the particular encoding you're using.

Airbrake's robust <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-nodejs-error-handling">error monitoring software</a> provides real-time error monitoring and automatic error reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize error parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-nodejs-error-handling">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!