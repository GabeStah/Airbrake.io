using System;
using System.Text.RegularExpressions;
using Utility;

namespace Airbrake.Text.RegularExpressions.RegexMatchTimeoutException
{
    class Program
    {
        public static void Main()
        {
            // Test a series of input lengths and timeout durations.

            // 100 nanosecond timeout.
            RegexTest(new string('a', 1), "(a)+", new TimeSpan(1));
            RegexTest(new string('a', 1_000), "(a)+", new TimeSpan(1));
            RegexTest(new string('a', 1_000_000), "(a)+", Regex.InfiniteMatchTimeout);

            // 1 second timeout.
            RegexTest(new string('a', 1), "(a)+", new TimeSpan(0, 0, 1));
            RegexTest(new string('a', 1_000), "(a)+", new TimeSpan(0, 0, 1));
            RegexTest(new string('a', 1_000_000), "(a)+", new TimeSpan(0, 0, 1));
            RegexTest(new string('a', 1_000_000_000), "(a)+", new TimeSpan(0, 0, 1));

            RegexTest(new string('a', 1_000_000_000), "(a)+", Regex.InfiniteMatchTimeout);
        }

        internal static void RegexTest(string input, 
            string pattern, 
            TimeSpan timeout, 
            RegexOptions options = RegexOptions.IgnoreCase)
        {
            try
            {
                Logging.LineSeparator($"LENGTH: {input.Length:#,#}, PATTERN: {pattern}, TIMEOUT: {timeout}", 75);
                // Get match.
                var match = Regex.Match(input, pattern, options, timeout);
                // Output successful match data.
                Logging.Log($"Match: {match.ToString().Shorten()}");
            }
            catch (System.Text.RegularExpressions.RegexMatchTimeoutException exception)
            {
                // Output expected RegexMatchTimeoutExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Output unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }
    }
}
