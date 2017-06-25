The powerful JavaScript language and development platform provides a number of useful core object constructors that allow for simple exception and error administration.  JavaScript error handling is typically performed through the generic `Error` object, or from any of a number of core error constructors.

Here we see the basic list of built-in error objects provided by JavaScript:

- Error
  - [Permission denied to access property "x"](https://airbrake.io/blog/javascript-error-handling/permission-denied)
- InternalError
  - [too much recursion](https://airbrake.io/blog/javascript-error-handling/internalerror-too-much-recursion)
- RangeError
    - [argument is not a valid code point](https://airbrake.io/blog/javascript/rangeerror-argument-is-not-a-valid-code-point)
    - [invalid array length](https://airbrake.io/blog/javascript/rangeerror-invalid-array-length)
    - [precision is out of range](https://airbrake.io/blog/javascript-error-handling/java)
    - [radix must be an integer](https://airbrake.io/blog/javascript/radix-must-be-an-integer)
    - [repeat count must be less than infinity](https://airbrake.io/blog/javascript/rangeerror-repeat-count-less-than-infinity)
    - [repeat count must be non-negative](https://airbrake.io/blog/javascript-error-handling/rangeerror-repeat-count-non-negative)
- ReferenceError
    - ["x" is not defined](https://airbrake.io/blog/javascript/referenceerror-x-is-not-defined)
    - [assignment to undeclared variable "x"](https://airbrake.io/blog/javascript-error-handling/referenceerror-assignment-to-undeclared-variable-x)
    - [deprecated caller or arguments usage](https://airbrake.io/blog/javascript/referenceerror-deprecated-caller-or-arguments-usage)
    - [invalid assignment left-hand side](https://airbrake.io/blog/javascript-error-handling/invalid-assignment-left-hand-side)
    - [reference to undefined property "x"](https://airbrake.io/blog/javascript/referenceerror-reference-to-undefined-property-x)
- SyntaxError
    - ["use strict" not allowed in function with non-simple parameters](https://airbrake.io/blog/javascript-error-handling/syntaxerror-use-strict-not-allowed-non-simple-parameters)
    - [ "x" is not a legal ECMA-262 octal constant](https://airbrake.io/blog/javascript/x-not-legal-ecma-262-octal-constant)
    - [JSON.parse: bad parsing](https://airbrake.io/blog/javascript/syntaxerror-json-parse-bad-parsing)
    - [Malformed formal parameter](https://airbrake.io/blog/javascript/syntaxerror-malformed-formal-parameter)
    - [Unexpected token](https://airbrake.io/blog/javascript/unexpected-token)
    - [Using //@ to indicate sourceURL pragmas is deprecated. Use //# instead](https://airbrake.io/blog/javascript/invalid-source-map-url)
    - [missing ) after argument list](https://airbrake.io/blog/javascript/javascript-error-handling-syntaxerror-missing-after-argument-list)
    - [missing ; before statement](https://airbrake.io/blog/javascript/syntaxerror-missing-before-statement)
    - [missing bracket after element list](https://airbrake.io/blog/javascript/syntaxerror-missing-after-element-list)
    - [missing } after property list](https://airbrake.io/blog/javascript/syntaxerror-missing-after-property-list)
    - [redeclaration of formal parameter "x"](https://airbrake.io/blog/javascript/redeclaration-formal-parameter-x)
    - [return not in function](https://airbrake.io/blog/javascript/syntaxerror-return-not-function)
    - [test for equality (==) mistyped as assignment (=)?](https://airbrake.io/blog/javascript/test-for-equality-mistyped-assignment)
    - [unterminated string literal](https://airbrake.io/blog/javascript/javascript-errors-syntaxerror-unterminated-string-literal)
- TypeError
    - ["x" has no properties](https://airbrake.io/blog/javascript/null-undefined-properties)
    - ["x" is (not) "y"](https://airbrake.io/blog/javascript/javascript-errors-x-not-y-typeerror)
    - ["x" is not a constructor](https://airbrake.io/blog/javascript/javascript-errors-x-not-constructor-typeerror)
    - ["x" is not a function](https://airbrake.io/blog/javascript/javascript-errors-x-is-not-a-function-typeerror)
    - ["x" is read-only](https://airbrake.io/blog/javascript/javascript-errors-x-is-read-only-typeerror)
    - [More arguments needed](https://airbrake.io/blog/javascript/javascript-errors-more-arguments-needed)
    - [invalid Array.prototype.sort argument](https://airbrake.io/blog/javascript/javascript-errors-invalid-array-sort-argument-typeerror)
    - [property "x" is non-configurable and can't be deleted](https://airbrake.io/blog/javascript/javascript-errors-property-x-cannot-be-deleted-typeerror)
    - [variable "x" redeclares argument](https://airbrake.io/blog/javascript/javascript-errors-variable-x-redeclares-argument-typeerror)
- Warning
    - [-file- is being assigned a //# sourceMappingURL, but already has one]()
    - [JavaScript 1.6's for-each-in loops are deprecated]()
    - [unreachable code after return statement]()

Below we'll examine each of the core error constructors provided by JavaScript, looking at simple examples and descriptions for what can cause each type of error to occur.

## `Error`

At the most basic level, the [`Error`] object is thrown when a runtime error occurs.  `Error` is best used for a generic, user-defined error type that should be thrown, which may not match an existing core error constructor:

```js
try {
  throw new Error('Uh oh');
} catch (e) {
  console.log(e.name + ': ' + e.message);
}
```

__Output__: `Error: Uh oh`.

## `EvalError`

The [`EvalError`] object represents an error that occurs during the use of the global [`eval()`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/eval) function.  While the `EvalError` is no longer thrown by newer versions of JavaScript, and is thus deprecated, it remains intact for backward compatibility purposes.

Here is an example of throwing an `EvalError` object error:

```js
try {
  throw new EvalError('Uh oh', 'myFile.js', 25);
} catch (e) {
  console.log(e instanceof EvalError); // true
  console.log(e.message);              // "Uh oh"
  console.log(e.name);                 // "EvalError"
  console.log(e.fileName);             // "myFile.js"
  console.log(e.lineNumber);           // 25
}
```

## `InternalError`

The [`InternalError`] object is thrown when the JavaScript engine itself experiences an internal error.  While the `InternalError` object is considered non-standard and thus shouldn't be relied upon in production environments, it can be utilized in _some_ cases to detect engine failure points.

Executing the following code will produce an `InternalError` in _some_ browsers or environments (while others will instead produce the `RangeError`, as described below):

```js
function recurse(){
    recurse();
}
recurse();
```

__Output__: `InternalError: too much recursion`.

## `RangeError`

A [`RangeError`] indicates when an error occurs when an argument value is outside of the allowed bounds for a particular method's parameter.  This can be seen when passing improper values to built-in methods such as [`Number.toFixed()`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Number/toFixed):

```js
try {
    var float = 1.2345;
    float.toFixed(21);    
} catch (e) {
    console.log(e.name + ': ' + e.message);
}
```

__Output__: `RangeError: toFixed() digits argument must be between 0 and 20(…)`.

## `ReferenceError`

The [`ReferenceError`] object represents an error when a reference is made to a non-existent variable:

```js
try {
  var a = myUndefinedVariable;
} catch (e) {
  console.log(e.name + ': ' + e.message);
}
```

__Output__: `ReferenceError: myUndefinedVariable is not defined`.

## `SyntaxError`

As the name implies, the [`SyntaxError`] object appears when an error occurs trying to execute code that is syntactically invalid.  For example, here we can catch a `SyntaxError` object by trying to use `eval()` on invalid code:

```js
try {
  eval("this will fail");
} catch (e) {
  console.log(e.name + ': ' + e.message);
}
```

__Output__: `SyntaxError: Unexpected identifier`.

## `TypeError`

The [`TypeError`] object occurs from an error when a value doesn't match the expected data type.

Here we're creating a new variable `foo`, assigning it to the value `null`, and then attempting to call the missing `foo.myMethod` property, which doesn't exist:

```js
try {
  var foo = null;
  foo.myMethod();
} catch (e) {
  console.log(e.name + ': ' + e.message);
}
```

__Output__: `TypeError: Cannot read property 'myMethod' of null`.

## `URIError`

Finally, the [`URIError`] object represents an error that occurred when one of JavaScript's global URI functions (such as [`decodeURI()`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/decodeURI)) is improperly called:

```js
try {
  decodeURI('%foo%bar%');
} catch (e) {
  console.log(e.name + ': ' + e.message);
}
```

__Output__: `URIError: URI malformed`.

[`Error`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Error
[`EvalError`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/EvalError
[`InternalError`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/InternalError
[`RangeError`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/RangeError
[`ReferenceError`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/ReferenceError
[`SyntaxError`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SyntaxError
[`TypeError`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/TypeError
[`URIError`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/URIError

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Error
- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors#List_of_errors
