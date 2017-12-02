# Node.js Error Handling - AssertionError

Starting off into our detailed [**Node.js Error Handling**](https://airbrake.io/blog/nodejs-error-handling/nodejs-error-class-hierarchy) series, today we'll be going over the **AssertionError**.  As with many other programming languages, Node.js comes with an [`assert`](https://nodejs.org/api/assert.html) class as part of its core modules set, allowing simple assertion tests to be performed.  When such an assertion fails an `AssertionError` is thrown to indicate what went wrong.

Throughout this article we'll explore the `AssertionError` in greater detail, starting with where it sits in the overall [Node.js Error Class Hierarchy](https://airbrake.io/blog/nodejs-error-handling/nodejs-error-class-hierarchy).  We'll also look at some functional sample code illustrating how these errors are typically thrown and how you can handle them in your own code, so let's get crackin'!

## The Technical Rundown

Most Node.js errors inherit from the [`Error`](https://nodejs.org/api/errors.html#errors_class_error) base class, or extend from an inherited class therein.  The full error hierarchy of this error is:

- [`Error`](https://nodejs.org/api/errors.html#errors_class_error)
    - `AssertionError`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```js
const assert = require('assert');
const AssertionError = require('assert').AssertionError;

function executeTests () {
  console.log("++++++++++++++++++++++++++++++");

  assertStrictEquality(0, 1);

  console.log("++++++++++++++++++++++++++++++");

  assertStrictEquality(0, 1, "0 and 1 are not equal!");

  console.log("++++++++++++++++++++++++++++++");

  assertStrictEquality(1, 1);

  console.log("++++++++++++++++++++++++++++++");

  assertStrictInequality(4, 4);

  console.log("++++++++++++++++++++++++++++++");

  assertStrictInequality(4, 4, "4 and 4 are equivalent!");

  console.log("++++++++++++++++++++++++++++++");

  assertStrictInequality(4, 5);
}

function assertStrictEquality (a, b, message = null) {
  try {
    // Output test.
    console.log(`----- ASSERTING: ${a} === ${b} -----`);
    // Assert equality of a and b parameters.
    assert.strictEqual(a, b, message);
    // Output confirmation of successful assertion.
    console.log(`----- CONFIRMED: ${a} === ${b} -----`);
  } catch (e) {
    if (e instanceof AssertionError) {
      // Output expected AssertionErrors.
      console.log(e);
    } else {
      // Output unexpected Errors.
      console.log(e);
    }
  }
}

function assertStrictInequality (a, b, message = null) {
  try {
    console.log(`----- ASSERTING: ${a} !== ${b} -----`);
    // Assert inequality of a and b parameters.
    assert.notStrictEqual(a, b, message);
    // Output confirmation of successful assertion.
    console.log(`----- CONFIRMED: ${a} !== ${b} -----`);
  } catch (e) {
    if (e instanceof AssertionError) {
      // Output expected AssertionErrors.
      console.log(e);
    } else {
      // Output unexpected Errors.
      console.log(e);
    }
  }
}

executeTests();
```

## When Should You Use It?

As mentioned, the only time you should experience an `AssertionError` in a Node.js application is when making use of [`assert`](https://nodejs.org/api/assert.html) API calls.  Thus, it's worth briefly going over what `assert` is used for before we get into the errors it produces.

In general terms, a `test assertion` is an expression or statement that encapsulates a snippet of testable logic, with a clearly defined `target` to be tested.  Each test assertion should be simple and easily executable, without attempting to perform more than a singular task.  Thus, most assertion libraries/modules found in programming languages perform only the most rudimentary tests, such as testing for _equality_ of two values.

The [`assert`](https://nodejs.org/api/assert.html) class in Node.js provides a handful of basic methods, such as `assert.equal` to test equality, `assert.notEqual` for inequality, and `assert.ok` to test the "truthiness" of the passed argument.  Regardless of what `assert` method is being called a failure will always throw an `AssertionError`.

To illustrate this in action we've got some super simple code:

```js
const assert = require('assert');
const AssertionError = require('assert').AssertionError;
```

We start by requiring the `assert` core module, along with assigning the `AssertionError` constant to the `AssertionError` found in the `assert` module.  It's worth noting that `AssertionError` directly inherits from the `Error` class, which gives it a number of built-in functionalities we'll look at in a moment.

Next we define the first of our two test functions, `assertStrictEquality (a, b, message = null)`:

```js
function assertStrictEquality (a, b, message = null) {
  try {
    // Output test.
    console.log(`----- ASSERTING: ${a} === ${b} -----`);
    // Assert equality of a and b parameters.
    assert.strictEqual(a, b, message);
    // Output confirmation of successful assertion.
    console.log(`----- CONFIRMED: ${a} === ${b} -----`);
  } catch (e) {
    if (e instanceof AssertionError) {
      // Output expected AssertionErrors.
      console.log(e);
    } else {
      // Output unexpected Errors.
      console.log(e);
    }
  }
}
```

For the most part, this function is just a wrapper to perform an `assert.strictEquality(...)` call on the two `a` and `b` parameters of our function.  We've included some useful log output showing what our attempted assertion is, and if it succeeded.  In the event of a failure, we catch the error and perform a check of the error class type before outputting it to the log.

Let's make a few calls to `assertStrictEquality(...)` and see what happens:

```js
console.log("++++++++++++++++++++++++++++++");

assertStrictEquality(0, 1);

console.log("++++++++++++++++++++++++++++++");

assertStrictEquality(0, 1, "0 and 1 are not equal!");

console.log("++++++++++++++++++++++++++++++");

assertStrictEquality(1, 1);
```

Running these tests produces the following output:

```
++++++++++++++++++++++++++++++
----- ASSERTING: 0 === 1 -----
{ AssertionError [ERR_ASSERTION]: 0 === 1
    at assertStrictEquality (D:\work\Airbrake.io\Exceptions\NodeJS\Error\AssertionError\app.js:35:12)
    at executeTests (D:\work\Airbrake.io\Exceptions\NodeJS\Error\AssertionError\app.js:7:3)
    at Object.<anonymous> (D:\work\Airbrake.io\Exceptions\NodeJS\Error\AssertionError\app.js:67:1)
    at Module._compile (module.js:632:14)
    at Object.Module._extensions..js (module.js:646:10)
    at Module.load (module.js:554:32)
    at tryModuleLoad (module.js:497:12)
    at Function.Module._load (module.js:489:3)
    at Function.Module.runMain (module.js:676:10)
    at startup (bootstrap_node.js:187:16)
  generatedMessage: true,
  name: 'AssertionError [ERR_ASSERTION]',
  code: 'ERR_ASSERTION',
  actual: 0,
  expected: 1,
  operator: '===' }

++++++++++++++++++++++++++++++
----- ASSERTING: 0 === 1 -----
{ AssertionError [ERR_ASSERTION]: 0 and 1 are not equal!
    at assertStrictEquality (D:\work\Airbrake.io\Exceptions\NodeJS\Error\AssertionError\app.js:35:12)
    at executeTests (D:\work\Airbrake.io\Exceptions\NodeJS\Error\AssertionError\app.js:11:3)
    at Object.<anonymous> (D:\work\Airbrake.io\Exceptions\NodeJS\Error\AssertionError\app.js:67:1)
    at Module._compile (module.js:632:14)
    at Object.Module._extensions..js (module.js:646:10)
    at Module.load (module.js:554:32)
    at tryModuleLoad (module.js:497:12)
    at Function.Module._load (module.js:489:3)
    at Function.Module.runMain (module.js:676:10)
    at startup (bootstrap_node.js:187:16)
  generatedMessage: false,
  name: 'AssertionError [ERR_ASSERTION]',
  code: 'ERR_ASSERTION',
  actual: 0,
  expected: 1,
  operator: '===' }

++++++++++++++++++++++++++++++
----- ASSERTING: 1 === 1 -----
----- CONFIRMED: 1 === 1 -----
```

As expected, the first assertion that `1` equals `0` fails and produces our first `AssertionError` with the default error message of `0 === 1`.  As mentioned, since `AssertionError` derives from the base `Error` class, the actual `AssertionError` that is caught is an anonymous object that contains a handful of useful properties.  One such property is `generatedMessage`, which is a boolean indicating if the associated message was automatically generated by Node.js, or whether it was manually provided elsewhere.

Our second function call makes use of this custom message capability by passing an argument to the `message` parameter, which is passed as the third argument to the underlying `assert.strictEqual(...)` method.  Consequently, even though we get another `AssertionError` since `0` and `1` are not equal, the associated message is now a custom message: `0 and 1 are not equal!`.

Finally, to make sure everything works as expected we pass two equivalent values of `1` and `1` and the log output confirms the assertion succeeded.

Next up, our `assertStrictInequality (a, b, message = null)` function does much the same as the previous function, but inverses the logic:

```js
function assertStrictInequality (a, b, message = null) {
  try {
    console.log(`----- ASSERTING: ${a} !== ${b} -----`);
    // Assert inequality of a and b parameters.
    assert.notStrictEqual(a, b, message);
    // Output confirmation of successful assertion.
    console.log(`----- CONFIRMED: ${a} !== ${b} -----`);
  } catch (e) {
    if (e instanceof AssertionError) {
      // Output expected AssertionErrors.
      console.log(e);
    } else {
      // Output unexpected Errors.
      console.log(e);
    }
  }
}
```

Just as before, we'll perform three test calls with various arguments for each invocation:

```js
console.log("++++++++++++++++++++++++++++++");

assertStrictInequality(4, 4);

console.log("++++++++++++++++++++++++++++++");

assertStrictInequality(4, 4, "4 and 4 are equivalent!");

console.log("++++++++++++++++++++++++++++++");

assertStrictInequality(4, 5);
```

And, also as we saw with the `strictEqual` assertion tests above, these calls produce the expected console outputs:

```
++++++++++++++++++++++++++++++
----- ASSERTING: 4 !== 4 -----
{ AssertionError [ERR_ASSERTION]: 4 !== 4
    at assertStrictInequality (D:\work\Airbrake.io\Exceptions\NodeJS\Error\AssertionError\app.js:53:12)
    at executeTests (D:\work\Airbrake.io\Exceptions\NodeJS\Error\AssertionError\app.js:19:3)
    at Object.<anonymous> (D:\work\Airbrake.io\Exceptions\NodeJS\Error\AssertionError\app.js:67:1)
    at Module._compile (module.js:632:14)
    at Object.Module._extensions..js (module.js:646:10)
    at Module.load (module.js:554:32)
    at tryModuleLoad (module.js:497:12)
    at Function.Module._load (module.js:489:3)
    at Function.Module.runMain (module.js:676:10)
    at startup (bootstrap_node.js:187:16)
  generatedMessage: true,
  name: 'AssertionError [ERR_ASSERTION]',
  code: 'ERR_ASSERTION',
  actual: 4,
  expected: 4,
  operator: '!==' }
++++++++++++++++++++++++++++++
----- ASSERTING: 4 !== 4 -----
{ AssertionError [ERR_ASSERTION]: 4 and 4 are equivalent!
    at assertStrictInequality (D:\work\Airbrake.io\Exceptions\NodeJS\Error\AssertionError\app.js:53:12)
    at executeTests (D:\work\Airbrake.io\Exceptions\NodeJS\Error\AssertionError\app.js:23:3)
    at Object.<anonymous> (D:\work\Airbrake.io\Exceptions\NodeJS\Error\AssertionError\app.js:67:1)
    at Module._compile (module.js:632:14)
    at Object.Module._extensions..js (module.js:646:10)
    at Module.load (module.js:554:32)
    at tryModuleLoad (module.js:497:12)
    at Function.Module._load (module.js:489:3)
    at Function.Module.runMain (module.js:676:10)
    at startup (bootstrap_node.js:187:16)
  generatedMessage: false,
  name: 'AssertionError [ERR_ASSERTION]',
  code: 'ERR_ASSERTION',
  actual: 4,
  expected: 4,
  operator: '!==' }
++++++++++++++++++++++++++++++
----- ASSERTING: 4 !== 5 -----
----- CONFIRMED: 4 !== 5 -----
```

Airbrake's robust <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-nodejs-error-handling">error monitoring software</a> provides real-time error monitoring and automatic error reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize error parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-nodejs-error-handling">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A brief look at the AssertionError in Node.js, including an overview of test assertions and functional code samples showing how to execute assertions.

---

__SOURCES__

- https://nodejs.org/api/errors.html