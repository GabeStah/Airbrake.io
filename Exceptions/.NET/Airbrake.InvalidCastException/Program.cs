using System;
using Utility;

namespace Airbrake.InvalidCastException
{
    class Program
    {
        static void Main(string[] args)
        {
            int value = 10;
            decimal dec = (decimal)value;
            Logging.Log($"Cast succeeded from {value.GetType().Name} to {dec.GetType().Name}.");
            CastToString();
            Logging.LineSeparator();
            CallToStringMethod();
            Logging.LineSeparator();
            CastToDerivedType();
            Logging.LineSeparator();
            CastToBaseType();
            Logging.LineSeparator();
            ConvertBoolToChar();
            Logging.LineSeparator();
            ConvertIntToChar();
        }

        private static void ConvertBoolToChar()
        {
            try
            {
                bool value = true;
                IConvertible converter = value;
                Char character = converter.ToChar(null);
                Logging.Log($"Conversion from Bool to Char succeeded.");
            }
            catch (System.InvalidCastException exception)
            {
                Logging.Log("Conversion failed.");
                Logging.Log(exception);
            }
        }

        private static void ConvertIntToChar()
        {
            try
            {
                int value = 25;
                IConvertible converter = value;
                Char character = converter.ToChar(null);
                Logging.Log($"Conversion from Int to Char succeeded.");
            }
            catch (System.InvalidCastException exception)
            {
                Logging.Log("Conversion failed.");
                Logging.Log(exception);
            }
        }

        private static void CallToStringMethod()
        {
            try
            {
                object age = 30;
                string convertedAge = age.ToString();
                Logging.Log($"Converted age is: {convertedAge}.");
            }
            catch (System.InvalidCastException exception)
            {
                Logging.Log("ToString() method conversion failed.");
                Logging.Log(exception);
            }
        }

        private static void CastToString()
        {
            try
            {
                object age = 30;
                string convertedAge = (string)age;
                Logging.Log($"Converted age is: {convertedAge}.");
            }
            catch (System.InvalidCastException exception)
            {
                Logging.Log("String cast failed.");
                Logging.Log(exception);
            }
        }

        private static void CastToBaseType()
        {
            try
            {
                var publishedBook = new PublishedBook("It", "Stephen King", new DateTime(1986, 9, 1));

                // Upcasting PublishedBook to base type of Book.
                Book castBook = (Book)publishedBook;
                Logging.Log("Upcasting successful.");
                Logging.Log(castBook);
            }
            catch (System.InvalidCastException exception)
            {
                Logging.Log("Upcasting failed.");
                Logging.Log(exception);
            }
        }

        private static void CastToDerivedType()
        {
            try
            {
                var book = new Book("Sword in the Darkness", "Stephen King");

                // Downcasting PublishedBook to base type of Book.
                var castBook = (PublishedBook)book;
                Logging.Log("Downcasting successful:");
                Logging.Log(castBook);
            }
            catch (System.InvalidCastException exception)
            {
                Logging.Log("Downcasting failed.");
                Logging.Log(exception);
            }
        }
    }

    internal class Book
    {
        public string Title { get; set; }
        public string Author { get; set; }

        public Book(string title, string author)
        {
            Title = title;
            Author = author;
        }
    }

    internal class PublishedBook : Book
    {
        public DateTime PublishedAt { get; set; }

        public PublishedBook(string title, string author, DateTime published_at)
            : base(title, author)
        {
            Title = title;
            Author = author;
            PublishedAt = published_at;
        }
    }
}
