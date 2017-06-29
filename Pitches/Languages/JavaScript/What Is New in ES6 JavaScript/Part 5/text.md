# ES6 JavaScript: What's New? - Part 5

Today we continue our journey of exploring all the cool new features that ES6 JavaScript has to offer.  Thus far in this series we've covered quite a bit of ground:

- In [Part 1](https://airbrake.io/blog/javascript/es6-javascript-whats-new-1) we took a look at `default parameters`, `classes`, and `block-scoping` with the `let` keyword.
- In [Part 2](https://airbrake.io/blog/javascript/es6-javascript-whats-new-part-two) we explored `constants`, `destructuring`, and `constant literals syntax`.
- In [Part 3](https://airbrake.io/blog/javascript/es6-javascript-whats-new-part-3) we dove deep into just one major feature known as `template literals`.
- In [Part 4](https://airbrake.io/blog/javascript/es6-javascript-whats-new-part-4) we examined the world of `iterators` and `generators`.

For Part 5, today we'll be looking at `arrow functions` and `promises`.

## Arrow Functions

In the past, defining functions in JavaScript was a bit cumbersome compared to some other languages.  The new ES6 feature of `arrow function` syntax aims to reduce the amount of fluff required to define a function in many cases, often reducing the number of characters (or lines) required to create new functions.  We'll be using arrow functions throughout the next section of this article, so it's a good idea to cover them here to illustrate how they differ from normal function definitions of the past.

To illustrate let's start with an example right out of the gate.  Here we have two functions that perform the same operation, but using the standard and arrow syntax, respectively:

```js
// Old syntax.
function addNumbers(a, b) {
    return a + b;
}

// Arrow function syntax.
var addNumbers = (a, b) => a + b;
```

The first big change with arrow function syntax is we no longer need the `function` keyword.  Instead, the inclusion of `=>` indicates the separation of the argument list (`(a, b)` in this case) from the function code body (`a + b;`).  This tells JavaScript that this is a function definition without requiring `function` to be used anywhere in there.

You'll also notice we've done away with the curly braces (`{ ... }`).  This is allowed _only when_ the body of the function is a single line/expression.  If we were to extend our function body to two or more expressions we'd need to include the curly braces once again.

Additionally, you'll notice the above example also discards the `return` keyword.  Again, if our function body contains only one expression and we've omitted the curly braces, the `return` statement is _implied_ but not necessary to write in there.

However, if we want to expand our function body to two or more expressions we now have to add the curly braces and the return statement (if applicable) back in there:

```js
var addNumbers = (a, b) => {
    console.log('Adding some numbers.');
    return a + b;
}
```

That's about all there is to arrow function syntax, but it's a good idea to get used to the pattern since you'll often find it in modern JavaScript code bases (including later on in this very article)!

## Promises

Promises are often compared to `callbacks`, which is the commonly accepted term used to describe a coding convention of creating a function that returns a result which may take some time to process.  Such functions are often referred to as `asynchronous` (or `async` for short), indicating that these functions require additional time (beyond what it would take to call the function during execution) to return their result.

A "normal" function is one that returns a result immediately.  For example, performing a simple mathematical operation in JavaScript can be performed immediately and isn't considered a callback function:

```js
let total = addNumbers(1, 2);
console.log(total); // 3
```

On the other hand, a function which might require extra time to return a result, such as querying a database or performing some form of IO operation, are good candidates for callback functions:

```js
let user = getUserFromDatabase(12345);
console.log(user); // undefined
```

Making use of just a handful of callback functions in an application is usually no problem, but a common term in the realm of JS development is `callback hell`.  This loose term refers to code that contains callback upon callback upon callback, all of which are nested together in a tangle that's extremely difficult to write let alone debug.  For example, consider something simple like determining if a `User` is `registered` in the database.  The callback chain could contain half a dozen functions looking something like this:

```js
function isUserRegistered(id, callback) {
    openDatabase(function(db) {
        getCollection(db, 'users', function(col) {
            find(col, {'id': id}, function(result) {
                result.filter(function(user) {
                    callback(user.registered)
                })
            })
        })
    })
}
```

While smart developers will aim to keep their code as shallow as possible (that is, removing as much callback nesting as possible), it can be difficult to accomplish when business requirements demand certain functionality.

This is where the power of the ES6 `promise` feature comes into play.  A promise is _similar_ to a callback in that it allows for asynchronous function execution.  However, the big advantage promises have over callbacks is that a promise _represents a future result of an operation_.  A promise is actually a new type of JavaScript object -- conveniently called a `Promise` -- which itself has a number of unique capabilities and methods.  In particular, a promise is a _time-independent_ container wrapped around the value of a normal function.

A `Promise` object only has three possible states:

- `pending`: The initial state prior to execution.
- `fulfilled`: Indicates a successful operation.
- `rejected`: Indicates a failed operation.

Let's see how promises actually work with a simple code example.  Here we have a `Book` class with a `queryDatabase()` function that we can call to perform simple CRUD queries regarding a specific book object.  Critically, `queryDatabase()` actually returns a new `Promise` object, which we'll use later to determine the async results of our query attempts.  For this simple example the actual asynchronous operations are replaced with `setTimeout()` calls to generate delays, but in a real world application actual database calls would be placed here instead:

```js
class Book {
    constructor(title, author, pageCount, wordCount) {
        this.title = title;
        this.author = author;
        this.pageCount = pageCount;
        this.wordCount = wordCount;                
    }

    get wordsPerPage() {
        return this.wordCount / this.pageCount;
    }

    static wordsPerPage(pageCount, wordCount) {
        return wordCount / pageCount;
    }

    // Perform basic CRUD queries regarding this instance.
    queryDatabase(type) {
        type = type || 'CREATE';
        // Return a new Promise to be asynchronously handled elsewhere.
        return new Promise((resolve, reject) => {
            // Perform async task here.
            // Using setTimeout() to simulate an async call by delaying for 1 second.            
            if (type == 'CREATE') {
                setTimeout(() => {
                    resolve(`Creating [${this.title} by ${this.author}] in database.`);
                }, 1000);                
            } else if (type == 'READ') {
                setTimeout(() => {
                    resolve(`Reading [${this.title} by ${this.author}] from database.`);
                }, 1000);  
            } else if (type == 'UPDATE') {
                setTimeout(() => {
                    resolve(`Updating [${this.title} by ${this.author}] in database.`);
                }, 1000);  
            } else if (type == 'DESTROY') {
                setTimeout(() => {
                    reject(new Error(`Destruction of [${this.title} by ${this.author}] failed. Book not found.`));
                }, 1000);  
            }
        });
    }
}
```

To query our database let's start by creating a new `Book` instance and then we'll call the `queryDatabase()` method.  To simulate different database queries we pass different values to `queryDatabase()` such as `READ` or `UPDATE`:

```js
// Create a new Book instance.
let book = new Book("The Stand", "Stephen King", 823, 472376);

// Query the database for book.
// First READ the existing record.
// Once READ is successful then perform an UPDATE.
// Output the resolve messages for each call.
book.queryDatabase('READ').then(
    (message) => console.log(message)
);
```

The `.then()` method of a `Promise` object accepts up to two arguments: the callback functions representing a success or failure of the promise itself.  Thus, in the example above our call to `book.queryDatabase('READ')` returns a new `Promise` object, with which we immediately invoke the `.then()` method and pass it a single function argument:

```js
book.queryDatabase('READ').then(
    (message) => console.log(message)
    // Reading [The Stand by Stephen King] from database.
);
```

The first argument of `.then()` is the `fulfillment` operation, meaning the passed parameter of this first callback function is simply the `fulfillment` (or success) message of our invoked promise object.  As you'll recall, inside the `Book.queryDatabase()` method the returned `Promise` actually calls the special `resolve()` function when we're reading from the database, passing in a message specific to the `fulfillment` of the `Promise` object:

```js
queryDatabase(type) {
    type = type || 'CREATE';
    // Return a new Promise to be asynchronously handled elsewhere.
    return new Promise((resolve, reject) => {
        // ...
        } else if (type == 'READ') {
            setTimeout(() => {
                resolve(`Reading [${this.title} by ${this.author}] from database.`);
            }, 1000); 
        }
        // ...
    )};
}
```

Therefore, when we call `book.queryDatabase('READ').then(...)` the output we get is our `fulfillment` message from the underlying `Promise` object result, indicating we successfully queried the database and read the book record:

```js
book.queryDatabase('READ').then(
    (message) => console.log(message)
    // Reading [The Stand by Stephen King] from database.
);
```

Pretty cool, but promises are capable of quite a bit more.  Not only are promises executed independent of time (asynchronous), they are also independent (and unaware) of the code used to call them in the first place.  This means a promise can be called from anywhere, or even _chained_ onto one another.  For example, let's take our previous example above, but let's modify the function invoked in the first `.then()` call and add _another_ `Promise` call.  In this case we're just using `book.queryDatabase('UPDATE')` since we know that it returns a `Promise` object and it will be unique from the original promise when calling `book.queryDatabase('READ')`:

```js
// Create a new Book instance.
let book = new Book("The Stand", "Stephen King", 823, 472376);

// Query the database for book.
// First READ the existing record.
// Once READ is successful then perform an UPDATE.
// Output the resolve messages for each call.
book.queryDatabase('READ').then((message) => {
    console.log(message);
    return book.queryDatabase('UPDATE');
}).then(
    (message) => console.log(message)
);
```

Since the first `.then()` method call returned a second `Promise` object we're now able to _chain_ a second `.then()` method call onto our first one.  The arguments of the second `.this()` call will be invoked when the `book.queryDatabase('UPDATE')` promise is returned.  Once again, our `fulfillment` function spits out the success message to our console.  As a result, executing the above example shows both of the promises were created and executed, just as we intended:

```
Reading [The Stand by Stephen King] from database.
Updating [The Stand by Stephen King] in database.
```

So far our promises have all been successful, but often we'll run into situations where a database query or IO operation fails and we need to plan for this.  Thankfully, promises provide such functionality by making failure and error handling quite simple.

As we previously mentioned, the `.then()` method expects up to _two_ arguments to be passed: the first represents the `fulfillment` function and the second is the `rejection` function.  This means we can provide two different operations depending on whether our promise worked or not.  In this example we have a new `Book` instance and we're trying to remove it from the database.  Here we've provided two function arguments to `.then()`.  While they both look almost identical in this example, we can actually perform any sort of operations within these functions that we want, based on whether the promise succeeded or failed:

```js
// Create a new Book instance.
let book2 = new Book("The Hobbit", "J.R.R. Tolkien", 320, 95022);

// Try to destroy book database record.
book2.queryDatabase('DESTROY').then(
    // Output the resolve message, if applicable.
    (message) => console.log(message),
    // Catch any errors or failures and output that message instead.
    (errorMessage) => console.log(errorMessage)
);
```

Recall back to our `Book` class and the `.queryDatabase()` method definition.  Just to make this example more interesting, we've ensured that calling `.queryDatabase('DESTROY')` produces a `reject()` result.  In this rejection we create a new `Error()` object and indicate that our book couldn't be destroyed because it wasn't found:

```js
queryDatabase(type) {
    type = type || 'CREATE';
    // Return a new Promise to be asynchronously handled elsewhere.
    return new Promise((resolve, reject) => {
        // ...
        } else if (type == 'DESTROY') {
            setTimeout(() => {
                reject(new Error(`Destruction of [${this.title} by ${this.author}] failed. Book not found.`));
            }, 1000);  
        }
        // ...
    )};
}
```

Now, when we execute the `book2.queryDatabase('DESTROY')` example above the second (`rejection`) argument of the `.then()` method is invoked and the log output shows the error that we produced:

```
Error: Destruction of [The Hobbit by J.R.R. Tolkien] failed. Book not found.
```
### Promise.all()

The `Promise` API also provides a number of useful static methods that make working with promises easier.  One useful example is [`Promise.all()`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Promise/all), which accepts an iterable object (like an `Array`) that contains a list of promises.  `Promise.all()` returns a new `Promise` object with a `fulfillment` or `rejection` status indicating whether _all_ the provided promises were successful or not.  If one promise in the iterable is rejected the returned promise is a `rejection` and includes the reason it failed.

To see this in action let's first create another `Book` instance.  With our new `book3` object in hand we then call `Promise.all()` and pass a new `Array` of three elements, each of which is a new `Promise` object representing the result of calling `CREATE`, `READ`, and `UPDATE` queries on our database.  Remember, `Book.queryDatabase()` returns a `Promise` object itself, so this is equivalent to directly creating `new Promise()` instances within our `Array`:

```js
// Create a new Book instance.
let book3 = new Book("The Name of the Wind", "Patrick Rothfuss", 662, 250000);

// Ensure that CREATE, READ, and UPDATE are all successful.
Promise.all([
    book3.queryDatabase('CREATE'),
    book3.queryDatabase('READ'),
    book3.queryDatabase('UPDATE')
]).then(
    (message) => console.log(`CREATE, READ, and UPDATE succeeded for [${book3.title} by ${book3.author}].`),
    (errorMessage) => console.log(errorMessage)
);
```

Since `Promise.all()` returns its own `Promise` object we can call the `.then()` method and pass in the two standard arguments, the first for our `fulfillment` function and the second for the `rejection` function.  In this case, we want to confirm that we're able to perform all three queries.  Sure enough, executing the above code produces a successful `fulfillment` message in our log:

```
CREATE, READ, and UPDATE succeeded for [The Name of the Wind by Patrick Rothfuss].
```

Just to confirm `Promise.all()` produces a failing result if any of the provided promises fail we can also add `queryDatabase('DESTROY')` into the `Array` and see what happens:

```js
// Ensure that CREATE, READ, UPDATE, and DESTROY are all successful.
Promise.all([
    book3.queryDatabase('CREATE'),
    book3.queryDatabase('READ'),
    book3.queryDatabase('UPDATE'),
    book3.queryDatabase('DESTROY')
]).then(
    (message) => console.log(`CREATE, READ, UPDATE, and DESTROY succeeded for [${book3.title} by ${book3.author}].`),
    (errorMessage) => console.log(errorMessage)
);
```

Unsurprisingly, our `DESTROY` query still throws an `Error`, resulting in a `rejection` message in our log:

```
Error: Destruction of [The Name of the Wind by Patrick Rothfuss] failed. Book not found.
```

### Promise.race()

The last feature of promises we'll cover here is the [`Promise.race()`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Promise/race) method.  Similar to `Promise.all()`, the `Promise.race()` method accepts an iterable of `Promise` objects and invokes all of them together.  However, whereas `Promise.all()` returns `fulfillment` only if _all_ promises succeed, `Promise.race()` returns the result of the _first_ promise in the list to complete.

For example, here we've created another `Book` instance and are using `Promise.race()` to call all four of our `queryDatabase()` queries:

```js
// Create a new Book instance.
let book4 = new Book("Seveneves", "Neal Stephenson", 880, 272800);

// Check which operation is fulfilled (or rejected) first.
Promise.race([
    book4.queryDatabase('CREATE'),
    book4.queryDatabase('READ'),
    book4.queryDatabase('UPDATE'),
    book4.queryDatabase('DESTROY')
]).then(
    (message) => console.log(message),
    (errorMessage) => console.log(errorMessage)
);
```

Even though we know that `queryDatabase('DESTROY')` produces an `Error` (and therefore a `rejection` result), all that matters to `Promise.race()` is which promise is the _quickest_.  In this particular case our log shows that `READ` was the quickest:

```
Reading [Seveneves by Neal Stephenson] from database.
```

However, keen observers may have noticed that all of our "psuedo-callback" functions using `setTimeout()` used the same `delay` argument value of `1000` milliseconds.  Therefore, we'd expect that the promise returned by `Promise.race()` would be slightly randomized, which we can see somewhat in this example since our `READ` promise finished first, even though it's listed second.  This just illustrates the minor differences in execution when dealing with delayed callbacks/promises, since another execution of the same code results in `CREATE` as the "winner" of our race:

```
Creating [Seveneves by Neal Stephenson] in database.
```

To help you and your team with JavaScript development, particularly when dealing with unexpected errors, check out the revolutionary <a class="js-cta-utm" href="https://airbrake.io/languages/javascript_exception_handler?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-js">Airbrake JavaScript</a> error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

---

__META DESCRIPTION__

Part 5 of our journey through the exciting new features introduced in the latest version of JavaScript, ECMAScript 6 (ES6).

---

__SOURCES__

- https://github.com/getify/You-Dont-Know-JS/tree/master/es6%20%26%20beyond
- https://github.com/lukehoban/es6features
- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Template_literals
- https://developer.mozilla.org/en/docs/Web/JavaScript/Reference/Classes