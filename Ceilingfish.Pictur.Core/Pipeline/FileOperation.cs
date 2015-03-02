using Ceilingfish.Pictur.Core.Persistence;

namespace Ceilingfish.Pictur.Core.Pipeline
{
    public class FileOperation
    {
        public readonly Models.File File;
        public readonly FileOperationType Type;

        public FileOperation(Models.File file, FileOperationType type)
        {
            File = file;
            Type = type;
        }
    }
}
