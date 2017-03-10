# C# Coding Best Practices: An Overview

C# provides an abundance of power and capability under the hood.  However, after 17 years of development and with seven major version releases, the plethora of potential capabilities that C# provides has evolved to such a degree that functionally identical code can be written in a many, vastly different ways.

Over the years, the C# community, in large part thanks to the guidance of Microsoft, have developed a series of conventional standards.  While there are many best practices and common conventional categories, throughout this article we'll focus on the three fundamental aspects of quality code convention: `naming`, `layout`, and `documentation`.  For each, we'll provide a series of simple `DOs` and `DON'Ts` that support the modern best practices of C# coding convention, in order to assist you with your own future coding endeavors.

## Naming Conventions

Proper naming convention is one of the most important, yet often misunderstood, components used for creating understandable and maintainable source code.  If `classes`, `methods`, and `variables` are named properly, any developer can quickly scan a bit of code and understand both the purpose and the functionality of the code therein.

Conversely, poorly named elements quickly lead to confusion, at best, and broken code, at worst.  Even in strongly-controlled development environments like Visual Studio, improper `methods` or `variables` names can often produce exceptions.  Not only is proper naming convention vital for new applications, but for legacy or modified C# software, it can be a real headache to maintain and update the naming throughout.  When exceptions are thrown by old or rarely used code, identifying the _cause_ of exceptions due to improper naming is a hassle, making exception handling software, such as [Airbrake's .NET bug tracker](https://airbrake.io/languages/net_bug_tracker), all the more useful.

To assist with your own naming convention practices, below we've identified a few of the most critical best practices.

- **DO** use `PascalCasing` for `class` and `method` names.
- **DO** use `camelCasing` for `method` arguments and local variables.

```cs
// DO
public class Author 
{
    public void AddBook(Book book)
    {
        // ...
    }

    public string GetFullName(string first, string last)
    {
        string fullName = first + " " + last;
        // ...
    }    
}

// DON'T
public class author 
{
    public void addBook(Book book)
    {
        // ...
    }

    public string get_full_name(string first, string last)
    {
        string full_name = first + " " + last;
        // ...
    }    
}
```

- **DO** use nouns (or noun phrases) when naming classes, as classes typically represent objects or things.  Consequently, class names should rarely be verbs or actions.

```cs
// DO
public class Person
{
    // ..
}

public class Author : Person
{
    // ..
}

// DON'T
public class Delete
{
    // ...
}
```

- **DO** use verbs (or verb phrases) when naming methods.  As an extension of classes, which are the nouns or objects, methods should perform actions upon those objects.

```cs
// DO
public class Person
{
    public void Delete()
    {
        // ...
    }

    public string GetFullName(string first, string last)
    {
        // ...
    }
}

// DON'T
public class Person
{
    public void DeletedPerson()
    {
        // ...
    }

    public string FullName(string first, string last)
    {
        // ...
    }
}
```

- **DON'T** use type identifiers within variable or argument names; allow the explicit typing in the code to specify the type for you.

```cs
// DO
public class Author 
{
    public void AddBook(Book book)
    {
        // ...
    }

    public string GetFullName(string first, string last)
    {
        string fullName = first + " " + last;
        // ...
    }    
}

// DON'T
public class Author 
{
    public void AddBook(Book bookBook)
    {
        // ...
    }

    public string GetFullName(string strFirst, string strLast)
    {
        string strFullName = strFirst + " " + strLast;
        // ...
    }    
}
```

- **DO** use `PascalCasing` for `constants`, rather than `FULLCAPS` as seen in other languages.

```cs
// DO
public static const string ClassificationType = "LibraryOfCongress";

// DON'T
public static const string CLASSIFICATIONTYPE = "LibraryOfCongress";
```

- **DON'T** use abbreviations or try to shorten variable or argument names, except where common convention says it's OK (e.g. `Id` instead of `Identification`).  Common _acronyms_, such as `Http` or `Xml`, are fine.

```cs
// DO
int userId = 1;
double explosionRadius = 1.2345;
string xmlPath = "/documents/xml/data.xml";

// DON'T
int userIdentification = 1;
double explRad = 1.2345;
```

- **DON'T** user `underscores` (`_`) within variable or argument names.  Note: While, historically, underscores were used in C# convention as a prefix for private static variables within a class, current conventions have done away with the practice entirely.

```cs
// DO
int userId = 1;
double explosionRadius = 1.2345;
string xmlPath = "/documents/xml/data.xml";

// DON'T
int user_Id = 1;
double explosion_radius = 1.2345;
private string _xmlPath = "/documents/xml/data.xml";
```

### Layout Conventions

While arguably less critical than proper naming conventions, formatting your code using intelligent layout conventions is a great practice.  Proper layout ensures that code is legible and consistent across the entire project, while also remaining easy to maintain.

Below are a few common best practices you can implement today, in order to improve your own C# code layout.

- **DO** use the `smart indentation` setting in your code editor.
- **DO** use `four spaces` as the indentation length.
- **DO** indent continuation lines one `tab stop` (four spaces).
- **DO** use parentheses to clearly illustrate logical clauses.

```cs
// DO
if ((a > b) && (b < c))
{
    // ...
}

// DON'T
if (a > b && b < c)
{
    // ...
}
```

- **DO** use the [`Allman`](https://en.wikipedia.org/wiki/Indent_style) brace placement style, with each brace on its own separate line, and at the same indentation level as the producing statement:

```cs
// DO
if (name == "Bob")
{
    // ...
}

// DON'T
if (name == "Bob") {
    // ...
}

// DON'T
if (name == "Bob")
    {
        // ...
    }

// DON'T
if (name == "Bob") { // ... }
```

- **DON'T** use the `tab` character for indentation, as a `tab` may represent a different number of columns (spacing) from one development environment to the next.

## Documentation Comments

Proper documentation is a critical component for nearly any successful and malleable software project.  To simplify this documentation process, C# was designed with a built-in mechanism to allow developers to easily document their code using a special XML-based syntax, directly within the source files.  During compilation, a `documentation generator` parses all `documentation commments` that it finds in the source code and produces a `documentation file` that can then be read by a `documentation viewer`, such as a web application.

While the full specification can be found in the [official documentation](https://msdn.microsoft.com/en-us/library/b2s063f7.aspx), the core practice of proper documentation within C# revolves around including a `<summary></summary>` XML tag preceding every user-defined type (`class`, `delegate`, `interface`, etc) or member (`field`, `event`, `property`, `method`, etc).  Inline comments do not need to follow this XML-based syntax, since they are typically just simple notes to oneself, or to other developers.

There are many valid `tags` that can be used within the syntax, but we'll just give an example using a handful of common tags:

- `summary`: The base tag that contains all other tags, and should, at the least, contain a basic summary of the element.
- `example`: Contains example code that illustrates the proper use of the element.  Often includes the `code` tag.
- `code`: Valid C# code, typically used to show a snippet example of the element in question.
- `see`: Used by the `documentation generator` to create an automatic link to other documented elements.
- `param`: Indicates a parameter of the element.
- `returns`: Describes what the element should return, if applicable.

Given all that, our example comment is quite extensive for what amounts to a very simple method, but it illustrates the proper use of C#'s documentation syntax:

```cs
/// <summary>Concatenates the First and Last names into a Full Name value.
/// <example>For example:
/// <code>
///	string full = GetFullName("Jane", "Doe");
/// Console.WriteLine($"Full Name is: {full}");
/// </code>
/// <see cref="System.Console.WriteLine(System.String)"/> for information about output statements.
/// </example>
/// <param name="first">First name</param>
/// <param name="last">Last name</param>
/// <returns>The concatenated Full Name.</returns>
/// </summary>
public string GetFullName(string first, string last)
{
	return $"{first} {last}";
}
```

### Inline Comment Guidelines

Outside of `documentation comments`, there are also a handful of best practices and conventions intended for inline comments as well:

- **DO** place comments on separate lines, not at the end of a code statement.
- **DO** begin comment lines with an uppercase letter and end with a period.
- **DO** place a single space between the comment indicator (`//`) and the comment text.

```cs
// DO
// Check if value is updated.
if (old == new)
{
    // ...
}

// DON'T
if (old == new) // Check if value is updated.
{
    // ...
}

// DON'T
// check if value is updated
if (old == new)
{
    // ...
}

// DON'T
//Check if value is updated.
if (old == new)
{
    // ...
}
```

Above all else, remember that all conventions are just a guideline -- **DON'T** feel obligated to follow them all blindly, especially if doing so would negatively impact your code or the functionality of the overall application.  While these are best practices that are generally accepted by the C# development community, they do not need to be adopted by all developers for all projects.

Instead, what matters most is _consistency_.  If you and your fellow developers decide upon a set of conventions and stick with them throughout the entire software development life cycle, it doesn't matter whether those conventions are "typical" or your own concoction; the end result will be a well-defined and easily maintained product.

---

__SOURCES__

- https://msdn.microsoft.com/en-us/library/ms228593.aspx
- https://msdn.microsoft.com/en-us/library/ff926074.aspx
- http://www.dofactory.com/reference/csharp-coding-standards