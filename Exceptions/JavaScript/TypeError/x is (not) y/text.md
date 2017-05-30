# JavaScript Errors - "x" is (not) "y" TypeError

Next up in our [__JavaScript Error Handling__](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) series we'll be going over the `"x" is (not) "y" TypeError`.  The `"x" is (not) "y" TypeError` is just one of the many `TypeErrors` we'll take a look at in this error handling series, which typically deal with accessing values that are not the appropriate data types.  In this case, the `"x" is (not) "y" TypeError` is a rather general-purpose error that is thrown most often when methods expect a certain data type as an argument, but are passed a different data type instead.

In this article we'll explore the `"x" is (not) "y" TypeError` in more detail, seeing where it sits within the JavaScript `Exception` hierarchy and also looking at a few simple code examples that show how `"x" is (not) "y" TypeErrors` are commonly thrown.  Let's get to it!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object, or an inherited object therein.
- The [`TypeError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/TypeError) object is inherited from the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object.
- The `"x" is (not) "y" TypeError` is a descendant of [`TypeError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/TypeError) object.

## When Should You Use It?

The `"x" is (not) "y" TypeError` can occur when dealing with most any data type in JavaScript, though it's most common when using `undefined` or `null` types.  We won't go into the full details of these special types in this article, but if you'd like more information check out the details in our [Null or Undefined Has No Properties article](https://airbrake.io/blog/javascript/null-undefined-properties#user-content-when-should-you-use-it) earlier in this series.

The `"x" is (not) "y" TypeError` comes in many forms, hence the relaxed naming convention found in the Mozilla Developer Network documentation.  Simply put, this error occurs during a few different situations:

- The target object is `null` when it shouldn't be.
- The target object is `undefined` when it shouldn't be.
- The target object is _not_ of a specific expected type (`Object`, `Symbol`, `null`, etc).

Let's start with the first two causes, when the target object is `null` or `undefined` yet it's expected to be something else.  To see this in action we have little example snippet with a `title` variable that we've declared as `undefined`.  We then try to extract a substring of `title` using the [`String.prototype.substring()`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/String/substring) method and output the result to the console:

```js
// 1
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    // Initialize title as undefined
    var title = undefined;
    // Try to get substring of undefined title
    console.log(title.substring(1));
} catch (e) {
    if (e instanceof TypeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

The problem here is that `title` is `undefined`, which doesn't have a property or method named `substring`.  Our Chrome output indicates as much in the error message, while Firefox behaves differently and gives us the `"x" is (not) "y" TypeError` format:

```
// CHROME
[EXPLICIT] TypeError: Cannot read property 'substring' of undefined

// FIREFOX
[EXPLICIT] TypeError: title is undefined
```

Let's try `null` instead of `undefined` in the same scenario:

```js
try {
    // Initialize title as null
    var title = null;
    // Try to get substring of null title
    console.log(title.substring(1));
} catch (e) {
    if (e instanceof TypeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

Unsurprisingly, this also throws a `"x" is (not) "y" TypeError` that looks very similar:

```
// CHROME
[EXPLICIT] TypeError: Cannot read property 'substring' of null

// FIREFOX
[EXPLICIT] TypeError: title is null
```

We also saw that, outside of manipulating `null` and `undefined` objects that should be other types, we can also get a `"x" is (not) "y" TypeError` when trying to directly manipulate certain object prototypes by passing in incompatible argument types.  For example, here we've created a simple `String` `title` variable and assigned it a value.  We then want to create an `Object` type from our `title` variable:

```js
try {
    // Initialize title as string
    var title = 'The Hobbit';
    // Try to create object from string title
    console.log(Object.create(title));
} catch (e) {
    if (e instanceof TypeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

The problem arises because the [`Object.create()`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Object/create) method expects a prototype `Object` to be passed in as the first argument, instead of something like a `String`, as in this case.  This results in another `"x" is (not) "y" TypeError` being thrown our way:

```
// CHROME
[EXPLICIT] TypeError: Object prototype may only be an Object or null: The Hobbit

// FIREFOX
[EXPLICIT] TypeError: title is not an object or null
```

As is often the case, Chrome tends to report more accurate and human-readable error messages, so it's explicitly telling us that the passed argument equal to `The Hobbit` should actually be an `Object` or a `null`, whereas Firefox sticks with the `"x" is (not) "y" TypeError` format.

To dive even deeper into understanding how your applications deal with JavaScript Errors, check out the revolutionary <a class="js-cta-utm" href="https://airbrake.io/languages/javascript_exception_handler?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-js">Airbrake JavaScript</a> error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

---

__META DESCRIPTION__

A close look at the "x" is (not) "y" TypeError within JavaScript, including a look at null, undefined, and other incompatible data types.

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
