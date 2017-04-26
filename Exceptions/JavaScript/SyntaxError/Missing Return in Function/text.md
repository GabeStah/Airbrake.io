# JavaScript Errors - SyntaxError: return not in function

Today we'll be continuing our expedition through our [__JavaScript Error Handling__](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) series by looking at another `SyntaxError`, the `Missing Return in Function` error.  As the name implies, the `Missing Return in Function` error pops up when the JavaScript engine detects that a `return` or `yield` statement is being called outside of a function.

We'll take some time in this article to dive into the `Missing Return in Function` error in more detail, examining where it resides in the JavaScript `Exception` hierarchy, while also looking at a few simple code examples to illustrate how `Missing Return in Function` errors might be thrown.  Let's get going!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object, or an inherited object therein.
- The [`SyntaxError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SyntaxError) object is inherited from the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object.
- The `Missing Return in Function` error is a specific type of [`SyntaxError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SyntaxError) object.

## When Should You Use It?

As with all `SyntaxErrors`, the `Missing Return in Function` error doesn't appear due to any sort of logical failing within the code, but instead is due to the JavaScript parsing engine coming across a section of code that it doesn't know how to handle.  In most cases, this is due to a simple typo on the part of the developer, but as with other errors we've explored, it's easiest to explore some example code and see just how a `Missing Return in Function` error might come up.

For starters, the most obvious way to throw a `Missing Return in Function` -- and the method which is _intended_ by the error message itself -- is to simply have a `return` or `yield` statement hanging outside of a function:

```js
// Loose return statement.
return 'Jon Snow';
```

Executing the above code produces a `Missing Return in Function` error, just as we expected.  Moreover, as we've learned is commonly the case, different browsers (and thus different JavaScript engines) produce slightly different error messages:

```
// CHROME
Uncaught SyntaxError: Illegal return statement
// FIREFOX
SyntaxError: return not in function
```

We get similarly bad results for a loose `yield` statement as well:

```js
var name = "You know nothing";
// Loose yield statement.
yield name;
```

However, because of the syntax that JavaScript expects to be present and surrounding a `yield` statement, the errors are not the same, thus we don't technically produce a `Missing Return in Function` error:

```
// CHROME
Uncaught SyntaxError: Unexpected identifier
// FIREFOX
SyntaxError: missing ; before statement
```

A much more common scenario is when trying to create a typical function, but with a minor typo somewhere that alters the way the engine parses the function, thus making it think there is no `return` or `yield` statement where one should exist.  Here we have defined a simple `fullName` function that takes two parameters, `first` and `last`, and combines the two into a full name `string` value to return.  However, if the `first` name value is `John`, we want to change it to `Jonathan` (for some reason, just go with me here), and combine _that_ with the `last` name instead:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    var fullName = function(first, last) {
        if (first == 'John')
            return `Jonathan ${last}`;
        };
        return `${first} ${last}`;
    }
    console.log(fullName('Jane', 'Doe'));
} catch (e) {
    if (e instanceof SyntaxError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

However, running this code throws a `Missing Return in Function` error at us:

```
// CHROME
Uncaught SyntaxError: Illegal return statement
// FIREFOX
SyntaxError: return not in function
```

Keen observers will have noticed the problem, which is that we forgot the opening brace (`{`) after declaring our `if` statement.  This causes the JavaScript engine to parse the intended function incorrectly, and indicating we are missing the `return` statement.  To fix this, we just need to correct that small typo, like so:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    var fullName = function(first, last) {
        if (first == 'John') {
            return `Jonathan ${last}`;
        };
        return `${first} ${last}`;
    }
    console.log(fullName('Jane', 'Doe'));
} catch (e) {
    if (e instanceof SyntaxError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

Also notice that even though we've surrounded our example code with a typical `try-catch` block, as with other `SyntaxErrors`, we cannot catch the `Missing Return in Function` error within the same scope of code in which the issue occurs.  This is because our parser cannot properly parse the surrounding code once it reaches the line containing our `Missing Return in Function` error snippet, since everything else is "offset" by the snippet of improper syntax, and therefore it no longer functions correctly.

To dive even deeper into understanding how your applications deal with JavaScript Errors, check out the revolutionary <a class="js-cta-utm" href="https://airbrake.io/languages/javascript_exception_handler?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-js">Airbrake JavaScript</a> error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

---

__META DESCRIPTION__

A close look at the return not in function SyntaxError within JavaScript, including sample code snippets illustrating how these errors might occur.

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
