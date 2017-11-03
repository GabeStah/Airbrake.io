using System;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using Utility;

namespace Airbrake.Data.SqlTypes.SqlTypeException
{
    internal class Program
    {
        private const string ConnectionString = @"Data Source=I7\SQLEXPRESS;Initial Catalog=pubs;Integrated Security=True";

        private enum SqlCommandExecutionType
        {
            NonQuery,
            Reader,
            Scalar,
            XmlReader
        }

        private static void Main()
        {
            Logging.LineSeparator("INSERT VALID, PROPER DATE");
            ExecuteQuery(GetQueryStringFromBook(
                new Book("Magician", "Raymond E. Feist", 681, new DateTime(1982, 10, 1))
            ));

            Logging.LineSeparator("GET DATA");
            ExecuteQuery("SELECT * FROM dbo.Book;", SqlCommandExecutionType.Reader);
            
            Logging.LineSeparator("INSERT INVALID DATE");
            ExecuteQuery(GetQueryStringFromBook(
                new Book("Silverthorn", "Raymond E. Feist", 432, new DateTime(1750, 1, 1))
            ));

            Logging.LineSeparator("INSERT INVALID, CONVERTED DATE");
            ExecuteQuery(GetQueryStringFromBook(
                new Book("A Darkness At Sethanon", "Raymond E. Feist", 527, new DateTime(1750, 1, 1)), true)
            );
        }

        /// <summary>
        /// Executes the passed query string, using the passed SqlCommandExecutionType.
        /// </summary>
        /// <param name="query">Query string to execute.</param>
        /// <param name="type">SqlCommandExecutionType to use, if applicable.</param>
        private static void ExecuteQuery(string query, SqlCommandExecutionType type = SqlCommandExecutionType.NonQuery)
        {
            // Instantiate connection in using block to properly close afterward.
            using (var connection = new SqlConnection(ConnectionString))
            {
                // Instantiate a command.
                var command = new SqlCommand(query, connection);
 
                try
                {
                    connection.Open();
                    // If no command text, return.
                    if (command.CommandText == "") return;

                    // Check passed execution type.
                    switch (type)
                    {
                        case SqlCommandExecutionType.NonQuery:
                            command.ExecuteNonQuery();
                            break;
                        case SqlCommandExecutionType.Reader:
                            var reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                var data = new object[reader.FieldCount - 1];
                                reader.GetValues(data);
                                Logging.Log(data);
                            }
                            reader.Close();
                            break;
                        case SqlCommandExecutionType.Scalar:
                            command.ExecuteScalar();
                            break;
                        case SqlCommandExecutionType.XmlReader:
                            var xmlReader = command.ExecuteXmlReader();
                            while (xmlReader.Read())
                            {
                                Logging.Log(xmlReader);
                            }
                            xmlReader.Close();
                            break;
                        default:
                            command.ExecuteNonQuery();
                            break;
                    }
                }
                catch (System.Data.SqlTypes.SqlTypeException exception)
                {
                    // Output expected SqlTypeExceptions.
                    Logging.Log(exception);
                }
                catch (SqlException exception)
                {
                    // Output unexpected SqlExceptions.
                    Logging.Log(exception, false);
                }
                catch (Exception exception)
                {
                    // Output unexpected Exceptions.
                    Logging.Log(exception, false);
                }
            }
        }

        /// <summary>
        /// Create a query string from passed Book.
        /// </summary>
        /// <param name="book">Book from which to create query string.</param>
        /// <param name="shouldChangeDateType">Determines if date values should be converted to compatible type.</param>
        /// <returns></returns>
        private static string GetQueryStringFromBook(IBook book, bool shouldChangeDateType = false)
        {
            try
            {
                if (shouldChangeDateType)
                {
                    return "INSERT INTO dbo.Book (Title, Author, PageCount, PublicationDate) " +
                           $"VALUES ('{book.Title}', '{book.Author}', '{book.PageCount}', '{new SqlDateTime(book.PublicationDate.Value)}');";
                }
                return "INSERT INTO dbo.Book (Title, Author, PageCount, PublicationDate) " +
                       $"VALUES ('{book.Title}', '{book.Author}', '{book.PageCount}', '{book.PublicationDate}');";
            }
            catch (System.Data.SqlTypes.SqlTypeException exception)
            {
                // Output expected SqlTypeExceptions.
                Logging.Log(exception);
            }
            catch (SqlException exception)
            {
                // Output unexpected SqlExceptions.
                Logging.Log(exception, false);
            }
            catch (Exception exception)
            {
                // Output unexpected Exceptions.
                Logging.Log(exception, false);
            }
            return null;
        }
    }
}
