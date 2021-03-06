﻿using System.IO;
using Ceilingfish.Pictur.Core.Helpers;
using Ceilingfish.Pictur.Core.Persistence;
using Ceilingfish.Pictur.Core.Pipeline;

namespace Ceilingfish.Pictur.Core.FileSystem
{
    //TODO de-duplicate events
    public class WatchedDirectory
    {
        internal readonly Models.Directory Directory;
        private readonly FileSystemWatcher _watcher;
        private readonly IDatabase _db;
        private readonly IExecutor _executor;

        public WatchedDirectory(Models.Directory directory, IDatabase db, IExecutor pipeline)
        {
            Directory = directory;
            _db = db;
            _executor = pipeline;
            _watcher = new FileSystemWatcher(directory.Path);
            _watcher.Created += OnFileCreated;
            _watcher.Deleted += OnFileRemoved;
            _watcher.Renamed += OnFileMoved;
            _watcher.Changed += OnFileChanged;
        }

        internal void OnFileMoved(object sender, RenamedEventArgs e)
        {
            var oldPath = Path.GetFullPath(e.OldFullPath);
            var newPath = Path.GetFullPath(e.FullPath);
            var checksum = ChecksumHelper.GetMd5HashFromFile(newPath);
            var previousFiles = _db.Files.GetByPathAndChecksum(oldPath, checksum);

            foreach (var file in previousFiles)
                _executor.Execute(new MovedExecutorContext(file, oldPath, newPath));
        }

        internal void OnFileChanged(object sender, FileSystemEventArgs e)
        {
            var path = Path.GetFullPath(e.FullPath);
            var modifiedFiles = _db.Files.GetByPath(path);

            foreach (var modified in modifiedFiles)
                _executor.Execute(new ExecutorContext(modified, FileOperationType.Modified));
        }

        internal void OnFileCreated(object sender, FileSystemEventArgs e)
        {
            var path = Path.GetFullPath(e.FullPath);
            var checksum = ChecksumHelper.GetMd5HashFromFile(path);
            var file = new Models.File { Path = path, DirectoryId = Directory.Id, Checksum = checksum };
            _executor.Execute(new ExecutorContext(file, FileOperationType.Added));
        }

        internal void OnFileRemoved(object sender, FileSystemEventArgs e)
        {
            var path = Path.GetFullPath(e.FullPath);
            var removedFiles = _db.Files.GetByPath(path);

            foreach (var removed in removedFiles)
            {
                removed.Deleted = true;
                _db.Files.Update(removed);
                _executor.Execute(new ExecutorContext(removed, FileOperationType.Removed));
            }
        }

        public void Start()
        {
            _watcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            _watcher.EnableRaisingEvents = false;
            _watcher.Dispose();
        }
    }
}