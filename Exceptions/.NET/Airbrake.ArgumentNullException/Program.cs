using Utility;

namespace Airbrake.ArgumentNullException
{
    class Program
    {
        static void Main(string[] args)
        {
            ValidExample();
            InvalidExample();
            ValidCopyExample();
            InvalidCopyExample();
        }

        private static void ValidExample()
        {
            try
            {
                // Instantiate book with valid Title and Author arguments.
                var book = new Book("The Stand", "Stephen King");
                // Output book results.
                Logging.Log(book);
            }
            catch (System.ArgumentNullException e)
            {
                Logging.Log(e);
            }
        }

        private static void InvalidExample()
        {
            try
            {
                // Instantiate book with valid Title but invalid (null) Author arguments.
                var book = new Book("The Stand", null);
                // Output book results.
                Logging.Log(book);
            }
            catch (System.ArgumentNullException e)
            {
                Logging.Log(e);
            }
        }

        private static void ValidCopyExample()
        {
            try
            {
                // Instantiate book with valid Title and Author arguments.
                var book = new Book("The Stand", "Stephen King");
                var copy = new Book(book);
                // Output copy results.
                Logging.Log(copy);
            }
            catch (System.ArgumentNullException e)
            {
                Logging.Log(e);
            }
        }

        private static void InvalidCopyExample()
        {
            try
            {
                // Instantiate book with valid Title and Author arguments.
                var copy = new Book(null);
                // Output copy results.
                Logging.Log(copy);
            }
            catch (System.ArgumentNullException e)
            {
                Logging.Log(e);
            }
        }
    }

    public class Book
    {
        private string _author;
        private string _title;

        public string Author
        {
            get
            {
                return _author;
            }
            set
            {
                // Check if value is null.
                if (value is null)
                    // Throw a new ArgumentNullException with "Author" parameter name.
                    throw new System.ArgumentNullException("Author");
                _author = value;
            }
        }

        public bool IsCopy { get; set; }

        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                // Check if value is null.
                if (value is null)
                    // Throw a new ArgumentNullException with "Title" parameter name.
                    throw new System.ArgumentNullException("Title");
                _title = value;
            }
        }

        public Book(string title, string author)
        {
            Author = author;
            Title = title;
        }

        // Validates for non-null book parameter before copying.
        public Book(Book book)
            : this(NullValidator(book).Title, 
                   NullValidator(book).Author)
        {
            // Specify that this is a copy.
            IsCopy = true;
        }

        // Validate for non-null copy construction.
        private static Book NullValidator(Book book)
        {
            if (book is null)
                throw new System.ArgumentNullException("book");
            // If book isn't null then return.
            return book;
        }
    }
}
