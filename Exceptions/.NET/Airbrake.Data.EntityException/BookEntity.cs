using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.RegularExpressions;
using Utility;

namespace Airbrake.Data.EntityCommandCompilationException
{
    public class BookEntity : Book
    {
        /// <summary>
        /// Composited key for entity.
        /// 
        /// Concatenates alphanumeric characters from Author and Title properties.
        /// </summary>
        [Key, Column("CompositeId", Order = 1)]
        public string CompositeId
        {
            get => $"{Regex.Replace(Author.ToLower(), @"[^A-Za-z0-9]+", "")}-{Regex.Replace(Title.ToLower(), @"[^A-Za-z0-9]+", "")}";
            set
            {
                if (value == null) throw new ArgumentNullException(nameof(value));
            }
        }

        /// <summary>
        /// Primary key for entity.
        /// 
        /// Column.Order determines which key is used primarily and secondarily.
        /// </summary>
        [Key, Column("Id", Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        #region Constructors

        public BookEntity() { }

        public BookEntity(string title, string author)
            : base(title, author) { }

        public BookEntity(string title, string author, int pageCount)
            : base(title, author, pageCount) { }

        public BookEntity(string title, string author, int pageCount, DateTime publicationDate)
            : base(title, author, pageCount, publicationDate) { }

        #endregion
    }
}