As we stroll our way down the winding path of our __JavaScript Error Handling__ series, today we're stopping to smell the aroma of the `Negative Repeat Count` error.  The `Negative Repeat Count` error, similar to the `Infinite Repeat Count` error, occurs when using the [`repeat()`] method of a `String` object in JavaScript, but in this case when the `count` parameter passed to that method is a __negative__ value.

In this article we'll take a look at a few specific code examples that might produce a `Negative Repeat Count` error, and how to catch and handle this error when it appears.  Here we go!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`] object, or an inherited object therein.
- The [`RangeError`] object is inherited from the [`Error`] object.
- The `Negative Repeat Count` error is a specific type of [`RangeError`] object.

To dive even deeper into understanding how your applications deal with JavaScript Errors, check out the revolutionary [`Airbrake JavaScript`] error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

## When Should You Use It?

Similar to the `Infinite Repeat Count` article, we're using a simple code example to illustrate when a `Negative Repeat Count` error might appear.  Below we have our `count` variable, which represents the number of repetitions of our `String`, and the `name` variable, which is our string to be repeated.  Then we move on to calling the `repeat()` method and attempt to catch our expected `RangeErrors` using a simple `printError` function to format the error output:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    var count = -5;
    var name = 'Bob';
    name.repeat(count);
} catch (e) {
    if (e instanceof RangeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

Since the `count` parameter we're passing here has a negative value of `-5`, as expected, we catch a `Negative Repeat Count` error with our little example snippet.

```
// FIREFOX
[EXPLICIT] RangeError: repeat count must be non-negative

// CHROME
[EXPLICIT] RangeError: Invalid count value
```

As with many other JavaScript errors, often the output messages will differ depending on the JavaScript engine (usually based on the browser in question), and here we see a clear example of this.  Often the engine powering Chrome tends to be more explicit and verbose in its error reporting, but this is one exception (no pun intended) where Firefox takes that honor by explicitly stating that the repeat count value was a negative, whereas Chrome just indicates it is invalid, but fails to indicate why.

As it happens, that's all there is to the `Negative Repeat Count` error, since it only occurs __explicitly__ when that `count` parameter value is a negative integer.  The `repeat()` method itself is even smart enough to catch attempts to pass other object types, such as `Strings`, into the `count` parameter, and will automatically convert such a value to an `integer` before proceeding as normal.  Here, we're trying to pass a `String` representation of a negative number, `'-99'`, but JavaScript will have no part of these shenanigans:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    var count = '-99';
    var name = 'Bob';
    name.repeat(count);
} catch (e) {
    if (e instanceof RangeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

Both browsers first convert this value to a number, then produce the same expected `Negative Repeat Count` error:

```
// FIREFOX
[EXPLICIT] RangeError: repeat count must be non-negative

// CHROME
[EXPLICIT] RangeError: Invalid count value
```

[`Airbrake JavaScript`]: https://airbrake.io/languages/javascript_exception_handler
[`Error`]: https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy
[`JavaScript Errors`]: https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy
[`RangeError`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/RangeError
[`repeat()`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/String/repeat

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/RangeError
- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors/Resulting_string_too_large
