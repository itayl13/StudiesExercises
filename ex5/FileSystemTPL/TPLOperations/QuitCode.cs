using System;

namespace FileSystemTPL.TPLOperations
{
    class QuitCode : Operation
    {
        public QuitCode(CommandDetails commandDetails, ExecutionManager executionManager) : 
            base(commandDetails, executionManager) { }
        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }
}
