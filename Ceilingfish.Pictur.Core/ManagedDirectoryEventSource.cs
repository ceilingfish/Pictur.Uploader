using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ceilingfish.Pictur.Core.Events;
using Ceilingfish.Pictur.Core.Persistence;

namespace Ceilingfish.Pictur.Core
{
    public class ManagedDirectoryEventSource
    {
        public class WatchedDirectory
        {
            private readonly ManagedDirectory _directory;
            private FileSystemWatcher _watcher;

            public WatchedDirectory(ManagedDirectory directory, ManagedDirectoryEventSource parent)
            {
                _directory = directory;
                _watcher = new FileSystemWatcher(directory.Path);
                _watcher.Created += OnFileCreated;
                _watcher.Deleted += OnFileRemoved;
            }

            internal void OnFileCreated(object sender, FileSystemEventArgs e)
            {
                throw new NotImplementedException();
            }

            internal void OnFileRemoved(object sender, FileSystemEventArgs e)
            {
                
            }

        }

        private List<WatchedDirectory> _directories;
        
        public ManagedDirectoryEventSource(IEnumerable<ManagedDirectory> directories)
        {
            var dirArray = directories.ToArray();
            _directories = new List<WatchedDirectory>(dirArray.Length);

            foreach (var directory in dirArray)
            {
                Add(directory);
            }
        }

        public void OnDirectoryAdded(ManagedDirectory directory)
        {
            
        }

        public void OnDirectoryRemoved(ManagedDirectory directory)
        {
            
        }

        public event EventHandler<FileAddedArgs> Added;

        public event EventHandler<FileRemovedArgs> Removed;

        private void Add(ManagedDirectory directory)
        {
            _directories.Add(new WatchedDirectory(directory,this));
        }
    }
}
