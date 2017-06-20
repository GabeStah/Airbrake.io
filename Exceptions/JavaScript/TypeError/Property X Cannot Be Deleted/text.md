# JavaScript Errors - Property X Cannot Be Deleted TypeError

Next up in our [__JavaScript Error Handling__](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) series we're looking at the `Property X Cannot Be Deleted TypeError` in greater detail.  The `Property X Cannot Be Deleted TypeError` is typically thrown when trying to delete an object property that is `non-configurable`.

In this article we'll explore the `Property X Cannot Be Deleted TypeError` to see where it sits within the JavaScript `Exception` hierarchy along with a few sample code snippets to illustrate how `Property X Cannot Be Deleted TypeErrors` are thrown.  We'll also briefly take a gander at JavaScript's built-in data properties (like `configurable`), so let's get started!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object, or an inherited object therein.
- The [`TypeError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/TypeError) object is inherited from the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object.
- The `Property X Cannot Be Deleted TypeError` is a descendant of [`TypeError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/TypeError) object.

## When Should You Use It?

As we've seen many times before the `Property X Cannot Be Deleted TypeError` is a `strict mode` only exception.  Strict mode is an optional variant of JavaScript that uses slightly different (and typically more restrictive) semantics when compared to normal JavaScript.  One specific change is that strict mode contains a set of error types that are not thrown when strict mode is disabled and `Property X Cannot Be Deleted TypeError` is one such example.

Strict mode can be enabled by placing the `'use strict';` statement at the top of our script.  We won't go into any more details of strict mode's particular behaviors here, but feel free to check out the [MDN documentation](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Strict_mode) for more info.

As previously mentioned the `Property X Cannot Be Deleted TypeError` occurs when trying to `delete` a property value that is marked as `non-configurable`.  The `configurable` state of a property is actually an `attribute` in JavaScript.  Such an `attribute` is essentially a built-in configuration setting that can be altered throughout code and will effect how that property behaves.  Some methods allow for direct modification of property `attributes`, while other methods modify certain `attributes` behind the scenes.

We can use the [`Object.defineProperties()`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Object/defineProperties) method to directly add or modify object `properties`, including the `name`, `value`, and `attributes`.  Here we have a simple example where we've defined a `book` object and then called `Object.defineProperties()` which expects the first argument to be the object we're modifying, while the second argument is an object defining the list of `properties` to add/modify and their respective values:

```js
// Enable strict mode.
'use strict';

let printError = function (error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    // Create book object with a misspelled title.
    let book = {};

    // Assign properties.
    Object.defineProperties(book, {
        'author': {
            value: 'Patrick Rothfuss',
            configurable: true                    
        },                
        'title': {
            value: 'The Name of the Wind'
        }
    });
    // Output book.
    console.log(book); // Object {title: "The Name of the Wind", author: "Patrick Rothfuss"}
    // Delete the author property.
    delete book.author;
    // Output modified book.
    console.log(book); // Object {title: "The Name of the Wind"}
} catch (e) {
    if (e instanceof TypeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

In the above case we specified the `name` of our `properties`, the `value`, and also set the `configurable` attribute to `true` for the `author` property only.  The `configurable` attribute defaults to `false`, which specifies that the `proeprty` cannot be deleted or have most other attributes changed.

We can see from the commented output next to the `console.log()` calls above that our first output shows both properties (`title` and `author`).  We then delete the `book.author` property and then output again, while shows the modified object only contains `title`.  

Since `configurable` defaults to `false` our `title` property is currently considered `non-configurable`.  Therefore, let's try deleting the `title` property this time instead of `author` and see what happens:

```js
// Create book object with a misspelled title.
let book = {};

// Assign properties.
Object.defineProperties(book, {
    'author': {
        value: 'Patrick Rothfuss',
        configurable: true                    
    },                
    'title': {
        value: 'The Name of the Wind'
    }
});
// Output book.
console.log(book); // Object {title: "The Name of the Wind", author: "Patrick Rothfuss"}
// Delete the title property.
delete book.title;
// Output modified book.
console.log(book);
```

Unsurprisingly, the attempt to delete the `non-configurable` `title` property results in a `Property X Cannot Be Deleted TypeError`, which appears as slightly different messages depending on the browser in use:

```js
// CHROME
[EXPLICIT] TypeError: Cannot delete property 'title' of #<Object>

// FIREFOX
[EXPLICIT] TypeError: property "title" is non-configurable and can't be deleted
```

While methods like `Object.defineProperties()` allow us to directly add/modify property attributes like `configurable` there are many built-in methods in the API that will change attributes on their own.  For example, one such method that modifies the `configurable` attribute is [`Object.freeze()`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Object/freeze), which provides a simple way to prevent properties from being added, removed, or modified in anyway.  Such an object is typically referred to as `immutable`.

For example, here we have a similar example from before except we're declaring our `book` object's properties inline.  After outputting the full book content we `freeze` the `book` object and then try to `delete` the `author` property:

```js
// Enable strict mode.
'use strict';

let printError = function (error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    // Create book object with a misspelled title.
    let book = { title: 'The Name of the Wind', author: 'Patrick Rothfuss' };
    // Output book.
    console.log(book); // Object {title: "The Name of the Wind", author: "Patrick Rothfuss"}
    // Freeze book object.
    Object.freeze(book);            
    // Attempt to delete the author property.
    delete book.author;
    // Output modified book.
    console.log(book); // Object {title: "The Name of the Wind"}
} catch (e) {
    if (e instanceof TypeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

As it turns out this throws another `Property X Cannot Be Deleted TypeError`:

```js
// CHROME
[EXPLICIT] TypeError: Cannot delete property 'author' of #<Object>

// FIREFOX
[EXPLICIT] TypeError: property "author" is non-configurable and can't be deleted
```

We're seeing another `Property X Cannot Be Deleted TypeError` because, just as before when we explicitly set our `configurable` property to `false`, calling `Object.freeze()` on our object also sets `configurable` to `false` behind the scenes.  This can be verified by calling the `Object.getOwnPropertyDescriptor()` method and passing the object and name of the property in question.  In this case it doesn't matter which property we check since the entire object is frozen, but here we are outputting the `author` property object which shows the _direct_ property descriptor (as opposed to the indirect descriptor attached to the prototype chain of our object instance):

```js
// Create book object with a misspelled title.
let book = { title: 'The Name of the Wind', author: 'Patrick Rothfuss' };
// Freeze book object.
Object.freeze(book);
// Output `author` property descriptor object.
console.log(Object.getOwnPropertyDescriptor(book, 'author'));
// {value: "Patrick Rothfuss", writable: false, enumerable: true, configurable: false}
```

To dive even deeper into understanding how your applications deal with JavaScript Errors, check out the revolutionary <a class="js-cta-utm" href="https://airbrake.io/languages/javascript_exception_handler?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-js">Airbrake JavaScript</a> error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

---

__META DESCRIPTION__

A close look at the Property X Cannot Be Deleted TypeError in JavaScript, including functional code examples and a brief look at property attributes.

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
