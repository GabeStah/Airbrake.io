# Node.js Error Handling - TypeError

Moving along through our detailed [**Node.js Error Handling**](https://airbrake.io/blog/nodejs-error-handling/nodejs-error-class-hierarchy) series, today we'll be exploring the **TypeError**.  Within the Node framework a `TypeError` indicates that a passed argument is not of the appropriate type.  This method is used liberally throughout the built-in Node API modules, and should also be used within your own custom code to perform type checking at the top of of your functions and methods.

Throughout this article we'll explore Node's `TypeError` in more detail, starting with where it sits in the overall [Node.js Error Class Hierarchy](https://airbrake.io/blog/nodejs-error-handling/nodejs-error-class-hierarchy).  We'll also look over some simple, functional code samples that illustrate how both the built-in API modules, as well as custom modules, make use of the `TypeError` for type checking purposes.  Let's get to it!

## The Technical Rundown

Most Node.js errors inherit from the [`Error`](https://nodejs.org/api/errors.html#errors_class_error) base class, or extend from an inherited class therein.  The full error hierarchy of this error is:

- [`Error`](https://nodejs.org/api/errors.html#errors_class_error)
    - `TypeError`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```js
const Book = require('book');
const logging = require('logging');
const path = require('path');

function executeTests () {
  logging.lineSeparator("parsePath('/Error/TypeError/index.js')", 60);
  parsePath('/Error/TypeError/index.js');

  logging.lineSeparator("parsePath(12345)", 60);
  parsePath(12345);

  logging.lineSeparator("createValidBook()", 60);
  createValidBook();

  logging.lineSeparator("createInvalidBook()", 60);
  createInvalidBook();
}

function parsePath (value) {
  try {
    logging.log(path.parse(value));
  } catch (e) {
    if (e instanceof TypeError) {
      // Output expected TypeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

function createValidBook () {
  try {
    // Create a valid Book instance.
    let book = new Book('The Name of the Wind', 'Patrick Rothfuss', 662, new Date(2007, 2, 27));
    // Output Book to log.
    logging.log(book);
  } catch (e) {
    if (e instanceof TypeError) {
      // Output expected TypeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

function createInvalidBook () {
  try {
    // Create a valid Book instance.
    let book = new Book("The Wise Man's Fear", new String('Patrick Rothfuss'), 994, new Date(2011, 2, 1));
    // Output Book to log.
    logging.log(book);
  } catch (e) {
    if (e instanceof TypeError) {
      // Output expected TypeErrors.
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
 * @param publishedAt Book publication date.
 * @param publicationType Book publication type.
 * @constructor
 */
function Book (title, author, pageCount, publishedAt = null, publicationType = PublicationType.Digital) {
  this.setAuthor(author);
  this.setPageCount(pageCount);
  this.setPublicationType(publicationType);
  this.setPublishedAt(publishedAt);
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
 * Get publication type of book.
 *
 * @returns {*} Publication type.
 */
Book.prototype.getPublicationType = function () {
  return this.publicationType;
};

/**
 * Get publication date of book.
 *
 * @returns {*} Publication date.
 */
Book.prototype.getPublishedAt = function () {
  return this.publishedAt;
};

/**
 * Get a formatted tagline with author, title, page count, publication date, and publication type.
 *
 * @returns {string} Formatted tagline.
 */
Book.prototype.getTagline = function() {
  return `'${this.getTitle()}' by ${this.getAuthor()} is ${this.getPageCount()} pages, published ${this.getPublishedAt()} as ${this.getPublicationType().key} type.`
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
 * Set publication type of book.
 *
 * @param value Publication type.
 */
Book.prototype.setPublicationType = function (value) {
  this.publicationType = value;
};

/**
 * Set publication date of book.
 *
 * @param value Publication date.
 */
Book.prototype.setPublishedAt = function (value) {
  if (!(value instanceof Date)) {
    throw new TypeError(`'PublishedAt' value of (${value}) must be an instance of Date, not ${typeof value}.`);
  }
  this.publishedAt = value;
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
// logging module - app.js
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
  // Output stack, without initial error message line to avoid duplication.
  console.log(error.stack.slice(error.stack.indexOf("\n") + 1));
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

As previously mentioned, the `TypeError` should be used to indicate that an argument is not the proper type for what the function/method expects.  For example, if a `string` value is expected, the top of the function code block will perform some type check to ensure the relevant parameter is a `string`.  If not, a `new TypeError(...)` instance should be thrown.

Many of the built-in Node API modules use `TypeErrors`.  For example, here's a snippet of the [`path.js` core module](https://nodejs.org/api/path.html):

```js
function assertPath(path) {
  if (typeof path !== 'string') {
    throw new TypeError('Path must be a string. Received ' + inspect(path));
  }
}
```

The `assertPath(path)` function is called at the top of nearly every API method provided by the `path` module, as a simple check that the passed `path` argument is actually a string.  In our example code we'll use the `path` module's `parse(path)` method to try parsing a string path and outputting the result to the console:

```js
function parsePath (value) {
  try {
    logging.log(path.parse(value));
  } catch (e) {
    if (e instanceof TypeError) {
      // Output expected TypeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}
```

To test this out we'll pass two values, starting with a normal looking unix-style path string of `'/Error/TypeError/index.js'`, followed by a simple number of `12345`:

```js
logging.lineSeparator("parsePath('/Error/TypeError/index.js')", 60);
parsePath('/Error/TypeError/index.js');

logging.lineSeparator("parsePath(12345)", 60);
parsePath(12345);
```

Executing these test calls produces the following output:

```
--------- parsePath('/Error/TypeError/index.js') ---------
{ root: '/',
  dir: '/Error/TypeError',
  base: 'index.js',
  ext: '.js',
  name: 'index' }
-------------------- parsePath(12345) --------------------
[EXPLICIT] TypeError: Path must be a string. Received 12345
    at assertPath (path.js:28:11)
    at Object.parse (path.js:999:5)
    at parsePath (D:\work\Airbrake.io\Exceptions\NodeJS\Error\TypeError\index.js:21:22)
    at executeTests (D:\work\Airbrake.io\Exceptions\NodeJS\Error\TypeError\index.js:10:3)
    at Object.<anonymous> (D:\work\Airbrake.io\Exceptions\NodeJS\Error\TypeError\index.js:67:1)
    at Module._compile (module.js:632:14)
    at Object.Module._extensions..js (module.js:646:10)
    at Module.load (module.js:554:32)
    at tryModuleLoad (module.js:497:12)
    at Function.Module._load (module.js:489:3)
```

The normal string argument works as expected and outputs the parsed path object.  Meanwhile, the numeric value of `12345` throws a new `TypeError` indicating that the passed path parameter must be a string.  Moreover, we can see from the detailed call stack that we invoked the `parse(path)` method in the `path` module, which itself invoked the `assertPath(path)` function we looked at above, which is what actually threw the `TypeError` during execution.

Similar practices should be used within your own custom code, as well.  Here we've modified our `Book` class to perform some simple type checking for our `setXYZ(...)` methods.  This will help ensure that the user cannot set improper value types for properties such as the `title`, `author`, or `pageCount`.  

```js
// ...

/**
 * Constructs a basic book, with page count, publication date, and publication type.
 *
 * @param title Book title.
 * @param author Book author.
 * @param pageCount Book page count.
 * @param publishedAt Book publication date.
 * @param publicationType Book publication type.
 * @constructor
 */
function Book (title, author, pageCount, publishedAt = null, publicationType = PublicationType.Digital) {
  this.setAuthor(author);
  this.setPageCount(pageCount);
  this.setPublicationType(publicationType);
  this.setPublishedAt(publishedAt);
  this.setTitle(title);
}

// ...

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
Book.prototype.setPublishedAt = function (value) {
  if (!(value instanceof Date)) {
    throw new TypeError(`'PublishedAt' value of (${value}) must be an instance of Date, not ${typeof value}.`);
  }
  this.publishedAt = value;
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

// ...
```

It's worth noting that you'll need to perform the proper type checking, depending on what kind of value you're interested in.  For example, if we just need to know if an object is a primitive type (like a `string`, `number`, or `object`) we can use the [`typeof` operator](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Operators/typeof), as seen in `Book.prototype.setAuthor` and others.  However, the `Book.publishedAt` property is meant to be not only an `object`, but an explicit _type_ of object -- a `Date`, in this case.  Therefore, we have to use the [`instanceof` operator](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Operators/instanceof) to perform more complex instance type checking.

As before, we'll be performing two different invocations to test this, starting with a valid set of arguments passed to the `Book` constructor:

```js
function createValidBook () {
  try {
    // Create a valid Book instance.
    let book = new Book('The Name of the Wind', 'Patrick Rothfuss', 662, new Date(2007, 2, 27));
    // Output Book to log.
    logging.log(book);
  } catch (e) {
    if (e instanceof TypeError) {
      // Output expected TypeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}
```

Executing this function creates a valid `Book` instance and outputs the object to the console:

```
------------------- createValidBook() --------------------
Book {
  author: 'Patrick Rothfuss',
  pageCount: 662,
  publicationType: { [Number: 1] key: 'Digital', value: 1, _options: { ignoreCase: false } },
  publishedAt: 2007-03-27T07:00:00.000Z,
  title: 'The Name of the Wind' }
```

However, for our second `Book` we're slightly modifying the `author` argument by explicitly calling a `new String(...)` constructor, which is part of the ECMAScript API:

```js
function createInvalidBook () {
  try {
    // Create a valid Book instance.
    let book = new Book("The Wise Man's Fear", new String('Patrick Rothfuss'), 994, new Date(2011, 2, 1));
    // Output Book to log.
    logging.log(book);
  } catch (e) {
    if (e instanceof TypeError) {
      // Output expected TypeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}
```

Executing this code throws a `TypeError` from the `Book.prototype.setAuthor(value)` method:

```
------------------ createInvalidBook() -------------------
[EXPLICIT] TypeError: 'Author' value of (Patrick Rothfuss) must be a string, not object.
    at Book.setAuthor (D:\work\Airbrake.io\lib\node\book\book.js:90:11)
    at new Book (D:\work\Airbrake.io\lib\node\book\book.js:22:8)
    at createInvalidBook (D:\work\Airbrake.io\Exceptions\NodeJS\Error\TypeError\index.js:53:16)
    at executeTests (D:\work\Airbrake.io\Exceptions\NodeJS\Error\TypeError\index.js:16:3)
    at Object.<anonymous> (D:\work\Airbrake.io\Exceptions\NodeJS\Error\TypeError\index.js:67:1)
    at Module._compile (module.js:632:14)
    at Object.Module._extensions..js (module.js:646:10)
    at Module.load (module.js:554:32)
    at tryModuleLoad (module.js:497:12)
    at Function.Module._load (module.js:489:3)
```

This is a subtle change, but it illustrates an important consideration that must be made when coding your own type checks.  The `Book.prototype.setAuthor(value)` method _only_ expects that the passed argument be of type `string`, as checked by the `typeof` operator.  Therefore, the new `String` object that we passed in the `createInvalidBook()` function fails this check and throws a `TypeError`.  In reality, we'd probably want to perform some more robust type checking and maybe conversion, to be able to get the primitive string type from a passed `String` object. 

Airbrake's robust <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-nodejs-error-handling">error monitoring software</a> provides real-time error monitoring and automatic error reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize error parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-nodejs-error-handling">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A deep dive into the TypeError in Node.js, with sample code showing how both built-in Node API modules, along with custom modules, use TypeErrors.

---

__SOURCES__

- https://nodejs.org/api/errors.html