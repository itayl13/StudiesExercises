using System;
using System.Threading;

namespace FileSystemTPL.TPLOperations
{
    public class PathStatus
    {
        internal int FolderCount { get; set; }
        internal int FileCount { get; set; }
        internal FileStats LargestFile { get; set; }
        internal FileStats OldestFile { get; set; }
        private readonly Mutex folderCountMutex, fileCountMutex, LargestFileMutex, OldestFileMutex;

        public PathStatus()
        {
            LargestFile = new FileStats() { CreationTime = DateTime.Now.AddDays(10), Path = "", Size = -1 };
            OldestFile = new FileStats() { CreationTime = DateTime.Now.AddDays(10), Path = "", Size = -1 };
            folderCountMutex = fileCountMutex = LargestFileMutex = OldestFileMutex = new Mutex();
        }

        internal void UpdateByChildDirStats(PathStatus childPathStats)
        {
            UpdateFolderCount(childPathStats);
            UpdateFileCount(childPathStats);
            TryUpdateLargestFile(childPathStats.LargestFile);
            TryUpdateOldestFile(childPathStats.OldestFile);
        }

        internal void UpdateByChildFileStats(FileStats childFileStats)
        {
            TryUpdateLargestFile(childFileStats);
            TryUpdateOldestFile(childFileStats);
        }

        private void UpdateFolderCount(PathStatus childPathStats)
        {
            try
            {
                folderCountMutex.WaitOne();
                FolderCount += childPathStats.FolderCount;
            }
            finally
            {
                folderCountMutex.ReleaseMutex();
            }
        }

        private void UpdateFileCount(PathStatus childPathStats)
        {
            try
            {
                fileCountMutex.WaitOne();
                FileCount += childPathStats.FileCount;
            }
            finally
            {
                fileCountMutex.ReleaseMutex();
            }
        }

        private void TryUpdateLargestFile(FileStats candidateStats)
        {
            try
            {
                LargestFileMutex.WaitOne();

                if (candidateStats.Size > LargestFile.Size)
                {
                    LargestFile = candidateStats;
                }
            }
            finally
            {
                LargestFileMutex.ReleaseMutex();
            }
        }

        private void TryUpdateOldestFile(FileStats candidateStats)
        {
            try
            {
                OldestFileMutex.WaitOne();

                if (DateTime.Compare(candidateStats.CreationTime, OldestFile.CreationTime) < 0)
                {
                    OldestFile = candidateStats;
                }
            }
            finally
            {
                OldestFileMutex.ReleaseMutex();
            }
        }

        public override string ToString()
        {
            string status = $"Total number of folders: {FolderCount}\n" +
                $"Total number of files: {FileCount}\n";

            if (LargestFile.Size >= 0)
            {
                status += $"Largest file is {LargestFile.Path} - size: {LargestFile.Size} bytes\n" +
                    $"Oldest file is {OldestFile.Path}";
                return status;
            }
            status += "No file exists in the directory or its subdirectories";
            return status;
        }
    }
}
