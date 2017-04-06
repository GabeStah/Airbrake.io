# JavaScript Errors - SyntaxError: missing } after property list

Today, as we move along through our [__JavaScript Error Handling__](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) series, we'll be examining the `Missing Brace After Property List` JavaScript error.  As the name implies, the `Missing Brace After Property List` error is another in the line of `SyntaxErrors`, which occurs specifically when initializing `properties` of an `Object` where the parser expects a closing brace (`}`) to appear.

Throughout this article we'll take a closer look at the `Missing Brace After Property List` error, including where it resides within the JavaScript `Exception` hierarchy, along with some potential causes of a `Missing Brace After Property List` error, so let's get going!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object, or an inherited object therein.
- The [`SyntaxError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SyntaxError) object is inherited from the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object.
- The `Missing Brace After Property List` error is a specific type of [`SyntaxError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SyntaxError) object.

## When Should You Use It?

As with a handful of other `SyntaxErrors` we've explored in previous articles, the `Missing Brace After Property List` error is directly related to incorrect syntax when initializing `Objects`.  Therefore, to explore this error in more detail, we should first take a moment to see how `Object` initialization syntax works in JavaScript.

First, it's important to understand that JavaScript itself has only two types of entities: `Primitives` and `Objects`.

An [`Object`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Object) is simply a [collection of properties](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Guide/Working_with_Objects), used to programmatically represent a real world object.  On the other hand, a [`Primitive`](https://developer.mozilla.org/en-US/docs/Glossary/Primitive) represents a fundamental data type and is `immutable`, meaning its value cannot be changed.  `Primitive` types include: `string`, `number`, `boolean`, `null`, `undefined`, and `symbol`.

To work with `Objects` and their underlying `properties`, your code will use the `dot syntax`:

```js
// Get the title property of the book Object.
book.title;
```

As for initialization, just as with `Arrays` and other collection types in JavaScript, there are a few different syntaxes that can be used to create an `Object`:

```js
// Inline, without properties.
var bookA = {};
bookA.author = 'Patrick Rothfuss';
bookA.title = 'The Name of the Wind';
console.log(bookA);

// Inline, with properties.
var bookB = {
    author: 'Patrick Rothfuss',
    title: 'The Name of the Wind'
};
console.log(bookB);

// Using new keyword constructor.
var bookC = new Object(bookB);
console.log(bookC);

// Failed equivalence test.
console.log(bookA === bookB);
```

This produces the following output:

```
Object {author: "Patrick Rothfuss", title: "The Name of the Wind"}
Object {author: "Patrick Rothfuss", title: "The Name of the Wind"}
Object {author: "Patrick Rothfuss", title: "The Name of the Wind"}
false
```

The double-braces syntax is the most common when generating a new `Object`.  In the first example above, we begin by initializing our `Object`, and only then do we assign a few `properties` to it.  In the second example, we assign `properties` inline, using the `property: value` syntax.  In the third example, we use the `new Object()` constructor, which accepts any value as its argument, and attempts to create an `Object` of the same `Type` of the passed argument.  In this case, we passed in `bookB` to the `new Object()` constructor, so the call creates a new `Object` type of `Object` (as opposed to a `String`, `Array`, or what not), with the same `properties` as `bookB`.

The final line illustrates one last important point, which is that `equivalence` is not measured based on the `property values` of `Objects`, but instead upon their in-memory reference.  Therefore, even if all `properties` of two `Objects` are the same, if they don't literally point to the same `Object` reference, they won't be considered equivalent.

Now, with that basic understanding and syntax out of the way, we can take a look at how the `Missing Brace After Property List` error might come about.  The most common cause is when initializing an `Object` using the inline syntax, with comma-separated `property: value` pairs, and forgetting to include a comma somewhere in the list:

```js
// Missing comma between author and title properties.
var book = {
    author: 'Patrick Rothfuss'
    title: 'The Name of the Wind'
};
console.log(book);
```

Sure enough, this produces a `Missing Brace After Property List` error in Firefox, while it throws an `Unexpected Identifier` error in Chrome:

```
// CHROME
Uncaught SyntaxError: Unexpected identifier

// FIREFOX
SyntaxError: missing } after property list
```

Interestingly, just as we saw with the `Missing Bracket After Element List` error, unique JavaScript engines evaluate the `Missing Brace After Property List` error in different ways.  And, just as before, Chrome's engine more accurately evaluates the problem and provides a better error message.  In this case, the parser recognizes that we're defining an `Object` due to the opening brace (`{`).  It then looks for an appropriate symbol to follow the opening brace:

- An `identifier`, such as: `property_name`.
- A `number` such as: `5`.
- A `string`, such as: `'property_name'`.
- Or, a closing brace: `}`.

If the engine finds one of those following the opening brace, all is well and it continues.  If it wasn't a closing brace, it next looks for a colon delimiter (`:`).  After the colon, it then expects any type of value, which will complete a single `property: value` pair.

Following a `property: value` pair, it expects one of two symbols:

- A comma delimiter: `,`.
- Or, a closing brace: `}`.

The comma indicates another `property` is about to be listed, and the process repeats.  Meanwhile, a closing brace indicates the list is complete.

With that in mind, we can see how Chrome behaves and reports the issue more accurately than Firefox in the example above.  When the Chrome parser reaches the end of the first `property: value` pair of `author: 'Patrick Rothfuss'` it expects a comma or a closing brace.  When it finds that the next symbol is a new identifier (`title`) instead, it produces an accurate error, informing us there was an `Unexpected identifier`.

Meanwhile, Firefox reaches that same point in parsing, but even though our syntax includes a closing brace, Firefox still reports the issue as a `Missing Brace After Property List` error.

To dive even deeper into understanding how your applications deal with JavaScript Errors, check out the revolutionary <a class="js-cta-utm" href="https://airbrake.io/languages/javascript_exception_handler?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-js">Airbrake JavaScript</a> error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

---

__META DESCRIPTION__

A detailed examination of the missing } after property list SyntaxError error in JavaScript, including a brief overview of Object initialization techniques.

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
