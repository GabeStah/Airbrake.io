# 5 Frustrating .NET Issues and How to Fix Them

While .NET developers must handle numerous frustrations and many potential issues throughout the entire software development life cycle, we've honed in on five issues in particular that can really grind the mental gears.  Throughout this article we'll examine these potential issues in detail, providing both code examples and, hopefully, some advice to help you avoid these frustrations in your own projects.  Let's get to it!

## Improper Collection Types

.NET provides a plethora of different collection types, including: `Dictionary<TKey, TValue>`, `HashSet<T>`, `List<T>`, `Stack<T>`, and many more.  Unfortunately, for many developers, this abundance of choices can often lead to writing code that may be inefficient, or even prone to issues and exceptions, simply due to choosing the wrong collection type during initial development.  This is particularly true when developers are still somewhat new to the language, but have just enough experience to become accustomed and comfortable to a particular style of coding.

Therefore, it's always best to take a moment to truly consider what type of collection is best suited for the task at hand.  Luckily, in many cases, .NET already provides a built-in collection type to meet your needs.  And, of course, when an exact fit doesn't exist, you can always create your own collection class.

For example, if we need an application to track and manage callers into a telephone support system, we'd want a convenient collection to track the order that callers came in.  We'd also want to accept the calls in the order they were received, so this is a perfect example of a first-in, first-out (`FIFO`) system, which is exactly what the `Queue<T>` class is intended to handle:

```cs
static void QueueExample()
{
    Queue<string> callers = new Queue<string>();
    callers.Enqueue("Adam");
    callers.Enqueue("Barbara");
    callers.Enqueue("Chris");
    callers.Enqueue("Danielle");
    callers.Enqueue("Eric");

    foreach(string caller in callers)
    {
        Log(caller);
    }

    Log("-----------------");

    Log($"Next item to dequeue: {callers.Peek()}");
    Log($"Dequeuing {callers.Dequeue()}");
    Log($"Next item to dequeue: {callers.Peek()}");
    Log($"Dequeuing {callers.Dequeue()}");
}

static void Log(object value)
{
    #if DEBUG
        System.Diagnostics.Debug.WriteLine(value);
    #else
        Console.WriteLine(value);
    #endif
}
```

Here we've created a new `Queue<string>` object named `callers`, and added the names of the callers as they came in using the `Enqueue()` method.  We output every caller, to verify that their order is correct, then we start looking at the next collection item to be dequeued using the `Peek()` method.  As we intended, the next item in the collection to be removed is always the oldest, or first added, starting with `Adam`, then `Barbara`, and so on.  The output confirms the results:

```
Adam
Barbara
Chris
Danielle
Eric
-----------------
Next item to dequeue: Adam
Dequeuing Adam
Next item to dequeue: Barbara
Dequeuing Barbara
```

While this is one simple example, it illustrates the importance of carefully examining all the possible collection types available through .NET, and picking the right tool for the job.

## Ignoring Compiler Warnings

We've all been there: We're deep into the zone of coding a new feature, and Visual Studio keeps blathering on with warning after warning in the compiler with each new build.  Unlike compiler `errors`, `warnings` don't prevent the build from completing nor stop execution or debugging, so in many cases, it's all too easy to ignore these annoying warnings.  Their cause can be fairly wide ranging, often out of the control of a single developer, but it's not all that uncommon for developers to completely turn off indications in their editor when compiler warnings occur in the first place, electing only to concern themselves with build-breaking errors.

This is a dangerous practice and, whenever possible, should largely be avoided.  Cleaning up all compiler `warnings` will not only reduce stress and headaches during each subsequent build, but in some cases, ignoring compiler warnings can actually lead to unintended issues or even exceptions.

Take this basic example, where we have a simple `Author` class with a constructor that accepts the associated `id` and `name` value of the author, before passing them along to the class instance variables `index` and `name`:

```cs
static void CompilerExample()
{
    Author author = new Author(666, "Stephen King");
    Console.WriteLine($"Index is {author.index} for {author.name}.");
}

public class Author
{
    public int Id;
    public int index;
    public string name;

    public Author(int id, string name)
    {
        this.index = Id;
        this.name = name;
    }
}
```

Unfortunately, something has gone wrong, and rather than having the passed `index` value of `666`, our author's `index` is reported as `0` in the output:

```
Index is 0 for Stephen King.
```

If you look closely, you'll notice that we have a problem: We're assigning the `Id` (capitalized) value to `this.index`, when we actually intended it to be the `id` (lowercase) value instead.  Perhaps this may be considered an unlikely scenario, but the reason we potentially got to this point, and without our code editor noticing, is because when we initially created the `Author` class, we added the `public int Id;` line.  This produced a compiler warning because `Id` was never used, but once we added the constructor and accidentally assigned `Id` to our `.index` instead of `id`, the compiler saw no problems and away we went.

## String Comparisons and Encoding

It is common, particularly among developers coming to .NET with experience from other languages, to run into trouble when attempting to perform string comparisons.  One might argue that there's nothing particularly tricky about comparing obviously similar strings: `"Hello" == "Hello"` is obviously true.  The potential for problems arise when dealing with characters from different languages or locales, or even with many Unicode characters depending on the executing operating system.

Most people will perform string comparisons in code with a simple [`==`](https://msdn.microsoft.com/en-us/library/53k8ybth.aspx) equality operator.  In .NET, there are two core types of `string comparison` that can be made, as specified by the [`StringComparison`](https://msdn.microsoft.com/en-us/library/system.stringcomparison(v=vs.110).aspx) enumeration: `Ordinal` and `CurrentCulture`.

An `Ordinal` comparison will compare the numeric (Unicode code point) values of each character in the string.

`CurrentCulture` takes into account the user locale of the system when making comparisons.  `CurrentCulture` is typically for when you need to display or output the results of the operation to the user.  For example, if you're building a sorted list of items, performing sort comparisons by `CurrentCulture` will ensure that the alphabetizing is based on cultural, linguistic comparisons of the list items, rather than purely by the byte values.

`StringComparison` also features two additional types, which are simple case-insensitive variants of `Ordinal` and `CurrentCulture`.

To see this in action, we have some basic sample code comparing the Old English `encyclopædia`, with the modern English version, `encyclopaedia`:

```cs
using System;

namespace code
{
    class Program
    {
        static void Main(string[] args)
        {
            string old = "encyclopædia";
            string modern = "encyclopaedia";

            // False outputs.
            Log(old == modern);
            Log(old.Equals(modern));
            Log(old.Equals(modern, StringComparison.Ordinal));
            Log(old.Equals(modern, StringComparison.OrdinalIgnoreCase));

            // True outputs.
            Log(old.Equals(modern, StringComparison.CurrentCulture));
            Log(old.Equals(modern, StringComparison.CurrentCultureIgnoreCase));
        }

        static void Log(object value)
        {
            #if DEBUG
                System.Diagnostics.Debug.WriteLine(value);
            #else
                Console.WriteLine(value);
            #endif
        }
    }
}
```

As indicated by the comments in the code, the first four comparisons result in a `False` value, while the final two comparisons are `True`.  As it happens, the first three comparisons are functionally identical, and just different ways of writing the same code:

```cs
// All of these are equivalent.
Log(old == modern);
Log(old.Equals(modern));
Log(old.Equals(modern, StringComparison.Ordinal));
```

This is because the `==` equality operator in C# defaults to using the `StringComparison.Ordinal` type comparator.  In our case, it's obvious that the Unicode character codes for `æ` (`U+00E6`) and `a` (`U+0061`) are different, so the `Ordinal` comparison fails when it hits that point in the strings.

However, when using the `CurrentCulture` comparison, .NET is smart enough to recognize that `æ` is equivalent to the two-character combination in modern English of `ae`, so our strings are considered equivalent.

This can also be a major problem not just for linguistic or cultural differences, but even based on operating systems.  For example, when evaluating the directory path of a file on a Windows system, it's common to run into a problem where a Unicode character used in a comparison _doesn't even exist_ within the Windows character sorting weight tables.  Instead, the characters will be evaluated as blank or `null`, which causes all sorts of potential problems within the code.

The takeaway here is that it's always preferable to explicitly call the `String.Equals()` method and pass in the appropriate `StringComparison` type that is desired.

## Underlying Object Types with LINQ

[LINQ](https://msdn.microsoft.com/en-us/library/bb308959.aspx) is an extremely powerful tool in .NET development, which allows for the use of general-purpose query syntax to evaluate all manner of data types, from a wide range of sources.  Essentially, if the data set is `IEnumerable`, it can be queried using `LINQ`.

One common issue, however, is that developers sometimes neglect (or are simply unaware) `LINQ's` behavior, based on the underlying object types of the data in question.  While `LINQ` aims to be formatted and syntactically identical no matter the data set, the actual behavior (and thus results) can sometimes differ depending on the type of object that is behind the scenes.

To illustrate, here we have a simple example with an `Author` class with a few associated values, including the `publicationStatus`.  We've created a `List` of `Authors` and added three instances of our `Author` class to our list.  Finally, we're using `LINQ` to query all `Authors` where the `publicationStatus` is equal to `published`, then count up the total number and output the result:

```cs
static void LINQExample()
{
    List<Author> authors = new List<Author>();

    authors.Add(new Author(1, "Stephen King", "published"));
    authors.Add(new Author(2, "Herman Melville", "Published"));
    authors.Add(new Author(3, "John Doe", "not published"));

    int publishedCount = (from author in authors
                          where author.publicationStatus == "published"
                          select author).Count();

    Console.WriteLine($"Number of published authors: {publishedCount}");
}

public class Author
{
    public int id;
    public string name;
    public string publicationStatus;

    public Author(int id, string name, string publicationStatus = "published")
    {
        this.id = id;
        this.name = name;
        this.publicationStatus = publicationStatus;
    }
}
```

The result of our output shows that only _one_ author is published:

```
Number of published authors: 1
```

This should make sense, since we're performing a comparison within memory using the `==` (i.e. `Ordinal` `StringComparison` type), so the fact that `Herman Melville's` `publicationStatus` is accidentally capitalized (`Published`) means it isn't matched in our query, so only one record is found.

However, the potential issue comes in when we start dealing with data from different sources, and thus with different underlying object types.  If our `LINQ` statement wasn't referencing in-memory classes, but instead was a reference to an `SQL` table that contained our author data, .NET would convert that statement into `T-SQL` before evaluating it.  The result would be that `Published` and `published` would be considered equivalent, since the `T-SQL` comparator that is used is case insensitive.

The lesson here is that results can differ depending on the underlying object to which your `LINQ` statement refers.  Therefore, it's critical to understand whether your `LINQ` statement will be converted to anything other than `C#` (or `VB.NET`, as the case may be) before being executed.

## Unexpected or Missing Exceptions

While this could be an entire article unto itself, one commonly frustrating issue for .NET developers is properly dealing with exceptions.  As of the current .NET version at the time of writing, there are some 300+ possible `Exception` class-inherited objects that can occur during execution, dwarfing the number in nearly every other popular language.  That's not to suggest that this abundance of potential exceptions makes .NET superior in anyway, as it can, in fact, be rather overwhelming for both new and experienced developers alike.

Developers will often write code that simply _avoids_ the potential for throwing exceptions in the first place.  In some cases, this is unintentional on the part of the developer, and is simply due to the multiple methods .NET provides for performing functionally equivalent code.  In one case, the code will produce an `Exception` if it fails, and in the other case, it will not.

For example, two common methods are `Int32.Parse()` and `Int32.TryParse()`, both of which attempt to parse a string argument and convert the value to a valid integer.  However, the fundamental difference is that `TryParse()` **will not** throw an exception if it fails; it will merely produce a `bool` indicating if the parse was successful or not.  Conversely, the `Parse()` method will throw an exception if the parse fails.

Therefore, here we have some simple code where we loop through an array of `strings`, first trying `TryParse()` then `Parse()`:

```cs
static void ParseExample()
{
    string[] values = { "123", "-123", "123.0", "01AD" };

    foreach (var value in values)
    {
        Log($"TryParse of {value} is: {Int32.TryParse(value, out int number)}");
    }

    try
    {
        foreach (var value in values)
        {
            Log($"Parse of {value} is: {Int32.Parse(value)}");
        }
    }
    catch (System.FormatException e)
    {
        Log(e);
    }
}

static void Log(object value)
{
    #if DEBUG
        System.Diagnostics.Debug.WriteLine(value);
    #else
        Console.WriteLine(value);
    #endif
}
```

The produced output tells the whole story:

```
TryParse of 123 is: True
TryParse of -123 is: True
TryParse of 123.0 is: False
TryParse of 01AD is: False
Parse of 123 is: 123
Parse of -123 is: -123
System.FormatException: Input string was not in a correct format.
   at System.Number.StringToNumber(String str, NumberStyles options, NumberBuffer& number, NumberFormatInfo info, Boolean parseDecimal)
   at System.Number.ParseInt32(String s, NumberStyles style, NumberFormatInfo info)
   at code.Program.ParseExample() in Program.cs:line 69
```

Even though we didn't surround the `TryParse()` block with a `try-catch` block in case of errors, no exceptions were thrown, as it simply told us that `TryParse()` failed for the final two values.  On the other hand, we caught a `FormatException` when we hit the third value (`123.0`) when trying to use `Parse()` on it, because that method doesn't allow a decimal indicator in the string.

There are two major takeaways here.  The first is that it's typically the best course of action to use methods and other code conventions that will produce exceptions if something goes wrong, so that you and your team are better able to see exactly what is failing and why.

Second, given that .NET can produce such a huge quantity of different exceptions -- and as we learned above, they can come in areas you least expect them or didn't plan for -- implementing some form of exception tracking is can be hugely beneficial.  Applications like [Airbrake's .NET bug tracker](https://airbrake.io/languages/net_bug_tracker) can dramatically simplify the process of tracking and resolving exception examples like those above, where code that wasn't even expected to be problematic is throwing errors.

---

__SOURCES__

- https://msdn.microsoft.com/en-us/library/67ef8sbd.aspx
- https://www.toptal.com/c-sharp/top-10-mistakes-that-c-sharp-programmers-make
