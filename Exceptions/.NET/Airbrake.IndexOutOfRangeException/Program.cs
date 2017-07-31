using System;
using System.Collections.Generic;
using Utility;

namespace Airbrake.IndexOutOfRangeException
{
    /// <summary>
    /// Book interface.
    /// </summary>
    internal interface IBook
    {
        string Author { get; set; }
        string Title { get; set; }
    }

    /// <summary>
    /// Basic Book class.
    /// </summary>
    internal class Book : IBook
    {
        public string Author { get; set; }
        public string Title { get; set; }

        public Book(string title, string author)
        {
            Author = author;
            Title = title;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            ValidListIterationExample();
            Logging.LineSeparator();
            ArgumentOutOfRangeExample();
            Logging.LineSeparator();
            ChangeExistingElement();
            Logging.LineSeparator();
            OutOfRangeReferenceExample();
        }

        static void ValidListIterationExample()
        {
            try
            {
                // Create Book list.
                List<Book> library = new List<Book>();
                // Add a few books.
                library.Add(new Book("The Stand", "Stephen King"));
                library.Add(new Book("The Name of the Wind", "Patrick Rothfuss"));
                library.Add(new Book("Robinson Crusoe", "Daniel Defoe"));
                library.Add(new Book("The Hobbit", "J.R.R. Tolkien"));
                // Iterate over Book list using index.
                for (int index = 0; index < library.Count; index++)
                {
                    // Output Book instance.
                    Logging.Log(library[index]);
                }
            }
            catch (Exception exception) when (exception is System.ArgumentOutOfRangeException ||
                                  exception is System.IndexOutOfRangeException)
            {
                // Output caught exception.
                Logging.Log(exception);
            }
        }

        static void ArgumentOutOfRangeExample()
        {
            try
            {
                // Create Book list.
                List<Book> library = new List<Book>();
                // Add a few books.
                library.Add(new Book("The Stand", "Stephen King"));
                library.Add(new Book("The Name of the Wind", "Patrick Rothfuss"));
                library.Add(new Book("Robinson Crusoe", "Daniel Defoe"));
                library.Add(new Book("The Hobbit", "J.R.R. Tolkien"));
                // Iterate over Book list using index.
                // Count can equal library.Count, which will exceed index count by 1.
                for (int index = 0; index <= library.Count; index++)
                {
                    // Output Book instance.
                    Logging.Log(library[index]);
                }
            }
            catch (Exception exception) when (exception is System.ArgumentOutOfRangeException ||
                                              exception is System.IndexOutOfRangeException)
            {
                // Output caught exception.
                Logging.Log(exception);
            }
        }

        static void ChangeExistingElement()
        {
            try
            {
                // Create Book list.
                List<Book> library = new List<Book>();
                // Add a few books.
                library.Add(new Book("The Stand", "Stephen King"));
                library.Add(new Book("The Name of the Wind", "Patrick Rothfuss"));
                library.Add(new Book("Robinson Crusoe", "Daniel Defoe"));
                library.Add(new Book("The Hobbit", "J.R.R. Tolkien"));
                // Verify current library count.
                Logging.Log($"Current library count: {library.Count}");
                // Assign new book to last item in list.
                library[library.Count - 1] = new Book("Seveneves", "Neal Stephenson");
                // Output updated library.
                Logging.Log(library);
            }
            catch (Exception exception) when (exception is System.ArgumentOutOfRangeException ||
                                              exception is System.IndexOutOfRangeException)
            {
                // Output caught exception.
                Logging.Log(exception);
            }
        }

        static void OutOfRangeReferenceExample()
        {
            try
            {
                // Create Book list.
                List<Book> library = new List<Book>();
                // Add a few books.
                library.Add(new Book("The Stand", "Stephen King"));
                library.Add(new Book("The Name of the Wind", "Patrick Rothfuss"));
                library.Add(new Book("Robinson Crusoe", "Daniel Defoe"));
                library.Add(new Book("The Hobbit", "J.R.R. Tolkien"));
                // Verify current library count.
                Logging.Log($"Current library count: {library.Count}");
                // Assign new book to invalid index (index maxes out at Count - 1).
                library[library.Count] = new Book("Seveneves", "Neal Stephenson");
                // Output updated library.
                Logging.Log(library);
            }
            catch (Exception exception) when (exception is System.ArgumentOutOfRangeException ||
                                              exception is System.IndexOutOfRangeException)
            {
                // Output caught exception.
                Logging.Log(exception);
            }
        }
    }
}
