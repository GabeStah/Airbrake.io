# Structural Design Patterns: Facade

Today, as we make our way through the last few `Structural` design patterns in our extensive [`Guide to Software Design Patterns`](https://airbrake.io/blog/software-design/software-design-patterns-guide) series, we'll be taking a closer look at the facade design pattern.  The `facade design pattern` is one of the simpler (and thus most commonly used) patterns we'll come across, as it provides a simple interface that can be used to manipulate more complex logic behind the scenes, hidden from the client executor.

Throughout this article we'll dig into the `facade pattern` a bit more by looking at a real world example, along with some fully-functional `C#` code to illustrate how the pattern can be used in your own development.  Let's get this party started!

## In the Real World

The `facade design pattern` makes it rather easy to simplify complex processes into more abstract concepts, unbeknownst to the instigator of said processes.  This practice is applied in virtually _all_ forms of business today, but the most obvious application is in manufacturing.  For example, imagine you are an executive at General Motors and you want to produce a new model of car.  Your involvement in that process will be rather limited, relative to the overall complexity of releasing a new car to the world.  Ultimately, you might make some top-level decisions, such as the name, style, major features, pricing, and so forth, but the actual creation of the car (and all its subsequent components) will be accomplished by thousands of other employees and departments across your company.

This is a simple example of the `facade design pattern` in action: The client (you, as an executive at GM in this case) makes a top-level decision about a task that is to be performed (creating a new car model).  The client initiates this process with some basic inputs (name, style, price, etc), but then the majority of the grunt work is performed by more concrete worker objects (in this case, actual workers).  One depertment might handle electronics, another the body shape and styling, another the engine, and so forth.  As the instigating client you'll likely perform regular checkups to see how progress is coming along, but these updates will typically be abstract notions rather than highly-detailed specifics.  The Head of Electronics might indicate that 80% of the work is complete, but an extra two weeks are required.  The specifics of what exactly is going on with the electronics -- which components are working and which need to be redesigned -- will often be irrelevant to you, as the client that began the process of creating a new model.

These concepts bleed into nearly every aspect of business, but that's a relatively common (and simple) real world example of the `facade design pattern` being put to good use.

## How It Works In Code

For `facade design pattern` code example we'll be sticking with one of my favorite topics: books.  In this case, our client wants to check if a particular book is ready to be published, but the client doesn't want to concern him or herself with the specific status of each component involved in publication (author, content, editor, publisher, cover, and so forth).  The client just wants an easy way to determine if a particular book is ready to be published or not, so this is a perfect scenario in which to implement a `facade pattern`.

As is often the case, we'll start with the full working code example right away, and afterward we'll go over it in more detail to see how the `facade design pattern` is implemented:

```cs
using Utility;

namespace Facade
{
    /// <summary>
    /// Houses all Author logic.
    /// </summary>
    class Author
    {
        public string Name { get; set; }
        
        public Author() { }

        public Author(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Determine if Book has a Author.
        /// </summary>
        /// <param name="book">Book to check.</param>
        /// <returns>Indicates if Book has a Author.</returns>
        public bool HasAuthor(Book book)
        {
            return !string.IsNullOrEmpty(book.Author?.Name);
        }
    }

    /// <summary>
    /// Houses all content logic.
    /// </summary>
    class Content
    {
        public string Title { get; set; }

        public Content() { }

        public Content(string title)
        {
            Title = title;
        }

        /// <summary>
        /// Determine if Book has Content.
        /// </summary>
        /// <param name="book">Book to check.</param>
        /// <returns>Indicates if Book has Content.</returns>
        public bool HasContent(Book book)
        {
            return !string.IsNullOrEmpty(book.Content?.Title);
        }
    }

    /// <summary>
    /// Houses all Cover logic.
    /// </summary>
    class Cover
    {
        /// <summary>
        /// Determine if Book has a Cover.
        /// </summary>
        /// <param name="book">Book to check.</param>
        /// <returns>Indicates if Book has a Cover.</returns>
        public bool HasCover(Book book) => book.Cover != null;
    }

    /// <summary>
    /// Houses all Editor logic.
    /// </summary>
    class Editor
    {
        /// <summary>
        /// Determine if Book is Edited.
        /// </summary>
        /// <param name="book">Book to check.</param>
        /// <returns>Indicates if Book is Edited.</returns>
        public bool HasEditor(Book book) => book.Editor != null;
    }

    /// <summary>
    /// Houses all Publisher logic.
    /// </summary>
    class Publisher
    {
        /// <summary>
        /// Determine if Book has a Publisher.
        /// </summary>
        /// <param name="book">Book to check.</param>
        /// <returns>Indicates if Book has a Publisher.</returns>
        public bool HasPublisher(Book book) => book.Publisher != null;
    }

    /// <summary>
    /// Basic Book object.
    /// </summary>
    class Book
    {
        public Author Author { get; set; }        
        public Content Content { get; set; }
        public Editor Editor { get; set; }
        public Cover Cover { get; set; }
        public Publisher Publisher { get; set; }

        public Book() { }

        public Book(Author author, Content content, Editor editor, Cover cover, Publisher publisher)
        {
            Author = author;            
            Content = content;
            Editor = editor;
            Cover = cover;
            Publisher = publisher;
        }
    }

    /// <summary>
    /// Facade which delegates client requests to appropriate sub-objects.
    /// In this case, determines if passed Book is ready to be published.
    /// </summary>
    class BookFacade
    {
        internal Author Author { get; set; } = new Author();
        internal Content Content { get; set; } = new Content();
        internal Cover Cover { get; set; } = new Cover();
        internal Editor Editor { get; set; } = new Editor();
        internal Publisher Publisher { get; set; } = new Publisher();

        /// <summary>
        /// Determine if passed Book is publishable.
        /// </summary>
        /// <param name="book">Book instance to verify.</param>
        /// <returns>If Book is ready to be published.</returns>
        public bool IsPublishable(Book book)
        {
            return Author.HasAuthor(book) &&
                   Content.HasContent(book) &&
                   Cover.HasCover(book) &&
                   Editor.HasEditor(book) &&
                   Publisher.HasPublisher(book);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // New BookFacade instance.
            var bookFacade = new BookFacade();

            // Create empty Book instance.
            var emptyBook = new Book();
            // Output book.
            Logging.Log(emptyBook);
            // Check if book is publishable.
            Logging.Log($"Book is publishable? {bookFacade.IsPublishable(emptyBook)}");

            Logging.LineSeparator(40);

            // Create populated Book.
            var populatedBook = new Book(author: new Author("Stephen King"),
                                         content: new Content("The Stand"),
                                         editor: new Editor(),
                                         cover: new Cover(),
                                         publisher: new Publisher());
            // Output book.
            Logging.Log(populatedBook);
            // Check if book is publishable.
            Logging.Log($"Book is publishable? {bookFacade.IsPublishable(populatedBook)}");
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
        public static void LineSeparator(int length = 20)
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

The `facade design pattern` is intended to simplify the behavior of many sub-components that are required to create a larger, more abstract concept.  Therefore, in the case of publishing a book we start with a series of classes that all perform a unique role in creating the completed work:

```cs
/// <summary>
/// Houses all Author logic.
/// </summary>
class Author
{
    public string Name { get; set; }
    
    public Author() { }

    public Author(string name)
    {
        Name = name;
    }

    /// <summary>
    /// Determine if Book has a Author.
    /// </summary>
    /// <param name="book">Book to check.</param>
    /// <returns>Indicates if Book has a Author.</returns>
    public bool HasAuthor(Book book)
    {
        return !string.IsNullOrEmpty(book.Author?.Name);
    }
}

/// <summary>
/// Houses all content logic.
/// </summary>
class Content
{
    public string Title { get; set; }

    public Content() { }

    public Content(string title)
    {
        Title = title;
    }

    /// <summary>
    /// Determine if Book has Content.
    /// </summary>
    /// <param name="book">Book to check.</param>
    /// <returns>Indicates if Book has Content.</returns>
    public bool HasContent(Book book)
    {
        return !string.IsNullOrEmpty(book.Content?.Title);
    }
}

/// <summary>
/// Houses all Cover logic.
/// </summary>
class Cover
{
    /// <summary>
    /// Determine if Book has a Cover.
    /// </summary>
    /// <param name="book">Book to check.</param>
    /// <returns>Indicates if Book has a Cover.</returns>
    public bool HasCover(Book book) => book.Cover != null;
}

/// <summary>
/// Houses all Editor logic.
/// </summary>
class Editor
{
    /// <summary>
    /// Determine if Book is Edited.
    /// </summary>
    /// <param name="book">Book to check.</param>
    /// <returns>Indicates if Book is Edited.</returns>
    public bool HasEditor(Book book) => book.Editor != null;
}

/// <summary>
/// Houses all Publisher logic.
/// </summary>
class Publisher
{
    /// <summary>
    /// Determine if Book has a Publisher.
    /// </summary>
    /// <param name="book">Book to check.</param>
    /// <returns>Indicates if Book has a Publisher.</returns>
    public bool HasPublisher(Book book) => book.Publisher != null;
}
```

We have five component classes that we'll be using here.  As you can see, `Author` and `Content` include a few `constructor overloads` to allow for population of the `Name` and `Title` properties, respectively.  Otherwise, all five of these component classes are quite basic, simply implementing a `HasClassName()` method that returns a `boolean` indicating if the passed in `book` parameter meets the requirements of that class.  For `Cover.HasCover()` and the like, this method merely checks for the existence of the `Cover` property associated with the `book` parameter.  `Author`, on the other hand, also verifies that the `Author` has a `Name`.

Again, the logic of these component classes can be expanded as much as necessary, but the basic principles still apply.  We're able to use all of these classes, along with their internal logic, and in tandem with a `facade` of some sort, to determine if a book is ready to be published.

We next need to define our `Book` class, which each of our component classes expects as a passed parameter:

```cs
/// <summary>
/// Basic Book object.
/// </summary>
class Book
{
    public Author Author { get; set; }
    public Content Content { get; set; }
    public Editor Editor { get; set; }
    public Cover Cover { get; set; }
    public Publisher Publisher { get; set; }

    public Book() { }

    public Book(Author author, Content content, Editor editor, Cover cover, Publisher publisher)
    {
        Author = author;
        Content = content;
        Editor = editor;
        Cover = cover;
        Publisher = publisher;
    }
}
```

Nothing too fancy going on here.  Just to simplify things we're keeping track of `public properties` of each of our component classes within the `Book` class, which can be easily populated with one of the constructor overloads.  That said, we're not performing _any_ logic about the `Book` instance or its readiness for publishing within this class, because we want to handle that via a `facade` if some sort.  As it happens, we'll handle that `facade` right now with our final definition, the `BookFacade` class:

```cs
/// <summary>
/// Facade which delegates client requests to appropriate sub-components.
/// In this case, determines if passed Book is ready to be published.
/// </summary>
class BookFacade
{
    internal Author Author { get; set; } = new Author();
    internal Content Content { get; set; } = new Content();
    internal Cover Cover { get; set; } = new Cover();
    internal Editor Editor { get; set; } = new Editor();
    internal Publisher Publisher { get; set; } = new Publisher();

    /// <summary>
    /// Determine if passed Book is publishable.
    /// </summary>
    /// <param name="book">Book instance to verify.</param>
    /// <returns>If Book is ready to be published.</returns>
    public bool IsPublishable(Book book)
    {
        return Author.HasAuthor(book) &&
               Content.HasContent(book) &&
               Cover.HasCover(book) &&
               Editor.HasEditor(book) &&
               Publisher.HasPublisher(book);
    }
}
```

Here we've decided to use `internal` properties for each of our component classes (along with initial instances as their default values).  This ensures that we can _use_ instances of these components within our `BookFacade` logic, without exposing them to outside clients or other objects.

Our entire `facade design pattern` example here boils down to the `BookFacade.IsPublishable(Book book)` method, which checks that every component class returns a positive (`true`) value for their respective `HasClassName()` method call.  Thus, a client only needs to call `IsPublishable()` with a passed in `Book` instance to determine if all the underlying component logic is valid or not.

To put this into action we have two book instances we've created:

```cs
class Program
{
    static void Main(string[] args)
    {
        // New BookFacade instance.
        var bookFacade = new BookFacade();

        // Create empty Book instance.
        var emptyBook = new Book();
        // Output book.
        Logging.Log(emptyBook);
        // Check if book is publishable.
        Logging.Log($"Book is publishable? {bookFacade.IsPublishable(emptyBook)}");

        Logging.LineSeparator(40);

        // Create populated Book.
        var populatedBook = new Book(author: new Author("Stephen King"),
                                     content: new Content("The Stand"),
                                     editor: new Editor(),
                                     cover: new Cover(),
                                     publisher: new Publisher());
        // Output book.
        Logging.Log(populatedBook);
        // Check if book is publishable.
        Logging.Log($"Book is publishable? {bookFacade.IsPublishable(populatedBook)}");
    }
}
```

We start by instantiating a new `BookFacade` object, then move onto creating an empty `Book` object, before outputting the content of `emptyBook` and the status of the `bookFacade.IsPublishable()` method result.  As we should expect, since a completely empty `Book` instance doesn't contain any of the requisite component property instances, `emptyBook` isn't publishable:

```
{Facade.Book(HashCode:46104728)}
  Author: { }
    null
  Content: { }
    null
  Editor: { }
    null
  Cover: { }
    null
  Publisher: { }
    null

Book is publishable? False
```

We've also created a second `Book` instance called `populatedBook`, and passed in new instances of all the component classes.  This should populate all the required properties of `populatedBook`, giving us a positive result from the `IsPublishable()`.  Sure enough, it works!

```
{Facade.Book(HashCode:1707556)}
  Author: { }
    {Facade.Author(HashCode:15368010)}
      Name: "Stephen King"
  Content: { }
    {Facade.Content(HashCode:4094363)}
      Title: "The Stand"
  Editor: { }
    {Facade.Editor(HashCode:36849274)}
  Cover: { }
    {Facade.Cover(HashCode:63208015)}
  Publisher: { }
    {Facade.Publisher(HashCode:32001227)}

Book is publishable? True
```

This is just a small taste of what can be accomplished with the `facade design pattern`, but I hope it gave you a bit more insight into the simplicity and potential power it can provide.  Check out more design patterns in our [ongoing series over here](https://airbrake.io/blog/software-design/software-design-patterns-guide)!

---

__META DESCRIPTION__

Part 11 of our Software Design Pattern series in which examine the facade design pattern using fully-functional C# example code.