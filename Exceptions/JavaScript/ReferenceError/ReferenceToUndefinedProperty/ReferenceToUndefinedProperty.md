Today, as we continue through the next thrilling segment in our __JavaScript Error Handling__ series, we're examining the `Reference to Undefined Property` error with a fine-toothed comb.  The `Reference to Undefined Property` error can only appear when [`strict mode`] is enabled, and will occur when the code attempts to access an object property that simply doesn't exist.

Below we'll take a look at a few examples to show where `Reference to Undefined Property` errors might appear, then take a closer look at how to deal with these little errors when they pop up.  Let's get started!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`] object, or an inherited object therein.
- The [`ReferenceError`] object is inherited from the [`Error`] object.
- The `Reference to Undefined Property` error is a specific type of [`ReferenceError`] object.

## When Should You Use It?

To properly examine `Reference to Undefined Property` errors we first need to understand what [`strict mode`] means in the context of JavaScript.  Put simply, `strict mode` is a toggle which forces JavaScript to behave in a slightly altered manner, typically by opting into less secure limitations placed on upon the code, opening up execution to more dangers and exploits.  However, in some cases it may be necessary to enable `strict mode`, and in such cases, it's entirely possible to produce a `Reference to Undefined Property` error.

The `Reference to Undefined Property` error itself is rather straightforward: It will be thrown anytime a call is made on a property that hasn't been defined for the referenced object.  A call to a property is always in the form of `object.property`, such as `foo.bar`.

As an example, we'll enable `strict mode` below, then create the `greeting` object and attempt to call an undefined `language` property of it:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    'use strict';

    var greeting = {
        text: 'Hello there!',
    };
    console.log(greeting.text);
    console.log(`Language is ${greeting.language}`);
} catch (e) {
    if (e instanceof ReferenceError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

As expected, the `greeting.text` property is displayed properly in the console, but the call to the undefined `greeting.language` property causes an expected `Reference to Undefined Property` error:

```
ReferenceError: reference to undefined property "x"
```

It's important to note that as JavaScript evolves and modern browsers adopt the latest versions, not all errors that once existed continue to be necessary or relevant, and thus they may be deprecated.  The `Reference to Undefined Property` error is one such example; while older browsers will still produce the above error output by catching the `Reference to Undefined Property` error, most modern browsers simply ignore it, as if `strict mode` wasn't enabled.  Unfortunately, there's no programmatic work around for this, other than ensuring there are no references to undefined properties within your code.

Executing the exact same code as above on `Chrome 55` or `Firefox 50`, for example, outputs the `greeting.text` value, but then also outputs `Language is undefined`:

```
Hello there!
Language is undefined
```

This behavior is identical to simply _not_ using `strict mode` at all in an older browser where the `Reference to Undefined Property` error can still be thrown.  JavaScript is effectively silently ignoring the improper reference to the undefined property.

Another safe practice to ensure no `Reference to Undefined Property` errors appear when your code is executed in older browsers is to use the [`Object.prototype.hasOwnProperty()`] method on the instance of the object in question.  This method accepts one parameter, which is the name or symbol of the property to test and ensure it exists as part of the referenced object.

As a simple example, we can modify our above code by including an `if` statement that checks if the `.language` property is actually defined on the `greeting` object:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    'use strict';

    var greeting = {
        text: 'Hello there!',
    };
    console.log(greeting.text);
    if (greeting.hasOwnProperty('language')) {
        console.log(`Language is ${greeting.language}`);
    }
} catch (e) {
    if (e instanceof ReferenceError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

Since `greeting` has no `.language` property, the second output is not produced, nor is a `Reference to Undefined Property` error thrown.

To dive even deeper into understanding how your applications deal with JavaScript Errors, check out the revolutionary [`Airbrake JavaScript`] error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

[`Airbrake JavaScript`]: https://airbrake.io/languages/javascript_exception_handler
[`Error`]: https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy
[`JavaScript Errors`]: https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy
[`ReferenceError`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/ReferenceError
[`Strict Mode`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Strict_mode
[`Object.prototype.hasOwnProperty()`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Object/hasOwnProperty


---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
