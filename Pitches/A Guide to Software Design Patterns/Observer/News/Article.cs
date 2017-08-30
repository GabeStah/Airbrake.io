// <News/>Article.cs
using System;

namespace Observer.News
{
    /// <summary>
    /// Contains basic publication information, including source Agency.
    /// </summary>
    public class Article : IComparable
    {
        public Agency Agency { get; }

        public string Author { get; }

        public string Title { get; }

        internal Article(string title, string author, Agency agency)
        {
            Agency = agency;
            Author = author;
            Title = title;
        }

        /// <summary>
        /// Comparison method for IComparison interface, used for sorting.
        /// </summary>
        /// <param name="article">Article to be compared.</param>
        /// <returns>Comparison result.</returns>
        public int CompareTo(object article)
        {
            if (article is null) return 1;

            var other = article as Article;

            // Check that parameter is Article.
            if (other is null) throw new ArgumentException("Compared object must be an Article instance.", nameof(article));

            // If author difference, sort by author first.
            // Otherwise, sort by title.
            var authorDiff = string.Compare(Author, other.Author, StringComparison.Ordinal);
            return authorDiff != 0 ? authorDiff : string.Compare(Title, other.Title, StringComparison.Ordinal);
        }

        public override string ToString()
        {
            return $"'{Title}' by {Author} via {Agency.Name}";
        }
    }
}