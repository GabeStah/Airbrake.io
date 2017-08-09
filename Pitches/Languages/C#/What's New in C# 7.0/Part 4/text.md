# What's New in C# 7.0? - Expression-Bodied Members and Throw Expressions

C# 7.0, the latest major version of the extremely popular programming language, was released in March 2017 alongside Visual Studio 2017, bringing a number of new features and capabilities to the table.  Today we'll continue our deep dive into some of these awesome features in our ongoing series, _What's New in C# 7.0?_:

- In [Part 1](https://airbrake.io/blog/csharp/whats-new-in-csharp-7-0) we thoroughly explored `tuple types`, `tuple literals`, and `out variables`.
- In [Part 2](https://airbrake.io/blog/csharp/whats-new-in-c-7-0-pattern-matching-and-local-functions) we looked at `pattern matching` and `local functions`.
- In [Part 3](https://airbrake.io/blog/csharp/digit-separators-reference-returns-and-binary-literals) we examined the `digit separator`, `binary literals`, `returning references` and `local reference variables`.

Today in part 4 we'll cover new `expression-bodied members` and `throw expressions`, so let's get going!

## New Expression-bodied Members

C# 6.0 introduced expression body definitions with `method` and `property get` declarations.  The expression body definition syntax allows single-expression bodies to be written in a more concise and compact format.  At the most basic level, an expression-bodied member looks like: `member => expression;`.

As of C# 6.0, expression bodies can be used for `methods` and `property get` declarations.  For example, here is a basic `User` class that provides the `Email` and `Name` properties using expression body syntax.  The same syntax is also used for the `ToString()` override method:

```cs
internal class User
{
    private readonly string _email;
    private readonly string _name;

    /// <summary>
    /// Email property getter with expression body syntax.
    /// </summary>
    internal string Email => _email;
    /// <summary>
    /// Name property getter with expression body syntax.
    /// </summary>
    internal string Name => _name;

    internal User(string name, string email)
    {
        _email = email;
        _name = name;
    }

    /// <summary>
    /// Override ToString() method with expression body syntax.
    /// </summary>
    /// <returns>Name and email combination.</returns>
    public override string ToString() => $"{Name} - {Email}";
}
```

C# 7.0 adds a number of new expression-bodied members to the list valid list, including `constructors`, `finalizers`, `property setters`, and `indexers`.  For the first three, below we've modified our `User` class to include all expression-bodied members (except an `indexer`, since it doesn't make much sense in this context):

```cs
internal class User
{
    private string _email;
    private string _name;

    /// <summary>
    /// Email property with expression body syntax.
    /// </summary>
    internal string Email
    {
        get => _email;
        set => _email = value;
    }

    /// <summary>
    /// Name property with expression body syntax.
    /// </summary>
    internal string Name
    {
        get => _name;
        set => _name = value;
    }

    /// <summary>
    /// Constructor with expression body syntax.
    /// </summary>
    /// <param name="email"></param>
    internal User(string email) => _email = email;

    internal User(string name, string email)
    {
        _email = email;
        _name = name;
    }

    /// <summary>
    /// Override ToString() method with expression body syntax.
    /// </summary>
    /// <returns>Name and email combination.</returns>
    public override string ToString() => $"{Name} - {Email}";

    /// <summary>
    /// Destructor with expression body syntax.
    /// </summary>
    ~User() => Logging.Log($"{this} is being destroyed.");
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
                length -= insert.Length + 2;
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

We also have a `UserCollection` class that contains a private `User[] _users` property:

```cs
internal class UserCollection
{
    private readonly User[] _users =
    {
        new User("Alice Smith", "alice.smith@airbrake.io"),
        new User("Bob Smith", "bob.smith@airbrake.io"),
        new User("Christine Parker", "christine.parker@airbrake.io"),
        new User("Danny Danson", "danny.danson@airbrake.io")
    };

    /// <summary>
    /// Indexer with get/set expression body syntax.
    /// </summary>
    /// <param name="index">Index of User.</param>
    /// <returns>User.</returns>
    public User this[int index]
    {
        get => _users[index];
        set => _users[index] = value;
    }
}
```

We can then instantiate a new `UserCollection` instance, use the `indexer` to retrieve the second `User` element, and then output the returned value to the console (using the `ToString()` override method we defined earlier):

```cs
class Program
{
    static void Main(string[] args)
    {
        var users = new UserCollection();
        Logging.Log(users[1].ToString());
        // Bob Smith - bob.smith@airbrake.io
    }
}
```

## Throw Expressions

Closely related to expression-bodied members, C# 7.0 also introduces the ability to create `throw expressions`.  In many places where an expression might be valid, a `throw expression` can be used to directly `throw` an `Exception`.  For example, here we're causing attempts to use the setter of the `User.Email` property to `throw` a new `NotImplementedException`:

```cs
internal class User
{
    private string _email;
    private string _name;

    /// <summary>
    /// Email property with expression body syntax 
    /// getter and throw expression for setter.
    /// </summary>
    internal string Email
    {
        get => _email;
        set => throw new NotImplementedException("User.Email.set is not implemented.");
    }
    
    // ...
}
```

The <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-dotnet">Sharpbrake</a> library provides robust exception tracking capabilities for all of your C# and .NET applications.  `Sharpbrake` provides real-time error monitoring and automatic exception reporting across your entire project, so you and your team are immediately alerted to even the smallest hiccup, and can appropriately respond before major problems arise.  With a robust API and tight integration with the powerful `Airbrake` web dashboard, `Sharpbrake` will revolutionize how your team manages exceptions.

Check out all the great features <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-dotnet">Sharpbrake</a> brings to the table and see why so many of the world's top development teams use `Airbrake` to dramatically improve their exception handling practices!

---

__META DESCRIPTION__

Part 4 of our exploration into what's new in C# 7.0, including new expression-bodied members and throw expressions.

---

__SOURCES__

- http://reducing-suffering.org/how-many-wild-animals-are-there/#Mammals
- https://insights.stackoverflow.com/survey/2017#most-popular-technologies
- https://www.visualstudio.com/en-us/news/releasenotes/vs2017-relnotes
- https://github.com/dotnet/roslyn/blob/master/docs/Language%20Feature%20Status.md#c-70-and-vb-15
- https://blogs.msdn.microsoft.com/dotnet/2017/03/09/new-features-in-c-7-0/