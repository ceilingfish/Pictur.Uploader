using Ceilingfish.Pictur.Core.Pipeline;

namespace Ceilingfish.Pictur.Core.Persistence
{
    public class CommitFileToDatabaseExecutor : IExecutor
    {
        private readonly IDatabase _db;

        public CommitFileToDatabaseExecutor(IDatabase db)
        {
            _db = db;
        }

        public void Execute(ExecutorContext op)
        {
            switch (op.FileOperation)
            {
                case FileOperationType.Added:
                    _db.Files.Add(op.File);
                    break;
                case FileOperationType.Moved:
                    var movedOp = op as MovedExecutorContext;
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
