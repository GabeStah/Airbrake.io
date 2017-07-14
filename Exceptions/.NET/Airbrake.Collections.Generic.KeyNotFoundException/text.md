# .NET Exceptions - System.Collections.Generic.KeyNotFoundException

Next up in our continued [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series we come to the System.Collections.Generic.KeyNotFoundException.  In most cases the `System.Collections.Generic.KeyNotFoundException` is thrown when attempting to access a `Collection` element using a key that doesn't exist within said collection.

In this article we'll take a closer look at the `System.Collections.Generic.KeyNotFoundException`, including where it sits in the .NET exception hierarchy, along with some functional C# code examples to help illustrate how `System.Collections.Generic.KeyNotFoundExceptions` are typically thrown, as well as how to handle the differences in collection types (and the inability to natively capture the _key_ that caused the exception in the first place).  Let's get to it!

## The Technical Rundown

- All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.
- [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception) is inherited from the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) class.
- `System.Collections.Generic.KeyNotFoundException` inherits directly from [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception).

## When Should You Use It?

Unfortunately, in spite of the fact that it belongs to the `System.Collections` namespace, the `System.Collections.Generic.KeyNotFoundException` sometimes has trouble capturing key errors from certain types of collections.  For example, accessing an invalid key in `List<T>` doesn't throw a `System.Collections.Generic.KeyNotFoundException`, whereas doing the same in a `Dictionary<TKey, TValue>` _does_.  To help illustrate some of these differences we'll start with the full code sample below, after which we'll go through the various methods and examples in more detail:

```cs
using System;
using System.Collections.Generic;
using Utility;

namespace Airbrake.Collections.Generic.KeyNotFoundException
{
    /// <summary>
    /// Book interface.
    /// </summary>
    public interface IBook
    {
        string Author { get; set; }
        string Title { get; set; }
    }

    /// <summary>
    /// Basic Book class.
    /// </summary>
    public class Book : IBook
    {
        public string Author { get; set; }
        public string Title { get; set; }

        public Book(string title, string author)
        {
            Author = author;
            Title = title;
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            // Create Book Dictionary.
            var list = new List<Book>
            {
                new Book("The Stand", "Stephen King"),
                new Book("The Name of the Wind", "Patrick Rothfuss"),
                new Book("Robinson Crusoe", "Daniel Defoe"),
                new Book("The Hobbit", "J.R.R. Tolkien")
            };

            ListExample(list);
            Logging.LineSeparator();

            // Create Book Dictionary.
            var dictionary = new Dictionary<int, Book>
            {
                { 0, new Book("The Stand", "Stephen King") },
                { 1, new Book("The Name of the Wind", "Patrick Rothfuss") },
                { 2, new Book("Robinson Crusoe", "Daniel Defoe") },
                { 3, new Book("The Hobbit", "J.R.R. Tolkien") }
            };

            DictionaryExample(dictionary);
            Logging.LineSeparator();

            DictionaryUsingTryGetValueExample(dictionary);
            Logging.LineSeparator();

            // Create Book Dictionary.
            var improvedDictionary = new ImprovedDictionary<int, Book>
            {
                { 0, new Book("The Stand", "Stephen King") },
                { 1, new Book("The Name of the Wind", "Patrick Rothfuss") },
                { 2, new Book("Robinson Crusoe", "Daniel Defoe") },
                { 3, new Book("The Hobbit", "J.R.R. Tolkien") }
            };

            ImprovedDictionaryExample(improvedDictionary);
        }

        private static void ListExample(IReadOnlyList<Book> list)
        {
            try
            {
                // Output current library.
                Logging.Log(list);
                // Add line seperator for readability.
                Logging.LineSeparator();
                // Attempt to output element of index equal to count.
                Logging.Log(list[list.Count]);
            }
            catch (System.Collections.Generic.KeyNotFoundException exception)
            {
                // Catch KeyNotFoundExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Catch unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }

        private static void DictionaryExample(IReadOnlyDictionary<int, Book> dictionary)
        {
            try
            {
                // Output current library.
                Logging.Log(dictionary);
                // Add line seperator for readability.
                Logging.LineSeparator();
                // Attempt to output element of index equal to count.
                Logging.Log(dictionary[dictionary.Count]);
            }
            catch (System.Collections.Generic.KeyNotFoundException exception)
            {
                // Catch KeyNotFoundExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Catch unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }

        private static void DictionaryUsingTryGetValueExample(IReadOnlyDictionary<int, Book> dictionary)
        {
            try
            {
                // Output current library.
                Logging.Log(dictionary);
                // Add line seperator for readability.
                Logging.LineSeparator();
                // Attempt to output element of index equal to count.
                if (dictionary.TryGetValue(dictionary.Count, out var book))
                {
                    Logging.Log(book);
                }
                else
                {
                    Logging.Log($"Element at index [{dictionary.Count}] could not be found.");
                }
            }
            catch (System.Collections.Generic.KeyNotFoundException exception)
            {
                // Catch KeyNotFoundExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Catch unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }

        private static void ImprovedDictionaryExample(ImprovedDictionary<int, Book> dictionary)
        {
            try
            {
                // Output current library.
                Logging.Log(dictionary);
                // Add line seperator for readability.
                Logging.LineSeparator();
                // Attempt to output element of index equal to count.
                Logging.Log(dictionary[dictionary.Count]);
            }
            catch (System.Collections.Generic.KeyNotFoundException exception)
            {
                // Catch KeyNotFoundExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Catch unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }
    }

    /// <summary>
    /// Improves base Dictionary behavior by including missing key value on failed key retrieval.
    /// 
    /// Inherits from Dictionary.
    /// </summary>
    /// <typeparam name="TKey">Key type.</typeparam>
    /// <typeparam name="TValue">Value type.</typeparam>
    public class ImprovedDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public new TValue this[TKey key]
        {
            get
            {
                // Try retrieving value by key and assign to value out variable.
                if (base.TryGetValue(key, out var value))
                {
                    return value;
                }

                // If failure throw new KeyNotFoundException, including missing key.
                throw new System.Collections.Generic.KeyNotFoundException(
                    $"The given key [{key}] was not present in the dictionary.");
            }
            set => base[key] = value;
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
        /// When <see cref="Exception"/> parameter is passed, modifies the output to indicate
        /// if <see cref="Exception"/> was expected, based on passed in `expected` parameter.
        /// <para>Outputs the full <see cref="Exception"/> type and message.</para>
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to output.</param>
        /// <param name="expected">Boolean indicating if <see cref="Exception"/> was expected.</param>
        public static void Log(Exception exception, bool expected = true)
        {
            string value = $"[{(expected ? "EXPECTED" : "UNEXPECTED")}] {exception.ToString()}: {exception.Message}";
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

We start with a simple `IBook` interface and the `Book` class that implements it, which we'll be using throughout our examples to create some data collections:

```cs
/// <summary>
/// Book interface.
/// </summary>
public interface IBook
{
    string Author { get; set; }
    string Title { get; set; }
}

/// <summary>
/// Basic Book class.
/// </summary>
public class Book : IBook
{
    public string Author { get; set; }
    public string Title { get; set; }

    public Book(string title, string author)
    {
        Author = author;
        Title = title;
    }
}
```

Next we define the `ListExample(IReadOnlyList<Book> list)` method, which will output the `list` before attempting to explicitly output the element at index `list.Count` (which, as we know, will exceed the existing indices by one since elements are zero-based):

```cs
private static void ListExample(IReadOnlyList<Book> list)
{
    try
    {
        // Output current library.
        Logging.Log(list);
        // Add line seperator for readability.
        Logging.LineSeparator();
        // Attempt to output element of index equal to count.
        Logging.Log(list[list.Count]);
    }
    catch (System.Collections.Generic.KeyNotFoundException exception)
    {
        // Catch KeyNotFoundExceptions.
        Logging.Log(exception);
    }
    catch (Exception exception)
    {
        // Catch unexpected Exceptions.
        Logging.Log(exception, false);
    }
}
```

We call the `ListExample(IReadOnlyList<Book> list)` method in our `Program.Main()` method by instantiating a new `List<Book>` collection to pass in:

```cs
// Create Book Dictionary.
var list = new List<Book>
{
    new Book("The Stand", "Stephen King"),
    new Book("The Name of the Wind", "Patrick Rothfuss"),
    new Book("Robinson Crusoe", "Daniel Defoe"),
    new Book("The Hobbit", "J.R.R. Tolkien")
};

ListExample(list);
```

As you can probably guess this throws an exception that we see in our log output, however, that thrown exception it _isn't_ a `System.Collections.Generic.KeyNotFoundException`:

```
{Airbrake.Collections.Generic.KeyNotFoundException.Book(HashCode:46104728)}
  Author: "Stephen King"
  Title: "The Stand"
{Airbrake.Collections.Generic.KeyNotFoundException.Book(HashCode:12289376)}
  Author: "Patrick Rothfuss"
  Title: "The Name of the Wind"
{Airbrake.Collections.Generic.KeyNotFoundException.Book(HashCode:43495525)}
  Author: "Daniel Defoe"
  Title: "Robinson Crusoe"
{Airbrake.Collections.Generic.KeyNotFoundException.Book(HashCode:55915408)}
  Author: "J.R.R. Tolkien"
  Title: "The Hobbit"

--------------------
[UNEXPECTED] System.ArgumentOutOfRangeException: Index was out of range. Must be non-negative and less than the size of the collection.
Parameter name: index
```

As previously mentioned, unfortunately, using an invalid key within some collection types (such as `List<>`) produces a `System.ArgumentOutOfRangeException`.  Let's try a `Dictionary<TKey, TValue>` object instead and see what happens.  Here we have the `DictionaryExample(IReadOnlyDictionary<int, Book> dictionary)` method that performs the exact same logic as `ListExample(IReadOnlyList<Book> list)`, except with a passed in `Dictionary<int, Book>` instead:

```cs
private static void DictionaryExample(IReadOnlyDictionary<int, Book> dictionary)
{
    try
    {
        // Output current library.
        Logging.Log(dictionary);
        // Add line seperator for readability.
        Logging.LineSeparator();
        // Attempt to output element of index equal to count.
        Logging.Log(dictionary[dictionary.Count]);
    }
    catch (System.Collections.Generic.KeyNotFoundException exception)
    {
        // Catch KeyNotFoundExceptions.
        Logging.Log(exception);
    }
    catch (Exception exception)
    {
        // Catch unexpected Exceptions.
        Logging.Log(exception, false);
    }
}
```

To test this method we start by creating a new dictionary instance to pass to the method:

```cs
// Create Book Dictionary.
var dictionary = new Dictionary<int, Book>
{
    { 0, new Book("The Stand", "Stephen King") },
    { 1, new Book("The Name of the Wind", "Patrick Rothfuss") },
    { 2, new Book("Robinson Crusoe", "Daniel Defoe") },
    { 3, new Book("The Hobbit", "J.R.R. Tolkien") }
};

DictionaryExample(dictionary);
```

Executing this code now produces a `System.Collections.Generic.KeyNotFoundException`:

```
Key:
  0
Value:
  {Airbrake.Collections.Generic.KeyNotFoundException.Book(HashCode:36849274)}
    Author: "Stephen King"
    Title: "The Stand"
Key:
  1
Value:
  {Airbrake.Collections.Generic.KeyNotFoundException.Book(HashCode:63208015)}
    Author: "Patrick Rothfuss"
    Title: "The Name of the Wind"
Key:
  2
Value:
  {Airbrake.Collections.Generic.KeyNotFoundException.Book(HashCode:32001227)}
    Author: "Daniel Defoe"
    Title: "Robinson Crusoe"
Key:
  3
Value:
  {Airbrake.Collections.Generic.KeyNotFoundException.Book(HashCode:19575591)}
    Author: "J.R.R. Tolkien"
    Title: "The Hobbit"

--------------------
[EXPECTED] System.Collections.Generic.KeyNotFoundException: The given key was not present in the dictionary.
```

There's one major problem though: The `System.Collections.Generic.KeyNotFoundException` class doesn't provide any means of determining _which key_ was the problematic key that wasn't present and caused the exception in the first place.  There are a few different ways to deal with this problem:

- Use the [`Dictionary<TKey, TValue>.TryGetValue()`](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2.trygetvalue?view=netframework-4.7) method to explicitly check for a value at the specified key, then handle the `boolean` result of that method call to determine what should occur.
- Create a custom `ImprovedKeyNotFoundException` class that inherits from `System.Collections.Generic.KeyNotFoundException`, and includes a `.Key` property to track the problematic key.  Then create an `extension method` for `Dictionary<TKey, TValue>` that checks if a passed `key` exists in the dictionary.  If the `key` isn't found, throw a new `ImprovedKeyNotFoundException` with the associated problematic `.Key` property set for use in the exception output message.
- Alternatively, create a custom `ImprovedDictionary<TKey, TValue>` class that inherits from `Dictionary<TKey, TValue>` and overrides the `TValue` method to perform a `TryGetValue()` method call for the provided key.  If the `key` isn't found, throw a new `System.Collections.Generic.KeyNotFoundException` with a modified `Message` that includes the problematic `key` value.

Implementing the first solution of using [`Dictionary<TKey, TValue>.TryGetValue()`](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.idictionary-2.trygetvalue?view=netframework-4.7) is quite easy, so we'll start with that in the `DictionaryUsingTryGetValueExample(IReadOnlyDictionary<int, Book> dictionary)` method:

```cs
private static void DictionaryUsingTryGetValueExample(IReadOnlyDictionary<int, Book> dictionary)
{
    try
    {
        // Output current library.
        Logging.Log(dictionary);
        // Add line seperator for readability.
        Logging.LineSeparator();
        // Attempt to output element of index equal to count.
        if (dictionary.TryGetValue(dictionary.Count, out var book))
        {
            Logging.Log(book);
        }
        else
        {
            Logging.Log($"Element at index [{dictionary.Count}] could not be found.");
        }
    }
    catch (System.Collections.Generic.KeyNotFoundException exception)
    {
        // Catch KeyNotFoundExceptions.
        Logging.Log(exception);
    }
    catch (Exception exception)
    {
        // Catch unexpected Exceptions.
        Logging.Log(exception, false);
    }
}
```

We're passing the same `Dictionary<TKey, TValue>` instance that we used before to call this, so executing the above causes the `TryGetValue()` method to fail, outputting our custom failure message that includes the missing key:

```
Element at index [4] could not be found.
```

The two remaining options for handling the invalid `key` are a bit more complex.  However, of the two, the latter option of creating a new `ImprovedDictionary<TKey, TValue>` class that inherits from `Dictionary<TKey, TValue>` is far less code to implement, so we'll go with that one.  To do so we start with the new class:

```cs
/// <summary>
/// Improves base Dictionary behavior by including missing key value on failed key retrieval.
/// 
/// Inherits from Dictionary.
/// </summary>
/// <typeparam name="TKey">Key type.</typeparam>
/// <typeparam name="TValue">Value type.</typeparam>
public class ImprovedDictionary<TKey, TValue> : Dictionary<TKey, TValue>
{
    public new TValue this[TKey key]
    {
        get
        {
            // Try retrieving value by key and assign to value out variable.
            if (base.TryGetValue(key, out var value))
            {
                return value;
            }

            // If failure throw new KeyNotFoundException, including missing key.
            throw new System.Collections.Generic.KeyNotFoundException(
                $"The given key [{key}] was not present in the dictionary.");
        }
        set => base[key] = value;
    }
}
``` 

Since this inherits `Dictionary<TKey, TValue>` the behavior is nearly identical to a normal dictionary, except for the extra check when retrieving a `TValue`.  The downside, of course, is remembering to explicitly create dictionary instances using the new `ImprovedDictionary<TKey, TValue>` class.  To test this out we've created just such an object here:

```cs
// Create Book Dictionary.
var improvedDictionary = new ImprovedDictionary<int, Book>
{
    { 0, new Book("The Stand", "Stephen King") },
    { 1, new Book("The Name of the Wind", "Patrick Rothfuss") },
    { 2, new Book("Robinson Crusoe", "Daniel Defoe") },
    { 3, new Book("The Hobbit", "J.R.R. Tolkien") }
};

ImprovedDictionaryExample(improvedDictionary);
```

The result of our `ImprovedDictionaryExample()` method call attempting to access an invalid `key` element still throws a `System.Collections.Generic.KeyNotFoundException`, but notice that we successfully (and subtly) modified the exception `Message` property to include the problematic key:

```
Key:
  0
Value:
  {Airbrake.Collections.Generic.KeyNotFoundException.Book(HashCode:41962596)}
    Author: "Stephen King"
    Title: "The Stand"
Key:
  1
Value:
  {Airbrake.Collections.Generic.KeyNotFoundException.Book(HashCode:42119052)}
    Author: "Patrick Rothfuss"
    Title: "The Name of the Wind"
Key:
  2
Value:
  {Airbrake.Collections.Generic.KeyNotFoundException.Book(HashCode:43527150)}
    Author: "Daniel Defoe"
    Title: "Robinson Crusoe"
Key:
  3
Value:
  {Airbrake.Collections.Generic.KeyNotFoundException.Book(HashCode:56200037)}
    Author: "J.R.R. Tolkien"
    Title: "The Hobbit"

--------------------
[EXPECTED] System.Collections.Generic.KeyNotFoundException: The given key [4] was not present in the dictionary.
```

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A close look at the System.Collections.Generic.KeyNotFoundException in .NET, including a C# code showing how to handle different collection types.