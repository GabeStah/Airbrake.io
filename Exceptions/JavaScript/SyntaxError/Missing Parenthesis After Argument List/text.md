# SyntaxError: missing ) after argument list

Continuing through our __JavaScript Error Handling__ series, today we'll be looking closely at the `Missing Parenthesis After Argument List` JavaScript error.  The `Missing Parenthesis After Argument List` error can occur for a variety of reasons, but like most `SyntaxErrors`, it commonly pops up when there's a typo, an operator is missing, or a string is not escaped properly.

In this article we'll examine the `Missing Parenthesis After Argument List` error in a bit more detail, including where it fits in the JavaScript `Exception` hierarchy, and what causes such errors to occur.  Away we go!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object, or an inherited object therein.
- The [`SyntaxError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SyntaxError) object is inherited from the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object.
- The `Missing Parenthesis After Argument List` error is a specific type of [`SyntaxError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SyntaxError) object.

## When Should You Use It?

As mentioned in the introduction, the `Missing Parenthesis After Argument List` error can occur for a variety of reasons.  Most of the time, the issue relates to a typo or a forgotten operator of some kind.  To better illustrate this, we can just explore a few simple examples.

A very typical action in JavaScript is to `concatenate` multiple strings together to form one larger string.  This can be performed using a simple `+` operator between two strings: `console.log("Hello " + "world");`

Or, you can also concatenate strings inline, using backtick (`` ` ``) and bracket (`{}`) syntax: `` console.log(`Hello ${worldVar}`); ``

Regardless of how it's done, many JavaScript methods accept an indefinite number of arguments (such as strings), including the `console.log()` method.  In the example below, notice what happens if we forget to include any form of concatenation for our two strings:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    var name = "Jane Doe";
    console.log("Name is:" name);
} catch (e) {
    if (e instanceof SyntaxError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

The result is that we immediately produce a `Missing Parenthesis After Argument List` error:

```
Uncaught SyntaxError: missing ) after argument list
```

As you may notice, we passed two arguments to `console.log()`, but we did not separate them by a typical comma (`,`), nor did we concatenate our two string values together with one of the above methods.  This causes JavaScript to parse our code just fine, until it reaches the end of the first string (`is:"`) and moves onto the next argument (`name`).  Since we didn't tell it to concatenate, nor to expect another argument through the use of a comma separator, JavaScript expects that to be the end of our argument list to the `console.log()` method, and finds that our closing parenthesis is missing (`)`), thus throwing a `Missing Parenthesis After Argument List` error.

The solution depends on how we want our code to behave, but in this case because we're passing arguments to `console.log()`, we can achieve concatenation directly, or by simply adding a comma separator.  The comma separator is generally more readable for our purposes, so let's go with that option:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    var name = "Jane Doe";
    console.log("Name is:", name);
} catch (e) {
    if (e instanceof SyntaxError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

This gives us our name output as expected:

```
Name is: Jane Doe
```

A rather simple fix, to be sure, but that's the issue with the `Missing Parenthesis After Argument List` error, and with `SyntaxErrors` in general.  They're all so obvious once discovered, but unless your code editor parses and evaluates your code for syntax errors on the fly, it's often easy to miss them until your test out the code yourself.

It's also worth noting that, like other `SyntaxErrors`, the `Missing Parenthesis After Argument List` error cannot be easily captured by the typical `try-catch` block.  Since the problem is syntax, the JavaScript engine's attempt to execute the problematic code fails out at that exact moment.  This usually means that it doesn't reach the point in execution where it can continue to the `catch` portion of the block, since it doesn't know how to parse that correctly.  This can be worked around by displacing execution through different files and layers, but for all basic intents and purposes, `catching` `SyntaxErrors` is a major challenge.

To dive even deeper into understanding how your applications deal with JavaScript Errors, check out the revolutionary <a class="js-cta-utm" href="https://airbrake.io/languages/javascript_exception_handler?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-js">Airbrake JavaScript</a> error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

---

__META DESCRIPTION__

A close examination of the Missing Parenthesis After Argument List error in JavaScript, including a brief look at string concatenation.

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
