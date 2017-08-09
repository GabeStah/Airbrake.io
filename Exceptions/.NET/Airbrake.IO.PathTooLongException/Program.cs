using System;
using System.IO;
using System.Reflection;
using Utility;

namespace Airbrake.IO.PathTooLongException
{
    class Program
    {
        public static string CurrentPath => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private static void Main(string[] args)
        {
            // Creating file with path length of 1.
            CreateFileByPathLength(1);
            // Creating file with path length of 90.
            CreateFileByPathLength(90, 'b');
            // Creating file with path length of 255.
            CreateFileByPathLength(255, 'c');
            // Creating file with path length of 259.
            CreateFileByPathLength(259, 'd');
            // Creating file with path length of 260.
            CreateFileByPathLength(260, 'e');
            // Create file with name length of 32766 (Int16 max value)
            CreateFileByPathLength(short.MaxValue - 1, 'f');
            // Create file with name length of 32767 (Int16 max value)
            CreateFileByPathLength(short.MaxValue, 'f');
            // Create file with name length of 32768.
            CreateFileByPathLength(short.MaxValue + 1, 'g');
        }

        /// <summary>
        /// Create file by passed name.
        /// </summary>
        /// <param name="name">Name of file.</param>
        private static void CreateFileByName(string name)
        {
            try
            {
                // Output shortened file name and actual length.
                Logging.Log($"Creating file: {name.Shorten(20)} [Name length: {name.Length}, Full path length: {Path.GetFullPath(name).Length}]");
                // Try to create file.
                var file = File.Create(name);
                // Output success message.
                Logging.Log($"{file.Name.Shorten(20)} successfully created.");
            }
            catch (System.IO.PathTooLongException exception)
            {
                // Catch expected PathTooLongExceptions.
                Logging.Log(exception);
            }
            catch (IOException exception)
            {
                // Catch expected IOExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Catch unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }

        /// <summary>
        /// Create file by specified total path length,
        /// by repeating passed character parameter for file name.
        /// </summary>
        /// <param name="length">Total path length of created file.</param>
        /// <param name="character">Character to repeat for file name.</param>
        private static void CreateFileByPathLength(int length, char character = 'a')
        {
            try
            {
                const string extension = ".txt";

                // Check if current path plus extension length is larger than total length.
                if (CurrentPath.Length + extension.Length - 1 >= length)
                {
                    Logging.Log($"Length of {length} is less than base path length, aborting.");
                    return;
                }

                // Create full path.
                // Manually creating path, rather than calling Path.GetFullPath(),
                // to avoid potential exceptions before log output can be generated.
                var path =
                    $"{CurrentPath}\\{new string(character, length - CurrentPath.Length - extension.Length - 1)}{extension}";

                // Output shortened file name and actual length.
                Logging.Log($"Attempting to create file: {path.Shorten()} [Actual length: {path.Length}]");
                // Try to create file.
                var file = File.Create(path);
                // Output success message.
                Logging.Log($"{path.Shorten()} successfully created.");
            }
            catch (System.IO.PathTooLongException exception)
            {
                // Catch expected PathTooLongExceptions.
                Logging.Log(exception);
            }
            catch (IOException exception)
            {
                // Catch expected IOExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Catch unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }
    }
}
