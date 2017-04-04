using Utility;

namespace Airbrake.DesignPatterns.AbstractFactory
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a PoemFactory, generating a Poem book type and a Blog publisher type.
            new PoemFactory(author: "Edgar Allan Poe", 
                            title: "The Raven",
                            publisher: "The American Review");

            // Create a ResearchPaperFactory, generating a ResearchPaper book type
            // and a ScientificJournal publisher type.
            new ResearchPaperFactory(author: "Charles Darwin", 
                                     title: "On the Origin of Species", 
                                     publisher: "John Murray");
        }
    }

    public interface IBook
    {
        string Author { get; set; }
        string Title { get; set; }
    }

    public class Poem : IBook
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public Poem(string author, string title)
        {
            Author = author;
            Title = title;
            Logging.Log($"Made an IBook of type: {this.ToString()}.");
        }
    }

    public class ResearchPaper : IBook
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public ResearchPaper(string author, string title)
        {
            Author = author;
            Title = title;
            Logging.Log($"Made an IBook of type: {this.ToString()}.");
        }
    }

    public interface IPublisher
    {
        string Name { get; set; }
    }

    public class Blog : IPublisher
    {
        public string Name { get; set; }
        public Blog(string name)
        {
            Name = name;
            Logging.Log($"Made an IPublisher of type: {this.ToString()}.");
        }
    }

    public class ScientificJournal : IPublisher
    {
        public string Name { get; set; }
        public ScientificJournal(string name)
        {
            Name = name;
            Logging.Log($"Made an IPublisher of type: {this.ToString()}.");
        }
    }

    public interface IBookFactory
    {
        IBook MakeBook(string author, string title);
        IPublisher MakePublisher(string name);
    }

    public class PoemFactory : IBookFactory
    {
        public PoemFactory() { }

        public PoemFactory(string author, string title, string publisher)
        {
            MakeBook(author: author, title: title);
            MakePublisher(name: publisher);
            Logging.Log($"Made an IBookFactory of type: {this.ToString()}.");
        }

        public IBook MakeBook(string author, string title)
        {
            return new Poem(author: author, title: title);
        }
        
        public IPublisher MakePublisher(string name)
        {
            return new Blog(name: name);
        }
    }

    public class ResearchPaperFactory : IBookFactory
    {
        public ResearchPaperFactory() { }

        public ResearchPaperFactory(string author, string title, string publisher)
        {
            MakeBook(author: author, title: title);
            MakePublisher(name: publisher);
            Logging.Log($"Made an IBookFactory of type: {this.ToString()}.");
        }

        public IBook MakeBook(string author, string title)
        {
            return new ResearchPaper(author: author, title: title);
        }

        public IPublisher MakePublisher(string name)
        {
            return new ScientificJournal(name: name);
        }
    }
}
