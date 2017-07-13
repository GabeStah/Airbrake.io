using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;

namespace CSharp7Features
{
    class Program
    {
        static void Main(string[] args)
        {
            TupleExamples();
            Logging.LineSeparator();
            OutExamples();
        }

        #region Tuples
        private static void TupleExamples()
        {
            CreateOldTuple();
            CreateLiteralTuple();
            InvokeGetTupleMethods();
        }

        private static void CreateOldTuple()
        {
            var book = new Tuple<string, string, int>("The Name of the Wind", "Patrick Rothfuss", 662);
            Logging.Log($"'{book.Item1}' by {book.Item2} [{book.Item3} pgs]");
        }

        private static void CreateLiteralTuple()
        {
            // Create literal tuple.
            var book = ("The Stand", "Stephen King", 823);
            Logging.Log($"'{book.Item1}' by {book.Item2} [{book.Item3} pgs]");

            var book2 = (title: "Seveneves", author: "Neal Stephenson", pageCount: 880);
            // Access by element names.
            Logging.Log($"'{book2.title}' by {book2.author} [{book2.pageCount} pgs]");
        }

        private static void InvokeGetTupleMethods()
        {
            var book = GetTupleLiteral();
            Logging.Log($"'{book.Item1}' by {book.Item2} [{book.Item3} pgs]");

            var book2 = GetTupleLiteralWithNames();
            Logging.Log($"'{book2.title}' by {book2.author} [{book2.pageCount} pgs]");
        }

        /// <summary>
        /// Get a tuple literal.
        /// </summary>
        /// <returns>Three-part tuple literal result.</returns>
        private static (string, string, int) GetTupleLiteral()
        {
            return ("Robinson Crusoe", "Daniel Defoe", 198);
        }

        /// <summary>
        /// Get a tuple literal with specified tuple element names.
        /// </summary>
        /// <returns>Three-part tuple literal result, with element names.</returns>
        private static (string title, string author, int pageCount) GetTupleLiteralWithNames()
        {
            return ("The Great Train Robbery", "Michael Crichton", 266);
        }
        #endregion

        #region Out Variables
        private static void OutExamples()
        {
            OldOutExample();
            OutVariableExample();
            OutVariableWithVarExample();
            OutVariableWithDiscardExample();
            DateTimeTryParseExample("today");
            DateTimeTryParseExample("1/1/2000");
            DateTimeTryParseWithDiscardExample("1/2/3456 12:34");
        }

        private static void GetBookData(out string title, out string author, out int pageCount)
        {
            title = "The Stand";
            author = "Stephen King";
            pageCount = 823;
        }

        private static void OldOutExample()
        {
            string author, title;
            int pageCount;
            GetBookData(out title, out author, out pageCount);
            Logging.Log($"'{title}' by {author} [{pageCount} pgs]");
        }

        private static void OutVariableExample()
        {
            GetBookData(out string title, out string author, out int pageCount);
            Logging.Log($"'{title}' by {author} [{pageCount} pgs]");
        }

        private static void OutVariableWithVarExample()
        {
            GetBookData(out var title, out var author, out var pageCount);
            Logging.Log($"'{title}' by {author} [{pageCount} pgs]");
        }

        private static void OutVariableWithDiscardExample()
        {
            GetBookData(out var title, out var author, out _);
            Logging.Log($"'{title}' by {author} [UNKNOWN pgs]");
        }

        private static void DateTimeTryParseExample(string value)
        {
            try
            {
                if (DateTime.TryParse(value, out var result))
                {
                    Logging.Log($"Value of '{value}' successfully converted to DateTime of {result}.");
                }
                else
                {
                    throw new ArgumentException($"Cannot parse passed value of '{value}' to DateTime.", nameof(value));
                }
            }
            catch (ArgumentException exception)
            {
                Logging.Log(exception);
            }
        }

        private static void DateTimeTryParseWithDiscardExample(string value)
        {
            try
            {
                // Use discard for out variable.
                if (DateTime.TryParse(value, out _))
                {
                    Logging.Log($"Value of '{value}' successfully converted to DateTime!");
                }
                else
                {
                    throw new ArgumentException($"Cannot parse passed value of '{value}' to DateTime.", nameof(value));
                }
            }
            catch (ArgumentException exception)
            {
                Logging.Log(exception);
            }
        }
        #endregion
    }
}
