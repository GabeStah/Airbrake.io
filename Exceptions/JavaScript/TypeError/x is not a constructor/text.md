# JavaScript Errors - "x" is Not a Constructor TypeError

Today, as we continue along through our [__JavaScript Error Handling__](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) series, we'll be taking a closer look at the `"x" is Not a Constructor TypeError`.  As the name suggests a `"x" is Not a Constructor TypeError` is thrown when incorrectly trying to invoke the `constructor` of a variable or object that _doesn't_ actually have a `constructor` itself.

In this article we'll examine the `"x" is Not a Constructor TypeError` in more detail by looking at where it resides within the JavaScript `Exception` hierarchy.  We'll also use a few simple code examples to illustrate how `"x" is Not a Constructor TypeErrors` are commonly thrown, which will hopefully help you in avoiding them in your own coding adventures.  Let's get started!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object, or an inherited object therein.
- The [`TypeError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/TypeError) object is inherited from the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object.
- The `"x" is Not a Constructor TypeError` is a descendant of [`TypeError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/TypeError) object.

## When Should You Use It?

To dig into the `"x" is Not a Constructor TypeError` we should first refresh ourselves on how `constructors` work in JavaScript.  A `constructor` is merely a special method that is automatically added to every `Object` (or derived type therein) that, when called, actually performs the instantiation and overall creation of the object in question.  In most cases the `constructor` is not called explicitly, but is instead implicitly invoked by using the `new` keyword (e.g. `var house = new House()`).

Moreover, `constructors` can be explicitly set when creating a `class`, or they can be implicitly added by JavaScript when defining a basic `function`.  For example, here we've defined a simple `Book` class that contains the special `constructor()` method in which we expect two parameters used to set the `author` and `title` properties.  For illustration purposes we also output a message after construction to indicate how this `Book` instance was created:

```js
// Book as Class
class Book {
    // Special constructor method
    constructor(title, author) {
        // Set properties
        this.title = title;
        this.author = author;
        // Output creation message
        console.log(`'Created Book via class: ${title} by ${author}'`)
    }
}
```

Now let's try creating some instances of our `Book` class.  First we'll call it using the standard `constructor` method (via the `new` keyword):

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    // New keyword to access constructor
    var bookConstructed = new Book('The Stand', 'Stephen King')
    console.log(bookConstructed)           
} catch (e) {
    if (e instanceof TypeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

As expected, this behaves normally and outputs our `constructor` message plus the `Book` instance object:

```
Created Book via class: The Stand by Stephen King
Book {title: "The Stand", author: "Stephen King"}
```

Now let's drop the `new` keyword and try calling the `Book` class directly:

```js
// Direct class call
var book = Book('The Stand', 'Stephen King')
console.log(book) 
```

Here we actually get a `TypeError` because `new` is required to invoke our `constructor`:

```
[EXPLICIT] TypeError: Class constructor Book cannot be invoked without 'new'
```

What many people don't consider is that `functions` also have `constructors`.  Here we recreated our `Book` class as a function:

```js
// Book as Function
function Book(title, author) {
    // Set properties
    this.title = title;
    this.author = author;
    // Output creation message
    console.log(`'Created Book via function: ${title} by ${author}'`)
}
```

Let's try the same thing as before.  First we'll call the `constructor` using the `new` keyword:

```js
try {
    // New keyword to access constructor
    var bookConstructed = new Book('The Hobbit', 'J.R.R. Tolkien')
    console.log(bookConstructed)          
} catch (e) {
    if (e instanceof TypeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

The output looks very similar to before, but our console message confirms that we've called the `constructor` of our function version of `Book`:

```
Created Book via function: The Hobbit by J.R.R. Tolkien
Book {title: "The Hobbit", author: "J.R.R. Tolkien"}
```

Since this is a function we can also call it _without_ the `new` keyword, so let's try that and see what happens:

```js
// Direct function call
var book = Book('The Hobbit', 'J.R.R. Tolkien')
console.log(book)     
```

There are no errors thrown here because, as with all functions, our `Book` function can be invoked without calling its `constructor`.  In this case the output shows what's going on -- since our `Book` function doesn't `return` anything our `book` variable remains `undefined`:

```
Created Book via function: The Hobbit by J.R.R. Tolkien
undefined
```

Now that we're a bit clearer on how JavaScript handles `constructors` we can dive back into the `"x" is Not a Constructor TypeError` to see what might cause it to be thrown.  In the simplest sense, a `"x" is Not a Constructor TypeError` is thrown when attempting to call a `constructor` on an object type that doesn't possess a `constructor` in the first place.  For example, here we've declared a `String` type named `title` and tried to invoke the `constructor` of our `String` type variable via the `new` keyword:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    // Create a string variable
    var title = 'Fifty Shades of Grey';
    // Invoke the constructor of title string variable
    console.log(new title());
} catch (e) {
    if (e instanceof TypeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

Sure enough a `"x" is Not a Constructor TypeError` is thrown because `title()` isn't a `constructor`:

```
[EXPLICIT] TypeError: title is not a constructor
```

To dive even deeper into understanding how your applications deal with JavaScript Errors, check out the revolutionary <a class="js-cta-utm" href="https://airbrake.io/languages/javascript_exception_handler?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-js">Airbrake JavaScript</a> error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

---

__META DESCRIPTION__

A deep dive into the "x" is not a constructor TypeError within JavaScript, including a quick overview of constructors with simple code examples.

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
