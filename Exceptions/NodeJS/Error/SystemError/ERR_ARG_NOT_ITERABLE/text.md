# Node.js Error Handling - ERR_ARG_NOT_ITERABLE

Making our way through our in-depth [**Node.js Error Handling**](https://airbrake.io/blog/nodejs-error-handling/nodejs-error-class-hierarchy) series, today we'll be going over the first of the various `System Errors` that can be thrown in Node, **ERR_ARG_NOT_ITERABLE**.  Node throws a `System Error` when an exception occurs within the program's runtime environment and are typically an indication that there was an operational problem within the application.  In the case of the `ERR_ARG_NOT_ITERABLE` error, the its appearance indicates that a Node.js API method or function expected an iterable argument, but the actual value passed was not an iterable.

Within this article we'll examine the `ERR_ARG_NOT_ITERABLE` system error in greater detail by first looking at where it resides in the overall [Node.js Error Class Hierarchy](https://airbrake.io/blog/nodejs-error-handling/nodejs-error-class-hierarchy).  We'll also examine some functional code samples that illustrate how a typical Node.js API class ([`URLSearchParams`](https://nodejs.org/dist/latest-v9.x/docs/api/url.html#url_class_urlsearchparams)) might throw an `ERR_ARG_NOT_ITERABLE` if passed arguments are the incorrect type.  Let's get into it!

## The Technical Rundown

Most Node.js errors inherit from the [`Error`](https://nodejs.org/api/errors.html#errors_class_error) base class, or extend from an inherited class therein.  The full error hierarchy of this error is:

- [`Error`](https://nodejs.org/api/errors.html#errors_class_error)
    - [`SystemError`](https://nodejs.org/dist/latest-v8.x/docs/api/errors.html#errors_system_errors)
      - `ERR_ARG_NOT_ITERABLE`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```js
const logging = require('logging');
const { URL, URLSearchParams } = require('url');

function executeTests () {
  let params;

  params = 'title=The Hobbit&author=J.R.R. Tolkien&pageCount=366';
  logging.lineSeparator('STRING TEST', 60);
  getURLSearchParams(params);

  params = {
    title: 'The Stand',
    author: 'Stephen King',
    pageCount: 1153
  };
  logging.lineSeparator('OBJECT TEST', 60);
  getURLSearchParams(params);

  params = [
    ['title', 'The Name of the Wind'],
    ['author', 'Patrick Rothfuss'],
    ['pageCount', 662]
  ];
  logging.lineSeparator('ITERABLE TEST', 60);
  getURLSearchParams(params);

  params = new Map([
    ['title', "The Wise Man's Fear"],
    ['author', 'Patrick Rothfuss'],
    ['pageCount', 994]
  ]);
  logging.lineSeparator('MAP TEST', 60);
  getURLSearchParams(params);

  params = {
    [Symbol.iterator]: [
      ['title', 'The Slow Regard of Silent Things'],
      ['author', 'Patrick Rothfuss'],
      ['pageCount', 159]
    ]
  };
  logging.lineSeparator('NON-WELL-FORMED ITERABLE FUNCTION TEST', 60);
  getURLSearchParams(params);

  params = {
    [Symbol.iterator]: function* () {
      yield ['title', 'The Slow Regard of Silent Things'];
      yield ['author', 'Patrick Rothfuss'];
      yield ['pageCount', 159];
    }
  };
  logging.lineSeparator('WELL-FORMED ITERABLE FUNCTION TEST', 60);
  getURLSearchParams(params);
}

/**
 * Gets a URLSearchParams instance object by passing the value param to the constructor.
 * @param value Value to be passed to URLSearchParams constructor.
 * @returns {*} URLSearchParams instance, or undefined.
 */
function getURLSearchParams (value) {
  try {
    // Get instance from constructor, log to console, and return object.
    let instance = new URLSearchParams(value);
    logging.log(instance);
    return instance;
  } catch (e) {
    // Catch TypeError with code property of ERR_ARG_NOT_ITERABLE.
    if (e instanceof TypeError && e.code === 'ERR_ARG_NOT_ITERABLE') {
      // Output expected ERR_ARG_NOT_ITERABLE TypeErrors.
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

The `ERR_ARG_NOT_ITERABLE` code indicates that an iterable argument was expected within a particular Node.js API method call, but a non-iterable object type was passed instead.  At present, there are not many Node.js API methods that throw `ERR_ARG_NOT_ITERABLEs`, so we'll just be taking a look at a single example class that does this, the [`URLSearchParams`](https://nodejs.org/dist/latest-v9.x/docs/api/url.html#url_class_urlsearchparams) constructor.  The primary purpose of `URLSearchParams` is to provide read/write access to the query string values of a `URL`.  It can easily parse, append, or otherwise modify parameters, to simplify the process of handling URLs and URIs.

To illustrate the use of `URLSearchParams` we start with a simple test method that wraps the `URLSearchParams` constructor and passes the provided parameter to it, before logging and returning the result:

```js
/**
 * Gets a URLSearchParams instance object by passing the value param to the constructor.
 * @param value Value to be passed to URLSearchParams constructor.
 * @returns {*} URLSearchParams instance, or undefined.
 */
function getURLSearchParams (value) {
  try {
    // Get instance from constructor, log to console, and return object.
    let instance = new URLSearchParams(value);
    logging.log(instance);
    return instance;
  } catch (e) {
    // Catch TypeError with code property of ERR_ARG_NOT_ITERABLE.
    if (e instanceof TypeError && e.code === 'ERR_ARG_NOT_ITERABLE') {
      // Output expected ERR_ARG_NOT_ITERABLE TypeErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}
```

With this function ready to go we can perform a series of simple tests by passing various types of values to see how the `URLSearchParams` constructor handles them.  We'll begin with a simple string that most of us are accustomed to seeing in our browsers:

```js
let params;

params = 'title=The Hobbit&author=J.R.R. Tolkien&pageCount=366';
logging.lineSeparator('STRING TEST', 60);
getURLSearchParams(params);
```

Executing this code produces the following output:

```
---------------------- STRING TEST -----------------------
URLSearchParams {
  'title' => 'The Hobbit',
  'author' => 'J.R.R. Tolkien',
  'pageCount' => '366' }
```

The `--- STRING TEST ---` header indicates the type of argument value that was passed, and below that we see the produced `URLSearchParams` object that resulted.  Everything works as expected!

We'll next try passing an object:

```js
params = {
  title: 'The Stand',
  author: 'Stephen King',
  pageCount: 1153
};
logging.lineSeparator('OBJECT TEST', 60);
getURLSearchParams(params);
```

This works just as before, producing a value instance and output:

```
---------------------- OBJECT TEST -----------------------
URLSearchParams {
  'title' => 'The Stand',
  'author' => 'Stephen King',
  'pageCount' => '1153' }
```

The `URLSearchParams` constructor can also accept iterable objects, such as arrays or `Map` objects:

```js
params = [
  ['title', 'The Name of the Wind'],
  ['author', 'Patrick Rothfuss'],
  ['pageCount', 662]
];
logging.lineSeparator('ITERABLE TEST', 60);
getURLSearchParams(params);

params = new Map([
  ['title', "The Wise Man's Fear"],
  ['author', 'Patrick Rothfuss'],
  ['pageCount', 994]
]);
logging.lineSeparator('MAP TEST', 60);
getURLSearchParams(params);  
```

As before, both of these tests work as expected and produce valid outputs:

```
--------------------- ITERABLE TEST ----------------------
URLSearchParams {
  'title' => 'The Name of the Wind',
  'author' => 'Patrick Rothfuss',
  'pageCount' => '662' }
------------------------ MAP TEST ------------------------
URLSearchParams {
  'title' => 'The Wise Man\'s Fear',
  'author' => 'Patrick Rothfuss',
  'pageCount' => '994' }
```

Modern JavaScript includes the ability [to create `iterators` and `generators`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Guide/Iterators_and_Generators).  An `iterator` is basically a function that knows how to provide access to a sequential set of items.  There are a number of ways to create an iterator, but [ECMAScript 2015](http://www.ecma-international.org/ecma-262/6.0/#sec-symbol.iterator) introduced the `Symbol.iterator` well-known symbol, which is used to define an object's default iterator method.  Once an iterator function is defined for an object, that object can then be used in iterable situations, such as the common `for...of` loop.

In the following example we try to pass a set of params to `URLSearchParams` that uses the `Symbol.iterator` to define an iterator function as an array:

```js
params = {
  [Symbol.iterator]: [
    ['title', 'The Slow Regard of Silent Things'],
    ['author', 'Patrick Rothfuss'],
    ['pageCount', 159]
  ]
};
logging.lineSeparator('NON-WELL-FORMED ITERABLE FUNCTION TEST', 60);
getURLSearchParams(params);
```

Running this code finally throws a `TypeError` with the explicit `ERR_ARG_NOT_ITERABLE` `code` attribute, indicating that the query pairs of the iterator must be iterable:

```
--------- NON-WELL-FORMED ITERABLE FUNCTION TEST ---------
[EXPLICIT] TypeError [ERR_ARG_NOT_ITERABLE]: Query pairs must be iterable
    at new URLSearchParams (internal/url.js:111:17)
    at getURLSearchParams (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_ARG_NOT_ITERABLE\index.js:64:20)
    at executeTests (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_ARG_NOT_ITERABLE\index.js:43:3)
    at Object.<anonymous> (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SystemError\ERR_ARG_NOT_ITERABLE\index.js:79:1)
    at Module._compile (module.js:632:14)
    at Object.Module._extensions..js (module.js:646:10)
    at Module.load (module.js:554:32)
    at tryModuleLoad (module.js:497:12)
    at Function.Module._load (module.js:489:3)
    at Function.Module.runMain (module.js:676:10)
```

This particular error might not make much sense, but it becomes much clearer if we look at the internal [`url.js:111` source code](https://github.com/nodejs/node/blob/master/lib/internal/url.js#L111) that threw the error in the first place:

```js
// see: https://github.com/nodejs/node/blob/master/lib/internal/url.js#L111
class URLSearchParams {
  constructor(init = undefined) {
    if (init === null || init === undefined) {
      this[searchParams] = [];
    } else if ((typeof init === 'object' && init !== null) ||
               typeof init === 'function') {
      const method = init[Symbol.iterator];
      if (method === this[Symbol.iterator]) {
        const childParams = init[searchParams];
        this[searchParams] = childParams.slice();
      } else if (method !== null && method !== undefined) {
        if (typeof method !== 'function') {
          throw new errors.TypeError('ERR_ARG_NOT_ITERABLE', 'Query pairs');
        }
        
// ...
```

The `URLSearchParams` constructor accepts `object` and `function` type arguments.  It then generates a local `method` variable to retrieve the `@@iterator` method of the passed `init` parameter using `inti[Symbol.iterator]`.  In our example code case, this iterator isn't equivalent to `this[Symbol.iterator]`, but `method` is also not a `function`, so it throws an `ERR_ARG_NOT_ITERABLE` `TypeError`.

To resolve this issue we merely need to actually assign an iterable function to the `Symbol.iterator` value in the `params` object:


```js
params = {
  [Symbol.iterator]: function* () {
    yield ['title', 'The Slow Regard of Silent Things'];
    yield ['author', 'Patrick Rothfuss'];
    yield ['pageCount', 159];
  }
};
logging.lineSeparator('WELL-FORMED ITERABLE FUNCTION TEST', 60);
getURLSearchParams(params);
```

Here we're using the [`function*`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Statements/function*) declaration, which defines a `generator` function that can use the special `yield` keyword to iterate through returned values.  In this case, we're yielding key/value pairs for the query params we want to create.  Executing this well-formed iterable function test works as expected and produces the following output:

```
----------- WELL-FORMED ITERABLE FUNCTION TEST -----------
URLSearchParams {
  'title' => 'The Slow Regard of Silent Things',
  'author' => 'Patrick Rothfuss',
  'pageCount' => '159' }
```

Airbrake's robust <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-nodejs-error-handling">error monitoring software</a> provides real-time error monitoring and automatic error reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize error parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-nodejs-error-handling">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look at the ERR_ARG_NOT_ITERABLE TypeError in Node.js, with sample code showing how passing non-iterable arguments can produce such errors.

---

__SOURCES__

- https://nodejs.org/api/errors.html
- https://github.com/nodejs/node/blob/master/test/common/wpt.js