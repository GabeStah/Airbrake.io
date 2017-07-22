using System;
using System.ServiceModel;
using Utility;

namespace Airbrake.Web.Services.Protocols.SoapException
{
    class Program
    {
        static void Main(string[] args)
        {
            // Instantiate new LibraryServiceClient.
            var client = new LibraryServiceReference.LibraryServiceClient();

            try
            {
                // Open client connection.
                client.Open();

                // Reserve a valid book.
                client.ReserveBook("The Hobbit", "J.R.R. Tolkien");
                // Reserve an invalid book.
                client.ReserveBook("The Hobbit", null);

                // Close client connection.
                client.Close();
            }
            catch (System.Web.Services.Protocols.SoapException exception)
            {
                // Never triggers since client only receives FaultExceptions.
                Logging.Log(exception);
                client.Abort();
            }
            // Catch our SOAP fault type. 
            catch (FaultException<LibraryServiceReference.InvalidBookFault> exception)
            {
                // Log expected FaultException<LibraryServiceReference.InvalidBookFault>.
                Logging.Log(exception);
                client.Abort();
            }
            catch (FaultException exception)
            {
                // Log unexpected FaultExceptions.
                Logging.Log(exception, false);
                client.Abort();
            }
            catch (Exception exception)
            {
                // Log unexpected Exceptions.
                Logging.Log(exception, false);
                client.Abort();
            }
        }
    }
}
