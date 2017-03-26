# What's New in ES6 JavaScript? - Part 1

The latest and greatest active version of JavaScript is `ES6` (aka `ECMAScript 6`, aka `ECMAScript 2015`).  [Most modern browsers now support the majority](https://kangax.github.io/compat-table/es6/) of the new features and capabilities offered with `ES6`, so we thought we'd try to show you just what cool new stuff you can do with the latest version of JavaScript.  There are many new features added to `ES6`, so we'll just cover a handful in this article, and try to touch on the remainders in future pieces, so let's get to it!

#### A Quick Note on Naming Conventions

For better or worse (most developers would argue for worse), JavaScript version naming has been a somewhat confusing mess lately.  JavaScript is generally standardized using the [`ECMAScript` language specification](http://www.ecma-international.org), which has been through roughly seven major versions since its creation in 1997.  However, version naming have been rather complicated by mixing between numeric iterations (e.g. `ECMAScript 6 / ES6`) and year-based iterations (e.g. `ECMAScript 2015`).  `ES6`, which was standardized in 2015, is often referred to by either the numeric or year-based version, which can often cause confusion.  However, the real issue with a year-based system is that the upcoming version, `ES7` or `ECMAScript 2016`, has not been standardized yet, well into 2017.

The consensus is likely to land on sticking with numeric naming moving forward, so hopefully `ES6` will move to `ES7`, and so forth down the line.

## Default Parameters

`ES6` introduces a much-beloved feature of many other languages: the ability to define the [default values](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Functions/Default_parameters) for parameters within your functions or methods.  Making use of this feature is rather simple; just include an equals sign after the parameter name, followed by the default value.  For example, here our `last` parameter for the `getFullName` function includes a default value of `"the Unknown"`:

```js
function getFullName(first, last = "the Unknown") {
    return `${first} ${last}`;
}
```

This makes it simple to provide values only for the parameters that are required, allowing the default to be inserted where necessary:

```js
console.log(getFullName("Alice"));
console.log(getFullName("Alice", undefined));
console.log(getFullName("Alice", "Jones"));
```

This produces the output of `"Alice the Unknown"` when no `last` argument is given:

```
Alice the Unknown
Alice the Unknown
Alice Jones
```


## Classes

`ES6` also introduces the class ability to create traditional `classes`, similar to many other object-oriented languages.  While behind-the-scenes JavaScript is effectively treating classes in `ES6` as `prototypes`, `classes` provide a clean new syntax and something that will be familiar to many people coming to `ES6` from other object-oriented languages.

The introduction of `classes` brings a lot of new syntax and possibilities, so we'll just start with a simple example, then discuss what's new:

```js
class Book {
    constructor(title, author, pageCount, wordCount) {
        this.title = title;
        this.author = author;
        this.pageCount = pageCount;
        this.wordCount = wordCount;                
    }

    get wordsPerPage() {
        return this.wordCount / this.pageCount;
    }

    static wordsPerPage(pageCount, wordCount) {
        return wordCount / pageCount;
    }    
}

class PaperbackBook extends Book {
    constructor(...args) {
        super(...args);
        this.coverType = "paperback";
    }
}
```

The obvious new addition is defining our `Book` class with: `class Book {}`.  We're also using a `constructor()` method, which defines how we initialize our class when the time comes, providing a number of properties for our `constructor()` parameters.

We've also included a `prototype method` (often known as a `class method` in other languages) which is a method attached to an `instance` of that class (as opposed to a `static method`, which cannot be accessed by an instance of that class object).  In this case, we're creating the `wordsPerPage()` `prototype method`.

```js
get wordsPerPage() {
    return this.wordCount / this.pageCount;
}
```

Since we're also using the [`get`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Functions/get) keyword, which informs JavaScript that this is a `getter`, `wordsPerPage` isn't technically a method, but, instead, is a `property`.  This means we call it without closing parentheses or passed in arguments: `book.wordsPerPage`.

We've next created a second class called `PaperbackBook`, which inherits from our `Book` class using the `extends` keyword:

```js
class PaperbackBook extends Book {
    constructor(...args) {
        super(...args);
        this.coverType = "paperback";
    }
}
```

Notice in our `constructor()` we're also using `...args` as our list of arguments, which are known as [`rest parameters`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Functions/rest_parameters).  `Rest parameters` simply represent an indefinite number of arguments, in the form of an array.  Therefore, when we create a new instance of `PaperbackBook`, since it `extends` `Book`, the `...args` argument will contain an array of all four actual arguments passed into it, which is what `Book` expects.

From there, we then make use of the [`super`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Operators/super) keyword, which allows us to reference the _parent_ of our `PaperbackBook` object.  Thus, combining `super` with `...args`, we can simply call `super(...args)` in our `constructor`, which will effectively call the `constructor` of the parent `Book` class, and pass all provided arguments along with it.  This makes it easy for us to extend the functionality of `PaperbackBook's` `constructor`, in this case simply by adding the `coverType` property.

With our classes defined, we create a new instance of each class, passing in values that represent two different books, then output the information to our console to ensure everything works as expected:

```js
var book = new Book("The Shining", "Stephen King", 512, 158720);
console.log(book);
console.log(`Words per page: ${book.wordsPerPage}`);
console.log(`Cover type: ${book.coverType}`);

var paperback = new PaperbackBook("The Name of the Wind", "Patrick Rothfuss", 722, 223820)
console.log(paperback);
console.log(`Words per page: ${paperback.wordsPerPage}`);
console.log(`Cover type: ${paperback.coverType}`);
```

The output:

```
Book {title: "The Shining", author: "Stephen King", pageCount: 512, wordCount: 158720}
Words per page: 310
Cover type: undefined
PaperbackBook {title: "The Name of the Wind", author: "Patrick Rothfuss", pageCount: 722, wordCount: 223820, coverType: "paperback"}
Words per page: 310
Cover type: paperback
```

Sure enough, we're able to output the particular class objects we created, along with the `wordsPerPage` property value that is a `getter`, and then also confirm that `coverType` is only defined for our `PaperbackBook` class instance, but not the `Book` class.

Lastly, we're also using the new `static` keyword to define a `static method` inside our `Book` class.  In fact, you may notice we're using the same name of `wordsPerPage`, so that we can use our `Book` class to make calls to `static methods`, without the need for a reference to a particular _instance_ of the class.  In this case, if we want to calculate the words per page from two numbers, without using a `Book` class instance, we can do so:

```js
// Testing static method.
console.log(`Static method words per page: ${Book.wordsPerPage(500, 50000)}`);
```

Using our `static method` of `wordsPerPage` here works just as expected:

```
Static method words per page: 100
```

## Block-Scoping and the `let` Keyword

Properly scoping localized variables has always been a bit frustrating, or at least abnormal, in JavaScript.  In most languages, including JavaScript, it is often necessary to create a `block scope`, which is basically a section of code that is separated and retains privacy from code outside of that block.  In `ES5` the common technique to create a `block scope` is to use an [immediately invoked function expression](https://en.wikipedia.org/wiki/Immediately-invoked_function_expression) (`IIFE`).  This is effectively a syntactic method of defining a function, which is itself surrounded by `parentheses`, which effectively tells the JavaScript engine that this function should be treated as a `grouped` entity.

For example, here's how we'd use an `IIFE` in `ES5` as a way to create a `block scope`, allowing us to privatize the different value assignments of our `name` variable:

```js
var name = "Alice";

(function immediateExecutionFunction(){
    var name = "Bob";
    console.log(`Name is ${name}.`);
})();

console.log(`Name is ${name}.`);
```

The result is that our first `console.log()` output is `Bob`, while our second, which is outside of the `block scope` of our `IIFE`, is `Alice`:

```
Name is Bob.
Name is Alice.
```

Without the `IIFE` here, we could try creating a `block scope` with a pair of braces (`{}`), like so:

```js
var name = "Alice";

{
    var name = "Bob";
    console.log(`Name is ${name}.`);
}

console.log(`Name is ${name}.`);
```

The problem is, our braces don't properly retain the private scope of our `name` assignment to `Bob` since we're using the `var` keyword.  Therefore, our output is `Bob` both times:

```
Name is Bob.
Name is Bob.
```

The proper way to do this in `ES6` is to continue using the braces to define a `block scope`, but to use the `let` keyword instead of `var` inside our `block scope`:

```js
var name = "Alice";

{
    let name = "Bob";
    console.log(`Name is ${name}.`);
}

console.log(`Name is ${name}.`);
```

The [`let`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Statements/let) keyword is an easy way to create a `block scope local variable` in `ES6`.  Thus, our `name` is properly retained inside our block, as well as outside:

```
Name is Bob.
Name is Alice.
```

To help you and your team with JavaScript development, particularly when dealing with unexpected errors, check out the revolutionary <a class="js-cta-utm" href="https://airbrake.io/languages/javascript_exception_handler?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-js">Airbrake JavaScript</a> error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

---

__META DESCRIPTION__

Part 1 of our exploration into the exciting new features introduced in the latest version of JavaScript, ECMAScript 6 (ES6).

---

__SOURCES__

- https://github.com/getify/You-Dont-Know-JS/tree/master/es6%20%26%20beyond
- https://github.com/lukehoban/es6features
- https://developer.mozilla.org/en/docs/Web/JavaScript/Reference/Classes