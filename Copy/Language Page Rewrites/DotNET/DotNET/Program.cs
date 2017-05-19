using System;
using Utility;

namespace DotNET
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
                Logging.Log($"TryParse of {value} is: {Int32.TryParse(value, out int number)}");
            }

            try
            {
                foreach (var value in values)
                {
                    Logging.Log($"Parse of {value} is: {Int32.Parse(value)}");
                }
            }
            catch (System.FormatException e)
            {
                var result = notifier.NotifyAsync(e).Result;
                Logging.Log(e);
                Logging.Log(result);
            }
        }
    }
}
