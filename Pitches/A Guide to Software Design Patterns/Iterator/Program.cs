// Program.cs
using Iterator.Advanced;
using Iterator.Basic;
using Utility;

namespace Iterator
{
    class Program
    {
        static void Main()
        {
            Logging.LineSeparator("BASIC ITERATOR");
            BasicIteratorTest();

            Logging.LineSeparator("ADVANCED ITERATOR");
            AdvancedIteratorTest();
        }

        public static void BasicIteratorTest()
        {
            var aggregate = new Aggregate();

            // Add Books to aggregate collection.
            aggregate.Add(new Book("The Hobbit", "J.R.R. Tolkien", 304));
            aggregate.Add(new Book("The Name of the Wind", "Patrick Rothfuss", 662));
            aggregate.Add(new Book("To Kill a Mockingbird", "Harper Lee", 281));
            aggregate.Add(new Book("1984", "George Orwell", 328));
            aggregate.Add(new Book("Jane Eyre", "Charlotte Brontë", 507));

            // Get new Iterator from aggregate.
            var iterator = aggregate.CreateIterator();
            // Loop while Next() element exists.
            while (iterator.Next())
            {
                // Output current object.
                Logging.Log(iterator.Current);
            }
        }

        public static void AdvancedIteratorTest()
        {
            // Create a new Advanced.Iterator<Book> collection.
            var collection = new Iterator<Book>
            {
                // Pass Books to Values property.
                Values = {
                    new Book("The Hobbit", "J.R.R. Tolkien", 304),
                    new Book("The Name of the Wind", "Patrick Rothfuss", 662),
                    new Book("To Kill a Mockingbird", "Harper Lee", 281),
                    new Book("1984", "George Orwell", 328) ,
                    new Book("Jane Eyre", "Charlotte Brontë", 507)
                }
            };

            // Iterate through collection and retrieve each Book.
            foreach (var book in collection)
            {
                // Output Book.
                Logging.Log(book);
            }
        }
    }
}
