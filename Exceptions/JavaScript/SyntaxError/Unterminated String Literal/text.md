# JavaScript Errors - SyntaxError: unterminated string literal

Moving along through our [__JavaScript Error Handling__](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) series, today we'll take a closer look at the `Unterminated String Literal` `SyntaxError`.  JavaScript requires that string literals be enclosed in quotations, whether that be single (`'`) or double (`"`) quotes.  The `Unterminated String Literal` error is thrown when when a string is detected that doesn't properly terminate, like when it is missing closing quotations.

In this article we'll explore the `Unterminated String Literal` error in more detail, looking at where it fits within the JavaScript `Exception` hierarchy and examining a few simple code examples to see how `Unterminated String Literal` errors might occur in your own coding adventures, so let's get started!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object, or an inherited object therein.
- The [`SyntaxError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SyntaxError) object is inherited from the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object.
- The `Unterminated String Literal` error is a specific type of [`SyntaxError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SyntaxError) object.

## When Should You Use It?

To understand how an `Unterminated String Literal` error might occur we should first explore how JavaScript deals with strings and, in particular, `string literals`.  As we discussed in the introduction, a string literal must be enclosed by quotations.  JavaScript doesn't care if we use single or double quotes, so it is common practice to use single quotes for all string literals _except_ if that string must contain an apostrophe (thus requiring an escape sequence to prevent the string from being terminated earlier than intended).

For example, our `first` variable is assigned to the value `Conan` using the single quote literal notation, while the `last` name is double-quoted because of the apostrophe:

```js
var first = 'Conan';
var last = "O'Brien";
```

Outside of requiring quotations to enclose the string, we can do anything we want, including place the string literal on multiple lines (typically used to better format the code by keeping line lengths shorter):

```js
var lipsum = 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. ' +
             'Nunc aliquam accumsan mauris, sed porta enim. Morbi arcu ' +
             'enim, rutrum eu arcu nec, egestas venenatis dui. Aenean ' +
             'quis elementum erat, at porttitor erat.';
```

Notice that we're terminating the string literal contained on each individual line, then concatenating them together using the plus (`+`) operator.  Let's try not enclosing our strings in quotations for each line and removing the `+` operator:

```js
var lipsum = 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. 
             Nunc aliquam accumsan mauris, sed porta enim. Morbi arcu 
             enim, rutrum eu arcu nec, egestas venenatis dui. Aenean 
             quis elementum erat, at porttitor erat.';
```

Lo and behold, this produces our first `Unterminated String Literal` error in Firefox:

```
// FIREFOX
SyntaxError: unterminated string literal

// CHROME
Uncaught SyntaxError: Invalid or unexpected token
```

This shouldn't be all that surprising.  JavaScript evaluates each line and, in fact, each character per line is evaluated to identify the appropriate `tokens` to be built and placed in the execution tree.  Thus, when the parser reaches the end of our first line it expects one of a handful of appropriate characters (`tokens`), like a closing single quotation in this case, but since it fails to find any of those it throws an error instead.

However, we aren't restricted to only using `+` operators for multi-line string literals.  A second option is using the backslash character (`\`) as our separator:

```js
var lipsum = 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. \
             Nunc aliquam accumsan mauris, sed porta enim. Morbi arcu \
             enim, rutrum eu arcu nec, egestas venenatis dui. Aenean \
             quis elementum erat, at porttitor erat.';
console.log(lipsum);
```

Unfortunately we've made a simple yet understandable mistake: We left our indentations in the code to make it more readable.  The problem is that JavaScript doesn't know that we don't want that extra spacing in front of the second, third, and fourth lines, so the resulting string output is rather screwy:

```
Lorem ipsum dolor sit amet, consectetur adipiscing elit.              Nunc aliquam accumsan mauris, sed porta enim. Morbi arcu              enim, rutrum eu arcu nec, egestas venenatis dui. Aenean              quis elementum erat, at porttitor erat.
```

Therefore, when using backslash separators for string literals it's important to ensure there is no unwanted trailing space after each backslash.  Thus, our code should look something like this instead:

```js
var lipsum = 'Lorem ipsum dolor sit amet, consectetur adipiscing elit. \
Nunc aliquam accumsan mauris, sed porta enim. Morbi arcu \
enim, rutrum eu arcu nec, egestas venenatis dui. Aenean \
quis elementum erat, at porttitor erat.'; 
console.log(lipsum);
```

It's not very readable, but it gets the job done and our output is properly formatted as we intended:

```
Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc aliquam accumsan mauris, sed porta enim. Morbi arcu enim, rutrum eu arcu nec, egestas venenatis dui. Aenean quis elementum erat, at porttitor erat.
```

Lastly, ECMAScript 2015 -- which we have been writing about in [another series of articles](https://airbrake.io/blog/javascript/es6-javascript-whats-new-1) -- introduced a new syntactic feature called [`template literals`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Template_literals).  A template literal is created by surrounding a string literal with backtick (`` ` ``) characters, like so:

```js
var lipsum = `Lorem ipsum dolor sit amet, consectetur adipiscing elit. 
              Nunc aliquam accumsan mauris, sed porta enim. Morbi arcu 
              enim, rutrum eu arcu nec, egestas venenatis dui. Aenean 
              quis elementum erat, at porttitor erat.`;
console.log(lipsum);
```

Unfortunately, in this particular case the template literal syntax tells JavaScript that we want every single character between the backticks to be included in the string.  This is useful in some situations, of course, but for our purposes it once again adds some strange formatting to the output:

```
Lorem ipsum dolor sit amet, consectetur adipiscing elit. 
              Nunc aliquam accumsan mauris, sed porta enim. Morbi arcu 
              enim, rutrum eu arcu nec, egestas venenatis dui. Aenean 
              quis elementum erat, at porttitor erat.
```

To dive even deeper into understanding how your applications deal with JavaScript Errors, check out the revolutionary <a class="js-cta-utm" href="https://airbrake.io/languages/javascript_exception_handler?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-js">Airbrake JavaScript</a> error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

---

__META DESCRIPTION__

A close look at the unterminated string literal SyntaxError in JavaScript, with code examples and a quick look at string literals.

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
