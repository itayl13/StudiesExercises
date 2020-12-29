using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace FileSystemTPL
{
    internal class FSAccessManager
    {
        internal FSAccessManager(int delayMilliseconds)
        {
            if (delayMilliseconds <= 0)
            {
                throw new ArgumentException(nameof(delayMilliseconds), "The delay must be positive");
            }
            _delayMilliseconds = delayMilliseconds;
        }
        internal void MoveFile(string sourcePath, string destPath)
        {
            Delay();
            File.Move(sourcePath, destPath);
        }
        internal void MoveDir(string sourcePath, string destPath)
        {
            EnsureDirIsEmpty(sourcePath);
            Delay();
            Directory.Move(sourcePath, destPath);
        }
        internal IEnumerable<string> EnumerateEntriesInDir(string dirPath)
        {
            return Directory.EnumerateFileSystemEntries(dirPath).Select(EnumEntry);
        }
        internal IEnumerable<string> EnumerateFilesInDir(string dirPath)
        {
            return Directory.EnumerateFiles(dirPath).Select(EnumEntry);
        }
        internal IEnumerable<string> EnumerateDirsInDir(string dirPath)
        {
            return Directory.EnumerateDirectories(dirPath).Select(EnumEntry);
        }
        internal void DeleteFile(string filePath)
        {
            Delay();
            File.Delete(filePath);
        }
        internal void DeleteDir(string dirPath)
        {
            EnsureDirIsEmpty(dirPath);
            Delay();
            Directory.Delete(dirPath);
        }
        internal FileStats GetFileStats(string filePath)
        {
            Delay(0.5);
            FileInfo fileInfo = new FileInfo(filePath);
            return new FileStats
            {
                Path = filePath,
                CreationTime = fileInfo.CreationTime,
                Size = fileInfo.Length
            };
        }
        internal DirStats GetDirStats(string dirPath)
        {
            Delay(0.5);
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            return new DirStats
            {
                Path = dirPath,
                FilesCount = dirInfo.EnumerateFiles().Count(),
                DirsCount = dirInfo.EnumerateDirectories().Count()
            };
        }
        private string EnumEntry(string entry)
        {
            Delay(0.2);
            return entry;
        }
        private void Delay()
        {
            Delay(1);
        }
        private void Delay(double factor)
        {
            int delayTime = (int)(_delayMilliseconds * factor);
            Thread.Sleep(delayTime);
        }
        private static void EnsureDirIsEmpty(string sourcePath)
        {
            if (Directory.EnumerateFileSystemEntries(sourcePath).Any())
            {
                throw new IOException("The directory is not empty");
            }
        }
        private readonly int _delayMilliseconds;
    }
    internal class FileStats
    {
        public string Path { get; set; }
        public DateTime CreationTime { get; set; }
        /// <summary>
        /// In bytes
        /// </summary>
        public long Size { get; set; }
    }
    internal class DirStats
    {
        public string Path { get; set; }
        /// <summary>
        /// Only for the top level, not recursive counts
        /// </summary>
        public int FilesCount { get; set; }
        /// <summary>
        /// Only for the top level, not recursive counts
        /// </summary>
        public int DirsCount { get; set; }
    }
}
