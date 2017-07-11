# Structural Design Patterns: Flyweight

Dive into the exciting world of the flyweight design pattern in today's article, as we continue looking at `Structural` design patterns throughout our extensive [Guide to Software Design Patterns](https://airbrake.io/blog/software-design/software-design-patterns-guide) series.  The `flyweight design pattern`, named after the boxing weight class of the same name, is intended to be just as agile and adaptive as those nimble athletes.  `Flyweight` gains this agility by minimizing memory and computational usage by sharing and reusing objects.

In this article we'll explore the `flyweight pattern` in more detail, looking at both a real world and fully-functional C# code example to help illustrate how the pattern can best be used, so let's get to it!

## In the Real World

Virtually any instance of reuse that we might encounter in our day-to-day lives would arguably constitute a form of the `flyweight pattern`.  However, since one major principle of the pattern is the notion of "sharing" -- and since I personally just returned a movie rental yesterday -- the immediate example that comes to my mind is physical movie rental services like the ubiquitous Redbox.

In case you aren't aware, Redbox is a DVD-rental service that provides little red kiosks, plopped outside storefronts and the like, each packed with an assortment of movies and console games that can be rented for a day at the cost of a few bucks.  Select your movie, swipe your credit card, and the machine spits out your Blu-ray or DVD to take home.  If can get home fast enough, you can possibly binge-watch _50 Shades of Grey_ **eleven times** during your 24-hour rental period, before you'll need to rush back to a Redbox kiosk and return the now well-worn disc.

Movie tastes aside, the Redbox business model (and the entire concept of movie rentals, for the matter), is a great real world example of the `flyweight design pattern`.  Movie studios and distributors don't want to print an excess of discs, but they don't want an availability shortage either.  Since discs are generally quite cheap to print and distribute, most companies err on the side of excess.  This is where Redbox (and similar services) carve out their entire business model: Scooping up those excess discs on the cheap and renting them out to viewers who don't mind stopping by a kiosk to grab a movie that isn't available on streaming services.

Since each Redbox kiosk may only contain a handful of copies of a specific movie, most individual discs will be rented and viewed by many people over the course of their lifespan.  The distributor and/or Redbox only need a relatively small library of discs inside a single kiosk to facilitate rentals from many, many individuals.  The ability to "reuse" discs between different renters, as well as sharing discs between customers and across other kiosks, is exactly what the `flyweight pattern` aims to accomplish.

## How It Works In Code

Even though I used movies as a real world example of the `flyweight design pattern`, we're going to use more literary examples in our code sample.  As usual, let's start with the full code below, then we'll dig into it a bit more afterward:

```cs
using System;
using System.Collections.Generic;
using Utility;

namespace Flyweight
{
    class Program
    {
        static void Main(string[] args)
        {
            Example1();
            Example2();
            Example3();
        }

        public static void Example1()
        {
            var library = new Library();
            var book = library.GetPublication(
                Tuple.Create(
                    new Author("Patrick Rothfuss"),
                    "The Name of the Wind",
                    PublicationType.Book
                )
            );

            var graphicNovel = library.GetPublication(
                Tuple.Create(
                    new Author("Julie Doucet"),
                    "My New York Diary",
                    PublicationType.GraphicNovel
                )
            );

            // Try retrieving Publication with same key.
            book = library.GetPublication(
                Tuple.Create(
                    new Author("Patrick Rothfuss"),
                    "The Name of the Wind",
                    PublicationType.Book
                )
            );

            Logging.Log($"Library contains [{library.GetPublicationCount}] publications.");
        }

        public static void Example2()
        {
            // Create library.
            var library = new Library();

            // Create Author instances.
            var patrickRothfuss = new Author("Patrick Rothfuss");
            var julieDoucet = new Author("Julie Doucet");

            // Create or retrieve new book.
            var book = library.GetPublication(
                Tuple.Create(
                    patrickRothfuss,
                    "The Name of the Wind",
                    PublicationType.Book
                )
            );

            var graphicNovel = library.GetPublication(
                Tuple.Create(
                    julieDoucet,
                    "My New York Diary",
                    PublicationType.GraphicNovel
                )
            );

            // Try retrieving Publication with same key.
            book = library.GetPublication(
                Tuple.Create(
                    patrickRothfuss,
                    "The Name of the Wind",
                    PublicationType.Book
                )
            );

            Logging.Log($"Library contains [{library.GetPublicationCount}] publications.");
        }

        public static void Example3()
        {
            var library = new Library();

            // Try to retrieve a Publication with an invalid PublicationType.
            library.GetPublication(
                Tuple.Create(
                    new Author("Dante"), 
                    "Divine Comedy", 
                    PublicationType.Epic
                )
            );
        }
    }
    
    /// <summary>
    /// Houses all Author logic.
    /// </summary>
    public class Author
    {
        public string Name { get; set; }

        public Author(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// Houses all Illustrator logic.
    /// </summary>
    public class Illustrator
    {
        public string Name { get; set; }

        public Illustrator(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// Houses all Publisher logic.
    /// </summary>
    public class Publisher
    {
        public string Name { get; set; }

        public Publisher(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// 
    /// 
    /// Acts as the Flyweight interface.
    /// </summary>
    public interface IPublication
    {
        Author Author { get; set; }
        Publisher Publisher { get; set; }
        string Title { get; set; }
    }

    /// <summary>
    /// Defines the allowed publication types.
    /// </summary>
    public enum PublicationType
    {
        Book,
        Epic,
        GraphicNovel
    }

    /// <summary>
    /// Acts as a ConcreteFlyweight class.
    /// </summary>
    public class Book : IPublication
    {
        public Author Author { get; set; }
        public int PageCount { get; set; }
        public Publisher Publisher { get; set; }
        public string Title { get; set; }

        public Book(Author author, Publisher publisher, string title)
        {
            Author = author;
            Publisher = publisher;
            Title = title;
        }

        public Book(Author author, int pageCount, Publisher publisher, string title)
        {
            Author = author;
            PageCount = pageCount;
            Publisher = publisher;
            Title = title;
        }
    }

    /// <summary>
    /// Acts as a ConcreteFlyweight class.
    /// </summary>
    public class GraphicNovel : IPublication
    {
        public Author Author { get; set; }
        public Illustrator Illustrator { get; set; }
        public Publisher Publisher { get; set; }
        public string Title { get; set; }

        public GraphicNovel(Author author, Illustrator illustrator, Publisher publisher, string title)
        {
            Author = author;
            Illustrator = illustrator;
            Publisher = publisher;
            Title = title;
        }
    }

    /// <summary>
    /// Houses all publications.
    /// Storage uses Dictionary with Tuple key for author, title, and publication type.
    /// 
    /// Acts as FlyweightFactory.
    /// </summary>
    public class Library
    {
        /// <summary>
        /// Stores all publication data privately.  Should not be publically accessible 
        /// since we want to force access through GetPublication() method.
        /// </summary>
        protected Dictionary<Tuple<Author, string, PublicationType>, IPublication> Publications = 
            new Dictionary<Tuple<Author, string, PublicationType>, IPublication>();

        /// <summary>
        /// Get the count of all publications in library.
        /// </summary>
        public int GetPublicationCount => Publications.Count;

        /// <summary>
        /// Retrieve a Publication by passed key Tuple.
        /// If an item with matching key exists, retrieve from private Publications property.
        /// Otherwise, generate a new instance, add to list, and return result.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IPublication GetPublication(Tuple<Author, string, PublicationType> key)
        {
            IPublication publication = null;
            try
            {
                if (Publications.ContainsKey(key))
                {
                    publication = Publications[key];
                    // Output existing publication data.
                    Logging.LineSeparator();
                    Logging.Log($"Existing Publication located:");
                    Logging.Log(publication);
                }
                else
                {
                    switch (key.Item3)
                    {
                        case PublicationType.Book:
                            // Create a new Book (ConcreteFlyweight) example.
                            publication = new Book(
                                author: key.Item1,
                                pageCount: 662,
                                publisher: new Publisher("DAW Books"),
                                title: key.Item2
                            );
                            break;
                        case PublicationType.GraphicNovel:
                            // Create a new GraphicNovel (ConcreteFlyweight) example.
                            publication = new GraphicNovel(
                                author: key.Item1,
                                illustrator: new Illustrator(key.Item1.Name),
                                publisher: new Publisher("Drawn & Quarterly"),
                                title: key.Item2
                            );
                            break;
                        default:
                            throw new ArgumentException($"[PublicationType.{key.Item3}] is not configured.  Publication ('{key.Item2}' by {key.Item1.Name}) cannot be created.");
                    }
                    // Output new publication data.
                    Logging.LineSeparator();
                    Logging.Log($"New Publication created:");
                    Logging.Log(publication);
                    // Add new publication to global list.
                    Publications.Add(key, publication);
                }
            }
            catch (ArgumentException exception)
            {
                Logging.Log(exception);
            }
            // Return publication, whether newly-created or existing record.
            return publication;
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

As briefly mentioned in the introduction, the overall purpose of the `flyweight pattern` is to make it easier to reuse objects whenever possible, ideally saving both memory and processor time.  To assist with this goal, a typical `flyweight pattern` implementation consists of the following three components:

- `Flyweight interface`: This interface defines the basic members of all flyweight objects.  
- `ConcreteFlyweight class`: A class that implements the `Flyweight interface`.  Must be shareable.
- `FlyweightFactory class`: Handles all flyweight object sharing.  Can retrieve existing flyweight objects, or create new ones when necessary, usually through a shared collection.

While we won't show an example of it here, another common type of object is the optional `UnsharedFlyweight`:

- (Optional) `UnsharedFlyweight class`: Also implements the `Flyweight interface`, but these do not require sharing.

---

For our example code above we're using these fundamental concepts to build the `Library` class, which behaves as our `FlyweightFactory`.  We add `IPublication` objects, like `Books` and `GraphicNovels`, to our `Library` collection, sharing all these objects in a `Dictionary` collection.

We start by defining a few helper classes specific to our example.  `Author`, `Illustrator`, and `Publisher` are not part of the `flyweight design pattern`, but they're included to flesh out the code and illustrate something closer to a production example.  The same goes for the `PublicationType` enumeration:

```cs
public class Author
{
    public string Name { get; set; }

    public Author(string name)
    {
        Name = name;
    }
}

public class Illustrator
{
    public string Name { get; set; }

    public Illustrator(string name)
    {
        Name = name;
    }
}

public class Publisher
{
    public string Name { get; set; }

    public Publisher(string name)
    {
        Name = name;
    }
}

/// <summary>
/// Defines the allowed publication types.
/// </summary>
public enum PublicationType
{
    Book,
    Epic,
    GraphicNovel
}
```

Next, we get to the first major component of our `flyweight pattern`, the `IPublication` interface, which acts as our `Flyweight interface` and defines a few properties:

```cs
/// <summary>
/// Acts as the Flyweight interface.
/// </summary>
public interface IPublication
{
    Author Author { get; set; }
    Publisher Publisher { get; set; }
    string Title { get; set; }
}
```

We now need a few `ConcreteFlyweight classes` in the mix, so here we define our `Book` and `GraphicNovel` classes, both of which implement the `IPublication` interface:

```cs
/// <summary>
/// Acts as a ConcreteFlyweight class.
/// </summary>
public class Book : IPublication
{
    public Author Author { get; set; }
    public int PageCount { get; set; }
    public Publisher Publisher { get; set; }
    public string Title { get; set; }

    public Book(Author author, Publisher publisher, string title)
    {
        Author = author;
        Publisher = publisher;
        Title = title;
    }

    public Book(Author author, int pageCount, Publisher publisher, string title)
    {
        Author = author;
        PageCount = pageCount;
        Publisher = publisher;
        Title = title;
    }
}

/// <summary>
/// Acts as a ConcreteFlyweight class.
/// </summary>
public class GraphicNovel : IPublication
{
    public Author Author { get; set; }
    public Illustrator Illustrator { get; set; }
    public Publisher Publisher { get; set; }
    public string Title { get; set; }

    public GraphicNovel(Author author, Illustrator illustrator, Publisher publisher, string title)
    {
        Author = author;
        Illustrator = illustrator;
        Publisher = publisher;
        Title = title;
    }
}
```

In this case, `Book` and `GraphicNovel` contain slightly different property signatures, which is a bit more realistic.  Regardless, the point is that we can define as many `ConcreteFlyweight classes` as we need to, and within our `FlyweightFactory class` we'll actually differentiate between them when we need to handle sharing logic.

Speaking of the `FlyweightFactory class`, now we finally declare our own in the form of the `Library` class:

```cs
/// <summary>
/// Houses all publications.
/// Storage uses Dictionary with Tuple key for author, title, and publication type.
/// 
/// Acts as FlyweightFactory.
/// </summary>
public class Library
{
    /// <summary>
    /// Stores all publication data privately.  Should not be publically accessible 
    /// since we want to force access through GetPublication() method.
    /// </summary>
    protected Dictionary<Tuple<Author, string, PublicationType>, IPublication> Publications = 
        new Dictionary<Tuple<Author, string, PublicationType>, IPublication>();

    /// <summary>
    /// Get the count of all publications in library.
    /// </summary>
    public int GetPublicationCount => Publications.Count;

    /// <summary>
    /// Retrieve a Publication by passed key Tuple.
    /// If an item with matching key exists, retrieve from private Publications property.
    /// Otherwise, generate a new instance, add to list, and return result.
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public IPublication GetPublication(Tuple<Author, string, PublicationType> key)
    {
        IPublication publication = null;
        try
        {
            if (Publications.ContainsKey(key))
            {
                publication = Publications[key];
                // ...
            }
            else
            {
                switch (key.Item3)
                {
                    case PublicationType.Book:
                        // Create a new Book (ConcreteFlyweight) example.
                        publication = new Book(
                            author: key.Item1,
                            pageCount: 662,
                            publisher: new Publisher("DAW Books"),
                            title: key.Item2
                        );
                        break;
                    case PublicationType.GraphicNovel:
                        // Create a new GraphicNovel (ConcreteFlyweight) example.
                        publication = new GraphicNovel(
                            author: key.Item1,
                            illustrator: new Illustrator(key.Item1.Name),
                            publisher: new Publisher("Drawn & Quarterly"),
                            title: key.Item2
                        );
                        break;
                    default:
                        throw new ArgumentException($"[PublicationType.{key.Item3}] is not configured.  Publication ('{key.Item2}' by {key.Item1.Name}) cannot be created.");
                }
                // ...
                // Add new publication to global list.
                Publications.Add(key, publication);
            }
        }
        catch (ArgumentException exception)
        {
            Logging.Log(exception);
        }
        // Return publication, whether newly-created or existing record.
        return publication;
    }
}
```

While this may appear a little complicated, we'll break down the fundamental components of the `Library` class and you'll see that there's really not a lot going on at all.

A fundamental aspect of the `flyweight pattern` is the ability to share and reuse objects, so our factory needs a collection, or some other means of tracking all flyweight objects.  Therefore, we begin with the `Publications` property:

```cs
protected Dictionary<Tuple<Author, string, PublicationType>, IPublication> Publications = 
    new Dictionary<Tuple<Author, string, PublicationType>, IPublication>();
```

We're using a `Dictionary` here to store our collection, which makes it easy to use a complex `key` value (a three-part tuple, in this case) that can be associated with each `IPublication` object `value`.

The `GetPublication()` method is where most of the magic happens.  In this example we're using this method to perform all sharing, reuse, and creation logic, but obviously we could split this logic up if necessary.  Again, we need a way to uniquely identify our `IPublication` (flyweight) objects, so we use the same three-value tuple as the key, then immediately create a new `IPublication` instance variable.  We'll use this variable throughout the method logic to hold either the _new_ or _existing_ object.

Next, when implementing reusability we won't want to create objects that already exist, so we first check if the `Publications` collection property contains the `key` parameter.  If the `key` exists in the collection we simply assign the existing object to the local `publication` value:

```cs
if (Publications.ContainsKey(key))
{
    publication = Publications[key];
    // ...
}
```

On the other hand, if `key` _doesn't_ exist in the collection we probably need to create a new object instance and add it to the collection.  For this example we're using the third value of our tuple key to store the `PublicationType` of the object.  Therefore, we perform a `switch()` using that third item of the tuple, and try to find a match of either `PublicationType.Book` or `PublicationType.GraphicNovel`, both of which our method can handle:

```cs
else
{
    switch (key.Item3)
    {
        case PublicationType.Book:
            // Create a new Book (ConcreteFlyweight) example.
            publication = new Book(
                author: key.Item1,
                pageCount: 662,
                publisher: new Publisher("DAW Books"),
                title: key.Item2
            );
            break;
        case PublicationType.GraphicNovel:
            // Create a new GraphicNovel (ConcreteFlyweight) example.
            publication = new GraphicNovel(
                author: key.Item1,
                illustrator: new Illustrator(key.Item1.Name),
                publisher: new Publisher("Drawn & Quarterly"),
                title: key.Item2
            );
            break;
        default:
            throw new ArgumentException($"[PublicationType.{key.Item3}] is not configured.  Publication ('{key.Item2}' by {key.Item1.Name}) cannot be created.");
    }
    // ...
    // Add new publication to global list.
    Publications.Add(key, publication);
}
```

If we're dealing with a valid `PublicationType` our code creates a new instance of the respective object.  _Note:_ In this example only some of the arguments passed to `new Book()` and `new GraphicNovel()` are dynamic (e.g. obtained from the `key` tuple), while the remaining arguments are hard-coded.  Obviously, this is a poor practice in a real-world application, but creating a five- or six-part tuple is a bit of a hassle, so I decided to leave it as is for now.

Lastly, since the local `publication` variable within this `else` block scope was assigned to the newly-generated `IPublication` instance, we need to `Add()` it to the shared `Publication` collection.

Now that we're all set up let's try actually using our `Library` flyweight configuration.  We always begin by creating a new `FlyweightFactory class` instance (`Library`, in this case), and then use the `GetPublication()` method to create (or retrieve) object instances.  In our first example here, we start by creating a new book and a new graphic novel, then we attempt to `GetPublication()` using the same key tuple values we passed in the first call:

```cs
public static void Example1()
{
    var library = new Library();
    var book = library.GetPublication(
        Tuple.Create(
            new Author("Patrick Rothfuss"),
            "The Name of the Wind",
            PublicationType.Book
        )
    );

    var graphicNovel = library.GetPublication(
        Tuple.Create(
            new Author("Julie Doucet"),
            "My New York Diary",
            PublicationType.GraphicNovel
        )
    );

    // Try retrieving Publication with same key.
    book = library.GetPublication(
        Tuple.Create(
            new Author("Patrick Rothfuss"),
            "The Name of the Wind",
            PublicationType.Book
        )
    );

    Logging.Log($"Library contains [{library.GetPublicationCount}] publications.");
}
```

We end the example with an output from `library.GetPublicationCount`, which simply retrieves the quantity of `IPublication` objects stored in the library.  Since our second `Book` retrieval attempt uses the same values as the first, if our code is working correctly we'd expect to see only `2` publications in the collection, since the second `Book` call should be a retrieval of an existing record.  However, our output actually shows that the second `Book` call _also_ created a new instance, thereby giving us `3` objects in the collection:

```
--------------------
New Publication created:
{Flyweight.Book(HashCode:30015890)}
  Author: { }
    {Flyweight.Author(HashCode:1707556)}
      Name: "Patrick Rothfuss"
  PageCount: 662
  Publisher: { }
    {Flyweight.Publisher(HashCode:15368010)}
      Name: "DAW Books"
  Title: "The Name of the Wind"

--------------------
New Publication created:
{Flyweight.GraphicNovel(HashCode:36849274)}
  Author: { }
    {Flyweight.Author(HashCode:4094363)}
      Name: "Julie Doucet"
  Illustrator: { }
    {Flyweight.Illustrator(HashCode:63208015)}
      Name: "Julie Doucet"
  Publisher: { }
    {Flyweight.Publisher(HashCode:32001227)}
      Name: "Drawn & Quarterly"
  Title: "My New York Diary"

--------------------
New Publication created:
{Flyweight.Book(HashCode:41962596)}
  Author: { }
    {Flyweight.Author(HashCode:19575591)}
      Name: "Patrick Rothfuss"
  PageCount: 662
  Publisher: { }
    {Flyweight.Publisher(HashCode:42119052)}
      Name: "DAW Books"
  Title: "The Name of the Wind"

Library contains [3] publications.
```

Keen observers will probably already notice the problem: The `Author` value we're passing into our tuple creation is always a `new Author()` instance in this example.  Even though the string `Name` property value of the `Author` is the same in both cases, the underlying `Author` object is different, and therefore, the generated key that is used for comparison within the `Library.GetPublication()` method differs.

The solution is to explicitly pass the same instance of `Author` to both our `Book` retrieval attempts, which we do here in `Example2()`:

```cs
public static void Example2()
{
    // Create library.
    var library = new Library();

    // Create Author instances.
    var patrickRothfuss = new Author("Patrick Rothfuss");
    var julieDoucet = new Author("Julie Doucet");

    // Create or retrieve new book.
    var book = library.GetPublication(
        Tuple.Create(
            patrickRothfuss,
            "The Name of the Wind",
            PublicationType.Book
        )
    );

    var graphicNovel = library.GetPublication(
        Tuple.Create(
            julieDoucet,
            "My New York Diary",
            PublicationType.GraphicNovel
        )
    );

    // Try retrieving Publication with same key.
    book = library.GetPublication(
        Tuple.Create(
            patrickRothfuss,
            "The Name of the Wind",
            PublicationType.Book
        )
    );

    Logging.Log($"Library contains [{library.GetPublicationCount}] publications.");
}
```

Executing the above code now gives us the output we'd expect: The first call to `Book` and `GraphicNovel` both create new object instances, while the second identical `Book` call performs a _retrieval_ of the existing book object, resulting in only `2` total publications in the collection:

```
New Publication created:
{Flyweight.Book(HashCode:43527150)}
  Author: { }
    {Flyweight.Author(HashCode:56200037)}
      Name: "Patrick Rothfuss"
  PageCount: 662
  Publisher: { }
    {Flyweight.Publisher(HashCode:36038289)}
      Name: "DAW Books"
  Title: "The Name of the Wind"

--------------------
New Publication created:
{Flyweight.GraphicNovel(HashCode:33420276)}
  Author: { }
    {Flyweight.Author(HashCode:55909147)}
      Name: "Julie Doucet"
  Illustrator: { }
    {Flyweight.Illustrator(HashCode:32347029)}
      Name: "Julie Doucet"
  Publisher: { }
    {Flyweight.Publisher(HashCode:22687807)}
      Name: "Drawn & Quarterly"
  Title: "My New York Diary"

--------------------
Existing Publication located:
{Flyweight.Book(HashCode:43527150)}
  Author: { }
    {Flyweight.Author(HashCode:56200037)}
      Name: "Patrick Rothfuss"
  PageCount: 662
  Publisher: { }
    {Flyweight.Publisher(HashCode:36038289)}
      Name: "DAW Books"
  Title: "The Name of the Wind"

Library contains [2] publications.
```

Just for fun we've also included a final example illustrating what might happen if we pass a `key` to the retrieval method that doesn't exist in the collection, but _also_ cannot be used to create a new object instance either.  To accomplish this we've added a third `PublicationType` enumeration, but the `Library.GetPublication()` method is only built to handle `Book` or `GraphicNovel`, throwing an `ArgumentException` if another type is used:

```cs
public static void Example3()
{
    var library = new Library();

    // Try to retrieve a Publication with an invalid PublicationType.
    library.GetPublication(
        Tuple.Create(
            new Author("Dante"), 
            "Divine Comedy", 
            PublicationType.Epic
        )
    );
}
```

Sure enough, executing the above example method throws an exception our way, as expected:

```
[EXPECTED] System.ArgumentException: [PublicationType.Epic] is not configured.  Publication ('Divine Comedy' by Dante) cannot be created.
```

---

This is just a small taste of what can be accomplished with the `flyweight design pattern`, but hopefully it helped to illustrate the agility and potential for resource savings that this pattern can provide.  Check out more design patterns in our [ongoing series over here](https://airbrake.io/blog/software-design/software-design-patterns-guide)!

---

__META DESCRIPTION__

Part 12 of our Software Design Pattern series in which examine the flyweight design pattern using fully-functional C# example code.