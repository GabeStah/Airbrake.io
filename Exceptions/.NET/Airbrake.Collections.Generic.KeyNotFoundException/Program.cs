using System;
using System.Collections.Generic;
using Utility;

namespace Airbrake.Collections.Generic.KeyNotFoundException
{
    /// <summary>
    /// Book interface.
    /// </summary>
    internal interface IBook
    {
        string Author { get; set; }
        string Title { get; set; }
    }

    /// <summary>
    /// Basic Book class.
    /// </summary>
    internal class Book : IBook
    {
        public string Author { get; set; }
        public string Title { get; set; }

        public Book(string title, string author)
        {
            Author = author;
            Title = title;
        }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            // Create Book Dictionary.
            var list = new List<Book>
            {
                new Book("The Stand", "Stephen King"),
                new Book("The Name of the Wind", "Patrick Rothfuss"),
                new Book("Robinson Crusoe", "Daniel Defoe"),
                new Book("The Hobbit", "J.R.R. Tolkien")
            };

            ListExample(list);
            Logging.LineSeparator();

            // Create Book Dictionary.
            var dictionary = new Dictionary<int, Book>
            {
                { 0, new Book("The Stand", "Stephen King") },
                { 1, new Book("The Name of the Wind", "Patrick Rothfuss") },
                { 2, new Book("Robinson Crusoe", "Daniel Defoe") },
                { 3, new Book("The Hobbit", "J.R.R. Tolkien") }
            };

            DictionaryExample(dictionary);
            Logging.LineSeparator();

            DictionaryUsingTryGetValueExample(dictionary);
            Logging.LineSeparator();

            // Create Book Dictionary.
            var improvedDictionary = new ImprovedDictionary<int, Book>
            {
                { 0, new Book("The Stand", "Stephen King") },
                { 1, new Book("The Name of the Wind", "Patrick Rothfuss") },
                { 2, new Book("Robinson Crusoe", "Daniel Defoe") },
                { 3, new Book("The Hobbit", "J.R.R. Tolkien") }
            };

            ImprovedDictionaryExample(improvedDictionary);
        }

        private static void ListExample(IReadOnlyList<Book> list)
        {
            try
            {
                // Output current library.
                Logging.Log(list);
                // Add line seperator for readability.
                Logging.LineSeparator();
                // Attempt to output element of index equal to count.
                Logging.Log(list[list.Count]);
            }
            catch (System.Collections.Generic.KeyNotFoundException exception)
            {
                // Catch KeyNotFoundExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Catch unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }

        private static void DictionaryExample(IReadOnlyDictionary<int, Book> dictionary)
        {
            try
            {
                // Output current library.
                Logging.Log(dictionary);
                // Add line seperator for readability.
                Logging.LineSeparator();
                // Attempt to output element of index equal to count.
                Logging.Log(dictionary[dictionary.Count]);
            }
            catch (System.Collections.Generic.KeyNotFoundException exception)
            {
                // Catch KeyNotFoundExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Catch unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }

        private static void DictionaryUsingTryGetValueExample(IReadOnlyDictionary<int, Book> dictionary)
        {
            try
            {
                // Output current library.
                Logging.Log(dictionary);
                // Add line seperator for readability.
                Logging.LineSeparator();
                // Attempt to output element of index equal to count.
                if (dictionary.TryGetValue(dictionary.Count, out var book))
                {
                    Logging.Log(book);
                }
                else
                {
                    Logging.Log($"Element at index [{dictionary.Count}] could not be found.");
                }
            }
            catch (System.Collections.Generic.KeyNotFoundException exception)
            {
                // Catch KeyNotFoundExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Catch unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }

        private static void ImprovedDictionaryExample(ImprovedDictionary<int, Book> dictionary)
        {
            try
            {
                // Output current library.
                Logging.Log(dictionary);
                // Add line seperator for readability.
                Logging.LineSeparator();
                // Attempt to output element of index equal to count.
                Logging.Log(dictionary[dictionary.Count]);
            }
            catch (System.Collections.Generic.KeyNotFoundException exception)
            {
                // Catch KeyNotFoundExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Catch unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }
    }

    /// <summary>
    /// Improves base Dictionary behavior by including missing key value on failed key retrieval.
    /// 
    /// Inherits from Dictionary.
    /// </summary>
    /// <typeparam name="TKey">Key type.</typeparam>
    /// <typeparam name="TValue">Value type.</typeparam>
    public class ImprovedDictionary<TKey, TValue> : Dictionary<TKey, TValue>
    {
        public new TValue this[TKey key]
        {
            get
            {
                // Try retrieving value by key and assign to value out variable.
                if (base.TryGetValue(key, out var value))
                {
                    return value;
                }

                // If failure throw new KeyNotFoundException, including missing key.
                throw new System.Collections.Generic.KeyNotFoundException(
                    $"The given key [{key}] was not present in the dictionary.");
            }
            set => base[key] = value;
        }
    }
}
