# ES6 JavaScript: What's New? - Part 2

In [Part 1](https://airbrake.io/blog/javascript/es6-javascript-whats-new-1) of our `ES6 JavaScript` series we tackled the strange naming conventions that ECMAScript is finally settling on, then took a look at new features within JavaScript like `default parameters`, `classes`, and `block-scoping` with the `let` keyword.  In Part 2 of this series we'll check out a handful of other new features introduced in `ES6` including: `constants`, `destructuring`, and `constant literals syntax`.  Let's get to it!

## Constants

Many other programming languages have the concept of a `constant`, which is simply an immutable (unchangeable) variable.  Now, with `ES6`, JavaScript also has access to constants through the use of the `const` keyword.

In this first snippet we're defining a new constant to represent [`Euler's number`](https://en.wikipedia.org/wiki/E_(mathematical_constant)), which we've rounded to `2.71828` for our purposes:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    // Define constant for Euler's number.
    const e = 2.71828;
    console.log(e);
    // Attempt reassignment.
    e = 2.7
} catch (e) {
    if (e instanceof TypeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

Defining a new `const` as `e` with the specified value works fine and we output the value to the log, but once we attempt to reassign the value JavaScript throws a `TypeError`, informing us that we're attempting to make an assignment to a constant variable:

```
2.7182818284
[EXPLICIT] TypeError: Assignment to constant variable.
```

What's important to note here is that constants can only be _assigned_ a value at declaration, but once declared, the value they reference can never be altered.  This doesn't mean the _value itself_ cannot be changed, only the _assignment to that referenced variable_.  While the value and the reference to that value are one and the same for simple values like `2.71828` above, we can also declare more complex objects to a constant, such as an `Array`:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    // Define constant array.
    const names = ['Alice', 'Bob', 'Cindy'];
    console.log(names);
    // Change first array value.
    names[0] = 'Andrew';
    console.log(names);
    // Attempt reassignment of constant.
    names = ['Dave', 'Elizabeth', 'Fred'];
} catch (e) {
    if (e instanceof TypeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}   
```

Here we're declaring our `names` constant as a simple array and outputting the values to the log.  We then alter the first value by changing it from `Alice` to `Andrew` then, once again, output the values and we see that the change was made without incident.  However, when we try to reassign the _reference_ of our `names` constant to another array,this throws another `TypeError` just as before:

```
["Alice", "Bob", "Cindy"]
["Andrew", "Bob", "Cindy"]
[EXPLICIT] TypeError: Assignment to constant variable.
```

Since JavaScript is only concerned that the _reference_ of our constant remain the same (to the originally declared array in this case), we can freely make internal changes to the data of that array without issue.  But once we try to reassign the constant's reference to something else, we have a problem.

## Destructuring

`ES6` also introduces a new syntax feature called `destructuring`.  To understand what this means, let's first look at how a previous form of structured assignment would have been accomplished.

Here we have two simple functions, `getBookArray` and `getBookObject`, which both return a value type indicated by their name, representing the book _The Hobbit_ by J.R.R. Tolkien:

```js
function getBookArray() {
    return [
        'The Hobbit', 
        'J. R. R. Tolkien', 
        320
    ];
}

function getBookObject() {
    return {
        title: 'The Hobbit', 
        author: 'J. R. R. Tolkien', 
        page_count: 320
    };
}
```

Prior to `ES6`, if we wanted to assign variables to the underlying values returned by the array/object of those functions, we'd have to create a temporary variable, assign that variable to the result of the function in question, then declare our individual variables to grab the values from the new temporary object.  It might look something like this:

```js
// Declare temporary book array and assign values.
var tempBookArray = getBookArray(), title = tempBookArray[0], author = tempBookArray[1], page_count = tempBookArray[2];
console.log(`${title} by ${author} [${page_count} pages]`);

// Declare temporary book object and assign values.
var tempBookObject = getBookObject(), title = tempBookObject.title, author = tempBookObject.author, page_count = tempBookObject.page_count;
console.log(`${title} by ${author} [${page_count} pages]`);
```

That's a lot of code just to pull out the structured assignments that we want from our objects.  Thankfully, `ES6` introduces `destructuring`, which is simply syntactic sugar to perform the above steps with much less code:

```js
function getBookArray() {
    return [
        'The Hobbit', 
        'J. R. R. Tolkien', 
        320
    ];
}  
        
var [ title, author, page_count ] = getBookArray();
console.log(`${title} by ${author} [${page_count} pages]`);

function getBookObject() {
    return {
        title: 'The Hobbit', 
        author: 'J. R. R. Tolkien', 
        page_count: 320
    };
}

var { title, author, page_count } = getBookObject();
console.log(`${title} by ${author} [${page_count} pages]`);
```

Even though our functions are identical to before, the structured assignment to variables (of `title`, `author`, and `page_count`) is performed with the new `destructured` syntax.  The syntax is even smart enough, in the case of the object, to assume that the underlying properties are the same name as the new value assignments we're creating, which is quite handy.  The result is the assignment and output we expect, with far less code:

```
The Hobbit by J. R. R. Tolkien [320 pages]
The Hobbit by J. R. R. Tolkien [320 pages]
```

Arrays are one thing, but what if we want to assign our object values to variables with names that _don't_ match the underlying object properties?  We can still use similar `destructuring` syntax, while also adding the `:` separator:

```js
function getBookObject() {
    return {
        title: 'The Hobbit', 
        author: 'J. R. R. Tolkien', 
        page_count: 320
    };
}

var { title: book_title, author: book_author, page_count: book_page_count } = getBookObject();
console.log(`${book_title} by ${book_author} [${book_page_count} pages]`);
```

Here we want our newly declared outer scope variables to be prefixed with `book_`, so we use the `:` separator syntax to accomplish this.  The result still works as intended:

```
The Hobbit by J. R. R. Tolkien [320 pages]
```

We can also combine the `default value` syntax we explored in [Part 1](https://airbrake.io/blog/javascript/es6-javascript-whats-new-1) of this series with the new `destructuring` syntax.  Here we're using the same `getBookObject` function with three returned properties (`title`, `author`, and `page_count`), but for our `destructured` assignment we want to include the `language` variable as well.  If it doesn't exist in our object, we've specified the default value of `English`:

```js
function getBookObject() {
    return {
        title: 'The Hobbit', 
        author: 'J. R. R. Tolkien', 
        page_count: 320
    };
}

var { title, author, page_count, language = 'English' } = getBookObject();
console.log(`${title} in ${language} by ${author} [${page_count} pages]`);
```

The result is that we still get the values that existed from our object, while filling in any gaps with the default value:

```
The Hobbit in English by J. R. R. Tolkien [320 pages]
```

## Object Literals Syntax

`ES6` also introduces some shorthand syntax for simplifying the declaration of object literals.  For example, here is the traditional method of assigning our `title` and `author` variables to the `book.title` and `book.author` object properties:

```js
var title = 'The Hobbit', author = 'J.R.R. Tolkien'
var book = {
    title: title,
    author: author
};
console.log(book);
```

This works just fine and outputs our `book` object:

```
Object {title: "The Hobbit", author: "J.R.R. Tolkien"}
```

However, now with `ES6`, there's no need to replicate the `:` separator syntax within the object declaration when the property name and variable name match (which they so often do).  Thus, the same `book` object declaration above can be written like so:

```js
var book = {
    title,
    author
};
```

This same simplification of syntax applies to object method declarations as well.  Here's the old way:

```js
var book = {
    getTitle: function() {
        return 'The Hobbit';
    },
    getAuthor: function() {
        return 'J.R.R. Tolkien';
    }
};
console.log(`${book.getTitle()} by ${book.getAuthor()}`);
```

And here's the new `ES6` shorthand syntax (essentially removing the `: function` part):

```js
var book = {
    getTitle() {
        return 'The Hobbit';
    },
    getAuthor() {
        return 'J.R.R. Tolkien';
    }
};
console.log(`${book.getTitle()} by ${book.getAuthor()}`);
```

To help you and your team with JavaScript development, particularly when dealing with unexpected errors, check out the revolutionary <a class="js-cta-utm" href="https://airbrake.io/languages/javascript_exception_handler?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-js">Airbrake JavaScript</a> error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

---

__META DESCRIPTION__

Part 2 of our journey through the exciting new features introduced in the latest version of JavaScript, ECMAScript 6 (ES6).

---

__SOURCES__

- https://github.com/getify/You-Dont-Know-JS/tree/master/es6%20%26%20beyond
- https://github.com/lukehoban/es6features
- https://developer.mozilla.org/en/docs/Web/JavaScript/Reference/Classes