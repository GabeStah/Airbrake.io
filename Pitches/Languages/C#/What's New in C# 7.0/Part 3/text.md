# What's New in C# 7.0? - Digit Separators, Reference Returns, and Binary Literals

Thanks to frequent updates and continuous support, C# remains one of the [most popular](https://insights.stackoverflow.com/survey/2017#most-popular-technologies) modern development languages.  The latest major version, released in March of 2017 and coinciding with the release of Visual Studio 2017, brings with it a number of new features, which we've been covering in our ongoing series, _What's New in C# 7.0?_.  Thus far we've investigated a number of topics:

- In [Part 1](https://airbrake.io/blog/csharp/whats-new-in-csharp-7-0) we thoroughly explored `tuple types`, `tuple literals`, and `out variables`.
- In [Part 2](https://airbrake.io/blog/csharp/whats-new-in-c-7-0-pattern-matching-and-local-functions) we looked at `pattern matching` and `local functions`.

For part 3 today we'll examine the `digit separator`, `binary literals`, `returning references` and `local reference variables`, so let's get to it!

## Digit Separator

A small but handy little feature introduced in C# 7.0 is the `digit separator` character, which takes the form of a single underscore (`_`).  This separator can be used within any numeric literal as a means of improving legibility.  The existence of a `digit separator` in a numeric literal _does not_ change the value in anyway.  Any given number is always the same to the common language runtime, regardless of whether it uses separators or not.

This feature can be particularly useful when creating extremely lengthy numeric literal values.  For example, it canact as a thousands-place separator:

```cs
// These are equivalent.
var bigNumber      = 123456789012345678;
var bigNumberSplit = 123_456_789_012_345_678;
```

Obviously, the first value is quite difficult to read at a glance, whereas it's relatively easy to count the thousands places in the second number and deduce it's somewhere around `123 quadrillion`.  While scientific notation can often be used for large numbers, in situations where exact precision is necessary (like above), the `digit separator` comes in handy.

## Binary Literals

C# 7.0 also provides the ability to create `binary literal` values (`base 2` numbers, effectively).  This adds onto the other numeric literal capabilities, such as `hexadecimal literals`, so you can write out exactly the value you mean to, in whatever format best suits your needs.  Moreover, we can also use the new `digit separator` within `hexadecimal` and `binary literals`.

Here we see the standard and digit-separated versions, using all three numeric literal forms:

```cs
// Integer representation.
Logging.Log(24601);
Logging.Log(24_601);

// Hexadecimal representation.
Logging.Log(0x6019);
Logging.Log(0x60_19);

// Binary representation.
Logging.Log(0b110000000011001);
Logging.Log(0b110_0000_0001_1001);

Logging.Log("My name is Jean Valjean.");
```

The output shows all our numbers are equivalent, as expected:

```
24601

24601

24601

24601

24601

24601

My name is Jean Valjean.
```

_Note: We're using the `Utility.Logging` class in these examples, which can be found below_:

```cs
// <Utility>/Logging.cs
using System;
using System.Diagnostics;

namespace Utility
{
    /// <summary>
    /// Houses all logging methods for various debug outputs.
    /// </summary>
    public static class Logging
    {
        private const char SeparatorCharacterDefault = '-';
        private const int SeparatorLengthDefault = 40;

        /// <summary>
        /// Determines type of output to be generated.
        /// </summary>
        public enum OutputType
        {
            /// <summary>
            /// Default output.
            /// </summary>
            Default,
            /// <summary>
            /// Output includes timestamp prefix.
            /// </summary>
            Timestamp
        }

        /// <summary>
        /// Outputs to <see cref="Debug.WriteLine(String)"/>.
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        /// <param name="outputType">Output type.</param>
        public static void Log(string value, OutputType outputType = OutputType.Default)
        {
            Debug.WriteLine(outputType == OutputType.Timestamp
                ? $"[{StopwatchProxy.Instance.Stopwatch.Elapsed}] {value}"
                : value);
        }
        
        /// <summary>
        /// When <see cref="Exception"/> parameter is passed, modifies the output to indicate
        /// if <see cref="Exception"/> was expected, based on passed in `expected` parameter.
        /// <para>Outputs the full <see cref="Exception"/> type and message.</para>
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to output.</param>
        /// <param name="expected">Boolean indicating if <see cref="Exception"/> was expected.</param>
        /// <param name="outputType">Output type.</param>
        public static void Log(Exception exception, bool expected = true, OutputType outputType = OutputType.Default)
        {
            var value = $"[{(expected ? "EXPECTED" : "UNEXPECTED")}] {exception}: {exception.Message}";

            Debug.WriteLine(outputType == OutputType.Timestamp
                ? $"[{StopwatchProxy.Instance.Stopwatch.Elapsed}] {value}"
                : value);
        }

        /// <summary>
        /// Outputs to <see cref="Debug.WriteLine(Object)"/>.
        /// 
        /// ObjectDumper: http://stackoverflow.com/questions/852181/c-printing-all-properties-of-an-object&amp;lt;/cref
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        /// <param name="outputType">Output type.</param>
        public static void Log(object value, OutputType outputType = OutputType.Default)
        {
            Debug.WriteLine(outputType == OutputType.Timestamp
                ? $"[{StopwatchProxy.Instance.Stopwatch.Elapsed}] {ObjectDumper.Dump(value)}"
                : ObjectDumper.Dump(value));
        }

        /// <summary>
        /// Outputs a dashed line separator to <see cref="Debug.WriteLine(String)"/>.
        /// </summary>
        /// <param name="length">Total separator length.</param>
        /// <param name="char">Separator character.</param>
        public static void LineSeparator(int length = SeparatorLengthDefault, char @char = SeparatorCharacterDefault)
        {
            Debug.WriteLine(new string(@char, length));
        }

        /// <summary>
        /// Outputs a dashed line separator to <see cref="Debug.WriteLine(String)"/>,
        /// with inserted text centered in the middle.
        /// </summary>
        /// <param name="insert">Inserted text to be centered.</param>
        /// <param name="length">Total separator length.</param>
        /// <param name="char">Separator character.</param>
        public static void LineSeparator(string insert, int length = SeparatorLengthDefault, char @char = SeparatorCharacterDefault)
        {
            // Default output to insert.
            var output = insert;

            if (insert.Length < length)
            {
                // Update length based on insert length, less a space for margin.
                length -= (insert.Length + 2);
                // Halve the length and floor left side.
                var left = (int) Math.Floor((decimal) (length / 2));
                var right = left;
                // If odd number, add dropped remainder to right side.
                if (length % 2 != 0) right += 1;

                // Surround insert with separators.
                output = $"{new string(@char, left)} {insert} {new string(@char, right)}";
            }
            
            // Output.
            Debug.WriteLine(output);
        }
    }
}
```

## Returning References and Local Reference Variables

In the past, using references in C# merely consisted of _passing an argument by reference_.  The `ref` keyword would precede the parameter type in the method definition.  To coincide with the parameter `ref` keyword, `ref` would also come before the argument value being passed to said method.  For example, here the `SetByReference(ref int, int)` method changes the value at the `reference` location to `value`, so we can change the index `2` array element from `-42` to `24601`:

```cs
var numbers = new[] {5, 11, -42};

Logging.Log(numbers[2]); // -42

SetByReference(ref numbers[2], 24_601);

Logging.Log(numbers[2]); // 24601

private static void SetByReference(ref int reference, int value)
{
    reference = value;
}
```

Now, in C# 7.0 the `ref` keyword can be used in a couple more useful situations: Returning a reference value from a method, and creating a local variable to store a reference value.  To illustrate these new features we have a simple `Library` class, the full code of which can be found below, then we'll explain what's going on in more detail to follow:

```cs
internal class Library
{
    internal Library()
    {
        // Create baseline Book collection.
        var books = new[]
        {
            new Book("The Stand", "Stephen King", 823),
            new Book("Moby Dick", "Herman Melville", 378),
            new Book("Fahrenheit 451", "Ray Bradbury", 158),
            new Book("The Name of the Wind", "Patrick Rothfuss", 722)
        };

        // Output Book collection.
        Logging.LineSeparator("COLLECTION");
        Logging.Log(books);

        // Title of Book to search for.
        const string searchTitle = "Moby Dick";
        
        // Get index of target Book.
        var index = Array.FindIndex(books, x => x.Title == searchTitle);
        // Output index.
        Logging.Log($"Search title: {searchTitle} found at index: {index}.");

        // Get reference of search title Book.
        ref var reference = ref GetReferenceByTitle(searchTitle, books);
        // Confirm reference retrieved.
        Logging.LineSeparator("REFERENCE");
        Logging.Log(reference);

        // Get value of search title Book.
        var value = GetValueByTitle(searchTitle, books);
        // Confirm value retrieved.
        Logging.LineSeparator("VALUE");
        Logging.Log(value);

        // Change value of reference to "Game of Thrones" Book.
        reference = new Book("A Game of Thrones", "George R.R. Martin", 694);

        // Use previously retrieved index to confirm array element changed.
        Logging.LineSeparator("UPDATED BY INDEX");
        Logging.Log(books[index]);

    }

    /// <summary>
    /// Get reference from Book collection based on passed title.
    /// </summary>
    /// <param name="title">Title to search for.</param>
    /// <param name="books">Book collection.</param>
    /// <returns>Matching Book reference.</returns>
    public ref Book GetReferenceByTitle(string title, Book[] books)
    {
        for (var i = 0; i < books.Length; i++)
        {
            if (books[i].Title == title)
            {
                return ref books[i];
            }
        }
        throw new IndexOutOfRangeException($"Book with title '{title}' not found.");
    }

    /// <summary>
    /// Get Book instance from Book collection based on passed title.
    /// </summary>
    /// <param name="title">Title to search for.</param>
    /// <param name="books">Book collection.</param>
    /// <returns>Matching Book instance.</returns>
    public Book GetValueByTitle(string title, Book[] books)
    {
        return books.FirstOrDefault(x => x.Title == title);
    }
}
```

---

Our goal is to retrieve a reference from a `Book` `Array` collection based on a searched book `Title` property value.  To accomplish this we have the `GetReferenceByTitle(string, Book[])` method:

```cs
/// <summary>
/// Get reference from Book collection based on passed title.
/// </summary>
/// <param name="title">Title to search for.</param>
/// <param name="books">Book collection.</param>
/// <returns>Matching Book reference.</returns>
public ref Book GetReferenceByTitle(string title, Book[] books)
{
    for (var i = 0; i < books.Length; i++)
    {
        if (books[i].Title == title)
        {
            return ref books[i];
        }
    }
    throw new IndexOutOfRangeException($"Book with title '{title}' not found.");
}
```

This method just loops through the `books` array, by index, until it finds a matching `Title`, at which point it returns the _reference_ to that matching `Book` element using the `ref` keyword.

To illustrate how this might differ from the "usual" method of returning _by-value_, we have the `GetValueByTitle(string, Book[])` method:

```cs
/// <summary>
/// Get Book instance from Book collection based on passed title.
/// </summary>
/// <param name="title">Title to search for.</param>
/// <param name="books">Book collection.</param>
/// <returns>Matching Book instance.</returns>
public Book GetValueByTitle(string title, Book[] books)
{
    return books.FirstOrDefault(x => x.Title == title);
}
```

Since `Book[]` is an `IEnumerable` collection we can use LINQ, so with just a single little line of code we can search and return our matching `Book` element _value_.

To illustrate how the `ref` return value works we start by creating our `Book` array collection, then specify the `Book` `Title` value we want to search for.  In this case, we're going to be looking in our collection for `"Moby Dick"`.  We also call `Array.FindIndex(T[], Predicate<T>)` and output the actual index value that our searched `Book` was found at, which we'll use at the end of the example:

```cs
// Create baseline Book collection.
var books = new[]
{
    new Book("The Stand", "Stephen King", 823),
    new Book("Moby Dick", "Herman Melville", 378),
    new Book("Fahrenheit 451", "Ray Bradbury", 158),
    new Book("The Name of the Wind", "Patrick Rothfuss", 722)
};

// Output Book collection.
Logging.LineSeparator("COLLECTION");
Logging.Log(books);

// Title of Book to search for.
const string searchTitle = "Moby Dick";

// Get index of target Book.
var index = Array.FindIndex(books, x => x.Title == searchTitle);
// Output index.
Logging.Log($"Search title: {searchTitle} found at index: {index}.");
```

The output of this initial setup code appears below:

```
-------------- COLLECTION --------------
{Utility.Book(HashCode:30015890)}
  Author: "Stephen King"
  PageCount: 823
  Title: "The Stand"
{Utility.Book(HashCode:1707556)}
  Author: "Herman Melville"
  PageCount: 378
  Title: "Moby Dick"
{Utility.Book(HashCode:15368010)}
  Author: "Ray Bradbury"
  PageCount: 158
  Title: "Fahrenheit 451"
{Utility.Book(HashCode:4094363)}
  Author: "Patrick Rothfuss"
  PageCount: 722
  Title: "The Name of the Wind"

Search title: Moby Dick found at index: 1.
```

We've confirmed that the collection contains four `Books`, and that `"Moby Dick"` is at index `1`, so now let's make use of new reference return value functionality.  To do so, we preface the call to `GetReferenceByTitle(string, Book[])` with the `ref` keyword, which indicates we expect a reference to be returned.  We also want to assign that returned reference value to a local variable, which is accomplished by preceding our `var` keyword with the `ref` keyword during assignment.  Thus, here the local variable named `reference` contains the reference value that is returned by `GetReferenceByTitle(string, Book[])`:

```cs
// Get reference of search title Book.
ref var reference = ref GetReferenceByTitle(searchTitle, books);
// Confirm reference retrieved.
Logging.LineSeparator("REFERENCE");
Logging.Log(reference);

// Get value of search title Book.
var value = GetValueByTitle(searchTitle, books);
// Confirm value retrieved.
Logging.LineSeparator("VALUE");
Logging.Log(value);
```

We've also made a call to `GetValueByTitle(string, Book[])` with the same arguments passed, so we can output and compare the results of both types of returns (reference vs value):

```
-------------- REFERENCE ---------------
{Utility.Book(HashCode:1707556)}
  Author: "Herman Melville"
  PageCount: 378
  Title: "Moby Dick"

---------------- VALUE -----------------
{Utility.Book(HashCode:1707556)}
  Author: "Herman Melville"
  PageCount: 378
  Title: "Moby Dick"
```

Excellent!  So far everything is working as expected, so now we can use our local reference variable (`reference`) to change the value that said reference refers to.  In this case, we'll create a new `Book` instance for `"Game of Thrones"` (the new season is awesome so far, by the way).  Just to confirm we're accessing the original array collection at the same location that `"Moby Dick"` was originally found at, we're outputting the value at the `index` location, which we saved earlier:

```cs
// Change value of reference to "Game of Thrones" Book.
reference = new Book("A Game of Thrones", "George R.R. Martin", 694);

// Use previously retrieved index to confirm array element changed.
Logging.LineSeparator("UPDATED BY INDEX");
Logging.Log(books[index]);
```

As intended, the output confirms that reassigning the `reference` variable successfully updated the underlying element value within our original `books` collection:

```
----------- UPDATED BY INDEX -----------
{Utility.Book(HashCode:36849274)}
  Author: "George R.R. Martin"
  PageCount: 694
  Title: "A Game of Thrones"
```

Stay tuned for future parts in this series where we'll continue exploring the new features introduced in C# 7.0!  And don't forget, the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-dotnet">Sharpbrake</a> library provides robust exception tracking capabilities for all of your C# and .NET applications.  `Sharpbrake` provides real-time error monitoring and automatic exception reporting across your entire project, so you and your team are immediately alerted to even the smallest hiccup, and can appropriately respond before major problems arise.  With a robust API and tight integration with the powerful `Airbrake` web dashboard, `Sharpbrake` will revolutionize how your team manages exceptions.

Check out all the great features <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-dotnet">Sharpbrake</a> brings to the table and see why so many of the world's top development teams use `Airbrake` to dramatically improve their exception handling practices!

---

__META DESCRIPTION__

Part 3 of our exploration into what's new in C# 7.0, including digit separator, binary literals, returning references and local reference variables.


---

__SOURCES__

- http://reducing-suffering.org/how-many-wild-animals-are-there/#Mammals
- https://insights.stackoverflow.com/survey/2017#most-popular-technologies
- https://www.visualstudio.com/en-us/news/releasenotes/vs2017-relnotes
- https://github.com/dotnet/roslyn/blob/master/docs/Language%20Feature%20Status.md#c-70-and-vb-15
- https://blogs.msdn.microsoft.com/dotnet/2017/03/09/new-features-in-c-7-0/