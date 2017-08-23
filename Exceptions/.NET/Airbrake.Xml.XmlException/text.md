# .NET Exceptions - System.Xml.XmlException

Taking the next step in our journey through the vast [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, today we're taking a closer look at the System.Xml.XmlException class.  As you might suspect from the name, the `System.Xml.XmlException` is the generic error that is thrown when most XML-related errors occur, such as failure to parse malformed XML content.

In this article we'll examine the `System.Xml.XmlException` by looking at where it resides in the .NET exception hierarchy.  We'll also go over some fully functional C# code examples that will illustrate how XML might be commonly used, and how that can potentially lead to `System.Xml.XmlExceptions` in your own code.  Let's get this party started!

## The Technical Rundown

- All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.
- [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception) is inherited from the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) class.
- `System.Xml.XmlException` inherits from [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception).

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```xml
<?xml version="1.0" encoding="utf-8" ?>
<!-- books.xml -->
<Books>
  <Book>
    <Title>The Stand</Title>
    <Author>Stephen King</Author>
    <PageCount>1153</PageCount>
  </Book>
  <Book>
    <Title>The Slow Regard of Silent Things</Title>
    <Author>Patrick Rothfuss</Author>
    <PageCount>159</PageCount>
  </Book>
  <Book>
    <Title>Magician</Title>
    <Author>Raymond E. Feist</Author>
    <PageCount>681</PageCount>
  </Book>
</Books>
```

```xml
<?xml version="1.0" encoding="utf-8" ?>
<!-- books-malformed.xml -->
<Books>
  <Book>
    <Title>Dune</Title>
    <Author>Frank Herbert</Author>
    <PageCount604</PageCount>
  </Book>
  <Book>
    <Title>The Revenant</Title>
    <Author>Michael Punke</Author>
    <PageCount>272</PageCount>
  </Book>
  <Book>
    <Title>The Code Book: The Science of Secrecy from Ancient Egypt to Quantum Cryptography</Title>
    <Author>Simon Singh</Author>
    <PageCount>412</PageCount>
  </Book>
</Books>
```

```cs
// <Airbrake.Xml.XmlException>/Program.cs
using System;
using System.Xml.Linq;
using Utility;

namespace Airbrake.Xml.XmlException
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create Books from books.xml.
            Logging.LineSeparator("USING 'books.xml'");
            CreateBooksFromXml(@"books.xml");

            // Create Books from books-malformed.xml.
            Logging.LineSeparator("USING 'books-malformed.xml'");
            CreateBooksFromXml(@"books-malformed.xml");
        }

        /// <summary>
        /// Create a series of Book instances from data in passed Xml.
        /// </summary>
        /// <param name="path">Path to Xml data file.</param>
        internal static void CreateBooksFromXml(string path)
        {
            try
            {
                // Load Xml from path and get Book elements.
                var books = XElement.Load(path).Elements("Book");
                // Loop through book elements.
                foreach (var element in books)
                {
                    // Attempt to convert page count string to integer.
                    int.TryParse(element.Element("PageCount")?.Value, out var pageCount);
                    // Create new book instance.
                    var book = new Book(element.Element("Title")?.Value, 
                        element.Element("Author")?.Value,
                        pageCount);
                    // Output book instance to log.
                    Logging.Log(book);
                }
            }
            catch (System.Xml.XmlException exception)
            {
                // Output expected XmlExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Output unexpected Exceptions.
                Logging.Log(exception, false);
            }
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
using System.Xml.Serialization;

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
            if (value is IXmlSerializable)
            {
                Debug.WriteLine(value);
            }
            else
            {
                Debug.WriteLine(outputType == OutputType.Timestamp
                    ? $"[{StopwatchProxy.Instance.Stopwatch.Elapsed}] {ObjectDumper.Dump(value)}"
                    : ObjectDumper.Dump(value));
            }
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

## When Should You Use It?

There's no time like the present, so let's just jump right into our code sample and get to working with some XML!  For our example code we have a small collection of books contained within a few local XML documents, `books.xml` and `books-malformed.xml`:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<!-- books.xml -->
<Books>
  <Book>
    <Title>The Stand</Title>
    <Author>Stephen King</Author>
    <PageCount>1153</PageCount>
  </Book>
  <Book>
    <Title>The Slow Regard of Silent Things</Title>
    <Author>Patrick Rothfuss</Author>
    <PageCount>159</PageCount>
  </Book>
  <Book>
    <Title>Magician</Title>
    <Author>Raymond E. Feist</Author>
    <PageCount>681</PageCount>
  </Book>
</Books>
```

```xml
<?xml version="1.0" encoding="utf-8" ?>
<!-- books-malformed.xml -->
<Books>
  <Book>
    <Title>Dune</Title>
    <Author>Frank Herbert</Author>
    <PageCount604</PageCount>
  </Book>
  <Book>
    <Title>The Revenant</Title>
    <Author>Michael Punke</Author>
    <PageCount>272</PageCount>
  </Book>
  <Book>
    <Title>The Code Book: The Science of Secrecy from Ancient Egypt to Quantum Cryptography</Title>
    <Author>Simon Singh</Author>
    <PageCount>412</PageCount>
  </Book>
</Books>
```

It's worth noting that, if you manually add a file to your Visual Studio project (such as `books.xml`), you'll likely need to adjust the file properties and set the `Copy to Output Directory` flag to `Copy always` or `Copy if newer`.  This will ensure that the project file in question is locally available to your executing assembly.

With our book data in hand, we want to extract that data and use it in code.  Our goal is to create a series of `Book` class instances for each book element found in our XML documents.  Here's the simple `Book` class we'll be using:

```cs
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
```

In order to parse our XML we have many options, including LINQ to XML.  However, our example XML documents are fairly basic in structure, so we don't need any fancy queries in this particular tutorial.  To read the XML and create our `Book` instances we've created the `CreateBooksFromXml(string path)` method:

```cs
/// <summary>
/// Create a series of Book instances from data in passed Xml.
/// </summary>
/// <param name="path">Path to Xml data file.</param>
internal static void CreateBooksFromXml(string path)
{
    try
    {
        // Load Xml from path and get Book elements.
        var books = XElement.Load(path).Elements("Book");
        // Loop through book elements.
        foreach (var element in books)
        {
            // Attempt to convert page count string to integer.
            int.TryParse(element.Element("PageCount")?.Value, out var pageCount);
            // Create new book instance.
            var book = new Book(element.Element("Title")?.Value, 
                element.Element("Author")?.Value,
                pageCount);
            // Output book instance to log.
            Logging.Log(book);
        }
    }
    catch (System.Xml.XmlException exception)
    {
        // Output expected XmlExceptions.
        Logging.Log(exception);
    }
    catch (Exception exception)
    {
        // Output unexpected Exceptions.
        Logging.Log(exception, false);
    }
}
```

This method starts by attempting to create an `XElement` from the passed XML `path`, then retrieves the `IEnumerable<XElement>` collection of elements with the corresponding element name of `Book`.  We then iterate over that collection of elements and create a new `Book` class instance for each element, passing the appropriate XML element values to the corresponding arguments of the `Book` constructor.  Since the `XElement.Value` property is a `string` data type, we need to explicitly convert the `PageCount` property element to an `int` using `int.TryParse()`.  Finally, once our `Book` instance is created we output it to the log.

The `Program.Main(string[] args)` method tests out our functionality two times, once for each respective XML document:

```cs
static void Main(string[] args)
{
    // Create Books from books.xml.
    Logging.LineSeparator("USING 'books.xml'");
    CreateBooksFromXml(@"books.xml");

    // Create Books from books-malformed.xml.
    Logging.LineSeparator("USING 'books-malformed.xml'");
    CreateBooksFromXml(@"books-malformed.xml");
}
```

As you might suspect from the XML document names, the first call using `books.xml` works just fine, creating new `Book` instances for each parsed XML element and outputting the `Book` instance objects to the log:

```
---------- USING 'books.xml' -----------
{Utility.Book(HashCode:30015890)}
  Author: "Stephen King"
  PageCount: 1153
  Title: "The Stand"

{Utility.Book(HashCode:1707556)}
  Author: "Patrick Rothfuss"
  PageCount: 159
  Title: "The Slow Regard of Silent Things"

{Utility.Book(HashCode:15368010)}
  Author: "Raymond E. Feist"
  PageCount: 681
  Title: "Magician"
```

However, the second call using `books-malformed.xml` throws a `System.Xml.XmlException` at us, indicating the position of the character that caused the issue:

```
----- USING 'books-malformed.xml' ------
[EXPECTED] System.Xml.XmlException: The '<' character, hexadecimal value 0x3C, cannot be included in a name. Line 7, position 18.
```

You may have picked up on it already, but the problem is a missing `greater-than sign` (`>`) following the opening `PageCount` element in the first `Book` element of _Dune_:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<!-- books-malformed.xml -->
<Books>
  <Book>
    <Title>Dune</Title>
    <Author>Frank Herbert</Author>
    <!-- Missing > character on the next line. -->
    <PageCount604</PageCount>
  </Book>
  <!-- ... -->
</Books>
```

Obviously, we can fix this by adding that missing greater-than sign back.  Doing so and running our code again produces the proper output we're after for the `books-malformed.xml` parse:

```
----- USING 'books-malformed.xml' ------
{Utility.Book(HashCode:4094363)}
  Author: "Frank Herbert"
  PageCount: 604
  Title: "Dune"

{Utility.Book(HashCode:36849274)}
  Author: "Michael Punke"
  PageCount: 272
  Title: "The Revenant"

{Utility.Book(HashCode:63208015)}
  Author: "Simon Singh"
  PageCount: 412
  Title: "The Code Book: The Science of Secrecy from Ancient Egypt to Quantum Cryptography"
```

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A deep dive into the System.Xml.XmlException in .NET, including C# code illustrating how to create and parse XML documents in C#.