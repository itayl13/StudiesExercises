using VendingDP.Lib.Commands;

namespace VendingDP.Lib
{
    public class CommandFactory
    {
        public Command InitCommand(PossibleCommands commandName, string fullCommand)
        {
            Command command;

            switch (commandName)
            {
                case PossibleCommands.ShowMenu:
                    command = new ShowMenu();
                    break;
                case PossibleCommands.ShowOrder:
                    command = new ShowOrder();
                    break;
                case PossibleCommands.EndOrder:
                    command = new EndOrder();
                    break;
                case PossibleCommands.Order:
                    command = new OrderSomething();
                    break;
                case PossibleCommands.Add:
                    command = new Add();
                    break;
                case PossibleCommands.Quit:
                    command = new Quit();
                    break;
                case PossibleCommands.ShowCommands:
                    command = new ShowCommands();
                    break;
                default:
                    command = new ShowMenu();
                    break;
            }
            command.fullCommand = fullCommand;
            return command;
        }
    }

    public abstract class Command
    {
        public string fullCommand;
        public abstract void Run(ref Order currentOrder);
        public abstract string CommandPrintingFormat();
    }
}
