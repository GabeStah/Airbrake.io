// <Utility>/Book.cs

using System;

namespace Utility
{
    public interface IBook
    {
        string Author { get; set; }
        int PageCount { get; set; }
        DateTime? PublicationDate { get; set; }
        string Title { get; set; }
    }

    /// <summary>
    /// Simple Book class.
    /// </summary>
    public class Book : IBook
    {
        public string Author { get; set; }
        public int PageCount { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string Title { get; set; }

        public Book() { }

        public Book(string title, string author)
        {
            Author = author;
            Title = title;
        }

        public Book(string title, string author, int pageCount)
        {
            Author = author;
            PageCount = pageCount;
            Title = title;
        }

        public Book(string title, string author, int pageCount, DateTime publicationDate)
        {
            Author = author;
            PageCount = pageCount;
            PublicationDate = publicationDate;
            Title = title;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var publicationDate = PublicationDate is null ? null : $", published on {PublicationDate.Value.ToLongDateString()}";
            return $"'{Title}' by {Author} at {PageCount} pages{publicationDate}";
        }
    }
}
