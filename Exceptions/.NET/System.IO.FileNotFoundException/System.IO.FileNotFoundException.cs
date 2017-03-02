using System;
using System.IO;

namespace ConsoleApplication
{
    public class FileNotFoundExceptionExample
    {

        public static void Main(string[] args)
        {
            WriteLineToFile(@"names.txt", "Jane Doe");
            ReadLineFromFile(@"names.txt");
            ReadLineFromFile(@"invalid.txt");
        }

        private static void ReadLineFromFile(string fileName)
        {
            FileStream fs = null;
            string line = null;
            
            try   
            {
                // Opening file stream
                fs = new FileStream(fileName, FileMode.OpenOrCreate);
                using (StreamReader reader = new StreamReader(fs))
                {
                    // Read first line
                    line = reader.ReadLine();
                    Console.WriteLine($"Reading first line: {line}");
                }
            }
            catch(FileNotFoundException exception)
            {
                LogException(exception);
            }
        }

        private static void WriteLineToFile(string fileName, string line)
        {
            FileStream fs = null;
            
            try   
            {
                // Opening file stream
                fs = new FileStream(fileName, FileMode.Append);
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    // Write line
                    writer.WriteLine(line);
                    Console.WriteLine($"Writing new line: {line}");
                }
            }
            catch(FileNotFoundException exception)
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
