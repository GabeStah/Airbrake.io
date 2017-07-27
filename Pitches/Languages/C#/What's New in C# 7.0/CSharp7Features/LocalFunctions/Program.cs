using System;
using System.Collections.Generic;
using Utility;

namespace LocalFunctions
{
    public interface IBook
    {
        string Author { get; set; }
        int PageCount { get; set; }
        string Title { get; set; }
    }

    public class Book : IBook
    {
        public string Author { get; set; }
        public int PageCount { get; set; }
        public string Title { get; set; }

        public Book() { }

        public Book(string title, string author, int pageCount)
        {
            Author = author;
            PageCount = pageCount;
            Title = title;
        }

        public override string ToString()
        {
            return $"'{Title}' by {Author} at {PageCount} pages";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Create baseline Book collection.
            var books = new List<Book>
            {
                new Book("The Stand", "Stephen King", 823),
                new Book("Moby Dick", "Herman Melville", 378),
                new Book("Fahrenheit 451", "Ray Bradbury", 158),
                new Book("A Game of Thrones", "George R.R. Martin", 694),
                new Book("The Name of the Wind", "Patrick Rothfuss", 722)
            };

            // Output baseline books.
            Logging.Log("Baseline books.");
            Logging.Log(books);
            Logging.LineSeparator();

            // Filter books where PageCount exceeds 400.
            var filteredBooks = Filter(books, (book) => book.PageCount > 400);

            // Output filtered books.
            Logging.Log("Filtered books with more than 400 pages.");
            Logging.Log(filteredBooks);
            Logging.LineSeparator();

            // Inverse filter by passing false argument to make the filter behave exclusively.
            filteredBooks = Filter(books, (book) => book.PageCount > 400, false);

            // Output filtered books.
            Logging.Log("Filtered books with fewer than or equal to 400 pages.");
            Logging.Log(filteredBooks);
            Logging.LineSeparator();
        }

        /// <summary>
        /// Filters a collection.
        /// </summary>
        /// <typeparam name="T">Type of element to filter.</typeparam>
        /// <param name="source">Source collection to iterator through.</param>
        /// <param name="filter">Filter action to apply.</param>
        /// <param name="inclusive">Determines if filter should act as inclusive or exclusive check.</param>
        /// <returns>Filtered collection.</returns>
        public static IEnumerable<T> Filter<T>(IEnumerable<T> source, Func<T, bool> filter, bool inclusive = true)
        {
            // Local function to perform iteration.
            IEnumerable<T> Iterator()
            {
                // Loop through each element of source.
                foreach (var element in source)
                {
                    // If inclusive, if element passes filter yield it.
                    // If exclusive, if element fails filter yield it.
                    if (inclusive ? filter(element) : !filter(element)) { yield return element; }
                }
            }

            // Return yielded Iterator result.
            return Iterator();
        }
    }
}
