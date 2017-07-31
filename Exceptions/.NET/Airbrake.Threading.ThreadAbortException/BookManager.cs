using System;
using System.Threading;
using Utility;

namespace Airbrake.Threading.ThreadAbortException
{
    internal class BookManager
    {
        public Singleton<Book> Singleton = Singleton<Book>.Instance;

        /// <summary>
        /// Destroy all Books in Singleton and output each.
        /// </summary>
        /// <param name="delay">Delay between destruction.</param>
        internal void DestroyBooks(int delay = 1000)
        {
            try
            {
                // Check if any values remain.
                while (Singleton.GetValues().IsAny())
                {
                    // Delay processing.
                    Thread.Sleep(delay);
                    // Pop (remove) value and output info.
                    Logging.Log($"Book has been destroyed: {Singleton.Pop().Value}, on {Thread.CurrentThread.Name} thread.", Logging.OutputType.Timestamp);
                }
            }
            catch (System.Threading.ThreadAbortException exception)
            {
                // Output expected ThreadAbortException.
                Logging.Log(exception, true, Logging.OutputType.Timestamp);
            }
            catch (Exception exception)
            {
                // Output unexpected Exceptions.
                Logging.Log(exception, false, Logging.OutputType.Timestamp);
            }
        }

        /// <summary>
        /// Count all Books in Singleton collection during loop.
        /// </summary>
        /// <param name="iterations">Number of iteration loops to perform count.</param>
        /// <param name="delay">Delay between counts.</param>
        internal void CountBooks(int iterations = 5, int delay = 900)
        {
            try
            {
                // Loop once per iteration.
                for (var i = 0; i < iterations; i++)
                {
                    // Delay processing.
                    Thread.Sleep(delay);
                    // Count books and output.
                    Logging.Log($"Book count: {Singleton.GetValues().Count}, on {Thread.CurrentThread.Name} thread.", Logging.OutputType.Timestamp);
                }
            }
            catch (System.Threading.ThreadAbortException exception)
            {
                // Output expected ThreadAbortException.
                Logging.Log(exception, true, Logging.OutputType.Timestamp);
            }
            catch (Exception exception)
            {
                // Output unexpected Exceptions.
                Logging.Log(exception, false, Logging.OutputType.Timestamp);
            }
        }
    }
}