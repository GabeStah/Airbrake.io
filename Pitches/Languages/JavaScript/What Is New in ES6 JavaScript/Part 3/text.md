# ES6 JavaScript: What's New? - Part 3

In [Part 1](https://airbrake.io/blog/javascript/es6-javascript-whats-new-1) of our `ES6 JavaScript` series we tackled the strange naming conventions that ECMAScript is finally settling on, then took a look at new features within JavaScript like `default parameters`, `classes`, and `block-scoping` with the `let` keyword.  In [Part 2](https://airbrake.io/blog/javascript/es6-javascript-whats-new-part-two) we explored `constants`, `destructuring`, and `constant literals syntax`.  Today for Part 3 we'll cover just one particular (yet exciting) new feature known as `template literals`, so let's get to it!

## Template Literals

ES6 brings a new feature to JavaScript that most other programming languages have had for some time: `string interpolation`.  ES6 has named this feature somewhat interestingly, opting to call such interpolated strings `template literals`, yet the functionality is much the same as with other languages such as C#, Ruby, Python, and so forth.

A `template literal` is a new type of string literal that uses the backtick (`` ` ``) delimiter instead of the traditional single- or double-quotes.  By defining a string literal with backticks, we're now able to insert interpolated expressions directly inside the string literal.  These interpolated expressions are chunks of code that are evaluated inline as they're parsed, making string creation a (usually) cleaner affair.

For example, here we have a common method for generating a string in traditional JavaScript by concatenating strings together with the `+` character:

```js
// Traditional String Interpolation
var title = 'Robinson Crusoe';
var author = 'Daniel Defoe';

var output = 'Check out ' + title + ', written by ' + author + '.';
console.log(output); // "Check out Robinson Crusoe, written by Daniel Defoe."
```

Using the new backtick syntax to create a `template literal` in ES6, we can now recreate the same `output` string by using `${...}` syntax everywhere we want an interpolated expression.  In this case, we're merely inserting the variables of `title` and `author` as inline evaluations, which are inserted into those positions in the string:

```js
// ES6 Template Literals
var title = 'Robinson Crusoe';
var author = 'Daniel Defoe';

var output = `Check out ${title}, written by ${author}.`;
console.log(output); // "Check out Robinson Crusoe, written by Daniel Defoe."
```

In both cases the output is identical, but the latter string literal is much shorter and easier to read.

### Interpolated Expressions

While we just used simple string variables for interpolation in our previous example, the new `template literals` syntax allows us to insert _any_ valid expression within the `${...}` interpolation statement.  For example, here we've defined the `getYearsBetweenDates()` function, which does as the name describes.  Traditionally we'd still concatenate our evaluated statements with string literals to form our total `output string`:

```js
// Traditional Function Interpolation
function getYearsBetweenDates(a, b) {
    var milliseconds;
    // If no second parameter use current date.
    if (!b) b = new Date();
    // Make sure to subtract largest from smallest.
    a >= b ? milliseconds = a - b : milliseconds = b - a;
    // Reduce milliseconds to years and round.
    return Math.round(milliseconds / (1000 * 60 * 60 * 24 * 365));
}

var name = 'Alice';
var output = name + ' is ' + getYearsBetweenDates(new Date(1985, 1, 20)) + ' years old.';
console.log(output); // Alice is 32 years old.
```

With ES6 and `interpolated expressions` we can simply insert any inline expression we want within a `${...}` statement and the result is the same:

```js
// ES6 Function Interpolation
var name = 'Alice';
var output = `${name} is ${getYearsBetweenDates(new Date(1985, 1, 20))} years old.`;
console.log(output); // Alice is 32 years old.    
```

This also greatly simplifies inline express interpolations where we'd be performing mathematical calculations such as addition, which must use the same `+` symbol as our concatenation of strings:

```js
// Traditional Mathematical Expression Interpolation
var output = 'One plus two is ' + (1 + 2) + ', while three plus four, which is ' + (3 + 4) + '.';
console.log(output); // One plus two is 3, while three plus four, which is 7.
```

To ensure that the mathematical expression is evaluated separately from the concatenation of the strings and the resulting expression value, traditionally we have to surround our calculation in parentheses `(...)`.  With ES6, we can remove the parentheses entirely and just continue using the same `${...}` syntax as before:

```js
// ES6 Mathematical Expression Interpolation
var output = `One plus two is ${1 + 2}, while three plus four, which is ${3 + 4}.`;
console.log(output); // One plus two is 3, while three plus four, which is 7.
```

We can go even deeper if we want.  As it happens, template interpolated expressions can be _nested_, meaning we can write a `template literal` inside the interpolated expression of another template literal.  For example, maybe we have a function call that we pass a value to -- such as this `upper()` function below -- and we want to make a few calls to that function but pass in a variable one time, then a slightly modified variable the next.

For example, here we're trying to emphasize the name of `Alice` by making it uppercase, but the second time we call it we're showing possession by also adding an apostrophe plus "s" to the `name` variable before we pass it to the `upper` function.  The result is a fairly messy series of calls and concatenations in traditional JavaScript:

```js
// Traditional String Interpolation Within Expression Interpolation 
function upper(a) {
    return a.toUpperCase();
}
var name = 'Alice';
var output = upper(name) + ' sells seashells down by the seashore.  ' + upper( name + "'s" ) + ' special seashells are seagreen.';
console.log(output); // ALICE sells seashells down by the seashore.  ALICE'S special seashells are seagreen.
```

While this produces the output we want, we can dramatically simplify this with the new ES6 syntax by using a `template literal` inside our `interpolated expression` call within our _outer_ template literal, allowing us to (relatively) easily create the possessive form of our `name` noun for the output:

```js
// ES6 String Interpolation Within Expression Interpolation 
var name = 'Alice';
var output = `${upper( name )} sells seashells down by the seashore.  ${upper( `${name}'s` )} special seashells are seagreen.`;
console.log(output); // ALICE sells seashells down by the seashore.  ALICE'S special seashells are seagreen.
```

I leave it to you to determine when it is appropriate to use this new technique, but it's nice to know it's there.  Obviously there is some potential for ugly code if nesting occurs too frequently or too deeply within a single literal, so take care to ensure its use is warranted.

### Tagged Template Literals

Another feature that `template literals` provide in ES6 is known as `tagged template literals`, which allow you to parse the literal strings provided in a template literal through the use of a function.  It's a bit difficult to describe what that means, so it's best to just show an example and then we can talk about what's going on.

Here we have a function we'll be using to perform our tagging, aptly named `tag()`.  Let's ignore the logic of this for now until after we call this function via a tagged literal to see what it does:

```js
// ES6 Tagged Template Literals
function tag(strings, a, diff, b) {
    // Output the strings array.
    console.log(strings); // ["The number ", " is ", " than the number ", ".", raw: Array(4)]

    // Adjust difference verbiage if necessary.
    diff = 'greater';
    if (a < b) diff = 'less';

    // Return recompiled string.
    return `${strings[0]}${a}${strings[1]}${diff}${strings[2]}${b}${strings[3]}`;
}
```

Here we're calling our `tag` function as a `tagged template literal`, which means we're using the new syntax of: `` functionName`Tagged literal string` ``.  This calls our `tag()` function and passes our template literal as a series of parameters:

```js
var a = 1234;
var b = 5678;

var output = tag`The number ${a} is ${ true } than the number ${b}.`
console.log(output); // The number 1234 is less than the number 5678.
```

If we look back up at the `tag()` function definition we can see that we're expecting a total of four parameters: `strings`, `a`, `diff`, and `b`.  The first argument of a tag function will automatically contain an array of string values which make up the `template literal` string that was passed into it, separated by the `${...}` interpolators, if any exist.  For our example here we see that the `template literal` we passed to our tag function contained three `${...}` interpolations: `The number ${a} is ${ true } than the number ${b}.`

If we were to split our `template literal` string using each `${...}` interpolation statement as a separator, we'd have _four_ strings remaining (don't forget the final period which is a small string unto itself).  Therefore, `console.log(strings)` shows that the generated `strings` array contains all four of those string values automatically.

The remaining arguments of the tag function are simply the expression interpolations we included in our template literal.  In this case, `a` is equal to `1234` and `b` is equal to `5678`.  `diff` is the interesting parameter because when we call the `tag()` function, we just included the value of `true` for that interpolation.  The reason is that we want to "insert" our own value in place of this `diff` parameter using some simple logic to check if the number `a` is `greater than` or `less than` the number `b`.  In this case `a` is less than `b`, so we set `diff` to `less`.

Finally, for the return statement we concatenate our full `template literal` string once again by alternating through all four values in `strings` with each of the other three parameters.  The result is our intended, formatted string output: `"The number 1234 is less than the number 5678."`

Just to show the logic works as expected, we change the values of `a` and `b` then call `tag()` again:

```js
var a = 99999;
var b = 1;

var output = tag`The number ${a} is ${ true } than the number ${b}.`
console.log(output); // The number 99999 is greater than the number 1.
```

As expected, this time our `diff` value changes to `greater` since `9999` is by far the larger of the two.

### Raw Strings

The last feature worth noting about the new `template literals` functionality is `raw strings`.  Keen observers may have noticed in the previous example discussing `tagged template literals` that the first parameter of our tag function (`strings`) actually contained _five_ values: our four delimited string values from our `template literal` string, _plus_ a value with the key `raw` that was its own array.  The `raw` value of the `strings` array is another copy of all the delimited strings from our `template literal` that was passed in, _except_ these are the raw strings, which means they inherently ignore any [escape sequences](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Guide/Grammar_and_types#Using_special_characters_in_strings) which might be present.

For example, here we have another simple `tag()` function and we're passing in a `template literal` that contains a number of tab (`\t`) escape sequences between each word:

```js
// ES6 Raw Strings in Tagged Template Literals
function tag(strings, ...values) {
    console.log(strings[0]); // These	words	are	separated	by	tabs	.
    console.log(strings.raw[0]); // These\twords\tare\tseparated\tby\ttabs\t.
}

tag`These\twords\tare\tseparated\tby\ttabs\t.`
```

While the basic forms of the `strings` array evaluates these escape sequences as normal, thereby inserting tabs between each word, the `strings.raw` array ignores all such sequences, giving us a string with many `\t` characters within it.

To help you and your team with JavaScript development, particularly when dealing with unexpected errors, check out the revolutionary <a class="js-cta-utm" href="https://airbrake.io/languages/javascript_exception_handler?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-js">Airbrake JavaScript</a> error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

---

__META DESCRIPTION__

Part 3 of our journey through the exciting new features introduced in the latest version of JavaScript, ECMAScript 6 (ES6).

---

__SOURCES__

- https://github.com/getify/You-Dont-Know-JS/tree/master/es6%20%26%20beyond
- https://github.com/lukehoban/es6features
- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Template_literals
- https://developer.mozilla.org/en/docs/Web/JavaScript/Reference/Classes