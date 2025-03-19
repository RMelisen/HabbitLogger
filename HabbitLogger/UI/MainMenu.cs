using HabbitLogger.Enums;
using Spectre.Console;
using HabbitLogger.Commons.Classes;
using HabbitLogger.DAL;
using System.Globalization;

namespace HabbitLogger.UI
{
    internal class MainMenu
    {
        #region CONST

        public static readonly Color NEUTRAL_INDICATOR_COLOR = Color.Blue;
        public static readonly Color NEGATIVE_INDICATOR_COLOR = Color.Red;
        public static readonly Color POSITIVE_INDICATOR_COLOR = Color.Green;

        #endregion

        internal static void WelcomeUser()
        {
            AnsiConsole.MarkupLine($"[{NEUTRAL_INDICATOR_COLOR} Bold]Welcome to HabbitLogger ![/]\n");
        }

        internal static void ShowMainMenu()
        {
            bool shouldLoopMainMenu = true;
            MainMenuOption mainMenuChoice;

            while (shouldLoopMainMenu)
            {
                mainMenuChoice = AnsiConsole.Prompt(new SelectionPrompt<MainMenuOption>().Title($"What do you want to do ?").AddChoices(Enum.GetValues<MainMenuOption>()));

                switch (mainMenuChoice)
                {
                    case MainMenuOption.Insert:
                        switch (SelectTable("insert"))
                        {
                            case ViewTablesOption.Habbits:
                                InsertHabbit();
                                break;
                            case ViewTablesOption.UnitOfMeasures:
                                InsertUnitOfMeasure();
                                break;
                            case ViewTablesOption.HabbitOccurences:
                                InsertHabbitOccurence();
                                break;
                            case ViewTablesOption.Back:
                                break;
                        }
                        break;
                    case MainMenuOption.Delete:
                        switch (SelectTable("delete"))
                        {
                            case ViewTablesOption.Habbits:
                                DeleteHabbit();
                                break;
                            case ViewTablesOption.UnitOfMeasures:
                                DeleteUnitOfMeasure();
                                break;
                            case ViewTablesOption.HabbitOccurences:
                                DeleteHabbitOccurence();
                                break;
                            case ViewTablesOption.Back:
                                break;
                        }
                        break;
                    case MainMenuOption.Update:
                        break;
                    case MainMenuOption.View:
                        switch (SelectTable("view"))
                        {
                            case ViewTablesOption.Habbits:
                                ViewHabbits();
                                break;
                            case ViewTablesOption.UnitOfMeasures:
                                ViewUnitsOfMeasure();
                                break;
                            case ViewTablesOption.HabbitOccurences:
                                ViewHabbitOccurences();
                                break;
                            case ViewTablesOption.Back:
                                break;
                        }
                        break;
                    case MainMenuOption.Quit:
                        shouldLoopMainMenu = false;
                        break;
                }
                AnsiConsole.Clear();
            }
            AnsiConsole.MarkupLine($"[Bold]See you soon ![/]");
        }

        #region View Tables

        private static void ViewHabbits()
        {
            Table habbitsTable = new Table();
            habbitsTable.Border(TableBorder.Rounded);

            habbitsTable.AddColumn($"[{NEUTRAL_INDICATOR_COLOR}]Id[/]");
            habbitsTable.AddColumn($"[{NEUTRAL_INDICATOR_COLOR}]Name[/]");
            habbitsTable.AddColumn($"[{NEUTRAL_INDICATOR_COLOR}]Description[/]");
            habbitsTable.AddColumn($"[{NEUTRAL_INDICATOR_COLOR}]Unit of measure[/]");

            // Populate rows
            foreach (Habbit habbit in HabbitloggerDAL.GetAllHabbits())
            {
                habbitsTable.AddRow(habbit.Id.ToString(), habbit.Name, habbit.Description, habbit.UnitOfMeasure.Name);
            }

            AnsiConsole.MarkupLine($"[{NEUTRAL_INDICATOR_COLOR} Bold]Habbits :[/]");
            AnsiConsole.Write(habbitsTable);
            AnsiConsole.MarkupLine($"Press any key to [{NEUTRAL_INDICATOR_COLOR}]continue[/]...");
            Console.ReadKey();
        }

        private static void ViewUnitsOfMeasure()
        {
            Table unitsOfMeasureTable = new Table();
            unitsOfMeasureTable.Border(TableBorder.Rounded);

            unitsOfMeasureTable.AddColumn($"[{NEUTRAL_INDICATOR_COLOR}]Id[/]");
            unitsOfMeasureTable.AddColumn($"[{NEUTRAL_INDICATOR_COLOR}]Name[/]");

            // Populate rows
            foreach (UnitOfMeasure unitOfMeasure in HabbitloggerDAL.GetAllUnitsOfMeasures())
            {
                unitsOfMeasureTable.AddRow(unitOfMeasure.Id.ToString(), unitOfMeasure.Name);
            }

            AnsiConsole.MarkupLine($"[{NEUTRAL_INDICATOR_COLOR} Bold]Units of Measure :[/]");
            AnsiConsole.Write(unitsOfMeasureTable);
            AnsiConsole.MarkupLine($"Press any key to [{NEUTRAL_INDICATOR_COLOR}]continue[/]...");
            Console.ReadKey();
        }

        private static void ViewHabbitOccurences()
        {
            Table habbitOccurencesTable = new Table();
            habbitOccurencesTable.Border(TableBorder.Rounded);

            habbitOccurencesTable.AddColumn($"[{NEUTRAL_INDICATOR_COLOR}]Id[/]");
            habbitOccurencesTable.AddColumn($"[{NEUTRAL_INDICATOR_COLOR}]Habbit Description[/]");
            habbitOccurencesTable.AddColumn($"[{NEUTRAL_INDICATOR_COLOR}]Unit Amount[/]");
            habbitOccurencesTable.AddColumn($"[{NEUTRAL_INDICATOR_COLOR}]Date[/]");

            // Populate rows
            foreach (HabbitOccurence habbitOccurence in HabbitloggerDAL.GetAllHabbitOccurences())
            {
                habbitOccurencesTable.AddRow(habbitOccurence.Id.ToString(), habbitOccurence.Habbit.Description, habbitOccurence.UnitAmount.ToString() + " " + habbitOccurence.Habbit.UnitOfMeasure.Name, habbitOccurence.Datetime.ToString());
            }

            AnsiConsole.MarkupLine($"[{NEUTRAL_INDICATOR_COLOR} Bold]Habbit Occurences :[/]");
            AnsiConsole.Write(habbitOccurencesTable);
            AnsiConsole.MarkupLine($"Press any key to [{NEUTRAL_INDICATOR_COLOR}]continue[/]...");
            Console.ReadKey();
        }

        #endregion

        #region Insert in Table

        private static void InsertHabbit()
        {
            AnsiConsole.Clear();

            string name = AnsiConsole.Ask<string>($"Enter an habbit [{NEUTRAL_INDICATOR_COLOR}]name[/] : ");
            string description = AnsiConsole.Ask<string>($"Enter the habbit [{NEUTRAL_INDICATOR_COLOR}]description[/] : ");
            UnitOfMeasure unitOfMeasure = AnsiConsole.Prompt(new SelectionPrompt<UnitOfMeasure>().Title($"Select a [{NEUTRAL_INDICATOR_COLOR}]unit of measure[/]").AddChoices(HabbitloggerDAL.GetAllUnitsOfMeasures()));

            HabbitloggerDAL.InsertHabbit(name, description, unitOfMeasure.Id);
        }

        private static void InsertUnitOfMeasure()
        {
            AnsiConsole.Clear();

            string name = AnsiConsole.Ask<string>($"Enter a [{NEUTRAL_INDICATOR_COLOR}]unit of measure[/] : ");

            HabbitloggerDAL.InsertUnitOfMeasure(name);
        }

        private static void InsertHabbitOccurence()
        {
            AnsiConsole.Clear();

            Habbit habbit = AnsiConsole.Prompt(new SelectionPrompt<Habbit>().Title($"Select a [{NEUTRAL_INDICATOR_COLOR}]habbit[/]").AddChoices(HabbitloggerDAL.GetAllHabbits()));
            int unitAmount = AnsiConsole.Ask<int>($"Enter an [{NEUTRAL_INDICATOR_COLOR}]amount[/] of [{NEUTRAL_INDICATOR_COLOR}]{habbit.UnitOfMeasure.Name}[/] : ");
            DateTime? datetime = GetDateFromUser();
            while (datetime == null)
            {
                AnsiConsole.MarkupLine($"[{NEGATIVE_INDICATOR_COLOR}]Wrong format ![/]");
                datetime = GetDateFromUser();
            }

            HabbitloggerDAL.InsertHabbitOccurence(habbit.Id, unitAmount, datetime);
        }

        #endregion

        #region Delete in Table

        private static void DeleteHabbit()
        {
            int idToDelete = AnsiConsole.Ask<int>($"Enter the [{NEUTRAL_INDICATOR_COLOR}]id[/] of the habbit to [{NEGATIVE_INDICATOR_COLOR}]delete[/] (0 to quit) : ");

            if (idToDelete == 0)
                return;

            HabbitloggerDAL.DeleteHabbitById(idToDelete);
        }

        private static void DeleteUnitOfMeasure()
        {
            int idToDelete = AnsiConsole.Ask<int>($"Enter the [{NEUTRAL_INDICATOR_COLOR}]id[/] of the unit of measure to [{NEGATIVE_INDICATOR_COLOR}]delete[/] (0 to quit) : ");

            if (idToDelete == 0)
                return;

            HabbitloggerDAL.DeleteUnitOfMeasureById(idToDelete);
        }

        private static void DeleteHabbitOccurence()
        {
            int idToDelete = AnsiConsole.Ask<int>($"Enter the [{NEUTRAL_INDICATOR_COLOR}]id[/] of the habbit occurence to [{NEGATIVE_INDICATOR_COLOR}]delete[/] (0 to quit) : ");

            if (idToDelete == 0)
                return;

            HabbitloggerDAL.DeleteHabbitOccurenceById(idToDelete);
        }

        #endregion

        #region Update in Table


        #endregion

        #region Utils

        internal static ViewTablesOption SelectTable(string operation)
        {
            string viewTablesOptionChoiceString;

            //  Retrieves all enum values, casts them to Enum, gets their descriptions, and converts them to an array of strings.
            var options = Enum.GetValues(typeof(ViewTablesOption))
                .Cast<ViewTablesOption>()
                .Select(e => e.GetDescription())
                .ToArray();

            // Use the string[] to display the description attribute of the enum
            // I do this because I want to use the description instead of the raw value in my Spectre.Console.Prompt()
            AnsiConsole.Clear();
            viewTablesOptionChoiceString = AnsiConsole.Prompt(new SelectionPrompt<string>().Title($"What do you want to [{NEUTRAL_INDICATOR_COLOR}]{operation}[/] ?").AddChoices(options));

            // Convert the chosen option back to an enum value
            return Enum.GetValues(typeof(ViewTablesOption))
                .Cast<ViewTablesOption>()
                .FirstOrDefault(e => e.GetDescription() == viewTablesOptionChoiceString);
        }

        public static DateTime? GetDateFromUser()
        {
            string dateString = AnsiConsole.Prompt(new TextPrompt<string>($"Enter a [{NEUTRAL_INDICATOR_COLOR}]date and time[/] (yyyy-MM-dd HH:mm) [['t' for now]]: "));

            string[] formats = {"yyyy-MM-dd HH:mm", "yyyyMMdd HH:mm", "yyyy-MM-dd", "yyyyMMdd" };
            DateTime parsedDate;

            if (dateString == "t" || dateString == "now" ||dateString == "n")
                return DateTime.Now;

            if (DateTime.TryParseExact(dateString, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDate))
            {
                return parsedDate;
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}
