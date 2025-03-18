using Microsoft.Data.Sqlite;

internal class Program
{
    private static void Main(string[] args)
    {
        string connectionString = @"Data Source=habbitLogger.db";

        using (SqliteConnection dbConnection = new SqliteConnection(connectionString))
        {
            dbConnection.Open();

            SqliteCommand tableCommand = dbConnection.CreateCommand();
            
            tableCommand.CommandText = 
                @"CREATE TABLE IF NOT EXISTS habbit (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Importance INTEGER)";
            
            tableCommand.ExecuteNonQuery();

            dbConnection.Close();
        }
    }
}