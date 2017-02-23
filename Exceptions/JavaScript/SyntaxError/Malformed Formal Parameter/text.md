# SyntaxError: Malformed formal parameter

Moving along through our __JavaScript Error Handling__ series, today we're going to closely examine the `Malformed Formal Parameter` error.  `Malformed Formal Parameter` errors appear when attempting to use the [`Function()`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Function) constructor to create your own function object, but then specifying invalid parameters when doing so.

In this article, we'll dive a bit deeper into the `Malformed Formal Parameter`, see where it sits within the JavaScript error hierarchy, and look at just how to deal with any `Malformed Formal Parameter` errors you may encounter in your own swims into the murky waters of coding.  Let's get this boat sailing!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object, or an inherited object therein.
- The [`SyntaxError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SyntaxError) object is inherited from the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object.
- The `Malformed Formal Parameter` error is a specific type of [`SyntaxError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SyntaxError) object.

## When Should You Use It?

To understand just what an `Malformed Formal Parameter` error means, we must first briefly look at the [`Function()`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Function) constructor.  Simply put, `Function()` is used to _programmatically_ create a new function.  Rather than creating your function inline, as is normally the case, you can use `Function()` and pass in however many arguments you wish your function to have, along with the final argument you pass, which is the `functionBody` (the code between the brackets `{ ... }` that will be executed when calling the function).

For example, here we have a normal inline function called `fullName(first, last)`, that we use to combine the `first` and `last` parameters into the returned, full name value:

```js
function fullName(first, last) {
    return `${first} ${last}`;
}

console.log(fullName('Jane', 'Doe'));
```

Calling our function combines the two parameters of `Jane` and `Doe` and returns that string, which we can then output to the console:

```
Jane Doe
```

However, if we wish, we can programmatically create this same function using the `Function()` constructor, by passing the arguments as strings, along with the `functionBody` as our final argument.  Here's the same example as above, but using `Function()`:

```js
var fullName = Function("first", "last", "return `${first} ${last}`;");

console.log(fullName('Jane', 'Doe'));
```

As expected, this functions the same as before, outputting our full name value to the console:

```
Jane Doe
```

With a basic understanding of how `Function()` is used, we can now take a look at `Malformed Formal Parameter` errors.  Simply put, a `Malformed Formal Parameter` error occurs when calling the `Function()` constructor, but when the formatting of the argument string(s) passed to it are malformed.  This might include extra or missing comma separators, or invalid argument names.

For example, here we're using the same `Function()` constructor call as above, but we've accidentally added an extra comma after the initial parameter (`first`):

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    var fullName = Function("first,", "last", "return `${first} ${last}`;");

    console.log(fullName('Jane', 'Doe'));
} catch (e) {
    if (e instanceof SyntaxError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

The difference is subtle, but notice that extra comma after the word first (`"first,"`).  Sure enough, this is a malformed parameter, and JavaScript produces an `Malformed Formal Parameter` error (in Firefox, at least), to tell us as much:

```
FIREFOX:
[EXPLICIT] SyntaxError: malformed formal parameter

CHROME:
[EXPLICIT] SyntaxError: Unexpected token ,
```

Since the use of `Function()` allows for programmatic function definition, it can be dangerous and also sometimes difficult to avoid errors in your definition strings.  Another common issue that might cause a `Malformed Formal Parameter` error is accidentally including `keywords` as part of your argument list; stuff that JavaScript uses internally as part of the syntactical structure of its code.  For example, here we've accidentally used the `var` keyword ahead of our `first` argument, which JavaScript is none too pleased with:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    var fullName = Function("var first", "last", "return `${first} ${last}`;");

    console.log(fullName('Jane', 'Doe'));
} catch (e) {
    if (e instanceof SyntaxError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

Sure enough, another `Malformed Formal Parameter` error from Firefox, while Chrome gives a more specific error message:

```
FIREFOX:
[EXPLICIT] SyntaxError: malformed formal parameter

CHROME:
[EXPLICIT] SyntaxError: Unexpected token var
```

To dive even deeper into understanding how your applications deal with JavaScript Errors, check out the revolutionary <a class="js-cta-utm" href="https://airbrake.io/languages/javascript_exception_handler?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-js">Airbrake JavaScript</a> error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

---

__META DESCRIPTION__

A close examination of the Malformed Formal Parameter SyntaxError in JavaScript, along with a brief look at using the Function() constructor as well.

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
