# SyntaxError: missing ; before statement

As we march along through our __JavaScript Error Handling__ series, today we'll be parading our way through the `Missing Semicolon Before Statement` JavaScript error.  As the name implies, the `Missing Semicolon Before Statement` error is typically thrown when a semicolon (`;`) was forgotten somewhere in the code.

In this article we'll go over the `Missing Semicolon Before Statement` error in more detail, including where it resides within the JavaScript `Exception` hierarchy, and what possible causes could produce a `Missing Semicolon Before Statement` error in the first place.  Let's get this band a'steppin!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object, or an inherited object therein.
- The [`SyntaxError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SyntaxError) object is inherited from the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object.
- The `Missing Semicolon Before Statement` error is a specific type of [`SyntaxError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SyntaxError) object.

## When Should You Use It?

A `Missing Semicolon Before Statement` error, on the face of it, means that the JavaScript engine expected a semicolon (`;`), yet none was provided.  There are many cases where this might occur, ranging from actually forgetting to add a semicolon where needed, to other errors or syntax issues that would accidentally cause a `Missing Semicolon Before Statement` error instead.

As a general rule, JavaScript [`statements`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Statements) must (almost) always be terminated by a semicolon.  The final semicolon is a simple way for the parsing JavaScript engine to determine when a statement both starts and finishes, so it can easily evaluate what should be executed and what should be separate from other code statements.

However, like all rules, the `"JavaScript statements must be terminated with a semicolon"` rule has some exceptions.  This is a behavior in JavaScript known as [`automatic semicolon insertion`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Lexical_grammar#Automatic_semicolon_insertion).  This forces the parsing JavaScript engine to automatically insert (or assume insertion of) semicolons when none are present, following specific types of code statements.  In most cases, statements that qualify for `automatic semicolon insertion` are those statements that **require** a semicolon to follow, lest they be invalid code entirely.

In practice, this manifests itself when JavaScript interprets a typical line break as including a semicolon, even where one doesn't exist.  For example, here we're using the `var` keyword to initialize a variable without a closing semicolon:

```js
var name = "John Smith"
console.log("Hello", name);
```

This is no problem, because the `automatic semicolon insertion` (`ASI`) notices our `var` keyword as the beginning of a statement, and thus it automatically inserts a closing semicolon when it reaches the end of the line.  From the perspective of the JavaScript engine, the above code looks like this:

```js
var name = "John Smith";
console.log("Hello", name);
```

In fact, we can rewrite our original example to be even more abstract, with a line break after every element:

```js
var
name
=
"John Smith"
console.log("Hello", name)
```

Once again, JavaScript inteprets that, using `ASI`, to mean the following (intended) code:

```js
var name = "John Smith";
console.log("Hello", name);
```

With a basic understanding of how JavaScript understands semicolons, we can tackle the `Missing Semicolon Before Statement` error.  As a simple example, what happens if don't abide by the `ASI` rules and try to assign a function to our `getFullName` variable, without remembering the `function` keyword, like so:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    var getFullName;
    getFullName(first, last) {
        return `${first} ${last}`;
    };
    console.log(getFullName("John", "Smith"))
} catch (e) {
    if (e instanceof SyntaxError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

JavaScript is not too happy with this, and throws a `Missing Semicolon Before Statement` error (in Firefox at least, Chrome parses it slightly differently):

```
// FIREFOX
SyntaxError: missing ; before statement
// CHROME
Uncaught SyntaxError: Unexpected token {
```

The issue here is that, after initializing our `getFullName` variable, we wanted to create a function and assign it to that `getFullName` variable, but we forgot the `= function()` syntax, writing this line instead: `getFullName(first, last) {`.  The problem here is that JavaScript interprets this to mean that `getFullName` is a function to be called, in which we're passing in two argument values of `first` and `last`.  Since it then recognizes that a new statement is occurring due to the opening curly brace (`{`) that follows, JavaScript expects a closing semicolon after `last)`, which is missing.

The solution is to cleanup our syntax using `getFullName = function(first, last) {`.  We can also skip the extra line for initializing, and assign our function on the same line that we initialize via `var`:

```js
var getFullName = function(first, last) {
    return `${first} ${last}`;
};
console.log(getFullName("John", "Smith"))
```

We should also note that, similar to other `SyntaxErrors`, the `Missing Semicolon Before Statement` error cannot be captured by most `try-catch` blocks.  Since the syntax issue breaks further execution of code, JavaScript doesn't know what to do after that failure point, so it rarely reaches any surrounding `catch` block.

To dive even deeper into understanding how your applications deal with JavaScript Errors, check out the revolutionary <a class="js-cta-utm" href="https://airbrake.io/languages/javascript_exception_handler?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-js">Airbrake JavaScript</a> error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

---

__META DESCRIPTION__

A close look at the Missing Semicolon Before Statement error in JavaScript, including a brief glimpse at automatic semicolon insertion.

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
