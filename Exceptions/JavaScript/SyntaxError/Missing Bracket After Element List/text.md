# SyntaxError: missing ] after element list

Next up in our adventure through the [__JavaScript Error Handling__](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) series we'll be going over the `Missing Bracket After Element List` JavaScript error.  As directly implied by the name itself, the `Missing Bracket After Element List` error is thrown when an array is initialized with incorrect syntax, such as a missing closing bracket (`]`) or comma (`,`).

In this article we'll explore the `Missing Bracket After Element List` error in more detail, including where it sits within the JavaScript `Exception` hierarchy, along with a few examples of what might cause `Missing Bracket After Element List` errors in your own code.  Let's get crackin'!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object, or an inherited object therein.
- The [`SyntaxError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SyntaxError) object is inherited from the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object.
- The `Missing Bracket After Element List` error is a specific type of [`SyntaxError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SyntaxError) object.

## When Should You Use It?

To understand what could cause a `Missing Bracket After Element List` error, we first need to understand how `Array` objects in JavaScript work.  Specifically, we need to know how `Arrays` are properly formatted, syntactically, so we can then see how improper syntax would lead to a `Missing Bracket After Element List` error.

JavaScript [`Arrays`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array) are simple lists of data, and can be initialized in a few different ways.  Here we use three slightly different methods to create the same five-value `Array`:

```js
// Inline, with values.
var namesA = ['Alice', 'Bob', 'Chris', 'Danielle', 'Elizabeth'];
console.log(namesA);

// New Array object, with arguments.
var namesB = new Array('Alice', 'Bob', 'Chris', 'Danielle', 'Elizabeth');
console.log(namesB);

// New Array object.
var namesC = new Array();
// Add values via .push() method.
namesC.push('Alice');
namesC.push('Bob');
namesC.push('Chris');
namesC.push('Danielle');
namesC.push('Elizabeth');
console.log(namesC);
```

As expected, the output is three different, yet identical, `Arrays` with the same values:

```
["Alice", "Bob", "Chris", "Danielle", "Elizabeth"]
["Alice", "Bob", "Chris", "Danielle", "Elizabeth"]
["Alice", "Bob", "Chris", "Danielle", "Elizabeth"]
```

By far, the inline method of creating a new `Array` using surrounding brackets (`[ ... ]`) is the most popular, and thus, that is where the name of our `Missing Bracket After Element List` error comes from.

Let's take that same initial example above, and see what happens if we neglect the final closing bracket (`]`):

```js
// Missing closing bracket.
var names = ['Alice', 'Bob', 'Chris', 'Danielle', 'Elizabeth';
console.log(names);
```

As it happens, the resulting error is slightly different depending on the browser engine:

```
// CHROME
Uncaught SyntaxError: Unexpected token ;
// FIREFOX
SyntaxError: missing ] after element list
```

This difference in how the browser's JavaScript engines parse, and thus report, the `Missing Bracket After Element List` error is rather interesting.

Chrome notices that the `names` `Array` is being initialized and defined, and it reaches the end of `'Elizabeth'` and expects one of a handful of possible characters:

- A comma (`,`) to indicate another item in the `Array`.
- A closing bracket (`]`) to indicate the `Array` initialization is complete.
- Or, an `operator` of some kind to indicate that additional values (beyond our first `'Elizabeth'` string) should be considered as part of this particular `Array` item value, such as a plus sign (`+`) to concatenate it with another value (`'Elizabeth' + ' Frost'`).

In this particular case, Chrome sees that the next character is a semicolon (`;`), which _is not_ a valid way to complete the initialization of this `Array`, and thus it throws an error indicating as such.

Meanwhile, Firefox's JavaScript engine is less subtle.  Even though the underlying parser recognizes that any of the above types of characters are completely valid to follow our `'Elizabeth'` value, Firefox _explicitly_ tells us that a closing bracket (`]`) is what is missing.  While the specific `Missing Bracket After Element List` error that Firefox reports does inform us that there's a syntax issue when defining our `Array`, it isn't _technically_ accurate, since another character, besides a closing bracket (`]`), could go there and be perfectly valid.  In the long run it doesn't much matter, but these differences are somewhat interesting.

Another potential cause for a `Missing Bracket After Element List` error when creating a new `Array` is when we're missing a comma delimiter between multiple values:

```js
// Missing comma between first two values.
var names = ['Alice' 'Bob', 'Chris', 'Danielle', 'Elizabeth'];
console.log(names);
```

Just as before, there's a distinct difference between the various browsers when reporting the `Missing Bracket After Element List` error:

```
// CHROME
Uncaught SyntaxError: Unexpected string
// FIREFOX
SyntaxError: missing ] after element list
```

Again, Chrome recognizes the subtlety of the syntax a bit more, noticing that there are two strings within the "element" that should represent a single value of our `Array`, but there is no `operator` to indicate what action to take on both string values (such as concatenating them).  Thus, Chrome reports that the second string of `'Bob'` is `unexpected`.

Meanwhile, Firefox once again just throws the `Missing Bracket After Element List` error, even though there _is_ a closing bracket (`]`) at the end of our `Array` definition.

To dive even deeper into understanding how your applications deal with JavaScript Errors, check out the revolutionary <a class="js-cta-utm" href="https://airbrake.io/languages/javascript_exception_handler?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-js">Airbrake JavaScript</a> error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

---

__META DESCRIPTION__

A close look at the missing ] after element list SyntaxError error in JavaScript, including the subtle differences between browser error reports.

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
