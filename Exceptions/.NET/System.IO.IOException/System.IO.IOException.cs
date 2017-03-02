using System;
using System.IO;

namespace ConsoleApplication
{
    public class IOExceptionExample
    {

        public static void Main(string[] args)
        {
            try
            {
                // Specify our directory
                string dir = @"g:\dev\missing";

                // Set the current directory
                Directory.SetCurrentDirectory(dir);
            }
            catch (IOException exception)
            {
                LogException(exception);
            }
        }

        private static void LogException(Exception exception, bool expected = true)
        {
            Console.WriteLine($"[{(expected ? "EXPECTED" : "UNEXPECTED")}] {exception.ToString()}: {exception.Message}");
        }
    }
}
