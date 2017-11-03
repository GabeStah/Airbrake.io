using System;

namespace Utility
{
    /// <summary>
    /// Basic String extensions.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Shortens the string by concatenating beginning and ending portions equally.
        /// Combines both ends with series of delimiters.
        /// </summary>
        /// <param name="string">String to shorten.</param>
        /// <param name="maxLength">Maximum total length of string plus delimeter.</param>
        /// <param name="delimiter">Delimiter character to separate ends.</param>
        /// <param name="delimiterLength">Length of delimeter string.</param>
        /// <returns>Shortened string.</returns>
        public static string Shorten(this string @string, int maxLength = 30, char delimiter = '.', int delimiterLength = 3)
        {
            // If string is already short enough, no change.
            if (@string.Length <= maxLength)
            {
                return @string;
            }

            // Adjust totalLength remaining for string minus delimiter length.
            var totalLength = maxLength - delimiterLength;
            // Halve totalLength and floor for left side.
            var leftLength = (int) Math.Floor((decimal) totalLength / 2);
            var rightLength = leftLength;
            // If odd, add remainder to right.
            if (totalLength % 2 != 0) rightLength += 1;

            // Create splits from left and right length values.
            var left = @string.Substring(0, leftLength);
            var right = @string.Substring(@string.Length - rightLength);

            // Return concatenated result.
            return $"{left}{new string(delimiter, delimiterLength)}{right}";
        }
    }

    
}
