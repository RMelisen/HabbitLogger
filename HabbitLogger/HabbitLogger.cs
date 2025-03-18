using HabbitLogger.UI;
using Microsoft.Data.Sqlite;
using SQLitePCL;
using System.Data.Common;

namespace HabbitLogger
{
    internal class HabbitLogger
    {
        private static void Main(string[] args)
        {
            string connectionString = @"Data Source=habbitLogger.db";

            Batteries.Init();   // Needed before opening a connection to the database

            using (SqliteConnection dbConnection = new SqliteConnection(connectionString))
            {
                dbConnection.Open();

                SqliteCommand dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = "PRAGMA foreign_keys = ON;";    // Mandatory to enable foreign key constraints (Seems disabled by default for Sqlite)
                dbCommand.ExecuteNonQuery();

                CreateDbTables(dbCommand);
                PopulateDbTables(dbCommand);

                dbConnection.Close();
            }

            MainMenu.WelcomeUser();
            MainMenu.ShowMainMenu();
        }

        private static void CreateDbTables(SqliteCommand dbCommand)
        {
            dbCommand.CommandText =
                @"CREATE TABLE IF NOT EXISTS unitsOfMeasure (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT
                    );";
            dbCommand.ExecuteNonQuery();

            dbCommand.CommandText =
                @"CREATE TABLE IF NOT EXISTS habbits (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT,
                    Description TEXT,
                    UnitOfMeasureID INT,
                    FOREIGN KEY (UnitOfMeasureID) REFERENCES unitsOfMeasure(Id)
                    );";
            dbCommand.ExecuteNonQuery();

            dbCommand.CommandText =
                @"CREATE TABLE IF NOT EXISTS habbitOccurences (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    HabbitID INT,
                    UnitAmount INTEGER,
                    Datetime DATETIME,
                    FOREIGN KEY (HabbitID) REFERENCES habbits(Id)
                    );";
            dbCommand.ExecuteNonQuery();
        }

        private static void PopulateDbTables(SqliteCommand dbCommand)
        {
            // Add some units of measure to the database
            dbCommand.CommandText = @"INSERT OR IGNORE INTO unitsOfMeasure (Id, Name) VALUES (1, 'Glass');";
            dbCommand.ExecuteNonQuery();

            dbCommand.CommandText = @"INSERT OR IGNORE INTO unitsOfMeasure (Id, Name) VALUES (2, 'Step');";
            dbCommand.ExecuteNonQuery();

            dbCommand.CommandText = @"INSERT OR IGNORE INTO unitsOfMeasure (Id, Name) VALUES (3, 'Minutes');";
            dbCommand.ExecuteNonQuery();

            // Add some habbits to the database
            dbCommand.CommandText = @"INSERT OR IGNORE INTO habbits (Id, Name, Description, UnitOfMeasureID) VALUES (1, 'WATER_INTAKE', 'Ammount of water drunk', 1);";
            dbCommand.ExecuteNonQuery();

            dbCommand.CommandText = @"INSERT OR IGNORE INTO habbits (Id, Name, Description, UnitOfMeasureID) VALUES (2, 'STEPS_WALKED', 'Ammount of steps walked', 2);";
            dbCommand.ExecuteNonQuery();

            dbCommand.CommandText = @"INSERT OR IGNORE INTO habbits (Id, Name, Description, UnitOfMeasureID) VALUES (3, 'STUDY', 'Time spent studying', 3);";
            dbCommand.ExecuteNonQuery();

            // Add some habbit occurences to the database
            dbCommand.CommandText = @"INSERT OR IGNORE INTO habbitOccurences (Id, HabbitID, UnitAmount, Datetime) VALUES (1, 1, 2, '2024-10-27 10:30:00');";
            dbCommand.ExecuteNonQuery();

            dbCommand.CommandText = @"INSERT OR IGNORE INTO habbitOccurences (Id, HabbitID, UnitAmount, Datetime) VALUES (2, 1, 3, '2024-10-30 11:41:00');";
            dbCommand.ExecuteNonQuery();
        }
    }
}