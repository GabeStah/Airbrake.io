using Utility;

namespace Airbrake.ArgumentException
{
    class Program
    {
        static void Main(string[] args)
        {
            ValidExample();
            InvalidExample();
        }

        private static string GetFullName(string first, string last)
        {
            // Check if first value is null or has no characters.
            // Null must be checked prior to length to avoid checking an invalid object.
            if (first is null || first.Length == 0)
            {
                throw new System.ArgumentException($"GetFullName() parameter 'first' must be a valid string.");
            }
            // Check if last value is null or has no characters.
            // Null must be checked prior to length to avoid checking an invalid object.
            if (last is null || last.Length == 0)
            {
                throw new System.ArgumentException($"GetFullName() parameter 'last' must be a valid string.");
            }
            // Return full name.
            return $"{first} {last}";
        }

        private static void ValidExample()
        {
            try
            {
                // Get full name with two valid strings.
                var fullName = GetFullName("John", "Doe");
                // Output name result.
                Logging.Log($"Full name is: {fullName}.");
            }
            catch (System.ArgumentException e)
            {
                Logging.Log(e);
            }
        }

        private static void InvalidExample()
        {
            try
            {
                // Get full name with a valid first name, but an invalid last name.
                // Expectation is a thrown ArgumentException.
                var fullName = GetFullName("John", null);
                // Output name result.
                Logging.Log($"Full name is: {fullName}.");
            }
            catch (System.ArgumentException e)
            {
                Logging.Log(e);
            }
        }
    }
}
