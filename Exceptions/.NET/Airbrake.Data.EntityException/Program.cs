using System;
using System.Data.Entity.Core;
using System.Linq;
using Utility;

namespace Airbrake.Data.EntityCommandCompilationException
{
    internal class Program
    {
        private static void Main()
        {
            try
            {
                // Instantiate database context.
                var context = new BookEntityContext();

                // Delete database, if already exists.
                Logging.LineSeparator("DELETE DATABASE");
                context.Database.Delete();

                // Create database.
                Logging.LineSeparator("CREATE DATABASE");
                context.Database.Create();

                // Add some BookEntities to context.
                AddBookEntityToContext(
                    new BookEntity(
                        "Magician: Apprentice",
                        "Raymond E. Feist",
                        485,
                        new DateTime(1982, 10, 1)),
                    context);

                AddBookEntityToContext(
                    new BookEntity(
                        "Magician: Master",
                        "Raymond E. Feist",
                        499,
                        new DateTime(1982, 11, 1)),
                    context);

                AddBookEntityToContext(
                    new BookEntity(
                        "Silverthorn",
                        "Raymond E. Feist",
                        432,
                        new DateTime(1985, 5, 7)),
                    context);

                // Output BookEntities found in context.
                OutputBookEntitiesOfContext(context);

                UpdateBookPageCount(1, 24_601, context);
                OutputBookEntitiesOfContext(context);
            }
            catch (EntityException exception)
            {
                // Output expected EntityExceptions.
                Logging.Log(exception);
                if (exception.InnerException != null)
                {
                    // Output unexpected InnerExceptions.
                    Logging.Log(exception.InnerException, false);
                }
            }
            catch (Exception exception)
            {
                // Output unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }

        /// <summary>
        /// Add a BookEntity to pass context and save changes.
        /// </summary>
        /// <param name="book">BookEntity to be added.</param>
        /// <param name="context">Context to which BookEntity should be added.</param>
        private static void AddBookEntityToContext(BookEntity book, BookEntityContext context)
        {
            context.Books.Add(book);
            context.SaveChanges();
        }

        /// <summary>
        /// Logs the list of BookEntities in passed context.
        /// </summary>
        /// <param name="context">Context containing BookEntities.</param>
        private static void OutputBookEntitiesOfContext(BookEntityContext context)
        {
            // Select all books, ordered by descending publication date.
            var query = 
                from book
                in context.Books
                orderby book.PublicationDate descending
                select book;
            Logging.LineSeparator("CURRENT BOOK LIST");
            // Output query result (books).
            Logging.Log(query);
        }

        /// <summary>
        /// Update book page count via iteration.
        /// </summary>
        /// <param name="id">Book Id.</param>
        /// <param name="pageCount">Book Page Count.</param>
        /// <param name="context">Context containing Book to update.</param>
        private static void UpdateBookPageCount(int id, int pageCount, BookEntityContext context)
        {
            // Find book by id.
            var result = context.Books.SingleOrDefault(book => book.Id == id);
            if (result == null) return;
            // Update page count.
            result.PageCount = pageCount;
            // Save changes to context.
            context.SaveChanges();
        }
    }
}
