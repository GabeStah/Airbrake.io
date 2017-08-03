using System;
using System.Linq;
using Utility;

namespace RefReturnsAndLocals
{
    class Program
    {
        static void Main(string[] args)
        {
            new Library();
        }
    }

    internal class Library
    {
        internal Library()
        {
            // Create baseline Book collection.
            var books = new[]
            {
                new Book("The Stand", "Stephen King", 823),
                new Book("Moby Dick", "Herman Melville", 378),
                new Book("Fahrenheit 451", "Ray Bradbury", 158),
                new Book("The Name of the Wind", "Patrick Rothfuss", 722)
            };

            // Output Book collection.
            Logging.LineSeparator("COLLECTION");
            Logging.Log(books);

            // Title of Book to search for.
            const string searchTitle = "Moby Dick";
            
            // Get index of target Book.
            var index = Array.FindIndex(books, x => x.Title == searchTitle);
            // Output index.
            Logging.Log($"Search title: {searchTitle} found at index: {index}.");

            // Get reference of search title Book.
            ref var reference = ref GetReferenceByTitle(searchTitle, books);
            // Confirm reference retrieved.
            Logging.LineSeparator("REFERENCE");
            Logging.Log(reference);

            // Get value of search title Book.
            var value = GetValueByTitle(searchTitle, books);
            // Confirm value retrieved.
            Logging.LineSeparator("VALUE");
            Logging.Log(value);

            // Change value of reference to "Game of Thrones" Book.
            reference = new Book("A Game of Thrones", "George R.R. Martin", 694);

            // Use previously retrieved index to confirm array element changed.
            Logging.LineSeparator("UPDATED BY INDEX");
            Logging.Log(books[index]);

        }

        /// <summary>
        /// Get reference from Book collection based on passed title.
        /// </summary>
        /// <param name="title">Title to search for.</param>
        /// <param name="books">Book collection.</param>
        /// <returns>Matching Book reference.</returns>
        public ref Book GetReferenceByTitle(string title, Book[] books)
        {
            for (var i = 0; i < books.Length; i++)
            {
                if (books[i].Title == title)
                {
                    return ref books[i];
                }
            }
            throw new IndexOutOfRangeException($"Book with title '{title}' not found.");
        }

        /// <summary>
        /// Get Book instance from Book collection based on passed title.
        /// </summary>
        /// <param name="title">Title to search for.</param>
        /// <param name="books">Book collection.</param>
        /// <returns>Matching Book instance.</returns>
        public Book GetValueByTitle(string title, Book[] books)
        {
            return books.FirstOrDefault(x => x.Title == title);
        }
    }
}
