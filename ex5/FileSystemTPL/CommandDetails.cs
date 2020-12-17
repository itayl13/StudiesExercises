using System;
using System.Collections.Generic;
using System.Linq;

namespace FileSystemTPL
{
    public class CommandDetails
    {
        public string FullCommand { get; set; }
        public FileSystemCommands fileSystemCommand;
        public IEnumerable<string> CommandArgs { get; set; }
        public CommandStatuses CommandStatus { get; set; }
        public long ExecutionTimeInMs { get; set; }

        public CommandDetails(string command)
        {
            FullCommand = command.Trim();
            
            if (FullCommand == string.Empty)
            {
                fileSystemCommand = FileSystemCommands.Empty;
                return;
            }
            ParseCommand();
            ExitIfRequested();
        }

        private void ParseCommand()
        {
            string commandName = FullCommand.Split(" ").First();
            if (Enum.TryParse(commandName, ignoreCase: true, out fileSystemCommand))
            {
                CommandArgs = FullCommand.Split(" ").AsEnumerable().Skip(1);
                CommandStatus = CommandStatuses.Running;
                return;
            }
            CommandStatus = CommandStatuses.Failed;
        }

        private void ExitIfRequested()
        {
            if (fileSystemCommand == FileSystemCommands.Quit)
                Environment.Exit(0);
        }

        public void UpdateStatusAndTime(bool success, long elapsedMilliseconds)
        {
            if (success)
            {
                CommandStatus = CommandStatuses.Succeeded;
                ExecutionTimeInMs = elapsedMilliseconds;
                return;
            }
            CommandStatus = CommandStatuses.Failed;
        }

        public string CommandLogMessage()
        {
            if (CommandStatus == CommandStatuses.Failed)
            {
                return $"{FullCommand} - Failed";
            }
            return $"{FullCommand} - Succeeded in {ExecutionTimeInMs} ms";
        }
    }

    public enum FileSystemCommands { Empty, Move, DeleteFile, DeleteFolder, Search, GetStatus, GetLog, Quit }

    public enum CommandStatuses { Running, Succeeded, Failed }
}
