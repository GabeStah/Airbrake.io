using System;
using System.Data.SQLite;
using System.IO;
using Utility;

namespace Airbrake.Data.Linq.DuplicateKeyExceptionSqlite
{
    class Program
    {
        static void Main(string[] args)
        {
            var adapter = new SqliteAdapter();
        }
    }

    public interface IBook
    {
        string Author { get; set; }
        int PageCount { get; set; }
        string Title { get; set; }
    }

    public class Book : IBook
    {
        public string Author { get; set; }
        public int PageCount { get; set; }
        public string Title { get; set; }

        public Book() { }

        public Book(string title, string author, int pageCount)
        {
            Author = author;
            PageCount = pageCount;
            Title = title;
        }

        public override string ToString()
        {
            return $"'{Title}' by {Author} at {PageCount} pages";
        }
    }

    internal class SqliteAdapter
    {
        private SQLiteConnection Connection { get; }
        private const string DatabaseFilePath = "development.sqlite3";

        public SqliteAdapter()
        {
            // Create database.
            CreateDatabase();
            // Connect to database.
            Connection = new SQLiteConnection($"Data Source={DatabaseFilePath};Version=3");
            // Create book table.
            CreateBookTable();
            // Add default books to database.
            AddBooksToDatabase();
            // Add book with existing Id to database.
            AddBookToDatabase(new Book
            {
                Title = "Moby Dick",
                Author = "Herman Melville",
                PageCount = 752
            }, 1);
        }

        /// <summary>
        /// Add some basic books to the database.
        /// </summary>
        private void AddBooksToDatabase()
        {
            AddBookToDatabase(new Book
            {
                Title = "The Hobbit",
                Author = "J.R.R. Tolkien",
                PageCount = 304
            });
            AddBookToDatabase(new Book
            {
                Title = "The Shining",
                Author = "Stephen King",
                PageCount = 823
            });
            AddBookToDatabase(new Book
            {
                Title = "The Name of the Wind",
                Author = "Patrick Rothfuss",
                PageCount = 722
            });
        }

        /// <summary>
        /// Add passed IBook to database.
        /// </summary>
        /// <param name="book">Book to be added.</param>
        private void AddBookToDatabase(IBook book)
        {
            var sql = $"INSERT INTO [Book] (Title, Author, PageCount) VALUES ('{book.Title}', '{book.Author}', {book.PageCount})";
            ExecuteSql(sql);
            Logging.Log($"Book successfully added to database: {book}");
        }

        /// <summary>
        /// Add passed IBook to database, using passed identity.
        /// </summary>
        /// <param name="book">Book to be added.</param>
        /// <param name="id">Id to be used.</param>
        private void AddBookToDatabase(IBook book, int id)
        {
            var sql = $"INSERT INTO [Book] (Id, Title, Author, PageCount) VALUES ({id}, '{book.Title}', '{book.Author}', {book.PageCount})";
            ExecuteSql(sql);
            Logging.Log($"Book successfully added to database: {book}");
        }

        /// <summary>
        /// Creates Book table in database, if necessary.
        /// </summary>
        private void CreateBookTable()
        {
            const string sql = @"CREATE TABLE IF NOT EXISTS Book
                                (
                                    Id INTEGER PRIMARY KEY,
                                    Title TEXT NOT NULL,
                                    Author TEXT NOT NULL,
                                    PageCount INTEGER
                                );";
            ExecuteSql(sql);
        }

        /// <summary>
        /// Creates database at DatabaseFilePath, if necessary.
        /// </summary>
        private static void CreateDatabase()
        {
            if (!File.Exists(DatabaseFilePath))
                SQLiteConnection.CreateFile(DatabaseFilePath);
        }

        /// <summary>
        /// Executes passed SQL string using appropriate SQLiteExecuteType.
        /// </summary>
        /// <param name="sql">SQL string to execute.</param>
        /// <param name="executeType">Type of execution to use.</param>
        private async void ExecuteSql(string sql, SQLiteExecuteType executeType = SQLiteExecuteType.Default)
        {
            try
            {
                // Open connection, if necessary.
                if (Connection.State != System.Data.ConnectionState.Open)
                {
                    await Connection.OpenAsync();
                }
                // Create command from passed SQL string.
                var command = new SQLiteCommand(sql, Connection);
                // Check executeType parameter.
                switch (executeType)
                {
                    case SQLiteExecuteType.Default:
                        var rowsAffected = await command.ExecuteNonQueryAsync();
                        // Output number of affected rows.
                        Logging.Log($"Query complete: {rowsAffected} row(s) affected.");
                        break;
                    case SQLiteExecuteType.Reader:
                        var reader = await command.ExecuteReaderAsync();
                        while (await reader.ReadAsync())
                        {
                            // Retrieve values for each read row.
                            var values = new object[reader.FieldCount - 1];
                            reader.GetValues(values);
                            // Output values.
                            Logging.Log(values);
                        }
                        break;
                    case SQLiteExecuteType.Scalar:
                        await command.ExecuteScalarAsync();
                        break;
                    case SQLiteExecuteType.None:
                        break;
                    default:
                        // Throw exception if executeType value is something unexpected.
                        throw new ArgumentOutOfRangeException(nameof(executeType), executeType, null);
                }
            }
            catch (System.Data.Linq.DuplicateKeyException exception)
            {
                // Output expected DuplicateKeyException.
                Logging.Log(exception);
            }
            catch (SQLiteException exception)
            {
                // Output unexpected SQLiteException.
                Logging.Log(exception, false);
            }
        }
    }
}
