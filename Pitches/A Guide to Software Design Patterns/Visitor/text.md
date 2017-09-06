# Behavioral Design Patterns: Visitor

Next up in our detailed [Guide to Software Design Patterns](https://airbrake.io/blog/software-design/software-design-patterns-guide) series we'll look at the **visitor design pattern**, which is one of the more complex patterns we'll be discussing.  The `visitor pattern` is ideal for adding new capabilities to existing objects, without modifying the classes on which those objects operate.

In this article we'll be showing a real world and fully-functional C# code sample of the `visitor` pattern, so let's jump right in!

## In the Real World

The `visitor pattern` is made up of a number of components, but the primary ones are:

- `Visitor` - Declares a `Visit()` method that accepts a passed `Visitable` argument.  The method signature determines the object `type` that will be used when executing the method code.  This allows the code to differentiate between different `Visitable` objects, and route subsequent logic accordingly.
- `Visitable` - Declares an `Accept()` method, which expects a passed `Visitor` argument.  Within this `Accept()` method, the `Visitor` parameter invokes its `Visit()` method, passing the calling `Visitable` object.  This ties the two objects together, without either worrying about the specific data `type` of the other.

I know that's a bit vague and perhaps difficult to follow, but we'll clear things up when you see it in code.  For now, let's consider the real world example of _visiting_ Disneyland (or the amusement park of your choice).  Upon arrival, you buy a full day pass, which costs a pretty penny but gives you unlimited access to all the rides for the rest of the day.  Woohoo!

Now that you have your pass, you don't need to jump through any additional hoops to go on Space (or Splash) Mountain.  You just hop in line and flash your pass to the attendant when the time comes for some fun.  Likewise, the attendant doesn't need to perform any extra checks; once he or she sees your pass, the unspoken contract has been agreed to.

Ignoring the fact that I haven't personally been to Disneyland in years, and they may not even use a pass-based system anymore for all I know, the concept still holds true.  The `visitor design pattern` is used for, well, visiting theme parks so that, once you have access, nobody inside needs to worry about selling you tickets for individual rides.

## Full Code Sample

Below is the full code sample we'll be using in this article.  It can be copied and pasted if you'd like to play with the code yourself and see how everything works.

```cs
// Program.cs
using Utility;

namespace Visitor
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a new document.
            var document = new Document();
            
            // Add some elements to the document.
            document.Elements.Add(new Text("This is plain text."));
            document.Elements.Add(new Hyperlink("Hyperlink to Airbrake.io", "http://airbrake.io"));
            document.Elements.Add(new Paragraph("Lorem ipsum dolor sit amet, consectetur adipiscing elit."));
            document.Elements.Add(new BoldText("Important text to bold!"));

            // Create a few visitors.
            var html = new HtmlVisitor();
            var markdown = new MarkdownVisitor();
            var bbCode = new BbVisitor();

            // Force document to accept passed visitors, 
            // which will each uniquely alter output.
            document.Accept(html);
            document.Accept(markdown);
            document.Accept(bbCode);

            // Log each visitor's output.
            Logging.LineSeparator("HTML");
            Logging.LineSeparator(html.ToString());

            Logging.LineSeparator("Markdown");
            Logging.LineSeparator(markdown.ToString());

            Logging.LineSeparator("BBCode");
            Logging.LineSeparator(bbCode.ToString());
        }
    }
}

// Document.cs
using System.Collections.Generic;

namespace Visitor
{
    /// <summary>
    /// Creates a basic virtual document composed of numerous elements.
    /// </summary>
    public class Document
    {
        public List<Visitable> Elements = new List<Visitable>();

        /// <summary>
        /// Accept the passed IVisitor for each element.
        /// </summary>
        /// <param name="visitor">Visitor to accept.</param>
        public void Accept(IVisitor visitor)
        {
            foreach (var element in Elements)
            {
                element.Accept(visitor);
            }
        }
    }
}

// Visitor.cs
namespace Visitor
{
    public interface IVisitor
    {
        void Visit(IVisitable visitable);
    }

    /// <summary>
    /// Baseline visitor, which stores basic string Content information.
    /// </summary>
    public abstract class Visitor : IVisitor
    {
        public string Content { get; set; } = "";

        /// <summary>
        /// Add IVisitable.Text to Content property.
        /// 
        /// Default Visit method.  Overridden in inherited classes.
        /// </summary>
        /// <param name="visitable">Visitable to grab text from.</param>
        public void Visit(IVisitable visitable)
        {
            Content += visitable.Text;
        }

        public override string ToString()
        {
            return Content;
        }
    }
}

// Visitors.cs
namespace Visitor
{
    /// <summary>
    /// Converts passed IVisitable elements to HTML.
    /// </summary>
    public class HtmlVisitor : Visitor
    {
        public void Visit(BoldText element)
        {
            Content += $"<b>{element.Text}</b>";
        }

        public void Visit(Heading element)
        {
            Content += $"<h{element.Level}>{element.Text}</h{element.Level}>";
        }

        public void Visit(Hyperlink element)
        {
            Content += $"<a href=\"{element.Url}\">{element.Text}</a>";
        }

        public void Visit(Paragraph element)
        {
            Content += $"<p>{element.Text}</p>";
        }

        public void Visit(Text element)
        {
            Content += $"<span>{element.Text}</span>";
            // Arguably we should throw a NotImplementedException, 
            // since a plain Text element in HTML is typically a paragraph.
            //throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts passed IVisitable elements to BBCode.
    /// </summary>
    public class BbVisitor : Visitor
    {
        public void Visit(BoldText element)
        {
            Content += $"[b]{element.Text}[/b]";
        }

        public void Visit(Heading element)
        {
            Content += $"[heading]{element.Text}[/heading]";
        }

        public void Visit(Hyperlink element)
        {
            Content += $"[url={element.Url}]{element.Text}[/url]";
        }

        public void Visit(Paragraph element)
        {
            Content += $"\n\n{element.Text}\n\n";
        }
    }

    /// <summary>
    /// Converts passed IVisitable elements to Markdown.
    /// </summary>
    public class MarkdownVisitor : Visitor
    {
        public void Visit(BoldText element)
        {
            Content += $"**{element.Text}**";
        }

        public void Visit(Heading element)
        {
            // Add seperator character for each Level.
            Content += $"{('#', element.Level)} {element.Text}";
        }

        public void Visit(Hyperlink element)
        {
            Content += $"[{element.Text}]({element.Url})";
        }

        public void Visit(Paragraph element)
        {
            Content += $"\n\n{element.Text}\n\n";
        }
    }
}

// Visitable.cs
namespace Visitor
{
    public interface IVisitable
    {
        string Text { get; set; }

        void Accept(IVisitor visitor);
    }

    /// <summary>
    /// Baseline Visitable, which stores string Text for element.
    /// </summary>
    public abstract class Visitable : IVisitable
    {
        public string Text { get; set; }

        protected Visitable(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Accepts the passed IVisitor.
        /// 
        /// This is the default Accept method.  When called, dynamic object types
        /// route execution to the correct inherited object types.
        /// </summary>
        /// <param name="visitor">Visitor to accept.</param>
        public void Accept(IVisitor visitor)
        {
            // Use dynamic types to force proper reflective calls.
            dynamic dynamicVisitable = this;
            dynamic dynamicVisitor = visitor;
            // Call Visit of passed IVisitor, for inherited IVisitable.
            dynamicVisitor.Visit(dynamicVisitable);
        }
    }
}

// Visitables.cs
namespace Visitor
{
    public class BoldText : Visitable
    {
        public BoldText(string text) : base(text) { }
    }

    public class Heading : Visitable
    {
        public int Level { get; set; }

        public Heading(string text, int level) : base(text)
        {
            Level = level;
        }
    }

    public class Hyperlink : Visitable
    {
        public string Url { get; set; }

        public Hyperlink(string text, string url) : base(text)
        {
            Url = url;
        }
    }

    public class Paragraph : Visitable
    {
        public Paragraph(string text) : base(text) { }
    }

    public class Text : Visitable
    {
        public Text(string text) : base(text) { }
    }
}

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

## How It Works In Code

Our `visitor pattern` code sample is built around the basic concept of creating digital text documents.  Each `Document` consists of a series of elements such as `Text`, `Heading`, `BoldText`, and so forth.  We then want to format the document appropriately, depending whether the output should be in `HTML`, `Markdown`, or `BBCode`.

The simplest place to start is with the `IVisitable` interface, which is where we declare our `Accept(IVisitor visitor)` method.  `IVisitable` also has a `Text` field, which we'll use to store the primary text content of each element.

`Visitable` is an abstract class that implements `IVisitable`.  This class allows us to define any default method logic, such as that for our `Accept(IVisitor visitor)` method:

```cs
// Visitable.cs
namespace Visitor
{
    public interface IVisitable
    {
        string Text { get; set; }

        void Accept(IVisitor visitor);
    }

    /// <summary>
    /// Baseline Visitable, which stores string Text for element.
    /// </summary>
    public abstract class Visitable : IVisitable
    {
        public string Text { get; set; }

        protected Visitable(string text)
        {
            Text = text;
        }

        /// <summary>
        /// Accepts the passed IVisitor.
        /// 
        /// This is the default Accept method.  When called, dynamic object types
        /// route execution to the correct inherited object types.
        /// </summary>
        /// <param name="visitor">Visitor to accept.</param>
        public void Accept(IVisitor visitor)
        {
            // Use dynamic types to force proper reflective calls.
            dynamic dynamicVisitable = this;
            dynamic dynamicVisitor = visitor;
            // Call Visit of passed IVisitor, for inherited IVisitable.
            dynamicVisitor.Visit(dynamicVisitable);
        }
    }
}
```

As the comments state, one important note is that we're using the [`dynamic`](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/types/using-type-dynamic) keyword within `Accept(IVisitor visitor)`, which allows both the `Visitable` and `Visitor` instances to bypass static type checking.  This allows the appropriate `Visit()` method signature, within the appropriate `Visitor` class declaration, to be called at runtime.  We'll see this in action in a moment, but it allows our code to only include method declarations that are critical to behavior, while allowing the code to use defaults elsewhere.

Next we need to define some `Visitables`, all of which inherit the `Visitable` class:

```cs
// Visitables.cs
namespace Visitor
{
    public class BoldText : Visitable
    {
        public BoldText(string text) : base(text) { }
    }

    public class Heading : Visitable
    {
        public int Level { get; set; }

        public Heading(string text, int level) : base(text)
        {
            Level = level;
        }
    }

    public class Hyperlink : Visitable
    {
        public string Url { get; set; }

        public Hyperlink(string text, string url) : base(text)
        {
            Url = url;
        }
    }

    public class Paragraph : Visitable
    {
        public Paragraph(string text) : base(text) { }
    }

    public class Text : Visitable
    {
        public Text(string text) : base(text) { }
    }
}
```

I've left out the comments for these since they're quite self-explanatory.  We call `: base(text)` within each constructor, so we don't need to reassign the passed `string text` parameter to the `Text` property of the base `Visitable` class.  Other than that, each of these classes allows us to create unique element types within our `Document` class, which we'll see in a moment.

With our `Visitables` out of the way, it's now time to create our `IVisitor` interface, and the `Visitor` that that implements it:

```cs
// Visitor.cs
namespace Visitor
{
    public interface IVisitor
    {
        void Visit(IVisitable visitable);
    }

    /// <summary>
    /// Baseline visitor, which stores basic string Content information.
    /// </summary>
    public abstract class Visitor : IVisitor
    {
        public string Content { get; set; } = "";

        /// <summary>
        /// Add IVisitable.Text to Content property.
        /// 
        /// Default Visit method.  Overridden in inherited classes.
        /// </summary>
        /// <param name="visitable">Visitable to grab text from.</param>
        public void Visit(IVisitable visitable)
        {
            Content += visitable.Text;
        }

        public override string ToString()
        {
            return Content;
        }
    }
}
```

As previously mentioned, the fundamental method we need is `Visit(IVisitable visitable)`.  As you'll recall, this is invoked within the `Visitable.Accept(IVisitor visitor)` method, and is the primary means by which all `Visitors` interact with all `Visitables`.  In this case, the default logic consists of simply concatenating the `Text` property from the passed `IVisitable` onto the `string Content` property of the `Visitor` instance.

Now we need to define the various types of document formatting we want, which is accomplished within unique `Visitor`-inherited classes:

```cs
// Visitors.cs
namespace Visitor
{
    /// <summary>
    /// Converts passed IVisitable elements to HTML.
    /// </summary>
    public class HtmlVisitor : Visitor
    {
        public void Visit(BoldText element)
        {
            Content += $"<b>{element.Text}</b>";
        }

        public void Visit(Heading element)
        {
            Content += $"<h{element.Level}>{element.Text}</h{element.Level}>";
        }

        public void Visit(Hyperlink element)
        {
            Content += $"<a href=\"{element.Url}\">{element.Text}</a>";
        }

        public void Visit(Paragraph element)
        {
            Content += $"<p>{element.Text}</p>";
        }

        public void Visit(Text element)
        {
            Content += $"<span>{element.Text}</span>";
            // Arguably we should throw a NotImplementedException, 
            // since a plain Text element in HTML is typically a paragraph.
            //throw new NotImplementedException();
        }
    }

    /// <summary>
    /// Converts passed IVisitable elements to BBCode.
    /// </summary>
    public class BbVisitor : Visitor
    {
        public void Visit(BoldText element)
        {
            Content += $"[b]{element.Text}[/b]";
        }

        public void Visit(Heading element)
        {
            Content += $"[heading]{element.Text}[/heading]";
        }

        public void Visit(Hyperlink element)
        {
            Content += $"[url={element.Url}]{element.Text}[/url]";
        }

        public void Visit(Paragraph element)
        {
            Content += $"\n\n{element.Text}\n\n";
        }
    }

    /// <summary>
    /// Converts passed IVisitable elements to Markdown.
    /// </summary>
    public class MarkdownVisitor : Visitor
    {
        public void Visit(BoldText element)
        {
            Content += $"**{element.Text}**";
        }

        public void Visit(Heading element)
        {
            // Add seperator character for each Level.
            Content += $"{('#', element.Level)} {element.Text}";
        }

        public void Visit(Hyperlink element)
        {
            Content += $"[{element.Text}]({element.Url})";
        }

        public void Visit(Paragraph element)
        {
            Content += $"\n\n{element.Text}\n\n";
        }
    }
}
```

Here we're starting to see the `visitor design pattern` come to fruition.  Each `Visit(...)` method signature above is contained within a unique `Visitor` class, and is differentiated by the unique `Visitable` data `type` it accepts.  This combination of two unique instance types determining the logical behavior is the **primary purpose** of the `visitor pattern`, and is known as [`multiple dispatch`](https://en.wikipedia.org/wiki/Multiple_dispatch) (or `double dispatch`).  This allows languages like C#, which are typically only `single dispatch`, to perform function calls based on the runtime `types` of multiple objects, as seen above.

Anyway, each of the `Visitor` classes above concatenates the passed element `Text` onto the parent `Content` property string, while also formatting it in a way that matches that particular `Visitor` style.  For example, I'm writing these words right now in a `Markdown` document, so when I create [a url like this](https://airbrake.io), I format it in exactly the same way that `MarkdownVisitor(Hyperlink element)` formats its passed element.

What's also critically important here is that we would _typically_ need to declare a `Visit(...)` method overload for **each and every** `Visitable` class in the application.  However, you'll notice that we've left out such method declarations for some `Visitor` classes.  For example, neither `MarkdownVisitor` nor `BbVisitor` have a `Visit(Text element)` method.  This is because, as you may recall, we configured this example to use `dynamic` values when invoking our `Visit(...)` methods.  This allows C# to dynamically determine if a matching method signature exists at runtime.  If so, it passes execution along to that method, but if not, it defaults to the baseline `Visitor.Visit(IVisitable visitable)` method.

The last component to our example is the basic `Document` class, which isn't necessary to the `visitor pattern`.  However, it serves the purpose here of holding a `List<Visitable>` property called `Elements`, which is all the `Visitable` objects we've added to the document.

```cs
// Document.cs
using System.Collections.Generic;

namespace Visitor
{
    /// <summary>
    /// Creates a basic virtual document composed of numerous elements.
    /// </summary>
    public class Document
    {
        public List<Visitable> Elements = new List<Visitable>();

        /// <summary>
        /// Accept the passed IVisitor for each element.
        /// </summary>
        /// <param name="visitor">Visitor to accept.</param>
        public void Accept(IVisitor visitor)
        {
            foreach (var element in Elements)
            {
                element.Accept(visitor);
            }
        }
    }
}
```

`Accept(IVisitor visitor)` iterates over all elements and invokes their respective `Accept(IVisitor visitor)`, using the passed `IVisitor` instance.  This will allow us `Accept()` a single `Visitor` with one call.

Now, with everything setup, let's test this out in our `Program.Main(string[] args)` method:

```cs
static void Main(string[] args)
{
    // Create a new document.
    var document = new Document();
    
    // Add some elements to the document.
    document.Elements.Add(new Text("This is plain text."));
    document.Elements.Add(new Hyperlink("Hyperlink to Airbrake.io", "http://airbrake.io"));
    document.Elements.Add(new Paragraph("Lorem ipsum dolor sit amet, consectetur adipiscing elit."));
    document.Elements.Add(new BoldText("Important text to bold!"));

    // Create a few visitors.
    var html = new HtmlVisitor();
    var markdown = new MarkdownVisitor();
    var bbCode = new BbVisitor();

    // Force document to accept passed visitors, 
    // which will each uniquely alter output.
    document.Accept(html);
    document.Accept(markdown);
    document.Accept(bbCode);

    // Log each visitor's output.
    Logging.LineSeparator("HTML");
    Logging.LineSeparator(html.ToString());

    Logging.LineSeparator("Markdown");
    Logging.LineSeparator(markdown.ToString());

    Logging.LineSeparator("BBCode");
    Logging.LineSeparator(bbCode.ToString());
}
```

The comments walk through what's happening, but we basically start with a new `Document` and add some elements to it.  We then create a few `Visitor` instances, and force the `document` instance to `Accept()` each of those, which, as we saw above, calls `Accept()` for each element in the `Document`.  Now, when we output the `Visitors` to the log, we should see each element in the `Document`, but properly formatted to match the formatting rules specified in each unique `Visitor` class:

```
----------------- HTML -----------------
<span>This is plain text.</span><a href="http://airbrake.io">Hyperlink to Airbrake.io</a><p>Lorem ipsum dolor sit amet, consectetur adipiscing elit.</p><b>Important text to bold!</b>

--------------- Markdown ---------------
This is plain text.[Hyperlink to Airbrake.io](http://airbrake.io)

Lorem ipsum dolor sit amet, consectetur adipiscing elit.

**Important text to bold!**

---------------- BBCode ----------------
This is plain text.[url=http://airbrake.io]Hyperlink to Airbrake.io[/url]

Lorem ipsum dolor sit amet, consectetur adipiscing elit.

[b]Important text to bold![/b]
```

Sure enough, that's exactly what we get!  HTML all runs together since it doesn't typically include linebreaks.  Markdown has proper formatting, including linebreaks around the `Paragraph` element.  BBCode has similar `Paragraph` linebreaks, but handles formatting of other types, like `BoldText` and `Hyperlinks`, quite differently.

---

There you have it!  Hopefully this article provided a bit of insight into the capabilities of the `visitor design pattern`, and gave you some basic tools you can use in your own future projects.  For more information on all the other popular design patterns, head on over to our [ongoing design pattern series here](https://airbrake.io/blog/software-design/software-design-patterns-guide)!

---

__META DESCRIPTION__

Part 20 of our Software Design Pattern series in which examine the visitor design pattern using fully-functional C# example code.
