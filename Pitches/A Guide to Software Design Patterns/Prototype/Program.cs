using System;
using Utility;

namespace Prototype
{
    class Program
    {
        static void Main(string[] args)
        {
            var book = new Book("A Game of Thrones", "George R.R. Martin", 694);

            var shallowClone = book.Clone();
            Logging.Log("---- Base Book ----");
            Logging.Log(book);
            Logging.Log("---- Shallow Clone ----");
            Logging.Log(shallowClone);


            Logging.Log("#### MODIFIED BASE BOOK ####");
            book.Title = "A Clash of Kings";
            book.Pages.PageCount = 768;

            Logging.Log("---- Base Book ----");
            Logging.Log(book);
            Logging.Log("---- Shallow Clone ----");
            Logging.Log(shallowClone);


            book = new Book("A Game of Thrones", "George R.R. Martin", 694);

            var deepClone = book.DeepClone();
            Logging.Log("---- Base Book ----");
            Logging.Log(book);
            Logging.Log("---- Deep Clone ----");
            Logging.Log(deepClone);

            Logging.Log("#### MODIFIED BASE BOOK ####");
            book.Title = "A Clash of Kings";
            book.Pages.PageCount = 768;

            Logging.Log("---- Base Book ----");
            Logging.Log(book);
            Logging.Log("---- Deep Clone ----");
            Logging.Log(deepClone);
        }
    }

    public class Pages
    {
        public int PageCount { get; set; }

        public Pages(int pageCount)
        {
            this.PageCount = pageCount;
        }
    }

    public interface IBook
    {
        string Author { get; set; }
        string Title { get; set; }
        Pages Pages { get; set; }
    }

    public class Book : IBook, ICloneable
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public Pages Pages { get; set; }

        public Book(string title, string author, int pageCount)
        {
            Title = title;
            Author = author;
            Pages = new Pages(pageCount);
        }

        // Create a deep clone of Book instance.
        public Book DeepClone()
        {
            // Create shallow clone with explicit conversion to Book type.
            Book clone = (Book)this.MemberwiseClone();
            // Copy Title string.
            clone.Title = String.Copy(Title);
            // Copy Author string.
            clone.Author = String.Copy(Author);
            // Create new instance of Pages class and pass instance's page count.
            clone.Pages = new Pages(Pages.PageCount);
            // Return deep clone object.
            return clone;
        }

        // Create a shallow clone of instance.
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
