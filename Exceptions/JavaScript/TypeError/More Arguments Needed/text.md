# JavaScript Errors - More Arguments Needed

Next up in our [__JavaScript Error Handling__](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) series is the `More Arguments Needed TypeError`.  The `More Arguments Needed TypeError` is typically thrown when calling a method or function that requires a minimum number of arguments that simply weren't provided by the calling code.

In today's article we'll examine the `More Arguments Needed TypeError` in a bit more detail by looking at where it sits within the JavaScript `Exception` hierarchy.  We'll also use a few code examples to illustrate how `More Arguments Needed TypeErrors` most commonly occur, so let's get started!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object, or an inherited object therein.
- The [`TypeError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/TypeError) object is inherited from the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object.
- The `More Arguments Needed TypeError` is a descendant of [`TypeError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/TypeError) object.

## When Should You Use It?

In this first example we have an array containing some basic info of one of my all-time favorite books.  However, we want to split the data up a bit into more manageable sub-arrays: One array with just the title and author and the other with just the publication date.  

To accomplish this we'll use the `Array.prototype.filter()` method which expects the first argument to be a function that will be executed for every entry in the array.  If that function returns a `true` boolean value the element is added/kept in the array and if it returns `false` that element is tossed out.  Therefore, we're using the new `ES6` `arrow function` syntax to create an anonymous inline function that will assign our element to the `v` parameter.  We then check that the `typeof v` is a `string` to get the author and title, while we verify the `instanceof` value to be `Date` to grab our publication date:

```js
let printError = function (error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    // Create book array with a misspelled title.
    let book = [ 'The Stand', 'Stephen King', new Date(1978, 9) ];
    // Filter the title and author.
    let titleAuthor = book.filter(v => typeof v === 'string');
    // Filter without a required argument.
    let publicationDate = book.filter(v => v instanceof Date);
    // Output the results.
    console.log(titleAuthor); // ["The Stand", "Stephen King"]
    console.log(publicationDate); // [Sun Oct 01 1978 00:00:00 GMT-0700 (Pacific Daylight Time)]
} catch (e) {
    if (e instanceof TypeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

Sure enough this works as expected and we get two filtered arrays.  However, like many methods in JavaScript the `Array.prototype.filter()` method expects _at least one_ argument to be passed to it.  This makes sense since there'd be no point in looping through every array element and performing a filter that does nothing at all, which is what would happen if we passed zero arguments.  Therefore, let's see what happens if we try this same example but without providing an argument to `Array.prototype.filter`:

```js
try {
    // Create book array with a misspelled title.
    let book = [ 'The Stand', 'Stephen King', new Date(1978, 9) ];
    // Filter the title and author.
    let titleAuthor = book.filter(v => typeof v === 'string');
    // Try to filter without required argument.
    let publicationDate = book.filter();
    // Output the results.
    console.log(titleAuthor);
    console.log(publicationDate); 
} catch (e) {
    if (e instanceof TypeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

Perhaps unsurprisingly this throws a `More Arguments Needed TypeError`, although the formatting of the message differs depending on the JavaScript engine (i.e. browser) in use:

```
// CHROME
[EXPLICIT] TypeError: undefined is not a function
// FIREFOX
[EXPLICIT] TypeError: missing argument 0 when calling function Array.prototype.filter
```

There seems to be little rhyme or reason to determine which built-in methods and functions in JavaScript _will_ throw a `More Arguments Needed TypeError` when not enough arguments are provided, so it'll require a bit of experimentation to figure out which abide by these requirements and which do not.  Obviously the best practice is to try to just write code that doesn't make improper argument quantity calls in the first place (ESLint and other IDE parsers help a lot in this regard).

Another related and fun consideration when working with JavaScript is to recall that JavaScript doesn't enforce any sort of _maximum_ number of arguments that can be passed to a method.  Instead, JavaScript automatically creates a metaparameter called `arguments` within the scope of every function/method block.  `arguments` is an Array-like object that contains all the argument values passed to that particular function/method call.  This knowledge means that even though many methods have a _minimum_ number of arguments that must be passed there is never a _maximum_ number, which can be used to your advantage in some cool ways.

For example, here we have a little `Book` class that lets us create book objects more easily.  We also have the `Book.toArray()` method that returns an array of the instance properties, with or without the `.publicationDate` property, depending on the boolean value of the single `withPublicationDate` parameter:

```js
class Book {
    // Special constructor method
    constructor(title, author, publicationDate) {
        // Set properties
        this.title = title;
        this.author = author;
        this.publicationDate = publicationDate;
    }

    toArray(withPublicationDate) {
        if (withPublicationDate) {
            // Return title, author, and publication date.
            return [this.title, this.author, this.publicationDate];
        }
        // Return just title and author.
        return [this.title, this.author];
    }
}

try {
    // Create book array with a misspelled title.
    let book = new Book('The Stand', 'Stephen King', new Date(1978, 9));

    // Output default array.
    console.log(book.toArray());
    // ["The Stand", "Stephen King"]

    // Output array with publication date.
    console.log(book.toArray(true));
    // ["The Stand", "Stephen King", Sun Oct 01 1978 00:00:00 GMT-0700 (Pacific Daylight Time)]
} catch (e) {
    if (e instanceof TypeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

As you can see from the output below our calls to the `Book.toArray()` method work as expected, generating an array we can output with and without `publicationDate` depending on that first argument.  However, let's try modifying our `Book.toArray()` method a bit to make use of the hidden `arguments` parameter:

```js
toArray(withPublicationDate) {
    if (withPublicationDate) {
        // Convert arguments Array-like object to Array.
        let extra = Array.from(arguments);
        // Return title, author, publication date, and combine with extra arguments array.
        return [this.title, this.author, this.publicationDate].concat(extra);
    }
    // Return just title and author.
    return [this.title, this.author];
}
```

Now when we call `Book.toArray()` any and all parameters we provide -- even beyond the first `withPublicationDate` boolean -- will be combined into the output of our full array.  Sure enough, if we test this out by passing a few extra parameters to our second `book.toArray()` call we get the extra elements in the returned and output array:

```js
try {
    // Create book array with a misspelled title.
    let book = new Book('The Stand', 'Stephen King', new Date(1978, 9));

    // Output default array.
    console.log(book.toArray());
    // ["The Stand", "Stephen King"]

    // Output array with publication date.
    console.log(book.toArray(true, 12345, 'banana'));
    // ["The Stand", "Stephen King", Sun Oct 01 1978 00:00:00 GMT-0700 (Pacific Daylight Time), true, 12345, "banana"]
} catch (e) {
    if (e instanceof TypeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

To dive even deeper into understanding how your applications deal with JavaScript Errors, check out the revolutionary <a class="js-cta-utm" href="https://airbrake.io/languages/javascript_exception_handler?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-js">Airbrake JavaScript</a> error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

---

__META DESCRIPTION__

A closer look at the More Arguments Needed TypeError in JavaScript, including working code examples examples with the hidden arguments metaparameter.

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
