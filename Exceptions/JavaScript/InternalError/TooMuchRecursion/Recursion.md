Today in our continue journey through our __JavaScript Error Handling__ series, we're examining the `InternalError` object known as the `Recursion` error.  

Below we'll explore the cause of a `Recursion` error in JavaScript, as well as how to manage and avoid this error in your own projects, so let's begin!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`] object, or an inherited object therein.
- The [`InternalError`] object is inherited from the [`Error`] object.
- The [`Recursion`] error is a specific type of [`InternalError`] object.

## When Should You Use It?

As the name of the `Recursion` error implies, this error will pop up when the JavaScript engine detects an excessive level of recursion.  The challenge with properly capturing and designing around hitting this recursive limit is trying to estimate what the JavaScript engine deems to be `"excessive"`.

As you'll likely recall, recursion occurs anytime a function _calls itself_ during execution.  Typically this is done as a means of iterating over a collection of some type, until a specific criteria is met, and then the recursive behavior is halted and execution continues on.

For our simple example, we'll be performing a basic countdown in the `console.log`, starting from the initial argument value down to zero:

```js
var countdown = function(value) {
    console.log(value);
    return (value > 0) ? countdown(value - 1) : value;
};
countdown(10);
```

This produces an expected output in the `console.log` of:

```
10
9
8
7
6
5
4
3
2
1
0
```

With this simple function, it's easy to see when and how our recursion will fail out, exiting the recursive process.  In this case, it's when `value > 0` is `false`; that is, when `value` is `0` or less.

Increasing the level of recursion, which in our example case means simply increasing our starting argument above `10`, we can start to push the envelope of the JavaScript engine and produce a `Recursion` error.

The difficulty, as previously mentioned, is determining how much recursion is considered `excessive` and will throw our `Recursion` error in any given JavaScript engine.

Let's modify our `countdown` example above to help us handle any potential errors we get, and then start upping the number of recursions until we break something.

Here we're trying `10000` recursions.  _Note: We've commented out the `console.log` output for now, just so we don't output thousands of items:_

```js
var printError = function(error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}
var countdown = function(value) {
    //console.log(value);
    try {
        if (value > 0) countdown(value - 1);
    } catch (e) {
        if (e instanceof InternalError) {
            printError(e, true);      
        } else {
            printError(e, false);      
        }
    }
};
countdown(10000);
```

As discussed, this behaves differently depending on the JavaScript engine.  In `Firefox 50`, this produces a `Recursion` error of the `explicit` type we expected (`InternalError`):

```
[EXPLICIT] InternalError: too much recursion
```

On the other hand, `Chrome 54` handles it just fine.  With a bit of testing, we can actually determine that each browser version has different limits on the level of allowed recursion.  Below you'll find that `Firefox 46` can handle up to `7705` recursions, but number `7706` produces a `Recursion` error.  This number increases slightly for `Firefox 50`, while `Chrome 54` allows for a much higher level of recursion before it gets upset and errors out.

| Browser | Acceptable Limit | Excessive Limit |
| --- | --- | --- |
| Firefox 46 | 7705 | 7706 |
| Firefox 50 | 7718 | 7719 |
| Chrome 54 | 31416 | 31417 |

The bottom line when planning around and dealing with potential `Recursion` errors is to limit the potential iterations to a manageable number that never approaches these limits.  Generally speaking, as we look back in time at older generations of browsers, [these limits are smaller and smaller](https://www.nczonline.net/blog/2009/05/19/javascript-stack-overflow-error/), so a developer creating a site for all users and platforms should plan for the worst case scenario.

[`Error`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Error
[`InternalError`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/InternalError
[`Recursion`]: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors/Too_much_recursion

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors/Too_much_recursion
