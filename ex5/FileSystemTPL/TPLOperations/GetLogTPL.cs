using System;

namespace FileSystemTPL.TPLOperations
{
    class GetLogTPL : Operation
    {
        public GetLogTPL(CommandDetails commandDetails, ExecutionManager executionManager) :
            base(commandDetails, executionManager)
        { }
        public override void Execute()
        {
            executionManager.LockCommandsLog();
            Console.WriteLine("\nLast (at most) 10 commands log:\n" + string.Join("\n", executionManager.lastTenCommandsLog));
            executionManager.ReleaseCommandsLog();
        }
    }
}
