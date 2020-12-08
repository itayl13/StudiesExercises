using System;

namespace VendingDP.Lib.Commands
{
    public class ShowOrder : Command
    {
        public override string CommandPrintingFormat() => "ShowOrder";

        public override void Run(ref Order currentOrder)
        {
            Console.WriteLine(currentOrder);
        }
    }
}
