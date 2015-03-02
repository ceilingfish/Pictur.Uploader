using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ceilingfish.Pictur.Core.FileSystem;
using Ceilingfish.Pictur.Core.Models;

namespace Ceilingfish.Pictur.Core.Persistence
{
    public class DirectoryChangePoller
    {
        private readonly RavenPersistenceCollection<Directory> _databaseCollection;
        private readonly DirectoryWatcher _watcher;
        private readonly Timer _timer;


        public DirectoryChangePoller(RavenPersistenceCollection<Directory> collection, DirectoryWatcher watcher, TimeSpan interval)
        {
            _databaseCollection = collection;
            _watcher = watcher;
            _timer = new Timer(CheckForChanges, null, interval, interval);
        }

        private void CheckForChanges(object state)
        {
            var dbDirectories = _databaseCollection.ToArray();
            var addDirectories = dbDirectories
                .Where(d => !_watcher.Directories.Any(wd => wd.Id.Equals(d.Id)))
                .ToArray();
            var removedDirectories = _watcher
                .Directories
                .Where(wd => !dbDirectories.Any(d => d.Id.Equals(wd.Id)))
                .ToArray();
            var modifiedDirectories = _watcher
                .Directories
                .Join(dbDirectories, d => d.Id, d => d.Id, (a, b) => new Tuple<Directory, Directory>(a, b))
                .Where(t => t.Item1.ModifiedAt < t.Item2.ModifiedAt)
                .ToArray();

            foreach (var directory in addDirectories)
            {
                _watcher.Add(directory);
            }

            foreach (var directory in removedDirectories)
            {
                _watcher.Remove(directory);
            }

            foreach (var modification in modifiedDirectories)
            {
                _watcher.Remove(modification.Item1);
                _watcher.Add(modification.Item2);
            }
        }

        public void Stop()
        {
            _timer.Dispose();
        }

    }
}
