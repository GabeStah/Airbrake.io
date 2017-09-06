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