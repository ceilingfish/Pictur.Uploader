using Ceilingfish.Pictur.Core.Events;
using Ceilingfish.Pictur.Core.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ceilingfish.Pictur.Service
{
    internal class ManagedDirectoryPoller
    {
        internal event EventHandler<DirectoryAddedArgs> Added;
        internal event EventHandler<DirectoryRemovedArgs> Removed;

        private readonly CancellationToken _token;
        private readonly IDatabase _db;
        private readonly List<ManagedDirectory> _directories;

        internal ManagedDirectoryPoller(IEnumerable<ManagedDirectory> dirs, CancellationToken token, IDatabase db)
        {
            _db = db;
            _directories = dirs.ToList();
            _token = token;
        }

        internal void StartAsync()
        {
            Task.Run((Action)StartInternal,_token);
        }

        private void StartInternal()
        {
            while (_token.IsCancellationRequested)
            {
                var polledDirectories = _db.ManagedDirectories.ToArray();

                if (Removed != null)
                {
                    var removed = _directories.Where(cd => !polledDirectories.Any(pd => cd.Id.Equals(pd.Id)));
                    foreach (var removedDir in removed)
                    {
                        Removed(this, new DirectoryRemovedArgs(removedDir));
                    }
                }

                if (Added != null)
                {
                    var newDirs = polledDirectories.Where(pd => !_directories.Any(cd => cd.Id.Equals(pd.Id)));

                    foreach (var newDir in newDirs)
                    {
                        Added(this, new DirectoryAddedArgs(newDir));
                    }
                }
            }
        }
    }
}
