# What's New in C# 7.0? - Part 2

C# 7.0, the latest major version of the exceptionally popular language, was released in March 2017 alongside Visual Studio 2017, bringing a number of new features and capabilities to the table.  Today we'll continue looking deeper at some of these features in our ongoing series, _What's New in C# 7.0?_:

- In [Part 1](https://airbrake.io/blog/csharp/whats-new-in-csharp-7-0) we thoroughly explored `tuple types`, `tuple literals`, and `out variables`.

In part 2 today we'll be looking at `pattern matching` and `local functions`, so let's get to it!

## Pattern Matching

One handy feature that C# 7.0 brings to the table is `patterns`, which provide a simple syntax to test whether an object meets a criteria related to its value or type (for now).  As of writing, there are currently three different `pattern matching` types, but the C# team has promised that additional pattern types will be introduced in the future.

- `Constant patterns` are quite standard and something we've seen before.  These effectively test if an input is equal to a particular constant value.
- `Type patterns` check if an input has a particular `type`, and if so, extracts the input value into a new variable of that type.
- `Var patterns` don't perform a conditional match of any kind, thereby making them always match.  The purpose of a `var pattern` is to generate a new variable with the value and type of the input.

These will make far more sense in code, so let's jump right into the code sample.  We start with a series of classes inheriting from one another, all based on the `IOrganism` interface:

```cs
interface IOrganism
{
    double Population { get; set; }
}

class Insect : IOrganism
{
    public double Population { get; set; }

    public Insect()
    {
        // Approximately 19 quadrillion insects.
        Population = 1e19;
    }

    public Insect(double population)
    {
        Population = population;
    }
}

class Mammal : IOrganism
{
    public double Population { get; set; }

    public Mammal()
    {
        // Approximately 1 trillion mammals.
        Population = 1e12;
    }

    public Mammal(double population)
    {
        Population = population;
    }
}

class Human : Mammal
{
    public Human()
    {
        // Approximately 7.52 billion humans.
        Population = 7.52e9;
    }

    public Human(double population)
    {
        Population = population;
    }
}

class Bee : Insect
{
    public Bee()
    {
        // Approximately 10 - 50 trillion bees.
        Population = 30e12;
    }

    public Bee(double population)
    {
        Population = population;
    }
}
```

These classes don't do much on their own, other than estimate their respective global populations (the stats of which were acquired from [this publication](http://reducing-suffering.org/how-many-wild-animals-are-there/)).  However, we'll use these classes to illustrate the differences (and potential uses) of the various `pattern matching` types.

### Type Patterns

We start with the `GetPopulationUsingType(IOrganism organism)` method, which uses a `switch` case in combination with the new `type pattern` to make it easy for us to differentiate between all the potential types that could be passed into this method:

```cs
private static void GetPopulationUsingType(IOrganism organism)
{
    double population = 0;
    // Switch on passed organism using type pattern matching.
    switch (organism)
    {
        case Bee bee:
            population = bee.Population;
            break;
        case Human human when (human.Population < 1e7):
            // If a Human is passed and the population is too low, panic!
            Logging.Log($"The human population is too low at {human.Population:n0}!  Apocalypse!");
            return;
        case Human human:
            // If the Human population is alright, proceed as normal.
            population = human.Population;
            break;
        case Insect insect:
            population = insect.Population;
            break;
        case Mammal mammal:
            population = mammal.Population;
            break;
        default:
            // Output alert if organism type is unknown.
            Logging.Log($"Unknown organism type ({organism.GetType().Name}), or population exceeds all known estimates.");
            return;
    }
    // Output retrieved population estimate.
    Logging.Log($"Estimated number of {organism.GetType().Name.ToLower()}s on Earth: {population:n0}.");
}
```

Since `IOrganism` is the baseline interface, and the `Mammal` and `Insect` classes implement that interface, which are, in turn, inherited by `Human` and `Bee`, it would typically be somewhat challenging to properly differentiate between these types using a series of `if-else` statements.  However, with `type patterns`, we can simply use a `switch (organism)` statement, and then create a type-specific `case` statement that will check if the passed type matches.  We can go even further and apply a bit of filtering, which we've done with the two `case Human human` statements.  In the event that a `Human` object is passed, if the `Population` is too low our first `case Human human` matches and executes, otherwise the second `case Human human` will do so.

To test this out we'll call this method a few times by passing different organism types:

```cs
private static void TypePatternExample()
{
    // Pass new Human.
    GetPopulationUsingType(new Human());
    // Pass new Bee.
    GetPopulationUsingType(new Bee());
    // Pass new Mammal.
    GetPopulationUsingType(new Mammal());
    // Pass new Insect.
    GetPopulationUsingType(new Insect());

    Logging.LineSeparator();

    // Pass new Human, with low population argument.
    GetPopulationUsingType(new Human(4.2e6));
}
```

The resulting output from this looks expected -- the populations are output for the first four, but the low human population output on the final call causes a problem:

```
Estimated number of humans on Earth: 7,520,000,000.
Estimated number of bees on Earth: 30,000,000,000,000.
Estimated number of mammals on Earth: 1,000,000,000,000.
Estimated number of insects on Earth: 10,000,000,000,000,000,000.
--------------------
The human population is too low at 4,200,000!  Apocalypse!
```

### Var Patterns

To see the `var pattern` in action we've created a `GetPopulationUsingVar(IOrganism organism)` method:

```cs
private static void GetPopulationUsingVar(IOrganism organism)
{
    double population = 0;
    // Switch on passed organism using var pattern matching.
    switch (organism)
    {
        // Assign organism to new bee variable, if population is roughly equal to 30 trillion.
        case var bee when Math.Abs(bee.Population - 30e12) <= 1:
            population = bee.Population;
            break;
        // Assign organism to new human variable, if object type Name is "Human."
        case var human when human.GetType().Name == "Human":
            population = human.Population;
            break;
        default:
            // Output alert if organism type is unknown.
            Logging.Log($"Unknown organism type ({organism.GetType().Name}), or population exceeds all known estimates.");
            return;
    }
    // Output retrieved population estimate.
    Logging.Log($"Estimated number of {organism.GetType().Name.ToLower()}s on Earth: {population:n0}.");
}
```

This method also accepts an `IOrganism` instance and uses a `switch` statement to handle logic based on the type that was passed.  However, notice the syntax of our `case` statements.  By using `case var bee` in the first `case` statement we've implemented a `var pattern`.  This extracts a matched value of `organism` and assigns it to the newly-created `bee` variable, which is then a local variable we can use within the `case` statement scope.  Thus, our first `case` statement tries to capture a passed `Bee` object by checking that the population is roughly equal to what we expect for bees, while the human `case` statement performs a check of the type `Name` property.

We can test these out and verify everything works as expected in the `VarPatternExample()` method:

```cs
private static void VarPatternExample()
{
    // Pass new Human.
    GetPopulationUsingVar(new Human());
    // Pass new Bee.
    GetPopulationUsingVar(new Bee());

    Logging.LineSeparator();

    // Pass new Mammal, an unknown type..
    GetPopulationUsingVar(new Mammal());
}
```

Executing this results in the first two calls working fine, however, the `Mammal` passed to the final call is an unknown type, so we get a different output informing us the method doesn't know how to handle it (couldn't find a match):

```
Estimated number of humans on Earth: 7,520,000,000.
Estimated number of bees on Earth: 30,000,000,000,000.
--------------------
Unknown organism type (Mammal), or population exceeds all known estimates.
```

### Constant Pattern and Is-Expressions

The last pattern to look at is the `constant pattern`.  Since such a pattern is very basic, we'll also explore it along with the `is-expression`, which allows us to check if an object is equivalent to a particular value, or is of a particular type (via a `type pattern`).  The `GetPopulationUsingIs(object organism)` method accepts an `object`, so we can test for the `null` constant using a `constant pattern` via an `is-expression`.  We also check if the passed object is of the type `IOrganism`.  If so, we assign it to the new variable of `o`, which we use for the output:

```cs
private static void GetPopulationUsingIs(object organism)
{
    if (organism is null)
    {
        Logging.Log($"Organism is null, cancelling.");
    }
    if (organism is IOrganism o)
    {
        // Output retrieved population estimate.
        Logging.Log($"Estimated number of {o.GetType().Name.ToLower()}s on Earth: {o.Population:n0}.");
    }
}
```

To test this method out we'll use `IsExpressionPatternExample()`, which passes a couple objects that both inherit from `IOrganism`, followed by passing `null`:

```cs
private static void IsExpressionPatternExample()
{
    // Pass new Human.
    GetPopulationUsingIs(new Insect());
    // Pass new Bee.
    GetPopulationUsingIs(new Mammal());

    Logging.LineSeparator();

    // Pass null.
    GetPopulationUsingIs(null);
}
```

The output from running this method is what we expected -- population data for `Insect` and `Mammal`, followed by a cancellation message when passing `null`:

```
Estimated number of insects on Earth: 10,000,000,000,000,000,000.
Estimated number of mammals on Earth: 1,000,000,000,000.
--------------------
Organism is null, cancelling.
```

## Local Functions

Another cool feature C# 7.0 adds is the ability to create `local functions`.  A `local function` is, as the name implies, a function that is embedded _directly inside_ the scope of another method.  To see this in action, we'll start with the full code snippet, then break it down afterward:

```cs
using System;
using System.Collections.Generic;
using Utility;

namespace LocalFunctions
{
    public interface IBook
    {
        string Author { get; set; }
        int PageCount { get; set; }
        string Title { get; set; }
    }

    public class Book : IBook
    {
        public string Author { get; set; }
        public int PageCount { get; set; }
        public string Title { get; set; }

        public Book() { }

        public Book(string title, string author, int pageCount)
        {
            Author = author;
            PageCount = pageCount;
            Title = title;
        }

        public override string ToString()
        {
            return $"'{Title}' by {Author} at {PageCount} pages";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Create baseline Book collection.
            var books = new List<Book>
            {
                new Book("The Stand", "Stephen King", 823),
                new Book("Moby Dick", "Herman Melville", 378),
                new Book("Fahrenheit 451", "Ray Bradbury", 158),
                new Book("A Game of Thrones", "George R.R. Martin", 694),
                new Book("The Name of the Wind", "Patrick Rothfuss", 722)
            };

            // Output baseline books.
            Logging.Log("Baseline books.");
            Logging.Log(books);
            Logging.LineSeparator();

            // Filter books where PageCount exceeds 400.
            var filteredBooks = Filter(books, (book) => book.PageCount > 400);

            // Output filtered books.
            Logging.Log("Filtered books with more than 400 pages.");
            Logging.Log(filteredBooks);
            Logging.LineSeparator();

            // Inverse filter by passing false argument to make the filter behave exclusively.
            filteredBooks = Filter(books, (book) => book.PageCount > 400, false);

            // Output filtered books.
            Logging.Log("Filtered books with fewer than or equal to 400 pages.");
            Logging.Log(filteredBooks);
            Logging.LineSeparator();
        }

        /// <summary>
        /// Filters a collection.
        /// </summary>
        /// <typeparam name="T">Type of element to filter.</typeparam>
        /// <param name="source">Source collection to iterator through.</param>
        /// <param name="filter">Filter action to apply.</param>
        /// <param name="inclusive">Determines if filter should act as inclusive or exclusive check.</param>
        /// <returns>Filtered collection.</returns>
        public static IEnumerable<T> Filter<T>(IEnumerable<T> source, Func<T, bool> filter, bool inclusive = true)
        {
            // Local function to perform iteration.
            IEnumerable<T> Iterator()
            {
                // Loop through each element of source.
                foreach (var element in source)
                {
                    // If inclusive, if element passes filter yield it.
                    // If exclusive, if element fails filter yield it.
                    if (inclusive ? filter(element) : !filter(element)) { yield return element; }
                }
            }

            // Return yielded Iterator result.
            return Iterator();
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace Utility
{
    /// <summary>
    /// Houses all logging methods for various debug outputs.
    /// </summary>
    public static class Logging
    {
        /// <summary>
        /// Outputs to <see cref="System.Diagnostics.Debug.WriteLine"/> if DEBUG mode is enabled,
        /// otherwise uses standard <see cref="Console.WriteLine"/>.
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        public static void Log(string value)
        {
#if DEBUG
            Debug.WriteLine(value);
#else
            Console.WriteLine(value);
#endif
        }

        /// <summary>
        /// Outputs to <see cref="System.Diagnostics.Debug.WriteLine"/> if DEBUG mode is enabled,
        /// otherwise uses standard <see cref="Console.WriteLine"/>.
        /// 
        /// ObjectDumper class from <see cref="http://stackoverflow.com/questions/852181/c-printing-all-properties-of-an-object"/>.
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        public static void Log(object value)
        {
#if DEBUG
            Debug.WriteLine(ObjectDumper.Dump(value));
#else
            Console.WriteLine(ObjectDumper.Dump(value));
#endif
        }

        /// <summary>
        /// Outputs a dashed line separator to <see cref="System.Diagnostics.Debug.WriteLine"/> 
        /// if DEBUG mode is enabled, otherwise uses standard <see cref="Console.WriteLine"/>.
        /// </summary>
        public static void LineSeparator(int length = 40)
        {
#if DEBUG
            Debug.WriteLine(new string('-', length));
#else
            Console.WriteLine(new string('-', length));
#endif
        }
    }
}
```

---

We begin with a basic `IBook` interface and `Book` class that implements `IBook`.  We'll use these to create a simple collection of books in just a moment.  However, first we need a reason to use a `local function` within another method.  Creating an iterator method is a common scenario in which a local function may prove useful.  An iterator typically performs some action upon each element in the collection, then calls a `yield` statement in order to yield the next element in the collection.

For example, let's look closer at the `Filter<T>(IEnumerable<T> source, Func<T, bool> filter, bool inclusive = true)` method:

```cs
/// <summary>
/// Filters a collection.
/// </summary>
/// <typeparam name="T">Type of element to filter.</typeparam>
/// <param name="source">Source collection to iterator through.</param>
/// <param name="filter">Filter action to apply.</param>
/// <param name="inclusive">Determines if filter should act as inclusive or exclusive check.</param>
/// <returns>Filtered collection.</returns>
public static IEnumerable<T> Filter<T>(IEnumerable<T> source, Func<T, bool> filter, bool inclusive = true)
{
    // Local function to perform iteration.
    IEnumerable<T> Iterator()
    {
        // Loop through each element of source.
        foreach (var element in source)
        {
            // If inclusive, if element passes filter yield it.
            // If exclusive, if element fails filter yield it.
            if (inclusive ? filter(element) : !filter(element)) { yield return element; }
        }
    }

    // Return yielded Iterator result.
    return Iterator();
}
```

As the name suggests, its purpose is to filter an enumerable collection using the passed `Func`, which should return a boolean indicating if the element passed or failed the filtration process.  This is a perfect scenario to use a local function, which is exactly what we've done with the `Iterator()` local function found inside.  `Iterator()` just loops through the elements of `source` and checks if each element passes the `filter` check, thereby determingin if the element should be yielded.  The entire collection of filtered, yielded elements is bubbled up from the `Iterator()` `local function` to the `return Iterator()` statement of the `Filter<T>(IEnumerable<T> source, Func<T, bool> filter, bool inclusive = true)` method.

The advantage to using a local function here, as opposed to an internal or private method, is that we may not want the local `Iterator()` function to be available to other members of the parent class.  A `local function` makes it easy to maintain the exact scope level that is required, without exposing any of the functionality to outside callers.

To test this out and make sure it works as expected, we'll start by creating a Book collection:

```cs
static void Main(string[] args)
{
    // Create baseline Book collection.
    var books = new List<Book>
    {
        new Book("The Stand", "Stephen King", 823),
        new Book("Moby Dick", "Herman Melville", 378),
        new Book("Fahrenheit 451", "Ray Bradbury", 158),
        new Book("A Game of Thrones", "George R.R. Martin", 694),
        new Book("The Name of the Wind", "Patrick Rothfuss", 722)
    };

    // Output baseline books.
    Logging.Log("Baseline books.");
    Logging.Log(books);
    Logging.LineSeparator();

    // ...
}
```

This produces the output of all our initial books, as expected:

```
Baseline books.
{LocalFunctions.Book(HashCode:30015890)}
  Author: "Stephen King"
  PageCount: 823
  Title: "The Stand"
{LocalFunctions.Book(HashCode:1707556)}
  Author: "Herman Melville"
  PageCount: 378
  Title: "Moby Dick"
{LocalFunctions.Book(HashCode:15368010)}
  Author: "Ray Bradbury"
  PageCount: 158
  Title: "Fahrenheit 451"
{LocalFunctions.Book(HashCode:4094363)}
  Author: "George R.R. Martin"
  PageCount: 694
  Title: "A Game of Thrones"
{LocalFunctions.Book(HashCode:36849274)}
  Author: "Patrick Rothfuss"
  PageCount: 722
  Title: "The Name of the Wind"
```

Now we'll try filtering our collection using the `Filter<T>(IEnumerable<T> source, Func<T, bool> filter, bool inclusive = true)` method.  We're also using lambda syntax to simplify the passing of our `filter` function argument, since we only need to return the result of a single statement.  In this case, we're just checking if the `PageCount` for each book exceeds `400`:

```cs
// Filter books where PageCount exceeds 400.
var filteredBooks = Filter(books, (book) => book.PageCount > 400);

// Output filtered books.
Logging.Log("Filtered books with more than 400 pages.");
Logging.Log(filteredBooks);
Logging.LineSeparator();
```

Our original collection contained three books with high page counts, and our output confirms that the filter behaves as desired:

```
Filtered books with more than 400 pages.
{LocalFunctions.Book(HashCode:30015890)}
  Author: "Stephen King"
  PageCount: 823
  Title: "The Stand"
{LocalFunctions.Book(HashCode:4094363)}
  Author: "George R.R. Martin"
  PageCount: 694
  Title: "A Game of Thrones"
{LocalFunctions.Book(HashCode:36849274)}
  Author: "Patrick Rothfuss"
  PageCount: 722
  Title: "The Name of the Wind"
```

Lastly, to illustrate how we can further alter the behavior of our inner `local function`, we also added the `bool inclusive` parameter to the `Filter<T>(IEnumerable<T> source, Func<T, bool> filter, bool inclusive = true)` method.  This allows us to effectively _inverse_ the behavior of the filter process, so any element that would return `true` from the filter now returns `false` (and, therefore, is excluded):

```cs
// Inverse filter by passing false argument to make the filter behave exclusively.
filteredBooks = Filter(books, (book) => book.PageCount > 400, false);

// Output filtered books.
Logging.Log("Filtered books with fewer than or equal to 400 pages.");
Logging.Log(filteredBooks);
Logging.LineSeparator();
```

Here we should see the opposite result of our previous filtration in the output:

```
Filtered books with fewer than or equal to 400 pages.
{LocalFunctions.Book(HashCode:1707556)}
  Author: "Herman Melville"
  PageCount: 378
  Title: "Moby Dick"
{LocalFunctions.Book(HashCode:15368010)}
  Author: "Ray Bradbury"
  PageCount: 158
  Title: "Fahrenheit 451"
```

Stay tuned for future parts in this series where we'll continue exploring the new features introduced in C# 7.0!  And don't forget, the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-dotnet">Sharpbrake</a> library provides robust exception tracking capabilities for all of your C# and .NET applications.  `Sharpbrake` provides real-time error monitoring and automatic exception reporting across your entire project, so you and your team are immediately alerted to even the smallest hiccup, and can appropriately respond before major problems arise.  With a robust API and tight integration with the powerful `Airbrake` web dashboard, `Sharpbrake` will revolutionize how your team manages exceptions.

Check out all the great features <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-dotnet">Sharpbrake</a> brings to the table and see why so many of the world's top development teams use `Airbrake` to dramatically improve their exception handling practices!

---

__META DESCRIPTION__

Part 2 of our exploration into what's new in C# 7.0, including pattern matching and local functions.

---

__SOURCES__

- http://reducing-suffering.org/how-many-wild-animals-are-there/#Mammals
- https://insights.stackoverflow.com/survey/2017#most-popular-technologies
- https://www.visualstudio.com/en-us/news/releasenotes/vs2017-relnotes
- https://github.com/dotnet/roslyn/blob/master/docs/Language%20Feature%20Status.md#c-70-and-vb-15
- https://blogs.msdn.microsoft.com/dotnet/2017/03/09/new-features-in-c-7-0/