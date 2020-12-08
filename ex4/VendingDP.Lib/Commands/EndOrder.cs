using System;

namespace VendingDP.Lib.Commands
{
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
}
