# Behavioral Design Patterns: Iterator

Today we'll be exploring the Iterator design pattern in our ongoing [Guide to Software Design Patterns](https://airbrake.io/blog/software-design/software-design-patterns-guide) series.  The `iterator design pattern` may be the _most frequently used_ pattern we'll be looking at throughout this entire collection of articles.  The purpose of this pattern is to traverse through a collection of objects and retrieve each element in turn.  This practice is used in programming so often that many developers may not even realize it's considered a design pattern at all!

In this article we'll examine the `iterator design pattern` in more detail, looking at both a real world example and some fully-functional C# code that will illustrate a few different ways this pattern can be implemented.  Let's get going!

## In the Real World

One of the fundamental features of the `iterator pattern` is that it should allow a client to iterate through a collection, _without_ any knowledge about the implementation of that collection, or the specifics and ordering of individual elements.  Thus, this sort of iteration through an anonymous collection reminds me a lot of a real-world audition process.

Imagine you're the theater director of the popular local theater.  A playwright comes to you looking to put on a production of her next big hit, _Hamilton 2: Lincoln's Revenge_.  You put the word out that there are half a dozen major parts available and auditions will be held the next day.  Audition day rolls around and you sit third row center.  After meticulously arranging your pen and notepad just so, you call out, with appropriately dramatic flourish, that you're ready to begin.  The first auditionee is a meek, mousy young woman who doesn't seem to fit the part of the boisterous and fiendish queen she's trying out for, so you jot down a note and quickly shout, "Thank you.  Next!"  Out comes the next lamb to the slaughter, and the process repeats, again and again.

This is the core of the `iterator design pattern`.  There's a group (collection) of performers waiting backstage, but you (as the client) know nothing of what that group is made up of, the order they are in, or the individual people (elements) within it.  All you know is that when you yell, "Next!", another person from the larger group comes on stage and gives it their all, and then awaits your judgment.

## How It Works In Code

Since we're using C# for our sample code, implementing our own `iterator design pattern` is extremely easy, if we want it to be.  .NET provides built-in interfaces, like [`IEnumerable`](https://docs.microsoft.com/en-us/dotnet/api/system.collections.ienumerable?view=netframework-4.7), that make traversal over a collection a piece of cake to code.

Therefore, in the spirit of actually illustrating the underlying components of a traditional `iterator pattern`, our code sample will contain _two_ different types of iterator implementations.  The **basic** iterator will show a more traditional pattern and all the actual components necessary to make a simple iterator in just about any programming language.  The **advanced** iterator, on the other hand, illustrates how we can use built-in .NET interfaces and members to implement an iterator as quickly and easily as possible.

As usual, we begin with the full code sample below, so it can be copy/pasted for your own testing purpose if you're following along.  After the code break, we'll then dive into the specifics and see exactly what's going on:

```cs
// Program.cs
using Iterator.Advanced;
using Iterator.Basic;
using Utility;

namespace Iterator
{
    class Program
    {
        static void Main()
        {
            Logging.LineSeparator("BASIC ITERATOR");
            BasicIteratorTest();

            Logging.LineSeparator("ADVANCED ITERATOR");
            AdvancedIteratorTest();
        }

        public static void BasicIteratorTest()
        {
            var aggregate = new Aggregate();

            // Add Books to aggregate collection.
            aggregate.Add(new Book("The Hobbit", "J.R.R. Tolkien", 304));
            aggregate.Add(new Book("The Name of the Wind", "Patrick Rothfuss", 662));
            aggregate.Add(new Book("To Kill a Mockingbird", "Harper Lee", 281));
            aggregate.Add(new Book("1984", "George Orwell", 328));
            aggregate.Add(new Book("Jane Eyre", "Charlotte Brontë", 507));

            // Get new Iterator from aggregate.
            var iterator = aggregate.CreateIterator();
            // Loop while Next() element exists.
            while (iterator.Next())
            {
                // Output current object.
                Logging.Log(iterator.Current);
            }
        }

        public static void AdvancedIteratorTest()
        {
            // Create a new Advanced.Iterator<Book> collection.
            var collection = new Iterator<Book>
            {
                // Pass Books to Values property.
                Values = {
                    new Book("The Hobbit", "J.R.R. Tolkien", 304),
                    new Book("The Name of the Wind", "Patrick Rothfuss", 662),
                    new Book("To Kill a Mockingbird", "Harper Lee", 281),
                    new Book("1984", "George Orwell", 328) ,
                    new Book("Jane Eyre", "Charlotte Brontë", 507)
                }
            };

            // Iterate through collection and retrieve each Book.
            foreach (var book in collection)
            {
                // Output Book.
                Logging.Log(book);
            }
        }
    }
}

// <Basic>/Aggregate.cs
using System.Collections;

namespace Iterator.Basic
{
    /// <summary>
    /// Defines members of Aggregate.
    /// </summary>
    public interface IAggregate
    {
        IIterator CreateIterator();
    }

    /// <summary>
    /// Holds the iterable collection and allows retrieval of collection objects.
    /// </summary>
    public class Aggregate : IAggregate
    {
        /// <summary>
        /// Iterable collection.
        /// </summary>
        private readonly ArrayList _items = new ArrayList();

        /// <summary>
        /// Create and get a new Iterator instance of this collection.
        /// </summary>
        /// <returns>New Iterator instance.</returns>
        public IIterator CreateIterator()
        {
            return new Iterator(this);
        }

        /// <summary>
        /// Get a collection element by index.
        /// </summary>
        /// <param name="index">Index to retrieve.</param>
        /// <returns>Collection object.</returns>
        public object this[int index] => _items[index];

        /// <summary>
        /// Current collection count.
        /// </summary>
        public int Count => _items.Count;

        /// <summary>
        /// Adds the passed object to collection.
        /// </summary>
        /// <param name="o">Object to be added.</param>
        public void Add(object o)
        {
            _items.Add(o);
        }
    }
}

// <Basic>/Iterator.cs
namespace Iterator.Basic
{
    /// <summary>
    /// Defines members of Iterator.
    /// </summary>
    public interface IIterator
    {
        object Current { get; }
        bool Next();
    }

    /// <summary>
    /// Handles iteration logic for passed Aggregate.
    /// </summary>
    public class Iterator : IIterator
    {
        private readonly Aggregate _aggregate;
        private int _index = -1;

        public Iterator(Aggregate aggregate)
        {
            _aggregate = aggregate;
        }

        /// <summary>
        /// Get the Aggregate collection element of current index, otherwise null.
        /// </summary>
        public object Current => _index < _aggregate.Count ? _aggregate[_index] : null;

        /// <summary>
        /// Iterate the index count.
        /// </summary>
        /// <returns>Indicates if index is within bounds of collection indices.</returns>
        public bool Next()
        {
            _index++;
            return _index < _aggregate.Count;
        }
    }
}

// <Advanced>/Iterator.cs
using System.Collections;
using System.Collections.Generic;

namespace Iterator.Advanced
{
    /// <summary>
    /// Generic iterator.
    /// </summary>
    /// <typeparam name="T">Any object type to be enumerated.</typeparam>
    public class Iterator<T> : IEnumerable<T>
    {
        /// <summary>
        /// Value collection list to be iterated.
        /// </summary>
        public List<T> Values = new List<T>();

        /// <summary>
        /// Gets the current value count.
        /// </summary>
        public int Count => Values.Count;

        /// <summary>
        /// Yields enumerated object from collection.
        /// </summary>
        /// <returns>Next iterated object.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (var index = Count - 1; index >= 0; index--)
            {
                yield return Values[index];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}

// <Utility>/Book.cs
namespace Utility
{
    public interface IBook
    {
        string Author { get; set; }
        int PageCount { get; set; }
        string Title { get; set; }
    }

    /// <summary>
    /// Simple Book class.
    /// </summary>
    public class Book : IBook
    {
        public string Author { get; set; }
        public int PageCount { get; set; }
        public string Title { get; set; }

        public Book() { }

        public Book(string title, string author)
        {
            Author = author;
            Title = title;
        }

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
}


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

---

For all generic implementations of the `iterator design pattern` we effectively need three components:

- `Aggregate` - Houses the collection of elements to be iterated, along with a way to access an `Iterator` instance.
- `Iterator` - Handles iteration logic of the passed `Aggregate` collection.
- `Client` - Creates the `Aggregate` instance and collection, then performs the actual iteration looping.

Therefore, our **basic** iterator example begins with the `IAggregate` interface and `Aggregate` class that implements it:

```cs
// <Basic>/Aggregate.cs
using System.Collections;

namespace Iterator.Basic
{
    /// <summary>
    /// Defines members of Aggregate.
    /// </summary>
    public interface IAggregate
    {
        IIterator CreateIterator();
    }

    /// <summary>
    /// Holds the iterable collection and allows retrieval of collection objects.
    /// </summary>
    public class Aggregate : IAggregate
    {
        /// <summary>
        /// Iterable collection.
        /// </summary>
        private readonly ArrayList _items = new ArrayList();

        /// <summary>
        /// Create and get a new Iterator instance of this collection.
        /// </summary>
        /// <returns>New Iterator instance.</returns>
        public IIterator CreateIterator()
        {
            return new Iterator(this);
        }

        /// <summary>
        /// Get a collection element by index.
        /// </summary>
        /// <param name="index">Index to retrieve.</param>
        /// <returns>Collection object.</returns>
        public object this[int index] => _items[index];

        /// <summary>
        /// Current collection count.
        /// </summary>
        public int Count => _items.Count;

        /// <summary>
        /// Adds the passed object to collection.
        /// </summary>
        /// <param name="o">Object to be added.</param>
        public void Add(object o)
        {
            _items.Add(o);
        }
    }
}
```

The primary functionality is that the `Aggregate` should hold our object collection and provide an `Iterator` instance so we can iterate over the elements.  For simplicity, we've also provide access to the underlying elements by index via the `this[int index]` property.  This will be used in the `Iterator` class to perform iteration based on the element index.

Speaking of the iterator, let's take a look at the `IIterator` interface and the `Iterator` class that implements it:

```cs
// <Basic>/Iterator.cs
namespace Iterator.Basic
{
    /// <summary>
    /// Defines members of Iterator.
    /// </summary>
    public interface IIterator
    {
        object Current { get; }
        bool Next();
    }

    /// <summary>
    /// Handles iteration logic for passed Aggregate.
    /// </summary>
    public class Iterator : IIterator
    {
        private readonly Aggregate _aggregate;
        private int _index = -1;

        public Iterator(Aggregate aggregate)
        {
            _aggregate = aggregate;
        }

        /// <summary>
        /// Get the Aggregate collection element of current index, otherwise null.
        /// </summary>
        public object Current => _index < _aggregate.Count ? _aggregate[_index] : null;

        /// <summary>
        /// Iterate the index count.
        /// </summary>
        /// <returns>Indicates if index is within bounds of collection indices.</returns>
        public bool Next()
        {
            _index++;
            return _index < _aggregate.Count;
        }
    }
}
```

The fundamental requirements of the `Iterator` is that it can retrieve the next element in the underlying `Aggregate` collection.  We just iterate the private `_index` property when the `Next()` method is called, returning a boolean that indicates if the index is within the bounds of the collection or not.  Alternatively, we could also have the `Next()` method return the next object element, but we've abstracted that task to the `Current` property instead.

To test out our **basic** iterator we now need some sort of `client` code, which is setup in the main `Program` class:

```cs
// Program.cs
using Iterator.Advanced;
using Iterator.Basic;
using Utility;

namespace Iterator
{
    class Program
    {
        static void Main()
        {
            Logging.LineSeparator("BASIC ITERATOR");
            BasicIteratorTest();

            Logging.LineSeparator("ADVANCED ITERATOR");
            AdvancedIteratorTest();
        }

        public static void BasicIteratorTest()
        {
            var aggregate = new Aggregate();

            // Add Books to aggregate collection.
            aggregate.Add(new Book("The Hobbit", "J.R.R. Tolkien", 304));
            aggregate.Add(new Book("The Name of the Wind", "Patrick Rothfuss", 662));
            aggregate.Add(new Book("To Kill a Mockingbird", "Harper Lee", 281));
            aggregate.Add(new Book("1984", "George Orwell", 328));
            aggregate.Add(new Book("Jane Eyre", "Charlotte Brontë", 507));

            // Get new Iterator from aggregate.
            var iterator = aggregate.CreateIterator();
            // Loop while Next() element exists.
            while (iterator.Next())
            {
                // Output current object.
                Logging.Log(iterator.Current);
            }
        }

        // ...
    }
}
```

The `BasicIteratorTest()` method performs all the client logic.  In this case, we create a new `Aggregate` instance and populate its collection with a list of `Books`.  Next, we need to retrieve an instance of the `Iterator` that will iterate over the aggregate collection.  With the `iterator` in hand we can call `iterator.Next()` to see if the next element exists, and while it does so, call `iterator.Current` and output the result to the log.

If all is working correctly, we should get a log output for each `Book`.  Sure enough, that's just what we see:

```
------------ BASIC ITERATOR ------------
{Utility.Book(HashCode:30015890)}
  Author: "J.R.R. Tolkien"
  PageCount: 304
  Title: "The Hobbit"

{Utility.Book(HashCode:1707556)}
  Author: "Patrick Rothfuss"
  PageCount: 662
  Title: "The Name of the Wind"

{Utility.Book(HashCode:15368010)}
  Author: "Harper Lee"
  PageCount: 281
  Title: "To Kill a Mockingbird"

{Utility.Book(HashCode:4094363)}
  Author: "George Orwell"
  PageCount: 328
  Title: "1984"

{Utility.Book(HashCode:36849274)}
  Author: "Charlotte Brontë"
  PageCount: 507
  Title: "Jane Eyre"
```

That shows the generic way of implementing an iterator in most languages.  Now, let's look at the **advanced** iterator, where we use built-in components provided by .NET to simplify the process and limit our code requirement as much as possible.

Our iterator and aggregate components are all contained within the `Iterator<T>` class:

```cs
// <Advanced>/Iterator.cs
using System.Collections;
using System.Collections.Generic;

namespace Iterator.Advanced
{
    /// <summary>
    /// Generic iterator.
    /// </summary>
    /// <typeparam name="T">Any object type to be enumerated.</typeparam>
    public class Iterator<T> : IEnumerable<T>
    {
        /// <summary>
        /// Value collection list to be iterated.
        /// </summary>
        public List<T> Values = new List<T>();

        /// <summary>
        /// Gets the current value count.
        /// </summary>
        public int Count => Values.Count;

        /// <summary>
        /// Yields enumerated object from collection.
        /// </summary>
        /// <returns>Next iterated object.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (var index = Count - 1; index >= 0; index--)
            {
                yield return Values[index];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
```

We want to ensure this iterator is generic (can be used with any type of object), so we use `<T>` throughout the code.  Most importantly, the `Iterator<T>` class implements the [`IEnumerable<T>`](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.ienumerable-1?view=netframework-4.7) interface, which exposes built-in members that handle iteration logic for us.  All we need to do is define the `GetEnumerator()` and `IEnumerable.GetEnumerator()` methods within our class, and .NET will handle the rest for us.  In this case, `GetEnumerator()` loops through the collection via the index and returns each element via the [`yield`](https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/yield) keyword.  The `yield` keyword halts execution each time it is called, allowing the iterator to call each subsequent element within the collection.

The `Program.AdvancedIteratorTest()` method acts as our `client` component:

```cs
public static void AdvancedIteratorTest()
{
    // Create a new Advanced.Iterator<Book> collection.
    var collection = new Iterator<Book>
    {
        // Pass Books to Values property.
        Values = {
            new Book("The Hobbit", "J.R.R. Tolkien", 304),
            new Book("The Name of the Wind", "Patrick Rothfuss", 662),
            new Book("To Kill a Mockingbird", "Harper Lee", 281),
            new Book("1984", "George Orwell", 328) ,
            new Book("Jane Eyre", "Charlotte Brontë", 507)
        }
    };

    // Iterate through collection and retrieve each Book.
    foreach (var book in collection)
    {
        // Output Book.
        Logging.Log(book);
    }
}
```

We start by creating a new `Iterator<Book>` instance, then add the `Book` collection to the `Values` property.  Now we can loop through the `Iterator` collection just as we would any other `IEnumerable` in C#.  In this case, we can use `foreach` to retrieve each object, which is `yielded` to us via the `GetEnumerator()` method.

As before, we should see the output showing us all the `Books` in our collection:

```
---------- ADVANCED ITERATOR -----------
{Utility.Book(HashCode:63208015)}
  Author: "Charlotte Brontë"
  PageCount: 507
  Title: "Jane Eyre"

{Utility.Book(HashCode:32001227)}
  Author: "George Orwell"
  PageCount: 328
  Title: "1984"

{Utility.Book(HashCode:19575591)}
  Author: "Harper Lee"
  PageCount: 281
  Title: "To Kill a Mockingbird"

{Utility.Book(HashCode:41962596)}
  Author: "Patrick Rothfuss"
  PageCount: 662
  Title: "The Name of the Wind"

{Utility.Book(HashCode:42119052)}
  Author: "J.R.R. Tolkien"
  PageCount: 304
  Title: "The Hobbit"
```

---

There we have it!  We saw two different methods for implementing the `iterator design pattern`, depending how much you want to rely on built-in APIs versus your own explicit code.  For more information on all the other popular design patterns, head on over to our [ongoing design pattern series here](https://airbrake.io/blog/software-design/software-design-patterns-guide)!

---

__META DESCRIPTION__

Part 16 of our Software Design Pattern series in which examine the iterator design pattern using fully-functional C# example code.