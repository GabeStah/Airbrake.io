using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Airbrake.StackOverflowException
{
    class Program
    {
        private const int COUNTER_MAX = 10000;
        private const int OUTPUT_FREQUENCY = 1000;
        static int counter = 0;

        static void Main(string[] args)
        {
            StackOverflowExample();
            Logging.Log("-----------------");
            // Reset counter.
            counter = 0;
            PreventativeExample();
        }

        private static void StackOverflowExample()
        {
            try
            {
                // Iterate counter.
                counter++;

                // Output counter value every so often.
                if (counter % OUTPUT_FREQUENCY == 0)
                {
                    Logging.Log($"Current counter: {counter}.");
                }

                // Recursively call self method.
                StackOverflowExample();
            }
            catch (System.StackOverflowException exception)
            {
                Logging.Log(exception);
            }
        }

        private static void PreventativeExample()
        {
            try
            {
                // Iterate counter.
                counter++;

                // Output counter value every so often.
                if (counter % OUTPUT_FREQUENCY == 0)
                {
                    Logging.Log($"Current counter: {counter}.");
                }

                // Check if counter has reached maximum value; if not, allow recursion.
                if (counter <= COUNTER_MAX)
                {
                    // Recursively call self method.
                    PreventativeExample();
                }
                else
                {
                    Logging.Log("Recursion halted.");
                }
            }
            catch (System.StackOverflowException exception)
            {
                Logging.Log(exception);
            }
        }
    }
}
