using System.Collections.Generic;
using Ceilingfish.Pictur.Core.Events;

namespace Ceilingfish.Pictur.Core.Persistence
{
    public interface IDatabase
    {
        event RecordAdded<ManagedDirectory> DirectoryAdded;

        event RecordRemoved<ManagedDirectory> DirectoryRemoved;
            
        IEnumerable<ManagedDirectory> ManagedDirectories { get; }

        bool Add(ManagedDirectory directory);

        void Remove(ManagedDirectory directory);
    }
}
