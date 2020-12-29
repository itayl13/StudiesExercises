using System.IO;
using System.Linq;

namespace FileSystemTPL.TPLOperations
{
    class DeleteFolderTPL : Operation
    {
        public DeleteFolderTPL(CommandDetails commandDetails, ExecutionManager executionManager) :
            base(commandDetails, executionManager)
        { }

        public override void Execute()
        {
            string dirToRemove = Path.Combine(Program.DIRECTORY_PATH, commandDetails.CommandArgs.First());
            DeleteDirRecursively(dirToRemove);
        }

        private void DeleteDirRecursively(string dirPath)
        {
            try
            {
                executionManager.LockPath(dirPath, ObjectInActionStatus.ToBeDeleted);
                DeleteChildFiles(dirPath);
                DeleteChildDirs(dirPath);
                basicOperations.DeleteDir(dirPath);
            }
            finally
            {
                executionManager.ReleasePath(dirPath);
            }
        }

        private void DeleteChildFiles(string dirPath)
        {
            ApplyOnChildren((childFile) => basicOperations.DeleteEntrySafely(childFile, FileSystemObjects.File),
                dirPath, FileSystemObjects.File);
        }

        private void DeleteChildDirs(string dirPath)
        {
            ApplyOnChildren((childDir) => DeleteDirRecursively(childDir), dirPath, FileSystemObjects.Dir);
        }
    }
}

