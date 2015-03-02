using Ceilingfish.Pictur.Core.Models;
using Ceilingfish.Pictur.Core.Persistence;

namespace Ceilingfish.Pictur.Core.Persistence
{
    public interface IDatabase
    {
        FileCollection Files { get; }

        RavenPersistenceCollection<Directory> Directories { get; }

        RemoveFileCollection RemovedFileOperations { get; }
    }
}
