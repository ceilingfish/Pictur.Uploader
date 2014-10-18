using Ceilingfish.Pictur.Core.Events;
using Ceilingfish.Pictur.Core.Helpers;
using Ceilingfish.Pictur.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceilingfish.Pictur.Core
{
    public class DatabaseFileVerifier
    {
        private readonly IDatabase _database;

        public DatabaseFileVerifier(IDatabase db)
        {
            _database = db;
        }

        public void OnFileRemoved(object sender, FileRemovedArgs args)
        {
            var current = _database.GetFileByPath(args.FilePath);
            if (current == null)
                return;

            current.ModifiedAt = DateTime.UtcNow;
            _database.Update(current);
        }

        public void OnFileAdded(object sender, FileAddedArgs args)
        {
            AddFile(args.FilePath, args.Directory);
        }

        public void OnFileDetected(object sender, FileDetectedArgs args)
        {
            AddFile(args.FilePath, args.Directory);
        }

        public void OnFileModified(object sender, FileChangedArgs args)
        {
            var file = _database.GetFileByPath(args.OriginalPath);
            if (!string.Equals(args.OriginalPath, args.NewPath))
            {
                file.Path = args.NewPath;
            }

            file.ModifiedAt = DateTime.UtcNow;
            _database.Update(file);

            var synchronisations = _database.GetHarmonizationsByFile(file.Id);
            foreach (var sync in synchronisations)
            {
                if (sync.State != HarmonizationState.Ready)
                {
                    sync.State = HarmonizationState.Ready;
                    _database.Update(sync);
                }
            }
        }

        private void AddFile(string path, ManagedDirectory directory)
        {
            var checksum = ChecksumHelper.GetMd5HashFromFile(path);
            var current = _database.GetFileByCheckSum(checksum);
            if (current != null)
                return;

            var newFile = new Persistence.File
            {
                Path = path,
                Checksum = checksum
            };

            _database.Add(newFile);
        }
    }
}
