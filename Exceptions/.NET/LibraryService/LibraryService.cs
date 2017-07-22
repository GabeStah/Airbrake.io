using System.ServiceModel;
using System.Web.Services.Protocols;
using Utility;

namespace LibraryService
{
    public interface IBook
    {
        string Author { get; set; }
        string Title { get; set; }
        bool Reserved { get; set; }
    }

    public class Book : IBook
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public bool Reserved { get; set; }

        public Book() { }

        public Book(string title, string author)
        {
            Title = title;
            Author = author;
        }
    }

    public class LibraryService : ILibraryService
    {
        public bool ReserveBook(string title, string author)
        {
            try
            {
                // Check if title value is null or has no characters.
                // Null must be checked prior to length to avoid checking an invalid object.
                if (title is null || title.Length == 0)
                {
                    throw new SoapException($"ReserveBook() parameter 'title' must be a valid string.",
                        new System.Xml.XmlQualifiedName("InvalidParameter", this.ToString()));
                }
                // Check if author value is null or has no characters.
                // Null must be checked prior to length to avoid checking an invalid object.
                if (author is null || author.Length == 0)
                {
                    throw new SoapException($"ReserveBook() parameter 'title' must be a valid string.",
                        new System.Xml.XmlQualifiedName("InvalidParameter", this.ToString()));
                }

                // Create book record and reserve it (a real service
                // would likely use a database connection instead).
                var book = new Book(title, author)
                {
                    Reserved = true
                };
                // Output reservation and book data to server.
                Logging.Log("RESERVATION SUCCESSFUL");
                Logging.LineSeparator();
                Logging.Log(book);
                return true;
        }
            catch (SoapException exception)
            {
                // Log the exception to server.
                Logging.Log(exception);

                // Generate new fault and set details.
                var fault = new InvalidBookFault
                {
                    Description = exception.Message,
                    Message = exception.Message,
                    Result = false
                };

                // Throw newly created FaultException of appropriate type.
                throw new FaultException<InvalidBookFault>(fault, new FaultReason(exception.Message));
            }
        }
    }
}
