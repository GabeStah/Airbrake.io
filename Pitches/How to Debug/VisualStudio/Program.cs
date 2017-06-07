using System;
using System.Diagnostics;
using Utility;
using StringDialogVisualizer;

namespace VisualStudio
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                String myString = "Hello, World";
                DebuggerSide.TestShowVisualizer(myString);
                Book book = new Book("Green Eggs and Ham", "Dr. Seuss");
                Book book2 = new Book("This is a thing", "something");
                throw new Exception("blah");
                Logging.Log(book);
            }
            catch (Exception exception)
            {
                Logging.Log(exception);
            }
        }

        [DebuggerTypeProxy(typeof(BookDebugView))]
        [DebuggerDisplay("{Title} by {Author}")]
        public class Book
        {
            private string _title { get; set; }
            private string _author { get; set; }

            public Book(string title, string author)
            {
                _title = title;
                _author = author;
            }

            public String Author
            {
                get { return _author; }
                set { _author = value; }
            }

            public String Title
            {
                get { return _title; }
                set { _title = value; }
            }

            internal class BookDebugView
            {
                private Book _book;

                public String Author
                {
                    get { return _book.Author; }
                    set { _book.Author = value; }
                }

                public String Title
                {
                    get { return _book.Title; }
                    set { _book.Title = value; }
                }

                public BookDebugView(Book book)
                {
                    _book = book;
                }
            }
        }
    }
}
