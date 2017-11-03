using System.Collections.Generic;

namespace Utility
{
    /// <summary>
    /// Generic Singleton class used to store a List of type T.
    /// </summary>
    /// <typeparam name="T">Type of objects to store.</typeparam>
    public sealed class Singleton<T>
    {
        /// <summary>
        /// Store the singleton instance of this.
        /// </summary>
        public static Singleton<T> Instance { get; } = new Singleton<T>();

        /// <summary>
        /// List of values.
        /// </summary>
        private List<T> Values { get; } = new List<T>();

        static Singleton() { }

        private Singleton() { }

        /// <summary>
        /// Add value to Values List.
        /// </summary>
        /// <param name="value">Value to add.</param>
        public void Add(T value)
        {
            Values.Add(value);
        }

        /// <summary>
        /// Get the current Values List.
        /// </summary>
        /// <returns>Current Values List.</returns>
        public List<T> GetValues()
        {
            return Values;
        }

        /// <summary>
        /// Remove last value and return tuple of index and value.
        /// </summary>
        /// <returns>Tuple of index and value that was removed.</returns>
        public (int Index, object Value) Pop()
        {
            if (!Values.IsAny()) return (-1, null);
            var index = Values.Count - 1;
            var value = Values[index];
            RemoveAt(index);
            return (index, value);
        }

        /// <summary>
        /// Remove value from Values List.
        /// </summary>
        /// <param name="value">Value to remove.</param>
        public void Remove(T value)
        {
            Values.Remove(value);
        }

        /// <summary>
        /// Remove value, via index, from Values List.
        /// </summary>
        /// <param name="index">Index to remove.</param>
        public void RemoveAt(int index)
        {
            Values.RemoveAt(index);
        }
    }
}