using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ceilingfish.Pictur.Core.Events;
using Ceilingfish.Pictur.Core.Persistence;

namespace Ceilingfish.Pictur.Core
{
    public class ManagedDirectoryWatcher : IDisposable
    {
        public class WatchedDirectory : IDisposable
        {
            internal readonly ManagedDirectory Directory;
            private readonly FileSystemWatcher _watcher;
            private readonly ManagedDirectoryWatcher _parent;

            public WatchedDirectory(ManagedDirectory directory, ManagedDirectoryWatcher parent)
            {
                Directory = directory;
                _parent = parent;
                _watcher = new FileSystemWatcher(directory.Path);
                _watcher.Created += OnFileCreated;
                _watcher.Deleted += OnFileRemoved;
                _watcher.Renamed += OnFileChanged;
                _watcher.Changed += OnFileChanged;
            }

            internal void OnFileChanged(object sender, FileSystemEventArgs e)
            {
                //TODO implement args
                //TODO implement move detection
                _parent.Changed(_parent, new FileChangedArgs());
            }

            internal void OnFileCreated(object sender, FileSystemEventArgs e)
            {
                _parent.Added(_parent,new FileAddedArgs(Directory,e.FullPath));
            }

            internal void OnFileRemoved(object sender, FileSystemEventArgs e)
            {
                _parent.Removed(_parent, new FileRemovedArgs(Directory, e.FullPath));
            }

            public void Dispose()
            {
                _watcher.EnableRaisingEvents = false;
                _watcher.Dispose();
            }
        }

        private readonly List<WatchedDirectory> _directories;
        
        public ManagedDirectoryWatcher(IEnumerable<ManagedDirectory> directories)
        {
            var dirArray = directories.ToArray();
            _directories = new List<WatchedDirectory>(dirArray.Length);

            foreach (var directory in dirArray)
            {
                Add(directory);
            }
        }

        public void Add(ManagedDirectory directory)
        {
            _directories.Add(new WatchedDirectory(directory, this));
        }

        public void Remove(ManagedDirectory dir)
        {
            _directories.RemoveAll(wd => wd.Directory.Id.Equals(dir.Id));
        }

        public event EventHandler<FileAddedArgs> Added;

        public event EventHandler<FileRemovedArgs> Removed;

        public event EventHandler<FileChangedArgs> Changed;

        public void Dispose()
        {
            foreach (var item in _directories)
            {
                item.Dispose();
            }
        }
    }
}
