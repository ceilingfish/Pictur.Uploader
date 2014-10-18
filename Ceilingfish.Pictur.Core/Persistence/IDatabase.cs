using System.Collections.Generic;
using Ceilingfish.Pictur.Core.Events;

namespace Ceilingfish.Pictur.Core.Persistence
{
    public interface IDatabase
    {
        IEnumerable<ManagedDirectory> ManagedDirectories { get; }

        IEnumerable<File> RecentlyModifiedFiles { get; }

        void Add(File newFile);

        void Add(FileHarmonization harmonization);

        void Add(ManagedDirectory directory);

        void Update(File current);

        void Update(FileHarmonization harmonization);

        bool Remove(ManagedDirectory directory);

        File GetFileByPath(string path);

        File GetFileByCheckSum(string checksum);

        IEnumerable<FileHarmonization> GetHarmonizationsByFile(string id);
    }
}
