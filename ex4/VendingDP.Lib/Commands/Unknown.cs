using System;

namespace VendingDP.Lib.Commands
{
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
