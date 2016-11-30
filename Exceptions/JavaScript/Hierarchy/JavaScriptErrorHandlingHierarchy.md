The powerful JavaScript language and development platform provides a number of useful core object constructors that allow for simple exception and error administration.  JavaScript error handling is typically performed through the generic `Error` object, or from any of a number of core error constructors.

Here we see the basic list of built-in error objects provided by JavaScript:

- `Error`
- `EvalError`
- `InternalError`
- `RangeError`
- `ReferenceError`
- `SyntaxError`
- `TypeError`
- `URIError`

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
