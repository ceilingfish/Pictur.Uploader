using Ceilingfish.Pictur.Core.Persistence;

namespace Ceilingfish.Pictur.Core.Pipeline
{
    public class ExecutorContext
    {
        public readonly Models.File File;
        public readonly FileOperationType Type;

        public ExecutorContext(Models.File file, FileOperationType type)
        {
            File = file;
            Type = type;
        }
    }
}
