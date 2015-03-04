using Serilog;

namespace Ceilingfish.Pictur.Core.Pipeline
{
    public class DebugExecutor : IExecutor
    {
        private static readonly ILogger Log = Serilog.Log.Logger.ForContext<DebugExecutor>();
        public void Execute(ExecutorContext op)
        {
            Log.Information("Peforming {@op}", op);
        }
    }
}
