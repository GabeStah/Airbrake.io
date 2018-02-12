---
categories: 
  - NodeJS Error Handling
date: 2018-02-12
description: "A deep dive into the ERR_CONSOLE_WRITABLE_STREAM TypeError in Node.js, with sample code illustrating how to use the Console class API."
published: true
sources:
  - https://nodejs.org/api/errors.html
  - https://github.com/nodejs/
title: "Node.js Error Handling - ERR_CONSOLE_WRITABLE_STREAM"  
---

Making our way through our in-depth [**Node.js Error Handling**](https://airbrake.io/blog/nodejs-error-handling/nodejs-error-class-hierarchy) series, today we'll be diving into the **ERR_CONSOLE_WRITABLE_STREAM** error, which is one of the many `System Errors` Node produces.  Node throws `System Errors` when an exception occurs within the application's runtime environment and such errors typically indicate that there was an operational problem within the program.  An `ERR_CONSOLE_WRITABLE_STREAM` error indicates that an attempt was made to create a new [`Console`](https://nodejs.org/api/console.html) API class without passing a valid `stdout` stream into which to place any created output.

Throughout this article we'll explore the `ERR_CONSOLE_WRITABLE_STREAM` error in a bit more detail, starting with where it resides in the overall [Node.js Error Class Hierarchy](https://airbrake.io/blog/nodejs-error-handling/nodejs-error-class-hierarchy).  We'll also look at some functional code samples illustrating how the [`Console`](https://nodejs.org/api/console.html) class can be used to generate stream outputs, and how improper use of this class can potentially lead to `ERR_CONSOLE_WRITABLE_STREAM` errors in your own applications.  Let's get going!

## The Technical Rundown

Most Node.js errors inherit from the [`Error`](https://nodejs.org/api/errors.html#errors_class_error) base class, or extend from an inherited class therein.  The full error hierarchy of this error is:

- [`Error`](https://nodejs.org/api/errors.html#errors_class_error)
    - [`SystemError`](https://nodejs.org/dist/latest-v8.x/docs/api/errors.html#errors_system_errors)
      - `ERR_CONSOLE_WRITABLE_STREAM`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```js
/**
 * index.js
 */
const Book = require('book');
const { Console } = require('console');
const fs = require('fs');
const logging = require('logging');

function executeTests () {
  // Create new Book instance.
  let bookA = new Book('The Name of the Wind', 'Patrick Rothfuss', 662, new Date(2007, 2, 27));
  logging.lineSeparator('BOOK A', 60);
  logging.log(bookA.toString());

  // Output Book A to stream.
  outputBookToStream(bookA, fs.createWriteStream('books.json'));

  let bookB = new Book('The Wise Man\'s Fear', 'Patrick Rothfuss', 994, new Date(2011, 2, 1));
  logging.lineSeparator('BOOK B', 60);
  logging.log(bookB.toString());

  // Output Book B to stream.
  outputBookToStream(bookB, fs.createWriteStream('books.json'));

  // Add both Books to new stream in order to properly format JSON.
  logging.lineSeparator('ADDING BOOKS A & B SIMULTANEOUSLY', 60);
  addValueToStream(JSON.stringify([
    bookA,
    bookB
  ]), fs.createWriteStream('books.json'));

  // Create Book C instance.
  let bookC = new Book('Doors of Stone', 'Patrick Rothfuss', 0);
  logging.lineSeparator('BOOK C', 60);
  logging.log(bookC.toString());

  // Output Book C to null stream.
  outputBookToStream(bookC);
}

/**
 * Adds passed value to passed writeStream by creating new Console instance.
 *
 * @param value Value to be added.
 * @param writeStream (Optional) Write stream to add value to.
 * @param errorStream (Optional) Error stream to output errors to.
 */
function addValueToStream (value, writeStream = null, errorStream = null) {
  try {
    // Get console instance using passed streams.
    let streamConsole = getConsole(writeStream, errorStream);
    if (!streamConsole) return;

    // Log passed value to stream.
    streamConsole.log(value);

    // Confirm value addition.
    logging.log(`Successfully added ${value} to stream.`);
  } catch (e) {
    if (e instanceof TypeError && e.code === 'ERR_CONSOLE_WRITABLE_STREAM') {
      // Output expected ERR_CONSOLE_WRITABLE_STREAM TypeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

/**
 * Instantiates a new Console using passed Writables.
 *
 * @param {Writable} stdout (Optional) Writable to be written to.
 * @param {Writable} stderr (Optional) Writable for error to be written to.
 * @returns {Console.Console} Console instance.
 */
function getConsole (stdout = null, stderr = null) {
  try {
    if (!stderr) {
      stderr = stdout;
    }

    return new Console(stdout, stderr);
  } catch (e) {
    if (e instanceof TypeError && e.code === 'ERR_CONSOLE_WRITABLE_STREAM') {
      // Output expected ERR_CONSOLE_WRITABLE_STREAM TypeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

/**
 * Converts passed Book to JSON and outputs it to passed stream via Console.
 *
 * @param book Book to be added.
 * @param stream Stream into which Book should be output.
 */
function outputBookToStream (book, stream = null) {
  // Convert book to JSON.
  let json = JSON.stringify(book);
  logging.lineSeparator('BOOK TO JSON', 60);
  logging.log(json);

  // Output JSON to stream.
  addValueToStream(json, stream);
}

executeTests();
```

```js
// Book module - Book.js
let Enum = require('enum');

/**
 * Publication types enumeration.
 *
 * @type {*|Enum} Publication types.
 */
let PublicationType = new Enum(['Digital', 'Paperback', 'Hardcover']);

/**
 * Constructs a basic book, with page count, publication date, and publication type.
 *
 * @param title Book title.
 * @param author Book author.
 * @param pageCount Book page count.
 * @param publicationDate Book publication date.
 * @param publicationType Book publication type.
 * @constructor
 */
function Book (title, author, pageCount, publicationDate = null, publicationType = PublicationType.Digital) {
  this.setAuthor(author);
  this.setPageCount(pageCount);
  this.setPublicationDate(publicationDate);
  this.setPublicationType(publicationType);
  this.setTitle(title);
}

/**
 * Get author of book.
 *
 * @returns {*} Author name.
 */
Book.prototype.getAuthor = function () {
  return this.author;
};

/**
 * Get page count of book.
 *
 * @returns {*} Page count.
 */
Book.prototype.getPageCount = function () {
  return this.pageCount;
};

/**
 * Get publication date of book.
 *
 * @returns {*} Publication date.
 */
Book.prototype.getPublicationDate = function () {
  return this.publicationDate;
};

/**
 * Get publication type of book.
 *
 * @returns {*} Publication type.
 */
Book.prototype.getPublicationType = function () {
  return this.publicationType;
};

/**
 * Get a formatted tagline with author, title, page count, publication date, and publication type.
 *
 * @returns {string} Formatted tagline.
 */
Book.prototype.getTagline = function() {
  return `'${this.getTitle()}' by ${this.getAuthor()} is ${this.getPageCount()} pages, published ${this.getPublicationDate()} as ${this.getPublicationType().key} type.`
};

/**
 * Get title of book.
 *
 * @returns {*} Book title.
 */
Book.prototype.getTitle = function () {
  return this.title;
};

/**
 * Set author of book.
 *
 * @param value Author.
 */
Book.prototype.setAuthor = function (value) {
  if (typeof value !== 'string') {
    throw new TypeError(`'Author' value of (${value}) must be a string, not ${typeof value}.`);
  }
  this.author = value;
};

/**
 * Set page count of book.
 *
 * @param value Page count.
 */
Book.prototype.setPageCount = function (value) {
  if (typeof value !== 'number') {
    throw new TypeError(`'PageCount' value of (${value}) must be a number, not ${typeof value}.`);
  }
  this.pageCount = value;
};

/**
 * Set publication date of book.
 *
 * @param value Publication date.
 */
Book.prototype.setPublicationDate = function (value) {
  if (value && !(value instanceof Date)) {
    throw new TypeError(`'setPublicationDate' value of (${value}) must be an instance of Date, not ${typeof value}.`);
  }
  this.publicationDate = value;
};

/**
 * Set publication type of book.
 *
 * @param value Publication type.
 */
Book.prototype.setPublicationType = function (value) {
  this.publicationType = value;
};

/**
 * Set title of book.
 *
 * @param value Title.
 */
Book.prototype.setTitle = function (value) {
  if (typeof value !== 'string') {
    throw new TypeError(`'Title' value of (${value}) must be a string, not ${typeof value}.`);
  }
  this.title = value;
};

/**
 * Get string representation of book.
 *
 * @returns {string} String representation.
 */
Book.prototype.toString = function () {
  return this.getTagline();
};

/**
 * Exports Book class.
 *
 * @type {Book} Book constructor.
 */
module.exports = Book;
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

Let's jump right into our sample code to understand what might cause an unexpected `ERR_CONSOLE_WRITABLE_STREAM` error.  Our first helper function, `getConsole(strout, stderr)`, is used to merely create a new `Console` class instance by passing along the provided `stdout` and `stderr` parameters:

```js
/**
 * Instantiates a new Console using passed Writables.
 *
 * @param {Writable} stdout (Optional) Writable to be written to.
 * @param {Writable} stderr (Optional) Writable for error to be written to.
 * @returns {Console.Console} Console instance.
 */
function getConsole (stdout = null, stderr = null) {
  try {
    if (!stderr) {
      stderr = stdout;
    }

    return new Console(stdout, stderr);
  } catch (e) {
    if (e instanceof TypeError && e.code === 'ERR_CONSOLE_WRITABLE_STREAM') {
      // Output expected ERR_CONSOLE_WRITABLE_STREAM TypeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}
```

With `getConsole(...)` setup we then have the `addValueToStream(value, writeStream, errorStream)` function, which does what the name suggests and generates a new `Console` instance, into which we insert the passed `value` parameter:

```js
/**
 * Adds passed value to passed writeStream by creating new Console instance.
 *
 * @param value Value to be added.
 * @param writeStream (Optional) Write stream to add value to.
 * @param errorStream (Optional) Error stream to output errors to.
 */
function addValueToStream (value, writeStream = null, errorStream = null) {
  try {
    // Get console instance using passed streams.
    let streamConsole = getConsole(writeStream, errorStream);
    if (!streamConsole) return;

    // Log passed value to stream.
    streamConsole.log(value);

    // Confirm value addition.
    logging.log(`Successfully added ${value} to stream.`);
  } catch (e) {
    if (e instanceof TypeError && e.code === 'ERR_CONSOLE_WRITABLE_STREAM') {
      // Output expected ERR_CONSOLE_WRITABLE_STREAM TypeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}
```

To make our sample code a bit more realistic we're going to create some `Book` class instances, convert those to `JSON` strings, then add those strings to a new file via the `fs.createWriteStream(...)` method.  The `outputBookToStream(book, stream)` function performs the stringification of the passed `book` value and passes the `JSON` string along to the `addValueToStream(value, writeStream, errorStream)` function:

```js
/**
 * Converts passed Book to JSON and outputs it to passed stream via Console.
 *
 * @param book Book to be added.
 * @param stream Stream into which Book should be output.
 */
function outputBookToStream (book, stream = null) {
  // Convert book to JSON.
  let json = JSON.stringify(book);
  logging.lineSeparator('BOOK TO JSON', 60);
  logging.log(json);

  // Output JSON to stream.
  addValueToStream(json, stream);
}
```

Alright!  We're all set, so let's test this out by creating our first `Book` instance, which we'll then add to the newly-created write stream targeting the local `books.json` file:

```js
// Create new Book instance.
let bookA = new Book('The Name of the Wind', 'Patrick Rothfuss', 662, new Date(2007, 2, 27));
logging.lineSeparator('BOOK A', 60);
logging.log(bookA.toString());

// Output Book A to stream.
outputBookToStream(bookA, fs.createWriteStream('books.json'));
```

Executing this test code produces the following output:

```
------------------------- BOOK A -------------------------
'The Name of the Wind' by Patrick Rothfuss is 662 pages, published Tue Mar 27 2007 00:00:00 GMT-0700 (Pacific Daylight Time) as Digital type.
---------------------- BOOK TO JSON ----------------------
{"author":"Patrick Rothfuss","pageCount":662,"publicationDate":"2007-03-27T07:00:00.000Z","publicationType":"Digital","title":"The Name of the Wind"}
Successfully added {"author":"Patrick Rothfuss","pageCount":662,"publicationDate":"2007-03-27T07:00:00.000Z","publicationType":"Digital","title":"The Name of the Wind"} to stream.
```

And, sure enough, the contents of `books.json` now contains the `JSON` we generated:

```json
{
  "author": "Patrick Rothfuss",
  "pageCount": 662,
  "publicationDate": "2007-03-27T07:00:00.000Z",
  "publicationType": "Digital",
  "title": "The Name of the Wind"
}
```

We'll create a second, slightly different `Book` instance and combine both into an array before converting to `JSON`, since adding multiple objects one at a time to the `books.json` file doesn't actually create valid `JSON`:

```js
let bookB = new Book('The Wise Man\'s Fear', 'Patrick Rothfuss', 994, new Date(2011, 2, 1));
logging.lineSeparator('BOOK B', 60);
logging.log(bookB.toString());

// Output Book B to stream.
outputBookToStream(bookB, fs.createWriteStream('books.json'));

// Add both Books to new stream in order to properly format JSON.
logging.lineSeparator('ADDING BOOKS A & B SIMULTANEOUSLY', 60);
addValueToStream(JSON.stringify([
  bookA,
  bookB
]), fs.createWriteStream('books.json'));
```

Our new `books.json` file now contains a collection of both `Books`:

```json
[
  {
    "author": "Patrick Rothfuss",
    "pageCount": 662,
    "publicationDate": "2007-03-27T07:00:00.000Z",
    "publicationType": "Digital",
    "title": "The Name of the Wind"
  },
  {
    "author": "Patrick Rothfuss",
    "pageCount": 994,
    "publicationDate": "2011-03-01T08:00:00.000Z",
    "publicationType": "Digital",
    "title": "The Wise Man's Fear"
  }
]
```

As you can see, we're explicitly creating and passing a `writeStream` value by calling `fs.createWriteStream('books.json')`.  However, let's do one more test and see what happens if we neglect to pass a valid `Writable` stream object:

```js
// Create Book C instance.
let bookC = new Book('Doors of Stone', 'Patrick Rothfuss', 0);
logging.lineSeparator('BOOK C', 60);
logging.log(bookC.toString());

// Output Book C to null stream.
outputBookToStream(bookC);
```

As I'm sure you guessed might happen, failing to pass a valid stream up the stack (and, therefore, to the `Console` constructor) throws an `ERR_CONSOLE_WRITABLE_STREAM` `TypeError`:

```
------------------------- BOOK C -------------------------
'Doors of Stone' by Patrick Rothfuss is 0 pages, published null as Digital type.
---------------------- BOOK TO JSON ----------------------
{"author":"Patrick Rothfuss","pageCount":0,"publicationDate":null,"publicationType":"Digital","title":"Doors of Stone"}
[EXPLICIT] TypeError [ERR_CONSOLE_WRITABLE_STREAM]: Console expects a writable stream instance for stdout
    at new Console (console.js:38:11)
    at getConsole (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_CONSOLE_WRITABLE_STREAM\index.js:83:12)
    at addValueToStream (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_CONSOLE_WRITABLE_STREAM\index.js:51:25)
    at outputBookToStream (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_CONSOLE_WRITABLE_STREAM\index.js:108:3)
    at executeTests (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_CONSOLE_WRITABLE_STREAM\index.js:38:3)
    at Object.<anonymous> (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_CONSOLE_WRITABLE_STREAM\index.js:111:1)
    at Module._compile (module.js:657:14)
    at Object.Module._extensions..js (module.js:671:10)
    at Module.load (module.js:573:32)
    at tryModuleLoad (module.js:513:12)
```

Airbrake's robust <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-nodejs-error-handling">error monitoring software</a> provides real-time error monitoring and automatic error reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize error parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-nodejs-error-handling">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!