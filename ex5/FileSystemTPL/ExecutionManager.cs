using System.Collections.Generic;
using System.Threading;

namespace FileSystemTPL
{
    public class ExecutionManager
    {
        public int DelayMilliseconds { get; set; }
        private readonly Mutex pathsInActionMutex;
        private readonly Dictionary<string, ObjectInActionStatus> pathsInAction;
        private readonly Mutex commandsLogMutex;
        public readonly Queue<string> lastTenCommandsLog;

        public ExecutionManager(int delayMilliseconds)
        {
            DelayMilliseconds = delayMilliseconds;
            pathsInActionMutex = new Mutex();
            pathsInAction = new Dictionary<string, ObjectInActionStatus>();
            commandsLogMutex = new Mutex();
            lastTenCommandsLog = new Queue<string>();
        }

        public void LockPaths(ObjectInActionStatus actionStatus, params string[] paths)
        {
            foreach (string path in paths)
            {
                LockPath(path, actionStatus);
            }
        }

        public void LockPath(string path, ObjectInActionStatus actionStatus)
        {
            try
            {
                pathsInActionMutex.WaitOne();
                pathsInAction.Add(path, actionStatus);
            }
            
            finally
            {
                pathsInActionMutex.ReleaseMutex();
            }
        }

        public void ReleasePaths(params string[] paths)
        {
            foreach (string path in paths)
                ReleasePath(path);
        }

        public void ReleasePath(string path)
        {
            try
            {
                pathsInActionMutex.WaitOne();
                pathsInAction.Remove(path);
            }

            finally
            { 
                pathsInActionMutex.ReleaseMutex(); 
            }
        }

        public void UpdateLog(CommandDetails commandDetails)
        {
            try
            {
                LockCommandsLog();

                if (lastTenCommandsLog.Count == 10)
                {
                    lastTenCommandsLog.Dequeue();
                }
                lastTenCommandsLog.Enqueue(commandDetails.CommandLogMessage());
            }

            finally
            {
                ReleaseCommandsLog();
            }
        }

        public ObjectInActionStatus GetFileStatus(string path, bool LockPathsInAction = true)
        {
            try
            {
                if (LockPathsInAction)
                {
                    pathsInActionMutex.WaitOne();
                }

                if (pathsInAction.ContainsKey(path))
                {
                    return pathsInAction[path];
                }
                return ObjectInActionStatus.Free;
            }

            finally
            {
                if (LockPathsInAction)
                {
                    pathsInActionMutex.ReleaseMutex();
                }
            }
        }

        public void BlockAllActions()
        {
            pathsInActionMutex.WaitOne();
            Thread.Sleep(DelayMilliseconds);
        }

        public void ReleaseAllActions() => pathsInActionMutex.ReleaseMutex();

        public void LockCommandsLog() => commandsLogMutex.WaitOne();

        public void ReleaseCommandsLog() => commandsLogMutex.ReleaseMutex();
    }

    public enum ObjectInActionStatus { Free, ToBeMoved, MovingDestination, ToBeDeleted, OnSearch, SearchResults }
}
