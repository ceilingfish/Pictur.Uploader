using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ceilingfish.Pictur.Core.Persistence;

namespace Ceilingfish.Pictur.Core.Pipeline
{
    public class CommitFileToDatabaseExecutor : IExecutor
    {
        private readonly IDatabase _db;

        public CommitFileToDatabaseExecutor(IDatabase db)
        {
            _db = db;
        }

        public void Execute(FileOperation op)
        {
            switch (op.Type)
            {
                case FileOperationType.Added:
                    _db.Files.Add(op.File);
                    break;
                case FileOperationType.Moved:
                    var movedOp = op as MovedFileOperation;
                    var file = movedOp.File;
                    file.Path = movedOp.NewPath;
                    _db.Files.Update(file);
                    break;
                case FileOperationType.Modified:
                    _db.Files.Update(op.File);
                    break;
                case FileOperationType.Removed:
                    _db.Files.Remove(op.File);
                    break;
            }
        }
    }
}
