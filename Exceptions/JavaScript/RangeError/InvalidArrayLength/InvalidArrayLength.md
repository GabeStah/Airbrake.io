Moving right along with our __JavaScript Error Handling__ series, we next come to the `Invalid Array Length` error.  The `Invalid Array Length` error pops up anytime, as the name implies, an `Array` object is created with a length value that is negative or larger than the allowed maximum.

Below we'll examine what causes an `Invalid Array Length` error, as well as explore how to capture and handle this error.  Let's get started!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`] object, or an inherited object therein.
- The [`RangeError`] object is inherited from the [`Error`] object.
- The `Invalid Array Length` error is a specific type of [`RangeError`] object.

## When Should You Use It?

As mentioned above, `Invalid Array Length` errors will occur when JavaScript code attempts to generate an `Array` or `ArrayBuffer` object where the first and only parameter, `arrayLength`, is a number but is also is invalid.  The validity of an `arrayLength` is fairly straight-forward.  It must obey the following rule set:

- Be an integer.
- Be between zero and 2<sup>32</sup>-1 (inclusive).

Any other value provided to the first and only parameter of `Array` or `ArrayBuffer`, that is also a number, is considered invalid and will throw an `Invalid Array Length` error.

Let's see this in action by generating an `Invalid Array Length` error for ourselves.  Here we're creating a new `Array` with the `arrayLength` set to an invalid number, `-1`.  We're also using a basic `try-catch` to grab any produced errors and then a simple `printError` function to output our error messages based on whether they were expected or not (explicit or inexplicit).

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}
try {
    var items = Array(-1);
    console.log(`Length is: ${items.length}`);
} catch (e) {
    if (e instanceof RangeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```
Sure enough, our `arrayLength` is invalid and the output indicates an `Invalid Array Length` error was thrown:

```
[EXPLICIT] RangeError: Invalid array length
```

We can also try an excessively large number, greater than the 2<sup>32</sup>-1 limit, with the same result:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}
try {
    var items = Array(Math.pow(2, 32));
    console.log(`Length is: ${items.length}`);
} catch (e) {
    if (e instanceof RangeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

No matter what, if the output doesn't fall within the limitations, an `Invalid Array Length` error is thrown:

```
[EXPLICIT] RangeError: Invalid array length
```

While it's now simple enough to see what causes an `Invalid Array Length` error, we need to be careful about capturing it properly.  Since its base object type is simply a `RangeError`, if we stick to catching `instanceof` a `RangeError` only, we'll obviously also catch any other types of `RangeErrors` that might occur.

For this reason, to explicitly capture only the `Invalid Array Length` error in this case, we'll need a bit of extra code to determine whether the `RangeError` we're catching is `Invalid Array Length` after all.

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}
try {
    var items = Array(-1);
    console.log(`Length is: ${items.length}`);
} catch (e) {
    if (e instanceof RangeError) {
        if (e.message.toLowerCase().indexOf('invalid array') !== -1) {
            printError(e, true);
        } else {
            printError(e, false);
        }
    } else {
        printError(e, false);
    }
}
```

We've now thrown in a simple check within the error `.message` property to ensure it contains the phrase `'invalid array'` somewhere in the message text.  If so, that is our expected `Invalid Array Length` error.  This means that when a _different_ `RangeError` is produced, there's no need to worry that we're responding to the wrong type.

[`Error`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Error
[`RangeError`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/RangeError
[`Invalid Code Point`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors/Not_a_codepoint
[`code point`]: https://en.wikipedia.org/wiki/Code_point
[`String.fromCodePoint()`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/String/fromCodePoint

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/RangeError
- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors/Invalid_array_length
