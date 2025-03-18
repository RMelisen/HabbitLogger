using HabbitLogger.Enums;
using Spectre.Console;

namespace HabbitLogger.UI
{
    internal class MainMenu
    {
        #region CONST

        private static readonly Color NEUTRAL_INDICATOR_COLOR = Color.Blue;

        #endregion

        internal static void WelcomeUser()
        {
            AnsiConsole.MarkupLine($"[{NEUTRAL_INDICATOR_COLOR} Bold]Welcome to HabbitLogger ![/]\n");
        }

        internal static void ShowMainMenu()
        {
            MainMenuOption mainMenuChoice = AnsiConsole.Prompt(new SelectionPrompt<MainMenuOption>().Title($"What do you want to do ?").AddChoices(Enum.GetValues<MainMenuOption>()));

            switch (mainMenuChoice)
            {
                case MainMenuOption.Insert:
                    break;
                case MainMenuOption.Delete:
                    break;
                case MainMenuOption.Update:
                    break;
                case MainMenuOption.View:
                    break;
                case MainMenuOption.Quit: 
                    break;
            }
        }
    }
}
