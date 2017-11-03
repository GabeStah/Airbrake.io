using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Utility
{
    public static class Extensions
    {
        public enum TypeProperty
        {
            Name,
            FullName
        }

        /// <summary>
        /// Provides iterable hierarchy collection of types up to System.Object/basest type.
        /// </summary>
        /// <param name="type">Type to analyze.</param>
        /// <returns>Next parent/base Type.</returns>
        public static IEnumerable<Type> GetInheritanceHierarchy(this Type type)
        {
            for (var current = type; current != null; current = current.BaseType)
                yield return current;
        }

        /// <summary>
        /// Output Type hierarchy to console.
        /// </summary>
        /// <param name="type">Type to output.</param>
        /// <param name="propertyType">Property to output (default: FullName)</param>
        public static void LogInheritanceHierarchy(this Type type, Extensions.TypeProperty propertyType = Extensions.TypeProperty.FullName)
        {
            foreach (var item in type.GetInheritanceHierarchy())
            {
                var property = item.GetType().GetProperty(propertyType.ToString(), BindingFlags.Instance |
                                                                                   BindingFlags.Public |
                                                                                   BindingFlags.NonPublic);
                Logging.Log(property.GetValue(item).ToString());
            }
        }
    }
}
