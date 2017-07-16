using System;
using System.Linq;
using Utility;

namespace Airbrake.FormatException
{
    class Program
    {
        static void Main(string[] args)
        {
            // Convert name string to character.
            var name = "John";
            ConvertStringToChar(name);
            // Convert first character of name to character.
            name = name.First().ToString();
            ConvertStringToChar(name);

            Logging.LineSeparator();

            // Convert string to boolean.
            var booleanString = "true";
            ConvertStringToBoolean(booleanString);
            // Convert invalid string to boolean.
            booleanString = "truthy";
            ConvertStringToBoolean(booleanString);

            Logging.LineSeparator();

            // Format decimal using currency format string.
            var value = 49.95m;
            var format = "{0:C2}";
            Logging.Log($"Formatted decimal: {value} via format: {format} into result: {FormatDecimal(value, format)}");
            // Format decimal using non-standard format string.
            format = "{0:Z2}";
            Logging.Log($"Formatted decimal: {value} via format: {format} into result: {FormatDecimal(value, format)}");
        }

        /// <summary>
        /// Converts a string to a character.
        /// </summary>
        /// <param name="value">String value to be converted.</param>
        /// <returns>Converted character result.</returns>
        static char? ConvertStringToChar(string value)
        {
            try
            {
                // Convert string to character.
                var result = Convert.ToChar(value);
                // Log conversion result.
                Logging.Log($"Converted string: {value} to character: {result}.");
                // Return result.
                return result;
            }
            catch (System.FormatException exception)
            {
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                Logging.Log(exception, false);
            }
            return null;
        }

        /// <summary>
        /// Converts a string to a boolean.
        /// </summary>
        /// <param name="value">String value to be converted.</param>
        /// <returns>Converted boolean result.</returns>
        static bool? ConvertStringToBoolean(string value)
        {
            try
            {
                // Convert string to boolean.
                var result = Convert.ToBoolean(value);
                // Log conversion result.
                Logging.Log($"Converted string: {value} to boolean: {result}.");
                // Return result.
                return result;
            }
            catch (System.FormatException exception)
            {
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                Logging.Log(exception, false);
            }
            return null;
        }

        /// <summary>
        /// Format a decimal value using provided format string.
        /// </summary>
        /// <param name="value">Decimal value to format.</param>
        /// <param name="format">Format string to use.</param>
        /// <returns>Resulting formatted string.</returns>
        static string FormatDecimal(decimal value, string format)
        {
            try
            {
                // Directly format passed value.
                return String.Format($"{format}.", value);
            }
            catch (System.FormatException exception)
            {
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                Logging.Log(exception, false);
            }
            return null;
        }
    }
}
