using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using System;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using Utility;

namespace Airbrake.Net.WebException
{
    class Program
    {
        static void Main(string[] args)
        {
            // Connect to Airbrake.io.
            ParseTest("https://airbrake.io");
            Logging.LineSeparator();
            // Try connecting to localhost:24601.
            ParseTest("http://localhost:24601");
        }

        static void ParseTest(string uri)
        {
            try
            {
                // Generate document using AngleSharp.
                var document = WebParser.GetHtmlDocument(uri);
                // Get all h1 and h2 headers from document, concat into single 
                // collection, and then get TextContent and insert into array.
                var headers = document.QuerySelectorAll("h1")
                                      .Concat(document.QuerySelectorAll("h2"))
                                      .Select(element => element.TextContent)
                                      .ToArray();
                // Output title and headers to test if connection was successful.
                Logging.Log($"Title: {document.Title}");
                Logging.Log("Headers:");
                Logging.Log(headers);
            }
            catch (System.Net.WebException exception)
            {
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                Logging.Log(exception, false);
            }
        }
    }

    public static class WebParser
    {
        public static IHtmlDocument GetHtmlDocument(string uri)
        {
            HttpContent content;
            HttpClient client = new HttpClient();
            HttpResponseMessage response;
            HtmlParser parser = new HtmlParser();

            try
            {
                // Get async result from uri.
                response = client.GetAsync(uri).Result;
                // Get response content.
                content = response.Content;
                // Read result string (HTML).
                var result = content.ReadAsStringAsync().Result;
                // Parse HTML and return produced IHtmlDocument.
                return parser.Parse(result);
            }
            catch (Exception exception) when (exception is System.Net.WebException ||
                                              exception is HttpRequestException ||
                                              exception is SocketException)
            {
                Logging.Log(exception);
            }
            catch (Exception exception) when (exception is AggregateException)
            {
                // Gather InnerExceptions into a collection then filter out only WebException matches.
                foreach (var e in exception.FromHierarchy(e => e.InnerException)
                                           .Where(e => e.GetType() == typeof(System.Net.WebException)))
                {
                    // Log any expected and matched exceptions.
                    Logging.Log(e);
                }
            }

            return null;
        }
    }
}
