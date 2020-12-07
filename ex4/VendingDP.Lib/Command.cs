using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace VendingDP.Lib
{
    public class CommandSelector
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

    public class ShowMenu : Command
    {
        public override string CommandPrintingFormat() => "ShowMenu";

        public override void Run(ref Order currentOrder)
        {
            Console.WriteLine(currentOrder.Menu);
        }
    }

    public class ShowOrder : Command
    {
        public override string CommandPrintingFormat() => "ShowOrder";

        public override void Run(ref Order currentOrder)
        {
            Console.WriteLine(currentOrder);
        }
    }

    public class EndOrder : Command
    {
        public override string CommandPrintingFormat() => "EndOrder";

        public override void Run(ref Order currentOrder)
        {

            if (currentOrder.Price > 0)
            {
                currentOrder.EndOrder();
                Console.WriteLine(currentOrder);
                currentOrder = new Order(menu: currentOrder.Menu);
                Console.WriteLine("\n" + CommandExecutor.HELLO);
            }

            else
            {
                Console.WriteLine(CommandExecutor.END_EMPTY_ORDER);
            }
        }
    }

    public class OrderSomething : Command
    {
        public override string CommandPrintingFormat() => "Order [productName]";

        public override void Run(ref Order currentOrder)
        {
            string productName = Regex.Match(fullCommand, @"(?i)(?<=Order).*").ToString().Trim();
            currentOrder.OrderProduct(productName);
        }
    }

    public class Add : Command
    {
        public override string CommandPrintingFormat() => "Add [toppingName] [quantity = 1]";

        public override void Run(ref Order currentOrder)
        {
            string toppingDetails = Regex.Match(fullCommand, @"(?i)(?<=Add).*").ToString().Trim();

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
        }
    }

    public class Quit : Command
    {
        public override string CommandPrintingFormat() => "\nCommand to end the program: Quit";

        public override void Run(ref Order currentOrder)
        {
            Environment.Exit(0);
        }
    }

    public class ShowCommands : Command
    {
        public override string CommandPrintingFormat() => "\nCommand to show possible commands: ShowCommands";

        public override void Run(ref Order currentOrder)
        {
            Run();
        }

        public void Run()
        {
            CommandSelector commandSelector = new CommandSelector();
            Console.WriteLine("Possible Commands: " +
                string.Join(", ", ((IEnumerable<PossibleCommands>)Enum.GetValues(typeof(PossibleCommands)))
                .Select(commandName => commandSelector.InitCommand(commandName, "").CommandPrintingFormat())));
        }
    }

    public class Unknown : Command
    {
        public override string CommandPrintingFormat()
        {
            throw new NotImplementedException();
        }

        public override void Run(ref Order currentOrder)
        {
            Console.WriteLine(CommandExecutor.UNKNOWN);
        }
    }
}
