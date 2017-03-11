using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpBrake;

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
                e.SendToAirbrake();
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
