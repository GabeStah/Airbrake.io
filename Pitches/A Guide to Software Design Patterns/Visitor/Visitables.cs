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