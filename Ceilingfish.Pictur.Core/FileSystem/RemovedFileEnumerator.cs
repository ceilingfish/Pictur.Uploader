using Ceilingfish.Pictur.Core.Models;
using Ceilingfish.Pictur.Core.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ceilingfish.Pictur.Core.FileSystem
{
    public class RemovedFileEnumerator : IFileScanEnumerator
    {
        private readonly IDatabase _db;

        public RemovedFileEnumerator(IDatabase db)
        {
            _db = db;
        }

        public IEnumerable<Models.File> Scan()
        {
            return _db
                    .Files
                    .Where(f => !f.Deleted)
                    .Where(file => !System.IO.File.Exists(file.Path));
        }
    }
}
