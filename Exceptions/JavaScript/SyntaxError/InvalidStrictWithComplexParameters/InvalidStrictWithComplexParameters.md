Delving deeper into the misty (and mysterious) mountains of __JavaScript Error Handling__, we've come upon the frightening and dangerous lair of the beast known as the `Invalid Strict with Complex Parameters` error!  [INSERT TERRIFIED SCREAMS].  The `Invalid Strict with Complex Parameters` error rears its ugly head anytime `strict mode` is enabled within a function that contains any sort of `complex parameter`.

Below we'll take a look at a few examples to show when `Invalid Strict with Complex Parameters` errors may appear, then take a deeper dive into dealing with such errors if they should pop up.  Let's get to it!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`] object, or an inherited object therein.
- The [`SyntaxError`] object is inherited from the [`Error`] object.
- The `Invalid Strict with Complex Parameters` error is a specific type of [`SyntaxError`] object.

## When Should You Use It?

Since the appearance of an `Invalid Strict with Complex Parameters` error in the first place indicates that we've enabled the use of `strict mode`, we should first take a few moments to examine what `strict mode` actually is.  In short, `strict mode` is a toggled directive that forces JavaScript to behave in a slightly altered manner, usually by opting into less secure limitations placed upon the code, and thereby opening up execution to more dangers and exploits.  While `strict mode` can lessen overall security, it can also be a requirement in certain coding situations, and in such cases, it's entirely possible to produce a `Invalid Strict with Complex Parameters` error.

The `Invalid Strict with Complex Parameters` error itself will appear when `strict mode` is enabled within a function that has one of the following `parameter types`:

- __Default Parameters__
- __Rest Parameters__
- __Destructuring Parameters__

A [`default parameter`] is when a parameter definition includes a default value, in the form of `paramName = defaultValue`.  For example, here we're defining a default value of `99` for the `age` parameter in our `addUser()` function:

```js
function addUser(name, age = 99) {
    console.log(`Name is: ${name}, aged: ${age}`);
}

addUser('Alice');
```

A [`rest parameter`] is the JavaScript term for what's also known as a [`variadic function`] parameter -- a parameter that represents an indefinite number of arguments.  In many languages, and JavaScript is no different, this variable number of parameters is represented by an ellipsis (`...`) prior to the variable name.  For example, here our `addUsers()` function accepts an indeterminate number of parameters, which we defined as the argument `names`, each representing the name of a new user to add:

```js
function addUsers(...names) {
    names.forEach(function(element) {
        console.log(`Name is: ${element}`);
    })
}

addUsers('Alice', 'Bob', 'Chris', 'David');
```

We can then pass any number of `names` to the calling of our `addUsers()` function.  As expected, these passed names will be automatically converted into an array of values, which are then output to the console as specified in our `addUsers()` function:

```
Name is: Alice
Name is: Bob
Name is: Chris
Name is: David
```

Lastly, the third type of parameter that can cause an `Invalid Strict with Complex Parameters` error is a [`destructuring parameter`], or `destructuring assignment`.  `Destructuring` is a syntax by which we can _extract_ data from arrays or other objects into distinct variables.  For example, rather than looping through each of our provided `names` from our last `addUsers()` example, maybe we just want to extract the first _two_ names in the array, and assign them to unique variables:

```js
function addUsers(...names) {
    var [first, second] = names;
    console.log(`First name is: ${first}`);
    console.log(`Second name is: ${second}`);
}

addUsers('Alice', 'Bob', 'Chris', 'David');
```

By utilizing the `[enclosing brackets]` syntax to indicate our `destructuring` variables, we are expecting to extract the first and second values in the `names` array, and assign them to the `first` and `second` variables, respectively.  Sure enough, the output matches:

```
First name is: Alice
Second name is: Bob
```

Now that we know what constitutes a type of `complex parameter` in JavaScript we can test it out by enabling `strict mode` and see how the engine reacts.  Let's try it with our first example above:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}


try {
    function addUser(name, age = 99) {
        'use strict';
        console.log(`Name is: ${name}, aged: ${age}`);
    }

    addUser('Alice');
} catch (e) {
    if (e instanceof SyntaxError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

While we've surrounded the important code with a bit of extra fluff to make it easier to catch any errors, as expected, simply by adding the `use strict` declaration to our `addUser()` function, which contains a `default parameter` specification, we produce an `Invalid Strict with Complex Parameters` error:

```
Uncaught SyntaxError: Illegal 'use strict' directive in function with non-simple parameter list
```

To dive even deeper into understanding how your applications deal with JavaScript Errors, check out the revolutionary [`Airbrake JavaScript`] error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

[`Airbrake JavaScript`]: https://airbrake.io/languages/javascript_exception_handler
[`Error`]: https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy
[`SyntaxError`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SyntaxError
[`Strict Mode`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Strict_mode
[`default parameter`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Functions/Default_parameters
[`rest parameter`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Functions/rest_parameters
[`destructuring parameter`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Operators/Destructuring_assignment
[`variadic function`]: https://en.wikipedia.org/wiki/Variadic_function

---

__META DESCRIPTION__

A detailed examination of the Invalid Strict with Complex Parameters SyntaxError in JavaScript.

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
