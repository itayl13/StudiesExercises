using System;
namespace VendingDP.Lib.Commands
{
    public class ShowMenu : Command
    {
        public override string CommandPrintingFormat() => "ShowMenu";

        public override void Run(ref Order currentOrder)
        {
            Console.WriteLine(currentOrder.Menu);
        }
    }
}
