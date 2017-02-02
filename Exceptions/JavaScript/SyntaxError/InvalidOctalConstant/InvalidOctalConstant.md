Continuing right along through our __JavaScript Error Handling__ series, today we'll be taking a closer look at the `Invalid Octal Constant` warning.  The `Invalid Octal Constant` warning is quite unique in the realm of all the JavaScript errors we've explored thus far, because it only appears in two very specific instances: when defining an `octal literal` value of `08` or `09`.

Below we'll explore just what `octal literals` are, how an `Invalid Octal Constant` warning might then appear, and what to do to avoid these errors yourself.  Let's get going!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`] object, or an inherited object therein.
- The [`SyntaxError`] object is inherited from the [`Error`] object.
- The `Invalid Octal Constant` warning is a specific type of [`SyntaxError`] object.

## When Should You Use It?

Since the appearance of an `Invalid Octal Constant` warning deals with `octal literals`, we should first take a closer look at what `octal literals` are and why they're used.  Time for a tiny bit of math (apologies all around)!

In short, the [`octal`] numeral system is a base-8 number system.  This differs from the `decimal` system that we all use daily, which is base-10, meaning that every place represented by a `decimal` number is a power of ten.  On the other hand, the `octal` system is base-8, so every place represents a power of eight.

To see this in action, first let's pick any `decimal` number, in this case we'll go with `99`.  Now, to convert from `decimal` to `octal`, we must take the `binary` representation of our `decimal` number (`99`), and group it into groups of __three__ binary digits, beginning at the right side.

So, to begin, we start with the `99`:

```
99 (decimal)
01100011 (binary)
```

Since we need groups of __three__ binary digits to convert to `octal`, and our binary representation only contains eight digits in this case, we must add extra zeroes to the left side until we have a quantity of binary digits divisible by three.  So here, we're adding one zero to the left side:

```
99 (decimal)
01100011 (binary)
001 100 011 (binary triplets)
```

Next, we convert each of those binary triplets into their `octal` equivalent.  The trick here is that since the maximum value that a trio of binary digits can represent is `7` (`111` in binary), we are now using `octal` representations within binary.  So in this case, our binary triplets are converted to: `1 4 3`

```
99 (decimal)
01100011 (binary)
001 100 011 (binary triplets)
1 4 3 (octal numerals)
```

If we squish it together, we get the `octal numeral` of `143`, which is the `octal` equivalent to `99` in `decimal`!  To prove this, we can simply take our separated `octal numerals` (`1 4 3`) and go through each one at a time, multiplying the numeral by the `octal power` represented by that place in the number.  So the right-most place would multiplied by `8` raised to the power of `0`, the next place would be multiplied by `8` raised to the power of `1`, and so on down the line.  The resulting calculation looks like this:

```
1 4 3 (octal numerals)

143 (octal) = 3 * 8^0 + 4 * 8^1 + 1 * 8^2
 =
3 + 32 + 64
 =
99
```

Whew!  Now that we know what an `octal` is, how does this apply to JavaScript at the appearance of the `Invalid Octal Constant` warning?  JavaScript allows for the expression of numeric [`literals`] to be defined in a variety of ways:

```js
// With no lead
123; // decimal, base 10

// With leading zero, up to 7
04; // octal, base 8

// With leading zero, above 9
056; // decimal, base 10

// With leading 0x
0x1234 // hexadecimal, base 16

// With leading 0b
0b10 // binary, base 2
```

The problem, and where we can see an `Invalid Octal Constant` warning appear, is when attempting to create a `literal` with a leading zero that falls within that gap between `octal` and `decimal`: `08` or `09`.  JavaScript initially tries to convert a leading-zero `literal` to `octal`, but since we know that `08` and `09` are too large to be represented in `octal` (which is maximum of zero through seven), but too small to be `decimal` (which starts at ten), JavaScript throws the `Invalid Octal Constant` warning.  Here's a simple example illustrating this:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}


try {
    console.log(08);
} catch (e) {
    if (e instanceof SyntaxError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

What's important to note here is that this isn't technically an `error`, but instead it's a `warning`.  Therefore, in spite of our best efforts, we cannot `catch` this `Invalid Octal Constant` warning, and the engine spits out both a decimal number to the log, as well as the warning message:

```
8
SyntaxError: 08 is not a legal ECMA-262 octal constant
```

To dive even deeper into understanding how your applications deal with JavaScript Errors, check out the revolutionary [`Airbrake JavaScript`] error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

[`Airbrake JavaScript`]: https://airbrake.io/languages/javascript_exception_handler
[`Error`]: https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy
[`SyntaxError`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SyntaxError
[`strict mode`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Strict_mode
[`octal`]: https://en.wikipedia.org/wiki/Octal
[`literals`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Guide/Grammar_and_types#Literals


---

__META DESCRIPTION__

A detailed exploration of octals, numeric literals, and the invalid octal constant SyntaxError in JavaScript.

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
