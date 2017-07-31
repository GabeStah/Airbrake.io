using System.Collections.Generic;
using Utility;

namespace Airbrake.InvalidOperationException
{
    class Program
    {
        static void Main(string[] args)
        {
            ValidExample();
            Logging.Log("-----------------");
            InvalidExample();
        }

        private static void InvalidExample()
        {
            try
            {
                List<Book> books = new List<Book>();
                books.Add(new Book("The Stand", "Stephen King"));
                books.Add(new Book("Moby Dick", "Herman Melville"));
                books.Add(new Book("Fahrenheit 451", "Ray Bradbury"));
                books.Add(new Book("A Game of Thrones", "George R.R. Martin"));
                books.Add(new Book("The Name of the Wind", "Patrick Rothfuss"));

                Logging.Log($"Total Book count: {books.Count}.");

                Book newBook = new Book("Robinson Crusoe", "Daniel Defoe");
                foreach (var book in books)
                {
                    Logging.Log($"Current book title: {book.Title}, author: {book.Author}.");
                    if (!books.Contains(newBook))
                    {
                        books.Add(newBook);
                        Logging.Log($"Adding new book: {newBook.Title}, author: {newBook.Author}.");
                    }
                    Logging.Log($"New total Book count: {books.Count}.");
                }
            }
            catch (System.InvalidOperationException exception)
            {
                Logging.Log(exception);
            }
        }

        private static void ValidExample()
        {
            try
            {
                List<Book> books = new List<Book>();
                books.Add(new Book("The Stand", "Stephen King"));
                books.Add(new Book("Moby Dick", "Herman Melville"));
                books.Add(new Book("Fahrenheit 451", "Ray Bradbury"));
                books.Add(new Book("A Game of Thrones", "George R.R. Martin"));
                books.Add(new Book("The Name of the Wind", "Patrick Rothfuss"));

                Logging.Log($"Total Book count: {books.Count}.");

                Book newBook = new Book("Robinson Crusoe", "Daniel Defoe");
                int maxCount = books.Count - 1;
                for (int index = 0; index <= maxCount; index++)
                {
                    Logging.Log($"Current book title: {books[index].Title}, author: {books[index].Author}.");
                    if (!books.Contains(newBook))
                    {
                        books.Add(newBook);
                        Logging.Log($"Adding new book: {newBook.Title}, author: {newBook.Author}.");
                    }
                }
                Logging.Log($"New total Book count: {books.Count}.");
            }
            catch (System.InvalidOperationException exception)
            {
                Logging.Log(exception);
            }
        }
    }

    internal interface IBook
    {
        string Author { get; set; }
        string Title { get; set; }
    }

    internal class Book : IBook
    {
        public string Title { get; set; }
        public string Author { get; set; }

        public Book() { }

        public Book(string title, string author)
        {
            Title = title;
            Author = author;
        }
    }
}
