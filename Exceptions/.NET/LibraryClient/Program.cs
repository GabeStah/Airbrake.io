using System;
using System.ServiceModel;
using Utility;

namespace LibraryClient
{
    class Program
    {
        static void Main(string[] args)
        {           
            LibraryServiceReference.LibraryServiceClient client = new LibraryServiceReference.LibraryServiceClient(); ;

            try
            {
                // Open client connection.
                client.Open();

                // Reserve a valid book.
                client.ReserveBook("The Stand", "Stephen King");
                // Reserve an invalid book.
                client.ReserveBook("The Stand", null);

                // Close client connection.
                client.Close();
            }
            // Catch our SOAP fault type. 
            catch (FaultException<LibraryServiceReference.InvalidBookFault> e)
            {
                Logging.Log(e);
                client.Abort();
            }
            catch (FaultException e)
            {
                Logging.Log(e, false);
                client.Abort();
            }
            catch (Exception e)
            {
                Logging.Log(e, false);
                client.Abort();
            }
        }
    }
}
