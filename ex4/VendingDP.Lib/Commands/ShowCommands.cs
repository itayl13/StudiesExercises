using System;
using System.Collections.Generic;
using System.Linq;

namespace VendingDP.Lib.Commands
{
    public class ShowCommands : Command
    {
        public override string CommandPrintingFormat() => "\nCommand to show possible commands: ShowCommands";

        public override void Run(ref Order currentOrder)
        {
            Run();
        }

        public void Run()
        {
            CommandFactory commandSelector = new CommandFactory();
            Console.WriteLine("Possible Commands: " +
                string.Join(", ", ((IEnumerable<PossibleCommands>)Enum.GetValues(typeof(PossibleCommands)))
                .Select(commandName => commandSelector.InitCommand(commandName, "").CommandPrintingFormat())));
        }
    }
}
