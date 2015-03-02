using Ceilingfish.Pictur.Core.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceilingfish.Pictur.Core.FileSystem
{
    public class ModifiedFileEnumerator : IFileScanEnumerator
    {
        private readonly Persistence.IDatabase _db;

        public ModifiedFileEnumerator(Persistence.IDatabase db)
        {
            _db = db;
        }

        public IEnumerable<Models.File> Scan()
        {
            foreach (var file in _db.Files)
            {
                if (!File.Exists(file.Path))
                    continue;

                var currentChecksum = ChecksumHelper.GetMd5HashFromFile(file.Path);
                if (currentChecksum.Equals(file.Checksum))
                    continue;

                yield return file;
            }
        }
    }
}
