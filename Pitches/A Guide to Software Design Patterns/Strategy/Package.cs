// Package.cs
using System.Collections.Generic;

namespace Strategy
{
    /// <summary>
    /// Basic packing materials, which are used within packaging strategies.
    /// </summary>
    internal enum PackingMaterials
    {
        Box,
        BubbleWrap,
        DryIce,
        Envelope,
        Foam,
        LargeBox,
        Tape
    }

    /// <summary>
    /// Defines a basic package to be shipped.
    /// 
    /// Contains package content and packing materials used.
    /// </summary>
    internal class Package
    {
        internal string Content { get; set; }
        internal List<PackingMaterials> Packaging { get; set; } = new List<PackingMaterials>();

        internal Package(string content)
        {
            Content = content;
        }

        /// <summary>
        /// Outputs package Content and comma-delimited list of packing materials.
        /// </summary>
        /// <returns>Formatted string.</returns>
        public override string ToString()
        {
            return $"{Content} was packaged using {string.Join(", ", Packaging.ToArray())}.";
        }
    }
}