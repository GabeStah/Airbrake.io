using System;
using Utility;

namespace Airbrake.InvalidCastException
{
    class Program
    {
        static void Main(string[] args)
        {
            CastToDerivedType();
            CastToBaseType();
            CastToString();
        }

        private static void CastToString()
        {
            
        }

        private static void CastToBaseType()
        {
            try
            {
                var publishedBook = new PublishedBook("It", "Stephen King", new DateTime(1986, 9, 1));

                // Upcasting PublishedBook to base type of Book.
                var castBook = (Book)publishedBook;
                Logging.Log("Upcasting successful:");
                Logging.Log(castBook);
            }
            catch (System.InvalidCastException exception)
            {
                Logging.Log("Upcasting failed.");
                Logging.Log(exception);
            }
        }

        private static void CastToDerivedType()
        {
            try
            {
                var book = new Book("Sword in the Darkness", "Stephen King");

                // Downcasting PublishedBook to base type of Book.
                var castBook = (PublishedBook)book;
                Logging.Log("Downcasting successful:");
                Logging.Log(castBook);
            }
            catch (System.InvalidCastException exception)
            {
                Logging.Log("Downcasting failed.");
                Logging.Log(exception);
            }
        }
    }

    public interface IBook
    {
        string Author { get; set; }
        string Title { get; set; }
    }

    public class Book : IBook
    {
        public string Title { get; set; }
        public string Author { get; set; }

        public Book(string title, string author)
        {
            Title = title;
            Author = author;
        }
    }

    public class PublishedBook : Book
    {
        public DateTime PublishedAt { get; set; }

        public PublishedBook(string title, string author, DateTime published_at)
            : base(title, author)
        {
            Title = title;
            Author = author;
            PublishedAt = published_at;
        }
    }
}
