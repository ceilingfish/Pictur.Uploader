using Ceilingfish.Pictur.Core.Persistence;

namespace Ceilingfish.Pictur.Core.Events
{
    public class FileAddedArgs
    {
        public readonly ManagedDirectory Directory;
        public string FilePath;

        public FileAddedArgs(ManagedDirectory directory, string path)
        {
            Directory = directory;
            FilePath = path;
        }
    }
}
