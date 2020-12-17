using System;
using System.IO;
using System.Linq;

namespace FileSystemTPL.TPLOperations
{
    class MoveTPL : Operation
    {
        public MoveTPL(CommandDetails commandDetails, ExecutionManager executionManager) :
            base(commandDetails, executionManager)
        { }

        public override void Execute()
        {
            string source = Path.Combine(Program.DIRECTORY_PATH, commandDetails.CommandArgs.First());
            string destination = Path.Combine(Program.DIRECTORY_PATH, commandDetails.CommandArgs.Last());

            Action<string, string> MovingFunction = GetMovingFunction(source);
            MovingFunction(source, destination);
        }

        private Action<string, string> GetMovingFunction(string source)
        {
            try
            {
                executionManager.LockPath(source, ObjectInActionStatus.ToBeMoved);
                
                if (File.Exists(source))
                {
                    return MoveFileTPL;
                }

                else if (Directory.Exists(source))
                {
                    return MoveDirRecursively;
                }

                else
                {
                    throw new FileNotFoundException($"The path {source} does not exist.");
                }
            }

            finally
            {
                executionManager.ReleasePath(source);
            }
        }

        private void MoveFileTPL(string source, string destination)
        {
            try
            {
                executionManager.LockPath(destination, ObjectInActionStatus.MovingDestination);
                basicOperations.MoveFile(source, destination);
            }

            finally
            {
                executionManager.ReleasePath(destination);
            }
        }

        private void MoveDirRecursively(string source, string destination)
        {
            try
            {
                executionManager.LockPath(destination, ObjectInActionStatus.MovingDestination);
                CreateDestinationDir(destination);
                MoveChildFiles(source, destination);
                MoveChildDirs(source, destination);
                DeleteSourceDir(source);
            }

            finally
            {
                executionManager.ReleasePath(destination);
            }
        }

        private void CreateDestinationDir(string destination)
        {
            if (!Directory.Exists(destination))
            {
                Directory.CreateDirectory(destination);
            }
        }

        private void MoveChildFiles(string source, string destination)
        {
            ApplyOnChildren((childFile) => MoveFileTPL(childFile, Path.Combine(destination, GetEntryName(childFile))),
                source, FileSystemObjects.File);
        }

        private void MoveChildDirs(string source, string destination)
        {
            ApplyOnChildren((childDir) => MoveDirRecursively(childDir, Path.Combine(destination, GetEntryName(childDir))),
                source, FileSystemObjects.Dir);
        }

        private void DeleteSourceDir(string source)
        {
            if (Directory.Exists(source))
            {
                basicOperations.DeleteEntrySafely(source, FileSystemObjects.Dir);
            }
        }
    }
}
