using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace DemoClient
{
    class Program
    {
        static void Main(string[] args)
        {
            LibraryServiceReference.LibraryServiceClient client = new LibraryServiceReference.LibraryServiceClient();
            try
            {
                client.Open();
                client.ReserveBook("The Stand", null);
                client.Close();
            }
            catch (FaultException<LibraryServiceReference> e)
            {

            }
            catch (Exception e)
            {
                Logging.Log(e, false);
            }
        }
    }
}
