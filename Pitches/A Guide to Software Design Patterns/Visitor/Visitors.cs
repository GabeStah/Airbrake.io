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
