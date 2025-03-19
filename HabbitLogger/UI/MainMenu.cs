using HabbitLogger.Enums;
using Spectre.Console;
using HabbitLogger.Commons.Classes;
using HabbitLogger.DAL;

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
            ViewTablesOption viewTablesOptionChoice;
            string viewTablesOptionChoiceString;

            while (shouldLoopMainMenu)
            {
                mainMenuChoice = AnsiConsole.Prompt(new SelectionPrompt<MainMenuOption>().Title($"What do you want to do ?").AddChoices(Enum.GetValues<MainMenuOption>()));

                switch (mainMenuChoice)
                {
                    case MainMenuOption.Insert:
                        break;
                    case MainMenuOption.Delete:
                        break;
                    case MainMenuOption.Update:
                        break;
                    case MainMenuOption.View:
                        //  Retrieves all enum values, casts them to Enum, gets their descriptions, and converts them to an array of strings.
                        var options = Enum.GetValues(typeof(ViewTablesOption))
                            .Cast<ViewTablesOption>()
                            .Select(e => e.GetDescription())
                            .ToArray();

                        // Use the string[] to display the description attribute of the enum
                        // I do this because I want to use the description instead of the raw value in my Spectre.Console.Prompt()
                        AnsiConsole.Clear();
                        viewTablesOptionChoiceString = AnsiConsole.Prompt(new SelectionPrompt<string>().Title($"What do you want to [{NEUTRAL_INDICATOR_COLOR}]view[/] ?").AddChoices(options));

                        // Convert the chosen option back to an enum value
                        viewTablesOptionChoice = Enum.GetValues(typeof(ViewTablesOption))
                            .Cast<ViewTablesOption>()
                            .FirstOrDefault(e => e.GetDescription() == viewTablesOptionChoiceString);

                        switch (viewTablesOptionChoice)
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
            AnsiConsole.MarkupLine($"Press any key to [{NEUTRAL_INDICATOR_COLOR}]continue[/]");
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
            AnsiConsole.MarkupLine($"Press any key to [{NEUTRAL_INDICATOR_COLOR}]continue[/]");
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
            AnsiConsole.MarkupLine($"Press any key to [{NEUTRAL_INDICATOR_COLOR}]continue[/]");
            Console.ReadKey();
        }
    }
}
