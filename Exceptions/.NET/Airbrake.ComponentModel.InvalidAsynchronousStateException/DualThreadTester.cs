using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using Utility;

namespace Airbrake.ComponentModel.InvalidAsynchronousStateException
{
    internal class DualThreadTester
    {
        private Form _backgroundForm;

        public DualThreadTester()
        {
            // Create foreground and background threads.
            var foreground = new Thread(IterationTest)
            {
                Name = "Foreground",
                IsBackground = false
            };
            var background = new Thread(IterationTest)
            {
                Name = "Background",
                IsBackground = true
            };

            // Start both threads nearly simultaneously.
            foreground.Start();
            background.Start();

        }

        /// <summary>
        /// Delegate to use with Background thread.
        /// </summary>
        /// <param name="message">Message to be output to log.</param>
        public delegate void BackgroundThreadDelegate(string message);

        /// <summary>
        /// Output passed message to log.
        /// </summary>
        /// <param name="message">Message to output.</param>
        public void BackgroundThreadDelegateMethod(string message)
        {
            Logging.Log(message);
        }

        /// <summary>
        /// Performs basic iteration test for currently active thread.
        /// </summary>
        public void IterationTest()
        {
            try
            {
                // Check if current thread is background.
                if (Thread.CurrentThread.IsBackground)
                {
                    // Create and show BackgroundForm.
                    _backgroundForm = new BackgroundForm();
                    _backgroundForm.Show();
                }
                // Loop a few times.
                for (var count = 0; count < 10; count++)
                {
                    // Check if thread is foreground.
                    if (!Thread.CurrentThread.IsBackground)
                    {
                        // Confirm BackgroundForm exists.
                        if (_backgroundForm != null)
                        {
                            // Confirm that invocation is required.
                            if (_backgroundForm.InvokeRequired)
                            {
                                // Invoke BackgroundThreadDelegateMethod with current count argument.
                                _backgroundForm.Invoke(new BackgroundThreadDelegate(BackgroundThreadDelegateMethod),
                                    count);
                            }
                            else
                            {
                                // If no invocation required, directly set color.
                                _backgroundForm.BackColor = Color.Red;
                            }
                        }
                    }
                    Logging.Log($"{Thread.CurrentThread.Name} thread count: {count}");
                    Thread.Sleep(250);
                }
                Logging.Log($"{Thread.CurrentThread.Name} finished.");
            }
            catch (System.ComponentModel.InvalidAsynchronousStateException exception)
            {
                // Output expected InvalidAsynchronousStateExceptions.
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