using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Utility;

namespace Airbrake.OutOfMemoryException
{
    class Program
    {
        static void Main(string[] args)
        {
            StringBuilderExample();
            Logging.Log("--------------------");
            LargeDataSetExample();
            Logging.Log("--------------------");
            ThrowExample();
        }

        public static void StringBuilderExample()
        {
            try
            {
                string firstName = "Bob";
                string lastName = "Smith";
                // Initialize with allocated length (MaxCapacity) equal to initial value length.
                StringBuilder builder = new StringBuilder(firstName.Length, firstName.Length);
                Logging.Log($"builder.MaxCapacity: {builder.MaxCapacity}");
                // Append initial value.
                builder.Append(firstName);
                // Attempt to insert additional value to builder already at MaxCapacity character count.
                builder.Insert(value: lastName,
                               index: firstName.Length - 1,
                               count: 1);
            }
            catch (System.OutOfMemoryException e)
            {
                Logging.Log(e, true);
            }
        }

        public static void ThrowExample()
        {
            try
            {
                // Outer block to handle any unexpected exceptions.
                try
                {
                    string s = "This";
                    s = s.Insert(2, "is ");

                    // Throw an OutOfMemoryException exception.
                    throw new System.OutOfMemoryException();
                }
                catch (ArgumentException)
                {
                    Logging.Log("ArgumentException in String.Insert");
                }

                // Execute program logic.
            }
            catch (System.OutOfMemoryException e)
            {
                Logging.Log("Terminating application unexpectedly...");
                Environment.FailFast(String.Format("Out of Memory: {0}",
                                                   e.Message));
            }
        }

        private static void LargeDataSetExample()
        {
            Random random = new Random();
            List<Double> list = new List<Double>();
            int maximum = 200000000;
            int split = 10000000;
            try
            {
                for (int count = 1; count <= maximum; count++)
                {
                    list.Add(random.NextDouble());
                    if (count % split == 0)
                    {
                        Logging.Log($"Total item count: {count}.");
                    }
                }
            }
            catch (System.OutOfMemoryException e)
            {
                Logging.Log(e, true);
            }
        }
    }
}
