# ES6 JavaScript: What's New? - Part 6

Today we'll continue down the path of exploring all the cool new features that ES6 JavaScript brings to the table.  Thus far in this series we've covered quite a bit of ground:

- In [Part 1](https://airbrake.io/blog/javascript/es6-javascript-whats-new-1) we took a look at `default parameters`, `classes`, and `block-scoping` with the `let` keyword.
- In [Part 2](https://airbrake.io/blog/javascript/es6-javascript-whats-new-part-two) we explored `constants`, `destructuring`, and `constant literals syntax`.
- In [Part 3](https://airbrake.io/blog/javascript/es6-javascript-whats-new-part-3) we dove deep into just one major feature known as `template literals`.
- In [Part 4](https://airbrake.io/blog/javascript/es6-javascript-whats-new-part-4) we examined the world of `iterators` and `generators`.
- In [Part 5](https://airbrake.io/blog/javascript/es6-javascript-whats-new-part-5) we dug into `arrow functions` and `promises`.

In Part 6 we'll look at `maps`, `sets`, and a slew of new API methods, so let's get to it!

## Maps

In the past, storing unordered key/value pairs in JavaScript was a job typically relegated to `Objects`.  Unfortunately, the inability to use a non-string value as a `key` within an object mapping data structure has always been a major drawback.

For example, here's a typical key/value mapping within an object.  We start with some unique book objects, then create a parent `library` object to house our books.  We use `book1` and `book2` objects as keys and assign those to the proper publication dates of each book, respectively:

```js
// ES5 Example
// Create book objects.
var book1 = { title: "To Kill a Mockingbird", author: "Harper Lee", pageCount: 281 };
var book2 = { title: "The Book Thief", author: "Markus Zusak", pageCount: 584 };

// Create parent library object.
var library = {};

// Set published dates using book objects as keys.
library[book1] = new Date(1960, 6, 11);
library[book2] = new Date(2005, 0, 1);
```

Now, we'll try outputting the published dates by passing our `book1` and `book2` object keys back to `library`:

```js
// Get published dates.
console.log(`'${book1.title}' by ${book1.author} published on ${library[book1]}.`);
// 'To Kill a Mockingbird' by Harper Lee published on Sat Jan 01 2005.
console.log(`'${book2.title}' by ${book2.author} published on ${library[book2]}.`);
// 'The Book Thief' by Markus Zusak published on Sat Jan 01 2005.
```

As we can see, our `library` object cannot differentiate between `book1` and `book2` as key values.  This is because JavaScript converts those objects into strings in order to use them as key values.  Since both book objects are identical in "string form", the `library[book1]` and `library[book2]` statements are identical.  To verify this, we can loop through all our key/value pairs in the `library` object after assignment:

```js
// Output all key/value pairs in library object.
for (var key in library) {
    if (library.hasOwnProperty(key)) {
        console.log(`${key}: ${library[key]}`);
    }
}
// [object Object]: Sat Jan 01 2005 00:00:00 GMT-0800 (Pacific Standard Time)
```

As we can see, this confirms that only a single key/value pair exists in `library`, with the key being `[object Object]` (the string representation of both our book objects).  This causes the second publication date assignment to override the original date assignment.

To resolve this issue, ES6 has added the [`Map`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Map) object, which behaves similar to a normal object in that it holds key/value pairs.  However, unlike an `Array`, a `Map` can use _any value_ as a key, including both primitive types and even objects.

Here we're using the new ES6 `Map` object for our `library`:

```js
// ES6 Mapping Example
// Create book objects.
let book1 = { title: "To Kill a Mockingbird", author: "Harper Lee", pageCount: 281 };
let book2 = { title: "The Book Thief", author: "Markus Zusak", pageCount: 584 };

// Create library map object.
let library = new Map();

// Set published dates using book objects as keys.
library.set(book1, new Date(1960, 6, 11));
library.set(book2, new Date(2005, 0, 1));

// Get published dates.
console.log(`'${book1.title}' by ${book1.author} published on ${library.get(book1).toDateString()}.`);
// 'To Kill a Mockingbird' by Harper Lee published on Mon Jul 11 1960.
console.log(`'${book2.title}' by ${book2.author} published on ${library.get(book2).toDateString()}.`);
// 'The Book Thief' by Markus Zusak published on Sat Jan 01 2005.
```

`Map` objects don't use bracket (`[...]`) syntax to access values, instead implementing the `set()` and `get()` methods, which perform the same basic functionality.  As we can see from the produced output above, this allows us to use our `book1` and `book2` objects as keys for the `library` map object.

A `Map` object also includes a number of additional helper methods, such as:

- `Map.prototype.keys()` to retrieve all keys.
- `Map.prototype.values()` to retrieve all values.
- `Map.prototype.has()` to determine if a provided key exists.

Plus, many more methods.  Check out the [MDN documentation](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Map) for more details.

## Sets

ES6 also introduces the new `Set` object, which is single-dimension collection used to hold unique values.  `Set` provides many of the same methods as the `Map` object, but the behavior (and purpose) is quite different.  The function of a `Set` is to store a list of unique, unordered values.  Since there is no `key` or `index` associated with a given value, there's no need for a `Set.prototype.get()` method.  Instead, we'd use `Set.prototype.has()` to check if a given value exists in the `Set`.

As a basic example we'll create a few book objects again, then play around with adding and removing them from our `library` `Set` object:

```js
// ES6 Set Example
// Create book objects.
let book1 = { title: "To Kill a Mockingbird", author: "Harper Lee", pageCount: 281 };
let book2 = { title: "The Book Thief", author: "Markus Zusak", pageCount: 584 };

// Create library set object.
let library = new Set();

// Add book1 to set.
library.add(book1);

// Determine if books exists in set.
console.log(`'Is ${book1.title}' by ${book1.author} in the library? ${library.has(book1) ? 'YES' : 'NO'}`);
// 'Is To Kill a Mockingbird' by Harper Lee in the library? YES
console.log(`'Is ${book2.title}' by ${book2.author} in the library? ${library.has(book2) ? 'YES' : 'NO'}`);
// 'Is The Book Thief' by Markus Zusak in the library? NO
```

As we can see from the output, by only adding `book1` to the set, `Set.prototype.has()` returns true only for `book1`, but not for `book2`.

We can also `Set.prototype.add()` `book2` and `Set.prototype.delete()` `book1`, then confirm from our output that the objects were properly swapped within the `library` set:

```js
// Add book2 to set.
library.add(book2);
// Delete book1 from set.
library.delete(book1);

// Determine if books exists in set.
console.log(`'Is ${book1.title}' by ${book1.author} in the library? ${library.has(book1) ? 'YES' : 'NO'}`);
// 'Is To Kill a Mockingbird' by Harper Lee in the library? NO
console.log(`'Is ${book2.title}' by ${book2.author} in the library? ${library.has(book2) ? 'YES' : 'NO'}`);
// 'Is The Book Thief' by Markus Zusak in the library? YES
```

## New API Methods

As should be expected with any new major language release, ES6 also includes a number of new API methods that aim to help shore up problematic scenarios found in previous JavaScript versions.  Most of these are quite basic, so we won't go into them in too much detail.

### Array.prototype.find

Useful for finding if a value exists in an array.  The first parameter passed to `Array.prototype.find()` should be a `predicate` function, which expects an array element as its parameter and returns a boolean value indicating if the current array element meets the criteria or not.  If a `true` value is returned by the predicate function, `Array.prototype.find()` returns the matching element.

As a simple example, here we create a predicate function (using arrow syntax) to check if the element is greater than or equal to `5`.  As a result, the first element of `1` fails, but the second element of `6` passes and is returned.

```js
// Define some data.
let data = [1, 6, 3, 4, 2, 5, 7];

// Find first value greater than or equal to 5.
console.log(data.find(x => x >= 5)); // 6
```

### Array.prototype.findIndex()

Similar to `Array.prototype.find()`, the `Array.prototype.findIndex()` method accepts a predicate function, but this time it returns the _index_ of the first matching element, rather than the value.  Here we're finding the index of the first value equal to `4`, which is located at the index of `3`:

```js
// Define some data.
let data = [1, 6, 3, 4, 2, 5, 7];

// Find index of first value equal to 4.
console.log(data.findIndex(x => x == 4)); // 3
```

### String Searching Methods

There are a handful of new `String.prototype` methods designed to make it easier to search _within_ a string.  For example, `String.protype.startWith()` allows us to check if the target string begins with the characters of another string:

```js
// Create name string.
let name = "Chris Decker";

// Check if name starts with 'Chr'.
console.log(name.startsWith("Chr")); // true
```

`String.prototype.endsWith()` checks for the final characters of the string, rather than the beginning:

```js
// Create name string.
let name = "Chris Decker";

// Check if name ends with 'Chr'.
console.log(name.endsWith("Chr")); // false
```

`String.prototype.includes()` is a convenient way to search for a substring within a string.  Plus, just as with `String.prototype.startsWith()` and `String.prototype.endsWith()`, `String.prototype.includes()` can accept an optional second parameter indicating what position the search function should begin at:

```js
// Create name string.
let name = "Chris Decker";

// Check if name includes 'ecker'.
console.log(name.includes("ecker")); // true
// Check if name includes 'ecker' starting at position 10.
console.log(name.includes("ecker", 10)); // false
```

### String.prototype.repeat()

Developers that work with Ruby may feel spoiled, given that it's so easy to create repeated strings (e.g. `"Alice Becker" * 10` to repeat the string ten times).  With ES6, JavaScript now provides a simple method for accomplishing this task through `String.prototype.repeat()`:

```js
// ES6 String.prototype.repeat()
let name = "Alice Becker";

// Repeat name 10 times.
console.log(name.repeat(10));
// Alice BeckerAlice BeckerAlice BeckerAlice BeckerAlice BeckerAlice BeckerAlice BeckerAlice BeckerAlice BeckerAlice Becker
```

### Number.isSafeInteger()

JavaScript is designed to elegantly switch from a integer representation to a floating point representation when a provided number becomes too big.  However, there are situations where it's useful to check whether a given value falls within the bounds of an unambiguous integer -- what JavaScript refers to as `safe integers`.

ES6 now provides a few helper methods to accomplish this.  The first pair are constants indicating the minimum and maximum safe number values, `Number.MIN_SAFE_INTEGER` and `Number.MAX_SAFE_INTEGER`:

```js
// Get minimum safe number.
console.log(Number.MIN_SAFE_INTEGER); // -9007199254740991
// Get maximum safe number.
console.log(Number.MAX_SAFE_INTEGER); // 9007199254740991
```

As a convenience, ES6 also includes the `Number.isSafeInteger()` static function, which does the work of checking if the passed argument falls within the bounds of the minimum and maximum safe integer values:

```js
// Check if 12345 is safe.
console.log(Number.isSafeInteger(12345)); // true
// Check if 2^53 is safe.
console.log(Number.isSafeInteger(Math.pow(2, 53))); // false
// Check if 2^53 - 1 is safe.
console.log(Number.isSafeInteger(Math.pow(2, 53) - 1)); // true
```

### Math.trunc()

The new `Math.trunc()` static function saves you the trouble of performing calculations with `Math.floor()` when trying to retrieve only the integer value of a decimal number:

```js
// Truncate 12.34.
console.log(Math.trunc(12.34)); // 12
// Truncate -12.34.
console.log(Math.trunc(-12.34)); // -12

// Truncate 0.987.
console.log(Math.trunc(0.987)); // 0
// Truncate -0.987.
console.log(Math.trunc(-0.987)); // -0
```

### Math.sign()

Lastly, the `Math.sign()` static function allows us to retrieve the sign of the passed argument.  `Math.sign()` will attempt to convert non-numbers into numbers before checking the sign, if possible.  Moreover, `Math.sign()` can only return five possible values, which represent the five possible signs a number can have in JavaScript:

- `0`: Positive zero.
- `-0`: Negative zero.
- `1`: Positive number.
- `-1`: Negative number.
- `NaN`: Not-A-Number.

Here we illustrate using various arguments to return all possible `Math.sign()` results:

```js
// Get sign of 12.34.
console.log(Math.sign(12.34)); // 1
// Get sign of -12.34.
console.log(Math.sign(-12.34)); // -1

// Get sign of 0.987.
console.log(Math.sign(0.987)); // 1
// Get sign of -0.987.
console.log(Math.sign(-0.987)); // -1

// Get sign of 0.
console.log(Math.sign(0)); // 0
// Get sign of -0.
console.log(Math.sign(-0)); // -0

// Get sign of NaN.
console.log(Math.sign(NaN)); // NaN
// Get sign of null.
console.log(Math.sign(null)); // 0
// Get sign of nothing.
console.log(Math.sign()); // NaN
```

Check out the revolutionary <a class="js-cta-utm" href="https://airbrake.io/languages/javascript_exception_handler?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-js">Airbrake-JS</a> error tracking tool, which quickly and easily integrates into any new or existing JavaScript application, providing you and your team with real-time error reporting and instant insight into exactly what went wrong.  `Airbrake-JS` works with all the latest JavaScript frameworks and supports full source map control, to ensure your error reports are as detailed as possible.  `Airbrake-JS` also allows you to easily customize error parameters and grants simple filtering tools so you only capture and report the errors you care about most.

---

__META DESCRIPTION__

Part 6 of our journey through the exciting new features introduced in the latest version of JavaScript, ECMAScript 6 (ES6).

---

__SOURCES__

- https://github.com/getify/You-Dont-Know-JS/tree/master/es6%20%26%20beyond
- https://github.com/lukehoban/es6features
- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Template_literals
- https://developer.mozilla.org/en/docs/Web/JavaScript/Reference/Classes