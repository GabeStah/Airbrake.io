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