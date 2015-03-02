using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Ceilingfish.Pictur.Core.FileSystem;
using Ceilingfish.Pictur.Core.Persistence;
using Ceilingfish.Pictur.Core.Pipeline;

namespace Ceilingfish.Pictur.Service
{
    public class Uploader
    {
        private readonly CancellationToken _token;
        private Task _task;

        public Uploader(CancellationToken token)
        {
            _token = token;
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
                var executor = new DebugExecutor();

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

        internal void Wait()
        {
            _task.Wait(_token);
        }
    }
}
