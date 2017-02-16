Travelling deftly through to the next item in our __JavaScript Error Handling__ series, today we're taking a hard look at the  `JSON Parse` error.  The `JSON Parse` error, as the name implies, surfaces when using the [`JSON.parse()`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/JSON/parse) method, but also failing to pass valid JSON as an argument.

In this article, we'll dig deeper into where `JSON Parse` errors sit in the JavaScript error hierarchy, as well as when it might appear and how to handle it when it does.  Let's get started!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`] object, or an inherited object therein.
- The [`SyntaxError`] object is inherited from the [`Error`] object.
- The `JSON Parse` error is a specific type of [`SyntaxError`] object.

## When Should You Use It?

While most developers are probably intimately familiar with JSON and the proper formatting syntax it requires, it doesn't hurt to briefly review it ourselves, to better understand some common causes of the `JSON Parse` error in JavaScript.

[`JavaScript Object Notation`](http://www.json.org/), better known as `JSON`, is a human-readable text format, commonly used to transfer data across the web.  The basic structure of JSON consists of `objects`, which are sets of `string: value` pairs surrounded by curly braces:

```json
{
    "first": "Jane",
    "last": "Doe"
}
```

An `array` is a set of `values`, surrounded by brackets:

```json
[
    "Jane",
    "Doe"
]
```

A `value` can be a `string`, `number`, `object`, `array`, `boolean`, or `null`.

That's really all there is to the JSON syntax.  Since `values` can be other `objects` or `arrays`, JSON can be infinitely nested (theoretically).

In JavaScript, when passing JSON to the `JSON.parse()` method, the method expects properly formatted JSON as the first argument.  When it detects invalid JSON, it throws a `JSON Parse` error.

For example, one of the most common typos or syntax errors in JSON is adding an extra comma separator at the end of an `array` or `object` `value` set.  Notice in the first few examples above, we only use a comma to **literally separate** `values` from one another.  Here we'll try adding an extra, or "hanging", comma after our final `value`:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    var json = `
        {
            "first": "Jane",
            "last": "Doe",
        }
    `
    console.log(JSON.parse(json));
} catch (e) {
    if (e instanceof SyntaxError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

_Note: We're using the backtick (`` ` ``) string syntax to initialize our JSON, which just allows us to present it in a more readable form.  Functionally, this is identical to a string that is contained on a single line._

As expected, our extraneous comma at the end throws a `JSON Parse` error:

```
[EXPLICIT] SyntaxError: Unexpected token } in JSON at position 107
```

In this case, it's telling us the `}` token is unexpected, because the comma at the end informs JSON that there should be a third `value` to follow.

Another common syntax issue is neglecting to surround `string` values within `string: value` pairs with quotations (`"`).  Many other language syntaxes use similar `key: value` pairings to indicate named arguments and the like, so developers may find it easy to forget that JSON requires the string to be explicitly indicated using quotation marks:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    var json = `
        {
            "first": "Jane",
            last: "Doe",
        }
    `
    console.log(JSON.parse(json));
} catch (e) {
    if (e instanceof SyntaxError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}  
```

Here we forgot quotations around the `"last"` key `string`, so we get another `JSON Parse` error:

```
[EXPLICIT] SyntaxError: Unexpected token l in JSON at position 76
```

A few examples is probably sufficient to see how the `JSON Parse` error comes about, but as it so happens, there are dozens of possible versions of this error, depending on how JSON was improperly formatted.  Here's the full list:

| JSON Parse Error Messages |
-------
| SyntaxError: JSON.parse: unterminated string literal |
| SyntaxError: JSON.parse: bad control character in string literal |
| SyntaxError: JSON.parse: bad character in string literal |
| SyntaxError: JSON.parse: bad Unicode escape |
| SyntaxError: JSON.parse: bad escape character |
| SyntaxError: JSON.parse: unterminated string |
| SyntaxError: JSON.parse: no number after minus sign |
| SyntaxError: JSON.parse: unexpected non-digit |
| SyntaxError: JSON.parse: missing digits after decimal point |
| SyntaxError: JSON.parse: unterminated fractional number |
| SyntaxError: JSON.parse: missing digits after exponent indicator |
| SyntaxError: JSON.parse: missing digits after exponent sign |
| SyntaxError: JSON.parse: exponent part is missing a number |
| SyntaxError: JSON.parse: unexpected end of data |
| SyntaxError: JSON.parse: unexpected keyword |
| SyntaxError: JSON.parse: unexpected character |
| SyntaxError: JSON.parse: end of data while reading object contents |
| SyntaxError: JSON.parse: expected property name or '}' |
| SyntaxError: JSON.parse: end of data when ',' or ']' was expected |
| SyntaxError: JSON.parse: expected ',' or ']' after array element |
| SyntaxError: JSON.parse: end of data when property name was expected |
| SyntaxError: JSON.parse: expected double-quoted property name |
| SyntaxError: JSON.parse: end of data after property name when ':' was expected |
| SyntaxError: JSON.parse: expected ':' after property name in object |
| SyntaxError: JSON.parse: end of data after property value in object |
| SyntaxError: JSON.parse: expected ',' or '}' after property value in object |
| SyntaxError: JSON.parse: expected ',' or '}' after property-value pair in object literal |
| SyntaxError: JSON.parse: property names must be double-quoted strings |
| SyntaxError: JSON.parse: expected property name or '}' |
| SyntaxError: JSON.parse: unexpected character |
| SyntaxError: JSON.parse: unexpected non-whitespace character after JSON data |

To dive even deeper into understanding how your applications deal with JavaScript Errors, check out the revolutionary [`Airbrake JavaScript`] error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

[`Airbrake JavaScript`]: https://airbrake.io/languages/javascript_exception_handler
[`Error`]: https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy
[`SyntaxError`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SyntaxError

---

__META DESCRIPTION__

A refresher on the purpose and syntax of JSON, as well as a detailed exploration of the JSON Parse SyntaxError in JavaScript.

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
