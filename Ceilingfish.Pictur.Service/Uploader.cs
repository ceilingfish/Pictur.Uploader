using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Ceilingfish.Pictur.Core.FileSystem;
using Ceilingfish.Pictur.Core.Persistence;
using Ceilingfish.Pictur.Core.Pipeline;
using Serilog;

namespace Ceilingfish.Pictur.Service
{
    public class Uploader
    {
        private static readonly ILogger Log = Serilog.Log.Logger.ForContext<Uploader>();
        private readonly CancellationToken _token;
        private readonly CancellationTokenSource _errorCancellation;
        private Task _task;

        public Uploader(CancellationToken token)
        {
            _errorCancellation = new CancellationTokenSource();
            _token = CancellationTokenSource.CreateLinkedTokenSource(token, _errorCancellation.Token).Token;
        }

        public Task Execute()
        {
            if (_task == null)
                _task = Task.Run((Action)ExecuteInternal, _token);

            return _task;
        }

        private void ExecuteInternal()
        {
            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "Ceilingfish.Uploadr", "Persistence.Raven");
            using (var db = new RavenDatabase(path))
            {
                var executor = new ExceptionHandlingExecutor(new ExecutorChain
                {
                    new DebugExecutor(),
                    new CommitFileToDatabaseExecutor(db)
                });

                executor.Exception += HandleExecutorException;

                var watcher = new DirectoryWatcher(db, executor);
                var directoryChangePoller = new DirectoryChangePoller(db.Directories, watcher, TimeSpan.FromMinutes(1));

                var loader = new FileSystemEventSource(db, executor);
                watcher.Start();
                loader.InitialScan(1, _token);

                _token.WaitHandle.WaitOne();

                directoryChangePoller.Stop();
                watcher.Stop();
            }
        }

        private void HandleExecutorException(object sender, ExecutorExceptionArgs executorExceptionArgs)
        {
            Log.Fatal(executorExceptionArgs.Exception, "Unhandled exception processing @executorExceptionArgs.Context", executorExceptionArgs);
            _errorCancellation.Cancel();
        }

        internal void Wait()
        {
            _task.Wait(_token);
        }
    }
}
