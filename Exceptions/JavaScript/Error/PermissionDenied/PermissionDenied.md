As we continue through our in-depth __JavaScript Error Handling__ series, we'll take a closer look at one of the most basic errors in JavaScript, the `Permission Denied` error.  

Below we'll examine what exactly causes a `Permission Denied` error in JavaScript, and how to both handle and avoid this error in your own projects, so let's get started!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`] object, or an inherited object therein.
- While the `Permission Denied` error we're examining here is a base [`Error`] object, the `Error` object itself can be used and extended for custom error constructors.

## When Should You Use It?

Since the `Permission Denied` error does not have an explicit inherited constructor associated with it below the baseline `Error` object itself, it can be difficult to accurately determine when this particular error has reared its ugly head purely within the standard `try-catch` syntax of JavaScript.

That said, we'll begin examining the `Permission Denied` error with a simple example.  This error most commonly appears when JavaScript attempts to access an object for which it  has no permission, such as an `<iframe>` element that is attempting to load content from another domain.

For this simple example, our HTML contains a simple `<iframe>` with `src` property that violates the [`same-origin policy`]:

```html
<iframe id="myframe" src="https://en.wikipedia.org"></iframe>
```

Our JavaScript contains a simple `printError()` function which assists with formatting the output of a passed in error, telling us the error type, the error message, and allowing us to specify if the error was explicitly provided.

Additionally, we have a simple `try-catch` clause after attempting to access and output the first (and only) frame in our document, the above `<iframe>`:

```js
function printError(error, explicit) {
	console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);    
}

try {
    console.log(frames[0].document);
} catch (e) {
    if (e instanceof Error) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

The expected output is an `EXPLICIT` `Permission Denied` error, and sure enough that's what we see:

```
[EXPLICIT] Error: Permission denied to access property "document"
```

Since we are explicitly checking for our `e` error object to be an `instanceof` the `Error` object, it is expected that this is caught and sent to our `printError()` function as an `EXPLICIT` object reference.  If we change this so that our `catch` clause doesn't check for any explicit `instanceof` `Error`, the same error would be produced, but it would be `INEXPLICIT` by our own measurement:

```js
function printError(error, explicit) {
	console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);    
}

try {
  	console.log(frames[0].document);
} catch (e) {
    printError(e, false);
}
```

```
[INEXPLICIT] Error: Permission denied to access property "document"
```

The real problem with trying to capture the `Permission Denied` error comes about when we recognize that simply catching an `instanceof` the base `Error` object isn't enough, as this will catch all sorts of other errors that are unrelated.

For example, let's change our code a little bit to execute a recursion loop and trigger an `InternalError: Too much recursion`:

```js
function printError(error, explicit) {
	console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);    
}

function repeat() {
  repeat();
}

try {
  	repeat();
} catch (e) {
    if (e instanceof Error) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

Upon execution of our new code where we call the `repeat()` function, which recursively calls itself ad infinitum, we get the following output:

```
[EXPLICIT] InternalError: too much recursion
```

JavaScript has assumed that our explicit check for our error to be an `instanceof` `Error` is what we wanted, even though the actual error object that was caught was `InternalError`.

As you might imagine, the problem here is that `InternalError` (along with virtually all other error objects), is a descendant of the `Error` object, so capturing `Error` captures everything.

Therefore, in order to properly capture __only__ the `Permission Denied` error, we need additional logic within our `catch` clause.  Since using the `name` property for our error does us no good, the only other standard property to the `Error` prototype that's available is `message`, so we'll need to parse that to verify we're getting the `Permission Denied` error and not something else:

```js
function printError(error, explicit) {
	console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);    
}

try {
  	console.log(frames[0].document);  
} catch (e) {
	if (e.message.toLowerCase().indexOf('permission denied') == 0) {
    	printError(e, true);
    } else {
    	printError(e, false);
    }
}
```

While this is perhaps not the most efficient method, above we're simply checking whether our `Error` object `message` property text contains the phrase `permission denied` at the beginning, and if so, we can consider that the `EXPLICIT` error we're looking for.

The output is as expected:

```
[EXPLICIT] Error: Permission denied to access property "document"
```

[`Error`]: https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy
[`same-origin policy`]: https://developer.mozilla.org/en-US/docs/Web/Security/Same-origin_policy

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Error
