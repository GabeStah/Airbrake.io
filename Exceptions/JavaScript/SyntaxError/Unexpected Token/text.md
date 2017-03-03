# SyntaxError: Unexpected Token

Today, as we slice through the choppy seas of our mighty __JavaScript Error Handling__ series, we're going to smash ourselves against the rocks of the `Unexpected Token` error.  `Unexpected Token` errors are a subset of `SyntaxErrors` and, thus, will only appear when attempting to execute code that has an extra (or missing) character in the syntax, different from what JavaScript expects.

Throughout this adventurous article we'll explore the `Unexpected Token` error, down to its briny depths, including where it sits within the JavaScript error hierarchy.  We'll also look at how to deal with the many different types of `Unexpected Token` errors that may occur, depending on the scenario you find yourself in, so let's get a bit of that wind in our hair and get to it!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object, or an inherited object therein.
- The [`SyntaxError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SyntaxError) object is inherited from the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object.
- The `Unexpected Token` error is a specific type of [`SyntaxError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SyntaxError) object.

## When Should You Use It?

Like most programming languages, JavaScript tends to be fairly particular about its syntax and the way it is written.  While we don't have time to cover everything that JavaScript expects (far more information can be found in the [official documentation](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Guide/Grammar_and_types)), it's important to understand the basic premise of how JavaScript parsers work, and therefore what syntax is expected.

Statements in JavaScript are instructions that are concluded with a semicolon (`;`), while all spaces/tabs/newlines are considered whitespace, just as with most languages.  JavaScript code is parsed from left to right, a process in which the parser converts the statements and whitespace into unique elements:

- `Tokens`: These are words or symbols used by code to specify the logic of the application.  These include things like `+`, `-`, `?`, `if`, `else`, and `var`.  These are reserved by the JavaScript engine, and thus cannot be used incorrectly, or as part of variable names and the like.
- `Control characters`: A subset of `tokens` which are used to direct the "flow" of the code into code blocks.  These are typically used to maintain `scope`, with braces (`{ ... }`) and the like.
- `Line terminators`: As the name implies, a new line termination character.
- `Comments`: Comments are typically indicated using (2) forward-slash characters (`//`), and are not parsed by the JavaScript engine.
- `Whitespace`: Any space or tab characters that _do not_ appear within a string definition.  Effectively, if it can be removed without changing the functionality of the code, it is whitespace.

Therefore, when JavaScript parses our code, behind the scenes it's converting everything into these appropriate characters, then the engine attempts to execute our statements in order.  However, in situations where the syntax is wrong (often) due to a typo, we might come across an `Unexpected Token` error, indicating that the parser thinks there should be another element in a particular place, rather than the `token` that it found there.

For example, here we have a very simple call to the `Math.max()` method, which accepts a list of numeric arguments and returns the largest number from that list.  However, keen eyes will notice that we've accidentally included an additional comma (`,`) after our second argument (`2,`), which will be trouble for our parser:

```js
// 1
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    // Extra comma in Math.max
    var value = Math.max(1, 2,);
    console.log(value);
} catch (e) {
    if (e instanceof SyntaxError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

As expected, JavaScript didn't appreciate this and wasn't sure how to properly parse it, so we get an `Unexpected Token` error as a result:

```
// CHROME
Uncaught SyntaxError: Unexpected token )

// FIREFOX
SyntaxError: expected expression, got ')'
```

Now, the problem is either one of two things:

- We wanted to list **three** arguments but forgot one: `Math.max(1, 2, 3)`.
- Or, we only wanted to list **two** arguments, and accidentally included the additional comma: `Math.max(1, 2)`.

By including that extra comma in our method call, JavaScript **expects** a third argument to be included between that final comma and the closing parenthesis.  The lack of that third argument is what causes the `Unexpected Token` error, and is why JavaScript is none too pleased.

As mentioned earlier, there are many different types of `Unexpected Token` errors, but rather than cover all the possibilities, we'll just take a look at one more.  The important takeaway is that JavaScript's parser expects tokens and symbols in a particular order, with relevant values or variables in between.  Often, `Unexpected Token` errors are just due to an accidental typo, but to help avoid these, it is quite beneficial to use a code editor that provides some form of auto-completion.  This is particularly useful when forming basic logical blocks or writing out method argument lists, since the editor will often automatically provide the necessary syntax surrounding your code snippet.

For example, to create the following snippet, my code editor tried to fix it for me, so I had to manually remove a `brace` (`}`) to get the desired result.  Here we just have a simple `if-else` block, but we're forgotten the closing `brace` (`}`) prior to the `else`:

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    var name = "Bob";
    if (name === "Bob") {
        console.log(`Whew, it's just ${name}`);
    else {
        console.log("Imposter!");
    }
} catch (e) {
    if (e instanceof SyntaxError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

When JavaScript parses this, it expects that `brace` character, but instead it gets the `else`:

```
// CHROME
Uncaught SyntaxError: Unexpected token else

// FIREFOX
SyntaxError: expected expression, got keyword 'else'
```

It's also worth noting, as some keen observers may have noticed, that even though we're using the standard trappings of error capturing via a `try-catch` block to grab an instance of our `SyntaxError`, this code is never executed.  The reason for this is that the parser finds our `SyntaxError` and reports on it _before our `catch` block is even evaluated_.  Such `Unexpected Token` errors _can_ be caught, but doing so typically requires execution of the two sections (problematic code versus `try-catch` block) to take place in separate locations.

To dive even deeper into understanding how your applications deal with JavaScript Errors, check out the revolutionary <a class="js-cta-utm" href="https://airbrake.io/languages/javascript_exception_handler?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-js">Airbrake JavaScript</a> error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

---

__META DESCRIPTION__

A deep look at the Unexpected Token Error in JavaScript, including a short examination of JavaScript's syntax best practices.

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
