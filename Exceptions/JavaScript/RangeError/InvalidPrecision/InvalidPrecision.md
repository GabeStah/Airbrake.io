Today, as we continue along through our in-depth __JavaScript Error Handling__ series, we're going to take a look at the `Invalid Precision` error.  The `Invalid Precision` error will occur anytime a `number` object is provided an invalid numeric value for one of the precision-based JavaScript methods, which we'll explore more below.

In this post, we'll take a look at what causes an `Invalid Precision` error, and also examine how to capture and handle this error.  Off we go!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`] object, or an inherited object therein.
- The [`RangeError`] object is inherited from the [`Error`] object.
- The `Invalid Precision` error is a specific type of [`RangeError`] object.

## When Should You Use It?

To understand what explicitly causes an `Invalid Precision` error, we must first understand the basic methods JavaScript includes and attached to the `Number` object prototype, which allow us to affect the precision representation of a number.  These include:

- [`toExponential()`]: Returns a `String` representing the `Number` in exponential notation.
- [`toFixed()`]: Returns a `Number` using fixed-point notation.
- [`toPrecision()`]: Returns a `String` representing the `Number`, out to the specified level of precision.

Now, anytime one of these methods is called, the first (and only) parameter each accepts is essentially the number of digits (i.e. level of precision) with which to convert the associated `Number` object before returning the new value.

In the event that the provided parameter is invalid (outside the bounds of what each method allows), a `Invalid Precision` error is produced.

For example, let's start with the `toExponential()` method.  Depending on the browser, this allows a value from about `0` to `100` at most.  Thus, providing a value of `101` to this method should produce an `Invalid Precision` error:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}
try {
    var num = 12.3456
    num.toExponential(101);
    console.log(`Num is: ${num}`)
} catch (e) {
    if (e instanceof RangeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

Sure enough, we get an `Invalid Precision` error output, though the specific text varies between the browser engines, a seen below:

```
// Chrome
[EXPLICIT] RangeError: toExponential() argument must be between 0 and 20
// Firefox
[EXPLICIT] RangeError: precision 101 out of range
```

One important note is that, as mentioned, the allowed range of numeric values for each of these three methods varies based on the browser, as shown in the table below:

| Method | Firefox (SpiderMonkey) | Chrome, Opera (V8) |
| --- | --- | --- |
| `toExponential()` | 0 to 100 | 0 to 20 |
| `toFixed()` | -20 to 100 | 0 to 20 |
| `toPrecision()` | 1 to 100 | 1 to 21 |

Therefore, changing our above example to use a precision value of `100` would be valid in Firefox, but considered invalid in `Chrome` or `Opera`.

We can also try the `toFixed()` method with the same precision value of `101` and expect an `Invalid Precision` error as well:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}
try {
    var num = 12.3456
    num.toFixed(101);
    console.log(`Num is: ${num}`)
} catch (e) {
    if (e instanceof RangeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

Sure enough, while Chrome acknowledges specifically which method we're utilizing to produce the error, either way both browsers don't like this:

```
// Chrome
[EXPLICIT] RangeError: toFixed() digits argument must be between 0 and 20
// Firefox
[EXPLICIT] RangeError: precision 101 out of range
```

This difference in error `message` formatting between browsers presents a small challenge when trying to correctly catch and identify an `Invalid Precision` error by properly differentiating it from other errors with the same parent `RangeError` object type.  While we won't go into the specifics of how to identify a user's browser in JavaScript in this little guide, the best method will likely be to verify the browser via [feature detection](http://stackoverflow.com/questions/9847580/how-to-detect-safari-chrome-ie-firefox-and-opera-browser/9851769#9851769), then proceed to parse the appropriate error `message` based on the formatting for that browser engine.

[`Error`]: https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy
[`RangeError`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/RangeError
[`toExponential()`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Number/toExponential
[`toFixed()`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Number/toFixed
[`toPrecision()`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Number/toPrecision

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/RangeError
- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors/Invalid_array_length
