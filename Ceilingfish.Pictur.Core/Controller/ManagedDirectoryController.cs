using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Ceilingfish.Pictur.Core.Helpers;

namespace Ceilingfish.Pictur.Core.Controller
{
    public class ManagedDirectoryController : IController
    {
        public event EventHandler<FileCreatedArgs> Created;
        public event EventHandler<FileDeletedArgs> Deleted;
        public event EventHandler<FileModifiedArgs> Modified;

        private class ManagedDirectoryItem
        {
            internal readonly ManagedDirectory Directory;
            internal readonly FileSystemWatcher Watcher;

            internal ManagedDirectoryItem(ManagedDirectory dir)
            {
                Directory = dir;
                Watcher = new FileSystemWatcher(dir.Directory.FullName);
            }
        }

        private readonly List<ManagedDirectoryItem> _directories; 

        private 

        public ManagedDirectoryController(PersistenceController db)
        {
            _db = db;
            _directories = new List<ManagedDirectoryItem>();
        }

        internal void Start()
        {
            foreach (var managedDirectory in _db.GetManagedDirectories())
            {
                SubmitDirectory(managedDirectory.Directory);

                var item = new ManagedDirectoryItem(managedDirectory);

                item.Watcher.Created += OnDirectoryItemCreated;
                item.Watcher.Changed += OnDirectoryItemChanged;
                item.Watcher.Deleted += OnDirectoryItemDeleted;
                item.Watcher.Renamed += OnDirectoryItemRenamed;
                _directories.Add(item);

                item.Watcher.EnableRaisingEvents = true;
            }
        }

        #region File System Watchers

        private void OnDirectoryItemCreated(object sender, FileSystemEventArgs e)
        {
            CreateFile(new FileInfo(e.FullPath));
        }

        private void OnDirectoryItemRenamed(object sender, RenamedEventArgs e)
        {
            var oldFile = _db.FindFileByPath(e.OldFullPath);

            if (oldFile == null)
                return;

            ModifiedFile(oldFile,e.FullPath);
        }

        private void OnDirectoryItemDeleted(object sender, FileSystemEventArgs e)
        {
            var f = _db.FindFileByPath(e.FullPath);

            DeleteFile(f);
        }

        private void OnDirectoryItemChanged(object sender, FileSystemEventArgs e)
        {
            var file = _db.FindFileByPath(e.FullPath);

            ModifiedFile(file,e.FullPath);
        }

        #endregion

        private void CreateFile(FileInfo file)
        {
            var checksum = ChecksumHelper.GetMd5HashFromFile(file.FullName);

            var newFile = new Model.File(file)
            {
                Checksum = checksum
            };

            _db.Create(newFile);

            var vetoed = false;
            if (Created != null)
            {
                var createEvent = new FileCreatedArgs(newFile);
                Created(this, createEvent);

                vetoed = createEvent.Cancel;
            }

            if(vetoed)
                _db.Ignore(newFile);
        }

        private void ModifiedFile(Model.File oldFile, string path)
        {
            var newFile = new Model.File(new FileInfo(path))
            {
                Checksum = oldFile.Checksum
            };

            if (Modified != null)
                Modified(this, new FileModifiedArgs(oldFile, newFile));

            _db.UpdateFile(newFile);
        }

        private void DeleteFile(Model.File f)
        {
            if (Deleted != null)
                Deleted(this, new FileDeletedArgs(f));

            _db.Delete(f);
        }
        
        private void SubmitDirectory(DirectoryInfo directory)
        {
            foreach (var fileInfo in directory.GetFiles())
            {
                SubmitFile(fileInfo);
            }

            foreach (var directoryInfo in directory.GetDirectories())
            {
                SubmitDirectory(directoryInfo);
            }
        }

        private void SubmitFile(FileInfo path)
        {
            //TODO this needs more configurability
            ThreadPool.QueueUserWorkItem(CheckFile, path);
        }

        private void CheckFile(object fileObj)
        {
            var fileInfo = fileObj as FileInfo;

            var file = _db.FindFileByPath(fileInfo.FullName);

            var checksum = ChecksumHelper.GetMd5HashFromFile(fileInfo.FullName);

            var previousFile = _db.FindFileByChecksum(checksum);

            //if we found a previous file, then check to see if it exists
            if(previousFile != null)
            {
                //If it's the same file then 
                if (fileInfo.FullName.Equals(previousFile.Path))
                    return;

                ModifiedFile(previousFile, fileInfo.FullName);
            }
            //If we found no cache indicator of a previous file then create a new entry
            else if (file == null)
            {
                CreateFile(fileInfo);
            }
            else if(file.FileLength != fileInfo.Length || !checksum.Equals(file.Checksum))
            {
                ModifiedFile(previousFile, fileInfo.FullName);
            }
        }

        public void AddDirectory(string p)
        {
            
        }
    }
}
