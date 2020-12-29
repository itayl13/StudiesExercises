using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileSystemTPL.TPLOperations
{
    public abstract class Operation
    {
        internal CommandDetails commandDetails;
        internal BasicOperations basicOperations;
        internal ExecutionManager executionManager;

        protected Operation(CommandDetails commandDetails, ExecutionManager executionManager)
        {
            this.commandDetails = commandDetails;
            basicOperations = new BasicOperations(executionManager);
            this.executionManager = executionManager;
        }

        protected string GetEntryName(string fullPath) => Regex.Match(fullPath, @"(?<=\\)[^\\]+\\?$").Value;

        public abstract void Execute();

        protected void ApplyOnChildren(Action<string> DoWithChild, string path, FileSystemObjects whatToEnumerate,
            bool LockPathsInAction = true)
        {
            List<Task> tasksOnChildEntries = new List<Task>();

            foreach (string childEntry in EnumerateObjectsInDir(path, whatToEnumerate, LockPathsInAction))
            {
                tasksOnChildEntries.Add(Task.Factory.StartNew(() => DoWithChild(childEntry)));
            }
            Task.WaitAll(tasksOnChildEntries.ToArray());
        }

        private IEnumerable<string> EnumerateObjectsInDir(string path, FileSystemObjects whatToEnumerate,
            bool LockPathsInAction = true)
        {
            if (!Directory.Exists(path) 
                || executionManager.GetFileStatus(path, LockPathsInAction) == ObjectInActionStatus.MovingDestination
                || executionManager.GetFileStatus(path, LockPathsInAction) == ObjectInActionStatus.ToBeDeleted)
            {
                return Enumerable.Empty<string>();
            }

            return whatToEnumerate switch
            {
                FileSystemObjects.File => basicOperations.EnumerateFilesInDir(path),
                FileSystemObjects.Dir => basicOperations.EnumerateDirsInDir(path),
                FileSystemObjects.Entry => basicOperations.EnumerateEntriesInDir(path),
                _ => basicOperations.EnumerateEntriesInDir(path)
            };
        }
    }
}
