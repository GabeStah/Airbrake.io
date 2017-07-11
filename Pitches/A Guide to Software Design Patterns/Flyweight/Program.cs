using System;
using System.Collections.Generic;
using Utility;

namespace Flyweight
{
    class Program
    {
        static void Main(string[] args)
        {
            Example1();
            Example2();
            Example3();
        }

        public static void Example1()
        {
            var library = new Library();
            var book = library.GetPublication(
                Tuple.Create(
                    new Author("Patrick Rothfuss"),
                    "The Name of the Wind",
                    PublicationType.Book
                )
            );

            var graphicNovel = library.GetPublication(
                Tuple.Create(
                    new Author("Julie Doucet"),
                    "My New York Diary",
                    PublicationType.GraphicNovel
                )
            );

            // Try retrieving Publication with same key.
            book = library.GetPublication(
                Tuple.Create(
                    new Author("Patrick Rothfuss"),
                    "The Name of the Wind",
                    PublicationType.Book
                )
            );

            Logging.Log($"Library contains [{library.GetPublicationCount}] publications.");
        }

        public static void Example2()
        {
            // Create library.
            var library = new Library();

            // Create Author instances.
            var patrickRothfuss = new Author("Patrick Rothfuss");
            var julieDoucet = new Author("Julie Doucet");

            // Create or retrieve new book.
            var book = library.GetPublication(
                Tuple.Create(
                    patrickRothfuss,
                    "The Name of the Wind",
                    PublicationType.Book
                )
            );

            var graphicNovel = library.GetPublication(
                Tuple.Create(
                    julieDoucet,
                    "My New York Diary",
                    PublicationType.GraphicNovel
                )
            );

            // Try retrieving Publication with same key.
            book = library.GetPublication(
                Tuple.Create(
                    patrickRothfuss,
                    "The Name of the Wind",
                    PublicationType.Book
                )
            );

            Logging.Log($"Library contains [{library.GetPublicationCount}] publications.");
        }

        public static void Example3()
        {
            var library = new Library();

            // Try to retrieve a Publication with an invalid PublicationType.
            library.GetPublication(
                Tuple.Create(
                    new Author("Dante"), 
                    "Divine Comedy", 
                    PublicationType.Epic
                )
            );
        }
    }
    
    public class Author
    {
        public string Name { get; set; }

        public Author(string name)
        {
            Name = name;
        }
    }

    public class Illustrator
    {
        public string Name { get; set; }

        public Illustrator(string name)
        {
            Name = name;
        }
    }

    public class Publisher
    {
        public string Name { get; set; }

        public Publisher(string name)
        {
            Name = name;
        }
    }

    /// <summary>
    /// Defines the allowed publication types.
    /// </summary>
    public enum PublicationType
    {
        Book,
        Epic,
        GraphicNovel
    }

    /// <summary>
    /// Acts as the Flyweight interface.
    /// </summary>
    public interface IPublication
    {
        Author Author { get; set; }
        Publisher Publisher { get; set; }
        string Title { get; set; }
    }

    /// <summary>
    /// Acts as a ConcreteFlyweight class.
    /// </summary>
    public class Book : IPublication
    {
        public Author Author { get; set; }
        public int PageCount { get; set; }
        public Publisher Publisher { get; set; }
        public string Title { get; set; }

        public Book(Author author, Publisher publisher, string title)
        {
            Author = author;
            Publisher = publisher;
            Title = title;
        }

        public Book(Author author, int pageCount, Publisher publisher, string title)
        {
            Author = author;
            PageCount = pageCount;
            Publisher = publisher;
            Title = title;
        }
    }

    /// <summary>
    /// Acts as a ConcreteFlyweight class.
    /// </summary>
    public class GraphicNovel : IPublication
    {
        public Author Author { get; set; }
        public Illustrator Illustrator { get; set; }
        public Publisher Publisher { get; set; }
        public string Title { get; set; }

        public GraphicNovel(Author author, Illustrator illustrator, Publisher publisher, string title)
        {
            Author = author;
            Illustrator = illustrator;
            Publisher = publisher;
            Title = title;
        }
    }

    /// <summary>
    /// Houses all publications.
    /// Storage uses Dictionary with Tuple key for author, title, and publication type.
    /// 
    /// Acts as FlyweightFactory.
    /// </summary>
    public class Library
    {
        /// <summary>
        /// Stores all publication data privately.  Should not be publically accessible 
        /// since we want to force access through GetPublication() method.
        /// </summary>
        protected Dictionary<Tuple<Author, string, PublicationType>, IPublication> Publications = 
            new Dictionary<Tuple<Author, string, PublicationType>, IPublication>();

        /// <summary>
        /// Get the count of all publications in library.
        /// </summary>
        public int GetPublicationCount => Publications.Count;

        /// <summary>
        /// Retrieve a Publication by passed key Tuple.
        /// If an item with matching key exists, retrieve from private Publications property.
        /// Otherwise, generate a new instance, add to list, and return result.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IPublication GetPublication(Tuple<Author, string, PublicationType> key)
        {
            IPublication publication = null;
            try
            {
                if (Publications.ContainsKey(key))
                {
                    publication = Publications[key];
                    // Output existing publication data.
                    Logging.LineSeparator();
                    Logging.Log($"Existing Publication located:");
                    Logging.Log(publication);
                }
                else
                {
                    switch (key.Item3)
                    {
                        case PublicationType.Book:
                            // Create a new Book (ConcreteFlyweight) example.
                            publication = new Book(
                                author: key.Item1,
                                pageCount: 662,
                                publisher: new Publisher("DAW Books"),
                                title: key.Item2
                            );
                            break;
                        case PublicationType.GraphicNovel:
                            // Create a new GraphicNovel (ConcreteFlyweight) example.
                            publication = new GraphicNovel(
                                author: key.Item1,
                                illustrator: new Illustrator(key.Item1.Name),
                                publisher: new Publisher("Drawn & Quarterly"),
                                title: key.Item2
                            );
                            break;
                        default:
                            throw new ArgumentException($"[PublicationType.{key.Item3}] is not configured.  Publication ('{key.Item2}' by {key.Item1.Name}) cannot be created.");
                    }
                    // Output new publication data.
                    Logging.LineSeparator();
                    Logging.Log($"New Publication created:");
                    Logging.Log(publication);
                    // Add new publication to global list.
                    Publications.Add(key, publication);
                }
            }
            catch (ArgumentException exception)
            {
                Logging.Log(exception);
            }
            // Return publication, whether newly-created or existing record.
            return publication;
        }
    }
}
