using System;
using System.Collections.Generic;
using System.Linq;

namespace Utility
{
    /// <summary>
    /// All collection and enumerable methods.
    /// </summary>
    public static class Collections
    {
        /// <summary>
        /// Convert Enumerable in hierarchy format to Enumerable collection.
        /// </summary>
        /// <typeparam name="T">Originating source collection type.</typeparam>
        /// <param name="source">Originating source collection type.</param>
        /// <param name="nextItem">Function to retrieve next item in collection.</param>
        /// <param name="canContinue">Boolean function indicating if next item exists.</param>
        /// <returns>The collection from a hierarchical format.</returns>
        public static IEnumerable<T> FromHierarchy<T>(
            this T source,
            Func<T, T> nextItem,
            Func<T, bool> canContinue)
        {
            for (var current = source; canContinue(current); current = nextItem(current))
            {
                yield return current;
            }
        }

        /// <summary>
        /// Recursively enumerates over hierarchy to get collection.
        /// </summary>
        /// <typeparam name="T">Originating source collection type.</typeparam>
        /// <param name="source">Originating source collection type.</param>
        /// <param name="nextItem">Function to retrieve next item in collection.</param>
        /// <returns>Single yielded enumerable object.</returns>
        public static IEnumerable<T> FromHierarchy<T>(
            this T source,
            Func<T, T> nextItem)
            where T : class
        {
            return FromHierarchy(source, nextItem, s => s != null);
        }

        /// <summary>
        /// Check if enumerable has any data.
        /// </summary>
        /// <typeparam name="T">Type of data collection.</typeparam>
        /// <param name="data">Enumerable data collection.</param>
        /// <returns>Boolean indicating if collection contains data.</returns>
        public static bool IsAny<T>(this IEnumerable<T> data)
        {
            return data != null && data.Any();
        }
    }
}
