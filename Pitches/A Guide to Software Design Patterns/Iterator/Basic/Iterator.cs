// <Basic>/Iterator.cs
namespace Iterator.Basic
{
    /// <summary>
    /// Defines members of Iterator.
    /// </summary>
    public interface IIterator
    {
        object Current { get; }
        bool Next();
    }

    /// <summary>
    /// Handles iteration logic for passed Aggregate.
    /// </summary>
    public class Iterator : IIterator
    {
        private readonly Aggregate _aggregate;
        private int _index = -1;

        public Iterator(Aggregate aggregate)
        {
            _aggregate = aggregate;
        }

        /// <summary>
        /// Get the Aggregate collection element of current index, otherwise null.
        /// </summary>
        public object Current => _index < _aggregate.Count ? _aggregate[_index] : null;

        /// <summary>
        /// Iterate the index count.
        /// </summary>
        /// <returns>Indicates if index is within bounds of collection indices.</returns>
        public bool Next()
        {
            _index++;
            return _index < _aggregate.Count;
        }
    }
}