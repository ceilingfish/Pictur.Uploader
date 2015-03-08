using Ceilingfish.Pictur.Core.Models;
using ImageMagick;

namespace Ceilingfish.Pictur.Core.Pipeline
{
    public class ExecutorContext
    {
        public readonly File File;
        public readonly FileOperationType Type;

        public ExecutorContext(File file, FileOperationType type)
        {
            File = file;
            Type = type;
        }
    }
}
