using System;
using System.ServiceModel;
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
                    throw new System.ArgumentException($"CheckoutBook() parameter 'title' must be a valid string.");
                }
                // Check if author value is null or has no characters.
                // Null must be checked prior to length to avoid checking an invalid object.
                if (author is null || author.Length == 0)
                {
                    throw new System.ArgumentException($"CheckoutBook() parameter 'author' must be a valid string.");
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
                var fault = new InvalidBookFault();
                fault.Description = e.Message;
                fault.Message = e.Message;
                fault.Result = false;

                // Throw newly created FaultException of appropriate type.
                throw new FaultException<InvalidBookFault>(fault, new FaultReason(e.Message));
            }
            catch (Exception e)
            {
                Logging.Log(e, false);
            }
            return false;
        }
    }
}
