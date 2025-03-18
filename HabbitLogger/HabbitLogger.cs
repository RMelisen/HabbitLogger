using HabbitLogger.Commons.Classes;
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
            DAL.HabbitloggerDAL.InitializeDatabase();

            MainMenu.WelcomeUser();

            MainMenu.ShowMainMenu();
        }
    }
}