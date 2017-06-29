using System.Diagnostics;
using Utility;

namespace Airbrake.ComponentModel.Win32Exception
{
    class Program
    {
        static void Main(string[] args)
        {
            StartProcessFromPath("c:/windows/notepad.exe");
            Logging.LineSeparator();
            StartProcessFromPath("c:/windows/invalid.exe");
        }

        static void StartProcessFromPath(string path)
        {
            try
            {
                // Create a new process with StartInfo.FileName set to provided path.
                var process = new Process { StartInfo = { FileName = path } };
                // Attempt to start the process using provided executable path.
                var success = process.Start();
                if (success)
                {
                    Logging.Log($"Successfully launched '{process.ProcessName.ToString()}' process!");
                    // Sleep for two seconds to allow time for window to be shown.
                    System.Threading.Thread.Sleep(2000);
                    // Kill process.
                    process.Kill();
                    Logging.Log($"Killed '{process.ProcessName.ToString()}' process.");
                }
                else
                {
                    // This code never executes since we're catching
                    // an exception from the process.Start() invocation line.
                }
            }
            catch (System.ComponentModel.Win32Exception exception)
            {
                // Indicate failure to start.
                Logging.Log($"Unable to start process with executable path: '{path}'.");
                // Output caught exception.
                Logging.Log(exception);
                Logging.Log($"Native Win32 Error Code: {exception.NativeErrorCode}");
            }
        }
    }
}
