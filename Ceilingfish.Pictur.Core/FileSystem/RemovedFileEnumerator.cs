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
            foreach (var file in _db.Files)
            {
                if (System.IO.File.Exists(file.Path))
                    continue;

                var removeOp = _db.RemovedFileOperations.GetByFileId(file.Id);
                if (removeOp == null)
                    yield return file;
            }
        }
    }
}
