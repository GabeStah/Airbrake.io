using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Ports;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace Airbrake.TimeoutException
{
    class Program
    {
        static void Main(string[] args)
        {
            string input;
            try
            {
                var result = GetRequestWithHttpClient("http://192.166.166.166");
                var blah = true;
            }

            // Only catch timeout exceptions.
            catch (System.TimeoutException e)
            {
                Logging.Log(e);
            }
        }

        public static async Task<string> GetRequestWithHttpClient(string url)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.Timeout = TimeSpan.FromMilliseconds(5000);
                HttpResponseMessage response = client.GetAsync(url).Result;
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (System.TimeoutException exception)
            {
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                Logging.Log(exception, false);
                throw exception;
            }
            return null;
        }
    }
}
