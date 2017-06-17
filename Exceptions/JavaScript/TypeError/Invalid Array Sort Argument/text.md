# JavaScript Errors - Invalid Array Sort Argument TypeError

Today in the continued journey through our [__JavaScript Error Handling__](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) series we'll be exploring the `Invalid Array Sort Argument TypeError`.  The `Invalid Array Sort Argument TypeError` is thrown by the Firefox JavaScript engine in a very specific situation: When the argument passed to the `Array.prototype.sort()` method isn't a valid function.

In this article we'll look into the `Invalid Array Sort Argument TypeError` a bit more to see where it resides within the JavaScript `Exception` hierarchy, and we'll also take a look at a few simple code examples that should show you how `Invalid Array Sort Argument TypeErrors` might occur and what changes in behavior you may see depending on your browser/JavaScript engine.  Let's get goin'!

## The Technical Rundown

- All JavaScript error objects are descendants of the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object, or an inherited object therein.
- The [`TypeError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/TypeError) object is inherited from the [`Error`](https://airbrake.io/blog/javascript-error-handling/javascript-error-hierarchy) object.
- The `Invalid Array Sort Argument TypeError` is a descendant of [`TypeError`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/TypeError) object.

## When Should You Use It?

The [`Array.prototype.sort()`](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/sort?v=control) is a fairly simple method -- it attempts to sort the instanced array _in place_ (meaning it doesn't need to return a value to be used).  By default the sort logic is based on string Unicode code point comparisons, which is commonly realized in the form of alphanumeric sorting.

Optionally, `Array.prototype.sort()` accepts a single argument that should be a `function` that will be executed to compare each element in the array and sort it.  The inner sort function that is provided as the argument has two parameters that represent the two array elements to be compared.  

Internally JavaScript only knows (or cares about) the indices of the two current elements being compared.  Not all sorting algorithms are the same, and certainly not all JavaScript engine implementations of sort are the same either, but it's somewhat interesting to dig deeper into how one particular implementation works to see what's going on behind the scenes.

For example, the Chrome JavaScript engine uses the `MergeSort` algorithm to perform its internal sorting comparisons.  To see this in action we have a simple sorting example.  Here we've declared a numeric array called `data` and a function within `sort()`.  `sort()` starts by outputting the whole array in its current form, then outputs the values of the comparison that is being made, and then finally makes the comparison of the two parameters `a` and `b`.  Checking if `a` is greater than or equal to `b`, as seen here, will cause the resulting array to be in ascending order, while inversing the check to `a <= b` will result in descending order:

```js
let printError = function (error, explicit) {
    console.log(`[${explicit ? 'EXPLICIT' : 'INEXPLICIT'}] ${error.name}: ${error.message}`);
}

try {
    // Create a list of unordered data.
    let data = [5, 7, 4, 3, 2, 1, 8, 6];

    // Sort function that performs basic sorting.
    let sort = (a, b) => {
        console.log(data.join(', '));                
        // Output comparison chart.
        console.log(`(${a} >= ${b}) is ${a >= b}`);
        // Sort all other elements as usual.
        return a >= b;
    };

    // Output original list.
    console.log(`Original: ${data.join(', ')}`);
    // Call Array.prototype.sort and use sort to process.
    data.sort(sort);
    // Output sorted list.
    console.log(`Sorted: ${data.join(', ')}`);
} catch (e) {
    if (e instanceof TypeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

The result is a reasonably formatted output that shows the transition that our array goes through and the various elements that are being compared with each iteration:

```js
Original: 5, 7, 4, 3, 2, 1, 8, 6
5, 7, 4, 3, 2, 1, 8, 6
(5 >= 7) is false
5, 7, 4, 3, 2, 1, 8, 6
(7 >= 4) is true
5, 7, 7, 3, 2, 1, 8, 6
(5 >= 4) is true
4, 5, 7, 3, 2, 1, 8, 6
(7 >= 3) is true
4, 5, 7, 7, 2, 1, 8, 6
(5 >= 3) is true
4, 5, 5, 7, 2, 1, 8, 6
(4 >= 3) is true
// ...
1, 2, 3, 4, 5, 7, 8, 6
(8 >= 6) is true
1, 2, 3, 4, 5, 7, 8, 8
(7 >= 6) is true
1, 2, 3, 4, 5, 7, 7, 8
(5 >= 6) is false
Sorted: 1, 2, 3, 4, 5, 6, 7, 8
```

I've trimmed out a number of lines to keep things a bit more concise, but as it happens to sort this particular array requires `19` iterations of our `sort()` function.  What you might notice is that each output of our array from within the `sort()` function only changes one value at most.  In fact, since this uses a `merge sort` we can see just how that algorithm performs sorting.

The basic functionality of a merge sort relies on the concepts of `divide and conquer`.  We start by (effectively) dividing our initial array in half, then each of those halves are split again down into quarters, then all four are split into eighths, and so on until each sub-array contains a maximum of `one` element.  This halving/splitting is called the `divide` phase.

To illustrate we'll take our `data` array from before:

```js
[5, 7, 4, 3, 2, 1, 8, 6]
```

To begin the `divide` process we take the first index (`0`) and the last index (`7`), add them together, divide by `2`, then round that down to the nearest whole number.  This would give us: `Math.floor((0 + 7) / 2)` or `3` as the answer.  This value is the `middle` index and tells us where we create our `divide` inflection point within the array for the first split into multiple sub-arrays.  The `left` side sub-array goes from `0` to `middle index` and the `right` sub-array is from `middle index + 1` to `last index`.  This gives us:

```js
let data = [5, 7, 4, 3, 2, 1, 8, 6]
[5, 7, 4, 3] is first left sub-array
[2, 1, 8, 6] is first right sub-array
```

Now this `divide` process keeps repeating until we have sub-arrays that contain only a single element.  What may feel strange about this first `divide` phase is that there is _no sorting_ going on at all during this phase.  It's just a recursive process of splitting the array into halves over and over until we're left with single elements.

Once elements are all within single-size sub-arrays they can be `merged`.  This process simply takes the sub-arrays that were generated and now `merges` them together, recursively and back **up** the chain of size.  For example our first sub-array pair would be: `[5] [7]`

The process of `merging` two sub-arrays together is fairly simple:

1. Take each pair of the smallest (single-element) sub-arrays (`left` and `right`) and add both elements to a new `merged` sub-array that holds both elements, ensuring that the smaller of the two elements is first.

For example, if we take our single-element sub-arrays and `merge` the pairs we get:

```js
[5] [7] becomes [5, 7]
[4] [3] becomes [3, 4]
[2] [1] becomes [1, 2]
[8] [6] becomes [6, 8]
```

Now that we're dealing with multi-element arrays things get a bit trickier but still follow simple rules.

2. Take the first unused element of the left array (known as `a`) and the first unused element of the right array (known as `b`) and compare the two.
3. Push whichever of `a` or `b` is the lesser value onto our new `merged` array and consider that particular index of that sub-array to be `used` this iteration.

For example, we'd compare and `merge` our pair sub-arrays like so:

```js
[5, 7] [3, 4] compares 5 to 3, 3 is used and pushed onto merge array: [3]
[5, 7] [4] compares 5 to 4, 4 is pushed: [3, 4]
[5, 7] compares 5 to 7, 5 is pushed: [3, 4, 5]
[7] is last unused element, 7 is pushed: [3, 4, 5, 7]
```

(Note: The same process happens with the right side sub-array pairs, but we won't go through those here.)

4. Repeat this process for each iteration by doubling the size of the `merged` sub-arrays.
5. Eventually you'll reach the final iteration where you have two sub-arrays that are both the halved sizes of the original split data array.  Repeat comparisons of the first element of each sub-array and added to a new `merged` array and you'll end up with the final, sorted result!

For example, `merging` our final two sub-arrays would look something like this:

```js
[3, 4, 5, 7] [1, 2, 6, 8] compares 3 to 1, 1 is pushed: [1]
[3, 4, 5, 7] [2, 6, 8] compares 3 to 2, 2 is pushed: [1, 2]
[3, 4, 5, 7] [6, 8] compares 3 to 6, 3 is pushed: [1, 2, 3]
// etc.
```

Whew!  Now that we understand how `merge sort` works in most JavaScript implementations we'll have a better grasp of what `Array.prototype.sort()` expects for its arguments and return value.  The (optional) argument provided to `Array.prototype.sort()` is expected to be a `function` that itself contains up to two parameters.  This inner sort function begins processing by grabbing the first two elements (indices `0` and `1`) of the array and then executes the sort function.  The return value of the sort function determines if JavaScript performs any change to array elements.  If the sort function returns a value of `true` (or anything that is `"truthy"`) then the _first_ parameter element (which we'll call `a`) is assumed to be 'greater than' the _second_ parameter element (`b`).  The result is that JavaScript will process the swapping of `a` and `b` as normal.

While we can implement our own `sort` function quite easily, what happens if we forgo a custom sort function entirely?  As mentioned, since the function argument is optional leaving it out simply performs the standard alphanumeric ascending sort that we saw above.

Now let's take this a step further and see what happens if we pass a **non-function** object type argument to the `Array.prototype.sort()` method:

```js
try {
        // Create a list of unordered data.
    let data = [5, 7, 4, 3, 2, 1, 8, 6];

    // Sort function is not a function type.
    let sort = 'sort';

    // Output original list.
    console.log(`Original: ${data.join(', ')}`);
    // Call Array.prototype.sort and use sort to process.
    data.sort(sort);
    // Output sorted list.
    console.log(`Sorted: ${data.join(', ')}`);
} catch (e) {
    if (e instanceof TypeError) {
        printError(e, true);
    } else {
        printError(e, false);
    }
}
```

Here we once again see the fundamental differences between JavaScript engines on Chrome versus Firefox.  Chrome has no problem with the syntax above: Even though we didn't provide a valid function as the argument to the `Array.prototype.sort()` call, Chrome doesn't care and just ignores that argument entirely, thereby assuming a normal alphanumeric ascending sort was intended.  Meanwhile, Firefox doesn't like this at all and spits out an `Invalid Array Sort Argument TypeError` at us:

```js
// CHROME
Original: 5, 7, 4, 3, 2, 1, 8, 6
Sorted: 1, 2, 3, 4, 5, 6, 7, 8

// FIREFOX
Original: 5, 7, 4, 3, 2, 1, 8, 6
[EXPLICIT] TypeError: invalid Array.prototype.sort argument
```

That about does it for the `Invalid Array Sort Argument TypeError` and exploring a little bit about how JavaScript sorting works.  Check out this great [Khan Academy tutorial](https://www.khanacademy.org/computing/computer-science/algorithms/merge-sort/a/divide-and-conquer-algorithms) on merge sorting to learn more!

To dive even deeper into understanding how your applications deal with JavaScript Errors, check out the revolutionary <a class="js-cta-utm" href="https://airbrake.io/languages/javascript_exception_handler?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-js">Airbrake JavaScript</a> error tracking tool for real-time alerts and instantaneous insight into what went wrong with your JavaScript code.

---

__META DESCRIPTION__

A detailed look at the Invalid Array Sort Argument TypeError in JavaScript, with code samples and an exploration of JavaScript merge sorting.

---

__SOURCES__

- https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Errors
