// <Activities>/LoggerActivity.cs
using System;
using System.Activities;

namespace Utility.Activities
{
    /// <summary>
    /// Activity used for logging messages to console.
    /// </summary>
    public class LoggerActivity : CodeActivity
    {
        /// <summary>
        /// Argument for Exception output.
        /// </summary>
        public InArgument<Exception> Exception { get; set; }

        /// <summary>
        /// Determines if output is a line separator.
        /// </summary>
        public InArgument<bool> IsLineSeparator { get; set; }

        /// <summary>
        /// Argument for String output.
        /// </summary>
        public InArgument<string> String { get; set; }

        /// <inheritdoc />
        protected override void Execute(CodeActivityContext context)
        {
            // Output Exception.
            if (context.GetValue(Exception) != null)
            {
                Logging.Log(context.GetValue(Exception));
            }

            if (context.GetValue(String) == null) return;
            // Check if LineSeparator is set.
            if (context.GetValue(IsLineSeparator))
            {
                // Output String as LineSeparator insert.
                Logging.LineSeparator(context.GetValue(String));
            }
            else
            {
                // Output String.
                Logging.Log(context.GetValue(String));
            }
        }
    }
}
