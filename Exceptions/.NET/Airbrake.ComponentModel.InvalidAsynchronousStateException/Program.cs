using System;
using System.Windows.Forms;

namespace Airbrake.ComponentModel.InvalidAsynchronousStateException
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Instantiate DualThreadTester.
            new DualThreadTester();

            // Create form.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
