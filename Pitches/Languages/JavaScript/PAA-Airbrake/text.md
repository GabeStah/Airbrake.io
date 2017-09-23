# JavaScript Async/Await Exception Handling with Airbrake!

Introduced in ES6, [`promises`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Promise) are the big leap forward into asynchronous operations that JavaScript has needed for some time.  However, in ES7 (or `ESNext`, as the upcoming release is sometimes referred to), promises were dramatically improved with the introduction of [`async`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Statements/async_function) functions and the [`await`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Operators/await) operator.  In short, an `async` function simply defines and returns an asynchronous function, while the `await` operator is used to wait for a [`Promise`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Promise) object.  

The secret sauce when using `async` and `await` together is that they effectively allow code to be written _synchronously_, while their behavior is still carried out behind the scenes as an asynchronous action.  This is an extremely powerful feature, but the most noticeable effect when implementing async/await into your code is just how **simple** and easy to maintain the new syntax becomes.

## Full Code Sample

Below is the full code sample we'll be using in this example.  Feel free to copy and paste it into your own project to try it out or follow along:

```js
class Book {
    get author() {
        return this.get('_author');
    }

    set author(value) {
        this.set('_author', value);
    }

    get pageCount() {
        return this.get('_pageCount');
    }

    set pageCount(value) {
        this.set('_pageCount', value);
    }

    get title() {
        return this.get('_title');
    }

    set title(value) {
        this.set('_title', value);
    }
    constructor(title, author, pageCount) {
        this.author = author;
        this.pageCount = pageCount;
        this.title = title;
    }

    /**
     * General getter.
     *
     * @param property
     * @returns {*}
     */
    get(property) {
        return this[property];
    }

    /**
     * General setter.
     *
     * Used to simulate different property set results,
     * depending on property that is modified and existing values.
     *
     * @param property
     * @param value
     */
    set(property, value) {
        return new Promise((resolve, reject) => {
            if (property === '_title') {
                // Simulate IO with 1 second delay.
                setTimeout(() => {
                    let previousValue = this[property];
                    this[property] = value;
                    // Set title to new value no matter what.
                    if (typeof previousValue === 'undefined') {
                        resolve(`Updated Title to ${this.title}.`);
                    } else {
                        resolve(`Updated Title from '${previousValue}' to '${this.title}'.`);
                    }
                }, 1000);
            } else if (property === '_author') {
                // Simulate IO with 1 second delay.
                setTimeout(() => {
                    let previousValue = this[property];
                    // Set author to new value, if no author property is defined.
                    if (typeof previousValue === 'undefined') {
                        this[property] = value;
                        resolve(`Updated Title to ${this.title}.`);
                    } else {
                        // If author is already defined, reject update
                        // and throw new Error.
                        reject(new Error(`Cannot update Author from ${previousValue} to ${value}.`));
                    }
                }, 1000);
            }
        });
    }

    /**
     * Output Book to formatted string.
     *
     * @returns {string}
     */
    toString() {
        return `'${this.title}' by ${this.author} (${this.pageCount} pgs)`;
    }
}
```

```js
// main.js
require(['airbrakeJs/client'], function (AirbrakeClient) {
    let airbrake = new AirbrakeClient({
        projectId: 157536,
        projectKey: 'e6b2c1bd63c0c26ab5751a7ce89d2757'
    });

    testPromise();
    testPromiseWithAirbrake(airbrake);
    testAsyncAwait();
    testAsyncAwaitWithAirbrake(airbrake);
});

const testPromise = () => {
    // Create new Book instance.
    let book = new Book('The Stand', 'Stephen King', 1153);
    // Output current title.
    console.log(book.title);
    // Set title new using promises.
    book.set('_title', 'Promise Title').then(
        // Handle resolve message.
        (message) => console.log(message)
    );
    // Set new author using promises.
    book.set('_author', 'Promise Author').then(
        // Handle resolve message.
        (message) => console.log(message),
        // Handle reject message.
        (err) => console.log(err)
    );
};

const testPromiseWithAirbrake = (airbrake) => {
    // Create new Book instance.
    let book = new Book('The Stand', 'Stephen King', 1153);
    // Output current title.
    console.log(book.title);
    // Set title new using promises.
    book.set('_title', 'Promise w/ Airbrake Title').then(
        // Handle resolve message.
        (message) => console.log(message)
    );
    // Set new author using promises.
    book.set('_author', 'Promise w/ Airbrake Author').then(
        // Handle resolve message.
        (message) => console.log(message),
        // Handle reject message.
        (err) => {
            // Handle error with Airbrake.
            let promise = airbrake.notify(err);
            promise.then(
                (notice) => console.log('Airbrake Notice Id:', notice.id),
                (noticeError) => console.log('Airbrake Notification Failed:', noticeError)
            );
        }
    );
};

const testAsyncAwait = async () => {
    // With Async/Await, can use inline try-catch block.
    try {
        let book = new Book('The Stand', 'Stephen King', 1153);
        await book.set('_title', 'Await Title');
        await book.set('_author', 'Await Author');
    } catch (err) {
        console.log(err);
    }
};

const testAsyncAwaitWithAirbrake = async (airbrake) => {
    try {
        let book = new Book('The Stand', 'Stephen King', 1153);
        await book.set('_title', 'Await w/ Airbrake Title')
        await book.set('_author', 'Await w/ Airbrake Author');
    } catch (err) {
        // Handle error with Airbrake, by awaiting promise from notify.
        await airbrake.notify(err).then(
            (notice) => console.log('Airbrake Notice Id:', notice.id)
        );
    }
};
```

```js
// app.js
requirejs.config({
    baseUrl: 'lib',
    paths: {
        app: '../app',
        airbrakeJs: 'node_modules/airbrake-js/dist',
        book: 'book'
    }
});

requirejs(['app/main']);
```

```html
<!DOCTYPE html>
<!-- index.html -->
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Promise, Async, and Await with Airbrake</title>
    <script data-main="app" src="lib/require.js"></script>
</head>
<body>

</body>
</html>
```

## The Setup

For our example code we'll be using the basic `Book` class:

```js
class Book {
    get author() {
        return this.get('_author');
    }

    set author(value) {
        this.set('_author', value);
    }

    get pageCount() {
        return this.get('_pageCount');
    }

    set pageCount(value) {
        this.set('_pageCount', value);
    }

    get title() {
        return this.get('_title');
    }

    set title(value) {
        this.set('_title', value);
    }
    constructor(title, author, pageCount) {
        this.author = author;
        this.pageCount = pageCount;
        this.title = title;
    }

    /**
     * General getter.
     *
     * @param property
     * @returns {*}
     */
    get(property) {
        return this[property];
    }

    /**
     * General setter.
     *
     * Used to simulate different property set results,
     * depending on property that is modified and existing values.
     *
     * @param property
     * @param value
     */
    set(property, value) {
        return new Promise((resolve, reject) => {
            if (property === '_title') {
                // Simulate IO with 1 second delay.
                setTimeout(() => {
                    let previousValue = this[property];
                    this[property] = value;
                    // Set title to new value no matter what.
                    if (typeof previousValue === 'undefined') {
                        resolve(`Updated Title to ${this.title}.`);
                    } else {
                        resolve(`Updated Title from '${previousValue}' to '${this.title}'.`);
                    }
                }, 1000);
            } else if (property === '_author') {
                // Simulate IO with 1 second delay.
                setTimeout(() => {
                    let previousValue = this[property];
                    // Set author to new value, if no author property is defined.
                    if (typeof previousValue === 'undefined') {
                        this[property] = value;
                        resolve(`Updated Title to ${this.title}.`);
                    } else {
                        // If author is already defined, reject update
                        // and throw new Error.
                        reject(new Error(`Cannot update Author from ${previousValue} to ${value}.`));
                    }
                }, 1000);
            }
        });
    }

    /**
     * Output Book to formatted string.
     *
     * @returns {string}
     */
    toString() {
        return `'${this.title}' by ${this.author} (${this.pageCount} pgs)`;
    }
}
```

It has a handful of properties with explicit getters and setters (using the `get` and `set` keywords), but otherwise, the only special logic is the specific `set(property, value)` and `get(property)` methods.  These are generalized methods which make it easier to create `Promises` for our example.  In particular, notice that the `set(property, value)` method returns a `new Promise(...)` object.  To simulate an asynchronous action we're explicitly calling `setTimeout()` and delaying by one second.  Since we're returning a `Promise`, we call the `resolve(...)` method to indicate a successful call, while we invoke `reject(...)` when the call has failed.  In this case, `reject(...)` is called only when attempting to update the `author` property _while an `author` value already exists_.  This behavior could simulate business logic that requires rejecting a data record update when a value already exists, but, since its an IO action, it must be performed asynchronously.

## Using Promises

We'll begin with the most basic implementation of asynchronous operations, which is a plain old `Promise` object.  As we saw above, the `Book.set(property, value)` method returns a `Promise` object, so our `testPromise()` function below creates a new `Book` instance, sets the `title` using the `set(property, value)` method, then invokes the `.then` method of the returned `Promise` object, which is called once the `Promise` comes back with a result:

```js
const testPromise = () => {
    // Create new Book instance.
    let book = new Book('The Stand', 'Stephen King', 1153);
    // Output current title.
    console.log(book.title);
    // Set title new using promises.
    book.set('_title', 'Promise Title').then(
        // Handle resolve message.
        (message) => console.log(message)
    );
    // Set new author using promises.
    book.set('_author', 'Promise Author').then(
        // Handle resolve message.
        (message) => console.log(message),
        // Handle reject message.
        (err) => console.log(err)
    );
};
```

Since setting the `title` property always succeeds, that will work and output the `Promise` result to the log.  However, the attempt to set a new `author` value will fail, since an author already exists.  This is confirmed by the log output, which occurs after the artificial one second delay:

```
Updated Title from 'The Stand' to 'Promise Title'.
Error: Cannot update Author from Stephen King to Promise Author.
```

## Using Promises with Airbrake

Let's integrate our `Promise` asynchronous methodology with the [`Airbrake-JS`](https://airbrake.io/languages/javascript_exception_handler) module.  We won't go over the setup process of `Airbrake-JS` here, but be sure to check out the [`official documentation`](https://airbrake.io/docs/installing-airbrake/installing-airbrake-in-a-javascript-application/) for information on installing the library.

Once `Airbrake-JS` is installed and we've created a new project on the `Airbrake` dashboard, we can include the library however is easiest.  For this example, we're using [`requirejs`](http://requirejs.org/), so we start by defining the location of our app within the `app.js` file, along with the location of the `Airbrake-JS` module:

```js
// app.js
requirejs.config({
    baseUrl: 'lib',
    paths: {
        app: '../app',
        airbrakeJs: 'node_modules/airbrake-js/dist',
        book: 'book'
    }
});

requirejs(['app/main']);
```

Now, within our main application code we require `Airbrake-JS`, and then we can use the Airbrake client object to set our `projectId` and `projectKey`, both of which are found on the Airbrake dashboard:

```js
// main.js
require(['airbrakeJs/client'], function (AirbrakeClient) {
    let airbrake = new AirbrakeClient({
        projectId: YOUR_PROJECT_ID,
        projectKey: 'YOUR_PROJECT_KEY'
    });

    // ...

    testPromiseWithAirbrake(airbrake);

    // ...
});
```

As you can see, we've opted to pass the Airbrake client object as a parameter to the `testPromiseWithAirbrake(airbrake)` function, but we could just as easily instantiate it inside the function if we wish.  Here we see the code of the aforementioned function:

```js
const testPromiseWithAirbrake = (airbrake) => {
    // Create new Book instance.
    let book = new Book('The Stand', 'Stephen King', 1153);
    // Output current title.
    console.log(book.title);
    // Set title new using promises.
    book.set('_title', 'Promise w/ Airbrake Title').then(
        // Handle resolve message.
        (message) => console.log(message)
    );
    // Set new author using promises.
    book.set('_author', 'Promise w/ Airbrake Author').then(
        // Handle resolve message.
        (message) => console.log(message),
        // Handle reject message.
        (err) => {
            // Handle error with Airbrake.
            let promise = airbrake.notify(err);
            promise.then(
                (notice) => console.log('Airbrake Notice Id:', notice.id),
                (noticeError) => console.log('Airbrake Notification Failed:', noticeError)
            );
        }
    );
};
```

In may look a bit complicated, but very little is different from our previous `testPromise()` function.  All we've added is the call to `airbrake.notify(err)`.  Since the `notify(...)` method returns a `Promise` object itself, we can then call the `then(...)` method on that object to perform various actions depending if the `Promise` succeeded or failed.

Executing the function above produces the following output, indicating that the attempt to set a new `author` failed again, but this time we caught the error and processed it via Airbrake, which generated a new notification on the Airbrake dashboard:

```
Updated Title from 'The Stand' to 'Promise w/ Airbrake Title'.
Airbrake Notice Id: 000559d3-17a9-f4db-5760-7b1f87af2312
```

Sure enough, opening the Airbrake dashboard and looking at the `Errors` panel shows a new error report, with an `Error Message` of `"Cannot update Author from Stephen King to Await w/ Airbrake Author."`.  Neat!

## Using Async/Await

Now, let's see what happens if we update our original `testPromise()` function to use `async/await` functionality.  Here's the `testAsyncAwait()` function to do just that:

```js
const testAsyncAwait = async () => {
    // With Async/Await, can use inline try-catch block.
    try {
        let book = new Book('The Stand', 'Stephen King', 1153);
        await book.set('_title', 'Await Title');
        await book.set('_author', 'Await Author');
    } catch (err) {
        console.log(err);
    }
};
```

What should immediately jump out at you is just _how few_ lines of code there are.  This performs the same logic as the `testPromise()` function, but requires nearly half the number of characters and lines of code.  Moreover, there's no more need to mess with the complication of `then(...)` method callbacks.  Instead, `await` handles that for us, by waiting for the result of the `awaited` `Promise` to move on, synchronously, to the next line within the same `async` function.

The result of executing this function is just as we saw before, where the attempt to overwrite the `author` property fails:

```
Error: Cannot update Author from Stephen King to Await Author.
```

## Using Async/Await with Airbrake

Finally, let's pull it all together and see how to combine the advanced `async/await` methodology with the simplicity of handling errors and exceptions via Airbrake.  This is accomplished within the `testAsyncAwaitWithAirbrake(airbrake)` method:

```js
const testAsyncAwaitWithAirbrake = async (airbrake) => {
    try {
        let book = new Book('The Stand', 'Stephen King', 1153);
        await book.set('_title', 'Await w/ Airbrake Title')
        await book.set('_author', 'Await w/ Airbrake Author');
    } catch (err) {
        // Handle error with Airbrake, by awaiting promise from notify.
        await airbrake.notify(err).then(
            (notice) => console.log('Airbrake Notice Id:', notice.id)
        );
    }
};
```

Again, we've managed to cut down the lines/characters by half from the previous `Promise-based` example that integrated with Airbrake.  Best of all, since the Airbrake client's `notify(...)` method returns a `Promise`, we can simply prefix that call with the `await` operator to halt execution at that point while waiting for the result.  While we've then chosen to immediately invoke the `then(...)` method of the returned `Promise` object in this case, we could continue integrating `async/await` all the way down the chain, to completely get rid of `Promise.then(...)` method calls.  For this case, however, `then(...)` does do anything that requires delayed processing/IO, so we can execute it immediately, once `await` comes back.

The result of executing this code is just as we saw before:

```
Airbrake Notice Id: 000559d3-3bff-5c6e-2760-135cc8132852
```

And, just as before, a new error corresponding with this generated notification appears on the Airbrake dashboard, with the `Error Message` of `"Cannot update Author from Stephen King to Await w/ Airbrake Author."`

That's just a small taste of the power of the [`Airbrake-JS`](https://airbrake.io/languages/javascript_exception_handler) module and the newest `async/await` methodology introduced in the upcoming JavaScript release!

---

__META DESCRIPTION__

An overview of using JavaScript Async and Await with Airbrake exception handling, including code samples showing the improvements async/await provide.

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Promise
- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Statements/async_function
- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Operators/await
- https://hackernoon.com/6-reasons-why-javascripts-async-await-blows-promises-away-tutorial-c7ec10518dd9?gi=b46bf37a27ef
