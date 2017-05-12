# JavaScript Errors - TypeError: "x" has no properties

Moving along through our [__JavaScript Error Handling__](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) series, today we'll take a look at the `Null or Undefined Has No Properties` error.  `Null or Undefined Has No Properties` is the first `TypeError` we've explored thus far in this series, which generally encompasses issues where values are accessed that are not of the expected `type`.  The `Null or Undefined Has No Properties` error occurs specifically when attempting to call a `property` of the `null` object or `undefined` type.

Throughout this article we'll explore the `Null or Undefined Has No Properties` error in more detail, looking at where it sits in the JavaScript `Exception` hierarchy as well as providing some simple code examples to see how `Null or Undefined Has No Properties` errors are thrown, so let's get started!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object, or an inherited object therein.
- The [`TypeError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/TypeError) object is inherited from the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object.
- The `Null or Undefined Has No Properties` error is a specific type of [`TypeError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/TypeError) object.

## When Should You Use It?

The `Null or Undefined Has No Properties` error deals directly with `null` and `undefined` types in JavaScript, of course, so it's important to understand how these work before understanding why this error occurs in the first place.

The [`null`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/null) type is one of JavaScript's `primitive values` along with stuff like `string`, `number`, `boolean`, `undefined`, and `symbol`.  A `null` reference represents a complete lack of identification.  Simply put, if a variable is assigned to the value of `null`, that indicates that the variable points to _no_ object at all.

```js
console.log(null); // null

var name = null;
console.log(name); // null
```

While they may seem similar, it's important to understand the difference between `null` and `undefined`.  In basic terms, `undefined` means that a variable has been `declared` but has not yet been assigned a value.  Moreover, `null` and `undefined` are different `types`: `null` is actually an object whereas `undefined` is a type unto itself:

```js
console.log(typeof(null)); // object
console.log(typeof(undefined)); // undefined
```

We can also compare the similarity and differences of `undefined` and `null` by checking them using equality (`==`) and identity (`===`) operators:

```js
console.log(null == null); // true
console.log(null === null); // true

console.log(undefined == undefined); // true
console.log(undefined === undefined); // true

// Check equality.
console.log(null == undefined); // true
// Check identity.
console.log(null === undefined); // false
```

At the end we see that even though `null` and `undefined` are considered _equal_, they are not the same identity (equal without type conversion).  As discussed, this is because they are of different types behind the scenes: `null` being an object and `undefined` being an undefined type.

With that out of the way we can start to understand why trying to access a `property` of `null` or `undefined` may fail.  For example, here we're trying to access the `name` property of `undefined`:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    console.log(undefined.name);
} catch (e) {
    if (e instanceof TypeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}  
```

As expected, this throws a `Null or Undefined Has No Properties` error at us (although, as is commonly the case, Chrome reports the error slightly differently):

```
// CHROME
[EXPLICIT] TypeError: Cannot read property 'name' of undefined

// FIREFOX
[EXPLICIT] TypeError: undefined has no properties
``` 

This particular error is probably easiest to understand from the perspective of `undefined`, since `undefined` is not considered an `object` type at all (but its own `undefined` type instead), and properties can only belong to objects within JavaScript.

Let's try accessing the same `name` property of a `null` object and see what happens:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    console.log(null.name);
} catch (e) {
    if (e instanceof TypeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

Sure enough, we get another `Null or Undefined Has No Properties` error:

```
// CHROME
[EXPLICIT] TypeError: Cannot read property 'name' of null

// FIREFOX
[EXPLICIT] TypeError: null has no properties
```

As we saw above, `null` is considered an object type, which can inherently have properties, so why is it that `null` has no properties?  The reason is because, unlike all other objects, `null` represents _nothing_ -- a nonexistent entity.  Unlike every other `object` which might be defined, the JavaScript engine sees a `null` value and immediately treats it as a pointer to nothing.  Since `null` references nothing, it therefore cannot have any properties of its own.

To dive even deeper into understanding how your applications deal with JavaScript Errors, check out the revolutionary <a class="js-cta-utm" href="https://airbrake.io/languages/javascript_exception_handler?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-js">Airbrake JavaScript</a> error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

---

__META DESCRIPTION__

A close look at the TypeError: "x" has no properties TypeError within JavaScript, including a quick look at null and undefined types, with code samples.

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
