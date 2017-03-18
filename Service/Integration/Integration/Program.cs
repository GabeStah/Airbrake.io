using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using Sharpbrake.Client;

namespace Integration
{
    class Program
    {
        static void Main(string[] args)
        {
            ParseExample();
        }

        static void ParseExample()
        {
            string[] values = { "123", "-123", "123.0", "01AD" };

            var integration = new AirbrakeIntegration();
            var notifier = integration.Notifier;

            foreach (var value in values)
            {
                Log($"TryParse of {value} is: {Int32.TryParse(value, out int number)}");
            }

            try
            {
                foreach (var value in values)
                {
                    Log($"Parse of {value} is: {Int32.Parse(value)}");
                }
            }
            catch (System.FormatException e)
            {
                Log(e);
                notifier.Notify(e);
            }
        }

        static void Log(object value)
        {
            #if DEBUG
                System.Diagnostics.Debug.WriteLine(value);
            #else
                Console.WriteLine(value);
            #endif
        }
    }
}
