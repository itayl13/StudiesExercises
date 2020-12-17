
namespace FileSystemTPL.TPLOperations
{
    internal class BasicOperations: FSAccessManager
    {
        private readonly ExecutionManager executionManager;

        public BasicOperations(ExecutionManager executionManager) : 
            base(executionManager.DelayMilliseconds) 
        {
            this.executionManager = executionManager;
        }

        public void DeleteEntrySafely(string path, FileSystemObjects objectType)
        {
            try
            {
                executionManager.LockPath(path, ObjectInActionStatus.ToBeDeleted);

                if (objectType == FileSystemObjects.File)
                {
                    DeleteFile(path);
                }

                else
                {
                    DeleteDir(path);
                }
            }

            finally
            {
                executionManager.ReleasePath(path);
            }
        }
    }

    public enum FileSystemObjects { File, Dir, Entry }
}
