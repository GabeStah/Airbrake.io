using System.Data;
using System.Data.SqlClient;
using Utility;

namespace Airbrake.Data.SqlClient.SqlException
{
    class Program
    {
        static void Main(string[] args)
        {
            // Valid query.
            PerformQuery(@"SELECT TOP 20 pc.Name as CategoryName, p.name as ProductName
                FROM [SalesLT].[ProductCategory] pc
                JOIN [SalesLT].[Product] p
                ON pc.productcategoryid = p.productcategoryid");

            // Invalid query.
            PerformQuery(@"EXECUTE InvalidStoredProcedure");
        }

        private static void PerformQuery(string query)
        {
            try
            {
                // Generate connection string from application settings data.
                SqlConnectionStringBuilder stringBuilder = new SqlConnectionStringBuilder(Properties.Settings.Default.SqlConnectionString)
                {
                    UserID = Properties.Settings.Default.SqlUserID,
                    Password = Properties.Settings.Default.SqlPassword
                };

                // Use connection string to generate new SqlConnection.
                using (SqlConnection connection = new SqlConnection(stringBuilder.ConnectionString))
                {
                    // Open the connection.
                    connection.Open();

                    // Generate a blank command using connection, then assign CommandText.
                    SqlCommand command = new SqlCommand("", connection)
                    {
                        CommandType = CommandType.Text,
                        CommandText = query
                    };

                    // Execute command as SqlDataReader.
                    SqlDataReader reader = command.ExecuteReader();

                    // Loop through reader to evaluate returned data.
                    while (reader.Read())
                    {
                        // Output data.
                        Logging.Log($"{reader.GetString(0)}\t{reader.GetString(1)}");
                    }

                    // Close reader.
                    reader.Close();

                    // Close connection.
                    connection.Close();
                }
            }
            catch (System.Data.SqlClient.SqlException exception)
            {
                // Output exception information to log.
                Logging.Log(exception);
            }
        }
    }
}
