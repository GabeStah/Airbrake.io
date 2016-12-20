Next on the docket for our continued dive into the __JavaScript Error Handling__ series is the `Invalid Radix` error.  The `Invalid Radix` error occurs in a specific instance, when the [`toString()`] method of a `Number` object in JavaScript is called, and it includes an _invalid_ passed parameter representing the `radix` (or base) with which to convert the number to a string.  

Below we'll examine a few of these specific instances which can raise an `Invalid Radix` error, and also explore how to deal with this error when it pops up.  Let's get to it!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`] object, or an inherited object therein.
- The [`RangeError`] object is inherited from the [`Error`] object.
- The `Invalid Radix` error is a specific type of [`RangeError`] object.

## When Should You Use It?

As mentioned in the introduction, the `Invalid Radix` error occurs only when JavaScript code calls the specific [`toString()`] method of a `Number` object, _and_ does so by including an invalid value for the only parameter `toString()` accepts.  This (optional) parameter is the [`radix`], which should be an integer between `2` and `36`, that specifies the numeric base to use when representing the `String` version of this numeric value.

For example, within the decimal system that is most commonly used today, the radix is `ten`, since it uses the ten digits from `0` to `9`.  Binary has a radix of `2`, because it only uses the digits of `0` or `1`, while hexadecimal has a radix of `16`, since it uses the digits `0` through `9`, plus the alphabetic characters `a` through `f` as well.

At any rate, the JavaScript `toString()` method limits the radix value passed to it to a range of `2` (binary) through `36` because JavaScript is written using the Latin alphabet, which contains `26` additional alphabetic characters that the base can use to represent digits beyond the initial ten of `0` through `9`, giving a total character count with which to represent digits of `36` at most.

Whew!  With that out of the way, we can explore a bit of code to see just how JavaScript deals with these valid or invalid radix values passed to the `toString()` method.

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}
try {
    var radix = 10;
    var num = 10;
    console.log(`${num}, radix ${radix} = ${(num).toString(radix)}`);
    var radix = 16;
    var num = 10;
    console.log(`${num}, radix ${radix} = ${(num).toString(radix)}`);
    var radix = 2;
    var num = 10;
    console.log(`${num}, radix ${radix} = ${(num).toString(radix)}`);
} catch (e) {
    if (e instanceof RangeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

In this example we've just taken our number of `10` and passed in a few different radices to the `toString()` method to see the results:

```
10, radix 10 = 10
10, radix 16 = a
10, radix 2 = 1010
```

As expected, using a base of `10` the output is the expected value of `10`.  With a base `16` our value is `a`, and within binary using a base of `2`, `10` is converted to `1010`.

If we throw in a value that is outside the bounds of `2` to `36`, however, we expect an `Invalid Radix` error to occur:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}
try {
    var radix = 37;
    var num = 10;
    console.log(`${num}, radix ${radix} = ${(num).toString(radix)}`);
} catch (e) {
    if (e instanceof RangeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

The output is roughly as expected, with a few minor differences in formatting depending on the browser engine that is in use:

```
// Firefox
[EXPLICIT] RangeError: radix must be an integer at least 2 and no greater than 36
// Chrome
[EXPLICIT] RangeError: toString() radix argument must be between 2 and 36
```

As small but interesting note is that while the Firefox engine's `Invalid Radix` error message states that the radix value passed to `toString()` must be an integer, the JavaScript engine will actually automatically convert the passed value to an integer before processing it, if possible.  This means that string representations of valid integers will be converted to integers, and decimals will be rounded as appropriate:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}
try {
    var radix = 10.75;
    var num = 10;
    console.log(`${num}, radix ${radix} = ${(num).toString(radix)}`);
    var radix = "10";
    var num = 10;
    console.log(`${num}, radix ${radix} = ${(num).toString(radix)}`);
} catch (e) {
    if (e instanceof RangeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

No errors are produced and the results are as expected:

```
10, radix 10.75 = 10
10, radix 10 = 10
```

[`Error`]: https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy
[`RangeError`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/RangeError
[`toString()`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Number/toString
[`radix`]: https://en.wikipedia.org/wiki/Radix

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/RangeError
- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors/Bad_radix
