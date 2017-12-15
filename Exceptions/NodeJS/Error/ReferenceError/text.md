# Node.js Error Handling - ReferenceError

Moving along through our detailed [**Node.js Error Handling**](https://airbrake.io/blog/nodejs-error-handling/nodejs-error-class-hierarchy) series, in today's article we'll be checking out the **ReferenceError** in all its glory.  If you're familiar with plain JavaScript `ReferenceErrors`, the `ReferenceError` class in Node will look quite familiar, since it performs the same role: When a reference is made to an object that hasn't been defined in previously executed statements, a `ReferenceError` is thrown.

Within this article we'll explore the `ReferenceError` in greater detail, starting with where it sits in the overall [Node.js Error Class Hierarchy](https://airbrake.io/blog/nodejs-error-handling/nodejs-error-class-hierarchy).  We'll also look at some functional code samples to illustrate a common scenario in which a `ReferenceError` might be thrown in your own code, so let's get to it!

## The Technical Rundown

Most Node.js errors inherit from the [`Error`](https://nodejs.org/api/errors.html#errors_class_error) base class, or extend from an inherited class therein.  The full error hierarchy of this error is:

- [`Error`](https://nodejs.org/api/errors.html#errors_class_error)
    - `ReferenceError`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```js
const logging = require('logging');
const Book = require('book');

function executeTests () {
  logging.lineSeparator('EXECUTING PASSING TEST');
  passingTest();

  logging.lineSeparator('EXECUTING FAILING TEST');
  failingTest();
}

function passingTest () {
  try {
    // Create a new Book instance.
    let book = new Book('The Stand', 'Stephen King', 1153, new Date(1978, 0, 1));
    // Output valid Book instance, 'book'.
    logging.log(book);
  } catch (e) {
    if (e instanceof ReferenceError) {
      // Output expected ReferenceErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

function failingTest () {
  try {
    // Create a new Book instance.
    let book = new Book('The Stand', 'Stephen King', 1153, new Date(1978, 0, 1));
    // Output invalid Book instance, 'boo'.
    logging.log(boo);
  } catch (e) {
    if (e instanceof ReferenceError) {
      // Output expected ReferenceErrors.
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
  return `'${this.getTitle()}' by ${this.getAuthor()} is ${this.getPageCount()} pages, published ${this.getPublishedAt()} as ${this.getPublicationType()} type.`
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
  this.author = value;
};

/**
 * Set page count of book.
 *
 * @param value Page count.
 */
Book.prototype.setPageCount = function (value) {
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
  this.publishedAt = value;
};

/**
 * Set title of book.
 *
 * @param value Title.
 */
Book.prototype.setTitle = function (value) {
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

As discussed in the introduction, a `ReferenceError` is a common programming fault that occurs when code references an object that hasn't been previously defined or initialized.  The most common cause of such an error is simple typos.  However, many programming languages (and common integrated development environments) are capable of detecting when a reference error is made due to a typo, and will alert you before you even execute your code.  However, Node.js (and JavaScript that it's built upon) is not a strongly-typed language, so many IDEs may have trouble determining if a reference is actually incorrect, or if the code is proper and intentional.  Consequently, even if your IDE _warns_ you about an invalid reference, you're still able to execute your code since there's no pre-compiler double-checking the code, which can lead to a higher rate of `ReferenceErrors` in JavaScript-based applications than in some other stricter languages.

To illustrate we have a very simple code sample.  For a more realistic example we'll be using our `Book` class module, which we can use to define simple book objects with properties like `title`, `author`, `pageCount`, and so forth:

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
  return `'${this.getTitle()}' by ${this.getAuthor()} is ${this.getPageCount()} pages, published ${this.getPublishedAt()} as ${this.getPublicationType()} type.`
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
  this.author = value;
};

/**
 * Set page count of book.
 *
 * @param value Page count.
 */
Book.prototype.setPageCount = function (value) {
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
  this.publishedAt = value;
};

/**
 * Set title of book.
 *
 * @param value Title.
 */
Book.prototype.setTitle = function (value) {
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

We have two test functions that make use of our `Book` class.  We'll start with the `passingTest()` function, which instantiates a new `Book` and then uses the `logging.log()` method to output the object to the console:

```js
function passingTest () {
  try {
    // Create a new Book instance.
    let book = new Book('The Stand', 'Stephen King', 1153, new Date(1978, 0, 1));
    // Output valid Book instance, 'book'.
    logging.log(book);
  } catch (e) {
    if (e instanceof ReferenceError) {
      // Output expected ReferenceErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}
```

Executing this function produces the following output:

```
------- EXECUTING PASSING TEST -------
Book {
  author: 'Stephen King',
  pageCount: 1153,
  publicationType: { [Number: 1] key: 'Digital', value: 1, _options: { ignoreCase: false } },
  publishedAt: 1978-01-01T08:00:00.000Z,
  title: 'The Stand' }
```

It's worth noting that the `publicationType` property is making use of the [`enum`](https://www.npmjs.com/package/enum) module found on [NPM](https://www.npmjs.com/package/enum), which allows us to implement an enumeration for this property.  Consequently, the simple `console.log()` output of our `book` instance object shows the full `publicationType` enum object representation for the default value of `PublicationType.Digital`.  However, if we use a different form of output, such as the `Book.getTagline()` method, we get the expected string value representation of the `publicationType` property:

```js
logging.log(book.getTagline());
// 'The Stand' by Stephen King is 1153 pages, published Sun Jan 01 1978 00:00:00 GMT-0800 (Pacific Standard Time) as Digital type.
```

Anyway, everything above works just as expected, so let's now look at our slightly modified version of this function, `failingTest()`:

```js
function failingTest () {
  try {
    // Create a new Book instance.
    let book = new Book('The Stand', 'Stephen King', 1153, new Date(1978, 0, 1));
    // Output invalid Book instance, 'boo'.
    logging.log(boo);
  } catch (e) {
    if (e instanceof ReferenceError) {
      // Output expected ReferenceErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}
```

The name of the function is a strong indicator that something is wrong here.  As it happens, you may notice that we instantiate the same `Book` instance again, but this time our call to `logging.log()` passes an invalid reference to `boo`, rather than `book`.  As mentioned, the most common cause of `ReferenceErrors` is typos, so this scenario isn't too out of the ordinary.  Executing this code produces the following output, showing that a `ReferenceError` was, indeed, thrown:

```
------- EXECUTING FAILING TEST -------
[EXPLICIT] ReferenceError: boo is not defined
    at failingTest (D:\work\Airbrake.io\Exceptions\NodeJS\Error\ReferenceError\app.js:35:17)
    at executeTests (D:\work\Airbrake.io\Exceptions\NodeJS\Error\ReferenceError\app.js:9:3)
    at Object.<anonymous> (D:\work\Airbrake.io\Exceptions\NodeJS\Error\ReferenceError\app.js:47:1)
    at Module._compile (module.js:632:14)
    at Object.Module._extensions..js (module.js:646:10)
    at Module.load (module.js:554:32)
    at tryModuleLoad (module.js:497:12)
    at Function.Module._load (module.js:489:3)
    at Function.Module.runMain (module.js:676:10)
    at startup (bootstrap_node.js:187:16)
```

Airbrake's robust <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-nodejs-error-handling">error monitoring software</a> provides real-time error monitoring and automatic error reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize error parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-nodejs-error-handling">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A deep dive into the ReferenceError in Node.js, with sample code showing how even the simplest code could result in typos and thrown ReferenceErrors.

---

__SOURCES__

- https://nodejs.org/api/errors.html