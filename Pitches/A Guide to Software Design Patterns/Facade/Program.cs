using Utility;

namespace Facade
{
    /// <summary>
    /// Houses all Author logic.
    /// </summary>
    class Author
    {
        public string Name { get; set; }
        
        public Author() { }

        public Author(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Determine if Book has a Author.
        /// </summary>
        /// <param name="book">Book to check.</param>
        /// <returns>Indicates if Book has a Author.</returns>
        public bool HasAuthor(Book book)
        {
            return !string.IsNullOrEmpty(book.Author?.Name);
        }
    }

    /// <summary>
    /// Houses all content logic.
    /// </summary>
    class Content
    {
        public string Title { get; set; }

        public Content() { }

        public Content(string title)
        {
            Title = title;
        }

        /// <summary>
        /// Determine if Book has Content.
        /// </summary>
        /// <param name="book">Book to check.</param>
        /// <returns>Indicates if Book has Content.</returns>
        public bool HasContent(Book book)
        {
            return !string.IsNullOrEmpty(book.Content?.Title);
        }
    }

    /// <summary>
    /// Houses all Cover logic.
    /// </summary>
    class Cover
    {
        /// <summary>
        /// Determine if Book has a Cover.
        /// </summary>
        /// <param name="book">Book to check.</param>
        /// <returns>Indicates if Book has a Cover.</returns>
        public bool HasCover(Book book) => book.Cover != null;
    }

    /// <summary>
    /// Houses all Editor logic.
    /// </summary>
    class Editor
    {
        /// <summary>
        /// Determine if Book is Edited.
        /// </summary>
        /// <param name="book">Book to check.</param>
        /// <returns>Indicates if Book is Edited.</returns>
        public bool HasEditor(Book book) => book.Editor != null;
    }

    /// <summary>
    /// Houses all Publisher logic.
    /// </summary>
    class Publisher
    {
        /// <summary>
        /// Determine if Book has a Publisher.
        /// </summary>
        /// <param name="book">Book to check.</param>
        /// <returns>Indicates if Book has a Publisher.</returns>
        public bool HasPublisher(Book book) => book.Publisher != null;
    }

    /// <summary>
    /// Basic Book object.
    /// </summary>
    class Book
    {
        public Author Author { get; set; }
        public Content Content { get; set; }
        public Editor Editor { get; set; }
        public Cover Cover { get; set; }
        public Publisher Publisher { get; set; }

        public Book() { }

        public Book(Author author, Content content, Editor editor, Cover cover, Publisher publisher)
        {
            Author = author;
            Content = content;
            Editor = editor;
            Cover = cover;
            Publisher = publisher;
        }
    }

    /// <summary>
    /// Facade which delegates client requests to appropriate sub-objects.
    /// In this case, determines if passed Book is ready to be published.
    /// </summary>
    class BookFacade
    {
        internal Author Author { get; set; } = new Author();
        internal Content Content { get; set; } = new Content();
        internal Cover Cover { get; set; } = new Cover();
        internal Editor Editor { get; set; } = new Editor();
        internal Publisher Publisher { get; set; } = new Publisher();

        /// <summary>
        /// Determine if passed Book is publishable.
        /// </summary>
        /// <param name="book">Book instance to verify.</param>
        /// <returns>If Book is ready to be published.</returns>
        public bool IsPublishable(Book book)
        {
            return Author.HasAuthor(book) &&
                   Content.HasContent(book) &&
                   Cover.HasCover(book) &&
                   Editor.HasEditor(book) &&
                   Publisher.HasPublisher(book);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // New BookFacade instance.
            var bookFacade = new BookFacade();

            // Create empty Book instance.
            var emptyBook = new Book();
            // Output book.
            Logging.Log(emptyBook);
            // Check if book is publishable.
            Logging.Log($"Book is publishable? {bookFacade.IsPublishable(emptyBook)}");

            Logging.LineSeparator(40);

            // Create populated Book.
            var populatedBook = new Book(author: new Author("Stephen King"),
                                         content: new Content("The Stand"),
                                         editor: new Editor(),
                                         cover: new Cover(),
                                         publisher: new Publisher());
            // Output book.
            Logging.Log(populatedBook);
            // Check if book is publishable.
            Logging.Log($"Book is publishable? {bookFacade.IsPublishable(populatedBook)}");
        }
    }
}

