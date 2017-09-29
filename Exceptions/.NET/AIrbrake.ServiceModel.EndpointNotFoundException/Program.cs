// Program.cs
using System;
using AIrbrake.ServiceModel.EndpointNotFoundException.MyServiceReference;
using Utility;

namespace AIrbrake.ServiceModel.EndpointNotFoundException
{
    internal class Program
    {
        internal static void Main(string[] args)
        {
            Logging.LineSeparator("GetDataTest(0)");
            GetDataTest(0);

            Logging.LineSeparator("GetDataAsyncTest(0)");
            GetDataAsyncTest(0);
        }

        internal static void GetDataTest(int value)
        {
            try
            {
                var client = new MyServiceClient();
                client.Open();
                var result = client.GetData(value);
                Logging.Log(result);
            }
            catch (System.ServiceModel.EndpointNotFoundException exception)
            {
                // Output expected EndpointNotFoundExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Output unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }

        internal static void GetDataAsyncTest(int value)
        {
            try
            {
                var client = new MyServiceClient();
                client.Open();
                var result = client.GetDataAsync(value);
                Logging.Log(result);
            }
            catch (System.ServiceModel.EndpointNotFoundException exception)
            {
                // Output expected EndpointNotFoundExceptions.
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
