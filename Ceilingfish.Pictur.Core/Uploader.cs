using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Ceilingfish.Pictur.Core.FileSystem;
using Ceilingfish.Pictur.Core.Persistence;
using Ceilingfish.Pictur.Core.Pipeline;
using Serilog;

namespace Ceilingfish.Pictur.Core
{
    public class Uploader
    {
        private static readonly ILogger Log = Serilog.Log.Logger.ForContext<Uploader>();
        private readonly CancellationToken _token;
        private readonly CancellationTokenSource _internalCancellation;
        private Task _task;
        
        public TaskStatus Status
        {
            get
            {
                return _task?.Status ?? TaskStatus.Created;
            }
        }

        public Uploader()
        {
            _internalCancellation = new CancellationTokenSource();
            _token = _internalCancellation.Token;
        }

        public Uploader(CancellationToken token)
            : this()
        {
            _token = CancellationTokenSource.CreateLinkedTokenSource(token, _internalCancellation.Token).Token;
        }

        public Task Start()
        {
            return _task ?? Task.Run((Action)ExecuteInternal, _token);
        }

        public void Stop()
        {
            if (!_token.IsCancellationRequested)
                _internalCancellation.Cancel();

            _task.Wait(_token);
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
            _internalCancellation.Cancel();
        }
    }
}
