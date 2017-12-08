# Node.js Error Handling - RangeError

Continuing along through our in-depth [**Node.js Error Handling**](https://airbrake.io/blog/nodejs-error-handling/nodejs-error-class-hierarchy) series, today we'll be going over the **RangeError**.  As with many other programming languages, including Ruby with the [RangeError](https://airbrake.io/blog/ruby-exception-handling/rangeerror) article we covered in that language, the Node.js `RangeError` is thrown to indicate that a passed function argument does not fall within the valid set or range of acceptable values.  This might be because the value simply falls outside  a given numeric range, or because there is a specific set of allowed values and the passed argument is not one of them.

Throughout this article we'll examine the `RangeError` in more detail by first looking at where it resides in the larger [Node.js Error Class Hierarchy](https://airbrake.io/blog/nodejs-error-handling/nodejs-error-class-hierarchy).  We'll also look at some functional sample code illustrating how many of the built-in Node.js modules purposefully throw `RangeErrors` when invalid a arguments are passed, and how you might deal with them in your own code.  Let's get this party train rollin'!

## The Technical Rundown

Most Node.js errors inherit from the [`Error`](https://nodejs.org/api/errors.html#errors_class_error) base class, or extend from an inherited class therein.  The full error hierarchy of this error is:

- [`Error`](https://nodejs.org/api/errors.html#errors_class_error)
    - `RangeError`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```js
const net = require('net');
const logging = require('gw-logging');

function executeTests () {
  try {
    // Create server via socket.
    const server = net.createServer((socket) => {
      socket.end('Socket disconnected.\n');
    }).on('error', (e) => {
      throw e;
    });

    // Open server at port 24601.
    logging.lineSeparator(`SERVER OPENED AT PORT: ${24601}`, 50);
    server.listen(24601);

    // Perform series of client connections to different ports.
    logging.lineSeparator('CONNECT TO PORT: 24601', 50);
    connectToPort(24601);

    logging.lineSeparator('CONNECT TO PORT: 31234', 50);
    connectToPort(31234);

    logging.lineSeparator('CONNECT TO PORT: 1000000', 50);
    connectToPort(1000000);
  } catch (e) {
    if (e instanceof RangeError) {
      // Output expected RangeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

function connectToPort (port) {
  try {
    // Create connection at passed port.
    let client = net.createConnection({ port: port }, () => {
      // Log and output from client.
      logging.log(`Connected to server at port: ${port}`);
      client.write('Hello world!\r\n');
    });

    // Log data, if applicable.
    client.on('data', (data) => {
      logging.log(data.toString());
      client.end();
    });

    // When connection ends, log disconnection from server.
    client.on('end', () => {
      logging.log('Disconnected from server.');
    });
  } catch (e) {
    if (e instanceof RangeError) {
      // Output expected RangeErrors.
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
// gw-logging module - app.js
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
    if (typeof(value) === 'string') {
      logValue(value);
    }
    if (value instanceof Error) {
      logError(value, arguments[1]);
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
 */
function logError(error, explicit = true) {
  console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

/**
 * Logs a value (string, object, number, etc).
 *
 * @param value Value to be logged.
 */
function logValue(value) {
  console.log(value);
}
```

## When Should You Use It?

As outlines in the introduction, a `RangeError` is intended to indicate when a passed function argument falls outside the bounds of allowed values, whether numeric or not within a set.  To illustrate this in our code example we're performing a simple client/server connection test using the [`net`](https://nodejs.org/api/net.html) module, which provides an API for asynchronous networking functionality.  Thus, our `app.js` begins by requiring the `net` module, along with our custom `gw-logging` module seen above, to help us simplify API calls for logging exceptions and other information to the console:

```js
const net = require('net');
const logging = require('gw-logging');
```

The `connectToPort(port)` function performs most of the logic for us, by attempting to create a connection via the `net.createConnection()` method.  This client instance can then be used to attempt to connect to the passed `port` parameter, while we catch any unexpected `Errors`:

```js
function connectToPort (port) {
  try {
    // Create connection at passed port.
    let client = net.createConnection({ port: port }, () => {
      // Log and output from client.
      logging.log(`Connected to server at port: ${port}`);
      client.write('Hello world!\r\n');
    });

    // Log data, if applicable.
    client.on('data', (data) => {
      logging.log(data.toString());
      client.end();
    });

    // When connection ends, log disconnection from server.
    client.on('end', () => {
      logging.log('Disconnected from server.');
    });
  } catch (e) {
    if (e instanceof RangeError) {
      // Output expected RangeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}
```

Our main test function, `executeTests()`, consists of first establishing a basic server via the `net.createServer()` method.  We then open up a single port (`24601`) to listen for incoming client connections:

```js
function executeTests () {
  try {
    // Create server via socket.
    const server = net.createServer((socket) => {
      socket.end('Socket disconnected.\n');
    }).on('error', (e) => {
      throw e;
    });

    // Open server at port 24601.
    logging.lineSeparator(`SERVER OPENED AT PORT: ${24601}`, 50);
    server.listen(24601);

    // ...
  } catch (e) {
    if (e instanceof RangeError) {
      // Output expected RangeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}
```

With the server created and listening at port `24601` we can start a series of test client connections at various ports via the `connectToPort(port)` function.  First, let's try the expected port of `24601`:

```js
// Perform series of client connections to different ports.
logging.lineSeparator('CONNECT TO PORT: 24601', 50);
connectToPort(24601);
```

Executing this code produces the following output:

```
--------- SERVER OPENED AT PORT: 24601 ---------
------------ CONNECT TO PORT: 24601 ------------
Connected to server at port: 24601
Socket disconnected.
Disconnected from server.
```

Everything works as expected.  Our server was created, we opened port `24601` on it to listen for incoming connections, then our client attempted and successfully connected to that same port, before the client disconnected and the socket was destroyed.

Now, let's try connecting to a different port (`31234`), which the server _isn't_ actively listening on:

```js
logging.lineSeparator('CONNECT TO PORT: 31234', 50);
connectToPort(31234);
```

This produces an uncaught error, indicating that the server refused the connection at that port:

```
------------ CONNECT TO PORT: 31234 ------------
Uncaught Error: connect ECONNREFUSED 127.0.0.1:31234
  _errnoException	
  _exceptionWithHostPort	
  afterConnect
```

Finally, let's connect to port number `1,000,000`, which is well above the expected number of [maximum ports for TCP/UDP](https://en.wikipedia.org/wiki/Port_(computer_networking)) connections (`65535`):

```js
logging.lineSeparator('CONNECT TO PORT: 1000000', 50);
connectToPort(1000000);
```

Executing this code doesn't even _attempt_ to connect to port `1000000` and give us a connection refusal message like before, but instead throws a `RangeError` instead, indicating that the port is well outside the bounds of allowed port range:

```
----------- CONNECT TO PORT: 1000000 -----------
[EXPLICIT] RangeError: "port" option should be >= 0 and < 65536: 1000000
```

Airbrake's robust <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-nodejs-error-handling">error monitoring software</a> provides real-time error monitoring and automatic error reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize error parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-nodejs-error-handling">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look at the RangeError in Node.js, with sample code showing how to create a client/server connection, where invalid ports cause RangeErrors.

---

__SOURCES__

- https://nodejs.org/api/errors.html