Today as we progress through our __JavaScript Error Handling__ series we're going to take a closer look at the `Assignment to Undeclared Variable` error in all its magnificent glory.  The `Assignment to Undeclared Variable` error crops up anytime code attempts to assign a value to a variable that has yet to be declared (via the `var` keyword).

Below we'll go over a few code examples that will illustrate reproduction of a typical `Assignment to Undeclared Variable` error, as well as outline how to handle this error when it comes about.  Let's get to it!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`] object, or an inherited object therein.
- The [`ReferenceError`] object is inherited from the [`Error`] object.
- The `Assignment to Undeclared Variable` error is a specific type of [`ReferenceError`] object.

To dive even deeper into understanding how your applications deal with JavaScript Errors, check out the revolutionary [`Airbrake JavaScript`] error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

## When Should You Use It?

As mentioned in the introduction, an `Assignment to Undeclared Variable` error will occur when attempting to assign a value to a variable which has not previously been declared using the `var` keyword.  However, beyond this cause, there is also one strict limitation within JavaScript itself, which can cause `Assignment to Undeclared Variable` errors to be _suppressed_ in many circumstances: __An `Assignment to Undeclared Variable` error will only fire when [`Strict Mode`] is active within the executing JavaScript.__

When [`Strict Mode`] is activated, it is a way to programmatically opt in to a restricted variant of JavaScript that has intentionally different semantics and behaviors in a number of cases.  While the entirety of `Strict Mode` is out of the scope of this article, for the purposes of analyzing errors like the `Assignment to Undeclared Variable` error, it's critical to understand that `Strict Mode` causes JavaScript to __disallow__ the creation of accidental global variables.

For example, in normal JavaScript, the following code would be allowed, as the JavaScript engine assumes the intention is to create a new global variable, `names`, and assign it to the value of splitting the full name string of "Bob Smith" that was provided:

```js
names = "Bob Smith".split();
console.log(`First name is ${names[0]}.`)
```

Sure enough, in normal JavaScript, this executes properly without any thrown errors and gives us the expected output:

```
First name is Bob.
```

Therefore, in order for JavaScript to assume it is not the intention of the developer and the code to automatically generate a new global variable anytime an assignment is made to a previously undeclared variable, `Strict Mode` must be enabled.  This is done by including the line `'use strict';` somewhere in the code, preferably at the same scope as the checks for undeclared assignments.

In this example, we are expanding a bit on the concept above, by creating a simple `getFirstName()` function, which takes a single `full_name` parameter, enters into `Strict Mode`, splits the `full_name`, and returns the first value (i.e. the first name).  By assigning `Strict Mode` within the function only, we ensure our strict behavior is only within the scope we want, rather than globally across our entire script:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

var getFirstName = function(full_name) {
    // Setting strict mode
    'use strict';
    // Assigning to `names`, which is undefined
    names = full_name.split(' ');
    return names[0];
}

try {
    var first_name = getFirstName("Bob Smith");
    console.log(`First name is ${first_name}.`)
} catch (e) {
    if (e instanceof ReferenceError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

Now that `Strict Mode` is enabled, the line that worked previously and created a new `names` global variable now, instead, produces an `Assignment to Undeclared Variable` error as expected.  The specific error message produced will differ slightly depending on the JavaScript engine, but the idea is the same:

```
// FIREFOX
[EXPLICIT] ReferenceError: assignment to undeclared variable names

// CHROME
[EXPLICIT] ReferenceError: names is not defined
```

The obvious solution to our issue is to ensure that anytime a variable assignment is made, it must already be declared with proper scope, __or__ we must always specify this new variable declaration with the `var` keyword preceding it:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

var getFirstName = function(full_name) {
    // Setting strict mode
    'use strict';
    // Assigning to `names`, which is now defined
    var names = full_name.split(' ');
    return names[0];
}

try {
    var first_name = getFirstName("Bob Smith");
    console.log(`First name is ${first_name}.`)
} catch (e) {
    if (e instanceof ReferenceError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

With no `Assignment to Undeclared Variable` error produced, as expected, we get our first name output:

```
First name is Bob.
```

[`Airbrake JavaScript`]: https://airbrake.io/languages/javascript_exception_handler
[`Error`]: https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy
[`JavaScript Errors`]: https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy
[`ReferenceError`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/ReferenceError
[`Strict Mode`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Strict_mode

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
