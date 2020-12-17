using FileSystemTPL.TPLOperations;
using System.Diagnostics;

namespace FileSystemTPL
{
    public class TPLOperationsFactory
    {
        private readonly ExecutionManager executionManager;

        public TPLOperationsFactory(ExecutionManager executionManager)
        {
            this.executionManager = executionManager;
        }

        public void RunningDecorator(CommandDetails commandDetails)
        {
            bool success;
            Stopwatch watch = Stopwatch.StartNew();

            try
            {
                Operation command = GetCommand(commandDetails);
                command.Execute();
                success = true;
            }

            catch
            {
                success = false;
            }
            watch.Stop();
            commandDetails.UpdateStatusAndTime(success, watch.ElapsedMilliseconds);
        }

        private Operation GetCommand(CommandDetails commandDetails)
        {
            return commandDetails.fileSystemCommand switch
            {
                FileSystemCommands.Move => new MoveTPL(commandDetails, executionManager),
                FileSystemCommands.DeleteFile => new DeleteFileTPL(commandDetails, executionManager),
                FileSystemCommands.DeleteFolder => new DeleteFolderTPL(commandDetails, executionManager),
                FileSystemCommands.Search => new SearchTPL(commandDetails, executionManager),
                FileSystemCommands.GetStatus => new GetStatusTPL(commandDetails, executionManager),
                FileSystemCommands.GetLog => new GetLogTPL(commandDetails, executionManager),
                FileSystemCommands.Quit => new QuitCode(commandDetails, executionManager),
                _ => new QuitCode(commandDetails, executionManager),
            };
        }
    }
}