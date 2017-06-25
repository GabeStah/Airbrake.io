# JavaScript Errors - Variable X Redeclares Argument TypeError

Today, as we approach the end of our [__JavaScript Error Handling__](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) series, we'll be looking at the `Variable X Redeclares Argument TypeError` a bit closer.  `Variable X Redeclares Argument TypeError` is a `strict mode` only error that occurs when attempting to redeclare an argument (althought it's technically a parameter in this context) within a function definition.

Throughout this article we'll explore the `Variable X Redeclares Argument TypeError` in more detail, looking at where it sits within the JavaScript `Exception` hierarchy as well as going over a few simple code examples that try to show how `Variable X Redeclares Argument TypeErrors` might pop up.  Let's get to it!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object, or an inherited object therein.
- The [`TypeError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/TypeError) object is inherited from the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object.
- The `Variable X Redeclares Argument TypeError` is a descendant of [`TypeError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/TypeError) object.

## When Should You Use It?

As with numerous JavaScript errors we've already explored in this article series the `Variable X Redeclares Argument TypeError` is an error that only occurs within `strict mode`.  Strict mode is an optional variant of JavaScript that uses slightly different (and typically more restrictive) semantics when compared to normal JavaScript.  One specific change is that strict mode contains a set of error types that are not thrown when strict mode is disabled.  `Variable X Redeclares Argument TypeError` is one example of an exception that only occurs while strict mode is enabled.

Typically, strict mode is enabled by adding the `'use strict';` line at the beginning of the code.  Strict mode is usually applied to _entire scripts_, so once it's enabled it is active for the entirety of that execution chain.  While we won't go into any more details of strict mode's particular behaviors here, there is far more information that can be found in the [MDN documentation](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Strict_mode) and elsewhere.

Now that we know how to use strict mode let's go through a few simple code snippets to see how `Variable X Redeclares Argument TypeErrors` might crop up in day-to-day coding.  Here we have defined a `Book` class with a `constructor()` method, a few attributes to give our book some info, and finally a basic `getBookData()` instance method that returns a nicely formatted string with book data in it:

```js
// Enable strict mode.
'use strict';

let printError = function (error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

// Book as Class
class Book {
    // Special constructor method.
    constructor(title, author, publicationDate) {
        // Set properties
        this.title = title;
        this.author = author;
        this.publicationDate = publicationDate;
    }

    getBookData(includePublicationDate)
    {
        return `"${this.title}" by ${this.author}${includePublicationDate ? `, published on ${this.publicationDate}` : `` }`
    }
}

try {
    // Create new Book instance.
    let book = new Book('The Name of the Wind', 'Patrick Rothfuss', new Date(2007, 3, 27));
    // Output book.
    console.log(book);
    // Book {title: "The Name of the Wind", author: "Patrick Rothfuss", publicationDate: Fri Apr 27 2007 00:00:00 GMT-0700 (Pacific Daylight Time)}

    // Output default getBookData() return.
    console.log(book.getBookData());
    // "The Name of the Wind" by Patrick Rothfuss

    // Output getBookData(true) return (includes publication date).
    console.log(book.getBookData(true));
    // "The Name of the Wind" by Patrick Rothfuss, published on Fri Apr 27 2007 00:00:00 GMT-0700 (Pacific Daylight Time)
} catch (e) {
    if (e instanceof TypeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

We've allowed our `Book.getBookData()` method to accept an optional argument to define the `includePublicationDate` boolean, which we use to indicate whether the output should include the date of publication or not.  Our two output tests above show that this argument is working as intended.

However, let's see what happens if we try _redeclaring_ the `includePublicationDate` parameter within the `getBookData()` method definition:

```js
// Enable strict mode.
'use strict';

let printError = function (error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

// Book as Class
class Book {
    // Special constructor method.
    constructor(title, author, publicationDate) {
        // Set properties
        this.title = title;
        this.author = author;
        this.publicationDate = publicationDate;
    }

    getBookData(includePublicationDate)
    {
        var includePublicationDate = false;
        return `"${this.title}" by ${this.author}${includePublicationDate ? `, published on ${this.publicationDate}` : `` }`
    }
}

try {
    'use strict';
    // Create new Book instance.
    let book = new Book('The Name of the Wind', 'Patrick Rothfuss', new Date(2007, 3, 27));
    // Output book.
    console.log(book);
    // Book {title: "The Name of the Wind", author: "Patrick Rothfuss", publicationDate: Fri Apr 27 2007 00:00:00 GMT-0700 (Pacific Daylight Time)}

    // Output default getBookData() return.
    console.log(book.getBookData());
    // "The Name of the Wind" by Patrick Rothfuss

    // Output getBookData(true) return (includes publication date).
    console.log(book.getBookData(true));
    // "The Name of the Wind" by Patrick Rothfuss, published on Fri Apr 27 2007 00:00:00 GMT-0700 (Pacific Daylight Time)
} catch (e) {
    if (e instanceof TypeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

Most of our code is the same here except we've added one line inside our `getBookData()` method: `var includePublicationDate = 'false';`  As a result, in _older_ versions of JavaScript (prior to ES6 essentially) calling this code produces a `Variable X Redeclares Argument TypeError`:

```
// FIREFOX
TypeError: variable includePublicationDate redeclares argument
```

This is because the declaration already exists since `includePublicationDate` was defined in the parameter list of the method.  The obvious solution is to forgo redeclaration in most cases and just set the variable to a new value, if necessary:

```js
getBookData(includePublicationDate)
{
    includePublicationDate = false;
    return `"${this.title}" by ${this.author}${includePublicationDate ? `, published on ${this.publicationDate}` : `` }`
}
```

That said, this issue error is largely eliminated in modern JavaScript engines and with ES6.  Instead of throwing a `Variable X Redeclares Argument TypeError` while in strict mode most JavaScript parsers now recognize that the _intent_ of redeclaring a parameter is to just set it to the new value (if applicable), so modern browsers will effectively **ignore** the `var` keyword in that statement, as if it read merely `includePublicationDate = false;` instead.

In this case, running our above code where we attempt redeclare our parameter with the statement `var includePublicationDate = false;` results in the following output for both Chrome and Firefox browsers:

```
Book {title: "The Name of the Wind", author: "Patrick Rothfuss", publicationDate: Fri Apr 27 2007 00:00:00 GMT-0700 (Pacific Daylight Time)}
"The Name of the Wind" by Patrick Rothfuss
"The Name of the Wind" by Patrick Rothfuss
```

Notice that, even though we pass a `true` value to the first argument of `getBookData()` our "redeclaration" takes over, recognizes the intent is just to set the parameter value to `false` no matter what argument was passed, and so our output shows that result.

Another related consideration with ES6 is the introduction and use of the `let` keyword, which should typically be used in scenarios like this anyway.  For example, let's take the same redeclaration snippet above but only change the `var` keyword to `let`:

```js
// Enable strict mode.
'use strict';

let printError = function (error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

// Book as Class
class Book {
    // Special constructor method.
    constructor(title, author, publicationDate) {
        // Set properties
        this.title = title;
        this.author = author;
        this.publicationDate = publicationDate;
    }

    getBookData(includePublicationDate)
    {
        let includePublicationDate = false;
        return `"${this.title}" by ${this.author}${includePublicationDate ? `, published on ${this.publicationDate}` : `` }`
    }
}

try {
    'use strict';
    // Create new Book instance.
    let book = new Book('The Name of the Wind', 'Patrick Rothfuss', new Date(2007, 3, 27));
    // Output book.
    console.log(book);
    // Book {title: "The Name of the Wind", author: "Patrick Rothfuss", publicationDate: Fri Apr 27 2007 00:00:00 GMT-0700 (Pacific Daylight Time)}

    // Output default getBookData() return.
    console.log(book.getBookData());
    // "The Name of the Wind" by Patrick Rothfuss

    // Output getBookData(true) return (includes publication date).
    console.log(book.getBookData(true));
    // "The Name of the Wind" by Patrick Rothfuss, published on Fri Apr 27 2007 00:00:00 GMT-0700 (Pacific Daylight Time)
} catch (e) {
    if (e instanceof TypeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

This is the best practice technique for handling localized variables such as the `includePublicationDate` parameter with ES6.  Running the above code produces two interesting results.

First and foremost is we get an error message upon execution but it's actually a `SyntaxError`, informing us that the `includePublicationDate` identifier has already been declared:

```
// CHROME
Uncaught SyntaxError: Identifier 'includePublicationDate' has already been declared
// FIREFOX
SyntaxError: redeclaration of formal parameter includePublicationDate
```

Moreover, most modern code editors will actually parse and _detect_ this issue well before the code is actually executed.  For example, [`Visual Studio Code`](https://code.visualstudio.com/) (the editor this article is being written in) shows a warning at the two lines in question and indicates there's an issue via the following message: `"Duplicate identifier 'includePublicationDate'."`

To dive even deeper into understanding how your applications deal with JavaScript Errors, check out the revolutionary <a class="js-cta-utm" href="https://airbrake.io/languages/javascript_exception_handler?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-js">Airbrake JavaScript</a> error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

---

__META DESCRIPTION__

A deep dive into the Variable X Redeclares Argument TypeError in JavaScript, including functional code examples and look at behavioral changes in ES6.

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
