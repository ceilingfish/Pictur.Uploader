using System.Collections.Generic;
using System.Linq;
using Ceilingfish.Pictur.Core.Events;
using Ceilingfish.Pictur.Core.Models;
using Ceilingfish.Pictur.Core.Persistence;
using Ceilingfish.Pictur.Core.Pipeline;

namespace Ceilingfish.Pictur.Core.FileSystem
{
    public class DirectoryWatcher
    {
        private readonly List<WatchedDirectory> _directories = new List<WatchedDirectory>();
        private readonly IDatabase _db;
        private readonly IExecutor _executor;

        public IEnumerable<Directory> Directories
        {
            get { return _directories.Select(d => d.Directory); }
        }

        public DirectoryWatcher(IDatabase db, IExecutor operation)
        {
            _db = db;
            _executor = operation;

            foreach (var directory in db.Directories)
            {
                Add(directory);
            }
        }

        public void Add(Directory directory)
        {
            if (_directories.Any(w => w.Directory.Id.Equals(directory.Id)))
                return;

            _directories.Add(new WatchedDirectory(directory, _db, _executor));
        }

        public void Remove(Directory directory)
        {
            var match = _directories.SingleOrDefault(w => w.Directory.Id.Equals(directory.Id));
            if (match != null)
            {
                _directories.Remove(match);
                match.Stop();
            }
        }

        public void Start()
        {
            foreach (var item in _directories)
            {
                item.Start();
            }
        }

        public void Stop()
        {
            foreach (var item in _directories)
            {
                item.Stop();
            }
        }

    }
}