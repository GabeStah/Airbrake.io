using System;
using Utility;

namespace SimpleFactory
{
    class Program
    {
        static void Main(string[] args)
        {
            var book = BookFactory.MakeBook("The Stand", "Stephen King", 823);
            Logging.Log("Title: " + book.Title);
            Logging.Log("Author: " + book.Author);
            Logging.Log("Page Count: " + book.PageCount);
            Logging.Log("Cover Type: " + book.CoverType);
            Logging.Log("Object Class: " + book.ToString());
        }
    }
    
    public enum CoverType
    {
        Digital,
        Hard,
        Paperback
    }

    public interface IBook
    {
        string Author { get; set; }
        string Title { get; set; }
        int PageCount { get; set; }
        CoverType CoverType { get; }
    }

    public class PaperbackBook : IBook
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public int PageCount { get; set; }
        public CoverType CoverType { get; private set; }

        public PaperbackBook(string title, string author, int pageCount)
        {
            Title = title;
            Author = author;
            PageCount = pageCount;
            CoverType = CoverType.Paperback;
        }
    }

    class BookFactory
    {
        public static IBook MakeBook(string title, string author, int pageCount)
        {
            return new PaperbackBook(title, author, pageCount);
        }
    }
}