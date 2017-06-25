# JavaScript Errors - Using //@ to Indicate SourceURL Pragmas is Deprecated SyntaxError

Moving right along through our __JavaScript Error Handling__ series, today we'll be tackling the `Invalid Source Map Format` JavaScript error.  Although the `Invalid Source Map Format` error is technically a descendant of the base `SyntaxError` object, it applies to a _very_ specific case; when using a deprecated symbol to indicate the URL for a `source map`, which allows for easier debugging of obfuscated code.

In this article we'll explore a bit more about the `Invalid Source Map Format` error, including where it fits in the JavaScript `Exception` hierarchy, and what causes `Invalid Source Map Format` errors.  We'll also examine the concept of `source mapping` that is so tightly tied to `Invalid Source Map Format` errors, so let's get going!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object, or an inherited object therein.
- The [`SyntaxError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SyntaxError) object is inherited from the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object.
- The `Invalid Source Map Format` error is a specific type of [`SyntaxError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/SyntaxError) object.

## When Should You Use It?

While the error message itself from an `Invalid Source Map Format` error pretty much says everything you need to know about solving it (replace `//@` with `//#`), it doesn't explain anything beyond that quick fix.  To understand just what `Invalid Source Map Format` errors mean, we'll need to spend a bit of time examining [`source maps`](https://github.com/ryanseddon/source-map/wiki/Source-maps%3A-languages,-tools-and-other-info) in the context of JavaScript.

As many front-end web developers can attest, the name of the game for delivering solid web application experiences is often speed, speed, speed.  The longer it takes for a page to finish loading, the more likely it is the user will move onto something else.  Consequently, modern web development relies heavily on tools that help reduce load times, including a process known as [`minification`](https://en.wikipedia.org/wiki/Minification_(programming)).

Most commonly applied to JavaScript code, `minification` takes the full, origin source code and shrinks it down to the minimal number of characters necessary for the JavaScript engine to still execute the code identically to the original intention.  There are numerous tricks involved with `minification` that are well beyond the scope of this article, but in the simplest terms, `minifying` a JavaScript file typically performs two main tasks: Replaces long object names with single-character names and removes all unnecessary whitespace.

Since each character in a file represents a `byte` of data that must be sent from the server to the client, this `minification` process can dramatically reduce the overall file size of JavaScript files, prior to sending them to the client for execution and display of the web site.

Now, one common problem with `minification` is that, since the intent is to produce the smallest footprint of code as possible, `minified` code is generally unreadable to humans.  As an example, here's the full, original source code of the `code.js` file:

```js
function printError(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

function increment(x)
{
    return add(x, 1);
}

function add(x, y)
{
    console.log(`add.caller is ${add.caller}.`)
    console.log(`add.caller.name is ${add.caller.name}.`)
    return x + y;
}

try {
    var value = increment(1);
} catch (e) {
    if (e instanceof TypeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}

//# sourceMappingURL=code-min.js.map
```

So we have a few simple functions and then we call those functions to increment a number.  That's all easy to read for a human developer, but it could be shrunk down significantly by being `minified`.  After `minification`, our new `code-min.js` file has dropped from `560 bytes` to `272 bytes` in size; more than a 50% reduction!

Unfortunately, the code of our `minified` version is not very human-friendly:

```js
function c(a,b){console.log("["+(b?"EXPLICIT":"INEXPLICIT")+"] "+a.name+": "+a.message)}function d(a,b){console.log("add.caller is "+d.caller+".");console.log("add.caller.name is "+d.caller.name+".");return a+b}try{d(1,1)}catch(a){a instanceof TypeError?c(a,!0):c(a,!1)};
```

Since this isn't a very complicated example, one could still probably spend the time to parse this and figure out what is going on, but it's not efficient by any means.

This presents a problem for developers who don't have direct access to the source code when debugging inside their Chrome or Firefox browser: Viewing the source of most web pages (ours included) would simply show the `minified` code above, which is very difficult to debug with, especially compared to the original source code.

The solution is `source mapping`.  `Source mapping` provides a way of mapping the (often obfuscated) production code to the original source code that was authored by the developer.  

Keen observers may have noticed that our original `code.js` source file included a commented line at the bottom: `//# sourceMappingURL=code-min.js.map`.  This line tells our browser that we've created a `source map` file, along with the URL it can be found at.  If this statement is present, when a developer attempts to view the source code in their browser's debugger, the `source map` file provides a _translation between the `minified` code and the original source code_.

For example, here is the `source map` file (`code-min.js.map`) that we produced for this example:

```json
{
    "version":3,
    "file":"code-min.js",
    "lineCount":1,
    "mappings":"AAAAA,QAASA,EAAU,CAACC,CAAD,CAAQC,CAAR,CAAkB,CACjCC,OAAAC,IAAA,CAAY,GAAZ,EAAgBF,CAAA,CAAW,UAAX,CAAwB,YAAxC,EAAoD,IAApD,CAAyDD,CAAAI,KAAzD,CAAmE,IAAnE,CAAwEJ,CAAAK,QAAxE,CADiC,CASrCC,QAASA,EAAG,CAACC,CAAD,CAAIC,CAAJ,CACZ,CACIN,OAAAC,IAAA,CAAY,gBAAZ,CAA6BG,CAAAG,OAA7B,CAAuC,GAAvC,CACAP,QAAAC,IAAA,CAAY,qBAAZ,CAAkCG,CAAAG,OAAAL,KAAlC,CAAiD,GAAjD,CACA,OAAOG,EAAP,CAAWC,CAHf,CAMA,GAAI,CAVOF,CAAA,CAWeC,CAXf,CAAO,CAAP,CAUP,CAEF,MAAOG,CAAP,CAAU,CACJA,CAAJ,WAAiBC,UAAjB,CACIZ,CAAA,CAAWW,CAAX,CAAc,CAAA,CAAd,CADJ,CAGIX,CAAA,CAAWW,CAAX,CAAc,CAAA,CAAd,CAJI;",
    "sources":["code.js"],
    "names":["printError","error","explicit","console","log","name","message","add","x","y","caller","e","TypeError"]
}
```

It is actually in the common `JSON` format.  The basic fields indicate the `minified` `file` along with the original `source` files, then mappings which are automatically generated.  The result is when `source mapping` is enabled in the browser, a developer can view the original source code rather than the `minified` version, so debugging can take place "within" the original code.

Whew!  Now that we have a basic understanding of what `source mapping` is, we can look back at our `Invalid Source Map Format` error and see where it comes from.  As mentioned above, the text of the error pretty much says it all.  Originally, the `source mapping` directive pragma was to be a comment followed by an at sign (`//@`).  However, due to some compatibility issues with Internet Explorer (isn't it always IE?), this was later changed to a comment followed by a pound sign (`//#`).

Therefore, if we were to change our original source code in `code.js` to include the deprecated pragma of `//@`, we'd get an `Invalid Source Map Format` error:

```js
function printError(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

function increment(x)
{
    return add(x, 1);
}

function add(x, y)
{
    console.log(`add.caller is ${add.caller}.`)
    console.log(`add.caller.name is ${add.caller.name}.`)
    return x + y;
}

try {
    var value = increment(1);
} catch (e) {
    if (e instanceof TypeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}

//@ sourceMappingURL=code-min.js.map
```

Sure enough, Firefox issues a warning (Chrome does not):

```
// FIREFOX
Using //@ to indicate sourceMappingURL pragmas is deprecated. Use //# instead
```

To dive even deeper into understanding how your applications deal with JavaScript Errors, check out the revolutionary <a class="js-cta-utm" href="https://airbrake.io/languages/javascript_exception_handler?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-js">Airbrake JavaScript</a> error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

---

__META DESCRIPTION__

A deep look at the Invalid Source Map URL SyntaxError in JavaScript, as well as a brief overview of source mapping techniques.

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
