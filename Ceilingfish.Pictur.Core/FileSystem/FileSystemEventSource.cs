using System.Threading;
using System.Threading.Tasks;
using Ceilingfish.Pictur.Core.Models;
using Ceilingfish.Pictur.Core.Persistence;
using Ceilingfish.Pictur.Core.Pipeline;

namespace Ceilingfish.Pictur.Core.FileSystem
{
    public class FileSystemEventSource
    {
        private readonly IExecutor _pipeline;
        private readonly IDatabase _db;

        public FileSystemEventSource(IDatabase db, IExecutor pipeline)
        {
            _db = db;
            _pipeline = pipeline;
        }

        public Task InitialScan(int threadCount, CancellationToken token)
        {
            var modificationEnumerator = new ModifiedFileEnumerator(_db);
            var additionEnumerator = new AddedFileEnumerator(_db);
            var removedEnumerator = new RemovedFileEnumerator(_db);

            var parallelOpts = new ParallelOptions { MaxDegreeOfParallelism = threadCount, CancellationToken = token };

            var modificationTask = Task.Run(() => Parallel.ForEach(modificationEnumerator.Scan(), parallelOpts, OnFileModified), token);
            var addTask = Task.Run(() => Parallel.ForEach(additionEnumerator.Scan(), parallelOpts, OnFileAdded), token);
            var removedTask = Task.Run(() => Parallel.ForEach(removedEnumerator.Scan(), parallelOpts, OnFileRemoved), token);

            return Task.WhenAll(modificationTask, addTask, removedTask);
        }

        internal void OnFileAdded(File file)
        {
            _pipeline.Execute(new ExecutorContext(file, FileOperationType.Added));
        }

        internal void OnFileModified(File file)
        {
            _pipeline.Execute(new ExecutorContext(file, FileOperationType.Modified));
        }

        internal void OnFileRemoved(File file)
        {
            _pipeline.Execute(new ExecutorContext(file, FileOperationType.Removed));
        }
    }
}