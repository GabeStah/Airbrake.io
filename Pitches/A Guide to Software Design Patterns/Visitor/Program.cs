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
