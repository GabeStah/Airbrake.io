# ES6 JavaScript: What's New? - Part 4

Today we continue our journey of exploring all the cool new features that ES6 JavaScript has to offer.  This isn't the first part in this series by any means and thus far we've covered quite a bit of ground:

- In [Part 1](https://airbrake.io/blog/javascript/es6-javascript-whats-new-1) we took a look at `default parameters`, `classes`, and `block-scoping` with the `let` keyword.
- In [Part 2](https://airbrake.io/blog/javascript/es6-javascript-whats-new-part-two) we explored `constants`, `destructuring`, and `constant literals syntax`.
- In [Part 3](https://airbrake.io/blog/javascript/es6-javascript-whats-new-part-3) we dove deep into just one major feature known as `template literals`.

For Part 4 today we'll be looking at `iterators` and `generators`, so let's get to it!

## Iterators

The concept of `iterators` is not new to programming by any means.  Most modern languages offer an interface for defining an iterator, which is a pattern that specifies that an object can be _traversed_ in order to pull a single, consecutive item from a larger list.

In previous versions of JavaScript developers would be forced to create their own ad-hoc iterators.  Here's a basic iterator example from ES5 where we have a `doubler` object with a `next()` property that retrieves the next iteration (in this case, the doubled value of our number):

```js
// ES5 Iterator
var doubler = {
    // Create our own IIFE to retrieve next iteration.
    next: (function () {
        var value = 1;
        // IIFE returns a function with next value.
        return function () {
            value *= 2;
            return value;
        };
    })()
};

var v;
// Messy infinite loop if break doesn't occur.
while (true) {
    // Call next iteration function.
    v = doubler.next();
    if (v > 1024) {
        break;
    }
    console.log(v);
}
```

As expected, we get the doubled output from this every time until we reach our maximum of `1024`:

```
2
4
8
16
32
64
128
256
512
1024
```

Unfortunately, this old iterator pattern isn't the most user-friendly implementation.  For one thing we're using an infinite loop, which always spells trouble if we fail to hit our `break` statement.  We could get around that by performing our check inside the `next()` method call and if the iteration is complete return some value indicating as much (like `undefined`).

Regardless, the new iterator syntax introduced in ES6 aims to standardize these patterns.  Here we have the same `doubler` iterator as before:

```js
// ES6 Iterator
var doubler = {
    [Symbol.iterator]() {
        // State variables.
        var value = 1;

        return {
            // Make this iterable.
            [Symbol.iterator]() { return this; },
            // Automatically-called function when retrieving next iteration.
            next() {
                value *= 2;
                // Check if iterations are complete.
                if (value > 1024) {
                    return { done: true }
                }
                // Output value and non-complete instruction.
                return { value: value, done: false };
            }
        };
    }
};

// Loop through each iteration.
for (var v of doubler) {
    console.log(v);
}
```

ES6 iterators require a `next()` method which is used to retrieve the next `IteratorResult`.  The `IteratorResult` should be an object with two properties: `value` and `done`.  `value` can be any value (or omitted if `done` is `true`).  `done` is a boolean indicating whether the iterator has completed its sequence.

Therefore, you can see that our iterator example returns `{ value: value, done: false }` for most calls of `next()`, but when we reach the end of our sequence -- when `value` is greater than `1024` -- we return `{ done: true }`.  This results in the exact same output as our previous example since we're iterating through our `doubler` via a `for (var...of)` loop:

```
2
4
8
16
32
64
128
256
512
1024
```

## Generators and Yield

While `iterators` require that execution be completed all at once, and therefore looping through the full set of iterations before any additional action can be taken, ES6 also introduces a new tool to get around this limitation called `generators`.  A generator is a special kind of function that allows execution to be paused at any time, only to be resumed later on.  Generators take a lot of functionality from iterators (in fact, underneath each generator is an iterator created by the JavaScript engine).

The syntax for a generator is the same as a normal function _except_ an asterisk (`*`) must precede the function name: `function *myGenerator() { }`

The special sauce of the generator largely comes from the new `yield` keyword that it also introduces.  You may be familiar with `yield` from other programming languages, but the basic idea is that the `yield` statement tells the executing code that it should temporarily pause at that location, stepping out of the current function scope for the time being, while remembering that location for later resumption.

For example, here we have a simple `*myGenerator()` function that outputs a `before` message, then issues a `yield` call, before finally outputting an `after` message:

```js
function *myGenerator() {
    console.log("before");
    yield;
    console.log("after");
}

var myGenerator = myGenerator();

// Call next iteration, executing until end (or until `yield` is found).
myGenerator.next();
console.log("Doing other, out of scope stuff...");
myGenerator.next();
```

As discussed, since a generator is merely a wrapper for an iterator, we assign it to a variable and call it via the `next()` method.  In our example above, our first `next()` call outputs the `before` message, then we're free to execute any other code we want since the inner-execution of our `*myGenerator()` function was halted when it encountered the first `yield` statement.  Finally we make another call to `next()` to produce our post-yield `after` message:

```
before
Doing other, out of scope stuff...
after
```

In addition to acting as a pause point within the generator function, the `yield` statement can also be used to return a `value`, which is merely substituted into the `value` property of our `IteratorResult` object of our wrapped iterator.  Let's modify our above example to `yield` the value `50` and see how output changes:

```js
function *myGenerator() {
    console.log("Pre-yield!");
    yield 50;
    console.log("Post-yield!");
}

var myGenerator = myGenerator();

console.log(myGenerator.next().value);
console.log("Doing other, out of scope stuff...");
myGenerator.next().value;
```

Notice that, in addition to adding our output `value` of `50` after our `yield` statement, we also want to retrieve (and output) the `value` that we get from our first `next()` iteration call, which will contain the `yielded` value in our output:

```
Pre-yield!
50
Doing other, out of scope stuff...
Post-yield
```

Things really start to get cool when we stick `yield` inside a loop.  This allows us to easily generate a pseudo-infinite iterator that can be called anywhere and at anytime to grab the next value that our generator spits out.  Let's modify our previous `doubler` iterator to be a generator instead that can be called over and over to give us the next doubled output:

```js
// ES6 Generator
function *doubler() {
    var value = 1;
    while (true) {
        value *= 2
        yield value;
    }
}

var doubler = doubler();

console.log(doubler.next().value);
console.log(doubler.next().value);
console.log(doubler.next().value);
console.log(doubler.next().value);
console.log(doubler.next().value);
console.log(doubler.next().value);
```

As you can see, we can call `doubler.next()` as many times we want, wherever we want, and we'll generate and return the next iteration.  Calling it six times, as seen above, produces the expected output result:

```
2
4
8
16
32
64
```

Just as an asterisk (`*`) prior to our function names converts a normal function into an iterable generator, we can also precede a `yield` value with an asterisk, so long as the value is itself an iterable.  This will cause the parent generator which houses the `yield` to use the underlying iterator associated with the yielded value to retrieve (and return) each iteration.

As an example, let's take our infinite `*doubler()` generator from before, which you'll recall is an iterator unto itself, and call make a generator call to it via `yield` inside a new `*useDoubler()` generator:

```js
function *doubler() {
    var value = 1;
    while (true) {
        value *= 2
        yield value;
    }
}

function *useDoubler() {
    yield *doubler();
}
```

Now we can assign our `*useDoubler()` generator just as we did before and make however many `next()` calls we wish.  This will cause the `yield` to call the `next()` method of the underlying iterator it is associated with, which is `*doubler()` in this case:

```js
var useDoubler = useDoubler();

console.log(useDoubler.next().value);
console.log(useDoubler.next().value);
console.log(useDoubler.next().value);
console.log(useDoubler.next().value);
console.log(useDoubler.next().value);
console.log(useDoubler.next().value);
```

The returned value propagates back up through the chain so we get the same output result as before:

```
2
4
8
16
32
64
```

To help you and your team with JavaScript development, particularly when dealing with unexpected errors, check out the revolutionary <a class="js-cta-utm" href="https://airbrake.io/languages/javascript_exception_handler?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-js">Airbrake JavaScript</a> error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

---

__META DESCRIPTION__

Part 4 of our journey through the exciting new features introduced in the latest version of JavaScript, ECMAScript 6 (ES6).

---

__SOURCES__

- https://github.com/getify/You-Dont-Know-JS/tree/master/es6%20%26%20beyond
- https://github.com/lukehoban/es6features
- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Template_literals
- https://developer.mozilla.org/en/docs/Web/JavaScript/Reference/Classes