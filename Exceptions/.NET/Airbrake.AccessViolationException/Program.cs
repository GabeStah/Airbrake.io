using System;
using System.Runtime.ExceptionServices;
using System.Runtime.InteropServices;
using System.Security;
using Utility;

namespace Airbrake.AccessViolationException
{
    class Program
    {       
        static void Main(string[] args)
        {
            //ReferenceTest();
            ReferenceTestWithHandler();
        }

        /// <summary>
        /// Create a reference from unmanaged FailingApp.dll C++ code.
        /// </summary>
        /// <returns>Reference.</returns>
        [DllImport(@"D:\work\Airbrake.io\Exceptions\.NET\Debug\FailingApp.dll")]
        private static extern int CreateReference();

        /// <summary>
        /// Test reference creation through unmanaged code.
        /// </summary>
        /// <returns>Reference result.</returns>
        public static int ReferenceTest()
        {
            try
            {
                // Attempt to create a reference through unmanaged code (C++ DLL).
                var result = CreateReference();
                // If no exception occurred, output successful result.
                Logging.Log($"Reference successfully created at: {result}.");
                // Return result.
                return result;
            }
            catch (System.AccessViolationException exception)
            {
                // Output explicit exception.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Output inexplicit exception.
                Logging.Log(exception, false);
            }
            // Return zero to indicate failure.
            return 0;
        }

        /// <summary>
        /// Test reference creation through unmanaged code.
        /// HandleProcessCorruptedStateExceptions attribute allows CLR
        /// to catch normally ignored exceptions due to unmanaged code.
        /// </summary>
        /// <returns>Reference result.</returns>
        [HandleProcessCorruptedStateExceptions]
        public static int ReferenceTestWithHandler()
        {
            try
            {
                // Attempt to create a reference through unmanaged code (C++ DLL).
                var result = CreateReference();
                // If no exception occurred, output successful result.
                Logging.Log($"Reference successfully created at: {result}.");
                // Return result.
                return result;
            }
            catch (System.AccessViolationException exception)
            {
                // Output explicit exception.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Output inexplicit exception.
                Logging.Log(exception, false);
            }
            // Return zero to indicate failure.
            return 0;
        }
    }
}
