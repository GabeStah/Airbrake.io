# JavaScript Errors - X Is Not a Function TypeError

Making our way through our [__JavaScript Error Handling__](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) series, today we'll tackle the fun little error known as the `X Is Not a Function TypeError`.  As indicated by the name itself, the `X Is Not a Function TypeError` is most often thrown when attempting to invoke a `function()` call on a value or object that doesn't actually represent a function itself.

In this article we'll explore the `X Is Not a Function TypeError` in greater detail, including where it sits in the JavaScript `Exception` hierarchy, as well as a few simple code examples that illustrate how `X Is Not a Function TypeErrors` may occur.  Let's get crackin'!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object, or an inherited object therein.
- The [`TypeError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/TypeError) object is inherited from the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object.
- The `X Is Not a Function TypeError` is a descendant of [`TypeError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/TypeError) object.

## When Should You Use It?

An `X Is Not a Function TypeError` typically occurs in one of the following three scenarios:

- When a function call is made on a property of that simply _isn't_ a function.
- When a function call is made on an object type that doesn't contain that function or method.
- When a function call is made on a built-in method that expects a `callback` function argument to be provided, but no such function exists.

Let's go down the list and take a look at some code that illustrate each of these scenarios.  To begin we have a sparse HTML page with a `title` property.  The proper way to retrieve that property is typically by calling the `document.title` property.  Thus, in our JavaScript `<script>` tag we call `document.title` and output the result to confirm it works:

```html
<!DOCTYPE html>
<html>

<head>
    <meta charset="UTF-8" />
    <title>Welcome to the Site!</title>
    <script>
        var printError = function (error, explicit) {
            console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
        }

        try {
            // Output document.title
            console.log(`Title is: ${document.title}`);
        } catch (e) {
            if (e instanceof TypeError) {
                printError(e, true);
            } else {
                printError(e, false);
            }
        }
    </script>
</head>

</html>
```

As expected, the output shows us what we're after:

```
Title is: Welcome to the Site!
```

However, since `document.title` is a property, what happens if we try to call it like a function (with parentheses)?

```html
<!DOCTYPE html>
<html>

<head>
    <meta charset="UTF-8" />
    <title>Welcome to the Site!</title>
    <script>
        var printError = function (error, explicit) {
            console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
        }

        try {
            // Output document.title()
            console.log(`Title is: ${document.title()}`);
        } catch (e) {
            if (e instanceof TypeError) {
                printError(e, true);
            } else {
                printError(e, false);
            }
        }
    </script>
</head>

</html>
```

Now we've accidentally caused a `X Is Not a Function TypeError` to be thrown because we're trying to call `document.title()` like a function instead of a property:

```
[EXPLICIT] TypeError: document.title is not a function
```

Another common cause of `X Is Not a Function TypeErrors` is when trying to call a particular method on an object that doesn't contain that method/function call.  For example, here we have a pair of `Arrays` that contain information about our book (`Robinson Crusoe`) that we want to combine into one single array.  Thankfully, we can simply use the [`Array.prototype.concat()`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/concat?v=control) method to take our `book` array and combine it with the passed in `publisher` array:

```js
var printError = function (error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    // Book title and author
    var book = ['Robinson Crusoe', 'Daniel Defoe'];
    // Book publisher
    var publisher = ['W. Taylor'];

    // Combine book and publisher arrays with Array.prototype.concat()
    var combined = book.concat(publisher);
    
    // Output result
    console.log(combined);
} catch (e) {
    if (e instanceof TypeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

The resulting `combined` output shows our concatenation works as expected:

```
(3) ["Robinson Crusoe", "Daniel Defoe", "W. Taylor"]
```

Let's try the same thing but for a pair of `Objects` with the same sort of data about our book.  Here we try to combine the `book` and `publisher` objects using the `concat()` method call on `book` just as before:

```js
// Book title and author
var book = {
    title: 'Robinson Crusoe',
    author: 'Daniel Defoe'
};
// Book publisher
var publisher = {
    name: 'W. Taylor'
};

// Try to combine book and publisher objects with concat()
var combined = book.concat(publisher);

// Output result
console.log(combined);
```

Unfortunately, `Object.prototype.concat()` isn't a valid method, so a `X Is Not a Function TypeError` is thrown:

```
[EXPLICIT] TypeError: book.concat is not a function
```

In this case we can concatenate our `Objects` together by calling the `Object.assign()` method:

```js
// Book title and author
var book = {
    title: 'Robinson Crusoe',
    author: 'Daniel Defoe'
};
// Book publisher
var publisher = {
    name: 'W. Taylor'
};

// Combine book and publisher objects with Object.assign()
var combined = Object.assign(book, publisher);

// Output result
console.log(combined);
```

Sure enough this accomplished what we were after and outputs the expected result, which is similar to the `Array` concatenation functionality above:

```
Object {title: "Robinson Crusoe", author: "Daniel Defoe", name: "W. Taylor"}
```

Our final scenario where `X Is Not a Function TypeErrors` can commonly occur is when using any of the built-in methods that expect a provided callback function as an argument, but no function is given.  For example, here we want to use the `Array.prototype.every()` method to loop through every element of our array and check if it passes some logic.  However, in this case we've forgotten to provide the required callback function as the first argument (instead we just have a string):

```js
var printError = function (error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    // Declare a data array
    var data = [1, 2, 3, 4, 5];
    // Call Array.prototype.every() without provided callback function
    var result = data.every("My String");
    // Output result
    console.log(result);
} catch (e) {
    if (e instanceof TypeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

As you might imagine this results in a `X Is Not a Function TypeError` output:

```
[EXPLICIT] TypeError: My String is not a function
```

The correct way to use built-in methods like `Array.prototype.every()` is to provide a callback function as the first argument, which will execute once for every element in the array.  In this case, we're testing whether all elements are less than or equal to `10` -- if so, we return `true`:


```js
// Declare a data array
var data = [1, 2, 3, 4, 5];
// Call Array.prototype.every() with callback function
var result = data.every(function(element) {
    return element <= 10;
});
// Output result
console.log(`Result is: ${result}`);
```

This outputs our expected result:

```
Result is: true
```

To dive even deeper into understanding how your applications deal with JavaScript Errors, check out the revolutionary <a class="js-cta-utm" href="https://airbrake.io/languages/javascript_exception_handler?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-js">Airbrake JavaScript</a> error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

---

__META DESCRIPTION__

A closer look at the X Is Not a Function TypeError within JavaScript, including a handful of functional code examples.

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
