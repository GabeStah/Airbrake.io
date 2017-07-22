using System;
using System.ServiceModel;
using System.Web.Services.Protocols;
using Airbrake.Web.Services.Protocols.SoapExceptionService;
using Utility;

namespace SoapExceptionService
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
                    throw new SoapException("Error Message", SoapException.ClientFaultCode);
                }
                // Check if author value is null or has no characters.
                // Null must be checked prior to length to avoid checking an invalid object.
                if (author is null || author.Length == 0)
                {
                    throw new SoapException("Error Message", SoapException.ClientFaultCode);
                }

                // Create book record and reserve it (in a real service
                // a database record would likely be modified.
                Book book = new Book(title, author);
                book.Reserved = true;
                Logging.Log("RESERVATION SUCCESSFUL");
                Logging.LineSeparator();
                Logging.Log(book);
                return true;
            }
            catch (ArgumentException e)
            {
                // Log the exception.
                Logging.Log(e);

                // Generate new fault and set details.
                var fault = new SoapExceptionFault();
                fault.Description = e.Message;
                fault.Message = e.Message;
                fault.Result = false;

                // Throw newly created FaultException of appropriate type.
                throw new FaultException<SoapExceptionFault>(fault, new FaultReason(e.Message));
            }
            catch (Exception e)
            {
                Logging.Log(e, false);
            }
            return false;
        }
    }
}
