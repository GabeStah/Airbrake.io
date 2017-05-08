# .NET Exceptions - System.NotImplementedException

Making our way through the [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, today we'll dive into the depths of the `System.NotImplementedException`.  Similar to `System.ArgumentException` and a handful of other exceptions of this type, the `System.NotImplementedException` is not an error that is _accidentally_ thrown.  Instead, a `System.NotImplementedException` is used when calling a method or accessor which _exists_, but has not yet been _implemented_.  In large part, this is used to differentiate between methods that are fully implemented for production code and those that are still in development.

To explore a bit further we'll take some time in this article to swim through all the nooks and crannies of `System.NotImplementedException`, including where it resides in the .NET exception hierarchy.  We'll also take a brief look at the related errors of `System.NotSupportedException` and `System.PlatformNotSupportedException`, including some code examples of each, so let's dive in!

## The Technical Rundown

- All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.
- [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception) is inherited from the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) class.
- `System.NotImplementedException` inherits directly from [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception).

## When Should You Use It?

As mentioned in the introduction, a `System.NotImplementedException` is not something you'll run into often, and when you do, it's because the developer of the method that you're calling has explicitly decided to throw an exception to indicate that the method is still under development.  For this reason, it is recommended that if you encounter a `System.NotImplementedException` when using a third-party library or module you shouldn't attempt to handle the error with a typical `try/catch` block.  Instead, you should (temporarily) remove the code that invokes the non-implemented method.  It's a far safer practice, since it will keep your code base stable until a later date when the method is actually implemented, at which point you can choose to integrate it back into the application, if desired.

Throwing a `System.NotImplementedException` is fairly straightforward.  As mentioned, it should be thrown inside any method or property that must actually _exist_, but otherwise has no functional purpose.  For example, if you're using test-driven development (`TDD`) practices, particularly when implementing larger features, it may be helpful to create methods and properties so they can be invoked _prior_ to their actual functional implementation.  This allows you to create an (initially failing) test, then create a series of "empty" methods and properties that are just placeholders that throw `System.NotImplementedException` when invoked.  When executing your tests, these methods will clearly cause the tests to fail due to the thrown exception, but now you can then make your way through those skeletal methods until everything is working as expected and you no longer have any `System.NotImplementedExceptions` being thrown.

Implementing a `System.NotImplementedException` in code is quite simple.  Here we have a basic `IBook` interface that is used by our `Book` class.  This class allows us to create `Book` instances that are empty, or which are assigned properties by passing the `title` and `author` parameters:

```cs
public interface IBook
{
    string Author { get; set; }
    string Title { get; set; }
}

public class Book : IBook
{
    public string Title { get; set; }
    public string Author { get; set; }

    public Book() { }

    public Book(string title, string author)
    {
        Title = title;
        Author = author;
    }

    public DateTime PublicationDate()
    {
        // Get namespace and class type.
        var type = this.GetType().FullName;
        // Get current method name.
        var methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
        // Throw NotImplementedException.
        throw new System.NotImplementedException($"{type}.{methodName} is not yet implemented.");
    }
}
```

Beyond that, we don't currently have any properties or parameters related to publication, but we know that eventually we'll want a way to find out the publication date of our book.  Therefore, rather than not including anything right now, we've decided to add a `PublicationDate()` method that throws a `System.NotImplementedException`, indicating to the user that this method is not implemented.  We also use a few tricks with .NET reflection to get the `type` (namespace and class) and the method name automatically, so we can output that information within the `System.NotImplementedException` error message.

Using our `Book` class is just like any other class.  Here we create a new instance for _The Stand_ by Stephen King, then output the contents of our `book` instance using our `Logging.Log` method (which uses quite a bit of reflection capabilities itself to spit out a human-readable representation of our passed object):

```cs
class Program
{
    static void Main(string[] args)
    {
        Example();
    }

    private static void Example()
    {
        try
        {
            var book = new Book("The Stand", "Stephen King");
            Logging.Log(book);
            Logging.Log(book.PublicationDate());
        }
        catch (System.NotImplementedException exception)
        {
            Logging.Log(exception);
        }
    }
}
```

The resulting output shows our `book` instance was created and outputs the contents of it, but then our call to `book.PublicationDate()` throws a `System.NotImplementedException` just as we intended:

```
{Airbrake.NotImplementedException.Book}
  Title: "The Stand"
  Author: "Stephen King"

[EXPECTED] System.NotImplementedException: Airbrake.NotImplementedException.Book.PublicationDate is not yet implemented.
```

As we can see, using `System.NotImplementedException` is quite simple.  However, there are a few additional .NET exceptions that are related to `System.NotImplementedException` and used in similar yet slightly different situations: `System.PlatformNotSupportedException` and `System.NotSupportedException`.  We'll briefly cover the use of these related exceptions, as recommended by the [official documentation](https://docs.microsoft.com/en-us/dotnet/api/system.notimplementedexception?view=netframework-4.7).

### System.PlatformNotSupportedException

A `System.PlatformNotSupportedException` should be thrown when the method in question is technically _implemented_, but is not intended to be used on the particular platform the code is being run on.  For example, let's add the `PageCount()` method to our `Book` class.  However, we want to ensure that `PageCount()` cannot be used on `Windows 7` platforms (for whatever reason).

```cs
static class Platforms
{
    public const string Windows7    = "Windows NT 6.1";
    public const string Windows8    = "Windows NT 6.2";
    public const string Windows8_1  = "Windows NT 6.3";
    public const string Windows10   = "Windows NT 10";
}

public class Book : IBook
{

    // ...

    public int PageCount()
    {
        // Get current platform.
        var platform = Environment.OSVersion;

        // Check if platform is unsupported.
        if (platform.ToString().Contains(Platforms.Windows7))
        {
            // Get namespace and class type.
            var type = this.GetType().FullName;
            // Get current method name.
            var methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            // Throw PlatformNotSupportedException.
            throw new System.PlatformNotSupportedException($"{type}.{methodName} does not support the current platform: {platform} ({nameof(Platforms.Windows7.ToString()}).");
        }
        return 0;
    }
}
```

To help us determine if the platform is supported or not we have a new static class called `Platforms` that just contains a list of substrings which represent the underlying operating system value for the related string.  Within the `PageCount()` method we use `Environment.OSVersion` to get the current full platform string, which looks something like this: `Microsoft Windows NT 6.1.7601 Service Pack 1`.  While this example isn't robust, we can check whether our current full platform string contains the string of any OS we don't support.  In this case, we don't support `Windows 7`, so if our current platform string contains that matching substring, we throw a `System.PlatformNotSupportedException`.

Here we're making use of this new `PageCount()` method to see it in action:

```cs
private static void PlatformExample()
{
    try
    {
        var book = new Book("Moby Dick", "Herman Melville");
        Logging.Log(book);
        Logging.Log(book.PageCount());
    }
    catch (System.PlatformNotSupportedException exception)
    {
        Logging.Log(exception);
    }
}
```

The output shows that, sure enough, when running on Windows 7 a `System.PlatformNotSupportedException` is thrown:

```
{Airbrake.NotImplementedException.Book}
  Title: "Moby Dick"
  Author: "Herman Melville"

[EXPECTED] System.PlatformNotSupportedException: Airbrake.NotImplementedException.Book.PageCount does not support the current platform: Microsoft Windows NT 6.1.7601 Service Pack 1 (Windows7).
```

### System.Not​Supported​Exception

It is also recommended that a `System.Not​Supported​Exception` be thrown when a method must be implemented in your code, but supporting that method doesn't make any sense in the current context.  This might occur when you have an `abstract` class that is intended to be overriden by subclasses.  In some situations, not all of the methods attached to the abstract parent class are applicable to every possible subclass that may inherit from it.

As a simple example, here we have a new abstract `Publisher` class with two properties: `Name` and `Revenue`.  Most of us would probably agree that all publishers typically have a name of some sort, so that property makes sense.  However, we might imagine a _type_ of publisher -- such as a blogger using their laptop to make posts about cute cat -- which doesn't have any revenue to speak of, nor any need to track it.  So while a big name publisher like "Simon & Schuster" would track their revenue, your aunt making posts to her own blog would not.

For that reason, our `Blog` class that inherits from `Publisher` must override both `Name` and `Revenue` properties.  However, we don't have a use for the `Revenue` property in this context of a simple blog, so we're throwing a `System.Not​Supported​Exception` in both the getter and setter of the `Revenue` property:

```cs
public abstract class Publisher
{
    public abstract string Name { get; set; }
    public abstract decimal Revenue { get; set; }
}

public class Blog : Publisher
{
    public override string Name { get; set; }

    public Blog(string name)
    {
        Name = name;
    }

    public override decimal Revenue
    {
        get
        {
            // Get namespace and class type.
            var type = this.GetType().FullName;
            // Get current method name.
            var methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            // Throw NotSupportedException.
            throw new System.NotSupportedException($"{type} does not support the {methodName} method.");
        }
        set
        {
            // Get namespace and class type.
            var type = this.GetType().FullName;
            // Get current method name.
            var methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            // Throw NotSupportedException.
            throw new System.NotSupportedException($"{type} does not support the {methodName} method.");
        }
    }
}
```

Let's try creating a new `Blog` instance and then calling the `Revenue` property:

```cs
private static void PublisherExample()
{
    try
    {
        var blog = new Blog("My Cool Cat Blog");
        Logging.Log(blog.Revenue);
    }
    catch (System.NotSupportedException exception)
    {
        Logging.Log(exception);
    }
}
```

Sure enough a `System.Not​Supported​Exception` is thrown because we've decided not to support the `Revenue` property in this particular subclass:

```
[EXPECTED] System.NotSupportedException: Airbrake.NotImplementedException.Blog does not support the get_Revenue method.
```

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A deep dive into the System.NotImplementedException in .NET, including C# code examples and an examination of NotSupportedException and PlatformNotSupportedException as well.