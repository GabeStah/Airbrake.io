// <Advanced>/Iterator.cs
using System.Collections;
using System.Collections.Generic;

namespace Iterator.Advanced
{
    /// <summary>
    /// Generic iterator.
    /// </summary>
    /// <typeparam name="T">Any object type to be enumerated.</typeparam>
    public class Iterator<T> : IEnumerable<T>
    {
        /// <summary>
        /// Value collection list to be iterated.
        /// </summary>
        public List<T> Values = new List<T>();

        /// <summary>
        /// Gets the current value count.
        /// </summary>
        public int Count => Values.Count;

        /// <summary>
        /// Yields enumerated object from collection.
        /// </summary>
        /// <returns>Next iterated object.</returns>
        public IEnumerator<T> GetEnumerator()
        {
            for (var index = Count - 1; index >= 0; index--)
            {
                yield return Values[index];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
