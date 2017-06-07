# .NET Exceptions - System.ArgumentNullException

Making our way through our [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, today we'll take a closer look at the `System.ArgumentNullException`.  Similar to the `System.ArgumentException` that we covered in [another article](https://airbrake.io/blog/dotnet/net-exceptions-system-argumentexception) the `System.ArgumentNullException` is the result of passing an invalid argument to a method -- in this case, passing a `null` object when the method requires a non-null value.

Similar to other argument exception types the `System.ArgumentNullException` isn't typically raised by the .NET Framework library itself or the `CLR`, but is usually thrown by the library or application as an indication of improper `null` arguments.

In this article we'll explore the `System.ArgumentNullException` in more detail including where it resides in the .NET exception hierarchy.  We'll also take a look at some functional sample C# code to illustrate how `System.ArgumentNullException` should typically be thrown in your own projects, so let's get going!

## The Technical Rundown

- All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.
- [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception) is inherited from the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) class.
- [`System.ArgumentException`](https://docs.microsoft.com/en-us/dotnet/api/system.argumentexception?view=netframework-4.7) inherits directly from [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception).
- Finally, `System.ArgumentNullException` inherits directly from [`System.ArgumentException`](https://docs.microsoft.com/en-us/dotnet/api/system.argumentexception?view=netframework-4.7).

## When Should You Use It?

As mentioned in the introduction the occurrence of a `System.ArgumentNullException` typically means the developer of the module or library you're using wanted to ensure that a non-null object was passed to the method in question that caused the exception.  Similarly, when writing your own code it's considered a best practice to always validate the passed arguments of your methods to ensure no passed values are `null` and, therefore, might break your code or lead to unintended consequences.  For more information on the code quality rules that apply see [CA1062: Validate arguments of public methods](https://docs.microsoft.com/en-us/visualstudio/code-quality/ca1062-validate-arguments-of-public-methods).

In practice this means that `throwing` `System.ArgumentNullExceptions` should be performed within virtually every method you write that should not accept a `null` argument.  To illustrate we have a simple example of our custom `Book` class with a few properties of `Author` and `Title`:

```cs
public class Book
{
    private string _author;
    private string _title;

    public string Author
    {
        get
        {
            return _author;
        }
        set
        {
            // Check if value is null.
            if (value is null)
                // Throw a new ArgumentNullException with "Author" parameter name.
                throw new System.ArgumentNullException("Author");
            _author = value;
        }
    }

    public string Title
    {
        get
        {
            return _title;
        }
        set
        {
            // Check if value is null.
            if (value is null)
                // Throw a new ArgumentNullException with "Title" parameter name.
                throw new System.ArgumentNullException("Title");
            _title = value;
        }
    }

    public Book(string title, string author)
    {
        Author = author;
        Title = title;
    }
}
```

We've opted to create the `private` `_author` and `_title` fields and then use them for our `public` `properties` of `Author` and `Title`, respectively.  This allows us to create a custom `Author.set()` and `Title.set()` methods in which we check if the passed `value` is `null`.  In such cases we `throw` a new `System.ArgumentNullException` and, rather than passing in a full error message as is often the case, `System.ArgumentNullException` expects just the name of the parameter that cannot be `null`.

To illustrate this behavior we have two basic example methods:

```cs
private static void ValidExample()
{
    try
    {
        // Instantiate book with valid Title and Author arguments.
        var book = new Book("The Stand", "Stephen King");
        // Output book results.
        Logging.Log(book);
    }
    catch (System.ArgumentNullException e)
    {
        Logging.Log(e);
    }
}

private static void InvalidExample()
{
    try
    {
        // Instantiate book with valid Title but invalid (null) Author argument.
        var book = new Book("The Stand", null);
        // Output book results.
        Logging.Log(book);
    }
    catch (System.ArgumentNullException e)
    {
        Logging.Log(e);
    }
}
```

As you can see the `ValidExample()` method creates a new `Book` instance where both the `Title` and `Author` parameters are provided so no exceptions are thrown and the output shows us our `book` object as expected:

```cs
{Airbrake.ArgumentNullException.Book}
  Author: "Stephen King"
  Title: "The Stand"
```

On the other hand our `InvalidExample()` method passed `null` as the second `Author` parameter, which is caught by our `null` check and throws a new `System.ArgumentNullException` our way:

```cs
[EXPECTED] System.ArgumentNullException: Value cannot be null.
Parameter name: Author
```

Using copy constructors is another area to be careful of and to potentially throw `System.ArgumentNullExceptions` within.  For example, here we've modified our `Book` class slightly to include a copy constructor with a single `Book` parameter that copies the `Author` and `Title` properties.  In addition, to illustrate the difference between regular instances and copies, we've also added the `IsCopy` property and set it to `true` within the copy constructor method _only_:

```cs
public class Book
{
    // ...

    public bool IsCopy { get; set; }

    public Book(string title, string author)
    {
        Author = author;
        Title = title;
    }

    // Improper since passed Book parameter could be null.
    public Book(Book book)
        : this(book.Title, book.Author)
    {
        // Specify that this is a copy.
        IsCopy = true;
    }
}
```

This presents a potential problem which we'll illustrate using two more example methods:

```cs
private static void ValidCopyExample()
{
    try
    {
        // Instantiate book with valid Title and Author arguments.
        var book = new Book("The Stand", "Stephen King");
        var copy = new Book(book);
        // Output copy results.
        Logging.Log(copy);
    }
    catch (System.ArgumentNullException e)
    {
        Logging.Log(e);
    }
}

private static void InvalidCopyExample()
{
    try
    {
        // Instantiate book with valid Title and Author arguments.
        var copy = new Book(null);
        // Output copy results.
        Logging.Log(copy);
    }
    catch (System.ArgumentNullException e)
    {
        Logging.Log(e);
    }
}
```

`ValidCopyExample()` works just fine because we first create a base `Book` instance then copy that using our copy constructor to create the `copy` instance, which we then output to our log.  The result is a `Book` instance with the `IsCopy` property equal to `true`:

```cs
{Airbrake.ArgumentNullException.Book}
  IsCopy: True
  Author: "Stephen King"
  Title: "The Stand"
```

However, we run into trouble in the `InvalidCopyExample()` method when trying to pass a `null` object to our copy constructor (remember that C# _knows_ we're using that copy constructor since we've only passed one argument and the other constructor [`Book(string title, string author)`] requires two arguments).  This actually throws a `System.NullReferenceException` when we hit the `: this(book.Title, book.Author)` line since our code cannot reference properties of the `null` `book` object that we passed.

The solution is to use an intermediary `null` checking method on our passed instance _before_ we actually attempt to set the properties.  Here we've modified our copy constructor to use the newly added `NullValidator()` method:

```cs
// Validates for non-null book parameter before copying.
public Book(Book book)
    // Use method-chaining to call the Title and Author properties
    // on the passed-through book instance, if valid.
    : this(NullValidator(book).Title, 
           NullValidator(book).Author)
{
    // Specify that this is a copy.
    IsCopy = true;
}

// Validate for non-null copy construction.
private static Book NullValidator(Book book)
{
    if (book is null)
        throw new System.ArgumentNullException("book");
    // If book isn't null then return.
    return book;
}
```

With these changes we can now invoke our `InvalidCopyExample()` method again and produce the `System.ArgumentNullException` that we expect because our passed `book` argument is still `null`:

```cs
[EXPECTED] System.ArgumentNullException: Value cannot be null.
Parameter name: book
```

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A close look at the System.ArgumentNullException in .NET, including some basic C# code examples and illustration of copy constructor best practices.