using System;

namespace FileSystemTPL.TPLOperations
{
    class GetStatusTPL : Operation
    {
        public GetStatusTPL(CommandDetails commandDetails, ExecutionManager executionManager) :
            base(commandDetails, executionManager)
        { }

        public override void Execute()
        {
            try
            {
                executionManager.BlockAllActions();
                PathStatus baseDirStats = GetPathStats(Program.DIRECTORY_PATH);
                Console.WriteLine("\nCurrent Status:\n" + baseDirStats);
            }

            finally
            {
                executionManager.ReleaseAllActions();
            }
        }

        private PathStatus GetPathStats(string path)
        {
            PathStatus pathStats = new PathStatus();
            UpdateByCurrentFolder(path, pathStats);
            UpdateByChildrenDirs(path, pathStats);
            return pathStats;
        }

        private void UpdateByCurrentFolder(string path, PathStatus pathStats)
        {
            DirStats dirCounts = basicOperations.GetDirStats(path);
            pathStats.FolderCount = dirCounts.DirsCount;
            pathStats.FileCount = dirCounts.FilesCount;
            ApplyOnChildren((childFile) => UpdateByChildFile(childFile, pathStats), path, FileSystemObjects.File,
                LockPathsInAction: false);
        }

        private void UpdateByChildFile(string childFile, PathStatus pathStats)
        {
            FileStats childFileStats = basicOperations.GetFileStats(childFile);
            pathStats.UpdateByChildFileStats(childFileStats);
        }

        private void UpdateByChildrenDirs(string path, PathStatus pathStats)
        {
            ApplyOnChildren((childDir) => UpdateByChildDir(childDir, pathStats), path, FileSystemObjects.Dir,
                LockPathsInAction: false);
        }

        private void UpdateByChildDir(string childDir, PathStatus pathStats)
        {
            PathStatus childDirStats = GetPathStats(childDir);
            pathStats.UpdateByChildDirStats(childDirStats);
        }
    }
}
