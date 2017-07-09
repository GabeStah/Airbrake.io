using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Utility;

namespace Airbrake.DllNotFoundException
{
    class Program
    {
        static void Main(string[] args)
        {
            var isNotepadRunning = Process.GetProcessesByName("notepad");
            // Check if no processes found.
            if (isNotepadRunning.Length == 0)
            {
                // Create new process for notepad.exe.
                var process = new Process { StartInfo = { FileName = "c:/windows/notepad.exe" } };
                // Attempt to start the process using provided executable path.
                process.Start();
                // Wait until notepad is ready for user input (otherwise we'll send key posts before it can handle them).
                process.WaitForInputIdle();
            }

            // Create new Screen instance for Notepad input window.
            var screen = new Screen("Notepad", "Untitled - Notepad", "Edit");

            // Enter "hello world";
            screen.PostKeyDown(Keys.H);
            screen.PostKeyDown(Keys.E);
            screen.PostKeyDown(Keys.L);
            screen.PostKeyDown(Keys.L);
            screen.PostKeyDown(Keys.O);
            screen.PostKeyDown(Keys.Space);
            screen.PostKeyDown(Keys.W);
            screen.PostKeyDown(Keys.O);
            screen.PostKeyDown(Keys.R);
            screen.PostKeyDown(Keys.L);
            screen.PostKeyDown(Keys.D);

            // New line.
            screen.PostKeyDown(Keys.Return);
        }
    }

    public class Screen
    {
        public IntPtr Handle { get; set; }

        public enum VirtualKeyCodes : int
        {
            KeyDown = 0x0100,
            KeyUp = 0x0101,
        }

        [DllImport("user23.dll", EntryPoint = "FindWindowEx")]
        public static extern IntPtr GetChildWindow(IntPtr parentWindowHandle, IntPtr childWindowHandle, string className, string windowName);

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        public static extern IntPtr GetWindow(string className, string windowName);

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hWindow);

        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr windowHandle, VirtualKeyCodes message, int lowParam, int highParam);

        [DllImport("user32.dll", EntryPoint = "ReleaseDC")]
        public static extern bool ReleaseContext(IntPtr windowHandle, IntPtr contextHandle);

        /// <summary>
        /// Instantiate by setting Handle to target child window.
        /// </summary>
        /// <param name="parentClassName">Parent window class name.</param>
        /// <param name="parentWindowName">Parent window caption name.</param>
        /// <param name="childClassName">Child window class name to find.</param>
        /// <param name="childWindowName">Child window caption name to find.</param>
        public Screen(string parentClassName, string parentWindowName, string childClassName = "", string childWindowName = "")
        {
            try
            {
                // Get parent window handle.
                var parentHandle = GetWindow(parentClassName, parentWindowName);
                // Get child window handle.
                Handle = GetChildWindow(parentHandle, (IntPtr) 0, childClassName, childWindowName);
            }
            catch (System.DllNotFoundException exception)
            {
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                Logging.Log(exception, false);
            }
        }

        /// <summary>
        /// Destructor to dispose obtained handle.
        /// </summary>
        ~Screen()
        {
            // Release window handle reference.
            ReleaseContext(Handle, GetWindowDC(Handle));
        }

        /// <summary>
        /// Post key press (down) to window.
        /// </summary>
        /// <param name="key">System.Windows.Input.Key to be pressed.</param>
        /// <returns>Indicates if message post was successful.</returns>
        public bool PostKeyDown(Keys key)
        {
            // Key down.
            return PostMessage(Handle, VirtualKeyCodes.KeyDown, (int)key, 0);
        }
    }
}
