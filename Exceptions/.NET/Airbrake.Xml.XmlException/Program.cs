// <Airbrake.Xml.XmlException>/Program.cs
using System;
using System.Xml.Linq;
using Utility;

namespace Airbrake.Xml.XmlException
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create Books from books.xml.
            Logging.LineSeparator("USING 'books.xml'");
            CreateBooksFromXml(@"books.xml");

            // Create Books from books-malformed.xml.
            Logging.LineSeparator("USING 'books-malformed.xml'");
            CreateBooksFromXml(@"books-malformed.xml");
        }

        /// <summary>
        /// Create a series of Book instances from data in passed Xml.
        /// </summary>
        /// <param name="path">Path to Xml data file.</param>
        internal static void CreateBooksFromXml(string path)
        {
            try
            {
                // Load Xml from path and get Book elements.
                var books = XElement.Load(path).Elements("Book");
                // Loop through book elements.
                foreach (var element in books)
                {
                    // Attempt to convert page count string to integer.
                    int.TryParse(element.Element("PageCount")?.Value, out var pageCount);
                    // Create new book instance.
                    var book = new Book(element.Element("Title")?.Value, 
                        element.Element("Author")?.Value,
                        pageCount);
                    // Output book instance to log.
                    Logging.Log(book);
                }
            }
            catch (System.Xml.XmlException exception)
            {
                // Output expected XmlExceptions.
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
