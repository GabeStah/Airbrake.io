# .NET Exceptions - System.Web.HttpException

Moving along through our in-depth [__.NET Exception Handling__](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) series, today we'll be going over the **System.Web.HttpException**.  The `HttpException` is the most basic type of web-based exception, so it can be used for many general purposes that relate to something going wrong when processing an HTTP request in an ASP.NET or similar application.

Throughout this article we'll explore the `HttpException` in more detail, starting with where it sits in the overall .NET exception hierarchy.  Then we'll take a look at a fully-functional C# code sample that illustrates one example of how you might use `HttpExceptions` in your own code.  Let's get started!

## The Technical Rundown

All .NET exceptions are derived classes of the [`System.Exception`](https://airbrake.io/blog/dotnet-exception-handling/exception-class-hierarchy) base class, or derived from another inherited class therein.  The full exception hierarchy of this error is:

- [`System.Object`](https://docs.microsoft.com/en-us/dotnet/api/system.object)
    - [`System.Exception`](https://docs.microsoft.com/en-us/dotnet/api/system.exception)
        - [`System.SystemException`](https://docs.microsoft.com/en-us/dotnet/api/system.systemexception)
            - [`System.Runtime.InteropServices.ExternalException`](https://docs.microsoft.com/en-us/dotnet/api/system.runtime.interopservices.externalexception?view=netframework-4.7)
                - `HttpException`

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```asp
<%@ page title="Home Page" language="C#" masterpagefile="~/Site.Master" autoeventwireup="true" codebehind="Default.aspx.cs" inherits="Airbrake.Web.HttpException._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-4">
        <div class="form-group">
            <asp:Label AssociatedControlID="BookTitle" runat="server">Title</asp:Label>
            <asp:TextBox ID="BookTitle" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label AssociatedControlID="BookAuthor" runat="server">Author</asp:Label>
            <asp:TextBox ID="BookAuthor" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label AssociatedControlID="BookPageCount" runat="server">Page Count</asp:Label>
            <asp:TextBox ID="BookPageCount" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label AssociatedControlID="BookPublicationDate" runat="server">Publication Date</asp:Label>
            <asp:Calendar ID="BookPublicationDate" runat="server"></asp:Calendar>
        </div>
        <asp:Button ID="BookSubmit" CssClass="btn btn-primary" Text="Submit" OnClick="BookSubmit_Click" runat="server" />
        <br />
        <asp:Label ID="BookLabel" runat="server"></asp:Label>
    </div>
</asp:Content>
```

```cs
// Default.aspx.cs
using System;
using System.Web.UI;
using Utility;

namespace Airbrake.Web.HttpException
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Creates a new Book using Book form.
        /// </summary>
        /// <returns>Created Book.</returns>
        private Book CreateBook()
        {
            try
            {
                // Ensure that book title is present.
                if (BookTitle.Text == "")
                {
                    throw new System.Web.HttpException($"Book Title cannot be empty.");
                }

                // Ensure that book author is present.
                if (BookAuthor.Text == "")
                {
                    throw new System.Web.HttpException($"Book Author cannot be empty.");
                }

                // Check if no publication date was selected.
                if (BookPublicationDate.SelectedDate.Date == DateTime.MinValue)
                {
                    return new Book(
                        BookTitle.Text,
                        BookAuthor.Text,
                        BookPageCount.Text == "" ? 0 : Convert.ToInt32(BookPageCount.Text)
                    );
                }

                // Instantiate new Book.
                return new Book(
                    BookTitle.Text,
                    BookAuthor.Text,
                    BookPageCount.Text == "" ? 0 : Convert.ToInt32(BookPageCount.Text),
                    BookPublicationDate.SelectedDate
                );
            }
            catch (FormatException exception)
            {
                // Output expected FormatExceptions.
                Logging.Log(exception);
                // Throw new HttpException.
                throw new System.Web.HttpException(exception.Message);
            }
        }

        protected void BookSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                // Get generated Book.
                var book = CreateBook();
                // Generate output message.
                var output = $"New Book: {book}";
                // Output to log.
                Logging.Log(output);
                // Modify label for Standard output.
                BookLabel.Text = output;
                BookLabel.ForeColor = System.Drawing.Color.Black;
            }
            catch (System.Web.HttpException exception)
            {
                // Output expected HttpExceptions.
                Logging.Log(exception);
                // Modify label for Error output.
                BookLabel.Text = $"HttpException: {exception.Message}";
                BookLabel.ForeColor = System.Drawing.Color.Red;
            }
        }
    }
}
```

```cs
// <Utility>/Book.cs
using System;

namespace Utility
{
    public interface IBook
    {
        string Author { get; set; }
        int PageCount { get; set; }
        DateTime? PublicationDate { get; set; }
        string Title { get; set; }
    }

    /// <summary>
    /// Simple Book class.
    /// </summary>
    public class Book : IBook
    {
        public string Author { get; set; }
        public int PageCount { get; set; }
        public DateTime? PublicationDate { get; set; }
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

        public Book(string title, string author, int pageCount, DateTime publicationDate)
        {
            Author = author;
            PageCount = pageCount;
            PublicationDate = publicationDate;
            Title = title;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var publicationDate = PublicationDate is null ? null : $", published on {PublicationDate.Value.ToLongDateString()}";
            return $"'{Title}' by {Author} at {PageCount} pages{publicationDate}";
        }
    }
}
```

```cs
// <Utility/>Logging.cs
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
            Output(value, outputType);
        }

        /// <summary>
        /// Outputs to <see cref="Debug.WriteLine(String)"/>.
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        /// <param name="arg0"></param>
        public static void Log(string value, object arg0)
        {
            Debug.WriteLine(value, arg0);
        }

        /// <summary>
        /// Outputs to <see cref="Debug.WriteLine(String)"/>.
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        public static void Log(string value, object arg0, object arg1)
        {
            Debug.WriteLine(value, arg0, arg1);
        }

        /// <summary>
        /// Outputs to <see cref="Debug.WriteLine(String)"/>.
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        /// <param name="arg0"></param>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        public static void Log(string value, object arg0, object arg1, object arg2)
        {
            Debug.WriteLine(value, arg0, arg1, arg2);
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

            Output(value, outputType);
        }

        private static void Output(string value, OutputType outputType = OutputType.Default)
        {
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

As mentioned in the introduction, the `HttpException` is a baseline, generic HTTP-based exception, so it can be used in a variety of situations that you deem appropriate.  In the simplest sense, an `HttpException` should be thrown when something goes wrong while processing any HTTP request.  Thus, you can freely throw `HttpExceptions` throughout your web application, wherever they are appropriate.

To illustrate how you might use a `HttpException` in your own code our sample begins by creating a standard ASP.NET web app.  There's little reason to go through all the files included in the project, since they're primarily all the defaults.  Instead, the goal of this example code is to create a simple web form that allows the user to enter some fields to create a `Book`, such as `Title`, `Author`, `Page Count`, and `Publication Date`.  Upon submitting the form, our code performs some basic validation on the server-side.  If validation is successful, a new `Book` instance is created and the string format is output to the user.  If validation fails, a new `HttpException` is thrown, indicating what went wrong so the user can resolve it.

Thus, we start with a basic `Book` class that includes the previously mentioned properties.  It also includes a slightly modified `ToString()` method override, which checks if `PublicationDate` is present, and therefore, whether it should be included in the `Book` output:

```cs
// <Utility>/Book.cs
using System;

namespace Utility
{
    public interface IBook
    {
        string Author { get; set; }
        int PageCount { get; set; }
        DateTime? PublicationDate { get; set; }
        string Title { get; set; }
    }

    /// <summary>
    /// Simple Book class.
    /// </summary>
    public class Book : IBook
    {
        public string Author { get; set; }
        public int PageCount { get; set; }
        public DateTime? PublicationDate { get; set; }
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

        public Book(string title, string author, int pageCount, DateTime publicationDate)
        {
            Author = author;
            PageCount = pageCount;
            PublicationDate = publicationDate;
            Title = title;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var publicationDate = PublicationDate is null ? null : $", published on {PublicationDate.Value.ToLongDateString()}";
            return $"'{Title}' by {Author} at {PageCount} pages{publicationDate}";
        }
    }
}
```

Our `Default.aspx` page uses `Bootstrap` for CSS classes, but otherwise is a pretty traditional collection of `TextBox` fields to enter data, and associated `Labels`:

```asp
<%@ page title="Home Page" language="C#" masterpagefile="~/Site.Master" autoeventwireup="true" codebehind="Default.aspx.cs" inherits="Airbrake.Web.HttpException._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="col-md-4">
        <div class="form-group">
            <asp:Label AssociatedControlID="BookTitle" runat="server">Title</asp:Label>
            <asp:TextBox ID="BookTitle" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label AssociatedControlID="BookAuthor" runat="server">Author</asp:Label>
            <asp:TextBox ID="BookAuthor" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label AssociatedControlID="BookPageCount" runat="server">Page Count</asp:Label>
            <asp:TextBox ID="BookPageCount" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label AssociatedControlID="BookPublicationDate" runat="server">Publication Date</asp:Label>
            <asp:Calendar ID="BookPublicationDate" runat="server"></asp:Calendar>
        </div>
        <asp:Button ID="BookSubmit" CssClass="btn btn-primary" Text="Submit" OnClick="BookSubmit_Click" runat="server" />
        <br />
        <asp:Label ID="BookLabel" runat="server"></asp:Label>
    </div>
</asp:Content>
```

We've also added the `BookSubmit_Click()` method to the `OnClick` event of the `BookSubmit` `Button`, which will trigger our validation.  The `BookLabel` `Label` will serve as a basic output for the user should anything go wrong.

Finally, the actual C# code for our `Default.aspx` page starts with the standard `Page_Load(object sender, EventArgs e)` event method, but we aren't performing any actions in there so it's blank:

```cs
// Default.aspx.cs
using System;
using System.Web.UI;
using Utility;

namespace Airbrake.Web.HttpException
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
    
    // ...
}
```

We next have a private `CreateBook()` helper method that attempts to create a new `Book` instance using all the entered data from our `Book` form:

```cs
/// Creates a new Book using Book form.
/// </summary>
/// <returns>Created Book.</returns>
private Book CreateBook()
{
    try
    {
        // Ensure that book title is present.
        if (BookTitle.Text == "")
        {
            throw new System.Web.HttpException($"Book Title cannot be empty.");
        }

        // Ensure that book author is present.
        if (BookAuthor.Text == "")
        {
            throw new System.Web.HttpException($"Book Author cannot be empty.");
        }

        // Check if no publication date was selected.
        if (BookPublicationDate.SelectedDate.Date == DateTime.MinValue)
        {
            return new Book(
                BookTitle.Text,
                BookAuthor.Text,
                BookPageCount.Text == "" ? 0 : Convert.ToInt32(BookPageCount.Text)
            );
        }

        // Instantiate new Book.
        return new Book(
            BookTitle.Text,
            BookAuthor.Text,
            BookPageCount.Text == "" ? 0 : Convert.ToInt32(BookPageCount.Text),
            BookPublicationDate.SelectedDate
        );
    }
    catch (FormatException exception)
    {
        // Output expected FormatExceptions.
        Logging.Log(exception);
        // Throw new HttpException.
        throw new System.Web.HttpException(exception.Message);
    }
}
```

We don't want to create a `Book` that has no `Author` or `Title`, so we explicitly throw a new `HttpException` if either field is empty.  We have decided to allow `PageCount` to be empty, so we set the default value to `0` if it's not empty, otherwise we convert the `string` value in `BookPageCount.Text` to an integer.  Finally, we also need to check if the user has selected a date within the `BookPublicationDate` calendar control.  If not, the value is equal to the `DateTime.MinValue`, so we call a different `Book` constructor overload then if a publication date was selected by the user.

From there, we need to add the `BookSubmit_Click(object sender, EventArgs e)` method, which is our event handler for when the submit button is clicked:

```cs
protected void BookSubmit_Click(object sender, EventArgs e)
{
    try
    {
        // Get generated Book.
        var book = CreateBook();
        // Generate output message.
        var output = $"New Book: {book}";
        // Output to log.
        Logging.Log(output);
        // Modify label for Standard output.
        BookLabel.Text = output;
        BookLabel.ForeColor = System.Drawing.Color.Black;
    }
    catch (System.Web.HttpException exception)
    {
        // Output expected HttpExceptions.
        Logging.Log(exception);
        // Modify label for Error output.
        BookLabel.Text = $"HttpException: {exception.Message}";
        BookLabel.ForeColor = System.Drawing.Color.Red;
    }
}
```

Here we're just creating a new `Book` instance using the `CreateBook()` method, then outputting the newly-generated `Book` to the debug log, as well as formatting the `BookLabel` to show the user the new `Book`.  However, if an `HttpException` occurs we output that to the log, and also alter the label output to display the error for the user.

With everything in place let's run our little web application and try playing around with creating some `Books`.  We start with all four fields (`Title`, `Author`, `Page Count`, and `Publication Date`) empty/unselected.  If we click `Submit` without making any changes, the first validation that fails is the `Title` check, which outputs an `HttpException` to the user and the log:

```
[EXPECTED] System.Web.HttpException (0x80004005): Book Title cannot be empty.
```

Let's enter `The Stand` into the `Title` field and try to `Submit` again.  Now `Author` is the problematic field:

```
[EXPECTED] System.Web.HttpException (0x80004005): Book Author cannot be empty.
```

In our case, we're talking about `The Stand` by Stephen King, so let's add him to the `Author` field and `Submit` a third time.  Lo and behold, no exceptions are thrown and we get a new `Book` output:

```
New Book: 'The Stand' by Stephen King at 0 pages
```

We explicitly allowed zero pages to be specified, so that's what we get.  However, that's not quite right, so let's change it to the correct value of `1153` and try again:

```
New Book: 'The Stand' by Stephen King at 1153 pages
```

Now let's select the publication date of `September 1st, 1978` and `Submit` once more:

```
New Book: 'The Stand' by Stephen King at 1153 pages, published on Friday, September 1, 1978
```

This all works as intended, but what happens if we try to use an incorrect value type?  For example, let's change the numeric `Page Count` field from `1153` to a written form of `one-thousand, fifty-three` and `Submit` one final time:

```
[EXPECTED] System.FormatException: Input string was not in a correct format.
[EXPECTED] System.Web.HttpException (0x80004005): Input string was not in a correct format.
```

Here we see two exceptions are output to the log (while only the `HttpException` is shown to the user on the page).  The reason for this is that we attempt to convert the `BookPageCount.Text` string value to an integer using `Convert.ToInt32(...)`, but the string we passed cannot be parsed and converted by the default API.  Therefore, this throws a `System.FormatException`, which we explicitly catch in the `CreateBook()` method, outputting that to the log and manually throwing our own `HttpException` that can be caught elsewhere and shown to the user.

To get the most out of your own applications and to fully manage any and all .NET Exceptions, check out the <a class="js-cta-utm" href="https://airbrake.io/languages/net_bug_tracker?utm_source=blog&amp;utm_medium=end-post&amp;utm_campaign=airbrake-net">Airbrake .NET Bug Handler</a>, offering real-time alerts and instantaneous insight into what went wrong with your .NET code, along with built-in support for a variety of popular development integrations including: JIRA, GitHub, Bitbucket, and much more.

---

__META DESCRIPTION__

A close look at the System.Web.HttpException in .NET, including fully-functional code sample illustrating how to use HttpExceptions in ASP.NET apps.