using System;
using System.Data;
using System.Data.SqlClient;
using Utility;

namespace Airbrake.Data.ObjectNotFoundException
{
    class Program
    {
        static void Main(string[] args)
        {
            // Find Customers with last name 'Miller'.
            Logging.LineSeparator("LastName: Miller");
            ExecuteQuery(
                @"SELECT TOP 20 
                    c.CustomerID, 
                    c.FirstName,
                    c.LastName,
                    c.CompanyName
                FROM
                    [SalesLT].[Customer] c
                WHERE
                    c.LastName = 'Miller'");

            // Find Customers with last name 'Bates'.
            Logging.LineSeparator("LastName: Bates");
            ExecuteQuery(
                @"SELECT TOP 20 
                    c.CustomerID, 
                    c.FirstName,
                    c.LastName,
                    c.CompanyName
                FROM 
                    [SalesLT].[Customer] c
                WHERE
                    c.LastName = 'Bates'");
        }

        /// <summary>
        /// Perform the passed SQL query.
        /// </summary>
        /// <param name="query">Query string to be executed.</param>
        private static void ExecuteQuery(string query)
        {
            try
            {
                // Generate connection string from application settings data.
                var stringBuilder = new SqlConnectionStringBuilder(Properties.Settings.Default.SqlConnectionString)
                {
                    UserID = Properties.Settings.Default.SqlUserID,
                    Password = Properties.Settings.Default.SqlPassword
                };

                // Use connection string to generate new SqlConnection.
                using (var connection = new SqlConnection(stringBuilder.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Generate a blank command using connection, then assign CommandText.
                    var command = new SqlCommand("", connection)
                    {
                        CommandType = CommandType.Text,
                        CommandText = query
                    };

                    // Execute command as SqlDataReader.
                    var reader = command.ExecuteReader();
                    // If no rows in data set, throw ObjectNotFoundException.
                    if (!reader.HasRows)
                        throw new System.Data.ObjectNotFoundException($"No results returned from query: {query}");

                    // Loop through reader to evaluate returned data.
                    while (reader.Read())
                    {
                        // Output row data to log.
                        var data = new object[reader.FieldCount];
                        reader.GetValues(data);
                        Logging.Log(data);
                    }

                    // Close reader.
                    reader.Close();

                    // Close connection.
                    connection.Close();
                }
            }
            catch (System.Data.ObjectNotFoundException exception)
            {
                // Output expected ObjectNotFoundExceptions.
                Logging.Log(exception);
            }
            catch (Exception exception)
            {
                // Output unexpected Exceptions.
                Logging.Log(exception, false);
            }
        }
    }

}
