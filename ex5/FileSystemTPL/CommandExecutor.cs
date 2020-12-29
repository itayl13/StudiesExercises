using System;
using System.IO;
using System.Threading.Tasks;

namespace FileSystemTPL
{
    public class CommandExecutor
    {
        private readonly ExecutionManager executionManager;
        private readonly TPLOperationsFactory operationsFactory;

        public CommandExecutor(int delayMilliseconds)
        {
            executionManager = new ExecutionManager(delayMilliseconds);
            operationsFactory = new TPLOperationsFactory(executionManager);
    }

        public void RunCommandsFromFile()
        {
            string filePath = GetFilePath();
            string[] fileCommands = File.ReadAllLines(filePath);

            foreach (string command in fileCommands)
            {
                CommandDetails commandDetails = new CommandDetails(command);
                Task.Factory.StartNew(() => Execute(commandDetails)).Wait();
            }
        }

        private string GetFilePath()
        {
            Console.WriteLine("Enter path to commands file (Default: press enter).");
            string filePath = Console.ReadLine();

            if (string.IsNullOrEmpty(filePath.Trim()))
            {
                return Program.DEFAULT_COMMANDS_PATH;
            }
            return filePath;
        }

        public void GetUserCommands()
        {
            PrintPossibleCommands();

            while (true)
            {
                Console.WriteLine("Enter Command:");
                string userCommand = Console.ReadLine();
                CommandDetails commandDetails = new CommandDetails(userCommand);
                Task.Factory.StartNew(() => Execute(commandDetails));
            }
        }

        private void PrintPossibleCommands()
        {
            string possibleCommands = "Possible Commands:\n";

            foreach (FileSystemCommands command in Enum.GetValues(typeof(FileSystemCommands)))
            {
                possibleCommands += PossibleCommandName(command) + "\n";
            }
            possibleCommands += "Important Note: All paths are considered relative to the base folder!\n";
            Console.WriteLine(possibleCommands);
        }

        private string PossibleCommandName(FileSystemCommands command)
        {
            return command switch
            {
                FileSystemCommands.Empty => "Plain enter has no effect",
                FileSystemCommands.Move => "Move [source] [destination]",
                FileSystemCommands.DeleteFile => "DeleteFile [fileName]",
                FileSystemCommands.DeleteFolder => "DeleteFolder [folderName]",
                FileSystemCommands.Search => "Search [key] [resultsFilePath]",
                FileSystemCommands.GetStatus => "GetStatus",
                FileSystemCommands.GetLog => "GetLog",
                FileSystemCommands.Quit => "To exit, enter 'Quit'",
                _ => ""
            };
        }

        private void Execute(CommandDetails commandDetails)
        {
            if (commandDetails.fileSystemCommand == FileSystemCommands.Empty)
            {
                return;
            }

            if (commandDetails.CommandStatus != CommandStatuses.Failed)
            {
                operationsFactory.RunningDecorator(commandDetails);
            }
            executionManager.UpdateLog(commandDetails);
        }
    }
}
