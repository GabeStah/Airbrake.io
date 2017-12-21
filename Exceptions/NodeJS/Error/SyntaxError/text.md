# Node.js Error Handling - SyntaxError

Making our way through the twists and turns of our full [**Node.js Error Handling**](https://airbrake.io/blog/nodejs-error-handling/nodejs-error-class-hierarchy) series, today we'll be checking out the **SyntaxError** in Node.  As with nearly ever other programming language on the market, a Node `SyntaxError` indicates that a code statement could not be correctly parsed, which means that the executor has no way of determining the _intent_ behind the code.  This usually happens as a result of typos, but the actual error messages associated with `SyntaxErrors` can be fairly vague and not all that useful.

In this article we'll examine the Node `SyntaxError` by first looking at where it resides in the larger [Node.js Error Class Hierarchy](https://airbrake.io/blog/nodejs-error-handling/nodejs-error-class-hierarchy).  We'll then examine four different functional code samples, each of which demonstrate the current techniques of code evaluation that can potentially throw Node `SyntaxErrors`, so let's get to it!

## The Technical Rundown

Most Node.js errors inherit from the [`Error`](https://nodejs.org/api/errors.html#errors_class_error) base class, or extend from an inherited class therein.  The full error hierarchy of this error is:

- [`Error`](https://nodejs.org/api/errors.html#errors_class_error)
    - `SyntaxError`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```js
const logging = require('logging');
const vm = require('vm');

function executeTests () {
  logging.lineSeparator("functionTest(2, 5, 'return x * y')", 60);
  functionTest(2, 5, 'return x * y');

  logging.lineSeparator("functionTest(2, 5, 'return x  y')", 60);
  functionTest(2, 5, 'return x  y');

  logging.lineSeparator("evalTest('3 * 6')", 60);
  evalTest('3 * 6');

  logging.lineSeparator("evalTest('3 | 6')", 60);
  evalTest('3 _ 6');

  logging.lineSeparator("requireTest(4, 7, './multiply.js')", 60);
  requireTest(4, 7, './multiply.js');

  logging.lineSeparator("requireTest(4, 7, './multiply_invalid.js')", 60);
  requireTest(4, 7, './multiply_invalid.js');

  logging.lineSeparator("vmTest('5 * 8')", 60);
  vmTest('5 * 8');

  logging.lineSeparator("vmTest('5 # 8')", 60);
  vmTest('5 # 8');
}

function evalTest (body) {
  try {
    // Execute eval(body) and output result.
    logging.log(eval(body));
  } catch (e) {
    if (e instanceof SyntaxError) {
      // Output expected SyntaxErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

function functionTest (x, y, body) {
  try {
    // Create function body.
    let f = new Function ('x', 'y', body);
    // Output function result with params passed as args.
    logging.log(`${x} * ${y} = ${f(x, y)}`);
  } catch (e) {
    if (e instanceof SyntaxError) {
      // Output expected SyntaxErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

function requireTest (x, y, path) {
  try {
    // Get multiply function from require path.
    let f = require(path).multiply;
    // Output function result with params passed as args.
    logging.log(`${x} * ${y} = ${f(x, y)}`);
  } catch (e) {
    if (e instanceof SyntaxError) {
      // Output expected SyntaxErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}

function vmTest (body) {
  try {
    // Run the body code in current context.
    logging.log(vm.runInThisContext(body));
  } catch (e) {
    if (e instanceof SyntaxError) {
      // Output expected SyntaxErrors.
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

As mentioned, a `SyntaxError` will be thrown when evaluating code that doesn't make lexical sense to the interpreter.  For example, this may occur because an expected `token` is missing between two values: `x + y` is a completely valid statement, while neglecting the `+` between would result in a `SyntaxError`.

In Node (and JavaScript above it), it is only possible to throw a `SyntaxError` within `evaluated` code.  Code evaluation is the practice of taking a collection of code from another context (such as a `String` or an outside script file) and _evaluating_ it inline, during execution of another script.  For example, the built-in [`eval()`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/eval) function can be used for exactly this purpose, by evaluating a passed `String` argument as JavaScript code.  In Node.js there are four possible forms of code evaluation that can throw `SyntaxErrors`: `eval(...)`, `new Function(...)`, `vm`, or `require(...)`.  We'll examine each of these scenarios one at a time in the small code samples found below.

We start with the [`Function` constructor](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Function), which is a special keyword that allows functions to be created and evaluated inline using the `new Function([arg1[, arg2[, ...argN]],] functionBody)` syntax, as if creating a new instance of any other class:

```js
function functionTest (x, y, body) {
  try {
    // Create function body.
    let f = new Function ('x', 'y', body);
    // Output function result with params passed as args.
    logging.log(`${x} * ${y} = ${f(x, y)}`);
  } catch (e) {
    if (e instanceof SyntaxError) {
      // Output expected SyntaxErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}
```

The `functionTest (x, y, body)` function creates a `new Function(...)` and assigns it to the `f` local variable, which is then executed within the `logging.log(...)` call to output the result to the console.  Consequently, calling `functionTest(x, y, body)` evaluates the `body` argument as the body of the `new Function(...)`.  We'll test this out with two different calls to this `Function` test function:

```js
  logging.lineSeparator("functionTest(2, 5, 'return x * y')", 60);
  functionTest(2, 5, 'return x * y');

  logging.lineSeparator("functionTest(2, 5, 'return x  y')", 60);
  functionTest(2, 5, 'return x  y');
```

Executing these tests produces the following output:

```
----------- functionTest(2, 5, 'return x * y') -----------
2 * 5 = 10
----------- functionTest(2, 5, 'return x  y') ------------
[EXPLICIT] SyntaxError: Unexpected identifier
    at new Function (<anonymous>)
    at functionTest (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SyntaxError\app.js:48:13)
    at executeTests (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SyntaxError\app.js:9:3)
    at Object.<anonymous> (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SyntaxError\app.js:94:1)
    at Module._compile (module.js:632:14)
    at Object.Module._extensions..js (module.js:646:10)
    at Module.load (module.js:554:32)
    at tryModuleLoad (module.js:497:12)
    at Function.Module._load (module.js:489:3)
    at Function.Module.runMain (module.js:676:10)
```

Unsurprisingly, the first invocation with a `body` of `'return x * y'` works properly, while a `body` of `'return x y'` is missing a token between the two parameters, so a `SyntaxError` is thrown.

Next up let's look at the `evalTest(body)` function, which tests the `eval()` built-in function by evaluating the passed `body` string code:

```js
function evalTest (body) {
  try {
    // Execute eval(body) and output result.
    logging.log(eval(body));
  } catch (e) {
    if (e instanceof SyntaxError) {
      // Output expected SyntaxErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}
```

Here's the test calls we're making:

```js
  logging.lineSeparator("evalTest('3 * 6')", 60);
  evalTest('3 * 6');

  logging.lineSeparator("evalTest('3 | 6')", 60);
  evalTest('3 _ 6');
```

And this is the output that is produced:

```
------------------- evalTest('3 * 6') --------------------
18
------------------- evalTest('3 | 6') --------------------
[EXPLICIT] SyntaxError: Unexpected identifier
    at evalTest (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SyntaxError\app.js:33:22)
    at executeTests (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SyntaxError\app.js:15:3)
    at Object.<anonymous> (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SyntaxError\app.js:94:1)
    at Module._compile (module.js:632:14)
    at Object.Module._extensions..js (module.js:646:10)
    at Module.load (module.js:554:32)
    at tryModuleLoad (module.js:497:12)
    at Function.Module._load (module.js:489:3)
    at Function.Module.runMain (module.js:676:10)
    at startup (bootstrap_node.js:187:16)
```

There's a clear (and intentional) pattern forming here: The first of the test pairs works, while the second fails with some kind of `SyntaxError`.

Our third test function is `requireTest(x, y, path)`, which invokes the `require(...)` built-in function to require the code file passed via the `path` parameter.  In this case, it expects an exported function called `multiply`, so it grabs that value and uses it in a function call with the passed `x` and `y` parameters to output the multiplication result:

```js
function requireTest (x, y, path) {
  try {
    // Get multiply function from require path.
    let f = require(path).multiply;
    // Output function result with params passed as args.
    logging.log(`${x} * ${y} = ${f(x, y)}`);
  } catch (e) {
    if (e instanceof SyntaxError) {
      // Output expected SyntaxErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}
```

Since we're requiring outside files here this example requires a bit more code, found within two separate files.  The first file is `multiply.js`:

```js
module.exports = {
  multiply: function(x, y) {
    return x * y;
  }
};
```

And the second is `multiply_broken.js`:

```js
module.exports = {
  multiply: function(x, y) {
    return x $ y;
  }
};
```

We now invoke the `requireTest(x, y, body)` function twice, once for each outside context file:

```js
  logging.lineSeparator("requireTest(4, 7, './multiply.js')", 60);
  requireTest(4, 7, './multiply.js');

  logging.lineSeparator("requireTest(4, 7, './multiply_invalid.js')", 60);
  requireTest(4, 7, './multiply_invalid.js');
```

This results in the following output:

```
----------- requireTest(4, 7, './multiply.js') -----------
4 * 7 = 28
------- requireTest(4, 7, './multiply_invalid.js') -------
[EXPLICIT] SyntaxError: Unexpected identifier
    return x $ y;
             ^

SyntaxError: Unexpected identifier
    at createScript (vm.js:80:10)
    at Object.runInThisContext (vm.js:139:10)
    at Module._compile (module.js:599:28)
    at Object.Module._extensions..js (module.js:646:10)
    at Module.load (module.js:554:32)
    at tryModuleLoad (module.js:497:12)
    at Function.Module._load (module.js:489:3)
    at Module.require (module.js:579:17)
    at require (internal/module.js:11:18)
    at requireTest (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SyntaxError\app.js:65:13)
```

As we can see, the `multiply.js` file contains no syntax issues, so it functions as expected,  Meanwhile, the `multiply_broken.js` file contains an invalid token between `return x` and `y;`, so it throws a `SyntaxError`.

The final scenario in which Node.js can throw a `SyntaxError` is using the built-in [`vm`](https://nodejs.org/api/vm.html) module, which the ability to compile and run code in a V* Virtual Machine context.  After requiring the module at the top of the file, our `vmTest(body)` function handles the testing:

```js
function vmTest (body) {
  try {
    // Run the body code in current context.
    logging.log(vm.runInThisContext(body));
  } catch (e) {
    if (e instanceof SyntaxError) {
      // Output expected SyntaxErrors.
      logging.log(e);
    } else {
      // Output unexpected Errors.
      logging.log(e, false);
    }
  }
}
```

Just as we saw with the `evalTest(body)` function, `vmTest(body)` merely attempts to execute the passed `body` parameter `String` code in the current context and logs the result.  Invoking this function occurs below:

```js
  logging.lineSeparator("vmTest('5 * 8')", 60);
  vmTest('5 * 8');

  logging.lineSeparator("vmTest('5 # 8')", 60);
  vmTest('5 # 8');
```

Once again, the first invocation works properly, while the second contains an invalid token between the numerals, resulting in a thrown `SyntaxError`:

```
-------------------- vmTest('5 * 8') ---------------------
40
-------------------- vmTest('5 # 8') ---------------------
[EXPLICIT] SyntaxError: Invalid or unexpected token
5 # 8
  ^

SyntaxError: Invalid or unexpected token
    at createScript (vm.js:80:10)
    at Object.runInThisContext (vm.js:139:10)
    at vmTest (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SyntaxError\app.js:82:20)
    at executeTests (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SyntaxError\app.js:27:3)
    at Object.<anonymous> (D:\work\Airbrake.io\Exceptions\NodeJS\Error\SyntaxError\app.js:94:1)
    at Module._compile (module.js:632:14)
    at Object.Module._extensions..js (module.js:646:10)
    at Module.load (module.js:554:32)
    at tryModuleLoad (module.js:497:12)
    at Function.Module._load (module.js:489:3)
```

Airbrake's robust <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-nodejs-error-handling">error monitoring software</a> provides real-time error monitoring and automatic error reporting for all your development projects.  Airbrake's state of the art web dashboard ensures you receive round-the-clock status updates on your application's health and error rates.  No matter what you're working on, Airbrake easily integrates with all the most popular languages and frameworks.  Plus, Airbrake makes it easy to customize error parameters, while giving you complete control of the active error filter system, so you only gather the errors that matter most.

Check out <a class="js-cta-utm" href="https://airbrake.io/account/new?utm_source=blog&utm_medium=end-post&utm_campaign=airbrake-nodejs-error-handling">Airbrake's error monitoring software</a> today and see for yourself why so many of the world's best engineering teams use Airbrake to revolutionize their exception handling practices!

---

__META DESCRIPTION__

A close look at the SyntaxError in Node.js, with sample code showing all four possible code evaluation techniques that might throw SyntaxErrors.

---

__SOURCES__

- https://nodejs.org/api/errors.html