// Extensions.cs
using System;

namespace Airbrake.Management.ManagementException
{
    public static class Extensions
    {
        private static readonly string[] StorageSuffixes =
        {
            "B",
            "KB",
            "MB",
            "GB",
            "TB",
            "PB",
            "EB",
            "ZB",
            "YB"
        };

        /// <summary>
        /// Converts ulong value of total bytes into formatted string, 
        /// representing the number of converted KB, MB, GB, etc.
        /// </summary>
        /// <param name="value">Total number of bytes.</param>
        /// <returns>Formatted string indicating largest order of magnitudinal measurement.</returns>
        public static string ToStorageString(this ulong value)
        {
            var i = 0;
            var decimalValue = (decimal)value;
            while (Math.Round(decimalValue) >= 1000)
            {
                decimalValue /= 1024;
                i++;
            }

            return string.Format($"{decimalValue:n2} {StorageSuffixes[i]}");
        }
    }
}