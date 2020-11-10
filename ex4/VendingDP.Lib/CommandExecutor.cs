using System;
using System.Linq;

namespace VendingDP.Lib
{
    public class CommandExecutor
        {
            internal string hello = "Hello! What would you like to order?";
            internal string possibleCommands = "Possible commands: ShowMenu, ShowOrder, EndOrder, Order [productName], Add [toppingName] [quantity = 1].\n" +
                "Command to end the program: Quit\nCommand to show possible commands: ShowCommands";
            public void Introduce()
            {
                Console.WriteLine(hello + "\n" + possibleCommands);
            }
            public void Execute(string command, ref Order currentOrder)
            {
            if (command == "ShowMenu")
            {
                Console.WriteLine(currentOrder.Menu);
            }
            else if (command == "ShowOrder")
            {
                Console.WriteLine(currentOrder);
            }
            else if (command == "EndOrder")
            {
                if (currentOrder.Price > 0)
                {
                    currentOrder.EndOrder();
                    Console.WriteLine(currentOrder);
                    currentOrder = new Order() { Menu = currentOrder.Menu };
                    Console.WriteLine("\n" + hello);
                }
                else
                {
                    Console.WriteLine("Ending an empty order. This has no effect.");
                }
            }
            else if (command.StartsWith("Order "))
            {
                string productName = string.Join(" ", command.Split(' ').Skip(1));
                currentOrder.OrderProduct(productName);
            }
            else if (command.StartsWith("Add "))
            {
                string toppingDetails = string.Join(" ", command.Split(' ').Skip(1));
                try
                {
                    int quantity = int.Parse(toppingDetails.Split(' ').Last());
                    string toppingName = string.Join(" ", toppingDetails.Split(' ').Reverse().Skip(1).Reverse());
                    currentOrder.AddTopping(toppingName, quantity);
                }
                // Quantity = 1 and wasn't written - impossible to parse the string to int.
                catch (FormatException)
                {
                    currentOrder.AddTopping(toppingDetails);
                }
            }
            else if (command == "Quit")
            {
                Environment.Exit(0);
            }
            else if (command == "ShowCommands")
            {
                Console.WriteLine(possibleCommands);
            }
            else
            {
                Console.WriteLine("Unknown Command");
            }   
            }
        }
    }
