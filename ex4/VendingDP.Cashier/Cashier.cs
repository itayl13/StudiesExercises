using System;
using VendingDP.Lib;

namespace VendingDP.Cashier
{
    class Cashier
    {
        static void Main()
        {
            // Validate the products in the menu have correct names and formats (for future menu changes):
            Order currentOrder = new Order() { Menu = new Menu().ToString() };
            CommandExecutor Executor = new CommandExecutor();
            Executor.Introduce();
            while (true)
            {
                Console.Write("\n");
                string command = Console.ReadLine();
                Executor.Execute(command, ref currentOrder);
            }
        }
    }
}
