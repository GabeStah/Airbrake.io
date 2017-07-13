# What's New in C# 7.0?

First released in early 2002, alongside the initial release of .NET 1.0, C# has had a lot of success in the development community.  Thanks to continued support and frequent updates that coincide with each new release of Visual Studio, C# remains one of the [most popular languages](https://insights.stackoverflow.com/survey/2017#most-popular-technologies) in use today.  The latest major version, released in March of this year alongside Visual Studio 2017, brings with it a number of new features, so we thought it would be fun to explore what's new in C# 7.0.  Let's get to it!

## Tuple Types and Tuple Literals

Tuples are used with many programming languages when it's necessary to return multiple values from a single method.  While C# has included tuple support through the [`System.Tuple`](https://msdn.microsoft.com/en-us/library/system.tuple(v=vs.110).aspx) since version 4.0 (released early 2010), using them has been a somewhat clunky affair.  Not only does it require the explicit creation (and use) of a new `System.Tuple` object, but accessing items was a hassle and had to be performed using rather vague item signature names.

For example, here we see the old way of creating a tuple using `System.Tuple`:

```cs
static void CreateOldTuple()
{
    // Declare a tuple using the old System.Tuple<>() syntax.
    var book = new Tuple<string, string, int>("The Name of the Wind", "Patrick Rothfuss", 662);
    // Output book data.
    Logging.Log($"'{book.Item1}' by {book.Item2} [{book.Item3} pgs]");
}
```

_Note_: `Logging.Log()` is just a helper method to output messages to the console.  In this example (as well as future examples), we'll use it to help illustrate results by outputting the title, author, and page count of our books, like so:

```
'The Name of the Wind' by Patrick Rothfuss [662 pgs]
```

Anyway, C# 7.0 has introduced `tuple types`, `tuple literals`, and `tuple names` to help reduce some of these pain points when dealing with tuples!

Creating a `tuple literal` merely requires surrounding a series of values with parentheses:

```cs
var tupleLiteral = (10, true, "Hello");
```

This means the old `System.Tuple` creation example from above can be created with much simpler syntax:

```cs
static void CreateLiteralTuple()
{
    // Create literal tuple.
    var book = ("The Stand", "Stephen King", 823);
    // Output book data.
    Logging.Log($"'{book.Item1}' by {book.Item2} [{book.Item3} pgs]");
}
```

Not only can we avoid calling `new Tuple<>` as in the past, but the compiler is able to infer the value types automatically, so there's no need to include a type list either.

We can also now easily use the `tuple names` feature by including item names prior to each value in our `tuple literal` declaration:

```cs
// Create a literal tuple with provided tuple names.
var book2 = (title: "Seveneves", author: "Neal Stephenson", pageCount: 880);
// Output book data using tuple element names.
Logging.Log($"'{book2.title}' by {book2.author} [{book2.pageCount} pgs]");
```

Notice how the same output structure allows us to call `book2.title` instead of `book2.Item1`, making it much more obvious what we're doing and what the intention of our code is.

That's all well and good, but tuples are typically used as return values from methods, so C# 7.0 also introduced the ability to define a tuple as a return type within a method definition.  For example, here in the `GetTupleLiteral()` method we're expecting a three-part tuple of `(string, string, int)` to be returned, so that's exactly what the method provides:

```cs
/// <summary>
/// Get a tuple literal.
/// </summary>
/// <returns>Three-part tuple literal result.</returns>
static (string, string, int) GetTupleLiteral()
{
    return ("Robinson Crusoe", "Daniel Defoe", 198);
}
```

And, just as we can define item names in literal tuple declarations, we can now also define item names within tuple return types as well:

```cs
/// <summary>
/// Get a tuple literal with specified tuple element names.
/// </summary>
/// <returns>Three-part tuple literal result, with element names.</returns>
static (string title, string author, int pageCount) GetTupleLiteralWithNames()
{
    return ("The Great Train Robbery", "Michael Crichton", 266);
}
```

To illustrate that these work just as we'd expect, here we'll call both `GetTupleLiteral()` and `GetTupleLiteralWithNames()`:

```cs
static void InvokeGetTupleMethods()
{
    // Get tuple literal.
    var book = GetTupleLiteral();
    // Output book data.
    Logging.Log($"'{book.Item1}' by {book.Item2} [{book.Item3} pgs]");

    // Get tuple literal with element names.
    var book2 = GetTupleLiteralWithNames();
    // Use element names to produce output.
    Logging.Log($"'{book2.title}' by {book2.author} [{book2.pageCount} pgs]");
}
```

Sure enough, they both function as intended and output the the expected, formatted results as before:

```
'Robinson Crusoe' by Daniel Defoe [198 pgs]
'The Great Train Robbery' by Michael Crichton [266 pgs]
```

## Out Variables

Using the `out` keyword within method arguments is an easy way to pass values by reference, but in older versions of C# it was necessary to declare the variables you're passing before they could be used.  For example, here's the older method of using the `out` keyword in variable arguments:

```cs
private static void GetBookData(out string title, out string author, out int pageCount)
{
    title = "The Stand";
    author = "Stephen King";
    pageCount = 823;
}

private static void OldOutExample()
{
    // Declare variables prior to use.
    string author, title;
    int pageCount;
    // Pass variables by reference.
    GetBookData(out title, out author, out pageCount);
    // Output referenced book data.
    Logging.Log($"'{title}' by {author} [{pageCount} pgs]");
}
```

Not only do we have to declare the `author`, `title`, and `pageCount` variables ahead of time, but we also have to specify their type.  This still works as expected, passing the reference to these arguments into the `GetBookData()` method, which (for this example) assigns them to specific values, after which we output the book info to the log:

```
'The Stand' by Stephen King [823 pgs]
```

Now, C# 7.0 has introduced the `out variable` syntax, which allows for inline declaration of the variables within the argument list of the method in question.  Thus, we're able to perform the exact same logic and behavior that we saw in `OldOutExample()` here in `OutVariableExample()`, but C# recognizes that the intention is to declare these out variables inline:

```cs
private static void OutVariableExample()
{
    // Use out variables, declaring them inline as arguments.
    GetBookData(out string title, out string author, out int pageCount);
    // Output referenced book data.
    Logging.Log($"'{title}' by {author} [{pageCount} pgs]");
}
```

Moreover, for most basic variable types we don't need to specify the type and can use `var` instead:

```cs
// Use out variables with var keyword.
GetBookData(out var title, out var author, out var pageCount);
```

C# 7.0 also introduces the ability to use `discards`, which are just local, unnamed, write-only variables that are represented with an underscore (`_`) character where a normal variable name would go.  As you can imagine, a `discards` are useful for situations where an `out variable` is necessary to invoke a particular method, but where we can ignore the `out variable` argument that is used.

For example, imagine we are using the `DateTime.TryParse()` method to evaluate a string and check if the CLR is able to convert it into a `DateTime` object.  Here we have a simple method to perform this task for us.  It uses an `out variable` called `result` if the `TryParse()` attempt it successful, outputting a message with the converted result.  If the parse fails we throw (and catch) a new `ArgumentException` indicating the parse didn't work:

```cs
private static void DateTimeTryParseExample(string value)
{
    try
    {
        // Attempt date parse and assign resulting DateTime to result.
        if (DateTime.TryParse(value, out var result))
        {
            Logging.Log($"Value of '{value}' successfully converted to DateTime of {result}.");
        }
        else
        {
            // Throw exception if parse fails.
            throw new ArgumentException($"Cannot parse passed value of '{value}' to DateTime.", nameof(value));
        }
    }
    catch (ArgumentException exception)
    {
        Logging.Log(exception);
    }
}
```

To test this out we'll first pass in a string value of `"today"`:

```cs
DateTimeTryParseExample("today");
```

Unfortunately, `TryParse()` doesn't know what we mean by this, probably because the API already includes the [`DateTime.Today`](https://docs.microsoft.com/en-us/dotnet/api/system.datetime.today?view=netframework-4.7#System_DateTime_Today) property for retrieving today's date, so an exception is thrown instead:

```
[EXPECTED] System.ArgumentException: Cannot parse passed 'value' of today to DateTime.
Parameter name: value
```

Now let's try passing a string to `DateTimeTryParseExample()` that we know will work:

```cs
DateTimeTryParseExample("1/1/2000");
```

Sure enough, this parse and conversion work as expected, assigning the generated `DateTime` object to our `result` `out variable`:

```
Value of '1/1/2000' successfully converted to DateTime of 1/1/2000 12:00:00 AM.
```

However, there may be situations where we don't care about the `result` of the `TryParse()` call at all, and all we want to know is whether it worked or not (the returned `boolean` value).  This is a perfect use of the new `discard` placeholder for the `out variable`:

```cs
private static void DateTimeTryParseWithDiscardExample(string value)
{
    try
    {
        // Ignore out variable by using discard variable instead.
        if (DateTime.TryParse(value, out _))
        {
            Logging.Log($"Value of '{value}' successfully converted to DateTime!");
        }
        else
        {
            throw new ArgumentException($"Cannot parse passed value of '{value}' to DateTime.", nameof(value));
        }
    }
    catch (ArgumentException exception)
    {
        Logging.Log(exception);
    }
}
```

Now we can just focus on using the boolean result of `TryParse()` and don't need to concern ourselves with the `out variable` declaration or result.

Stay tuned for future parts in this series where we'll continue exploring the new features introduced in C# 7.0!  And don't forget, the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-dotnet">Sharpbrake</a> library provides robust exception tracking capabilities for all of your C# and .NET applications.  `Sharpbrake` provides real-time error monitoring and automatic exception reporting across your entire project, so you and your team are immediately alerted to even the smallest hiccup, and can appropriately respond before major problems arise.  With a robust API and tight integration with the powerful `Airbrake` web dashboard, `Sharpbrake` will revolutionize how your team manages exceptions.

Check out all the great features <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-dotnet">Sharpbrake</a> brings to the table and see why so many of the world's top development teams use `Airbrake` to dramatically improve their exception handling practices!

---

__META DESCRIPTION__

Part 1 of our exploration into what's new in C# 7.0, including tuple types, tuple literals, out variables, and discards.

---

__SOURCES__

- https://insights.stackoverflow.com/survey/2017#most-popular-technologies
- https://www.visualstudio.com/en-us/news/releasenotes/vs2017-relnotes
- https://github.com/dotnet/roslyn/blob/master/docs/Language%20Feature%20Status.md#c-70-and-vb-15
- https://blogs.msdn.microsoft.com/dotnet/2017/03/09/new-features-in-c-7-0/