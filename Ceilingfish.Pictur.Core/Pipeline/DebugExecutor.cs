using Serilog;

namespace Ceilingfish.Pictur.Core.Pipeline
{
    public class DebugExecutor : IExecutor
    {
        private static readonly ILogger Log = Serilog.Log.Logger.ForContext<DebugExecutor>();
        public void Execute(ExecutorContext op)
        {
            Log.Information("{FilePath} has been {OperationType}", op.File.Path, op.FileOperation);
        }
    }
}
