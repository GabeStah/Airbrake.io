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