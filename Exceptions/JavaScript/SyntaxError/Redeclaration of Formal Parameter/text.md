# JavaScript Errors - SyntaxError: redeclaration of formal parameter "x"

Continuing our journey through the [__JavaScript Error Handling__](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) series, today we'll be tackling the `Redeclaration of Formal Parameter` JavaScript error.  The `Redeclaration of Formal Parameter` error occurs when attempting to redeclare a function parameter, within the function body itself, by using the `let` assignment.

In this article we'll further explore the `Redeclaration of Formal Parameter` error, looking at where it sits within the JavaScript `Exception` hierarchy, and examining a brief code snippet that illustrates what might cause a `Redeclaration of Formal Parameter` error in your own code, so let's get to it!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object, or an inherited object therein.
- The [`SyntaxError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SyntaxError) object is inherited from the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object.
- The `Redeclaration of Formal Parameter` error is a specific type of [`SyntaxError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SyntaxError) object.

## When Should You Use It?

Since the cause of a `Redeclaration of Formal Parameter` error is directly related to attempting to redeclare a variable, let's take a moment to examine how JavaScript variable declarations work.  In most cases, JavaScript developers are accustomed to using the [`var`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Statements/var) statement, which declares the variable that follows, optionally doing so with an initial value:

```js
// Declare the variable named first.
var first;

// Declare first and initialize its value.
var first = 'Jane';
```

Alternatively, ECMAScript 2015 introduced the [`let`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Statements/let) statement, which also declares variables (along with optional initialization).  However, the difference between `var` and `let` is that `let` locks a variable declaration to the current `scope` context.  That is, a `let` variable is contained within the `code block` in which it is declared.

For example, in the `varExample` function below we declare the `title` variable using `var`, then inside an `if` block scope we redeclare it to a different value:

```js
function varExample() {
    var title = 'The Stand';
    if (true) {
        var title = 'The Shining';
        console.log(`Inner: ${title}`);
    }
    console.log(`Outer: ${title}`);
}
varExample();
```

The output shows us that, by using `var`, the referenced `title` variable is the same in both cases, regardless of the block scope context, so the `title` value is changed to `The Shining` prior to the inner `console.log` output, and then it retains that value for the outer output as well:

```
Inner: The Shining
Outer: The Shining
```

However, if we use `let` instead of `var`, the JavaScript engine understands that we intend to declare a new, unique instance of `title` variable within the `if` block, which is self-contained inside that block scope and retains a different value that we've assigned from the outer version of `title`:

```js
function letExample() {
    let title = 'The Stand';
    if (true) {
        let title = 'The Shining';
        console.log(`Inner: ${title}`);
    }
    console.log(`Outer: ${title}`);
}
letExample();
```

The result is that we get unique values for `title` in both `console.log()` calls:

```
Inner: The Shining
Outer: The Stand
```

With that refresher of `var` versus `let` under our belts, let's tackle the `Redeclaration of Formal Parameter` error now.  Since the error occurs when attempting to redeclare a parameter variable within a function scope, we can start with a normal, working example, using a simple `fullName()` function, which concatenates the `first` and `last` parameter values and returns the result:

```js
// Typical function.
function fullName(first, last) {
    return `${first} ${last}`;
}
console.log(fullName('Jane', 'Doe'));
```

As expected, this outputs the name: `Jane Doe`.

Now, let's try redeclaring our `first` parameter variable within the function scope using the `var` statement:

```js
// Redeclaring first parameter using var.
function fullName(first, last) {
    var first = 'John';
    return `${first} ${last}`;
}
console.log(fullName('Jane', 'Doe'));
```

As we learned above, a second declaration to an existing variable using `var` simply redeclares (and potentially reassigns) that variable.  Therefore, we've just hard-coded a new value of `John` to the `first` value before concatenating and returning it.  This produces an output of the name: `John Doe`.

However, let's see what happens if we try that redeclaration with the `let` statement instead of `var`:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    // Redeclaring first parameter using let.
    function fullName(first, last) {
        let first = 'John';
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

As it happens, this throws a `Redeclaration of Formal Parameter` error at us, indicating that our `first` variable has already been declared:

```
// CHROME
Uncaught SyntaxError: Identifier 'first' has already been declared
// FIREFOX
SyntaxError: redeclaration of formal parameter first
```

As with other `SyntaxErrors` in JavaScript, it's also worth noting that we cannot directly `catch` the `Redeclaration of Formal Parameter` error with a typical `try-catch` block.  Since the issue is improper syntax, our JavaScript parser recognizes the issue once it reaches the problematic snippet within the code, which inadvertently affects the surrounding code syntax, so the engine cannot properly recognize how the surrounding `try-catch` statement should function.

To dive even deeper into understanding how your applications deal with JavaScript Errors, check out the revolutionary <a class="js-cta-utm" href="https://airbrake.io/languages/javascript_exception_handler?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-js">Airbrake JavaScript</a> error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

---

__META DESCRIPTION__

A detailed examination of the redeclaration of formal parameter "x" SyntaxError in JavaScript, including a brief overview of variable declaration techniques.

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
