using System.IO;
using System.Linq;

namespace FileSystemTPL.TPLOperations
{
    class DeleteFileTPL : Operation
    {
        public DeleteFileTPL(CommandDetails commandDetails, ExecutionManager executionManager) :
            base(commandDetails, executionManager)
        { }
        public override void Execute()
        {
            string filePath = Path.Combine(Program.DIRECTORY_PATH, commandDetails.CommandArgs.First());
            basicOperations.DeleteEntrySafely(filePath, FileSystemObjects.File);
        }
    }
}
