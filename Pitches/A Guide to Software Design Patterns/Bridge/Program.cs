using Utility;

namespace Bridge
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a new binding instance.
            var spiralBinding = new SpiralBinding();
            // Create a fantasy book instance using spiral binding.
            var fantasyBook = new FantasyBook("The Name of the Wind", "Patrick Rothfuss", spiralBinding);
            // Create a different book type using the same spiral binding instance.
            var sciFiBook = new ScienceFictionBook("Dune", "Frank Herbert", spiralBinding);
        }
    }

    /// <summary>
    /// Basic interface for book binding types with one Name method.
    /// </summary>
    interface IBinding
    {
        string Name { get; }
    }

    /// <summary>
    /// Base class for all binding class types.
    /// </summary>
    class Binding : IBinding
    {
        /// <summary>
        /// Retrieves the name of the current binding class.
        /// </summary>
        public string Name
        {
            get
            {
                return this.GetType().Name;
            }
        }
    }

    class PerfectBinding : Binding { }

    class SpiralBinding : Binding { }

    class SaddleStitchBinding : Binding { }

    /// <summary>
    /// Basic interface for books with Author, Binding, and Title.
    /// </summary>
    interface IBook
    {
        string Author { get; set; }
        Binding Binding { get; set; }
        string Title { get; set; }
    }

    /// <summary>
    /// Base class for all book genre types.  Assigns passed arguments as basic property values and 
    /// outputs creation message with book class type, title, author, and binding name.
    /// </summary>
    class Book : IBook
    {
        public string Author { get; set; }
        public Binding Binding { get; set; }
        public string Title { get; set; }

        public Book(string title, string author, Binding binding)
        {
            Author = author;
            Binding = binding;
            Title = title;
            Logging.Log($"Created {this.GetType().Name} of \"{title}\" by {author} using {binding.Name}.");
        }
    }

    class MysteryBook : Book
    {
        public MysteryBook(string title, string author, Binding binding) : base(title, author, binding) { }
    }

    class FantasyBook : Book
    {
        public FantasyBook(string title, string author, Binding binding) : base(title, author, binding) { }
    }

    class ScienceFictionBook : Book
    {
        public ScienceFictionBook(string title, string author, Binding binding) : base(title, author, binding) { }
    }
}
