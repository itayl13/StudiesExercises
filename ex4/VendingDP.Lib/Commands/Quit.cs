using System;

namespace VendingDP.Lib.Commands
{
    public class Quit : Command
    {
        public override string CommandPrintingFormat() => "\nCommand to end the program: Quit";

        public override void Run(ref Order currentOrder)
        {
            Environment.Exit(0);
        }
    }
}
