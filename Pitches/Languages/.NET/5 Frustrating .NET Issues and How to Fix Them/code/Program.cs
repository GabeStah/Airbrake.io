using System;
using System.Linq;
using System.Collections.Generic;

namespace code
{
    class Program
    {
        static void Main(string[] args)
        {
            // StringExample();

            // QueueExample();

            // ParseExample();

            // CompilerExample();

            LINQExample();
        }

        static void StringExample()
        {
            string old = "encyclopædia";
            string modern = "encyclopaedia";

            // False outputs.
            Log(old == modern);
            Log(old.Equals(modern));
            Log(old.Equals(modern, StringComparison.Ordinal));
            Log(old.Equals(modern, StringComparison.OrdinalIgnoreCase));

            // True outputs.
            Log(old.Equals(modern, StringComparison.CurrentCulture));
            Log(old.Equals(modern, StringComparison.CurrentCultureIgnoreCase));
        }

        static void QueueExample()
        {
            Queue<string> callers = new Queue<string>();
            callers.Enqueue("Adam");
            callers.Enqueue("Barbara");
            callers.Enqueue("Chris");
            callers.Enqueue("Danielle");
            callers.Enqueue("Eric");

            foreach(string caller in callers)
            {
                Log(caller);
            }

            Log("-----------------");

            Log($"Next item to dequeue: {callers.Peek()}");
            Log($"Dequeuing {callers.Dequeue()}");
            Log($"Next item to dequeue: {callers.Peek()}");
            Log($"Dequeuing {callers.Dequeue()}");
        }

        static void ParseExample()
        {
            string[] values = { "123", "-123", "123.0", "01AD" };

            foreach (var value in values)
            {
                Log($"TryParse of {value} is: {Int32.TryParse(value, out int number)}");
            }

            try
            {
                foreach (var value in values)
                {
                    Log($"Parse of {value} is: {Int32.Parse(value)}");
                }
            }
            catch (System.FormatException e)
            {
                Log(e);
            }
        }

        static void CompilerExample()
        {
            Author author = new Author(666, "Stephen King");
            Log($"Id is {author.id} for {author.name}.");
        }

        static void LINQExample()
        {
            List<Author> authors = new List<Author>();

            authors.Add(new Author(1, "Stephen King", "published"));
            authors.Add(new Author(2, "Herman Melville", "Published"));
            authors.Add(new Author(3, "John Doe", "not published"));

            int publishedCount = (from author in authors
                                  where author.publicationStatus == "published"
                                  select author).Count();

            Log($"Number of published authors: {publishedCount}");
        }

        static void Log(object value)
        {
            #if DEBUG
                System.Diagnostics.Debug.WriteLine(value);
            #else
                Console.WriteLine(value);
            #endif
        }
    }

    public class Author
    {
        public int id;
        public string name;
        public string publicationStatus;

        public Author(int id, string name, string publicationStatus = "published")
        {
            this.id = id;
            this.name = name;
            this.publicationStatus = publicationStatus;
        }
    }
}