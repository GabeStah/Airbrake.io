using System.Threading;
using Utility;

namespace Airbrake.Threading.ThreadAbortException
{
    class Program
    {
        public static void Main()
        {
            // Begin stopwatch.
            StopwatchProxy.Instance.Stopwatch.Start();

            // Set name of main thread.
            Thread.CurrentThread.Name = "Main";

            // Instantiate basic thread tests.
            var basicThreadTester = new BasicThreadTester();

            Logging.LineSeparator();
            Logging.LineSeparator();

            // Restart stopwatch.
            StopwatchProxy.Instance.Stopwatch.Restart();

            // Instantiate advanced thread tests.
            var advancedThreadTester = new AdvancedThreadTester();
        }
    }

    internal class BasicThreadTester
    {
        internal BasicThreadTester()
        {
            // Create secondary thread and set name.
            var thread = new Thread(PerformSuspension)
                { Name = "Secondary" };
            
            // Start thread.
            thread.Start();
            Logging.Log(thread.ThreadState, Logging.OutputType.Timestamp);

            // Sleep one millisecond so process can begin.
            Thread.Sleep(1);

            // Abort thread.
            thread.Abort();
            Logging.Log(thread.ThreadState, Logging.OutputType.Timestamp);

            // Join new thread with main thread.
            thread.Join();
            Logging.Log($"Joining {Thread.CurrentThread.Name} and {thread.Name} threads.", Logging.OutputType.Timestamp);
        }

        internal void PerformSuspension()
        {
            try
            {
                Logging.Log($"{Thread.CurrentThread.Name} thread started.", Logging.OutputType.Timestamp);
            }
            finally
            {
                // Delay for one second after abort.
                Thread.Sleep(1000);
            }
        }
    }

    internal class AdvancedThreadTester
    {
        internal AdvancedThreadTester()
        {
            // Instantiate thread manager.
            var bookManager = new BookManager();

            // Add Books to Singleton instance List.
            bookManager.Singleton.Add(new Book("Magician", "Raymond E. Feist", 681));
            bookManager.Singleton.Add(new Book("The Revenant", "Michael Punke", 272));
            bookManager.Singleton.Add(new Book("The Final Empire", "Brandon Sanderson", 541));
            bookManager.Singleton.Add(new Book("The Code Book", "Simon Singh", 412));
            bookManager.Singleton.Add(new Book("Ship of Magic", "Robin Hobb", 880));

            // Create secondary thread and assign threadManager.DestroyBooks() as delegate.
            var thread = new Thread(
                () => bookManager.DestroyBooks())
                { Name = "Secondary" };

            // Start secondary thread.
            thread.Start();
            Logging.Log($"{thread.Name} thread started.", Logging.OutputType.Timestamp);

            // Count Books in main thread
            bookManager.CountBooks();

            // Abort secondary thread.
            thread.Abort();
            Logging.Log($"{thread.Name} thread aborted.", Logging.OutputType.Timestamp);

            // Join main and secondary thread.
            thread.Join();
            Logging.Log($"Joining {Thread.CurrentThread.Name} and {thread.Name} threads.", Logging.OutputType.Timestamp);
        }
    }
}
