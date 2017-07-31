using System;
using Utility;
using Microsoft.VisualBasic;

namespace Airbrake.NotImplementedException
{
    class Program
    {
        static void Main(string[] args)
        {
            Example();
            PlatformExample();
            PublisherExample();
        }

        private static void PublisherExample()
        {
            try
            {
                var blog = new Blog("My Cool Cat Blog");
                Logging.Log(blog.Revenue);
            }
            catch (System.NotSupportedException exception)
            {
                Logging.Log(exception);
            }
        }

        private static void PlatformExample()
        {
            try
            {
                var book = new Book("Moby Dick", "Herman Melville");
                Logging.Log(book);
                Logging.Log(book.PageCount());
            }
            catch (System.PlatformNotSupportedException exception)
            {
                Logging.Log(exception);
            }
        }

        private static void Example()
        {
            try
            {
                var book = new Book("The Stand", "Stephen King");
                Logging.Log(book);
                Logging.Log(book.PublicationDate());
            }
            catch (System.NotImplementedException exception)
            {
                Logging.Log(exception);
            }
        }
    }

    internal interface IBook
    {
        string Author { get; set; }
        string Title { get; set; }
    }

    internal class Platforms
    {
        public const string Windows7    = "Windows NT 6.1";
        public const string Windows8    = "Windows NT 6.2";
        public const string Windows8_1  = "Windows NT 6.3";
        public const string Windows10   = "Windows NT 10";
    }

    internal class Book : IBook
    {
        public string Title { get; set; }
        public string Author { get; set; }

        public Book() { }

        public Book(string title, string author)
        {
            Title = title;
            Author = author;
        }

        public DateTime PublicationDate()
        {
            // Get namespace and class type.
            var type = this.GetType().FullName;
            // Get current method name.
            var methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
            // Throw NotImplementedException.
            throw new System.NotImplementedException($"{type}.{methodName} is not yet implemented.");
        }

        public int PageCount()
        {
            // Get current platform.
            var platform = Environment.OSVersion;

            // Check if platform is unsupported.
            if (platform.ToString().Contains(Platforms.Windows7))
            {
                // Get namespace and class type.
                var type = this.GetType().FullName;
                // Get current method name.
                var methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                // Throw NotImplementedException.
                throw new System.PlatformNotSupportedException($"{type}.{methodName} does not support the current platform: {platform} ({nameof(Platforms.Windows7)}).");
            }
            return 0;
        }
    }

    internal abstract class Publisher
    {
        public abstract string Name { get; set; }
        public abstract decimal Revenue { get; set; }
    }

    internal class Blog : Publisher
    {
        public override string Name { get; set; }

        public Blog(string name)
        {
            Name = name;
        }

        public override decimal Revenue
        {
            get
            {
                // Get namespace and class type.
                var type = this.GetType().FullName;
                // Get current method name.
                var methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                // Throw NotImplementedException.
                throw new System.NotSupportedException($"{type} does not support the {methodName} method.");
            }
            set
            {
                // Get namespace and class type.
                var type = this.GetType().FullName;
                // Get current method name.
                var methodName = System.Reflection.MethodBase.GetCurrentMethod().Name;
                // Throw NotImplementedException.
                throw new System.NotSupportedException($"{type} does not support the {methodName} method.");
            }
        }
    }
}
