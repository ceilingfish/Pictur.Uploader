using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Ceilingfish.Pictur.Core.Events;
using Ceilingfish.Pictur.Core.Helpers;
using Ceilingfish.Pictur.Core.Models;
using Raven.Database.Tasks;
using File = Ceilingfish.Pictur.Core.Models.File;
using Ceilingfish.Pictur.Core.Persistence;

namespace Ceilingfish.Pictur.Core.FileSystem
{
    public class AddedFileEnumerator : IFileScanEnumerator
    {
        private readonly IDatabase _db;

        public AddedFileEnumerator(IDatabase db)
        {
            _db = db;
        }

        public IEnumerable<File> Scan()
        {
            return from dir in _db.Directories
                   where System.IO.Directory.Exists(dir.Path)
                   from file in System.IO.Directory.EnumerateFiles(dir.Path, "*", SearchOption.AllDirectories)
                   where !_db.Files.GetByPath(Path.GetFullPath(file)).Any(f => Equals(f.DirectoryId, dir.Id))
                   select new File { DirectoryId = dir.Id, Path = Path.GetFullPath(file), Checksum = ChecksumHelper.GetMd5HashFromFile(file) };
        }
    }
}
