Plowing straight ahead through our __JavaScript Error Handling__ series, today we'll be exploring the fun world of the `Invalid Code Point` error.

Below we'll examine what causes an `Invalid Code Point` error, followed by how to capture and handle this error.  Off we go!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`] object, or an inherited object therein.
- The [`RangeError`] object is inherited from the [`Error`] object.
- The [`Invalid Code Point`] error is a specific type of [`RangeError`] object.

## When Should You Use It?

Generally speaking, the core `RangeError` object manifests anytime a designated value does not fall within the range or set of allowed values.  The type of value and circumstance this can appear in varies, hence why `RangeError` has a number of unique objects under its umbrella, intended to report on whichever error is most appropriate.

For this post, we're examining the `Invalid Code Point` error, so first we must discuss what a `code point` actually is.  Typically, a [`code point`] refers to the number and position of characters within a character encoding scheme, such as `ASCII` or `Unicode`.  In the case of JavaScript and the `Invalid Code Point` error, the `code point` always refers to an integer value that represents the `Unicode` character scheme, within the range of `1,114,112` characters that `Unicode` is comprised of.

One method of utilizing `code points` in JavaScript is through the [`String.fromCodePoint()`] method, which returns a `Unicode` string from the specified sequence of `code point` parameters.

For example, we can output the [`Black Chess Queen`](http://unicode-table.com/en/#265B) character, which is `Unicode` number `U+265B`, with the following code:

```js
console.log(String.fromCodePoint(0x265B));
```

Which outputs a nice little Queen character: `♛`

While we accessed this character above using hexadecimal, we can also do so via the numeric `integer` value instead, which in this case is `9819`:

```js
console.log(String.fromCodePoint(9819));
```

Now, to actually produce an `Invalid Code Point` error, we simply provide an invalid `code point` value to the `String.fromCodePoint()` method.  For example, we'll try giving it a negative number:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}
try {
    console.log(String.fromCodePoint(-10));
} catch (e) {
    if (e instanceof RangeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

We've thrown in a basic `printError()` function to format and output our error message, but sure enough, we see that trying to return the `code point` `Unicode` character of the number `-10` fails and gives us an `Invalid Code Point` error:

```
[EXPLICIT] RangeError: -10 is not a valid code point
```

With a basic understanding of what causes `Invalid Code Point` errors to occur, it's also worth considering that our above example of simply grabbing the first `instanceof` a `RangeError` object won't always be our `Invalid Code Point` error.  The `RangeError` object represents some half-dozen different errors which inherit from the `RangeError` type.  In order to verify that we're only responding to the specific `Invalid Code Point` error, we need to add additional logic by parsing the `.message` property of the error object:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}
try {
    console.log(String.fromCodePoint(-10));
} catch (e) {
    if (e instanceof RangeError) {
        if (e.message.toLowerCase().indexOf('code point') !== -1) {
            printError(e, true);
        } else {
            printError(e, false);
        }
    } else {
        printError(e, false);
    }
}
```

We've now thrown in a simple check within the error `.message` property to ensure it contains the phrase `'code point'` somewhere in the message text.  If so, that is our expected `Invalid Code Point` error.  This means that when a _different_ `RangeError` is produced, such as [`Invalid Array Length`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors/Invalid_array_length), we don't need to worry.

[`Error`]: https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy
[`RangeError`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/RangeError
[`Invalid Code Point`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors/Not_a_codepoint
[`code point`]: https://en.wikipedia.org/wiki/Code_point
[`String.fromCodePoint()`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/String/fromCodePoint

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors/Too_much_recursion
- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/RangeError
- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors/Not_a_codepoint
