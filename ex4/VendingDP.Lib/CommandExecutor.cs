using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace VendingDP.Lib
{
    public class CommandExecutor
    {
        private const string HELLO = "Hello! What would you like to order?";
        private const string UNKNOWN = "Unknown Command";
        private const string END_EMPTY_ORDER = "Ending an empty order. This has no effect.";
        public void Introduce()
        {
            Console.WriteLine(HELLO);
            PrintPossibleCommands();
        }
        public void Execute(string command, ref Order currentOrder)
        {
            string firstWordOfCommand = FirstWordOfCommand(command);
            if (Enum.TryParse(firstWordOfCommand, ignoreCase: true, out PossibleCommands commandName))
            {
                switch (commandName)
                {
                    case PossibleCommands.ShowMenu:
                        Console.WriteLine(currentOrder.Menu);
                        break;
                    case PossibleCommands.ShowOrder:
                        Console.WriteLine(currentOrder);
                        break;
                    case PossibleCommands.EndOrder:
                        {
                            if (currentOrder.Price > 0)
                            {
                                currentOrder.EndOrder();
                                Console.WriteLine(currentOrder);
                                currentOrder = new Order(menu: currentOrder.Menu);
                                Console.WriteLine("\n" + HELLO);
                            }
                            else
                            {
                                Console.WriteLine(END_EMPTY_ORDER);
                            }
                            break;
                        }
                    case PossibleCommands.Order:
                        string productName = Regex.Match(command, @"(?i)(?<=Order).*").ToString().Trim();
                        currentOrder.OrderProduct(productName);
                        break;
                    case PossibleCommands.Add:
                        string toppingDetails = Regex.Match(command, @"(?i)(?<=Add).*").ToString().Trim();
                        if (Regex.Match(toppingDetails, @"\d$").Success)
                        {
                            string toppingName = Regex.Match(toppingDetails, @".*\D(?=-?\d+$)").Value.Trim();
                            string quantityAsString = Regex.Match(toppingDetails, @"-?\d+$").Value;
                            int quantity = int.Parse(quantityAsString);
                            currentOrder.AddTopping(toppingName, quantity);
                        }
                        else
                        {
                            currentOrder.AddTopping(toppingDetails);
                        }
                        break;
                    case PossibleCommands.Quit:
                        Environment.Exit(0);
                        break;
                    case PossibleCommands.ShowCommands:
                        PrintPossibleCommands();
                        break;
                }
            }
            else
            {
                Console.WriteLine(UNKNOWN);
            }
        }
        private string CommandDetails(string command)
        {
            PossibleCommands commandName = (PossibleCommands)Enum.Parse(typeof(PossibleCommands), command, ignoreCase: true);
            switch (commandName)
            {
                case PossibleCommands.Order:
                    return "Order [productName]";
                case PossibleCommands.Add:
                    return "Add [toppingName] [quantity = 1]";
                case PossibleCommands.Quit:
                    return "\nCommand to end the program: Quit";
                case PossibleCommands.ShowCommands:
                    return "\nCommand to show possible commands: ShowCommands";
                default:
                    return command;
            }
        }
        private string FirstWordOfCommand(string command) => Regex.Match(command.Trim(), @"^\S+").Value;
        private void PrintPossibleCommands()
        {
            Console.WriteLine("Possible Commands: " +
                string.Join(", ", Enum.GetNames(typeof(PossibleCommands)).Select(CommandDetails)));
        }
        private enum PossibleCommands { ShowMenu, ShowOrder, EndOrder, Order, Add, Quit, ShowCommands }
    }
}
