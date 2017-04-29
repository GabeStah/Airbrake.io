# JavaScript Errors - SyntaxError: test for equality (==) mistyped as assignment (=)?

Making our way through the wonders of the [__JavaScript Error Handling__](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) series, today we'll explore the `Equality Mistyped as Assignment` `SyntaxError`.  The `Equality Mistyped as Assignment` error is thrown when an attempt is made to declare an `assignment` of a variable, where the parsing engine believes the intention was an `equality` test instead.

In this article we'll look closely at the `Equality Mistyped as Assignment` error, seeing where it fits within the JavaScript `Exception` hierarchy, and we'll also go over a simple code example to illustrate how `Equality Mistyped as Assignment` errors might be typically thrown in real code.  Let's get to it!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object, or an inherited object therein.
- The [`SyntaxError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SyntaxError) object is inherited from the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object.
- The `Equality Mistyped as Assignment` error is a specific type of [`SyntaxError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SyntaxError) object.

## When Should You Use It?

Since the `Equality Mistyped as Assignment` error deals with the concepts of `equality` and `assignments`, we should briefly cover both before diving into the error itself.

As with many other programming languages, JavaScript's [`equality` operator](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Operators/Comparison_Operators?v=control#Equality_operators) is simply a pair of equals signs (`==`) separating the two objects to be compared.  If the two objects are not the same `type`, the engine will first attempt to convert them to matching types (such as converting a `string` to a `number`), before comparing their equivalence.  In the case where both items to be compared are `objects`, the comparison doesn't check the objects' `values` (since it's unaware of what those are), and instead simply checks the in-memory reference (`memory address`) of both objects, to see if they're equivalent.

As discussed in the [official documentation](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Operators/Comparison_Operators?v=control#Equality_operators), these differences in `equality` operators can be seen below:

```js
1       ==  1        // true
'1'     ==  1        // true
1       == '1'       // true
0       == false     // true
0       == null      // false
var object1 = {'value': 'key'}, object2 = {'value': 'key'}; 
object1 == object2   // false
0       == undefined // false
null    == undefined // true
```

The [`assignment` operator](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Operators/Assignment_Operators#Assignment) found in JavaScript is even more common across other languages and is typically a single equals sign (`=`) separating the variable and the value.  JavaScript also allows `assignment` operators to be `chained` onto one another, which forces the engine to execute (and assign) via "outside-in" order, starting with the outermost assignment and working inward:

```js
name = 'John'
age = 40
eyes = 'blue'

age = name
console.log(age) // John
name = age = eyes
console.log(age) // blue
console.log(name) // blue
```

Armed with the basics of `assignment` and `equality` operators, we can now take a closer look at the `Equality Mistyped as Assignment` error.  As mentioned in the introduction, the basic problem when an `Equality Mistyped as Assignment` error is thrown is that the JavaScript parser notices an `assignment` operator in a location it expects that an `equality` operator might have been intended.

Before we see an example of this it's critical to note that, as with a few other errors we've explored in this series, the `Equality Mistyped as Assignment` error can only be thrown when `strict mode` is enabled in JavaScript.  In short, `strict mode` is a toggled directive that forces JavaScript to behave in a slightly altered manner, usually by opting into more secure limitations placed upon the code, and thereby reducing the possibility of executing unintended code or performing unintended functionality.  Since `strict mode` can heighten overall security, it is considered a requirement in certain coding situations, and in such cases, it's entirely possible to throw an `Equality Mistyped as Assignment` error.

In most cases, `strict mode` is enabled by including the `'use strict';` directive at the start (and within the same `scope`) of the code you wish to execute via `strict` limitations:

```js
'use strict';
// Do something...
```

Since `strict mode` remembers its own local scope of execution, most uses of `strict mode` are performed inside an [immediately invoked function expression](https://en.wikipedia.org/wiki/Immediately-invoked_function_expression) (`IIFE`), which is a syntactic method of defining a function, by surrounding it with `parentheses`, which effectively tells the JavaScript engine that this function should be treated as a `grouped` entity and executed (`invoked`) immediately.  Thus, our example code is contained within an `IIFE`, and includes an `assignment` operator inside the `if` statement clause:

```js
(function() {
    // Enable strict mode.
    "use strict";
    // Create new name var.
    var name = 'John';
    // Assign name to new value, which should be disallowed in strict mode.
    if (name = 'Jane') {
        // Output name.
        console.log(name);
    }  
})(); 
```

While very basic, this code performs an `assignment` within the `if` statement on line 7, which is interpreted as an intended `equality` operator instead.  This causes an `Equality Mistyped as Assignment` error to be thrown in Firefox:

```
// FIREFOX
SyntaxError: test for equality (==) mistyped as assignment (=)?

// CHROME
Jane
```

As we've discussed many times before, different JavaScript engines (from different browsers) handle parsing and exceptions differently.  In this case, the latest version of Chrome does not recognize this as an issue, and thus ignores the "error" entirely, simply performing the `name = 'Jane'` `assignment`, as our code told it to, and outputting the new result.  I leave it up to you to decide if this is an issue or not, but it's just something to be aware of when developing and testing on different platforms.

It's also worth noting that, just as with other `SyntaxErrors`, the `Equality Mistyped as Assignment` error is difficult to `catch` in a typical `try-catch` code block.  Here we try surrounding our `IIFE` example snippet in a `try-catch` block, but the JavaScript parser recognizes (and reacts to) the `SyntaxError` before it has a chance to notice (and thus execute) the surrounding `try-catch` code:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    (function() {
        // Enable strict mode.
        "use strict";
        // Create new name var.
        var name = 'John';
        // Assign name to new value, which should be disallowed in strict mode.
        if (name = 'Jane') {
            // Output name.
            console.log(name);
        }  
    })(); 
} catch (e) {
    if (e instanceof SyntaxError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

To dive even deeper into understanding how your applications deal with JavaScript Errors, check out the revolutionary <a class="js-cta-utm" href="https://airbrake.io/languages/javascript_exception_handler?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-js">Airbrake JavaScript</a> error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

---

__META DESCRIPTION__

A deep dive into the test for equality (==) mistyped as assignment (=)? SyntaxError within JavaScript, including a brief review of strict mode and simple code examples.

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
