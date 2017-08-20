// <Airbrake.OverflowException>/Program.cs
using System;
using System.Reflection;
using Utility;

namespace Airbrake.OverflowException
{
    class Program
    {
        static void Main(string[] args)
        {
            // Multiple one billion by three in checked context.
            Logging.LineSeparator("CHECKED MULTIPLICATION");
            MultiplyNumbers(1_000_000_000, 3);

            // Multiple one billion by three in unchecked context.
            Logging.LineSeparator("UNCHECKED MULTIPLICATION");
            MultiplyNumbers(1_000_000_000, 3, false);

            const int value = 200;
            // Convert 200 to SByte in checked context.
            Logging.LineSeparator("CHECKED TYPE CONVERSION");
            ConvertValueToType<sbyte>(value);

            // Convert 200 to SByte in unchecked context.
            Logging.LineSeparator("UNCHECKED TYPE CONVERSION");
            ConvertValueToType<sbyte>(value, false);

            // Convert 200 directly, without Convert.ChangeType().
            Logging.LineSeparator("DIRECT UNCHECKED TYPE CONVERSION");
            unchecked
            {
                var result = (sbyte) value;
                // Output result.
                Logging.Log($"[UNCHECKED] {value:n0} converted to type {result.GetType().Name}: {result:n0}.");
            }
        }

        /// <summary>
        /// Attempts to multiple two passed integer values together.
        /// </summary>
        /// <param name="a">First value.</param>
        /// <param name="b">Second value.</param>
        /// <param name="checked">Determines if 'checked' context is used for multiplication attempt.</param>
        internal static void MultiplyNumbers(int a, int b, bool @checked = true)
        {
            try
            {
                int result;
                if (@checked)
                {
                    // Enable overflow checking.
                    checked
                    {
                        // Multiple numbers.
                        result = a * b;
                    }
                }
                // Disable overflow checking.
                unchecked
                {
                    // Multiple numbers.
                    result = a * b;
                }
                // Output result.
                Logging.Log($"[{(@checked ? "CHECKED" : "UNCHECKED")}] {a:n0} * {b:n0} = {result:n0}");
            }
            catch (System.OverflowException exception)
            {
                // Catch expected OverflowExceptions.
                Logging.Log(exception);
                Logging.Log($"[{(@checked ? "CHECKED" : "UNCHECKED")}] {a:n0} * {b:n0} exceeds int.MaxValue: {int.MaxValue:n0}");
            }
            catch (Exception exception)
            {
                // Catch unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }

        /// <summary>
        /// Attempts to convert passed value to specified generic type T.
        /// </summary>
        /// <typeparam name="T">Generic type to convert to.</typeparam>
        /// <param name="value">Value to be converted.</param>
        /// <param name="checked">Determines if 'checked' context is used for multiplication attempt.</param>
        internal static void ConvertValueToType<T>(int value, bool @checked = true)
        {
            try
            {
                object result;
                if (@checked)
                {
                    // Enable overflow checking.
                    checked
                    {
                        // Convert to type T.
                        result = (T) Convert.ChangeType(value, typeof(T));
                        
                    }
                }
                // Disable overflow checking.
                unchecked
                {
                    // Convert to type T.
                    result = (T) Convert.ChangeType(value, typeof(T));
                }
                // Output result.
                Logging.Log($"[{(@checked ? "CHECKED" : "UNCHECKED")}] {value:n0} converted to type {result.GetType().Name}: {result:n0}.");
            }
            catch (System.OverflowException exception)
            {
                // Catch expected OverflowExceptions.
                Logging.Log(exception);
                // Since this is a generic type, need to use reflection to get the MaxValue field, if applicable.
                var maxValueField = typeof(T).GetField("MaxValue", BindingFlags.Public
                                                                         | BindingFlags.Static);
                if (maxValueField == null)
                {
                    throw new NotSupportedException(typeof(T).Name);
                }
                var maxValue = (T) maxValueField.GetValue(null);

                Logging.Log($"[{(@checked ? "CHECKED" : "UNCHECKED")}] {value:n0} cannot be converted to type {typeof(T).Name} because it exceeds {typeof(T).Name}.MaxValue: {maxValue:n0}");

                //Logging.Log($"[{(@checked ? "CHECKED" : "UNCHECKED")}] {value:n0} cannot be converted to type {typeof(T).Name}.");
            }
            catch (Exception exception)
            {
                // Catch unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }
    }
}
