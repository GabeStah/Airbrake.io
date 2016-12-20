Next up on the list of articles in our __JavaScript Error Handling__ series we take a closer look at the `Undefined Variable` error.  The `Undefined Variable` error is thrown when a reference to a variable or object is made in code that either doesn't exist, or is outside the scope of the executing code.

Below we'll take a look at a couple of specific examples that will commonly produce a `Undefined Variable` error, as well as how to catch and deal with this error when it appears.  Let's get started!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`] object, or an inherited object therein.
- The [`ReferenceError`] object is inherited from the [`Error`] object.
- The `Undefined Variable` error is a specific type of [`ReferenceError`] object.

To dive even deeper into understanding how your applications deal with JavaScript Errors, check out the revolutionary [`Airbrake JavaScript`] error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

## When Should You Use It?

When deep in the process of coding with JavaScript, it isn't all that unheard of to make a typo or simply forget to initialize a variable or object before calling said variable later down the line.  When this occurs, JavaScript will show its displeasure by throwing a `Undefined Variable` error, indicating that the referenced object was not previously defined.

For example, here we're making a simple statement of attempting to grab the `.length` property of our undefined `item` variable.  We're also using a simple `try-catch` block and grabbing any `ReferenceErrors` that might occur, then passing them along to a simple `printError` function to beautify the output of our error messages:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    // Calling an undefined `item `variable
    var length = item.length;
    console.log(`Length is ${length}.`)
} catch (e) {
    if (e instanceof ReferenceError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

Sure enough, as expected, JavaScript notices that the `item` variable is undefined, and produces the explicit `Undefined Variable` error:

```
// FIREFOX
[EXPLICIT] ReferenceError: item is not defined

// CHROME
[EXPLICIT] ReferenceError: item is not defined
```

It's worth noting that unlike many other JavaScript errors we've covered in this series, the `Undefined Variable` error message text does not differ between the two engines powering Firefox or Chrome.

The obvious and simple fix to this particular `Undefined Variable` error is to simply declare our `item` variable prior to calling it:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    // Defining `item` first
    var item = "Bob";
    var length = item.length;
    console.log(`Length is ${length}.`)
} catch (e) {
    if (e instanceof ReferenceError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

Now we get past the `item.length` call without throwing any errors and thus produce our `console.log` output of the length of our `item` string:

```
Length is 3.
```

Technically, while the `Undefined Variable` error is intended to identify references to undefined variables, it also plays a role when attempting to reference variables that _are_ defined, but are outside of the current scope context where the code is being executed.

For example, here we have a simple `getFullName` function, which defines two variables inside itself, `firstName` and `lastName`.  Outside of that function's scope, we attempt to get the `length` property of the `firstName` variable:

```js
var getFullName = function() {
    var firstName = "Bob";
    var lastName = "Smith";
    return `${firstName} ${lastName}`;
}

var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    // Accessing `firstName` from outside its scope
    var length = firstName.length;
    console.log(`Length is ${length}.`)
} catch (e) {
    if (e instanceof ReferenceError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

While the `firstName` variable is technically defined already, it is inaccessible to us at this level of execution, and thus a `Undefined Variable` error is thrown:

```
[EXPLICIT] ReferenceError: firstName is not defined
```

In this case, resolution is a matter of pulling the `firstName` and `lastName` variable outside the scope of the `getFullName` function, so they are within the same context of execution as our `try-catch` block:

```js
// 4
// Declaring the variables outside our function
var firstName = "Bob";
var lastName = "Smith";
var getFullName = function() {
    return `${firstName} ${lastName}`;
}

var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    // Accessing `firstName` is now allowed
    var length = firstName.length;
    console.log(`Length is ${length}.`)
} catch (e) {
    if (e instanceof ReferenceError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

As expected, no errors are produced and we get the length of the `firstName` variable as output:

```
Length is 3.
```

[`Airbrake JavaScript`]: https://airbrake.io/languages/javascript_exception_handler
[`Error`]: https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy
[`JavaScript Errors`]: https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy
[`ReferenceError`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/ReferenceError

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/ReferenceError
- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors/Not_defined
