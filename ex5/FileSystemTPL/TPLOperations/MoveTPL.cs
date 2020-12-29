using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

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
            ValidateMovingIsPossible(source, destination);
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

        private void ValidateMovingIsPossible(string source, string destination)
        {
            DirectoryInfo sourceInfo = new DirectoryInfo(source.TrimEnd('\\'));
            DirectoryInfo destinationInfo = new DirectoryInfo(destination.TrimEnd('\\'));
            if (sourceInfo.FullName == destinationInfo.FullName)
            {
                throw new ArgumentException("The destination folder is the same as the source folder");
            }
            else
            {
                ValidateDestinationIsNotSubdirOfSource(sourceInfo, destinationInfo);
            }
        }

        private void ValidateDestinationIsNotSubdirOfSource(DirectoryInfo sourceInfo, DirectoryInfo destinationInfo)
        {
            while (destinationInfo.Parent != null)
            {
                if (destinationInfo.Parent.FullName == sourceInfo.FullName)
                {
                    throw new ArgumentException("The destination folder is a subfolder of the source folder");
                }
                else
                {
                    destinationInfo = destinationInfo.Parent;
                }
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
