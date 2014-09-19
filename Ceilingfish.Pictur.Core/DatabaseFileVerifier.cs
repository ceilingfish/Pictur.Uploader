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
            current.FileRemoved = true; 

            _database.Update(current);
        }

        public void OnFileAdded(object sender, FileAddedArgs args)
        {
            UploadFileIfNecessary(args.FilePath, args.Directory);
        }

        public void OnFileDetected(object sender, FileDetectedArgs args)
        {
            UploadFileIfNecessary(args.FilePath, args.Directory);
        }

        private void UploadFileIfNecessary(string path, ManagedDirectory directory)
        {
            var checksum = ChecksumHelper.GetMd5HashFromFile(path);

            var current = _database.GetFileByCheckSum(checksum);

            if (current != null && current.Path.Equals(path))
            {
                if (current.FileRemoved)
                {
                    //looks like a file has been restored
                    current.FileRemoved = false;
                    current.ModifiedAt = DateTime.UtcNow;
                    _database.Update(current);
                }

            }
            var newFile = new Persistence.File
            {
                Path = path,
                Checksum = checksum
            };

            _database.Add(newFile);
        }
    }
}
