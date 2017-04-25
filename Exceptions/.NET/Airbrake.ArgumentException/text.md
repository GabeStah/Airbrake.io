# .NET Exceptions - System.ArgumentException

Continuing through our [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, today we'll take a little gander at the `System.ArgumentException`.  As the name implies, the `System.ArgumentException` is commonly thrown when there is an issue with a provided argument.  As with a few other exceptions of its type, the `System.ArgumentException` is not thrown during normal execution by internal .NET classes or API calls but, instead, is commonly used by developers as an _indication_ of an improper argument attempt.

Throughout this article we'll further explore the `System.ArgumentException`, including where it sits in the .NET exception hierarchy, along with a bit of C# example code to see how we might go about using `System.ArgumentExceptions` in our own projects.  Let's get to it!

## The Technical Rundown

- All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.
- [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception) is inherited from the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) class.
- `System.ArgumentException` inherits directly from [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception).

## When Should You Use It?

As described in the introduction, the `System.ArgumentException` is among a handful of .NET exceptions that were added into the .NET API, but are not typically triggered during the course of normal execution.  That is to say, if you encounter a `System.ArgumentException` during the execution of an application, the act of `throwing` that particular error was put there intentionally by a developer, as a means of alerting you to a problem with your arguments, as opposed to some sort of system-level issue.

In most cases, the common language runtime (`CLR`), or the particular class library you're using, will have chosen to `throw` a `System.ArgumentException`, based on the arguments you're providing.  For example, here we've created a simple `GetFullName()` method, which accepts two `string` parameters named `first` and `last`.  As the name implies, we're attempting to combine both of them and return the result:

```cs
private static string GetFullName(string first, string last)
{
    // Check if first value is null or has no characters.
    // Null must be checked prior to length to avoid checking an invalid object.
    if (first is null || first.Length == 0)
    {
        throw new System.ArgumentException($"GetFullName() parameter 'first' must be a valid string.");
    }
    // Check if last value is null or has no characters.
    // Null must be checked prior to length to avoid checking an invalid object.
    if (last is null || last.Length == 0)
    {
        throw new System.ArgumentException($"GetFullName() parameter 'last' must be a valid string.");
    }
    // Return full name.
    return $"{first} {last}";
}
```

Now, the first option that comes to mind when attempting to pass invalid arguments to a method may be to simply pass nothing where a required parameter belongs:

```cs
GetFullName("John")
```

However, most _code editors_, or at least the `CLR` itself, will catch this issue during build and not allow the application to be executed, since it recognizes a required parameter is missing.  Here is the particular build error reported to me after a failed build attempt:

```
Error	CS7036	There is no argument given that corresponds to the required formal parameter 'last' of 'Program.GetFullName(string, string)'
```

The other way we might provide invalid arguments to a method is to pass all the correct `types` where the method expects them, but using invalid values.  This is where our `if` blocks within the `GetFullName()` method come in:

```cs
// Check if first value is null or has no characters.
// Null must be checked prior to length to avoid checking an invalid object.
if (first is null || first.Length == 0)
{
    throw new System.ArgumentException($"GetFullName() parameter 'first' must be a valid string.");
}
// Check if last value is null or has no characters.
// Null must be checked prior to length to avoid checking an invalid object.
if (last is null || last.Length == 0)
{
    throw new System.ArgumentException($"GetFullName() parameter 'last' must be a valid string.");
}
```

Here we've decided that neither parameter can be `null`, nor can their `Length` be zero.  This is a basic form of validation, but it gets the job done for our purposes, and ensures that an empty `String` or `null` value are not acceptable.  If either parameter matches those criteria, we throw a `new System.ArgumentException` to inform the user of the issue.

With `GetFullName()` ready to go, we have a couple methods to test it out with.  First is `ValidExample()`, which executes our code within a `try-catch` block and attempts to combine the names `John` and `Doe` then output the result:

```cs
private static void ValidExample()
{
    try
    {
        // Get full name with two valid strings.
        var fullName = GetFullName("John", "Doe");
        // Output name result.
        Logging.Log($"Full name is: {fullName}.");
    }
    catch (System.ArgumentException e)
    {
        Logging.Log(e);
    }
}
```

As expected, this works just fine and the output shows our combined name:

```
Full name is: John Doe.
```

On the other hand, our `InvalidExample()` method passes a `null` value to the second parameter of `GetFullName()`, which should throw a `System.ArgumentException` like we've specified:

```cs
private static void InvalidExample()
{
    try
    {
        // Get full name with a valid first name, but an invalid last name.
        // Expectation is a thrown ArgumentException.
        var fullName = GetFullName("John", null);
        // Output name result.
        Logging.Log($"Full name is: {fullName}.");
    }
    catch (System.ArgumentException e)
    {
        Logging.Log(e);
    }
}
```

Sure enough, while there's no compilation/build issue, since the `CLR` and code editor both recognize `null` as a valid type to be passed to a `String` parameter field, our backup code catches that the `last` value is `null`, and throws our `System.ArgumentException` at us:

```
[EXPECTED] System.ArgumentException: GetFullName() parameter 'last' must be a valid string.
```

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A deep dive into the System.ArgumentException in .NET, including some basic C# code examples and discussion of different .NET error types.