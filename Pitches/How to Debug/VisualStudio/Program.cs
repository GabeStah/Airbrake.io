using System;
using Utility;

namespace VisualStudio
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Book book = new Book("Green Eggs and Ham", "Dr. Seuss");
                Book book2 = new Book("This is a thing", "something");
                book.Author = "Another guy";
                Logging.Log(book);
            }
            catch (System.FieldAccessException exception)
            {
                Logging.Log(exception);
            }
        }

        public class Book
        {
            public string Title { get; set; }
            public string Author { get; set; }

            public Book(string title, string author)
            {
                Title = title;
                Author = author;
            }
        }
    }
}
