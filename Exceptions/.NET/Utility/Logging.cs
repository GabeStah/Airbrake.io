using System;
using System.Diagnostics;

namespace Utility
{
    /// <summary>
    /// Houses all logging methods for various debug outputs.
    /// </summary>
    public static class Logging
    {
        /// <summary>
        /// Outputs to <see cref="System.Diagnostics.Debug.WriteLine"/> if DEBUG mode is enabled,
        /// otherwise uses standard <see cref="Console.WriteLine"/>.
        /// </summary>
        /// <param name="value">Value to be output to log.</param>
        public static void Log(object value)
        {
            #if DEBUG
                Debug.WriteLine(value);
            #else
                Console.WriteLine(value);
            #endif
        }

        /// <summary>
        /// Uses format string to insert arg into output.
        /// </summary>
        /// <param name="format">Format string.</param>
        /// <param name="arg0">Argument to insert into format string.</param>
        public static void Log(string format, object arg0)
        {
            #if DEBUG
                Debug.WriteLine(format, arg0);
            #else
                Console.WriteLine(format, arg0);
            #endif
        }

        /// <summary>
        /// Uses format string to insert args into output.
        /// </summary>
        /// <param name="format">Format string.</param>
        /// <param name="arg0">Argument to insert into format string.</param>
        /// <param name="arg1">Argument to insert into format string.</param>
        public static void Log(string format, object arg0, object arg1)
        {
            #if DEBUG
                Debug.WriteLine(format, arg0, arg1);
            #else
                Console.WriteLine(format, arg0, arg1);
            #endif
        }

        /// <summary>
        /// Uses format string to insert args into output.
        /// </summary>
        /// <param name="format">Format string.</param>
        /// <param name="arg0">Argument to insert into format string.</param>
        /// <param name="arg1">Argument to insert into format string.</param>
        /// <param name="arg2">Argument to insert into format string.</param>
        public static void Log(string format, object arg0, object arg1, object arg2)
        {
            #if DEBUG
                Debug.WriteLine(format, arg0, arg1, arg2);
            #else
                Console.WriteLine(format, arg0, arg1, arg2);
            #endif
        }

        /// <summary>
        /// Uses format string to insert arg(s) into output.
        /// </summary>
        /// <param name="format">Format string.</param>
        /// <param name="arg0">Argument to insert into format string.</param>
        /// <param name="arg1">Argument to insert into format string.</param>
        /// <param name="arg2">Argument to insert into format string.</param>
        /// <param name="arg3">Argument to insert into format string.</param>
        public static void Log(string format, object arg0, object arg1, object arg2, object arg3)
        {
            #if DEBUG
                Debug.WriteLine(format, arg0, arg1, arg2, arg3);
            #else
                Console.WriteLine(format, arg0, arg1, arg2, arg3);
            #endif
        }

        /// <summary>
        /// When <see cref="Exception"/> parameter is passed, modifies the output to indicate
        /// if <see cref="Exception"/> was expected, based on passed in `expected` parameter.
        /// <para>Outputs the full <see cref="Exception"/> type and message.</para>
        /// </summary>
        /// <param name="exception">The <see cref="Exception"/> to output.</param>
        /// <param name="expected">Boolean indicating if <see cref="Exception"/> was expected.</param>
        public static void Log(Exception exception, bool expected = true)
        {
            string value = $"[{(expected ? "EXPECTED" : "UNEXPECTED")}] {exception.ToString()}: {exception.Message}";
            #if DEBUG
                Debug.WriteLine(value);
            #else
                Console.WriteLine(value);
            #endif
        }
    }
}

