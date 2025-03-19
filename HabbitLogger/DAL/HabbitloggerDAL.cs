using HabbitLogger.Commons.Classes;
using HabbitLogger.UI;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using SQLitePCL;
using System.Data.Common;
using System.Reflection.PortableExecutable;
using System.Xml.Linq;

namespace HabbitLogger.DAL
{
    internal class HabbitloggerDAL
    {
        private readonly static string connectionString = @"Data Source=habbitLogger.db";
        internal static void InitializeDatabase()
        {
            Batteries.Init();   // Needed before opening a connection to the database

            using (SqliteConnection dbConnection = new SqliteConnection(connectionString))
            {
                dbConnection.Open();

                SqliteCommand dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = "PRAGMA foreign_keys = ON;";    // Mandatory to enable foreign key constraints (Seems disabled by default for Sqlite)
                dbCommand.ExecuteNonQuery();

                CreateDbTables(dbCommand);

                dbConnection.Close();   // Implicit because of the "using" statement
            }

            PopulateDbTables();
        }

        #region Initialization

        internal static void CreateDbTables(SqliteCommand dbCommand)
        {
            dbCommand.CommandText =
                @"CREATE TABLE IF NOT EXISTS unitsOfMeasure (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Name TEXT UNIQUE
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

        internal static void PopulateDbTables()
        {
            // Add some units of measure to the database
            InsertOrIgnoreUnitOfMeasure(1, "Glass");
            InsertOrIgnoreUnitOfMeasure(2, "Step");
            InsertOrIgnoreUnitOfMeasure(3, "Minutes");

            // Add some habbits to the database
            InsertOrIgnoreHabbit(1, "WATER_INTAKE", "Amount of water drunk", 1);
            InsertOrIgnoreHabbit(2, "STEPS_WALKED", "Amount of steps walked", 2);
            InsertOrIgnoreHabbit(3, "STUDY", "Time spent studying", 3);

            // Add some habbit occurences to the database
            InsertOrIgnoreHabbitOccurence(1, 1, 2, "2024-10-27 10:30:00");
            InsertOrIgnoreHabbitOccurence(2, 1, 3, "2024-10-30 11:41:00");
        }

        #endregion

        #region Habbits

        internal static void InsertHabbit(string name, string description, int unitOfMeasureId)
        {
            using (SqliteConnection dbConnection = new SqliteConnection(connectionString))
            {
                dbConnection.Open();

                SqliteCommand dbCommand = dbConnection.CreateCommand();

                dbCommand.CommandText = $@"INSERT INTO habbits (Name, Description, UnitOfMeasureID) VALUES ('{name}', '{description}', {unitOfMeasureId});";
                dbCommand.ExecuteNonQuery();
            }
        }

        internal static void InsertOrIgnoreHabbit(int id, string name, string description, int unitOfMeasureId)
        {
            using (SqliteConnection dbConnection = new SqliteConnection(connectionString))
            {
                dbConnection.Open();

                SqliteCommand dbCommand = dbConnection.CreateCommand();

                dbCommand.CommandText = $@"INSERT OR IGNORE INTO habbits (Id, Name, Description, UnitOfMeasureID) VALUES ({id}, '{name}', '{description}', {unitOfMeasureId});";
                dbCommand.ExecuteNonQuery();
            }
        }

        internal static void DeleteHabbit()
        {
            // TODO
        }

        internal static void UpdateHabbit()
        {
            // TODO
        }

        internal static Habbit? GetHabbitByID(int id)
        {
            using (SqliteConnection dbConnection = new SqliteConnection(connectionString))
            {
                dbConnection.Open();

                SqliteCommand dbCommand = dbConnection.CreateCommand();

                dbCommand.CommandText = $@"SELECT * FROM habbits WHERE Id = {id};";
                SqliteDataReader sqlDataReader = dbCommand.ExecuteReader();

                if (sqlDataReader.HasRows)
                {
                    sqlDataReader.Read();

                    return new Habbit(sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("Id")),
                        sqlDataReader.GetString(sqlDataReader.GetOrdinal("Name")),
                        sqlDataReader.GetString(sqlDataReader.GetOrdinal("Description")),
                        sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("UnitOfMeasureID"))
                        );
                }
                else
                {
                    AnsiConsole.MarkupLine($"No [{MainMenu.NEUTRAL_INDICATOR_COLOR}][[Habbit]][/] entry [{MainMenu.NEGATIVE_INDICATOR_COLOR}]found[/] !");
                    return null;
                }
            }
        }

        internal static List<Habbit> GetAllHabbits()
        {
            List<Habbit> habbitList = new List<Habbit>();

            using (SqliteConnection dbConnection = new SqliteConnection(connectionString))
            {
                dbConnection.Open();

                SqliteCommand dbCommand = dbConnection.CreateCommand();

                dbCommand.CommandText = $@"SELECT * FROM habbits;";
                SqliteDataReader sqlDataReader = dbCommand.ExecuteReader();

                if(sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        habbitList.Add(new Habbit(sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("Id")),
                            sqlDataReader.GetString(sqlDataReader.GetOrdinal("Name")),
                            sqlDataReader.GetString(sqlDataReader.GetOrdinal("Description")),
                            sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("UnitOfMeasureID"))
                            ));                        
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine($"No [{MainMenu.NEUTRAL_INDICATOR_COLOR}][[Habbit]][/] entry [{MainMenu.NEGATIVE_INDICATOR_COLOR}]found[/] !");
                }

                return habbitList;
            }
        }

        #endregion

        #region UnitOfMeasures

        internal static void InsertUnitOfMeasure(string name)
        {
            using (SqliteConnection dbConnection = new SqliteConnection(connectionString))
            {
                dbConnection.Open();
                SqliteCommand dbCommand = dbConnection.CreateCommand();

                dbCommand.CommandText = $@"INSERT INTO unitsOfMeasure (Name) VALUES ('{name}');";
                dbCommand.ExecuteNonQuery();
            }
        }

        internal static void InsertOrIgnoreUnitOfMeasure(int id, string name)
        {
            using (SqliteConnection dbConnection = new SqliteConnection(connectionString))
            {
                dbConnection.Open();
                SqliteCommand dbCommand = dbConnection.CreateCommand();

                dbCommand.CommandText = $@"INSERT OR IGNORE INTO unitsOfMeasure (Id, Name) VALUES ({id}, '{name}');";
                dbCommand.ExecuteNonQuery();
            }
        }

        internal static void DeleteUnitOfMeasure()
        {
            // TODO
        }

        internal static void UpdateUnitOfMeasure()
        {
            // TODO
        }

        internal static UnitOfMeasure? GetUnitOfMeasureByID(int id)
        {
            using (SqliteConnection dbConnection = new SqliteConnection(connectionString))
            {
                dbConnection.Open();

                SqliteCommand dbCommand = dbConnection.CreateCommand();

                dbCommand.CommandText = $@"SELECT * FROM unitsOfMeasure WHERE Id = {id};";
                SqliteDataReader sqlDataReader = dbCommand.ExecuteReader();

                if (sqlDataReader.HasRows)
                {
                    sqlDataReader.Read();

                    return new UnitOfMeasure(sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("Id")),
                        sqlDataReader.GetString(sqlDataReader.GetOrdinal("Name"))
                        );
                }
                else
                {
                    AnsiConsole.MarkupLine($"No [{MainMenu.NEUTRAL_INDICATOR_COLOR}][[UnitOfMeasure]][/] entry [{MainMenu.NEGATIVE_INDICATOR_COLOR}]found[/] !");
                    return null;
                }
            }
        }

        internal static List<UnitOfMeasure> GetAllUnitsOfMeasures()
        {
            List<UnitOfMeasure> unitOfMeasureList = new List<UnitOfMeasure>();

            using (SqliteConnection dbConnection = new SqliteConnection(connectionString))
            {
                dbConnection.Open();

                SqliteCommand dbCommand = dbConnection.CreateCommand();

                dbCommand.CommandText = $@"SELECT * FROM unitsOfMeasure;";
                SqliteDataReader sqlDataReader = dbCommand.ExecuteReader();

                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        unitOfMeasureList.Add(new UnitOfMeasure(sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("Id")),
                            sqlDataReader.GetString(sqlDataReader.GetOrdinal("Name"))
                            ));
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine($"No [{MainMenu.NEUTRAL_INDICATOR_COLOR}][[UnitOfMeasure]][/] entry [{MainMenu.NEGATIVE_INDICATOR_COLOR}]found[/] !");
                }

                return unitOfMeasureList;
            }
        }

        #endregion

        #region HabbitOccurenceOccurences

        internal static void InsertHabbitOccurence(int habbitId, int unitAmount, string datetime)
        {
            using (SqliteConnection dbConnection = new SqliteConnection(connectionString))
            {
                dbConnection.Open();

                SqliteCommand dbCommand = dbConnection.CreateCommand();

                dbCommand.CommandText = $@"INSERT INTO habbitOccurences (HabbitID, UnitAmount, Datetime) VALUES ({habbitId}, {unitAmount}, '{datetime}');";
                dbCommand.ExecuteNonQuery();
            }
        }

        internal static void InsertHabbitOccurence(int habbitId, int unitAmount, DateTime? datetime)
        {
            using (SqliteConnection dbConnection = new SqliteConnection(connectionString))
            {
                dbConnection.Open();

                SqliteCommand dbCommand = dbConnection.CreateCommand();

                dbCommand.CommandText = $@"INSERT INTO habbitOccurences (HabbitID, UnitAmount, Datetime) VALUES ({habbitId}, {unitAmount}, '{datetime.ToString()}');";
                dbCommand.ExecuteNonQuery();
            }
        }

        internal static void InsertOrIgnoreHabbitOccurence(int id, int habbitId, int unitAmount, string datetime)
        {
            using (SqliteConnection dbConnection = new SqliteConnection(connectionString))
            {
                dbConnection.Open();

                SqliteCommand dbCommand = dbConnection.CreateCommand();

                dbCommand.CommandText = $@"INSERT OR IGNORE INTO habbitOccurences (Id, HabbitID, UnitAmount, Datetime) VALUES ({id}, {habbitId}, {unitAmount}, '{datetime}');";
                dbCommand.ExecuteNonQuery();
            }
        }

        internal static void DeleteHabbitOccurence()
        {
            // TODO
        }

        internal static void UpdateHabbitOccurence()
        {
            // TODO
        }

        internal static HabbitOccurence? GetHabbitOccurenceByID(int id)
        {
            using (SqliteConnection dbConnection = new SqliteConnection(connectionString))
            {
                dbConnection.Open();

                SqliteCommand dbCommand = dbConnection.CreateCommand();

                dbCommand.CommandText = $@"SELECT * FROM habbitOccurences WHERE Id = {id};";
                SqliteDataReader sqlDataReader = dbCommand.ExecuteReader();

                if (sqlDataReader.HasRows)
                {
                    sqlDataReader.Read();

                    return new HabbitOccurence(sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("Id")),
                            sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("HabbitID")),
                            sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("UnitAmount")),
                            sqlDataReader.GetString(sqlDataReader.GetOrdinal("Datetime"))
                        );
                }
                else
                {
                    AnsiConsole.MarkupLine($"No [{MainMenu.NEUTRAL_INDICATOR_COLOR}][[HabbitOccurence]][/] entry [{MainMenu.NEGATIVE_INDICATOR_COLOR}]found[/] !");
                    return null;
                }
            }
        }

        internal static List<HabbitOccurence> GetAllHabbitOccurences()
        {
            List<HabbitOccurence> habbitOccurencesList = new List<HabbitOccurence>();

            using (SqliteConnection dbConnection = new SqliteConnection(connectionString))
            {
                dbConnection.Open();

                SqliteCommand dbCommand = dbConnection.CreateCommand();

                dbCommand.CommandText = $@"SELECT * FROM habbitOccurences;";
                SqliteDataReader sqlDataReader = dbCommand.ExecuteReader();

                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        habbitOccurencesList.Add(new HabbitOccurence(sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("Id")),
                            sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("HabbitID")),
                            sqlDataReader.GetInt32(sqlDataReader.GetOrdinal("UnitAmount")),
                            sqlDataReader.GetString(sqlDataReader.GetOrdinal("Datetime"))
                            ));
                    }
                }
                else
                {
                    AnsiConsole.MarkupLine($"No [{MainMenu.NEUTRAL_INDICATOR_COLOR}][[HabbitOccurence]][/] entry [{MainMenu.NEGATIVE_INDICATOR_COLOR}]found[/] !");
                }

                return habbitOccurencesList;
            }
        }

        #endregion
    }
}
