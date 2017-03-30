using System;
using System.Reflection;
using Utility;

namespace Airbrake.BadImageFormatException
{
    class Program
    {
        static void Main(string[] args)
        {
            LoadingNonDotNetLibraryExample();
            Logging.Log("-----------------");
            DifferingCPUExample();
            Logging.Log("-----------------");
            OldDotNetExample();
        }

        private static void LoadingNonDotNetLibraryExample()
        {
            try
            {
                // Generate path to notepad.exe.
                string filePath = Environment.ExpandEnvironmentVariables("%windir%") + @"\System32\notepad.exe";
                Assembly assem = Assembly.LoadFile(filePath);
            }
            catch (System.BadImageFormatException exception)
            {
                Logging.Log(exception);
            }
        }

        private static void DifferingCPUExample()
        {
            try
            {
                // Generate path to Utility.dll, a 64-bit assembly.
                Assembly assem = Assembly.LoadFrom(@".\Utility.dll");
                Logging.Log(assem.ToString());
            }
            catch (System.BadImageFormatException exception)
            {
                Logging.Log(exception);
            }
        }

        private static void OldDotNetExample()
        {
            try
            {
                // Load Author-1.1.dll (compiled in .NET 1.1).
                Assembly assem = Assembly.LoadFrom(@".\Author-1.1.dll");
                Logging.Log(assem.ToString());
            }
            catch (System.BadImageFormatException exception)
            {
                Logging.Log(exception);
            }
        }
    }
}
