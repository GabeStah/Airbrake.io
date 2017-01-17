Today as we progress through our __JavaScript Error Handling__ series we're going to take a dive into the `Deprecated Caller/Deprecated Arguments` error in all its magnificent glory.  The `Deprecated Caller/Deprecated Arguments` error appears when code attempts to call one of two specific properties of a function while [`strict mode`] is enabled: [`Function.caller`] or [`Function.arguments`].

Below we'll go over a few code examples that will illustrate normal reproduction of a `Deprecated Caller/Deprecated Arguments` error, as well as outline how to deal with this error when it comes about.  Let's get to it!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`] object, or an inherited object therein.
- The [`TypeError`] object is inherited from the [`Error`] object.
- The `Deprecated Caller/Deprecated Arguments` error is a specific type of [`TypeError`] object.

## When Should You Use It?

As mentioned in the introduction, an `Deprecated Caller/Deprecated Arguments` error will occur only when `strict mode` is enabled, which is a method to programmatically opt into a restricted variation of JavaScript that uses a number of different behaviors and semantics from the baseline JavaScript environment.  For our purposes of analyzing the behavior of the `Deprecated Caller/Deprecated Arguments` error, we simply need to know that one behavior in particular of `strict mode` is to prevent the dangerous calling of deprecated properties, such as the aforementioned `Function.caller` or `Function.arguments`.

In normal, `non-strict` JavaScript, the `Function.caller` property returns the function that `invoked` the specified function on which the property is being called.  For example, here we've created two simple functions, `add` and `increment`.  Our `add` function simply adds the two arguments together and returns the result.  To make use of this, within `increment`, we pass the `x` parameter and the value `1`:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

function increment(x)
{
    return add(x, 1);
}

function add(x, y)
{
    console.log(`add.caller is ${add.caller}`)
    console.log(`add.caller.name is ${add.caller.name}.`)
    return x + y;
}

try {
    var value = increment(1);
} catch (e) {
    if (e instanceof TypeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

Ignoring the inefficiency of this method for our purposes, we can see that by calling the `add.caller` property within the `add` function, our console log produces the expected output:

```
add.caller is function increment(x)
      {
          return add(x, 1);
      }
add.caller.name is increment.
```

This tells us that the call made to our `add` function actually took place within another function, in this case the `increment` function.  If we had simply called `add` directly from our main loop, `add.caller` would be `null`.

With that out of the way, let's take a look at how a `Deprecated Caller/Deprecated Arguments` error might show up once we enable `strict mode`.  This is done by simply including the line `'use strict;'` somewhere in the code, ideally within the same scope as the code we expect to cause problems.  In this case, we're enabled `strict mode` within the `add` function, the same scope in which we attempt to access the `add.caller` property:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

function increment(x)
{
    return add(x, 1);
}

function add(x, y)
{
    'use strict';    
    console.log(`add.caller is ${add.caller}`)
    console.log(`add.caller.name is ${add.caller.name}.`)
    return x + y;
}

try {
    var value = increment(1);
} catch (e) {
    if (e instanceof TypeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

Now, instead of getting the `.caller` property information in our output, an ugly `Deprecated Caller/Deprecated Arguments` error pops up, telling us we've attempted to access a restricted property:

```
// Firefox
[EXPLICIT] TypeError: 'caller', 'callee', and 'arguments' properties may not be accessed on strict mode functions or the arguments objects for calls to them

// Chrome
[EXPLICIT] TypeError: 'caller' and 'arguments' are restricted function properties and cannot be accessed in this context.
```

Now that `Strict Mode` is enabled, we can no longer safely call to the `.caller` property, as it has been deprecated due to the security risks it presents.  The same issue occurs if a call is made to the `.arguments` property as well while in `strict mode`.

While the `.caller` property is completely off limits in this context, it's still possible to safely call the `.arguments` property, by alternatively calling a specific local variable within the function called `arguments`:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

function increment(x)
{
    return add(x, 1);
}

function add(x, y)
{
    'use strict';
    console.log(`arguments[0] is ${arguments[0]}`)
    console.log(`arguments[1] is ${arguments[1]}`)
    return x + y;
}

try {
    var value = increment(5);
} catch (e) {
    if (e instanceof TypeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

Notice that `arguments` is not an explicitly created argument within the `add` function, but is instead automatically created by JavaScript within every function.  For this reason, we are able to call `arguments[0]` and `arguments[1]` to get the expected values of `5` and `1` in return:

```
arguments[0] is 5
arguments[1] is 1
```

To dive even deeper into understanding how your applications deal with JavaScript Errors, check out the revolutionary [`Airbrake JavaScript`] error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

[`Airbrake JavaScript`]: https://airbrake.io/languages/javascript_exception_handler
[`Error`]: https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy
[`JavaScript Errors`]: https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy
[`TypeError`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/TypeError
[`Strict Mode`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Strict_mode
[`Function.caller`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Function/caller
[`Function.arguments`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Function/arguments

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
