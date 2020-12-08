using System;
using VendingDP.Lib;

namespace VendingDP.Cashier
{
    class Cashier
    {
        static void Main()
        {
            Order currentOrder = new Order(menu: new Menu());
            CommandExecutor executor = new CommandExecutor();
            executor.Introduce();

            while (true)
            {
                Console.WriteLine();
                string command = Console.ReadLine();
                executor.Execute(command, ref currentOrder);
            }
        }
    }
}