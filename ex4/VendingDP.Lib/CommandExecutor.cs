using System;
using System.Text.RegularExpressions;

namespace VendingDP.Lib
{
    public class CommandExecutor
    {
        public const string HELLO = "Hello! What would you like to order?";
        public const string UNKNOWN = "Unknown Command";
        public const string END_EMPTY_ORDER = "Ending an empty order. This has no effect.";
        private readonly CommandSelector commandSelector = new CommandSelector();

        public void Introduce()
        {
            Console.WriteLine(HELLO);
            PrintPossibleCommands();
        }

        private void PrintPossibleCommands()
        {
            ShowCommands showCommands = new ShowCommands();
            showCommands.Run();
        }

        public void Execute(string command, ref Order currentOrder)
        {
            Command matchingCommand = GetCommand(command);
            matchingCommand.Run(ref currentOrder);
        }

        private Command GetCommand(string command)
        {
            string firstWordOfCommand = FirstWordOfCommand(command);

            if (Enum.TryParse(firstWordOfCommand, ignoreCase: true, out PossibleCommands commandName))
            {
                return commandSelector.InitCommand(commandName, fullCommand: command);
            }
            return new Unknown();
        }

        private string FirstWordOfCommand(string command) => Regex.Match(command.Trim(), @"^\S+").Value;
    }

    public enum PossibleCommands { ShowMenu, ShowOrder, EndOrder, Order, Add, Quit, ShowCommands }
}
