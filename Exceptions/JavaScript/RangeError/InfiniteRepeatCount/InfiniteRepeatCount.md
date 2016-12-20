Today as we plow ahead through our __JavaScript Error Handling__ series we're going to be tackling the `Infinite Repeat Count` error.  The `Infinite Repeat Count` error occurs during the use of the [`repeat()`] method of a `String` object in JavaScript, and specifically when the `count` parameter passed to that method is too large.

Below we'll take a look at a few specific examples seen in the wild which can raise an `Infinite Repeat Count` error, and also explore how to deal with this error when it pops up.  Let's get to it!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`] object, or an inherited object therein.
- The [`RangeError`] object is inherited from the [`Error`] object.
- The `Infinite Repeat Count` error is a specific type of [`RangeError`] object.

To dive even deeper into understanding how your applications deal with JavaScript Errors, check out the revolutionary [`Airbrake JavaScript`] error tracking tool for real-time alerts and instantaneous insight into what went wrong with your code.

## When Should You Use It?

As mentioned in the introduction, the `Infinite Repeat Count` error is quite a rather specific error that only rears its ugly head in a handful of situations.  The real clue is in the second word in the name, _repeat_, which hints that the `Infinite Repeat Count` error will occur only when the [`repeat()`] method is used on a `String` object.  Furthermore, the `Infinite Repeat Count` error will then only pop up when the first (and only) parameter, `count`, is provided and is given a value of either `Infinity` _or_ a value which would otherwise cause the resulting `String` to exceed the maximum size limitation within the executing JavaScript engine (depending on the browser).

Let's try it out with a simple example and see if we can't produce our very own `Infinite Repeat Count` error.  Here we're creating a few variables: `count` to represent the number of repetitions of our `String`, and `name` to hold the actual string value.  Then we call the `repeat()` method and attempt to catch our expected `RangeErrors` using a simple `printError` function to format the error output:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    var count = Infinity;
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

As mentioned above, the `Infinite Repeat Count` error can behave slightly differently depending on the JavaScript engine in use.  In this case we're testing with both Chrome and Firefox, and both produced an `Infinite Repeat Count` error but the message of the error itself differs significantly:

```
// FIREFOX
[EXPLICIT] RangeError: repeat count must be less than infinity and not overflow maximum string size

// CHROME
[EXPLICIT] RangeError: Invalid count value
```

Now, setting our `count` value to `Infinity` seems like a cheap trick to trigger an `Infinite Repeat Count` error, so let's try just a very large value to see where we can exceed the `String` size limits of the JavaScript engine.

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    var count = Math.pow(2, 28);
    var name = 'B';
    name.repeat(count);
    console.log(`Count of ${count} is OK!`);
} catch (e) {
    if (e instanceof RangeError) {
        printError(e, true);
        console.log(`Count of ${count} is too much!`);
    } else {
        printError(e, false);
    }
}
```

Here we've change `name` to be only a single character long so the math is simplified, so we know that every repeat from our `count` variable is just adding one single character to our string.  Unfortunately, a `String` of length 2<sup>28</sup> is too large for both Firefox and Chrome:

```
// FIREFOX
[EXPLICIT] RangeError: repeat count must be less than infinity and not overflow maximum string size
Count of 268435456 is too much!

// CHROME
[EXPLICIT] RangeError: Invalid string length
Count of 268435456 is too much!
```

Notice that Chrome's engine recognized that the issue wasn't actually an invalid `count` value passed to the `repeat()` method, but that the resulting string length was too large.

In this case, the max size for our `count` value for Firefox would be 2<sup>28</sup>-1 and 2<sup>27</sup> for Chrome, although this is likely to dramatically increase in future browser versions as the usage of the latest JavaScript, [ECMA 6.0](http://www.ecma-international.org/ecma-262/6.0/#sec-ecmascript-language-types-string-type), becomes more prevalent.

```js
// FIREFOX
var count = Math.pow(2, 28) - 1;

// CHROME
var count = Math.pow(2, 27);
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
