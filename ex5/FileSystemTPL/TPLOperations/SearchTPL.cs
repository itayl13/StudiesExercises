using System;
using System.IO;
using System.Linq;

namespace FileSystemTPL.TPLOperations
{
    class SearchTPL : Operation
    {
        private string resultsFilePath;
        private StreamWriter resultsFile;
        private string searchKey;

        public SearchTPL(CommandDetails commandDetails, ExecutionManager executionManager) :
            base(commandDetails, executionManager)
        { }

        public override void Execute()
        {
            SetArgs();

            try
            {
                executionManager.LockPath(resultsFilePath, ObjectInActionStatus.SearchResults);
                SearchInDir(Program.DIRECTORY_PATH);
            }
            finally
            {
                resultsFile.Close();
                executionManager.ReleasePath(resultsFilePath);
            }
        }

        private void SetArgs()
        {
            searchKey = commandDetails.CommandArgs.First();
            resultsFilePath = commandDetails.CommandArgs.Last();
            resultsFile = new StreamWriter(resultsFilePath);
        }

        private void SearchInDir(string path)
        {
            if (!Directory.Exists(path))
            {
                return;
            }
            ApplyOnChildren((childFile) => MatchAndWrite(childFile), path, FileSystemObjects.File);
            ApplyOnChildren((childDir) => SearchInDir(childDir), path, FileSystemObjects.Dir);
        }

        private void MatchAndWrite(string childFile)
        {

            try
            {
                executionManager.BlockAllActions();

                if (!File.Exists(childFile) || 
                    executionManager.GetFileStatus(childFile) == ObjectInActionStatus.MovingDestination)
                {
                    return;
                }

                if (GetEntryName(childFile).Contains(searchKey, StringComparison.InvariantCultureIgnoreCase))
                {
                    resultsFile.WriteLine(childFile);
                }
            }
            finally
            {
                executionManager.ReleaseAllActions();
            }
        }
    }
}
