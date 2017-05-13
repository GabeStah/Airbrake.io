# .NET Exceptions - System.InvalidCastException

Moving along through our [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, today we'll dig into the `System.InvalidCastException`.  Put simply, a `System.InvalidCastException` is thrown when trying to perform some type of conversion an object to an invalid type.

In this article we'll examine everything about the `System.InvalidCastException`, including where it sits within the .NET exception hierarchy and by giving a few code examples to illustrate how this exception might come about.  Let's get going!

## The Technical Rundown

- All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.
- [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception) is inherited from the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) class.
- `System.InvalidCastException` inherits directly from [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception).

## When Should You Use It?

There are a number of possible ways to throw a `System.InvalidCastException`, so let's just jump right into some example code to see exactly what this error means and how to deal with it.

Probably the most commonly used technique that could result in a `System.InvalidCastException` is performing a `cast` to convert one type to another type.  In statically-typed languages like `C#`, after a variable is declared, it typically cannot be declared again or used to store data that is incompatible with that particular type.  By using a `cast`, we're able to tell the common language runtime (`CLR`) that we want to convert from one type to another while acknowledging that there could be data loss in the process.

The `cast` syntax is typically to precede the variable name with the type to be `cast` into, which should be within bounded parentheses.  For example, here we want to `cast` an integer to a decimal:

```cs
int value = 10;
decimal dec = (decimal)value;
Logging.Log($"Cast succeeded from {value.GetType().Name} to {dec.GetType().Name}.");
// Cast succeeded from Int32 to Decimal.
```

This works without a problem because .NET understands how to properly convert from an integer (whole number) to a decimal representation of that same number.

However, trouble can occur when trying to perform what is known as `downcasting`: Trying to convert an instance of a base (parent) type to one of its derived (child) types.  For example, here we have a simple `Book` class with an `Author` and `Title` property.  We then inherit the `Book` class to create the `PublishedBook` class, which adds the `PublishedAt` property:

```cs
public class Book
{
    public string Title { get; set; }
    public string Author { get; set; }

    public Book(string title, string author)
    {
        Title = title;
        Author = author;
    }
}

public class PublishedBook : Book
{
    public DateTime PublishedAt { get; set; }

    public PublishedBook(string title, string author, DateTime published_at)
        : base(title, author)
    {
        Title = title;
        Author = author;
        PublishedAt = published_at;
    }
}
```

Let's try to `cast` an instance of of `Book` to the derived type of `PublishedBook`:

```cs
private static void CastToDerivedType()
{
    try
    {
        var book = new Book("Sword in the Darkness", "Stephen King");

        // Downcasting PublishedBook to base type of Book.
        var castBook = (PublishedBook)book;
        Logging.Log("Downcasting successful:");
        Logging.Log(castBook);
    }
    catch (System.InvalidCastException exception)
    {
        Logging.Log("Downcasting failed.");
        Logging.Log(exception);
    }
}
```

Sure enough, this fails and throws a `System.InvalidCastException`:

```
Downcasting failed.
[EXPECTED] System.InvalidCastException: Unable to cast object of type 'Airbrake.InvalidCastException.Book' to type 'Airbrake.InvalidCastException.PublishedBook'.
```

While `downcasting` like this doesn't work, let's try `upcasting` instead, in which we convert from a derived type "up" to a base type:

```cs
private static void CastToBaseType()
{
    try
    {
        var publishedBook = new PublishedBook("It", "Stephen King", new DateTime(1986, 9, 1));

        // Upcasting PublishedBook to base type of Book.
        var castBook = (Book)publishedBook;
        Logging.Log("Upcasting successful.");
        Logging.Log(castBook);
    }
    catch (System.InvalidCastException exception)
    {
        Logging.Log("Upcasting failed.");
        Logging.Log(exception);
    }
}
```

The output shows us that `upcasting` works just fine, since the `CLR` knows how to "reduce" a more complex object and convert to its simpler, base type.

```
Upcasting successful.
{Airbrake.InvalidCastException.PublishedBook}
  PublishedAt: 9/1/1986
  Title: "It"
  Author: "Stephen King"
```

Another common action that could throw a `System.InvalidCastException` is when trying to use the `cast` operator syntax to convert to a string value.  Here we're attempting to convert our `age` object to a string value via `casting`:

```cs
private static void CastToString()
{
    try
    {
        object age = 30;
        // Attempt cast to string.
        string convertedAge = (string)age;
        Logging.Log($"Converted age is: {convertedAge}.");
    }
    catch (System.InvalidCastException exception)
    {
        Logging.Log("String cast failed.");
        Logging.Log(exception);
    }
}
```

Unfortunately, .NET isn't pleased with this and fails while also throwing a `System.InvalidCastException`:

```
String cast failed.
[EXPECTED] System.InvalidCastException: Unable to cast object of type 'System.Int32' to type 'System.String'.
```

The recommended technique to use here is to call the `ToString()` method on the object in question.  Since `ToString()` is defined by the `Object` class (and thus is inherited by all derived objects therein), it's always available and will work:

```cs
private static void CallToStringMethod()
{
    try
    {
        object age = 30;
        // Convert via the ToString() method.
        string convertedAge = age.ToString();
        Logging.Log($"Converted age is: {convertedAge}.");
    }
    catch (System.InvalidCastException exception)
    {
        Logging.Log("ToString() method conversion failed.");
        Logging.Log(exception);
    }
}
```

This works just fine and outputs our converted age:

```
Converted age is: 30.
```

While there are a few others ways to incur a `sysex`, the last technique we'll cover here is when trying to call a primitive type's [`IConvertible`](https://docs.microsoft.com/en-us/dotnet/api/system.iconvertible?view=netframework-4.7) implementation to perform a conversion to a type that isn't supported.  The `IConvertible` interface can be used to provide a wide range of conversion methods, allowing the implementing type to convert to common language runtime types such as `Boolean`, `String`, `Int32`, and so forth.

To see `IConvertible` in practice, here we have a simple example where we're trying to convert from a `bool` to a `char` type using `IConvertible's` `ToChar()` method:

```cs
private static void ConvertBoolToChar()
{
    try
    {
        bool value = true;
        // Create an IConvertible interface.
        IConvertible converter = value;
        // Convert using ToChar() method.
        Char character = converter.ToChar(null);
        Logging.Log($"Conversion from Bool to Char succeeded.");
    }
    catch (System.InvalidCastException exception)
    {
        Logging.Log("Conversion failed.");
        Logging.Log(exception);
    }
}
```

Unfortunately, there's no way to convert from a `bool` to `char` so a `System.InvalidCastException` is thrown:

```
Conversion failed.
[EXPECTED] System.InvalidCastException: Invalid cast from 'Boolean' to 'Char'.
```

The only solution in cases like this is to ensure the types you're working with are compatible with one another, meaning that the `CLR` knows how to convert from one to the other.  Here we're trying to convert from an `int` to to `Char`, which should work just fine:

```cs
private static void ConvertIntToChar()
{
    try
    {
        int value = 25;
        // Create an IConvertible interface.
        IConvertible converter = value;
        // Convert using ToChar() method.
        Char character = converter.ToChar(null);
        Logging.Log($"Conversion from int to Char succeeded.");
    }
    catch (System.InvalidCastException exception)
    {
        Logging.Log("Conversion failed.");
        Logging.Log(exception);
    }
}
```

Sure enough, the `CLR` knows how to convert from an intger to a character, so our conversion is successful:

```
Conversion from Int to Char succeeded.
```

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A close look at the System.InvalidCastException in .NET, including C# code examples covering upcasting/downcasting and string conversions.