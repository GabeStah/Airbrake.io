// <Basic>/Aggregate.cs
using System.Collections;

namespace Iterator.Basic
{
    /// <summary>
    /// Defines members of Aggregate.
    /// </summary>
    public interface IAggregate
    {
        IIterator CreateIterator();
    }

    /// <summary>
    /// Holds the iterable collection and allows retrieval of collection objects.
    /// </summary>
    public class Aggregate : IAggregate
    {
        /// <summary>
        /// Iterable collection.
        /// </summary>
        private readonly ArrayList _items = new ArrayList();

        /// <summary>
        /// Create and get a new Iterator instance of this collection.
        /// </summary>
        /// <returns>New Iterator instance.</returns>
        public IIterator CreateIterator()
        {
            return new Iterator(this);
        }

        /// <summary>
        /// Get a collection element by index.
        /// </summary>
        /// <param name="index">Index to retrieve.</param>
        /// <returns>Collection object.</returns>
        public object this[int index] => _items[index];

        /// <summary>
        /// Current collection count.
        /// </summary>
        public int Count => _items.Count;

        /// <summary>
        /// Adds the passed object to collection.
        /// </summary>
        /// <param name="o">Object to be added.</param>
        public void Add(object o)
        {
            _items.Add(o);
        }
    }
}