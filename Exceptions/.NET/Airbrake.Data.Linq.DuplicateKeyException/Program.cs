using System.Linq;
using System.Data.SqlClient;
using Utility;

namespace Airbrake.Data.Linq.DuplicateKeyException
{
    class Program
    {

        static void Main(string[] args)
        {
            var adapter = new LinqAdapter();
        }
        
    }

    internal class LinqAdapter
    {
        private LibraryDataContext Context { get; }

        public LinqAdapter()
        {
            // Set data context.
            Context = new LibraryDataContext();
            // Create basic books and add to database.
            AddBooksToDatabase();
            // Retrieve existing book from database.
            var existingBook = (from book in Context.Books
                select book).FirstOrDefault();
            // Add book with explicit Id equal to existing Id.
            if (existingBook != null)
            {
                AddBookToDatabase(new Book
                {
                    Id = existingBook.Id,
                    Title = "Moby Dick",
                    Author = "Herman Melville",
                    PageCount = 752
                });
            }
        }

        /// <summary>
        /// Add passed Book to database.
        /// </summary>
        /// <param name="book">Book record to be added.</param>
        private void AddBookToDatabase(Book book)
        {
            try
            {
                // Specify that insertion should be performed when submission occurs.
                Context.Books.InsertOnSubmit(book);
                // Submit the insertion changes.
                Context.SubmitChanges();
                // Output successful addition.
                Logging.Log($"Book added successfully: '{book.Title}' by {book.Author} at {book.PageCount} pages.");
            }
            catch (System.Data.Linq.DuplicateKeyException exception)
            {
                // Output expected DuplicateKeyException.
                Logging.Log(exception);
            }
            catch (SqlException exception)
            {
                // Output unexpected SqlExceptions.
                Logging.Log(exception, false);
            }
        }

        /// <summary>
        /// Add a set of default Books to the database.
        /// </summary>
        private void AddBooksToDatabase()
        {
            AddBookToDatabase(new Book
            {
                Title = "The Hobbit",
                Author = "J.R.R. Tolkien",
                PageCount = 304
            });
            AddBookToDatabase(new Book
            {
                Title = "The Shining",
                Author = "Stephen King",
                PageCount = 823
            });
            AddBookToDatabase(new Book
            {
                Title = "The Name of the Wind",
                Author = "Patrick Rothfuss",
                PageCount = 722
            });
        }
    }
}
